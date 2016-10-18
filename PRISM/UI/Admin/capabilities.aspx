<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Capabilities" CodeFile="Capabilities.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Capabilities</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script>
    function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
    }
	</script>
    <%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
	<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">Capabilities list</asp:label></td>
		</tr>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<Items>
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox CssClass="Search" Visible="True" Width="150px" Id="txtFilter" MaxLength="50"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
						</Items>
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<tr valign="top" style="height:1px">
				<td>
					<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label></td>
			</tr>
			<tr valign="top">
				<td>
                <%--Removed the css property and added inline property in headerstyledefault to fix infragestic issue by Radha S--%>
					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" BackColor="LightGray" Cursor="Hand"> <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </FrameStyle>
							<RowAlternateStyleDefault CssClass="uga">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="Capabilities" Key="Capabilities" BorderCollapse="Collapse" DataKeyField="Id">
								<Columns>
									<igtbl:UltraGridColumn Key="Id" ServerOnly="False" Width="20px" HeaderText="Id" BaseColumnName="Id"></igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="160px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name"></Footer>
										<Header Key="Name" Caption="Name"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description"
										CellMultiline="Yes">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="Description"></Footer>
										<Header Key="Description" Caption="Description"></Header>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label></CENTER>
				</td>
			</tr>
		</asp:panel>
		<tr valign="top">
			<td>
						<igtab:ultrawebtab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
							BorderWidth="1px" BorderColor="#949878" width="100%" Height="101%" LoadAllTargetUrls="False" BarHeight="0"
							ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
							<DefaultTabStyle Height="25px" Font-Size="8pt" Font-Names="Microsoft Sans Serif" ForeColor="Black"
								BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="ig_tab_lightb2.gif"
								NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
					<Tabs>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties" Key="Properties">
							<ContentPane TargetUrl="./capabilities/capability_Properties.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab></td>
		</tr>
	</table>
</asp:Content>
