<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="itemlevels_Properties" CodeFile="itemlevels_properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
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
		</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<br/>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" border="0" width="100%">
							<asp:Panel id="panelId" Visible="False" Runat="server">
								<tr valign="middle">
									<td class="editLabelCell">
										<asp:Label id="lbLevelId" runat="server">Id</asp:Label></td>
									<td class="ugd">
										<asp:textbox id="txtLevelId" runat="server" width="30px" Enabled="false" MaxLength="1" Text="<%# levelId %>">
										</asp:textbox></td>
								</tr>
							</asp:Panel>
							<tr valign="middle">
								<td class="editLabelCell"><asp:Label id="Label3" runat="server">Name</asp:Label></td>
								<td class="uga">
									<asp:textbox id="txtLevelName" runat="server" Visible="true" MaxLength="50" Columns="50"></asp:textbox>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="errorMessage" ErrorMessage="*"
										ControlToValidate="txtLevelName"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lOptional" Runat="server">Optional</asp:label></td>
								<td class="ugd"><asp:checkbox id="cbOptional" runat="server"></asp:checkbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lSkulevel" Runat="server">Sku level</asp:label></td>
								<td class="uga"><asp:checkbox id="cbSkuLevel" runat="server"></asp:checkbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lExportName" Runat="server">Export name</asp:label></td>
								<td class="ugd"><asp:textbox id="txtExportName" runat="server" Enabled="false" MaxLength="50" Columns="50"></asp:textbox></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>