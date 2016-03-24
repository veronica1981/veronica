<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ChangeUser" Title="Change User" Codebehind="ChangeUser.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
     
                <br /><asp:Label ID="username" runat="server"></asp:Label>
             
             
            
                              
                                                   
                              
                                                    
   <table style="width: 80%;  margin-top: 11px;">
    <tr>
             <td style="width: 161px" class="cls"><asp:Label ID="Label3" runat="server" Text="Adresare:"></asp:Label>
  </td>
                <td style="width: 282px">
                    <asp:TextBox runat="server" ID="Salutation" />
                    
                </td>
                </tr>
           
             <tr>
                <td style="width: 161px">
                     <asp:Label ID="Label1" runat="server" Text="Prenume:"></asp:Label>
                </td>
            
                <td style="width: 282px">
                    <asp:TextBox runat="server" ID="FirstName" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="FirstName" 
                        ErrorMessage="First name is required." />
                </td>
              </tr>
              <tr>
                <td style="width: 161px"><asp:Label ID="Label2" runat="server" Text="Nume:"></asp:Label>
  </td>
                <td style="width: 282px">
                    <asp:TextBox runat="server" ID="LastName" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="LastName" 
                        ErrorMessage="Last name is required." />
                </td>
               </tr>
               <tr>
                <td style="width: 161px"><asp:Label ID="Label4" runat="server" Text="Cod ferma:"></asp:Label>
  </td>
                <td style="width: 282px">
                <asp:DropDownList id="UserCod" runat="server"  DataTextField="Nume" DataValueField="Cod" 
                        DataSourceID="SqlDataSource1">
               
                </asp:DropDownList>
               </td>
               </tr>

               
                 <tr>
                <td style="width: 161px"><asp:Label ID="Label5" runat="server" Text="Email:"></asp:Label>
  </td>
                <td style="width: 282px">
                    <asp:TextBox runat="server" ID="Email" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="Email" 
                        ErrorMessage="Email is required." />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                </td>
            </tr>
        </table>
        
                              
    
    
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                                OnClick="btnUpdate_Click"   
            CausesValidation="False" />
                              
                                 &nbsp;&nbsp;&nbsp;                  
                  
                                <asp:Button ID="btnDelele" runat="server" Text="Delete" 
                                   
            CausesValidation="False" onclick="btnDelele_Click" />
                              
                                 &nbsp;&nbsp;&nbsp;                  
                  
                         
                                <asp:Button ID="linkHome" runat="server" Text="Back" 
                                OnClick="linkHome_Click"   />
                              
                                                   
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:fccl2ConnectionString %>" 
        SelectCommand="SELECT [Cod], [Nume] FROM [Ferme_CCL] Where ([Cod] <> '') ORDER BY [Nume]">
     
    </asp:SqlDataSource>
</asp:Content>

