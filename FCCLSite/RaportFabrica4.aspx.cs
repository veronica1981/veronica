using System.Web.UI;
using FCCL_BL.Bus;
using FCCL_BL.Managers;
using FCCL_DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Report = FCCL_DAL.Report;


public partial class RaportFabrica4 : Page
{
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
        if (TextBox1.Text.Trim() == "")
            TextBox1.Text = (DateTime.Now).ToString("dd/MM/yyyy");
        if (TextBox2.Text.Trim() == "")
            TextBox2.Text = (DateTime.Now).ToString("dd/MM/yyyy");

		if(!IsPostBack)
		{
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
        nume_fin = nume_fin.Replace("_", "");// _ folosit in regula de parse pt email
        nume_fin = nume_ini.Replace(" ", "");
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
        LocalReport report = new LocalReport();
        byte[] file;
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;

		long reportNumber;
		ReportManager rManager = new ReportManager(ctx);
		Report dalReport = rManager.GetOrCreateReport(FCCLReportType.Fabrica, ObjectId
			, DropDownList1.SelectedItem.Text, TestDate, PrintDate, long.TryParse(TextBox3.Text, out reportNumber) ? (long?)reportNumber : null);
		TextBox3.Text = dalReport.ReportNumber.ToString();

		ReportDataSource rds = new ReportDataSource();
        rds.Name = "MostreFabrica";

        List<MostreFabrica> mostre = MostreFabrica.GetMostreFabrica(DropDownList1.SelectedValue, TextBox1.Text);
        rds.Value = mostre;
        string datatestare = TextBox1.Text;

        string datatmin = datatestare;
        string datatmax = datatestare;
        string datatesttitlu = datatestare;

        string dataprimirii ="";
        string datapmin = dataprimirii;
        string datapmax = dataprimirii;
        string dataprimtitlu = dataprimirii;
        
        string nrcomanda = "";
        string combi = mostre[0].Combi;
        foreach (MostreFabrica mf in mostre)
        {
            string datamin = mf.DataTestare;
            string datamax = mf.DataTestareFinala;
            dataprimirii = mf.DataPrimirii;
            if (datapmin == "")
                datapmin = dataprimirii;
            if (datapmax == "")
                datapmax = dataprimirii;
            
            if (mf.NrComanda.Trim() != "")
                nrcomanda = mf.NrComanda.Trim();
            if (datamin != datatmin)
            {
                DateTime dt1 = new DateTime(Int32.Parse(datamin.Substring(6, 4)), Int32.Parse(datamin.Substring(3, 2)), Int32.Parse(datamin.Substring(0, 2)));
                DateTime dt2 = new DateTime(Int32.Parse(datatmin.Substring(6, 4)), Int32.Parse(datatmin.Substring(3, 2)), Int32.Parse(datatmin.Substring(0, 2)));
                if (DateTime.Compare(dt1, dt2) < 0)
                    datatmin = datamin;
            }
            if (datamax != datatmax)
            {
                DateTime dt1 = new DateTime(Int32.Parse(datamax.Substring(6, 4)), Int32.Parse(datamax.Substring(3, 2)), Int32.Parse(datamax.Substring(0, 2)));
                DateTime dt2 = new DateTime(Int32.Parse(datatmax.Substring(6, 4)), Int32.Parse(datatmax.Substring(3, 2)), Int32.Parse(datatmax.Substring(0, 2)));
                if (DateTime.Compare(dt1, dt2) > 0)
                    datatmax = datamax;
            }
            // data primirii
            if (dataprimirii != datapmin)
            {
                DateTime dt1 = new DateTime(Int32.Parse(dataprimirii.Substring(6, 4)), Int32.Parse(dataprimirii.Substring(3, 2)), Int32.Parse(dataprimirii.Substring(0, 2)));
                DateTime dt2 = new DateTime(Int32.Parse(datapmin.Substring(6, 4)), Int32.Parse(datapmin.Substring(3, 2)), Int32.Parse(datapmin.Substring(0, 2)));
                if (DateTime.Compare(dt1, dt2) < 0)
                    datapmin = dataprimirii;
            }
            if (dataprimirii != datapmax)
            {
                DateTime dt1 = new DateTime(Int32.Parse(dataprimirii.Substring(6, 4)), Int32.Parse(dataprimirii.Substring(3, 2)), Int32.Parse(dataprimirii.Substring(0, 2)));
                DateTime dt2 = new DateTime(Int32.Parse(datapmax.Substring(6, 4)), Int32.Parse(datapmax.Substring(3, 2)), Int32.Parse(datapmax.Substring(0, 2)));
                if (DateTime.Compare(dt1, dt2) > 0)
                    datapmax = dataprimirii;
            }

        }
        if (datatmin != datatmax)
            datatesttitlu = datatmin + " - " + datatmax;
        if (datapmin != datapmax)
            dataprimtitlu = datapmin + " si " + datapmax;
        else
            dataprimtitlu = datapmin;

        rds.Value = mostre;

        report.DataSources.Clear();
        report.DataSources.Add(rds);

        report.ReportPath = "ReportFabrica4.rdlc";
        report.Refresh();

        string fabricaid = DropDownList1.SelectedValue;
        string fabricaname = DropDownList1.Items[DropDownList1.SelectedIndex].ToString();
        // read fabrica
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Fabrici.Strada,Fabrici.Numar,Fabrici.Oras,Judete.Denloc " + "FROM Fabrici,Judete WHERE Fabrici.ID=" + fabricaid + " AND Fabrici.Judet=Judete.ID";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        string fabricastrada = Convert.ToString(reader["Strada"]);
        string fabricanumar = Convert.ToString(reader["Numar"]);
        string fabricaoras = Convert.ToString(reader["Oras"]);
        string fabricajudet = Convert.ToString(reader["Denloc"]);
        reader.Close();
        cnn.Close();
        string nrb = dalReport.ReportNumber.ToString();
        //set report parameters
        string laborator = ddlLaborator.Items[ddlLaborator.SelectedIndex].ToString();
        string responsabil = ddlResponsabil.Items[ddlResponsabil.SelectedIndex].ToString();

        ReportParameter pNrcomanda = new ReportParameter("nrcomanda", nrcomanda);
        ReportParameter pDatatestare = new ReportParameter("datatestare", datatesttitlu);
        ReportParameter pDataprimirii = new ReportParameter("dataprimirii", dataprimtitlu);
        ReportParameter pCombi = new ReportParameter("combi", combi);
        ReportParameter pDatab = new ReportParameter("datab", TextBox2.Text);
        ReportParameter pNrb = new ReportParameter("nrb", nrb);
        ReportParameter pFabricanume = new ReportParameter("fabricanume", fabricaname);
        ReportParameter pFabricastrada = new ReportParameter("fabricastrada", fabricastrada);
        ReportParameter pFabricanumar = new ReportParameter("fabricanumar", fabricanumar);
        ReportParameter pFabricaoras = new ReportParameter("fabricaoras", fabricaoras);
        ReportParameter pFabricajudet = new ReportParameter("fabricajudet", fabricajudet);
        ReportParameter pLaborator = new ReportParameter("laborator", laborator);
        ReportParameter pResponsabil = new ReportParameter("responsabil", responsabil);
		ReportParameter pVersiune = new ReportParameter("Versiune", new SettingManager(StaticDataHelper.FCCLDbContext).GetValueByName("ReportFabrica4"));
        ReportParameter[] p ={ pDatatestare, pDataprimirii,pCombi, pNrcomanda, pDatab, pNrb, pFabricanume, pFabricastrada, pFabricanumar, pFabricaoras, 
								 pFabricajudet, pLaborator, pResponsabil, pVersiune };
        report.SetParameters(p);

        file = report.Render("PDF", StaticData.DEVICE_INFO_PDF, out mimeType, out encoding, out fileExtension, out streams, out warnings);

		string httppath = StaticDataHelper.SettingsManager.CaleRapoarteHttp;
		string filepath = StaticDataHelper.SettingsManager.CaleRapoarte;
        fabricaname = replace_special_car_null(fabricaname);
        string raport_name = "RaportFabrica" + datatestare.Replace("/", "_") + "_" + fabricaid + "-" + fabricaname + ".pdf";
        string raport_excel = "RaportFabrica" + datatestare.Replace("/", "_") + "_" + fabricaid + "-" + fabricaname + ".xls";
        string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
        string pdf_link = path_raport_http + @"Fabrici/" + raport_name;
        string pdf_file = filepath + @"Fabrici/" + raport_name;
        // writefile

        string excel_link = path_raport_http + @"Fabrici/" + raport_excel;
        string excel_file = filepath + @"Fabrici/" + raport_excel;

        File.WriteAllBytes(pdf_file, file);
		File.WriteAllBytes(pdf_file, file);
		dalReport.SampleCount = mostre.Count;
		dalReport.PageCount = PDFHelper.GetNumberOfPdfPages(pdf_file);
		rManager.Save(dalReport);
		ctx.SaveChanges();

        // raport excel
        LocalReport rapexcel = new LocalReport();

        ReportDataSource rdse = new ReportDataSource();
        rdse.Name = "MostreFabrica";
        rdse.Value = mostre;
        rapexcel.DataSources.Clear();
        rapexcel.DataSources.Add(rdse);
        rapexcel.ReportPath = "ReportFabricaExcel4.rdlc";
        rapexcel.SetParameters(p);
        rapexcel.Refresh();

        byte[] file_xls = rapexcel.Render("EXCEL", StaticData.DEVICE_INFO_XLS, out mimeType, out encoding, out fileExtension, out streams, out warnings);
        // end raport excel
        File.WriteAllBytes(excel_file, file_xls);

        pdflink.Visible = true;
        pdflink.NavigateUrl = pdf_link;
        pdflink.Text = raport_name;

        xlslink.Visible = true;
        xlslink.NavigateUrl = excel_link;
        xlslink.Text = raport_excel;
    }
}
