<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditareFerme" Title="Ferme" Codebehind="EditareFerme.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:Panel ID="Panel1" runat="server" Width="90%">
        <table  cellspacing=0 cellpadding=5 width=100%>
            <tr>
                <td style=" height: 32px;">
                </td>
                <td style=" height: 32px;">
                </td>
                <td style="height: 32px; ">
                </td>
            </tr>
            <tr>
                <td style=" height: 35px;">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Nume Fabrica:"></asp:Label></td>
                <td style=" height: 35px;">
                    <asp:TextBox ID="Fabrica" runat="server" AutoPostBack="True" OnTextChanged="Fabrica_TextChanged"></asp:TextBox></td>
                <td align="right" style=" height: 35px;">
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Nume Ferma:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="Ferma" runat="server" AutoPostBack="True" OnTextChanged="Ferma_TextChanged"></asp:TextBox></td>
                <td>
                </td>
            </tr>
            <tr >
            <td>
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                    ForeColor="SeaGreen" Text="Cod Ferma:"></asp:Label>
                &nbsp;<td>
                <asp:TextBox ID="Cod" runat="server" AutoPostBack="True" OnTextChanged="Cod_TextChanged"></asp:TextBox></td>
           <td align="right">
                    <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                        Font-Names="Arial" Font-Size="Medium" OnClick="Button1_Click" Text="Cautare" Width="112px" /></td>
           
            </tr>
            <tr>
            <td></td>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                    ForeColor="SeaGreen" NavigateUrl="~/DetailsFerme.aspx">Adaugare</asp:HyperLink></td>
            <td style="width: 370px"></td>

            </tr>
            <tr bgcolor=mediumseagreen>
             <td style="height: 21px">
           <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
               Width="129px"></asp:Label></td>
           <td style="height: 21px" bgcolor="honeydew"></td>
           <td style="height: 21px;" bgcolor="white"></td>
           
            </tr>
        </table>
    
    </asp:Panel>

  
  
  
    <asp:Panel ID="PanelE" runat="server" Width="90%">
           
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="ID" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing"  Width="100%">
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
        <Columns>
         <asp:BoundField DataField="id" HeaderText="ID"  />                  
          <asp:BoundField DataField="cod" HeaderText="Cod" />
          <asp:BoundField DataField="nume" HeaderText="Nume" />
         <asp:BoundField DataField="strada" HeaderText="Strada" />
         <asp:BoundField DataField="numar" HeaderText="Numar" />
          <asp:BoundField DataField="oras" HeaderText="Oras" />
            <asp:BoundField DataField="judet" HeaderText="Judet" />
           <asp:BoundField DataField="telefon" HeaderText="Telefon" />
           <asp:BoundField DataField="email" HeaderText="Email" />
          <asp:BoundField DataField="fabricaid" HeaderText="Fabrica" />
        
       
            <asp:HyperLinkField HeaderText="Detalii..." Text="Detalii..." DataNavigateUrlFields="ID"
            DataNavigateUrlFormatString="DetailsFerme.aspx?ID={0}" />   
            
   
        </Columns>
            <PagerSettings PageButtonCount="20" />
            <RowStyle BackColor="CornflowerBlue" Font-Bold="False" Font-Names="Arial" Font-Size="9pt" BorderColor="White" BorderStyle="Solid" />
            <HeaderStyle BackColor="CornflowerBlue" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                ForeColor="White" BorderStyle="Solid" />
            <AlternatingRowStyle BackColor=PaleTurquoise />
        </asp:GridView>
        &nbsp;&nbsp;
        
</asp:Panel>   
</asp:Content>

