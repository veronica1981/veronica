using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for MostreStatistici
/// </summary>
public class MostreStatistici
{
	public MostreStatistici()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int iNrMostre;
    public int iTime;
    public int iDay;
    public int iNcs;
    public int iNtg;

    public int NrMostre
    {
        get
        {
            return iNrMostre;
        }
        set
        {
            iNrMostre = value;
        }
    }

    public int Time
    {
        get
        {
            return iTime;
        }
        set
        {
            iTime = value;
        }
    }


    public int Day
    {
        get
        {
            return iDay;
        }
        set
        {
            iDay = value;
        }
    }

    public int Ncs
    {
        get
        {
            return iNcs;
        }
        set
        {
            iNcs = value;
        }
    }

    public int Ntg
    {
        get
        {
            return iNtg;
        }
        set
        {
            iNtg = value;
        }
    }


     public static List<MostreStatistici> GetTotalMostre(string fabricaid)
    {
        List<MostreStatistici> list = new List<MostreStatistici>();

        SqlConnection cnn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        //    SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        for (int i = 1; i < 13; i++)
        {

            cmd.CommandText = "SELECT COUNT(*) AS TotalMostre FROM MostreTancuri INNER JOIN FERME_CCL ON MostreTancuri.FermaID = Ferme_CCL.ID WHERE "
+ " MONTH(CONVERT(datetime, MostreTancuri.DataTestareFinala, 103)) = " + i
+ " AND Ferme_CCL.FabricaID = " + fabricaid;

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            reader.Read();

            MostreStatistici ms = new MostreStatistici();
            ms.NrMostre = Convert.ToInt32(reader["TotalMostre"]);
            ms.Time = i;
            list.Add(ms);


            reader.Close();
            cnn.Close();
        }
        return list;
    }


    public static List<MostreStatistici> GetMostreLuna(string fabricaid,string luna)
    {
        List<MostreStatistici> list = new List<MostreStatistici>();

        SqlConnection cnn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        //    SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "SELECT DAY(CONVERT(datetime, MostreTancuri.DataTestare, 103)) AS Ziua, MostreTancuri.DataTestareFinala,MostreTancuri.NumarCeluleSomatice,MostreTancuri.IncarcaturaGermeni FROM MostreTancuri INNER JOIN FERME_CCL ON MostreTancuri.FermaID = Ferme_CCL.ID WHERE "
+ " MONTH(CONVERT(datetime, MostreTancuri.DataTestareFinala, 103)) = " + luna
+ " AND Ferme_CCL.FabricaID = " + fabricaid
+ " ORDER BY DAY(CONVERT(datetime,DataTestareFinala,103)) ASC";

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int totncs, nrncs, totntg, nrntg;
            int day;
            totncs = totntg = nrntg = nrncs = 0;
            if (reader.Read())
            {
               
                day = Convert.ToInt32(reader["Ziua"]);
                do
                {
                    
                    if (day != Convert.ToInt32(reader["Ziua"]))
                    {
                        try
                        {
                            MostreStatistici ms = new MostreStatistici();
                            ms.Day = day;
                            ms.Ncs = totncs / nrncs;
                            ms.Ntg = totntg / nrntg;
                            list.Add(ms);
                        }
                        catch { }
                        day = Convert.ToInt32(reader["Ziua"]);
                        totncs = totntg = nrntg = nrncs = 0;
                    }
                  //  if (Convert.ToInt32(reader["NumarCeluleSomatice"])!=0)
                    {
                        totncs += Convert.ToInt32(reader["NumarCeluleSomatice"]);
                        nrncs++;
                    }
                  //  if (Convert.ToInt32(reader["IncarcaturaGermeni"])!=0)
                    {
                        totntg += Convert.ToInt32(reader["IncarcaturaGermeni"]);
                        nrntg++;
                    }

                } while (reader.Read());
            }
            reader.Close();
            cnn.Close();
        
        return list;
    }
}
