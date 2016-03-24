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
using System.Collections.Generic;
/// <summary>
/// Summary description for MostreFabrica
/// </summary>
public class MostreFabrica
{
    public MostreFabrica()
    {
        //
        // TODO: Add constructor logic here
        //
    }




    public string strFermeId;
    public string strFermeNume;

    public string strDataPrelevare;
    public string strDataPrimirii;
    public string strDataTestare;
    public string strDataTestareFinala;
    public string strCodBare;
    public string strGrasime;
    public string strProteina;
    public string strLactoza;
    public string strSubstu;
    public string strPuncti;
    public string strApa;
    public string strAntib;
    public string strNCS;
    public string strNTG;
    public string strUrea;
    public string strPh;
    public string strCombi;
    public string strCasein;
    public string strPrelevatoriId;
    public string strPrelevatoriNume;

    public string strFabriciId;
    public string strFabriciNume;
    public string strFabriciStrada;
    public string strFabriciNumar;
    public string strFabriciOras;
    public string strFabriciJudet;
    public string strNumeProba;
    public string strNumePrelevator;

    public string strCodFerma;
    public int iNrf;
    public string strStradaFerma;
    public string strNumarFerma;
    public string strOrasFerma;
    public string strJudetFerma;
    public string strPersContact;
    public string strTelContact;
    public string strNrComanda;
    public string strCant;
    public bool bDefinitiv;
    public bool bValidat;
    public bool bSentsms;
    public string strIdZilnic;
    public string strId;

    public string CodBare
    {
        get
        {
            return strCodBare;
        }
        set
        {
            strCodBare = value;
        }
    }

    public string NrComanda
    {
        get
        {
            return strNrComanda;
        }
        set
        {
            strNrComanda = value;
        }
    }

    public string FermeId
    {
        get
        {
            return strFermeId;
        }
        set
        {
            strFermeId = value;
        }
    }

    public string FermeNume
    {
        get
        {
            return strFermeNume;
        }
        set
        {
            strFermeNume = value;
        }
    }



    public string PrelevatoriId
    {
        get
        {
            return strPrelevatoriId;
        }
        set
        {
            strPrelevatoriId = value;
        }
    }

    public string PrelevatoriNume
    {
        get
        {
            return strPrelevatoriNume;
        }
        set
        {
            strPrelevatoriNume = value;
        }
    }

    public string FabriciId
    {
        get
        {
            return strFabriciId;
        }
        set
        {
            strFabriciId = value;
        }
    }

    public string FabriciNume
    {
        get
        {
            return strFabriciNume;
        }
        set
        {
            strFabriciNume = value;
        }
    }

    public string FabriciStrada
    {
        get
        {
            return strFabriciStrada;
        }
        set
        {
            strFabriciStrada = value;
        }
    }

    public string FabriciNumar
    {
        get
        {
            return strFabriciNumar;
        }
        set
        {
            strFabriciNumar = value;
        }
    }

    public string FabriciOras
    {
        get
        {
            return strFabriciOras;
        }
        set
        {
            strFabriciOras = value;
        }
    }

    public string FabriciJudet
    {
        get
        {
            return strFabriciJudet;
        }
        set
        {
            strFabriciJudet = value;
        }
    }

    public string DataPrimirii
    {
        get
        {
            return strDataPrimirii;
        }
        set
        {
            strDataPrimirii = value;
        }
    }

    public string DataPrelevare
    {
        get
        {
            return strDataPrelevare;
        }
        set
        {
            strDataPrelevare = value;
        }
    }

    public string DataTestare
    {
        get
        {
            return strDataTestare;
        }
        set
        {
            strDataTestare = value;
        }
    }

    public string DataTestareFinala
    {
        get
        {
            return strDataTestareFinala;
        }
        set
        {
            strDataTestareFinala = value;
        }
    }

    public string Grasime
    {
        get
        {
            return strGrasime;
        }
        set
        {
            strGrasime = value;
        }
    }

    public string Proteina
    {
        get
        {
            return strProteina;
        }
        set
        {
            strProteina = value;
        }
    }

