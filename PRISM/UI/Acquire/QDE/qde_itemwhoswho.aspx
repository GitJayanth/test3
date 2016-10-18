<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" CodeFile="qde_itemwhoswho.aspx.cs"
  Inherits="HyperCatalog.UI.Acquire.QDE.qde_itemwhoswho"%>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Item Who's who</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Close') 
		    {
					oEvent.cancelPostBack = true;
          window.close();
        }
		  } 
		  
		  function OpenDetail(id, c, u)
      {
	      var url = 'qde_itemwhoswhodetail.aspx?i='+id+'&c='+c+'&u='+u; 
        winWhosWhodetail = OpenModalWindow(url, "cr2", 500, 800, 'yes');
      }
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr class="selectlanguage">
      <td>
        &nbsp;<asp:Label ID="lItemName" runat="server">CurrentItemName</asp:Label>
      </td>
      <td align="right">
        <asp:Label ID="lItemLevel" runat="server">CurrentItemLevel</asp:Label>&nbsp;</td>
    </tr>
    <tr valign="top" style="height: 1px">
      <td colspan="2">
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
          CssClass="hc_toolbar">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar">
          </ClientSideEvents>
          <Items>
            <igtbar:TBarButton Image="/hc_v4/img/ed_cancel.gif" Text="Close" ToolTip="Close window"
              Key="Close">
            </igtbar:TBarButton>
          </Items>
        </igtbar:UltraWebToolbar>
        <asp:Panel runat="server" ID="panelUsers">
        </asp:Panel>
        <asp:Label ID="lbWhosWho" runat="server" Height="100%" Width="100%"></asp:Label></td>
    </tr>
    <tr valign="top">
      <td>
        <igmisc:WebPanel ID="WebPanel1" runat="server" Visible="false">
        </igmisc:WebPanel>
      </td>
    </tr>
  </table>
  <asp:Label ID="lbError" runat="server" Visible="false"></asp:Label>
</asp:Content>
