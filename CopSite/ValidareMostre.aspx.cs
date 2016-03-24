using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ValidareMostre : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (datatestare.Text.Trim() == "")
            datatestare.Text = (DateTime.Now).ToString("dd/MM/yyyy");
        bsave.Enabled = false;
        babort.Enabled = false;
        Button1.Visible = false;
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string date = args.Value;
        string[] arr = date.Split('/');
        if (arr.Length < 3)
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

    public ArrayList createTable(DateTime tdata)
    {
        ArrayList values = new ArrayList();

        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "select * from MostreTancuri where DataTestare = convert(date, '"+tdata.ToShortDateString()+"', 103) ";
        if (prelid.Text.Trim().Length > 0)
        {
            cmd.CommandText += " AND PrelevatorId =" + prelid.Text.Trim();
        }
        if (codbare.Text.Trim().Length > 0)
        {
            cmd.CommandText += " AND CodBare Like  '%" + codbare.Text.Trim() + "%'";
        }

        cmd.CommandText += " ORDER BY IdZilnic ASC";
        //SqlParameter p = new SqlParameter("@DataTestareFinala", tdata.ToShortDateString());
        //cmd.Parameters.Add(p);
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            VMostre vm = new VMostre();
            vm.Id = Convert.ToString(reader["ID"]);
            vm.codbare = Convert.ToString(reader["CodBare"]);
            vm.idzilnic = Convert.ToString(reader["IdZilnic"]);
            vm.datat = Convert.ToDateTime(reader["DataTestare"]);

            vm.casein = Convert.ToString(reader["Caseina"]);
            vm.grasime = Convert.ToString(reader["Grasime"]);
            vm.proteina = Convert.ToString(reader["ProcentProteine"]);
            vm.lactoza = Convert.ToString(reader["ProcentLactoza"]);
            vm.substu = Convert.ToString(reader["SubstantaUscata"]);
            vm.pcti = Convert.ToString(reader["PunctInghet"]);
            vm.apaad = "";
            vm.antib = Convert.ToString(reader["Antibiotice"]);
            vm.ncs = Convert.ToString(reader["NumarCeluleSomatice"]);
            vm.ntg = Convert.ToString(reader["IncarcaturaGermeni"]);
            vm.urea = Convert.ToString(reader["Urea"]);
            vm.ph = Convert.ToString(reader["PH"]);
            vm.rasa = Convert.ToString(reader["Rasa"]);
            vm.validat = Convert.ToString(reader["Validat"]);
            vm.definitiv = Convert.ToString(reader["Definitiv"]);
            vm.sentsms = Convert.ToBoolean(reader["SentSms"]);
            vm.dataf = Convert.ToDateTime(reader["DataTestareFinala"]);
            vm.prelid = Convert.ToString(reader["PrelevatorId"]);
            values.Add(vm);
            // add to values
            nrrec++;

        }
        lcount.Text = nrrec + " mostre";
        reader.Close();
        cnn.Close();
        return values;
    }

    private string NomalizeInput(string input)
    {
        return input.Trim().Equals("") ? "0" : input.Trim().Equals("0") ? "0.00001" : Convert.ToDouble(input.Trim()).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
    }

    void updateFlags()
    {
        ArrayList values = createTable(DateTime.Parse(datatestare.Text));
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        string preleronati = "";
        for (int i = 0; i < Repeater1.Items.Count; i++)
        {
            RepeaterItem item = Repeater1.Items[i];
            VMostre vm = (VMostre)values[i];
            Label lbcod = (Label)item.FindControl("codbare");
            string codbare = lbcod.Text;
            Label lbdata = (Label)item.FindControl("datat");
            string tdata = lbdata.Text;

            CheckBox chkdef = (CheckBox)item.FindControl("definitiv");
            CheckBox chkval = (CheckBox)item.FindControl("validat");

            int validat = 0;
            if (chkval != null && chkval.Checked)
                validat = 1;
            int definitiv = 0;
            if (chkdef.Checked)
                definitiv = 1;
            int sentsms = 0;
            if (!Page.User.IsInRole("admin"))
            {
                if (definitiv == 1)
                    validat = 1;
            }
            else
            {
                if (validat == 0)
                    definitiv = 0;
            }
            string grasime = NomalizeInput(((TextBox)item.FindControl("grasime")).Text);
            string proteina = NomalizeInput(((TextBox)item.FindControl("proteina")).Text);
            string caseina = NomalizeInput(((TextBox)item.FindControl("caseina")).Text);
            string lactoza = NomalizeInput(((TextBox)item.FindControl("lactoza")).Text);
            string substu = NomalizeInput(((TextBox)item.FindControl("substu")).Text);
            string pcti = "0.00001";
            string antib = "";
            string ncs = NomalizeInput(((TextBox)item.FindControl("ncs")).Text);
            string ph = NomalizeInput(((TextBox)item.FindControl("ph")).Text);
            string urea = NomalizeInput(((TextBox)item.FindControl("urea")).Text);
            string ntg = "0.00001";

            TextBox tprelid = (TextBox)item.FindControl("prelid");
            string prelid = tprelid.Text;
            if (prelid.Trim().Equals(""))
                prelid = "0";
            if (prelid != "0")
            {
                if (!prelid.Trim().Equals(codbare.Substring(0, 5)))
                {
                    if (!ExistaPrelevator(prelid))
                        if (preleronati.Length > 0)
                            preleronati += "," + prelid;
                        else
                            preleronati += prelid;
                }
            }
            //update table
            //if changed update
            {

                string query = "UPDATE MostreTancuri SET Validat = " + validat + ",Definitiv = " + definitiv + ",Sentsms = " + sentsms +
                ",Grasime= " + grasime + ",ProcentProteine=" + proteina + ",Caseina=" + caseina + ",ProcentLactoza=" + lactoza + ",PunctInghet=" + pcti +
                ",SubstantaUscata=" + substu + ",NumarCeluleSomatice=" + ncs + ",IncarcaturaGermeni=" + ntg +
                ",PH=" + ph + ",Urea=" + urea +
                ",Antibiotice='" + antib + "',PrelevatorId=" + prelid +
                " WHERE ID = " + vm.Id;
                SqlCommand cmd = new SqlCommand(query, cnn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }

        }
        if (preleronati.Length > 0)
            Label4.Text = "Prelevatorii: " + preleronati + " nu exista!";
    }

    protected void Edit_Click(object sender, EventArgs e)
    {
        bsave.Enabled = true;
        babort.Enabled = true;
        bedit.Enabled = false;
        Button1.Visible = true;

        ArrayList values = createTable(DateTime.Parse(datatestare.Text));

        Repeater1.DataSource = values;
        Repeater1.DataBind();
        top.Visible = true;

    }
    protected void Save_Click(object sender, EventArgs e)
    {
        updateFlags();
        bedit.Enabled = true;
        bsave.Enabled = false;
        babort.Enabled = false;
        top.Visible = false;

    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        ArrayList values = createTable(DateTime.Parse(datatestare.Text));

        Repeater1.DataSource = values;
        Repeater1.DataBind();
        top.Visible = true;
        bedit.Enabled = true;
        babort.Enabled = false;
        bsave.Enabled = false;

    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        bsave.Enabled = true;
        babort.Enabled = true;
        foreach (RepeaterItem item in Repeater1.Items)
        {


            CheckBox chkdef = (CheckBox)item.FindControl("definitiv");
            CheckBox chkval = (CheckBox)item.FindControl("validat");

            chkdef.Checked = true;
            chkval.Checked = true;


        }
    }

    public bool ExistaPrelevator(string prelid)
    {
        bool exista = true;
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT * from Prelevatori "
        + "WHERE CodPrelevator=" + prelid;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read())
            exista = false;
        reader.Close();
        cnn.Close();
        return exista;

    }
}
