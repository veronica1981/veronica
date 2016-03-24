<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SmsTest" Title="Trimitere Sms" Codebehind="SmsTest.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" width="600">
					
					
					<tr>
					<td style="width: 425px">&nbsp;</td>
					<td>
                        &nbsp;</td>
					</tr>
					<tr>
						<td valign="top" style="width: 425px">&nbsp;</td>
						<td height="15" style="width: 907px"> &nbsp;</td>
					</tr>
					<tr>
						<td valign="top" style="width: 425px"><font face="Verdana" size="2" style="font-weight: bold; font-size: 12pt; color: #33cc99; font-family: Arial">La</font></td>
						<td height="15" valign="top" style="width: 907px"> 
						<asp:TextBox runat="server" Height="22px" Width="212px" ID="txtTo"></asp:TextBox>
                            &nbsp;
						&nbsp;<asp:RequiredFieldValidator ID = "req4" ControlToValidate = "txtTo" 
                                Runat = "server" ErrorMessage = "Please enter recipients mobile" ></asp:RequiredFieldValidator></td>
					</tr>
			
					<tr>
						<td valign="top" style="width: 425px"><font face="Verdana" size="2" style="font-weight: bold; font-size: 12pt; color: #33cc99; font-family: Arial">Mesaj:</font></td>
						<td height="50" style="width: 907px"> <asp:TextBox runat="server" Height="107px" TextMode="MultiLine" Width="206px" ID="txtComments"></asp:TextBox>
						
						<asp:RequiredFieldValidator ID = "req2" ControlToValidate = "txtComments" Runat = "server" ErrorMessage = "Please enter your comments"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
					<td style="font-weight: bold; font-size: 12pt; color: #33cc99; font-family: Arial; width: 425px;">
                        &nbsp;</td>
					<td style="width: 907px">
					    &nbsp;</td>
					</tr>
					<tr>
						<td colspan="2" valign="top" height="30">
						<p align="center">


			<asp:Button Runat = server ID = btnSubmit OnClick = btnSubmit_Click Text = "Trimite" BackColor="MediumSeaGreen" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"></asp:Button>
						&nbsp;&nbsp;</td>
					</tr>
					<tr>
						<td colspan="2" valign="top" height="30">
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label></td>
						</tr>
					
					</table>
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
        SelectCommand="SELECT [Nume], [Email] FROM [Fabrici] ORDER BY [Nume]"></asp:SqlDataSource>
    <br />
    <br />
</asp:Content>

