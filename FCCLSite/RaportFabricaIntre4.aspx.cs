using System.Web.UI;
using FCCL_BL.Managers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

public partial class RaportFabricaIntre4 : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        pdflink.Visible = false;
        xlslink.Visible = false;
        if (TextBox1.Text.Trim() == "")
            TextBox1.Text = (DateTime.Now).ToString("dd/MM/yyyy");
        if (TextBox2.Text.Trim() == "")
            TextBox2.Text = (DateTime.Now).ToString("dd/MM/yyyy");

        if (!IsPostBack)
        {
            ddlLaborator.SelectedIndex = 0;
            ddlResponsabil.SelectedIndex = 1;
        }
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        DropDownList1.DataBind();
    }

    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {
        DropDownList1.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.Items.Count > 0)
            CreateReport();
    }

    public void CreateReport()
    {
        LocalReport report = new LocalReport();
        byte[] file;
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;

        ReportDataSource rds = new ReportDataSource();
        rds.Name = "MostreFabrica";

        List<MostreFabrica> mostre = MostreFabrica.GetMostreFabrica(DropDownList1.SelectedValue, TextBox1.Text, TextBox2.Text);

        string datatestare = TextBox1.Text;
        string datatestare2 = TextBox2.Text;

        rds.Value = mostre;

        report.DataSources.Clear();
        report.DataSources.Add(rds);

        report.ReportPath = "ReportFabricaIntre4.rdlc";

        report.Refresh();

        string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>29.7cm</PageWidth>" +
            "  <PageHeight>21cm</PageHeight>" +
            "  <MarginTop>0.0cm</MarginTop>" +
            "  <MarginLeft>0.1cm</MarginLeft>" +
            "  <MarginRight>0.1cm</MarginRight>" +
            "  <MarginBottom>0.5cm</MarginBottom>" +
            "</DeviceInfo>";

        string deviceInfoXls =
            "<DeviceInfo>" +
            "  <OutputFormat>XLS</OutputFormat>" +
            "  <PageWidth>29.7cm</PageWidth>" +
            "  <PageHeight>21cm</PageHeight>" +
            "  <MarginTop>0.0cm</MarginTop>" +
            "  <MarginLeft>0.0cm</MarginLeft>" +
            "  <MarginRight>0.1cm</MarginRight>" +
            "  <MarginBottom>0.1cm</MarginBottom>" +
            "</DeviceInfo>";

        string fabricaid = DropDownList1.SelectedValue;
        string fabricaname = DropDownList1.Items[DropDownList1.SelectedIndex].ToString();
        // read fabrica
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Fabrici.Strada,Fabrici.Numar,Fabrici.Oras,Judete.Denloc "
        + "FROM Fabrici,Judete WHERE Fabrici.ID=" + fabricaid + " AND Fabrici.Judet=Judete.ID";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        string fabricastrada = Convert.ToString(reader["Strada"]);
        string fabricanumar = Convert.ToString(reader["Numar"]);
        string fabricaoras = Convert.ToString(reader["Oras"]);
        string fabricajudet = Convert.ToString(reader["Denloc"]);
        reader.Close();
        cnn.Close();

        //set report parameters
        string laborator = ddlLaborator.Items[ddlLaborator.SelectedIndex].ToString();
        string responsabil = ddlResponsabil.Items[ddlResponsabil.SelectedIndex].ToString();
        string combi = mostre[0].Combi;
        ReportParameter pDatatestare = new ReportParameter("datatestare", datatestare);
        ReportParameter pDatatestare2 = new ReportParameter("datatestare2", datatestare2);
        ReportParameter pCombi = new ReportParameter("combi", combi);
        ReportParameter pFabricanume = new ReportParameter("fabricanume", fabricaname);
        ReportParameter pFabricastrada = new ReportParameter("fabricastrada", fabricastrada);
        ReportParameter pFabricanumar = new ReportParameter("fabricanumar", fabricanumar);
        ReportParameter pFabricaoras = new ReportParameter("fabricaoras", fabricaoras);
        ReportParameter pFabricajudet = new ReportParameter("fabricajudet", fabricajudet);
        ReportParameter pLaborator = new ReportParameter("laborator", laborator);
        ReportParameter pResponsabil = new ReportParameter("responsabil", responsabil);
        ReportParameter pVersiune = new ReportParameter("Versiune", new SettingManager(StaticDataHelper.FCCLDbContext).GetValueByName("ReportFabricaIntre4"));

        ReportParameter[] p = { pDatatestare, pDatatestare2, pCombi, pFabricanume, pFabricastrada, pFabricanumar, pFabricaoras, pFabricajudet, pLaborator, pResponsabil, pVersiune };
        report.SetParameters(p);

        file = report.Render("PDF", deviceInfo, out mimeType, out encoding, out fileExtension, out streams, out warnings);
        string httppath = StaticDataHelper.SettingsManager.CaleRapoarteHttp;
        string filepath = StaticDataHelper.SettingsManager.CaleRapoarte;

        fabricaname = replace_special_car_null(fabricaname);

        string raport_name = "RaportFabrica" + datatestare.Replace("/", "_") + "_" + datatestare2.Replace("/", "_") + "_" + fabricaid + "-" + fabricaname + ".pdf";
        string raport_excel = "RaportFabrica" + datatestare.Replace("/", "_") + "_" + datatestare2.Replace("/", "_") + "_" + fabricaid + "-" + fabricaname + ".xls";

        string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
        string pdf_link = path_raport_http + @"Fabrici/" + raport_name;
        string pdf_file = filepath + @"Fabrici/" + raport_name;
        // writefile

        string excel_link = path_raport_http + @"Fabrici/" + raport_excel;
        string excel_file = filepath + @"Fabrici/" + raport_excel;
        report.SetParameters(new ReportParameter("BkImage", ""));
        File.WriteAllBytes(pdf_file, file);
        byte[] file_xls = report.Render("EXCEL", deviceInfoXls, out mimeType, out encoding, out fileExtension, out streams, out warnings);
        File.WriteAllBytes(excel_file, file_xls);

        pdflink.Visible = true;
        pdflink.NavigateUrl = pdf_link;
        pdflink.Text = raport_name;

        xlslink.Visible = true;
        xlslink.NavigateUrl = excel_link;
        xlslink.Text = raport_excel;
    }

    protected string replace_special_car(string nume_ini)
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

    protected string replace_special_car_null(string nume_ini)
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
}