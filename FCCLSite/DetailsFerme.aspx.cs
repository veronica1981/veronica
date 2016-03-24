using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DetailsFerme : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["ID"] != null)
            DetailsView1.DefaultMode = DetailsViewMode.Edit;
        else
            DetailsView1.DefaultMode = DetailsViewMode.Insert;

    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        Response.Redirect("~/EditareFerme.aspx");
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {

    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        Response.Redirect("~/EditareFerme.aspx");
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        string cod = e.Values["Cod"].ToString();


        cmd.CommandText = "SELECT * from Ferme_CCL WHERE Cod ='"
        + cod + "'";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Label2.Text = "Codul " + cod + " exista deja!";
            e.Cancel = true;
        }
        else
            Label2.Text = "";

        reader.Close();
        cnn.Close();

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        Response.Redirect("~/EditareFerme.aspx");
    }
}
