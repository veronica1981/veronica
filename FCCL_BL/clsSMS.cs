using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace FCCL_BL
{
    public class clsSMS
    {
        //public SerialPort port;
        public AutoResetEvent receiveNow;

        //Open Port
        public SerialPort OpenPort(string strPortName, string strBaudRate)
        {
            receiveNow = new AutoResetEvent(false);
            SerialPort port = new SerialPort();
            port.PortName = strPortName;
            port.BaudRate = Convert.ToInt32(strBaudRate);               //updated by Anila (9600)
            port.DataBits = 8;
            port.StopBits = StopBits.One;
            port.Parity = Parity.None;
            port.ReadTimeout = 300;
            port.WriteTimeout = 300;
            port.Encoding = Encoding.GetEncoding("iso-8859-1");
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            if (port.IsOpen == false)
                port.Open();
            port.DtrEnable = true;
            port.RtsEnable = true;
            String PhoneNo = "+40722004000";
            String command = "AT+CSCA=\"" + PhoneNo + "\"";
            String recievedData = ExecCommand(port, command, 300, "Failed to accept phoneNo - " + PhoneNo + "," + "");
            return port;
        }

        //Close Port
        public void ClosePort(SerialPort port)
        {
            port.Close();
            port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
            port = null;
        }

        //Execute AT Command
        public string ExecCommand(SerialPort port, string command, int responseTimeout, string errorMessage)
        {
            string input = "";
            try
            {
                // receiveNow = new AutoResetEvent();

                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
                port.Write(command + "\r");

                // Thread.Sleep(3000); //3 seconds
                input = ReadResponse(port, responseTimeout);
                if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                    throw new ApplicationException("No success message was received.");
                return input;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message + "  " + errorMessage + "\r\n" + input + ".", ex);
            }
        }

        //Receive data from port
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
                receiveNow.Set();
        }
        public string ReadResponse(SerialPort port, int timeout)
        {
            string buffer = string.Empty;
            do
            {
                if (receiveNow.WaitOne(timeout, false))
                {
                    string t = port.ReadExisting();
                    buffer += t;
                }
                else
                {

                    port.DiscardOutBuffer();
                    port.DiscardInBuffer();
                    receiveNow.Reset();
                    if (buffer.Length > 0)
                        throw new ApplicationException("Response received is incomplete - " + buffer);
                    else
                        throw new ApplicationException("No data received from phone.");
                }
            }
            while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            return buffer;
        }



        #region Send SMS

        static AutoResetEvent readNow = new AutoResetEvent(false);

        public bool sendMsg(SerialPort port, string strPortName, string strBaudRate, string PhoneNo, string Message)
        {
            bool isSend = false;
            try
            {

                //this.port = OpenPort(strPortName,strBaudRate);
                string recievedData = ExecCommand(port, "AT", 500, "No phone connected at " + strPortName + " - " + PhoneNo + "," + Message);
                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format -" + PhoneNo + "," + Message);
                String command = "AT+CMGS=\"" + PhoneNo + "\"";
                recievedData = ExecCommand(port, command, 300, "Failed to accept phoneNo - " + PhoneNo + "," + Message);
                command = Message + char.ConvertFromUtf32(26) + "\r";
                recievedData = ExecCommand(port, command, 15000, "Failed to send message - " + PhoneNo + "," + Message); //3 seconds
                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    recievedData = "Message sent successfully";
                    isSend = true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    string recievedError = recievedData;
                    recievedError = recievedError.Trim();
                    recievedData = "Following error occured while sending the message" + recievedError;
                    isSend = false;
                }
                else
                {
                    recievedData = "Nothing";
                    isSend = true;
                }

                return isSend;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (port != null)
                {
                    //port.Close();
                    //port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                    //port = null;
                }
            }
        }
        static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
                readNow.Set();
        }

        #endregion

        #region Delete SMS
        public void DeleteMsg(SerialPort port, string strPortName, string strBaudRate)
        {
            try
            {
                #region Open Port
                //this.port = OpenPort(strPortName,strBaudRate);
                #endregion

                #region Execute Command
                string recievedData = ExecCommand(port, "AT", 300, "No phone connected at " + strPortName + ".");
                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                String command = "AT+CMGD=1,3";
                recievedData = ExecCommand(port, command, 300, "Failed to delete message");
                #endregion

                if (recievedData.EndsWith("\r\nOK\r\n"))
                    recievedData = "Message delete successfully";
                if (recievedData.Contains("ERROR"))
                {
                    string recievedError = recievedData;
                    recievedError = recievedError.Trim();
                    recievedData = "Following error occured while sending the message" + recievedError;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (port != null)
                {
                    #region Close Port
                    //port.Close();
                    //port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                    //port = null;
                    #endregion
                }
            }
        }
        #endregion

    }
}
