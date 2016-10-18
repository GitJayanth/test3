<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="linktype_items" CodeFile="linktype_items.aspx.cs" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Items</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
        <script>
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
		    if (oButton.Key == 'List') 
				{
          back();
          oEvent.cancelPostBack = true;
        }
		    else if (oButton.Key == 'LinkToFrom') 
		    {
          oEvent.cancelPostBack = false;
        }
  }
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving button issue  by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
            <table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td colspan="2">
					  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="120px">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
									<DefaultStyle Width="80px" Height="20px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBSeparator Key="LinkToFromSep"></igtbar:TBSeparator>
                                <%--Code modified for Links Requirement (PR668013) - to change 'Hardware' to 'Host' by Prachi on 10th Dec 2012--%>
								<%--<igtbar:TBarButton Key="LinkToFrom" Text="Display hardware"></igtbar:TBarButton>--%>
                                <igtbar:TBarButton Key="LinkToFrom" Text="Display host"></igtbar:TBarButton>
                             </Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td colspan="2">
						<asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:label>
					</td>
				</tr>
				<tr valign="top">
				  <asp:Panel runat="server" ID="pnlTitleTreeView">
					<td class="sectiontitle">
						<center>
							<asp:Label id="lblTitleTree" runat="server">Possible companions</asp:Label>
						</center>
					</td>
					</asp:Panel>
					<td class="sectiontitle">
						<center>
							<asp:Label id="lblTitleGrid" runat="server">Link type scope (Companions)</asp:Label>
						</center>
					</td>
				</tr>
				<tr valign="top" height="100%">
				  <asp:Panel runat="server" ID="pnlTreeView">
					<td noWrap width="1">
						<ignav:ultrawebtree id="webTree" runat="server" WebTreeTarget="HierarchicalTree" LoadOnDemand="Manual"
							ImageDirectory="/hc_v4/ig/" Indentation="10" CollapseImage="ig_treeXPMinus.gif" ExpandImage="ig_treeXPPlus.gif"
							Width="250px" Height="100%" CheckBoxes="True" BorderWidth="1px" BorderStyle="inset" BackColor="#F0F0F0"
							RenderAnchors="True" DefaultImage="s_l.gif" DefaultSelectedImage="s_l.gif" InitialExpandDepth="1">
							<SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
							<NodeStyle CssClass="hc_node"></NodeStyle>
							<HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
							<NodePaddings Top="2px"></NodePaddings>
							<Levels>
								<ignav:Level Index="0" LevelCheckBoxes="True"></ignav:Level>
								<ignav:Level Index="1"></ignav:Level>
							</Levels>
							<AutoPostBackFlags NodeExpanded="False" NodeChecked="True"></AutoPostBackFlags>
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
						</ignav:ultrawebtree>
					</td>
					</asp:Panel>
					<td>
						<igtbl:ultrawebgrid id="dg" runat="server" Visible="False" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="Nothing in link type">
								<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh" QC#7384--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                <%-- Fix for QC# 7386 by Rekha Thomas. Added borderdetails tag to fix the gridlines missing issue --%>
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
								<RowAlternateStyleDefault CssClass="uga">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
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
						</igtbl:ultrawebgrid>
					</td>
				</tr>
			</table>
</asp:Content>