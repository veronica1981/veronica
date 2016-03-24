<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Account_SelectDataAsoc" Codebehind="SelectDataAsoc.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:Panel ID="PanelA" runat="server">
    <p>
        &nbsp;</p>
    <p style="margin-left:5px;padding-left:5px;">
        &nbsp;Pentru a vizualiza rezultatele analizelor, dati click pe iconitele Excel in lista de mai jos
        
    </p>
            
    <div>
     <table><tr><td>Arhiva:</td>
     <td>
    <asp:DropDownList ID="yearsList" runat="server" OnSelectedIndexChanged="UpdateLinks"></asp:DropDownList>
     </td></tr></table>  

   <asp:Repeater id="xlsLinks" runat="server"> 
   <ItemTemplate> 
      <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DownloadLink") %>'  Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' ImageUrl="~/Images/Excel-32-d.gif">
      </asp:HyperLink> 
      <asp:Literal ID="Filename" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Literal>
   </ItemTemplate> 
   </asp:Repeater> 
    </div>
 
    </asp:Panel>
    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
      
</asp:Content>

