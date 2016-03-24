using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;
using NLog;

public partial class Account_PasswordRecovery : Page
{
    public static Logger Logger = LogManager.GetCurrentClassLogger();

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
    {
        Label lbl = (Label)PasswordRecovery1.SuccessTemplateContainer.FindControl("EmailLabel");
        lbl.Text = e.Message.To[0].Address;
        MembershipUser user = Membership.GetUser(PasswordRecovery1.UserName);
        string username = user.UserName;
        UserInfos userinfos = UserInfos.getUserInfos(username);
        string generatedPassword = user.ResetPassword();
        bool isChanged = false;
        try
        {
            string newPassword = "";
            newPassword = userinfos.UserCod ?? generatedPassword;
           //string newPassword = GetRandomPasswordUsingGUID(10);
           isChanged = user.ChangePassword(generatedPassword, newPassword);
		   user.IsApproved = true;
           e.Message.Body = "Parola dvs. a fost resetata. Va rugam sa va logati folosind ID-ul de  cont: " + user.UserName + " si parola: " + newPassword ;
           e.Message.Subject = "Recuperare parola";
           e.Message.Bcc.Add(new MailAddress("office@control-lapte.ro"));
            //send mail to fccl 
        }
        catch (Exception ex)
        {
            string err = ex.Message;
            string trace = ex.StackTrace;
            Logger.Error(trace+"|"+err);
        }
    }
    protected void PasswordRecovery1_VerifyingUser(object sender, LoginCancelEventArgs e)
    {
        ((Literal)PasswordRecovery1.UserNameTemplateContainer.FindControl("FailureText")).Text = "ID cont inexistent.";
 
    }
}