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
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;

public partial class Account_SelectDataAsoc : Page
{
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
        string httppath = ConfigurationManager.AppSettings["httppath"];
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
      //  asocnume.Text = userinfos.NumeAsoc;
        //string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath +"/" + userinfos.AsocId;
        //HyperLinkAsoc.NavigateUrl = path_raport_http;
        if (!IsPostBack)
        {
             int firstyear = Int32.Parse(ConfigurationManager.AppSettings["firstyear"]);
             int curryear = DateTime.Now.Year;
             for (int year=firstyear; year<=curryear; year++) 
                yearsList.Items.Add(""+year);
             yearsList.DataBind();
             yearsList.SelectedIndex = yearsList.Items.IndexOf(yearsList.Items.FindByValue(""+curryear)); 
             GetFilesAndFolders(userinfos, curryear);  
        }  
    }


    public void GetFilesAndFolders(UserInfos userinfos, int year)  
    {  
        DirectoryInfo dirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["filepath"] + userinfos.AsocId +"\\" + year +"\\");  
        FileInfo[] fileInfo = dirInfo.GetFiles("*.xls*", SearchOption.AllDirectories);
        List<FileInfos> fileInfos = new List<FileInfos>();
        foreach (FileInfo fi in fileInfo)
        {
            FileInfos f = new FileInfos();
            f.Name = fi.Name;
            f.DownloadLink = "~/Download.ashx?file="+ f.Name;
           
            fileInfos.Add(f);

        }
     
        xlsLinks.DataSource = fileInfos;
        xlsLinks.DataBind();
    }


    protected void UpdateLinks(object sender, EventArgs e)
    {
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int year = Int32.Parse(yearsList.SelectedItem.Value);
        GetFilesAndFolders(userinfos, year);  
    }

    protected void ButtonCreate_Click(object sender, EventArgs e)
    {
        
        CreateReportAll();
    }


    public void CreateReport()
    {

        LocalReport report = new LocalReport();
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;

        int year = Int32.Parse(Session["year"].ToString());
        ReportDataSource rds = new ReportDataSource();
        rds.Name = "SampleAnalize";
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int fermaid = GetFermaId(userinfos.UserCod);
        if (fermaid != 0)
        {
            List<SampleAnalize> samples = SampleAnalize.GetSampleAnalize(fermaid,"",year);
            if (samples.Count > 0)
            {
                string[] parIds = { "g", "p", "c", "l", "s", "h", "u", "n","z" };
                string[] chartLabelsName = { "Grasime (g/100g)", "Proteine (g/100g)", "Cazeina (g/100g)", "Lactoza (g/100g)", "SUN (g/100g)", "pH", "Uree (mg/dl)", "NCSx1000", "Cantitate lapte (l)" };
                string[] chartTitles = { "Grasime", "Proteine", "Cazeina", 
                                           "Lactoza", "SUN", "Ph",
                                           "Uree", "NCS","Cantitate lapte"};
                string[] um = { "g/100g", "g/100g", "g/100g", "g/100g", "g/100g", "pH", "mg/dl", "NCSx1000", "l" };

                ContentPlaceHolder holder = (ContentPlaceHolder)Master.FindControl("MainContent");
                for (int i = 0; i < parIds.Length; i++)
                {
                    string endChar = parIds[i].ToUpper();
                    CheckBox chk = (CheckBox)holder.FindControl("chk" + endChar);
                    if (chk.Checked)
                    {
                        //set current param
                        foreach (SampleAnalize sample in samples) {
                            switch (i)
                            {
                                case 0:
                                    sample.Val = sample.Grasime;break;
                                case 1:
                                    sample.Val = sample.Proteine;break;
                                case 2:
                                    sample.Val = sample.Caseina;break;
                                case 3:
                                    sample.Val = sample.Lactoza;break;
                                case 4:
                                    sample.Val = sample.Solide;break;
                                case 5:
                                    sample.Val = sample.Ph;break;
                                case 6:
                                    sample.Val = sample.Urea;break;
                                case 7:
                                    sample.Val = sample.Ncs;break;
                                case 8:
                                    sample.Val = sample.Cantitate;break;
                                default: break;
                            }
                            sample.Val = Math.Round(sample.Val, 2);
                            
                        
                        }
                        // create samplescontrol from samples 
                   
                // set reports datasource
                rds.Value = samples;
                report.DataSources.Clear();
                report.DataSources.Add(rds);

                //  
                ReportDataSource rdsc = new ReportDataSource();
                rdsc.Name = "SampleAnalizeControl";
                List<SampleAnalizeControl> samplescontrol = SampleAnalize.getSampleAnalizeControl(samples,year);
                rdsc.Value = samplescontrol;
                report.DataSources.Add(rdsc);
                // detaliu 
                ReportDataSource rdscrot = new ReportDataSource();
                rdscrot.Name = "SampleAnalizeCrotalia";
                List<SampleAnalizeCrotalia> samplescrot = SampleAnalize.getSampleAnalizeCrotalia(fermaid,i,year);
                rdscrot.Value = samplescrot;
                report.DataSources.Add(rdscrot);

                report.ReportPath = "ControlsSummary.rdlc";
                string datatestare = "Data: "+DateTime.Now.ToString("dd/MM/yyyy");
                // set reports parameters
                string title = userinfos.NumeFerma + " - " + "Cod exploatatie " + userinfos.UserCod;
                ReportParameter pCod = new ReportParameter("CodExpl", title);
                ReportParameter pData = new ReportParameter("DataExec",datatestare);
                ReportParameter pChartTitle = new ReportParameter("ChartTitle", chartTitles[i]);
                ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
                ReportParameter pUm = new ReportParameter("Um", um[i]);

                ReportParameter[] p = { pCod ,pData, pChartTitle,pChartLabelsName,pUm};
                report.SetParameters(p);
                report.Refresh();
                // write xls
                string httppath = ConfigurationManager.AppSettings["httppath"];
                //"/Downloads/";
                string filepath = ConfigurationManager.AppSettings["filepath"];
                //"D:\\portal\\Downloads\\";
                string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
                string fileid = chartTitles[i];
                if (i==8) fileid = fileid.Substring(0,4);
                string raport_excel = "Grafic"+ fileid + "_" + userinfos.UserCod + "_" + (DateTime.Now).ToString("ddMMyyyy") + ".xls";
                string excel_file = filepath + raport_excel;
                string excel_link = path_raport_http + raport_excel;
                byte[] xls_content = report.Render("EXCEL", deviceInfoXls, out mimeType, out encoding, out fileExtension, out streams, out warnings);
                File.WriteAllBytes(excel_file, xls_content);
                //rename sheets
                using (StreamReader input = new StreamReader(excel_file))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));
                    if (workbook != null)
                    {
                      workbook.SetSheetName(0,"Grafic");
                      workbook.SetSheetName(1,"Detaliu date");
                      WriteExcel(excel_file, workbook);  
                    }
                    
                }

                //

              //  HyperLink hyper = (HyperLink)holder.FindControl("xlslink" + parIds[i]);        
               // hyper.Visible = true;
                //hyper.NavigateUrl = excel_link;
                //hyper.Text = raport_excel;
                ErrorMessage.Text = "";
                    } //checked
                }//for
            }//samplecount >0
        }//fermaid >0
        else
        {
           // ErrorMessage.Text = "Nu exista date pentru codul "+ userinfos.UserCod;
        }
    }


    public void CreateReportAll()
    {

        LocalReport report = new LocalReport();
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;

        int year = Int32.Parse(Session["year"].ToString());
        ReportDataSource rds = new ReportDataSource();
        rds.Name = "SampleAnalize";
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int fermaid = GetFermaId(userinfos.UserCod);
        if (fermaid != 0)
        {
            List<SampleAnalize> samples = SampleAnalize.GetSampleAnalize(fermaid,"",year);
            if (samples.Count > 0)
            {
                string[] parIds = { "g", "p", "c", "l", "s", "h", "u", "n", "z" };
                string[] chartLabelsName = { "Grasime (g/100g)", "Proteine (g/100g)", "Cazeina (g/100g)", "Lactoza (g/100g)", "SUN (g/100g)", "pH", "Uree (mg/dl)", "NCSx1000", "Cantitate lapte (l)" };
                string[] chartTitles = { "Grasime", "Proteine", "Cazeina", 
                                           "Lactoza", "SUN", "Ph",
                                           "Uree", "NCS","Cantitate lapte"};
                string[] um = { "g/100g", "g/100g", "g/100g", "g/100g", "g/100g", "pH", "mg/dl", "NCSx1000", "l" };

                ContentPlaceHolder holder = (ContentPlaceHolder)Master.FindControl("MainContent");
                for (int i = 0; i < parIds.Length; i++)
                {
                    string endChar = parIds[i].ToUpper();
                    CheckBox chk = (CheckBox)holder.FindControl("chk" + endChar);
                    if (chk.Checked)
                    {
                        //set current param
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
                        // create samplescontrol from samples 

                        // set reports datasource
                        rds.Value = samples;
                        report.DataSources.Clear();
                        report.DataSources.Add(rds);

                        //  
                        ReportDataSource rdsc = new ReportDataSource();
                        rdsc.Name = "SampleAnalizeControl";
                        List<SampleAnalizeControl> samplescontrol = SampleAnalize.getSampleAnalizeControl(samples,year);
                        rdsc.Value = samplescontrol;
                        report.DataSources.Add(rdsc);
                        // detaliu 
                        ReportDataSource rdscrot = new ReportDataSource();
                        rdscrot.Name = "SampleAnalizeCrotalia";
                        List<SampleAnalizeCrotalia> samplescrot = SampleAnalize.getSampleAnalizeCrotalia(fermaid, i,year);
                        rdscrot.Value = samplescrot;
                        report.DataSources.Add(rdscrot);

                        report.ReportPath = "ControlsSummaryAll.rdlc";
                        string datatestare = "Data: " + DateTime.Now.ToString("dd/MM/yyyy");
                        // set reports parameters
                        string title = userinfos.NumeFerma + " - " + "Cod exploatatie " + userinfos.UserCod;
                        ReportParameter pCod = new ReportParameter("CodExpl", title);
                        ReportParameter pData = new ReportParameter("DataExec", datatestare);
                        ReportParameter pChartTitle = new ReportParameter("ChartTitle", chartTitles[i]);
                        ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
                        ReportParameter pUm = new ReportParameter("Um", um[i]);

                        ReportParameter[] p = { pCod, pData, pChartTitle, pChartLabelsName, pUm };
                        report.SetParameters(p);
                        report.Refresh();
                        // write xls
                        string httppath = ConfigurationManager.AppSettings["httppath"];
                        //"/Downloads/";
                        string filepath = ConfigurationManager.AppSettings["filepath"];
                        //"D:\\portal\\Downloads\\";
                        string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
                        string fileid = chartTitles[i];
                        if (i == 8) fileid = fileid.Substring(0, 4);
                        string raport_excel = "Grafic" + fileid + "_" + userinfos.UserCod + "_" + (DateTime.Now).ToString("ddMMyyyy") + ".xls";
                        string excel_file = filepath + raport_excel;
                        string excel_link = path_raport_http + raport_excel;
                        byte[] xls_content = report.Render("EXCEL", deviceInfoXls, out mimeType, out encoding, out fileExtension, out streams, out warnings);
                        File.WriteAllBytes(excel_file, xls_content);
                        //rename sheets
                        using (StreamReader input = new StreamReader(excel_file))
                        {
                            HSSFWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));
                            if (workbook != null)
                            {
                                workbook.SetSheetName(0, chartTitles[i]);
                              //  workbook.SetSheetName(1, "Detaliu date");
                                WriteExcel(excel_file, workbook);
                            }

                        }

                        //

                        HyperLink hyper = (HyperLink)holder.FindControl("xlslink" + parIds[i]);
                        hyper.Visible = true;
                        hyper.NavigateUrl = excel_link;
                        hyper.Text = raport_excel;
                        ErrorMessage.Text = "";
                    } //checked
                }//for
            }//samplecount >0
        }//fermaid >0
        else
        {
            // ErrorMessage.Text = "Nu exista date pentru codul "+ userinfos.UserCod;
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