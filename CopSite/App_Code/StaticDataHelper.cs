using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using FCCL_BL;
using FCCL_BL.Managers;
using FCCL_DAL;
using System.Configuration;

public class StaticDataHelper
{
	public static IntervalManager IntervalManager 
	{ 
		get
		{
			return new IntervalManager(FCCLDbContext);
		}
	}

	public static SettingManager SettingsManager
	{
		get
		{
			return new SettingManager(FCCLDbContext);
		}
	}

	public static FCCLDbContext FCCLDbContext
	{
		get
		{
			return ContextHelper.GetContext(ConfigurationManager.AppSettings["ApplicationNo"].ToString());
		}
	}

    public static void SendMailAsync(object mailMessage)
    {
        var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        var credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);

        var smtpClient = new SmtpClient()
        {
            UseDefaultCredentials = false,
            Credentials = credentials
        };

        smtpClient.SendAsync((MailMessage)mailMessage, null);
    }
}
