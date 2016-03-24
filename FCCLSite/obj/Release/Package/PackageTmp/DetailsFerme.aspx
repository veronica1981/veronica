<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsFerme" Title="Detalii Ferma" Codebehind="DetailsFerme.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
       <div>
        <br />
        <table>
        <tr>
        <td style="width: 190px">
        <asp:Label ID="Label1" runat="server" Text="Detalii Ferma" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
        </td>
        <td style="width: 292px">
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            ForeColor="Red" Width="200px"></asp:Label></td>
        </tr>
        </table>
        <br />
        </div>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"
            Height="50px" Width="500px"  DataKeyNames="ID" DataSourceID="SqlDataSource1" BackColor="White" CellPadding="2" CellSpacing="2" OnItemDeleted="DetailsView1_ItemDeleted" OnItemUpdating="DetailsView1_ItemUpdating" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting" OnItemUpdated="DetailsView1_ItemUpdated">
         <HeaderTemplate>
         
       
        
         </HeaderTemplate>
             <Fields>
             
         <asp:BoundField DataField="ID"  HeaderText="ID" ReadOnly=True InsertVisible=False />    
                 <asp:TemplateField HeaderText="Cod">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Cod") %>'></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regCod" runat="server"   Font-Size=8 ControlToValidate="TextBox2"
            ErrorMessage="Codul trebuie sa fie numeric pe 5 pozitii!!"   ValidationExpression="\d{5}"></asp:RegularExpressionValidator>
                     </EditItemTemplate>
                     <InsertItemTemplate>
                         <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Cod") %>'></asp:TextBox>
                          <asp:RegularExpressionValidator ID="regCod" runat="server"   Font-Size=8 ControlToValidate="TextBox2"
            ErrorMessage="Codul trebuie sa fie numeric pe 5 pozitii!!"   ValidationExpression="\d{5}"></asp:RegularExpressionValidator>
                     </InsertItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("Cod") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Ferma">
                     <EditItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Nume") %>' Width=250></asp:TextBox>
                     </EditItemTemplate>
                     <InsertItemTemplate>
                         <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Nume") %>' Width=250></asp:TextBox>
                     </InsertItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("Nume") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
          <asp:TemplateField HeaderText="Fabrica" >
         <ItemTemplate>
          <asp:DropDownList    Enabled = false ID=Fabrica runat=server DataSourceID="SqlDataSource3" DataTextField="Nume" DataValueField="ID" SelectedValue='<%# Bind("FabricaID") %>' Width="150px">
         </asp:DropDownList>  
         </ItemTemplate>
         <InsertItemTemplate>
         <asp:DropDownList   ID=Fabrica runat=server DataSourceID="SqlDataSource3" DataTextField="Nume" DataValueField="ID" SelectedValue='<%# Bind("FabricaID") %>' Width="150px">
         </asp:DropDownList>  
         </InsertItemTemplate>
            <EditItemTemplate>
         <asp:DropDownList   ID=Fabrica runat=server DataSourceID="SqlDataSource3" DataTextField="Nume" DataValueField="ID" SelectedValue='<%# Bind("FabricaID") %>' Width="150px">
         </asp:DropDownList>  
         </EditItemTemplate>
         <ControlStyle BackColor="White"  Font-Names="Arial" Font-Size="Small" />
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Judet" >
         <ItemTemplate>
          <asp:DropDownList    Enabled = false ID=Judete runat=server DataSourceID="SqlDataSource2" DataTextField="Denloc" DataValueField="ID"  SelectedValue='<%# Bind("JudetID") %>' Width="150px">
         </asp:DropDownList>  
         </ItemTemplate>
         <InsertItemTemplate>
         <asp:DropDownList   ID=Judete runat=server DataSourceID="SqlDataSource2" DataTextField="Denloc" DataValueField="ID"  SelectedValue='<%# Bind("JudetID") %>' Width="150px">
         </asp:DropDownList>  
         </InsertItemTemplate>
            <EditItemTemplate>
         <asp:DropDownList   ID=Judete runat=server DataSourceID="SqlDataSource2" DataTextField="Denloc" DataValueField="ID"  SelectedValue='<%# Bind("JudetID") %>' Width="150px">
         </asp:DropDownList>  
         </EditItemTemplate>
         <ControlStyle BackColor="White"  Font-Names="Arial" Font-Size="Small" />
         </asp:TemplateField>
         <asp:BoundField DataField="Email" HeaderText="Email" />
         <asp:BoundField DataField="Oras" HeaderText="Localitate" />
       

         <asp:BoundField DataField="Strada" HeaderText="Strada" />
         <asp:BoundField DataField="Numar" HeaderText="Numar" />
         <asp:BoundField DataField="CodPostal" HeaderText="Cod Postal" />   
         <asp:BoundField DataField="Telefon" HeaderText="Telefon"  />
          <asp:BoundField DataField="Fax" HeaderText="Fax"  />  
        
           <asp:BoundField DataField="PersonaDeContact" HeaderText="Pers. contact" />
           <asp:BoundField DataField="TelPersoanaContact" HeaderText="Tel. pers. contact" />
        
               
         
         <asp:TemplateField HeaderText="Ferme/CCL">
         <ItemTemplate>
          <p>
              <asp:DropDownList  Enabled = false ID="Ferme_CCL" runat="server" SelectedValue='<%# Bind("Ferme_CCL")%>'>
                  <asp:ListItem Value="F">Ferma</asp:ListItem>
                  <asp:ListItem Value="C">CCL</asp:ListItem>
              </asp:DropDownList></p> 
         </ItemTemplate>
             <EditItemTemplate>
                 <asp:DropDownList ID="Ferme_CCL" runat="server" SelectedValue='<%# Bind("Ferme_CCL")%>'>
                     <asp:ListItem Value="F">Ferma</asp:ListItem>
                     <asp:ListItem Value="C">CCL</asp:ListItem>
                 </asp:DropDownList>
             </EditItemTemplate>
             <InsertItemTemplate>
                 <asp:DropDownList ID="Ferme_CCL" runat="server" SelectedValue='<%# Bind("Ferme_CCL")%>'>
                     <asp:ListItem Value="F">Ferma</asp:ListItem>
                     <asp:ListItem Value="C">CCL</asp:ListItem>
                 </asp:DropDownList>
             </InsertItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField HeaderText="Send Sms">
    <ItemTemplate>
        <asp:CheckBox ID="SendSms" runat="server" Checked='<%# Bind("SendSms") %>' />
    </ItemTemplate>
