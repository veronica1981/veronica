<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Login" Title="Home" Codebehind="Login.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 90%" cellspacing=5 border=0 cellpadding=2>
        <tr>
            <td style="height: 21px">
            </td>
            <td style="height: 21px">
            </td>
          
        </tr>
        <tr>
            <td  width=30% valign=top>
             <asp:Panel ID="Panel2" runat="server" >
                <asp:Login ID="Login1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" LoginButtonText="sign-in" PasswordLabelText="Parola:" RememberMeText="Reamintire parola" TitleText="Intrare in cont" UserNameLabelText="User:" FailureText="Logare nereusita!"  DestinationPageUrl="~/AuthPage.aspx" >
                    <LoginButtonStyle BackColor="#E3EAEB" />
                </asp:Login>
                </asp:Panel>
                &nbsp;&nbsp;<br />
                <asp:Panel ID="Panel1" runat="server" Height="50px" Width="200px">
                    <asp:LoginName ID="LoginName1" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="Medium" ForeColor="MediumSeaGreen" FormatString="Welcome {0}!" Width="178px" />
                    &nbsp;<br />
                    &nbsp;<br />
                    <asp:LoginStatus ID="LoginStatus1" runat="server" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="Black" />
                </asp:Panel>
                </td>
            <td align=center>
                <span lang="EN-GB" style="font-size: 14pt; color: MediumSeaGreen; font-family: Arial">
                    Obiective si activitati</span><p>
                    </p>
                <p align="justify">
                    <font face="Tahoma">&nbsp;</font><font face="Arial" style="font-size: 11pt"> &nbsp;
                        &nbsp; &nbsp;</font><font color="MediumSeaGreen" face="Arial" size="2"><b></b> Furnizarea de
                            informatii obiective si impartiale cu privire la continutul si calitatea laptelui.
                            Testarea probelor de lapte cu privire la continut si calitate Fundatia isi desfasoara
                            activitatea la pret de cost si este NON PROFIT. Determina: <b></b></font>
                    <br />
                    </p>
                    <table style="width: 400px">
                        <tr>
                            <td style="width: 18px">
                            </td>
                            <td style="width: 187px" align=left>
                                <strong><span style="font-size: 10pt; color: MediumSeaGreen; font-family: Arial">Incarcaturi
                                    de germeni </span></strong>
                            </td>
                            <td>
                                <strong><span style="font-size: 10pt; color: MediumSeaGreen; font-family: Arial">Procentul
                                    de proteina </span></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                            </td>
                            <td style="width: 187px" align=left>
                                <span style="font-size: 10pt"><span style="color: MediumSeaGreen"><span style="font-family: Arial">
                                    <strong>Numarul de celule somatice</strong> </span></span></span>
                            </td>
                            <td>
                                <strong><span style="font-size: 10pt; color: MediumSeaGreen; font-family: Arial">Antibioticele</span></strong></td>
                        </tr>
                        <tr>
                            <td style="width: 18px">
                            </td>
                            <td style="width: 187px" align=left>
                                <span style="font-size: 10pt"><span style="color: MediumSeaGreen"><span style="font-family: Arial">
                                    <strong>Procentul de grasime</strong> </span></span></span>
                            </td>
                            <td>
                                <strong><span style="font-size: 10pt; color: MediumSeaGreen; font-family: Arial">Apa adaugata
                                </span></strong>
                            </td>
                        </tr>
                    </table>
                    <br />
                
                
                
            </td>
          
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
                  </tr>
    </table>
</asp:Content>

