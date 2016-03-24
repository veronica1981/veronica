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
        DateTime datatestare = DateTime.Parse(DataSelectie.Text);
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
           // text sms
            //TODO : uncomment !!!
            if (!string.IsNullOrEmpty(ms.TelContact))
            {
                if (ms.FermeNume.Length > 30)
                    ms.FermeNume = ms.FermeNume.Substring(0, 30);
                //  if (string.IsNullOrEmpty(ms.TelContact))
              //  ms.TelContact = "+40722217495";
                StringBuilder sms = new StringBuilder();
         //       sms.Append(ms.FermeNume + " analizele sunt incarcate pe site");
                sms.Append("In " + datatestare + " contul dvs. " + ms.FermeNume + " a fost actualizat cu rezultatele analizelor"); 
                string cellnumber = Regex.Replace(ms.TelContact, @"[\s()-]", "");
                cellnumber = (cellnumber.StartsWith("0") ? "+4"+cellnumber : cellnumber);

                try
                {
                    //todo uncomment
                    bool res = objclsSMS.sendMsg(port, portname, "9600", cellnumber, sms.ToString());
                  //  bool res = true;
                    if (res == true)
                    {
                        string cond = "(FermaID = " + ms.FermeId + " AND DataTestare = CONVERT(date, ''" + datatestare.ToShortDateString() + "', 103))";
                        string strupdate = (countSms > 0) ? " or "+ cond : cond;
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
                      mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname,ex.Message+ " - " + ex.StackTrace);
                }
                counter++;
            }
    
        }
        try
        {
            objclsSMS.ClosePort(port);
        }
        catch { }
        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + logname, "************" + "End Data: " + DateTime.Now + "**********");
        mostre.Addtext(adresafizicaserver + @"\Documents\Sms\" + smsname, "************" + "End Data: " + DateTime.Now + "**********");
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
      
   	    fislog.NavigateUrl  = HttpContext.Current.Request.ApplicationPath + @"/Documents/Sms/"+logname;
        fislog.Visible = true;
    }

 }
