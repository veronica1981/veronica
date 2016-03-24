<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Comenzi.aspx.cs" Inherits="Comenzi"  MasterPageFile="~/MasterPage.master" EnableTheming="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<table style="width: 90%;">
		<tr>
			<td>
				Data Primirii de la:
			</td>
			<td>
				<asp:TextBox ID="txtRDStart" runat="server"></asp:TextBox>
			</td>
			<td>
				Data Primirii pana la:
			</td>
			<td>
				<asp:TextBox ID="txtRDEnd" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Numar Comanda de la:
			</td>
			<td>
				<asp:TextBox ID="txtONStart" runat="server"></asp:TextBox>
			</td>
			<td>
				Numar Comanda pana la:
			</td>
			<td>
				<asp:TextBox ID="txtONEnd" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Nume Client:
			</td>
			<td>
				<asp:TextBox ID="txtCN" runat="server"></asp:TextBox>
			</td>
			<td>
				<asp:Button id="btnFilterApply" runat="server" Text="Filtreaza" OnClick="btnFilterApply_Click"/>
			</td>
			<td>
				<asp:Button id="btnFilterClear" runat="server" Text="Anuleaza Filtre" OnClick="btnFilterClear_Click"/>
			</td>
		</tr>
		<tr>
			<td colspan="4">
				<asp:Button id="btnExportToExcel" runat="server" Text="Export in Excel" OnClick="btnExportToExcel_Click"/>
			</td>
		</tr>
		<tr>
			<td colspan="4">
				<asp:GridView ID="grdOrders" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="Id"
					PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" 
					Width="100%" AllowCustomPaging="True" OnPageIndexChanging="grdOrders_PageIndexChanging">
					<PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
					<Columns>
						<asp:BoundField DataField="OrderNumber" HeaderText = "Numar Comanda" /> 
						<asp:BoundField DataField="FullOrderNumber" HeaderText = "Serie Comanda" /> 
						<asp:BoundField DataField="ClientName" HeaderText = "Client" /> 
						<asp:TemplateField SortExpression="SampleDate" HeaderText = "Data Prelevare"><ItemTemplate><%#DateTime.Parse(DataBinder.Eval(Container.DataItem, "SampleDate").ToString()).ToString("dd/MM/yyyy")%></ItemTemplate></asp:TemplateField>	
						<asp:TemplateField SortExpression="ReceiveDate" HeaderText = "Data Primire"><ItemTemplate><%#DateTime.Parse(DataBinder.Eval(Container.DataItem, "ReceivedDate").ToString()).ToString("dd/MM/yyyy")%></ItemTemplate></asp:TemplateField>	
						<asp:BoundField DataField="SampleCount" HeaderText = "Numar mostre" />
						<asp:BoundField DataField="AnalyzedSampleCount" HeaderText = "Numar mostre Importate" /> 
						<asp:BoundField DataField="Imported" HeaderText = "Importat" /> 
					</Columns>
					<PagerSettings PageButtonCount="20" />
					<RowStyle BackColor="#009999" Font-Bold="False" Font-Names="Arial" Font-Size="9pt" BorderColor="White" BorderStyle="Solid" />
					<HeaderStyle BackColor="#3CB371" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
						ForeColor="White" BorderStyle="Solid" />
					<AlternatingRowStyle BackColor="#99FF99" />
				</asp:GridView>
			</td>
		</tr>
	</table>
	<script type="text/javascript" language="javascript">
		$(window).load(function () {
			$('#<%=txtRDStart.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
			$('#<%=txtRDEnd.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
		});
	</script>
</asp:Content>