using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using NLog.Targets;
using NLog;
using System.Text;


public partial class EditareCrotalii : Page
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
		 cmd.CommandText = "select Crotalii.CodBare, Crotalii.Crotalia, Crotalii.Nume, Crotalii.Rasa, Crotalii.DataNasterii, Crotalii.FermaId, Ferme_CCL.Nume as NumeFerma from Crotalii, Ferme_CCL WHERE Crotalii.FermaId = Ferme_CCL.ID  ";
     
        if (Crotalia.Text.Trim().Length > 0)
            cmd.CommandText += " AND Crotalii.Crotalia Like '%" + Crotalia.Text.Trim() + "%'";
        if (Ferma.Text.Trim().Length > 0)
            cmd.CommandText += " AND Crotalii.CodBare Like '%" + Ferma.Text.Trim() + "%'";
        cmd.CommandText += " ORDER BY Crotalii.CodBare, Crotalii.Crotalia";

        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrrec = 0;

        while (reader.Read())
        {
            Crotalia kr = new Crotalia();

            kr.codbare = Convert.ToString(reader["Codbare"]);
            kr.crotalia = Convert.ToString(reader["crotalia"]);
            kr.nume = Convert.ToString(reader["Nume"]);
            kr.rasa = Convert.ToString(reader["Rasa"]);
            kr.ferma = Convert.ToString(reader["NumeFerma"]);
            kr.fermaid = Convert.ToString(reader["FermaId"]);
            kr.datanasterii = Convert.ToString(reader["DataNasterii"]);
            values.Add(kr);
            // add to values
            nrrec++;

        }
        lcount.Text = nrrec + " crotalii";
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
    protected void Crotalia_TextChanged(object sender, EventArgs e)
    {
        if (Crotalia.Text.Trim().Length > 0)
        {
            Ferma.Text = "";
            Ferma.Enabled = false;
        }
        else
        {
            Ferma.Enabled = true;
        }
    }
    protected void Ferma_TextChanged(object sender, EventArgs e)
    {
        if (Ferma.Text.Trim().Length > 0)
        {
            Crotalia.Text = "";
            Crotalia.Enabled = false;
        }
        else
        {
            Crotalia.Enabled = true;
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
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
                mostre.UpdateCrotalii(filenameserver);

            BindData();
        }
    }

    private string GetApplicationPath()
    {
        return HttpContext.Current.Request.ApplicationPath;
    }
    protected void Button3_Click(object sender, EventArgs e)
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
                mostre.UpdateCrotaliiBV(filenameserver);

			BindData();
        }
    }

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static string GetTodaysTagLog()
	{
		var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("fCrotalii");
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
