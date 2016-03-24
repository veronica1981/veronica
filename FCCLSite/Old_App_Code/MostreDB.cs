using FCCL_BL.Bus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using FCCL_BL.Managers;
using FCCL_DAL;
using FCCL_DAL.Entities;
using NLog;


    //*********************************************************************
    //
    // Mostre Class
    //
    // Class that encapsulates all data logic necessary to add/query/delete
    // Mostre within the Portal database.
    //
    //*********************************************************************


public class MostreDB : IMostreDB
{
	private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext;
	private static Logger logger = LogManager.GetCurrentClassLogger();
	private static Logger loggerReceptie = LogManager.GetLogger("Receptie");
	private static Logger loggerRezultate = LogManager.GetLogger("Rezultate");

	public int GetTotalDocs()
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (int)parameterTotalDocs.Value;

	}

	public DataSet MostrePage(int PageIndex, int PageSize)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
		SqlDataAdapter myCommand = new SqlDataAdapter("Portal_Mostre_Get", myConnection);

		// Mark the Command as a SPROC
		myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

		SqlParameter parameterPageIndex = new SqlParameter("@PageIndex", SqlDbType.Int, 4);
		parameterPageIndex.Value = PageIndex;
		myCommand.SelectCommand.Parameters.Add(parameterPageIndex);

		SqlParameter parameterPageSize = new SqlParameter("@PageSize", SqlDbType.Int, 4);
		parameterPageSize.Value = PageSize;
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
		else
			return codbare;
	}


	public List<Mostra> VerificFile(string filepath)
	{
		var coduri = new StringBuilder();
		var mostre = new List<Mostra>();
		FileStream fs;
		string linie = "";
		string[] a_linie;
		string[] coduri_eronate = new string[1000];
		string[,] fisier = new string[1000, 3];

		int posOfSlash = filepath.LastIndexOf(@"\");
		string zi = filepath.Substring(posOfSlash + 1, 2);
		string luna = filepath.Substring(posOfSlash + 3, 2);
		string an = filepath.Substring(posOfSlash + 5, 2);
		string filename = filepath.Substring(posOfSlash + 1);

		int i, j;
		int z;
		int b_file = 0;
		int c_file = 0;
		int c1_file = 0;
		int a_file = 0;
		int f_file = 0;
		int lineCnt = 0;
		int ff;
		bool primul = false;

		loggerRezultate.Info("verific fisier {0}", filepath);
		bool isNum;
		string job;
		double jobid;

		if (File.Exists(filepath))
		{
			if (filepath.ToUpper().EndsWith("B1.CSV") || filepath.ToUpper().EndsWith("B2.CSV"))
				b_file = 1;
			if (filepath.ToUpper().EndsWith("C.CSV"))
				c_file = 1;
			if (filepath.ToUpper().Contains("C1"))
				c1_file = 1;
			if (filename.ToUpper().Contains("A"))
				a_file = 1;
			if (filename.ToUpper().Contains("C2") || filename.ToUpper().Contains("C3"))
				f_file = 1;
			fs = new FileStream(filepath, FileMode.OpenOrCreate);
			StreamReader sr = new StreamReader(fs);
			if (b_file == 1)
			{
				linie = sr.ReadLine();
				lineCnt = 1;
				i = 0;
				while (linie != null)
				{
					linie = sr.ReadLine();
					lineCnt++;
					if (lineCnt > 6)
					{
						if (linie != null)
						{
							a_linie = linie.Split(';');
							if (a_linie[0] != null)
							{
								job = a_linie[0].ToString().Trim();
								isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
								if (isNum)//&& a_linie[4].ToString().Trim() != "")
								{
									coduri.Append(a_linie[2].Trim() + ";");
									fisier[i, 0] = a_linie[0];
									fisier[i, 1] = a_linie[2];
									fisier[i, 2] = "0";
									i++;
								}
							}
						}
					}
				}
			}
			// f_file
			if (f_file == 1)
			{
				linie = sr.ReadLine();
				lineCnt = 1;
				i = 0;
				while (linie != null)
				{
					linie = sr.ReadLine();
					lineCnt++;
					if (lineCnt > 6)
					{
						if (linie != null)
						{
							a_linie = linie.Split(';');
							if (a_linie[0] != null)
							{
								job = a_linie[0].ToString().Trim();
								isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
								if (isNum)// && ((a_linie.Length > 17 && a_linie[17].Equals("VALUE")) || (a_linie.Length > 30 && a_linie[30].Equals("VALUE"))))
								{
									coduri.Append(a_linie[1].Trim() + ";");
									fisier[i, 0] = a_linie[0];
									fisier[i, 1] = a_linie[1];
									fisier[i, 2] = "0";
									i++;
								}
							}
						}
					}
				}
			}


			//end f_file
			if (c_file == 1)
			{
				linie = sr.ReadLine();
				lineCnt = 1;
				i = 0;
				while (linie != null)
				{
					linie = sr.ReadLine();
					lineCnt++;
					if (lineCnt > 4)
					{
						if (linie != null)
						{
							a_linie = linie.Split(';');
							if (a_linie[0] != null)
							{
								job = a_linie[0].ToString().Trim();
								isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
								if (isNum)// && a_linie[4].ToString().Trim() != "")
								{
									coduri.Append(a_linie[2].Trim() + ";");
									fisier[i, 0] = a_linie[0];
									fisier[i, 1] = a_linie[2];
									fisier[i, 2] = "0";
									i++;
								}
							}
						}
					}
				}
			}

			if (c1_file == 1)
			{
				linie = sr.ReadLine();
				lineCnt = 1;
				i = 0;
				while (linie != null)
				{
					linie = sr.ReadLine();
					lineCnt++;
					if (lineCnt > 7)
					{
						if (linie != null)
						{
							a_linie = linie.Split(';');
							if (a_linie[0] != null)
							{
								job = a_linie[0].ToString().Trim();
								isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
								if (isNum)// && a_linie[5].ToString().Trim() != "")
								{
									coduri.Append(a_linie[3].Trim() + ";");
									fisier[i, 0] = a_linie[0];
									fisier[i, 1] = a_linie[3];
									fisier[i, 2] = "0";
									i++;
								}
							}
						}
					}
				}
			}


			bool urmeaza_cod_bare = false;
			bool urmeaza_pct_inghet = false;
			string pct_inghet = "";
			int idx = 0;
			string cod_bare = "";
			string a_data_test = "";
			if (a_file == 1)
			{
				i = 0;
				linie = sr.ReadLine();

				while (linie != null)
				{
					linie = sr.ReadLine();
					if (lineCnt <= 5 && ((linie.IndexOf(" PM") >= 0) || (linie.IndexOf(" AM") >= 0)))
						a_data_test = linie.TrimStart(' ').Substring(3, 2) + "/" + linie.TrimStart(' ').Substring(0, 2) + "/" + linie.TrimStart(' ').Substring(6, 4);
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
								fisier[i, 0] = Convert.ToString(i);
								fisier[i, 1] = cod_bare;
								fisier[i, 2] = "0";
								i++;
							}
							if (urmeaza_cod_bare)
							{
								cod_bare = linie.Substring(linie.Length - 10, 10).TrimStart(' ');
								urmeaza_pct_inghet = true;
							}
							idx = linie.IndexOf("I.D.#");
							if (idx >= 0)
								urmeaza_cod_bare = true;
							else
								urmeaza_cod_bare = false;
						}
						catch (Exception ex)
						{
							loggerRezultate.Info(string.Format("VerificFile|EROARE: Cod de bare gol la pozitia ID={0} ERROR:{1}", fisier[i, 0].Trim(), ex.Message));
							urmeaza_cod_bare = false;
							urmeaza_pct_inghet = false;
						}
					}
				}
			}
			sr.Close();
			fs.Close();

			var mostreManager = new MostreManager(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			mostre = mostreManager.GetDateCoduriBare(coduri.ToString().TrimEnd(';'));

			i = 0;
			z = 0;
			while (fisier[i, 0] != null)
			{

				if (fisier[i, 1].Trim() == "")
				{
					loggerRezultate.Info("EROARE: Cod de bare gol la pozitia ID=" + fisier[i, 0].Trim());
				}
				else
				{
					if (a_file == 0)
					{
						foreach (var cod in coduri.ToString().TrimEnd(';').Split(';'))
						{
							var mostra = mostre.FirstOrDefault(x => x.CodBare == cod);
							if (mostra == null)
							{
								loggerRezultate.Info("Codul de bare:" + fisier[i, 1] + " nu a fost importat din fisierul de receptie din data de:" + zi + "/" + luna + "/20" + an);
							}
						}
					}
					else
					{
						if (ErrorInt(fisier[i, 1].Trim()))
						{
							loggerRezultate.Info("EROARE: Valoare incorecta la Incrementul Zilnic: " + fisier[i, 1].Trim());
						}
						else
						{
							if (ExistaIdZilnic(a_data_test, Convert.ToInt32(fisier[i, 1].Trim())) == 0 && (fisier[i, 1].Trim() != "1111111111"))
							{
								loggerRezultate.Info("Nu exista incrementul zilnic:" + fisier[i, 1] + "in ziua : " + a_data_test);
								coduri_eronate[z++] = fisier[i, 1].Trim();
							}
						}

					}


					j = i + 1;
					primul = true;
					while (fisier[j, 0] != null)
					{

						ff = fisier[j, 2].Trim().CompareTo("0");
						if ((fisier[i, 1] == fisier[j, 1]) && (ff == 0))
						{
							if (primul)
							{
								loggerRezultate.Info("EROARE: Codul de bare:" + fisier[i, 1] + " se gaseste de mai multe ori: ");
								loggerRezultate.Info("                 Pozitia: " + fisier[i, 0].Trim());
								coduri_eronate[z++] = fisier[i, 1].Trim();
								primul = false;
							}
							loggerRezultate.Info("EROARE: Codul de bare:" + fisier[i, 1] + " se gaseste de mai multe ori: ");
							loggerRezultate.Info("                 Pozitia: " + fisier[j, 0].Trim());
							fisier[j, 2] = "1";
						}
						j++;
					}
				}
				i++;
			}
		}
		return mostre;
	}
	public void Addtext(string path, string text)
	{
		FileStream fs1;
		if (File.Exists(path))
			fs1 = new FileStream(path, FileMode.Append);
		else
			fs1 = new FileStream(path, FileMode.Create);

		StreamWriter sw = new StreamWriter(fs1);

		sw.WriteLine(text);
		sw.Close();
		fs1.Close();
	}

	public string ConvertValA(string res)
	{
		string resc = "0";
		double dbl;
		if (double.TryParse(res, out dbl))
		{
			if (res.Trim() == "0")
				resc = "0.00001";
			else
				resc = res.Trim();
			if (resc.Trim() == "") resc = "0";
		}
		return resc;

	}
	public void ImportCrio(string filepath, string dataTestare, string dataTestareFinala, string baseDir)
	{
		// read crio
		FileStream fs = new FileStream(filepath, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		// read xls
		int posOfSlash = filepath.LastIndexOf(@"\");
		string filename = filepath.Substring(posOfSlash + 1);
		string xlsname = filename.Replace("txt", "xls");
		string xlspath = baseDir + @"Downloads\CSV_ImportateManual\Crio\" + xlsname;
		xlspath = xlspath.Replace("txt", "xls");
		xlspath = xlspath.Replace(@"\", @"\\");
		string resname = xlsname.Replace(".xls", "_final.xls");
		string respath = baseDir + @"Downloads\CSV_ImportateManual\Crio\" + resname;

		string oledbConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + xlspath + ";" + "Extended Properties='Excel 8.0;HDR=No;IMEX=1;READONLY=FALSE;'";

		OleDbConnection cn;
		DataTable dtTables;
		cn = new OleDbConnection(oledbConn);
		cn.Open();
		dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
		string tblstr = dtTables.Rows[dtTables.Rows.Count - 1]["TABLE_NAME"].ToString();
		OleDbDataAdapter adapter =
		new OleDbDataAdapter("SELECT * FROM [" + tblstr + "]", cn);
		OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
		DataSet dataset = new DataSet();
		adapter.Fill(dataset);
		adapter.FillSchema(dataset, SchemaType.Source);
		DataTable table;
		table = dataset.Tables[0];
		cn.Close();
		// create positionlist
		List<MostreCrio> inilist = new List<MostreCrio>();
		foreach (DataRow dr in table.Rows)
		{
			MostreCrio mc = new MostreCrio();
			mc.id = dr[0].ToString();
			mc.codbare = dr[1].ToString();
			inilist.Add(mc);
		}
		// create res xls 

		//read crio
		string currDate = dataTestare;
		string currTime = "00:00";
		string cassette = "";
		string position = "";
		string line = "";
		// parse crio
		bool fileError = false;
		line = sr.ReadLine();
		int count = 1;
		char[] SplitSeparators = { ' ' };
		List<MostreCrio> testlist = new List<MostreCrio>();
		IFormatProvider culture = new CultureInfo("fr-FR", true);
		int posinilist = 1;
		while (line != null && !fileError)
		{

			line = sr.ReadLine();
			if (line == null)
				break;
			line = line.Trim();
			string[] fields = line.Split(SplitSeparators, StringSplitOptions.RemoveEmptyEntries);
			if (fields == null || fields.Length == 0)
				continue;
			if (!fields[0].Equals("Cassette") && !fields[0].Equals("Position") && !line.Equals("Test Time-out Error")
			&& !line.Equals("Test Time-out Error") && !line.Equals("Sample Pre-freeze") &&
			!line.Equals("Testing Complete") && !line.Equals("Cryoscope Ready") && !line.Equals("Running Diagnostics")
			&& (count > 10))

				try
				{
					DateTime datatest = DateTime.Parse(fields[0], culture, DateTimeStyles.NoCurrentDateDefault);
					string timetest = fields[1];
					currDate = datatest.ToString("dd/MM/yyyy");
					currTime = timetest;
				}
				catch
				{

				}
			if (fields[0].Equals("Cassette"))
				cassette = fields[1];
			if (fields[0].Equals("Position"))
			{
				position = fields[1];
				string pct_inghet = fields[2];
				MostreCrio mc = new MostreCrio();
				mc.datatest = currDate;
				mc.timetest = currTime;
				mc.pcti = pct_inghet;
				mc.cassette = cassette;
				mc.position = position;

				if (!cassette.Equals("5"))
				{
					if (posinilist < inilist.Count)
					{
						mc.codbare = inilist[posinilist].codbare;
						mc.id = inilist[posinilist].id;
						posinilist++;
						testlist.Add(mc);
						loggerRezultate.Info("proba analizata: " + mc.id + " " + mc.codbare + " " + mc.cassette + " / " + mc.position + " " + mc.pcti + " " + mc.datatest + " " + mc.timetest);
						string sdt = GetDataTestare(mc.codbare);
						int res = UpdateMostraAC(sdt, Int32.Parse(mc.id), mc.pcti, dataTestareFinala);
						if (res < 1)
							loggerRezultate.Info("actualizare pcti. esuata: " + mc.id + " " + mc.codbare + " " + mc.pcti);

					}
					else { fileError = true; }
				}
				else
				{                   //add lactrol
					mc.id = "";
					mc.codbare = "lactrol";
					testlist.Add(mc);
					loggerRezultate.Info("lactrol: " + mc.cassette + " / " + mc.position + " " + mc.pcti + " " + mc.datatest + " " + mc.timetest);
				}
				//add value


			}
			if (line.Equals("Test Time-out Error") || line.Equals("Sample Pre-freeze"))
			{

				if (posinilist < inilist.Count)
				{
					MostreCrio mc = new MostreCrio();
					mc.datatest = currDate;
					mc.timetest = currTime;
					mc.cassette = cassette;
					mc.position = position;
					mc.pcti = line;
					//add error

					mc.codbare = inilist[posinilist].codbare;
					mc.id = inilist[posinilist].id;
					loggerRezultate.Info("analiza esuata: " + mc.id + " " + mc.codbare + " " + mc.cassette + " / " + mc.position + " " + mc.pcti + " " + mc.datatest + " " + mc.timetest);
					posinilist++;
					testlist.Add(mc);
				}
				else
				{
					fileError = true;
				}
			}

			count++;
		}
		// update extended table, update mostre
		//create excel
		DataTable dt = new DataTable();
		dt.Columns.Add(new DataColumn("Id"));
		dt.Columns.Add(new DataColumn("Codbare"));
		dt.Columns.Add(new DataColumn("Cassette"));
		dt.Columns.Add(new DataColumn("Position"));
		dt.Columns.Add(new DataColumn("Punct_inghet"));
		dt.Columns.Add(new DataColumn("Data_analizei"));
		dt.AcceptChanges();
		MostreCrio prevmc = testlist[0];
		int nrmc = 0;
		foreach (MostreCrio mc in testlist)
		{
			DataRow row = dt.NewRow();
			row[0] = mc.id;
			row[1] = mc.codbare;
			row[2] = mc.cassette;
			row[3] = mc.position;
			row[4] = mc.pcti;
			if (mc.cassette.Equals("5") && ((nrmc == 0) || (nrmc > 0 && !prevmc.cassette.Equals("5"))))
				row[5] = mc.datatest + " " + mc.timetest;
			else
				row[5] = "";
			dt.Rows.Add(row);
			prevmc = mc;
			nrmc++;
		}
		dt.AcceptChanges();
		CreateExcelFile(dt, respath);
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
						colname = "Id laborator<br/>";
						break;
					case 1:
						colname = "Cod identificare proba<br>";
						break;
					case 2:
						colname = "Cassette<br/>";
						break;
					case 3:
						colname = "Position<br/>";
						break;
					case 4:
						colname = "Punct inghet<br/>";
						break;
					case 5:
						colname = "Data<br/>";
						break;

					default:
						break;

				}

				sTHead += @"<TH>" + colname + @"</TH>";
			}

			sTHead += @"</TR>";

			foreach (DataRow row in dt.Rows)
			{
				sTableData.Append(@"<TR>");
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					string text = row[i].ToString().Trim();
					sTableData.Append(@"<TD>" + text + @"</TD>");

				}
				sTableData.Append(@"</TR>");

			}


			string sTable = sTableStart + sTHead + sTableData.ToString() + sTableEnd;
			if (File.Exists(filename))
				File.Delete(filename);
			StreamWriter oExcelWriter = File.CreateText(filename);
			oExcelWriter.WriteLine(sTable);
			oExcelWriter.Close();

			//                FileStream  fs = new FileStream(filename, FileMode.Append);
			//              StreamWriter sw = new StreamWriter(fs);
			//            sw.Write.WriteLine(sTable);
			//          sw.Close();    
			//    fs.Close(); 
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void ImportManual(string filepath)
	{
		FileStream fs;
		string linie = "";
		int b_file = 0;
		int c_file = 0;
		int c1_file = 0;
		int a_file = 0;
		int f_file = 0;
		int ac_file = 0;
		int lineCnt = 0;
		bool f3_file = false;

		string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;

		int posOfSlash = filepath.LastIndexOf(@"\");
		string zi = filepath.Substring(posOfSlash + 1, 2);
		string luna = filepath.Substring(posOfSlash + 3, 2);
		string an = filepath.Substring(posOfSlash + 5, 2);
		string dataTestareFinala = zi + "/" + luna + "/20" + an;
		string dataTestare = zi + "/" + luna + "/20" + an;
		string filename = filepath.Substring(posOfSlash + 1);
		string saveDir = filepath.Substring(0, posOfSlash + 1) + @"FisiereImportate";

		if (filepath.ToUpper().EndsWith("B1.CSV") || filepath.ToUpper().EndsWith("B2.CSV"))
			b_file = 1;
		if (filepath.ToUpper().EndsWith("C.CSV"))
			c_file = 1;
		if (filepath.ToUpper().EndsWith("C1.CSV"))
			c1_file = 1;
		if (filename.ToUpper().IndexOf("A") >= 0)
			a_file = 1;
		if (filename.ToUpper().IndexOf("C2") >= 0)
			f_file = 1;
		if (filename.ToUpper().IndexOf("C3") >= 0)
			f3_file = true;
		if (filename.ToUpper().IndexOf("A3") >= 0)
			ac_file = 1;

		string cod_bare = "";
		int idCodBare = 0;
		bool fileError = false;
		var mostre = VerificFile(filepath);

		#region ac file
		if (ac_file == 1)
			ImportCrio(filepath, dataTestare, dataTestareFinala, dirServer);
		#endregion ac file

		if (!fileError && File.Exists(filepath) && ac_file != 1)
		{
			fs = new FileStream(filepath, FileMode.Open);
			StreamReader sr = new StreamReader(fs);
			#region b_file
			if (b_file == 1)
			{
				BLineProcess(idCodBare, sr, dataTestareFinala, mostre);
			}
			#endregion b file
			#region c_file
			if (c_file == 1)
			{
				CLineProcess(sr, dataTestareFinala, mostre);
			}
			#endregion
			#region c1_file
			if (c1_file == 1)
			{
				C1LineProcess(sr, dataTestareFinala, mostre);
			}
			#endregion
			#region a_file
			bool urmeaza_cod_bare = false;
			bool urmeaza_pct_inghet = false;
			string pct_inghet = "";
			int idx = 0;
			string a_data_test = "";
			bool cod_gresit;
			if (a_file == 1)
			{
				linie = sr.ReadLine();
				lineCnt = 0;
				while (linie != null)
				{
					if (lineCnt <= 5 && ((linie.IndexOf(" PM") >= 0) || (linie.IndexOf(" AM") >= 0)))
						a_data_test = linie.TrimStart(' ').Substring(3, 2) + "/" + linie.TrimStart(' ').Substring(0, 2) + "/" + linie.TrimStart(' ').Substring(6, 4);
					if (linie != null)
					{
						try
						{
							if (urmeaza_pct_inghet)
							{
								pct_inghet = linie.Substring(linie.IndexOf("mC") - 4, 3);
								urmeaza_pct_inghet = false;
								cod_gresit = false;

								if (ErrorDouble(pct_inghet) || ErrorInt(cod_bare))
								{
									loggerRezultate.Info("EROARE: Valoare incorecta la linia: " + Convert.ToString(lineCnt));
									cod_gresit = true;
								}
								if (a_data_test.Trim() == "")
								{
									linie = sr.ReadLine();
									lineCnt++;
									if (linie != null && lineCnt <= 5 && ((linie.IndexOf(" PM") >= 0) || (linie.IndexOf(" AM") >= 0)))
										a_data_test = linie.TrimStart(' ').Substring(3, 2) + "/" + linie.TrimStart(' ').Substring(0, 2) + "/" + linie.TrimStart(' ').Substring(6, 4);

								}
								if (!cod_gresit)
								{
									SqlDataReader drMostra = GetDateDataTestareIdZilnic(dataTestare, Convert.ToInt32(cod_bare));
									if (drMostra.Read() && !cod_gresit && Convert.ToString(drMostra["PunctInghet"]).Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(pct_inghet.Trim()) - Convert.ToDouble(drMostra["PunctInghet"])) > 0.00001)
											loggerRezultate.Info("EROARE: Identificatorul zilnic: " + cod_bare + " din data: " + dataTestare + " a mai fost analizat, avand ca rezultat alte valori pentru Punct de Inghet! ");
										cod_gresit = true;
									}
									drMostra.Close();
								}
								if (!cod_gresit && cod_bare.Trim() != "1111111111")
								{
									//	DateTime dtfinala = new DateTime(Int32.Parse(a_data_test.Substring(6,4)),Int32.Parse(a_data_test.Substring(3,2)),Int32.Parse(a_data_test.Substring(0,2)));
									DateTime dtfinala = new DateTime(Int32.Parse(dataTestareFinala.Substring(6, 4)), Int32.Parse(dataTestareFinala.Substring(3, 2)), Int32.Parse(dataTestareFinala.Substring(0, 2)));

									DateTime dt = new DateTime(Int32.Parse(dataTestare.Substring(6, 4)), Int32.Parse(dataTestare.Substring(3, 2)), Int32.Parse(dataTestare.Substring(0, 2)));
									if (DateTime.Compare(dtfinala, dt) < 0)
									{
										loggerRezultate.Info("EROARE: DataTestare finala(" + a_data_test + ") mai mica dacat data testare (" + dt.ToString("dd/MM/yyyy") + ")");
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
										UpdateMostraA(dataTestare, Convert.ToInt32(cod_bare), pcti_val, dataTestareFinala);
									}
								}
							}
							if (urmeaza_cod_bare)
							{
								cod_bare = linie.Substring(linie.Length - 10, 10).TrimStart(' ');
								if (cod_bare.Trim() == "")
									urmeaza_pct_inghet = false;
								else
									urmeaza_pct_inghet = true;
							}
							idx = linie.IndexOf("I.D.#");
							if (idx >= 0)
								urmeaza_cod_bare = true;
							else
								urmeaza_cod_bare = false;
						}
						catch (Exception ex)
						{
							urmeaza_cod_bare = false;
							urmeaza_pct_inghet = false;
							loggerRezultate.Info(string.Format("ImportManual|EROARE: Valoare incorecta la linia: {0}, ERROR{1}", Convert.ToString(lineCnt), ex.Message));
						}

					}
					linie = sr.ReadLine();
					lineCnt++;
				}
			}
			#endregion
			#region f_file
			if (f_file == 1)
			{
				FLineProcess(sr, dataTestareFinala, mostre);
			}
			#endregion
			#region f3_file
			if (f3_file)
			{
				F3LineProcess(sr, dataTestareFinala, mostre);
			}
			#endregion
			sr.Close();
			fs.Close();
		}
		if (!fileError)
			loggerRezultate.Info("Importarea fisierului a fost efectuata!");
		else
			loggerRezultate.Info("Importarea fisierului nu a fost efectuata!");
	}

	public bool ErrorDouble(string strvalue)
	{
		bool isNum = true;
		double jobid;
		if (strvalue != "")
			isNum = double.TryParse(strvalue, NumberStyles.Any, null, out jobid);
		return !isNum;
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

		string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;
		string saveDir = dirServer + @"Downloads\FisiereImportateAutomat";
		if (!Directory.Exists(saveDir))
			Directory.CreateDirectory(saveDir);
		saveDir = saveDir + @"\";

		//			DataTable schemaTable;
		strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" + "Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
		//You must use the $ after the object you reference in the spreadsheet"+numesheet+"
		//		OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]",strConn);
		OleDbConnection cn;
		DataTable dtTables;

		cn = new OleDbConnection(strConn);
		cn.Open();
		dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
		string tblstr = dtTables.Rows[dtTables.Rows.Count - 1]["TABLE_NAME"].ToString();
		//	OleDbCommand myCommand = new OleDbCommand ("SELECT * FROM [" + tblstr+ " $]", cn);
		// make log files
		string tbldate = tblstr.Substring(4, 8);
		loggerReceptie.Info("Data: " + DateTime.Now.ToString() + " nume sheet:" + tblstr);
		loggerReceptie.Info("*****************************************************************************");
		loggerReceptie.Info("*****************************************************************************");
		loggerReceptie.Info("Data: " + DateTime.Now.ToString() + " nume sheet:" + tblstr);
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
		string strDataPrelevare;
		string ferma_ccl_cod = "";
		int ferma_ccl_id = 0;
		int fabricaID = 0;
		int header = 0;
		string prelevator = "";
		string datat_testare = "";
		double prelid;
		bool isNum;
		string prelevatorini = table.Rows[0][1].ToString();

		List<Order> orders = new List<Order>();
		OrderManager oManager = new OrderManager(ctx);

		foreach (DataRow row in table.Rows)
		{
			header++;

			if (header > 2 && row[4].ToString().Trim() != "")
			{

				error = false;
				strcodbare = row[4].ToString().Trim();
				if (ExistaCodBare(strcodbare) != 0)
				{
					loggerReceptie.Info("Codul de bare:" + strcodbare + " exista deja");
					error = true;
				}
				else
				{
					datat_testare = tblstr.Substring(4);
					datat_testare = datat_testare.Substring(0, 2) + "/" + datat_testare.Substring(2, 2) + "/" + datat_testare.Substring(4, 4);
					//	strDataPrelevare = row[5].ToString().Trim().Replace(@".", @"\");
					strDataPrelevare = row[7].ToString().Trim();
					prelevator = row[1].ToString().Trim();
					/*
						if (prelevator == "")
						  prelevator = prelevatorini;
						else
						  prelevatorini = prelevator;
					 */
					isNum = double.TryParse(prelevator, NumberStyles.Integer, CultureInfo.CurrentCulture, out prelid);


					loggerReceptie.Info("Codul de bare: " + strcodbare + " are inhibitor:" + row[5].ToString().Trim());
					string nrComanda = row[0].ToString().Trim();
					string strDataPrimirii = row[8].ToString().Trim();
					string numeFerma = string.Empty;

					if (!isNum)
					{
						numeFerma = row[9].ToString().Trim();
						AddMostraFCB(0, Convert.ToInt32(row[3].ToString().Trim()), strcodbare, nrComanda,
							prelevator, row[2].ToString().Trim(), strDataPrelevare, strDataPrimirii,
							datat_testare, datat_testare, numeFerma, row[10].ToString().Trim(),
							row[11].ToString().Trim(), row[12].ToString().Trim(), 0, 0, "0", "0", "0", "0", "0", "0", row[5].ToString().Trim(), "0", "0", "0");
					}
					else
					{
						if (Convert.ToInt32(prelevator) == 0)
							prelevator = strcodbare.Substring(0, 5);
						ferma_ccl_cod = strcodbare.Substring(0, 5);
						ferma_ccl_id = GetFermeCCL_ID(ferma_ccl_cod);
						if (ferma_ccl_id == 0)
						{
							loggerReceptie.Info("La id Zilnic : " + row[3].ToString().Trim() + " Codul de ferma :" + ferma_ccl_cod + " nu exista !!! Mostra nu a fost importata");
							error = true;
						}
						else
						{
							fabricaID = ferma.GetFerma(ferma_ccl_id, out numeFerma);
							loggerReceptie.Info("Gasit Ferma id: {0}; nume: {1}", ferma_ccl_id, numeFerma);
						}
						if (!error)
							try
							{
								AddMostra(0, Convert.ToInt32(row[3].ToString().Trim()), strcodbare, nrComanda, ferma_ccl_cod, ferma_ccl_id, fabricaID
									, row[5].ToString().Trim(), "0", "0", "0", "0", "0", strDataPrelevare, "0", strDataPrimirii,
									"0", datat_testare, "0", Convert.ToInt32(prelevator), 0, 0, datat_testare, "0", "0", "0");
							}
							catch (Exception ex)
							{
								loggerReceptie.Info("Adaugare mostra esuata:" + strcodbare + " " + ex.Message);
							}
					}
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
						if (arr.Length == 2 && int.TryParse(arr[0], out orderNumber))
						{
							DateTime dataPrimirii;
							if (!DateTime.TryParseExact(strDataPrimirii, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataPrimirii))
							{
								dataPrimirii = DateTime.Now;
							}

							DateTime dataPrelevare;
							if (!DateTime.TryParseExact(strDataPrelevare, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataPrelevare))
							{
								dataPrelevare = DateTime.Now;
							}

							currentOrder = new Order
							{
								AnalyzedSampleCount = 1,
								ClientId = fabricaID > 0 ? fabricaID : -1,
								ClientName = numeFerma + " (" + fabricaID + ")",
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
							loggerReceptie.Info("Adaugare comanda esuata: " + nrComanda);
						}
					}

				}
			}

		}

		oManager.Save(orders);
		ctx.SaveChanges();

		loggerReceptie.Info("Importarea fisierului de receptie a fost efectuata!");
	}
	//
	public void UpdatePrelevatori(string filepath)
	{
		Ferme_CCL ferma = new Ferme_CCL();

		string strcodbare;
		string strConn;
		int posOfSlash = filepath.LastIndexOf(@"\");
		string dirServer = StaticDataHelper.SettingsManager.CaleFizicaServer;

		string filename = filepath.Substring(posOfSlash + 1);

		loggerReceptie.Info("Start Actualizare prelevatori");

		filepath = filepath.Replace(@"\", @"\\");
		//			DataTable schemaTable;
		strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" + "Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
		//You must use the $ after the object you reference in the spreadsheet"+numesheet+"
		//		OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]",strConn);
		OleDbConnection cn;
		DataTable dtTables;

		cn = new OleDbConnection(strConn);
		cn.Open();
		dtTables = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
		//    string tblstr = dtTables.Rows[dtTables.Rows.Count - 1]["TABLE_NAME"].ToString();
		//	OleDbCommand myCommand = new OleDbCommand ("SELECT * FROM [" + tblstr+ " $]", cn);
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

			// For each row, print the values of each column.
			string strDataPrelevare;
			int header = 0;
			string prelevator = "";
			string datat_testare = "";
			double prelid;
			bool isNum;
			string prelevatorini = table.Rows[0][1].ToString();
			foreach (DataRow row in table.Rows)
			{
				header++;

				if (header > 2 && row[4].ToString().Trim() != "")
				{

					strcodbare = row[4].ToString().Trim();


					datat_testare = tblstr.Substring(4);
					datat_testare = datat_testare.Substring(0, 2) + "/" + datat_testare.Substring(2, 2) + "/" + datat_testare.Substring(4, 4);
					strDataPrelevare = row[7].ToString().Trim();
					prelevator = row[1].ToString().Trim();
					if (prelevator == "")
					{
						prelevator = prelevatorini;
						isNum = double.TryParse(prelevator, NumberStyles.Integer, CultureInfo.CurrentCulture, out prelid);
						if (isNum)
						{
							UpdateMostraP(strcodbare, prelevator, false);
						}
						else
							UpdateMostraP(strcodbare, prelevator, true);
						loggerReceptie.Info("Codbare: " + strcodbare + " Prelevator: " + prelevator + "Datatestare :" + datat_testare);


					}
					else
						prelevatorini = prelevator;

				}
			}
		} //end update table 
	}

	public void UpdateMostraP(string codbare, string prelid, bool isFcb)
	{
		SqlConnection cnn = new SqlConnection(
ConfigurationManager.ConnectionStrings
["fccl2ConnectionString"].ConnectionString);
		string query = "";
		if (!isFcb)
			query = "UPDATE MostreTancuri set PrelevatorId =" + prelid +
			 ", CodFerma='" + codbare.Substring(0, 5) + "' WHERE Codbare Like'" + codbare + "'";
		else
			query = "UPDATE MostreTancuri set PrelevatorId =0, NumePrelevator='" + prelid +
		  "', CodFerma='" + codbare.Substring(0, 5) + "' WHERE Codbare Like'" + codbare + "'";

		SqlCommand cmd = new SqlCommand(query, cnn);
		cmd.Connection.Open();
		cmd.ExecuteNonQuery();
		cmd.Connection.Close();

	}

	public DataSet SearchMostre(string codbarec, string datatestare1, string datatestare2, string antibiotice,
		string pctinghetini, string pctinghetfin, string ntgini, string ntgfin, string ncsini, string ncsfin)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);


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
			query_cond += " PunctInghet <= " + pctinghetini.Substring(3, pctinghetini.Length - 3) + " AND PunctInghet <> 0 ";
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
				query_cond += " PunctInghet >= " + pctinghetfin.Substring(3, pctinghetfin.Length - 3) + " AND PunctInghet <> 0 ";
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);

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
			query_cond += " PunctInghet <= " + pctinghetini.Substring(3, pctinghetini.Length - 3) + " AND PunctInghet <> 0 ";
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
				query_cond += " PunctInghet >= " + pctinghetfin.Substring(3, pctinghetfin.Length - 3) + " AND PunctInghet <> 0 ";
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
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
			else
				return str;
		}
		else
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
		string[] keywordsExclus = new string[] { "and", "or", "the", "in", "of", "if", "is" };
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (string)parameterNumePrelevator.Value;
	}

	public SqlDataReader GetSingleMostre(int itemId)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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

	public SqlDataReader GetDateDataTestareIdZilnic(string DataTestare, int IdZilnic)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
		SqlCommand myCommand = new SqlCommand("Portal_GetDateDataTestareIdZilnic", myConnection);

		// Mark the Command as a SPROC
		myCommand.CommandType = CommandType.StoredProcedure;

		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Value = DataTestare;
		myCommand.Parameters.Add(parameterDataTestare);

		SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
		parameterIdZilnic.Value = IdZilnic;
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (int)parameterItemID.Value;

	}

	public int GetMaxIdZilnic(string DataTestare)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (int)parameterItemID.Value;

	}

	public void DeleteMostra(int itemID)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (int)parametermostraID.Value;
	}
	public string GetDataTestare(string codbare)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
		SqlCommand myCommand = new SqlCommand("Portal_GetDataTestare", myConnection);

		// Mark the Command as a SPROC
		myCommand.CommandType = CommandType.StoredProcedure;

		SqlParameter parametercodbare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 150);
		parametercodbare.Value = codbare;
		myCommand.Parameters.Add(parametercodbare);


		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Direction = ParameterDirection.Output;
		myCommand.Parameters.Add(parameterDataTestare);

		// Create and Fill the DataSet
		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
		if (parameterDataTestare.Value == DBNull.Value)
			return "";
		else
			return ((string)parameterDataTestare.Value);
	}

	public int ExistaCodBareDataTestare(string codbare, string DataTestare)
	{
		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (int)parametermostraID.Value;
	}

	public int ExistaIdZilnic(string DataTestare, int IdZilnic)
	{
		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
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

	public int AddMostra(int itemId, int IdZilnic, String CodBare, string NrComanda, string CodFerma, int FermaID, int FabricaID, string Antibiotice, String CantitatePrelevare, String NCS, String Grasime,
					String NTG, String Proteine, String DataPrelevare, String Lactoza, String DataPrimirii,
					String SubstantaUscata, String DataTestare, String PunctInghet, int Prelevator, int Definitiv, int Validare, string DataTestareFinala, string Urea, string Ph, string Casein)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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

		SqlParameter parameterCantitatePrelevare = new SqlParameter("@CantitatelaPrelevare", SqlDbType.Float, 8);
		parameterCantitatePrelevare.Value = Convert.ToDouble(CantitatePrelevare);
		myCommand.Parameters.Add(parameterCantitatePrelevare);

		SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
		parameterNCS.Value = Convert.ToDouble(NCS);
		myCommand.Parameters.Add(parameterNCS);

		SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
		parameterNTG.Value = Convert.ToDouble(NTG);
		myCommand.Parameters.Add(parameterNTG);

		SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
		parameterGrasime.Value = Convert.ToDouble(Grasime);
		myCommand.Parameters.Add(parameterGrasime);

		SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
		parameterProteine.Value = Convert.ToDouble(Proteine);
		myCommand.Parameters.Add(parameterProteine);

		SqlParameter parameterDataPrelevare = new SqlParameter("@DataPrelevare", SqlDbType.NVarChar, 255);
		parameterDataPrelevare.Value = DataPrelevare;
		myCommand.Parameters.Add(parameterDataPrelevare);

		SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
		parameterLactoza.Value = Convert.ToDecimal(Lactoza);
		myCommand.Parameters.Add(parameterLactoza);

		SqlParameter parameterDataPrimirii = new SqlParameter("@DataPrimirii", SqlDbType.DateTime, 8);
		IFormatProvider culture = new CultureInfo("fr-FR", true);
		parameterDataPrimirii.Value = DateTime.Parse(DataPrimirii, culture,
			DateTimeStyles.NoCurrentDateDefault);
		myCommand.Parameters.Add(parameterDataPrimirii);

		SqlParameter parameterSubstantaUscata = new SqlParameter("@SubstantaUscata", SqlDbType.Float, 8);
		parameterSubstantaUscata.Value = Convert.ToDouble(SubstantaUscata);
		myCommand.Parameters.Add(parameterSubstantaUscata);

		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Value = DataTestare;
		myCommand.Parameters.Add(parameterDataTestare);

		SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
		parameterPunctInghet.Value = Convert.ToDouble(PunctInghet);
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


		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
		parameterDataTestareFinala.Value = DataTestareFinala;
		myCommand.Parameters.Add(parameterDataTestareFinala);

		SqlParameter parameterUrea = new SqlParameter("@Urea", SqlDbType.Float, 8);
		parameterUrea.Value = Convert.ToDouble(Urea);
		myCommand.Parameters.Add(parameterUrea);

		SqlParameter parameterPh = new SqlParameter("@PH", SqlDbType.Float, 8);
		parameterPh.Value = Convert.ToDouble(Ph);
		myCommand.Parameters.Add(parameterPh);

		SqlParameter parameterCasein = new SqlParameter("@Casein", SqlDbType.Float, 8);
		parameterCasein.Value = Convert.ToDouble(Casein);
		myCommand.Parameters.Add(parameterCasein);

		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
		if (parameterItemID.Value == DBNull.Value)
			return 0;
		else
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
	}


	public int AddMostraFCB(int itemId, int IdZilnic, string CodBare, string NrComanda, string NumePrelevator,
		string NumeProba, string DataPrelevare, string DataPrimirii, string DataTestare, string DataTestareFinala,
		string NumeClient, string Adresa, string Localitate, string Judet, int Definitiv, int Validat, string NCS, string Grasime,
		string NTG, string Proteine, string Lactoza, string PunctInghet, string Antibiotice, string Urea, string Ph, string Casein)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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

		SqlParameter parameterDataPrelevare = new SqlParameter("@DataPrelevare", SqlDbType.NVarChar, 255);
		parameterDataPrelevare.Value = DataPrelevare;
		myCommand.Parameters.Add(parameterDataPrelevare);

		SqlParameter parameterDataPrimirii = new SqlParameter("@DataPrimirii", SqlDbType.DateTime, 8);
		IFormatProvider culture = new CultureInfo("fr-FR", true);
		parameterDataPrimirii.Value = DateTime.Parse(DataPrimirii, culture,
			DateTimeStyles.NoCurrentDateDefault);
		myCommand.Parameters.Add(parameterDataPrimirii);

		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Value = DataTestare;
		myCommand.Parameters.Add(parameterDataTestare);

		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
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
		parameterNCS.Value = Convert.ToDouble(NCS);
		myCommand.Parameters.Add(parameterNCS);

		SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
		parameterNTG.Value = Convert.ToDouble(NTG);
		myCommand.Parameters.Add(parameterNTG);

		SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
		parameterGrasime.Value = Convert.ToDouble(Grasime);
		myCommand.Parameters.Add(parameterGrasime);

		SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
		parameterProteine.Value = Convert.ToDouble(Proteine);
		myCommand.Parameters.Add(parameterProteine);

		SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
		parameterLactoza.Value = Convert.ToDecimal(Lactoza);
		myCommand.Parameters.Add(parameterLactoza);

		SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
		parameterPunctInghet.Value = Convert.ToDouble(PunctInghet);
		myCommand.Parameters.Add(parameterPunctInghet);


		SqlParameter parameterUrea = new SqlParameter("@Urea", SqlDbType.Float, 8);
		parameterUrea.Value = Convert.ToDouble(Urea);
		myCommand.Parameters.Add(parameterUrea);

		SqlParameter parameterPh = new SqlParameter("@PH", SqlDbType.Float, 8);
		parameterPh.Value = Convert.ToDouble(Ph);
		myCommand.Parameters.Add(parameterPh);

		SqlParameter parameterCasein = new SqlParameter("@Casein", SqlDbType.Float, 8);
		parameterCasein.Value = Convert.ToDouble(Casein);
		myCommand.Parameters.Add(parameterCasein);


		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
		if (parameterItemID.Value == DBNull.Value)
			return 0;
		else
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

	public void UpdateMostra(int itemId, String CodBare, string NrComanda, string CodFerma, int FermaID, int FabricaID, string Antibiotice, String CantitatePrelevare, String NCS, String Grasime,
		String NTG, String Proteine, String DataPrelevare, String Lactoza, String DataPrimirii,
		String SubstantaUscata, String DataTestare, String PunctInghet, int Prelevator, int Definitiv, int Validare, string DataTestareFinala)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		parameterCantitatePrelevare.Value = Convert.ToDouble(CantitatePrelevare);
		myCommand.Parameters.Add(parameterCantitatePrelevare);

		SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
		parameterNCS.Value = Convert.ToDouble(NCS);
		myCommand.Parameters.Add(parameterNCS);

		SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
		parameterNTG.Value = Convert.ToDouble(NTG);
		myCommand.Parameters.Add(parameterNTG);

		SqlParameter parameterGrasime = new SqlParameter("@Grasime", SqlDbType.Float, 8);
		parameterGrasime.Value = Convert.ToDouble(Grasime);
		myCommand.Parameters.Add(parameterGrasime);

		SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
		parameterProteine.Value = Convert.ToDouble(Proteine);
		myCommand.Parameters.Add(parameterProteine);

		SqlParameter parameterDataPrelevare = new SqlParameter("@DataPrelevare", SqlDbType.NVarChar, 255);
		parameterDataPrelevare.Value = DataPrelevare;
		myCommand.Parameters.Add(parameterDataPrelevare);

		SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
		parameterLactoza.Value = Convert.ToDecimal(Lactoza);
		myCommand.Parameters.Add(parameterLactoza);

		SqlParameter parameterDataPrimirii = new SqlParameter("@DataPrimirii", SqlDbType.DateTime, 8);
		IFormatProvider culture = new CultureInfo("fr-FR", true);
		parameterDataPrimirii.Value = DateTime.Parse(DataPrimirii, culture,
			DateTimeStyles.NoCurrentDateDefault);
		myCommand.Parameters.Add(parameterDataPrimirii);

		SqlParameter parameterSubstantaUscata = new SqlParameter("@SubstantaUscata", SqlDbType.Float, 8);
		parameterSubstantaUscata.Value = Convert.ToDouble(SubstantaUscata);
		myCommand.Parameters.Add(parameterSubstantaUscata);

		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Value = DataTestare;
		myCommand.Parameters.Add(parameterDataTestare);

		SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
		parameterPunctInghet.Value = Convert.ToDouble(PunctInghet);
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


		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
		parameterDataTestareFinala.Value = DataTestareFinala;
		myCommand.Parameters.Add(parameterDataTestareFinala);

		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
	}

	//TODO remove itemId parameter -> itemId is not used in the stored procedure
	public void UpdateMostraB(int itemId, String CodBare, String NTG, string DataTestareFinala)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
		SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraB", myConnection);

		// Mark the Command as a SPROC
		myCommand.CommandType = CommandType.StoredProcedure;

		SqlParameter parameterItemID = new SqlParameter("@ID", SqlDbType.Int, 4);
		parameterItemID.Value = itemId;
		myCommand.Parameters.Add(parameterItemID);

		SqlParameter parameterCodBare = new SqlParameter("@CodBare", SqlDbType.NVarChar, 255);
		parameterCodBare.Value = CodBare;
		myCommand.Parameters.Add(parameterCodBare);

		SqlParameter parameterNTG = new SqlParameter("@IncarcaturaGermeni", SqlDbType.Float, 8);
		parameterNTG.Value = Convert.ToDouble(NTG);
		myCommand.Parameters.Add(parameterNTG);

		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
		parameterDataTestareFinala.Value = DataTestareFinala;
		myCommand.Parameters.Add(parameterDataTestareFinala);

		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
	}

	public void UpdateMostraF(string cod_bare, string dataTestareFinala, string ncs, string ph, string urea, string substu, string lactoza, string casein, string proteine, string grasime, bool analizatg, bool analizatp, bool analizatl, bool analizats, bool analizatn, bool analizat)
	{
		if (!analizatg || !analizatp || !analizats || !analizatl || !analizatn || !analizat)
		{
			SqlConnection cnn = new SqlConnection(
			  ConfigurationManager.ConnectionStrings
				["fccl2ConnectionString"].ConnectionString);
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
			query += ",DataTestareFinala='" + dataTestareFinala + "' WHERE CodBare = '" + cod_bare + "'";

			SqlCommand cmd = new SqlCommand(query, cnn);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

	}

	public void UpdateMostraCombi(string cod_bare, string dataTestareFinala, string ncs, string substu, string lactoza, string proteine, string grasime, bool analizatg, bool analizatl, bool analizatp, bool analizats, bool analizatn)
	{
		if (!analizatg || !analizatp || !analizats || !analizatl || !analizatn)
		{
			SqlConnection cnn = new SqlConnection(
			  ConfigurationManager.ConnectionStrings
				["fccl2ConnectionString"].ConnectionString);
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

			query += ",DataTestareFinala='" + dataTestareFinala + "' WHERE CodBare = '" + cod_bare + "'";

			SqlCommand cmd = new SqlCommand(query, cnn);
			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}
	}

	public void UpdateMostraA(string DataTestare, int IdZilnic, string PctInghet, string DataTestareFinala)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
		SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraA", myConnection);

		// Mark the Command as a SPROC
		myCommand.CommandType = CommandType.StoredProcedure;

		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Value = DataTestare;
		myCommand.Parameters.Add(parameterDataTestare);

		SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
		parameterIdZilnic.Value = IdZilnic;
		myCommand.Parameters.Add(parameterIdZilnic);

		SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
		parameterPunctInghet.Value = Convert.ToDouble(PctInghet);
		myCommand.Parameters.Add(parameterPunctInghet);

		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
		parameterDataTestareFinala.Value = DataTestareFinala;
		myCommand.Parameters.Add(parameterDataTestareFinala);

		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
	}

	public int UpdateMostraAC(string DataTestare, int IdZilnic, string PctInghet, string DataTestareFinala)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
		SqlCommand myCommand = new SqlCommand("Portal_UpdateMostraA", myConnection);

		// Mark the Command as a SPROC
		myCommand.CommandType = CommandType.StoredProcedure;

		SqlParameter parameterDataTestare = new SqlParameter("@DataTestare", SqlDbType.NVarChar, 255);
		parameterDataTestare.Value = DataTestare;
		myCommand.Parameters.Add(parameterDataTestare);

		SqlParameter parameterIdZilnic = new SqlParameter("@IdZilnic", SqlDbType.Int, 4);
		parameterIdZilnic.Value = IdZilnic;
		myCommand.Parameters.Add(parameterIdZilnic);

		SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet", SqlDbType.Float, 8);
		parameterPunctInghet.Value = Convert.ToDouble(PctInghet);
		myCommand.Parameters.Add(parameterPunctInghet);

		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
		parameterDataTestareFinala.Value = DataTestareFinala;
		myCommand.Parameters.Add(parameterDataTestareFinala);

		myConnection.Open();
		int res = myCommand.ExecuteNonQuery();
		myConnection.Close();
		return res;
	}

	public void UpdateMostraC(int itemId, String CodBare, String Grasime,
		 String Proteine, String Lactoza, String SubstantaUscata, String NCS, string DataTestareFinala)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		parameterGrasime.Value = Convert.ToDouble(Grasime);
		myCommand.Parameters.Add(parameterGrasime);

		SqlParameter parameterProteine = new SqlParameter("@ProcentProteine", SqlDbType.Float, 8);
		parameterProteine.Value = Convert.ToDouble(Proteine);
		myCommand.Parameters.Add(parameterProteine);

		SqlParameter parameterLactoza = new SqlParameter("@ProcentLactoza", SqlDbType.Float, 8);
		parameterLactoza.Value = Convert.ToDecimal(Lactoza);
		myCommand.Parameters.Add(parameterLactoza);

		SqlParameter parameterSubstantaUscata = new SqlParameter("@SubstantaUscata", SqlDbType.Float, 8);
		parameterSubstantaUscata.Value = Convert.ToDouble(SubstantaUscata);
		myCommand.Parameters.Add(parameterSubstantaUscata);
		/*
					SqlParameter parameterPunctInghet = new SqlParameter("@PunctInghet",  SqlDbType.Float, 8);
					parameterPunctInghet.Value =  Convert.ToDouble(PunctInghet);
					myCommand.Parameters.Add(parameterPunctInghet);
		*/
		SqlParameter parameterNCS = new SqlParameter("@NumarCeluleSomatice", SqlDbType.Float, 8);
		//	if (NCS.Trim() != "0")
		parameterNCS.Value = Convert.ToDouble(NCS);
		//	else
		//		parameterNCS.Value = 1/10000;
		myCommand.Parameters.Add(parameterNCS);

		SqlParameter parameterDataTestareFinala = new SqlParameter("@DataTestareFinala", SqlDbType.NVarChar, 255);
		parameterDataTestareFinala.Value = DataTestareFinala;
		myCommand.Parameters.Add(parameterDataTestareFinala);

		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
	}

	public void UpdateMostraS(int itemId, String CodBare, String NCS)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		parameterNCS.Value = Convert.ToDouble(NCS);
		myCommand.Parameters.Add(parameterNCS);

		myConnection.Open();
		myCommand.ExecuteNonQuery();
		myConnection.Close();
	}


	public void UpdateMostraDefinitiv(int itemId, int Definitiv)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (string)parameterEmail.Value;
	}

	public int GetDocumentsNumber(int domainId, int moduleId)
	{

		// Create Instance of Connection and Command Object
		SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
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
		else
			return (int)parameterDocumentsNumber.Value;
	}

	private void FLineProcess(StreamReader sr, string dataTestareFinala, List<Mostra> mostre)
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
							var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[1].Trim());
							double jobid;
							string job = a_linie[0].ToString().Trim();
							bool isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
							if (isNum && a_linie[1].ToString().Trim() != "" && a_linie.Length > 17)
							{

								var cod_gresit = false;
								if ((ErrorDouble(a_linie[2].Trim()) && a_linie[2].Trim() != "")
									|| (ErrorDouble(a_linie[3].Trim()) && a_linie[3].Trim() != "")
									|| (ErrorDouble(a_linie[4].Trim()) && a_linie[4].Trim() != "")
									|| (ErrorDouble(a_linie[5].Trim()) && a_linie[5].Trim() != "")
									|| (ErrorDouble(a_linie[6].Trim()) && a_linie[6].Trim() != "")
									|| (ErrorDouble(a_linie[7].Trim()) && a_linie[8].Trim() != "")
									|| (ErrorDouble(a_linie[9].Trim()) && a_linie[8].Trim() != "")
									|| (ErrorDouble(a_linie[10].Trim()) && a_linie[9].Trim() != ""))
								{
									loggerRezultate.Info("EROARE: Valoare incorecta la Pos=" + a_linie[0].Trim());
									cod_gresit = true;
								}
								bool analizat = false;
								// verific daca mostra a mai fost analizata
								bool analizatg, analizatl, analizatp, analizats, analizatn;
								analizatg = analizatl = analizats = analizatp = analizatn = false;

								if (mostra != null && !cod_gresit)
								{
									if (mostra.NumarCeluleSomatice.Trim() != "0")
									{
										if (a_linie[10] == "")
											a_linie[10] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[10].Trim()) - Convert.ToDouble(mostra.NumarCeluleSomatice)) > 0.000001)

											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru NCS! ");
										analizatn = true;

									}

									if (mostra.Uree.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[9].Trim()) - Convert.ToDouble(mostra.Uree)) > 0.000001)

											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Urea! ");
										analizat = true;
									}
									if (mostra.Ph.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[7].Trim()) - Convert.ToDouble(mostra.Ph)) > 0.000001)
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru pH! ");
										analizat = true;
									}

									if (mostra.SubstrantaUscata.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[6].Trim()) - Convert.ToDouble(mostra.SubstrantaUscata)) > 0.000001)
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Subst.Uscata! ");
										analizats = true;
									}
									if (mostra.ProcentLactoza.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[5].Trim()) - Convert.ToDouble(mostra.ProcentLactoza)) > 0.000001)
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Lactoza! ");
										analizatl = true;
									}
									//cazeina
									if (mostra.Caseina.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[4].Trim()) - Convert.ToDouble(mostra.Caseina)) > 0.000001)
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Cazeina! ");
										analizat = true;
									}
									if (mostra.ProcentProteine.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[3].Trim()) - Convert.ToDouble(mostra.ProcentProteine)) > 0.000001)
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Proteine! ");
										analizatp = true;
									}
									if (mostra.Grasime.Trim() != "0")
									{
										if (Math.Abs(Convert.ToDouble(a_linie[2].Trim()) - Convert.ToDouble(mostra.Grasime)) > 0.000001)
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Grasime! ");
										analizatg = true;
									}
								}
								// test all 

								if (!cod_gresit && (a_linie[1].Trim() != "1111111111") /*&& (a_linie[2].Trim()!= "")*/ )//!error && 
								{
									DateTime dtfinala = new DateTime(Int32.Parse(dataTestareFinala.Substring(6, 4)), Int32.Parse(dataTestareFinala.Substring(3, 2)), Int32.Parse(dataTestareFinala.Substring(0, 2)));
									if (mostra.DataTestare != "")
									{
										DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
										if (DateTime.Compare(dtfinala, dt) < 0)
										{
											loggerRezultate.Info("EROARE: DataTestare finala(" + dataTestareFinala + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
											fileError = true;
										}
										else
										{
											string ncs = ConvertValA(a_linie[10]);
											string urea = Convert.ToDouble(ConvertValA(a_linie[9])) <= 0 ? "0" : ConvertValA(a_linie[9]);
											string ph = ConvertValA(a_linie[7]);
											string substu = ConvertValA(a_linie[6]);
											string lactoza = ConvertValA(a_linie[5]);
											string casein = ConvertValA(a_linie[4]);
											string proteine = ConvertValA(a_linie[3]);
											string grasime = ConvertValA(a_linie[2]);

											UpdateMostraF(mostra.CodBare, dataTestareFinala, ncs, ph, urea, substu, lactoza, casein, proteine, grasime, analizatg, analizatp, analizatl, analizats, analizatn, analizat);
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						loggerRezultate.Info(string.Format("FLineProcess|EROARE la linia Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message));
					}

				}
			}
		}
	}

	private void F3LineProcess(StreamReader sr, string dataTestareFinala, List<Mostra> mostre)
	{
		loggerRezultate.Info("Foss");
		string linie = sr.ReadLine();
		int lineCnt = 1;
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
							string job = a_linie[0].ToString().Trim();
							double jobid;
							bool isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
							if (isNum && a_linie[1].ToString().Trim() != "" && a_linie.Length > 17)
							{
								var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[1]);

								bool cod_gresit = false;

								if ((ErrorDouble(a_linie[2].Trim()) && a_linie[2].Trim() != "")
									&& (ErrorDouble(a_linie[3].Trim()) && a_linie[3].Trim() != "")
									&& (ErrorDouble(a_linie[4].Trim()) && a_linie[4].Trim() != "")
									&& (ErrorDouble(a_linie[5].Trim()) && a_linie[5].Trim() != "")
									&& (ErrorDouble(a_linie[6].Trim()) && a_linie[6].Trim() != "")
									&& (ErrorDouble(a_linie[7].Trim()) && a_linie[7].Trim() != "")
									&& (ErrorDouble(a_linie[8].Trim()) && a_linie[8].Trim() != ""))
								{
									loggerRezultate.Info("EROARE: Valoare incorecta la Pos=" + a_linie[0].Trim());
									cod_gresit = true;
								}

								bool analizat = false;
								// verific daca mostra a mai fost analizata
								bool analizatg, analizatl, analizatp, analizats, analizatn;
								analizatg = analizatl = analizats = analizatp = analizatn = false;

								double dbl;
								if (mostra != null && !cod_gresit)
								{
									#region check values
									if (Convert.ToString(mostra.NumarCeluleSomatice).Trim() != "0")
									{
										if (a_linie[10] == "")
											a_linie[10] = "0";
										if (double.TryParse(a_linie[8].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.NumarCeluleSomatice)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru NCS! ");
											analizatn = true;
										}
									}

									if (mostra.Uree.Trim() != "0")
									{
										if (double.TryParse(a_linie[7].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.Uree)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Urea! ");
											analizat = true;
										}
									}
									if (mostra.Ph.Trim() != "0")
									{
										if (double.TryParse(a_linie[6].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.Ph)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru pH! ");
											analizat = true;
										}
									}

									if (mostra.SubstrantaUscata.Trim() != "0")
									{
										if (double.TryParse(a_linie[5].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.SubstrantaUscata)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Subst.Uscata! ");
											analizats = true;
										}
									}
									if (mostra.ProcentLactoza.Trim() != "0")
									{
										if (double.TryParse(a_linie[4].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.ProcentLactoza)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Lactoza! ");
											analizatl = true;
										}
									}
									if (mostra.ProcentProteine.Trim() != "0")
									{
										if (double.TryParse(a_linie[3].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.ProcentProteine)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Proteine! ");
											analizatp = true;
										}
									}
									if (mostra.Grasime.Trim() != "0")
									{
										if (double.TryParse(a_linie[2].Trim(), out dbl))
										{
											if (Math.Abs(dbl - Convert.ToDouble(mostra.Grasime)) > 0.000001)
												loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[1].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Grasime! ");
											analizatg = true;
										}
									}
									#endregion
								}
								// test all 

								if (!cod_gresit && (a_linie[1].Trim() != "1111111111"))
								{

									DateTime dtfinala = new DateTime(Int32.Parse(dataTestareFinala.Substring(6, 4)), Int32.Parse(dataTestareFinala.Substring(3, 2)), Int32.Parse(dataTestareFinala.Substring(0, 2)));
									if (mostra.DataTestare != "")
									{
										DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
										if (DateTime.Compare(dtfinala, dt) < 0)
										{
											loggerRezultate.Info("EROARE: DataTestare finala(" + dataTestareFinala + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
											fileError = true;
										}
										else
										{
											string grasime = ConvertValA(a_linie[2]);
											string proteine = ConvertValA(a_linie[3]);
											string casein = "0";
											string lactoza = ConvertValA(a_linie[4]);
											string substu = ConvertValA(a_linie[5]);
											string ph = ConvertValA(a_linie[6]);
											string urea = Convert.ToDouble(ConvertValA(a_linie[7])) <= 0 ? "0" : ConvertValA(a_linie[7]);
											string ncs = ConvertValA(a_linie[8]);


											UpdateMostraF(a_linie[1].Trim(), dataTestareFinala, ncs, ph, urea, substu, lactoza, casein, proteine, grasime, analizatg, analizatp, analizatl, analizats, analizatn, analizat);
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						loggerRezultate.Info(string.Format("F3LineProcess|EROARE la linia Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message));
					}

				}
			}
		}
	}

	private void C1LineProcess(StreamReader sr, string dataTestareFinala, List<Mostra> mostre)
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
					var a_linie = linie.Replace(">", "").Replace("<", "").Split(';');
					try
					{
						if (a_linie[0] != null)
						{
							var job = a_linie[0].Trim();
							double jobid;
							var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
							if (isNum)
							{
								var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[3].Trim());
								if (a_linie[3].Trim().Length == 0 || (a_linie[3].Trim().Length < 9))
								{
									loggerRezultate.Info("EROARE la linia Pos=" + a_linie[0].Trim());
								}
								var cod_gresit = false;

								if ((ErrorDouble(a_linie[3].Trim()) && a_linie[3].Trim() != "") || //bar code
									(ErrorDouble(a_linie[4].Trim()) && a_linie[4].Trim() != "") || //fat
									(ErrorDouble(a_linie[5].Trim()) && a_linie[5].Trim() != "") || //protein
									(ErrorDouble(a_linie[6].Trim()) && a_linie[6].Trim() != "") || //lactose
									(ErrorDouble(a_linie[7].Trim()) && a_linie[7].Trim() != "") || //solid
									(ErrorInt(a_linie[9].Trim()) && a_linie[9].Trim() != ""))      //cel nr
								{
									loggerRezultate.Info("EROARE: Valoare incorecta la linia Pos=" + a_linie[0].Trim());
									cod_gresit = true;
								}
								// verific daca mostra a mai fost analizata
								bool analizatg, analizatl, analizatp, analizats, analizatn;
								analizatg = analizatl = analizats = analizatp = analizatn = false;
								if (mostra != null && !cod_gresit)
								{
									if (mostra.Grasime.Trim() != "0")
									{
										if (a_linie[4].Trim() == "") a_linie[4] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[4].Trim()) - Convert.ToDouble(mostra.Grasime)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[3].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Grasime! ");
											analizatg = true;
										}
									}
									if (mostra.ProcentProteine.Trim() != "0")
									{
										if (a_linie[5].Trim() == "") a_linie[5] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[5].Trim()) - Convert.ToDouble(mostra.ProcentProteine)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[3].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Proteine! ");
											analizatp = true;
										}
									}
									if (mostra.ProcentLactoza.Trim() != "0")
									{
										if (a_linie[6].Trim() == "") a_linie[6] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[6].Trim()) - Convert.ToDouble(mostra.ProcentLactoza)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[3].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Lactoza! ");
											analizatl = true;
										}
									}
									if (mostra.SubstrantaUscata.Trim() != "0")
									{
										if (a_linie[7].Trim() == "") a_linie[7] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[7].Trim()) - Convert.ToDouble(mostra.SubstrantaUscata)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[3].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Substanta Uscata! ");
											analizats = true;
										}
									}
									if (mostra.NumarCeluleSomatice.Trim() != "0")
									{
										if (a_linie[9].Trim() == "") a_linie[9] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[9].Trim()) - Convert.ToDouble(mostra.NumarCeluleSomatice)) > 0.000001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[3].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Substanta NCS! ");
											analizatn = true;
										}
									}

								}

								// end verificare
								if (!cod_gresit && (a_linie[3].Trim() != "1111111111"))
								{

									DateTime dtfinala = new DateTime(Int32.Parse(dataTestareFinala.Substring(6, 4)), Int32.Parse(dataTestareFinala.Substring(3, 2)), Int32.Parse(dataTestareFinala.Substring(0, 2)));
									if (mostra.DataTestare != "")
									{
										DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
										if (DateTime.Compare(dtfinala, dt) < 0)
										{
											loggerRezultate.Info("EROARE: DataTestare finala(" + dataTestareFinala + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
											fileError = true;
										}
										else
										{
											// incepand din data de 22 ianuarie 2008 daca aparatul da rezultat 0 pt ncs si ntg se va insera 0.00001
											// si va fi tratat separat in afisare in lista afisandu-se valoare 0 in caz de 0.00001,  in editare mostre 0.00001 si in rapoarte 0
											// facandu-se diferenta intre valoarea 0 care inseamna neanalizat si valoarea 0 de la aparat fin
											string ncs_val;
											if (a_linie[9].Trim() == "0")
												ncs_val = "0.00001";
											else
												ncs_val = a_linie[9].Trim();
											string grasime_val;
											if (a_linie[4].Trim() == "0")
												grasime_val = "0.00001";
											else
												grasime_val = a_linie[4].Trim();
											string proteine_val;
											if (a_linie[5].Trim() == "0")
												proteine_val = "0.00001";
											else
												proteine_val = a_linie[5].Trim();
											string lactoza_val;
											if (a_linie[6].Trim() == "0")
												lactoza_val = "0.00001";
											else
												lactoza_val = a_linie[6].Trim();
											string substu_val;
											if (a_linie[7].Trim() == "0")
												substu_val = "0.00001";
											else
												substu_val = a_linie[7].Trim();
											if (a_linie[4].Trim() == "") { grasime_val = "0"; };
											if (a_linie[5].Trim() == "") { proteine_val = "0"; };
											if (a_linie[6].Trim() == "") { lactoza_val = "0"; };
											if (a_linie[7].Trim() == "") { substu_val = "0"; };
											if (a_linie[9].Trim() == "") { ncs_val = "0"; };

											UpdateMostraCombi(a_linie[3].Trim(), dataTestareFinala, ncs_val, substu_val, lactoza_val, proteine_val, grasime_val, analizatg, analizatl, analizatp, analizats, analizatn);
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						loggerRezultate.Info(string.Format("C1LineProcess|EROARE la linia cu Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message));
					}

				}
			}
		}
	}

	private void CLineProcess(StreamReader sr, string dataTestareFinala, List<Mostra> mostre)
	{
		var linie = sr.ReadLine();
		var lineCnt = 1;
		while (linie != null)
		{
			linie = sr.ReadLine();
			lineCnt++;
			if (lineCnt > 4)
			{
				if (linie != null)
				{
					var a_linie = linie.Split(';');
					try
					{
						if (a_linie[0] != null)
						{
							var job = a_linie[0].ToString().Trim();
							double jobid;
							var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
							if (isNum)
							{
								var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[2].Trim());
								var idxPilot = a_linie[2].IndexOf("Pilot");
								if (a_linie[2].Trim().Length == 0 || (a_linie[2].Trim().Length < 9 && (idxPilot == -1)))
								{
									loggerRezultate.Info("EROARE la linia Pos=" + a_linie[0].Trim());
								}
								var cod_gresit = false;
								if ((ErrorDouble(a_linie[3].Trim()) && a_linie[3].Trim() != "") || (ErrorDouble(a_linie[4].Trim()) && a_linie[4].Trim() != "") ||
									(ErrorDouble(a_linie[5].Trim()) && a_linie[5].Trim() != "") ||
									(ErrorDouble(a_linie[6].Trim()) && a_linie[6].Trim() != "") || (ErrorDouble(a_linie[8].Trim()) && a_linie[8].Trim() != "") ||
									(ErrorInt(a_linie[9].Trim()) && a_linie[9].Trim() != ""))
								{
									loggerRezultate.Info("EROARE: Valoare incorecta la linia Pos=" + a_linie[0].Trim());
									cod_gresit = true;
								}
								// verific daca mostra a mai fost analizata
								bool analizatg, analizatl, analizatp, analizats, analizatn;
								analizatg = analizatl = analizats = analizatp = analizatn = false;
								if (mostra != null && !cod_gresit)
								{
									if (mostra.Grasime.Trim() != "0")
									{
										if (a_linie[3].Trim() == "") a_linie[3] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[3].Trim()) - Convert.ToDouble(mostra.Grasime)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[2].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Grasime! ");
											analizatg = true;
										}
									}
									if (mostra.ProcentProteine.Trim() != "0")
									{
										if (a_linie[4].Trim() == "") a_linie[4] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[4].Trim()) - Convert.ToDouble(mostra.ProcentProteine)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[2].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Proteine! ");
											analizatp = true;
										}
									}
									if (mostra.ProcentLactoza.Trim() != "0")
									{
										if (a_linie[5].Trim() == "") a_linie[5] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[5].Trim()) - Convert.ToDouble(mostra.ProcentLactoza)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[2].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Lactoza! ");
											analizatl = true;
										}
									}
									if (mostra.SubstrantaUscata.Trim() != "0")
									{
										if (a_linie[6].Trim() == "") a_linie[6] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[6].Trim()) - Convert.ToDouble(mostra.SubstrantaUscata)) > 0.00001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[2].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Substanta Uscata! ");
											analizats = true;
										}
									}
									if (mostra.NumarCeluleSomatice.Trim() != "0")
									{
										if (a_linie[9].Trim() == "") a_linie[9] = "0";
										if (Math.Abs(Convert.ToDouble(a_linie[9].Trim()) - Convert.ToDouble(mostra.NumarCeluleSomatice)) > 0.000001)
										{
											loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[2].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru Substanta NCS! ");
											analizatn = true;
										}
									}

								}

								// end verificare
								if (!cod_gresit && (a_linie[2].Trim() != "1111111111") && (idxPilot == -1))
								{
									DateTime dtfinala = new DateTime(Int32.Parse(dataTestareFinala.Substring(6, 4)), Int32.Parse(dataTestareFinala.Substring(3, 2)), Int32.Parse(dataTestareFinala.Substring(0, 2)));
									if (mostra.DataTestare != "")
									{
										DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
										if (DateTime.Compare(dtfinala, dt) < 0)
										{
											loggerRezultate.Info("EROARE: DataTestare finala(" + dataTestareFinala + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
										}
										else
										{
											// incepand din data de 22 ianuarie 2008 daca aparatul da rezultat 0 pt ncs si ntg se va insera 0.00001
											// si va fi tratat separat in afisare in lista afisandu-se valoare 0 in caz de 0.00001,  in editare mostre 0.00001 si in rapoarte 0
											// facandu-se diferenta intre valoarea 0 care inseamna neanalizat si valoarea 0 de la aparat fin
											string ncs_val;
											if (a_linie[9].Trim() == "0")
												ncs_val = "0.00001";
											else
												ncs_val = a_linie[9].Trim();
											string grasime_val;
											if (a_linie[3].Trim() == "0")
												grasime_val = "0.00001";
											else
												grasime_val = a_linie[3].Trim();
											string proteine_val;
											if (a_linie[4].Trim() == "0")
												proteine_val = "0.00001";
											else
												proteine_val = a_linie[4].Trim();
											string lactoza_val;
											if (a_linie[5].Trim() == "0")
												lactoza_val = "0.00001";
											else
												lactoza_val = a_linie[5].Trim();
											string substu_val;
											if (a_linie[6].Trim() == "0")
												substu_val = "0.00001";
											else
												substu_val = a_linie[6].Trim();
											if (a_linie[3].Trim() == "") { grasime_val = "0"; };
											if (a_linie[4].Trim() == "") { proteine_val = "0"; };
											if (a_linie[5].Trim() == "") { lactoza_val = "0"; };
											if (a_linie[6].Trim() == "") { substu_val = "0"; };
											if (a_linie[9].Trim() == "") { ncs_val = "0"; };

											UpdateMostraCombi(a_linie[2].Trim(), dataTestareFinala, ncs_val, substu_val, lactoza_val, proteine_val, grasime_val, analizatg, analizatl, analizatp, analizats, analizatn);
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						loggerRezultate.Info(string.Format("CLineProcess|EROARE la linia cu Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message));
					}

				}
			}
		}
	}

	private void BLineProcess(int idCodBare, StreamReader sr, string dataTestareFinala, List<Mostra> mostre)
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
							var job = a_linie[0].ToString().Trim();
							double jobid;
							var isNum = double.TryParse(job, NumberStyles.Integer, CultureInfo.CurrentCulture, out jobid);
							if (isNum && a_linie[4].ToString().Trim() != "")
							{
								var mostra = mostre.FirstOrDefault(x => x.CodBare == a_linie[2].Trim());
								var idxPilot = a_linie[2].IndexOf("Pilot");
								if (a_linie[2].Trim().Length == 0 || (a_linie[2].Trim().Length < 9 && (idxPilot == -1)))
								{
									loggerRezultate.Info("EROARE la linia Pos=" + a_linie[0].Trim());
								}

								var cod_gresit = false;

								if (ErrorDouble(a_linie[4].Trim()))
								{
									loggerRezultate.Info("EROARE: Valoare incorecta la Pos=" + a_linie[0].Trim());
									cod_gresit = true;
								}
								if (mostra != null && !cod_gresit && mostra.IncarcaturaGermeni.Trim() != "0")
								{
									if (Math.Abs(Convert.ToDouble(a_linie[4].Trim()) - Convert.ToDouble(mostra.IncarcaturaGermeni)) > 0.000001)
										loggerRezultate.Info("EROARE: Codul de bare=" + a_linie[2].Trim() + " a mai fost analizat, avand ca rezultat alte valori pentru NTG! ");
									cod_gresit = true;
								}

								if (!cod_gresit && (a_linie[2].Trim() != "1111111111") && (idxPilot == -1))
								{
									DateTime dtfinala = new DateTime(Int32.Parse(dataTestareFinala.Substring(6, 4)), Int32.Parse(dataTestareFinala.Substring(3, 2)), Int32.Parse(dataTestareFinala.Substring(0, 2)));
									if (mostra.DataTestare != "")
									{
										DateTime dt = new DateTime(Int32.Parse(mostra.DataTestare.Substring(6, 4)), Int32.Parse(mostra.DataTestare.Substring(3, 2)), Int32.Parse(mostra.DataTestare.Substring(0, 2)));
										if (DateTime.Compare(dtfinala, dt) < 0)
										{
											loggerRezultate.Info("EROARE: DataTestare finala(" + dataTestareFinala + ") mai mica dacat data testare (" + mostra.DataTestare.Trim() + ")");
											fileError = true;
										}
										else
										{
											string ntg_val;
											if (a_linie[4].Trim() == "0")
												ntg_val = "0.00001";
											else
												ntg_val = a_linie[4].Trim();
											if (a_linie[4].Trim() == "") { ntg_val = "0"; };

											UpdateMostraB(idCodBare, mostra.CodBare, ntg_val, dataTestareFinala);
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						loggerRezultate.Info(string.Format("BLineProcess|EROARE la linia Pos={0} ERROR:{1}", a_linie[0].Trim(), ex.Message));
					}

				}
			}
		}
	}
}



