<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="RaportFerme4" Title="Raport Ferme" Codebehind="RaportFerme4.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 708px">
        <tr>
            <td style="width: 122px; height: 21px">
            </td>
            <td style="width: 207px; height: 21px">
            </td>
            <td style="width: 179px; height: 21px">
                &nbsp;</td>
             <td style="height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 122px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Data testarii:"></asp:Label></td>
            <td style="width: 207px">
                <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox></td>
            <td style="width: 179px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Fabrica:"></asp:Label></td>
             <td>
                 <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Nume" DataValueField="ID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                 </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 122px">
                </td>
            <td style="width: 207px">
                </td>
            <td style="width: 179px">
                </td>
             <td>
                 </td>
        </tr>
           <tr>
            <td style="width: 122px">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Sef laborator:"></asp:Label></td>
            <td style="width: 207px">
                <asp:DropDownList ID="ddlLaborator" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nume" DataValueField="Fisier">
                </asp:DropDownList></td>
            <td style="width: 179px">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    ForeColor="MediumSeaGreen" Text="Responsabil incercare/analiza:"></asp:Label></td>
             <td>
                 <asp:DropDownList ID="ddlResponsabil" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nume" DataValueField="Fisier">
                 </asp:DropDownList></td>
        </tr>
           <tr>
            <td style="width: 122px">
            </td>
            <td style="width: 207px">
            </td>
            <td style="width: 179px">
            </td>
             <td>
            </td>
        </tr>
          
           <tr  bgcolor="SeaGreen">
            <td style="width: 122px">
            </td>
            <td style="width: 207px">
            </td>
            <td style="width: 179px" align="center">
                <asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                    Text="Raport PDF" OnClick="Button1_Click" /></td>
             <td>
            </td>
        </tr>
          <tr>
            <td style="width: 122px">
            </td>
            <td style="width: 207px">
            </td>
            <td style="width: 179px">
            </td>
             <td>
            </td>
        </tr>
            <tr>
            <td style="width: 122px">
            </td>
            <td style="width: 207px">
                &nbsp;<asp:HyperLink ID="linkall" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="Small" ForeColor="CornflowerBlue" Visible="False">[pdflink]</asp:HyperLink></td>
            <td style="width: 179px">
                </td>
             <td>
            </td>
        </tr>
          
    </table>
    <br />
    <asp:Repeater ID="Repeater1" runat="server" Visible="False">
     <HeaderTemplate>
                <table id=table2 cellspacing=2 cellpadding =2 width="700">
     </HeaderTemplate>
     <ItemTemplate>
     <tr>
            <td align=center>
                <asp:HyperLink ID="pdflink" runat="server" Font-Bold="True" Font-Names="Arial" Text='<%# DataBinder.Eval(Container.DataItem, "Des") %>'
                    Font-Size="Small" ForeColor="CornflowerBlue" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Url") %>'>[pdflink]</asp:HyperLink>
            </td>
             
     </tr>             
     </ItemTemplate>
     <FooterTemplate>
     </table>
     </FooterTemplate>
    </asp:Repeater>
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
        SelectCommand="SELECT  DISTINCT Fabrici.ID, Fabrici.Nume FROM FAbrici,Ferme_CCL,MostreTancuri Where Fabrici.ID = Ferme_CCL.FabricaID AND Ferme_CCL.ID = MostreTancuri.FermaID  &#13;&#10;AND MostreTancuri.Validat =1 AND CONVERT(datetime, MostreTancuri.DataTestareFinala, 103)  = CONVERT(datetime,  @datatestare, 103) ORDER BY Fabrici.Nume">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox1" Name="datatestare" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
        SelectCommand="SELECT [ID], [Nume],[Fisier] FROM [Semnaturi] ORDER BY [Nume]"></asp:SqlDataSource>
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

