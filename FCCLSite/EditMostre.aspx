<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  Title="Actualizare Mostre" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Data.SqlClient" %>
<%@ import Namespace="System.IO" %>

<script runat="server">

    int itemId = 0;
    int arata_msg = 0;
    string prelevator_selectat = "";
    string ferma_selectata = "";

//    int moduleId = 0;

//    DropDownList CategTmp;
//    String Activ;
    //DropDownList ActiviteTmp;
  
  
    //****************************************************************
    //
    // The Page_Load event on this Page is used to obtain the ModuleId
    // and ItemId of the mostre to edit.
    //
    // It then uses the MostreDB() data component
    // to populate the page's edit controls with the annoucement details.
    //
    //****************************************************************
    
    void Page_Load(Object Sender, EventArgs e) {

        PrelevatoriDB prelevatori = new PrelevatoriDB();
        Ferme_CCL ferma = new Ferme_CCL();
        // Determine ItemId of mostre to Update
        if (Request.Params["ItemId"] != null) {
            itemId = Int32.Parse(Request.Params["ItemId"]);
        }
                             
		if(ListaAntibiotice.Items.Count < 3){
			ListaAntibiotice.Items.Add (" ");
			ListaAntibiotice.Items.Add ("Pozitiv");
			ListaAntibiotice.Items.Add ("Negativ");
		}
		
        if (Page.IsPostBack == false) {
			deleteButton.Attributes.Add("onclick","return confirm('Doriti sa stergeti?');");
            if (itemId != 0) {
    
                // Obtain a single row of mostre information
                MostreDB mostre = new MostreDB();
                SqlDataReader dr = mostre.GetSingleMostre(itemId);
    
                // Load first row into DataReader
                dr.Read();
    
                CodBare.Text = Convert.ToString (dr["CodBare"]);
					if (dr["Antibiotice"] == DBNull.Value)
						ListaAntibiotice.Items.FindByText(" ").Selected = true;
					else
						if(Convert.ToString(dr["Antibiotice"]) == "-1" || Convert.ToString(dr["Antibiotice"])=="1")
							  ListaAntibiotice.Items.FindByText("Pozitiv").Selected = true;
						else
							if(Convert.ToString(dr["Antibiotice"]) == "0")
								ListaAntibiotice.Items.FindByText("Negativ").Selected = true;
						
//						 ListaAntibiotice.SelectedItem.Text = " ";
                CantitatePrelevare.Text =  Convert.ToString(dr["CantitateLaPrelevare"]);
                NCS.Text =  Convert.ToString(dr["NumarCeluleSomatice"]);
                Grasime.Text = Convert.ToString(dr["Grasime"]);
                NTG.Text = Convert.ToString(dr["IncarcaturaGermeni"]);
                Proteine.Text = Convert.ToString(dr["ProcentProteine"]);
                DataPrelevare.Text = Convert.ToString(dr["DataPrelevare"]);
                Lactoza.Text = Convert.ToString(dr["ProcentLactoza"]);
              //  DataPrimirii.Text = Convert.ToString(dr["DataPrimirii"]);
        //        DataPrimirii.Text = ((DateTime) dr["DataPrimirii"]).ToShortDateString();
         //       DataPrimirii.Text = ((DateTime) dr["DataPrimirii"]).ToString(ResourceManager.GetString("Utility_CurrentTime_ShortdateFormat"));
				DataPrimirii.Text = ((DateTime) dr["DataPrimirii"]).ToString("dd/MM/yyyy");
                SubstantaUscata.Text = Convert.ToString(dr["SubstantaUscata"]);
                DataTestare.Text = Convert.ToString(dr["DataTestare"]);
                DataTestareFinala.Text = Convert.ToString(dr["DataTestareFinala"]);
                PunctInghet.Text = Convert.ToString(dr["PunctInghet"]);
//                Prelevator.Text = Convert.ToString(dr["PrelevatorID"]);
                Definitiv.Checked =(Convert.ToInt32(dr["Definitiv"]) == 1)? true : false;
                Validat.Checked =(Convert.ToInt32(dr["Validat"]) == 1)? true : false;
                IdZilnic.Text=Convert.ToString(dr["IdZilnic"]);
                IdZilnic.Enabled = false;
//                FermeList.Enabled=true;
                int prelevatorID ;
                if((dr["PrelevatorID"] != DBNull.Value && Convert.ToInt32(dr["PrelevatorID"])!=0) && (dr["NumePrelevator"]==DBNull.Value || Convert.ToString(dr["NumePrelevator"])==""))
				{
	                prelevatorID = Convert.ToInt32(dr["PrelevatorID"]);
//	                ListaPrelevatori.Items.Add(" ");
//					ListaPrelevatori.Enabled=true;
                    NrComanda.Text = Convert.ToString(dr["NrComanda"]);
//					NrComanda.Enabled=false;
					NumePrelevator.Text = "";
//					NumePrelevator.Enabled=false;
					NumeClient.Text = "";
//					NumeClient.Enabled=false;
					Judet.Text = "";
//					Judet.Enabled=false;
					Localitate.Text = "";
//					Localitate.Enabled=false;
					AdresaClient.Text = "";
//					AdresaClient.Enabled=false;
					NumeProba.Text = "";
//					NumeProba.Enabled=false;
					
				}
				else
				{
					prelevatorID = 0;
//					ListaPrelevatori.Enabled=false;
					NrComanda.Text = Convert.ToString(dr["NrComanda"]);
//					NrComanda.Enabled=true;
					NumePrelevator.Text = Convert.ToString(dr["NumePrelevator"]);
//					NumePrelevator.Enabled=true;
					NumeClient.Text = Convert.ToString(dr["NumeClient"]);
//					NumeClient.Enabled=true;
					Judet.Text = Convert.ToString(dr["Judet"]);
//					Judet.Enabled=true;
					Localitate.Text = Convert.ToString(dr["Localitate"]);
//					Localitate.Enabled=true;
					AdresaClient.Text = Convert.ToString(dr["AdresaClient"]);
//					AdresaClient.Enabled=true;
					NumeProba.Text = Convert.ToString(dr["NumeProba"]);
//					NumeProba.Enabled=true;
                }
                prelevator_selectat = prelevatori.GetPrelevator(prelevatorID);
                
              //  if(dr["FermaID"] != System.DBNull.Value && Convert.ToInt32(dr["FermaID"]) != 0)
	            //    ferma_selectata = ferma.GetCodFerma_CCL(Convert.ToInt32(dr["FermaID"])) + " " + ferma.GetNumeFerma(Convert.ToInt32(dr["FermaID"]));
	             
                  
                // Close the datareader
                dr.Close();
            }
            else
{
//				DataPrimirii.Text = (DateTime.Now).ToString(ResourceManager.GetString("Utility_CurrentTime_ShortdateFormat"));
				DataPrelevare.Text = (DateTime.Now).ToString("dd/MM/yyyy");		
				DataPrimirii.Text = (DateTime.Now).ToString("dd/MM/yyyy");					
			    DataTestare.Text = (DateTime.Now).ToString("dd/MM/yyyy");
			    DataTestareFinala.Text = (DateTime.Now).ToString("dd/MM/yyyy");
			    ListaAntibiotice.SelectedItem.Text = " ";
//			    FermeList.Enabled=false;
			    
}	

           SqlDataReader drPrelevatori = prelevatori.GetAllPrelevatori();
            //if (judet_selectat != null)
				ListaPrelevatori.Items.Add(prelevator_selectat);
			while (drPrelevatori.Read())
			{
            	if (Convert.ToString(drPrelevatori["NumePrelevator"]) != prelevator_selectat)
	           		ListaPrelevatori.Items.Add (Convert.ToString(drPrelevatori["NumePrelevator"]));
            }
            ListaPrelevatori.Items.Add(" ");
            drPrelevatori.Close();
            

/*           	SqlDataReader drFermeCCL = ferma.GetAllFerme();
//			FermeList.Items.Add (ferma_selectata);
			while (drFermeCCL.Read())
			{
				if (ferma.GetNumeFerma(Convert.ToInt32(drFermeCCL["ID"])).Trim() != ferma_selectata.Trim()) //!!!
			         FermeList.Items.Add (Convert.ToString(Convert.ToString(drPrelevatori["Cod"] + " " + drFermeCCL["Nume"]));
  			}
  			drFermeCCL.Close();
*/
            
            ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();
        }
    }
    
    
    void UpdateBtn_Click(Object sender, EventArgs e) {
    	string ferma_ccl_cod = "";
		int ferma_ccl_id= 0;
		int fabricaID = 0;
    /*
		Ferme_CCL ferma = new Ferme_CCL();
		PrelevatoriDB prelevatori = new PrelevatoriDB();

        // Only Update if the Entered Data is Valid
        if (Page.IsValid == true) {
    
            // Create an instance of the mostre DB component
            MostreDB mostre = new MostreDB();
		    int prelevID = prelevatori.GetPrelevatorID(ListaPrelevatori.SelectedItem.Text.Trim());
		    string int_antibiotice;
				if(ListaAntibiotice.SelectedItem.Text.Trim() == "Pozitiv")
					int_antibiotice = "1";
				else
					if(ListaAntibiotice.SelectedItem.Text.Trim() == "Negativ")
						int_antibiotice = "0";
					else
						int_antibiotice = "";
		    ferma_ccl_cod = CodBare.Text.Substring(0,5);
            ferma_ccl_id =	mostre.GetFermeCCL_ID(ferma_ccl_cod);
            fabricaID = ferma.GetFabricaIDFerme_CCL(ferma_ccl_id);
       
       if (prelevID != 0 && (NumePrelevator.Text.Trim() != "" || NumeClient.Text.Trim() != "" || Judet.Text.Trim() != "" ||
			 Localitate.Text.Trim() != "" ||	AdresaClient.Text.Trim() != "" || NumeProba.Text.Trim() != "")) 
			arata_msg = 3;			     
		else
{		
       if (ferma_ccl_id == 0 && prelevID != 0)
            arata_msg = 2;
       else
       {     
                    
			if (itemId == 0) {
				int exista = mostre.ExistaCodBare(CodBare.Text.Trim());
				int idzilnic = 0; 
				
				if (mostre.ExistaIdZilnic (DataTestare.Text.Trim(), Convert.ToInt32(IdZilnic.Text.Trim())) == 0 )
					idzilnic = Convert.ToInt32(IdZilnic.Text.Trim());
				else
				{
					idzilnic = mostre.GetMaxIdZilnic(DataTestare.Text.Trim())+1;
					while (mostre.ExistaIdZilnic (DataTestare.Text.Trim(), idzilnic) != 0)
						idzilnic++;
				}
				if (exista == 0)
			    {
			        mostre.AddMostra(itemId, idzilnic, (CodBare.Text.Trim() == "")? "0": CodBare.Text, NrComanda.Text.Trim(), ferma_ccl_cod, ferma_ccl_id, fabricaID, int_antibiotice, (CantitatePrelevare.Text.Trim() == "")? "0": CantitatePrelevare.Text, (NCS.Text.Trim() == "")? "0": NCS.Text, (Grasime.Text.Trim() == "")? "0": Grasime.Text, 
						(NTG.Text.Trim() == "")? "0": NTG.Text, (Proteine.Text.Trim() == "")? "0": Proteine.Text, (DataPrelevare.Text.Trim() == "")? " ": DataPrelevare.Text, (Lactoza.Text.Trim() == "")? "0": Lactoza.Text, 
						(DataPrimirii.Text.Trim() == "")? " ": DataPrimirii.Text, (SubstantaUscata.Text.Trim() == "")? "0": SubstantaUscata.Text, 
						(DataTestare.Text.Trim() == "")? " ": DataTestare.Text, (PunctInghet.Text.Trim() == "")? "0" : PunctInghet.Text,
						prelevID, Definitiv.Checked ? 1 : 0, Validat.Checked ? 1 : 0, (DataTestareFinala.Text.Trim() == "")? " ": DataTestareFinala.Text,"0","0");
					mostre.UpdateMostraFCB((CodBare.Text.Trim() == "")? "0": CodBare.Text,(NrComanda.Text.Trim() == "")? "": NrComanda.Text , (NumePrelevator.Text.Trim() == "")? "": NumePrelevator.Text , 
			(NumeProba.Text.Trim() == "")? "": NumeProba.Text ,  (NumeClient.Text.Trim() == "")? "": NumeClient.Text , (AdresaClient.Text.Trim() == "")? "": AdresaClient.Text , ( Localitate.Text.Trim() == "")? "": Localitate.Text, (Judet.Text.Trim() == "")? "": Judet.Text );
				}
				else
				{
					arata_msg = 1;
					
				}	
            }
            else {

                mostre.UpdateMostra(itemId, (CodBare.Text.Trim() == "")? "0": CodBare.Text,NrComanda.Text.Trim(), ferma_ccl_cod, ferma_ccl_id, fabricaID, int_antibiotice, (CantitatePrelevare.Text.Trim() == "")? "0": CantitatePrelevare.Text, (NCS.Text.Trim() == "")? "0": NCS.Text, (Grasime.Text.Trim() == "")? "0": Grasime.Text, 
					(NTG.Text.Trim() == "")? "0": NTG.Text, (Proteine.Text.Trim() == "")? "0": Proteine.Text, (DataPrelevare.Text.Trim() == "")? " ": DataPrelevare.Text, (Lactoza.Text.Trim() == "")? "0": Lactoza.Text, 
					(DataPrimirii.Text.Trim() == "")? " ": DataPrimirii.Text, (SubstantaUscata.Text.Trim() == "")? "0": SubstantaUscata.Text, 
					(DataTestare.Text.Trim() == "")? " ": DataTestare.Text, (PunctInghet.Text.Trim() == "")? "0": PunctInghet.Text,
					prelevID, Definitiv.Checked ? 1 : 0, Validat.Checked ? 1 : 0,(DataTestareFinala.Text.Trim() == "")? " ": DataTestareFinala.Text);
				mostre.UpdateMostraFCB((CodBare.Text.Trim() == "")? "0": CodBare.Text,(NrComanda.Text.Trim() == "")? "": NrComanda.Text , (NumePrelevator.Text.Trim() == "")? "": NumePrelevator.Text , 
			(NumeProba.Text.Trim() == "")? "": NumeProba.Text ,  (NumeClient.Text.Trim() == "")? "": NumeClient.Text , (AdresaClient.Text.Trim() == "")? "": AdresaClient.Text , ( Localitate.Text.Trim() == "")? "": Localitate.Text, (Judet.Text.Trim() == "")? "": Judet.Text );
            }
       }
 }       
        if (arata_msg == 0)
            Response.Redirect((String) ViewState["UrlReferrer"]);
        }
     */ 
    }
    
    //****************************************************************
    //
    // The DeleteBtn_Click event handler on this Page is used to delete an
    // an WorkDoc.  It  uses the MostreDB()
    // data component to encapsulate all data functionality.
    //
    //****************************************************************
    
    void DeleteBtn_Click(Object sender, EventArgs e) {
    
        // Only attempt to delete the item if it is an existing item
        // (new items will have "ItemId" of 0)
    
        if (itemId != 0) {
    
            MostreDB mostre = new MostreDB();
            mostre.DeleteMostra(itemId);
        }
    
        // Redirect back to the portal home page
        Response.Redirect((String) ViewState["UrlReferrer"]);
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
        Response.Redirect((String) ViewState["UrlReferrer"]);
    }
		</script>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

		<table cellSpacing="0" cellPadding="4" width="110%" border="1">
			<tbody>
				<tr>
					<td bgColor="#99ccff" colSpan="4" height="6">
					</td>
				</tr>
				<tr vAlign="top">
					<td width="1" bgColor="#99ccff"></td>
					<td width="*">
						<table height="53" cellSpacing="0" cellPadding="0" width="100%">
							<tbody>
								<tr>
									<td class="Head" align="left">Detaliile&nbsp;Mostrei
									</td>
								</tr>
								<tr>
									<td colSpan="2">
										<hr noShade SIZE="1">
									</td>
								</tr>
							</tbody></table>
						<table height="24" cellSpacing="0" cellPadding="0" width="100%" border="1">
							<tbody>
								<tr>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">Cod 
										Bare:
									</td>
									<td vAlign="middle" width="27%" height="20"><asp:textbox id="CodBare" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="23%" bgColor="#99ccff" height="20">Inhibitori:
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:dropdownlist id="ListaAntibiotice"  Font-Name="Arial" Runat="server"></asp:dropdownlist>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Cantitate 
										la prelevare:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="CantitatePrelevare" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">NCS:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="NCS" runat="server" cssclass="NormalTextBox" width="200px" Columns="30" maxlength="50"
											Font-Name="Arial"></asp:textbox></td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Grasime:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="Grasime" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">NTG:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="NTG" runat="server" cssclass="NormalTextBox" width="200px" Columns="30" maxlength="50"
											Font-Name="Arial"></asp:textbox></td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Proteine:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="Proteine" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Data 
										prelevare:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="DataPrelevare" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Lactoza:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="Lactoza" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Data 
										primirii:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="DataPrimirii" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Substanta 
										Uscata:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="SubstantaUscata" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Data 
										Testare:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="DataTestare" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Punct 
										Inghet:
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="PunctInghet" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox></td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Identificator Zilnic
									</td>
									<td vAlign="middle" width="410" height="20"><asp:textbox id="IdZilnic" runat="server" cssclass="NormalTextBox" width="50px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
										</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Definitiv:
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:checkbox id="Definitiv" runat="server" Enabled=False Checked="false" Cssclass="Normal"></asp:checkbox>
									</td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Validat</td>
									<td class="Normal" width="380"><asp:checkbox id="Validat" runat="server" Enabled="false" Checked="false" Cssclass="Normal"></asp:checkbox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Data 
										Testare Finala
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:textbox id="DataTestareFinala" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Numar 
										Comanda</td>
									<td class="Normal" width="380"><asp:textbox id="NrComanda" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Prelevator:
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:dropdownlist id="ListaPrelevatori" SelectedValue="<%# Page %>" Font-Name="Arial" Runat="server"></asp:dropdownlist>
									</td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Nume 
										Prelevator</td>
									<td class="Normal" width="380"><asp:textbox id="NumePrelevator" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Nume 
										Client
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:textbox id="NumeClient" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Judet Client</td>
									<td class="Normal" width="380"><asp:textbox id="Judet" runat="server" cssclass="NormalTextBox" width="200px" Columns="30" maxlength="50"
											Font-Name="Arial"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Localitate Client
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:textbox id="Localitate" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Adresa 
										Client</td>
									<td class="Normal" width="380"><asp:textbox id="AdresaClient" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">Nume 
										Proba
									</td>
									<td vAlign="middle" width="410" height="20">
										<asp:textbox id="NumeProba" runat="server" cssclass="NormalTextBox" width="200px" Columns="30"
											maxlength="50" Font-Name="Arial"></asp:textbox>
									</td>
									<td class="SubHead" vAlign="middle" width="20%" bgColor="#99ccff" height="20">&nbsp;</td>
									<td class="Normal" width="380"></td>
								</tr>
								
								
							</tbody></table>
					</td>
					<td width="1" bgColor="#99ccff"></td>
				</tr>
				<tr>
					<td bgColor="#99ccff" colSpan="4" height="6">
					</td>
				</tr>
				<!--				<tr vAlign="top">
					<td class="SubHead" width="147" height="16">Level:
					</td>
					<td class="Normal" width="410" height="16"><asp:dropdownlist id=CategList SelectedValue="<%# Page %>" Runat="server">
							<asp:ListItem Value="" Selected="True"></asp:ListItem>
							<asp:ListItem Value="EU">EU</asp:ListItem>
							<asp:ListItem Value="National">National</asp:ListItem>
						</asp:dropdownlist></td>
				</tr>
				--></tbody></table>
		<asp:requiredfieldvalidator id="Req1" runat="server" Font-Name="Arial" Display="Static" ErrorMessage="Codul de bare este obligatoriu"
			ControlToValidate="CodBare"></asp:requiredfieldvalidator>
		<asp:RegularExpressionValidator ID="regCod" runat="server" Font-Name="Arial"  Font-Size=8 ControlToValidate="CodBare"
            ErrorMessage="Codul trebuie sa fie pe 10 pozitii!!"             ValidationExpression="\w{10}"></asp:RegularExpressionValidator> 
		<% if (arata_msg == 1) {%>
		<br>
		<asp:label Font-Bold="true" id="Label14" ForeColor="#cc0066" runat="server" Font-Name="Arial" Font-Size="14">
				Cod de bare deja existent!
			</asp:label>
		<% }%>
		<% if (arata_msg == 2) {%>
		<br>
		<asp:label Font-Bold="true" id="Label1" ForeColor="#cc0066" runat="server" Font-Name="Arial" Font-Size="14">
				Codul de ferma aferent codului de bare nu exista!
			</asp:label>
		<% }%>
		<% if (arata_msg == 3) {%>
		<br>
		<asp:label Font-Bold="true" id="Label2" ForeColor="#cc0066" runat="server" Font-Name="Arial" Font-Size="14">
				Neconcordanta: Au fost completate Prelevator si Nume Prelevator!<br>(mostra FCB sau cu cod de bare)
			</asp:label>
		<% }%>
		<p><asp:linkbutton id="updateButton" onclick="UpdateBtn_Click" runat="server" Font-Name="Arial" Text="Salvare"
				CssClass="CommandButton" BorderStyle="none"></asp:linkbutton>&nbsp;
			<asp:linkbutton id="cancelButton" onclick="CancelBtn_Click" runat="server" Font-Name="Arial" Text="Renuntare"
				CssClass="CommandButton" BorderStyle="none" CausesValidation="False"></asp:linkbutton>&nbsp;
			<asp:linkbutton id="deleteButton" onclick="DeleteBtn_Click" runat="server" Font-Name="Arial" Text="Stergere Mostra"
				CssClass="CommandButton" BorderStyle="none" CausesValidation="False"></asp:linkbutton></p>
		
</asp:Content>