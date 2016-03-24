using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
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

    public ArrayList createTable(string tdata)
    {
        ArrayList values = new ArrayList();

        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "select * from MostreTancuri where DataTestare like @DataTestareFinala ";
        if (prelid.Text.Trim().Length > 0)
        {
            cmd.CommandText += " AND PrelevatorId =" + prelid.Text.Trim();
        }
        if (codbare.Text.Trim().Length > 0)
        {
            cmd.CommandText += " AND CodBare Like  '%" + codbare.Text.Trim() + "%'";
        }
        cmd.CommandText += " ORDER BY IdZilnic ASC";

        SqlParameter p = new SqlParameter("@DataTestareFinala", tdata);
        cmd.Parameters.Add(p);
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            VMostre vm = new VMostre();
            vm.id = Convert.ToString(reader["ID"]);
            vm.codbare = Convert.ToString(reader["CodBare"]);
            vm.idzilnic = Convert.ToString(reader["IdZilnic"]);
            vm.datat = Convert.ToString(reader["DataTestare"]);
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
            vm.validat = Convert.ToString(reader["Validat"]);
            vm.definitiv = Convert.ToString(reader["Definitiv"]);
            vm.sentsms = Convert.ToBoolean(reader["SentSms"]);
            vm.dataf = Convert.ToString(reader["DataTestareFinala"]);
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

    void updateFlags()
    {
        ArrayList values = createTable(datatestare.Text);
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        string preleronati = "";
        for (int i = 0; i < Repeater1.Items.Count; i++)
        {
            RepeaterItem item = Repeater1.Items[i];
            VMostre vm = (VMostre)values[i];
            Label lbcod = (Label)item.FindControl("codbare");
            string codbare = lbcod.Text;

            CheckBox chkdef = (CheckBox)item.FindControl("definitiv");
            CheckBox chkval = (CheckBox)item.FindControl("validat");
            CheckBox chksms = (CheckBox)item.FindControl("sentsms");

            int validat = 0;
            if (chkval != null && chkval.Checked)
                validat = 1;
            int definitiv = 0;
            if (chkdef.Checked)
                definitiv = 1;
            int sentsms = 0;
            if (chksms.Checked)
                sentsms = 1;
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
            TextBox tgrasime = (TextBox)item.FindControl("grasime");
            string grasime = tgrasime.Text.Trim().Equals("") ? "0" : tgrasime.Text.Trim().Equals("0") ? "0.00001" : tgrasime.Text.Trim();
            TextBox tproteina = (TextBox)item.FindControl("proteina");
            string proteina = tproteina.Text.Trim().Equals("") ? "0" : tproteina.Text.Trim().Equals("0") ? "0.00001" : tproteina.Text.Trim();
            TextBox tcaseina = (TextBox)item.FindControl("caseina");
            string caseina = tcaseina.Text.Trim().Equals("") ? "0" : tcaseina.Text.Trim().Equals("0") ? "0.00001" : tcaseina.Text.Trim();
            TextBox tlactoza = (TextBox)item.FindControl("lactoza");
            string lactoza = tlactoza.Text.Trim().Equals("") ? "0" : tlactoza.Text.Trim().Equals("0") ? "0.00001" : tlactoza.Text.Trim();
            TextBox tsubstu = (TextBox)item.FindControl("substu");
            string substu = tsubstu.Text.Trim().Equals("") ? "0" : tsubstu.Text.Trim().Equals("0") ? "0.00001" : tsubstu.Text.Trim();
            TextBox tpcti = (TextBox)item.FindControl("pcti");
            string pcti = tpcti.Text.Trim().Equals("") ? "0" : tpcti.Text.Trim().Equals("0") ? "0.00001" : tpcti.Text.Trim();
            if (pcti.Length > 0 && (!pcti.Equals("0.00001")) && pcti.IndexOf('.') > 0)
                pcti = pcti.Substring(pcti.IndexOf('.') + 1);
            TextBox tantib = (TextBox)item.FindControl("antib");
            string antib = tantib.Text;
            TextBox tncs = (TextBox)item.FindControl("ncs");
            string ncs = tncs.Text.Trim().Equals("") ? "0" : tncs.Text.Trim().Equals("0") ? "0.00001" : tncs.Text.Trim();
            TextBox tntg = (TextBox)item.FindControl("ntg");
            string ntg = tntg.Text.Trim().Equals("") ? "0" : tntg.Text.Trim().Equals("0") ? "0.00001" : tntg.Text.Trim();
            TextBox tph = (TextBox)item.FindControl("ph");
            string ph = tph.Text.Trim().Equals("") ? "0" : tph.Text.Trim().Equals("0") ? "0.00001" : tph.Text.Trim();
            TextBox turea = (TextBox)item.FindControl("urea");
            string urea = turea.Text.Trim().Equals("") ? "0" : turea.Text.Trim().Equals("0") ? "0.00001" : turea.Text.Trim();
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

            string query = "UPDATE MostreTancuri SET Validat = " + validat + ",Definitiv = " + definitiv + ",Sentsms = " + sentsms +
            ",Grasime= " + grasime + ",ProcentProteine=" + proteina + ",Caseina=" + caseina + ",ProcentLactoza=" + lactoza + ",PunctInghet=" + pcti +
            ",SubstantaUscata=" + substu + ",NumarCeluleSomatice=" + ncs + ",IncarcaturaGermeni=" + ntg +
            ",PH=" + ph + ",Urea=" + urea + ",Antibiotice='" + antib + "',PrelevatorId=" + prelid + " WHERE ID = " + vm.id;
            SqlCommand cmd = new SqlCommand(query, cnn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
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

        ArrayList values = createTable(datatestare.Text);

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
        ArrayList values = createTable(datatestare.Text);

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

        cmd.CommandText = "SELECT * from Prelevatori " + "WHERE CodPrelevator=" + prelid;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read())
            exista = false;
        reader.Close();
        cnn.Close();
        return exista;
    }
}
