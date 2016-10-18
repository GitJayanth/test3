<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="pl_properties.aspx.cs" Inherits="UI_Admin_pls_pl_properties" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		  } 
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
				<tr valign="bottom" height="*">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
                                <igtbar:TBSeparator key="AddSep"></igtbar:TBSeparator>
                            <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
                            </Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<br/>
						<asp:Label id="lbMsg" runat="server" Visible="False" CssClass="hc_error"></asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" border="0" width="100%">
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbCode" runat="server">Code</asp:label></td>
							<td class="uga">
							<asp:label id="txtCode" runat="server">Code</asp:label>
							</td>
							<td class="ugd">
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbName" runat="server">Name</asp:label></td>
							<td class="ugd">
							<asp:TextBox id="txtName" runat="server" text="Name" Width="400"></asp:TextBox>
							
								</td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbPath" runat="server">Path</asp:label></td>
							<td class="uga">
							<asp:label id="txtPath" runat="server">Path</asp:label>
								</td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="Label1" runat="server">Is Active</asp:label></td>
							<td class="uga">
							<asp:CheckBox ID="cbActive" runat="server" /> <asp:label id="lbUsage" runat="server"></asp:label>
								</td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbPMActive" runat="server">PM Active</asp:label>
							</td>
							<td class="uga">
							<asp:CheckBox ID="cbPMActive" runat="server" /> 
							</td>
							
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbBusName" runat="server">Business Name</asp:label></td>
							<td class="ugd">
							<asp:DropDownList ID="ddlBusName" runat="server" width="200"></asp:DropDownList>
							</td>
						</tr>
						</table>
					</td>
				</tr>
</asp:Content>