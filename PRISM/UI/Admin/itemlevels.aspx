<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.ItemLevels" CodeFile="ItemLevels.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Item levels</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script>
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
    }
	</script>
	<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle"><asp:label id="lbTitle" runat="server">Item levels</asp:label></td>
		</tr>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
<%--							<igtbar:TBarButton Text="Add" Key="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
--%>							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox cssClass="Search" Width="150px" Id="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
						</Items>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
					</igtbar:ultrawebtoolbar>
				</td>
			</tr>
			<tr valign="top" style="height:1px">
				<td><asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error"></asp:Label></td>
			</tr>
			<tr valign="top">
				<td>
					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
						<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="ItemLevels" Key="ItemLevels" BorderCollapse="Collapse" DataKeyField="LevelId">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="40px" BaseColumnName="LevelId">
										<CellStyle HorizontalAlign="Center"></CellStyle>
										<Footer Key="Id"></Footer>
										<Header Key="Id" Caption="Id"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="200px" HeaderText="Name" BaseColumnName="LevelName" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Optional" Key="Optional" Width="100px" Type="CheckBox" BaseColumnName="Optional">
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="SkuLevel" Key="SkuLevel" Width="100px" Type="CheckBox" BaseColumnName="SkuLevel">
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label></CENTER>
				</td>
			</tr>
		</asp:panel>
		<tr>
			<td>
				<igtab:ultrawebtab id="webTab" runat="server" Width="100%" DummyTargetUrl="/hc_v4/pleasewait.htm" visible="False"
					LoadAllTargetUrls="False" BorderStyle="None" BorderColor="Gray" Height="100%" BorderWidth="1px">
					<DefaultTabStyle Height="25px" BackColor="WhiteSmoke"></DefaultTabStyle>
					<RoundedImage SelectedImage="ig_tab_lightb2.gif" NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
					<Tabs>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="./itemlevels/itemlevels_properties.aspx" Visible="True"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab></td>
		</tr>
	</table>
</asp:Content>
