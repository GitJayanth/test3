<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="linktype_properties" CodeFile="linktype_properties.aspx.cs" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		    else if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		    else if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
		  } 
		  
		  function DisplayTab(linktype)
		  {
				var webTab = parent.igtab_getTabById(parent.webtab);
				
				// Refresh Properties tab
				var tabProp = webTab.Tabs['Properties'];
				tabProp.setTargetUrl('./linktypes/linktype_properties.aspx?t='+linktype);
				
				// Display Items tab
				var tabItems = webTab.Tabs['Items'];
				if (tabItems)
				{
				  tabItems.setVisible(true);
				  tabItems.setTargetUrl('./linktypes/linktype_items.aspx?t='+linktype);
				}
				
				// Display Items tab
				var tabItemTypes = webTab.Tabs['ItemTypes'];
				if (tabItemTypes)
				{
				  tabItemTypes.setVisible(true);
				  tabItemTypes.setTargetUrl('./linktypes/linktype_itemtypes.aspx?t='+linktype);
				}
		  }
		  
		  function maxLengthException(textarea)
		  {
		    if (textarea.innerText.length > 255)
		    {
		      textarea.innerText = textarea.innerText.substr(0,255);
		    }
		  }
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving button issue  by Radha S--%>
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
						<igtbar:TBarButton Key="Delete" ToolTip="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
					</Items>
				</igtbar:ultrawebtoolbar></td>
		</tr>
		<tr valign="top" style="height:1px">
			<td><asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:label></td>
		</tr>
		<tr valign="top">
			<td>
				<table cellspacing="0" cellpadding="0" width="100%">
					<asp:panel id="pnlId" Runat="server">
						<tr valign="middle">
							<td class="editLabelCell" width="55"><asp:label id="lbTypeId" runat="server">Id</asp:label></td>
							<td class="ugd"><igtxt:webnumericedit id="wneTypeId" runat="server" width="50px" DataMode="Int" MinValue="0" ValueText="-1"></igtxt:webnumericedit></td>
						</tr>
					</asp:panel>
					<tr valign="middle">
						<td class="editLabelCell" width="55"><asp:label id="label1" runat="server">Name</asp:label></td>
						<td class="uga"><asp:textbox id="txtTypeName" MaxLength="50" runat="server" width="180px" Visible="true"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" CssClass="errorMessage" ErrorMessage="*"
								ControlToValidate="txtTypeName"></asp:requiredfieldvalidator></td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" width="55"><asp:label id="Label4" runat="server">Bidirectional</asp:label></td>
						<td class="ugd"><asp:checkbox id="cbBidirectional" runat="server"></asp:checkbox></td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" width="55"><asp:label id="Label3" runat="server">Icon</asp:label></td>
						<td class="uga"><asp:textbox id="txtIcon" runat="server" width="180px"  MaxLength="20"  Visible="true"></asp:textbox></td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" width="55"><asp:label id="Label6" runat="server">Description</asp:label></td>
						<td class="ugd"><asp:textbox id="txtDescription" runat="server" MaxLength="125" width="300px" Visible="true" TextMode="MultiLine"
								Rows="2"></asp:textbox>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>