using System;
using System.Web.UI;

public partial class CreateUser : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void linkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditareUseri.aspx");
    }
}