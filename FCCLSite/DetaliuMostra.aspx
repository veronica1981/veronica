<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DetaliuMostra" Title="Detalii Mostra" Codebehind="DetaliuMostra.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <table  width=700>
    <tr>
    <td style="width: 182px"></td>
    <td>
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt"
            ForeColor="Red"></asp:Label></td>
    </tr>
    </table>
    <asp:FormView ID="FormView1" runat="server" CellPadding="5" CellSpacing="5" DataSourceID="ObjectDataSource1" DataKeyNames="ID"
        DefaultMode="Edit" GridLines="Both" HeaderText="Detalii Mostra" OnItemInserted="FormView1_ItemInserted" OnItemInserting="FormView1_ItemInserting" OnItemUpdated="FormView1_ItemUpdated" OnItemUpdating="FormView1_ItemUpdating" OnItemDeleted="FormView1_ItemDeleted" >
        <EditItemTemplate>
        <table border=1 cellspacing=2 cellpadding=2 width=700>
        <tr>
        <td class="heading" bgcolor=cornflowerblue>Cod Bare:</td>
        <td width=200><asp:TextBox ID="CodBareTextBox" runat="server" Text='<%# Bind("CodBare") %>'>
            </asp:TextBox>
          </td>
        <td >
             </td>
        <td width=200><asp:RegularExpressionValidator ID="regCod" runat="server"   Font-Size=8 ControlToValidate="CodBareTextBox"
            ErrorMessage="Codul trebuie sa fie numeric pe 10 pozitii!!"   ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Id Zilnic:</td>
        <td> <asp:TextBox ID="IdZilnicTextBox" runat="server" Text='<%# Bind("IdZilnic") %>'>
            </asp:TextBox><br /></td>
        <td class="heading" bgcolor="cornflowerblue">Prelevator:</td>
        <td>  <asp:TextBox   ID=PrelevatorIDTextBox runat=server   Text='<%# Bind("PrelevatoriId") %>' Width="150px">
         </asp:TextBox>  
          </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr height=20px>
        <td class="heading" bgcolor="cornflowerblue">Data primirii:</td>
        <td><asp:TextBox ID="DataPrimiriiTextBox" runat="server" Text='<%# Bind("DataPrimirii") %>'>
            </asp:TextBox>
         
            </td>
        <td class="heading" bgcolor="cornflowerblue">Data prelevarii:</td>
        <td> <asp:TextBox ID="DataPrelevareTextBox" runat="server" Text='<%# Bind("DataPrelevare") %>'>
            </asp:TextBox>
         
        </td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Data testarii:</td>
        <td><asp:TextBox ID="DataTestareTextBox" runat="server" Text='<%# Bind("DataTestare") %>'>
            </asp:TextBox>
                     </td>
        <td class="heading" bgcolor="cornflowerblue">Data testarii finale</td>
        <td> <asp:TextBox ID="DataTestareFinalaTextBox" runat="server" Text='<%# Bind("DataTestareFinala") %>'>
            </asp:TextBox>
         
            </td>
        </tr>
         <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
         <tr>
        <td class="heading" bgcolor="cornflowerblue">Grasime:</td>
        <td><asp:TextBox ID="GrasimeTextBox" runat="server" Text='<%# Bind("Grasime") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Proteina:</td>
        <td><asp:TextBox ID="ProteinaTextBox" runat="server" Text='<%# Bind("Proteina") %>'>
            </asp:TextBox>
          </td>  
        </tr>
       
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Cazeina:</td>
         <td><asp:TextBox ID="CaseinaTextBox" runat="server" Text='<%# Bind("Casein") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">pH:</td>
        <td>
        <asp:TextBox ID="PhTextBox" runat="server" Text='<%# Bind("Ph") %>'>
            </asp:TextBox>
        </td>
        </tr>
        
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Lactoza:</td>
        <td><asp:TextBox ID="LactozaTextBox" runat="server" Text='<%# Bind("Lactoza") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Subst. uscata:</td>
        <td> <asp:TextBox ID="SubstuTextBox" runat="server" Text='<%# Bind("Substu") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Punct inghet:</td>
        <td> <asp:TextBox ID="PunctiTextBox" runat="server" Text='<%# Bind("Puncti") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Inhibitori:</td>
        <td>  <asp:DropDownList  ID=AntibDropDownList runat="server"  SelectedValue='<%# Bind("Antib") %>' Width="150px">
         <asp:ListItem Text="" Value=""></asp:ListItem>
         <asp:ListItem Text="Negativ" Value="0"></asp:ListItem>
         <asp:ListItem Text="Pozitiv" Value="1"></asp:ListItem> 
         </asp:DropDownList> 
        </td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">NCS:</td>
        <td> <asp:TextBox ID="NCSTextBox" runat="server" Text='<%# Bind("NCS") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">NTG:</td>
        <td>  <asp:TextBox ID="NTGTextBox" runat="server" Text='<%# Bind("NTG") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Urea:</td>
        <td> <asp:TextBox ID="UreaTextBox" runat="server" Text='<%# Bind("Urea") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Sms trimis:</td>
        <td><asp:CheckBox ID="SentsmsCheckBox" runat="server" Checked='<%# Bind("Sentsms") %>' /> 
        </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nr. Comanda:</td>
        <td> <asp:TextBox ID="NrComandaTextBox" runat="server" Text='<%# Bind("NrComanda") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Cant. Prelevata:</td>
        <td><asp:TextBox ID="CantTextBox" runat="server" Text='<%# Bind("Cant") %>'>
            </asp:TextBox></td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nume Prelevator:</td>
        <td> <asp:TextBox ID="NumePrelevatorTextBox" runat="server" Text='<%# Bind("PrelevatoriNume") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Nume Proba:</td>
        <td><asp:TextBox ID="NumeProbaTextBox" runat="server" Text='<%# Bind("NumeProba") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nume Client:</td>
        <td> <asp:TextBox ID="FabriciNumeTextBox" runat="server" Text='<%# Bind("FabriciNume") %>'>
            </asp:TextBox></td>
        <td class=heading bgcolor=cornflowerblue>Adresa Client:</td>
        <td>  <asp:TextBox ID="FabriciStradaTextBox" runat="server" Text='<%# Bind("FabriciStrada") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class=heading bgcolor=cornflowerblue>Localitate:</td>
        <td>  <asp:TextBox ID="FabriciOrasTextBox" runat="server" Text='<%# Bind("FabriciOras") %>'>
            </asp:TextBox></td>
        <td class=heading bgcolor=cornflowerblue>Judet:</td>
        <td>   <asp:TextBox ID="FabriciJudetTextBox" runat="server" Text='<%# Bind("FabriciJudet") %>'>
            </asp:TextBox></td>
        </tr>
          <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr> 
        <tr>
        <td class=heading bgcolor=cornflowerblue>Definitiv:</td>
        <td><asp:CheckBox ID="DefinitivCheckBox" runat="server" Checked='<%# Bind("Definitiv") %>' />
        </td>
        <td class=heading bgcolor=cornflowerblue>Validat:</td>
        <td><asp:CheckBox ID="ValidatCheckBox" runat="server" Checked='<%# Bind("Validat") %>' />
        </td>
        </tr> 
         <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
         <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        
       
         <tr>
        <td align=center>
     
          <asp:HyperLink runat=server  Text="Back" NavigateUrl="~/EditareMostre.aspx" Font-Bold=true Font-Size=12pt  ForeColor=SeaGreen>
          </asp:HyperLink>|
     
        </td>
        <td align=center><asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"  Font-Bold=true ForeColor=SeaGreen Font-Size=12pt
                Text="Salvare">
            </asp:LinkButton></td>
        <td>  <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" ForeColor=SeaGreen Font-Bold=true  Font-Size=12pt
                Text="Renuntare">
            </asp:LinkButton></td>
        <td align=center> <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Font-Bold=true Font-Size=12pt  ForeColor=SeaGreen
			Text="Stergere" OnClientClick="return confirm('Doriti stergerea mostrei?');"></asp:LinkButton></td>
        </tr>
         <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        </table>
           
       
             
         </EditItemTemplate>
     
        <InsertItemTemplate>
        <table border=1 cellspacing=2 cellpadding=2 width=700>
        <tr>
        <td class="heading" bgcolor=cornflowerblue>Cod Bare:</td>
        <td width=200><asp:TextBox ID="CodBareTextBox" runat="server" Text='<%# Bind("CodBare") %>'>
            </asp:TextBox>
          </td>
        <td>
           </td>
        <td width=200><asp:RegularExpressionValidator ID="regCod" runat="server"   Font-Size=8 ControlToValidate="CodBareTextBox"
            ErrorMessage="Codul trebuie sa fie numeric pe 10 pozitii!!"   ValidationExpression="\d{10}"></asp:RegularExpressionValidator> 
