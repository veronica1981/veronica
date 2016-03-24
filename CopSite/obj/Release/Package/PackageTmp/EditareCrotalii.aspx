<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditareCrotalii" Title="Crotalii" EnableTheming="true" Codebehind="EditareCrotalii.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<script type="text/javascript">
		$(window).load(function () {
			$('#divLogDialogue').dialog({ width: ($(window).width() * 80) / 100, height: ($(window).height() * 80) / 100 });
			$('#divLogDialogue').dialog('close');
			$('#btnTag').click(function () {
				retrieveTagLog();
			});
		});

		function retrieveTagLog() {
			$.ajax({
				async: true,
				type: "POST",
				url: "EditareCrotalii.aspx/GetTodaysTagLog",
				data: "{}",
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				success: function (data) {
					$('#txtLog').val(data.d);
					$('#divLogDialogue').dialog('open');
				},
				error: function (xhr, status, err) {
					alert(status + ' ' + err + ' ' + xhr);
				}
			});
		}
	</script>
	 <asp:Panel ID="Panel1" runat="server" Width="90%">
        <table  cellspacing=0 cellpadding=5 width="100%">
            <tr>
                <td style="height: 32px">
                </td>
                <td style="height: 32px">
					<input id="btnTag" type="button" value="Log update Crotalii <%=DateTime.Now.ToString("dd/MM/yy")%>" />
                </td>
                <td style="height: 32px;">
                </td>
            </tr>
            <tr>
                <td style="height: 35px;">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Crotalia:"></asp:Label></td>
                <td style="height: 35px;">
                    <asp:TextBox ID="Crotalia" runat="server" AutoPostBack="True" OnTextChanged="Crotalia_TextChanged"></asp:TextBox></td>
                <td align="right" style="height: 35px;">
                    </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                        ForeColor="SeaGreen" Text="Cod:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="Ferma" runat="server" AutoPostBack="True" OnTextChanged="Ferma_TextChanged"></asp:TextBox></td>
                <td align="right">
                    <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" 
                        Font-Bold="True" Font-Names="Arial" Font-Size="Medium" OnClick="Button1_Click" 
                        Text="Cautare" Width="112px" />
                </td>
            </tr>
            <tr >
            <td>
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
                    ForeColor="SeaGreen" Text="Fisier crotalii:"></asp:Label>
                &nbsp;<td align="justify">
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="255px" />
                </td>
           <td align="left" style="width: 370px">
                    <asp:Button ID="Button2" runat="server" BackColor="MediumSeaGreen" 
                        Font-Bold="True" Font-Names="Arial" Font-Size="Medium" OnClick="Button2_Click" 
                        Text="Import" Width="112px" />
                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button3" runat="server" BackColor="MediumSeaGreen" 
                        Font-Bold="True" Font-Names="Arial" Font-Size="Medium" OnClick="Button3_Click" 
                        Text="Import BV" Width="112px" />
                </td>
           
            </tr>
            <tr>
            <td>
              
                </td>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                    ForeColor="SeaGreen" NavigateUrl="~/DetailsCrotalii.aspx">Adaugare</asp:HyperLink></td>
            <td></td>

            </tr>
            <tr bgcolor=mediumseagreen>
             <td>
                  <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" 
                    Font-Size="Medium" SkinID="black" Width="129px"></asp:Label></td>
           <td style="height: 21px"></td>
           <td style="height: 21px;"></td>
           
            </tr>
             <tr>
             <td colspan=3 style="height: 15px"></td>
             </tr>
        </table>
    
    </asp:Panel>

  
  
  
    <asp:Panel ID="PanelE" runat="server" Width="90%">
           
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="crotalia" OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing"  Width="100%">
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
        <Columns>
         <asp:BoundField DataField="codbare" HeaderText="Cod"  />                  
          <asp:BoundField DataField="crotalia" HeaderText="Crotalia" />
          <asp:BoundField DataField="nume" HeaderText="Nume" />
         <asp:BoundField DataField="datanasterii" HeaderText="Data nasterii" />
         <asp:BoundField DataField="rasa" HeaderText="Rasa" />
         <asp:BoundField DataField="ferma" HeaderText="Ferma" />
          
       
            <asp:HyperLinkField HeaderText="Detalii..." Text="Detalii..." DataNavigateUrlFields="crotalia"
            DataNavigateUrlFormatString="DetailsCrotalii.aspx?Crotalia={0}" />   
            
   
        </Columns>
            <PagerSettings PageButtonCount="20" />
        
        </asp:GridView>
        &nbsp;&nbsp;
        
</asp:Panel>  
<div id="divLogDialogue" style="padding:0 !important; overflow: hidden;">
		<textarea id="txtLog" readonly="readonly" style="width: 100%; height: 100%;"></textarea>
</div>
 
</asp:Content>

