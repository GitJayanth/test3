<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="user_products" CodeFile="user_products.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Products</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
	<script type="text/javascript">
		function webTree_NodeChecked(treeId, nodeId, bChecked){
	    var node =  igtree_getNodeById(nodeId);
		  var tree = igtree_getTreeById(treeId);
		  alert(node.getText());
    }
	  function uwToolbar_Click(oToolbar, oButton, oEvent){
	    if (oButton.Key == 'List') {
        back();
        oEvent.cancelPostBack = true;
      }
	  } 
	</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr height="1" valign="top">
					<td colspan="2">
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td colspan="2">
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td class="sectiontitle">
						<center>Items</center>
					</td>
					<td class="sectiontitle">
						<center>User scope</center>
					</td>
				</tr>
				<tr valign="top" height="100%">
					<td noWrap width="1">
						<ignav:ultrawebtree id="webTree" runat="server" WebTreeTarget="HierarchicalTree" LoadOnDemand="Manual"
							ImageDirectory="/hc_v4/ig/" Indentation="10" CollapseImage="ig_treeXPMinus.gif" ExpandImage="ig_treeXPPlus.gif"
							Width="250px" Height="100%" CheckBoxes="True" BorderWidth="1px" BorderStyle="inset" BackColor="#F0F0F0"
							RenderAnchors="True" DefaultImage="s_l.gif" DefaultSelectedImage="s_l.gif" InitialExpandDepth="1" DataKeyOnClient="True" DataMember="ItemId">
							<SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
							<NodeStyle CssClass="hc_node"></NodeStyle>
							<HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
							<NodePaddings Top="2px"></NodePaddings>
							<Levels>
								<ignav:Level Index="0" LevelCheckBoxes="True"></ignav:Level>
								<ignav:Level Index="1"></ignav:Level>
							</Levels>
							<AutoPostBackFlags NodeExpanded="False"></AutoPostBackFlags>
						</ignav:ultrawebtree>
					</td>
					<td>
						<igtbl:UltraWebGrid id="dg" runat="server" Visible="False" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
								<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand BaseTableName="UserItems" Key="UserItems" BorderCollapse="Collapse" DataKeyField="ItemId">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="Id" Key="ItemId" Width="50px" BaseColumnName="ItemId"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" Width="100%" BaseColumnName="ItemName"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Level" Key="LevelName" Width="100px" BaseColumnName="LevelName"></igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
					</td>
				</tr>
			</table>
</asp:Content>