using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FCCL_DAL.Entities;
using NLog;

namespace FCCL_BL.Managers
{
    public class FactoryManager
    {
        private string connectionString;
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public FactoryManager(string connectionString)
		{
            this.connectionString = connectionString;
		}

        public List<string> GetFactoriesForAutocomplete(string factoryName)
        {
            var factories = new List<string>();
            var cnn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "select Top 10 Nume from Fabrici where Nume like @Search order by nume";
                cmd.Parameters.AddWithValue("@Search", factoryName + "%");
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    factories.Add(reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFactoriesForAutocomplete|factoryName:{0} ERROR:{1}", factoryName, ex.Message));
            }
            finally
            {
                if(reader != null)
                    reader.Close();
                cnn.Close();
            }
            return factories;
        }

        public List<Fabrica> GetFactories(string factory = null)
        {
            var factories = new List<Fabrica>();
            var cnn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "SELECT ID, Nume FROM Fabrici";
                if (factory != null)
                {
                    cmd.CommandText += " WHERE Nume LIKE @FermaNume";
                    cmd.Parameters.AddWithValue("@FermaNume", "%" + factory + "%");
                }
                cmd.CommandText += " ORDER BY nume";
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    factories.Add(new Fabrica {Id = Convert.ToInt32(reader[0]), Nume = reader[1].ToString()});
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFactoriesForDropDown|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cnn.Close();
            }
            return factories;
        }
    }
}
