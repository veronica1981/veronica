<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsFermieri" Title="Untitled Page" Codebehind="DetailsFermieri.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
       <div>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Detalii Fabrica" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
        
        <br />
        </div>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
            Height="50px" Width="400px" DataKeyNames="FermierID" DataSourceID="SqlDataSource1" BackColor="White" CellPadding="2" CellSpacing="2" OnItemDeleted="DetailsView1_ItemDeleted" OnItemUpdating="DetailsView1_ItemUpdating" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdated="DetailsView1_ItemUpdated">
         <HeaderTemplate>
         
       
        
         </HeaderTemplate>
             <Fields>
          <asp:BoundField DataField="FermierID"  HeaderText="ID" ReadOnly=true InsertVisible =false/>    
         <asp:BoundField DataField="Nume" HeaderText="Fermier"  />
         <asp:BoundField DataField="Strada" HeaderText="Strada" />
         <asp:BoundField DataField="Nr" HeaderText="Numar" />
         <asp:BoundField DataField="Oras" HeaderText="Localitate" />
            <asp:BoundField DataField="CodPostal" HeaderText="CodPostal"  />
         <asp:BoundField DataField="Telefon" HeaderText="Telefon"  />
            <asp:BoundField DataField="Fax" HeaderText="Fax"  />
         <asp:BoundField DataField="Email" HeaderText="Email" />
          <asp:BoundField DataField="TelExtra" HeaderText="Tel. Extra"  />
             <asp:BoundField DataField="NumarReferinta" HeaderText="Nr. referinta"  />
        
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
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditareFermieri.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
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
            SelectCommand="SELECT [FermierID], [Nume], [Strada], [Nr], [Oras], [Judet], [Telefon], [Email],[Fax],[CodPostal],[TelExtra],[NumarReferinta] FROM [Fermier] WHERE ([FermierID] = @FermierID)" DeleteCommand="DELETE FROM [Fermier] WHERE [FermierID] = @FermierID" UpdateCommand="UPDATE [Fermier] SET [Nume] = @Nume, [Strada] = @Strada, [Nr] = @Nr, [Oras] = @Oras,  [Telefon] = @Telefon, [Email] = @Email,[Judet]=@Judet,[Fax]=@Fax,[CodPostal]=@CodPostal,[TelExtra]=@TelExtra,[NumarReferinta]=@NumarReferinta  WHERE [FermierID] = @FermierID" InsertCommand="INSERT INTO [Fermier] ([Nume],[Strada],[Nr],[Oras],[Judet],[Telefon],[Email],[Fax],[CodPostal],[TelExtra],[NumarReferinta]) VALUES (@Nume,@Strada,@Nr,@Oras,@Judet,@Telefon,@Email,@Fax,@CodPostal,@TelExtra,@NumarReferinta)">
            <SelectParameters>
               <asp:QueryStringParameter Name="FermierID" QueryStringField="FermierID" />
            </SelectParameters>
            <DeleteParameters>
              <asp:Parameter Name="FermierID" Type="Int32" />   
            </DeleteParameters>
            <UpdateParameters>
             <asp:Parameter Name="FermierID" Type=Int32 />
            <asp:Parameter Name="Judet" Type=Int32 />
            <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Strada" Type=String />
             <asp:Parameter Name="Nr" Type=String />
            <asp:Parameter Name="Oras" Type=String />
            <asp:Parameter Name="Telefon" Type=String />
            <asp:Parameter Name="Email" Type=String />
            <asp:Parameter Name="Fax" Type=String />
            <asp:Parameter Name="CodPostal" Type=String />
            <asp:Parameter Name="TelExtra" Type=String />
            <asp:Parameter Name="NumarReferinta" Type=String />
            </UpdateParameters>
            <InsertParameters>
  <asp:Parameter Name="Judet" Type=Int32 />
            <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Strada" Type=String />
             <asp:Parameter Name="Nr" Type=String />
            <asp:Parameter Name="Oras" Type=String />
            <asp:Parameter Name="Telefon" Type=String />
            <asp:Parameter Name="Email" Type=String />
            <asp:Parameter Name="Fax" Type=String />
            <asp:Parameter Name="CodPostal" Type=String />
            <asp:Parameter Name="TelExtra" Type=String />
            <asp:Parameter Name="NumarReferinta" Type=String />
          
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [Denloc] FROM [Judete] ORDER BY [Denloc]" >
        </asp:SqlDataSource>
        <br />
    </asp:Panel>
</asp:Content>

