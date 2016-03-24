using System.Web.UI;
using CopSite.Old_App_Code;
using FCCL_BL.Bus;
using FCCL_BL.Managers;
using FCCL_DAL;
using Microsoft.Reporting.WebForms;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.Security;
using Report = FCCL_DAL.Report;
using System.Threading;


public partial class RaportCrotalii : Page
{
	private static Logger logger = LogManager.GetCurrentClassLogger();
	private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext; 
  
	protected int ObjectId
	{
		get
		{
			int objectId;
			int.TryParse(DropDownList1.SelectedValue, out objectId);
			return objectId;
		}
	}
	protected DateTime TestDate
	{
		get
		{
			DateTime date;
			if (!DateTime.TryParseExact(TextBox1.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				date = DateTime.Now;
			}
			return date;
		}
	}
	protected DateTime PrintDate
	{
		get
		{
			DateTime date;
			if (!DateTime.TryParseExact(TextBox2.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				date = DateTime.Now;
			}
			return date;
		}
	}
		
    protected void Page_Load(object sender, EventArgs e)
    {
        pdflink.Visible = false;
		xlslink.Visible = false;
		csvlink.Visible = false;
		if (TextBox1.Text.Trim() == "")
            TextBox1.Text = (DateTime.Now).ToString("dd/MM/yyyy");
        if (TextBox2.Text.Trim() == "")
            TextBox2.Text = (DateTime.Now).ToString("dd/MM/yyyy");

		if(!IsPostBack)
		{
			//nasty hack
			ddlLaborator.SelectedIndex = 0;
			ddlResponsabil.SelectedIndex = 1;
		}
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
		ReportManager rManager = new ReportManager(ctx);
		Report dalReport = rManager.GetByDateAndId(TestDate, ObjectId);
		if (dalReport != null)
		{
			TextBox1.Text = dalReport.TestDate.ToString("dd/MM/yyyy");
			TextBox2.Text = dalReport.PrintDate.ToString("dd/MM/yyyy");
			TextBox3.Text = dalReport.ReportNumber.ToString();
		}
		else
		{
			TextBox3.Text = string.Empty;
			TextBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
		}
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        DropDownList1.DataBind();
        if (DropDownList1.Items.Count > 0)
        {
			ReportManager rManager = new ReportManager(ctx);
			Report dalReport = rManager.GetByDateAndId(TestDate, ObjectId);
			if (dalReport != null)
			{
				TextBox1.Text = dalReport.TestDate.ToString("dd/MM/yyyy");
				TextBox2.Text = dalReport.PrintDate.ToString("dd/MM/yyyy");
				TextBox3.Text = dalReport.ReportNumber.ToString();
			}
			else
			{
				TextBox3.Text = string.Empty;
				TextBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
			}
		}
		if (TextBox2.Text.Trim() == "")
		{
			TextBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
		}
    }

    public string replace_special_car(string nume_ini)
    {
        // nu merge htmlencode 
        // le inlocuiesc pe cele posibil de intalnit in nume fabrica 
        // folosit pentru numele fisierului de listat
        string nume_fin = "";
        nume_fin = nume_ini.Replace(" ", "_");
        nume_fin = nume_fin.Replace("&", "");
        nume_fin = nume_fin.Replace("\"", "");
        nume_fin = nume_fin.Replace(".", "");
        nume_fin = nume_fin.Replace("\'", "");
        nume_fin = nume_fin.Replace("*", "");
        nume_fin = nume_fin.Replace("@", "");
        nume_fin = nume_fin.Replace("\\", "");
        nume_fin = nume_fin.Replace("-", "");
        nume_fin = nume_fin.Replace("/", "");
        nume_fin = nume_fin.Replace("|", "");
        nume_fin = nume_fin.Replace("=", "");

        return (nume_fin);
    }
    public string replace_special_car_null(string nume_ini)
    {
        // nu merge htmlencode 
        // numele fisierului care va fi trimis pe mail nu trebuie sa contina mai multe _ decat cele formatate pt data si intre data si id asa ca voi inlocui inclusiv _ din numele fermei sau fabricii
        // datorita parsului asupra numelui fisierului pdf care va fi trimis pe mail se vor inlocui cu "" urmatoarele caractere
        // calea spre fis este folosita ca si link
        // le inlocuiesc pe cele intalnite in nume fabrica si nume ferma pt fis care vor fi trimise pe e-mail ... altele adaugate
        string nume_fin = "";
        nume_fin = nume_ini.Replace("_", "");// _ folosit in regula de parse pt email
        nume_fin = nume_fin.Replace(" ", "");
        nume_fin = nume_fin.Replace("&", "");
        nume_fin = nume_fin.Replace("\"", "");
        nume_fin = nume_fin.Replace(".", "");
        nume_fin = nume_fin.Replace("\'", "");
        nume_fin = nume_fin.Replace("*", "");
        nume_fin = nume_fin.Replace("@", "");
        nume_fin = nume_fin.Replace("\\", "");
        nume_fin = nume_fin.Replace("-", "");// folosit in regula de parse pt email automat	
        nume_fin = nume_fin.Replace("/", "");
        nume_fin = nume_fin.Replace("|", "");
        nume_fin = nume_fin.Replace("=", "");
        // sper ca alte caractere ca <>?{}[]^()%$#!~ nu se vor folosi in numele fermei si al fabricii
        return (nume_fin);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.Items.Count >0)
        CreateReport();

    }

	public void CreateReport()
	{
        Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
		logger.Info(string.Format("GetMostreFerma|CreateReport"));

		LocalReport report = new LocalReport();
		byte[] file;
		string mimeType, encoding, fileExtension;
		string[] streams;
		Warning[] warnings;
		long reportNumber;
		ReportManager rManager = new ReportManager(ctx);
		Report dalReport = rManager.GetOrCreateReport(FCCLReportType.Crotalii, ObjectId
			, DropDownList1.SelectedItem.Text, TestDate, PrintDate, long.TryParse(TextBox3.Text, out reportNumber) ? (long?)reportNumber : null);
		TextBox3.Text = dalReport.ReportNumber.ToString();
		ReportDataSource rds = new ReportDataSource();
		rds.Name = "MostreFabrica";

		List<MostreFabrica> mostre = MostreFabrica.GetMostreFerma(DropDownList1.SelectedValue, DateTime.Parse(TextBox1.Text));
		logger.Info(string.Format("GetMostreFerma|mostre cnt:{0}", mostre.Count));
		rds.Value = mostre;
		DateTime datatestare = DateTime.Parse(TextBox1.Text);

		DateTime datatmin = datatestare;
		DateTime datatmax = datatestare;
		string datatesttitlu = datatestare.ToShortDateString();

		DateTime datapmin = DateTime.MaxValue;
		DateTime datapmax = DateTime.MinValue;
		string dataprimtitlu;

		string nrcomanda = "";
		string combi = mostre[0].Combi;
		foreach (MostreFabrica mf in mostre)
		{
			DateTime datamin = mf.DataTestare;
			DateTime datamax = mf.DataTestareFinala;

			if (mf.NrComanda.Trim() != "")
				nrcomanda = mf.NrComanda.Trim();
			
            if (DateTime.Compare(datamin, datatmin) < 0)
				datatmin = datamin;
			
            if (DateTime.Compare(datamax, datatmax) > 0)
				datatmax = datamax;

            if (DateTime.Compare(mf.DataPrimirii, datapmin) < 0)
                datapmin = mf.DataPrimirii;

            if (DateTime.Compare(mf.DataPrimirii, datapmax) > 0)
                datapmax = mf.DataPrimirii;
		}

		if (datatmin != datatmax)
			datatesttitlu = datatmin.ToShortDateString() + " - " + datatmax.ToShortDateString();
		if (datapmin != datapmax)
			dataprimtitlu = datapmin.ToShortDateString() + " si " + datapmax.ToShortDateString();
		else
			dataprimtitlu = datapmin.ToShortDateString();

		rds.Value = mostre;

		report.DataSources.Clear();
		report.DataSources.Add(rds);


		report.ReportPath = "ReportCrotalii.rdlc";

		report.Refresh();

		string fermaid = DropDownList1.SelectedValue;
		string fermaname = DropDownList1.Items[DropDownList1.SelectedIndex].ToString();
		// read fabrica
		SqlConnection cnn = new SqlConnection(
		ConfigurationManager.ConnectionStrings
		["fccl2ConnectionString"].ConnectionString);
		SqlCommand cmd = new SqlCommand();
		cmd.Connection = cnn;

		cmd.CommandText = "SELECT Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Judete.Denloc "
		+ "FROM Ferme_CCL,Judete WHERE Ferme_CCL.ID=" + fermaid
			+ " AND Ferme_CCL.Judet=Judete.ID";
		cnn.Open();
		SqlDataReader reader = cmd.ExecuteReader();
		reader.Read();
		string fermastrada = Convert.ToString(reader["Strada"]);
		string fermanumar = Convert.ToString(reader["Numar"]);
		string fermaoras = Convert.ToString(reader["Oras"]);
		string fermajudet = Convert.ToString(reader["Denloc"]);
		string fermacod = Convert.ToString(reader["Cod"]);
		reader.Close();
		cnn.Close();


		cmd.CommandText = "SELECT Fabrici.Nume, Ferme_CCL.FabricaID from Ferme_CCL, Fabrici  "
	  + " WHERE Ferme_CCL.ID =" + fermaid + " AND Ferme_CCL.FabricaID = Fabrici.ID";
		cnn.Open();
		reader = cmd.ExecuteReader();
		reader.Read();
		string asocid = Convert.ToString(reader["FabricaID"]);
		string asocnume = Convert.ToString(reader["Nume"]);

		reader.Close();
		cnn.Close();

		FermaName.Text = fermaname;
		FermaCod.Text = fermacod;
		FermaId.Text = "" + fermaid;


		string datab = TextBox2.Text;
		string nrb = dalReport.ReportNumber.ToString();
		//set report parameters
		string laborator = ddlLaborator.Items[ddlLaborator.SelectedIndex].ToString();
		string responsabil = ddlResponsabil.Items[ddlResponsabil.SelectedIndex].ToString();

		ReportParameter pAsocnume = new ReportParameter("asocnume", asocnume);

		ReportParameter pNrcomanda = new ReportParameter("nrcomanda", nrcomanda);

		ReportParameter pDatatestare = new ReportParameter("datatestare", datatesttitlu);
		ReportParameter pDataprimirii = new ReportParameter("dataprimirii", dataprimtitlu);

		ReportParameter pCombi = new ReportParameter("combi", combi);

		ReportParameter pDatab = new ReportParameter("datab", TextBox2.Text);
		ReportParameter pNrb = new ReportParameter("nrb", nrb);

		ReportParameter pFermanume = new ReportParameter("fabricanume", fermaname);
		ReportParameter pFermastrada = new ReportParameter("fabricastrada", fermastrada);
		ReportParameter pFermanumar = new ReportParameter("fabricanumar", fermanumar);
		ReportParameter pFermaoras = new ReportParameter("fabricaoras", fermaoras);
		ReportParameter pFermajudet = new ReportParameter("fabricajudet", fermajudet);

		ReportParameter pLaborator = new ReportParameter("laborator", laborator);

		ReportParameter pResponsabil = new ReportParameter("responsabil", responsabil);

		ReportParameter pVersiune = new ReportParameter("Versiune", new SettingManager(StaticDataHelper.FCCLDbContext).GetValueByName("ReportCrotalii"));

		ReportParameter[] p ={ pDatatestare, pDataprimirii,pCombi, pNrcomanda, pDatab, pNrb, pFermanume, pFermastrada, pFermanumar, pFermaoras, pFermajudet, pAsocnume, pLaborator
								 , pResponsabil, pVersiune };
		report.SetParameters(p);


		file = report.Render("PDF", StaticData.DEVICE_INFO_PDF, out mimeType, out encoding, out fileExtension, out streams, out warnings);

		string httppath = StaticDataHelper.SettingsManager.CaleRapoarteHttp;
		string filepath = StaticDataHelper.SettingsManager.CaleRapoarte;

		fermaname = replace_special_car_null(fermaname);
		string nrcom = nrcomanda.Replace("/", "_");
		nrcom = nrcom.Replace(".", "");
		string raport_name = "Raport" + nrcom + "-" + fermaname + "_" + fermacod + "-" + datatestare.ToShortDateString().Replace("/", "") + ".pdf";
		string raport_excel = "Raport" + nrcom + "-" + fermaname + "_" + fermacod + "-" + datatestare.ToShortDateString().Replace("/", "") + ".xls";

		string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
		string pdf_link = path_raport_http + @"Ferme/" + raport_name;
		string pdf_file = filepath + @"Ferme/" + raport_name;
		// writefile

		string excel_link = path_raport_http + @"Ferme/" + raport_excel;
		string excel_file = filepath + @"Ferme/" + raport_excel;
		//	Trace.Write(pdf_file);
		File.WriteAllBytes(pdf_file, file);
		logger.Info(string.Format("GetMostreFerma|pdf done"));

		dalReport.SampleCount = mostre.Count;
		dalReport.PageCount = PDFHelper.GetNumberOfPdfPages(pdf_file);
		rManager.Save(dalReport);
		ctx.SaveChanges();
		logger.Info(string.Format("GetMostreFerma|saved report"));

		// raport excel
		LocalReport rapexcel = new LocalReport();

		ReportDataSource rdse = new ReportDataSource();
		rdse.Name = "MostreFabrica";

		rdse.Value = mostre;

		rapexcel.DataSources.Clear();
		rapexcel.DataSources.Add(rdse);


		rapexcel.ReportPath = "ReportCrotalii.rdlc";

		rapexcel.SetParameters(p);

		rapexcel.Refresh();

		rapexcel.SetParameters(new ReportParameter("BkImage", ""));
		byte[] file_xls = rapexcel.Render("EXCEL", StaticData.DEVICE_INFO_XLS, out mimeType, out encoding, out fileExtension, out streams, out warnings);
		// end raport excel
		File.WriteAllBytes(excel_file, file_xls);
		logger.Info(string.Format("GetMostreFerma|excel done"));

		int firstyear;
		if (int.TryParse(ConfigurationManager.AppSettings["firstyear"], out firstyear))
		{
			// test rep. portal
			SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
			SqlCommand cmda = new SqlCommand("Select aspnet_Users.UserName from aspnet_users join usersinformation on aspnet_Users.userid = usersinformation.userid where usersinformation.asocid=" + asocid, connection);
			connection.Open();
			string username = "";
			using (SqlDataReader rdr = cmda.ExecuteReader())
			{
				if (rdr.Read())
				{
					username = rdr["UserName"].ToString();
				}

				rdr.Close();
			}
			connection.Close();
			if (username.Length > 0)
			{
				try
				{
					MembershipUser User = Membership.Providers["PortalProvider"].GetUser(username, false);

					Username.Text = User.UserName;
					//Start generating from test date
					for (int year = firstyear; year <= DateTime.Now.Year; year++)
					{
						logger.Info(string.Format("GetMostreFerma|ext call year:{0}", year));
						CreateReportExt(year);
					}
				}
				catch { }
			}
		}


		string raport_csv = "CSV_Registru_" + datatestare.ToShortDateString().Replace("/", "") + "_" + fermaid + "_" + fermaname + ".csv";
		string csv_link = path_raport_http + @"Registru/" + raport_csv;
		string csv_file = filepath + @"Registru/" + raport_csv;

		logger.Info(string.Format("CreateReportExt|write csv file {0}", csv_file));
		CreateExcelFile(mostre, csv_file);

		logger.Info(string.Format("GetMostreFerma|processing done"));

		pdflink.Visible = true;
		pdflink.NavigateUrl = pdf_link;
		pdflink.Text = raport_name;

		xlslink.Visible = true;
		xlslink.NavigateUrl = excel_link;
		xlslink.Text = raport_excel;

		csvlink.Visible = true;
		csvlink.NavigateUrl = csv_link;
		csvlink.Text = raport_csv;
	}

    public void CreateReportExt(int year)
    {

        LocalReport report = new LocalReport();
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;
        Session["year"] = year;
        report.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

        int fermaid = 0;
        try { fermaid = Convert.ToInt32(FermaId.Text); }
        catch { fermaid = 0; }
        string username = Username.Text;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        if (fermaid != 0)
        {

            string[] parIds = { "g", "p", "c", "l", "s", "h", "u", "n", "z" };
            string[] chartLabelsName = { "Grasime (g/100g)", "Proteine (g/100g)", "Cazeina (g/100g)", "Lactoza (g/100g)", "SUN (g/100g)", "pH", "Uree (mg/dl)", "NCSx1000", "Cantitate lapte (l)" };
            string[] chartTitles = { "Grasime", "Proteine", "Cazeina", 
                                           "Lactoza", "SUN", "Ph",
                                           "Uree", "NCS","Cantitate lapte"};
            string[] um = { "g/100g", "g/100g", "g/100g", "g/100g", "g/100g", "pH", "mg/dl", "NCSx1000", "l" };


            //controale
            ReportDataSource rdsm = new ReportDataSource();
            rdsm.Name = "SampleAnalizeExt";
			logger.Info(string.Format("CreateReportExt|getting samples"));
			List<SampleAnalizeExt> samplesext = SampleAnalize.GetSampleAnalizeExt(fermaid, year);
			logger.Info(string.Format("CreateReportExt|got {0} samples", samplesext.Count));
			rdsm.Value = samplesext;
            report.DataSources.Clear();
            report.DataSources.Add(rdsm);

            report.ReportPath = "ControlsSummaryExt.rdlc";
            string datatestare = "Data: " + DateTime.Now.ToString("dd/MM/yyyy");
            // set reports parameters
            string title = FermaName.Text + " - " + "Cod exploatatie " + FermaCod.Text;

            ReportParameter pCod = new ReportParameter("CodExpl", FermaCod.Text);
            ReportParameter pData = new ReportParameter("DataExec", datatestare);
            //    ReportParameter pChartTitle = new ReportParameter("ChartTitle", chartTitles[i]);
            //   ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
            // ReportParameter pUm = new ReportParameter("Um", um[i]);
            ReportParameter pTitle = new ReportParameter("Title", title);
            ReportParameter pUsername = new ReportParameter("UserName", username);
            string baseurl = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + ConfigurationManager.AppSettings["baseurl"];
            ReportParameter pBaseurl = new ReportParameter("BaseUrl", baseurl);
            ReportParameter[] p = { pCod, pData, pTitle, pBaseurl, pUsername };
            report.SetParameters(p);
            report.Refresh();
            // write xls
            string httppath = ConfigurationManager.AppSettings["httppath"] + userinfos.AsocId + "/" + year +"/";
            //"/Downloads/";
            string filepath = ConfigurationManager.AppSettings["filepath"] + userinfos.AsocId + "\\"+ year +"\\";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            //"D:\\portal\\Downloads\\";
            string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
            string fermaname = replace_special_car_null(FermaName.Text);
            string raport_excel = FermaCod.Text + "_" + fermaname + ".xls";
            string excel_file = filepath + raport_excel;
            string excel_link = path_raport_http + raport_excel;
      //      infomess.Text = infomess.Text + ";" + excel_file;
            byte[] xls_content = report.Render("EXCEL", StaticData.DEVICE_INFO_XLS, out mimeType, out encoding, out fileExtension, out streams, out warnings);
            File.WriteAllBytes(excel_file, xls_content);
			logger.Info(string.Format("CreateReportExt|wrote excel file {0}", excel_file));
			//rename sheets
            List<SampleAnalize> samples = SampleAnalize.GetSampleAnalize(fermaid, "",year);
            using (StreamReader input = new StreamReader(excel_file))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));
                if (workbook != null && samples.Count > 0)
                {
                    for (int count = 0; count < samples.Count; count++)
                        workbook.SetSheetName(count, samples[count].Nrcontrol);

                    for (int count = 0; count < chartTitles.Length; count++)
                    {
                        string sheetname = chartTitles[count].Replace(" ", "");
                        workbook.SetSheetName(count + samples.Count, "Grafic" + sheetname);
                    }
                    //  workbook.SetSheetName(1, "Detaliu date");
                    WriteExcel(excel_file, workbook);
                }

            }
        }
    }

    public void WriteExcel(string filePath, HSSFWorkbook workbook)
    {
        if (File.Exists(filePath)) File.Delete(filePath);
        FileStream file = new FileStream(filePath, FileMode.Create);
        workbook.Write(file);
        file.Flush();
        file.Close();
    }

    protected void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
    {
        int fermaid = 0;
        try { fermaid = Convert.ToInt32(FermaId.Text); }
        catch { fermaid = 0; }
        string username = Username.Text;

        UserInfos userinfos = UserInfos.getUserInfos(username);
        string reportpath = e.ReportPath;
        string nrpar = e.Parameters["NrPar"].Values[0];
        int year = Int32.Parse(Session["year"].ToString());
        int i = 0;
        try { i = Convert.ToInt32(nrpar); }
        catch { i = 0; }
        if (fermaid != 0)
        {
            e.DataSources.Clear();

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "SampleAnalize";
            List<SampleAnalize> samples = SampleAnalize.GetSampleAnalize(fermaid, "",year);
            foreach (SampleAnalize sample in samples)
            {
                switch (i)
                {
                    case 0:
                        sample.Val = sample.Grasime; break;
                    case 1:
                        sample.Val = sample.Proteine; break;
                    case 2:
                        sample.Val = sample.Caseina; break;
                    case 3:
                        sample.Val = sample.Lactoza; break;
                    case 4:
                        sample.Val = sample.Solide; break;
                    case 5:
                        sample.Val = sample.Ph; break;
                    case 6:
                        sample.Val = sample.Urea; break;
                    case 7:
                        sample.Val = sample.Ncs; break;
                    case 8:
                        sample.Val = sample.Cantitate; break;
                    default: break;
                }
                sample.Val = Math.Round(sample.Val, 2);


            }
            rds.Value = samples;
            e.DataSources.Add(rds);

            //  
            ReportDataSource rdsc = new ReportDataSource();
            rdsc.Name = "SampleAnalizeControl";
            List<SampleAnalizeControl> samplescontrol = SampleAnalize.getSampleAnalizeControl(samples,year);
            rdsc.Value = samplescontrol;
            e.DataSources.Add(rdsc);
            // detaliu 
            ReportDataSource rdscrot = new ReportDataSource();
            rdscrot.Name = "SampleAnalizeCrotalia";
            List<SampleAnalizeCrotalia> samplescrot = SampleAnalize.getSampleAnalizeCrotalia(fermaid, i,year);
            rdscrot.Value = samplescrot;
            e.DataSources.Add(rdscrot);


        }
    }

	private bool CreateExcelFile(List<MostreFabrica> mostre, string filename)
	{
		if (File.Exists(filename))
			File.Delete(filename);
		StreamWriter oExcelWriter = File.CreateText(filename);
		List<string> sTableData = new List<string>();
		try
		{
			foreach (var mostra in mostre)
			{
                var urea = MostraValue(mostra.Urea);
                sTableData.Clear();
				sTableData.Add(mostra.DataPrelevare.ToString("yyyy-MM-dd"));
				sTableData.Add(mostra.CodBare);
				sTableData.Add(MostraValue(mostra.Grasime));
				sTableData.Add(MostraValue(mostra.Proteina));
				sTableData.Add(MostraValue(mostra.Casein));
				sTableData.Add(MostraValue(mostra.Lactoza));
				sTableData.Add(MostraValue(mostra.Substu));
				sTableData.Add(MostraValue(mostra.Ph));
                sTableData.Add((urea == string.Empty || Utils.ConvertToDouble(urea) < 0.1) ? string.Empty : urea);
				sTableData.Add(MostraValue(mostra.NCS));
				oExcelWriter.WriteLine(string.Join(",", sTableData));
			}
			return true;
		}
		catch
		{
			return false;
		}
		finally
		{
			oExcelWriter.Close();
		}
	}

    private string MostraValue(string value)
	{
		string retValue = string.Empty;
		double dVal;
        if (double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"), out dVal) && dVal != 0.00001 && dVal != 0)
		{
			retValue = dVal.ToString();
		}
		return retValue;
	}

}
