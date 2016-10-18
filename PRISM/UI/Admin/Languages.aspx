<%@ Reference Page="~/ui/admin/capabilities.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" CodeFile="Languages.aspx.cs" Inherits="HyperCatalog.UI.Admin.Languages" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Languages</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script>
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
      if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
		    return;
      }
    }
	</script> 
	<asp:label id="lbSpacer" runat="server" visible="False"></asp:label>
	<table class="main" cellspacing="0" cellpadding="1" width="100%" border="0">
		<tr height="1">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">PageTitle</asp:label></td>
		</tr>
		<asp:panel id="panelGrid" runat="server" Visible="False">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
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
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<tr valign="top">
				<td>
                <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
					<igtbl:UltraWebGrid id="dg" runat="server" Visible="False" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" HeaderClickActionDefault="SortSingle"
							RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
							<Pager QuickPages="10" PageSize="200" AllowPaging="True"></Pager>
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </FrameStyle>
							<ClientSideEvents MouseOverHandler="dg_MouseOverHandler" MouseOutHandler="dg_MouseOutHandler"></ClientSideEvents>
							<ActivationObject AllowActivation="False"></ActivationObject>
							<RowAlternateStyleDefault CssClass="uga">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="Localizations" Key="Localizations" BorderCollapse="Collapse" DataKeyField="CultureCode">
								<Columns>
									<igtbl:TemplatedColumn Key="Code" Width="80px" HeaderText="Code" BaseColumnName="Code" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Code"></Footer>
										<Header Key="Code" Caption="Code"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:TemplatedColumn Key="Name" Width="200px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name"></Footer>
										<Header Key="Name" Caption="Name"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Rtl" Key="Rtl" Width="50px" Type="CheckBox" DataType="System.Boolean"
										BaseColumnName="Rtl" AllowUpdate="No"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Encoding" Key="Encoding" Width="100px" BaseColumnName="Encoding"></igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<center>
						<asp:Label id="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label>
				  </center>
				</td>
			</tr>
		</asp:panel>
		<tr>
			<td>
				<igtab:UltraWebTab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
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
						<igtab:Tab Text="Properties" DefaultImage="/hc_v4/img/ed_properties.gif">
							<ContentPane TargetUrl="./localizations/localization_Properties.aspx"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:UltraWebTab>
			</td>
		</tr>
	</table>
</asp:Content>
