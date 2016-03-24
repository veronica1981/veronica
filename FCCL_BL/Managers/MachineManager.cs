using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_BL.Managers
{
	public class MachineManager
	{
		private short application;
		private CachedRepository<Machine> repo;

		public MachineManager(FCCLDbContext context)
		{
			repo = new CachedRepository<Machine>(context);
			application = (short)context.Application;
		}

		public Machine GetMachineByName(string name)
		{
			return repo.Get().Where(m => (m.Application == application || m.Application == 0)).OrderByDescending(m => m.Application).FirstOrDefault(m => m.Name == name);
		}

		public List<Machine> GetMachines()
		{
			return repo.Get().Where(m => (m.Application == application || m.Application == 0)).GroupBy(m => m.Name).Select(g => g.OrderByDescending(m => m.Application).FirstOrDefault()).ToList();
		}
	}
}
