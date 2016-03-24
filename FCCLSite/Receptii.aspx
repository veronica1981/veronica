<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Receptii.aspx.cs" Inherits="Receptii" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="width: 90%;">
		<tr>
			<td>
				Data de la:
			</td>
			<td>
				<asp:TextBox ID="txtDateStart" runat="server"></asp:TextBox>
			</td>
			<td>
				Data pana la:
			</td>
			<td>
				<asp:TextBox ID="txtDateEnd" runat="server"></asp:TextBox>
			</td>
		</tr>
		
		<tr>
			<td>
				<asp:Button ID="btnGetReports" runat="server" Text="Exporta" OnClick="btnGetReports_Click"/>
			</td>
		</tr>
	</table>

	<script type="text/javascript" language="javascript">
		$(window).load(function () {
			$('#<%=txtDateStart.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
			$('#<%=txtDateEnd.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
		});
	</script>

</asp:Content>
