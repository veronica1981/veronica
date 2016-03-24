using System;
using System.Configuration;
using System.Drawing;
using System.Net.Mail;
using System.Threading;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using FCCL_BL.Managers;

public partial class DetailsFerme : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
			FCCL_DAL.Entities.Ferme_CCL farm = null;
			if (Request.Params["ID"] != null)
			{
				var farmManager = new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
				farm = farmManager.GetFarmById(Convert.ToInt32(Request.Params["ID"]));
			}

            var fabricaManager =
               new FactoryManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
            var fabrici = fabricaManager.GetFactories();
            Asociatia.DataSource = fabrici;
			if (farm != null && farm.FabricaId != null)
			{
				Asociatia.SelectedValue = farm.FabricaId.ToString();
			}
            Asociatia.DataBind();

            var countyManager =
                new CountyManager(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
            var counties = countyManager.GetAllCounties();
            Judet.DataSource = counties;
			if (farm != null && farm.JudetId != null)
			{
				Judet.SelectedValue = farm.JudetId.ToString();
			}
            Judet.DataBind();

            InsertF.Visible = true;

			if (farm != null)
			{
				//populate farm table
				IdFerma.Text = farm.Id.ToString();
				FermaCod.Text = farm.Cod;
				FermaName.Text = farm.Nume;
				FarmEmail.Text = farm.Email;
				Localitate.Text = farm.Oras;
				Strada.Text = farm.Strada;
				Numar.Text = farm.Numar;
				CodPostal.Text = farm.CodPostal;
				Telefon.Text = farm.Telefon;
				Fax.Text = farm.Fax;
				PersContact.Text = farm.PersoanaDeContact;
				TelPersContact.Text = farm.TelPersoanaContact;
				SendSms.Checked = farm.SendSms;

				//populate user table
				var userManager =
					new UserManager(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
				var userInfo = userManager.GetUserInformationByCod(farm.Cod);
				if (userInfo != null)
				{
					UserId.Value = userInfo.UserId.ToString();
					Nume.Text = userInfo.LastName;
					Prenume.Text = userInfo.FirstName;
					UserNameLbl.Text = userInfo.UserName;
					Email.Text = userInfo.Email;
					Update.Visible = true;
					ResetareParola.Visible = true;
					UserNameLbl.Visible = true;
					Parola.Visible = false;
				}
				else
				{
					Insert.Visible = true;
					UserName.Visible = true;
				}
				UserCodLbl.Text = farm.Cod;
				UpdateF.Visible = true;
				DeleteF.Visible = true;
			}
        }
    }

    #region User
    protected void UpdateUser(object sender, EventArgs e)
    {
        var userManager = new UserManager(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

        var oldUserInfo = userManager.GetUserInformationById(new Guid(UserId.Value));
        oldUserInfo.FirstName = Prenume.Text;
        oldUserInfo.LastName = Nume.Text;
        oldUserInfo.UserCod = UserCodLbl.Text;
        userManager.UpdateUser(oldUserInfo);

        var membership = Membership.Providers["PortalProvider"].GetUser(oldUserInfo.UserId, false);
        if (membership != null)
        {
            membership.Email = Email.Text;
            Membership.Providers["PortalProvider"].UpdateUser(membership);
        }

        Response.Redirect(string.Format("~/DetailsFerme.aspx?ID={0}", IdFerma.Text));
    }

    protected void InsertUser(object sender, EventArgs e)
    {
        try
        {
            MembershipCreateStatus status;
            var membership = Membership.Providers["PortalProvider"].CreateUser(UserName.Text, Parola.Text, Email.Text, null, null, true, null, out status);
            var userManager = new UserManager(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            userManager.AddUserInfo(membership.ProviderUserKey.ToString(), Nume.Text, Prenume.Text, UserCodLbl.Text);

            //send mail
            var mail = new MailMessage { From = new MailAddress("office@control-lapte.ro"), Subject = "Creare user FCCL" };
            mail.To.Add(new MailAddress(Email.Text));
            mail.Body =
                "Buna ziua, \n V-a fost creat contul cu userul: " + UserName.Text + " si parola: " + Parola.Text + " pe http://fccl.ro . Puteti intra sa vizualizati rezultatele. \n\n Cu respect, \n Fccl admin";
            mail.Bcc.Add(new MailAddress("office@control-lapte.ro"));

            ThreadPool.QueueUserWorkItem(StaticDataHelper.SendMailAsync, mail);

            Response.Redirect(string.Format("~/DetailsFerme.aspx?ID={0}", IdFerma.Text), false);
        }
        catch (Exception)
        {
            UserValidation.Text = "User name existent";
            UserValidation.Visible = true;
            UserValidation.ForeColor = Color.Red;
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string ResetareParolaUser(string username, string usercod)
    {
        var user = Membership.Providers["PortalProvider"].GetUser(username, false);
        var isChanged = false;
        string generatedPassword = "";
        string message = "Nu s-a putut schimba parola pentru userul " + username;
        try
        {
            if (user != null)
            {
                generatedPassword = user.ResetPassword();
                isChanged = user.ChangePassword(generatedPassword, usercod);
            }
        }
        catch (Exception)
        {
            message = "S-a schimbat parola pentru userul " + user.UserName + ". Noua parola este " + generatedPassword;
        }
        finally
        {
            generatedPassword = isChanged ? usercod : generatedPassword;

            //send mail
            var mail = new MailMessage
            {
                From = new MailAddress("office@control-lapte.ro"),
                Subject = "Resetare parola FCCL"
            };
            mail.To.Add(new MailAddress(user.Email));
            mail.Body =
                "Buna ziua, \n V-a fost resetata parola pentru userul: " + username + ". Noua parola este: " +
                generatedPassword +
                ". Va puteti loga pe siteul http://fccl.ro pentru a vizualiza rezultatele. \n\n Cu respect, \n Fccl admin";
            mail.Bcc.Add(new MailAddress("office@control-lapte.ro"));

            ThreadPool.QueueUserWorkItem(StaticDataHelper.SendMailAsync, mail);
        }
        return message;
    }
    #endregion User

    #region Farm
    protected void InsertFarm(object sender, EventArgs e)
    {
        var farmManager =
               new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        var farm = farmManager.GetFarmByName(FermaName.Text);
        if (farm != null)
        {
            FarmValidation.Text = "Ferma " + FermaName.Text + " exista deja!";
            FarmValidation.Visible = true;
            FarmValidation.ForeColor = Color.Red;
        }
        else
        {
            farm = new FCCL_DAL.Entities.Ferme_CCL
            {
                Cod = FermaCod.Text,
                CodPostal = CodPostal.Text,
                Email = FarmEmail.Text,
                FabricaId = Convert.ToInt32(Asociatia.SelectedValue),
                Fax = Fax.Text,
                Ferme = "F",
                FermierId = 0,
                Judet = Judet.SelectedValue,
                JudetId = Convert.ToInt32(Judet.SelectedValue),
                Nume = FermaName.Text,
                Numar = Numar.Text,
                Oras = Localitate.Text,
                PersoanaDeContact = PersContact.Text,
                Telefon = Telefon.Text,
                TelPersoanaContact = TelPersContact.Text,
                Strada = Strada.Text,
                SendSms = SendSms.Checked
            };
            var result = farmManager.InsertFarm(farm);
            if (result)
                Response.Redirect("~/EditareFerme.aspx");
            else
            {
                FarmValidation.Text = "Ferma " + FermaName.Text + " nu poate fi inserata!";
                FarmValidation.Visible = true;
                FarmValidation.ForeColor = Color.Red;
            }
        }
    }

    protected void UpdateFarm(object sender, EventArgs e)
    {
        var farmManager =
               new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        var farm = new FCCL_DAL.Entities.Ferme_CCL
        {
            Id = Convert.ToInt32(IdFerma.Text),
            Cod = FermaCod.Text,
            CodPostal = CodPostal.Text,
            Email = FarmEmail.Text,
            FabricaId = Convert.ToInt32(Asociatia.SelectedValue),
            Fax = Fax.Text,
            Ferme = "F",
            FermierId = 0,
            Judet = Judet.SelectedItem.Value,
            JudetId = Convert.ToInt32(Judet.SelectedItem.Value),
            Nume = FermaName.Text,
            Numar = Numar.Text,
            Oras = Localitate.Text,
            PersoanaDeContact = PersContact.Text,
            Telefon = Telefon.Text,
            TelPersoanaContact = TelPersContact.Text,
            Strada = Strada.Text,
            SendSms = SendSms.Checked
        };
        var result = farmManager.UpdateFarm(farm);
        if (result)
            Response.Redirect("~/EditareFerme.aspx");
        else
        {
            FarmValidation.Text = "Ferma " + FermaName.Text + " nu poate fi updatata!";
            FarmValidation.Visible = true;
            FarmValidation.ForeColor = Color.Red;
        }
    }

    protected void DeleteFarm(object sender, EventArgs e)
    {
        var farmManager =
               new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        var result = farmManager.DeleteFarm(IdFerma.Text);
        if (result)
            Response.Redirect("~/EditareFerme.aspx");
        else
        {
            FarmValidation.Text = "Ferma " + FermaName.Text + " nu poate fi stearsa!";
            FarmValidation.Visible = true;
            FarmValidation.ForeColor = Color.Red;
        }
    }
    #endregion Farm
}
