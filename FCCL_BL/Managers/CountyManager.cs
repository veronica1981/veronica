using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FCCL_DAL.Entities;
using NLog;

namespace FCCL_BL.Managers
{
    public class CountyManager
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private string connectionString;

        public CountyManager(string connectionString)
		{
            this.connectionString = connectionString;
		}

        public List<Judet> GetAllCounties()
        {
            var counties = new List<Judet>();
            var cnn = new SqlConnection(connectionString);
            var cmd = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmd.Connection = cnn;
                cmd.CommandText = "select ID, Denloc from Judete order by Denloc";
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    counties.Add(new Judet {Id = Convert.ToInt32(reader[0]), DenLoc = reader[1].ToString()});
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAllCounties|error:{0}", ex.Message));
            }
            finally
            {
                if(reader != null)
                    reader.Close();
                cnn.Close();
            }
            return counties;
        }
    }
}
