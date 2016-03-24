<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EditareMostre" Title="Editare Mostre" EnableTheming="true" Codebehind="EditareMostre.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<script type="text/javascript">
		$(window).load(function () {
			$('#divLogDialogue').dialog({ width: ($(window).width() * 80) / 100, height: ($(window).height() * 80) / 100 });
			$('#divLogDialogue').dialog('close');
			$('#btnReceptie').click(function () {
				retrieveReceptionLog();
			});
			$('#btnRezultate').click(function () {
				retrieveResultsLog();
			});
		});

		function retrieveReceptionLog() {
			$.ajax({
				async: true,
				type: "POST",
				url: "EditareMostre.aspx/GetTodaysReceptionLog",
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
		function retrieveResultsLog() {
			$.ajax({
				async: true,
				type: "POST",
				url: "EditareMostre.aspx/GetTodaysResultsLog",
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
        <table style="width: 100%"  cellspacing="0" cellpadding="3">
           <tr>
           <td style="width: 35%"></td>
           <td style="width: 15%"><input id="btnReceptie" type="button" value="Log import Receptie <%=DateTime.Now.ToString("dd/MM/yy")%>" /></td>
           <td style="width: 15%"><input id="btnRezultate" type="button" value="Log import Rezultate <%=DateTime.Now.ToString("dd/MM/yy")%>" /></td>
           <td style="width: 40%"></td>
           </tr>
           <tr>
           <td colspan=3 style="height: 28px">
               <asp:FileUpload ID="FileUpload1" runat="server" Width="410px" />
               <asp:Button ID="importm" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                   Font-Names="Arial" Font-Size="Small" Text="Import Manual" 
                   OnClick="importm_Click" Width="150px"/></td>
          
           <td style="height: 28px">
               &nbsp;</td>
           </tr>
           <tr>
           <td colspan=3  style="height: 28px">
               <asp:FileUpload ID="FileUpload2" runat="server" Width="410px" />
               <asp:Button ID="importp" runat="server" 
                   BackColor="#993399" Font-Bold="True" SkinID="violet"
                   Font-Names="Arial" Font-Size="Small" Text="Verificare/Import BV" 
                   OnClick="importp_Click" Width="150px"  />
            </td>
           <td align="left" style="height: 31px;">
               </td>
           </tr>
           <tr>
           <td></td>
           <td>
               <asp:Button ID="importap" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                   Font-Names="Arial" Font-Size="Small" Text="Import Aparate" 
                   OnClick="importap_Click" Width="140px" /></td>
           <td align="center" >
               &nbsp;</td>
           <td>
               <asp:Button ID="importa" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                   Font-Names="Arial" Font-Size="Small" Text="Import Automat" 
                   OnClick="importa_Click" Width="140px" /></td>
           </tr>
           <tr>
           <td colspan=4></td>
           </tr>

            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        ForeColor="SeaGreen" Text="Cod Bare:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="cod" runat="server" Width="120px"></asp:TextBox></td>
                <td>
               	<asp:RegularExpressionValidator ID="regCod" runat="server"   Font-Size=8 ControlToValidate="cod"
            ErrorMessage="Codul trebuie sa fie numeric <= 10 pozitii!!"   ValidationExpression="\d{1,10}"></asp:RegularExpressionValidator> 
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        ForeColor="SeaGreen" Text="Data testare initiala:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="datatest1" runat="server" Width="120px"></asp:TextBox></td>
                <td align="left">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        ForeColor="SeaGreen" Text="Data testare finala:"></asp:Label></td>
            <td>
                <asp:TextBox ID="datatest2" runat="server" Width="120px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="height: 34px;">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        ForeColor="SeaGreen" Text="NCS initial:"></asp:Label></td>
                <td style="height: 34px;">
                    <asp:TextBox ID="ncs1" runat="server" Width="120px"></asp:TextBox></td>
                <td style="height: 34px;">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        ForeColor="SeaGreen" Text="NCS final:"></asp:Label></td>
                <td style="height: 34px; ">
                    <asp:TextBox ID="ncs2" runat="server" Width="120px"></asp:TextBox></td>
            </tr>
            <tr>
            <td style="height: 10px;">
                &nbsp;</td>
            <td style="height: 10px;">
                &nbsp;</td>
            <td style="height: 10px;">
                &nbsp;</td>
            <td style="height: 10px;">
                &nbsp;</td>
            </tr>
            <tr>
            <td style="height: 25px;"></td>
            <td style="height: 25px;">
                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="DateValidate"
                    ControlToValidate="datatest1" ErrorMessage="Data invalida!" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
            </td>
            <td style="height: 25px;"></td>
            <td style="height: 25px;">
                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="DateValidate"
                    ControlToValidate="datatest2" ErrorMessage="Data invalida!" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator></td>
            </tr>
           <tr>
           <td colspan="4"></td>
           </tr>
            <tr>
            <td style="height: 30px; background-color: mediumseagreen;">
			   <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" SkinID=black
				   Width="129px"></asp:Label>
            </td>
           <td style="height: 30px;"></td>
           <td style="height: 30px;"><asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                        Font-Names="Arial" Font-Size="Small" OnClick="Button1_Click" Text="Cautare" Width="112px" /></td>
           <td style="height: 30px;"><asp:Button ID="Button2" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                        Font-Names="Arial" Font-Size="Small" OnClick="Button2_Click" Text="Anulare filtre" Width="112px" /></td>
            </tr>
            <tr>
             <td style="height: 15px;"></td>
           <td style="height: 15px;" align="center">
               <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
                   ForeColor="SeaGreen" NavigateUrl="~/DetaliuMostra.aspx">Adaugare</asp:HyperLink></td>
           <td style="height: 15px;"></td>
           <td style="height: 15px"></td>
            </tr>


        </table>
    
    </asp:Panel>

 
    <asp:Panel ID="PanelE" runat="server" Width="90%">
      
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True"  DataKeyNames="codbare"
			 OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="200" CellPadding="2" CellSpacing="1" BorderColor="White" BorderStyle="Solid" GridLines="Vertical" 
			OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing"  Width="100%" AllowCustomPaging="True">
            <PagerStyle Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="SeaGreen" />
        <Columns>
         <asp:BoundField DataField="id" HeaderText = "ID" /> 
         <asp:BoundField DataField="codbare" HeaderText="Cod Bare"  />   
           <asp:BoundField DataField="rasa" HeaderText="Rasa"  />                  
         <asp:BoundField DataField="datat" HeaderText="Data testare" />
         <asp:BoundField DataField="idzilnic" HeaderText="Id zilnic" />
         <asp:BoundField DataField="grasime" HeaderText="Grasime" />
          <asp:BoundField DataField="proteina" HeaderText="% Proteine" />
          <asp:BoundField DataField="casein" HeaderText="% Cazeina" />
           <asp:BoundField DataField="lactoza" HeaderText="% Lactoza" />
          <asp:BoundField DataField="substu" HeaderText="Subst. uscata" />
          <asp:BoundField DataField="ph" HeaderText="pH" />
           <asp:BoundField DataField="urea" HeaderText="Urea" />
           <asp:BoundField DataField="ncs" HeaderText="NCS" />
           <asp:CheckBoxField DataField="bdefinitiv" HeaderText="Definitiv" />
           <asp:CheckBoxField DataField="bvalidat" HeaderText="Validat"  AccessibleHeaderText="Validat" />
       
            <asp:HyperLinkField  HeaderText="Detalii..." Text="Detalii..." DataNavigateUrlFields="id" 
            DataNavigateUrlFormatString="~/DetaliuMostra.aspx?id={0}" />   
            
   
        </Columns>
            <PagerSettings PageButtonCount="20" />
            <RowStyle BackColor="#009999" Font-Bold="False" Font-Names="Arial" Font-Size="9pt" BorderColor="White" BorderStyle="Solid" />
            <HeaderStyle BackColor="#3CB371" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
                ForeColor="White" BorderStyle="Solid" />
            <AlternatingRowStyle BackColor="#99FF99" />
        </asp:GridView>
        &nbsp;&nbsp;
        
</asp:Panel>   

	<div id="divLogDialogue" style="padding:0 !important; overflow: hidden;">
			<textarea id="txtLog" readonly="readonly" style="width: 100%; height: 100%;"></textarea>
	</div>
</asp:Content>

