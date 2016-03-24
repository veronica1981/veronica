<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsFerme" Title="Detalii Ferma" EnableTheming="true" CodeBehind="DetailsFerme.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:ScriptManager runat="server" EnablePageMethods="true" EnablePartialRendering="true"/>
    <asp:Panel ID="Panel1" runat="server" Height="131px">
        <div style="float: left;">
            <br />

<%--            <asp:HiddenField runat="server" ID="IdFerma" />--%>
            <table class="table">
                <tr>
                    <th style="width: 190px; padding-bottom: 3em;">
                        <asp:Label ID="Label1" runat="server" Text="Detalii Ferma" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
                    </th>
                    <asp:Label ID="Label2" runat="server" Visible="False"></asp:Label>
                </tr>
                <tr class="onFarmInsert">
                    <td>
                        <asp:Label runat="server" Text="Id"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="IdFerma" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Cod"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="FermaCod"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="FermaCod"
                            CssClass="failureNotification" ErrorMessage="Codul fermei este obligatoriu." ToolTip="Field-ul Cod este obligatoriu"
                            ValidationGroup="FarmValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Ferma"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="FermaName"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="FermaName"
                            CssClass="failureNotification" ErrorMessage="Field-ul Ferma este obligatoriu." ToolTip="Field-ul Ferma este obligatoriu."
                            ValidationGroup="FarmValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label runat="server" Text="Asociatia"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="Asociatia" DataTextField="Nume" DataValueField="Id" Width="250px"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Asociatia"
                            CssClass="failureNotification" ErrorMessage="Field-ul Asociatia este obligatoriu." ToolTip="Field-ul Asociatia este obligatoriu."
                            ValidationGroup="FarmValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Judet"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="Judet" DataTextField="DenLoc" DataValueField="Id"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Judet"
                            CssClass="failureNotification" ErrorMessage="Field-ul Judet este obligatoriu." ToolTip="Field-ul Judet este obligatoriu."
                            ValidationGroup="FarmValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Email"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="FarmEmail"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Localitate"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="Localitate"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Strada"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="Strada"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Numar"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="Numar"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="CodPostal"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="CodPostal"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Telefon"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="Telefon"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Fax"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="Fax"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="PersContact"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="PersContact"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="TelPersContact"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="TelPersContact"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="SendSms"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="SendSms"></asp:CheckBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label runat="server" Visible="False" ID="FarmValidation" CssClass="failureNotification"></asp:Label>
            <br />
            <asp:ValidationSummary ID="FarmValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="FarmValidationGroup" />
            <br />
            <asp:Button ID="InsertF" runat="server" CommandName="MoveNext" Text="Insert" Visible="False"
                ValidationGroup="FarmValidationGroup" OnClick="InsertFarm" />
            <asp:Button ID="UpdateF" runat="server" CommandName="MoveNext" Text="Update" Visible="False" 
                ValidationGroup="FarmValidationGroup" OnClick="UpdateFarm" />
            <asp:Button ID="DeleteF" runat="server" CommandName="MoveNext" Text="Delete" Visible="False" OnClick="DeleteFarm" />
            &nbsp;&nbsp;<br />
            <br />

        </div>

        <div style="float: right; margin-right: 10%;">
            <br />

            <asp:HiddenField runat="server" ID="UserId" />

            <table class="table">
                <tr>
                    <th style="width: 190px; padding-bottom: 3em;">
                        <asp:Label ID="Label3" runat="server" Text="Detalii User" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="UserName"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="UserName" runat="server" Visible="False"></asp:TextBox>
                        <asp:Label ID="UserNameLbl" runat="server" Visible="False"></asp:Label>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            CssClass="failureNotification" ErrorMessage="User name-ul este obligatoriu." ToolTip="Field-ul User name este obligatoriu"
                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Cod exploatare"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="UserCodLbl" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Nume"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Nume" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Prenume"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Prenume" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="E-mail"></asp:Label>

                    </td>
                    <td>
                        <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Email"
                            CssClass="failureNotification" ErrorMessage="Adresa de Email este obligatorie." ToolTip="Field-ul User name este obligatoriu"
                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="validateEmail"
                            runat="server" ErrorMessage="Email invalid."
                            ControlToValidate="Email"
                            ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />
                    </td>
                </tr>
                <tr class="onUpdate">
                    <td>
                        <asp:Label runat="server" Text="Parola"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="Parola" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Parola"
                            CssClass="failureNotification" ErrorMessage="Parola este obligatorie." ToolTip="Field-ul User name este obligatoriu"
                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="onUpdate">
                    <td>
                        <asp:Label runat="server" Text="Confirmare Parola"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification" Display="Dynamic"
                            ErrorMessage="Confirmarea parolei este obligatorie" ID="ConfirmPasswordRequired" runat="server"
                            ToolTip="Confirm Password is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Parola" ControlToValidate="ConfirmPassword"
                            CssClass="failureNotification" Display="Dynamic" ErrorMessage="Confirmarea parolei nu coincide cu parola"
                            ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                    </td>
                </tr>
            </table>

            <br />
            <asp:Label runat="server" Visible="False" ID="UserValidation" CssClass="failureNotification"></asp:Label>
            <br />
            <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="RegisterUserValidationGroup" />
            <br />
            <asp:Button ID="Update" runat="server" Text="Update" Visible="False" OnClick="UpdateUser"></asp:Button>
            <asp:Button ID="ResetareParola" runat="server" Text="Resetare Parola" Visible="False" OnClientClick="ResetareParolaUser()"></asp:Button>
            <asp:Button ID="Insert" runat="server" CommandName="MoveNext" Text="Creare cont" Visible="False"
                ValidationGroup="RegisterUserValidationGroup" OnClick="InsertUser" />

        </div>
    </asp:Panel>

    <script type="text/javascript">
        $(function () {
            if ($('#<%=UserId.ClientID%>').val() != "") {
                $(".onUpdate").css("display", "none");
            }
            if ($('#<%=IdFerma.ClientID%>').text() == "") {
                $(".onFarmInsert").css("display", "none");
            }
        });

       function ResetareParolaUser() {
           var username = $('#<%=UserNameLbl.ClientID%>').text();
           var usercod = $('#<%=UserCodLbl.ClientID%>').text();
           $.ajax({
               async: true,
               type: "POST",
               url: "DetailsFerme.aspx/ResetareParolaUser",
               data: "{ username:'" + username + "',usercod:'" + usercod + "' }",
               contentType: 'application/json; charset=utf-8',
               dataType: 'json',
               success: function (data) {
                   alert(data.d);
               },
               error: function(xhr, status, err) {
                   alert(status + ' ' + err + ' ' + xhr);
               }
           });
       }
    </script>
</asp:Content>

