using System.Web;
using System.Web.UI;
using FCCL_BL.Helpers;
using FCCL_BL.Managers;
using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

public partial class Receptii : Page
{
	private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext;
	private const string LOCAL_FILE_FORMAT = @"{0}\{1}.docx";

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void btnGetReports_Click(object sender, EventArgs e)
	{
		DateTime dateStart = DateTime.Now;
		DateTime dateEnd = DateTime.Now;

		StringBuilder sbHeader = new StringBuilder();
		sbHeader.AppendLine("\"Seria PV/Data\",\"Nume/Cod Prelevator\",\"Nume proba\",\"Cod identificare proba\",\"Tipul de analiza\"");
		sbHeader.AppendLine("\"Cerere analiza\",\"\",\"\",\"\",\"a),b),c),d),e)*\"");

		if (!string.IsNullOrWhiteSpace(txtDateStart.Text))
		{
			DateTime.TryParseExact(txtDateStart.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateStart);
		}
		if (!string.IsNullOrWhiteSpace(txtDateEnd.Text))
		{
			DateTime.TryParseExact(txtDateEnd.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateEnd);
		}

		string name = DateTime.Now.ToString("yyyyMMdd hhmmss") + " " + dateStart.ToString("dd.MM.yyyy") + " - " + dateEnd.ToString("dd.MM.yyyy");

		string exportDir = string.Format(StaticDataHelper.SettingsManager.TempPathFormat, name);
		string zipFile = string.Format(StaticDataHelper.SettingsManager.TempPathFormat, name + ".zip");

		if(!Directory.Exists(exportDir))
		{
			Directory.CreateDirectory(exportDir);
		}

		ReceptieManager rManager = new ReceptieManager(ctx);
		int fileCnt = 0;
		for(DateTime dateIdx = dateStart.Date; dateIdx <= dateEnd.Date; dateIdx = dateIdx.AddDays(1))
		{
			List<sp_Get_RegistruRegeptie_Result> lines = rManager.Get(dateIdx);
			if (lines.Count > 0)
			{
				WordHelper.GenerateReceptionRegister(lines, string.Format(LOCAL_FILE_FORMAT, exportDir, dateIdx.ToString("yyyyMMdd")), dateIdx);
                fileCnt++;
			}	
		}

		if(fileCnt > 0)
		{
			ZipFile.CreateFromDirectory(exportDir, zipFile, CompressionLevel.Fastest, false);
		}
		var updateFile = new FileInfo(zipFile);
		Response.Cache.SetCacheability(HttpCacheability.NoCache);
		Response.AddHeader("content-disposition", "attachment;filename=\"" + Path.GetFileName(updateFile.FullName) + "\"");
		Response.AddHeader("content-length", updateFile.Length.ToString());
		Response.TransmitFile(updateFile.FullName);
		Response.Flush();

		File.Delete(zipFile);
		Directory.Delete(exportDir, true);
	}
}
