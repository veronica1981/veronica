using System.Web.UI;
using FCCL_BL.Managers;
using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Registru : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			BindData();
		}
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

	protected void BindData()
	{
		int count;
		grdReports.DataSource = GetData(true, out count);
		grdReports.VirtualItemCount = count;
		grdReports.DataBind();
	}

	protected void grdReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		grdReports.PageIndex = e.NewPageIndex;
		BindData();
	}

	protected void btnFilterClear_Click(object sender, EventArgs e)
	{
		txtTDStart.Text = string.Empty;
		txtTDEnd.Text = string.Empty;
		txtRNStart.Text = string.Empty;
		txtRNEnd.Text = string.Empty;
		txtON.Text = string.Empty;
		BindData();
	}

	protected void btnFilterApply_Click(object sender, EventArgs e)
	{
		BindData();
	}

	protected void btnExportToExcel_Click(object sender, EventArgs e)
	{
		Response.Clear();
		Response.Buffer = true;
		Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
		Response.Charset = "";
		Response.ContentType = "application/vnd.ms-excel";
		StringBuilder sb = new StringBuilder();

		sb.Append("<table><tr><td>Nr. Raport</td><td>Data</td><td>Client</td><td>Numar Pagini</td><td>Numar Probe</td><td>Transmis</td><td>Primit</td></tr>");
		int count;
		foreach (Report report in GetData(false, out count))
		{
			sb.Append("<tr>");
			sb.Append("<td>" + report.ReportNumber + "</td>");
			sb.Append("<td>" + report.PrintDate.ToString("dd/MM/yyyy") + "</td>");
			sb.Append("<td>" + report.ObjectName + "</td>");
			sb.Append("<td>" + report.PageCount + "</td>");
			sb.Append("<td>" + report.SampleCount + "</td>");
			sb.Append("<td>" + string.Empty + "</td>");
			sb.Append("<td>" + string.Empty + "</td>");
			sb.Append("</tr>");
		}
		sb.Append("</table>");
		//style to format numbers to string
		string style = @"<style> .textmode { } </style>";
		Response.Write(style);
		Response.Output.Write(sb.ToString());
		Response.Flush();
		Response.End();
	}

	private List<Report> GetData(bool paged, out int count)
	{
		List<Report> reports = null;
		ReportManager rManager = new ReportManager(StaticDataHelper.FCCLDbContext);
		#region filter
		DateTime date;
		long no;
		DateTime? testDateStart = null;
		DateTime? testDateEnd = null;
		long? reportNoStart = null;
		long? reportNoEnd = null;

		if (!string.IsNullOrWhiteSpace(txtTDStart.Text))
		{
			if (DateTime.TryParseExact(txtTDStart.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				testDateStart = date;
			}
			else
			{
				testDateStart = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(txtTDEnd.Text))
		{
			if (DateTime.TryParseExact(txtTDEnd.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				testDateEnd = date;
			}
			else
			{
				testDateEnd = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(txtRNStart.Text))
		{
			if (long.TryParse(txtRNStart.Text, out no))
			{
				reportNoStart = no;
			}
			else
			{
				reportNoStart = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(txtRNEnd.Text))
		{
			if (long.TryParse(txtRNEnd.Text, out no))
			{
				reportNoEnd = no;
			}
			else
			{
				reportNoEnd = null;
			}
		}
		#endregion
		if (paged)
		{
			reports = rManager.GetReportsPaged(testDateStart, testDateEnd, reportNoStart, reportNoEnd, txtON.Text, grdReports.PageIndex, grdReports.PageSize);
			count = rManager.GetReportsCount(testDateStart, testDateEnd, reportNoStart, reportNoEnd, txtON.Text);
		}
		else
		{
			reports = rManager.GetReportsFiltered(testDateStart, testDateEnd, reportNoStart, reportNoEnd, txtON.Text);
			count = reports.Count;
		}
		return reports;
	}

}
