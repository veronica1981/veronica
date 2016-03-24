<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsPrelevatori" Title="Detalii Prelevatori" Codebehind="DetailsPrelevatori.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
       <div>
        <br />
        <table>
        <tr>
        <td style="width: 158px">
        <asp:Label ID="Label1" runat="server" Text="Detalii Prelevator" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label>
        </td>
        <td style="width: 232px">
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                ForeColor="Red"></asp:Label></td>
        </tr>
        </table>
        <br />
        </div>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"  
            Height="50px" Width="400px" DataKeyNames="ID" DataSourceID="SqlDataSource1" BackColor="White" CellPadding="2" CellSpacing="2" OnItemDeleted="DetailsView1_ItemDeleted" OnItemUpdating="DetailsView1_ItemUpdating" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdated="DetailsView1_ItemUpdated" OnModeChanging="DetailsView1_ModeChanging">
         <HeaderTemplate>
         
       
        
         </HeaderTemplate>
             <Fields>
         <asp:BoundField DataField="ID" HeaderText="ID" Readonly=true InsertVisible=false/>
          <asp:BoundField DataField="CodPrelevator" HeaderText="Cod Prelevator" />
         <asp:BoundField DataField="NumePrelevator" HeaderText="Nume Prelevator" />
        <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
        <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen
			Text="Delete" OnClientClick="return confirm('Doriti stergerea prelevatorului?');"></asp:LinkButton>
	    </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditarePrelevatori.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
          </asp:HyperLink>|
          </ItemTemplate>
          </asp:TemplateField>
          </Fields>
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
            <FieldHeaderStyle BackColor="CornflowerBlue" Font-Bold="True" Font-Names="Arial"
                Font-Size="Small" />
            <CommandRowStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" Wrap="True" />
            <FooterStyle BackColor="White" BorderStyle="Solid" BorderWidth="10px" />
        </asp:DetailsView>
        <br />
        <br />
        <br />
        <br />
        &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
           SelectCommand="SELECT  [ID], [CodPrelevator], [NumePrelevator] FROM [Prelevatori] WHERE ([ID] = @ID)" DeleteCommand="DELETE FROM [Prelevatori] WHERE [ID] = @ID" UpdateCommand="UPDATE [Prelevatori] SET [NumePrelevator] = @NumePrelevator, [CodPrelevator]=@CodPrelevator WHERE [ID] = @ID" InsertCommand= "INSERT INTO [Prelevatori] (NumePrelevator,CodPrelevator) VALUES (@NumePrelevator,@CodPrelevator)">
            <SelectParameters>
               <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
            </SelectParameters>
            <DeleteParameters>
              <asp:Parameter Name="ID" Type="String" />   
            </DeleteParameters>
            <UpdateParameters>
             <asp:Parameter Name="ID" Type=Int32 />
              <asp:Parameter Name="CodPrelevator" Type=Int32 />
            <asp:Parameter Name="NumePrelevator" Type=String />
            <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
            </UpdateParameters>
            <InsertParameters>
            <asp:Parameter Name="NumePrelevator" Type=String />
            <asp:Parameter Name="CodPrelevator" Type=Int32 />

            </InsertParameters>
        </asp:SqlDataSource>
        <br />
    </asp:Panel>
</asp:Content>

