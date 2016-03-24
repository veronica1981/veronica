using System;
using System.Collections.Generic;
using System.Data;
using NLog;
using FCCL_DAL.Entities;
using System.Data.SqlClient;

namespace FCCL_BL.Managers
{
    public class SmsArchiveManager
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private string connectionString;

        public SmsArchiveManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Sms> GetAllUnsendSms()
        {
            var sms = new List<Sms>();
            var cnn = new SqlConnection(connectionString);
            var cmd = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmd.Connection = cnn;
                cmd.CommandText = "SELECT Id, DateCreated, Message, CellNr, TryNr FROM SmsArchive WHERE DateSend IS NULL";
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sms.Add(new Sms
                    {
                        Id = new Guid(reader[0].ToString()),
                        DateCreated = Convert.ToDateTime(reader[1].ToString()),
                        Message = reader[2].ToString(),
                        CellNr = reader[3].ToString(),
                        TryNr = Convert.ToInt32(reader[4].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAllSms|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cnn.Close();
            }
            return sms;
        }

        public Sms Dequeue()
        {
            using (var conn = new SqlConnection(connectionString))
            using (var command = new SqlCommand("ProcedureName", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            return null;


        }

        public bool UpdateSms(Sms sms)
        {
            var cn = new SqlConnection(connectionString);
            int affectedRows = 0;
            try
            {
                var cmd =
                    new SqlCommand(
                        "UPDATE SmsArchive SET Message=@Message, CellNr=@CellNr, DateCreated=@DateCreated, DateSend=@DateSend, TryNr=@TryNr WHERE Id=@Id");
                cmd.Parameters.AddWithValue("@Id", sms.Id);
                cmd.Parameters.AddWithValue("@Message", sms.Message);
                cmd.Parameters.AddWithValue("@CellNr", sms.CellNr);
                cmd.Parameters.AddWithValue("@DateCreated", sms.DateCreated);
                if (sms.DateSend == DateTime.MinValue)
                    cmd.Parameters.AddWithValue("@DateSend", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@DateSend", sms.DateSend);
                cmd.Parameters.AddWithValue("@TryNr", sms.TryNr);
                cmd.Connection = cn;
                cn.Open();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("UpdateSms|sms id:{0} ERROR:{1}", sms.Id, ex.Message));
            }
            finally
            {
                cn.Close();
            }
            return affectedRows > 0;
        }

        public Sms GetSmsById(string id)
        {
            Sms sms = null;
            var cnn = new SqlConnection(connectionString);
            var cmd = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmd.Connection = cnn;
                cmd.CommandText = "SELECT Id, DateCreated, Message, CellNr, TryNr FROM SmsArchive WHERE Id=@Id";
                cmd.Parameters.AddWithValue("Id", id);
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sms = new Sms
                    {
                        Id = new Guid(reader[0].ToString()),
                        DateCreated = Convert.ToDateTime(reader[1].ToString()),
                        Message = reader[2].ToString(),
                        CellNr = reader[3].ToString(),
                        TryNr = Convert.ToInt32(reader[4].ToString())
                    };
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetSmsById|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cnn.Close();
            }
            return sms;
        }
    }
}
