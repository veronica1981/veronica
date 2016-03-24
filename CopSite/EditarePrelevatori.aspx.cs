using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditarePrelevatori : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
            BindData();

    }

    public void BindData()
    {
        ArrayList values = createTable();
        GridView1.DataSource = values;
        GridView1.DataBind();
    
    }

    public ArrayList createTable()
    {
        ArrayList values = new ArrayList();
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "select * from Prelevatori Where 1=1";
        if (Prelevator.Text.Trim().Length > 0)
            cmd.CommandText += " AND NumePrelevator Like '%" + Prelevator.Text.Trim() + "%'";
        if (CodPrelevator.Text.Trim().Length > 0)
            cmd.CommandText += " AND CodPrelevator =" + CodPrelevator.Text.Trim();

        cmd.CommandText += "ORDER BY CodPrelevator DESC";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            Prelevatori pv = new Prelevatori();
            pv.id = Convert.ToString(reader["ID"]);
          
            pv.codprelevator = Convert.ToString(reader["CodPrelevator"]);
            pv.numeprelevator = Convert.ToString(reader["NumePrelevator"]);
            values.Add(pv);
            // add to values
            nrrec++;

        }
        lcount.Text = nrrec + " prelevatori";
        reader.Close();
        cnn.Close();
        return values;
    }
    protected void GridView1_PageIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;

        BindData(); 

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        
    }
    protected void GridView1_DataBinding(object sender, EventArgs e)
    {
        
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void DetailsView1_DataBinding(object sender, EventArgs e)
    {
      
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ArrayList values = createTable();
        GridView1.DataSource = values;
        GridView1.DataBind();
    
    }
}
