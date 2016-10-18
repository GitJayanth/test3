<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" CodeFile="language_properties.aspx.cs" Inherits="language_properties" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Localization properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		    if (oButton.Key == 'List') {
					back();
					oEvent.cancelPostBack = true;
				}
				if (oButton.Key == 'Save') {
					oEvent.cancelPostBack = MandatoryFieldMissing();
				}
					if (oButton.Key == 'Delete') {
					oEvent.cancelPostBack = !confirm("Do you really want to delete this culture ?");
				}
		  } 
		</script>
</asp:Content>
<%--Removed the width propert form ultrawebtoolbar to fix enlarged button issue--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr style="height: 1px" valign="top">
					<td>
					  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ImageDirectory=" " ItemWidthDefault="80px"
							CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" ToolTip="Save Analyse in library" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height: 1px">
					<td>
					  <br/>
						<asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lCode" runat="server">Code</asp:label></td>
								<td class="ugd">
								<asp:textbox id="txtLanguageCode" runat="server" width="50px" MaxLength="3"></asp:textbox>
								<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtLanguageCode"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lName" runat="server">Name</asp:label></td>
								<td class="uga"><asp:textbox id="txtLanguageName" runat="server" width="250px" MaxLength="100"></asp:textbox>
								<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtLanguageName"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="Label1" runat="server">Delivery name</asp:label></td>
								<td class="uga"><asp:textbox id="txtDeliveryLanguageName" runat="server" width="250px" MaxLength="100"></asp:textbox>
								<asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtDeliveryLanguageName"></asp:requiredfieldvalidator></td>
							</tr>
							
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lRtl" runat="server">Rtl</asp:label></td>
								<td class="ugd">
                  <asp:CheckBox ID="cbRtl" runat="server" /></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lEncoding" runat="server">Encoding</asp:label></td>
								<td class="uga"><asp:textbox id="txtEncoding" runat="server" width="250px" MaxLength="100"></asp:textbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell"><asp:label id="lTRIsoCode" runat="server">TR Iso code</asp:label></td>
								<td class="ugd"><asp:dropdownlist id="dlTRIsoCode" runat="server" width="180px" DataTextField="TRIsoName" DataValueField="TRIsoCode"></asp:dropdownlist></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>