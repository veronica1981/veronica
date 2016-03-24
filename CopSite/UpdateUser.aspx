<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="UpdateUser.aspx.cs" Inherits="CopSite.UpdateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="131px" Width="800px">
        <div>
            <br />
            <table cellpadding="5px" frame="box" style="border-style: groove; border-width: thin; height: 95px; width: 432px">
                <tr>
                    <td style="width: 190px">
                        <asp:Label ID="Label1" runat="server" Text="Editare User" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" ForeColor="SeaGreen"></asp:Label><br />
                        <br />
                    </td>
                </tr>
                <caption>
                    <br/>
                    <asp:HiddenField runat="server" ID="UserId"/>
                    <asp:HiddenField runat="server" ID="UserCod"/>
                    <tr>
                        <td>
                            <asp:Label runat="server">User Name</asp:Label>
                        </td>
                        <td style="width: 353px">
                            <asp:Label ID="UserName" runat="server" SkinID="blackv"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server">Nume :</asp:Label></td>
                        <td style="width: 353px">
                            <asp:TextBox ID="Nume" runat="server" Height="20px" Width="250px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server">Prenume :</asp:Label>
                        </td>
                        <td style="width: 353px">
                            <asp:TextBox ID="Prenume" runat="server" Height="20px" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td> <asp:Label runat="server">Email :</asp:Label></td>
                        <td style="width: 353px">
                            <asp:TextBox ID="Email" runat="server" Height="20px" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td> <asp:Label runat="server">Asociatie :</asp:Label></td>
                        <td style="width: 353px">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="Asociatie" runat="server" CssClass="gridCheckBox"/>
                        </td>
                    </tr>
                    <tr>
                        <td> <asp:Label runat="server">Cod User :</asp:Label></td>
                        <td style="width: 353px">
                            <asp:DropDownList ID="CodUser" runat="server" Height="20px" Width="250px" SelectedValue='<%# UserCod.Value %>'></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td> <asp:Label runat="server">Locked :</asp:Label></td>
                        <td style="width: 353px">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="Locked" runat="server" CssClass="gridCheckBox"/>
                        </td>
                    </tr>
                </caption>
                <tr>
                    <td>&nbsp;</td>
                    <td style="width: 353px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="UpdateBtn" runat="server" Text="Update" OnClick="UpdateBtn_Click" Width="108px" />
                    </td>
                    <td style="width: 353px">&nbsp;</td>
                </tr>
            </table>
            <br />
        </div>

    

    </asp:Panel>
</asp:Content>


