<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsCrotalii" Title="Detalii Crotalie" EnableTheming="true" Codebehind="DetailsCrotalii.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
       <div>
        <br />
        <table>
        <tr>
        <td style="width: 190px">
        <asp:Label ID="Label1" runat="server" Text="Detalii Croatalie" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
        </td>
        <td style="width: 292px">
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            ForeColor="Red" Width="200px"></asp:Label></td>
        </tr>
        </table>
        <br />
        </div>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"
            Height="50px" Width="500px"  DataKeyNames="Crotalia" DataSourceID="SqlDataSource1" BackColor="White" CellPadding="2" CellSpacing="2"
             OnItemDeleted="DetailsView1_ItemDeleted" OnItemUpdating="DetailsView1_ItemUpdating" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdated="DetailsView1_ItemUpdated">
         <HeaderTemplate>
         
       
        
         </HeaderTemplate>
             <Fields>
             
         
                 <asp:TemplateField HeaderText="Cod">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CodBare") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <InsertItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CodBare") %>'></asp:TextBox>
                     </InsertItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("CodBare") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Crotalia">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Crotalia") %>' Width=250></asp:TextBox>
                     </EditItemTemplate>
                     <InsertItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Crotalia") %>' Width=250></asp:TextBox>
                     </InsertItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("Crotalia") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
          <asp:TemplateField HeaderText="Ferma" >
         <ItemTemplate>
          <asp:DropDownList    Enabled = false ID=Ferma runat=server DataSourceID="SqlDataSource3" DataTextField="Nume" DataValueField="ID" SelectedValue='<%# Bind("FermaId") %>' Width="150px">
         </asp:DropDownList>  
         </ItemTemplate>
         <InsertItemTemplate>
         <asp:DropDownList   ID=Ferma runat=server DataSourceID="SqlDataSource3" DataTextField="Nume" DataValueField="ID" SelectedValue='<%# Bind("FermaId") %>' Width="150px">
         </asp:DropDownList>  
         </InsertItemTemplate>
            <EditItemTemplate>
         <asp:DropDownList   ID=Ferma runat=server DataSourceID="SqlDataSource3" DataTextField="Nume" DataValueField="ID" SelectedValue='<%# Bind("FermaId") %>' Width="150px">
         </asp:DropDownList>  
         </EditItemTemplate>
         <ControlStyle BackColor="White"  Font-Names="Arial" Font-Size="Small" />
         </asp:TemplateField>
    
         <asp:BoundField DataField="Nume" HeaderText="Nume" />
         <asp:BoundField DataField="Rasa" HeaderText="Rasa" />
         <asp:BoundField DataField="DataNasterii" HeaderText="Data Nasterii" />
       

         
       
         
       
        <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
        <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen
			Text="Delete" OnClientClick="return confirm('Doriti stergerea crotaliei?');"></asp:LinkButton>
	    </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditareCrotalii.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
          </asp:HyperLink>|
          </ItemTemplate>
          </asp:TemplateField>
          </Fields>
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
       
        </asp:DetailsView>
        &nbsp;&nbsp;<br />
        <br />
        <br />
        <br />
        &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [CodBare], [Crotalia], [Nume], [Rasa], [DataNasterii], [FermaId] FROM [Crotalii] WHERE ([Crotalia] = @Crotalia)" DeleteCommand="DELETE FROM [Crotalii] WHERE [Crotalia] = @Crotalia" UpdateCommand="UPDATE [Crotalii] SET [Nume] = @Nume,  [CodBare]=@CodBare,[Crotalia]=@Crotalia,[Rasa]=@Rasa,[DataNasterii]=@DataNasterii,[FermaId]=@FermaId WHERE [Crotalia] = @KeyCrot" InsertCommand="INSERT INTO Crotalii (CodBare,Crotalia,Nume,Rasa,DataNasterii,FermaId) VALUES (@CodBare,@Crotalia,@Nume,@Rasa,@DataNasterii,@FermaId)">
            <SelectParameters>
               <asp:QueryStringParameter Name="Crotalia" QueryStringField="Crotalia" />
            </SelectParameters>
            <DeleteParameters>
              <asp:Parameter Name="Crotalia" Type="String" />   
            </DeleteParameters>
            <UpdateParameters>
            <asp:Parameter Name="FermaId" Type=Int32 />
             <asp:Parameter Name="CodBare" Type=String />
               <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Crotalia" Type=String />
             <asp:Parameter Name="Rasa" Type=String />
            <asp:Parameter Name="DataNasterii" Type=String />
            <asp:ControlParameter ControlID="OldCrot" Name="KeyCrot" Type =String  PropertyName="Text"/>
            </UpdateParameters>
            <InsertParameters>
            <asp:Parameter Name="FermaId" Type=Int32 />
             <asp:Parameter Name="CodBare" Type=String />
               <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Crotalia" Type=String />
             <asp:Parameter Name="Rasa" Type=String />
            <asp:Parameter Name="DataNasterii" Type=String />
            </InsertParameters>
        </asp:SqlDataSource>
        
     
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [Nume] FROM [Ferme_CCL] ORDER BY [Nume]" >
        </asp:SqlDataSource>
        <br />
        <asp:Label ID = "OldCrot"  runat="server" Text="" ForeColor="White"></asp:Label>
    </asp:Panel>
</asp:Content>

