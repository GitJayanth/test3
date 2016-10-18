<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="PL_Add.aspx.cs" Inherits="UI_Admin_pls_PL_Add" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Add PL Code</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		    if (oButton.Key == 'List') 
		    {
				back();
				oEvent.cancelPostBack = true;
			}
			if (oButton.Key == 'Save') 
			{
				oEvent.cancelPostBack = MandatoryFieldMissing();
			}
					
		  } 
		</script>
</asp:Content>
<%--Removed the width property in ultrawebtoolbar to fix enlarge button issue by Radha S--%>
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
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height: 1px">
					<td>
					  <br/>
						<asp:label id="lbMsg" runat="server" CssClass="hc_error" Visible="False"></asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr valign="middle">
								<td class="editLabelCell" width="130"><asp:label id="lCode" runat="server">GBU Name</asp:label></td>
								<td class="ugd">
								<asp:dropdownlist id="ddlGBUName" runat="server" width="400" ></asp:dropdownlist>
								</td>
								<tr>
								<td class="editLabelCell" width="130"><asp:label id="lbBusName" runat="server">Business Name</asp:label></td>
								<td class="ugd">
								<asp:dropdownlist id="ddlBusName" runat="server" width="400"></asp:dropdownlist>
								</td>
								</tr>
								<tr>
								<td class="editLabelCell" width="130"><asp:label id="lbPLCode" runat="server">PL Code</asp:label></td>
								<td class="ugd">
								<asp:TextBox id="txtPLCode" runat="server" Width="400"></asp:TextBox>
								</td>
								</tr>
								<tr>
								<td class="editLabelCell" width="130"><asp:label id="lbPLName" runat="server">PL Name</asp:label></td>
								<td class="ugd">
								<asp:TextBox id="txtPLName" runat="server" Width="400"></asp:TextBox>
								</td>
								</tr>
								<tr>
								<td class="editLabelCell" width="130"><asp:label id="lbIsActive" runat="server">IsActive</asp:label></td>
								<td class="ugd">
								<asp:CheckBox id="chkIsActive" runat="server"/>
								</td>
								</tr>
								<tr>
								<td class="editLabelCell" width="130"><asp:label id="lblPMActive" runat="server">PM Active</asp:label></td>
								<td class="ugd">
								<asp:CheckBox id="chkPMActive" runat="server"/>
								</td>
								</tr>
							</tr>
							
						</table>
					</td>
				</tr>
			</table>
</asp:Content>


