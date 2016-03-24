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
/// Summary description for FileLinks
/// </summary>
public class FileLinks
{
	public FileLinks()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string strUrl;
    public string strDes;

    public string Url
    {
        get
        {
            return strUrl;
        }
        set
        {
            strUrl = value;
        }
    }

    public string Des
    {
        get
        {
            return strDes;
        }
        set
        {
            strDes = value;
        }
    }
}
