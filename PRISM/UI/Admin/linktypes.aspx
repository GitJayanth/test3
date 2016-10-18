<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.LinkTypes" CodeFile="LinkTypes.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Link Types</asp:Content>
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
	    if (oButton.Key == 'List') 
	    {
        back();
        oEvent.cancelPostBack = true;
      }
      else if ((oButton.Key == 'Export') || (oButton.Key == 'Add') || (oButton.Key == 'Filter') || (oButton.Key == 'Apply'))
      {
				oEvent.cancelPostBack = false;
      }
	  } 
		  
	</script>
	<script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>
	<table class="main" cellspacing="0" cellpadding="0">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">PageTitle</asp:label></td>
		</tr>
        <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:UltraWebToolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" >
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Apply" Text="Apply new sort" Image="/hc_v4/img/ed_save.gif">
								<DefaultStyle Width="120px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
							<igtbar:TBarButton Text="Export" Image="/hc_v4/img/ed_download.gif" Key="export"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox id="txtFilter" runat="server" Width="150px" MaxLength="50" cssClass="Search"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Image="/hc_v4/img/ed_search.gif" Key="filter">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
						</Items>
						<ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
					</igtbar:UltraWebToolbar>
				</td>
			</tr>
			<tr valign="top" style="height:1px">
				<td>
					<asp:Label id="lbErrorGrid" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label></td>
			</tr>
			<tr valign="top">
				<td>
					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%" >
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
							<%--<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>--%>
                            <%-- Fix for QC# 7384 by Rekha Thomas. Added borderdetails tag to fix the bold issue --%>
                            <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh" QC#7384--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd" ></RowStyleDefault>
							<FilterOptionsDefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
                                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                    CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                    Font-Size="11px" Width="200px">
                                    <Padding Left="2px" />
                                </FilterDropDownStyle>
                                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="#FFFFFF">
                                </FilterHighlightRowStyle>
                            </FilterOptionsDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="LinkTypes" Key="LinkTypes" BorderCollapse="Collapse" DataKeyField="ItemTypeId">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="40px" BaseColumnName="Id">
										<Footer Key="Id"></Footer>
										<Header Key="Id" Caption="Id"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="200px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name"></Footer>
										<Header Key="Name" Caption="Name"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="B" Key="isBidirectional" Width="25px" Type="CheckBox" BaseColumnName="IsBidirectional">
										<CellStyle HorizontalAlign="Center"></CellStyle>
										<Footer Key="isBidirectional"></Footer>
										<Header Key="isBidirectional" Caption="B"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Icon" Key="Icon" Width="150px" BaseColumnName="Icon">
										<Footer Key="Icon"></Footer>
										<Header Key="Icon" Caption="Icon"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="Description"></Footer>
										<Header Key="Description" Caption="Description"></Header>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<center>
						<asp:Label id="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label></center>
				</td>
			</tr>
		</asp:panel>
		<tr>
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
						<igtab:Tab Key="Properties" DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="./linktypes/linktype_properties.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab Key="Items" DefaultImage="/hc_v4/img/ed_items.gif" Text="Items" Visible="False">
							<ContentPane TargetUrl="./linktypes/linktype_items.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab Key="ItemTypes" DefaultImage="/hc_v4/img/ed_itemtypes.gif" Text="ItemTypes" Visible="False">
							<ContentPane TargetUrl="./linktypes/linktype_itemtypes.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
	<input id="txtSortColPos" type="hidden" value="5" name="txtSortColPos" runat="server">
</asp:Content>