using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL
{
	public class Repository<TEntity> where TEntity : class
	{
		internal DbContext context;
		internal DbSet<TEntity> dbSet;

		public Repository(DbContext context)
		{
			this.context = context;
			dbSet = context.Set<TEntity>();
		}

		public virtual IEnumerable<TEntity> Get()
		{
			IQueryable<TEntity> query = dbSet;
			return query.ToList();
		}

		public virtual TEntity GetByID(object id)
		{
			return dbSet.Find(id);
		}

		public virtual void Insert(TEntity entity)
		{
			dbSet.Add(entity);
		}

		public virtual void Delete(object id)
		{
			TEntity entityToDelete = dbSet.Find(id);
			Delete(entityToDelete);
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (context.Entry(entityToDelete).State == EntityState.Detached)
			{
				dbSet.Attach(entityToDelete);
			}
			dbSet.Remove(entityToDelete);
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			dbSet.Attach(entityToUpdate);
			context.Entry(entityToUpdate).State = EntityState.Modified;
		}

		public virtual TEntity Create()
		{
			return dbSet.Create();
		}
	}
}
