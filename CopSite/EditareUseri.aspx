<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditareUseri" CodeBehind="EditareUseri.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server" Width="90%">
        <table style="width: 100%" cellspacing="0" cellpadding="5">
            <tr>
                <td style="height: 32px"></td>
                <td style="height: 32px"></td>
                <td style="height: 32px;"></td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="pwd" runat="server"></asp:Literal>
                </td>
                <td>&nbsp;</td>
                <td align="right">&nbsp;</td>
            </tr>

            <tr bgcolor="mediumseagreen">
                <td>
                    <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" SkinID="black"></asp:Label>
                </td>
                <td></td>

            </tr>
            <tr>
                <td style="height: 21px"></td>
                <td style="height: 21px;"></td>

            </tr>
        </table>

    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Width="90%">

        <asp:Repeater ID="UserGrid" runat="server" OnItemCommand="UserGrid_ItemCommand">
            <HeaderTemplate>
                <table id="tableusers" cellspacing="2" cellpadding="2" width="100%">
                    <tr bgcolor="#00D269" class="heading">
                        <td style="height: 36px;">UserName</td>
                        <td style="height: 36px;">Asoc.</td>
                        <td style="height: 36px;">UserCod</td>
                        <td style="height: 36px;">Email</td>
                        <td style="height: 36px;">Locked</td>
                        <td style="height: 36px;">Nume</td>
                        <td style="height: 36px;">Prenume</td>
                        <td style="height: 36px;">&nbsp;</td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr bgcolor="#3CB371" class="data">
                    <td style="width: 15%; height: 33px;">
                        <asp:Label ID="UserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>' SkinID="blackv"></asp:Label></td>
                    <td style="width: 5%; height: 33px;" class="gridCheckBox">
                        <asp:CheckBox ID="IsAsoc" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "IsAsoc") %>'></asp:CheckBox>
                    </td>
                    <td style="width: 15%; height: 33px;">
                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UserCod") %>' SkinID="blackv"></asp:Label>
                    </td>
                    <td style="width: 15%; height: 33px;">
                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>' SkinID="blackv"></asp:Label>
                    </td>
                    <td style="width: 5%; height: 33px;" class="gridCheckBox">
                        <asp:CheckBox ID="IsLocked" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "IsLockedOut") %>'></asp:CheckBox></td>

                    <td style="width: 15%; height: 33px;">
                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>' SkinID="blackv"></asp:Label>
                    </td>
                    <td style="width: 15%; height: 33px;">
                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>' SkinID="blackv"></asp:Label>
                    </td>


                    <td style="width: 15%; height: 33px;">
                        
                        <asp:HyperLink Text="Editare" NavigateUrl='<%# String.Format("UpdateUser.aspx?UserName={0}",Eval("UserName")) %>' runat="server" SkinID="black"/>

                        &nbsp;&nbsp;
                        <asp:LinkButton CommandName="Resetare" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserName") %>' runat="server"
                            SkinID="black">Resetare</asp:LinkButton>
                        &nbsp;&nbsp;
                             <asp:LinkButton CommandName="Stergere" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserName") %>' runat="server"
                                 SkinID="black" OnClientClick="return confirm('Doriti stergerea userului?');">Stergere</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <br />
        <asp:Button ID="linkUpdate" runat="server" OnClick="linkUpdate_Click" Text="Update"
            ToolTip="Start Page" />


        <div id="dialog" style="display: none">
        </div>
    </asp:Panel>

<%--    <script type="text/javascript">
        $(function () {

            var url = "UpdateUser.aspx";

                $("#dialog").dialog({
                    title: "Editare useri",
                    modal: true,
                    autoOpen: false,
                    height: 350,
                    width: 540,
                    open: function () {
                        $(this).load(url);
                    },
                    buttons: {
                        Save: function () {

                        },
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    close: function (e) {
                        $(this).empty();
                        $(this).dialog('destroy');
                    },
                });
        });

        function OpenDialog(userName) {
            $('#dialog').dialog('open');


        }
    </script>--%>
</asp:Content>

