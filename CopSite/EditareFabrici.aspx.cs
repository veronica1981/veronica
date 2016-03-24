using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditareFabrici : Page
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

        SqlConnection cnn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "select Fabrici.ID, Fabrici.Nume,Fabrici.Strada,Fabrici.Numar,Fabrici.Oras, Fabrici.Email, Fabrici.Telefon,Fabrici.PersonaDeContact, Judete.Denloc from Fabrici,Judete WHERE (Fabrici.Judet = Judete.ID) ";
        if (Fabrica.Text.Trim().Length > 0)
            cmd.CommandText += " AND Fabrici.Nume Like '%" + Fabrica.Text.Trim() + "%'";
        cmd.CommandText +=" ORDER BY Fabrici.Nume";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            Fabrica fm = new Fabrica();

            fm.id = Convert.ToString(reader["ID"]);
            fm.nume = Convert.ToString(reader["Nume"]);
            fm.strada = Convert.ToString(reader["Strada"]);
            fm.numar = Convert.ToString(reader["Numar"]);
            fm.oras = Convert.ToString(reader["Oras"]);
            fm.judet = Convert.ToString(reader["Denloc"]);
            string emails =Convert.ToString(reader["Email"]);
            string[] recipients =emails.Split(new Char[] { ';' });
            fm.email ="";
            foreach (string recipient in recipients)
                fm.email +=recipient+" ";

            fm.telefon = Convert.ToString(reader["Telefon"]);
            
            values.Add(fm);
            // add to values
            nrrec++;

        }
        lcount.Text = nrrec + " asociatii";
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
        BindData();
    }
}
