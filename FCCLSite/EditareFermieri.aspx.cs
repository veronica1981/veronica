using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditareFermieri : Page
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
        cmd.CommandText = "select Fermier.FermierID, Fermier.Nume, Fermier.Strada, Fermier.Nr, Fermier.Oras, Fermier.Judet,  Fermier.Telefon, Fermier.Email, Judete.Denloc FROM Fermier, Judete  WHERE Fermier.Judet=Judete.ID ";
        if (Fabrica.Text.Trim().Length > 0)
            cmd.CommandText += " AND Fermier.Nume Like '%" + Fabrica.Text.Trim() + "%'";
        cmd.CommandText += " ORDER BY Fermier.Nume";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            Fabrica fm = new Fabrica();

            fm.id = Convert.ToString(reader["FermierID"]);
            fm.nume = Convert.ToString(reader["Nume"]);
            fm.strada = Convert.ToString(reader["Strada"]);
            fm.numar = Convert.ToString(reader["Nr"]);
            fm.oras = Convert.ToString(reader["Oras"]);
            fm.judet = Convert.ToString(reader["Denloc"]);
            fm.email = Convert.ToString(reader["Email"]);
            fm.telefon = Convert.ToString(reader["Telefon"]);
            values.Add(fm);
            // add to values
            nrrec++;
        }
        lcount.Text = nrrec + " fermieri";
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
