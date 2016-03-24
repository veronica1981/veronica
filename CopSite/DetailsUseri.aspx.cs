using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DetailsUseri : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        Response.Redirect("EditareUseri.aspx");
    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        Response.Redirect("EditareUseri.aspx");
    }
}