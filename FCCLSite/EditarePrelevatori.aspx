<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditarePrelevatori" Title="Prelevatori" Codebehind="EditarePrelevatori.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript">
<!--

function TABLE1_onclick() {

}

// -->
</script>

    <asp:Panel ID="Panel1" runat="server" Width="90%">
        <table style="width: 100%"  cellspacing=0 cellpadding=5 id="TABLE1" language="javascript" onclick="return TABLE1_onclick()">
            <tr>
                <td style="width: 238px; height: 32px">
                </td>
                <td style="width: 172px; height: 32px">
                </td>
                <td style="height: 32px">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Nume Prelevator:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="Prelevator" runat="server"></asp:TextBox></td>
                <td align="center">
                    <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                        Font-Names="Arial" Font-Size="Medium" OnClick="Button1_Click" Text="Cautare" Width="112px" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Cod Prelevator:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="CodPrelevator" runat="server"></asp:TextBox></td>
                <td align="center">
                    <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                        ForeColor="SeaGreen" NavigateUrl="~/DetailsPrelevatori.aspx">Adaugare</asp:HyperLink></td>
            </tr>
            <tr bgcolor=mediumseagreen>
            <td>
           <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
               Width="129px"></asp:Label>
           <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label></td>
           <td></td>
           <td></td>
           
            </tr>
            <tr>
             <td style="height: 21px"></td>
           <td style="height: 21px"></td>
           <td style="height: 21px"></td>
           
            </tr>
        </table>
    
    </asp:Panel>
    <asp:Panel ID="PanelE" runat="server" Width="90%">
      
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="id" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing"  Width="100%">
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
        <Columns>
         <asp:BoundField DataField="id" HeaderText="ID" />
          <asp:BoundField DataField="codprelevator" HeaderText="Cod Prelevator" />
           <asp:BoundField DataField="numeprelevator" HeaderText="Nume Prelevator"  />                  
       
            <asp:HyperLinkField HeaderText="Detalii..." Text="Detalii..." DataNavigateUrlFields="id"
            DataNavigateUrlFormatString="DetailsPrelevatori.aspx?ID={0}" />   

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

