using System.Web.UI;
using FCCL_BL.Managers;
using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

public partial class Comenzi : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			BindData();
		}
	}

	protected void BindData()
	{
		int count;
		grdOrders.DataSource = GetData(true, out count);
		grdOrders.VirtualItemCount = count;
		grdOrders.DataBind();
	}

	protected void grdOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		grdOrders.PageIndex = e.NewPageIndex;
		BindData();
	}

	protected void btnFilterClear_Click(object sender, EventArgs e)
	{
		txtRDStart.Text = string.Empty;
		txtRDEnd.Text = string.Empty;
		txtONStart.Text = string.Empty;
		txtONEnd.Text = string.Empty;
		txtCN.Text = string.Empty;
		BindData();
	}

	protected void btnFilterApply_Click(object sender, EventArgs e)
	{
		BindData();
	}

	protected void btnExportToExcel_Click(object sender, EventArgs e)
	{
		Response.Clear();
		Response.Buffer = true;
		Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
		Response.Charset = "";
		Response.ContentType = "application/vnd.ms-excel";
		StringBuilder sb = new StringBuilder();

		sb.Append("<table><tr><td>Nr. Comanda</td><td>Client</td><td>Numar Probe</td><td>Data Prelevare</td><td>Data Primirii</td></tr>");
		int count;
		foreach (Order order in GetData(false, out count))
		{
			sb.Append("<tr>");
			sb.Append("<td>" + order.OrderNumber + "</td>");
			sb.Append("<td>" + order.ClientName + "</td>");
			sb.Append("<td>" + order.SampleCount + "</td>");
			sb.Append("<td>" + order.SampleDate.ToString("dd/MM/yyyy") + "</td>");
			sb.Append("<td>" + order.ReceivedDate.ToString("dd/MM/yyyy") + "</td>");
			sb.Append("</tr>");
		}
		sb.Append("</table>");
		//style to format numbers to string
		string style = @"<style> .textmode { } </style>";
		Response.Write(style);
		Response.Output.Write(sb.ToString());
		Response.Flush();
		Response.End();
	}

	private List<Order> GetData(bool paged, out int count)
	{
		List<Order> orders = null;
		OrderManager oManager = new OrderManager(StaticDataHelper.FCCLDbContext);

		#region filter
		DateTime date;
		long no;
		DateTime? receiveDateStart = null;
		DateTime? receiveDateEnd = null;
		long? orderNoStart = null;
		long? orderNoEnd = null;

		if (!string.IsNullOrWhiteSpace(txtRDStart.Text))
		{
			if (DateTime.TryParseExact(txtRDStart.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				receiveDateStart = date;
			}
			else
			{
				receiveDateStart = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(txtRDEnd.Text))
		{
			if (DateTime.TryParseExact(txtRDEnd.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				receiveDateEnd = date;
			}
			else
			{
				receiveDateEnd = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(txtONStart.Text))
		{
			if (long.TryParse(txtONStart.Text, out no))
			{
				orderNoStart = no;
			}
			else
			{
				orderNoStart = null;
			}
		}
		if (!string.IsNullOrWhiteSpace(txtONEnd.Text))
		{
			if (long.TryParse(txtONEnd.Text, out no))
			{
				orderNoEnd = no;
			}
			else
			{
				orderNoEnd = null;
			}
		}
		#endregion

		if (paged)
		{
			orders = oManager.GetOrdersPaged(receiveDateStart, receiveDateEnd, orderNoStart, orderNoEnd, txtCN.Text, grdOrders.PageIndex, grdOrders.PageSize);
			count = oManager.GetOrdersCount(receiveDateStart, receiveDateEnd, orderNoStart, orderNoEnd, txtCN.Text);
		}
		else
		{
			orders = oManager.GetOrdersFiltered(receiveDateStart, receiveDateEnd, orderNoStart, orderNoEnd, txtCN.Text);
			count = orders.Count;
		}
		return orders;
	}
}
