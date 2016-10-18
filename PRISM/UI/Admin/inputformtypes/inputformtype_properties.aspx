<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="inputformtype_properties" CodeFile="inputformtype_properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
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
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
							</Items>
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
						<table cellspacing="0" cellpadding="0" border="0" width="100%">
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="label1" runat="server">Code</asp:Label></td>
								<td class="ugd">
									<igtxt:WebMaskEdit id="txtInputFormTypeCode" runat="server" Width="30px" InputMask=">L" PromptChar=" "></igtxt:WebMaskEdit>
									<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtInputFormTypeCode"
										CssClass="errorMessage"></asp:requiredfieldvalidator>
								</td>
							</tr>
							<TR>
								<td class="editLabelCell" width="125">
									<asp:Label id="Label2" runat="server">Input form type</asp:Label></td>
								<td class="uga">
									<asp:textbox id="txtInputFormType" runat="server" Visible="true" width="180px" MaxLength="15"></asp:textbox>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" ErrorMessage="*" ControlToValidate="txtInputFormType"
										CssClass="errorMessage"></asp:requiredfieldvalidator></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>