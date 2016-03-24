using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

/// <summary>
/// Summary description for UserUtils
/// </summary>
public class UserInfos
{
    public UserInfos()
    {
    }

    public UserInfos(string firstname, string lastname, string usercod)
    {
        LastName = lastname;
        FirstName = firstname;
        UserCod = usercod;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string UserCod { get; set; }
    
    public string NumeFerma { get; set; }

    public string NumeAsoc { get; set; }

    public bool IsAsoc { get; set; }

    public int AsocId { get; set; }

    public static UserInfos getUserInfos(string username)
    {
        UserInfos userinfos = new UserInfos();
        MembershipUser User = Membership.Providers["PortalProvider"].GetUser(username, false);
//           MembershipUser User = Membership.GetUser(username);
        if (User != null)
        {
            SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(
                "select * from UsersInformation where UserId='" + User.ProviderUserKey + "'", connection);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    userinfos.FirstName = rdr["FirstName"].ToString();
                    userinfos.LastName = rdr["LastName"].ToString();
                    userinfos.UserCod = rdr["UserCod"].ToString();
                    userinfos.IsAsoc = Convert.ToBoolean(rdr["IsAsoc"].ToString());
                    userinfos.AsocId = Convert.ToInt32(rdr["AsocId"].ToString());
                }

                rdr.Close();
                connection.Close();
            }
            if (userinfos.UserCod != null && userinfos.UserCod.Trim().Length > 0)
            {
                conn.Open();
                SqlCommand cmdf = new SqlCommand();
                cmdf.Connection = conn;
//                   cmdf.CommandText = "select Nume as NumeFerma from Ferme_CCL where Cod='" + userinfos.UserCod + "'";
                cmdf.CommandText =
                    "select Ferme_CCL.Nume as NumeFerma,Fabrici.Nume as NumeAsoc from Ferme_CCL,Fabrici where Ferme_CCL.FabricaID = Fabrici.ID AND ferme_CCL.Cod='" +
                    userinfos.UserCod + "'";
                using (SqlDataReader rdr = cmdf.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        userinfos.NumeFerma = rdr["NumeFerma"].ToString();
                        userinfos.NumeAsoc = rdr["NumeAsoc"].ToString();
                    }

                    rdr.Close();
                    conn.Close();
                }
            }
        }
        return userinfos;
    }


    public static string GetRandomPasswordUsingGUID(int length)
    {
        // Get the GUID
        string guidResult = Guid.NewGuid().ToString();

        // Remove the hyphens
        guidResult = guidResult.Replace("-", string.Empty);

        // Make sure length is valid
        if (length <= 0 || length > guidResult.Length)
            throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

        // Return the first length bytes
        return guidResult.Substring(0, length);
    }
}