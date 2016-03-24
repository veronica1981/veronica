using System;
using System.Net.Mail;
using System.IO;
using System.Web.UI;


public partial class EmailManual : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtFrom.Text = "office@control-lapte.ro";
        txtComments.Text = "Buletin Analize";
        txtSubject.Text = "Fundatia Control Lapte";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MailMessage objEmail = new MailMessage();
        string[] recipients = txtTo.Text.Split(new Char[] { ';' });
        foreach (string recipient in recipients)
            objEmail.To.Add(new MailAddress(recipient));

        objEmail.From = new MailAddress(txtFrom.Text);
        objEmail.Subject = txtSubject.Text.Trim();
        objEmail.Body = txtComments.Text;
        objEmail.Priority = MailPriority.High;
        objEmail.IsBodyHtml = true;
        string strfilename =
            Path.GetFileName(File1.PostedFile.FileName);
        if (strfilename != null && strfilename.Length > 0)
        {
            objEmail.Attachments.Add(new Attachment(File1.PostedFile.InputStream, File1.FileName));
        }
        try
        {
            SmtpClient client = new SmtpClient();
            client.Send(objEmail);
            Label1.Text = "Mailul a fost trimis!";
        }
        catch (Exception exc)
        {
            Label1.Text = "Trimitere esuata: " + exc;
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtTo.Text = fabrica.SelectedValue;
    }
}
