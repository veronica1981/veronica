using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditareFerme : Page
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
        cmd.CommandText = "select Ferme_CCL.ID, Ferme_CCL.Nume, Ferme_CCL.Strada, Ferme_CCL.Numar, Ferme_CCL.Oras, Ferme_CCL.Judet, Ferme_CCL.Cod, Ferme_CCL.Telefon, Ferme_CCL.Email, Fabrici.Nume AS NumeFabrica, Judete.Denloc FROM Ferme_CCL, Judete, Fabrici WHERE Ferme_CCL.FabricaID = Fabrici.ID  AND Convert(int,Ferme_CCL.Judet,2)=Judete.ID  ";

        if (Fabrica.Text.Trim().Length > 0)
            cmd.CommandText += " AND Fabrici.Nume Like '%" + Fabrica.Text.Trim() + "%'";
        if (Ferma.Text.Trim().Length > 0)
            cmd.CommandText += " AND Ferme_CCL.Nume Like '%" + Ferma.Text.Trim() + "%'";
        if (Cod.Text.Trim().Length > 0)
            cmd.CommandText += " AND Ferme_CCL.Cod Like '%" + Cod.Text.Trim() + "%'";
        cmd.CommandText += " ORDER BY Ferme_CCL.Nume";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            Fabrica fm = new Fabrica();

            fm.id = Convert.ToString(reader["ID"]);
            fm.cod = Convert.ToString(reader["Cod"]);
            fm.fabricaid = Convert.ToString(reader["NumeFabrica"]);
            fm.nume = Convert.ToString(reader["Nume"]);
            fm.strada = Convert.ToString(reader["Strada"]);
            fm.numar = Convert.ToString(reader["Numar"]);
            fm.oras = Convert.ToString(reader["Oras"]);
            fm.judet = Convert.ToString(reader["Denloc"]);
            string emails = Convert.ToString(reader["Email"]);
            string[] recipients = emails.Split(new Char[] { ';' });
            fm.email = "";
            foreach (string recipient in recipients)
                fm.email += recipient + " ";
            fm.telefon = Convert.ToString(reader["Telefon"]);
            values.Add(fm);
            // add to values
            nrrec++;
        }
        lcount.Text = nrrec + " ferme";
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
    protected void Fabrica_TextChanged(object sender, EventArgs e)
    {
        if (Fabrica.Text.Trim().Length > 0)
        {
            Ferma.Text = "";
            Ferma.Enabled = false;
            Cod.Text = "";
            Cod.Enabled = false;

        }
        else
        {
            Ferma.Enabled = true;
            Cod.Enabled = true;
        }
    }
    protected void Ferma_TextChanged(object sender, EventArgs e)
    {
        if (Ferma.Text.Trim().Length > 0)
        {
            Fabrica.Text = "";
            Fabrica.Enabled = false;
            Cod.Text = "";
            Cod.Enabled = false;
        }
        else
        {
            Fabrica.Enabled = true;
            Cod.Enabled = true;
        }

    }
    protected void Cod_TextChanged(object sender, EventArgs e)
    {
        if (Cod.Text.Trim().Length > 0)
        {
            Ferma.Text = "";
            Ferma.Enabled = false;
            Fabrica.Text = "";
            Fabrica.Enabled = false;
        }
        else
        {
            Fabrica.Enabled = true;
            Ferma.Enabled = true;
        }
    }
}
