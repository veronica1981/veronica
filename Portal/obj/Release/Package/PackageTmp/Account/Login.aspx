<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Account_Login" Codebehind="Login.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h3>
        Autentificare
    </h3>
    <p>
        Nu aveti inca un cont ?
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Creare cont</asp:HyperLink> 
    </p>
   <p>
                    <asp:HyperLink ID="HyperLink1" runat="server" EnableViewState="False" 
                        NavigateUrl="~/PasswordRecovery.aspx">V-ati uitat parola | Ajutor</asp:HyperLink>
                </p>
    
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false" FailureText="ID cont sau parola invalide">
        <LayoutTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Informatii cont</legend>
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">ID cont:</asp:Label>
                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="failureNotification" ErrorMessage="ID-ul contului este obligatoriu." ToolTip="Va rugam introduceti Id-ul contului" 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Parola:</asp:Label>
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="failureNotification" ErrorMessage="Parola este obligatorie." ToolTip="Va rugam introduceti parola" 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server"/>
                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Pastreaza-ma autentificat</asp:Label>
                    </p>
                </fieldset>
                <p class="submitButton">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" 
                        Text="Autentificare" ValidationGroup="LoginUserValidationGroup"/>
                </p>
                
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>