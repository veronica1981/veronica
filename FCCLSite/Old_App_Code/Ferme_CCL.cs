using NLog;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


    //*********************************************************************
    //
    // Ferme_CCL Class
    //
    // Class that encapsulates all data logic necessary to add/query/delete
    // Ferme_CCL within the Portal database.
    //
    //*********************************************************************

	public class Ferme_CCL 
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public DataSet GetFerme_CCL() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_GetFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Create and Fill the DataSet
			DataSet myDataSet = new DataSet();
			myCommand.Fill(myDataSet);

			// Return the DataSet
			return myDataSet;
		}

		public int GetTotalFerme_CCL() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_TotalFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterTotalFerme_CCL = new SqlParameter("@TotalFerme_CCL", SqlDbType.Int, 4);
			parameterTotalFerme_CCL.Direction = ParameterDirection.Output;
			myCommand.SelectCommand.Parameters.Add(parameterTotalFerme_CCL);

			myConnection.Open();
			myCommand.SelectCommand.ExecuteNonQuery();
			myConnection.Close();

			if (parameterTotalFerme_CCL.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterTotalFerme_CCL.Value;	
			
		}

		public DataSet Ferme_CCLPage(int PageIndex, int PageSize) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_Ferme_CCL_Get", myConnection);

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

		public DataSet SearchFerme_CCL(string search_nume, string f_c, string search_judet) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);

			// Mark the Command as a SPROC
			myConnection.Open();
			SqlDataAdapter myCommand = new SqlDataAdapter ();
			string query_str = GetQuery(search_nume, f_c, search_judet);
			myCommand.SelectCommand = new SqlCommand(query_str, myConnection);
			
			// Create the DataSet	
			DataSet myDataSet = new DataSet();
		
			myCommand.Fill(myDataSet);
				
			myConnection.Close();

			// Return the DataSet
			return (myDataSet);
		}

		public string GetQuery(string search_nume, string f_c, string search_judet)
		{
			string keyword_s;
			string query_str = null;
			string expresion;
			query_str = "SELECT Ferme_CCL.ID, Ferme_CCL.IDOld, Ferme_CCL.Nume, Ferme_CCL.Strada, Ferme_CCL.Numar, Ferme_CCL.Oras, Ferme_CCL.Judet, Ferme_CCL.CodPostal, Ferme_CCL.Telefon, Ferme_CCL.Fax, Ferme_CCL.Email, Ferme_CCL.Cod, Ferme_CCL.TelPersoanaContact, Ferme_CCL.FabricaID, Ferme_CCL.FermierID, Ferme_CCL.DataAchizitie,"+
	"Ferme_CCL.Ferme_CCL,Judete.Denloc, Fabrici.Nume as NumeFabrica, Fermier.Nume as NumeFermier "+
				"FROM Ferme_CCL LEFT OUTER JOIN "+
                      "Judete ON Ferme_CCL.Judet = Judete.ID LEFT OUTER JOIN "+
                      "Fabrici ON Ferme_CCL.FabricaID = Fabrici.ID LEFT OUTER JOIN "+
                      "Fermier ON Ferme_CCL.FermierID = Fermier.FermierID WHERE 1=1 ";
						
			bool expres = false;
			for (int k=0; k < search_nume.Length;k++)
			{
				keyword_s = null;
				expresion = null;
				if (search_nume[k] == '\"' && !expres)
				{
					expres = true;
					while (k < search_nume.Length && search_nume[++k] != '\"' )
						expresion += search_nume[k];
					if (expresion != null)
						query_str += " AND Ferme_CCL.Nume LIKE '%" + expresion + "%' ";
				}
				else if (!expres)
				{
					while (k < search_nume.Length && search_nume[k] != ' ' )
					{
						keyword_s += search_nume[k];
						k++;
					}
					
					if ( keyword_s != null)
					{
						query_str += " AND Ferme_CCL.Nume LIKE '%" + keyword_s +"%' ";
					}
				}
			}
			if ( f_c != null && f_c != "")
			{
					query_str += " AND Ferme_CCL='"+f_c+"'";
			}
			if ( search_judet != null && search_judet != "")
			{
				query_str += " AND Judete.Denloc='"+search_judet+"'";
			}
							
			query_str += " ORDER BY Ferme_CCL.Nume ";
			return query_str;
		}

		public SqlDataReader GetSingleFerme_CCL(int Id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetSingleFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterId = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterId.Value = Id;
			myCommand.Parameters.Add(parameterId);

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		//*********************************************************************
		//
		// DeleteFerme_CCL Method
		//
		// The DeleteFerme_CCL method deletes the specified Ferme_CCL from
		// the Ferme_CCL database table.
		//
		// Other relevant sources:
		//     + <a href="DeleteFerme_CCL.htm" style="color:green">DeleteFerme_CCL Stored Procedure</a>
		//
		//*********************************************************************

		public void DeleteFerme_CCL(int ID) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_DeleteFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Value = ID;
			myCommand.Parameters.Add(parameterID);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

		//*********************************************************************
		//
		// AddFerme_CCL Method
		//
		// The AddFerme_CCL method adds a new Ferme_CCL to the Ferme_CCL
		// database table, and returns the Id value as a result.
		//
		// Other relevant sources:
		//     + <a href="AddFerme_CCL.htm" style="color:green">AddFerme_CCL Stored Procedure</a>
		//
		//*********************************************************************

		public int AddFerme_CCL(int id, int idOld, String nume, String strada, String numar, String oras, int judet, String codPostal, String telefon, String fax, String email, String cod, String personaDeContact, String telPersoanaContact,  int fabricaID, int fermierID, String dataAchizitie, String ferme_CCL) 
		{
			
			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_AddFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterID);

			SqlParameter parameterIDOld = new SqlParameter("@IDOld", SqlDbType.Int, 4);
			parameterIDOld.Value = idOld;
			myCommand.Parameters.Add(parameterIDOld);

			SqlParameter parameterCod = new SqlParameter("@Cod", SqlDbType.NVarChar, 255);
			parameterCod.Value = cod;
			myCommand.Parameters.Add(parameterCod);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			SqlParameter parameterStrada = new SqlParameter("@Strada", SqlDbType.NVarChar, 255);
			parameterStrada.Value = strada;
			myCommand.Parameters.Add(parameterStrada);

			SqlParameter parameterNumar = new SqlParameter("@Numar", SqlDbType.NVarChar, 255);
			parameterNumar.Value = numar;
			myCommand.Parameters.Add(parameterNumar);

			SqlParameter parameterOras = new SqlParameter("@Oras", SqlDbType.NVarChar, 255);
			parameterOras.Value = oras;
			myCommand.Parameters.Add(parameterOras);

			SqlParameter parameterJudet = new SqlParameter("@Judet", SqlDbType.Int, 4);
			parameterJudet.Value = judet;
			myCommand.Parameters.Add(parameterJudet);

			SqlParameter parameterCodPostal = new SqlParameter("@CodPostal", SqlDbType.NVarChar, 255);
			parameterCodPostal.Value = codPostal;
			myCommand.Parameters.Add(parameterCodPostal);

			SqlParameter parameterTelefon = new SqlParameter("@Telefon", SqlDbType.NVarChar, 255);
			parameterTelefon.Value = telefon;
			myCommand.Parameters.Add(parameterTelefon);

			SqlParameter parameterFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 255);
			parameterFax.Value = fax;
			myCommand.Parameters.Add(parameterFax);

			SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 255);
			parameterEmail.Value = email;
			myCommand.Parameters.Add(parameterEmail);

			SqlParameter parameterPersonaDeContact = new SqlParameter("@PersonaDeContact", SqlDbType.NVarChar, 255);
			parameterPersonaDeContact.Value = personaDeContact;
			myCommand.Parameters.Add(parameterPersonaDeContact);

			SqlParameter parameterTelPersoanaContact = new SqlParameter("@TelPersoanaContact", SqlDbType.NVarChar, 255);
			parameterTelPersoanaContact.Value = telPersoanaContact;
			myCommand.Parameters.Add(parameterTelPersoanaContact);

			SqlParameter parameterFermierID = new SqlParameter("@FermierID", SqlDbType.Int, 4);
			parameterFermierID.Value = fermierID;
			myCommand.Parameters.Add(parameterFermierID);

			SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterFabricaID.Value = fabricaID;
			myCommand.Parameters.Add(parameterFabricaID);

			SqlParameter parameterDataAchizitie = new SqlParameter("@DataAchizitie", SqlDbType.NVarChar, 255);
			parameterDataAchizitie.Value = dataAchizitie;
			myCommand.Parameters.Add(parameterDataAchizitie);

			SqlParameter parameterFerme_CCL = new SqlParameter("@Ferme_CCL", SqlDbType.NChar, 1);
			parameterFerme_CCL.Value = ferme_CCL;
			myCommand.Parameters.Add(parameterFerme_CCL);


			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			if (parameterID.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterID.Value;			
		}
		public void AddValidFabricaFerme_CCL (int ferma_CCLID, int fabricaID, DateTime validDate/*, int fabricaIDNou*/)
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_AddValidFabricaFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterFabricaID.Value = fabricaID;
			myCommand.Parameters.Add(parameterFabricaID);

			// Add Parameters to SPROC
			/*SqlParameter parameterFabricaIDNou = new SqlParameter("@FabricaIDNou", SqlDbType.Int, 4);
			parameterFabricaIDNou.Value = fabricaIDNou;
			myCommand.Parameters.Add(parameterFabricaIDNou);*/

			SqlParameter parameterFerma_CCLID = new SqlParameter("@Ferma_CCLID", SqlDbType.Int, 4);
			parameterFerma_CCLID.Value = ferma_CCLID;
			myCommand.Parameters.Add(parameterFerma_CCLID);

			// Add Parameters to SPROC
			SqlParameter parameterValidDate = new SqlParameter("@ValidDate", SqlDbType.DateTime, 8);
			parameterValidDate.Value = validDate;
			myCommand.Parameters.Add(parameterValidDate);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

		}
		public int ExistaCodFerma (string codferma)
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_ExistaCodFerma", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterID);

			SqlParameter parametercod = new SqlParameter("@Cod", SqlDbType.NVarChar, 150);
			parametercod.Value = codferma;
			myCommand.Parameters.Add(parametercod);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			if (parameterID.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterID.Value;
		}
	
		//*********************************************************************
		//
		// UpdateFerme_CCL Method
		//
		// The UpdateFerme_CCL method updates the specified Ferme_CCL within
		// the Ferme_CCL database table.
		//
		// Other relevant sources:
		//     + <a href="UpdateFerme_CCL.htm" style="color:green">UpdateFerme_CCL Stored Procedure</a>
		//
		//*********************************************************************

		public void UpdateFerme_CCL(int id, String nume, String strada, String numar, String oras, int judet, String codPostal, String telefon, String fax, String email, String cod, String personaDeContact, String telPersoanaContact, int fabricaID, int fermierID, String dataAchizitie, String ferme_CCL) 
		{

			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_UpdateFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Value = id;
			myCommand.Parameters.Add(parameterID);

			// Add Parameters to SPROC
			/*SqlParameter parameterIDOld = new SqlParameter("@IDOld", SqlDbType.Int, 4);
			parameterIDOld.Value = idOld;
			myCommand.Parameters.Add(parameterIDOld);*/

			SqlParameter parameterCod = new SqlParameter("@Cod", SqlDbType.NVarChar, 255);
			parameterCod.Value = cod;
			myCommand.Parameters.Add(parameterCod);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			SqlParameter parameterStrada = new SqlParameter("@Strada", SqlDbType.NVarChar, 255);
			parameterStrada.Value = strada;
			myCommand.Parameters.Add(parameterStrada);

			SqlParameter parameterNumar = new SqlParameter("@Numar", SqlDbType.NVarChar, 255);
			parameterNumar.Value = numar;
			myCommand.Parameters.Add(parameterNumar);

			SqlParameter parameterOras = new SqlParameter("@Oras", SqlDbType.NVarChar, 255);
			parameterOras.Value = oras;
			myCommand.Parameters.Add(parameterOras);

			SqlParameter parameterJudet = new SqlParameter("@Judet", SqlDbType.Int, 4);
			parameterJudet.Value = judet;
			myCommand.Parameters.Add(parameterJudet);

			SqlParameter parameterCodPostal = new SqlParameter("@CodPostal", SqlDbType.NVarChar, 255);
			parameterCodPostal.Value = codPostal;
			myCommand.Parameters.Add(parameterCodPostal);

			SqlParameter parameterTelefon = new SqlParameter("@Telefon", SqlDbType.NVarChar, 255);
			parameterTelefon.Value = telefon;
			myCommand.Parameters.Add(parameterTelefon);

			SqlParameter parameterFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 255);
			parameterFax.Value = fax;
			myCommand.Parameters.Add(parameterFax);

			SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 255);
			parameterEmail.Value = email;
			myCommand.Parameters.Add(parameterEmail);

			SqlParameter parameterPersonaDeContact = new SqlParameter("@PersonaDeContact", SqlDbType.NVarChar, 255);
			parameterPersonaDeContact.Value = personaDeContact;
			myCommand.Parameters.Add(parameterPersonaDeContact);

			SqlParameter parameterTelPersoanaContact = new SqlParameter("@TelPersoanaContact", SqlDbType.NVarChar, 255);
			parameterTelPersoanaContact.Value = telPersoanaContact;
			myCommand.Parameters.Add(parameterTelPersoanaContact);

			SqlParameter parameterFermierID = new SqlParameter("@FermierID", SqlDbType.Int, 4);
			parameterFermierID.Value = fermierID;
			myCommand.Parameters.Add(parameterFermierID);

			SqlParameter parameterDataAchizitie = new SqlParameter("@DataAchizitie", SqlDbType.NVarChar, 255);
			parameterDataAchizitie.Value = dataAchizitie;
			myCommand.Parameters.Add(parameterDataAchizitie);

			SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterFabricaID.Value = fabricaID;
			myCommand.Parameters.Add(parameterFabricaID);

			SqlParameter parameterFerme_CCL = new SqlParameter("@Ferme_CCL", SqlDbType.NChar, 1);
			parameterFerme_CCL.Value = ferme_CCL;
			myCommand.Parameters.Add(parameterFerme_CCL);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}
		public int GetIDOldMaxFerme_CCL()
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetIDOldMaxFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterMaxIDOld = new SqlParameter("@maxIDOld", SqlDbType.Int, 4);
			parameterMaxIDOld.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterMaxIDOld);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			if (parameterMaxIDOld.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterMaxIDOld.Value;	
			
		}

	    public int GetFerma(int idFerma, out string numeFerma)
	    {
            numeFerma = string.Empty;
            int idFabrica = 0;
	        var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
            var cmd = new SqlCommand("SELECT fa.Id, fe.Nume FROM Ferme_CCL fe JOIN Fabrici fa ON fa.Id = fe.FabricaId WHERE fe.ID=@id");
	        cmd.Parameters.AddWithValue("@id", idFerma);
	        cmd.Connection = cn;
            cn.Open();
            var reader = cmd.ExecuteReader();
	        try
	        {
	            if (reader.Read())
	            {
                    idFabrica = reader.GetInt32(0);
	                numeFerma = reader.GetString(1);
	            }
	        }
	        catch (Exception ex)
	        {
                logger.Fatal(string.Format("GetFerma|id:{0}|msg:{1}|stack:{2}", idFerma, ex.Message, ex.StackTrace));
	        }
	        finally
	        {
                reader.Close();
                cn.Close();
	        }
	        return idFabrica;
	    }

	    public DataSet ExportXLS() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_GetFerme_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Create and Fill the DataSet
			DataSet myDataSet = new DataSet();
			myCommand.Fill(myDataSet);

			// Return the DataSet
			return myDataSet;
		}
		public SqlDataReader GetAllFerme() 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFerme", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		public SqlDataReader GetAllFermePeriod(string data1, string data2) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFermePeriod", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameterData1 = new SqlParameter("@Data1", SqlDbType.NVarChar, 10);
			parameterData1.Value = data1;
			myCommand.Parameters.Add(parameterData1);

			SqlParameter parameterData2 = new SqlParameter("@Data2", SqlDbType.NVarChar, 10);
			parameterData2.Value = data2;
			myCommand.Parameters.Add(parameterData2);


			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		public SqlDataReader GetAllFermeDay(string data1) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFermeDay", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameterData1 = new SqlParameter("@Data1", SqlDbType.NVarChar, 10);
			parameterData1.Value = data1;
			myCommand.Parameters.Add(parameterData1);

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		public SqlDataReader GetAllFermeProcesatorDay(string data1, int fabricaId) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFermeProcesatorDay", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameterData1 = new SqlParameter("@Data1", SqlDbType.NVarChar, 10);
			parameterData1.Value = data1;
			myCommand.Parameters.Add(parameterData1);

			SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterFabricaID.Value = fabricaId;
			myCommand.Parameters.Add(parameterFabricaID);

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}


		public SqlDataReader GetFermeProcesator(string data1, int fabricaID/*, string ferma1, string ferma2*/) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetFermeProcesator", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter parameterData1 = new SqlParameter("@Data1", SqlDbType.NVarChar, 10);
			parameterData1.Value = data1;
			myCommand.Parameters.Add(parameterData1);

			SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterFabricaID.Value = fabricaID;
			myCommand.Parameters.Add(parameterFabricaID);

			/*SqlParameter parameterFerma1 = new SqlParameter("@Ferma1", SqlDbType.NVarChar, 255);
			parameterFerma1.Value = ferma1;
			myCommand.Parameters.Add(parameterFerma1);

			SqlParameter parameterFerma2 = new SqlParameter("@Ferma2", SqlDbType.NVarChar, 255);
			parameterFerma2.Value = ferma2;
			myCommand.Parameters.Add(parameterFerma2);*/


			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		public int GetCountFermeProcesator(string data1, int fabricaID/*, string ferma1, string ferma2*/) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetCountFermeProcesator", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterNrFerme = new SqlParameter("@NrFerme", SqlDbType.Int, 4);
			parameterNrFerme.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterNrFerme);

			SqlParameter parameterData1 = new SqlParameter("@Data1", SqlDbType.NVarChar, 10);
			parameterData1.Value = data1;
			myCommand.Parameters.Add(parameterData1);

			SqlParameter parameterFabricaID = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterFabricaID.Value = fabricaID;
			myCommand.Parameters.Add(parameterFabricaID);

			/*SqlParameter parameterFerma1 = new SqlParameter("@Ferma1", SqlDbType.NVarChar, 255);
			parameterFerma1.Value = ferma1;
			myCommand.Parameters.Add(parameterFerma1);

			SqlParameter parameterFerma2 = new SqlParameter("@Ferma2", SqlDbType.NVarChar, 255);
			parameterFerma2.Value = ferma2;
			myCommand.Parameters.Add(parameterFerma2);*/

			
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			if (parameterNrFerme.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterNrFerme.Value;

		}

		public string GetEmailFerma(int id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetEmailFerma", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterEmailFerma = new SqlParameter("@EmailFerma", SqlDbType.NVarChar, 255);
			parameterEmailFerma.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterEmailFerma);

			SqlParameter parameterId = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterId.Value = id;
			myCommand.Parameters.Add(parameterId);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			if (parameterEmailFerma.Value == DBNull.Value)
				return "";
			else
				return (string)parameterEmailFerma.Value;
			//return (string)parameterEmailFerma.Value;

		}

		public string GetCodFerma_CCL(int id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetCodFerma_CCL", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterCod = new SqlParameter("@Cod", SqlDbType.NVarChar, 255);
			parameterCod.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterCod);

			SqlParameter parameterId = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterId.Value = id;
			myCommand.Parameters.Add(parameterId);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			if (parameterCod.Value == DBNull.Value)
				return "";
			else
				return (string)parameterCod.Value;
			
		}

		public int GetFermaID(string nume) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetFermaID", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterIDFerma = new SqlParameter("@FermaID", SqlDbType.Int, 4);
			parameterIDFerma.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterIDFerma);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			if (parameterIDFerma.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterIDFerma.Value;	
		}
		public string GetNumeFerma(int idFerma) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetNumeFerma", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterIDFerma = new SqlParameter("@FermaID", SqlDbType.Int, 4);
			parameterIDFerma.Value = idFerma;
			myCommand.Parameters.Add(parameterIDFerma);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterNume);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			if (parameterNume.Value == DBNull.Value)
				return "";
			else
				return (string)parameterNume.Value;	
			

		}
	}