    public string Lactoza
    {
        get
        {
            return strLactoza;
        }
        set
        {
            strLactoza = value;
        }
    }

    public string Substu
    {
        get
        {
            return strSubstu;
        }
        set
        {
            strSubstu = value;
        }
    }

    public string Puncti
    {
        get
        {
            return strPuncti;
        }
        set
        {
            strPuncti = value;
        }
    }

    public string Apa
    {
        get
        {
            return strApa;
        }
        set
        {
            strApa = value;
        }
    }

    public string Antib
    {
        get
        {
            return strAntib;
        }
        set
        {
            strAntib = value;
        }
    }

    public string NCS
    {
        get
        {
            return strNCS;
        }
        set
        {
            strNCS = value;
        }
    }

    public string NTG
    {
        get
        {
            return strNTG;
        }
        set
        {
            strNTG = value;
        }
    }

    public string Casein
    {
        get
        {
            return strCasein;
        }
        set
        {
            strCasein = value;
        }
    }

    public string NumeProba
    {
        get
        {
            return strNumeProba;
        }
        set
        {
            strNumeProba = value;
        }
    }

    public string NumePrelevator
    {
        get
        {
            return strNumePrelevator;
        }
        set
        {
            strNumePrelevator = value;
        }
    }

    public string CodFerma
    {
        get
        {
            return strCodFerma;
        }
        set
        {
            strCodFerma = value;
        }
    }


    public int Nrf
    {
        get
        {
            return iNrf;
        }
        set
        {
            iNrf = value;
        }
    }

    public string StradaFerma
    {
        get
        {
            return strStradaFerma;
        }
        set
        {
            strStradaFerma = value;
        }
    }

    public string NumarFerma
    {
        get
        {
            return strNumarFerma;
        }
        set
        {
            strNumarFerma = value;
        }
    }

    public string OrasFerma
    {
        get
        {
            return strOrasFerma;
        }
        set
        {
            strOrasFerma = value;
        }
    }

    public string JudetFerma
    {
        get
        {
            return strJudetFerma;
        }
        set
        {
            strJudetFerma = value;
        }
    }

    public string PersContact
    {
        get
        {
            return strPersContact;
        }
        set
        {
            strPersContact = value;
        }
    }

    public string TelContact
    {
        get
        {
            return strTelContact;
        }
        set
        {
            strTelContact = value;
        }
    }

    public string Urea
    {
        get
        {
            return strUrea;
        }
        set
        {
            strUrea = value;
        }
    }

    public string Ph
    {
        get
        {
            return strPh;
        }
        set
        {
            strPh = value;
        }
    }

    public string Combi
    {
        get
        {
            return strCombi;
        }
        set
        {
            strCombi = value;
        }
    }
    public string Cant
    {
        get
        {
            return strCant;
        }
        set
        {
            strCant = value;
        }
    }

    public string IdZilnic
    {
        get
        {
            return strIdZilnic;
        }
        set
        {
            strIdZilnic = value;
        }
    }

    public string Id
    {
        get
        {
            return strId;
        }
        set
        {
            strId = value;
        }
    }

    public bool Definitiv
    {
        get
        {
            return bDefinitiv;
        }
        set
        {
            bDefinitiv = value;
        }
    }

    public bool Validat
    {
        get
        {
            return bValidat;
        }
        set
        {
            bValidat = value;
        }
    }

    public bool Sentsms
    {
        get
        {
            return bSentsms;
        }
        set
        {
            bSentsms = value;
        }
    }


