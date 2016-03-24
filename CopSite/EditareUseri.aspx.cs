using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using FCCL_BL.Managers;
using FCCL_DAL;
using NLog;

public partial class EditareUseri : Page
{
    public static Logger logger = LogManager.GetCurrentClassLogger();
    private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            FillGrid();
    }

    private void FillGrid()
    {
        var userManager = new UserManager(ctx);
        var allUsers = userManager.GetUsers();

        var farmManager = new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        var farms = farmManager.GetAllFarmsForDropDown();

        foreach (var user in allUsers)
        {
            var farm = farms.FirstOrDefault(x => x.Cod == user.UserCod);
            if (farm != null)
                user.UserCod = farm.Cod + " - " + farm.Nume;
        }

        lcount.Text = allUsers.Count + " useri";
        UserGrid.DataSource = allUsers;
        UserGrid.DataBind();

    }
    protected void linkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
    protected void linkUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);


        for (int i = 0; i < UserGrid.Items.Count; i++)
        {
            RepeaterItem item = UserGrid.Items[i];
            Label lUsername = (Label)item.FindControl("UserName");
            string username = lUsername.Text;
            DropDownList dUsercod = (DropDownList)item.FindControl("UserCod");
            string usercod = dUsercod.SelectedValue;
            TextBox tEmail = (TextBox)item.FindControl("Email");
            string email = tEmail.Text;
            TextBox tNume = (TextBox)item.FindControl("Nume");
            string lastname = tNume.Text;
            TextBox tPrenume = (TextBox)item.FindControl("Prenume");
            string firstname = tPrenume.Text;
            CheckBox cIsAsoc = (CheckBox)item.FindControl("IsAsoc");
            bool isasoc = cIsAsoc.Checked;
            CheckBox cIsLocked = (CheckBox)item.FindControl("IsLocked");
            bool islocked = cIsLocked.Checked;
            int asocid = 0;
            if (isasoc)
            {
                SqlCommand cmda = new SqlCommand("select * from Ferme_CCL where Cod='" + usercod + "'", conn);
                conn.Open();
                using (SqlDataReader rdr = cmda.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        asocid = Convert.ToInt32(rdr["FabricaID"].ToString());
                    }
                    rdr.Close();
                    conn.Close();
                }
            }
            MembershipUser User = Membership.Providers["PortalProvider"].GetUser(username, false);
            if (User != null)
            {
                User.Email = email;
                User.IsApproved = !islocked;
                Membership.Providers["PortalProvider"].UpdateUser(User);
                string[] usernames = { username };
                string[] roles = { "asoc" };
                if (!Roles.Providers["PortalProvider"].IsUserInRole(username, "asoc") && isasoc)
                {
                    Roles.Providers["PortalProvider"].AddUsersToRoles(usernames, roles);
                }
                if (Roles.Providers["PortalProvider"].IsUserInRole(username, "asoc") && !isasoc)
                {
                    Roles.Providers["PortalProvider"].RemoveUsersFromRoles(usernames, roles);
                }
            }
            // update usersinformation
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update UsersInformation set FirstName='" + firstname + "',LastName='" + lastname + "',UserCod='" + usercod + "',IsAsoc='" + isasoc + "',AsocId=" + asocid + " where UserId='" + User.ProviderUserKey.ToString() + "'", connection);
                int res = cmd.ExecuteNonQuery();
                if (res <= 0)
                {
                    cmd = new SqlCommand("insert into UsersInformation (UserId, FirstName,LastName,UserCod,IsAsoc,AsocId) VALUES('"
                        + User.ProviderUserKey + "','" + firstname + "','" + lastname + "','" + usercod + "','" + isasoc + "'," + asocid + ")", connection);
                    res = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("linkUpdate_Click|ERROR:{0}", ex.Message));
            }
            finally
            {
                connection.Close();
            }


        }
    }

    protected void UserGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string username = Convert.ToString(e.CommandArgument);
        if (e.CommandName.StartsWith("Resetare"))
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            string usercod = GetRandomPasswordUsingGUID(10);
            MembershipUser User = Membership.Providers["PortalProvider"].GetUser(username, false);
            SqlCommand cmd = new SqlCommand("select * from UsersInformation where UserId='" + User.ProviderUserKey + "'", connection);
            connection.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    usercod = rdr["UserCod"].ToString().Trim();
                }
                rdr.Close();
            }
            connection.Close();
            //
            string generatedPassword = User.ResetPassword();
            bool isChanged = false;
            try
            {
                isChanged = User.ChangePassword(generatedPassword, usercod);
                if (isChanged)
                {
                    pwd.Text = "Noua parola pentru " + username + " este : " + usercod;


                    //send mail
                    var mail = new MailMessage { From = new MailAddress("office@control-lapte.ro"), Subject = "Resetare parola FCCL" };
                    mail.To.Add(new MailAddress(User.Email));
                    mail.Body =
                        "Buna ziua, \n V-a fost resetata parola pentru userul: " + username + ". Noua parola este: " + usercod + ". Va puteti loga pe siteul http://fccl.ro pentru a vizualiza rezultatele. \n\n Cu respect, \n Fccl admin";
                    mail.Bcc.Add(new MailAddress("office@control-lapte.ro"));

                    ThreadPool.QueueUserWorkItem(StaticDataHelper.SendMailAsync, mail);
                }
                else
                    pwd.Text = " Eroare la resetarea parolei pentru: " + username;
            }
            catch
            {
                pwd.Text = " Eroare la resetarea parolei pentru: " + username;

            }

        }
        if (e.CommandName.StartsWith("Stergere"))
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            MembershipUser User = Membership.Providers["PortalProvider"].GetUser(username, false);
            SqlCommand cmd = new SqlCommand("delete from UsersInformation where UserId='" + User.ProviderUserKey + "'", connection);
            connection.Open();
            try
            {
                Membership.Providers["PortalProvider"].DeleteUser(username, false);
                cmd.ExecuteNonQuery();
                FillGrid();
            }
            catch
            {
                pwd.Text = " Eroare la stergerea userului: " + username;

            }
            finally
            {
                connection.Close();
            }

        }
    }

    protected string GetRandomPasswordUsingGUID(int length)
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