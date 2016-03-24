using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DetailsFermieri : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Judete_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        Response.Redirect("~/EditareFermieri.aspx");
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        /* SqlConnection cnn = new SqlConnection(
  ConfigurationManager.ConnectionStrings
     ["fccl2ConnectionString"].ConnectionString);
         string query = "UPDATE MostreTancuri SET PrelevatorID = " +e.NewValues[2]  +
          " WHERE CodBare = '" + e.NewValues[0]+ "'";
        
         SqlCommand cmd = new SqlCommand(query, cnn);
         cmd.Connection.Open();
         cmd.ExecuteNonQuery();
         cmd.Connection.Close();
         */
    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        Response.Redirect("~/EditareFermieri.aspx");
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        Response.Redirect("~/EditareFermieri.aspx");
    }
}
