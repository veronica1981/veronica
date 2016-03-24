<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditareFerme" Title="Ferme" EnableTheming="true" CodeBehind="EditareFerme.aspx.cs" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager runat="server" EnablePageMethods="true" />
    <asp:Panel ID="Panel1" runat="server" Width="90%">
        <table cellspacing="0" cellpadding="5" width="100%">
            <tr>
                <td style="height: 32px"></td>
                <td style="height: 32px"></td>
                <td style="height: 32px;"></td>
            </tr>
            <tr>
                <td style="height: 35px;">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Nume Asociatie:"></asp:Label></td>
                <td style="height: 35px;">
                    <asp:TextBox ID="Fabrica" runat="server"></asp:TextBox>
                </td>
                <td align="right" style="height: 35px;"></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Nume Ferma:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="Ferma" runat="server"></asp:TextBox></td>
                
            </tr>
            <tr>
                <td style="height: 35px;">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Cod:"></asp:Label></td>
                <td style="height: 35px;">
                    <asp:TextBox ID="Code" runat="server"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen"
                        Font-Bold="True" Font-Names="Arial" Font-Size="Medium" OnClick="Button1_Click"
                        Text="Cautare" Width="112px" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td></td>
                <td align="right">&nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                        ForeColor="SeaGreen" NavigateUrl="~/DetailsFerme.aspx">Adaugare</asp:HyperLink></td>
                <td></td>

            </tr>
            <tr bgcolor="mediumseagreen">
                <td>
                    <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial"
                        Font-Size="Medium" SkinID="black" Width="129px"></asp:Label></td>
                <td style="height: 21px"></td>
                <td style="height: 21px;"></td>

            </tr>
            <tr>
                <td colspan="3" style="height: 15px"></td>
            </tr>
        </table>
    </asp:Panel>


    <asp:Panel ID="PanelE" runat="server" Width="90%">

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ID" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" Width="100%">
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" />
                <asp:BoundField DataField="cod" HeaderText="Cod" />
                <asp:BoundField DataField="nume" HeaderText="Nume" />
                <asp:BoundField DataField="strada" HeaderText="Strada" />
                <asp:BoundField DataField="numar" HeaderText="Numar" />
                <asp:BoundField DataField="oras" HeaderText="Oras" />
                <asp:BoundField DataField="judet" HeaderText="Judet" />
                <asp:BoundField DataField="telefon" HeaderText="Telefon" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:BoundField DataField="fabricaid" HeaderText="Asociatia" />


                <asp:HyperLinkField HeaderText="Detalii..." Text="Detalii..." DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="DetailsFerme.aspx?ID={0}" />


            </Columns>
            <PagerSettings PageButtonCount="20" />

        </asp:GridView>
        &nbsp;&nbsp;
        
    </asp:Panel>
    <script type="text/javascript" language="javascript">
        $(function () {
            $('#<%=Ferma.ClientID%>').autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "EditareFerme.aspx/AutocompleteFarms",
                        data: "{ farmName:'" + request.term + "'  }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item }
                            }));
                        },
                        error: function (xhr, status, err) {
                            alert(status + ' ' + err + ' ' + xhr);
                        }
                    });
                },
                select: function (event, ui) {
                    $('#<%=Ferma.ClientID%>').val(ui.item.value);
                    $(this).closest("form").submit();
                }
            });

            $('#<%=Fabrica.ClientID%>').autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "EditareFerme.aspx/AutocompleteFactories",
                        data: "{ factoryName:'" + request.term + "'  }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        minLenght: 2,
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item }
                            }));
                        },
                        error: function (xhr, status, err) {
                            alert(status + ' ' + err + ' ' + xhr);
                        }
                    });
                },
                select: function (event, ui) {
                    $('#<%=Fabrica.ClientID%>').val(ui.item.value);
                    $(this).closest("form").submit();
                }
            });

            $('#<%=Code.ClientID%>').autocomplete({
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "EditareFerme.aspx/AutocompleteCode",
                        data: "{ code:'" + request.term + "'  }",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        minLenght: 2,
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item }
                            }));
                        },
                        error: function (xhr, status, err) {
                            alert(status + ' ' + err + ' ' + xhr);
                        }
                    });
                },
                select: function (event, ui) {
                    $('#<%=Code.ClientID%>').val(ui.item.value);
                    $(this).closest("form").submit();
                }
            });
        });


    </script>
</asp:Content>

