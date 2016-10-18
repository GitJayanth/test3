<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="BundlesInconsistencies.aspx.cs" Inherits="UI_Acquire_BundlesInconsistencies" %>
<%@ Register TagPrefix="ucs" TagName="PLWebTree" Src="../Admin/PLWebTree.ascx" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Bundles Report</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">

  <script type="text/javascript" language="JavaScript" src="/hc_v4/js/spell.js"></script>

  <script type="text/javascript" language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

	<script type="text/javascript" language="javascript">
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
  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top" style="height: 1px">
      <td>
        <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" itemwidthdefault="80px"
          cssclass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked1">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
							
							<Items>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox CssClass="Search" Visible="True" Width="150px" Id="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
									<DefaultStyle Width="25px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBarButton ToggleButton="True" Key="OnlyError" SelectedImage="/hc_v4/img/ed_checked.jpg"
									Text="Only inconsistencies" Image="/hc_v4/img/ed_unchecked.jpg" Selected="True">
									<SelectedStyle ForeColor="Black"></SelectedStyle>
									<DefaultStyle Width="150px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBCustom Width="150px">
									<asp:Button ID="btGenerate" runat="server" Text="Generate" OnClick="btGenerate_Click"></asp:Button>
								</igtbar:TBCustom>
							</Items>
						</igtbar:ultrawebtoolbar>
      </td>
    </tr>
    <tr valign="top">
        <td>
        <igmisc:webpanel id="wPanelPL" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img" Width="100%" >
				<Header Text="PL Selection">
					<ExpandedAppearance>
                        <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
				    <ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
                    <CollapsedAppearance>
                        <Style CssClass="hc_webpanelcol"></Style>
                    </CollapsedAppearance>
				</Header>
          <Template>
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr valign="top">
                    <td class="editLabelCell" style="width: 80px">
                        <asp:Label ID="Label7" runat="server">Product Lines</asp:Label>
                    </td>
                    <td class="uga">
                       <ucs:PLWebTree id="PLTree" runat="server" Expanded="false"/>
                    </td>
                </tr>
            </table>
            </Template>
        </igmisc:webpanel>
        </td>
    </tr>
    <tr valign="top">
      <td valign="top">
        <igtbl:ultrawebgrid id="dg" runat="server" visible="False" OnInitializeRow="dg_InitializeRow">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="No" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
								 <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                <%-- Fix for QC# 7386 by Nisha Verma Added borderdetails tag to fix the gridlines missing issue --%>
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
                                <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --Start --%>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
                                <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --End --%>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand BorderCollapse="Collapse">
									<Columns>
										<igtbl:UltraGridColumn Key="BundleId" Hidden="True" BaseColumnName="BundleId">
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="BundleName" Hidden="True" BaseColumnName="BundleName"  CellMultiline="Yes">
									 <CellStyle Wrap="true"></CellStyle>
    									
    									</igtbl:UltraGridColumn>
											
    									<igtbl:UltraGridColumn Key="PLCode"  BaseColumnName="PLCode" HeaderText ="PLCode" CellMultiline="Yes" Width ="120px">
										 <CellStyle Wrap="true" HorizontalAlign ="Center"></CellStyle>
    									</igtbl:UltraGridColumn>

										<igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId"  CellMultiline="Yes">
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="CountryCode" HeaderText="Country code" Hidden="True" BaseColumnName="CountryCode">
										<CellStyle Wrap="true"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="CountryFlag" Width="25px">
										  <CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="ItemName" HeaderText="Product Name" Width="100%" BaseColumnName="ItemName"  CellMultiline="Yes">
										<CellStyle Wrap="true"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="ItemNumber" HeaderText="Product Number" Width="100px" BaseColumnName="ItemNumber"  CellMultiline="Yes">
										<CellStyle Wrap="true"></CellStyle>
										</igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn Key="Status" HeaderText="Product Status" Width="50px" BaseColumnName="Status">
                    <CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="PID" HeaderText="PID" Width="100px" BaseColumnName="PID">
										  <CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="POD" HeaderText="POD" Width="100px" BaseColumnName="POD">
										  <CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="Anomaly" HeaderText="Anomaly" Width="200px" BaseColumnName="Anomaly" CellMultiline="Yes">
										 <CellStyle ForeColor="red" Wrap="true"></CellStyle>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
        <center>
          <asp:Label ID="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label></center>
      </td>
    </tr>
  </table>
</asp:Content>
