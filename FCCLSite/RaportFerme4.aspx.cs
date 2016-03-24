using System.Web.UI;
using FCCL_BL;
using FCCL_BL.Managers;
using FCCL_DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Report = FCCL_DAL.Report;


public partial class RaportFerme4 : Page
{
    private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext;

    protected void Page_Load(object sender, EventArgs e)
    {
        linkall.Visible = false;
        Repeater1.Visible = false;
        if (TextBox1.Text.Trim() == "")
            TextBox1.Text = (DateTime.Now).ToString("dd/MM/yyyy");

        if (!IsPostBack)
        {
            ddlLaborator.SelectedIndex = 0;
            ddlResponsabil.SelectedIndex = 1;
        }

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        DropDownList1.DataBind();
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
        nume_fin = nume_fin.Replace("\t", "");
        // sper ca alte caractere ca <>?{}[]^()%$#!~ nu se vor folosi in numele fermei si al fabricii
        return (nume_fin);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.Items.Count > 0)
            CreateReport();
    }

    public void CreateReport()
    {
        ReportManager rManager = new ReportManager(ctx);
        DateTime testDate;
        int farmId;
        DateTime.TryParse(TextBox1.Text, out testDate);
        Int32.TryParse(DropDownList1.SelectedValue, out farmId);

        Report b = rManager.GetByDateAndId(testDate, farmId);
        string nrb = b.ReportNumber.ToString();
        string datab = b.TestDate.ToShortDateString();
        int pos = 0;

        LocalReport report = new LocalReport();
        byte[] file;
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;

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
        string datatestare = TextBox1.Text;
        // date fabrica 

        //deviceinfo

        string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>29.7cm</PageWidth>" +
            "  <PageHeight>21cm</PageHeight>" +
            "  <MarginTop>0.0cm</MarginTop>" +
            "  <MarginLeft>0.5cm</MarginLeft>" +
            "  <MarginRight>0.5cm</MarginRight>" +
            "  <MarginBottom>0.5cm</MarginBottom>" +
            "</DeviceInfo>";

        string httppath = StaticDataHelper.SettingsManager.CaleRapoarteHttp;
        string filepath = StaticDataHelper.SettingsManager.CaleRapoarte;

        cmd.CommandTimeout = 300;
        cmd.CommandText = "SELECT DISTINCT Ferme_CCL.Nume,Ferme_CCL.ID, MostreTancuri.FermaID,  Ferme_CCL.Cod, "
+ "Ferme_CCL.Strada,Ferme_CCL.Numar, Ferme_CCL.Oras, Ferme_CCL.Judet,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Ferme_CCL.Ferme_CCL,Ferme_CCL.FermierID,Judete.Denloc "
+ " FROM MostreTancuri, Ferme_CCL INNER JOIN JUDETE ON  Convert(int,Ferme_CCL.Judet,2)=Judete.ID WHERE"
+ " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
+ " AND MostreTancuri.CodFerma = Ferme_CCL.Cod " + " AND Ferme_CCL.FabricaID = " + fabricaid
+ " AND MostreTancuri.Validat = 1 ORDER BY Ferme_CCL.Nume";

        cnn.Open();
        SqlDataReader drferme = cmd.ExecuteReader();
        //prepare report parameters
        string laborator = ddlLaborator.Items[ddlLaborator.SelectedIndex].ToString();
        string responsabil = ddlResponsabil.Items[ddlResponsabil.SelectedIndex].ToString();
        string semnlab = ddlLaborator.SelectedValue;
        string semnresp = ddlResponsabil.SelectedValue;
        semnresp = semnresp.Substring(0, semnresp.LastIndexOf("."));
        semnlab = semnlab.Substring(0, semnlab.LastIndexOf("."));
        semnresp = semnresp.ToLower();
        semnlab = semnlab.ToLower();

        ReportParameter pDatatestare = new ReportParameter("datatestare", datatestare);
        ReportParameter pDatab = new ReportParameter("datab", datab);
        ReportParameter pNrb = new ReportParameter("nrb", nrb);
        ReportParameter pFabricanume = new ReportParameter("fabricanume", fabricaname);
        ReportParameter pFabricastrada = new ReportParameter("fabricastrada", fabricastrada);
        ReportParameter pFabricanumar = new ReportParameter("fabricanumar", fabricanumar);
        ReportParameter pFabricaoras = new ReportParameter("fabricaoras", fabricaoras);
        ReportParameter pFabricajudet = new ReportParameter("fabricajudet", fabricajudet);
        ReportParameter pLaborator = new ReportParameter("laborator", laborator);
        ReportParameter pResponsabil = new ReportParameter("responsabil", responsabil);

        List<FileLinks> list = new List<FileLinks>();
        while (drferme.Read())
        {
            FileLinks filelink = new FileLinks();
            string fermacod = Convert.ToString(drferme["Cod"]);
            string fermanume = Convert.ToString(drferme["Nume"]);
            string fermastrada = Convert.ToString(drferme["Strada"]);
            string fermanumar = Convert.ToString(drferme["Numar"]);
            string fermaoras = Convert.ToString(drferme["Oras"]);
            string judet = Convert.ToString(drferme["Denloc"]);
            string codferma = Convert.ToString(drferme["Cod"]);
            string fermaadresa = "Str. " + fermastrada + " nr. " + fermanumar + ", " + fermaoras;
            string fermatelcontact = Convert.ToString(drferme["TelPersoanaContact"]);
            string fermaperscontact = Convert.ToString(drferme["PersonaDeContact"]);

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "MostreFabrica";
            List<MostreFabrica> mostre = MostreFabrica.GetMostreFerma(fabricaid, fermacod, datatestare);
            rds.Value = mostre;
            // build dates interval
            string datatmin = datatestare;
            string datatmax = datatestare;
            string datatesttitlu = datatestare;

            string dataprimirii = "";
            string datapmin = dataprimirii;
            string datapmax = dataprimirii;
            string dataprimtitlu = dataprimirii;

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

            // end dates interval           
            report.DataSources.Clear();
            report.DataSources.Add(rds);

            pos++;
            report.ReportPath = "ReportFerme4.rdlc";

            report.Refresh();

            ReportParameter pDatatitlu = new ReportParameter("datatestare", datatesttitlu);
            ReportParameter pDataprimirii = new ReportParameter("dataprimirii", dataprimtitlu);
            ReportParameter pCombi = new ReportParameter("combi", combi);
            ReportParameter pPos = new ReportParameter("pos", pos.ToString());
            ReportParameter pCodferma = new ReportParameter("codferma", codferma);
            ReportParameter pFermanume = new ReportParameter("fermanume", fermanume);
            ReportParameter pFermaadresa = new ReportParameter("fermaadresa", fermaadresa);
            ReportParameter pFermajudet = new ReportParameter("fermajudet", judet);
            ReportParameter pFermaperscontact = new ReportParameter("fermaperscontact", fermaperscontact);
            ReportParameter pFermatelcontact = new ReportParameter("fermatelcontact", fermatelcontact);
            ReportParameter versiuneRaport = new ReportParameter("Versiune", new SettingManager(StaticDataHelper.FCCLDbContext).GetValueByName("ReportFerme4"));

            ReportParameter[] p ={ pDatatitlu,pDataprimirii,pCombi,pDatab,pNrb,pFabricanume,pFabricastrada,pFabricanumar,pFabricaoras,pFabricajudet,pLaborator,pResponsabil,
    pPos,pFermanume,pFermaadresa,pFermajudet,pCodferma,pFermaperscontact,pFermatelcontact, versiuneRaport};

            report.SetParameters(p);

            file = report.Render("PDF", deviceInfo, out mimeType, out encoding, out fileExtension, out streams, out warnings);
            // read from date fixe!!!

            fermanume = replace_special_car_null(fermanume);

            string raport_name = "RaportFerma" + datatestare.Replace("/", "_") + "_" + fermacod + "-" + fermanume + ".pdf";
            string raport_excel = "RaportFerma" + datatestare.Replace("/", "_") + "_" + fermacod + "-" + fermanume + ".xls";

            string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
            string pdf_link = path_raport_http + @"Ferme/" + raport_name;
            string pdf_file = filepath + @"Ferme/" + raport_name;
            File.WriteAllBytes(pdf_file, file);

            filelink.Url = pdf_link;
            filelink.Des = raport_name;
            list.Add(filelink);
        }

        drferme.Close();
        cnn.Close();
        //raport cumulat
        LocalReport report_cum = new LocalReport();
        ReportDataSource rdsc = new ReportDataSource();
        rdsc.Name = "MostreFabrica1";
        rdsc.Value = MostreFabrica.GetMostreFabrica(fabricaid, datatestare);

        report_cum.DataSources.Clear();
        report_cum.DataSources.Add(rdsc);

        report_cum.ReportPath = "ReportCumulat4.rdlc";
        report_cum.Refresh();

        ReportParameter pSemnlab = new ReportParameter("semnlab", semnlab);
        ReportParameter pSemnresp = new ReportParameter("semnresp", semnresp);
        ReportParameter pVersiuneRaport = new ReportParameter("Versiune", new SettingManager(StaticDataHelper.FCCLDbContext).GetValueByName("ReportCumulat4"));

        ReportParameter[] rp = { pDatatestare, pDatab, pNrb, pFabricanume, pFabricastrada, pFabricanumar, pFabricaoras, pFabricajudet, pLaborator, pResponsabil, pSemnlab, pSemnresp, pVersiuneRaport };

        report_cum.SetParameters(rp);

        byte[] file_cum = report_cum.Render("PDF", deviceInfo, out mimeType, out encoding, out fileExtension, out streams, out warnings);
        string fabricanume = replace_special_car_null(fabricaname);

        string raport_name_cum = "RaportCumulat" + datatestare.Replace("/", "_") + "_" + fabricaid + "-" + fabricanume + ".pdf";

        string path_raport_http_cum = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
        string pdf_link_cum = path_raport_http_cum + @"Ferme/" + raport_name_cum;
        string pdf_file_cum = filepath + @"Ferme/" + raport_name_cum;
        // writefile

        File.WriteAllBytes(pdf_file_cum, file_cum);
        linkall.Visible = true;
        linkall.NavigateUrl = pdf_link_cum;
        linkall.Text = raport_name_cum;
        //
        Repeater1.DataSource = list;
        Repeater1.DataBind();
        Repeater1.Visible = true;
    }
}
