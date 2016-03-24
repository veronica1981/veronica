using FCCL_BL.Helpers;
using FCCL_BL.Managers;
using FCCL_DAL.Entities;
using NLog;
using System.Configuration;

namespace Worker
{
    public class SendSmsWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void RunProcess(string[] args)
        {
            Logger.Info("SendSmsWorker | Sms Worker started");

            var smsArchiveManager = new SmsArchiveManager(ConfigurationManager.ConnectionStrings["fccl_v2"].ConnectionString);

            Sms sms = null;

            while ((sms = smsArchiveManager.Dequeue()) != null)
            {
                Logger.Info(string.Format("SendSmsWorker | Sending sms with Id: {0}", sms.Id));
                SmsHelper.SendSms(sms);
            }

        }


    }
}
