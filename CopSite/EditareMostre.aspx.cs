using System.Drawing;
using FCCL_BL.Bus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using NLog.Targets;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;

public partial class EditareMostre : Page
{
    private static Logger logger = LogManager.GetCurrentClassLogger();

    protected void Page_Load(object sender, EventArgs e)
    {
		BindData();
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
        if (!Page.User.Identity.IsAuthenticated)
        {
            try
            {
                if (e.Row.Cells.Count > 2)
                {
                    CheckBox chkf = (CheckBox)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];
                    e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
                    GridView1.HeaderRow.Cells[e.Row.Cells.Count - 2].Visible = false;
                    if (chkf.Checked)
                        e.Row.Cells[e.Row.Cells.Count - 1].Enabled = false;
                }
            }
            catch
            {
            }
        }
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

    protected void Button2_Click(object sender, EventArgs e)
    {
        cod.Text = "";
        datatest1.Text = datatest2.Text = "";
        ncs1.Text = ncs2.Text = "";
        BindData();
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

    protected void importm_Click(object sender, EventArgs e)
    {
        MostreDB mostre = new MostreDB();

        if (FileUpload1.PostedFile.FileName != "")
        {
            string ServerPath = "";
            if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".XLS"))
            {
                ServerPath = Server.MapPath("~/Downloads/FisiereReceptie/");
            }
            else
            {
                if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith("B1COP.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("B2COP.CSV"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Bacto/");
                }

                if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith("CCOP.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C2COP.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C3COP.CSV"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Combi/");
                }
                if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C.CSV"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/CombiDelta/");
                }
                if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C1COP.CSV"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Combi/");
                }
                /*
                if ((Path.GetFileName(FileUpload1.PostedFile.FileName).ToUpper().IndexOf("A") >= 0) && FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".TXT"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Crio/");
                    loglink = GetApplicationPath() + @"/Downloads/CSV_ImportateManual/Crio/";
                }
                 */
            }


            string filenameserver = ServerPath + Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.PostedFile.SaveAs(filenameserver);

            if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".XLS"))
                mostre.ImportFisReceptie(filenameserver);
            if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".CSV") ||
                (Path.GetFileName(FileUpload1.PostedFile.FileName).ToUpper().IndexOf("A") >= 0 && FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".TXT")))
                mostre.ImportManual(filenameserver);

            BindData();
        }
    }
    protected void importp_Click(object sender, EventArgs e)
    {
        MostreDB mostre = new MostreDB();

        if (FileUpload1.PostedFile.FileName != "" && FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".XLS"))
        {
            string serverPath = Server.MapPath("~/Downloads/FisiereReceptie/");

            string filenameserver = serverPath + Path.GetFileName(FileUpload1.PostedFile.FileName);
            if (FileUpload2.PostedFile.FileName != "" && FileUpload2.PostedFile.FileName.ToUpper().EndsWith(".XLS"))
            {
                string filenamecsv = serverPath + Path.GetFileName(FileUpload2.PostedFile.FileName);
                FileUpload1.PostedFile.SaveAs(filenameserver);
                FileUpload2.PostedFile.SaveAs(filenamecsv);

                mostre.VerificareBV(filenameserver, filenamecsv);

                BindData();
            }
        }
    }

    private string GetApplicationPath()
    {
        return HttpContext.Current.Request.ApplicationPath;
    }

    protected void importa_Click(object sender, EventArgs e)
    {
        string adresafizicaserver = Server.MapPath("~");
        string cale_import = Server.MapPath("~/Downloads/");

        IMostreDB mostre = (IMostreDB)new MostreDB();
        FileImporter importer = new FileImporter(StaticDataHelper.FCCLDbContext, adresafizicaserver);
        importer.DoLocalImport(mostre, cale_import);

        BindData();
    }

    protected void importap_Click(object sender, EventArgs e)
    {
        string adresafizicaserver = Server.MapPath("~");

        IMostreDB mostre = (IMostreDB)new MostreDB();
        FileImporter importer = new FileImporter(StaticDataHelper.FCCLDbContext, adresafizicaserver);
        importer.DoRemoteImport(mostre);

        BindData();
    }

    public void BindData()
    {
        GridView1.DataSource = GetMostrePaged(GridView1.PageIndex, GridView1.PageSize);
        GridView1.VirtualItemCount = GetMostreCount();
        lcount.Text = GridView1.VirtualItemCount + " mostre";
        GridView1.DataBind();

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            try
            {
                if (GridView1.Rows[i].Cells[5].Text.Trim() != "" && !GridView1.Rows[i].Cells[5].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificGrasime(GridView1.Rows[i].Cells[5].Text) == 1)
                        GridView1.Rows[i].Cells[5].BackColor = Color.Red;
                    string grasime = GridView1.Rows[i].Cells[5].Text.Trim();
                    GridView1.Rows[i].Cells[5].Text = (Convert.ToDouble(grasime, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(grasime, CultureInfo.InvariantCulture) == 0.00001) ? "0" : grasime;
                }
                if (GridView1.Rows[i].Cells[6].Text.Trim() != "" && !GridView1.Rows[i].Cells[6].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificProteine(GridView1.Rows[i].Cells[6].Text) == 1)
                        GridView1.Rows[i].Cells[6].BackColor = Color.Red;
                    string proteine = GridView1.Rows[i].Cells[6].Text.Trim();
                    GridView1.Rows[i].Cells[6].Text = (Convert.ToDouble(proteine, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(proteine, CultureInfo.InvariantCulture) == 0.00001) ? "0" : proteine;
                }

                if (GridView1.Rows[i].Cells[7].Text.Trim() != "" && !GridView1.Rows[i].Cells[7].Text.StartsWith("&nbsp;"))
                {
                    string cazeina = GridView1.Rows[i].Cells[7].Text.Trim();
                    GridView1.Rows[i].Cells[7].Text = (Convert.ToDouble(cazeina, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(cazeina, CultureInfo.InvariantCulture) == 0.00001) ? "0" : cazeina;
                }

                if (GridView1.Rows[i].Cells[8].Text.Trim() != "" && !GridView1.Rows[i].Cells[8].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificLactoza(GridView1.Rows[i].Cells[8].Text) == 1)
                        GridView1.Rows[i].Cells[8].BackColor = Color.Red;
                    string lactoza = GridView1.Rows[i].Cells[8].Text.Trim();
                    GridView1.Rows[i].Cells[8].Text = (Convert.ToDouble(lactoza, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(lactoza, CultureInfo.InvariantCulture) == 0.00001) ? "0" : lactoza;
                }

                if (GridView1.Rows[i].Cells[9].Text.Trim() != "" && !GridView1.Rows[i].Cells[9].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificSolids(GridView1.Rows[i].Cells[9].Text) == 1)
                        GridView1.Rows[i].Cells[9].BackColor = Color.Red;
                    string substu = GridView1.Rows[i].Cells[9].Text.Trim();
                    GridView1.Rows[i].Cells[9].Text = (Convert.ToDouble(substu, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(substu, CultureInfo.InvariantCulture) == 0.00001) ? "0" : substu;
                }
                //ph
                if (GridView1.Rows[i].Cells[10].Text.Trim() != "" && !GridView1.Rows[i].Cells[10].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificPh(GridView1.Rows[i].Cells[10].Text) == 1)
                        GridView1.Rows[i].Cells[10].BackColor = Color.Red;
                    string ph = GridView1.Rows[i].Cells[10].Text.Trim();
                    GridView1.Rows[i].Cells[10].Text = (Convert.ToDouble(ph, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(ph, CultureInfo.InvariantCulture) == 0.00001) ? "0" : ph;
                }

                //urea
                if (GridView1.Rows[i].Cells[11].Text.Trim() != "" && !GridView1.Rows[i].Cells[11].Text.StartsWith("&nbsp;"))
                {
                    string urea = GridView1.Rows[i].Cells[11].Text.Trim();
                    if ((Convert.ToDouble(urea, CultureInfo.InvariantCulture) >= 0.1) && UMostre.VerificUrea(urea) == 1)
                        GridView1.Rows[i].Cells[11].BackColor = Color.Red;
                    GridView1.Rows[i].Cells[11].Text = (Convert.ToDouble(urea, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(urea, CultureInfo.InvariantCulture) == 0.00001) ? "" : (Convert.ToDouble(urea, CultureInfo.InvariantCulture) < 0.1) ? "" : urea;
                }

                if (GridView1.Rows[i].Cells[12].Text.Trim() != "" && !GridView1.Rows[i].Cells[12].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificNCS(GridView1.Rows[i].Cells[12].Text) == 1)
                        GridView1.Rows[i].Cells[12].BackColor = Color.Red;
                    string ncs = GridView1.Rows[i].Cells[12].Text.Trim();
                    GridView1.Rows[i].Cells[12].Text = (Convert.ToDouble(ncs, CultureInfo.InvariantCulture) == 0) ? "" : (Convert.ToDouble(ncs, CultureInfo.InvariantCulture) == 0.00001) ? "0" : ncs;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("BindData|ERROR:{0}", ex.Message));
            }
        }
    }

    public ArrayList GetMostrePaged(int pageIndex, int pageSize)
    {
        int startIdx = pageSize * pageIndex + 1;
        int endIdx = pageSize * (pageIndex + 1);
        ArrayList values = new ArrayList();

        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = GetSelectStatement(startIdx, endIdx);
        cmd.CommandTimeout = 300;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            VMostre vm = new VMostre();
            vm.Id = Convert.ToString(reader["ID"]);
            vm.codbare = Convert.ToString(reader["CodBare"]);
            vm.idzilnic = Convert.ToString(reader["IdZilnic"]);
            vm.datat = Convert.ToDateTime(reader["DataTestare"]);
            vm.rasa = Convert.ToString(reader["Rasa"]);

            vm.grasime = Convert.ToString(reader["Grasime"]);
            vm.proteina = Convert.ToString(reader["ProcentProteine"]);
            vm.lactoza = Convert.ToString(reader["ProcentLactoza"]);
            vm.substu = Convert.ToString(reader["SubstantaUscata"]);
            vm.pcti = Convert.ToString(reader["PunctInghet"]);
            vm.apaad = "";
            if (!string.IsNullOrEmpty(vm.pcti) && vm.pcti != "0" && vm.pcti.IndexOf(".") < 0)
                vm.apaad = UMostre.ApaAdaugata(vm.pcti);
            vm.antib = Convert.ToString(reader["Antibiotice"]);
            if (vm.antib == "-1" || vm.antib == "1")
                vm.antib = "Pozitiv";
            else if (vm.antib == "0")
                vm.antib = "Negativ";
            vm.ncs = Convert.ToString(reader["NumarCeluleSomatice"]);
            vm.ntg = Convert.ToString(reader["IncarcaturaGermeni"]);
            vm.urea = Convert.ToString(reader["Urea"]);
            vm.ph = Convert.ToString(reader["PH"]);
            vm.casein = Convert.ToString(reader["Caseina"]);


            vm.validat = Convert.ToString(reader["Validat"]);
            vm.Bvalidat = Convert.ToBoolean(reader["Validat"]);
            vm.Bdefinitiv = Convert.ToBoolean(reader["Definitiv"]);
            vm.definitiv = Convert.ToString(reader["Definitiv"]);
            vm.dataf = Convert.ToDateTime(reader["DataTestareFinala"]);
            values.Add(vm);
        }
        reader.Close();
        cnn.Close();
        return values;
    }

    public int GetMostreCount()
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = GetCountStatement();
        cmd.CommandTimeout = 300;
        cnn.Open();

        int cnt = (int)cmd.ExecuteScalar();

        return cnt;
    }

    public string GetSelectStatement(int startIdx, int endIdx)
    {
        StringBuilder command = new StringBuilder(";WITH CTE AS (SELECT *, ROW_NUMBER() OVER (ORDER BY DataTestare DESC, IdZilnic ASC) AS RN FROM MostreTancuri");
        string whereClause = GetWhereClause();
        if (whereClause != string.Empty)
        {
            command.Append(" WHERE ");
            command.Append(whereClause);
        }
        command.Append(string.Format(") SELECT * FROM CTE WHERE RN BETWEEN {0} AND {1} ", startIdx, endIdx));

        command.Append(" ORDER BY DataTestare DESC, IdZilnic ASC");
        return command.ToString();
    }

    public string GetCountStatement()
    {
        StringBuilder command = new StringBuilder("SELECT COUNT(*) FROM MostreTancuri");
        string whereClause = GetWhereClause();

        if (whereClause != string.Empty)
        {
            command.Append(" WHERE ");
            command.Append(whereClause);
        }

        return command.ToString();
    }

    private string GetWhereClause()
    {
        List<string> clauses = new List<string>();
        if (cod.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" CodBare Like '%{0}%' ", cod.Text.Trim()));
        }
        if (datatest1.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" DataTestare >= CONVERT(datetime, '{0}', 103) ", datatest1.Text.Trim()));
        }
        if (datatest2.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" DataTestare <= CONVERT(datetime, '{0}', 103) ", datatest2.Text.Trim()));
        }
        if (ncs1.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" NumarCeluleSomatice >= {0} ", ncs1.Text.Trim()));
        }
        if (ncs2.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" NumarCeluleSomatice <= {0} ", ncs2.Text.Trim()));
        }
        return string.Join("AND", clauses);
    }

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static string GetTodaysReceptionLog()
	{
		var fileTarget = (FileTarget) LogManager.Configuration.FindTargetByName("fReceptie");
		var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now }; 
		string fileName = fileTarget.FileName.Render(logEventInfo);
		string strLog = string.Empty;

		if (File.Exists(fileName))
		{
			using (StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8))
			{
				strLog = streamReader.ReadToEnd();
			}
		}
		else
		{
			strLog = string.Format("Fisierul de log: {0} nu a fost initializat.", fileName);
		}
		return strLog;
	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static string GetTodaysResultsLog()
	{
		var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("fRezultate");
		var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
		string fileName = fileTarget.FileName.Render(logEventInfo);
		string strLog = string.Empty;

		if (File.Exists(fileName))
		{
			using (StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8))
			{
				strLog = streamReader.ReadToEnd();
			}
		}
		else
		{
			strLog = string.Format("Fisierul de log: {0} nu a fost initializat.", fileName);
		}
		return strLog;
	}
}

