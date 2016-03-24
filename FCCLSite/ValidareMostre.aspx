<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ValidareMostre" Title="Validare Mostre" Codebehind="ValidareMostre.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;
    <asp:Panel ID="PanelV" runat="server" Width=90%>
    <a id="PageTop" name="PageTop"></a>
    <table style="width: 100%" border=0 cellspacing=2>
        <tr>
            <td style="width: 256px; height: 26px;">
                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="DateValidate"
                    ControlToValidate="datatestare" ErrorMessage="Data invalida!" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator></td>
            <td style="width: 185px; height: 26px;">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                    Text="Data testarii:"></asp:Label></td>
            <td style="height: 26px">
                <asp:TextBox ID="datatestare" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 256px; height: 21px;">
            </td>
            <td style="width: 185px; height: 21px;">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                    Text="Prelevator Id:"></asp:Label></td>
            <td style="height: 21px">
                <asp:TextBox ID="prelid" runat="server"></asp:TextBox></td>
        </tr>
        
        <tr>
        <td style="height: 21px">
                       	<asp:RegularExpressionValidator ID="regCod" runat="server"   Font-Size=8 ControlToValidate="codbare"
            ErrorMessage="Codul trebuie sa fie numeric <= 10 pozitii!!"   ValidationExpression="\d{1,10}"></asp:RegularExpressionValidator> 
        </td>
        <td style="height: 21px">
            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"
                Text="Cod Bare:"></asp:Label></td>
        <td style="height: 21px">
            <asp:TextBox ID="codbare" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td colspan=3>
            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                ForeColor="Red"></asp:Label></td>
        </tr>
        <tr bgcolor=seagreen>
            <td style="width: 256px; height: 26px;" align="center">
                <asp:Button ID="bedit" runat="server" Text="Editare" Font-Bold="True" Font-Names="Arial" Font-Size="Small" OnClick="Edit_Click" /></td>
            <td style="width: 185px; height: 26px;" align="center"><asp:Button ID="bsave" runat="server" Text="Salvare" Font-Bold="True" Font-Names="Arial" Font-Size="Small" OnClick="Save_Click" /></td>
            <td align="center" style="height: 26px"><asp:Button ID="babort" runat="server" Text="Abandonare" Font-Bold="True" Font-Names="Arial" Font-Size="Small" OnClick="Cancel_Click" /></td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lcount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
        Width="129px"></asp:Label><br />
    <asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
                <table id=table2 cellspacing=2 cellpadding =2 width="100%">
                <tr bgcolor="#6699cc" class="heading">
                    <td style="width: 10%; height: 36px;">
                        Cod bare</td>
                    <td style="width: 10%; height: 36px;">
                        Data testare</td>
                    <td style="width: 5%; height: 36px;" >
                        Id zilnic</td>
                    <td style="width: 5%; height: 36px;" >
                        Prel. Id</td>    
                    <td style="width: 5%; height: 36px;" >
                        <br />
                        Grasime</td>
                    <td style="width: 5%; height: 36px;" >
                        % Proteina</td>
                    <td style="width: 5%; height: 36px;" >
                        % Cazeina</td>
                    <td style="width: 5%; height: 36px;" >
                        % Lactoza</td>
                    <td style="width: 5%; height: 36px;" >
                        Subst. uscata</td>
                    <td style="width: 5%; height: 36px;" >
                        pH</td>
 
                    <td style="width: 5%; height: 36px;" >
                        Punct inghet</td>
                  
                    <td style="width: 5%; height: 36px;" >
                        Anti-<br />
                        biotice</td>
                    <td style="width: 5%; height: 36px;" >
                        Urea</td>
    
                    <td style="width: 5%; height: 36px;" >
                        NCS</td>
                    <td style="width: 5%; height: 36px;" >
                        NTG</td>
                    <td style="width: 5%; height: 36px;" >
                        Sms</td>    
                    <td style="width: 5%; height: 36px;" >
                        Definitiv</td>
                     <% if (Page.User.IsInRole("admin")) { %>   
                    <td style="width: 5%; height: 36px;">
                        Validat</td>
                        <%}else {%>
                        <td style="width: 5%; height: 36px;" bgcolor="#ffffff">
                        <%}%>
                </tr>

              </HeaderTemplate>
                  <ItemTemplate>
                    <tr bgcolor="#6699cc" class="data">
                    <td style="height: 33px;">
                        <asp:Label ID="codbare" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "codbare") %>' Width="77px"></asp:Label></td>
                    <td style="height: 33px;">
                        <asp:Label ID="datat" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "datat") %>' Width="62px"></asp:Label></td>
                        
                    <td style="height: 33px;" >
                        <asp:Label ID="idzilnic" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "idzilnic") %>'></asp:Label></td>
                        
                          <td style="height: 33px;" bgcolor="#6699cc">                                    
                        <asp:TextBox ID="prelid" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# DataBinder.Eval(Container.DataItem,"prelid") %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox>
                        </td>
                        
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificGrasime(Convert.ToString(DataBinder.Eval(Container.DataItem,"grasime"))) == 1) ? "#ff0000" : "#6699cc" %>>                                    
                        <asp:TextBox ID="grasime" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"grasime")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"grasime")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"grasime"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox>
                        </td>
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificProteine(Convert.ToString(DataBinder.Eval(Container.DataItem,"proteina"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="proteina" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"proteina")) == 0 ? "":(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"proteina")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"proteina"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                
                       <td style="height: 33px;" bgcolor=<%# (UMostre.VerificCasein(Convert.ToString(DataBinder.Eval(Container.DataItem,"casein"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="caseina" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"casein")) == 0 ? "":(Convert.ToDouble(DataBinder.Eval(Container.DataItem,"casein")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"casein"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                         
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificLactoza(Convert.ToString(DataBinder.Eval(Container.DataItem,"lactoza"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="lactoza" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"lactoza")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"lactoza")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"lactoza"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificSolids(Convert.ToString(DataBinder.Eval(Container.DataItem,"substu"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="substu" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"substu")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"substu")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"substu"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificPh(Convert.ToString(DataBinder.Eval(Container.DataItem,"ph"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="ph" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ph")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ph")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"ph"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificPctInghet(Convert.ToString(DataBinder.Eval(Container.DataItem,"pcti"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="pcti" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"pcti")) =="0")? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"pcti")) == 0.00001) ? "0" : "-0."+Convert.ToString(DataBinder.Eval(Container.DataItem,"pcti")) %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                  
                    <td style="height: 33px;" >
                        <asp:TextBox ID="antib" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# DataBinder.Eval(Container.DataItem, "antib") %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>>
                         
                        </asp:TextBox></td>
                     <td style="height: 33px;" bgcolor=<%# (UMostre.VerificUrea(Convert.ToString(DataBinder.Eval(Container.DataItem,"urea"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="urea" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"urea")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"urea")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"urea"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>    
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificNCS(Convert.ToString(DataBinder.Eval(Container.DataItem,"ncs"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="ncs" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ncs")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ncs")) == 0.00001 ) ? "0": DataBinder.Eval(Container.DataItem,"ncs"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                    <td style="height: 33px;" bgcolor=<%# (UMostre.VerificNTG(Convert.ToString(DataBinder.Eval(Container.DataItem,"ntg"))) == 1) ? "#ff0000" : "#6699cc" %>>
                        <asp:TextBox ID="ntg" runat="server" Font-Names="Arial" Font-Size="9pt" Width="43px" Text='<%# (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ntg")) == 0 ? "": (Convert.ToDouble(DataBinder.Eval(Container.DataItem,"ntg")) == 0.00001) ? "0": DataBinder.Eval(Container.DataItem,"ntg"))  %>' Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>></asp:TextBox></td>
                     <td style="height: 33px;" >
                        <asp:CheckBox ID="sentsms" runat="server"  AutoPostBack=false  Checked=<%#DataBinder.Eval(Container.DataItem,"sentsms")%>  Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>  /></td>
                    <td style="height: 33px;" >
                        <asp:CheckBox ID="definitiv" runat="server"  AutoPostBack=false  Checked=<%#(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1) ? true:false%>  Enabled=<%#((!Page.User.IsInRole("admin")) && (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"definitiv")) == 1))? false:true %>  /></td>
                    <td style="height: 33px;" bgcolor=<%# (Page.User.IsInRole("admin"))? "#6699cc":"#ffffff"%>>
                        <asp:CheckBox ID="validat" runat="server" Checked=<%#(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"validat")) == 1) ? true:false%> Visible=<%#(Page.User.IsInRole("admin"))? true:false%> /></td>


                </tr>

               
               </ItemTemplate>

                <FooterTemplate>
              </table>
              </FooterTemplate>  
    </asp:Repeater>
        <br />
        <table width=700>
            <tr>
                <td style="height: 21px; width: 300px;">
                </td>
                <td style="width: 100px; height: 21px">
                </td>
                <td style="height: 21px" align="right">
                    <asp:Button ID="Button1" runat="server" BackColor="MediumSeaGreen" Font-Bold="True"
                        Font-Names="Arial" Font-Size="10pt" OnClick="Button1_Click1" Text="Validare" /></td>
            </tr>
            <tr>
                <td>
                </td>
                <td></td>
                <td>
                <a href="ValidareMostre.aspx#PageTop" title="Top" id=top runat=server visible=false>La inceputul paginii</a> 
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <br />
   
   </asp:Panel>
    <br />
    <br />
</asp:Content>

