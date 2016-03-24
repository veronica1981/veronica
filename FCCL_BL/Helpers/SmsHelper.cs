using System;
using System.Configuration;
using System.IO.Ports;
using FCCL_BL.Managers;
using FCCL_DAL.Entities;
using NLog;

namespace FCCL_BL.Helpers
{
    public class SmsHelper
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void SendSms(Sms sms)
        {
            var smsArchiveManager = new SmsArchiveManager(ConfigurationManager.ConnectionStrings["fccl_v2"].ConnectionString);
            SerialPort port = new SerialPort();
            clsSMS objclsSMS = new clsSMS();
            string portname = ConfigurationManager.AppSettings["ComPort"].ToString();
            string baudrate = "57600";
            port = objclsSMS.OpenPort(portname, baudrate);
            try
            {
                if (objclsSMS.sendMsg(port, portname, "57600", sms.CellNr, sms.Message))
                {
                    Logger.Info(string.Format("SendSmsWorker | Sms-ul cu id-ul: {0} a fost trimis", sms.Id));
                    sms.DateSend = DateTime.Now;
                    smsArchiveManager.UpdateSms(sms);
                }
                else
                {
                    sms.TryNr++;
                    smsArchiveManager.UpdateSms(sms);
                    Logger.Error(string.Format("SendSmsWorker | Trimitere esuata a sms-ului cu id-ul: {0}", sms.Id));
                }
                objclsSMS.ClosePort(port);
            }
            catch (Exception ex)
            {
                sms.TryNr++;
                smsArchiveManager.UpdateSms(sms);
                Logger.Error(string.Format("SendSmsWorker | Trimitere esuata a sms-ului cu id-ul: {0} error: {1}",
                    sms.Id, ex.Message));

            }
            finally
            {
                objclsSMS.ClosePort(port);
            }
        }
    }
}
