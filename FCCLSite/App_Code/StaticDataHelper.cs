using FCCL_BL;
using FCCL_BL.Managers;
using FCCL_DAL;
using System.Configuration;


public class StaticDataHelper
{
	public static IntervalManager IntervalManager
	{
		get
		{
			return new IntervalManager(FCCLDbContext);
		}
	}

	public static SettingManager SettingsManager
	{
		get
		{
			return new SettingManager(FCCLDbContext);
		}
	}

	public static FCCLDbContext FCCLDbContext
	{
		get
		{
			return ContextHelper.GetContext(ConfigurationManager.AppSettings["ApplicationNo"].ToString());
		}
	}

}
