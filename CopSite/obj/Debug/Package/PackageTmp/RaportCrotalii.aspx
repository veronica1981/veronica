<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="RaportCrotalii" Title="Raport Ferma" EnableTheming="true" CodeBehind="RaportCrotalii.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<table style="width: 800px">
		<tr>
			<td style="width: 122px; height: 17px"></td>
			<td style="width: 191px; height: 17px"></td>
			<td style="width: 206px; height: 17px">&nbsp;</td>
			<td style="height: 17px"></td>
		</tr>
		<tr>
			<td style="width: 122px">
				<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="MediumSeaGreen" Text="Data testarii:"></asp:Label></td>
			<td style="width: 191px">
				<asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox></td>
			<td style="width: 206px">
				<asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="MediumSeaGreen" Text="Data emiterii buletinului:"></asp:Label></td>
			<td>
				<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
		</tr>
		<tr>
			<td colspan="4"></td>
		</tr>
		<tr>
			<td style="width: 122px">
				<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="MediumSeaGreen" Text="Ferma:"></asp:Label></td>
			<td style="width: 191px">
				<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Nume" DataValueField="ID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
				</asp:DropDownList></td>
			<td style="width: 206px">
				<asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="MediumSeaGreen" Text="Nr. buletinului:"></asp:Label></td>
			<td>
				<asp:TextBox ID="TextBox3" runat="server" autocomplete="off"></asp:TextBox></td>
		</tr>
		<tr>
			<td id="divDisplay" colspan="4">

			</td>
		</tr>
		<tr>
			<td style="width: 122px">
				<asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="MediumSeaGreen" Text="Sef laborator:"></asp:Label></td>
			<td style="width: 191px">
				<asp:DropDownList ID="ddlLaborator" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nume" DataValueField="ID">
				</asp:DropDownList></td>
			<td style="width: 206px">
				<asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="MediumSeaGreen" Text="Responsabil incercare/analiza:"></asp:Label></td>
			<td>
				<asp:DropDownList ID="ddlResponsabil" runat="server" DataSourceID="SqlDataSource2" DataTextField="Nume" DataValueField="ID">
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td colspan="4"></td>
		</tr>
		<tr>
			<td style="width: 122px; height: 21px;"></td>
			<td style="width: 191px; height: 21px;"></td>
			<td style="width: 206px; height: 21px;"></td>
			<td style="height: 21px"></td>
		</tr>
		<tr>
			<td colspan="4"></td>
		</tr>
		<tr>

			<td align="center" colspan="4" bgcolor="MediumSeaGreen">
				<asp:Button ID="Button1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					Text="Raport PDF&Excel" OnClick="Button1_Click" /></td>

		</tr>
		<tr>
			<td colspan="4"></td>
		</tr>
		<tr>
			<td colspan="4">&nbsp;</td>
		</tr>
		<tr>

			<td colspan="2" align="center">
				<asp:HyperLink ID="pdflink" runat="server" Font-Bold="True" Font-Names="Arial"
					Font-Size="Small" ForeColor="CornflowerBlue">[pdflink]</asp:HyperLink></td>

			<td colspan="2"></td>
		</tr>
		<tr>
			<td colspan="4"></td>
		</tr>
		<tr>
			<td colspan="2"></td>
			<td colspan="2" align="center">
				<asp:HyperLink ID="xlslink" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="CornflowerBlue">[xlslink]</asp:HyperLink></td>

		</tr>
		<tr>
			<td colspan="2" align="center">
				<asp:HyperLink ID="csvlink" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
					ForeColor="CornflowerBlue">[csvlink]</asp:HyperLink></td>
			<td colspan="2"></td>

		</tr>

	</table>
	<asp:Literal ID="infomess" runat="server"></asp:Literal>
	<br />
	<br />
	<asp:Label ID="FermaId" runat="server" Visible="False"></asp:Label>
	<asp:Label ID="Username" runat="server" Visible="False"></asp:Label>
	<asp:Label ID="FermaName" runat="server" Visible="False"></asp:Label>
	<asp:Label ID="FermaCod" runat="server" Visible="False"></asp:Label>
	<br />
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
		SelectCommand="SELECT  DISTINCT Ferme_CCL.ID, Ferme_CCL.Nume FROM Ferme_CCL,MostreTancuri Where Ferme_CCL.ID = MostreTancuri.FermaID  &#13;&#10;AND MostreTancuri.Validat =1 AND CONVERT(datetime, MostreTancuri.DataTestareFinala, 103)  = CONVERT(datetime,  @datatestare, 103) ORDER BY Ferme_CCL.Nume">
		<SelectParameters>
			<asp:ControlParameter ControlID="TextBox1" Name="datatestare" PropertyName="Text" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
		SelectCommand="SELECT [ID], [Nume] FROM [Semnaturi] ORDER BY [Nume]"></asp:SqlDataSource>
	&nbsp;<br />
	<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMostreFabrica"
		TypeName="MostreFabrica">
		<SelectParameters>
			<asp:Parameter DefaultValue="" Name="fabricaid" Type="String" />
			<asp:Parameter DefaultValue="" Name="datatestare" Type="DateTime" />
		</SelectParameters>
	</asp:ObjectDataSource>
	&nbsp; &nbsp;<br />
	<br />

	<script type="text/javascript" language="javascript">
		$(window).load(function () {
			$('#<%=TextBox3.ClientID%>').keyup(function () {
				var txtId = '<%=TextBox3.ClientID%>';
				setTimeout("retrieveReport('" + $('#' + txtId).val().trim() + "','" + txtId + "');", 500);
			});

			$('#<%=TextBox1.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
			$('#<%=TextBox2.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
		});

		function retrieveReport(callData, txtId) {
			var currVal = $('#' + txtId).val().trim();
			if (currVal != '') {
				if (callData == currVal) {
					data = { reportNumber: currVal };
					$.ajax({
						type: "POST",
						url: "Registru.aspx/GetReport",
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						data: JSON.stringify(data),
						success: function (resp) {
							$('#divDisplay').text(resp.d.displayTxt);
						},
						error: function (XmlHttpError, error, description) {
							miError = JSON.parse(XmlHttpError.responseText);
							alert(miError.Message);
						}
					});
				}
			} else {
				$('#divDisplay').text('');
			}
		}
	</script>
</asp:Content>

