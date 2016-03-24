using System.Drawing;
using System.Web.UI;
using FCCL_BL.Bus;
using NLog;
using NLog.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class EditareMostre : Page
{
    public static Logger logger = LogManager.GetCurrentClassLogger();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
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
        ntg1.Text = ntg2.Text = "";
        pcti1.Text = pcti2.Text = "";
        antib.SelectedIndex = 0;
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
                if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith("B1.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("B2.CSV"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Bacto/");
                }

                if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C2.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C3.CSV") || FileUpload1.PostedFile.FileName.ToUpper().EndsWith("C1.CSV"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Combi/");
                }
                if ((Path.GetFileName(FileUpload1.PostedFile.FileName).ToUpper().IndexOf("A") >= 0) && FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".TXT"))
                {
                    ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Crio/");
                }
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

        if (FileUpload1.PostedFile.FileName != "")
        {
            string ServerPath = "";
            if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".XLS"))
            {
                ServerPath = Server.MapPath("~/Downloads/FisiereReceptie/");
            }
            string filenameserver = ServerPath + Path.GetFileName(FileUpload1.PostedFile.FileName);

            FileUpload1.PostedFile.SaveAs(filenameserver);

            if (FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".XLS"))
                mostre.UpdatePrelevatori(filenameserver);

            BindData();
        }
    }

    protected void importcr_Click(object sender, EventArgs e)
    {
        MostreDB mostre = new MostreDB();

        if (FileUpload1.PostedFile.FileName != "" && FileUpload2.PostedFile.FileName != "")
        {
            string ServerPath = "";
            if ((Path.GetFileName(FileUpload1.PostedFile.FileName).ToUpper().IndexOf("A3") >= 0) && FileUpload1.PostedFile.FileName.ToUpper().EndsWith(".TXT"))
            {
                ServerPath = Server.MapPath("~/Downloads/CSV_ImportateManual/Crio/");
            }

            string filenameserver = ServerPath + Path.GetFileName(FileUpload1.PostedFile.FileName);
            string xlsnameserver = ServerPath + Path.GetFileName(FileUpload2.PostedFile.FileName);
            filenameserver = filenameserver.ToLower();
            xlsnameserver = xlsnameserver.ToLower();
            FileUpload1.PostedFile.SaveAs(filenameserver);
            FileUpload2.PostedFile.SaveAs(xlsnameserver);

            mostre.ImportManual(filenameserver);
            BindData();
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
                if (GridView1.Rows[i].Cells[4].Text.Trim() != "" && !GridView1.Rows[i].Cells[4].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificGrasime(GridView1.Rows[i].Cells[4].Text) == 1)
                        GridView1.Rows[i].Cells[4].BackColor = Color.Red;
                    string grasime = GridView1.Rows[i].Cells[4].Text.Trim();
                    GridView1.Rows[i].Cells[4].Text = (Convert.ToDouble(grasime) == 0) ? "" : (Convert.ToDouble(grasime) == 0.00001) ? "0" : grasime;
                }
                if (GridView1.Rows[i].Cells[5].Text.Trim() != "" && !GridView1.Rows[i].Cells[5].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificProteine(GridView1.Rows[i].Cells[5].Text) == 1)
                        GridView1.Rows[i].Cells[5].BackColor = Color.Red;
                    string proteine = GridView1.Rows[i].Cells[5].Text.Trim();
                    GridView1.Rows[i].Cells[5].Text = (Convert.ToDouble(proteine) == 0) ? "" : (Convert.ToDouble(proteine) == 0.00001) ? "0" : proteine;
                }

                if (GridView1.Rows[i].Cells[7].Text.Trim() != "" && !GridView1.Rows[i].Cells[7].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificLactoza(GridView1.Rows[i].Cells[7].Text) == 1)
                        GridView1.Rows[i].Cells[7].BackColor = Color.Red;
                    string lactoza = GridView1.Rows[i].Cells[7].Text.Trim();
                    GridView1.Rows[i].Cells[7].Text = (Convert.ToDouble(lactoza) == 0) ? "" : (Convert.ToDouble(lactoza) == 0.00001) ? "0" : lactoza;
                }

                if (GridView1.Rows[i].Cells[8].Text.Trim() != "" && !GridView1.Rows[i].Cells[8].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificSolids(GridView1.Rows[i].Cells[8].Text) == 1)
                        GridView1.Rows[i].Cells[8].BackColor = Color.Red;
                    string substu = GridView1.Rows[i].Cells[8].Text.Trim();
                    GridView1.Rows[i].Cells[8].Text = (Convert.ToDouble(substu) == 0) ? "" : (Convert.ToDouble(substu) == 0.00001) ? "0" : substu;
                }
                //ph
                if (GridView1.Rows[i].Cells[9].Text.Trim() != "" && !GridView1.Rows[i].Cells[9].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificPh(GridView1.Rows[i].Cells[9].Text) == 1)
                        GridView1.Rows[i].Cells[9].BackColor = Color.Red;
                    string ph = GridView1.Rows[i].Cells[9].Text.Trim();
                    GridView1.Rows[i].Cells[9].Text = (Convert.ToDouble(ph) == 0) ? "" : (Convert.ToDouble(ph) == 0.00001) ? "0" : ph;
                }
                string pcti = GridView1.Rows[i].Cells[10].Text.Trim();

                if (pcti != "" && !pcti.StartsWith("&nbsp;") && Convert.ToDouble(pcti) != 0.00001)
                {

                    if (UMostre.VerificPctInghet(pcti) == 1)
                        GridView1.Rows[i].Cells[10].BackColor = Color.Red;
                    GridView1.Rows[i].Cells[10].Text = (Convert.ToDouble(pcti) == 0) ? "" : (Convert.ToDouble(pcti) == 0.00001) ? "0" : "-0." + pcti;
                }
                //urea
                if (GridView1.Rows[i].Cells[13].Text.Trim() != "" && !GridView1.Rows[i].Cells[13].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificUrea(GridView1.Rows[i].Cells[13].Text) == 1)
                        GridView1.Rows[i].Cells[13].BackColor = Color.Red;
                    string urea = GridView1.Rows[i].Cells[13].Text.Trim();
                    GridView1.Rows[i].Cells[13].Text = (Convert.ToDouble(urea) == 0) ? "" : (Convert.ToDouble(urea) == 0.00001) ? "0" : urea;
                }

                if (GridView1.Rows[i].Cells[14].Text.Trim() != "" && !GridView1.Rows[i].Cells[14].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificNCS(GridView1.Rows[i].Cells[14].Text) == 1)
                        GridView1.Rows[i].Cells[14].BackColor = Color.Red;
                    string ncs = GridView1.Rows[i].Cells[14].Text.Trim();
                    GridView1.Rows[i].Cells[14].Text = (Convert.ToDouble(ncs) == 0) ? "" : (Convert.ToDouble(ncs) == 0.00001) ? "0" : ncs;
                }
                if (GridView1.Rows[i].Cells[15].Text.Trim() != "" && !GridView1.Rows[i].Cells[15].Text.StartsWith("&nbsp;"))
                {
                    if (UMostre.VerificNTG(GridView1.Rows[i].Cells[15].Text) == 1)
                        GridView1.Rows[i].Cells[15].BackColor = Color.Red;
                    string ntg = GridView1.Rows[i].Cells[15].Text.Trim();
                    GridView1.Rows[i].Cells[15].Text = (Convert.ToDouble(ntg) == 0) ? "" : (Convert.ToDouble(ntg) == 0.00001) ? "0" : ntg;
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("BindData|ERROR:{0}", ex.Message));
            }
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!Page.User.IsInRole("admin"))
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
            vm.id = Convert.ToString(reader["ID"]);
            vm.codbare = Convert.ToString(reader["CodBare"]);
            vm.idzilnic = Convert.ToString(reader["IdZilnic"]);
            vm.datat = Convert.ToString(reader["DataTestare"]);


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
            vm.bvalidat = Convert.ToBoolean(reader["Validat"]);
            vm.bdefinitiv = Convert.ToBoolean(reader["Definitiv"]);
            vm.definitiv = Convert.ToString(reader["Definitiv"]);
            vm.dataf = Convert.ToString(reader["DataTestareFinala"]);
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
        StringBuilder command = new StringBuilder(";WITH CTE AS (SELECT *, ROW_NUMBER() OVER (ORDER BY CONVERT(datetime, DataTestare, 103) DESC, IdZilnic ASC) AS RN FROM MostreTancuri");
        string whereClause = GetWhereClause();
        if (whereClause != string.Empty)
        {
            command.Append(" WHERE ");
            command.Append(whereClause);
        }
        command.Append(string.Format(") SELECT * FROM CTE WHERE RN BETWEEN {0} AND {1} ", startIdx, endIdx));
        command.Append(" ORDER BY CONVERT(datetime, DataTestare, 103) DESC, IdZilnic ASC");
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
            clauses.Add(string.Format(" CONVERT(datetime, DataTestare, 103) >= CONVERT(datetime, '{0}', 103) ", datatest1.Text.Trim()));
        }
        if (datatest2.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" CONVERT(datetime, DataTestare, 103) <= CONVERT(datetime, '{0}', 103) ", datatest2.Text.Trim()));
        }
        if (ntg1.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" IncarcaturaGermeni >= {0} ", ntg1.Text.Trim()));
        }
        if (ntg2.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" IncarcaturaGermeni <= {0} ", ntg2.Text.Trim()));
        }
        if (ncs1.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" NumarCeluleSomatice >= {0} ", ncs1.Text.Trim()));
        }
        if (ncs2.Text.Trim().Length > 0)
        {
            clauses.Add(string.Format(" NumarCeluleSomatice <= {0} ", ncs2.Text.Trim()));
        }

        if (antib.SelectedValue != "Toate")
        {
            switch (antib.SelectedValue)
            {
                case "Pozitive":
                    clauses.Add(" (Antibiotice ='1' OR Antibiotice = '-1') ");
                    break;
                case "Negative":
                    clauses.Add(" (Antibiotice = '0') ");
                    break;
                case "Netestate":
                    clauses.Add(" (Antibiotice = '') ");
                    break;
            }

        }
        if (pcti1.Text != "" && pcti1.Text.Length >= 3)
        {
            clauses.Add(string.Format(" PunctInghet <= {0} ", pcti1.Text.Substring(3, pcti1.Text.Length - 3)));
        }
        if (pcti2.Text != "" && pcti2.Text.Length >= 3)
        {
            clauses.Add(string.Format(" PunctInghet >= {0} ", pcti2.Text.Substring(3, pcti2.Text.Length - 3)));
        }
        return string.Join("AND", clauses);
    }

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static string GetTodaysReceptionLog()
	{
		var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("fReceptie");
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
