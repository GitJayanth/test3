<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Menus" CodeFile="Menus.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Menus</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <STYLE type="text/css">
    .Level1Item 
    {
	    BACKGROUND-IMAGE: url(/hc_v4/img/menu_back_ns.gif); WIDTH: 135px; COLOR: black; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BORDER-COLLAPSE: collapse; HEIGHT: 20px; BORDER-BOTTOM-STYLE: none;font-size: 9pt;
    }
    .Level2Item 
    {
	    BORDER-TOP-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px; BACKGROUND-IMAGE: url(/hc_v4/img/menuspacer.gif); BORDER-BOTTOM-WIDTH: 0px; WIDTH: 135px; BACKGROUND-REPEAT: repeat-y; BORDER-COLLAPSE: collapse; BORDER-RIGHT-WIDTH: 0px;color:black;font-size:8pt;
    }
  </STYLE>
  <table class="main" width="100%" border="0">
    <TR>
      <td>
        <asp:Label id="lbMenus" runat="server">Menu Table</asp:Label></td>
    </tr>
  </table>
</asp:Content>
