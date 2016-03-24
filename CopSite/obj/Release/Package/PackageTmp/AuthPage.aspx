<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="AuthPage" Title="Untitled Page" Codebehind="AuthPage.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 90%" cellspacing=5 border=0 cellpadding=2>
        <tr>
            <td style="height: 21px">
            </td>
            <td style="height: 21px">
            </td>
          
        </tr>
        <tr>
            <td width=30% valign=top>
             <asp:LoginName ID="LoginName1" runat="server" FormatString="Welcome {0}!" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="MediumSeaGreen" />
        <br />
        <br />
        <asp:LoginStatus ID="LoginStatus1" runat="server" Font-Names="Arial" Font-Size="Medium" ForeColor="Black"/>
                &nbsp;<br />
                </td>
            <td align=center>
                <span lang="EN-GB" style="font-size: 14pt; color: MediumSeaGreen; font-family: Arial">
                    Obiective si activitati</span></p>
                  
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

