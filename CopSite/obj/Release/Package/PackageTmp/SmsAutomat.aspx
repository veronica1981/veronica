<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SmsAutomat" Title="Trimitere Sms" Codebehind="SmsAutomat.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 601px; height: 145px">
        <tr>
            <td style="width: 211px">
            </td>
            <td style="width: 192px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan=3>
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                    ForeColor="MediumSeaGreen"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 211px">
            </td>
            <td style="width: 192px">
            </td>
            <td>
            </td>
        </tr>
          <tr>
            <td style="width: 211px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    Text="Data de selectie a rapoartelor:"></asp:Label></td>
            <td style="width: 192px">
                <asp:TextBox ID="DataSelectie" runat="server"></asp:TextBox>
                <asp:Button ID="btnDate1" runat="server" Text="..." OnClick="btnDate1_Click" /></td>
            <td>
            	<asp:Calendar id="cal1" runat="server" OnSelectionChanged="dateChanged1"  backcolor="#ffffff" width="125px" height="100px"
							font-size="12px" font-names="Arial" borderwidth="2px" bordercolor="#000000" nextprevformat="shortmonth" FirstDayOfWeek="Monday"
							daynameformat="firsttwoletters" Visible="False">
							<TodayDayStyle ForeColor="White" BackColor="Black"></TodayDayStyle>
							<NextPrevStyle Font-Size="12px" Font-Bold="True" ForeColor="#333333"></NextPrevStyle>
							<DayHeaderStyle Font-Size="12px" Font-Bold="True"></DayHeaderStyle>
							<TitleStyle Font-Size="14px" Font-Bold="True" BorderWidth="2px" ForeColor="#000055"></TitleStyle>
							<OtherMonthDayStyle ForeColor="#CCCCCC"></OtherMonthDayStyle>
							</asp:Calendar> 

            </td>
        </tr>
          <tr>
            <td style="width: 211px">
            </td>
            <td style="width: 192px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 211px">
            </td>
            <td style="width: 192px">
                <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                    Font-Names="Arial" Font-Size="10pt" Text="Trimite SMS" 
                    OnClick="Button1_Click" /></td>
            <td>
                <asp:HyperLink ID="fissms" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                    ForeColor="SeaGreen">Sms - Trimise</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="width: 211px">
            </td>
            <td style="width: 192px" align="center">
                </td>
            <td>
                <asp:HyperLink ID="fislog" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                    ForeColor="SeaGreen">Fisierul Log</asp:HyperLink></td>
        </tr>
    </table>


</asp:Content>

