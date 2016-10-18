<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.ContainerGroups" CodeFile="ContainerGroups.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Container groups</asp:Content>
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
	<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">PageTitle</asp:label></td>
                <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
			<asp:panel id="panelGrid" runat="server" Visible="True">
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
									<asp:TextBox cssClass="Search" Width="150px" Id="txtFilter" MaxLength="50"></asp:TextBox>
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
						<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
						<DisplayLayout AllowDeleteDefault="Yes" MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
							RowHeightDefault="20px" Version="4.00" HeaderClickActionDefault="SortMulti" BorderCollapseDefault="Separate"
							RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
								<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand BaseTableName="ContainerGroups" Key="ContainerGroups" BorderCollapse="Collapse"
									DataKeyField="ContainerGroupId">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="40px" BaseColumnName="Id">
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="Code" BaseColumnName="Code" Hidden=true>
										</igtbl:UltraGridColumn>
										<igtbl:TemplatedColumn Key="Name" Width="350px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
											<CellTemplate>
												<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
													<%#Container.Text%>
												</asp:LinkButton>
											</CellTemplate>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn HeaderText="Containers" Key="Containers" Width="100px" BaseColumnName="ContainersCount">
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="AbsolutePath" Key="AbsolutePath" Hidden="True" BaseColumnName="Path">
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
		<TR>
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
						<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="./datatypes/datatype_Properties.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab Key="Containers" DefaultImage="/hc_v4/img/ed_containers.gif" Text="Containers">
							<ContentPane TargetUrl="./datatypes/datatype_containers.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
</asp:Content>