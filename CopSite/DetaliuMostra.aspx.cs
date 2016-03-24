using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using CopSite.Old_App_Code;

public partial class DetaliuMostra : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["id"] != null)
            FormView1.DefaultMode = FormViewMode.Edit;
        else
        {
            FormView1.DefaultMode = FormViewMode.Insert;
            TextBox txtprel = (TextBox)(FormView1.FindControl("DataPrelevareTextBox"));
            if (txtprel.Text.Trim() == "")
                txtprel.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            TextBox txtprim = (TextBox)(FormView1.FindControl("DataPrimiriiTextBox"));
            if (txtprim.Text.Trim()=="")
                txtprim.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            TextBox txttest = (TextBox)(FormView1.FindControl("DataTestareTextBox"));
            if (txttest.Text.Trim() == "")
                txttest.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            TextBox txttestfin = (TextBox)(FormView1.FindControl("DataTestareFinalaTextBox"));
            if (txttestfin.Text.Trim() == "")
                txttestfin.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            TextBox txtidz = (TextBox)(FormView1.FindControl("IdZilnicTextBox"));
            if (txtidz.Text.Trim() == "")
            {
                txtidz.Text = "" + GetMaxIdZilnic(DateTime.Now);
            }
            TextBox txtprelid = (TextBox)(FormView1.FindControl("PrelevatorIDTextBox"));
            if (txtprelid.Text.Trim() == "")
                txtprelid.Text = "0";

        }
    }
    protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        var mostra = new MostreFabrica();

        mostra.CodBare = NullifyEmptyValues(e.NewValues["CodBare"]); 
        mostra.PrelevatoriId = NullifyEmptyValues(e.NewValues["PrelevatoriId"]);
        mostra.Rasa = NullifyEmptyValues(e.NewValues["Rasa"]);
        mostra.IdZilnic = NullifyEmptyValues(e.NewValues["IdZilnic"]);
        DateTime datatestare;
        DateTime datatestarefin;
        DateTime dataprimirii;
        DateTime dataprelevare;

        if (!DateTime.TryParse(e.NewValues["DataTestare"].ToString().Trim(), out datatestare))
        {
            Label1.Text = "Data testarii invalida!";
            e.Cancel = true;
            return;
        }
        mostra.DataTestare = datatestare;

        if (!DateTime.TryParse(e.NewValues["DataTestareFinala"].ToString().Trim(), out datatestarefin))
        {
            Label1.Text = "Data testarii finale invalida!";
            e.Cancel = true;
            return;
        }
        mostra.DataTestareFinala = datatestarefin;

        if (!DateTime.TryParse(e.NewValues["DataPrimirii"].ToString().Trim(), out dataprimirii))
        {
            Label1.Text = "Data primirii invalida!";
            e.Cancel = true;
            return;
        }
        mostra.DataPrimirii = dataprimirii;

        if (!DateTime.TryParse(e.NewValues["DataPrelevare"].ToString().Trim(), out dataprelevare))
        {
            Label1.Text = "Data prelevare invalida!";
            e.Cancel = true;
            return;
        }
        mostra.DataPrelevare = dataprelevare;

        // verific cod ferma
        if (mostra.PrelevatoriId != "0")
        {
            cmd.CommandText = "SELECT * from FERME_CCL WHERE Cod ='" + mostra.CodBare.Substring(0, 5) + "'";
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                Label1.Text = "Codul de ferma " + mostra.CodBare.Substring(0, 5) + " nu exista!";
                e.Cancel = true;
                reader.Close();
                cnn.Close();
                return;

            }
            else
            {
                Label1.Text = "";
                reader.Close();
                cnn.Close();
            }

        }
        // verific id zilnic        
        if (mostra.IdZilnic != "")
        {
            cmd.CommandText = "SELECT * from MostreTancuri Where IdZilnic =" + mostra.IdZilnic + " AND DataTestare = CONVERT(date, '" + mostra.DataTestare.ToShortDateString() + "', 103)";
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int count=0;
            while (reader.Read())
                count++;
            if (count >1)
            {
                reader.Close();
                //get max id
                cmd.CommandText = "SELECT MAX(IdZilnic) AS MaxId from MostreTancuri Where DataTestare = CONVERT(date, '" + mostra.DataTestare.ToShortDateString() + "', 103)";
                reader = cmd.ExecuteReader();
                int idzilnic = 1;
                if (reader.Read())
                    if (!reader.IsDBNull(0))
                       idzilnic = Convert.ToInt32(reader["MaxId"]) + 1;
                e.NewValues["IdZilnic"] = Convert.ToString(idzilnic);
                reader.Close();
                cnn.Close();
            }
        }
        else
        {
            cmd.CommandText = "SELECT MAX(IdZilnic) AS MaxId from MostreTancuri Where DataTestare=CONVERT(date, '" + mostra.DataTestare.ToShortDateString() + "', 103)";
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int idzilnic = 1;
            if (reader.Read())
                if (!reader.IsDBNull(0))
              idzilnic = Convert.ToInt32(reader["MaxId"]) + 1;
            e.NewValues["IdZilnic"] = Convert.ToString(idzilnic);
            reader.Close();
            cnn.Close();

        }
        //verific fcb

        mostra.NrComanda = NullifyEmptyValues(e.NewValues["NrComanda"]);
        mostra.NumePrelevator = NullifyEmptyValues(e.NewValues["PrelevatoriNume"]);
        mostra.FabriciNume = NullifyEmptyValues(e.NewValues["FabriciNume"]);
        mostra.FabriciStrada = NullifyEmptyValues(e.NewValues["FabriciStrada"]);
        mostra.FabriciOras = NullifyEmptyValues(e.NewValues["FabriciOras"]);
        mostra.FabriciJudet = NullifyEmptyValues(e.NewValues["FabriciJudet"]);
        mostra.NumeProba = NullifyEmptyValues(e.NewValues["NumeProba"]);
        if (mostra.PrelevatoriId != "0" && (mostra.NumePrelevator != "" || mostra.FabriciNume != ""
            || mostra.FabriciStrada != "" || mostra.FabriciOras != "" || mostra.FabriciJudet != "" || mostra.NumeProba != ""))
        {
            Label1.Text = "Au fost completate simultan date pt. FCB si Cod Bare!";
            e.Cancel = true;
            return;
        }
        else
            Label1.Text = "";
        if ((mostra.PrelevatoriId == null || mostra.PrelevatoriId.Equals("0")) && (mostra.NumePrelevator == null || mostra.NumePrelevator.Equals("")))
        {
            Label1.Text = "Nu a fost completat prelevatorul!";
            e.Cancel = true;
            return;
        }
        Label1.Text = "";

        mostra.Grasime = NullifyEmptyValues(e.NewValues["Grasime"]);
        mostra.Proteina = NullifyEmptyValues(e.NewValues["Proteina"]);
        mostra.Lactoza = NullifyEmptyValues(e.NewValues["Lactoza"]);
        mostra.Casein = NullifyEmptyValues(e.NewValues["Casein"]);
        mostra.Substu = NullifyEmptyValues(e.NewValues["Substu"]);
        mostra.NCS = NullifyEmptyValues(e.NewValues["NCS"]);
        mostra.Urea = NullifyEmptyValues(e.NewValues["Urea"]);
        mostra.Ph = NullifyEmptyValues(e.NewValues["Ph"]);
        mostra.Id = NullifyEmptyValues(e.NewValues["Id"]);
        mostra.Definitiv = e.NewValues["Definitiv"].ToString().Trim() == "True";
        mostra.Validat = e.NewValues["Validat"].ToString().Trim() == "True";
        mostra.Sentsms = false;

        mostra.UpdateMostra(mostra);
    }

    private string NullifyEmptyValues(object value)
    {
        if (value == null) return null;
        string text = value.ToString().Trim();
        if (text == string.Empty)
        {
            return null;
        }
        return text;
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        Response.Redirect("~/EditareMostre.aspx");
    }
    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        Response.Redirect("~/EditareMostre.aspx");
    }
    protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        string codbare = e.Values["CodBare"].ToString().Trim(); ;
        string prelevatorid = e.Values["PrelevatoriId"].ToString().Trim();
        string strid = e.Values["IdZilnic"].ToString().Trim();
        Label1.Text = "";
        DateTime datatestare;
        DateTime datatestarefin;
        DateTime dataprimirii;
        DateTime dataprelevare;

        if (!DateTime.TryParse(e.Values["DataTestare"].ToString().Trim(), out datatestare))
        {
            Label1.Text = "Data testarii invalida!";
            e.Cancel = true;
            return;
        }

        if (!DateTime.TryParse(e.Values["DataTestareFinala"].ToString().Trim(), out datatestarefin))
        {
            Label1.Text = "Data testarii finale invalida!";
            e.Cancel = true;
            return;
        }

        if (!DateTime.TryParse(e.Values["DataPrimirii"].ToString().Trim(), out dataprimirii))
        {
            Label1.Text = "Data primirii invalida!";
            e.Cancel = true;
            return;
        }

        if (!DateTime.TryParse(e.Values["DataPrelevare"].ToString().Trim(), out dataprelevare))
        {
            Label1.Text = "Data prelevare invalida!";
            e.Cancel = true;
            return;
        }

        cmd.CommandText = "SELECT * from MostreTancuri WHERE CodBare ='"
        + codbare + "'";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Label1.Text = "Codul " + codbare + " exista deja!";
            e.Cancel = true;
            reader.Close();
            cnn.Close();
            return;

        }
        else
        {
            Label1.Text = "";
            reader.Close();
            cnn.Close();
        }
        // verific cod ferma
        if (prelevatorid != "0")
        {
            cmd.CommandText = "SELECT * from FERME_CCL WHERE Cod ='"
      + codbare.Substring(0,5) + "'";
            cnn.Open();
            reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                Label1.Text = "Codul de ferma " + codbare.Substring(0,5) + " nu exista!";
                e.Cancel = true;
                reader.Close();
                cnn.Close();
                return;

            }
            else
            {
                Label1.Text = "";
                reader.Close();
                cnn.Close();
            }

        }
        // verific id zilnic        
        if (strid != "")
        {
            cmd.CommandText = "SELECT * from MostreTancuri Where IdZilnic =" + strid + " AND DataTestare = CONVERT(date, '" + datatestare.ToShortDateString() + "', 103)";
            cnn.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                //get max id
                cmd.CommandText = "SELECT MAX(IdZilnic) AS MaxId from MostreTancuri Where DataTestare = CONVERT(date, '" + datatestare.ToShortDateString() + "', 103)";
                reader = cmd.ExecuteReader();
                int idzilnic=1;
                if (reader.Read())
                    if (!reader.IsDBNull(0))
                    idzilnic = Convert.ToInt32(reader["MaxId"]) + 1;
                e.Values["IdZilnic"] = Convert.ToString(idzilnic);
                reader.Close();
                cnn.Close();
            }
        }
        else
        {
            cmd.CommandText = "SELECT MAX(IdZilnic) AS MaxId from MostreTancuri Where DataTestare = CONVERT(date, '" + datatestare.ToShortDateString() + "', 103)";
            cnn.Open();
            reader = cmd.ExecuteReader();
            int idzilnic = 1;
            if (reader.Read())
                 if (!reader.IsDBNull(0))
                     idzilnic = Convert.ToInt32(reader["MaxId"])+1;
          
            e.Values["IdZilnic"] = Convert.ToString(idzilnic);
            reader.Close();
            cnn.Close();

        }
        //verific fcb
        string numeprelevator = e.Values["PrelevatoriNume"].ToString().Trim();
        string numeclient = e.Values["FabriciNume"].ToString().Trim();
        string adresaclient = e.Values["FabriciStrada"].ToString().Trim();
        string locclient = e.Values["FabriciOras"].ToString().Trim();
        string judclient = e.Values["FabriciJudet"].ToString().Trim();
        string numeproba = e.Values["NumeProba"].ToString().Trim();
        if (prelevatorid != "0" && (numeprelevator != "" || numeclient != ""
            || adresaclient != "" || locclient != "" || judclient != "" || numeproba != ""))
        {
            Label1.Text = "Au fost completate simultan date pt. FCB si Cod Bare!";
            e.Cancel = true;
            return;
        }
        else
            Label1.Text = "";
        if (prelevatorid.Equals("0") && numeprelevator.Equals(""))
        {
            Label1.Text = "Nu a fost completat prelevatorul!";
            e.Cancel = true;
            return;
        }
        Label1.Text = "";

    }
    protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        Response.Redirect("~/EditareMostre.aspx");
    }
    
    protected void Date_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string date = args.Value.Trim();
        string[] arr = date.Split('/');
        if (arr.Length < 3 || date.Equals(""))
            args.IsValid = false;
        else
        {
            int iDay = int.Parse(arr[0]);
            int iMonth = int.Parse(arr[1]);
            int iYear = int.Parse(arr[2]);

            if ((iDay > 31) || (iMonth > 12) ||
                (iYear < 1900 || iYear > DateTime.Now.Year))
                args.IsValid = false;
            try
            {
                DateTime dummyDate = new DateTime(iYear, iMonth, iDay);
                if ((dummyDate.Day != iDay) ||
                (dummyDate.Month != iMonth) ||
                (dummyDate.Year != iYear))
                {
                    args.IsValid = false;
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }
    }

    public int GetMaxIdZilnic(DateTime datatestare)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        int idzilnic = 1;
        cmd.CommandText = "SELECT MAX(IdZilnic) AS MaxId from MostreTancuri Where DataTestare=CONVERT(date, '" + datatestare.ToShortDateString() + "', 103)";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
            if (!reader.IsDBNull(0))
                idzilnic = Convert.ToInt32(reader["MaxId"]) + 1;
        return idzilnic;
    }

    protected void FormView1_OnDataBound(object sender, EventArgs e)
    {
        var ureaText = Utils.NormalizeForDisplay(((TextBox) FormView1.FindControl("UreaTextBox")).Text, "0");
        if (Utils.IsDouble(ureaText))
        {
            if (Utils.ConvertToDouble(ureaText) < 0.1)
            {
                ((TextBox) FormView1.FindControl("UreaTextBox")).Text = "";
            }
        }
    }
}
