using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;

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
        strLastName = lastname;
        strFirstName = firstname;
        strUserCod = usercod;
                
    }
		//
		// TODO: Add constructor logic here
		//

        string strFirstName;
        string strLastName;
        string strUserCod;
        string strFerma;
        bool bIsAsoc;
        int iAsocId;
        string strAsoc;

        public string FirstName
        {
            get
            {
                return strFirstName;
            }
            set
            {
                strFirstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return strLastName;
            }
            set
            {
                strLastName = value;
            }
        }

        public string UserCod
        {
            get
            {
                return strUserCod;
            }
            set  
            {
                strUserCod = value;
            }
        }


        public string NumeFerma
        {
            get
            {
                return strFerma;
            }
            set
            {
                strFerma = value;
            }
        }


        public string NumeAsoc
        {
            get
            {
                return strAsoc;
            }
            set
            {
                strAsoc = value;
            }
        }

        public bool IsAsoc
        {
            get
            {
                return bIsAsoc;
            }
            set
            {
                bIsAsoc = value;
            }
        }

        public int AsocId
        {
            get
            {
                return iAsocId;
            }
            set
            {
                iAsocId = value;
            }
        }
       public static UserInfos getUserInfos(string username) {
           UserInfos userinfos = new UserInfos();
           MembershipUser User = Membership.GetUser(username);
           if (User != null)
           {

               SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
               SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
               connection.Open();
               SqlCommand cmd = new SqlCommand("select * from UsersInformation where UserId='" + User.ProviderUserKey.ToString() + "'", connection);
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
                    cmdf.CommandText = "select Ferme_CCL.Nume as NumeFerma,Fabrici.Nume as NumeAsoc,Fabrici.ID as AsocId from Ferme_CCL,Fabrici where Ferme_CCL.FabricaID = Fabrici.ID AND ferme_CCL.Cod='" + userinfos.UserCod +"'";
                   using (SqlDataReader rdr = cmdf.ExecuteReader())
                   {
                       if (rdr.Read())
                       {

                           userinfos.NumeFerma = rdr["NumeFerma"].ToString();
                           userinfos.NumeAsoc = rdr["NumeAsoc"].ToString();
                           userinfos.AsocId = Convert.ToInt32(rdr["AsocId"].ToString());
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