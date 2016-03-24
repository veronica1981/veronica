using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Prelevatori
/// </summary>
public class Prelevatori
{
	public Prelevatori()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string strid;
    public string strcodprelevator;
    public string strnumeprelevator;

    public string id
    {
        get
        {
            return strid;
        }
        set
        {
            strid = value;
        }
    }

    public string codprelevator
    {
        get
        {
            return strcodprelevator;
        }
        set
        {
            strcodprelevator = value;
        }
    }

    public string numeprelevator
    {
        get
        {
            return strnumeprelevator;
        }
        set
        {
            strnumeprelevator = value;
        }
    }
}
