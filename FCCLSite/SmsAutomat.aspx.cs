using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Web.UI;
using FCCL_BL;

public partial class SmsAutomat : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (DataSelectie.Text.Trim() == "")
            DataSelectie.Text = (DateTime.Now).ToString("dd/MM/yyyy");

        fislog.Visible = false;
        fissms.Visible = false;
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
        string cale_rap = Server.MapPath("~/Documents/Sms/");
        string cale_log = Server.MapPath("~/Documents/Sms/");

        string logname = "LogSmsFerme" + DataSelectie.Text.Trim().Replace(@"/", @"_") + ".txt";
        string smsname = "SmsTrimise" + DataSelectie.Text.Trim().Replace(@"/", @"_") + ".txt";
        string fulllogname = adresafizicaserver + @"\Documents\Sms\" + logname;
        string fullsmsname = adresafizicaserver + @"\Documents\Sms\" + smsname;
        string datatestare = DataSelectie.Text;
        MostreDB mostre = new MostreDB();
        List<MostreFabrica> mostresms = MostreFabrica.GetMostreSMS(datatestare);
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        string portname = ConfigurationManager.AppSettings["ComPort"].ToString();
        string baudrate = "9600";
        port = objclsSMS.OpenPort(portname, baudrate);

        StringBuilder updateSql = new StringBuilder("update mostretancuri set SentSms=1 where ");
        int countSms = 0;
        int countError = 0;

        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname, "************" + "Data: " + DateTime.Now.ToString() + "**********");
        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + smsname, "************" + "Data: " + DateTime.Now.ToString() + "**********");
        int counter = 0;
        foreach (MostreFabrica ms in mostresms)
        {
            //create sms string
            // G:99,99;P:9,99;C:9,99;L:9,99;SUN:99,99;pH:9,99;Pct ing:-0,513;
            //Inhib:N;U:99,99;NTG:10000000;NCS:99999999
            // G-grasime; P-proteina; C-cazeina; L-lactoza; SUN- subst. uscata negrasa; 
            //Pct. ing- punct inghet; 
            //Inhib- reziduuri inhibitori ce poate fi P-pozitiv sau N-negativ; U- uree

            //TODO : uncomment !!!
            if (!string.IsNullOrEmpty(ms.TelContact))
            {
                if (ms.FermeNume.Length > 30)
                    ms.FermeNume = ms.FermeNume.Substring(0, 30);
                StringBuilder sms = new StringBuilder();
                sms.Append(ms.FermeNume + ";");
                sms.Append("G:" + ms.Grasime + ";");
                sms.Append("P:" + ms.Proteina + ";");
                sms.Append("C:" + ms.Casein + ";");
                sms.Append("L:" + ms.Lactoza + ";");
                sms.Append("SUN:" + ms.Substu + ";");
                sms.Append("pH:" + ms.Ph + ";");
                sms.Append("Pct.ing.:" + ms.Puncti + ";");
                sms.Append("Inhib:" + ms.Antib + ";");
                sms.Append("U:" + ms.Urea + ";");
                try
                {
                    sms.Append("NTG:" + ms.NTG + "000;");
                }
                catch
                {
                    sms.Append("NTG:>10000000;");
                }
                try
                {
                    sms.Append("NCS:" + ms.NCS + "000;");
                }
                catch
                {
                    sms.Append("NCS:" + ms.NCS + ";");
                }
                sms.Append("Cod:" + ms.CodBare);

                string cellnumber = Regex.Replace(ms.TelContact, @"[\s()-]", "");
                cellnumber = (cellnumber.StartsWith("0") ? "+4" + cellnumber : cellnumber);

                try
                {
                    bool res = objclsSMS.sendMsg(port, portname, "9600", cellnumber, sms.ToString());
                    if (res)
                    {
                        string strupdate = (countSms > 0) ? " or codbare ='" + ms.CodBare + "' " : " codbare ='" + ms.CodBare + "' ";
                        updateSql.Append(strupdate);
                        countSms++;
                        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + smsname, countSms + ". " + ms.CodFerma + " " + ms.FermeNume + " " + cellnumber + " - " + DateTime.Now.ToString());
                        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + smsname, sms.ToString());
                    }
                    else
                    {
                        countError++;
                        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname, countError + ". " + ms.CodFerma + " " + ms.FermeNume + " " + cellnumber + " - " + DateTime.Now.ToString());
                        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname, sms.ToString());
                    }
                }
                catch (Exception ex)
                {
                    mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname, ex.Message + " - " + ex.StackTrace);
                }
                counter++;
            }
        }
        try
        {
            objclsSMS.ClosePort(port);
        }
        catch { }
        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname, "************" + "End Data: " + DateTime.Now.ToString() + "**********");
        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + smsname, "************" + "End Data: " + DateTime.Now.ToString() + "**********");
        // update moste tancuri set sentsms=1
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);

        try
        {
            if (countSms > 0)
            {
                SqlCommand cmd = new SqlCommand(updateSql.ToString(), cnn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
        catch (Exception exU)
        {
            Label1.Text = " Update MostreTancuri failed! " + exU.Message;
        }


        fissms.NavigateUrl = HttpContext.Current.Request.ApplicationPath + @"/Documents/Sms/" + smsname;
        fissms.Visible = true;


        fislog.NavigateUrl = HttpContext.Current.Request.ApplicationPath + @"/Documents/Sms/" + logname;
        fislog.Visible = true;
    }
}
