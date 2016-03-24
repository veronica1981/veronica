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
/// Summary description for Buletin
/// </summary>
public class Buletin
{
	public Buletin()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int iNrb;
    public string strDatab;

    public int Nrb
    {
        get
        {
            return iNrb;
        }
        set
        {
            iNrb = value;
        }
    }

    public string Datab
    {
        get
        {
            return strDatab;
        }
        set
        {
            strDatab = value;
        }
    }
}
