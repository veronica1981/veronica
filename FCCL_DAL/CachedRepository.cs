using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL
{
	public class CachedRepository<TEntity> : Repository<TEntity> where TEntity : class
	{
		private static object locker = new object();
		private static List<TEntity> entities;

		public CachedRepository(DbContext context) : base(context)
		{
			if(entities == null)
			{
				lock(locker)
				{
					if(entities == null)
					{
						entities = base.Get().ToList();
					}
				}
			}
		}

		public override IEnumerable<TEntity> Get()
		{
			return entities;
		}

		public override void Insert(TEntity entity)
		{
			entities.Add(entity);
			base.Insert(entity);
		}

		public override void Delete(TEntity entityToDelete)
		{
			entities.Remove(entityToDelete);
			base.Delete(entityToDelete);
		}
	}
}
