using System;
using System.Web.UI;
using FCCL_BL.Managers;
using FCCL_DAL;
using System.Configuration;
using System.Web.Security;

namespace CopSite
{
    public partial class UpdateUser : Page
    {
        private FCCLDbContext ctx = StaticDataHelper.FCCLDbContext;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var userManager = new UserManager(ctx);
                var user = userManager.GetUser(Request.Params["UserName"]);

                UserId.Value = user.UserId.ToString();
                UserCod.Value = user.UserCod;
                UserName.Text = user.UserName;
                Nume.Text = user.LastName;
                Prenume.Text = user.FirstName;
                Email.Text = user.Email;
                Asociatie.Checked = user.IsAsoc == true;

                Locked.Checked = user.IsLockedOut;
                var farmManager =
                    new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
                CodUser.DataSource = farmManager.GetAllFarmsForDropDown();
                CodUser.DataTextField = "Nume";
                CodUser.DataValueField = "Cod";
                CodUser.DataBind();
            }
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            var userManager = new UserManager(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

            var oldUserInfo = userManager.GetUserInformationById(new Guid(UserId.Value));
            oldUserInfo.FirstName = Prenume.Text;
            oldUserInfo.LastName = Nume.Text;
            oldUserInfo.IsAsoc = Asociatie.Checked;
            oldUserInfo.UserCod = CodUser.SelectedValue;
            userManager.UpdateUser(oldUserInfo);
            
            var membership = Membership.Providers["PortalProvider"].GetUser(oldUserInfo.UserId,false);
            if (membership != null)
            {
                if (!Locked.Checked)
                    membership.UnlockUser();
                membership.Email = Email.Text;
                Membership.Providers["PortalProvider"].UpdateUser(membership);
            }

            Response.Redirect("~/EditareUseri.aspx");
        }
    }
}