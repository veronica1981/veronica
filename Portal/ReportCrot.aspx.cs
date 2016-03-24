using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using NLog;

public partial class ReportCrot : Page
{
    public static Logger Logger = LogManager.GetCurrentClassLogger();

    static string deviceInfoXls =

"<DeviceInfo>" +

"  <OutputFormat>Excel</OutputFormat>" +

"  <PageWidth>29.7cm</PageWidth>" +

"  <PageHeight>21cm</PageHeight>" +

"  <MarginTop>0.1cm</MarginTop>" +

"  <MarginLeft>0.1cm</MarginLeft>" +

"  <MarginRight>0.1cm</MarginRight>" +

"  <MarginBottom>0.1cm</MarginBottom>" +

"</DeviceInfo>";

    protected void Page_Load(object sender, EventArgs e)
    {
       
        string username = Request.Params["codexpl"];
        string crot = Request.Params["crot"];
        string nrpar = Request.Params["nrpar"];
        string url = CreateReport(username, crot,nrpar);
        if (url.Length > 0)
        {
            Response.Redirect(url);
            FailureText.Text = "";
        }
        else
        {
            FailureText.Text = "Page cannot be displayed";
        }


    }

    public string CreateReport(string username, string crot, string nrpar)
    {
        string url = "";
        try
        {
            int i = Convert.ToInt32(nrpar);
            string[] parIds = {"g", "p", "c", "l", "s", "h", "u", "n", "z", "gp"};
            string[] chartLabelsName =
            {
                "Grasime (g/100g)", "Proteine (g/100g)", "Cazeina (g/100g)", "Lactoza (g/100g)",
                "SUN (g/100g)", "pH", "Uree (mg/dl)", "NCSx1000", "Cantitate lapte (l)", "Grasime/Proteine (%)"
            };
            string[] chartTitles =
            {
                "Grasime", "Proteine", "Cazeina",
                "Lactoza", "SUN", "Ph",
                "Uree", "NCS", "Cantitate lapte", "Raport Grasime/Proteine"
            };
            string[] um = {"g/100g", "g/100g", "g/100g", "g/100g", "g/100g", "pH", "mg/dl", "NCSx1000", "l", "%"};

            //  string username = Page.User.Identity.Name;
            UserInfos userinfos = UserInfos.getUserInfos(username);
            int fermaid = GetFermaId(userinfos.UserCod);
            int year = Int32.Parse(Session["year"].ToString());
            string baseurl = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" +
                             ConfigurationManager.AppSettings["httppath"];
            string filepath = ConfigurationManager.AppSettings["filepath"];
            string fileid = chartTitles[i];
            fileid = fileid.Replace("/", "");
            fileid = fileid.Replace("/", "");
            if (i == 8) fileid = fileid.Substring(0, 4);
            string raport_excel = "Grafic" + fileid + "_" + userinfos.UserCod + "_" + crot + "_" + year + ".xls";

            LocalReport report = new LocalReport();
            report.EnableHyperlinks = true;
            string mimeType, encoding, fileExtension;
            string[] streams;
            Warning[] warnings;

            if (fermaid != 0)
            {
                List<SampleAnalize> samples = SampleAnalize.GetSampleAnalize(fermaid, crot, year);
                if (samples.Count > 0)
                {
                    //set current param
                    foreach (SampleAnalize sample in samples)
                    {
                        switch (i)
                        {
                            case 0:
                                sample.Val = sample.Grasime;
                                break;
                            case 1:
                                sample.Val = sample.Proteine;
                                break;
                            case 2:
                                sample.Val = sample.Caseina;
                                break;
                            case 3:
                                sample.Val = sample.Lactoza;
                                break;
                            case 4:
                                sample.Val = sample.Solide;
                                break;
                            case 5:
                                sample.Val = sample.Ph;
                                break;
                            case 6:
                                sample.Val = sample.Urea;
                                break;
                            case 7:
                                sample.Val = sample.Ncs;
                                break;
                            case 8:
                                sample.Val = sample.Cantitate;
                                break;
                            case 9:
                                sample.Val = 0;
                                if (sample.Proteine > 0)
                                    sample.Val = sample.Grasime/sample.Proteine;
                                break;
                            default:
                                break;
                        }
                        sample.Val = Math.Round(sample.Val, 2);
                    }
                    //datasources
                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "SampleAnalize";
                    rds.Value = samples;
                    report.DataSources.Clear();
                    report.DataSources.Add(rds);

                    //  
                    ReportDataSource rdsc = new ReportDataSource();
                    rdsc.Name = "SampleAnalizeControl";
                    List<SampleAnalizeControl> samplescontrol = SampleAnalize.getSampleAnalizeControl(samples, year);
                    rdsc.Value = samplescontrol;
                    report.DataSources.Add(rdsc);
                    //parameters
                    report.ReportPath = "GraficCrotalie.rdlc";
                    string datatestare = "Data: " + DateTime.Now.ToString("dd/MM/yyyy");
                    // set reports parameters
                    string title = userinfos.NumeFerma + " - " + "Cod exploatatie " + userinfos.UserCod;
                    ReportParameter pCod = new ReportParameter("CodExpl", title);
                    ReportParameter pData = new ReportParameter("DataExec", datatestare);
                    ReportParameter pChartTitle = new ReportParameter("ChartTitle",
                        chartTitles[i] + " - Nr. matricol: " + crot);
                    ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
                    ReportParameter pUm = new ReportParameter("Um", um[i]);

                    ReportParameter[] p = {pCod, pData, pChartTitle, pChartLabelsName, pUm};
                    report.SetParameters(p);
                    report.Refresh();

                    string excel_file = filepath + raport_excel;
                    byte[] xls_content = report.Render("EXCEL", deviceInfoXls, out mimeType, out encoding,
                        out fileExtension, out streams, out warnings);
                    File.WriteAllBytes(excel_file, xls_content);
                    url = baseurl + raport_excel;

                }
            }
            else
            {
                FailureText.Text = "Nu exista date pentru codul " + userinfos.UserCod + " si numarul matricol " + crot;
            }
        }

        catch (Exception ex)
        {
            string err = ex.Message;
            string trace = ex.StackTrace;
            Logger.Error(trace + "|" + err);
        }
        return url;
    }

    public int GetFermaId(string codexpl)
    {
        // Create Instance of Connection and Command Object
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        SqlCommand cmd = new SqlCommand("Portal_GetFermeExpl", conn);

        // Mark the Command as a SPROC
        cmd.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterFermaId = new SqlParameter("@FermaId", SqlDbType.Int, 4);
        parameterFermaId.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(parameterFermaId);

        SqlParameter parameterExpl = new SqlParameter("@Cod", SqlDbType.VarChar, 15);
        parameterExpl.Value = codexpl;
        cmd.Parameters.Add(parameterExpl);

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        finally
        {
            conn.Close();
        }

        if (parameterFermaId.Value == DBNull.Value)
            return 0;
        else
            return (int)parameterFermaId.Value;
    }
}