</td>  
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Id Zilnic:</td>
        <td> <asp:TextBox ID="IdZilnicTextBox" runat="server" Text='<%# Bind("IdZilnic") %>'>
            </asp:TextBox><br /></td>
        <td class="heading" bgcolor="cornflowerblue">Prelevator:</td>
        <td> <asp:TextBox   ID=PrelevatorIDTextBox runat=server   Text='<%# Bind("PrelevatoriId") %>'  Width="150px">
         </asp:TextBox>  
       
          </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr height=20px>
        <td class="heading" bgcolor="cornflowerblue">Data primirii:</td>
        <td><asp:TextBox ID="DataPrimiriiTextBox" runat="server" Text='<%# Bind("DataPrimirii") %>'>
            </asp:TextBox>
            
            </td>
        <td class="heading" bgcolor="cornflowerblue">Data prelevarii:</td>
        <td> <asp:TextBox ID="DataPrelevareTextBox" runat="server" Text='<%# Bind("DataPrelevare") %>'>
            </asp:TextBox>
         
            </td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Data testarii:</td>
        <td><asp:TextBox ID="DataTestareTextBox" runat="server" Text='<%# Bind("DataTestare") %>'>
            </asp:TextBox>
        
            </td>
        <td class="heading" bgcolor="cornflowerblue">Data testarii finale</td>
        <td> <asp:TextBox ID="DataTestareFinalaTextBox" runat="server" Text='<%# Bind("DataTestareFinala") %>'>
            </asp:TextBox>
        
         </td>
        </tr>
         <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
         <tr>
        <td class="heading" bgcolor="cornflowerblue">Grasime:</td>
        <td><asp:TextBox ID="GrasimeTextBox" runat="server" Text='<%# Bind("Grasime") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Proteina:</td>
        <td><asp:TextBox ID="ProteinaTextBox" runat="server" Text='<%# Bind("Proteina") %>'>
            </asp:TextBox>
          </td>  
        </tr>
        
         <tr>
        <td class="heading" bgcolor="cornflowerblue">Cazeina:</td>
         <td><asp:TextBox ID="CaseinaTextBox" runat="server" Text='<%# Bind("Casein") %>'>
            </asp:TextBox></td>
        <td class="heading"  bgcolor="cornflowerblue">pH:</td>
        <td>
        <asp:TextBox ID="PhTextBox" runat="server" Text='<%# Bind("Ph") %>'>
            </asp:TextBox>
        </td>
        </tr>
        
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Lactoza:</td>
        <td><asp:TextBox ID="LactozaTextBox" runat="server" Text='<%# Bind("Lactoza") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Subst. uscata:</td>
        <td> <asp:TextBox ID="SubstuTextBox" runat="server" Text='<%# Bind("Substu") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Punct inghet:</td>
        <td> <asp:TextBox ID="PunctiTextBox" runat="server" Text='<%# Bind("Puncti") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Inhibitori:</td>
        <td><asp:DropDownList  ID=AntibDropDownList runat="server"   SelectedValue='<%# Bind("Antib") %>' Width="150px">
         <asp:ListItem Text="" Value=""></asp:ListItem>
         <asp:ListItem Text="Negativ" Value="0"></asp:ListItem>
         <asp:ListItem Text="Pozitiv" Value="1"></asp:ListItem> 
         </asp:DropDownList>  
     </td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">NCS:</td>
        <td> <asp:TextBox ID="NCSTextBox" runat="server" Text='<%# Bind("NCS") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">NTG:</td>
        <td>  <asp:TextBox ID="NTGTextBox" runat="server" Text='<%# Bind("NTG") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Urea:</td>
        <td> <asp:TextBox ID="UreaTextBox" runat="server" Text='<%# Bind("Urea") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Sms trimis:</td>
        <td><asp:CheckBox ID="SentsmsCheckBox" runat="server" Checked='<%# Bind("Sentsms") %>' /> 
        </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nr. Comanda:</td>
        <td> <asp:TextBox ID="NrComandaTextBox" runat="server" Text='<%# Bind("NrComanda") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Cant. Prelevata:</td>
        <td>
            <asp:TextBox ID="CantTextBox" runat="server" Text='<%# Bind("Cant") %>'>
            </asp:TextBox>
        </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nume Prelevator:</td>
        <td> <asp:TextBox ID="NumePrelevatorTextBox" runat="server" Text='<%# Bind("PrelevatoriNume") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Nume Proba:</td>
        <td><asp:TextBox ID="NumeProbaTextBox" runat="server" Text='<%# Bind("NumeProba") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nume Client:</td>
        <td> <asp:TextBox ID="FabriciNumeTextBox" runat="server" Text='<%# Bind("FabriciNume") %>'>
            </asp:TextBox></td>
        <td class=heading bgcolor=cornflowerblue>Adresa Client:</td>
        <td>  <asp:TextBox ID="FabriciStradaTextBox" runat="server" Text='<%# Bind("FabriciStrada") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class=heading bgcolor=cornflowerblue>Localitate:</td>
        <td>  <asp:TextBox ID="FabriciOrasTextBox" runat="server" Text='<%# Bind("FabriciOras") %>'>
            </asp:TextBox></td>
        <td class=heading bgcolor=cornflowerblue>Judet:</td>
        <td>   <asp:TextBox ID="FabriciJudetTextBox" runat="server" Text='<%# Bind("FabriciJudet") %>'>
            </asp:TextBox></td>
        </tr>
          <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr> 
        <tr>
        <td class=heading bgcolor=cornflowerblue>Definitiv:</td>
        <td><asp:CheckBox ID="DefinitivCheckBox" runat="server" Checked='<%# Bind("Definitiv") %>' />
        </td>
        <td class=heading bgcolor=cornflowerblue>Validat:</td>
        <td><asp:CheckBox ID="ValidatCheckBox" runat="server" Checked='<%# Bind("Validat") %>' />
        </td>
        </tr> 
         <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
         <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        
       
         <tr>
        <td></td>
        <td>   <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Font-Bold=true ForeColor=SeaGreen Font-Size=12pt
                Text="Salvare">
            </asp:LinkButton></td>
        <td>  <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold=true ForeColor=SeaGreen Font-Size=12pt
                Text="Renuntare">
            </asp:LinkButton></td>
        <td></td>
        </tr>
         <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        </table>
           
       
       
       
       
       
        </InsertItemTemplate>
        <ItemTemplate>
       <table border=1 cellspacing=2 cellpadding=2 width=700>
        <tr>
        <td class="heading" bgcolor=cornflowerblue>Cod Bare:</td>
        <td width=200><asp:TextBox ID="CodBareTextBox" runat="server" Text='<%# Bind("CodBare") %>'>
            </asp:TextBox>
          </td>
        <td></td>
        <td width=200></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Id Zilnic:</td>
        <td> <asp:TextBox ID="IdZilnicTextBox" runat="server" Text='<%# Bind("IdZilnic") %>'>
            </asp:TextBox><br /></td>
        <td class="heading" bgcolor="cornflowerblue">Prelevator:</td>
        <td> <asp:TextBox   ID=PrelevatorIDTextBox runat=server   Text='<%# Bind("PrelevatoriId") %>' Width="150px">
         </asp:TextBox> 

          </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr height=20px>
        <td class="heading" bgcolor="cornflowerblue">Data primirii:</td>
        <td><asp:TextBox ID="DataPrimiriiTextBox" runat="server" Text='<%# Bind("DataPrimirii") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Data prelevarii:</td>
        <td> <asp:TextBox ID="DataPrelevareTextBox" runat="server" Text='<%# Bind("DataPrelevare") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Data testarii:</td>
        <td><asp:TextBox ID="DataTestareTextBox" runat="server" Text='<%# Bind("DataTestare") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Data testarii finale</td>
        <td> <asp:TextBox ID="DataTestareFinalaTextBox" runat="server" Text='<%# Bind("DataTestareFinala") %>'>
            </asp:TextBox></td>
        </tr>
         <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
         <tr>
        <td class="heading" bgcolor="cornflowerblue">Grasime:</td>
        <td><asp:TextBox ID="GrasimeTextBox" runat="server" Text='<%# Bind("Grasime") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Proteina:</td>
        <td><asp:TextBox ID="ProteinaTextBox" runat="server" Text='<%# Bind("Proteina") %>'>
            </asp:TextBox>
          </td>  
        </tr>
        
         <tr>
        <td class="heading" bgcolor="cornflowerblue">Cazeina:</td>
         <td><asp:TextBox ID="CaseinaTextBox" runat="server" Text='<%# Bind("Casein") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">pH:</td>
        <td>
        <asp:TextBox ID="PhTextBox" runat="server" Text='<%# Bind("Ph") %>'>
            </asp:TextBox>
        </td>
        </tr>
        
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Lactoza:</td>
        <td><asp:TextBox ID="LactozaTextBox" runat="server" Text='<%# Bind("Lactoza") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Subst. uscata:</td>
        <td> <asp:TextBox ID="SubstuTextBox" runat="server" Text='<%# Bind("Substu") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Punct inghet:</td>
        <td>    <asp:TextBox ID="PunctiTextBox" runat="server" Text='<%# Bind("Puncti") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Inhibitori:</td>
        <td> 
        <asp:DropDownList  ID=AntibDropDownList runat="server"  SelectedValue='<%# Bind("Antib") %>' Width="150px">
         <asp:ListItem Text="" Value=""></asp:ListItem>
         <asp:ListItem Text="Negativ" Value="0"></asp:ListItem>
         <asp:ListItem Text="Pozitiv" Value="1"></asp:ListItem> 
         </asp:DropDownList> 
        </td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">NCS:</td>
        <td> <asp:TextBox ID="NCSTextBox" runat="server" Text='<%# Bind("NCS") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">NTG:</td>
        <td>  <asp:TextBox ID="NTGTextBox" runat="server" Text='<%# Bind("NTG") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Urea:</td>
        <td> <asp:TextBox ID="UreaTextBox" runat="server" Text='<%# Bind("Urea") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Sms trimis:</td>
        <td><asp:CheckBox ID="SentsmsCheckBox" runat="server" Checked='<%# Bind("Sentsms") %>' /> 
        </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nr. Comanda:</td>
        <td> <asp:TextBox ID="NrComandaTextBox" runat="server" Text='<%# Bind("NrComanda") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Cant. Prelevata</td>
        <td>
            <asp:TextBox ID="CantTextBox" runat="server" Text='<%# Bind("Cant") %>'>
            </asp:TextBox>
        </td>
        </tr>
        <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nume Prelevator:</td>
        <td> <asp:TextBox ID="NumePrelevatorTextBox" runat="server" Text='<%# Bind("PrelevatoriNume") %>'>
            </asp:TextBox></td>
        <td class="heading" bgcolor="cornflowerblue">Nume Proba:</td>
        <td><asp:TextBox ID="NumeProbaTextBox" runat="server" Text='<%# Bind("NumeProba") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class="heading" bgcolor="cornflowerblue">Nume Client:</td>
        <td> <asp:TextBox ID="FabriciNumeTextBox" runat="server" Text='<%# Bind("FabriciNume") %>'>
            </asp:TextBox></td>
        <td class=heading bgcolor=cornflowerblue>Adresa Client:</td>
        <td>  <asp:TextBox ID="FabriciStradaTextBox" runat="server" Text='<%# Bind("FabriciStrada") %>'>
            </asp:TextBox></td>
        </tr>
        <tr>
        <td class=heading bgcolor=cornflowerblue>Localitate:</td>
        <td>  <asp:TextBox ID="FabriciOrasTextBox" runat="server" Text='<%# Bind("FabriciOras") %>'>
            </asp:TextBox></td>
        <td class=heading bgcolor=cornflowerblue>Judet:</td>
        <td>   <asp:TextBox ID="FabriciJudetTextBox" runat="server" Text='<%# Bind("FabriciJudet") %>'>
            </asp:TextBox></td>
        </tr>
          <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr> 
        <tr>
        <td class=heading bgcolor=cornflowerblue>Definitiv:</td>
        <td><asp:CheckBox ID="DefinitivCheckBox" runat="server" Checked='<%# Bind("Definitiv") %>' />
        </td>
        <td class=heading bgcolor=cornflowerblue>Validat:</td>
        <td><asp:CheckBox ID="ValidatCheckBox" runat="server" Checked='<%# Bind("Validat") %>' />
        </td>
        </tr> 
         <tr bgcolor=silver>
        <td height=5px></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
         <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        
       
       
         <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        </tr>
        </table>
           
         
         
         </ItemTemplate>
        <HeaderStyle Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="SeaGreen" />
        <EditRowStyle Font-Bold="True" Font-Names="Arial" Font-Size="10pt" />
    </asp:FormView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMostra"
         DeleteMethod="DeleteMostra" InsertMethod="InsertMostra" UpdateMethod=UpdateMostra TypeName="MostreFabrica" DataObjectTypeName="MostreFabrica">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="" Name="id" QueryStringField="id"
                Type="String" />
        </SelectParameters>
     
       </asp:ObjectDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>"
        SelectCommand="SELECT [NumePrelevator], [CodPrelevator] FROM [Prelevatori] ORDER BY [NumePrelevator]">
    </asp:SqlDataSource>
    &nbsp;
    <br />
</asp:Content>
