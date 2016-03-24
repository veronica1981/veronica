using FCCL_DAL;
using System;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for UMostre
/// </summary>
public class UMostre
{ 
	public static int VerificGrasime(string valGrasime)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Grasime, valGrasime) ? 0 : 1;
	}

	public static int VerificProteine(string valProteine)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.ProcentProteine, valProteine) ? 0 : 1;
	}

	public static int VerificLactoza(string valLactoza)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.ProcentLactoza, valLactoza) ? 0 : 1;
	}

	public static int VerificNCS(string valNCS)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.NCS, valNCS) ? 0 : 1;
	}

	public static int VerificNTG(string valNTG)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.NTG, valNTG) ? 0 : 1;
	}

	public static int VerificSolids(string valSolids)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Solids, valSolids) ? 0 : 1;
	}

	public static int VerificPctInghet(string valPctInghet)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.FrzPnt, valPctInghet) ? 0 : 1;
	}

	public static int VerificPh(string valPh)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Ph, valPh) ? 0 : 1;
	}

	public static int VerificUrea(string valUrea)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Urea, valUrea) ? 0 : 1;
	}

	public static int VerificCasein(string valCasein)
	{
		return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Casein, valCasein) ? 0 : 1;
	}

    public static Buletin GetBuletin(string fabricaid, string datatestare)
    {
        SqlConnection cnn = new SqlConnection(
       ConfigurationManager.ConnectionStrings
       ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "select * from BuletinAnaliza where id_fabrica ="+fabricaid+" AND CONVERT(datetime,data,103) = CONVERT(datetime,'"+datatestare+"',103) ORDER BY ID DESC";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrb=0;
        string datab = "";
        if (reader.Read())
        {
            nrb = Convert.ToInt32(reader["buletin_nr"]);         
            datab = ((DateTime)reader["buletin_data"]).ToString("dd/MM/yyyy");

        }
        else
        {
            reader.Close();
          //  cmd.CommandText = "select * from BuletinAnaliza  ORDER BY ID DESC";
            cmd.CommandText = "select MAX(buletin_nr) AS MaxNrb from BuletinAnaliza";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                nrb = Convert.ToInt32(reader["MaxNrb"]);
                datab = (DateTime.Now).ToString("dd/MM/yyyy");
                //datab = Convert.ToString(reader["buletin_data"]);

            }
            nrb++;
        }
        reader.Close();
        cnn.Close();
        Buletin b = new Buletin();
        b.Nrb = nrb;
        b.Datab = datab;
        return b;
    }

    public static Buletin GetBuletinFCB(string client, string datatestare)
    {
        SqlConnection cnn = new SqlConnection(
       ConfigurationManager.ConnectionStrings
       ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "select * from BuletinAnalizaFaraCodBare where NumeClient ='" + client + "' AND CONVERT(datetime,data,103) = CONVERT(datetime,'" + datatestare + "',103) ORDER BY ID DESC";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        int nrb = 0;
        string datab = "";
        if (reader.Read())
        {
            nrb = Convert.ToInt32(reader["buletin_nr"]);
            datab = ((DateTime)reader["buletin_data"]).ToString("dd/MM/yyyy");

        }
        else
        {
            reader.Close();
            //  cmd.CommandText = "select * from BuletinAnaliza  ORDER BY ID DESC";
            cmd.CommandText = "select MAX(buletin_nr) AS MaxNrb from BuletinAnalizaFaraCodBare";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                nrb = Convert.ToInt32(reader["MaxNrb"]);
                datab = (DateTime.Now).ToString("dd/MM/yyyy");

            }
            nrb++;
        }
        reader.Close();
        cnn.Close();
        Buletin b = new Buletin();
        b.Nrb = nrb;
        b.Datab = datab;
        return b;
    }

    public static void AddBuletinAnaliza(string fabricaid, string nrb, string data, string datab)
    {

        SqlConnection cnn = new SqlConnection(
      ConfigurationManager.ConnectionStrings
         ["fccl2ConnectionString"].ConnectionString);
        string query = "INSERT INTO BuletinAnaliza (data, buletin_data, id_fabrica, buletin_nr) VALUES("
        +"CONVERT(datetime,'"+data+"',103),CONVERT(datetime,'"+datab+"',103),"+fabricaid+","+nrb+")";
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();




    }

    public static void AddBuletinAnalizaFCB(string client, string nrb, string data, string datab)
    {

        SqlConnection cnn = new SqlConnection(
      ConfigurationManager.ConnectionStrings
         ["fccl2ConnectionString"].ConnectionString);
        string query = "INSERT INTO BuletinAnalizaFaraCodBare (data, buletin_data, NumeClient, buletin_nr) VALUES("
        + "CONVERT(datetime,'" + data + "',103),CONVERT(datetime,'" + datab + "',103),'" + client + "'," + nrb + ")";
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();




    }

    public static string MyRound(string str)
    {
        int pos;
        pos = str.IndexOf(".");
        if (pos > 0)
        {
            int p;
            p = str.Length - (pos + 2);
            if (p > 0)
                return str.Remove(pos + 2, str.Length - (pos + 2));
            else
                return str;
        }
        else
            return str;
    }

    public static string ApaAdaugata(string valPctInghet)
    {
       

        string sverific = " ";
        double verific;
        if (valPctInghet == null || Convert.ToDouble(valPctInghet) > 5000.00)
            verific = 0;
        else
            verific = -((-0.515 - Convert.ToDouble("-0." + valPctInghet + "00")) / 0.005);
        if (verific <= 0)
            verific = 0;
        sverific = MyRound(Convert.ToString(verific));

        return sverific;
    }    

}
