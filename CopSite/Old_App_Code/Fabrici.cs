using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

//*********************************************************************
//
// Fabrici Class
//
// Class that encapsulates all data logic necessary to add/query/delete
// Fabrici within the Portal database.
//
//*********************************************************************
using System.Web.UI.WebControls;

public class Fabrici 
	{
	public RadioButtonList JudeteList;
		//*********************************************************************
		//
		// GetFabrici Method
		//
		// The GetFabrici method returns a DataSet containing all of the
		// Fabrici for a specific portal module from the Fabrici
		// database.
		//
		// NOTE: A DataSet is returned from this method to allow this method to support
		//
		//*********************************************************************

		public DataSet GetFabrici() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_GetFabrici", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			/* SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
			 parameterModuleId.Value = moduleId;
			 myCommand.SelectCommand.Parameters.Add(parameterModuleId);*/

			// Create and Fill the DataSet
			DataSet myDataSet = new DataSet();
			myCommand.Fill(myDataSet);

			// Return the DataSet
			return myDataSet;
		}

		public SqlDataReader GetAllFabrici() 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFabrici", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		public SqlDataReader GetAllFabriciPeriod(string data1, string data2) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFabriciPeriod", myConnection);

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

		public SqlDataReader GetAllFabriciDay(string data1) 
		{
			
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllFabriciDay", myConnection);

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
	
		public int GetTotalFabrici() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_TotalFabrici", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterTotalFabrici = new SqlParameter("@TotalFabrici", SqlDbType.Int, 4);
			parameterTotalFabrici.Direction = ParameterDirection.Output;
			myCommand.SelectCommand.Parameters.Add(parameterTotalFabrici);

			myConnection.Open();
			myCommand.SelectCommand.ExecuteNonQuery();
			myConnection.Close();

			if (DBNull.Value != parameterTotalFabrici.Value) 
				return (int)parameterTotalFabrici.Value;
			else
				return (0);					

		}

		public DataSet FabriciPage(int PageIndex, int PageSize) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_Fabrici_Get", myConnection);

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

		public DataSet SearchFabrici(string campo_search) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);

			// Mark the Command as a SPROC
			myConnection.Open();
			SqlDataAdapter myCommand = new SqlDataAdapter ();
			string query_str = GetQuery(campo_search);
			myCommand.SelectCommand = new SqlCommand(query_str, myConnection);
			
			// Create the DataSet	
			DataSet myDataSet = new DataSet();
		
			myCommand.Fill(myDataSet);
				
			myConnection.Close();

			// Return the DataSet
			return (myDataSet);
		}

		public string GetQuery(string campo_search)
		{
			string keyword_s;
			string query_str = null;
			string expresion;
			query_str = "SELECT *,Judete.Denloc "+
				"FROM Fabrici,Judete WHERE 1=1 ";
			//string[] keywordsExclus = new string[] {"and", "or", "the", "in", "of", "if", "is"};
			
			bool expres = false;
			for (int k=0; k < campo_search.Length;k++)
			{
				keyword_s = null;
				expresion = null;
				if (campo_search[k] == '\"' && !expres)
				{
					expres = true;
					while (k < campo_search.Length && campo_search[++k] != '\"' )
						expresion += campo_search[k];
					if (expresion != null)
						query_str += " AND Nume LIKE '%" + expresion + "%' ";
				}
				else if (!expres)
				{
					while (k < campo_search.Length && campo_search[k] != ' ' )
					{
						keyword_s += campo_search[k];
						k++;
					}
					if (keyword_s != null)
					{
						query_str += "AND Nume LIKE '%" + keyword_s +"%' ";
					}
				}
				query_str += " AND Judete.ID=Fabrici.Judet ";
			}
			
			query_str += " ORDER BY Nume ";
			return query_str;
		}

		public SqlDataReader GetSingleFabrica(int Id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetSingleFabrica", myConnection);

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
		// DeleteFabrica Method
		//
		// The DeleteFabrica method deletes the specified Fabrica from
		// the Fabrici database table.
		//
		// Other relevant sources:
		//     + <a href="DeleteFabrica.htm" style="color:green">DeleteFabrica Stored Procedure</a>
		//
		//*********************************************************************

		public void DeleteFabrica(int ID) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_DeleteFabrica", myConnection);

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
		// AddFabrica Method
		//
		// The AddFabrica method adds a new Fabrica to the Fabrici
		// database table, and returns the Id value as a result.
		//
		// Other relevant sources:
		//     + <a href="AddFabrica.htm" style="color:green">AddFabrica Stored Procedure</a>
		//
		//*********************************************************************

		public int AddFabrica(int id, String nume, String strada, String numar, String oras, int judet, String codPostal, String telefon, String fax, String email, String personaDeContact, String telPersContact) 
		{
			
			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_AddFabrica", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterID);

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

			SqlParameter parameterTelPersContact = new SqlParameter("@TelPersContact", SqlDbType.NVarChar, 255);
			parameterTelPersContact.Value = telPersContact;
			myCommand.Parameters.Add(parameterTelPersContact);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			/* SET IDENTITY_INSERT Fabrici  ON*/
			myConnection.Close();

			return (int)parameterID.Value;
		}

		//*********************************************************************
		//
		// UpdateFabrica Method
		//
		// The UpdateFabrica method updates the specified Fabrica within
		// the Fabrici database table.
		//
		// Other relevant sources:
		//     + <a href="UpdateFabrica.htm" style="color:green">UpdateFabrica Stored Procedure</a>
		//
		//*********************************************************************

		public void UpdateFabrica(int id, String nume, String strada, String numar, String oras, int judet, String codPostal, String telefon, String fax, String email, String personaDeContact, String telPersContact) 
		{

			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_UpdateFabrici", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Value = id;
			myCommand.Parameters.Add(parameterID);

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

			SqlParameter parameterTelPersContact = new SqlParameter("@TelPersContact", SqlDbType.NVarChar, 255);
			parameterTelPersContact.Value = telPersContact;
			myCommand.Parameters.Add(parameterTelPersContact);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}
		public int GetFabricaID(string nume) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetFabricaID", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterIDFabrica = new SqlParameter("@FabricaID", SqlDbType.Int, 4);
			parameterIDFabrica.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterIDFabrica);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			if (DBNull.Value != parameterIDFabrica.Value) 
				return (int)parameterIDFabrica.Value;
			else
				return (0);				

		}

		public string GetEmailFabrica(int id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetEmailFabrica", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterEmailFabrica = new SqlParameter("@EmailFabrica", SqlDbType.NVarChar, 255);
			parameterEmailFabrica.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterEmailFabrica);

			SqlParameter parameterId = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterId.Value = id;
			myCommand.Parameters.Add(parameterId);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			if (parameterEmailFabrica.Value == DBNull.Value)
				return "";
			else
				return (string)parameterEmailFabrica.Value;
//			return (string)parameterEmailFabrica.Value;

		}
		public string GetFabricaName(int id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetFabricaName", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterName = new SqlParameter("@Name", SqlDbType.NVarChar, 255);
			parameterName.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterName);

			SqlParameter parameterId = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterId.Value = id;
			myCommand.Parameters.Add(parameterId);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			if (DBNull.Value != parameterName.Value) 
				return (string)parameterName.Value;
			else
				return ("");

		}
		public int ExistaNumeFabrica (string numefabrica)
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_ExistaNumeFabrica", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterID);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 150);
			parameterNume.Value = numefabrica;
			myCommand.Parameters.Add(parameterNume);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			if (parameterID.Value == DBNull.Value)
				return 0;
			else
				return (int)parameterID.Value;
		}
	}


