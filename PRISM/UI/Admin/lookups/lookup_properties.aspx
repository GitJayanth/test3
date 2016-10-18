<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Lookup_Properties" CodeFile="Lookup_Properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		    if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
		  } 
		  
		  function DisplayTab(lgId)
		  {
				var webTab = parent.igtab_getTabById(parent.webtab);
				var tabProp = webTab.Tabs['Properties'];
				var tabValue = webTab.Tabs['Values'];
				tabProp.setTargetUrl('./lookups/Lookup_Properties.aspx?lg='+lgId);
				tabValue.setTargetUrl('./lookups/Lookup_Values.aspx?lg='+lgId);
				tabValue.setVisible(true);
		  }
			</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving button issue  by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<CLIENTSIdEEVENTS Click="uwToolbar_Click"></CLIENTSIdEEVENTS>
							<ITEMS>
								<igtbar:TBarButton Text="List" Image="/hc_v4/img/ed_back.gif" ToolTip="Back to list" Key="List"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Text="Save" Image="/hc_v4/img/ed_save.gif" Key="Save"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Text="Delete" Image="/hc_v4/img/ed_delete.gif" ToolTip="Delete from library" Key="Delete"></igtbar:TBarButton>
							</ITEMS>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<asp:panel id="panelId" runat="server" Visible="False">
									<tr valign="middle">
										<td class="editLabelCell" width="77">
											<asp:Label id="Label1" runat="server">Id</asp:Label></td>
										<td class="uga" width="393">
											<asp:textbox id=txtGroupId runat="server" width="30px" Enabled="False">
											</asp:textbox></td>
									</tr>
							</asp:panel>
							<tr valign="middle">
								<td class="editLabelCell" width="77"><asp:label id="Label3" runat="server">Name</asp:label></td>
								<td class="ugd" width="393">
									<asp:textbox id=txtGroupName runat="server" width="250px" MaxLength="100"></asp:textbox>
									<asp:requiredfieldvalidator id="rv1" runat="server" CssClass="errorMessage" ErrorMessage="*" ControlToValidate="txtGroupName"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="77"><asp:label id="Label4" runat="server">Comment</asp:label></td>
								<td class="uga" width="393"><asp:textbox id=txtComment runat="server" Visible="true" MaxLength="1000" TextMode="MultiLine" rows="2" Columns="60"></asp:textbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="77"><asp:label id="Label2" runat="server">MultiChoice</asp:label></td>
								<td class="ugd" width="393"><asp:checkbox id=cbMultiChoice runat="server"></asp:checkbox></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>