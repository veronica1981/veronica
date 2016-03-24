using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;
using NLog;

public partial class ChangeUser : Page
{
    public static Logger logger = LogManager.GetCurrentClassLogger();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["username"] != null)
            {
                string username = Request.Params["username"];
                this.username.Text = username;
                LoadUserInfo(username);
            }
        }
    }

    private void LoadUserInfo(string username)
    {
        MembershipUser User = Membership.GetUser(username);
        if (User != null)
        {

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["rolesConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("select * from UsersInformation where UserId='" + User.ProviderUserKey.ToString() + "'", connection);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    Salutation.Text = rdr["Salutation"].ToString();
                    FirstName.Text = rdr["FirstName"].ToString();
                    LastName.Text = rdr["LastName"].ToString();
                    string cod = rdr["UserCod"].ToString();
                    UserCod.SelectedIndex = UserCod.Items.IndexOf(UserCod.Items.FindByValue(cod));
                }
            
                rdr.Close();
                connection.Close();
            }

            Email.Text = User.Email;
        }
    }

    protected void linkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditareUseri.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        MembershipUser User = Membership.GetUser(username.Text);
        if (User != null)
        {
            User.Email = Email.Text;
          
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
                string salutation = Salutation.Text;
                string firstname = FirstName.Text;
                string lastname = LastName.Text;
                string usercod = UserCod.SelectedValue;
                SqlCommand cmd = new SqlCommand("update UsersInformation set Salutation='" + salutation + "',FirstName='" + firstname + "',LastName='" + lastname + "',UserCod='" + usercod + "' where UserId='" + User.ProviderUserKey.ToString() + "'", connection);
                int res = cmd.ExecuteNonQuery();
                if (res <= 0)
                {
                    cmd = new SqlCommand("insert into UsersInformation (UserId, Salutation, FirstName,LastName,UserCod) VALUES('"
                        + UserGUID + "','" +salutation + "','"+ firstname + "','" + lastname + "','" + usercod  + "')", connection);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("btnUpdate_Click|ERROR:{0}", ex.Message));
            }
            finally
            {
                connection.Close();
            }
        }
        Response.Redirect("EditareUseri.aspx");
    }


    protected void btnDelele_Click(object sender, EventArgs e)
    {
         MembershipUser User = Membership.GetUser(username.Text);
         if (User != null)
         {
             SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["rolesConnectionString"].ConnectionString);
             try
             {
                 connection.Open();
                 object UserGUID = User.ProviderUserKey;
                 Membership.DeleteUser(username.Text);
                 SqlCommand cmd = new SqlCommand("delete from UsersInformation where UserId='" + User.ProviderUserKey + "'", connection);
                 int res = cmd.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 logger.Error(string.Format("btnDelele_Click|ERROR:{0}", ex.Message));
             }
             finally
             {
                 connection.Close();
             }

         }
        Response.Redirect("EditareUseri.aspx");
    }
}
