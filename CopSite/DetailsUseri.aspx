<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetailsUseri" Codebehind="DetailsUseri.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="UserName"
        DefaultMode="Edit" HeaderText="Detalii User"
        DataSourceID="ObjectDataSource1" Height="50px" Width="478px" 
        onitemdeleted="DetailsView1_ItemDeleted" 
        onitemupdated="DetailsView1_ItemUpdated">
        <Fields>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="UserName" HeaderText="User Name" 
                SortExpression="UserName" />
            <asp:BoundField DataField="LastLogin" HeaderText="Last Login" 
                SortExpression="LastLogin" />
            <asp:TemplateField HeaderText="Is Approved" SortExpression="IsApproved">
                <EditItemTemplate>
                <asp:CheckBox ID="IsApproved" runat="server" Checked='<%#(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsApproved")) == true) ? true:false%>' />
                </EditItemTemplate>
                <InsertItemTemplate>
                <asp:CheckBox ID="IsApproved" runat="server" Checked='<%#(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsApproved")) == true) ? true:false%>' />
                </InsertItemTemplate>
                <ItemTemplate>
                <asp:CheckBox  Enabled="false" ID="IsApproved" runat="server" Checked='<%#(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsApproved")) == true) ? true:false%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role"  ReadOnly=true/>
            <asp:TemplateField HeaderText="User Cod" SortExpression="UserCod">
                <EditItemTemplate>
                 <asp:DropDownList id="UserCod" runat="server"  DataTextField="Nume" DataValueField="Cod" SelectedValue='<%# Bind("UserCod") %>'
                        DataSourceID="SqlDataSource1"/>
                </EditItemTemplate>
                <InsertItemTemplate>
                                 <asp:DropDownList id="UserCod" runat="server"  DataTextField="Nume" DataValueField="Cod" SelectedValue='<%# Bind("UserCod") %>'
                        DataSourceID="SqlDataSource1"/>
                </InsertItemTemplate>
                <ItemTemplate>
                 <asp:DropDownList  Enabled = "false" id="UserCod" runat="server"  DataTextField="Nume" DataValueField="Cod" SelectedValue='<%# Bind("UserCod") %>'
                        DataSourceID="SqlDataSource1"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FirstName" HeaderText="First Name" 
                SortExpression="FirstName" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" 
                SortExpression="LastName" />
            <asp:BoundField DataField="Salutation" HeaderText="Salutation" 
                SortExpression="Salutation" />
            <asp:BoundField DataField="UserKey" HeaderText="UserKey" 
                SortExpression="UserKey" />
            <asp:CommandField ShowEditButton="True" ShowInsertButton="False" ShowDeleteButton="True" />
        <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen
			Text="Delete" OnClientClick="return confirm('Doriti stergerea userului?');"></asp:LinkButton>
	    </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField ShowHeader="False">
          <ItemTemplate>
          <asp:HyperLink ID="HyperLink1" runat=server  Text="Back" NavigateUrl="~/EditareUseri.aspx" Font-Bold=true Font-Size=Small Font-Names="Arial" ForeColor=SeaGreen>
          </asp:HyperLink>|
          </ItemTemplate>
          </asp:TemplateField>
        </Fields>
         <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
    </asp:DetailsView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"  DeleteMethod="GetUser"  UpdateMethod="UpdateUser"
        SelectMethod="GetUser" TypeName="MUser" DataObjectTypeName="MUser">
        <SelectParameters>
            <asp:QueryStringParameter Name="username" QueryStringField="username" 
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>" 
        SelectCommand="SELECT [Cod], [Nume] FROM [Ferme_CCL] Where ([Cod] <> '') ORDER BY [Nume]">
     
    </asp:SqlDataSource>
</asp:Content>

