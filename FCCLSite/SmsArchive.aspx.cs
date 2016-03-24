using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCCL_BL.Helpers;
using FCCL_BL.Managers;

namespace FCCLSite
{
    public partial class SmsArchive : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        public void BindData()
        {
            var smsArchiveManager = new SmsArchiveManager(ConfigurationManager.ConnectionStrings["fccl_v2"].ConnectionString);
            var values = smsArchiveManager.GetAllUnsendSms();

            GridView1.Columns[0].Visible = true;
            GridView1.DataSource = values;
            GridView1.DataBind();
            GridView1.Columns[0].Visible = false;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void Retrimitere(object sender, EventArgs e)
        {
            var grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string id = grdrow.Cells[0].Text;

            var smsArchiveManager = new SmsArchiveManager(ConfigurationManager.ConnectionStrings["fccl_v2"].ConnectionString);
            var sms = smsArchiveManager.GetSmsById(id);
            SmsHelper.SendSms(sms);
            BindData();
        }
    }
}