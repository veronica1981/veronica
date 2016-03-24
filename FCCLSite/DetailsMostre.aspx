<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsMostre" Title="Untitled Page" Codebehind="DetailsMostre.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
       <div>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Detalii Mostra" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
        
        <br />
        </div>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"
            Height="50px" Width="400px" DataKeyNames="CodBare" DataSourceID="SqlDataSource1" BackColor="White" CellPadding="2" CellSpacing="2" OnItemDeleted="DetailsView1_ItemDeleted" OnItemUpdating="DetailsView1_ItemUpdating" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdated="DetailsView1_ItemUpdated">
         <HeaderTemplate>
         
       
        
         </HeaderTemplate>
             <Fields>
          <asp:BoundField DataField="ID"  HeaderText="ID" />    
         <asp:BoundField DataField="CodBare" HeaderText="Cod Bare"  />
         <asp:BoundField DataField="IdZilnic" HeaderText="Id Zilnic" />
         <asp:BoundField DataField="DataTestare" HeaderText="Data Testare" />
         <asp:BoundField DataField="DataTestareFinala" HeaderText="Data Testare Finala" />
         <asp:BoundField DataField="DataPrelevare" HeaderText="Data Prelevare" />
         <asp:TemplateField HeaderText="Prelevator" >
         <ItemTemplate>
          
           <asp:DropDownList    Enabled = false ID=Prelevatori runat=server DataSourceID="SqlDataSource2" DataTextField="NumePrelevator" DataValueField="ID" OnSelectedIndexChanged="Prelevatori_SelectedIndexChanged" SelectedValue='<%# Bind("PrelevatorID") %>' Width="150px">
         </asp:DropDownList>  
         </ItemTemplate>
         <InsertItemTemplate>
         <asp:DropDownList   ID=Prelevatori runat=server DataSourceID="SqlDataSource2" DataTextField="NumePrelevator" DataValueField="ID" OnSelectedIndexChanged="Prelevatori_SelectedIndexChanged" SelectedValue='<%# Bind("PrelevatorID") %>' Width="150px">
         </asp:DropDownList>  
         </InsertItemTemplate>
            <EditItemTemplate>
         <asp:DropDownList   ID=Prelevatori runat=server DataSourceID="SqlDataSource2" DataTextField="NumePrelevator" DataValueField="ID" OnSelectedIndexChanged="Prelevatori_SelectedIndexChanged" SelectedValue='<%# Bind("PrelevatorID") %>' Width="150px">
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
			Text="Delete" OnClientClick="return confirm('Doriti stergerea mostrei?');"></asp:LinkButton>
	    </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditareMostre.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
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
            SelectCommand="SELECT [CodBare], [ID], [IdZilnic], [MostraID], [CantitateLaPrelevare], [FermaID], [CodFerma], [CCLID], [CodCCL], [DataTestare], [Grasime], [ProcentLactoza], [ProcentProteine], [PunctInghet], [IncarcaturaGermeni], [NumarCeluleSomatice], [SubstantaUscata], [Antibiotice], [PrelevatorID], [DataPrimirii], [DataPrelevare], [Definitiv], [Validat], [NrComanda], [NumePrelevator], [NumeClient], [Judet], [Localitate], [AdresaClient], [NumeProba], [DataTestareFinala] FROM [MostreTancuri] WHERE ([CodBare] = @codbare)" DeleteCommand="DELETE FROM [MostreTancuri] WHERE [CodBare] = @CodBare" UpdateCommand="UPDATE [MostreTancuri] SET [PrelevatorID] = @PrelevatorID, [IdZilnic] = @IdZilnic, [DataTestare]=@DataTestare,[DataTestareFinala]=@DataTestareFinala,[DataPrelevare]=@DataPrelevare WHERE [ID] = @ID" InsertCommand= "INSERT INTO MOSTRETANCURI (ID,IdZilnic,PrelevatorID,DataTestare,DataTestareFinala,DataPrelevare) VALUES (@ID,@IdZilnic,@PrelevatorID,@DataTestare,@DataTestareFinala,@DataPrelevare)">
            <SelectParameters>
               <asp:QueryStringParameter Name="CodBare" QueryStringField="CodBare" />
            </SelectParameters>
            <DeleteParameters>
              <asp:Parameter Name="CodBare" Type="String" />   
            </DeleteParameters>
            <UpdateParameters>
            <asp:Parameter Name="PrelevatorID" Type=Int32 />
             <asp:Parameter Name="ID" Type=Int32 />
            <asp:Parameter Name="IdZilnic" Type=Int32 />
             <asp:Parameter Name="DataTestare" Type=String />
             <asp:Parameter Name="DataTestareFinala" Type=String />
              <asp:Parameter Name="DataPrelevare" Type=String />
              
            </UpdateParameters>
            <InsertParameters>
            <asp:Parameter Name="PrelevatorID" Type=Int32 />
             <asp:Parameter Name="ID" Type=Int32 />
            <asp:Parameter Name="IdZilnic" Type=Int32 />
             <asp:Parameter Name="DataTestare" Type=String />
             <asp:Parameter Name="DataTestareFinala" Type=String />
              <asp:Parameter Name="DataPrelevare" Type=String />
            
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [NumePrelevator] FROM [Prelevatori] ORDER BY [NumePrelevator], [ID]" >
        </asp:SqlDataSource>
        <br />
    </asp:Panel>
</asp:Content>



