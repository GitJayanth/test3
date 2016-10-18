<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="capability_Properties" CodeFile="capability_properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
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
		  } 
      function uwMenu_NodeChecked(treeId, nodeId, bChecked)
      {
        var selectedNode = igtree_getNodeById(nodeId);
        var childNodes = selectedNode.getChildNodes();
        for (n in childNodes) {
          childNodes[n].setChecked(bChecked);
          //childNodes[n].setEnabled(!bChecked);
        }
      }
    </script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<tr valign="bottom" height="*">
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
					<fieldset>
						<legend>
							<asp:Label id="Label2" runat="server">Mist</asp:Label>
						</legend>
						<table cellspacing="0" cellpadding="0" border="0" width="100%">
							<tr valign="middle">
								<td class="editLabelCell">
									<asp:Label id="Label3" runat="server">Name</asp:Label>
								</td>
								<td class="uga">
									<asp:textbox id="txtCapabilityName" runat="server" Visible="true" MaxLength="50" Columns="50"
										Enabled="False" Width="383px"></asp:textbox>
								</td>
							</tr>
							<tr valign="top">
								<td class="editLabelCell">
									<asp:Label id="Label5" runat="server">Description</asp:Label>
								</td>
								<td class="ugd">
									<asp:textbox id="txtDescription" runat="server" Visible="true" MaxLength="500" TextMode="MultiLine"
										Rows="2" Columns="60" Enabled="False"></asp:textbox>
								</td>
							</tr>
						</table>
					</fieldset>
				</td>
			</tr>
			</table>
			<fieldset><legend><asp:Label id="Label1" runat="server">Menu options</asp:Label></legend>
				<ignav:UltraWebTree id="uwMenu" runat="server" CheckBoxes="True" WebTreeTarget="ClassicTree" InitialExpandDepth="3">
					<SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
					<NodeStyle CssClass="hc_node"></NodeStyle>
					<HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
					<NodePaddings Top="2px"></NodePaddings>
					<Levels>
						<ignav:Level Index="0"></ignav:Level>
						<ignav:Level Index="1"></ignav:Level>
					</Levels>
					<AutoPostBackFlags NodeExpanded="False" NodeChecked="False"></AutoPostBackFlags>
					<Styles>
						<ignav:Style Cursor="Hand" BorderWidth="0px" Font-Size="xx-small" Font-Names="Verdana,Tahoma,Arial"
							BorderColor="#F0F0F0" BorderStyle="Solid" CssClass="DefaultItemClass">
							<Padding Bottom="0px" Left="1px" Top="3px" Right="0px"></Padding>
						</ignav:Style>
						<ignav:Style Cursor="Hand" BorderWidth="1px" Font-Size="xx-small" Font-Names="Verdana,Tahoma,Arial"
							BorderColor="Black" BorderStyle="Solid" ForeColor="Black" BackColor="#E5FFE5" CssClass="HiliteClass">
							<Padding Bottom="0px" Left="1px" Top="3px" Right="0px"></Padding>
						</ignav:Style>
						<ignav:Style Cursor="Hand" BorderWidth="1px" Font-Size="xx-small" Font-Names="Verdana,Tahoma,Arial"
							BorderColor="Gray" BorderStyle="Solid" ForeColor="Black" BackColor="Silver" CssClass="HoverClass">
							<Padding Bottom="0px" Left="1px" Top="3px" Right="0px"></Padding>
						</ignav:Style>
					</Styles>
					<ClientSideEvents NodeChecked="uwMenu_NodeChecked"></ClientSideEvents>
				</ignav:UltraWebTree>
		  </FIELDSET>
</asp:Content>