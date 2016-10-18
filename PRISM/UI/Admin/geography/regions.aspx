<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Admin.Architecture.Regions" CodeFile="Regions.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Regions</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <STYLE>
    .lbl_msgInformation{font-weight:bold;color:green;}
    .lbl_msgWarning{font-weight:bold;color:orange;}
    .lbl_msgError{font-weight:bold;color:red;}
  </STYLE>
  <script>
  function UpdateCurrentRegion(RegionCode)
  {
    var webTab = null;
    if (parent)
    {
	    webTab = parent.igtab_getTabById('ctl00_HOCP_webTab'); // Master Pages since Migration to .Net 2.0
	    if (webTab != null)
	    {
	      var tabRegions = webTab.Tabs["Regions"];
	      var url =  "";
	      if (tabRegions)
	      {
	        url = "./Regions.aspx?r=" + RegionCode;
	        tabRegions.setTargetUrl(url);
	      }
	      var tabCountries = webTab.Tabs["Countries"];
	      if (tabCountries)
	      {
	        url = "./Countries.aspx?r=" + RegionCode;
	        tabCountries.setTargetUrl(url);
	      }
	    }
    }
  }
  </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
		<table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
			<asp:panel id="panelGrid" Runat="server">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="mainToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Text=" ">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
							</Items>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top">
                <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
					<td>
						<asp:Label id="mainMsgLbl" runat="server"></asp:Label>
						<igtbl:UltraWebGrid id="regionsGrid" runat="server" Width="100%">
							<DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="OnClient" RowHeightDefault="100%"
								Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect"
								NoDataMessage="No region defined" LoadOnDemand="Manual">
								<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </FrameStyle>
								<SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="Regions" BorderCollapse="Collapse" DataKeyField="Code">
									<Columns>
										<igtbl:UltraGridColumn Key="RegionCode" Hidden="true"></igtbl:UltraGridColumn>
										<igtbl:TemplatedColumn HeaderText="Code" Key="DisplayCode">
											<CellTemplate>
												<asp:LinkButton id="CodeLink" onclick="UpdateGridItem" runat="server">
													<%#Container.Text%>
												</asp:LinkButton>
											</CellTemplate>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="Name" BaseColumnName="Name" Width="300"></igtbl:UltraGridColumn>
										<igtbl:TemplatedColumn HeaderText="Countries" Key="CountriesCount" Width="70">
											<CellStyle HorizontalAlign="center"/>
											<CellTemplate>
												<%#Container.Text%>
											</CellTemplate>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn Width="100%" Key="Empty"></igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
					</td>
				</tr>
				</asp:panel>
                <%--Removed the width property from ultrawebtoolbar to fix enlarge button issue by Radha S--%>
				<asp:panel id="panelProperties" Visible="false" Runat="server">
					<tr valign="top" style="height:1px">
						<td>
							<igtbar:ultrawebtoolbar id="propertiesToolBar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
								<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
								<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
								<Items>
									<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
									<igtbar:TBSeparator></igtbar:TBSeparator>
									<igtbar:TBarButton Key="Save" ToolTip="Save changes" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
									<igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
									<igtbar:TBarButton Key="Delete" ToolTip="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
								</Items>
							</igtbar:ultrawebtoolbar>
					<tr valign="top">
						<td>
							<asp:Label id="propertiesMsgLbl" runat="server"></asp:Label>
							<table cellspacing="0" cellpadding="0" width="100%" border="0">
								<tr valign="middle">
									<td class="editLabelCell" style="width:130px">
										<asp:label id="lbRegionCode" Runat="server" Text="Code"></asp:label></td>
									<td class="ugd">
										<asp:textbox id="txtRegionCodeValue" runat="server" enabled="false" width="180px" MaxLength="5"></asp:textbox></td>
								</tr>
								<tr valign="middle" runat="server" >
									<td class="editLabelCell" style="width:130px">
										<asp:label id="lbRegionCode2" Runat="server" Text="Code2"></asp:label></td>
									<td class="ugd">
										<asp:textbox id="txtRegionCode2Value" runat="server" enabled="false" width="180px" MaxLength="2"></asp:textbox></td>
								</tr>
								<tr valign="middle">
									<td class="editLabelCell" style="width:130px">
										<asp:label id="lbRegionName" Runat="server" Text="Name"></asp:label></td>
									<td class="ugd">
										<asp:textbox id="txtRegionNameValue" Enabled="false" runat="server" width="180px"></asp:textbox></td>
								</tr>
								<tr valign="middle">
									<td class="editLabelCell" style="width:130px">
										<asp:label id="lbParentRegionCode" Runat="server" Text="Part of region"></asp:label></td>
									<td class="ugd">
										<asp:dropdownlist id="txtParentRegionCodeValue" runat="server" width="180px"></asp:dropdownlist></td>
								</tr>
								 <tr valign="middle">
                                <td class="editLabelCell" style="width: 130px">
                                <asp:Label ID="Label4" runat="server" Text="Publishable"></asp:Label></td>
                                <td class="ugd">
                                <asp:CheckBox ID="cbPublishable" runat="server" /></td>                                
                                </tr>
                               <tr valign="middle">
                               <td class="editLabelCell" style="width: 130px">
                               <asp:Label ID="Label1" runat="server" Text="Fallback To English"></asp:Label></td>
                               <td class="ugd">
                               <asp:CheckBox ID="cbFallbackToEnglish" runat="server" /></td>
                              </tr>
							</table>
				</asp:panel>
		</table>
</asp:Content>