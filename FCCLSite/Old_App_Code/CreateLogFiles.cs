using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;


/// <summary>
/// Summary description for CreateLogFiles
/// </summary>
public class CreateLogFiles
{

   
    private string sErrorTime;

	public CreateLogFiles()
	{

        //sLogFormat used to create log files format :
        // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
      //  sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

        //this variable used to create log filename format "
        //for example filename : ErrorLogYYYYMMDD
        string sYear = DateTime.Now.Year.ToString();
        string sMonth = DateTime.Now.Month.ToString();
        string sDay = DateTime.Now.Day.ToString();
        sErrorTime = sYear + sMonth + sDay;
		//
		// TODO: Add constructor logic here
		//
	}

    public void ErrorLog(string sPathName, string sErrMsg)
    {

        StreamWriter sw = new StreamWriter(sPathName + sErrorTime+".txt", true);
        string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

        sw.WriteLine(sLogFormat + sErrMsg);
        sw.Flush();
        sw.Close();
    }
}
