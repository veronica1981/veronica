using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class Account_Register : Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
    }

    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {

        TextBox UserNameTextBox =
           (TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserName");

        SqlDataSource DataSource =
            (SqlDataSource)RegisterUserWizardStep.ContentTemplateContainer.FindControl("InsertUserInfo");

        MembershipUser User = Membership.GetUser(UserNameTextBox.Text);


        object UserGUID = User.ProviderUserKey;

        DataSource.InsertParameters.Add("UserId", UserGUID.ToString());

        DataSource.Insert();

        DropDownList UserCodDropDownList = (DropDownList)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserCod");
        

        FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

        string continueUrl = RegisterUser.ContinueDestinationPageUrl;
        if (String.IsNullOrEmpty(continueUrl))
        {
            continueUrl = "~/";
        }
        Response.Redirect(continueUrl);
    }

    protected void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
    {
        DropDownList UserCodDropDownList = (DropDownList)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserCod");
        string usercod = UserCodDropDownList.SelectedValue;
        
        //check cod exists
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        cnn.Open();
        SqlCommand cmd = new SqlCommand("select * from UsersInformation where UserCod='" + usercod + "'", cnn);
        cmd.Connection = cnn;
        using (SqlDataReader rdr = cmd.ExecuteReader())
        {
            if (rdr.Read())
            {
                ContentPlaceHolder holder = (ContentPlaceHolder)Master.FindControl("MainContent");
                Literal errmess = (Literal)holder.FindControl("ErrorMessage");
                errmess.Text = "Exista deja un user inregistrat pentru codul de exploatatie: " + usercod;
                e.Cancel = true; 
            }

            rdr.Close();
            cnn.Close();
        }
       

    }
}
