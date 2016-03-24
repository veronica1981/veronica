using System;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;

public partial class Login : Page
{
    public static Logger logger = LogManager.GetCurrentClassLogger();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Roles.CreateRole("admin");
        //Roles.AddUserToRole("superstar", "admin");
        if (Page.User.IsInRole("admin") || Page.User.IsInRole("user"))
        {
            Panel2.Visible = false;
            Panel1.Visible = true;
        }
        else
        {
            Panel2.Visible = true;
            Panel1.Visible = false;
        }
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string userName = Login1.UserName;
        string password = Login1.Password;
        if (Membership.ValidateUser(userName, password))
        {
         //   FormsAuthentication.RedirectFromLoginPage(userName, true);

        }
        else
        {
            Login1.FailureText = "Logare nereusita!";
          /*
            lblResults.Visible = true;
            lblResults.Text = "Unsuccessful login. Please re-enter your information and try again.";
            if ((Membership.GetUser(userName) != null) && (Membership.GetUser(userName).IsLockedOut == true))
                lblResults.Text += "  Your account has been locked out.";
           */ 
        }


    }
    protected void Login1_LoginError(object sender, EventArgs e)
    {
       

            

    }

    protected void Check_Backup()
    {
        string strBackup = ConfigurationManager.AppSettings["BackupName"];
        try
        {
            FileInfo fi = new FileInfo(strBackup);

            DateTime lastdate = fi.LastWriteTime;

            DateTime currdate = DateTime.Now;

            TimeSpan diffResult = currdate.Subtract(lastdate);

            if (diffResult.Days > 7)
            {
                string strdate = (lastdate).ToString("dd/MM/yyyy");
                string smtpserver = ConfigurationManager.AppSettings["SmtpServer"];
                string strto = ConfigurationManager.AppSettings["AdminEmail"];
                //sendmail
                MailMessage objEmail = new MailMessage();
                objEmail.To.Add(new MailAddress(strto));
                objEmail.From = new MailAddress(strto);
                objEmail.Subject = "Ultima data a salvarii" + strdate;
                objEmail.Body = "Ultima salvare este mai veche de 7 zile!";
                objEmail.Priority = MailPriority.High;
                objEmail.IsBodyHtml = true;
                try
                {
                    SmtpClient client = new SmtpClient(smtpserver);

                    client.Send(objEmail);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("Check_Backup|ERROR:{0}", ex.Message));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(string.Format("Check_Backup|ERROR:{0}", ex.Message));
        }
    }
}
