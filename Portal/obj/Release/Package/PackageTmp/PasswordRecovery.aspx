<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Account_PasswordRecovery" Codebehind="PasswordRecovery.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server"  GeneralFailureText="User inexistent" UserNameInstructionText="ID cont inexistent"
    onsendingmail="PasswordRecovery1_SendingMail" 
    onverifyinguser="PasswordRecovery1_VerifyingUser">
    <MailDefinition From="office@control-lapte.ro" Subject="Resetare parola" Priority="High">
</MailDefinition>
        <UserNameTemplate>
                    <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table cellpadding="0">
                            <tr>
                                <td align="center" colspan="2">
                                  <div>  V-ati uitat parola?</div></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                  <div>  Introduceti ID-ul contului pentru a primi o noua parola.<div></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">ID cont:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="ID-ul contului este obligatoriu." 
                                        ToolTip="ID-ul contului este obligatoriu." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Trimite" 
                                        ValidationGroup="PasswordRecovery1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </UserNameTemplate>
      
        <SuccessTemplate>
                Parola a fost trimisa la adresa 
                <asp:Label ID="EmailLabel" runat="server"></asp:Label>
                </SuccessTemplate>
               
          </asp:PasswordRecovery>
</asp:Content>

