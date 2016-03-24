<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  Title="Actualizare Ferme/CCL" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.SqlClient" %>
<%@ import Namespace="System.IO" %>

		<script runat="server">

    int itemId = 0;
    string fermierIDChar = "";
    int fermierID=0;
	int fabricaID =0;
	int judetID=0;
	int cod_existent = 0;
    
      
    //****************************************************************
    //
    // The Page_Load event on this Page is used to obtain the ModuleId
    // and itemId of the Ferme_CCL to edit.
    //
    // It then uses the Ferme_CCL() data component
    // to populate the page's edit controls with the Ferme_CCL details.
    //
    //****************************************************************
    
    void Page_Load(Object Sender, EventArgs e) {
    string judet_selectat= "";
    string codJudetChar = "";
	string fabricaIDChar = "";
	string fabrica_selectata = "";
	string fermier_selectat = "";
	string Ferme_CCLField = "";	
	
        // Determine itemId of Ferme_CCL to Update
        if (Request.Params["itemId"] != null) {
            itemId = Int32.Parse(Request.Params["itemId"]);
        }
    
        // If the page is being requested the first time, determine if an
        // Ferme_CCL itemId value is specified, and if so populate page
        // contents with the ferme_CCL details
    
        if (Page.IsPostBack == false) {
			deleteButton.Attributes.Add("onclick","return confirm('Doriti sa stergeti?');");
             Ferme_CCL ferme_CCL = new Ferme_CCL();
             Fermier fermier = new Fermier();
             Fabrici fabrica = new Fabrici();
             Judete judet = new Judete();
             
			if (itemId == 0)
				RadiobuttonFerme.Checked = true;
				
            if (itemId != 0) {
    
                // Obtain a single row of ferme_CCL information
                SqlDataReader dr = ferme_CCL.GetSingleFerme_CCL(itemId);				
				       
    
                // Read first row from database
                dr.Read();
				
				CodField.Text = Convert.ToString(dr["Cod"]);            
                NumeField.Text = Convert.ToString(dr["Nume"]);
                EmailField.Text = Convert.ToString(dr["Email"]);
                NumarField.Text = Convert.ToString(dr["Numar"]);
	            StradaField.Text = Convert.ToString(dr["Strada"]);
                OrasField.Text = Convert.ToString(dr["Oras"]);
                Ferme_CCLField = Convert.ToString(dr["Ferme_CCL"]);
                if (Ferme_CCLField == "F")
					RadiobuttonFerme.Checked = true;
				else if (Ferme_CCLField == "C")
					RadiobuttonCCL.Checked = true;
					
                JudetField.Text = codJudetChar = Convert.ToString(dr["Judet"]);
                int codJudet = 0;
                if (codJudetChar != null && codJudetChar !="")
					judetID = Convert.ToInt32(dr["Judet"]);
                //codJudetChar = Convert.ToString(dr["Judet"]);
                
                SqlDataReader dr1 = judet.GetJudet(judetID);
                dr1.Read();
                judet_selectat = Convert.ToString(dr1["Denloc"]);
                dr1.Close();
                
                TelefonField.Text = Convert.ToString(dr["Telefon"]);
                CodPostalField.Text = Convert.ToString(dr["CodPostal"]);
                FaxField.Text = Convert.ToString(dr["Fax"]);
                //PersonaDeContactField.Text = Convert.ToString(dr["PersonaDeContact"]);
                TelPersContactField.Text = Convert.ToString(dr["TelPersoanaContact"]);
                
                FabricaIDField.Text = fabricaIDChar = Convert.ToString(dr["FabricaID"]);                
                if (fabricaIDChar != null && fabricaIDChar !="")
					fabricaID = Convert.ToInt32(dr["FabricaID"]);
				//fabricaIDChar = Convert.ToString(dr["FabricaID"]);                
                SqlDataReader dr2 = fabrica.GetSingleFabrica(fabricaID);
                dr2.Read();
                fabrica_selectata = Convert.ToString(dr2["Nume"]);
				dr2.Close();
                
                fermierIDChar = Convert.ToString(dr["FermierID"]);               
                if (fermierIDChar != null && fermierIDChar != "")
					fermierID = Convert.ToInt32(dr["FermierID"]);
                //fermierIDChar = Convert.ToString(dr["FermierID"]);                
                SqlDataReader dr3 = fermier.GetSingleFermier(fermierID);
                dr3.Read();
                fermier_selectat = Convert.ToString(dr3["Nume"]);
				dr3.Close();
                
               // Close datareader
                dr.Close();
            }
            JudeteList.Items.Clear(); 
            SqlDataReader drJudete = judet.GetAllJudete();
            //if (judet_selectat != null)
				JudeteList.Items.Add(judet_selectat);
			while (drJudete.Read())
				{
            		if (Convert.ToString(drJudete["ID"]) != codJudetChar)
	            		JudeteList.Items.Add (Convert.ToString(drJudete["Denloc"]));
            	}
				drJudete.Close();
			FermieriList.Items.Clear();    
			SqlDataReader drFermier = fermier.GetAllFermieri();
            //if (fermier_selectat != null)
				FermieriList.Items.Add(fermier_selectat);
			while (drFermier.Read())
				{
            		if (Convert.ToString(drFermier["FermierID"]) != fermierIDChar)
	            		FermieriList.Items.Add (Convert.ToString(drFermier["Nume"]));
            	}
				drFermier.Close();
			FabriciList.Items.Clear();
			SqlDataReader drFabrica = fabrica.GetAllFabrici();
            //if (fabrica_selectata != null)
				FabriciList.Items.Add(fabrica_selectata);
			while (drFabrica.Read())
				{
            		if (Convert.ToString(drFabrica["ID"]) != fabricaIDChar)
	            		FabriciList.Items.Add (Convert.ToString(drFabrica["Nume"]));
            	}
				drFabrica.Close();
    
            // Store URL Referrer to return to portal
          //  ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();
        }
    }
    
    //****************************************************************
    //
    // The UpdateBtn_Click event handler on this Page is used to either
    // create or update a Ferme_CCL.  It  uses the Ferme_CCL()
    // data component to encapsulate all data functionality.
    //
    //****************************************************************
    
    void UpdateBtn_Click(Object sender, EventArgs e) {
    int idOld = 0;
    string Ferme_CCLField = "";
     
    Fermier fermier = new Fermier();
    Fabrici fabrica = new Fabrici();
    Judete judet = new Judete();
	judetID = judet.GetJudetID(JudeteList.SelectedItem.Text.Trim());    
    fermierID = fermier.GetFermierID(FermieriList.SelectedItem.Text.Trim());
    fabricaID = fabrica.GetFabricaID(FabriciList.SelectedItem.Text.Trim());
    
      
	if(RadiobuttonFerme.Checked == true)
			Ferme_CCLField = "F";
	if(RadiobuttonCCL.Checked == true)
				Ferme_CCLField = "C";
					
    
        // Only Update if Entered data is Valid
        if (Page.IsValid == true) {
    
            // Create an instance of the Ferme_CCL component
            Ferme_CCL ferme_CCL = new Ferme_CCL();
            int exista = ferme_CCL.ExistaCodFerma(CodField.Text.Trim());
            idOld = ferme_CCL.GetIDOldMaxFerme_CCL() +1;
	     if (itemId == 0) {
				
                // Add the Ferme_CCL within the Ferme_CCL table
                if (exista == 0)
					ferme_CCL.AddFerme_CCL(itemId, idOld, NumeField.Text, StradaField.Text, NumarField.Text, OrasField.Text, judetID, CodPostalField.Text, TelefonField.Text, FaxField.Text,  EmailField.Text, CodField.Text, PersonaDeContactField.Text, TelPersContactField.Text, fabricaID, fermierID, DataAchizitieField.Text, Ferme_CCLField);
                else
					cod_existent=1;
            }
            else {
				//get id_fabrica vechi
				
				int FabricaID_old = ferme_CCL.GetFabricaIDFerme_CCL(itemId);
				string Cod_old = ferme_CCL.GetCodFerma_CCL(itemId);
				if (fabricaID != FabricaID_old)
					ferme_CCL.AddValidFabricaFerme_CCL(itemId, FabricaID_old, DateTime.Today/*, fabricaID*/);
                // Update the Ferme_CCL within the ferme_CCL table
                if ((Cod_old != CodField.Text && exista == 0) || Cod_old == CodField.Text) 
					ferme_CCL.UpdateFerme_CCL(itemId, NumeField.Text, StradaField.Text, NumarField.Text, OrasField.Text, judetID, CodPostalField.Text, TelefonField.Text, FaxField.Text,  EmailField.Text, CodField.Text, PersonaDeContactField.Text, TelPersContactField.Text, fabricaID, fermierID, DataAchizitieField.Text, Ferme_CCLField);
				else
					cod_existent=1;
               }

               Response.Redirect("~/EditareFerme.aspx");
            // Redirect back to the portal home page
//            if (cod_existent == 0)
  //              Response.Redirect((String)ViewState["UrlReferrer"]);
    //        Response.Redirect((String) ViewState["UrlReferrer"]);
        }
    }
    
    //****************************************************************
    //
    // The DeleteBtn_Click event handler on this Page is used to delete an
    // a Ferme_CCL.  It  uses the Ferme_CCL()
    // data component to encapsulate all data functionality.
    //
    //****************************************************************
    
    void DeleteBtn_Click(Object sender, EventArgs e) {
    
        // Only attempt to delete the item if it is an existing item
        // (new items will have "itemId" of 0)
    
        if (itemId != 0) {
    
            Ferme_CCL ferme_CCL = new Ferme_CCL();
            ferme_CCL.DeleteFerme_CCL(itemId);
        }
    
        // Redirect back to the portal home page
        Response.Redirect("~/EditareFerme.aspx");
        //Response.Redirect((String) ViewState["UrlReferrer"]);
    }
    
    //****************************************************************
    //
    // The CancelBtn_Click event handler on this Page is used to cancel
    // out of the page, and return the user back to the portal home
    // page.
    //
    //****************************************************************
    
    void CancelBtn_Click(Object sender, EventArgs e) {
    
        // Redirect back to the portal home page
        Response.Redirect("~/EditareFerme.aspx");
    }
    
    bool stare_ins()
    {
    if (itemId != 0)
		return (true);
	else
		return (false);
    }
    
  void emailValidate(object source, ServerValidateEventArgs value)
{
   string emails = value.Value;
   char[] chSplit = {';'};
   string[] arrEmails = emails.Split(chSplit);
   value.IsValid = true;
    if (arrEmails == null || arrEmails.Length ==0)
        value.IsValid = false;	
   if (value.IsValid)
   {
     for (int i=0; i < arrEmails.Length; i++)
		{
		   if (arrEmails[i].IndexOf("@")== -1)
		   {
		     value.IsValid =false;
			 break;
			} 
		}
   }        
   
}
   		</script>
		
		<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

		<table cellspacing="0" cellpadding="4" width="110%" border="1">
		
				<tr>
					<td bgColor="#99ccff" colSpan="4" height="6">
					</td>
				</tr>
					<tr>
									<td class="Head" align="left">
										Detalii Ferma/Centru de Colectare
									</td>
								</tr>
						</table>
						<table height="24" cellspacing="0" cellpadding="0" width="100%" border="1">
							<tbody>
								<tr>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="6">
										Cod:
									</td>
									<td vAlign="middle" width="27%" height="6">
										<asp:TextBox id="CodField" runat="server" cssclass="NormalTextBox" Columns="30" maxlength="255"></asp:TextBox>
									</td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="6">
										Fabrica:
									</td>
									<asp:TextBox Visible="False" id="FabricaIDField" runat="server" cssclass="NormalTextBox" width="27%"
										Columns="30" maxlength="255"></asp:TextBox><td class="Normal" width="27%" height="6"><asp:dropdownlist id="FabriciList" Font-Name="Arial" SelectedValue="<%# Page %>" Runat="server">
										</asp:dropdownlist>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Nume:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="NumeField" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:TextBox>
									</td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Strada:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="StradaField" runat="server" cssclass="NormalTextBox" width="390" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Numar:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="NumarField" runat="server" cssclass="NormalTextBox" width="100" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Localitate:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="OrasField" runat="server" cssclass="NormalTextBox" width="390" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Judet:
									</td>
									<asp:TextBox Visible="False" id="JudetField" runat="server" cssclass="NormalTextBox" width="200"
										Columns="30" maxlength="255"></asp:TextBox><td class="Normal" width="27%"><asp:dropdownlist id=JudeteList SelectedValue="<%# Page %>" Font-Name="Arial" Runat="server">
										</asp:dropdownlist>
									</td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Cod Postal:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="CodPostalField" runat="server" cssclass="NormalTextBox" width="100" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
								</tr>
								<tr valign="top">
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Telefon:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="TelefonField" runat="server" cssclass="NormalTextBox" width="200" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Fax:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="FaxField" runat="server" cssclass="NormalTextBox" width="200" Columns="30" maxlength="255"></asp:TextBox>
									</td>
								</tr>
								<tr valign="top">
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Email:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="EmailField" runat="server" cssclass="NormalTextBox" width="200" Columns="30"
											maxlength="255"></asp:TextBox></td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Persoana de Contact:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="PersonaDeContactField" runat="server" cssclass="NormalTextBox" width="250" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
								</tr>
								<tr valign="top">
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Tel. Persoana Contact:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="TelPersContactField" runat="server" cssclass="NormalTextBox" width="200" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Fermier:
									</td>
									<td class="Normal" width="27%" height="6"><asp:dropdownlist id="FermieriList" SelectedValue="<%# Page %>" Font-Name="Arial" Runat="server">
										</asp:dropdownlist>
									</td>									
								<tr>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">
										Data Achizitie:
									</td>
									<td vAlign="middle" width="27%" height="20">
										<asp:TextBox id="DataAchizitieField" runat="server" cssclass="NormalTextBox" width="200" Columns="30"
											maxlength="255"></asp:TextBox>
									</td>
								</tr>
								<tr valign="top">
									<td class="SubHead" vAlign="middle" width="50%" colspan="2" bgColor="#99ccff" height="20">
										&nbsp;
										<asp:RadioButton GroupName="Ferme_CCL" AutoPostBack="True" runat="server" Text="Ferma" ID="RadiobuttonFerme" />
									</td>
									<td class="SubHead" vAlign="middle" width="50%" colspan="2" bgColor="#99ccff" height="20">
										&nbsp;
										<asp:RadioButton GroupName="Ferme_CCL" AutoPostBack="True" runat="server" Text="CCL" ID="RadiobuttonCCL" />
									</td>
								</tr>
				
		</table>
		
		<asp:requiredfieldvalidator id="Req1" runat="server" Font-Name="Arial" Display="Static" ErrorMessage="Codul este obligatoriu"
			ControlToValidate="CodField"></asp:requiredfieldvalidator>
		<% if (cod_existent == 1) {%>
		<br>
		<asp:label Font-Bold="true" id="Label14" ForeColor="#cc0066" runat="server" Font-Name="Arial"
			Font-Size="14">
				Codul exista deja!
			</asp:label>
		<% }%>
		
			<asp:RegularExpressionValidator ID="regCod" runat="server" Font-Name="Arial"  Font-Size=8 ControlToValidate="CodField"
            ErrorMessage="Codul trebuie sa fie pe 5 pozitii!!" 
            ValidationExpression="\w{5}"></asp:RegularExpressionValidator> 
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Static" ErrorMessage="Coloana Nume trebuie completata"
				ControlToValidate="NumeField"></asp:RequiredFieldValidator>
			<asp:CustomValidator ControlToValidate="EmailField" OnServerValidate="emailValidate"
									text="Adresa de email invalida" runat="server" ID="Customvalidator1" NAME="Customvalidator1">Adresa de email invalida
									</asp:CustomValidator>
	
		
			<asp:LinkButton class="CommandButton" id="updateButton" onclick="UpdateBtn_Click" runat="server"
				Text="Salveaza" BorderStyle="none">Salvare</asp:LinkButton>
			<asp:LinkButton class="CommandButton" id="cancelButton" onclick="CancelBtn_Click" runat="server"
				Text="Renunta" BorderStyle="none" CausesValidation="False">Renuntare</asp:LinkButton>
			<asp:LinkButton class="CommandButton" id="deleteButton" onclick="DeleteBtn_Click" runat="server"
				Text="Sterge" BorderStyle="none" CausesValidation="False">Sterge</asp:LinkButton>
		
		
		
		
</asp:Content>