<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Account_SelectData" Codebehind="SelectData.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style3
        {
            width: 401px;
        }
        .style4
        {
            width: 208px;
        }
        .style5
        {
            width: 208px;
            height: 35px;
        }
        .style6
        {
            width: 401px;
            height: 35px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<div>
 <table><tr><td>Arhiva:</td>
     <td>
    <asp:DropDownList ID="yearsList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UpdateLinks"></asp:DropDownList>
     </td></tr></table>  
</div>
  <asp:Panel ID="PanelF" runat="server">
    <asp:Panel ID="PanelR" runat="server">
       <asp:HyperLink ID="HL"  ImageUrl="~/Images/Excel32.gif" runat="server"></asp:HyperLink> 
      <asp:Literal ID="Filename" runat="server" Text="Raport cumulat"></asp:Literal>
      </asp:Panel>
       <p style="margin-left:5px;padding-left:5px;">
           Selectati parametrii pentru care doriti rapoarte suplimentare:
      </p>
       <table cellpadding="5" cellspacing="5">
       <tr><td class="style4">         
            <asp:CheckBox ID="chkG" runat="server" Text="Grasime"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkg" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkg]</asp:HyperLink>
            </td>
      </tr>
        <tr><td class="style5">
            <asp:CheckBox ID="chkP" runat="server" Text="Proteine"  Checked="true"></asp:CheckBox></td>
            <td class="style6"><asp:HyperLink ID="xlslinkp" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkp]</asp:HyperLink>
            </td>
       </tr>
       <tr><td class="style4">
            <asp:CheckBox ID="chkC" runat="server" Text="Cazeina"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkc" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkc]</asp:HyperLink>
            </td>
       </tr>
       <tr><td class="style4">
            <asp:CheckBox ID="chkL" runat="server" Text="Lactoza"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkl" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkl]</asp:HyperLink>
            </td>
        </tr>
        <tr><td class="style4">
            <asp:CheckBox ID="chkS" runat="server" Text="SUN"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinks" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinks]</asp:HyperLink>
            </td>
        </tr>
        <tr><td class="style4">
            <asp:CheckBox ID="chkH" runat="server" Text="pH"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkh" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkh]</asp:HyperLink>
            </td>
         </tr>
         <tr><td class="style4">
            <asp:CheckBox ID="chkU" runat="server" Text="Uree"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinku" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinku]</asp:HyperLink>
            </td>
          </tr>
          <tr><td class="style4">
            <asp:CheckBox ID="chkN" runat="server" Text="NCS"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkn" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkn]</asp:HyperLink>   
            </td>
            </tr>
            <tr><td class="style4">
            <asp:CheckBox ID="chkZ" runat="server" Text="Cant. lapte"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkz" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkz]</asp:HyperLink>   
            </td>
            </tr>
              <tr><td class="style4">
            <asp:CheckBox ID="chkGP" runat="server" Text="Grasime/Proteine %"  Checked="true"></asp:CheckBox></td>
            <td class="style3"><asp:HyperLink ID="xlslinkgp" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[xlslinkgp]</asp:HyperLink>   
            </td>
            </tr>
            </table>
           
    <table>
    <tr>
    <td>
     <p class="submitButton">
    <asp:Button ID="ButtonCreate" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    Text="Generare Grafice" onclick="ButtonCreate_Click" />
    </p>
    </td>
    <td>
        &nbsp;</td>
    </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="PanelA" runat="server">
    <p>
        &nbsp;</p>
    <p style="margin-left:5px;padding-left:5px;">
        &nbsp;Pentru a vizualiza rezultatele analizelor, dati click pe link-urile din lista de mai jos.
        
    </p>
            
   
    
   <asp:DataGrid runat="server" id="xlsList" Font-Name="Arial"
	AutoGenerateColumns="False" AlternatingItemStyle-BackColor="#eeeeee"
	HeaderStyle-BackColor="#496077" HeaderStyle-ForeColor="White" CellPadding="5"
	HeaderStyle-Font-Size="10pt" HeaderStyle-Font-Bold="True">
  <Columns>
  
    <asp:HyperLinkColumn DataNavigateUrlField="DownloadLink" DataTextField="Name" HeaderText="Nume fisier"  HeaderImageUrl="~/Images/Excel32.gif" ItemStyle-ForeColor=" #496077"/>
    <asp:BoundColumn DataField="LastWriteTime" HeaderText="Data actualizarii" HeaderStyle-HorizontalAlign="Center" 
        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy hh:mm}" />
    <asp:BoundColumn DataField="Length" HeaderText="KB" HeaderStyle-HorizontalAlign="Center"
		ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,000}" />
  </Columns>
</asp:DataGrid>  

    </asp:Panel>
    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
      
</asp:Content>

