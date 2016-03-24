using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using NLog;

/// <summary>
/// Summary description for MUser
/// </summary>
public class MUser
{
    public static Logger logger = LogManager.GetCurrentClassLogger();

    public string Email { get; set; }

    public string UserName { get; set; }

    public string LastLogin { get; set; }

    public string IsApproved { get; set; }

    public string Role { get; set; }

    public string UserCod { get; set; }

    public string Password { get; set; }

    public string PasswordQuestion { get; set; }

    public string PasswordAnswer { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Salutation { get; set; }

    public string UserKey { get; set; }
    
    public bool IsAsoc { get; set; }
    
    public bool IsLocked { get; set; }

    public int AsocId { get; set; }

    public List<MUser> GetAllUsers()
    {
        int totalUsers = 0;
        MembershipUserCollection allUsers = Membership.Providers["PortalProvider"].GetAllUsers(0, 1000, out totalUsers);
        List<MUser> users = new List<MUser>();

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        
        foreach (MembershipUser User in allUsers)
        {
            MUser user = new MUser();
            user.Email = User.Email;
            user.UserName = User.UserName;
            user.LastLogin = User.LastLoginDate.ToString("G");
            user.IsApproved = User.IsApproved.ToString();
            //  string[] roles = Roles.GetRolesForUser(User.UserName);
            //user.Role = roles[0];
            //   user.Password = User.GetPassword();
            //  user.PasswordQuestion = User.PasswordQuestion;
            SqlCommand cmd = new SqlCommand("select * from UsersInformation where UserId='" + User.ProviderUserKey + "'", connection);
            connection.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    user.FirstName = rdr["FirstName"].ToString();
                    user.LastName = rdr["LastName"].ToString();
                    user.UserCod = rdr["UserCod"].ToString();
                    user.IsAsoc = Convert.ToBoolean(rdr["IsAsoc"].ToString());
                    user.AsocId = Convert.ToInt32(rdr["AsocId"].ToString());
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2.Connection = conn;
                    cmd2.CommandText = "SELECT [Cod], [Nume] FROM [Ferme_CCL] Where ([Cod] <> '') ORDER BY [Nume]";
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    DataTable table = ds.Tables[0];
                    DataRow row = table.Rows[0];
                    user.UserCod = row["Cod"].ToString().Trim();
                    conn.Close();


                    user.FirstName = "";
                    user.LastName = "";
                }

                rdr.Close();
                connection.Close();
            }

            users.Add(user);
        }


        return users;
    }


    public List<MUser> GetUser(string username)
    {
        MembershipUser User = Membership.GetUser(username);
        List<MUser> users = new List<MUser>();

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["rolesConnectionString"].ConnectionString);
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);


        //foreach (MembershipUser User in allUsers)
        // {
        MUser user = new MUser();
        user.Email = User.Email;
        user.UserName = User.UserName;
        user.LastLogin = User.LastLoginDate.ToString("G");
        user.IsApproved = User.IsApproved.ToString();
        string[] roles = Roles.GetRolesForUser(User.UserName);
        user.Role = roles[0];
//            user.Password = User.GetPassword();
        user.PasswordQuestion = User.PasswordQuestion;
        SqlCommand cmd = new SqlCommand("select * from UsersInformation where UserId='" + User.ProviderUserKey + "'", connection);
        connection.Open();
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
            if (rdr.Read())
            {
                user.Salutation = rdr["Salutation"].ToString();
                user.FirstName = rdr["FirstName"].ToString();
                user.LastName = rdr["LastName"].ToString();
                user.UserCod = rdr["UserCod"].ToString();
            }
            else
            {
                SqlCommand cmd2 = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "SELECT [Cod], [Nume] FROM [Ferme_CCL] Where ([Cod] <> '') ORDER BY [Nume]"
                };
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable table = ds.Tables[0];
                DataRow row = table.Rows[0];
                user.UserCod = row["Cod"].ToString().Trim();
                conn.Close();

                user.Salutation = "";
                user.FirstName = "";
                user.LastName = "";
            }

            rdr.Close();
            connection.Close();
        }


//        }

        users.Add(user);
        return users;
    }

    public void UpdateUser(MUser user)
    {
        MembershipUser User = Membership.GetUser(user.UserName);
        if (User != null)
        {
            User.Email = user.Email;

            Membership.UpdateUser(User);

            // update roles
            if (!Roles.IsUserInRole(User.UserName, "portal"))
                Roles.AddUserToRole(User.UserName, "portal");
            // update extra info
            object UserGUID = User.ProviderUserKey;
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["rolesConnectionString"].ConnectionString);
            connection.Open();
            try
            {
                string salutation = user.Salutation;
                string firstname = user.FirstName;
                string lastname = user.LastName;
                string usercod = user.UserCod;
                SqlCommand cmd =
                    new SqlCommand(
                        "update UsersInformation set Salutation='" + salutation + "',FirstName='" + firstname +
                        "',LastName='" + lastname + "',UserCod='" + usercod + "' where UserId='" + User.ProviderUserKey +
                        "'", connection);
                int res = cmd.ExecuteNonQuery();
                if (res <= 0)
                {
                    cmd =
                        new SqlCommand(
                            "insert into UsersInformation (UserId, Salutation, FirstName,LastName,UserCod) VALUES('"
                            + User.ProviderUserKey + "','" + salutation + "','" + firstname + "','" + lastname + "','" +
                            usercod + "')", connection);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("UpdateUser|ERROR:{0}", ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }
    }


    public void DeleteUser(MUser user)
    {
        MembershipUser User = Membership.GetUser(user.UserName);
        if (User != null)
        {
            SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["rolesConnectionString"].ConnectionString);
            try
            {
                connection.Open();
                object UserGUID = User.ProviderUserKey;
                Membership.DeleteUser(User.UserName);
                SqlCommand cmd =
                    new SqlCommand("delete from UsersInformation where UserId='" + User.ProviderUserKey + "'",
                        connection);
                int res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DeleteUser|ERROR:{0}", ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }
    }
}