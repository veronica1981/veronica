<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="ReportCrot" Codebehind="ReportCrot.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <span class="failureNotification">
    <br />
    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
    <br />
    <br />
    </span>
</asp:Content>

