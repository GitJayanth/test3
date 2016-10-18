<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="containergroup_properties" CodeFile="containergroup_properties.aspx.cs"%>
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
          oEvent.cancelPostBack = !confirm("Are you sure this container group ?");
        }
		  } 
			</script>
</asp:Content>
<%--Removed the width property to fixed enlarged buttons by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
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
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<asp:Panel ID="panelId" Runat="server">
								<TBODY>
									<tr valign="middle">
										<td class="editLabelCell" width="100">
											<asp:label id="Label2" runat="server">Id</asp:label></td>
										<td class="ugd">
											<asp:TextBox id="txtGroupId" runat="server" Enabled="False" Width="50px"></asp:TextBox></td>
									</tr>
							</asp:Panel>
							<TR>
								<td class="editLabelCell" width="100"><asp:label id="label4" runat="server">Code</asp:label></td>
								<td class="uga"><asp:textbox id="txtGroupCode" runat="server" width="180px" Visible="true" MaxLength="5"></asp:textbox>
							</tr>
							<TR>
								<td class="editLabelCell" width="100"><asp:label id="label1" runat="server">Name</asp:label></td>
								<td class="uga"><asp:textbox id="txtGroupName" runat="server" width="180px" Visible="true"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="errorMessage" ControlToValidate="txtGroupName"
										ErrorMessage="*"></asp:requiredfieldvalidator>
									<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" CssClass="hc_error" ErrorMessage='Please, do not use the "/" character'
										ControlToValidate="txtGroupName" Display="Dynamic" ForeColor=" " ValidationExpression="^[^/]*"></asp:RegularExpressionValidator></td>
							</tr>
							<asp:Panel ID="panelParent" Runat="server">
								<TR>
									<td class="editLabelCell" width="100">
										<asp:label id="Label3" runat="server">Parent Group</asp:label></td>
									<td class="uga">
										<asp:label id="lParentName" runat="server" Visible="False"></asp:label>
										<asp:DropDownList id="ddlParentList" runat="server" Width="180px" DataTextField="Name" DataValueField="Id" AutoPostBack="True" onselectedindexchanged="ddlParentList_SelectedIndexChanged"></asp:dropdownlist>
										<asp:imagebutton id="btExpand" runat="server" CausesValidation="False" ImageUrl="/hc_v4/img/ed_new.gif"
											ImageAlign="AbsMiddle"></asp:imagebutton></td>
								</tr>
							</asp:Panel></table>
					</td>
				</tr>
			</table>
</asp:Content>