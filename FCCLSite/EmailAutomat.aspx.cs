using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using NLog;

public partial class EmailAutomat : Page
{
    public static Logger logger = LogManager.GetCurrentClassLogger();
    int eferma = 0;
    protected String a_loglink = "";
    string[] FilesArray1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["ferme"] != null)
        {
            eferma = Int32.Parse(Request.Params["ferme"]);
        }
        else
            eferma = 0;

        if (DataSelectie.Text.Trim() == "")
            DataSelectie.Text = (DateTime.Now).ToString("dd/MM/yyyy");

        fislog.Visible = false;
    }

    int CharCount(String strSource, String strToCount)
    {
        int iCount = 0;
        int iPos = strSource.IndexOf(strToCount);
        while (iPos != -1)
        {
            iCount++;
            strSource = strSource.Substring(iPos + 1);
            iPos = strSource.IndexOf(strToCount);
        }
        return iCount;
    }


    protected void dateChanged1(object sender, EventArgs e)
    {
        DataSelectie.Text = cal1.SelectedDate.ToString("dd/MM/yyyy");
        cal1.Visible = false;

    }
    protected void btnDate1_Click(object sender, EventArgs e)
    {
        try
        {
            if (DataSelectie.Text.Trim() != "") cal1.SelectedDate = Convert.ToDateTime(DataSelectie.Text);
        }
        catch
        {
        }
        cal1.Visible = true;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string adresafizicaserver = Server.MapPath("~");
        string cale_rap = Server.MapPath("~/Documents/");
        string cale_log = Server.MapPath("~/LogFiles/Mail/");

        string filename, filename_xls, serverfile;
        string paternfile;
        int id_ferma = 0;
        string strFerma;
        string strEmail;
        string nametmp;

        if (eferma == 1)
            paternfile = "*Ferma" + DataSelectie.Text.Trim().Replace(@"/", @"_") + "*.pdf";
        else
            paternfile = "*Fabrica" + DataSelectie.Text.Trim().Replace(@"/", @"_") + "*.pdf";


        if (eferma == 1)
            FilesArray1 = Directory.GetFiles(cale_rap + @"\Ferme\", paternfile);
        else
            FilesArray1 = Directory.GetFiles(cale_rap + @"\Fabrici\", paternfile);
        string logname;
        if (eferma == 1)
            logname = "LogEmailFerme" + DataSelectie.Text.Trim().Replace(@"/", @"_") + ".txt";
        else
            logname = "LogEmailFabrici" + DataSelectie.Text.Trim().Replace(@"/", @"_") + ".txt";

        string txtlog = "";
        MostreDB mostre = new MostreDB();
        if (eferma == 1)
            mostre.Addtext(adresafizicaserver + @"\Documents\Ferme\FisiereTrimisePeEmail\" + logname, "************" + "Data: " + DateTime.Now.ToString() + "**********");
        else
            mostre.Addtext(adresafizicaserver + @"\Documents\Fabrici\FisiereTrimisePeEmail\" + logname, "************" + "Data: " + DateTime.Now.ToString() + "**********");

        foreach (string strFile1 in FilesArray1)
        {
            filename = strFile1.Substring(strFile1.LastIndexOf(@"\") + 1);
            filename_xls = filename.Replace(".pdf", ".xls");
            string strFileXls = strFile1.Replace(".pdf", ".xls");
            if (CharCount(filename, "_") < 4 && !filename.ToUpper().EndsWith("TMP.PDF"))
            {
                if (eferma == 1)
                    serverfile = adresafizicaserver + @"\Documents\Ferme\FisiereTrimisePeEmail\" + filename;
                else
                    serverfile = adresafizicaserver + @"\Documents\Fabrici\FisiereTrimisePeEmail\" + filename;
                nametmp = filename.Substring(filename.LastIndexOf("-"));
                strFerma = filename.Replace(nametmp, "");
                strFerma = strFerma.Substring(filename.LastIndexOf("_") + 1);
                id_ferma = Convert.ToInt32(strFerma);
                if (eferma == 1)
                {
                    strEmail = GetEmailFerma(id_ferma);
                }
                else
                {
                    strEmail = GetEmailFabrica(id_ferma);
                }

                if (strEmail.Trim() != "")
                {
                    /***trimitere email***/
                    MailMessage objEmail = new MailMessage();
                    string to = strEmail.Trim();
                    string[] recipients = to.Split(new Char[] { ';' });
                    foreach (string recipient in recipients)
                        try
                        {
                            objEmail.To.Add(new MailAddress(recipient));
                        }
                        catch (Exception ex)
                        {
                            logger.Error(string.Format("Button1_Click|ERROR:{0}", ex.Message));
                            txtlog = "Failed to add address:" + recipient;
                            if (eferma == 1)
                                mostre.Addtext(adresafizicaserver + @"\Documents\Ferme\FisiereTrimisePeEmail\" + logname, txtlog);
                            else
                                mostre.Addtext(adresafizicaserver + @"\Documents\Fabrici\FisiereTrimisePeEmail\" + logname, txtlog);
                        }

                    objEmail.From = new MailAddress("office@control-lapte.ro");
                    objEmail.Subject = "Raport de Analiza a Laptelui";
                    objEmail.Body = "Atasat acestui email este raportul de analiza a laptelui";
                    //if (eferma==1)
                    {
                        objEmail.Body += " in format PDF. Pentru vizualizarea raportului veti avea nevoie de Acrobat Reader.<br/>";
                        objEmail.Body += "Puteti descarca ultima versiune de Acrobat Reader dand click pe link-ul de mai jos:<br/>";
                        objEmail.Body += "http://www.adobe.com/products/acrobat/readstep2.html";
                    }
                    objEmail.Priority = MailPriority.High;
                    objEmail.IsBodyHtml = true;
                    try
                    {
                        if (File.Exists(serverfile))
                            File.Delete(serverfile);
                        File.Move(strFile1, serverfile);

                        objEmail.Attachments.Add(new Attachment(serverfile));
                    }
                    catch { }
                    if (eferma == 0)
                    {
                        try
                        {
                            serverfile = serverfile.Replace(".pdf", ".xls");
                            if (File.Exists(serverfile))
                                File.Delete(serverfile);
                            File.Move(strFileXls, serverfile);

                            objEmail.Attachments.Add(new Attachment(serverfile));
                        }
                        catch { }
                    }

                    try
                    {
                        SmtpClient client = new SmtpClient();

                        client.Send(objEmail);


                        if (eferma == 1)
                            txtlog = "Fermei: " + nametmp + " cu adresa de email: " + strEmail + " i-a fost trimis fisierul: " + filename;
                        else
                            txtlog = "Fabricii: " + nametmp + " cu adresa de email: " + strEmail + " i-a fost trimis fisierul: " + filename + "," + filename_xls;
                    }
                    catch (Exception exc)
                    {
                        txtlog = "Send failure: " + exc.ToString();
                    }
                    if (eferma == 1)
                        mostre.Addtext(adresafizicaserver + @"\Documents\Ferme\FisiereTrimisePeEmail\" + logname, txtlog);
                    else
                        mostre.Addtext(adresafizicaserver + @"\Documents\Fabrici\FisiereTrimisePeEmail\" + logname, txtlog);
                } //if mail exists

            }// end count		
        }
        if (eferma == 1)
            fislog.NavigateUrl = HttpContext.Current.Request.ApplicationPath + @"/Documents/Ferme/FisiereTrimisePeEmail/" + logname;
        else
            fislog.NavigateUrl = HttpContext.Current.Request.ApplicationPath + @"/Documents/Fabrici/FisiereTrimisePeEmail/" + logname;

        // fislog.NavigateUrl = "LogFiles/Mail/" + logname + sErrorTime+".txt";
        fislog.Visible = true;
    }

    public string GetEmailFerma(int id)
    {
        string email = "";
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Ferme_CCL.Email " + "FROM Ferme_CCL WHERE Ferme_CCL.Cod=" + id;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        email = Convert.ToString(reader["Email"]);
        reader.Close();
        cnn.Close();
        return email;

    }

    public string GetEmailFabrica(int id)
    {
        string email = "";
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Fabrici.Email " + "FROM Fabrici WHERE Fabrici.ID=" + id;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        reader.Read();
        email = Convert.ToString(reader["Email"]);
        reader.Close();
        cnn.Close();
        return email;
    }
}