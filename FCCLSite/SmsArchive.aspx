<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="SmsArchive.aspx.cs" EnableEventValidation="false" Inherits="FCCLSite.SmsArchive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="ID" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" Width="80%" CssClass="centered">
        <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="10px" ItemStyle-Wrap="false"/>
            <asp:BoundField DataField="DateCreated" HeaderText="Data crearii" />
            <asp:BoundField DataField="CellNr" HeaderText="Numar de telefon" />
            <asp:BoundField DataField="Message" HeaderText="Mesaj" ItemStyle-Width="700px" ItemStyle-Wrap="false"/>
            <asp:BoundField DataField="TryNr" HeaderText="Numar de incercari" ItemStyle-Width="70px" ItemStyle-Wrap="false"/>
            
            
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton runat="server" CommandArguement='<%# Eval("Id")%>'  CommandName="Retrimitere" Text="Retrimitere"  OnClick="Retrimitere"/>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
        <PagerSettings PageButtonCount="20" />
        <RowStyle BackColor="CornflowerBlue" Font-Bold="False" Font-Names="Arial" Font-Size="9pt" BorderColor="White" BorderStyle="Solid" />
        <HeaderStyle BackColor="CornflowerBlue" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            ForeColor="White" BorderStyle="Solid" />
        <AlternatingRowStyle BackColor="PaleTurquoise" />
    </asp:GridView>
</asp:Content>
