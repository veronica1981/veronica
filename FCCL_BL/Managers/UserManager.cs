using System;
using System.Data.SqlClient;
using FCCL_DAL;
using System.Collections.Generic;
using System.Linq;
using FCCL_DAL.Entities;
using NLog;

namespace FCCL_BL.Managers
{
    public class UserManager
    {
        private FCCLDbContext dbContext;
        private string connectionString;
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public UserManager(FCCLDbContext context)
        {
            dbContext = context;
        }

        public UserManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<sp_Get_Users_Result> GetUsers()
        {
            var results = new List<sp_Get_Users_Result>();
            try
            {
                var result = dbContext.sp_Get_Users();
                if (result != null)
                {
                    results = result.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUsers|error: {0}",ex.Message));
            }

            return results;
        }

        public sp_Get_Users_Result GetUser(string userName)
        {
            var results = new sp_Get_Users_Result();
            try
            {
                var result = dbContext.sp_Get_Users(userName);
                if (result != null)
                {
                    results = result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetUser|userName: {0} ERROR:{1}", userName, ex.Message));
            }
            return results;
        }

        public User GetUserById(Guid userId)
        {
            User user = null;
            var cn = new SqlConnection(dbContext.Database.Connection.ConnectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand("SELECT * FROM aspnet_Users WHERE UserId = @userId", cn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cn.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        ApplicationId = new Guid(reader[0].ToString()),
                        UserId = userId,
                        UserName = reader[2].ToString(),
                        MobileAlias = reader[4].ToString(),
                        IsAnonymous = Convert.ToBoolean(reader[5].ToString()),
                        LastActivityDate = Convert.ToDateTime(reader[6].ToString())
                    };
                }
            }
            catch (Exception)
            {
                logger.Error(string.Format("GetUserById| userId:{0}", userId));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cn.Close();
            }
            return user;
        }

        public UserInformation GetUserInformationById(Guid userId)
        {
            UserInformation userInformation = null;
            var cn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand("SELECT * FROM UsersInformation WHERE UserId = @userId", cn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cn.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userInformation = new UserInformation
                    {
                        UserId = userId,
                        FirstName = reader[1].ToString(),
                        LastName = reader[2].ToString(),
                        UserCod = reader[3].ToString(),
                        IsAsoc = Convert.ToBoolean(reader[4].ToString()),
                        AsocId = Convert.ToInt32(reader[5].ToString())
                    };
                }
            }
            catch (Exception)
            {
                logger.Error(string.Format("GetUserInformationById| userId:{0}", userId));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cn.Close();
            }
            return userInformation;
        }

        public void UpdateUser(UserInformation oldUserInfo)
        {
            var cn = new SqlConnection(connectionString);
            try
            {
                var cmd =
                    new SqlCommand(
                        "UPDATE UsersInformation SET FirstName = @FirstName, LastName = @LastName, UserCod = @UserCod, IsAsoc = @IsAsoc, AsocId = @AsocId WHERE UserId = @userId",
                        cn);
                cmd.Parameters.AddWithValue("@FirstName", oldUserInfo.FirstName);
                cmd.Parameters.AddWithValue("@LastName", oldUserInfo.LastName);
                cmd.Parameters.AddWithValue("@UserCod", oldUserInfo.UserCod);
                cmd.Parameters.AddWithValue("@IsAsoc", oldUserInfo.IsAsoc);
                cmd.Parameters.AddWithValue("@AsocId", oldUserInfo.AsocId);
                cmd.Parameters.AddWithValue("@userId", oldUserInfo.UserId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                logger.Error(string.Format("UpdateUser| userId:{0}", oldUserInfo.UserId));
            }
            finally
            {
                cn.Close();
            }
        }

        public UserInformation GetUserInformationByCod(string cod)
        {
            UserInformation userInformation = null;
            var cn = new SqlConnection(connectionString);
            try
            {
                var cmd =
                    new SqlCommand(
                        "SELECT ui.UserId,ui.FirstName, ui.LastName, u.UserName, m.Email FROM UsersInformation as ui INNER JOIN aspnet_Users as u ON u.UserId = ui.UserId INNER JOIN aspnet_Membership m on u.UserId = m.UserId WHERE ui.UserCod = @cod",
                        cn);
                cmd.Parameters.AddWithValue("@cod", cod);
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userInformation = new UserInformation
                    {
                        UserId = new Guid(reader[0].ToString()),
                        FirstName = reader[1].ToString(),
                        LastName = reader[2].ToString(),
                        UserCod = cod,
                        UserName = reader[3].ToString(),
                        Email = reader[4].ToString()
                    };
                }
            }
            catch (Exception)
            {
                logger.Error(string.Format("GetUserInformationByCod| cod:{0}", cod));
            }
            finally
            {
                cn.Close();
            }
            return userInformation;
        }

        public void AddUserInfo(string userId, string firstName, string lastName, string userCod)
        {
            var cn = new SqlConnection(connectionString);
            try
            {
                var cmd =
                    new SqlCommand(
                        "INSERT INTO UsersInformation(UserId,FirstName,LastName,UserCod,IsAsoc,AsocId) VALUES (@userId,@firstName,@lastName,@userCod,0,0)");
                cmd.Parameters.AddWithValue("@userId", new Guid(userId));
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@userCod", userCod);
                cmd.Connection = cn;
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                logger.Error(string.Format("AddUserInfo| userId:{0}", userId));
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
