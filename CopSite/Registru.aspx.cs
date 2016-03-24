using System.Web.UI;
using FCCL_BL.Managers;
using FCCL_DAL;
using System;
using System.Web.Script.Services;
using System.Web.Services;

public partial class Registru : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}

	[WebMethod(CacheDuration = 0, EnableSession = true)]
	[ScriptMethod(UseHttpGet = false)]
	public static dynamic GetReport(long reportNumber)
	{
		ReportManager rManager = new ReportManager(StaticDataHelper.FCCLDbContext);
		Report report = rManager.GetByNumber(reportNumber);
		report = report ?? new Report();
		return new
		{
			rType = report.ReportType,
			objId = report.ObjectId,
			objName = report.ObjectName,
			testDate = report.TestDate.ToString("dd/MM/yyyy"),
			displayTxt = report.Id > 0
				? string.Format("A fost gasit raportul {0} pentru ferma {1} din data {2}.", (FCCLReportType)report.ReportType, report.ObjectName, report.TestDate.ToString("dd/MM/yyyy"))
				: string.Format("Nu a fost gasit raport pentru numarul {0}.", reportNumber)
		};
	}
}
