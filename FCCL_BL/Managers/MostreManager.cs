using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FCCL_DAL.Entities;
using NLog;
using System;

namespace FCCL_BL.Managers
{
    public class MostreManager
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private string connectionString;

        public MostreManager(string connectionString)
		{
            this.connectionString = connectionString;
		}

        public List<Mostra> GetDateCoduriBare(string coduri)
        {
            var mostre = new List<Mostra>();
			coduri = string.Join(";", coduri.Split(';').Distinct());
            var cnn = new SqlConnection(connectionString);
            var cmd = new SqlCommand("dbo.Portal_GetDateCoduriBare", cnn); 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@coduriBare", coduri));

            SqlDataReader reader = null;
            try
            {
                cmd.Connection = cnn;
                cnn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    mostre.Add(new Mostra
                    {
                        CodBare = reader["CodBare"].ToString(),
                        DataTestare = reader["DataTestare"].ToString(),
                        Grasime = reader["Grasime"].ToString(),
                        NumarCeluleSomatice = reader["NumarCeluleSomatice"].ToString(),
                        ProcentLactoza = reader["ProcentLactoza"].ToString(),
                        ProcentProteine = reader["ProcentProteine"].ToString(),
                        SubstrantaUscata = reader["SubstantaUscata"].ToString(),
                        Uree = reader["Urea"].ToString(),
                        Ph=reader["PH"].ToString(),
                        Caseina = reader["Caseina"].ToString(),
                        IncarcaturaGermeni = reader["IncarcaturaGermeni"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetDateCoduriBare|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cnn.Close();
            }
            return mostre;
        }
    }
}
