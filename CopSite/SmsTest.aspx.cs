using System;
using System.Configuration;
using System.IO.Ports;
using System.Web.UI;
using FCCL_BL;

public partial class SmsTest : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string cellnumber = txtTo.Text;
        string message = txtComments.Text;
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        string portname = ConfigurationManager.AppSettings["ComPort"];
        string baudrate = "57600";
        port = objclsSMS.OpenPort(portname, baudrate);
        try
        {
            if (objclsSMS.sendMsg(port, portname, "57600", cellnumber, message))
                Label1.Text = "Sms-ul a fost trimis!";
            else
                Label1.Text = "Trimitere esuata!";
            objclsSMS.ClosePort(port);
        }
        catch (Exception ex)
        {
            Label1.Text = ex.Message;
			 objclsSMS.ClosePort(port);
        }
    }
   
}