    public static List<MostreFabrica> GetMostreFabrica
    (string fabricaid, string datatestare)
    {
        SqlConnection cnn = new SqlConnection(
        ConfigurationManager.ConnectionStrings
        ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
cmd.CommandTimeout = 180;
        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Ferme_CCL.Ferme_CCL,Ferme_CCL.FermierID,Judete.Denloc,  "
      + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
            "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
"MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.Caseina,MostreTancuri.GrasimeProv, MostreTancuri.PrelevatorID," +
"MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
"FROM Ferme_CCL, Judete, MostreTancuri WHERE"
+ " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
+ " AND MostreTancuri.CodFerma = Ferme_CCL.Cod "
+ " AND Convert(int,Ferme_CCL.Judet,2)=Judete.ID "
+ " AND Ferme_CCL.FabricaID = " + fabricaid

+ " AND MostreTancuri.Validat = 1   ORDER BY Ferme_CCL.Nume";
        /*
       SqlParameter f=new SqlParameter("@fabricaid",SqlDbType.Int);
        f.Value = fabricaid;
       cmd.Parameters.Add(f);

       SqlParameter d = new SqlParameter("@datatestare", SqlDbType.NVarChar);
       d.Value = datatestare;
       cmd.Parameters.Add(d);
        */
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
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            if (mf.NTG != "-" && Double.Parse(mf.NTG) >= 10000)
                mf.NTG = ">10000";
            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "-";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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
            /* 
                        mf.Grasime = (Convert.ToString(reader["Grasime"])=="0") ? "": Convert.ToString(reader["Grasime"]);
                        mf.Proteina = (Convert.ToString(reader["ProcentProteine"]) == "0") ? "" : Convert.ToString(reader["ProcentProteine"]);
                        mf.Lactoza = (Convert.ToString(reader["ProcentLactoza"]) == "0") ? "" : Convert.ToString(reader["ProcentLactoza"]);
                        mf.Substu = (Convert.ToString(reader["SubstantaUscata"]) == "0") ? "" : Convert.ToString(reader["SubstantaUscata"]);
                        mf.NCS = (Convert.ToString(reader["NumarCeluleSomatice"]) == "0") ? "" : Convert.ToString(reader["NumarCeluleSomatice"]);
                        mf.NTG = (Convert.ToString(reader["IncarcaturaGermeni"]) == "0") ? "" : Convert.ToString(reader["IncarcaturaGermeni"]);
                        mf.Puncti = Convert.ToString(reader["PunctInghet"]);
                        mf.Apa = "";
                        if (Convert.ToDouble(mf.Puncti) == 0)
                            mf.Puncti = "";
                        else
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
             */
            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
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
    public static List<MostreFabrica> GetMostra
  (string id)
    {
        SqlConnection cnn = new SqlConnection(
        ConfigurationManager.ConnectionStrings
        ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
cmd.CommandTimeout = 180;
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
            mf.NrComanda = Convert.ToString(reader["NrComanda"]);
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
           
            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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

    //
    public static List<MostreFabrica> GetMostreFCB
(string client, string datatestare)
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
+ " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
+ " AND MostreTancuri.NumeClient = '" + client

+ "' AND MostreTancuri.Validat = 1  ORDER BY MostreTancuri.CodBare";
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            if (mf.NTG != "-" && Double.Parse(mf.NTG) >= 10000)
                mf.NTG = ">10000";

            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "-";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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


            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
           
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


    public static List<MostreFabrica> GetMostreFerma
   (string fabricaid, string codferma, string datatestare)
    {
        SqlConnection cnn = new SqlConnection(
        ConfigurationManager.ConnectionStrings
        ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
cmd.CommandTimeout = 300;
        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.PersonaDeContact, MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
            "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
"MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni, MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.GrasimeProv,MostreTancuri.Caseina, MostreTancuri.PrelevatorID," +
"MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
"FROM Ferme_CCL, MostreTancuri WHERE"
+ " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
+ " AND MostreTancuri.CodFerma = Ferme_CCL.Cod "
+ " AND Ferme_CCL.FabricaID = " + fabricaid
+ " AND Ferme_CCL.Cod = " + codferma
+ " AND MostreTancuri.Validat = 1  ORDER BY Ferme_CCL.Nume";

        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        List<MostreFabrica> list = new List<MostreFabrica>();
        while (reader.Read())
        {
            MostreFabrica mf = new MostreFabrica();
            mf.CodBare = Convert.ToString(reader["CodBare"]);
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            if (mf.NTG != "-" && Double.Parse(mf.NTG) >= 10000)
                mf.NTG = ">10000";

            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "-";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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
            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
       //     mf.PrelevatoriNume = Convert.ToString(reader["NumePrelevator"]);
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

            list.Add(mf);
        }
        cnn.Close();
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
        string query_date = " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) ";
        if (datatestare2 != "")
        {
            query_date = " (CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) >= CONVERT(datetime, '" + datatestare + "', 103))";

            query_date += " AND (CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) <= CONVERT(datetime, '" + datatestare2 + "', 103))";
        }

		 cmd.CommandTimeout = 180;
        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Judete.Denloc,  "
      + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
            "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
"MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.GrasimeProv,MostreTancuri.Caseina, MostreTancuri.PrelevatorID," +
"MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
"FROM Ferme_CCL, Judete, MostreTancuri WHERE"
            //  + " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
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
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            //mf.DataPrimirii = (DateTime)Convert.ToString(reader["DataPrimirii"]);
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            //                mf.Grasime = (Convert.ToString(reader["Grasime"]) == "0") ? "" : Convert.ToString(reader["Grasime"]);
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            //                mf.Proteina = (Convert.ToString(reader["ProcentProteine"]) == "0") ? "" : Convert.ToString(reader["ProcentProteine"]);
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            //                mf.Lactoza = (Convert.ToString(reader["ProcentLactoza"]) == "0") ? "" : Convert.ToString(reader["ProcentLactoza"]);
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            //                mf.Substu = (Convert.ToString(reader["SubstantaUscata"]) == "0") ? "" : Convert.ToString(reader["SubstantaUscata"]);
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            //                mf.NCS = (Convert.ToString(reader["NumarCeluleSomatice"]) == "0") ? "" : Convert.ToString(reader["NumarCeluleSomatice"]);
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            if (mf.NTG != "-" && Double.Parse(mf.NTG) >= 10000)
                mf.NTG = ">10000";

            //                mf.NTG = (Convert.ToString(reader["IncarcaturaGermeni"]) == "0") ? "" : Convert.ToString(reader["IncarcaturaGermeni"]);
            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "-";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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

            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
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
    public static List<MostreFabrica> GetMostreSMS
    (string  datatestare)
    {
        SqlConnection cnn = new SqlConnection(
        ConfigurationManager.ConnectionStrings
        ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Ferme_CCL.Ferme_CCL,Ferme_CCL.FermierID,Ferme_CCL.Telefon,  "
      + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
            "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
"MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.Caseina,MostreTancuri.GrasimeProv, MostreTancuri.PrelevatorID," +
"MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
"FROM Ferme_CCL, MostreTancuri WHERE"
+ " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
//+ " AND MostreTancuri.CodFerma is not null"
+ " AND MostreTancuri.CodFerma = Ferme_CCL.Cod "
+ " AND MostreTancuri.SentSms=0"
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
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            if (mf.NTG != "-" && Double.Parse(mf.NTG) >= 10000)
                mf.NTG = ">10000";
            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "-";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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
           
            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            //get prelevator name

            mf.PrelevatoriNume = Convert.ToString(reader["PersonaDeContact"]);
          
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
          
            mf.PersContact = Convert.ToString(reader["PersonaDeContact"]);
            mf.TelContact = Convert.ToString(reader["TelPersoanaContact"]);
            string fermeccl = Convert.ToString(reader["Ferme_CCL"]);
            string fermierid = Convert.ToString(reader["FermierID"]);
          
            list.Add(mf);
        }
        cnn.Close();
        return list;
    }
   
//
 //getmostresms
    public static List<MostreFabrica> GetMostreSMSTest
    (string  datatestare)
    {
        SqlConnection cnn = new SqlConnection(
        ConfigurationManager.ConnectionStrings
        ["fccl2ConnectionString"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;

        cmd.CommandText = "SELECT Ferme_CCL.ID, Ferme_CCL.Nume,Ferme_CCL.FabricaID,Ferme_CCL.Cod,Ferme_CCL.Strada,Ferme_CCL.Numar,Ferme_CCL.Oras,Ferme_CCL.PersonaDeContact,Ferme_CCL.TelPersoanaContact,Ferme_CCL.Ferme_CCL,Ferme_CCL.FermierID,Ferme_CCL.Telefon,  "
      + "MostreTancuri.DataPrelevare, MostreTancuri.DataPrimirii, MostreTancuri.CodBare," +
            "MostreTancuri.Grasime, MostreTancuri.ProcentProteine,MostreTancuri.ProcentLactoza, MostreTancuri.SubstantaUscata, MostreTancuri.PunctInghet, MostreTancuri.Antibiotice," +
"MostreTancuri.NumarCeluleSomatice, MostreTancuri.IncarcaturaGermeni,MostreTancuri.PH,MostreTancuri.Urea,MostreTancuri.Caseina,MostreTancuri.GrasimeProv, MostreTancuri.PrelevatorID," +
"MostreTancuri.DataTestare,MostreTancuri.DataTestareFinala,MostreTancuri.NrComanda " +
"FROM Ferme_CCL, MostreTancuri WHERE"
+ " CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) = CONVERT(datetime, '" + datatestare + "', 103) "
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
            mf.DataTestare = Convert.ToString(reader["DataTestare"]);
            mf.DataTestareFinala = Convert.ToString(reader["DataTestareFinala"]);
            mf.DataPrimirii = ((DateTime)reader["DataPrimirii"]).ToString("dd/MM/yyyy");
            mf.DataPrelevare = Convert.ToString(reader["DataPrelevare"]);

            mf.Grasime = (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Grasime"])) == 0.00001) ? "0" : Convert.ToString(reader["Grasime"]));
            mf.Proteina = (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentProteine"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentProteine"]));
            mf.Lactoza = (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["ProcentLactoza"])) == 0.00001) ? "0" : Convert.ToString(reader["ProcentLactoza"]));
            mf.Substu = (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["SubstantaUscata"])) == 0.00001) ? "0" : Convert.ToString(reader["SubstantaUscata"]));
            mf.NCS = (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["NumarCeluleSomatice"])) == 0.00001) ? "0" : Convert.ToString(reader["NumarCeluleSomatice"]));
            mf.NTG = (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["IncarcaturaGermeni"])) == 0.00001) ? "0" : Convert.ToString(reader["IncarcaturaGermeni"]));
            if (mf.NTG != "-" && Double.Parse(mf.NTG) >= 10000)
                mf.NTG = ">10000";
            mf.Puncti = Convert.ToString(reader["PunctInghet"]);
            mf.Apa = "-";
            if (Convert.ToDouble(mf.Puncti) == 0)
                mf.Puncti = "-";
            else if (Convert.ToDouble(mf.Puncti) == 0.00001)
                mf.Puncti = "0";
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
           
            mf.Urea = (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Urea"])) == 0.00001) ? "0" : Convert.ToString(reader["Urea"]));
            mf.Ph = (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["PH"])) == 0.00001) ? "0" : Convert.ToString(reader["PH"]));
            mf.Combi = Convert.ToString(reader["GrasimeProv"]);
            mf.Casein = (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0 ? "-" : (Convert.ToDouble(Convert.ToString(reader["Caseina"])) == 0.00001) ? "0" : Convert.ToString(reader["Caseina"]));
            mf.PrelevatoriId = Convert.ToString(reader["PrelevatorID"]);
            //get prelevator name

            mf.PrelevatoriNume = Convert.ToString(reader["PersonaDeContact"]);
          
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

        string grasime = mf.Grasime.Trim().Equals("") ? "0" : mf.Grasime.Trim().Equals("0") ? "0.00001" : mf.Grasime.Trim();
        string proteina = mf.Proteina.Trim().Equals("") ? "0" : mf.Proteina.Trim().Equals("0") ? "0.00001" : mf.Proteina.Trim();
        string lactoza = mf.Lactoza.Trim().Equals("") ? "0" : mf.Lactoza.Trim().Equals("0") ? "0.00001" : mf.Lactoza.Trim();
        string caseina = mf.Casein.Trim().Equals("") ? "0" : mf.Casein.Trim().Equals("0") ? "0.00001" : mf.Casein.Trim();
        string substu = mf.Substu.Trim().Equals("") ? "0" : mf.Substu.Trim().Equals("0") ? "0.00001" : mf.Substu.Trim();
        string pcti = mf.Puncti.Trim().Equals("") ? "0" : mf.Puncti.Trim().Equals("0") ? "0.00001" : mf.Puncti.Trim();
        //
        if (pcti.Length > 0 && (!pcti.Equals("0.00001")) && pcti.IndexOf('.') > 0)
            pcti = pcti.Substring(pcti.IndexOf('.') + 1);
        string antib = mf.Antib.Trim();
        // 
        string ncs = mf.NCS.Trim().Equals("") ? "0" : mf.NCS.Trim().Equals("0") ? "0.00001" : mf.NCS.Trim();
        string ntg = mf.NTG.Trim().Equals("") ? "0" : mf.NCS.Trim().Equals("0") ? "0.00001" : mf.NTG.Trim();

        string urea = mf.Urea.Trim().Equals("") ? "0" : mf.Urea.Trim().Equals("0") ? "0.00001" : mf.Urea.Trim();
        string ph = mf.Ph.Trim().Equals("") ? "0" : mf.Ph.Trim().Equals("0") ? "0.00001" : mf.Ph.Trim();
        string codferma = mf.CodBare.Substring(0,5);
        string definitiv = mf.Definitiv? "1": "0";
        string validat = mf.Validat? "1":"0";
        string sentsms = mf.Sentsms ? "1" : "0";
        string cant = mf.Cant.Trim().Equals("") ? "0" : mf.Cant.Trim();
        string prelid = mf.PrelevatoriId.Trim().Equals("") ? "0" : mf.PrelevatoriId.Trim();
        SqlConnection cnn = new SqlConnection(
  ConfigurationManager.ConnectionStrings
     ["fccl2ConnectionString"].ConnectionString);
       
        string query = "INSERT INTO MostreTancuri (CodBare,IdZilnic,CantitateLaPrelevare,PrelevatorID,DataPrimirii,DataPrelevare,DataTestare,DataTestarefinala," +