</asp:TemplateField>

       
            <asp:BoundField DataField="FermierID" HeaderText="Fermier" Visible=False />
       
       
        <asp:CommandField ShowEditButton="True" ShowInsertButton="True" />
        <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen
			Text="Delete" OnClientClick="return confirm('Doriti stergerea fermei?');"></asp:LinkButton>
	    </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditareFerme.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
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
        &nbsp;&nbsp;<br />
        <br />
        <br />
        <br />
        &nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [Nume], [Cod], [FabricaID], [Strada], [Numar], [Oras], [Judet],[JudetID], [Telefon], [Email],[FermierID],[Ferme_CCL],[CodPostal],[Fax],[PersonaDeContact],[TelPersoanaContact],[SendSms] FROM [Ferme_CCL] WHERE ([ID] = @ID)" DeleteCommand="DELETE FROM [Ferme_CCL] WHERE [ID] = @ID" UpdateCommand="UPDATE [Ferme_CCL] SET [Nume] = @Nume, [FabricaID] = @FabricaID, [Judet] = Convert(nvarchar,@JudetID,2),[JudetID]=@JudetID,[Cod]=@Cod,[Strada]=@Strada,[Numar]=@Numar,[Oras]=@Oras,[Email]=@Email,[Telefon]=@Telefon,[Fax]=@Fax,[FermierID]=@FermierID,[Ferme_CCL]=@Ferme_CCL,[CodPostal]=@CodPostal,[PersonaDeContact]=@PersonaDeContact,[TelPersoanaContact]=@TelPersoanaContact,[SendSms]=@SendSms WHERE [ID] = @ID" InsertCommand="INSERT INTO FERME_CCL (Cod,FabricaID,Nume,Strada,Numar,Oras,Judet,JudetID,Telefon,Email,FermierID,Ferme_CCL,CodPostal,Fax,PersonaDeContact,TelPersoanaContact,SendSms) VALUES (@Cod,@FabricaID,@Nume,@Strada,@Numar,@Oras,Convert(nvarchar,@JudetID,2),@JudetID,@Telefon,@Email,0,@Ferme_CCL,@CodPostal,@Fax,@PersonaDeContact,@TelPersoanaContact,@SendSms)">
            <SelectParameters>
               <asp:QueryStringParameter Name="ID" QueryStringField="ID" />
            </SelectParameters>
            <DeleteParameters>
              <asp:Parameter Name="ID" Type="Int32" />   
            </DeleteParameters>
            <UpdateParameters>
            <asp:Parameter Name="FabricaID" Type=Int32 />
             <asp:Parameter Name="ID" Type=Int32 />
            <asp:Parameter Name="JudetID" Type=Int32 />
             <asp:Parameter Name="Cod" Type=String />
               <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Strada" Type=String />
             <asp:Parameter Name="Numar" Type=String />
            <asp:Parameter Name="Oras" Type=String />
            <asp:Parameter Name="Telefon" Type=String />
            <asp:Parameter Name="Email" Type=String />
            <asp:Parameter Name="CodPostal" Type=String />
            <asp:Parameter Name="Fax" Type=String />
            <asp:Parameter Name="PersonaDeContact" Type=String />
            <asp:Parameter Name="TelPersoanaContact" Type=String />
            <asp:Parameter Name="Ferme_CCL" Type=Char/>
            <asp:Parameter Name="FermierID" Type=Int32 />
            <asp:Parameter Name="SendSms" Type=Byte />             
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="FabricaID" Type=Int32 />
            <asp:Parameter Name="JudetID" Type=Int32 />
             <asp:Parameter Name="Cod" Type=String />
               <asp:Parameter Name="Nume" Type=String />
            <asp:Parameter Name="Strada" Type=String />
             <asp:Parameter Name="Numar" Type=String />
            <asp:Parameter Name="Oras" Type=String />
            <asp:Parameter Name="Telefon" Type=String />
            <asp:Parameter Name="Email" Type=String />
            <asp:Parameter Name="CodPostal" Type=String />
            <asp:Parameter Name="Fax" Type=String />
            <asp:Parameter Name="PersonaDeContact" Type=String />
            <asp:Parameter Name="TelPersoanaContact" Type=String />
            <asp:Parameter Name="Ferme_CCL" Type=Char/>
            <asp:Parameter Name="SendSms" Type=Byte /> 
            </InsertParameters>
        </asp:SqlDataSource>
        
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [Denloc] FROM [Judete] ORDER BY [Denloc]" >
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [ID], [Nume] FROM [Fabrici] ORDER BY [Nume]" >
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
            SelectCommand="SELECT [FermierID], [Nume] FROM [Fermier] ORDER BY [Nume]"></asp:SqlDataSource>
        <br />
    </asp:Panel>
</asp:Content>

