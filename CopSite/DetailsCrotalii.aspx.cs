using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DetailsCrotalii : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["Crotalia"] != null)
            DetailsView1.DefaultMode = DetailsViewMode.Edit;
        else
            DetailsView1.DefaultMode = DetailsViewMode.Insert;

    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        Response.Redirect("~/EditareCrotalii.aspx");
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        string crot = e.NewValues["Crotalia"].ToString();
        string oldcrot = e.Keys["Crotalia"].ToString();
        e.Keys["Crotalia"] = crot;
        OldCrot.Text = oldcrot;
        
        if (crot.Trim().Length == 0)
        {
            Label2.Text = "Crotalia este camp obligatoriu !";
            e.Cancel = true;
            return;
        }
		if (oldcrot != crot) {
        cmd.CommandText = "SELECT * from Crotalii WHERE Crotalia ='"
        + crot + "'";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Label2.Text = "Crotalia " + crot + " exista deja!";
            e.Cancel = true;
        }
        else
            Label2.Text = "";
		
        reader.Close();
        cnn.Close();
        }
		else
		Label2.Text ="";

      
    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        Response.Redirect("~/EditareCrotalii.aspx");
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        string crot = e.Values["Crotalia"].ToString();
        if (crot.Trim().Length == 0)
        {
            Label2.Text = "Crotalia este camp obligatoriu !";
            e.Cancel = true;
            return;
        }

        cmd.CommandText = "SELECT * from Crotalii WHERE Crotalia ='"
        + crot + "'";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Label2.Text = "Crotalia " + crot + " exista deja!";
            e.Cancel = true;
        }
        else
            Label2.Text = "";

        reader.Close();
        cnn.Close();
        
    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        Response.Redirect("~/EditareCrotalii.aspx");
    }
}
