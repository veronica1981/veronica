<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsFabrici" Title="Detaliu Fabrica" Codebehind="DetailsFabrici.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
       <div>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Detalii Fabrica" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
        
        <br />
        </div>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
            Height="50px" Width="400px" DataKeyNames="ID" DataSourceID="SqlDataSource1" BackColor="White" CellPadding="2" CellSpacing="2" OnItemDeleted="DetailsView1_ItemDeleted" OnItemUpdating="DetailsView1_ItemUpdating" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdated="DetailsView1_ItemUpdated" OnModeChanging="DetailsView1_ModeChanging">
         <HeaderTemplate>
         
       
        
         </HeaderTemplate>
             <Fields>
          <asp:BoundField DataField="ID"  HeaderText="ID" Readonly=true InsertVisible=false/>    
         <asp:BoundField DataField="Nume" HeaderText="Fabrica"   />
         <asp:BoundField DataField="Strada" HeaderText="Strada" />
         <asp:BoundField DataField="Numar" HeaderText="Numar" />
         <asp:BoundField DataField="Oras" HeaderText="Localitate" />
            <asp:BoundField DataField="CodPostal" HeaderText="CodPostal"  />
         <asp:BoundField DataField="Telefon" HeaderText="Telefon"  />
            <asp:BoundField DataField="Fax" HeaderText="Fax"  />
         <asp:BoundField DataField="Email" HeaderText="Email"  ItemStyle-Width=150pt ControlStyle-Width="150" />
          <asp:BoundField DataField="PersonaDeContact" HeaderText="Pers. Contact"  />
             <asp:BoundField DataField="TelPersContact" HeaderText="Tel. Contact"  />
        
         <asp:TemplateField HeaderText="Judet" >
         <ItemTemplate>
          
           <asp:DropDownList    Enabled = false ID=Judete runat=server DataSourceID="SqlDataSource2" DataTextField="Denloc" DataValueField="ID" OnSelectedIndexChanged="Judete_SelectedIndexChanged" SelectedValue='<%# Bind("Judet") %>' Width="150px">
         </asp:DropDownList>  
         </ItemTemplate>
         <InsertItemTemplate>
         <asp:DropDownList   ID=Judete runat=server DataSourceID="SqlDataSource2" DataTextField="Denloc" DataValueField="ID" OnSelectedIndexChanged="Judete_SelectedIndexChanged" SelectedValue='<%# Bind("Judet") %>' Width="150px">
         </asp:DropDownList>  
         </InsertItemTemplate>
            <EditItemTemplate>
         <asp:DropDownList   ID=Judete runat=server DataSourceID="SqlDataSource2" DataTextField="Denloc" DataValueField="ID" OnSelectedIndexChanged="Judete_SelectedIndexChanged" SelectedValue='<%# Bind("Judet") %>' Width="150px">
         </asp:DropDownList>  
         </EditItemTemplate>

             <ControlStyle BackColor="White"  Font-Names="Arial" Font-Size="Small" />
         </asp:TemplateField>
         
         <asp:TemplateField ShowHeader=False>
         <ItemTemplate>
          <p></p> 
         </ItemTemplate>
         </asp:TemplateField>
        
        <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
     
        <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen
			Text="Delete" OnClientClick="return confirm('Doriti stergerea fabricii?');"></asp:LinkButton>
	    </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditareFabrici.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
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
            SelectCommand="SELECT [ID], [Nume], [Strada], [Numar], [Oras], [Judet], [Telefon], [Email],[Fax],[CodPostal],[PersonaDeContact],[TelPersContact] FROM [Fabrici] WHERE ([ID] = @ID)" DeleteCommand="DELETE FROM [Fabrici] WHERE [ID] = @ID" UpdateCommand="UPDATE [Fabrici] SET [Nume] = @Nume, [Strada] = @Strada, [Numar] = @Numar, [Oras] = @Oras,  [Telefon] = @Telefon, [Email] = @Email,[Judet]=@Judet,[Fax]=@Fax,[CodPostal]=@CodPostal,[PersonaDeContact]=@PersonaDeContact,[TelPersContact]=@TelPersContact   WHERE [ID] = @ID" InsertCommand="INSERT INTO [Fabrici] ([Nume],[Strada],[Numar],[Oras],[Judet],[Telefon],[Email],[Fax],[CodPostal],[PersonaDecontact],[TelPersContact]) VALUES (@Nume,@Strada,@Numar,@Oras,@Judet,@Telefon,@Email,@Fax,@CodPostal,@PersonaDeContact,@TelPersContact)">
            <SelectParameters>
               <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
            </SelectParameters>
            <DeleteParameters>
              <asp:Parameter Name="ID" Type="Int32" />   
            </DeleteParameters>
            <UpdateParameters>
             <asp:Parameter Name="ID" Type=Int32 />
            <asp:Parameter Name="Judet" Type=Int32 />
            <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Strada" Type=String />
             <asp:Parameter Name="Numar" Type=String />
            <asp:Parameter Name="Oras" Type=String />
            <asp:Parameter Name="Telefon" Type=String />
            <asp:Parameter Name="Email" Type=String />
            <asp:Parameter Name="Fax" Type=String />
            <asp:Parameter Name="CodPostal" Type=String />
            <asp:Parameter Name="PersonaDeContact" Type=String />
            <asp:Parameter Name="TelPersContact" Type=String />
            </UpdateParameters>
            <InsertParameters>
             <asp:Parameter Name="ID" Type=Int32 />
            <asp:Parameter Name="Judete" Type=Int32 />
            <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Strada" Type=String />
             <asp:Parameter Name="Numar" Type=String />
            <asp:Parameter Name="Oras" Type=String />
            <asp:Parameter Name="Telefon" Type=String />
            <asp:Parameter Name="Email" Type=String />
            <asp:Parameter Name="Fax" Type=String />
            <asp:Parameter Name="CodPostal" Type=String />
            <asp:Parameter Name="PersonaDeContact" Type=String />
            <asp:Parameter Name="TelPersContact" Type=String />

            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [Denloc] FROM [Judete] ORDER BY [Denloc]" >
        </asp:SqlDataSource>
        <br />
    </asp:Panel>
</asp:Content>

