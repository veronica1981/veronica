//*********************************************************************
//
// Mostre Class
//
// Class that encapsulates all data logic necessary to add/query/delete
// Mostre within the Portal database.
//
//*********************************************************************
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CopSite.Old_App_Code;
using FCCL_BL.Bus;
using FCCL_BL.Managers;
using FCCL_DAL;
using FCCL_DAL.Entities;
using NLog;

public class MostraAnalizata
{
    public string IncrementZilnic { get; set; }
    public string CodBare { get; set; }
}

public class MostreDB : IMostreDB
{
    private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext;
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private static Logger loggerReceptie = LogManager.GetLogger("Receptie");
    private static Logger loggerRezultate = LogManager.GetLogger("Rezultate");
    private static Logger loggerCrotalii = LogManager.GetLogger("Crotalii");

    private string CopConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString; }
    }

    public int GetTotalDocs()
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter("Portal_TotalDocs", myConnection);

        // Mark the Command as a SPROC
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterTotalDocs = new SqlParameter("@TotalDocs", SqlDbType.Int, 4);
        parameterTotalDocs.Direction = ParameterDirection.Output;
        myCommand.SelectCommand.Parameters.Add(parameterTotalDocs);

        myConnection.Open();
        myCommand.SelectCommand.ExecuteNonQuery();
        myConnection.Close();

        if (parameterTotalDocs.Value == DBNull.Value)
            return 0;
        return (int)parameterTotalDocs.Value;
    }

    public DataSet MostrePage(int pageIndex, int pageSize)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter("Portal_Mostre_Get", myConnection);

        // Mark the Command as a SPROC
        myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterPageIndex = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
        parameterPageIndex.Value = pageIndex;
        myCommand.SelectCommand.Parameters.Add(parameterPageIndex);

        SqlParameter parameterPageSize = new SqlParameter("@PageSize", SqlDbType.Int, 4);
        parameterPageSize.Value = pageSize;
        myCommand.SelectCommand.Parameters.Add(parameterPageSize);


        // Create and Fill the DataSet
        DataSet myDataSet = new DataSet();
        myCommand.Fill(myDataSet);


        // Return the DataSet
        return myDataSet;
    }

    public string VerificCodBare(string codbare)
    {
        string ultimele3;
        int int_ultimele3;
        string noul_codbare;
        bool gasit = true;
        noul_codbare = codbare;

        while (gasit)
        {
            if (ExistaCodBare(noul_codbare) != 0)
            {
                ultimele3 = noul_codbare.Substring(noul_codbare.Length - 3, 3);
                ultimele3 = "1" + ultimele3;
                int_ultimele3 = Convert.ToInt32(ultimele3);
                int_ultimele3 = int_ultimele3 + 1;
                ultimele3 = Convert.ToString(int_ultimele3).Substring(1, 3);
                noul_codbare = noul_codbare.Substring(0, noul_codbare.Length - 3) + ultimele3;
                gasit = true;
            }
            else
                gasit = false;
        }
        if (!gasit)
            return noul_codbare;
        return codbare;
    }

    public List<Mostra> VerificFile(string filepath)
    {
        var coduri = new StringBuilder();
        var mostre = new List<Mostra>();
        List<string> coduri_eronate = new List<string>();
        List<MostraAnalizata> mostreAnalizate = new List<MostraAnalizata>();

        int posOfSlash = filepath.LastIndexOf(@"\");
        string zi = filepath.Substring(posOfSlash + 1, 2);
        string luna = filepath.Substring(posOfSlash + 3, 2);
        string an = filepath.Substring(posOfSlash + 5, 2);
        string filename = filepath.Substring(posOfSlash + 1);

        bool b_file = false;
        bool c_file = false;
        bool c1_file = false;
        bool a_file = false;
        bool f_file = false;
        int lineCnt = 0;

        loggerRezultate.Info("verific fisier {0}", filepath);

        if (File.Exists(filepath))
        {
            if (filepath.ToUpper().EndsWith("B1COP.CSV") || filepath.ToUpper().EndsWith("B2COP.CSV"))
                b_file = true;
            if (filepath.ToUpper().EndsWith("C.CSV"))
                c_file = true;
            if (filepath.ToUpper().Contains("C1"))
                c1_file = true;
            if (filename.ToUpper().Contains("A"))
                a_file = true;
            if (filename.ToUpper().Contains("C2") || filename.ToUpper().Contains("C3"))
                f_file = true;
            FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string linie = "";
            string[] a_linie;
            if (b_file)
            {
                linie = sr.ReadLine();
                lineCnt = 1;
                while (linie != null)
                {
                    linie = sr.ReadLine();
                    lineCnt++;
                    if (lineCnt > 6 && linie != null)
                    {
                        a_linie = linie.Split(';');
                        if (a_linie[0] != null && Utils.IsDouble(a_linie[0].Trim(), NumberStyles.Integer))// && a_linie[4].ToString().Trim() != "")
                        {
                            coduri.Append(a_linie[2].Trim() + ";");
                            mostreAnalizate.Add(new MostraAnalizata { IncrementZilnic = a_linie[0], CodBare = a_linie[2] });
                        }
                    }
                }
            }

            if (f_file)
            {
                linie = sr.ReadLine();
                lineCnt = 1;
                while (linie != null)
                {
                    linie = sr.ReadLine();
                    lineCnt++;
                    if (lineCnt > 6 && linie != null)
                    {
                        a_linie = linie.Split(';');
                        if (a_linie[0] != null && Utils.IsDouble(a_linie[0].Trim(), NumberStyles.Integer))
                        // && ((a_linie.Length > 17 && a_linie[17].Equals("VALUE")) || (a_linie.Length > 30 && a_linie[30].Equals("VALUE"))))
                        {
                            coduri.Append(a_linie[1].Trim() + ";");
                            mostreAnalizate.Add(new MostraAnalizata { IncrementZilnic = a_linie[0], CodBare = a_linie[1] });
                        }
                    }
                }
            }

            if (c_file)
            {
                linie = sr.ReadLine();
                lineCnt = 1;
                while (linie != null)
                {
                    linie = sr.ReadLine();
                    lineCnt++;
                    if (lineCnt > 4 && linie != null)
                    {
                        a_linie = linie.Split(';');
                        if (a_linie[0] != null && Utils.IsDouble(a_linie[0].Trim(), NumberStyles.Integer))
                        // && a_linie[4].ToString().Trim() != "")
                        {
                            coduri.Append(a_linie[2].Trim() + ";");
                            mostreAnalizate.Add(new MostraAnalizata { IncrementZilnic = a_linie[0], CodBare = a_linie[2] });
                        }
                    }
                }
            }

            if (c1_file)
            {
                linie = sr.ReadLine();
                lineCnt = 1;
                while (linie != null)
                {
                    linie = sr.ReadLine();
                    lineCnt++;
                    if (lineCnt > 7 && linie != null)
                    {
                        a_linie = linie.Split(';');
                        if (a_linie[0] != null && Utils.IsDouble(a_linie[0].Trim(), NumberStyles.Integer))
                        // && a_linie[5].ToString().Trim() != "")
                        {
                            coduri.Append(a_linie[3].Trim() + ";");
                            mostreAnalizate.Add(new MostraAnalizata { IncrementZilnic = a_linie[0], CodBare = a_linie[3] });
                        }
                    }
                }
            }

            bool urmeaza_cod_bare = false;
            bool urmeaza_pct_inghet = false;
            string pct_inghet = "";
            string cod_bare = "";
            string a_data_test = "";
            if (a_file)
            {
                int i = 0;
                linie = sr.ReadLine();

                while (linie != null)
                {
                    linie = sr.ReadLine();
                    if (lineCnt <= 5 && ((linie.IndexOf(" PM") >= 0) || (linie.IndexOf(" AM") >= 0)))
                    {
                        a_data_test = linie.TrimStart(' ').Substring(3, 2) + "/" + linie.TrimStart(' ').Substring(0, 2) + "/" + linie.TrimStart(' ').Substring(6, 4);
                    }
                    lineCnt++;

                    if (linie != null)
                    {
                        try
                        {
                            if (urmeaza_pct_inghet)
                            {
                                coduri.Append(cod_bare + ";");
                                pct_inghet = linie.Substring(linie.IndexOf("mC") - 4, 3);
                                urmeaza_pct_inghet = false;
                                mostreAnalizate.Add(new MostraAnalizata { IncrementZilnic = i.ToString(), CodBare = cod_bare });
                                i++;
                            }
                            if (urmeaza_cod_bare)
                            {
                                cod_bare = linie.Substring(linie.Length - 10, 10).TrimStart(' ');
                                urmeaza_pct_inghet = true;
                            }
                            int idx = linie.IndexOf("I.D.#");
                            urmeaza_cod_bare = (idx >= 0);
                        }
                        catch (Exception ex)
                        {
                            urmeaza_cod_bare = false;
                            urmeaza_pct_inghet = false;
                            loggerRezultate.Error(string.Format("VerificFile|ERROR:{0}", ex.Message));
                        }
                    }
                }
            }

            sr.Close();
            fs.Close();

            var mostreManager = new MostreManager(CopConnectionString);
            mostre = mostreManager.GetDateCoduriBare(coduri.ToString().TrimEnd(';'));

            foreach (var mostraAnalizata in mostreAnalizate)
            {
                if (mostraAnalizata.CodBare.Trim() == "")
                {
                    loggerRezultate.Info("EROARE: Cod de bare gol la pozitia ID= {0}", mostraAnalizata.IncrementZilnic.Trim());
                }
                else
                {
                    if (a_file)
                    {
                        if (!mostre.Exists(m => m.CodBare == mostraAnalizata.CodBare.Trim()) && (mostraAnalizata.CodBare.Trim() != "1111111111"))
                        {
                            loggerRezultate.Info("Codul de bare: {0} nu a fost importat din fisierul de receptie din data de: {1}/{2}/20{3}", mostraAnalizata.CodBare, zi, luna, an);
                            coduri_eronate.Add(mostraAnalizata.CodBare.Trim());
                        }
                    }
                    else
                    {
                        if (ErrorInt(mostraAnalizata.IncrementZilnic.Trim()))
                        {
                            loggerRezultate.Info("EROARE: Valoare incorecta la Incrementul Zilnic: {0}", mostraAnalizata.IncrementZilnic.Trim());
                        }
                        //else
                        //{
                        //    if (ExistaIdZilnic(a_data_test, Convert.ToInt32(mostraAnalizata.IncrementZilnic.Trim())) == 0 && (mostraAnalizata.CodBare.Trim() != "1111111111"))
                        //    {
                        //        loggerRezultate.Info("Nu exista in baza de date incrementul zilnic: {0} in ziua : {1}", mostraAnalizata.IncrementZilnic, a_data_test);
                        //        coduri_eronate.Add(mostraAnalizata.CodBare.Trim());
                        //    }
                        //}
                    }
                }
            }
            foreach (var coduriDuplicate in mostreAnalizate.GroupBy(x => x.CodBare).Where(g => g.Count() > 1).Select(y => y.Key).ToList())
            {
                loggerRezultate.Info("EROARE: Codul de bare: {0} se gaseste de mai multe ori: ", coduriDuplicate);
                coduri_eronate.Add(coduriDuplicate.Trim());
            }
            foreach (var iduriDuplicate in mostreAnalizate.GroupBy(x => x.IncrementZilnic).Where(g => g.Count() > 1).Select(y => y.Key).ToList())
            {
                loggerRezultate.Info("EROARE: Incrementul zilnic: {0} se gaseste de mai multe ori: ", iduriDuplicate);
                coduri_eronate.Add(mostreAnalizate.First(m => m.IncrementZilnic == iduriDuplicate).CodBare);
            }
        }

        return coduri_eronate.Count > 0 ? null : mostre;
    }

    public void Addtext(string path, string text)
    {
        FileStream fs1 = new FileStream(path, File.Exists(path) ? FileMode.Append : FileMode.Create);

        StreamWriter sw = new StreamWriter(fs1);

        sw.WriteLine(text);
        sw.Close();
        fs1.Close();
    }

    public void Concatenare2fisieretxt(string path1, string path2)
    {
        FileStream fs1, fs2;
        fs1 = new FileStream(path1, FileMode.Open);
        fs2 = new FileStream(path2, FileMode.Append);

        StreamReader sr = new StreamReader(fs1);
        StreamWriter sw = new StreamWriter(fs2);
        sw.WriteLine(sr.ReadToEnd());
        sr.Close();
        fs1.Close();
        sw.Close();
        fs2.Close();
    }

    public string ConvertValA(string res)
    {
        string resc = "0";
        double dbl;
        if (double.TryParse(res, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"), out dbl))
        {
            resc = res.Trim() == "0" ? "0.00001" : res.Trim();
            if (resc.Trim() == "") resc = "0";
        }
        return resc;
    }

    public void ImportManual(string filepath)
    {
        FileStream fs;
        string linie = "";
        bool b_file = false;
        bool c_file = false;
        bool c1_file = false;
        bool a_file = false;
        bool f_file = false;
        bool o_file = false;
        bool f3_file = false;
        int lineCnt = 0;

        loggerRezultate.Info("*************************************************************************************************");
        loggerRezultate.Info("*************************************************************************************************");

        string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;

        int posOfSlash = filepath.LastIndexOf(@"\");
        int zi = int.Parse(filepath.Substring(posOfSlash + 1, 2));
        int luna = int.Parse(filepath.Substring(posOfSlash + 3, 2));
        int an = int.Parse("20" + filepath.Substring(posOfSlash + 5, 2));
        DateTime dataTestareFinala = new DateTime(an, luna, zi);
        DateTime dataTestare = new DateTime(an, luna, zi);
        string filename = filepath.Substring(posOfSlash + 1);
        string saveDir = filepath.Substring(0, posOfSlash + 1) + @"FisiereImportate";

        if (filepath.ToUpper().EndsWith("B1COP.CSV") || filepath.ToUpper().EndsWith("B2COP.CSV"))
            b_file = true;
        if (filepath.ToUpper().EndsWith("CCOP.CSV"))
            c_file = true;
        if (filepath.ToUpper().EndsWith("C1COP.CSV"))
            c1_file = true;
        if (filename.ToUpper().IndexOf("ACOP") >= 0)
            a_file = true;
        if (filename.ToUpper().IndexOf("C2COP") >= 0)
            f_file = true;
        if (filename.ToUpper().IndexOf("C3COP") >= 0)
            f3_file = true;
        if (filename.ToUpper().EndsWith("C.CSV"))
            o_file = true;
        string cod_bare = "";

        //loggerRezultate.Info("Start import rezultate din fisier {0}, tip {1}", filename, f_file);

        int idCodBare = 0;
        bool fileError = false;
        bool cod_gresit = false;
        var mostre = VerificFile(filepath);

        if (mostre == null)
        {
            loggerRezultate.Error("Eroare in analize, va rog sa-l corectati sa sa-l reimportati.");
        }

        if (!fileError && File.Exists(filepath))
        {
            if (!JobNameIsValid(filepath, c1_file))
            {
                loggerRezultate.Error("EROARE: Numele jobului nu coincide cu numele documentului");
                return;
            }

            fs = new FileStream(filepath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            if (b_file)
            {
                BLineProcess(idCodBare, sr, dataTestareFinala, mostre);
            }

            if (c1_file)
            {
                C1LineProcess(sr, dataTestareFinala, mostre);
            }

            if (c_file)
            {
                CLineProcess(sr, dataTestareFinala, mostre);
            }

            if (o_file)
            {
                CLineProcess(sr, dataTestareFinala, mostre);
            }

            bool urmeaza_cod_bare = false;
            bool urmeaza_pct_inghet = false;
            string a_data_test = "";
            if (a_file)
            {
                linie = sr.ReadLine();
                lineCnt = 0;
                while (linie != null)
                {
                    if (lineCnt <= 5 && ((linie.IndexOf(" PM") >= 0) || (linie.IndexOf(" AM") >= 0)))
                        a_data_test = linie.TrimStart(' ').Substring(3, 2) + "/" + linie.TrimStart(' ').Substring(0, 2) +
                                      "/" + linie.TrimStart(' ').Substring(6, 4);
                    if (linie != null)
                    {
                        try
                        {
                            if (urmeaza_pct_inghet)
                            {
                                string pct_inghet = linie.Substring(linie.IndexOf("mC") - 4, 3);
                                urmeaza_pct_inghet = false;
                                cod_gresit = false;

                                if (!Utils.IsDouble(pct_inghet) || ErrorInt(cod_bare))
                                {
                                    loggerRezultate.Error("EROARE: Valoare incorecta la linia: " + Convert.ToString(lineCnt));
                                    cod_gresit = true;
                                }
                                if (a_data_test.Trim() == "")
                                {
                                    linie = sr.ReadLine();
                                    lineCnt++;
                                    if (linie != null && lineCnt <= 5 &&
                                        ((linie.IndexOf(" PM") >= 0) || (linie.IndexOf(" AM") >= 0)))
                                        a_data_test = linie.TrimStart(' ').Substring(3, 2) + "/" +
                                                      linie.TrimStart(' ').Substring(0, 2) + "/" +
                                                      linie.TrimStart(' ').Substring(6, 4);
                                }
                                if (!cod_gresit)
                                {
                                    SqlDataReader drMostra = GetDateDataTestareIdZilnic(dataTestare, Convert.ToInt32(cod_bare));
                                    if (drMostra.Read() && !cod_gresit && Convert.ToString(drMostra["PunctInghet"]).Trim() != "0")
                                    {
                                        if (Math.Abs(Utils.ConvertToDouble(pct_inghet.Trim()) - Utils.ConvertToDouble(Convert.ToString(drMostra["PunctInghet"]).Trim())) > 0.00001)
                                            loggerRezultate.Error("EROARE: Identificatorul zilnic: " + cod_bare + " din data: " + dataTestare.ToShortDateString() + " a mai fost analizat, avand ca rezultat alte valori pentru Punct de Inghet! ");
                                        cod_gresit = true;
                                    }
                                    drMostra.Close();
                                }
                                if (!cod_gresit && cod_bare.Trim() != "1111111111")
                                {
                                    if (DateTime.Compare(dataTestareFinala, dataTestare) < 0)
                                    {
                                        loggerRezultate.Error("EROARE: DataTestare finala(" + a_data_test + ") mai mica dacat data testare (" + dataTestare.ToShortDateString() + ")");
                                        fileError = true;
                                    }
                                    else
                                    {
                                        string pcti_val;
                                        if (pct_inghet.Trim() == "0")
                                            pcti_val = "0.00001";
                                        else
                                            pcti_val = pct_inghet.Trim();
                                        // this.UpdateMostraA(dataTestare, Convert.ToInt32(cod_bare), pcti_val, a_data_test); // a_data_test (din fisier) este data testare finala
                                        UpdateMostraA(dataTestare, Convert.ToInt32(cod_bare), pcti_val,
                                            dataTestareFinala);
                                    }
                                }
                            }
                            if (urmeaza_cod_bare)
                            {
                                cod_bare = linie.Substring(linie.Length - 10, 10).TrimStart(' ');
                                urmeaza_pct_inghet = cod_bare.Trim() != "";
                            }
                            int idx = linie.IndexOf("I.D.#");
                            if (idx >= 0)
                                urmeaza_cod_bare = true;
                            else
                                urmeaza_cod_bare = false;
                        }
                        catch (Exception ex)
                        {
                            urmeaza_cod_bare = false;
                            urmeaza_pct_inghet = false;
                            loggerRezultate.Error("EROARE: Valoare incorecta la linia: {0}, ERROR:{1}",
                                Convert.ToString(lineCnt), ex.Message);
                        }
                    }
                    linie = sr.ReadLine();
                    lineCnt++;
                }
            }

            //f_file
            if (f_file)
            {
                FLineProcess(sr, dataTestareFinala, mostre);
            }

            if (f3_file)
            {
                F3LineProcess(sr, dataTestareFinala, mostre);
            }

            sr.Close();
            fs.Close();
        }
        loggerRezultate.Info(!fileError ? "Importarea fisierului a fost efectuata!" : "Importarea fisierului nu a fost efectuata!");
    }

    private bool JobNameIsValid(string filePath, bool c1File)
    {
        try
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var jobName = string.Empty;
            if (c1File)
            {
                var lineWithJobname = File.ReadAllLines(filePath)[2];
                jobName = lineWithJobname.Substring(0, lineWithJobname.IndexOf(";")).Trim();
            }
            else
            {
                jobName = File.ReadAllLines(filePath)[0].Substring(10).Split(new []{";"}, StringSplitOptions.RemoveEmptyEntries).First().Trim(); //"Job name: " 
            }

            return fileName.Equals(jobName);
        }
        catch (Exception)
        {
            return false;
        }

    }

    public bool ErrorInt(string strvalue)
    {
        bool isNum;
        double jobid;

        isNum = double.TryParse(strvalue, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
        return !isNum;
    }

    public void ImportFisReceptie(string filepath)
    {
        Ferme_CCL ferma = new Ferme_CCL();
        bool error = false;
        string strcodbare;
        string strConn;
        int posOfSlash = filepath.LastIndexOf(@"\");

        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" +
                  "Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
        OleDbConnection cn;
        DataTable dtTables;

        cn = new OleDbConnection(strConn);
        cn.Open();
        dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        string tblstr = dtTables.Rows[dtTables.Rows.Count - 1]["TABLE_NAME"].ToString();

        string tbldate = tblstr.Substring(4, 8);
        string filelogpath = filepath.Substring(0, filepath.Length - 4) + "_" + tbldate + ".txt";
        string filelogInhibpath = filepath.Substring(0, filepath.Length - 4) + "_Inhib" + tbldate + ".txt";
        string logfilename = filelogpath.Substring(posOfSlash + 1);
        filelogpath = filelogpath.Replace(" ", "_");
        logfilename = logfilename.Replace(" ", "_");

        loggerReceptie.Info("*************************************************************************************************");
        loggerReceptie.Info("*************************************************************************************************");
        loggerReceptie.Info("Start import receptie fisier: {0}, data: {1}, numet sheet: {2}", filepath, DateTime.Now, tblstr);
        // end log files
        filepath = filepath.Replace(@"\", @"\\");

        OleDbDataAdapter myCommand = new OleDbDataAdapter();
        myCommand.SelectCommand = new OleDbCommand("SELECT * FROM [" + tblstr + "]", cn);

        DataSet myDataSet = new DataSet();
        myCommand.Fill(myDataSet);

        DataTable table;
        table = myDataSet.Tables[0];
        cn.Close();

        // For each row, print the values of each column.
        string ferma_ccl_cod = "";
        string crot = "";
        string rasa = "";
        int ferma_ccl_id = 0;
        int fabricaID = 0;
        int header = 0;
        string prelevator = "";
        double prelid;
        bool isNum;
        string prelevatorini = table.Rows[0][1].ToString();
        List<Order> orders = new List<Order>();
        OrderManager oManager = new OrderManager(ctx);
        if (!VerificareFisierReceptie(table, 6, 5))
        {
            loggerReceptie.Error("Eroare la verificare fisierului, oprire import");
            return;
        }
        foreach (DataRow row in table.Rows)
        {
            header++;

            if (header > 2 && row[3].ToString().Trim() != "")
            {
                error = false;
                strcodbare = row[3].ToString().Trim();
                if (ExistaCodBare(strcodbare) != 0)
                {
                    loggerReceptie.Info("Codul de bare: {0} exista deja", strcodbare);
                    error = true;
                }
                else
                {
                    DateTime datat_testare = new DateTime(int.Parse(tblstr.Substring(8, 4)),
                        int.Parse(tblstr.Substring(6, 2)), int.Parse(tblstr.Substring(4, 2)));
                    //	strDataPrelevare = row[5].ToString().Trim().Replace(@".", @"\");

                    DateTime dataPrelevare = DateTime.ParseExact(row[5].ToString().Trim(), "dd/MM/yyyy", new CultureInfo("fr-FR"));

                    prelevator = row[1].ToString().Trim();
                    /*
                        if (prelevator == "")
                          prelevator = prelevatorini;
                        else
                          prelevatorini = prelevator;
                     */
                    isNum = double.TryParse(prelevator, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"), out prelid);

                    if (!isNum)
                    {
                        loggerReceptie.Info("La codul de bare: {0} nu exista prelevator.", strcodbare);
                        error = true;
                    }
                    else
                    {
                        crot = strcodbare.Substring(0, 7);
                        // verific crotalia
                        ferma_ccl_cod = crot;
                        ferma_ccl_id = ferma.GetFermeId(crot);
                        rasa = ferma.GetFermeRasa(crot);
                        if (ferma_ccl_id == 0)
                        {
                            error = true;
                            loggerReceptie.Info("Nu exista crotalie pt. codul: {0}, crotalie: {1}", strcodbare, crot);
                        }
                        string farmName;
                        fabricaID = ferma.GetFerma(ferma_ccl_id, out farmName);
                        if (!error)
                        {
                            try
                            {
                                DateTime dataPrimirii = DateTime.ParseExact(row[6].ToString().Trim(), "dd/MM/yyyy", new CultureInfo("fr-FR"));
                                //DateTime dataPrimirii;
                                //if (!DateTime.TryParse(row[6].ToString().Trim(), new CultureInfo("fr-FR"), DateTimeStyles.None, out dataPrimirii))
                                //{
                                //    dataPrimirii = DateTime.Now;
                                //}

                                string nrComanda = row[0].ToString().Trim();
                                AddMostra(0, Convert.ToInt32(row[2].ToString().Trim()), strcodbare, nrComanda,
                                    ferma_ccl_cod, ferma_ccl_id, fabricaID, ""
                                    , "0", "0", "0", "0", "0", dataPrelevare, "0", dataPrimirii,
                                    "0", datat_testare, "0", Convert.ToInt32(prelevator), "", 0, 0, datat_testare, "0",
                                    "0", "0", rasa);
                                loggerReceptie.Info(
                                    "Mostra cod bare {0} pentru ferma Id {1}, fabrica Id {2}, nr comanda {3} adaugata cu succes.",
                                    strcodbare, ferma_ccl_id, fabricaID, nrComanda);

                                Order currentOrder = orders.FirstOrDefault(o => o.FullOrderNumber == nrComanda);
                                if (currentOrder != null)
                                {
                                    currentOrder.SampleCount++;
                                    currentOrder.AnalyzedSampleCount++;
                                }
                                else
                                {
                                    string[] arr = nrComanda.Split('-', '/');
                                    int orderNumber;
                                    if (arr.Length == 3 && int.TryParse(arr[1], out orderNumber))
                                    {
                                        //if (!DateTime.TryParse(row[5].ToString().Trim(), new CultureInfo("fr-FR"), DateTimeStyles.None, out dataPrelevare))
                                        //{
                                        //    dataPrelevare = DateTime.Now;
                                        //}

                                        currentOrder = new Order
                                        {
                                            AnalyzedSampleCount = 1,
                                            ClientId = fabricaID > 0 ? fabricaID : -1,
                                            ClientName = farmName + " (" + fabricaID + ")",
                                            FullOrderNumber = nrComanda,
                                            Imported = true,
                                            OrderNumber = orderNumber,
                                            ReceivedDate = dataPrimirii,
                                            SampleCount = 1,
                                            SampleDate = dataPrelevare
                                        };
                                        orders.Add(currentOrder);
                                    }
                                    else
                                    {
                                        loggerReceptie.Info("Adaugare comanda esuata: {0}", nrComanda);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                loggerReceptie.Info("Adaugare mostra esuata: {0}, err: {1}", strcodbare, ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
        }

        oManager.Save(orders);
        ctx.SaveChanges();

        loggerReceptie.Info("Importarea fisierului de receptie a fost efectuata!");
    }

    private bool VerificareFisierReceptie(DataTable rows, int dataPrimiriiIndex, int dataPrelevareIndex)
    {
        bool valid = true;

        foreach (var coduriDuplicate in rows.AsEnumerable().GroupBy(x => x[2]).Where(g => g.Count() > 1).Select(y => y.Key).ToList().Where(coduriDuplicate => coduriDuplicate.ToString() != ""))
        {
            loggerReceptie.Error("EROARE: Id zilnic: {0} se gaseste de mai multe ori: ", coduriDuplicate);
            valid = false;
        }

        DateTime outValue = new DateTime();
        int header = 0;

        foreach (DataRow row in rows.Rows)
        {
            header++;

            if (header > 2 && row[3].ToString().Trim() != "")
            {
                if (!DateTime.TryParseExact(row[dataPrelevareIndex].ToString().Trim(), "dd/MM/yyyy", new CultureInfo("fr-FR"), DateTimeStyles.None, out outValue))
                {
                    loggerReceptie.Error("EROARE: Data Prelevare: {0} este in format gresit: ", row[dataPrelevareIndex].ToString());
                    valid = false;
                }

                if (!DateTime.TryParseExact(row[dataPrimiriiIndex].ToString().Trim(), "dd/MM/yyyy", new CultureInfo("fr-FR"), DateTimeStyles.None, out outValue))
                {
                    loggerReceptie.Error("EROARE: Data Primirii: {0} este in format gresit: ", row[dataPrimiriiIndex].ToString());
                    valid = false;
                }
            }
        }

        return valid;
    }

    public void VerificareBV(string filepath, string filescan)
    {
        string strConn;
        int posOfSlash = filepath.LastIndexOf(@"\");

        string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;

        loggerReceptie.Info("*************************************************************************************************");
        loggerReceptie.Info("*************************************************************************************************");
        loggerReceptie.Info("Verificare BV start pentru fisierul {0}", filescan);

        SqlConnection cnn = new SqlConnection(CopConnectionString);
        filepath = filepath.Replace(@"\", @"\\");
        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" +
                  "Extended Properties='Excel 8.0;HDR=No;IMEX=1;READONLY=FALSE;'";

        OleDbConnection cn;
        DataTable dtTables;
        cn = new OleDbConnection(strConn);

        cn.Open();
        dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        string tblstr = dtTables.Rows[dtTables.Rows.Count - 1]["TABLE_NAME"].ToString();

        OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + tblstr + "]", cn);
        OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);

        DataSet dataset = new DataSet();
        adapter.Fill(dataset);
        adapter.FillSchema(dataset, SchemaType.Source);
        DataTable table;
        table = dataset.Tables[0];
        cn.Close();
        filescan = filescan.Replace(@"\", @"\\");
        string strConns = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filescan + ";" +
                          "Extended Properties='Excel 8.0;HDR=No;IMEX=1;READONLY=FALSE'";

        OleDbConnection cns;
        DataTable dtScanTables;
        cns = new OleDbConnection(strConns);
        cns.Open();
        dtScanTables = cns.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        string tblscan = dtScanTables.Rows[0]["TABLE_NAME"].ToString();
        OleDbDataAdapter secCommand = new OleDbDataAdapter();
        secCommand.SelectCommand = new OleDbCommand("SELECT * FROM [" + tblscan + "]", cns);
        DataSet secDataSet = new DataSet();
        secCommand.Fill(secDataSet);
        DataTable scan;
        scan = secDataSet.Tables[0];
        cns.Close();
        try
        {
            // For each row, print the values of each column.
            string dataprelevare = "";
            double litri = 0.0;
            bool isNum;
            string nrmatricol = "";
            string codbare = "";
            string prelevator = "";
            string serieanaliza = "";
            Ferme_CCL ferma = new Ferme_CCL();
            string filename = Path.GetFileNameWithoutExtension(filescan);
            string[] tokens = filename.Split('_');
            string codexpl = string.Empty;
            if (tokens.Count() > 2)
            {
                codexpl = tokens[2].Replace(" ", "").Replace("-", "");
            }
            if (string.IsNullOrEmpty(codexpl))
            {
                throw new Exception(string.Format("Nu am putut extrage cod de exploatatie din numele fisierului: {0}", filename));
            }
            loggerReceptie.Info("Am obtinut codul de exploatatie: {0} din numele fisierului: {1}", codexpl, filename);
            string numeFerma;
            int fermaid = ferma.GetFermeExpl(codexpl, out numeFerma);
            if (fermaid <= 0)
            {
                throw new Exception(string.Format("Nu am gasit ID pentru codul de exploatatie: {0}", codexpl));
            }
            loggerReceptie.Info("Am gasit ID-ul de ferma: {0} cu numele {1} pentrul codul de exploatatie: {2}", fermaid, numeFerma, codexpl);

            List<Order> orders = new List<Order>();
            OrderManager oManager = new OrderManager(ctx);

            for (int j = 1; j < scan.Rows.Count; j++)
            {
                DataRow row = scan.Rows[j];
                serieanaliza = row[0].ToString().Trim();
                nrmatricol = row[1].ToString().Trim();
                codbare = row[2].ToString().Trim();
                dataprelevare = row[3].ToString().Trim();
                string cant = row[4].ToString().Trim();
                prelevator = row[5].ToString().Trim();
                if (cant.Length > 0)
                {
                    isNum = double.TryParse(cant, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"),
                        out litri);
                }

                bool found = false;
                if (codbare.Length > 0 && nrmatricol.Length > 0)
                {
                    for (int i = 2; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        if (codbare.Equals(dataset.Tables[0].Rows[i][4].ToString().Trim()))
                        {
                            found = true;
                            dataset.Tables[0].Rows[i][0] = serieanaliza;
                            dataset.Tables[0].Rows[i][3] = nrmatricol;
                            dataset.Tables[0].Rows[i][1] = prelevator;
                            dataset.Tables[0].Rows[i][5] = cant;
                            dataset.Tables[0].Rows[i][7] = dataprelevare;
                            dataset.Tables[0].Rows[i].AcceptChanges();
                            break;
                        }
                    }
                    if (!found)
                    {
                        loggerReceptie.Info("Codul de bare: {0} - {1} nu exista!", codbare, nrmatricol);
                    }
                }
            }
            //  adapter.Update(dataset.Tables[0]);
            dataset.Tables[0].AcceptChanges();
            dataset.AcceptChanges();

            if (false)
            {
                WriteMergedFileForDebug(dataset);
            }

            table = dataset.Tables[0];
            //
            // For each row, print the values of each column.
            DateTime dataPrelevare = DateTime.Now;
            string ferma_ccl_cod = "";
            string crot = "";
            string rasa = "";
            int ferma_ccl_id = 0;
            int fabricaID = 0;
            int header = 0;
            bool error = false;
            string strcodbare = "";
            string prelevatorini = table.Rows[0][1].ToString();
            DateTime datat_testare = new DateTime(int.Parse(tblstr.Substring(8, 4)), int.Parse(tblstr.Substring(6, 2)), int.Parse(tblstr.Substring(4, 2)));

            error = !VerificareFisierReceptie(table, 8, 7);
            if (error)
            {
                loggerReceptie.Error("Eroare la verificare fisierului, oprire import");
                return;
            }

            foreach (DataRow row in table.Rows)
            {
                header++;

                if (header > 2 && row[3].ToString().Trim() != "")
                {
                    error = false;
                    crot = row[3].ToString().Trim();
                    strcodbare = row[4].ToString().Trim();
                    if (ExistaCodBare(strcodbare) != 0)
                    {
                        loggerReceptie.Info("Codul de bare: {0} exista deja!", strcodbare);
                        error = true;
                    }
                    else
                    {
                        //	strDataPrelevare = row[5].ToString().Trim().Replace(@".", @"\");
                        //try
                        //{
                        dataPrelevare = DateTime.ParseExact(row[7].ToString().Trim().Substring(0, 10), "dd/MM/yyyy", new CultureInfo("fr-FR"));
                        //}
                        //catch (Exception de)
                        //{
                        //    loggerReceptie.Error("Eroare la interpretarea date pentru crotalie: {0}, data: {1}, eroare: {2}", crot, dataPrelevare.ToShortDateString(), de.Message);
                        //}
                        prelevator = row[1].ToString().Trim();

                        string lit = row[5].ToString().Trim();
                        ferma_ccl_cod = crot;
                        ferma_ccl_id = fermaid;


                        if (ferma_ccl_id == 0)
                        {
                            error = true;
                            loggerReceptie.Error("Nu exista ferma pt. codul de exploatatie:" + codexpl);
                        }
                        else
                        {
                            string crotalia = ferma.GetFermeCrot(crot, fermaid);
                            if (crotalia == null || crotalia.Trim().Length == 0)
                                loggerReceptie.Error("Nu exista crotalie pt. codul: {0}, crotalia: {1}", strcodbare, crot);
                        }
                        string farmName;
                        fabricaID = ferma.GetFerma(ferma_ccl_id, out farmName);
                        rasa = ferma.GetFermeRasa(crot, fermaid);
                        if (!error)
                        {
                            try
                            {
                                string nrComanda = row[0].ToString().Trim();

                                DateTime dataPrimirii = DateTime.ParseExact(row[8].ToString().Trim(), "dd/MM/yyyy", new CultureInfo("fr-FR"));

                                AddMostra(0, Convert.ToInt32(row[2].ToString().Trim()), strcodbare, nrComanda,
                                    ferma_ccl_cod, ferma_ccl_id, fabricaID, "", lit
                                    , "0", "0", "0", "0", dataPrelevare, "0", dataPrimirii,
                                    "0", datat_testare, "0", 0, prelevator, 0, 0, datat_testare, "0", "0", "0", rasa);
                                //loggerReceptie.Info("Mostra cod bare {0} pentru ferma Id {1}, fabrica Id {2}, nr comanda {3} adaugata cu succes.", strcodbare, ferma_ccl_id, fabricaID, nrComanda);
                                Order currentOrder = orders.FirstOrDefault(o => o.FullOrderNumber == nrComanda);
                                if (currentOrder != null)
                                {
                                    currentOrder.SampleCount++;
                                    currentOrder.AnalyzedSampleCount++;
                                }
                                else
                                {
                                    string[] arr = nrComanda.Split('-', '/');
                                    int orderNumber;
                                    if (arr.Length == 3 && int.TryParse(arr[1], out orderNumber))
                                    {
                                        currentOrder = new Order
                                        {
                                            AnalyzedSampleCount = 1,
                                            ClientId = fabricaID > 0 ? fabricaID : -1,
                                            ClientName = farmName + " (" + fabricaID + ")",
                                            FullOrderNumber = nrComanda,
                                            Imported = true,
                                            OrderNumber = orderNumber,
                                            ReceivedDate = dataPrimirii,
                                            SampleCount = 1,
                                            SampleDate = dataPrelevare
                                        };
                                        orders.Add(currentOrder);
                                    }
                                    else
                                    {
                                        loggerReceptie.Error("Adaugare comanda esuata: {0}", nrComanda);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                loggerReceptie.Error("Adaugare mostra esuata: {0} , eroare: {1}", strcodbare, ex.Message);
                            }
                        }
                    }
                }
            }

            oManager.Save(orders);
            ctx.SaveChanges();

            loggerReceptie.Info("Importarea fisierului de receptie a fost efectuata!");
            string filepath_ver = filepath.Replace(".xls", "_" + tblstr.Substring(1, tblstr.IndexOf("cop") - 5) + ".xls");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText =
                "SELECT NrComanda, NumePrelevator,IdZilnic,CodFerma,CodBare,CantitateLaPrelevare,DataPrelevare,DataPrimirii from MostreTancuri Where DataTestare = CONVERT(date, '" +
                datat_testare.ToShortDateString() + "', 103)";
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            table = ds.Tables[0];
            CreateExcelFile(table, filepath_ver);
        }
        catch (Exception ex)
        {
            loggerReceptie.Error("Eroare receptie: {0}", ex.Message);
        }
        finally
        {
            cnn.Close();
            cn.Close();
        }
    }

    private void WriteMergedFileForDebug(DataSet dataset)
    {
        string path = @"C:\Users\imrez\Desktop\FCCL\reimport\fill.xls";
        using (StreamWriter sw = new StreamWriter(path))
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                DataGrid dg = new DataGrid {DataSource = dataset.Tables[0]};
                dg.DataBind();
                dg.RenderControl(htw);
            }
        }
    }

    public static bool CreateExcelFile(DataTable dt, string filename)
    {
        try
        {
            string sTableStart = @"<HTML><BODY><TABLE Border=1>";
            string sTableEnd = @"</TABLE></BODY></HTML>";
            string sTHead = "<TR>";
            StringBuilder sTableData = new StringBuilder();


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn col = dt.Columns[i];
                string colname = col.ColumnName;
                switch (i)
                {
                    case 0:
                        colname = "Seria PV/Data<br/>Cerere analiza";
                        break;
                    case 1:
                        colname = "Nume/Cod Prelevator<br/>&nbsp;";
                        break;
                    case 2:
                        colname = "ID laborator<br/>&nbsp;";
                        break;
                    case 3:
                        colname = "Nr Matricol<br/>&nbsp;";
                        break;
                    case 4:
                        colname = "Cod identificare proba<br>&nbsp;";
                        break;
                    case 5:
                        colname = "Cantitate<br/>(l)";
                        break;
                    case 6:
                        colname = "Data prelevare<br/>&nbsp;";
                        break;
                    case 7:
                        colname = "Data primirii<br/>&nbsp;";
                        break;

                    default:
                        break;
                }

                sTHead += @"<TH>" + colname + @"</TH>";
                if (i == 5)
                    sTHead += @"<TH>" + "Tipul de analiza<br/>a),b),c),d),e)*" + @"</TH>";
            }

            sTHead += @"</TR>";

            foreach (DataRow row in dt.Rows)
            {
                sTableData.Append(@"<TR>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string text = row[i].ToString().Trim();
                    if (i == 7 && text.Length > 0)
                        text = text.Substring(0, 10);
                    sTableData.Append(@"<TD>" + text + @"</TD>");
                    if (i == 5)
                        sTableData.Append(@"<TD align='center'>" + "b,c" + @"</TD>");
                }
                sTableData.Append(@"</TR>");
            }


            string sTable = sTableStart + sTHead + sTableData + sTableEnd;
            if (File.Exists(filename))
                File.Delete(filename);
            StreamWriter oExcelWriter = File.CreateText(filename);
            oExcelWriter.WriteLine(sTable);
            oExcelWriter.Close();
            return true;
        }
        catch (Exception ex)
        {
            logger.ErrorException("|CreateExcelFile", ex);
            return false;
        }
    }

    public void UpdateMostraP(string codbare, string prelid, bool isFcb)
    {
        SqlConnection cnn = new SqlConnection(CopConnectionString);
        string query;
        if (!isFcb)
            query = "UPDATE MostreTancuri set PrelevatorId =" + prelid + ", CodFerma='" + codbare.Substring(0, 5) +
                    "' WHERE Codbare Like'" + codbare + "'";
        else
            query = "UPDATE MostreTancuri set PrelevatorId =0, NumePrelevator='" + prelid + "', CodFerma='" +
                    codbare.Substring(0, 5) + "' WHERE Codbare Like'" + codbare + "'";

        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }

    public void InsertCrot(string codbare, string crot, string rasa, string datanasterii, string nume, int fermaid)
    {
        SqlConnection cnn = new SqlConnection(CopConnectionString);
        string query = "INSERT INTo Crotalii (CodBare, Crotalia, Rasa, DataNasterii, Nume, FermaId) VALUES('" + codbare +
                       "','" + crot + "','" + rasa + "','" + datanasterii + "','" + nume + "'," + fermaid + ")";

        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }

    public void UpdateCrot(string codbare, string crot, string rasa, string datanasterii, string nume, int fermaid)
    {
        SqlConnection cnn = new SqlConnection(CopConnectionString);
        string query = "UPDATE Crotalii SET Crotalia ='" + crot + "',Rasa='" + rasa + "',DataNasterii ='" + datanasterii +
                       "',Nume='" + nume + "'" + " WHERE FermaId=" + fermaid + " AND CodBare = '" + codbare + "'";

        SqlCommand cmd = new SqlCommand(query, cnn);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }

    public DataSet SearchMostre(string codbarec, string datatestare1, string datatestare2, string antibiotice,
        string pctinghetini, string pctinghetfin, string ntgini, string ntgfin, string ncsini, string ncsfin)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);

        // Mark the Command as a SPROC
        myConnection.Open();

        SqlDataAdapter myCommand = new SqlDataAdapter();
        string query_str = " SELECT  top 3000 * from MostreTancuri ";
        if (codbarec != "" || datatestare1 != "" || datatestare2 != "" || antibiotice != "Toate" ||
            pctinghetini != "" || pctinghetfin != "" || ntgini != "" || ntgfin != "" || ncsini != "" || ncsfin != "")
            query_str += " Where ";
        else
            query_str = " SELECT top 1000 * from MostreTancuri ";
        string query_cond = "";

        if (codbarec != "")
            query_cond += " (codbare LIKE '%" + codbarec + "%' ) ";
        if (antibiotice != "Toate")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (antibiotice == "Pozitiv")
                query_cond += " (Antibiotice ='1' OR Antibiotice = '-1') ";
            if (antibiotice == "Negativ")
                query_cond += " (Antibiotice = '0') ";
            if (antibiotice == "Netestat")
                query_cond += " (Antibiotice = '') ";
        }

        if (datatestare1 != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            query_cond += " CONVERT(datetime, DataTestare, 103)>= CONVERT(datetime, '" + datatestare1 + "', 103) ";
        }
        if (datatestare2 != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            query_cond += " CONVERT(datetime, DataTestare, 103)<= CONVERT(datetime, '" + datatestare2 + "', 103) ";
        }
        if (pctinghetini != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            query_cond += " PunctInghet <= " + pctinghetini.Substring(3, pctinghetini.Length - 3) +
                          " AND PunctInghet <> 0 ";
        }
        if (pctinghetfin != "")
        {
            if (pctinghetfin == "0")
            {
                if (query_cond != "")
                    query_cond += " OR ";
                query_cond += " PunctInghet = 0.00001 ";
            }
            else
            {
                if (query_cond != "")
                    query_cond += " AND ";
                query_cond += " PunctInghet >= " + pctinghetfin.Substring(3, pctinghetfin.Length - 3) +
                              " AND PunctInghet <> 0 ";
            }
        }
        if (ntgini != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ntgini == "0")
                query_cond += " IncarcaturaGermeni > 0 ";
            else
                query_cond += " IncarcaturaGermeni >= " + ntgini + " AND IncarcaturaGermeni <> 0 ";
        }
        if (ntgfin != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ntgfin == "0")
                query_cond += " IncarcaturaGermeni < 1 ";
            else
                query_cond += " IncarcaturaGermeni <= " + ntgfin + " AND IncarcaturaGermeni <> 0 ";
        }
        if (ncsini != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ncsini == "0")
                query_cond += " NumarCeluleSomatice > 0 ";
            else
                query_cond += " NumarCeluleSomatice >= " + ncsini + " AND NumarCeluleSomatice <> 0 ";
        }
        if (ncsfin != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ncsfin == "0")
                query_cond += " NumarCeluleSomatice < 1 ";
            else
                query_cond += " NumarCeluleSomatice <= " + ncsfin + " AND NumarCeluleSomatice <> 0 ";
        }
        query_str += query_cond;

        query_str += " order by CONVERT(datetime, DataTestare, 103) desc, IdZilnic  ";

        myCommand.SelectCommand = new SqlCommand(query_str, myConnection);

        // Create the DataSet	
        DataSet myDataSet = new DataSet();

        myCommand.Fill(myDataSet);

        myConnection.Close();

        // Return the DataSet
        return (myDataSet);
    }

    public int SearchMostre_Count(string codbarec, string datatestare1, string datatestare2, string antibiotice,
        string pctinghetini, string pctinghetfin, string ntgini, string ntgfin, string ncsini, string ncsfin)
    {
        int TRecords;
        SqlConnection myConnection = new SqlConnection(CopConnectionString);

        myConnection.Open();
        string query_str = " SELECT  Count(*) as cnt from MostreTancuri ";
        if (codbarec != "" || datatestare1 != "" || datatestare2 != "" || antibiotice != "Toate" ||
            pctinghetini != "" || pctinghetfin != "" || ntgini != "" || ntgfin != "" || ncsini != "" || ncsfin != "")
            query_str += " Where ";
        else
            query_str = " SELECT top 1000 * from MostreTancuri ";
        string query_cond = "";

        if (codbarec != "")
            query_cond += " (codbare LIKE '%" + codbarec + "%' ) ";
        if (antibiotice != "Toate")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (antibiotice == "Pozitiv")
                query_cond += " (Antibiotice ='1' OR Antibiotice = '-1') ";
            if (antibiotice == "Negativ")
                query_cond += " (Antibiotice = '0') ";
            if (antibiotice == "Netestat")
                query_cond += " (Antibiotice = '') ";
        }

        if (datatestare1 != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            query_cond += " CONVERT(datetime, DataTestare, 103)>= CONVERT(datetime, '" + datatestare1 + "', 103) ";
        }
        if (datatestare2 != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            query_cond += " CONVERT(datetime, DataTestare, 103)<= CONVERT(datetime, '" + datatestare2 + "', 103) ";
        }
        if (pctinghetini != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            query_cond += " PunctInghet <= " + pctinghetini.Substring(3, pctinghetini.Length - 3) +
                          " AND PunctInghet <> 0 ";
        }
        if (pctinghetfin != "")
        {
            if (pctinghetfin == "0")
            {
                if (query_cond != "")
                    query_cond += " OR ";
                query_cond += " PunctInghet = 0.00001 ";
            }
            else
            {
                if (query_cond != "")
                    query_cond += " AND ";
                query_cond += " PunctInghet >= " + pctinghetfin.Substring(3, pctinghetfin.Length - 3) +
                              " AND PunctInghet <> 0 ";
            }
        }
        if (ntgini != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ntgini == "0")
                query_cond += " IncarcaturaGermeni > 0 ";
            else
                query_cond += " IncarcaturaGermeni >= " + ntgini + " AND IncarcaturaGermeni <> 0 ";
        }
        if (ntgfin != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ntgfin == "0")
                query_cond += " IncarcaturaGermeni < 1 ";
            else
                query_cond += " IncarcaturaGermeni <= " + ntgfin + " AND IncarcaturaGermeni <> 0 ";
        }
        if (ncsini != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ncsini == "0")
                query_cond += " NumarCeluleSomatice > 0 ";
            else
                query_cond += " NumarCeluleSomatice >= " + ncsini + " AND NumarCeluleSomatice <> 0 ";
        }
        if (ncsfin != "")
        {
            if (query_cond != "")
                query_cond += " AND ";
            if (ncsfin == "0")
                query_cond += " NumarCeluleSomatice < 1 ";
            else
                query_cond += " NumarCeluleSomatice <= " + ncsfin + " AND NumarCeluleSomatice <> 0 ";
        }
        query_str += query_cond;

        SqlCommand myCommand = new SqlCommand(query_str, myConnection);
        SqlDataReader reader;
        reader = myCommand.ExecuteReader();
        if (reader.Read())
            TRecords = Convert.ToInt32(reader["cnt"]);
        else
            TRecords = 0;
        myConnection.Close();

        return (TRecords);
    }


    public int GetFermeCCL_ID(string cod)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetFermaCCL_ID", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parametercod = new SqlParameter("@Cod", SqlDbType.NVarChar, 150);
        parametercod.Value = cod;
        myCommand.Parameters.Add(parametercod);

        SqlParameter parameterID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterID);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        if (parameterID.Value == DBNull.Value)
            return 0;
        return (int)parameterID.Value;
    }


    public string MyRound(string str)
    {
        int pos;
        pos = str.IndexOf(".");
        if (pos > 0)
        {
            int p;
            p = str.Length - (pos + 2);
            if (p > 0)
                return str.Remove(pos + 2, str.Length - (pos + 2));
            return str;
        }
        return str;
    }

    public string GetQuery(string campo_search)
    {
        string keyword_s;
        string query_str = null;
        string expresion;
        query_str = "SELECT ID,CodBare " +
            //			"ContentSize, Description, Title, ExpireDate, CreatedById, Portal_Documents.DomainId, " +
            //			"FileName1, FileName2, DomainName, Email "  +
                    "FROM MostreTancuri ";
        //				" AND Portal_Domains.DomainId = Portal_Documents.DomainId ";
        string[] keywordsExclus = { "and", "or", "the", "in", "of", "if", "is" };
        bool primoword = true;
        bool expres = false;
        for (int k = 0; k < campo_search.Length; k++)
        {
            keyword_s = null;
            expresion = null;
            if (campo_search[k] == '\"' && !expres)
            {
                expres = true;
                while (k < campo_search.Length && campo_search[++k] != '\"')
                    expresion += campo_search[k];
                if (expresion != null)
                    query_str += " AND (Title LIKE '%" + expresion + "%' OR codbare LIKE '%" + expresion + "%')";
            }
            else if (!expres)
            {
                while (k < campo_search.Length && campo_search[k] != ' ')
                {
                    keyword_s += campo_search[k];
                    k++;
                }
                bool word_ok = true;
                for (int j = 0; keyword_s != null && j < keywordsExclus.Length; j++)
                    if (keyword_s == keywordsExclus[j])
                        word_ok = false;
                if (word_ok && keyword_s != null)
                {
                    if (primoword)
                    {
                        primoword = false;
                        query_str += " AND (Title LIKE '%" + keyword_s + "%' OR codbare LIKE '%" + keyword_s + "%' ";
                    }
                    else
                        query_str += " OR Title LIKE '%" + keyword_s + "%' OR codbare LIKE '%" + keyword_s + "%'";
                }
            }
        }
        if (!expres && campo_search.Length > 0)
            query_str += " ) ";
        //		query_str += " ORDER BY DomainName ";
        return query_str;
    }

    public string GetPrelevator(int prelevatorID)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetPrelevator", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterPrelevatorID = new SqlParameter("@PrelevatorID", SqlDbType.Int, 4);
        parameterPrelevatorID.Value = prelevatorID;
        myCommand.Parameters.Add(parameterPrelevatorID);

        SqlParameter parameterNumePrelevator = new SqlParameter("@NumePrelevator", SqlDbType.NVarChar, 150);
        parameterNumePrelevator.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterNumePrelevator);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterNumePrelevator.Value == DBNull.Value)
            return "";
        return (string)parameterNumePrelevator.Value;
    }

    public SqlDataReader GetSingleMostre(int itemId)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetSingleMostre", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemId = new SqlParameter("@ItemID", SqlDbType.Int, 4);
        parameterItemId.Value = itemId;
        myCommand.Parameters.Add(parameterItemId);

        // Execute the command
        myConnection.Open();
        SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

        // Return the datareader 
        return result;
    }

    public SqlDataReader GetDateCodBare(string codbare)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetDateCodBare", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parametercodbare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 150);
        parametercodbare.Value = codbare;
        myCommand.Parameters.Add(parametercodbare);

        // Execute the command
        myConnection.Open();
        SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

        // Return the datareader 
        return result;
    }

    public SqlDataReader GetDateDataTestareIdZilnic(DateTime dataTestare, int idZilnic)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetDateDataTestareIdZilnic", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.Date);
        parameterDataTestare.Value = dataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
        parameterIdZilnic.Value = idZilnic;
        myCommand.Parameters.Add(parameterIdZilnic);

        // Execute the command
        myConnection.Open();
        SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

        // Return the datareader 
        return result;
    }

    public int GetMaxItemID()
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetMaxItemID", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemID = new SqlParameter("@maxItemID", SqlDbType.Int, 4);
        parameterItemID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterItemID);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterItemID.Value == DBNull.Value)
            return 0;
        return (int)parameterItemID.Value;
    }

    public int GetMaxIdZilnic(string DataTestare)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetMaxIdZilnic", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemID = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
        parameterItemID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
        parameterDataTestare.Value = DataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterItemID.Value == DBNull.Value)
            return 0;
        return (int)parameterItemID.Value;
    }

    public void DeleteMostra(int itemID)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_DeleteMostra", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
        parameterItemID.Value = itemID;
        myCommand.Parameters.Add(parameterItemID);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public int ExistaCodBare(string codbare)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_ExistaCodBare", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parametermostraID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parametermostraID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parametermostraID);

        SqlParameter parametercodbare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 150);
        parametercodbare.Value = codbare;
        myCommand.Parameters.Add(parametercodbare);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parametermostraID.Value == DBNull.Value)
            return 0;
        return (int)parametermostraID.Value;
    }

    public int ExistaCodBareDataTestare(string codbare, string DataTestare)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_ExistaCodBareDataTestare", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parametermostraID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parametermostraID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parametermostraID);

        SqlParameter parametercodbare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 150);
        parametercodbare.Value = codbare;
        myCommand.Parameters.Add(parametercodbare);

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 150);
        parameterDataTestare.Value = DataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parametermostraID.Value == DBNull.Value)
            return 0;
        return (int)parametermostraID.Value;
    }

    public int ExistaIdZilnic(string DataTestare, int IdZilnic)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_ExistaIdZilnic", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parametermostraID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parametermostraID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parametermostraID);

        SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
        parameterIdZilnic.Value = IdZilnic;
        myCommand.Parameters.Add(parameterIdZilnic);

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 150);
        parameterDataTestare.Value = DataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parametermostraID.Value == DBNull.Value)
            return 0;
        return (int)parametermostraID.Value;
    }

    //*********************************************************************
    //
    // AddWorkDoc Method
    //
    // The AddWorkDoc method adds a new WorkDoc to the
    // Documents database table, and returns the ItemId value as a result.
    //
    // Other relevant sources:
    //     + <a href="AddWorkDoc.htm" style="color:green">AddWorkDoc Stored Procedure</a>
    //
    //*********************************************************************

    public int AddMostra(int itemId, int IdZilnic, String CodBare, string NrComanda, string CodFerma, int FermaID,
        int FabricaID, string Antibiotice, String CantitatePrelevare, String NCS, String Grasime,
        String NTG, String Proteine, DateTime DataPrelevare, String Lactoza, DateTime DataPrimirii,
        String SubstantaUscata, DateTime DataTestare, String PunctInghet, int Prelevator, string NumePrelevator,
        int Definitiv, int Validare, DateTime DataTestareFinala, string Urea, string Ph, string Casein, string Rasa)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_AddMostra", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = CodBare;
        myCommand.Parameters.Add(parameterCodBare);

        SqlParameter parameterNrComanda = new SqlParameter("@NrComanda", SqlDbType.NVarChar, 150);
        parameterNrComanda.Value = NrComanda;
        myCommand.Parameters.Add(parameterNrComanda);

        SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
        parameterIdZilnic.Value = IdZilnic;
        myCommand.Parameters.Add(parameterIdZilnic);

        SqlParameter parameterCodFerma = new SqlParameter("@CodFerma", SqlDbType.NVarChar, 255);
        parameterCodFerma.Value = CodFerma;
        myCommand.Parameters.Add(parameterCodFerma);

        SqlParameter parameterFermaID = new SqlParameter("@FermaID", SqlDbType.Int, 4);
        parameterFermaID.Value = FermaID;
        myCommand.Parameters.Add(parameterFermaID);

        SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
        parameterFabricaID.Value = FabricaID;
        myCommand.Parameters.Add(parameterFabricaID);

        SqlParameter parameterAntibiotice = new SqlParameter("@Antibiotice", SqlDbType.NVarChar, 255);
        parameterAntibiotice.Value = Antibiotice;
        myCommand.Parameters.Add(parameterAntibiotice);

        SqlParameter parameterRasa = new SqlParameter("@Rasa", SqlDbType.NChar, 10);
        parameterRasa.Value = Rasa;
        myCommand.Parameters.Add(parameterRasa);

        SqlParameter parameterCantitatePrelevare = new SqlParameter("@CantitatelaPrelevare", SqlDbType.Float, 8);
        parameterCantitatePrelevare.Value = Utils.ConvertToDouble(CantitatePrelevare);
        myCommand.Parameters.Add(parameterCantitatePrelevare);

        SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
        parameterNCS.Value = Utils.ConvertToDouble(NCS);
        myCommand.Parameters.Add(parameterNCS);

        SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
        parameterNTG.Value = Utils.ConvertToDouble(NTG);
        myCommand.Parameters.Add(parameterNTG);

        SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
        parameterGrasime.Value = Utils.ConvertToDouble(Grasime);
        myCommand.Parameters.Add(parameterGrasime);

        SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
        parameterProteine.Value = Utils.ConvertToDouble(Proteine);
        myCommand.Parameters.Add(parameterProteine);

        SqlParameter parameterDataPrelevare = new SqlParameter("@DataPrelevare", SqlDbType.Date);
        parameterDataPrelevare.Value = DataPrelevare;
        myCommand.Parameters.Add(parameterDataPrelevare);

        SqlParameter parameterNumePrel = new SqlParameter("@NumePrelevator", SqlDbType.NVarChar, 255);
        parameterNumePrel.Value = NumePrelevator;
        myCommand.Parameters.Add(parameterNumePrel);

        SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
        parameterLactoza.Value = Convert.ToDecimal(Lactoza, CultureInfo.InvariantCulture);
        myCommand.Parameters.Add(parameterLactoza);

        SqlParameter parameterDataPrimirii = new SqlParameter("@DataPrimirii", SqlDbType.Date);
        parameterDataPrimirii.Value = DataPrimirii;
        myCommand.Parameters.Add(parameterDataPrimirii);

        SqlParameter parameterSubstantaUscata = new SqlParameter("@SubstantaUscata", SqlDbType.Float, 8);
        parameterSubstantaUscata.Value = Utils.ConvertToDouble(SubstantaUscata);
        myCommand.Parameters.Add(parameterSubstantaUscata);

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.Date);
        parameterDataTestare.Value = DataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
        parameterPunctInghet.Value = Utils.ConvertToDouble(PunctInghet);
        myCommand.Parameters.Add(parameterPunctInghet);

        SqlParameter parameterPrelevator = new SqlParameter("@PrelevatorID", SqlDbType.Int, 4);
        parameterPrelevator.Value = Prelevator;
        myCommand.Parameters.Add(parameterPrelevator);

        SqlParameter parameterDefinitiv = new SqlParameter("@Definitiv", SqlDbType.Int, 4);
        parameterDefinitiv.Value = Definitiv;
        myCommand.Parameters.Add(parameterDefinitiv);

        SqlParameter parameterValidare = new SqlParameter("@Validare", SqlDbType.Int, 4);
        parameterValidare.Value = Validare;
        myCommand.Parameters.Add(parameterValidare);

        SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.Date);
        parameterDataTestareFinala.Value = DataTestareFinala;
        myCommand.Parameters.Add(parameterDataTestareFinala);

        SqlParameter parameterUrea = new SqlParameter("@Urea", SqlDbType.Float, 8);
        parameterUrea.Value = Utils.ConvertToDouble(Urea);
        myCommand.Parameters.Add(parameterUrea);

        SqlParameter parameterPh = new SqlParameter("@PH", SqlDbType.Float, 8);
        parameterPh.Value = Utils.ConvertToDouble(Ph);
        myCommand.Parameters.Add(parameterPh);

        SqlParameter parameterCasein = new SqlParameter("@Casein", SqlDbType.Float, 8);
        parameterCasein.Value = Utils.ConvertToDouble(Casein);
        myCommand.Parameters.Add(parameterCasein);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterItemID.Value == DBNull.Value)
            return 0;
        return (int)parameterItemID.Value;

        /*
                  myCommand.CommandText =  "INSERT INTO MOSTRETANCURI " 
       +"(CodBare,NrComanda,IdZilnic,PrelevatorID,CodFerma,FermaID,DataTestare,DataTestareFinala,DataPrelevare,DataPrimirii,Definitiv,Validat,"
       + "Antibiotice,CantitateLaPrelevare,NumarCeluleSomatice,IncarcaturaGermeni,Grasime,ProcentProteine,ProcentLactoza,SubstantaUscata,PunctInghet,PrezentaInhibitori,CreareManuala) VALUES "
       +"(@CodBare,@NrComanda,@IdZilnic,@PrelevatorID,@CodFerma,@FermaID,@DataTestare,@DataTestareFinala,@DataPrelevare,@DataPrimirii,@Definitiv,@Validat,"
       +"@Antibiotice,@CantitateLaPrelevare,@NumarCeluleSomatice,@IncarcaturaGermeni,@Grasime,@ProcentProteine,@ProcentLactoza,@SubstantaUscata,@PunctInghet,0,0)";
      */
    }

    public void UpdateMostraFCB(string CodBare, string NrComanda, string NumePrelevator,
        string NumeProba, string NumeClient, string Adresa, string Localitate, string Judet)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraFCB", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = CodBare;
        myCommand.Parameters.Add(parameterCodBare);

        SqlParameter parameterNrComanda = new SqlParameter("@NrComanda", SqlDbType.NVarChar, 150);
        parameterNrComanda.Value = NrComanda;
        myCommand.Parameters.Add(parameterNrComanda);

        SqlParameter parameterNumePrelevator = new SqlParameter("@NumePrelevator", SqlDbType.NVarChar, 150);
        parameterNumePrelevator.Value = NumePrelevator;
        myCommand.Parameters.Add(parameterNumePrelevator);

        SqlParameter parameterNumeProba = new SqlParameter("@NumeProba", SqlDbType.NVarChar, 150);
        parameterNumeProba.Value = NumeProba;
        myCommand.Parameters.Add(parameterNumeProba);

        SqlParameter parameterNumeClient = new SqlParameter("@NumeClient", SqlDbType.NVarChar, 255);
        parameterNumeClient.Value = NumeClient;
        myCommand.Parameters.Add(parameterNumeClient);

        SqlParameter parameterAdresa = new SqlParameter("@Adresa", SqlDbType.NVarChar, 255);
        parameterAdresa.Value = Adresa;
        myCommand.Parameters.Add(parameterAdresa);

        SqlParameter parameterLocalitate = new SqlParameter("@Localitate", SqlDbType.NVarChar, 255);
        parameterLocalitate.Value = Localitate;
        myCommand.Parameters.Add(parameterLocalitate);

        SqlParameter parameterJudet = new SqlParameter("@Judet", SqlDbType.NVarChar, 255);
        parameterJudet.Value = Judet;
        myCommand.Parameters.Add(parameterJudet);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();

        //			return (int)parameterItemID.Value;
    }


    public int AddMostraFCB(int itemId, int IdZilnic, string CodBare, string NrComanda, string NumePrelevator,
        string NumeProba, DateTime DataPrelevare, DateTime DataPrimirii, DateTime DataTestare,
        DateTime DataTestareFinala, string NumeClient, string Adresa, string Localitate, string Judet,
        int Definitiv, int Validat, string NCS, string Grasime, string NTG, string Proteine, string Lactoza,
        string PunctInghet, string Antibiotice, string Urea, string Ph, string Casein)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_AddMostraFCB", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterItemID);


        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = CodBare;
        myCommand.Parameters.Add(parameterCodBare);

        SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
        parameterIdZilnic.Value = IdZilnic;
        myCommand.Parameters.Add(parameterIdZilnic);

        SqlParameter parameterNrComanda = new SqlParameter("@NrComanda", SqlDbType.NVarChar, 150);
        parameterNrComanda.Value = NrComanda;
        myCommand.Parameters.Add(parameterNrComanda);

        SqlParameter parameterNumePrelevator = new SqlParameter("@NumePrelevator", SqlDbType.NVarChar, 150);
        parameterNumePrelevator.Value = NumePrelevator;
        myCommand.Parameters.Add(parameterNumePrelevator);

        SqlParameter parameterNumeProba = new SqlParameter("@NumeProba", SqlDbType.NVarChar, 150);
        parameterNumeProba.Value = NumeProba;
        myCommand.Parameters.Add(parameterNumeProba);

        SqlParameter parameterDataPrelevare = new SqlParameter("@DataPrelevare", SqlDbType.Date);
        parameterDataPrelevare.Value = DataPrelevare;
        myCommand.Parameters.Add(parameterDataPrelevare);

        SqlParameter parameterDataPrimirii = new SqlParameter("@DataPrimirii", SqlDbType.Date);
        parameterDataPrimirii.Value = DataPrimirii;
        myCommand.Parameters.Add(parameterDataPrimirii);

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.Date);
        parameterDataTestare.Value = DataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.Date);
        parameterDataTestareFinala.Value = DataTestareFinala;
        myCommand.Parameters.Add(parameterDataTestareFinala);

        SqlParameter parameterNumeClient = new SqlParameter("@NumeClient", SqlDbType.NVarChar, 255);
        parameterNumeClient.Value = NumeClient;
        myCommand.Parameters.Add(parameterNumeClient);

        SqlParameter parameterAdresa = new SqlParameter("@Adresa", SqlDbType.NVarChar, 255);
        parameterAdresa.Value = Adresa;
        myCommand.Parameters.Add(parameterAdresa);

        SqlParameter parameterLocalitate = new SqlParameter("@Localitate", SqlDbType.NVarChar, 255);
        parameterLocalitate.Value = Localitate;
        myCommand.Parameters.Add(parameterLocalitate);

        SqlParameter parameterJudet = new SqlParameter("@Judet", SqlDbType.NVarChar, 255);
        parameterJudet.Value = Judet;
        myCommand.Parameters.Add(parameterJudet);

        SqlParameter parameterDefinitiv = new SqlParameter("@Definitiv", SqlDbType.Int, 4);
        parameterDefinitiv.Value = Definitiv;
        myCommand.Parameters.Add(parameterDefinitiv);

        SqlParameter parameterValidat = new SqlParameter("@Validat", SqlDbType.Int, 4);
        parameterValidat.Value = Validat;
        myCommand.Parameters.Add(parameterValidat);


        SqlParameter parameterAntibiotice = new SqlParameter("@Antibiotice", SqlDbType.NVarChar, 255);
        parameterAntibiotice.Value = Antibiotice;
        myCommand.Parameters.Add(parameterAntibiotice);

        SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
        parameterNCS.Value = Utils.ConvertToDouble(NCS);
        myCommand.Parameters.Add(parameterNCS);

        SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
        parameterNTG.Value = Utils.ConvertToDouble(NTG);
        myCommand.Parameters.Add(parameterNTG);

        SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
        parameterGrasime.Value = Utils.ConvertToDouble(Grasime);
        myCommand.Parameters.Add(parameterGrasime);

        SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
        parameterProteine.Value = Utils.ConvertToDouble(Proteine);
        myCommand.Parameters.Add(parameterProteine);

        SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
        parameterLactoza.Value = Convert.ToDecimal(Lactoza, CultureInfo.InvariantCulture);
        myCommand.Parameters.Add(parameterLactoza);

        SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
        parameterPunctInghet.Value = Utils.ConvertToDouble(PunctInghet);
        myCommand.Parameters.Add(parameterPunctInghet);

        SqlParameter parameterUrea = new SqlParameter("@Urea", SqlDbType.Float, 8);
        parameterUrea.Value = Utils.ConvertToDouble(Urea);
        myCommand.Parameters.Add(parameterUrea);

        SqlParameter parameterPh = new SqlParameter("@PH", SqlDbType.Float, 8);
        parameterPh.Value = Utils.ConvertToDouble(Ph);
        myCommand.Parameters.Add(parameterPh);

        SqlParameter parameterCasein = new SqlParameter("@Casein", SqlDbType.Float, 8);
        parameterCasein.Value = Utils.ConvertToDouble(Casein);
        myCommand.Parameters.Add(parameterCasein);


        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterItemID.Value == DBNull.Value)
            return 0;
        return (int)parameterItemID.Value;
    }


    //*********************************************************************
    //
    // UpdateWorkDoc Method
    //
    // The UpdateWorkDoc method updates the specified WorkDoc within
    // the Documents database table.
    //
    // Other relevant sources:
    //     + <a href="UpdateDocument.htm" style="color:green">UpdateDocument Stored Procedure</a>
    //
    //*********************************************************************

    public void UpdateMostra(int itemId, String CodBare, string NrComanda, string CodFerma, int FermaID, int FabricaID,
        string Antibiotice, String CantitatePrelevare, String NCS, String Grasime,
        String NTG, String Proteine, DateTime DataPrelevare, String Lactoza, DateTime DataPrimirii,
        String SubstantaUscata, DateTime DataTestare, String PunctInghet, int Prelevator, int Definitiv, int Validare,
        DateTime DataTestareFinala)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostra", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Value = itemId;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = CodBare;
        myCommand.Parameters.Add(parameterCodBare);

        SqlParameter parameterNrComanda = new SqlParameter("@NrComanda", SqlDbType.NVarChar, 150);
        parameterNrComanda.Value = NrComanda;
        myCommand.Parameters.Add(parameterNrComanda);

        SqlParameter parameterCodFerma = new SqlParameter("@CodFerma", SqlDbType.NVarChar, 255);
        parameterCodFerma.Value = CodFerma;
        myCommand.Parameters.Add(parameterCodFerma);

        SqlParameter parameterFermaID = new SqlParameter("@FermaID", SqlDbType.Int, 4);
        parameterFermaID.Value = FermaID;
        myCommand.Parameters.Add(parameterFermaID);

        SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
        parameterFabricaID.Value = FabricaID;
        myCommand.Parameters.Add(parameterFabricaID);

        SqlParameter parameterAntibiotice = new SqlParameter("@Antibiotice", SqlDbType.NVarChar, 255);
        parameterAntibiotice.Value = Antibiotice;
        myCommand.Parameters.Add(parameterAntibiotice);

        SqlParameter parameterCantitatePrelevare = new SqlParameter("@CantitatelaPrelevare", SqlDbType.Float, 8);
        parameterCantitatePrelevare.Value = Utils.ConvertToDouble(CantitatePrelevare);
        myCommand.Parameters.Add(parameterCantitatePrelevare);

        SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
        parameterNCS.Value = Utils.ConvertToDouble(NCS);
        myCommand.Parameters.Add(parameterNCS);

        SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
        parameterNTG.Value = Utils.ConvertToDouble(NTG);
        myCommand.Parameters.Add(parameterNTG);

        SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
        parameterGrasime.Value = Utils.ConvertToDouble(Grasime);
        myCommand.Parameters.Add(parameterGrasime);

        SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
        parameterProteine.Value = Utils.ConvertToDouble(Proteine);
        myCommand.Parameters.Add(parameterProteine);

        SqlParameter parameterDataPrelevare = new SqlParameter("@DataPrelevare", SqlDbType.Date);
        parameterDataPrelevare.Value = DataPrelevare;
        myCommand.Parameters.Add(parameterDataPrelevare);

        SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
        parameterLactoza.Value = Convert.ToDecimal(Lactoza, CultureInfo.InvariantCulture);
        myCommand.Parameters.Add(parameterLactoza);

        SqlParameter parameterDataPrimirii = new SqlParameter("@DataPrimirii", SqlDbType.Date);
        parameterDataPrimirii.Value = DataPrimirii;
        myCommand.Parameters.Add(parameterDataPrimirii);

        SqlParameter parameterSubstantaUscata = new SqlParameter("@SubstantaUscata", SqlDbType.Float, 8);
        parameterSubstantaUscata.Value = Utils.ConvertToDouble(SubstantaUscata);
        myCommand.Parameters.Add(parameterSubstantaUscata);

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.Date);
        parameterDataTestare.Value = DataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
        parameterPunctInghet.Value = Utils.ConvertToDouble(PunctInghet);
        myCommand.Parameters.Add(parameterPunctInghet);

        SqlParameter parameterPrelevator = new SqlParameter("@PrelevatorID", SqlDbType.Int, 4);
        parameterPrelevator.Value = Prelevator;
        myCommand.Parameters.Add(parameterPrelevator);

        SqlParameter parameterDefinitiv = new SqlParameter("@Definitiv", SqlDbType.Int, 4);
        parameterDefinitiv.Value = Definitiv;
        myCommand.Parameters.Add(parameterDefinitiv);

        SqlParameter parameterValidare = new SqlParameter("@Validare", SqlDbType.Int, 4);
        parameterValidare.Value = Validare;
        myCommand.Parameters.Add(parameterValidare);

        SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.Date);
        parameterDataTestareFinala.Value = DataTestareFinala;
        myCommand.Parameters.Add(parameterDataTestareFinala);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public void UpdateMostraB(int itemId, String codBare, String NTG, DateTime dataTestareFinala)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraB", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Value = itemId;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = codBare;
        myCommand.Parameters.Add(parameterCodBare);

        SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
        parameterNTG.Value = Utils.ConvertToDouble(NTG);
        myCommand.Parameters.Add(parameterNTG);

        SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.Date);
        parameterDataTestareFinala.Value = dataTestareFinala;
        myCommand.Parameters.Add(parameterDataTestareFinala);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public void UpdateMostraF(string cod_bare, DateTime dataTestareFinala, string ncs, string ph, string urea,
        string substu, string lactoza, string casein, string proteine, string grasime, bool analizatg, bool analizatp,
        bool analizatl, bool analizats, bool analizatn, bool analizat)
    {
        if (!analizatg || !analizatp || !analizats || !analizatl || !analizatn || !analizat)
        {
            SqlConnection cnn = new SqlConnection(CopConnectionString);
            string query = "UPDATE MostreTancuri set ";
            int count = 0;
            if (!analizatg)
            {
                query += "Grasime= " + grasime + ",GrasimeProv=1";
                count++;
            }
            if (!analizatp)
            {
                if (count > 0)
                    query += ",";
                query += "ProcentProteine=" + proteine + ",ProteineProv=1";
                count++;
            }
            if (!analizatl)
            {
                if (count > 0)
                    query += ",";
                query += "ProcentLactoza=" + lactoza + ",LactozaProv=1";
                count++;
            }
            if (!analizats)
            {
                if (count > 0)
                    query += ",";
                query += "SubstantaUscata=" + substu + ",SubstuProv=1";
                count++;
            }
            if (!analizatn)
            {
                if (count > 0)
                    query += ",";
                query += "NumarCeluleSomatice=" + ncs;
                count++;
            }
            if (!analizat)
            {
                if (count > 0)
                    query += ",";
                query += "pH=" + ph + ",Urea=" + urea + ",Caseina=" + casein;
            }
            query += ",DataTestareFinala = CONVERT(date, '" + dataTestareFinala.ToShortDateString() +
                     "', 103) WHERE CodBare = '" + cod_bare + "'";
            try
            {
                SqlCommand cmd = new SqlCommand(query, cnn);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                loggerRezultate.Error("UpdateMostraF|QUERY:{0} ERROR{1}", query, ex.Message);
            }
            finally
            {
                cnn.Close();
            }
        }
    }

    public void UpdateMostraCombi(string cod_bare, DateTime dataTestareFinala, string ncs, string substu, string lactoza,
        string proteine, string grasime, bool analizatg, bool analizatl, bool analizatp, bool analizats, bool analizatn)
    {
        if (!analizatg || !analizatp || !analizats || !analizatl || !analizatn)
        {
            SqlConnection cnn = new SqlConnection(CopConnectionString);
            string query = "UPDATE MostreTancuri set ";
            int count = 0;
            if (!analizatg)
            {
                query += "Grasime= " + grasime;
                count++;
            }
            if (!analizatp)
            {
                if (count > 0)
                    query += ",";
                query += "ProcentProteine=" + proteine;
                count++;
            }
            if (!analizatl)
            {
                if (count > 0)
                    query += ",";
                query += "ProcentLactoza=" + lactoza;
                count++;
            }
            if (!analizats)
            {
                if (count > 0)
                    query += ",";
                query += "SubstantaUscata=" + substu;
                count++;
            }
            if (!analizatn)
            {
                if (count > 0)
                    query += ",";
                query += "NumarCeluleSomatice=" + ncs;
                count++;
            }

            query += ",DataTestareFinala = CONVERT(date, '" + dataTestareFinala.ToShortDateString() +
                     "', 103) WHERE CodBare = '" + cod_bare + "'";

            SqlCommand cmd = new SqlCommand(query, cnn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }

    public void UpdateMostraA(DateTime dataTestare, int IdZilnic, string PctInghet, DateTime dataTestareFinala)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraA", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.Date);
        parameterDataTestare.Value = dataTestare;
        myCommand.Parameters.Add(parameterDataTestare);

        SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
        parameterIdZilnic.Value = IdZilnic;
        myCommand.Parameters.Add(parameterIdZilnic);

        SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
        parameterPunctInghet.Value = Utils.ConvertToDouble(PctInghet);
        myCommand.Parameters.Add(parameterPunctInghet);

        SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.Date);
        parameterDataTestareFinala.Value = dataTestareFinala;
        myCommand.Parameters.Add(parameterDataTestareFinala);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public void UpdateMostraC(int itemId, String CodBare, String Grasime,
        String Proteine, String Lactoza, String SubstantaUscata, String NCS, DateTime DataTestareFinala)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraC", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Value = itemId;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = CodBare;
        myCommand.Parameters.Add(parameterCodBare);


        SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
        parameterGrasime.Value = Utils.ConvertToDouble(Grasime);
        myCommand.Parameters.Add(parameterGrasime);

        SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
        parameterProteine.Value = Utils.ConvertToDouble(Proteine);
        myCommand.Parameters.Add(parameterProteine);

        SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
        parameterLactoza.Value = Convert.ToDecimal(Lactoza, CultureInfo.InvariantCulture);
        myCommand.Parameters.Add(parameterLactoza);

        SqlParameter parameterSubstantaUscata = new SqlParameter("@SubstantaUscata", SqlDbType.Float, 8);
        parameterSubstantaUscata.Value = Utils.ConvertToDouble(SubstantaUscata);
        myCommand.Parameters.Add(parameterSubstantaUscata);
        /*
                    SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet",  SqlDbType.Float, 8);
                    parameterPunctInghet.Value =  Utils.ConvertToDouble(PunctInghet);
                    myCommand.Parameters.Add(parameterPunctInghet);
        */
        SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
        //	if (NCS.Trim() != "0")
        parameterNCS.Value = Utils.ConvertToDouble(NCS);
        //	else
        //		parameterNCS.Value = 1/10000;
        myCommand.Parameters.Add(parameterNCS);

        SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.Date);
        parameterDataTestareFinala.Value = DataTestareFinala;
        myCommand.Parameters.Add(parameterDataTestareFinala);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public void UpdateMostraS(int itemId, String CodBare, String NCS)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraS", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Value = itemId;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
        parameterCodBare.Value = CodBare;
        myCommand.Parameters.Add(parameterCodBare);

        SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
        parameterNCS.Value = Utils.ConvertToDouble(NCS);
        myCommand.Parameters.Add(parameterNCS);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public void UpdateMostraDefinitiv(int itemId, int Definitiv)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraDefinitiv", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Value = itemId;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterDefinitiv = new SqlParameter("@Definitiv", SqlDbType.Int, 4);
        parameterDefinitiv.Value = Definitiv;
        myCommand.Parameters.Add(parameterDefinitiv);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public void UpdateMostraValidat(int itemId, int Validat)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraValidat", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
        parameterItemID.Value = itemId;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterValidat = new SqlParameter("@Validat", SqlDbType.Int, 4);
        parameterValidat.Value = Validat;
        myCommand.Parameters.Add(parameterValidat);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
    }

    public string GetEmail(int itemID)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetEmail", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterItemID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
        parameterItemID.Value = itemID;
        myCommand.Parameters.Add(parameterItemID);

        SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 150);
        parameterEmail.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterEmail);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterEmail.Value == DBNull.Value)
            return "";
        return (string)parameterEmail.Value;
    }

    public int GetDocumentsNumber(int domainId, int moduleId)
    {
        // Create Instance of Connection and Command Object
        SqlConnection myConnection = new SqlConnection(CopConnectionString);
        SqlCommand myCommand = new SqlCommand("Portal_GetDocumentsNumber", myConnection);

        // Mark the Command as a SPROC
        myCommand.CommandType = CommandType.StoredProcedure;

        // Add Parameters to SPROC
        SqlParameter parameterDomainId = new SqlParameter("@DomainId", SqlDbType.Int, 4);
        parameterDomainId.Value = domainId;
        myCommand.Parameters.Add(parameterDomainId);

        SqlParameter parameterDocumentsNumber = new SqlParameter("@DocumentsNumber", SqlDbType.Int, 4);
        parameterDocumentsNumber.Direction = ParameterDirection.Output;
        myCommand.Parameters.Add(parameterDocumentsNumber);

        SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
        parameterModuleId.Value = moduleId;
        myCommand.Parameters.Add(parameterModuleId);

        myConnection.Open();
        myCommand.ExecuteNonQuery();
        myConnection.Close();
        if (parameterDocumentsNumber.Value == DBNull.Value)
            return 0;
        return (int)parameterDocumentsNumber.Value;
    }

    public void UpdateCrotalii(string filepath)
    {
        Crotalia kr = new Crotalia();

        string strConn;
        int posOfSlash = filepath.LastIndexOf(@"\");
        string filelogpath = filepath.Substring(0, filepath.Length - 4) + "_" + DateTime.Now.ToString("ddMMyyyy") +
                             ".txt";
        string logfilename = filelogpath.Substring(posOfSlash + 1);
        filelogpath = filelogpath.Replace(" ", "_");
        logfilename = logfilename.Replace(" ", "_");

        string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;

        string filename = filepath.Substring(posOfSlash + 1);

        loggerCrotalii.Info("Start Actualizare crotalii");

        filepath = filepath.Replace(@"\", @"\\");
        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" +
                  "Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
        OleDbConnection cn;
        DataTable dtTables;

        cn = new OleDbConnection(strConn);
        cn.Open();
        dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        for (int i = 0; i < dtTables.Rows.Count; i++)
        {
            string tblstr = dtTables.Rows[i]["TABLE_NAME"].ToString();

            OleDbDataAdapter myCommand = new OleDbDataAdapter();
            myCommand.SelectCommand = new OleDbCommand("SELECT * FROM [" + tblstr + "]", cn);

            DataSet myDataSet = new DataSet();
            myCommand.Fill(myDataSet);

            DataTable table;
            table = myDataSet.Tables[0];
            cn.Close();

            string crot = "";
            string codbare = "";
            int header = 0;
            string rasa = "";
            string datanasterii = "";
            int fermaid = 0;
            int asocid = 0;
            string nume = "";
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    header++;

                    if (header > 1 && row[1].ToString().Trim() != "")
                    {
                        codbare = row[0].ToString().Trim();
                        if (codbare.Length > 7)
                            codbare = codbare.Substring(0, 7);
                        crot = row[1].ToString().Trim();
                        try
                        {
                            fermaid = Int32.Parse(row[2].ToString().Trim());
                            asocid = Int32.Parse(row[3].ToString().Trim());
                        }
                        catch (Exception ex)
                        {
                            loggerCrotalii.Error(string.Format("UpdateCrotalii|ERROR{0}", ex.Message));
                        }

                        rasa = row[4].ToString().Trim();
                        datanasterii = row[5].ToString().Trim();
                        nume = row[6].ToString().Trim();
                        try
                        {
                            if (datanasterii.Length > 4)
                            {
                                DateTime dt = DateTime.Parse(datanasterii);
                                datanasterii = dt.ToString("dd/MM/yyyy");
                            }
                        }

                        catch (Exception de)
                        {
                            loggerCrotalii.Error(crot + " " + datanasterii + de.Message);
                        }
                        int errCode = 0;

                        // test crot
                        string query = "SELECT * FROM CROTALII WHERE  Crotalia ='" + crot + "'";
                        if (ItemExist(query))
                            errCode = 1;
                        // verif. dupa cod ferma
                        query = "SELECT Count(*) FROM FERME_CCL Where ID = " + fermaid;
                        if (!ItemExist(query))
                            errCode = 2;
                        query = "SELECT Count(*) FROM FABRICI WHERE ID = " + asocid;
                        if (!ItemExist(query))
                            errCode = 3;

                        if (errCode == 0)
                        {
                            InsertCrot(codbare, crot, rasa, datanasterii, nume, fermaid);
                        }
                        else
                        {
                            string errMess = "";
                            switch (errCode)
                            {
                                case 1:
                                    errMess = "Crotalia " + crot + " exista deja !";
                                    break;
                                case 2:
                                    errMess = "Ferma " + fermaid + " nu exista !";
                                    break;
                                case 3:
                                    errMess = "Asociatia " + asocid + " nu exista !";
                                    break;
                                default:
                                    break;
                            }
                            loggerCrotalii.Error(errMess);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerCrotalii.Error(crot + " : " + ex.Message);
            }
        } //end update table 
    }

    public void UpdateCrotaliiBV(string filepath)
    {
        Crotalia kr = new Crotalia();

        string strConn;
        int posOfSlash = filepath.LastIndexOf(@"\");
        string filelogpath = filepath.Substring(0, filepath.Length - 4) + "_" + DateTime.Now.ToString("ddMMyyyy") +
                             ".txt";
        string logfilename = filelogpath.Substring(posOfSlash + 1);
        filelogpath = filelogpath.Replace(" ", "_");
        logfilename = logfilename.Replace(" ", "_");

        string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;

        string filename = filepath.Substring(posOfSlash + 1);

        loggerCrotalii.Info("Start Actualizare crotalii");

        filepath = filepath.Replace(@"\", @"\\");
        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" +
                  "Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
        OleDbConnection cn;
        DataTable dtTables;

        cn = new OleDbConnection(strConn);
        cn.Open();
        dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        for (int i = 0; i < dtTables.Rows.Count; i++)
        {
            string tblstr = dtTables.Rows[i]["TABLE_NAME"].ToString();

            OleDbDataAdapter myCommand = new OleDbDataAdapter();
            myCommand.SelectCommand = new OleDbCommand("SELECT * FROM [" + tblstr + "]", cn);

            DataSet myDataSet = new DataSet();
            myCommand.Fill(myDataSet);

            DataTable table;
            table = myDataSet.Tables[0];
            cn.Close();

            string crot = "";
            string codbare = "";
            int header = 0;
            string rasa = "";
            string datanasterii = "";
            int fermaid = 0;
            int asocid = 0;
            string nume = "";
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    header++;

                    if (header > 1 && row[1].ToString().Trim() != "")
                    {
                        codbare = row[1].ToString().Trim();


                        crot = row[0].ToString().Trim();
                        if (crot == null || crot.Trim().Length == 0)
                        {
                            crot = tblstr.Replace("$", "_") + codbare;
                        }
                        try
                        {
                            fermaid = Int32.Parse(row[2].ToString().Trim());
                            asocid = Int32.Parse(row[3].ToString().Trim());
                        }
                        catch (Exception ex)
                        {
                            loggerCrotalii.Error(string.Format("UpdateCrotaliiBV|ERROR{0}", ex.Message));
                        }

                        rasa = row[4].ToString().Trim();
                        datanasterii = row[5].ToString().Trim();
                        nume = row[6].ToString().Trim();
                        try
                        {
                            if (datanasterii.Length > 4)
                            {
                                DateTime dt = DateTime.Parse(datanasterii);
                                datanasterii = dt.ToString("dd/MM/yyyy");
                            }
                        }

                        catch (Exception ex)
                        {
                            loggerCrotalii.Error(crot + " " + datanasterii + ex.Message);
                        }
                        int errCode = 0;

                        // test crot
                        string query = "SELECT * FROM CROTALII WHERE  Crotalia ='" + crot + "'";
                        if (ItemExist(query))
                            errCode = 1;
                        // verif. dupa cod ferma
                        query = "SELECT Count(*) FROM FERME_CCL Where ID = " + fermaid;
                        if (!ItemExist(query))
                            errCode = 2;
                        query = "SELECT Count(*) FROM FABRICI WHERE ID = " + asocid;
                        if (!ItemExist(query))
                            errCode = 3;

                        if (errCode == 0)
                        {
                            query = "SELECT * FROM CROTALII WHERE  FermaId = " + fermaid + " AND CodBare= '" + codbare +
                                    "'";
                            if (!ItemExist(query))
                                InsertCrot(codbare, crot, rasa, datanasterii, nume, fermaid);
                            else
                                UpdateCrot(codbare, crot, rasa, datanasterii, nume, fermaid);
                        }
                        else
                        {
                            string errMess = "";
                            switch (errCode)
                            {
                                case 1:
                                    errMess = "Crotalia " + crot + " exista deja !";
                                    break;
                                case 2:
                                    errMess = "Ferma " + fermaid + " nu exista !";
                                    break;
                                case 3:
                                    errMess = "Asociatia " + asocid + " nu exista !";
                                    break;
                                default:
                                    break;
                            }
                            loggerCrotalii.Error(errMess);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerCrotalii.Error(crot + " : " + ex.Message);
            }
        } //end update table 
    }

    public Boolean ItemExist(string query)
    {
        SqlConnection cnn = new SqlConnection(CopConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cnn;
        cmd.CommandText = query;
        cnn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            reader.Close();
            cnn.Close();
            return true;
        }
        reader.Close();
        cnn.Close();
        return false;
    }

    private void F3LineProcess(StreamReader sr, DateTime dataTestareFinala, List<Mostra> mostre)
    {
        loggerRezultate.Error("Foss");
        string linie = sr.ReadLine();
        int lineCnt = 1;
        bool fileError = false;
        while (linie != null && !fileError)
        {
            linie = sr.ReadLine();
            lineCnt++;
            if (lineCnt <= 6 || linie == null) continue;

            var a_linie = linie.Split(';');
            try
            {
                if (a_linie[0] != null)
                {
                    string job = a_linie[0].Trim();
                    double jobid;
                    bool isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"), out jobid);
                    if (isNum && a_linie[1].Trim() != "" && a_linie.Length > 17)
                    {
                        string codbare = a_linie[1].Trim();
                        string grasime = a_linie[2].Trim();
                        string proteine = a_linie[3].Trim();
                        string lactoza = a_linie[4].Trim();
                        string substu = a_linie[5].Trim();
                        string ph = a_linie[6].Trim();
                        string uree = a_linie[7].Trim();
                        string ncs = a_linie[8].Trim();

                        var mostra = mostre.FirstOrDefault(x => x.CodBare == codbare.Trim());

                        bool cod_gresit = false;

                        //compared to other processes and is used here
                        if ((!Utils.IsDouble(codbare) && codbare != "") &&
                            RezultatulEsteInFormatGresit(grasime) &&
                            RezultatulEsteInFormatGresit(proteine) &&
                            RezultatulEsteInFormatGresit(lactoza) &&
                            RezultatulEsteInFormatGresit(substu) &&
                            RezultatulEsteInFormatGresit(ph) &&
                            RezultatulEsteInFormatGresit(uree) &&
                            RezultatulEsteInFormatGresit(ncs))
                        {
                            loggerRezultate.Error("EROARE: Valoare incorecta la Pos=" + a_linie[0].Trim());
                            cod_gresit = true;
                        }
                        bool analizat = false;
                        // verific daca mostra a mai fost analizata
                        bool analizatg, analizatl, analizatp, analizats, analizatn;
                        analizatg = analizatl = analizats = analizatp = analizatn = false;

                        if (mostra != null && !cod_gresit)
                        {
                            var trebuieIgnoratUreea = TrebuieIgnoratValoare(mostra.Uree, ref uree, codbare, "Urea", false);
                            var trebuieIgnoratPh = TrebuieIgnoratValoare(mostra.Ph, ref ph, codbare, "pH", false);

                            analizatn = TrebuieIgnoratValoare(mostra.NumarCeluleSomatice, ref ncs, codbare, "NCS");
                            analizat = trebuieIgnoratUreea || trebuieIgnoratPh;
                            analizats = TrebuieIgnoratValoare(mostra.SubstrantaUscata, ref substu, codbare, "Subst.Uscata", false);
                            analizatl = TrebuieIgnoratValoare(mostra.ProcentLactoza, ref lactoza, codbare, "Lactoza", false);
                            analizatp = TrebuieIgnoratValoare(mostra.ProcentProteine, ref proteine, codbare, "Proteine", false);
                            analizatg = TrebuieIgnoratValoare(mostra.Grasime, ref grasime, codbare, "Grasime", false);
                        }
                        // test all 

                        if (!cod_gresit && (codbare != "1111111111"))
                        {
                            if (mostra.DataTestare != "")
                            {
                                DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
                                if (DateTime.Compare(dataTestareFinala, dt) < 0)
                                {
                                    loggerRezultate.Error("EROARE: DataTestare finala(" + dataTestareFinala.ToShortDateString() + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
                                    fileError = true;
                                }
                                else
                                {
                                    UpdateMostraF(codbare, dataTestareFinala, ConvertValA(ncs), ConvertValA(ph),
                                        Utils.ConvertToDouble(ConvertValA(uree)) <= 0 ? "0" : ConvertValA(uree), ConvertValA(substu), ConvertValA(lactoza),
                                        "0", ConvertValA(proteine), ConvertValA(grasime), analizatg, analizatp, analizatl, analizats, analizatn, analizat);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerRezultate.Error("F3LineProcess|EROARE la linia Pos={0}, ERROR:{1}", a_linie[0].Trim(), ex.Message);
            }
        }
    }

    private void CLineProcess(StreamReader sr, DateTime dataTestareFinala, List<Mostra> mostre)
    {
        var linie = sr.ReadLine();
        var lineCnt = 1;
        while (linie != null)
        {
            linie = sr.ReadLine();
            lineCnt++;
            if (lineCnt <= 4 || linie == null) continue;

            var a_linie = linie.Split(';');
            try
            {
                if (a_linie[0] != null)
                {
                    var job = a_linie[0].Trim();
                    double jobid;
                    var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"), out jobid);
                    if (isNum)
                    {
                        string codbare = a_linie[2].Trim();
                        string grasime = a_linie[3].Trim();
                        string proteine = a_linie[4].Trim();
                        string lactoza = a_linie[5].Trim();
                        string substu = a_linie[6].Trim();
                        string ncs = a_linie[9].Trim();

                        var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[2].Trim());
                        var idxPilot = a_linie[2].IndexOf("Pilot");
                        if (a_linie[2].Trim().Length == 0 || (a_linie[2].Trim().Length < 9 && (idxPilot == -1)))
                        {
                            loggerRezultate.Error("EROARE la linia Pos=" + a_linie[0].Trim());
                        }
                        var cod_gresit = false;

                        if ((!Utils.IsDouble(codbare) && codbare != "") ||
                            RezultatulEsteInFormatGresit(grasime) ||
                            RezultatulEsteInFormatGresit(proteine) ||
                            RezultatulEsteInFormatGresit(lactoza) ||
                            RezultatulEsteInFormatGresit(substu) ||
                            (ErrorInt(ncs) && ncs != "") && !ncs.Contains("*"))
                        {
                            loggerRezultate.Error("EROARE: Valoare incorecta la linia Pos=" + a_linie[0].Trim());
                            cod_gresit = true;
                        }
                        // verific daca mostra a mai fost analizata sau contine *****
                        bool deIgnoratGrasime = false;
                        bool deIgnoratLactoza = false;
                        bool deIgnoratProteine = false;
                        bool deIgnoratSubstantaUscata = false;
                        bool deIgnoratNcs = false;

                        if (mostra != null && !cod_gresit)
                        {
                            deIgnoratGrasime = TrebuieIgnoratValoare(mostra.Grasime, ref grasime, codbare, "Grasime");
                            deIgnoratProteine = TrebuieIgnoratValoare(mostra.ProcentProteine, ref proteine, codbare, "Proteine");
                            deIgnoratLactoza = TrebuieIgnoratValoare(mostra.ProcentLactoza, ref lactoza, codbare, "Lactoza");
                            deIgnoratSubstantaUscata = TrebuieIgnoratValoare(mostra.SubstrantaUscata, ref substu, codbare, "Substanta Uscata");
                            deIgnoratNcs = TrebuieIgnoratValoare(mostra.NumarCeluleSomatice, ref ncs, codbare, "NCS");
                        }

                        // end verificare
                        if (!cod_gresit && (a_linie[2].Trim() != "1111111111") && (idxPilot == -1))
                        {
                            if (mostra.DataTestare != "")
                            {
                                DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
                                if (DateTime.Compare(dataTestareFinala, dt) < 0)
                                {
                                    loggerRezultate.Error("EROARE: DataTestare finala(" + dataTestareFinala.ToShortDateString() + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
                                }
                                else
                                {
                                    // incepand din data de 22 ianuarie 2008 daca aparatul da rezultat 0 pt ncs si ntg se va insera 0.00001
                                    // si va fi tratat separat in afisare in lista afisandu-se valoare 0 in caz de 0.00001,  in editare mostre 0.00001 si in rapoarte 0
                                    // facandu-se diferenta intre valoarea 0 care inseamna neanalizat si valoarea 0 de la aparat fin

                                    UpdateMostraCombi(mostra.CodBare, dataTestareFinala, Utils.NomalizeForStorage(ncs), Utils.NomalizeForStorage(substu),
                                        Utils.NomalizeForStorage(lactoza), Utils.NomalizeForStorage(proteine), Utils.NomalizeForStorage(grasime), deIgnoratGrasime,
                                        deIgnoratLactoza, deIgnoratProteine, deIgnoratSubstantaUscata, deIgnoratNcs);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerRezultate.Error("CLineProcess|EROARE la linia cu Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message);
            }
        }
    }

    private bool RezultatulEsteInFormatGresit(string grasime)
    {
        return !Utils.IsDouble(grasime) && grasime != "" && !grasime.Contains("*");
    }

    private void C1LineProcess(StreamReader sr, DateTime dataTestareFinala, List<Mostra> mostre)
    {
        var linie = sr.ReadLine();
        var lineCnt = 1;
        bool fileError = false;
        while (linie != null && !fileError)
        {
            linie = sr.ReadLine();
            lineCnt++;
            if (lineCnt <= 6 || linie == null) continue;

            var a_linie = linie.Replace(">", "").Replace("<", "").Split(';');
            try
            {
                if (a_linie[0] != null)
                {
                    var job = a_linie[0].Trim();
                    double jobid;
                    var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"), out jobid);
                    if (isNum)
                    {
                        string codbare = a_linie[3].Trim();
                        string grasime = a_linie[4].Trim();
                        string proteine = a_linie[5].Trim();
                        string lactoza = a_linie[6].Trim();
                        string substu = a_linie[7].Trim();
                        string ncs = a_linie[9].Trim();
                        var mostra = mostre.FirstOrDefault(x => x.CodBare == codbare);
                        if (codbare.Length == 0 || (codbare.Length < 9))
                        {
                            loggerRezultate.Error("EROARE la linia Pos= {0}", a_linie[0].Trim());
                        }
                        var cod_gresit = false;

                        if ((!Utils.IsDouble(codbare) && codbare != "") ||
                            RezultatulEsteInFormatGresit(grasime) ||
                            RezultatulEsteInFormatGresit(proteine) ||
                            RezultatulEsteInFormatGresit(lactoza) ||
                            RezultatulEsteInFormatGresit(substu) ||
                            (ErrorInt(ncs) && ncs != "") && !ncs.Contains("*"))
                        {
                            loggerRezultate.Error("EROARE: Valoare incorecta la linia Pos={0}", a_linie[0].Trim());
                            cod_gresit = true;
                        }
                        // verific daca mostra a mai fost analizata sau contine *****
                        bool deIgnoratGrasime = false;
                        bool deIgnoratLactoza = false;
                        bool deIgnoratProteine = false;
                        bool deIgnoratSubstantaUscata = false;
                        bool deIgnoratNcs = false;

                        if (mostra != null && !cod_gresit)
                        {
                            deIgnoratGrasime = TrebuieIgnoratValoare(mostra.Grasime, ref grasime, codbare, "Grasime");
                            deIgnoratProteine = TrebuieIgnoratValoare(mostra.ProcentProteine, ref proteine, codbare, "Proteine");
                            deIgnoratLactoza = TrebuieIgnoratValoare(mostra.ProcentLactoza, ref lactoza, codbare, "Lactoza");
                            deIgnoratSubstantaUscata = TrebuieIgnoratValoare(mostra.SubstrantaUscata, ref substu, codbare, "Substanta Uscata");
                            deIgnoratNcs = TrebuieIgnoratValoare(mostra.NumarCeluleSomatice, ref ncs, codbare, "NCS");
                        }

                        // end verificare
                        if (!cod_gresit && (codbare != "1111111111") && mostra.DataTestare != "")
                        {
                            DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
                            if (DateTime.Compare(dataTestareFinala, dt) < 0)
                            {
                                loggerRezultate.Error("EROARE: DataTestare finala({0}) mai mica dacat data testare ({1})", dataTestareFinala.ToShortDateString(), mostra.DataTestare.Trim());
                                fileError = true;
                            }
                            else
                            {
                                // incepand din data de 22 ianuarie 2008 daca aparatul da rezultat 0 pt ncs si ntg se va insera 0.00001
                                // si va fi tratat separat in afisare in lista afisandu-se valoare 0 in caz de 0.00001,  in editare mostre 0.00001 si in rapoarte 0
                                // facandu-se diferenta intre valoarea 0 care inseamna neanalizat si valoarea 0 de la aparat fin

                                UpdateMostraCombi(mostra.CodBare, dataTestareFinala, Utils.NomalizeForStorage(ncs), Utils.NomalizeForStorage(substu),
                                    Utils.NomalizeForStorage(lactoza), Utils.NomalizeForStorage(proteine), Utils.NomalizeForStorage(grasime), deIgnoratGrasime,
                                    deIgnoratLactoza, deIgnoratProteine, deIgnoratSubstantaUscata, deIgnoratNcs);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerRezultate.Error("C1LineProcess|EROARE la linia cu Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message);
            }
        }
    }

    private bool TrebuieIgnoratValoare(string valoareAnterioara, ref string valoareCurenta, string codbare, string tip, bool nimicDevineZero)
    {
        bool trebuieIgnoratValoare = valoareCurenta.Contains("*");
        if (valoareAnterioara.Trim() != "0")
        {
            if (nimicDevineZero && valoareCurenta == "") valoareCurenta = "0";
            else
            {
                if (Math.Abs(Utils.ConvertToDouble(valoareCurenta) - Utils.ConvertToDouble(valoareAnterioara)) > 0.00001)
                {
                    loggerRezultate.Error("EROARE: Codul de bare={0} a mai fost analizat, avand ca rezultat alte valori pentru {1}! ", codbare, tip);
                    trebuieIgnoratValoare = true;
                }
            }
        }
        return trebuieIgnoratValoare;
    }

    private bool TrebuieIgnoratValoare(string valoareAnterioara, ref string valoareCurenta, string codbare, string tip)
    {
        return TrebuieIgnoratValoare(valoareAnterioara, ref valoareCurenta, codbare, tip, true);
    }

    private void BLineProcess(int idCodBare, StreamReader sr, DateTime dataTestareFinala, List<Mostra> mostre)
    {
        var linie = sr.ReadLine();
        var lineCnt = 1;
        bool fileError = false;
        while (linie != null && !fileError)
        {
            linie = sr.ReadLine();
            lineCnt++;
            if (lineCnt > 6)
            {
                if (linie != null)
                {
                    var a_linie = linie.Split(';');
                    try
                    {
                        if (a_linie[0] != null)
                        {
                            var job = a_linie[0].Trim();
                            double jobid;
                            var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"), out jobid);
                            if (isNum)
                            {
                                var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[2].Trim());
                                var idxPilot = a_linie[2].IndexOf("Pilot");
                                if (a_linie[2].Trim().Length == 0 || (a_linie[2].Trim().Length < 9 && (idxPilot == -1)))
                                {
                                    loggerRezultate.Error("EROARE la linia Pos= {0}", a_linie[0].Trim());
                                }

                                var cod_gresit = false;

                                if (!Utils.IsDouble(a_linie[4].Trim()))
                                {
                                    loggerRezultate.Error("EROARE: Valoare incorecta la Pos= {0}", a_linie[0].Trim());
                                    cod_gresit = true;
                                }
                                if (mostra != null && !cod_gresit && mostra.IncarcaturaGermeni.Trim() != "0")
                                {
                                    if (Math.Abs(Utils.ConvertToDouble(a_linie[4].Trim()) - Utils.ConvertToDouble(mostra.IncarcaturaGermeni)) > 0.000001)
                                        loggerRezultate.Error("EROARE: Codul de bare= {0} a mai fost analizat, avand ca rezultat alte valori pentru NTG!", a_linie[2].Trim());
                                    cod_gresit = true;
                                }

                                if (!cod_gresit && (a_linie[2].Trim() != "1111111111") && (idxPilot == -1))
                                {
                                    if (mostra.DataTestare != "")
                                    {
                                        DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)),
                                            Int32.Parse(mostra.DataTestare.Substring(3, 2)),
                                            Int32.Parse(mostra.DataTestare.Substring(0, 2)));
                                        if (DateTime.Compare(dataTestareFinala, dt) < 0)
                                        {
                                            loggerRezultate.Error("EROARE: DataTestare finala({0}) mai mica dacat data testare ({1})", dataTestareFinala.ToShortDateString(), mostra.DataTestare.Trim());
                                            fileError = true;
                                        }
                                        else
                                        {
                                            string ntg_val = Utils.NomalizeForStorage(a_linie[4]);

                                            UpdateMostraB(idCodBare, mostra.CodBare, ntg_val, dataTestareFinala);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        loggerRezultate.Error("BLineProcess|EROARE la linia cu Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message);
                    }
                }
            }
        }
    }

    private void FLineProcess(StreamReader sr, DateTime dataTestareFinala, List<Mostra> mostre)
    {
        loggerRezultate.Error("Foss");
        var linie = sr.ReadLine();
        var lineCnt = 1;
        bool fileError = false;
        while (linie != null && !fileError)
        {
            linie = sr.ReadLine();
            lineCnt++;
            if (lineCnt <= 6) continue;
            if (linie == null) continue;
            var a_linie = linie.Split(';');
            try
            {
                if (a_linie[0] != null)
                {
                    var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[1].Trim());
                    var job = a_linie[0].Trim();
                    double jobid;
                    var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CreateSpecificCulture("en-GB"), out jobid);
                    if (isNum && a_linie[1].Trim() != "" && a_linie.Length > 17)
                    {
                        string codbare = a_linie[1].Trim();
                        string grasime = a_linie[2].Trim();
                        string proteine = a_linie[3].Trim();
                        string caseina = a_linie[4].Trim();
                        string lactoza = a_linie[5].Trim();
                        string substu = a_linie[6].Trim();
                        string ph = a_linie[7].Trim();
                        string uree = a_linie[9].Trim();
                        string ncs = a_linie[10].Trim();

                        var cod_gresit = false;
                        if ((!Utils.IsDouble(codbare) && codbare != "") ||
                            RezultatulEsteInFormatGresit(grasime) ||
                            RezultatulEsteInFormatGresit(proteine) ||
                            RezultatulEsteInFormatGresit(caseina) ||
                            RezultatulEsteInFormatGresit(lactoza) ||
                            RezultatulEsteInFormatGresit(substu) ||
                            RezultatulEsteInFormatGresit(ph) ||
                            RezultatulEsteInFormatGresit(uree) ||
                            (ErrorInt(ncs) && ncs != "") && !ncs.Contains("*"))
                        {
                            loggerRezultate.Error("EROARE: Valoare incorecta la Pos=" + a_linie[0].Trim());
                            cod_gresit = true;
                        }
                        bool analizat = false;
                        // verific daca mostra a mai fost analizata
                        bool analizatg, analizatl, analizatp, analizats, analizatn;
                        analizatg = analizatl = analizats = analizatp = analizatn = false;

                        if (mostra != null && !cod_gresit)
                        {
                            var trebuieIgnoratUree = TrebuieIgnoratValoare(mostra.Uree, ref uree, codbare, "Urea", false);
                            var trebuieIgnoratPh = TrebuieIgnoratValoare(mostra.Ph, ref ph, codbare, "pH", false);
                            var trebuieIgnoratCaseina = TrebuieIgnoratValoare(mostra.Caseina, ref caseina, codbare, "Cazeina", false);

                            analizatn = TrebuieIgnoratValoare(mostra.NumarCeluleSomatice, ref ncs, codbare, "NCS");
                            analizat = trebuieIgnoratUree || trebuieIgnoratPh || trebuieIgnoratCaseina;
                            analizats = TrebuieIgnoratValoare(mostra.SubstrantaUscata, ref substu, codbare, "Subst.Uscata", false);
                            analizatl = TrebuieIgnoratValoare(mostra.ProcentLactoza, ref lactoza, codbare, "Lactoza", false);
                            analizatp = TrebuieIgnoratValoare(mostra.ProcentProteine, ref proteine, codbare, "Proteine", false);
                            analizatg = TrebuieIgnoratValoare(mostra.Grasime, ref grasime, codbare, "Grasime", false);
                        }
                        // test all 

                        if (!cod_gresit && (codbare != "1111111111"))
                        {
                            if (mostra.DataTestare != "")
                            {
                                DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
                                if (DateTime.Compare(dataTestareFinala, dt) < 0)
                                {
                                    loggerRezultate.Error("EROARE: DataTestare finala(" + dataTestareFinala.ToShortDateString() + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
                                    fileError = true;
                                }
                                else
                                {
                                    UpdateMostraF(codbare, dataTestareFinala, ConvertValA(ncs), ConvertValA(ph), Utils.ConvertToDouble(uree) <= 0 ? "0" : ConvertValA(uree), ConvertValA(substu),
                                        ConvertValA(lactoza), ConvertValA(caseina), ConvertValA(proteine), ConvertValA(grasime),
                                        analizatg, analizatp, analizatl, analizats, analizatn, analizat);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerRezultate.Error("FLineProcess|EROARE la linia cu Pos={0} ERROR:{1}", a_linie[0].Trim(),
                    ex.Message);
            }
        }
    }
}