using System;
using System.Data.SqlClient; 
using System.Configuration;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Web.UI;

public partial class EmailRegistru : Page
{
    protected String a_loglink = "";
    string[] FilesArray1;

    protected void Page_Load(object sender, EventArgs e)
    {

        //eferma = 1;
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

        string filename,serverfile;
        string paternfile;
        string strEmail = "";
      
         paternfile = "CSV_Registru*" + DataSelectie.Text.Trim().Replace(@"/", @"") + "*.csv";

      
        FilesArray1 = Directory.GetFiles(cale_rap + @"\Registru\", paternfile);
        string logname;
        logname = "LogEmailRegistru" + DataSelectie.Text.Trim().Replace(@"/", @"_") + ".txt";
       
       
        string txtlog = "";
        MostreDB mostre = new MostreDB();
        mostre.Addtext(adresafizicaserver + @"\Documents\Registru\FisiereTrimisePeEmail\" + logname, "************" + "Data: " + DateTime.Now.ToString() + "**********");
        
        foreach (string strFile1 in FilesArray1)
        {
          filename = strFile1.Substring(strFile1.LastIndexOf(@"\") + 1);
          string strFileXls = strFile1;
        //  if (CharCount(filename, "_") < 4 && !filename.ToUpper().EndsWith("TMP.PDF"))
          {
              
               serverfile = adresafizicaserver + @"\Documents\Registru\FisiereTrimisePeEmail\" + filename;
			  string withoutext = filename.Substring(0, filename.LastIndexOf('.')); 
              string[] parts = withoutext.Split('_');
              string fermaid = parts[parts.Length - 2];

               strEmail = GetEmailFerma(fermaid);
                if (strEmail.Trim() != "")
                {
                    /***trimitere email***/
                    MailMessage objEmail = new MailMessage();
                    string to = strEmail.Trim();
                  //  string to ="ancay13@yahoo.de";
                    string[] recipients = to.Split(new Char[] { ';' });
                    foreach (string recipient in recipients)
                        objEmail.To.Add(new MailAddress(recipient));


                    objEmail.From = new MailAddress("office@control-lapte.ro");
                    objEmail.Subject = "Raport Registru";
                    objEmail.Body = "Atasat acestui email este raportul pentru registru";
                    
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
                    // send xls
                    try
                        {
                          
                            if (File.Exists(serverfile))
                                File.Delete(serverfile);
                            File.Move(strFileXls, serverfile);

                            objEmail.Attachments.Add(new Attachment(serverfile));
                        }
                        catch { }
                    

                    try
                    {
                        SmtpClient client = new SmtpClient();

                        client.Send(objEmail);


                        txtlog = "Fisierul : " + filename + " a fost trimis.";
                    }
                    catch (Exception exc)
                    {
                        txtlog = "Send failure: " + exc;
                    }
                    mostre.Addtext(adresafizicaserver + @"\Documents\Registru\FisiereTrimisePeEmail\" + logname, txtlog);
                } //if mail exists

            }// end count		
        }// end foreach
        string sYear = DateTime.Now.Year.ToString();
        string sMonth = DateTime.Now.Month.ToString();
        string sDay = DateTime.Now.Day.ToString();
        string sErrorTime = sYear + sMonth + sDay;
   	    fislog.NavigateUrl  = HttpContext.Current.Request.ApplicationPath + @"/Documents/Registru/FisiereTrimisePeEmail/"+logname;

        fislog.Visible = true;
    }

    public string GetEmailFerma(string cod)
    {
        string email = "";
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        try {
        cmd.CommandText = "SELECT Ferme_CCL.FabricaId " + "FROM Ferme_CCL WHERE Ferme_CCL.ID = " + cod;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        string fabricaid ="";
        if (reader.Read())
        {
            fabricaid = Convert.ToString(reader["FabricaId"]);
            reader.Close();
        }
        cmd.CommandText = "SELECT Fabrici.Email " + "FROM Fabrici WHERE Fabrici.ID = " + fabricaid;
            reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            email = Convert.ToString(reader["Email"]);
            reader.Close();
        }
		}
		catch {}
             
        cnn.Close();
        return email;
    }
}
