using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Web.UI;


public partial class RaportRegistru : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        xlslink.Visible = false;
        if (TextBox1.Text.Trim() == "")
            TextBox1.Text = (DateTime.Now).ToString("dd/MM/yyyy");
      

    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
   
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        DropDownList1.DataBind();
    }

   
    public string replace_special_car(string nume_ini)
    {
        // nu merge htmlencode 
        // le inlocuiesc pe cele posibil de intalnit in nume fabrica 
        // folosit pentru numele fisierului de listat
        string nume_fin = "";
        nume_fin = nume_ini.Replace(" ", "_");
        nume_fin = nume_fin.Replace("&", "");
        nume_fin = nume_fin.Replace("\"", "");
        nume_fin = nume_fin.Replace(".", "");
        nume_fin = nume_fin.Replace("\'", "");
        nume_fin = nume_fin.Replace("*", "");
        nume_fin = nume_fin.Replace("@", "");
        nume_fin = nume_fin.Replace("\\", "");
        nume_fin = nume_fin.Replace("-", "");
        nume_fin = nume_fin.Replace("/", "");
        nume_fin = nume_fin.Replace("|", "");
        nume_fin = nume_fin.Replace("=", "");

        return (nume_fin);
    }
    public string replace_special_car_null(string nume_ini)
    {
        // nu merge htmlencode 
        // numele fisierului care va fi trimis pe mail nu trebuie sa contina mai multe _ decat cele formatate pt data si intre data si id asa ca voi inlocui inclusiv _ din numele fermei sau fabricii
        // datorita parsului asupra numelui fisierului pdf care va fi trimis pe mail se vor inlocui cu "" urmatoarele caractere
        // calea spre fis este folosita ca si link
        // le inlocuiesc pe cele intalnite in nume fabrica si nume ferma pt fis care vor fi trimise pe e-mail ... altele adaugate
        string nume_fin = "";
        nume_fin = nume_fin.Replace("_", "");// _ folosit in regula de parse pt email
        nume_fin = nume_ini.Replace(" ", "");
        nume_fin = nume_fin.Replace("&", "");
        nume_fin = nume_fin.Replace("\"", "");
        nume_fin = nume_fin.Replace(".", "");
        nume_fin = nume_fin.Replace("\'", "");
        nume_fin = nume_fin.Replace("*", "");
        nume_fin = nume_fin.Replace("@", "");
        nume_fin = nume_fin.Replace("\\", "");
        nume_fin = nume_fin.Replace("-", "");// folosit in regula de parse pt email automat	
        nume_fin = nume_fin.Replace("/", "");
        nume_fin = nume_fin.Replace("|", "");
        nume_fin = nume_fin.Replace("=", "");
        // sper ca alte caractere ca <>?{}[]^()%$#!~ nu se vor folosi in numele fermei si al fabricii
        return (nume_fin);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.Items.Count >0)
        CreateReport();
    }

    public void CreateReport()
    {
        Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
        string datatestare = TextBox1.Text;
        string fermaid = DropDownList1.SelectedValue;
        string fermaname = DropDownList1.Items[DropDownList1.SelectedIndex].ToString();
        fermaname = replace_special_car_null(fermaname);
        // read fabrica
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

		string httppath = StaticDataHelper.SettingsManager.CaleRapoarteHttp;
		string filepath = StaticDataHelper.SettingsManager.CaleRapoarte;
       
        string raport_excel = "CSV_Registru_" +   datatestare.Replace("/", "") + "_" + fermaid+ "_"+fermaname +".csv";

        string path_raport_http = "http://" + Request.ServerVariables.Get("HTTP_HOST") + "/" + httppath;
        // writefile

        string excel_link = path_raport_http + @"Registru/" + raport_excel;
        string excel_file = filepath + @"Registru/" + raport_excel;
        // select data
        cmd.CommandText = "SELECT DataPrelevare, CodBare,Grasime,ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PH,Urea,NumarCeluleSomatice FROM MostreTancuri WHERE"
+ " MostreTancuri.DataTestareFinala = CONVERT(datetime, '" + datatestare + "', 103) "
+ " AND MostreTancuri.FermaID =" + fermaid + " AND MostreTancuri.Validat = 1 ORDER BY CodFerma";
        cnn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable table = ds.Tables[0];
        CreateExcelFile(table, excel_file);
        cnn.Close();

        // display link
        xlslink.Visible = true;
        xlslink.NavigateUrl = excel_link;
        xlslink.Text = raport_excel;
    }
    
    public static bool CreateExcelFile(DataTable dt, string filename)
    {
       if (File.Exists(filename))
            File.Delete(filename);
        StreamWriter oExcelWriter = File.CreateText(filename);

        try
        {
            foreach (DataRow row in dt.Rows)
            {
                StringBuilder sTableData = new StringBuilder();
                
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string text = row[i].ToString().Trim();

                    if (i == 0)
                        text = DateTime.Parse(text).ToString("yyyy-MM-dd");
                    else
                    {
                        try
                        {
                            if (Convert.ToDouble(text, CultureInfo.InvariantCulture) == 0 || Convert.ToDouble(text, CultureInfo.InvariantCulture) == 0.00001)
                                text = "";
                        }
                        catch { }

                        sTableData.Append(",");
                    }
                    sTableData.Append(text);
                    


                }
                oExcelWriter.WriteLine(sTableData.ToString());

            }
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            oExcelWriter.Close();
        }
    }         

}
