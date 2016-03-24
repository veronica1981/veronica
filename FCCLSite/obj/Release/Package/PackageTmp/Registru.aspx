<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registru.aspx.cs" Inherits="Registru"  MasterPageFile="~/MasterPage.master" EnableTheming="true"%>
<%@ Import Namespace="FCCL_DAL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<table style="width: 90%;">
		<tr>
			<td>
				Data Test de la:
			</td>
			<td>
				<asp:TextBox ID="txtTDStart" runat="server"></asp:TextBox>
			</td>
			<td>
				Data Test pana la:
			</td>
			<td>
				<asp:TextBox ID="txtTDEnd" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Numar Raport de la:
			</td>
			<td>
				<asp:TextBox ID="txtRNStart" runat="server"></asp:TextBox>
			</td>
			<td>
				Numar Raport pana la:
			</td>
			<td>
				<asp:TextBox ID="txtRNEnd" runat="server"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				Nume Ferma:
			</td>
			<td>
				<asp:TextBox ID="txtON" runat="server"></asp:TextBox>
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
				<asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="Id"
					PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" 
					Width="100%" AllowCustomPaging="True" OnPageIndexChanging="grdReports_PageIndexChanging">
					<PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
					<Columns>
						<asp:BoundField DataField="ReportNumber" HeaderText = "Numar Raport" /> 
						<asp:TemplateField HeaderText="Tip Raport" SortExpression="ReportType" >
								<ItemTemplate>
									<label><%# (FCCLReportType)int.Parse(DataBinder.Eval(Container.DataItem, "ReportType").ToString())%></label>
								</ItemTemplate>
						</asp:TemplateField>			
						<asp:BoundField DataField="ObjectName" HeaderText = "Ferma" /> 
						<asp:TemplateField SortExpression="TestDate" HeaderText = "Data Test"><ItemTemplate><%#DateTime.Parse(DataBinder.Eval(Container.DataItem, "TestDate").ToString()).ToString("dd/MM/yyyy")%></ItemTemplate></asp:TemplateField>	
						<asp:TemplateField SortExpression="PrintDate" HeaderText = "Data Printare"><ItemTemplate><%#DateTime.Parse(DataBinder.Eval(Container.DataItem, "PrintDate").ToString()).ToString("dd/MM/yyyy")%></ItemTemplate></asp:TemplateField>	
						<asp:BoundField DataField="PageCount" HeaderText = "Numar Pagini" /> 
						<asp:BoundField DataField="SampleCount" HeaderText = "Numar mostre" /> 
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
			$('#<%=txtTDStart.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
			$('#<%=txtTDEnd.ClientID%>').datepicker({ dateFormat: 'dd/mm/yy', firstDay: 1 });
		});
	</script>
</asp:Content>