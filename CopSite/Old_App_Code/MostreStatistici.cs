using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for MostreStatistici
/// </summary>
public class MostreStatistici
{
    public int NrMostre { get; set; }

    public int Time { get; set; }


    public int Day { get; set; }

    public int Ncs { get; set; }

    public int Ntg { get; set; }


    public static List<MostreStatistici> GetTotalMostre(string fabricaid)
    {
        List<MostreStatistici> list = new List<MostreStatistici>();

        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        //    SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        for (int i = 1; i <= 12; i++)
        {
            cmd.CommandText = "SELECT COUNT(*) AS TotalMostre FROM MostreTancuri INNER JOIN FERME_CCL ON MostreTancuri.FermaID = Ferme_CCL.ID WHERE "
                              + " MONTH(MostreTancuri.DataTestareFinala) = " + i
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


    public static List<MostreStatistici> GetMostreLuna(string fabricaid, string luna)
    {
        List<MostreStatistici> list = new List<MostreStatistici>();

        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        //    SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = "SELECT DAY(MostreTancuri.DataTestare) AS Ziua, MostreTancuri.DataTestareFinala, MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni FROM MostreTancuri INNER JOIN FERME_CCL ON MostreTancuri.FermaID = Ferme_CCL.ID WHERE "
                          + " MONTH(MostreTancuri.DataTestareFinala) = " + luna
                          + " AND Ferme_CCL.FabricaID = " + fabricaid
                          + " ORDER BY DAY(DataTestareFinala) ASC";

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
                        ms.Ncs = totncs/nrncs;
                        ms.Ntg = totntg/nrntg;
                        list.Add(ms);
                    }
                    catch
                    {
                    }
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