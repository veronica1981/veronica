using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using NLog;
using NPOI.HSSF.Record.Formula.Functions;
using CopSite.Old_App_Code;

/// <summary>
/// Summary description for MostreFabrica
/// </summary>
public class MostreFabrica
{
    private static Logger logger = LogManager.GetCurrentClassLogger();

    #region fields

    #endregion

    #region props

    public string CodBare { get; set; }

    public string Rasa { get; set; }

    public string Crotalia { get; set; }

    public string NrComanda { get; set; }

    public string FermeId { get; set; }

    public string FermeNume { get; set; }

    public string PrelevatoriId { get; set; }

    public string PrelevatoriNume { get; set; }

    public string FabriciId { get; set; }

    public string FabriciNume { get; set; }

    public string FabriciStrada { get; set; }

    public string FabriciNumar { get; set; }

    public string FabriciOras { get; set; }

    public string FabriciJudet { get; set; }

    public DateTime DataPrimirii { get; set; }

    public DateTime DataPrelevare { get; set; }

    public DateTime DataTestare { get; set; }

    public DateTime DataTestareFinala { get; set; }

    public string Grasime { get; set; }

    public string Proteina { get; set; }

    public string Lactoza { get; set; }

    public string Substu { get; set; }

    public string Puncti { get; set; }

    public string Apa { get; set; }

    public string Antib { get; set; }

    public string NCS { get; set; }

    public string NTG { get; set; }

    public string Casein { get; set; }

    public string NumeProba { get; set; }

    public string NumePrelevator { get; set; }

    public string CodFerma { get; set; }

    public int Nrf { get; set; }

    public string StradaFerma { get; set; }

    public string NumarFerma { get; set; }

    public string OrasFerma { get; set; }

    public string JudetFerma { get; set; }

    public string PersContact { get; set; }

    public string TelContact { get; set; }

    public string Urea { get; set; }

    public string Ph { get; set; }

    public string Combi { get; set; }

    public string Cant { get; set; }

    public string IdZilnic { get; set; }

    public string Id { get; set; }

    public bool Definitiv { get; set; }

    public bool Validat { get; set; }

    public bool Sentsms { get; set; }

    #endregion

    public static List<MostreFabrica> GetMostreFabrica(string fermaid, DateTime datatestare)
    {
        SqlConnection cnn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Ferme_CCL.Ferme_CCL,Ferme_CCL.FermierID,Judete.Denloc,  "
                          + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
                          "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
                          "MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.Caseina,MostreTancuri.GrasimeProv, MostreTancuri.PrelevatorID," +
                          "MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
                          "FROM Ferme_CCL, Judete, MostreTancuri WHERE"
                          + " MostreTancuri.DataTestareFinala = CONVERT(date, '" + datatestare.ToShortDateString() +
                          "', 103) "
                          + " AND MostreTancuri.CodFerma = Ferme_CCL.Cod "
                          + " AND Convert(int,Ferme_CCL.Judet,2)=Judete.ID "
                          + " AND Ferme_CCL.ID = " + fermaid
                          + " AND MostreTancuri.Validat = 1   ORDER BY MostreTancuri.CodBare";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        int pos = 0;
        string ferma = "";
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
            mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);


            mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], "-");
            mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], "-");
            mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], "-");
            mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], "-");
            mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], "-");
            mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], "-");
            mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], "-");

            double ntg = 0;
            if (mf.NTG != "-" && (double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.CurrentCulture, out ntg) ||
                 double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.InvariantCulture, out ntg)))
            {
                if (ntg >= 10000) mf.NTG = ">10000";
            }

            mf.Apa = "-";
            if (!mf.Puncti.Equals("-"))
            {
                mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                mf.Puncti = "-0." + mf.Puncti;
            }
            mf.Antib = Convert.ToString(reader["Antibiotice"]);
            if (mf.Antib != "")
            {
                if (mf.Antib == "0")
                    mf.Antib = "Negativ";
                else
                    mf.Antib = "Pozitiv";
            }
            else
                mf.Antib = "-";
            mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], "-");
            mf.Ph = Utils.NormalizeForDisplay(reader["PH"], "-");
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], "-");
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            //get prelevator name


            mf.PrelevatoriNume = Convert.ToString(reader["PersonaDeContact"]);
            if (!mf.PrelevatoriId.Equals(mf.CodBare.Substring(0, 5)))
            {
                SqlConnection cnnf = new SqlConnection(
                    ConfigurationManager.ConnectionStrings
                        ["fccl2ConnectionString"].ConnectionString);
                SqlCommand cmdf = new SqlCommand();
                cmdf.Connection = cnnf;
                cmdf.CommandText = "SELECT * FROM Prelevatori WHERE CodPrelevator =" + mf.PrelevatoriId;
                cnnf.Open();
                SqlDataReader drFerme = cmdf.ExecuteReader();
                if (drFerme.Read())
                {
                    mf.PrelevatoriNume = Convert.ToString(drFerme["NumePrelevator"]);
                }
                drFerme.Close();
                cnnf.Close();
            }

            // 
            mf.FermeNume = Convert.ToString(reader["Nume"]);
            mf.CodFerma = Convert.ToString(reader["Cod"]);
            if (!ferma.Equals(mf.FermeNume))
            {
                pos++;
            }
            mf.Nrf = pos;
            ferma = mf.FermeNume;

            mf.StradaFerma = Convert.ToString(reader["Strada"]);
            mf.NumarFerma = Convert.ToString(reader["Numar"]);
            mf.OrasFerma = Convert.ToString(reader["Oras"]);
            mf.JudetFerma = Convert.ToString(reader["Denloc"]);
            mf.PersContact = Convert.ToString(reader["PersonaDeContact"]);
            mf.TelContact = Convert.ToString(reader["TelPersoanaContact"]);
            string fermeccl = Convert.ToString(reader["Ferme_CCL"]);
            string fermierid = Convert.ToString(reader["FermierID"]);
            /*          
		   if (!fermeccl.Equals("C") && fermierid !="" && fermierid !=null)
			 {
				 SqlConnection cnnf = new SqlConnection(
	   ConfigurationManager.ConnectionStrings
	   ["fccl2ConnectionString"].ConnectionString);
				 SqlCommand cmdf = new SqlCommand();
				 cmdf.Connection = cnnf;
				 cmdf.CommandText = "SELECT * FROM Fermier WHERE Fermier.FermierID =" + fermierid;
				 cnnf.Open();
				 SqlDataReader drFerme = cmdf.ExecuteReader();
				 if (drFerme.Read())
				 {
					 mf.PersContact = Convert.ToString(drFerme["Nume"]);
					 mf.TelContact = Convert.ToString(drFerme["Telefon"]);
				 }
				 drFerme.Close();
				 cnnf.Close();
			 }    
 */


            list.Add(mf);
        }
        cnn.Close();
        return list;
    }

    //get mostra
    public static List<MostreFabrica> GetMostra(string id)
    {
        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings
                ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT * FROM MostreTancuri WHERE " +
            //"CodBare ='" + codbare + "'";
                          "ID =" + id;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();

        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.Id = Convert.ToString(reader["ID"]);
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.Rasa = Convert.ToString(reader["Rasa"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
            mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);

            mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], string.Empty);
            mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], string.Empty);
            mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], string.Empty);
            mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], string.Empty);
            mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], string.Empty);
            mf.Ph = Utils.NormalizeForDisplay(reader["PH"], string.Empty);
            mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], string.Empty);
            mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], string.Empty);
            mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], string.Empty);
            mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], string.Empty);

            mf.Apa = "-";
            if (!mf.Puncti.Equals(""))
            {
                mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                mf.Puncti = "-0." + mf.Puncti;
            }
            mf.Antib = Convert.ToString(reader["Antibiotice"]);
            if (mf.Antib == "-1")
            {
                mf.Antib = "1";
            }
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            mf.PrelevatoriNume = Convert.ToString(reader["NumePrelevator"]);
            mf.Cant = Convert.ToString(reader["CantitateLaPrelevare"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.NumeProba = Convert.ToString(reader["NumeProba"]);
            mf.IdZilnic = Convert.ToString(reader["IdZilnic"]);
            mf.Definitiv = Convert.ToBoolean(reader["Definitiv"]);
            mf.Validat = Convert.ToBoolean(reader["Validat"]);
            mf.Sentsms = Convert.ToBoolean(reader["SentSms"]);
            mf.FabriciNume = Convert.ToString(reader["NumeClient"]);
            mf.FabriciOras = Convert.ToString(reader["Localitate"]);
            mf.FabriciJudet = Convert.ToString(reader["Judet"]);
            mf.FabriciStrada = Convert.ToString(reader["AdresaClient"]);
            list.Add(mf);
        }
        cnn.Close();
        return list;
    }

    //get mostra
    public static MostreFabrica GetMostraByCodBare(string codbare)
    {
        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings
                ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT * FROM MostreTancuri WHERE " +
            "CodBare ='" + codbare + "'";
                          //"ID =" + id;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();

        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.Id = Convert.ToString(reader["ID"]);
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.Rasa = Convert.ToString(reader["Rasa"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
            mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);

            mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], string.Empty);
            mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], string.Empty);
            mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], string.Empty);
            mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], string.Empty);
            mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], string.Empty);
            mf.Ph = Utils.NormalizeForDisplay(reader["PH"], string.Empty);
            mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], string.Empty);
            mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], string.Empty);
            mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], string.Empty);
            mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], string.Empty);

            mf.Apa = "-";
            if (!mf.Puncti.Equals(""))
            {
                mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                mf.Puncti = "-0." + mf.Puncti;
            }
            mf.Antib = Convert.ToString(reader["Antibiotice"]);
            if (mf.Antib == "-1")
            {
                mf.Antib = "1";
            }
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            mf.PrelevatoriNume = Convert.ToString(reader["NumePrelevator"]);
            mf.Cant = Convert.ToString(reader["CantitateLaPrelevare"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.NumeProba = Convert.ToString(reader["NumeProba"]);
            mf.IdZilnic = Convert.ToString(reader["IdZilnic"]);
            mf.Definitiv = Convert.ToBoolean(reader["Definitiv"]);
            mf.Validat = Convert.ToBoolean(reader["Validat"]);
            mf.Sentsms = Convert.ToBoolean(reader["SentSms"]);
            mf.FabriciNume = Convert.ToString(reader["NumeClient"]);
            mf.FabriciOras = Convert.ToString(reader["Localitate"]);
            mf.FabriciJudet = Convert.ToString(reader["Judet"]);
            mf.FabriciStrada = Convert.ToString(reader["AdresaClient"]);
            list.Add(mf);
        }
        cnn.Close();
        return list.FirstOrDefault();
    }

    //
    public static List<MostreFabrica> GetMostreFCB(string client, string datatestare)
    {
        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings
                ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
                          "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
                          "MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni, MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.Caseina,MostreTancuri.GrasimeProv,MostreTancuri.PrelevatorID," +
                          "MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda,MostreTancuri.NumeProba, MostreTancuri.NumePrelevator, MostreTancuri.NumeClient,MostreTancuri.AdresaClient,MostreTancuri.Localitate,MostreTancuri.Judet " +
                          "FROM  MostreTancuri WHERE"
                          + " MostreTancuri.DataTestareFinala = CONVERT(date, '" +
                          datatestare + "', 103) "
                          + " AND MostreTancuri.NumeClient = '" + client
                          + "' AND MostreTancuri.Validat = 1  ORDER BY MostreTancuri.CodBare";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
            mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);

            mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], "-");
            mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], "-");
            mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], "-");
            mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], "-");
            mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], "-");
            mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], "-");
            mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], "-");

            double ntg = 0;
            if (mf.NTG != "-" && (double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.CurrentCulture, out ntg) ||
                 double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.InvariantCulture, out ntg)))
            {
                if (ntg >= 10000) mf.NTG = ">10000";
            }

            mf.Apa = "-";
            if (!mf.Puncti.Equals("-"))
            {
                mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                mf.Puncti = "-0." + mf.Puncti;
            }
            mf.Antib = Convert.ToString(reader["Antibiotice"]);
            if (mf.Antib != "")
            {
                if (mf.Antib == "0")
                    mf.Antib = "Negativ";
                else
                    mf.Antib = "Pozitiv";
            }
            else
                mf.Antib = "-";

            mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], "-");
            mf.Ph = Utils.NormalizeForDisplay(reader["PH"], "-");
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], "-");

            mf.NumeProba = Convert.ToString(reader["NumeProba"]);
            mf.NumePrelevator = Convert.ToString(reader["NumePrelevator"]);

            mf.FabriciStrada = Convert.ToString(reader["AdresaClient"]);
            mf.FabriciOras = Convert.ToString(reader["Localitate"]);
            mf.FabriciJudet = Convert.ToString(reader["Judet"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);


            list.Add(mf);
        }
        cnn.Close();
        return list;
    }


    public static List<MostreFabrica> GetMostreFerma(string fermaid, DateTime datatestare)
    {
        List<MostreFabrica> list = new List<MostreFabrica>();
        try
        {
            logger.Info("GetMostreFerma|fermaid:{0}|datatestare:{1}", fermaid, datatestare);
            SqlConnection cnn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            StringBuilder cmdText =
                new StringBuilder(
                    "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.PersonaDeContact, MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare,");
            cmdText.Append(
                "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice,");
            cmdText.Append(
                "MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni, MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.GrasimeProv,MostreTancuri.Caseina, MostreTancuri.PrelevatorID,");
            cmdText.AppendLine(
                "MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda,MostreTancuri.CodFerma,MostreTancuri.NumePrelevator ");
            cmdText.AppendLine("FROM Ferme_CCL, MostreTancuri ");
            cmdText.Append("WHERE MostreTancuri.DataTestareFinala = CONVERT(date, '" + datatestare.ToShortDateString() + "', 103) ");
            cmdText.Append(" AND Ferme_CCL.ID = " + fermaid);
            cmdText.Append(" AND Ferme_CCL.ID = MostreTancuri.FermaID ");
            cmdText.Append(" AND MostreTancuri.Validat = 1  ORDER BY Ferme_CCL.Nume");
            cmd.CommandText = cmdText.ToString();
            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            logger.Info("GetMostreFerma|executing:{0}", cmdText);
            while (reader.Read())
            {
                MostreFabrica mf = new MostreFabrica();
                mf.CodBare = Convert.ToString(reader["CodBare"]);
                mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
                mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
                mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
                mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);

                mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], "-");
                mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], "-");
                mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], "-");
                mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], "-");
                mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], "-");
                mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], "-");
                mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], "-");

                double ntg = 0;
                if (mf.NTG != "-" && (double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.CurrentCulture, out ntg) ||
                     double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.InvariantCulture, out ntg)))
                {
                    if (ntg >= 10000) mf.NTG = ">10000";
                }

                mf.Apa = "-";
                if (!mf.Puncti.Equals("-"))
                {
                    mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                    mf.Puncti = "-0." + mf.Puncti;
                }
                mf.Antib = Convert.ToString(reader["Antibiotice"]);
                if (mf.Antib != "")
                {
                    if (mf.Antib == "0")
                        mf.Antib = "Negativ";
                    else
                        mf.Antib = "Pozitiv";
                }
                else
                    mf.Antib = "-";
                //
                mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], "-");
                mf.Ph = Utils.NormalizeForDisplay(reader["PH"], "-");
                mf.Combi = Convert.ToString(reader["GrasimeProv"]);
                mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
                mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], "-");

                mf.CodFerma = Convert.ToString(reader["CodFerma"]);
                //     mf.PrelevatoriNume = Convert.ToString(reader["NumePrelevator"]);
                //get prelevator name
                mf.NrComanda = Convert.ToString(reader["NrComanda"]);

                if (mf.PrelevatoriId.Equals("0"))
                {
                    mf.PrelevatoriNume = Convert.ToString(reader["NumePrelevator"]);
                }
                else
                {
                    mf.PrelevatoriNume = Convert.ToString(reader["PersonaDeContact"]);
                }

                //
                mf.FermeNume = Convert.ToString(reader["Nume"]);

                list.Add(mf);
            }
            reader.Close();
            cnn.Close();

            SqlConnection cnnf =
                new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
            SqlCommand cmdf = new SqlCommand();
            cmdf.Connection = cnnf;
            string prelevatoriIds = "(" +
                                    string.Join(",",
                                        list.Where(
                                            mf =>
                                                !mf.PrelevatoriId.Equals(mf.CodBare.Substring(0, 5)) &&
                                                !mf.PrelevatoriId.Equals("0") &&
                                                !string.IsNullOrWhiteSpace(mf.PrelevatoriId))
                                            .Select(mf => mf.PrelevatoriId)) + ")";

            logger.Info(string.Format("GetMostreFerma|prelevatoriIds:{0}", prelevatoriIds));
            int cnt = 0;
            if (prelevatoriIds.Length > 2)
            {
                cmdf.CommandText = "SELECT ID, NumePrelevator FROM Prelevatori WHERE CodPrelevator IN " + prelevatoriIds;
                cnnf.Open();
                SqlDataReader drFerme = cmdf.ExecuteReader();
                while (drFerme.Read())
                {
                    string prelevatorId = Convert.ToString(drFerme["ID"]);
                    foreach (var mostra in list.Where(mf => mf.PrelevatoriId == prelevatorId))
                    {
                        mostra.PrelevatoriNume = Convert.ToString(drFerme["NumePrelevator"]);
                    }
                    cnt++;
                }
                drFerme.Close();
                cnnf.Close();
            }
            logger.Info(string.Format("GetMostreFerma|PrelevatoriNume cnt:{0}", cnt));
            string barcodes = "('" +
                              string.Join("','",
                                  list.Where(
                                      mf => !string.IsNullOrWhiteSpace(mf.CodBare) && mf.CodBare.Trim().Length > 7)
                                      .Select(mf => mf.CodBare.Trim().Substring(0, 7))) + "')";
            logger.Info(string.Format("GetMostreFerma|CodBare:{0}", barcodes));
            cnt = 0;
            if (barcodes.Length > 4)
            {
                // get crotalia : codbare = nr.matricol and fermaid = fermaid
                cmdf.CommandText = "SELECT CodBare, Crotalia FROM Crotalii where CodBare IN " + barcodes;
                cnnf.Open();
                SqlDataReader drCrot = cmdf.ExecuteReader();
                while (drCrot.Read())
                {
                    string barcode = Convert.ToString(drCrot["CodBare"]);
                    foreach (
                        var mostra in
                            list.Where(mf => !string.IsNullOrWhiteSpace(mf.CodBare) && mf.CodBare.StartsWith(barcode)))
                    {
                        mostra.Crotalia = Convert.ToString(drCrot["Crotalia"]);
                        cnt++;
                    }
                }
                drCrot.Close();
                cnnf.Close();
            }
            logger.Info(string.Format("GetMostreFerma|Crotalia cnt:{0}", cnt));

            logger.Info(string.Format("GetMostreFerma|Crotalia by code & ferma cnt:{0}",
                list.Count(mf => string.IsNullOrWhiteSpace(mf.Crotalia))));
            foreach (var mf in list.Where(mf => string.IsNullOrWhiteSpace(mf.Crotalia)))
            {
                cmdf.CommandText = "SELECT * FROM Crotalii where CodBare = '" + mf.CodFerma + "' AND FermaId = " +
                                   fermaid;
                cnnf.Open();
                SqlDataReader drCrot = cmdf.ExecuteReader();
                if (drCrot.Read())
                    mf.Crotalia = Convert.ToString(drCrot["Crotalia"]);
                else
                    mf.Crotalia = " ";
                if (mf.Crotalia.Length > 0 && mf.Crotalia.IndexOf("_") > 0)
                    mf.Crotalia = mf.Crotalia.Substring(mf.Crotalia.IndexOf("_") + 1);
                if (mf.Crotalia.Trim().Length == 0)
                    mf.Crotalia = mf.CodFerma;
                logger.Info("GetMostreFerma|Crotalia:{0}|codFerma:{1}|fermaid:{2}", mf.Crotalia, mf.CodFerma, fermaid);
                drCrot.Close();
                cnnf.Close();
            }

            list.Sort(delegate(MostreFabrica a, MostreFabrica b) { return a.Crotalia.CompareTo(b.Crotalia); });
        }
        catch (Exception ex)
        {
            logger.Fatal("GetMostreFerma|fermaid:{0}|datatestare:{1}|msg:{2}|stack:{3}", fermaid, datatestare,
                ex.Message, ex.StackTrace);
        }
        return list;
    }

    public static List<MostreFabrica> GetMostreFabrica
        (string fabricaid, string datatestare, string datatestare2)
    {
        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings
                ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        string query_date = " MostreTancuri.DataTestareFinala = CONVERT(date, '" + datatestare + "', 103) ";
        if (datatestare2 != "")
        {
            query_date = " (MostreTancuri.DataTestareFinala >= CONVERT(date, '" + datatestare + "', 103))";

            query_date += " AND MostreTancuri.DataTestareFinala <= CONVERT(date, '" + datatestare2 + "', 103))";
        }


        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Judete.Denloc,  "
                          + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
                          "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
                          "MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.GrasimeProv,MostreTancuri.Caseina, MostreTancuri.PrelevatorID," +
                          "MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
                          "FROM Ferme_CCL, Judete, MostreTancuri WHERE"
                          + query_date
                          + " AND MostreTancuri.CodFerma = Ferme_CCL.Cod "
                          + " AND Convert(int,Ferme_CCL.Judet,2)=Judete.ID "
                          + " AND Ferme_CCL.FabricaID = " + fabricaid
                          + " AND MostreTancuri.Validat = 1  ORDER BY Ferme_CCL.Nume";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        int pos = 0;
        string ferma = "";
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
            mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);

            mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], "-");
            mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], "-");
            mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], "-");
            mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], "-");
            mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], "-");
            mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], "-");
            mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], "-");

            double ntg = 0;
            if (mf.NTG != "-" && (double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.CurrentCulture, out ntg) ||
                 double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.InvariantCulture, out ntg)))
            {
                if (ntg >= 10000) mf.NTG = ">10000";
            }

            //                mf.NTG = (Convert.ToString(reader["IncarcaturaGermeni"]) == "0") ? "" : Convert.ToString(reader["IncarcaturaGermeni"]);
            mf.Apa = "-";
            if (!mf.Puncti.Equals("-"))
            {
                mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                mf.Puncti = "-0." + mf.Puncti;
            }
            mf.Antib = Convert.ToString(reader["Antibiotice"]);
            if (mf.Antib != "")
            {
                if (mf.Antib == "0")
                    mf.Antib = "Negativ";
                else
                    mf.Antib = "Pozitiv";
            }
            else
                mf.Antib = "-";

            mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], "-");
            mf.Ph = Utils.NormalizeForDisplay(reader["PH"], "-");
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], "-");
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            //    mf.PrelevatoriNume = Convert.ToString(reader["NumePrelevator"]);
            //get prelevator name

            mf.PrelevatoriNume = Convert.ToString(reader["PersonaDeContact"]);
            if (!mf.PrelevatoriId.Equals(mf.CodBare.Substring(0, 5)))
            {
                SqlConnection cnnf = new SqlConnection(
                    ConfigurationManager.ConnectionStrings
                        ["fccl2ConnectionString"].ConnectionString);
                SqlCommand cmdf = new SqlCommand();
                cmdf.Connection = cnnf;
                cmdf.CommandText = "SELECT * FROM Prelevatori WHERE CodPrelevator =" + mf.PrelevatoriId;
                cnnf.Open();
                SqlDataReader drFerme = cmdf.ExecuteReader();
                if (drFerme.Read())
                {
                    mf.PrelevatoriNume = Convert.ToString(drFerme["NumePrelevator"]);
                }
                drFerme.Close();
                cnnf.Close();
            }


            mf.FermeNume = Convert.ToString(reader["Nume"]);
            mf.CodFerma = Convert.ToString(reader["Cod"]);
            if (!ferma.Equals(mf.FermeNume))
            {
                pos++;
            }
            mf.Nrf = pos;
            ferma = mf.FermeNume;

            mf.StradaFerma = Convert.ToString(reader["Strada"]);
            mf.NumarFerma = Convert.ToString(reader["Numar"]);
            mf.OrasFerma = Convert.ToString(reader["Oras"]);
            mf.JudetFerma = Convert.ToString(reader["Denloc"]);
            mf.PersContact = Convert.ToString(reader["PersonaDeContact"]);
            mf.TelContact = Convert.ToString(reader["TelPersoanaContact"]);

            list.Add(mf);
        }
        cnn.Close();
        return list;
    }

    //getmostresms
    public static List<MostreFabrica> GetMostreSMS(DateTime datatestare)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText =
            "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.Cod,Ferme_CCL.TelPersoanaContact, MostreTancuri.NrComanda, MostreTancuri.DataTestare " +
            " FROM Ferme_CCL,  MostreTancuri WHERE Ferme_CCL.Id =MostreTancuri.FermaId AND " +
            "MostreTancuri.DataTestareFinala = CONVERT(date, '" + datatestare.ToShortDateString() + "',103) " +
            "AND MostreTancuri.Validat = 1 " +
            " AND MostreTancuri.SentSms=0 " +
            " AND Ferme_CCL.SendSms=1" +
            "GROUP BY Ferme_CCl.ID, ferme_CCL.Nume, Ferme_CCL.Cod,Ferme_CCL.TelPersoanaContact,MostreTancuri.NrComanda,MostreTancuri.DataTestare " +
            "ORDER BY Ferme_CCL.Nume";


        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        int pos = 0;
        string ferma = "";
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);


            mf.FermeNume = Convert.ToString(reader["Nume"]);
            mf.CodFerma = Convert.ToString(reader["Cod"]);
            SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmu =
                new SqlCommand(
                    "select u.UserName  from UsersInformation i join aspnet_Users u on i.UserId = u.UserId where i.UserCod ='" +
                    mf.CodFerma + "'", connection);
            //new SqlCommand("select * from UsersInformation where UserCod='" + mf.CodFerma + "'", connection);
            connection.Open();
            using (SqlDataReader rdr = cmu.ExecuteReader())
            {
                if (rdr.Read())
                {
                    mf.FermeNume = rdr["UserName"].ToString().Trim();
                }
                rdr.Close();
            }


            mf.FermeId = Convert.ToString(reader["ID"]);
            if (!ferma.Equals(mf.FermeNume))
            {
                pos++;
            }
            mf.Nrf = pos;
            ferma = mf.FermeNume;

            mf.TelContact = Convert.ToString(reader["TelPersoanaContact"]);

            list.Add(mf);
        }
        cnn.Close();
        return list;
    }

    //
    //getmostresms
    public static List<MostreFabrica> GetMostreSMSTest(DateTime datatestare)
    {
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Ferme_CCL.Ferme_CCL,Ferme_CCL.FermierID,Ferme_CCL.Telefon,  "
                          + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
                          "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
                          "MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.Caseina,MostreTancuri.GrasimeProv, MostreTancuri.PrelevatorID," +
                          "MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
                          "FROM Ferme_CCL, MostreTancuri WHERE"
                          + " MostreTancuri.DataTestareFinala = CONVERT(date, '" + datatestare.ToShortDateString() + "', 103) "
            //+ " AND MostreTancuri.CodFerma is not null"
                          + " AND MostreTancuri.CodFerma = Ferme_CCL.Cod "
            //+ " AND MostreTancuri.SentSms=0"
            //TODO: uncomment !!!
                          + " AND Ferme_CCL.SendSms=1"
                          + " AND MostreTancuri.Validat = 1   ORDER BY Ferme_CCL.Nume";

        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        int pos = 0;
        string ferma = "";
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.DataTestare = Convert.ToDateTime(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToDateTime(reader["DataTestareFinala"]);
            mf.DataPrimirii = Convert.ToDateTime(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToDateTime(reader["DataPrelevare"]);

            mf.Grasime = Utils.NormalizeForDisplay(reader["Grasime"], "-");
            mf.Proteina = Utils.NormalizeForDisplay(reader["ProcentProteine"], "-");
            mf.Lactoza = Utils.NormalizeForDisplay(reader["ProcentLactoza"], "-");
            mf.Substu = Utils.NormalizeForDisplay(reader["SubstantaUscata"], "-");
            mf.NCS = Utils.NormalizeForDisplay(reader["NumarCeluleSomatice"], "-");
            mf.NTG = Utils.NormalizeForDisplay(reader["IncarcaturaGermeni"], "-");
            mf.Puncti = Utils.NormalizeForDisplay(reader["PunctInghet"], "-");

            double ntg = 0;
            if (mf.NTG != "-" && (double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.CurrentCulture, out ntg) ||
                 double.TryParse(Convert.ToString(ntg), NumberStyles.Any, CultureInfo.InvariantCulture, out ntg)))
            {
                if (ntg >= 10000) mf.NTG = ">10000";
            }
            mf.Apa = "-";
            if (!mf.Puncti.Equals("-"))
            {
                mf.Apa = UMostre.ApaAdaugata(mf.Puncti);
                mf.Puncti = "-0." + mf.Puncti;
            }
            mf.Antib = Convert.ToString(reader["Antibiotice"]);
            if (mf.Antib != "")
            {
                if (mf.Antib == "0")
                    mf.Antib = "N";
                else
                    mf.Antib = "P";
            }
            else
                mf.Antib = "-";

            mf.Urea = Utils.NormalizeForDisplay(reader["Urea"], "-");
            mf.Ph = Utils.NormalizeForDisplay(reader["PH"], "-");
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = Utils.NormalizeForDisplay(reader["Caseina"], "-");
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            //get prelevator name

            mf.PrelevatoriNume = Convert.ToString(reader["PersonaDeContact"]);

            mf.FermeNume = Convert.ToString(reader["Nume"]);
            mf.CodFerma = Convert.ToString(reader["Cod"]);
            //
            SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmu =
                new SqlCommand(
                    "select u.UserName  from UsersInformation i join aspnet_Users u on i.UserId = u.UserId where i.UserCod ='" +
                    mf.CodFerma + "'");
            //new SqlCommand("select * from UsersInformation where UserCod='" + mf.CodFerma + "'", connection);
            connection.Open();
            using (SqlDataReader rdr = cmu.ExecuteReader())
            {
                if (rdr.Read())
                {
                    mf.FermeNume = rdr["UserName"].ToString().Trim();
                }
                rdr.Close();
            }


            //

            if (!ferma.Equals(mf.FermeNume))
            {
                pos++;
            }
            mf.Nrf = pos;
            ferma = mf.FermeNume;

            mf.StradaFerma = Convert.ToString(reader["Strada"]);
            mf.NumarFerma = Convert.ToString(reader["Numar"]);
            mf.OrasFerma = Convert.ToString(reader["Oras"]);

            mf.PersContact = Convert.ToString(reader["PersonaDeContact"]);
            mf.TelContact = Convert.ToString(reader["TelPersoanaContact"]);
            string fermeccl = Convert.ToString(reader["Ferme_CCL"]);
            string fermierid = Convert.ToString(reader["FermierID"]);

            list.Add(mf);
        }
        cnn.Close();
        return list;
    }


    /* public void InsertMostra(string CodBare,string IdZilnic, string Cant,string PrelevatoriId,
		 string DataPrimirii, string DataPrelevare, string DataTestare, string DataTestareFinala,
		 string Grasime, string Proteina, string Lactoza, string Substu, string Punti,string Antib,
		 string NCS, string NTG, string Urea, string Ph, string NrComanda,
		 string PrelevatoriNume, string NumeProba, string FabriciNume, string FabriciStrada,
		 string FabriciOras, string FabriciJudet, bool Definitiv, bool Validat)*/

    public void InsertMostra(MostreFabrica mf)
    {
        string grasime = mf.Grasime.Trim().Equals("")
            ? "0"
            : mf.Grasime.Trim().Equals("0") ? "0.00001" : mf.Grasime.Trim();
        string proteina = mf.Proteina.Trim().Equals("")
            ? "0"
            : mf.Proteina.Trim().Equals("0") ? "0.00001" : mf.Proteina.Trim();
        string lactoza = mf.Lactoza.Trim().Equals("")
            ? "0"
            : mf.Lactoza.Trim().Equals("0") ? "0.00001" : mf.Lactoza.Trim();
        string caseina = mf.Casein.Trim().Equals("") ? "0" : mf.Casein.Trim().Equals("0") ? "0.00001" : mf.Casein.Trim();
        string substu = mf.Substu.Trim().Equals("") ? "0" : mf.Substu.Trim().Equals("0") ? "0.00001" : mf.Substu.Trim();
        string pcti = "0.00001";
        //
        string antib = "";
        // 
        string ncs = mf.NCS.Trim().Equals("") ? "0" : mf.NCS.Trim().Equals("0") ? "0.00001" : mf.NCS.Trim();
        string ntg = "0.00001";
        string rasa = mf.Rasa;
        string urea = mf.Urea.Trim().Equals("") ? "0" : mf.Urea.Trim().Equals("0") ? "0.00001" : mf.Urea.Trim();
        string ph = mf.Ph.Trim().Equals("") ? "0" : mf.Ph.Trim().Equals("0") ? "0.00001" : mf.Ph.Trim();
        string codferma = mf.CodBare.Substring(0, 5);
        string definitiv = mf.Definitiv ? "1" : "0";
        string validat = mf.Validat ? "1" : "0";
        string sentsms = mf.Sentsms ? "1" : "0";
        string cant = "0";
        string prelid = mf.PrelevatoriId.Trim().Equals("") ? "0" : mf.PrelevatoriId.Trim();
        SqlConnection cnn = new SqlConnection(
            ConfigurationManager.ConnectionStrings
                ["fccl2ConnectionString"].ConnectionString);

        string query =
            "INSERT INTO MostreTancuri (CodBare,IdZilnic,CantitateLaPrelevare,PrelevatorID,DataPrimirii,DataPrelevare,DataTestare,DataTestarefinala," +
            "Grasime, ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PunctInghet,NumarCeluleSomatice,IncarcaturaGermeni,Antibiotice," +
            "Urea,PH,NrComanda,NumePrelevator,NumeProba,NumeClient,AdresaClient,Localitate,Judet,Definitiv,Validat,Sentsms,CodFerma,GrasimeProv,LactozaProv,SubstuProv,ProteineProv,Rasa) VALUES(" +
            "'" + mf.CodBare + "'," + mf.IdZilnic + "," + cant + "," + prelid + ",CONVERT(date,'" + mf.DataPrimirii.ToShortDateString() +
            "',103)," + "CONVERT(date,'" + mf.DataPrelevare.ToShortDateString() + "',103), CONVERT(date,'" + mf.DataTestare.ToShortDateString() +
            "',103), CONVERT(date,'" + mf.DataTestareFinala.ToShortDateString() + "',103)," +
            grasime + "," + proteina + "," + caseina + "," + lactoza + "," + substu + "," + pcti + "," + ncs + "," + ntg +
            ",'" + antib + "'," + urea + "," + ph + ",'" + mf.NrComanda + "','" + mf.PrelevatoriNume + "','" +
            mf.NumeProba + "','" + mf.FabriciNume + "','" +
            mf.FabriciStrada + "','" + mf.FabriciOras + "','" + mf.FabriciJudet + "'," + definitiv + "," + validat + "," +
            sentsms + ",'" + codferma + "','" + rasa + "',0,0,0,0)";


        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }

    public void UpdateMostra(MostreFabrica mf)
    {
        MostreFabrica mostraOriginala = GetMostraByCodBare(mf.CodBare);
        if (mostraOriginala == null) return;

        string grasime = (mf.Grasime != null && mf.Grasime.Equals(mostraOriginala.Grasime)) ? "" : ",Grasime=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Grasime));
        string proteina = (mf.Proteina != null && mf.Proteina.Equals(mostraOriginala.Proteina)) ? "" : ",ProcentProteine=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Proteina));
        string lactoza = (mf.Lactoza != null && mf.Lactoza.Equals(mostraOriginala.Lactoza)) ? "" : ",ProcentLactoza=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Lactoza));
        string caseina = (mf.Casein != null && mf.Casein.Equals(mostraOriginala.Casein)) ? "" : ",Caseina=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Casein));
        string substu = (mf.Substu != null && mf.Substu.Equals(mostraOriginala.Substu)) ? "" : ",SubstantaUscata=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Substu));
        string pcti = mf.Validat ? "0.00001" : Utils.NomalizeForStorage(mf.Puncti);
        string ncs = (mf.NCS != null && mf.NCS.Equals(mostraOriginala.NCS)) ? "" : ",NumarCeluleSomatice=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.NCS));
        string ntg = mf.Validat ? "0.00001" : Utils.NomalizeForStorage(mf.NTG);
        string rasa = mf.Rasa;
        string urea = (mf.Urea != null && mf.Urea.Equals(mostraOriginala.Urea)) ? "" : ",Urea=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Urea));
        string ph = (mf.Ph != null && mf.Ph.Equals(mostraOriginala.Ph)) ? "" : ",PH=" + ReformatNumberForSql(Utils.NomalizeForStorage(mf.Ph));
        string codferma = mf.CodBare.Substring(0, 5);
        string definitiv = mf.Definitiv ? "1" : "0";
        string validat = mf.Validat ? "1" : "0";
        string sentsms = mf.Sentsms ? "1" : "0";
        //string cant = mf.Validat ? ",CantitateLaPrelevare=0" : string.Empty;
        string prelid = mf.PrelevatoriId.Trim().Equals("") ? "0" : mf.PrelevatoriId.Trim();
        SqlConnection cnn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);

        string query = "UPDATE MostreTancuri SET IdZilnic=" + ReformatNumberForSql(mf.IdZilnic) +// cant +
                       ",PrelevatorID=" + ReformatNumberForSql(prelid) + ",DataPrimirii=CONVERT(date,'" + mf.DataPrimirii.ToShortDateString() + "',103)," +
                       "DataPrelevare=CONVERT(date,'" + mf.DataPrelevare.ToShortDateString() + "',103),DataTestare=CONVERT(date,'" + mf.DataTestare.ToShortDateString() +
                       "',103),DataTestareFinala=CONVERT(date,'" + mf.DataTestareFinala.ToShortDateString() + "',103)" +
                       grasime + proteina + caseina + lactoza + substu + ",PunctInghet=" +
                       ReformatNumberForSql(pcti) + ncs + ",IncarcaturaGermeni=" + ReformatNumberForSql(ntg) +
                       urea + ph + ",NrComanda=" + ReformatStringForSql(mf.NrComanda) + ",NumePrelevator=" +
                       ReformatStringForSql(mf.NumePrelevator) + ",NumeProba=" + ReformatStringForSql(mf.NumeProba) + ",NumeClient=" + ReformatStringForSql(mf.FabriciNume) +
                       ",AdresaClient=" + ReformatStringForSql(mf.FabriciStrada) + ",Localitate=" +
                       ReformatStringForSql(mf.FabriciOras) + ",Judet=" + ReformatStringForSql(mf.FabriciJudet) + ",Definitiv=" + ReformatNumberForSql(definitiv) + ",Validat=" +
                       ReformatNumberForSql(validat) + ",Sentsms=" + ReformatNumberForSql(sentsms) + ",CodFerma=" + ReformatStringForSql(codferma) + ",Rasa=" +
                       ReformatStringForSql(rasa) + " WHERE CodBare=" + ReformatStringForSql(mf.CodBare);

        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }

    private string ReformatStringForSql(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "null";
        }
        return string.Format("'{0}'", value);
    }

    private string ReformatNumberForSql(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "null";
        }
        return value;
    }

    public void DeleteMostra(MostreFabrica mf)
    {
        SqlConnection cnn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        string query = "DELETE FROM MostreTancuri WHERE " + "ID=" + mf.Id;
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }
}