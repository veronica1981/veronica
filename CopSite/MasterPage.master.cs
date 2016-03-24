using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
        Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
    }
    protected void Menu2_MenuItemClick(object sender, MenuEventArgs e)
    {

    }
    protected void Menu3_MenuItemClick(object sender, MenuEventArgs e)
    {

    }

  
}
