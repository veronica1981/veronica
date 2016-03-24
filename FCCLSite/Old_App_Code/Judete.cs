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

	public class Judete
	{
	
		public SqlDataReader GetJudet(int jud) 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetJudet", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterJud = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterJud.Value = jud;
			myCommand.Parameters.Add(parameterJud);

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}
		public SqlDataReader GetAllJudete() 
		{

			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetAllJudete", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Execute the command
			myConnection.Open();
			SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
			// Return the datareader 
			return result;
		}
		public int GetJudetID (string judet)
		{
			// Create Instance of Connection and Command Object
			SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
			SqlCommand myCommand = new SqlCommand("Portal_GetJudetID", myConnection);

			// Mark the Command as a SPROC
			myCommand.CommandType = CommandType.StoredProcedure;

			// Add Parameters to SPROC
			SqlParameter parameterID = new SqlParameter("@ID", SqlDbType.Int, 4);
			parameterID.Direction = ParameterDirection.Output;
			myCommand.Parameters.Add(parameterID);

			SqlParameter parameterNumeJudet = new SqlParameter("@Nume", SqlDbType.NVarChar, 150);
			parameterNumeJudet.Value = judet;
			myCommand.Parameters.Add(parameterNumeJudet);

			myConnection.Open();
			myCommand.ExecuteNonQuery();
			myConnection.Close();
		
			return (Convert.ToInt32(parameterID.Value));
		}
	}