"Grasime, ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PunctInghet,NumarCeluleSomatice,IncarcaturaGermeni,Antibiotice," +
"Urea,PH,NrComanda,NumePrelevator,NumeProba,NumeClient,AdresaClient,Localitate,Judet,Definitiv,Validat,Sentsms,CodFerma,GrasimeProv,LactozaProv,SubstuProv,ProteineProv) VALUES(" +
"'" + mf.CodBare + "'," + mf.IdZilnic + "," + cant + "," + prelid + ",CONVERT(datetime,'" + mf.DataPrimirii + "',103),'" + mf.DataPrelevare + "','" + mf.DataTestare + "','" + mf.DataTestareFinala + "'," +
grasime + "," + proteina + "," + caseina + "," + lactoza + "," + substu + "," + pcti + "," + ncs + "," + ntg + ",'" + antib+ "'," + urea + "," + ph + ",'" + mf.NrComanda + "','" + mf.PrelevatoriNume + "','" + mf.NumeProba + "','" + mf.FabriciNume + "','" +
mf.FabriciStrada + "','" + mf.FabriciOras + "','" + mf.FabriciJudet + "'," + definitiv + "," + validat + "," + sentsms + ",'"+codferma+"',0,0,0,0)";
        
     
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

    }

    public void UpdateMostra(MostreFabrica mf)
    {

        string grasime = mf.Grasime.Trim().Equals("") ? "0" : mf.Grasime.Trim().Equals("0") ? "0.00001" : mf.Grasime.Trim();
        string proteina = mf.Proteina.Trim().Equals("") ? "0" : mf.Proteina.Trim().Equals("0") ? "0.00001" : mf.Proteina.Trim();
        string lactoza = mf.Lactoza.Trim().Equals("") ? "0" : mf.Lactoza.Trim().Equals("0") ? "0.00001" : mf.Lactoza.Trim();
        string caseina = mf.Casein.Trim().Equals("") ? "0" : mf.Casein.Trim().Equals("0") ? "0.00001" : mf.Casein.Trim();
        string substu = mf.Substu.Trim().Equals("") ? "0" : mf.Substu.Trim().Equals("0") ? "0.00001" : mf.Substu.Trim();
        string pcti = mf.Puncti.Trim().Equals("") ? "0" : mf.Puncti.Trim().Equals("0") ? "0.00001" : mf.Puncti.Trim();
        //
        if (pcti.Length > 0 && (!pcti.Equals("0.00001")) && pcti.IndexOf('.') > 0)
            pcti = pcti.Substring(pcti.IndexOf('.') + 1);
        string antib = mf.Antib.Trim();
        // 
        string ncs = mf.NCS.Trim().Equals("") ? "0" : mf.NCS.Trim().Equals("0") ? "0.00001" : mf.NCS.Trim();
        string ntg = mf.NTG.Trim().Equals("") ? "0" : mf.NCS.Trim().Equals("0") ? "0.00001" : mf.NTG.Trim();

        string urea = mf.Urea.Trim().Equals("") ? "0" : mf.Urea.Trim().Equals("0") ? "0.00001" : mf.Urea.Trim();
        string ph = mf.Ph.Trim().Equals("") ? "0" : mf.Ph.Trim().Equals("0") ? "0.00001" : mf.Ph.Trim();
        string codferma = mf.CodBare.Substring(0, 5);
        string definitiv = mf.Definitiv ? "1" : "0";
        string validat = mf.Validat ? "1" : "0";
        string sentsms = mf.Sentsms ? "1" : "0";
        string cant = mf.Cant.Trim().Equals("") ? "0" : mf.Cant.Trim();
        string prelid = mf.PrelevatoriId.Trim().Equals("") ? "0" : mf.PrelevatoriId.Trim();
        SqlConnection cnn = new SqlConnection(
  ConfigurationManager.ConnectionStrings
     ["fccl2ConnectionString"].ConnectionString);
     
        string query = "UPDATE MostreTancuri SET IdZilnic="+mf.IdZilnic+",CantitateLaPrelevare="+cant+
            ",PrelevatorID="+prelid+",DataPrimirii=CONVERT(datetime,'" + mf.DataPrimirii + "',103),"+
"DataPrelevare='"+mf.DataPrelevare+"',DataTestare='"+mf.DataTestare+"',DataTestareFinala='"+mf.DataTestareFinala+"',"+
"Grasime="+grasime+",ProcentProteine="+proteina+",Caseina="+caseina+",ProcentLactoza="+lactoza+",SubstantaUscata="+substu+",PunctInghet="+
pcti+",NumarCeluleSomatice="+ncs+",IncarcaturaGermeni="+ntg+",Urea="+urea+",PH="+ph+",NrComanda='"+mf.NrComanda+"',NumePrelevator='"+
mf.PrelevatoriNume+"',NumeProba='"+mf.NumeProba+"',NumeClient='"+mf.FabriciNume+"',AdresaClient='"+mf.FabriciStrada+"',Localitate='"+
mf.FabriciOras+"',Judet='"+mf.FabriciJudet+"',Definitiv="+definitiv+",Validat="+validat+",Sentsms="+sentsms+",CodFerma='"+codferma+
"' WHERE CodBare='" + mf.CodBare + "'";
        
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        
    }

    public void DeleteMostra(MostreFabrica mf)     
    {
        SqlConnection cnn = new SqlConnection(
  ConfigurationManager.ConnectionStrings
     ["fccl2ConnectionString"].ConnectionString);
        string query = "DELETE FROM MostreTancuri WHERE "+
            //"CodBare='"+mf.CodBare+"'";
            "ID=" + mf.Id;
        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();

    }

}



