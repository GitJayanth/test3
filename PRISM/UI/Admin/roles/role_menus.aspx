<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="role_menus" CodeFile="role_menus.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Menus</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
	<script>
    
		function dg_DblClickHandler(gridName, id, ev){
			var cell = igtbl_getCellById(id);
      var grid = igtbl_getGridById(gridName);
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
				<tr valign="top" style="height:1px">
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
					<td>
						<br/>
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
				<tr valign="top">
					<td noWrap width="1">
						<ignav:ultrawebtree id="webTree" runat="server" Width="250px" Height="100%" BorderWidth="1px" BorderStyle="inset"
							BackColor="#F0F0F0" ExpandImage="ig_treePlus2.gif" DefaultItemClass="DefaultItemClass" CollapseImage="ig_treeMinus2.gif"
							EnableViewState="False" HiliteClass="HiliteClass" Indentation="12" ImageDirectory="/ig_common/WebNavigator31/"
							HoverClass="HoverClass" CheckBoxes="True" RenderAnchors="True" LoadOnDemand="Automatic">
							<SelectedNodeStyle Cursor="Default" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid" ForeColor="Black"
								BackColor="White">
								<Padding Bottom="1px" Left="2px" Top="1px" Right="1px"></Padding>
							</SelectedNodeStyle>
							<NodeStyle Cursor="Default" BorderWidth="1px" BorderColor="#F0F0F0" BorderStyle="Solid">
								<Padding Bottom="1px" Left="1px" Top="1px" Right="1px"></Padding>
							</NodeStyle>
							<HoverNodeStyle Cursor="Default" BorderWidth="1px" BorderColor="Gray" BorderStyle="Solid" ForeColor="Black"
								BackColor="Silver">
								<Padding Bottom="1px" Left="2px" Top="1px" Right="1px"></Padding>
							</HoverNodeStyle>
							<NodePaddings Top="0px"></NodePaddings>
							<Padding Top="1px"></Padding>
							<Levels>
								<ignav:Level Index="0" LevelCheckBoxes="false"></ignav:Level>
								<ignav:Level Index="1"></ignav:Level>
							</Levels>
							<Styles>
								<ignav:Style Cursor="Default" BorderWidth="1px" BorderColor="#F0F0F0" BorderStyle="Solid" CssClass="DefaultItemClass">
									<Padding Bottom="1px" Left="1px" Top="1px" Right="1px"></Padding>
								</ignav:Style>
								<ignav:Style Cursor="Default" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid" ForeColor="Black"
									BackColor="White" CssClass="HiliteClass">
									<Padding Bottom="1px" Left="2px" Top="1px" Right="1px"></Padding>
								</ignav:Style>
								<ignav:Style Cursor="Default" BorderWidth="1px" BorderColor="Gray" BorderStyle="Solid" ForeColor="Black"
									BackColor="Silver" CssClass="HoverClass">
									<Padding Bottom="1px" Left="2px" Top="1px" Right="1px"></Padding>
								</ignav:Style>
							</Styles>
						</ignav:ultrawebtree>
					</td>
					<td>
						<igtbl:UltraWebGrid id="dg" runat="server" Visible="False" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
								SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" 
								NoDataMessage="No input forms attached to this container">
								<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="RoleMenus" BorderCollapse="Collapse">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="Id" Key="MenuId" Width="30px" BaseColumnName="MenuId"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="MenuText" Width="100%" BaseColumnName="MenuText"></igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
					</td>
				</tr>
			</table>
</asp:Content>