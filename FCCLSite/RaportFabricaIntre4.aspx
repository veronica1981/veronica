<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="RaportFabricaIntre4.aspx.cs" Inherits="RaportFabricaIntre4" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
 <table style="width: 800px">
        <tr>
            <td style="width: 162px; height: 17px">
            </td>
            <td style="width: 175px; height: 17px">
            </td>
            <td style="width: 258px; height: 17px">
                &nbsp;</td>
             <td style="height: 17px">
            </td>
        </tr>
        <tr>
            <td style="width: 162px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Data testarii initiala:"></asp:Label></td>
            <td style="width: 175px">
                <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox></td>
            <td style="width: 258px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Data testarii finala:"></asp:Label></td>
             <td>
                 <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="True" OnTextChanged="TextBox2_TextChanged"></asp:TextBox></td>
        </tr>
        <tr>
        <td colspan=4></td>
        </tr>
        <tr>
            <td style="width: 162px">
                </td>
            <td style="width: 175px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Fabrica:"></asp:Label></td>
            <td style="width: 258px">
                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1"
                    DataTextField="Nume" DataValueField="ID">
                </asp:DropDownList></td>
             <td>
                 </td>
        </tr>
         <tr>
        <td colspan=4></td>
        </tr>
           <tr>
            <td style="width: 162px">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Sef laborator:"></asp:Label></td>
            <td style="width: 175px">
                <asp:DropDownList ID="ddlLaborator" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nume" DataValueField="ID">
                </asp:DropDownList></td>
            <td style="width: 258px">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Responsabil incercare/analiza:"></asp:Label></td>
             <td>
                 <asp:DropDownList ID="ddlResponsabil" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nume" DataValueField="ID">
                 </asp:DropDownList></td>
        </tr>
         <tr>
        <td colspan=4></td>
        </tr>
           <tr>
            <td style="width: 162px">
            </td>
            <td style="width: 175px">
            </td>
            <td style="width: 258px">
            </td>
             <td>
            </td>
        </tr>
           <tr>
        <td colspan=4></td>
        </tr>
           <tr>
           
            <td  align="center" colspan=4 bgcolor="MediumSeaGreen">
                <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    Text="Raport PDF&Excel" OnClick="Button1_Click"/></td>
           
        </tr>
         <tr>
        <td colspan=4></td>
        </tr>
               <tr>
        <td colspan=4>&nbsp;</td>
        </tr>
          <tr>
           
            <td colspan=2 align="center">
                <asp:HyperLink ID="pdflink" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue">[pdflink]</asp:HyperLink></td>
           
             <td colspan=2>
            </td>
        </tr>
            <tr>
        <td colspan=4></td>
        </tr>
            <tr >
             <td colspan=2>
            </td>           
            <td colspan=2 align="center">
                <asp:HyperLink ID="xlslink" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="CornflowerBlue">[xlslink]</asp:HyperLink></td>

        </tr>
          
    </table>
    <br />
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
        SelectCommand="SELECT  DISTINCT Fabrici.ID, Fabrici.Nume FROM FAbrici,Ferme_CCL,MostreTancuri Where Fabrici.ID = Ferme_CCL.FabricaID AND Ferme_CCL.ID = MostreTancuri.FermaID  &#13;&#10;AND MostreTancuri.Validat =1 AND (CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) >= CONVERT(datetime,  @datatestare, 103)) AND (CONVERT(datetime, MostreTancuri.DataTestareFinala, 103) <= CONVERT(datetime,  @datatestare2, 103)) ORDER BY Fabrici.Nume">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox1" Name="datatestare" PropertyName="Text" />
            <asp:ControlParameter ControlID="TextBox2" Name="datatestare2" PropertyName="Text" />

        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
        SelectCommand="SELECT [ID], [Nume] FROM [Semnaturi] ORDER BY [Nume]"></asp:SqlDataSource>
    &nbsp;<br />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMostreFabrica"
        TypeName="MostreFabrica">
        <SelectParameters>
            <asp:Parameter DefaultValue="" Name="fabricaid" Type="String" />
            <asp:Parameter DefaultValue="" Name="datatestare" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    &nbsp; &nbsp;<br />
    <br />    
    
</asp:Content>