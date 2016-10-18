<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_CrossSell" CodeFile="QDE_CrossSell.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Regionalize</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
			function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
        if (oButton.Key == 'Close')
        {
					oEvent.CancelPostBack = true;
					window.close();
        }
  	  }
  	  
  	  function UpdateAndClose()
  	  {
  			if (opener)
  			{
  				opener.document.location = 'QDE_ItemAbout.aspx';
  			}
  			window.close();
  	  }
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="main" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="25px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Text="Remove" Key="Apply" ToolTip="Remove Cross Sell" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
								<igtbar:TBarButton Text="Cancel" Key="Close" ToolTip="Close window" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="middle" align="center" height="100%">
					<td><asp:label id="lbTitle" Runat="server" CssClass="hc_itemname"></asp:label></td>
				</tr>
				<tr valign="top">
					<td><asp:label id="lbError" CssClass="hc_error" Visible="False" Runat="server"></asp:label></td>
				</tr>
				<tr valign="bottom" align="left">
					<td><asp:checkbox id="cbIncludeManual" Runat="Server"></asp:checkbox></td>
				</tr>
			</table>
</asp:Content>