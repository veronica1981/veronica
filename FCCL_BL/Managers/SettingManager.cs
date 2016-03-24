using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_BL.Managers
{
	public class SettingManager
	{
		private short application;
		private CachedRepository<Setting> repo;

		public SettingManager(FCCLDbContext context)
		{
			repo = new CachedRepository<Setting>(context);
			application = (short)context.Application;
		}

		public string GetValueByName(string name)
		{
			Setting setting = repo.Get().Where(s => (s.Application == application || s.Application == 0)).OrderByDescending(s => s.Application)
				.FirstOrDefault(s => s.Name == name);

			return setting != null ? setting.Value : string.Empty;
		}

		public string CaleFizicaServer
		{
			get
			{
				return GetValueByName("CaleFizicaServer");
			}
		}

		public string CaleRapoarteHttp
		{
			get
			{
				return GetValueByName("CaleRapoarteHttp");
			}
		}

		public string CaleRapoarte
		{
			get
			{
				return GetValueByName("CaleRapoarte");
			}
		}

		public string TempPathFormat
		{
			get
			{
				return GetValueByName("TempPathFormat");
			}
		}
	}
}
