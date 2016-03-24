<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="System.Net.Mail" %>
<%@ Import Namespace="System.IO" %>


<script runat="server">    

    void btnSubmit_Click(Object sender, EventArgs e) {

		MailMessage objEmail = new MailMessage();
		objEmail.To.Add(new MailAddress(txtTo.Text));
		objEmail.From = new MailAddress(txtFrom.Text);
		if (txtCc.Text != "")
		objEmail.CC.Add(new MailAddress(txtCc.Text));
		objEmail.Subject = txtComments.Text.Trim();
		objEmail.Body = txtName.Text + ", " +txtComments.Text;
		objEmail.Priority = MailPriority.High;
		objEmail.IsBodyHtml = true;
        string strdir = "C:\\temp\\";
        string strfilename =
            Path.GetFileName(File1.PostedFile.FileName);
		if (strfilename !=null && strfilename.Length >0)
			{
        File1.PostedFile.SaveAs(strdir + strfilename);
		objEmail.Attachments.Add(new Attachment(File1.PostedFile.InputStream, File1.FileName));

      //  objEmail.Attachments.Add(new MailAttachment(strdir + strfilename));
		}
		
	//	SmtpMail.SmtpServer = "localHost";
		try{
			SmtpClient client = new SmtpClient("server-fundatie");

			client.Send(objEmail);

 
	
			//SmtpMail.Send(objEmail);
			Response.Write("Your Email has been sent sucessfully - Thank You");
		}
		catch (Exception exc){
			Response.Write("Send failure: " + exc.ToString());
		}
		if (File.Exists(strdir + strfilename))
			File.Delete(strdir + strfilename);

    }
</script>

<html>
	<head>
	<title>E-mail</title>
	</head>
	<body>
		<form runat="server">
                <div align="left">
                <table border="0" width="350">
					<tr>
						<td valign="top"><font face="Verdana" size="2">Name:</font></td>
						<td height="24">   <asp:TextBox runat="server" Height="25px" Width="215px" ID="txtName"></asp:TextBox>
						<br>
						<asp:RequiredFieldValidator ID = "req1" ControlToValidate = "txtFrom" Runat = "server" ErrorMessage = "Please enter your name"></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td valign="top"><font face="Verdana" size="2">From</font></td>
						<td height="24"> <asp:TextBox runat="server" Height="22px" Width="213px" ID="txtFrom"></asp:TextBox>
						<br>
						<asp:RegularExpressionValidator ID = "reg1" ControlToValidate = "txtFrom" Runat = "server" ErrorMessage = "Invalid Email" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>&nbsp;<asp:RequiredFieldValidator ID = "req3" ControlToValidate = "txtFrom" Runat = "server" ErrorMessage = "Please enter your E-mail" ></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td valign="top"><font face="Verdana" size="2">To</font></td>
						<td height="24" valign="top"> 
						<asp:TextBox runat="server" Height="22px" Width="212px" ID="txtTo"></asp:TextBox>
						<br>
						<asp:RegularExpressionValidator ID = "reg2" ControlToValidate = "txtTo" Runat = "server" ErrorMessage = "Invalid Email" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
						&nbsp;<asp:RequiredFieldValidator ID = "req4" ControlToValidate = "txtTo" Runat = "server" ErrorMessage = "Please enter recipients E-mail" ></asp:RequiredFieldValidator></td>
					</tr>
					<tr>
						<td valign="top"><font face="Verdana" size="2">Cc</font></td>
						<td height="24" valign="top"> 
						<asp:TextBox runat="server" Height="22px" Width="210px" ID="txtCc"></asp:TextBox>
						<br>
						<asp:RegularExpressionValidator ID = "reg3" ControlToValidate = "txtCc" Runat = "server" ErrorMessage = "Invalid Email" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
						</td>
					</tr>
					<tr>
						<td>
					</tr>
					<tr>
						<td valign="top"><font face="Verdana" size="2">Comments:</font></td>
						<td height="112"> <asp:TextBox runat="server" Height="107px" TextMode="MultiLine" Width="206px" ID="txtComments"></asp:TextBox>
						<br>
						<asp:RequiredFieldValidator ID = "req2" ControlToValidate = "txtComments" Runat = "server" ErrorMessage = "Please enter your comments"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
					<td>
                        Attachment:</td>
					<td>
					 <asp:FileUpload ID="File1" runat="server" />

                      </td>
					</tr>
					<tr>
						<td colspan="2" valign="top" height="30">
						<p align="center">


			<asp:Button Runat = server ID = btnSubmit OnClick = btnSubmit_Click Text = "Submit"></asp:Button>
						&nbsp;<input type = "reset" runat = "server" value = "Clear"></td>
					</tr>
					
					</table>
				</div>
				
				<p>&nbsp;</p>
				<p><br>


			<!-- Insert content here -->
				</p>
		</form>
	</body>
</html>