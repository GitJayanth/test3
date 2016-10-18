<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="datatype_properties" CodeFile="datatype_properties.aspx.cs" %>
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
          oEvent.cancelPostBack = !confirm("Are you sure you want to delete this data type ?");
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
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="label1" runat="server">Code</asp:Label></td>
								<td class="ugd">
									<asp:textbox id="txtDataTypeCode" runat="server" width="30px" MaxLength="1"></asp:textbox>
									<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtDataTypeCode"
										CssClass="errorMessage"></asp:requiredfieldvalidator></td>
							</tr>
							<TR>
								<td class="editLabelCell" width="125">
									<asp:Label id="Label2" runat="server">Data Type</asp:Label></td>
								<td class="uga">
									<asp:textbox id="txtDataType" runat="server" Visible="true" width="180px"></asp:textbox>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" ErrorMessage="*" ControlToValidate="txtDataType"
										CssClass="errorMessage"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="Label3" runat="server">Description</asp:Label></td>
								<td class="ugd">
									<asp:textbox id="txtDescription" runat="server" Visible="true" TextMode="MultiLine" rows="2"
										Columns="60"></asp:textbox>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" ErrorMessage="*" ControlToValidate="txtDescription"
										CssClass="errorMessage"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="Label6" runat="server">Regular expression</asp:Label></td>
								<td class="uga">
									<asp:textbox id="txtRegularExpression" runat="server" Visible="true" Width="300" MaxLength="300"></asp:textbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="Label4" runat="server">Example</asp:Label></td>
								<td class="ugd">
									<asp:textbox id="txtExample" runat="server" Visible="true" TextMode="MultiLine" rows="2" Columns="60"></asp:textbox>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" ErrorMessage="*" ControlToValidate="txtExample"
										CssClass="errorMessage"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="Label5" runat="server">Comment</asp:Label></td>
								<td class="uga">
									<asp:textbox id="txtComment" runat="server" Visible="true" TextMode="MultiLine" rows="2" Columns="60"></asp:textbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="Label8" runat="server">Input type</asp:Label></td>
								<td class="ugd">
									<asp:textbox id="txtInputType" runat="server" Visible="true" width="180px"></asp:textbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="125">
									<asp:Label id="Label7" runat="server">Active</asp:Label></td>
								<td class="uga">
									<asp:CheckBox id="cbIsActive" runat="server"></asp:CheckBox>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>