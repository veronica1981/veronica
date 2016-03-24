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

public partial class Account_SelectData : Page
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
        xlslinkg.Visible = xlslinkp.Visible = xlslinkl.Visible = xlslinkc.Visible = xlslinkh.Visible = xlslinkn.Visible = xlslinks.Visible = xlslinku.Visible = xlslinkz.Visible = xlslinkgp.Visible =false;
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int firstyear = Int32.Parse(ConfigurationManager.AppSettings["firstyear"]);
        int curryear = DateTime.Now.Year;

        if (!IsPostBack)
        {
            Session["year"] = curryear;
            for (int i = firstyear; i <= curryear; i++)
                yearsList.Items.Add("" + i);
            yearsList.DataBind();
            yearsList.SelectedIndex = yearsList.Items.IndexOf(yearsList.Items.FindByValue("" + curryear));
        }
        else curryear = Int32.Parse(Session["year"].ToString());
        if (Page.User.IsInRole("asoc"))
        {
            PanelA.Visible = true;
            PanelF.Visible = false;
            if (!IsPostBack)
            {
               // Session["year"] = curryear;
                GetFilesAndFolders(userinfos,curryear);
            }
        }
        else
        {
            PanelA.Visible = false;
            PanelF.Visible = true;
            if (!IsPostBack)
            {
             //   Session["year"] = curryear;
                string downloadFile = UserFileLink(userinfos, curryear);
                // check file name
                if (downloadFile.Length > 0)
                {
                    PanelR.Visible = true;
                    HL.NavigateUrl = "~/Download.ashx?file=" + downloadFile+"&year="+curryear;
                }
                else PanelR.Visible = false;
            }
        }
    }


    protected void UpdateLinks(object sender, EventArgs e)
    {
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int year = Int32.Parse(yearsList.SelectedItem.Value);
        Session["year"] = year;
        if (Page.User.IsInRole("asoc"))
        {
            
            GetFilesAndFolders(userinfos, year);
        }
        else
        {
            string downloadFile = UserFileLink(userinfos, year);
            // check file name
            if (downloadFile.Length > 0)
            {
                PanelR.Visible = true;
                HL.NavigateUrl = "~/Download.ashx?file=" + downloadFile+"&year="+year;
            }
            else PanelR.Visible = false;
        }

    }

    public string UserFileLink(UserInfos userinfos, int year)
    {
        string downloadLink = "";
        string filepath = ConfigurationManager.AppSettings["filepath"] + userinfos.AsocId + "\\"+ year +"\\";
        //"D:\\portal\\Downloads\\113\\";
        string fermaname = replace_special_car_null(userinfos.NumeFerma);
        string raport_excel = userinfos.UserCod + "_" + fermaname + ".xls";
        string excel_file = filepath + raport_excel;
        if (File.Exists(excel_file))
        {
        //    string httppath = ConfigurationManager.AppSettings["httppath"] + userinfos.AsocId + "/";
        //    downloadLink = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath + "/"+ raport_excel;
            downloadLink = raport_excel;
        }
        return downloadLink;
    }

    public void GetFilesAndFolders(UserInfos userinfos, int year)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["filepath"] + userinfos.AsocId + "\\"+ year +"\\");
        FileInfo[] fileInfo = dirInfo.GetFiles("*.xls", SearchOption.AllDirectories);
        List<FileInfos> fileInfos = new List<FileInfos>();
        foreach (FileInfo fi in fileInfo)
        {
            FileInfos f = new FileInfos();
            f.Name = fi.Name;
            f.DownloadLink = "~/Download.ashx?file=" + f.Name +"&year="+year;
            f.LastWriteTime = fi.LastWriteTime;
            f.Length = fi.Length/1024;
            fileInfos.Add(f);

        }

       
        xlsList.DataSource = fileInfos;
        xlsList.DataBind();
    }  

    protected void ButtonCreate_Click(object sender, EventArgs e)
    {
        
        CreateReport();
    }


    public void CreateReport()
    {

        LocalReport report = new LocalReport();
        report.EnableHyperlinks = true;
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
                string[] parIds = { "g", "p", "c", "l", "s", "h", "u", "n","z","gp" };
                string[] chartLabelsName = { "Grasime (g/100g)", "Proteine (g/100g)", "Cazeina (g/100g)", "Lactoza (g/100g)", "SUN (g/100g)", "pH", "Uree (mg/dl)", "NCSx1000", "Cantitate lapte (l)","Grasime/Proteine (%)"};
                string[] chartTitles = { "Grasime", "Proteine", "Cazeina", 
                                           "Lactoza", "SUN", "Ph",
                                           "Uree", "NCS","Cantitate lapte","Raport Grasime/Proteine"};
                string[] um = { "g/100g", "g/100g", "g/100g", "g/100g", "g/100g", "pH", "mg/dl", "NCSx1000", "l","%" };

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
                                case 9:
                                    sample.Val = sample.GrasProt;break;
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
                ReportParameter pCod = new ReportParameter("CodExpl", userinfos.UserCod);
                ReportParameter pData = new ReportParameter("DataExec",datatestare);
                ReportParameter pChartTitle = new ReportParameter("ChartTitle", chartTitles[i]);
                ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
                ReportParameter pUm = new ReportParameter("Um", um[i]);
                ReportParameter pNrpar = new ReportParameter("Nrpar", Convert.ToString(i));
                ReportParameter pTitle = new ReportParameter("Title", title);
                ReportParameter pUsername = new ReportParameter("UserName", username);
                string baseurl = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/"+ ConfigurationManager.AppSettings["baseurl"];
                ReportParameter pBaseurl = new ReportParameter("BaseUrl", baseurl);
                ReportParameter[] p = { pCod ,pData, pChartTitle,pChartLabelsName,pUm,pNrpar,pBaseurl,pTitle,pUsername};
                report.SetParameters(p);
                report.Refresh();
                // write xls
                string httppath = ConfigurationManager.AppSettings["httppath"];
                //"/Downloads/";
                string filepath = ConfigurationManager.AppSettings["filepath"];
                //"D:\\portal\\Downloads\\";
                string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
                string fileid = chartTitles[i];
                fileid = fileid.Replace(" ", ""); fileid = fileid.Replace("/", ""); fileid.Replace("%", "");
                if (i==8) fileid = fileid.Substring(0,4);
                string raport_excel = "Grafic"+ fileid + "_" + userinfos.UserCod + "_" + year + ".xls";
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
                for (int i = 0; i < 1 ; i++)
                {//parIds.Length
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
                        //controale
                        ReportDataSource rdsm = new ReportDataSource();
                        rdsm.Name = "SampleAnalizeExt";
                        List<SampleAnalizeExt> samplesext = SampleAnalize.GetSampleAnalizeExt(fermaid,year);
                        rdsm.Value = samplesext;
                        report.DataSources.Add(rdsm);

                        report.ReportPath = "ControlsSummaryAll.rdlc";
                        string datatestare = "Data: " + DateTime.Now.ToString("dd/MM/yyyy");
                        // set reports parameters
                        string title = userinfos.NumeFerma + " - " + "Cod exploatatie " + userinfos.UserCod;
                      
                        ReportParameter pCod = new ReportParameter("CodExpl", userinfos.UserCod);
                        ReportParameter pData = new ReportParameter("DataExec", datatestare);
                        ReportParameter pChartTitle = new ReportParameter("ChartTitle", chartTitles[i]);
                        ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
                        ReportParameter pUm = new ReportParameter("Um", um[i]);
                        ReportParameter pTitle = new ReportParameter("Title", title);
                        ReportParameter pUsername = new ReportParameter("UserName", username);
                        string baseurl = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + ConfigurationManager.AppSettings["baseurl"];
                        ReportParameter pBaseurl = new ReportParameter("BaseUrl", baseurl);
                        ReportParameter[] p = { pCod, pData, pChartTitle, pChartLabelsName, pUm,pTitle,pUsername,pBaseurl };
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


    public void CreateReportExt()
    {

        LocalReport report = new LocalReport();
        string mimeType, encoding, fileExtension;
        string[] streams;
        Warning[] warnings;
        int year = Int32.Parse(Session["year"].ToString());
        report.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

        
        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int fermaid = GetFermaId(userinfos.UserCod);
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
                 List<SampleAnalizeExt> samplesext = SampleAnalize.GetSampleAnalizeExt(fermaid,year);
                  rdsm.Value = samplesext;
                  report.DataSources.Clear();  
                  report.DataSources.Add(rdsm);

                  report.ReportPath = "ControlsSummaryExt.rdlc";
                  string datatestare = "Data: " + DateTime.Now.ToString("dd/MM/yyyy");
                        // set reports parameters
                 string title = userinfos.NumeFerma + " - " + "Cod exploatatie " + userinfos.UserCod;

                        ReportParameter pCod = new ReportParameter("CodExpl", userinfos.UserCod);
                        ReportParameter pData = new ReportParameter("DataExec", datatestare);
                    //    ReportParameter pChartTitle = new ReportParameter("ChartTitle", chartTitles[i]);
                     //   ReportParameter pChartLabelsName = new ReportParameter("ChartLabelsName", chartLabelsName[i]);
                       // ReportParameter pUm = new ReportParameter("Um", um[i]);
                        ReportParameter pTitle = new ReportParameter("Title", title);
                        ReportParameter pUsername = new ReportParameter("UserName", username);
                        string baseurl = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + ConfigurationManager.AppSettings["baseurl"];
                        ReportParameter pBaseurl = new ReportParameter("BaseUrl", baseurl);
                        ReportParameter[] p = { pCod, pData,pTitle,pBaseurl,pUsername };
                        report.SetParameters(p);
                        report.Refresh();
                        // write xls
                        string httppath = ConfigurationManager.AppSettings["httppath"]+userinfos.AsocId+"/" + year +"/";
                        //"/Downloads/";
                        string filepath = ConfigurationManager.AppSettings["filepath"]+userinfos.AsocId+"\\" + year +"\\";
                        if (!Directory.Exists(filepath))
                        {
                            Directory.CreateDirectory(filepath);
                        } 

                        //"D:\\portal\\Downloads\\";
                        string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
                        string fileid = chartTitles[0];
                        string fermaname = replace_special_car_null(userinfos.NumeFerma); 
                        string raport_excel = userinfos.UserCod + "_" + fermaname +".xls";
                        string excel_file = filepath + raport_excel;
                        string excel_link = path_raport_http + raport_excel;
                        byte[] xls_content = report.Render("EXCEL", deviceInfoXls, out mimeType, out encoding, out fileExtension, out streams, out warnings);
                        File.WriteAllBytes(excel_file, xls_content);
                        //rename sheets
                        List<SampleAnalize> samples = SampleAnalize.GetSampleAnalize(fermaid, "",year);
                        using (StreamReader input = new StreamReader(excel_file))
                        {
                            HSSFWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));
                            if (workbook != null && samples.Count >0)
                            {
                                for (int count=0; count < samples.Count; count++)
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

                        //
                        ContentPlaceHolder holder = (ContentPlaceHolder)Master.FindControl("MainContent");
                        HyperLink hyper = (HyperLink)holder.FindControl("xlslink" + parIds[0]);
                        hyper.Visible = true;
                        hyper.NavigateUrl = excel_link;
                        hyper.Text = raport_excel;
                        ErrorMessage.Text = "";
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

    protected void ItemsSubreportProcessingEventHandler(object sender,SubreportProcessingEventArgs e)
    {

        string username = User.Identity.Name;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        int fermaid = GetFermaId(userinfos.UserCod);
        string reportpath = e.ReportPath;
        string nrpar = e.Parameters["NrPar"].Values[0].ToString();
        int i = 0;
        int year = Int32.Parse(Session["year"].ToString());
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
 
   


}