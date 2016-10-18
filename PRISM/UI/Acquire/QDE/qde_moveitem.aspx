<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE_MoveItem" CodeFile="QDE_MoveItem.aspx.cs" Async="True"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Move product</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
			function uwToolbar_Click(oToolbar, oButton, oEvent){
				if (oButton.Key == 'Cancel') {
				  oEvent.cancelPostBack = true
					window.close();
				}
			} 
			function UpdateTV()
			{
        if (opener)
        {
          opener.ReloadFrames(window)
        }
        else
        {
          window.close();        
        }
			}
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<igtbar:ultrawebtoolbar id="Ultrawebtoolbar1" runat="server" CssClass="hc_toolbartitle" ItemWidthDefault="80px"
				width="100%">
				<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				<ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
				<Items>
					<igtbar:TBLabel Text="ItemName" Key="ItemName">
						<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
					</igtbar:TBLabel>
				</Items>
			</igtbar:ultrawebtoolbar><igtbar:ultrawebtoolbar id="MainToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%">
				<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
				<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				<Items>
					<igtbar:TBarButton Key="Prev" ToolTip="Return to previous Step" Text="Previous" Image="/hc_v4/img/ed_Prev.gif"></igtbar:TBarButton>
					<igtbar:TBarButton ImageAlign="Right" Key="Next" ToolTip="Go to next Step" Text="Next" Image="/hc_v4/img/ed_Next.gif"></igtbar:TBarButton>
					<igtbar:TBarButton Key="Apply" ToolTip="Add Item" Text="Apply" Image="/hc_v4/img/ed_savefinal.gif"></igtbar:TBarButton>
					<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
					<igtbar:TBarButton Key="Cancel" ToolTip="Close window" Text="Cancel" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
				</Items>
				<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
			</igtbar:ultrawebtoolbar>
			<table class="datatable" cellspacing="0" cellpadding="4" width="100%" border="0">
				<TR>
					<td class="sectionTitle" colspan="2"><B><asp:label id="lbWizardTitle" Runat="server">Summary</asp:label></B></td>
				</tr>
			</table>
			<asp:label id="lbError" CssClass="hc_error" Runat="server" Visible="false"></asp:label>
			<asp:panel id="pMain" runat="server">
				<asp:label id="lbParents" runat="server" Font-Bold="True"></asp:label>
				<asp:repeater id="rPossibleParents" runat="server">
					<HeaderTemplate>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 308px">
							<table width="100%" cellpadding="0" cellspacing="0" class=main>
					</HeaderTemplate>
					<ItemTemplate>
						<tr height="20px">
							<td width="20px">
								<asp:Label ID="lbRabioButton" Runat="server" Visible="False"></asp:Label>
							<td>
							<td>
								<asp:Label ID="lbParent" Runat="server"></asp:Label></td>
						</tr>
					</ItemTemplate>
					<FooterTemplate>
						</table> </div>
					</FooterTemplate>
				</asp:repeater>
			</asp:panel><asp:panel id="pInputForms" runat="server">
				<asp:Label id="lInputforms" runat="server" Width="100%"></asp:Label>
			</asp:panel><asp:panel id="pCulturesInfos" runat="server"></asp:panel><asp:panel id="pSummarize" runat="server"></asp:panel>
</asp:Content>