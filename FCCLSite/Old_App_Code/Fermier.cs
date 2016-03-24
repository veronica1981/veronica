using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

    //*********************************************************************
    //
    // Fermier Class
    //
    // Class that encapsulates all data logic necessary to add/query/delete
    // Fermier within the Portal database.
    //
    //*********************************************************************

	public class Fermier 
	{
		//*********************************************************************
		//
		// GetFermier Method
		//
		// The GetFermier method returns a DataSet containing all of the
		// Fermier for a specific portal module from the Fermier
		// database.
		//
		// NOTE: A DataSet is returned from this method to allow this method to support
		//
		//*********************************************************************

		public DataSet GetFermieri() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_GetFermieri", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Create and Fill the DataSet
			DataSet myDataSet = new DataSet();
			myCommand.Fill(myDataSet);

			// Return the DataSet
			return myDataSet;
		}

		public SqlDataReader GetAllFermieri() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetFermieri", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		/*public int GetMaxItemID() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetMaxIDFermier", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterItemID = new SqlParameter("@maxID", SqlDbType.Int, 4);
			parameterItemID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterItemID);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return (int)parameterItemID.Value;

		}*/

		public int GetTotalFermieri() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_TotalFermieri", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			/*SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
			parameterModuleID.Value = ModuleID;
			myCommand.SelectCommand.Parameters.Add(parameterModuleID);*/


			// Add Parameters to SPROC
			SqlParameter parameterTotalFermieri = new SqlParameter("@TotalFermieri", SqlDbType.Int, 4);
			parameterTotalFermieri.Direction = ParameterDirection.Output;
			myCommand.SelectCommand.Parameters.Add(parameterTotalFermieri);

			myConnection.Open();
			myCommand.SelectCommand.ExecuteNonQuery();
			myConnection.Close();

			return (int)parameterTotalFermieri.Value;

		}

		public DataSet FermierPage(int PageIndex, int PageSize) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_Fermier_Get", myConnection);

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

		public DataSet SearchFermier(string campo_search) 
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
				"FROM Fermier,Judete WHERE 1=1 ";
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
						query_str += " AND Nume LIKE '%" + keyword_s +"%' ";
					}
				}
				query_str += " AND Judete.ID=Fermier.Judet ";
			}
			
			query_str += " ORDER BY Nume ";
			return query_str;
		}
		public int GetFermierID(string nume) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetFermierID", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterIDFermier = new SqlParameter("@FermierID", SqlDbType.Int, 4);
			parameterIDFermier.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterIDFermier);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return (int)parameterIDFermier.Value;

		}

		public SqlDataReader GetSingleFermier(int Id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetSingleFermier", myConnection);

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
		// DeleteFermier Method
		//
		// The DeleteFermier method deletes the specified Fermier from
		// the Fermier database table.
		//
		// Other relevant sources:
		//     + <a href="DeleteFermier.htm" style="color:green">DeleteFermier Stored Procedure</a>
		//
		//*********************************************************************

		public void DeleteFermier(int ID) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_DeleteFermier", myConnection);

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
		// AddFermier Method
		//
		// The AddFermier method adds a new Fermier to the Fermier
		// database table, and returns the Id value as a result.
		//
		// Other relevant sources:
		//     + <a href="AddFermier.htm" style="color:green">AddFermier Stored Procedure</a>
		//
		//*********************************************************************

		public int AddFermier(int id, String nume, String strada, String numar, String oras, int judet, String codPostal, String telefon, String fax, String email, String numarReferinta, String telExtra, int clasaDeProductie) 
		{
			
			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_AddFermier", myConnection);

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

			SqlParameter parameterNumar = new SqlParameter("@Nr", SqlDbType.NVarChar, 255);
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

			SqlParameter parameterNumarReferinta = new SqlParameter("@NumarReferinta", SqlDbType.NVarChar, 255);
			parameterNumarReferinta.Value = numarReferinta;
			myCommand.Parameters.Add(parameterNumarReferinta);

			SqlParameter parameterTelExtra = new SqlParameter("@TelExtra", SqlDbType.NVarChar, 255);
			parameterTelExtra.Value = telExtra;
			myCommand.Parameters.Add(parameterTelExtra);

			SqlParameter parameterClasaProd = new SqlParameter("@ClasaDeProductie", SqlDbType.Int, 4);
			parameterClasaProd.Value = clasaDeProductie;
			myCommand.Parameters.Add(parameterClasaProd);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			/* SET IDENTITY_INSERT Fermier  ON*/
			myConnection.Close();

			return (int)parameterID.Value;
		}

		//*********************************************************************
		//
		// UpdateFermier Method
		//
		// The UpdateFermier method updates the specified Fermier within
		// the Fermier database table.
		//
		// Other relevant sources:
		//     + <a href="UpdateFermier.htm" style="color:green">UpdateFermier Stored Procedure</a>
		//
		//*********************************************************************

		public void UpdateFermier(int id, String nume, String strada, String numar, String oras, int judet, String codPostal, String telefon, String fax, String email, String numarReferinta, String telExtra, int clasaDeProductie) 
		{

			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_UpdateFermier", myConnection);

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

			SqlParameter parameterNumar = new SqlParameter("@Nr", SqlDbType.NVarChar, 255);
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

			SqlParameter parameterNumarReferinta = new SqlParameter("@NumarReferinta", SqlDbType.NVarChar, 255);
			parameterNumarReferinta.Value = numarReferinta;
			myCommand.Parameters.Add(parameterNumarReferinta);

			SqlParameter parameterTelExtra = new SqlParameter("@TelExtra", SqlDbType.NVarChar, 255);
			parameterTelExtra.Value = telExtra;
			myCommand.Parameters.Add(parameterTelExtra);

			SqlParameter parameterClasaProd = new SqlParameter("@ClasaDeProductie", SqlDbType.Int, 4);
			parameterClasaProd.Value = clasaDeProductie;
			myCommand.Parameters.Add(parameterClasaProd);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}

	}


