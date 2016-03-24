<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Account_Register" Codebehind="Register.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
     <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
       </span>
    <asp:CreateUserWizard ID="RegisterUser" runat="server" EnableViewState="false" 
        OnCreatedUser="RegisterUser_CreatedUser" 
        oncreatinguser="RegisterUser_CreatingUser">
        <LayoutTemplate>
            <asp:PlaceHolder ID="wizardStepPlaceholder" runat="server"></asp:PlaceHolder>
            <asp:PlaceHolder ID="navigationPlaceholder" runat="server"></asp:PlaceHolder>
        </LayoutTemplate>
        <WizardSteps>
            <asp:CreateUserWizardStep ID="RegisterUserWizardStep" runat="server">
                <ContentTemplate>
                    <h2>
                        Creare cont
                    </h2>
                    <p>
                        Va rugam introduceti datele de mai jos
                    </p>
                    <p>
                        Parola trebuie sa contina minimum <%= Membership.MinRequiredPasswordLength %> caractere
                    </p>
               
                    <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification" 
                         ValidationGroup="RegisterUserValidationGroup"/>
                    <div class="accountInfo">
                        <fieldset class="register">
                            <legend>Informatii cont</legend>
                             <p>
                                <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">Prenume:</asp:Label>
                                <asp:TextBox ID="FirstName" runat="server" CssClass="textEntry"></asp:TextBox>
                            </p>
                             <p>
                                <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Nume:</asp:Label>
                                <asp:TextBox ID="LastName" runat="server" CssClass="textEntry"></asp:TextBox>
                            </p>
                            <p>
                               <asp:Label ID="UserCodLabel" runat="server" AssociatedControlID="UserCod">Cod exploatatie:</asp:Label>
                                <asp:DropDownList ID="UserCod" runat="server" DataSourceID="UserCodInfo" DataTextField="Nume" DataValueField="Cod" CssClass="textEntry"></asp:DropDownList>
                            </p>
                            <p>
                               <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">ID cont:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                     CssClass="failureNotification" ErrorMessage="ID-ul contului este obligatoriu." ToolTip="ID-ul contului este obligatoriu" 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                                     CssClass="failureNotification" ErrorMessage="Adresa de E-mail este obligatorie." ToolTip="Adresa de E-mail este obligatorie" 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Parola:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                     CssClass="failureNotification" ErrorMessage="Parola este obligatorie." ToolTip="Parola este obligatorie." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirmare parola:</asp:Label>
                                <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification" Display="Dynamic" 
                                     ErrorMessage="Confirmarea parolei este obligatorie" ID="ConfirmPasswordRequired" runat="server" 
                                     ToolTip="Confirm Password is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                     CssClass="failureNotification" Display="Dynamic" ErrorMessage="Confirmarea parolei nu coincide cu parola"
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                            </p>
                        </fieldset>
                        <p class="submitButton">
                            <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Creare cont" 
                                 ValidationGroup="RegisterUserValidationGroup"/>
                        </p>
                    </div>

                             <asp:SqlDataSource ID="InsertUserInfo" runat="server" ConnectionString="<%$ ConnectionStrings:ApplicationServices %>"
                        InsertCommand="INSERT INTO [UsersInformation] ([UserId], [FirstName], [LastName], [UserCod],[IsAsoc],[AsocId]) VALUES (@UserId, @FirstName, @LastName, @UserCod,0,0)"
                        ProviderName="<%$ ConnectionStrings:ApplicationServices.ProviderName %>">
                        <InsertParameters>
                            <asp:ControlParameter Name="FirstName" Type="String" ControlID="FirstName" PropertyName="Text" />
                            <asp:ControlParameter Name="LastName" Type="String" ControlID="LastName" PropertyName="Text" />
                            <asp:ControlParameter Name="UserCod" Type="String" ControlID="UserCod" PropertyName="Text" />
                            
                        </InsertParameters>
                       
          </asp:SqlDataSource>
          <asp:SqlDataSource ID="UserCodInfo" runat="server" 
        ConnectionString="<%$ ConnectionStrings:AdditionalInformation %>" 
        SelectCommand="SELECT [Cod], (Cod + ' - ' + Nume) as Nume FROM [Ferme_CCL] Where ([Cod] <> '') ORDER BY [Nume]">
     
    </asp:SqlDataSource>

                </ContentTemplate>
                <CustomNavigationTemplate>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
</asp:Content>