using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

	//*********************************************************************
	//
	// Mostre Class
	//
	// Class that encapsulates all data logic necessary to add/query/delete
	// Mostre within the Portal database.
	//
	//*********************************************************************

	public class PrelevatoriDB 
	{
		public DataSet GetPrelevatori() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_GetPrelevatori", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Create and Fill the DataSet
			DataSet myDataSet = new DataSet();
			myCommand.Fill(myDataSet);

			// Return the DataSet
			return myDataSet;
		}

		public SqlDataReader GetAllPrelevatori() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllPrelevatori", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}

		public int GetCodMaxPrelevator() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetCodMaxPrelevator", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;
		
			/*SqlParameter parameterCod = new SqlParameter("@cod", SqlDbType.Int, 4);
			parameterCod.Value = cod;
			myCommand.Parameters.Add(parameterCod);*/
		
			SqlParameter parameterCodMaxPrelevator = new SqlParameter("@codMaxPrelevator", SqlDbType.Int, 4);
			parameterCodMaxPrelevator.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterCodMaxPrelevator);

			// Create and Fill the DataSet
			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
			
			if (DBNull.Value != parameterCodMaxPrelevator.Value) 
				return (int)parameterCodMaxPrelevator.Value;
			else
				return (0);
		
		}

		public string GetPrelevator (int prelevatorID)
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

			if (DBNull.Value != parameterNumePrelevator.Value) 
				return (string)parameterNumePrelevator.Value;
			else
				return ("");
		
		}

		public int GetPrelevatorID (string prelevator)
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetPrelevatorID", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterPrelevatorID = new SqlParameter("@PrelevatorID", SqlDbType.Int, 4);
			parameterPrelevatorID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterPrelevatorID);

			SqlParameter parameterNumePrelevator = new SqlParameter("@NumePrelevator", SqlDbType.NVarChar, 150);
			parameterNumePrelevator.Value = prelevator;
			myCommand.Parameters.Add(parameterNumePrelevator);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			if (DBNull.Value != parameterPrelevatorID.Value) 
				return (int)parameterPrelevatorID.Value;
			else
				return (0);			
		}
		public int GetTotalPrelevatori() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_TotalPrelevatori", myConnection);

			// Mark the Command as a SPROC
			myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterTotalPrelevatori = new SqlParameter("@TotalPrelevatori", SqlDbType.Int, 4);
			parameterTotalPrelevatori.Direction = ParameterDirection.Output;
			myCommand.SelectCommand.Parameters.Add(parameterTotalPrelevatori);

			myConnection.Open();
			myCommand.SelectCommand.ExecuteNonQuery();
			myConnection.Close();

			if (DBNull.Value != parameterTotalPrelevatori.Value) 
				return (int)parameterTotalPrelevatori.Value;
			else
				return (0);					

		}

		public DataSet PrelevatorPage(int PageIndex, int PageSize) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlDataAdapter myCommand = new SqlDataAdapter("Portal_Prelevatori_Get", myConnection);

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

		public DataSet SearchPrelevator(string campo_search) 
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
			query_str = "SELECT * "+
				"FROM Prelevatori WHERE 1=1 ";
		
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
						query_str += " AND NumePrelevator LIKE '%" + keyword_s +"%' ";
					}
				}
			}
			
			query_str += " ORDER BY CodPrelevator ";
			return query_str;
		}

		public SqlDataReader GetSinglePrelevator(int Id) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetSinglePrelevator", myConnection);

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
		// DeletePrelevator Method
		//
		// The DeletePrelevator method deletes the specified Prelevator from
		// the Prelevator database table.
		//
		// Other relevant sources:
		//     + <a href="DeletePrelevator.htm" style="color:green">DeletePrelevator Stored Procedure</a>
		//
		//*********************************************************************

		public void DeletePrelevator(int ID) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_DeletePrelevator", myConnection);

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
		// AddPrelevator Method
		//
		// The AddPrelevator method adds a new Prelevator to the Prelevator
		// database table, and returns the Id value as a result.
		//
		// Other relevant sources:
		//     + <a href="AddPrelevator.htm" style="color:green">AddPrelevator Stored Procedure</a>
		//
		//*********************************************************************

		public int AddPrelevator(int id, String nume, int cod) 
		{
			
			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_AddPrelevator", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterID);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			SqlParameter parameterCod = new SqlParameter("@Cod", SqlDbType.Int, 4);
			parameterCod.Value = cod;
			myCommand.Parameters.Add(parameterCod);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();

			return (int)parameterID.Value;
		}

		//*********************************************************************
		//
		// UpdatePrelevator Method
		//
		// The UpdatePrelevator method updates the specified Prelevator within
		// the Prelevator database table.
		//
		// Other relevant sources:
		//     + <a href="UpdatePrelevator.htm" style="color:green">UpdatePrelevator Stored Procedure</a>
		//
		//*********************************************************************

		public void UpdatePrelevator(int id, String nume/*, int cod*/) 
		{

			if (nume.Length < 1) 
			{
				nume = "unknown";
			}

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_UpdatePrelevator", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ItemID", SqlDbType.Int, 4);
			parameterID.Value = id;
			myCommand.Parameters.Add(parameterID);

			SqlParameter parameterNume = new SqlParameter("@Nume", SqlDbType.NVarChar, 255);
			parameterNume.Value = nume;
			myCommand.Parameters.Add(parameterNume);

			/*SqlParameter parameterCod = new SqlParameter("@Cod", SqlDbType.Int, 4);
			parameterCod.Value = cod;
			myCommand.Parameters.Add(parameterCod);*/

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		}
	}
	

