<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditareFabrici" Title="Asociatii" EnableTheming="true" Codebehind="EditareFabrici.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:Panel ID="Panel1" runat="server" Width="90%">
        <table style="width: 100%"  cellspacing=0 cellpadding=5>
            <tr>
                <td style="height: 32px">
                </td>
                <td style="height: 32px">
                </td>
                <td style="height: 32px;">
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Nume Asociatie:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="Fabrica" runat="server"></asp:TextBox></td>
                <td align="right">
                    <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                        Font-Names="Arial" Font-Size="Medium" OnClick="Button1_Click" Text="Cautare" Width="112px" /></td>
            </tr>
            <tr>
                <td style="height: 29px;">
                </td>
                <td style="height: 29px;">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/DetailsFabrici.aspx">Adaugare</asp:HyperLink></td>
                <td style="height: 29px;">
                </td>
            </tr>
            <tr bgcolor=mediumseagreen>
            <td>
           <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" SkinID="black"
               ></asp:Label>
           <td </td>
           <td></td>
           
            </tr>
            <tr>
             <td style="height: 21px"></td>
           <td style="height: 21px;"></td>
           
            </tr>
        </table>
    
    </asp:Panel>

    
    
    <asp:Panel ID="PanelE" runat="server" Width="90%">
       
           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="ID" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing"  Width="100%">
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
        <Columns>
         <asp:BoundField DataField="id" HeaderText="ID"  />                  
         <asp:BoundField DataField="nume" HeaderText="Asociatia" />
         <asp:BoundField DataField="strada" HeaderText="Strada" />
         <asp:BoundField DataField="numar" HeaderText="Numar" />
          <asp:BoundField DataField="oras" HeaderText="Localitate" />
           <asp:BoundField DataField="judet" HeaderText="Judet" />
           <asp:BoundField DataField="email" HeaderText="Email" />

          <asp:BoundField DataField="telefon" HeaderText="Telefon" /> 
            <asp:HyperLinkField HeaderText="Detalii..." Text="Detalii..." DataNavigateUrlFields="id"
            DataNavigateUrlFormatString="DetailsFabrici.aspx?ID={0}" />   
            
   
        </Columns>
            <PagerSettings PageButtonCount="20" />
         
        </asp:GridView>
        &nbsp;&nbsp;
        
</asp:Panel>   
</asp:Content>

