using FCCL_DAL;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCCL_BL.Managers
{
	public class ReceptieManager
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private FCCLDbContext dbContext = null;

		public ReceptieManager(FCCLDbContext context)
		{
			dbContext = context;
		}

		public List<sp_Get_RegistruRegeptie_Result> Get(DateTime date)
		{
			logger.Info("Get|date: {1}", date.ToString());
			List<sp_Get_RegistruRegeptie_Result> results = new List<sp_Get_RegistruRegeptie_Result>();

			var result = dbContext.sp_Get_RegistruRegeptie(date);
			if (result != null)
			{
				results = result.ToList();
			}
			logger.Info("Get|count: {1}", results.Count);

			return results;
		}
	}
}
