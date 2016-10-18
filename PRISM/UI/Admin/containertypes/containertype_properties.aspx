<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="containertype_properties" CodeFile="containertype_properties.aspx.cs" %>
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
          oEvent.cancelPostBack = !confirm("Are you sure you want to delete this container type ?");
        }
		  } 
			</script>
</asp:Content>
<%--Removed the width property to fixed enlarged buttons by Radha S--%>
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
								<igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><br/>
						<asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%">
							<tr valign="middle">
								<td class="editLabelCell" style="width: 55px"><asp:label id="Label2" runat="server">Code</asp:label></td>
								<td class="ugd">
									<asp:textbox id="txtTypeCodeDisable" runat="server" width="30px" Visible="false" Enabled="false"></asp:textbox>
									<asp:textbox id="txtTypeCode" runat="server" width="30px" MaxLength="1"></asp:textbox>
								  <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="errorMessage" ErrorMessage="*"
										ControlToValidate="txtTypeCode"></asp:requiredfieldvalidator>
								</td>
							</tr>
							<tr>
								<td class="editLabelCell" style="width: 55px"><asp:label id="label1" runat="server">Name</asp:label></td>
								<td class="uga"><asp:textbox id="txtTypeName" runat="server" width="180px" Visible="true"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="errorMessage" ErrorMessage="*"
										ControlToValidate="txtTypeName"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="editLabelCell" style="width: 55px"><asp:label id="Label3" runat="server">Resource</asp:label></td>
								<td class="ugd"><asp:checkbox id="cbIsResource" runat="server"></asp:checkbox></td>
							</tr>
						</table>
					</td>
		    </tr>
		  </table>
<%--		  NOT MORE USED<igtxt:webmaskedit id="Webmaskedit1" runat="server" width="30px" InputMask=">L"></igtxt:webmaskedit>--%>
</asp:Content>
