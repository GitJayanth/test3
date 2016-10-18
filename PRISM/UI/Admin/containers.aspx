<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Containers" CodeFile="Containers.aspx.cs" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Containers</asp:Content>
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
	<table class="main" cellspacing="0" cellpadding="0">
		<tr height="1">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">Containers</asp:label></td>
		</tr>
        <%--Removed width propery in UltraWebToolbar to fix the horizontal width issue By Radha S--%>
		<asp:panel id="panelGrid" Runat="server">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
  					<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Group">
								<DefaultStyle Width="45px" Font-Bold="True" TextAlign="Left"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="300px">
								<asp:DropDownList id="ddlContainerGroup" runat="server" Width="300px"></asp:DropDownList>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="btnChangeGroup" Text="" Image="/hc_v4/img/bt_arrow_grey.gif">
							<DefaultStyle Width ="20" />
							</igtbar:TBarButton>						
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<tr valign="top" style="height:auto">
				<td>
					<igtbl:ultrawebgrid id="dg" runat="server" Width="100%" ondatabinding="dg_DataBinding">
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
							<igtbl:UltraGridBand Key="Id">
								<Columns>
									<igtbl:UltraGridColumn Key="Id" BaseColumnName="Id" ServerOnly="True">
										<Footer Key="Id"></Footer>
										<Header Key="Id"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Tag" Width="130px" HeaderText="xmlname" BaseColumnName="Tag" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Tag"></Footer>
										<Header Key="Tag" Caption="Tag"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:TemplatedColumn Key="Name" Width="250px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Ln" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name"></Footer>
										<Header Key="Name" Caption="Name"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Definition" Key="Definition" Width="100%" BaseColumnName="Definition"
										CellMultiline="Yes">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="Definition"></Footer>
										<Header Key="Definition" Caption="Definition"></Header>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:ultrawebgrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label></CENTER>
				</td>
			</tr>
		</asp:panel>
		<asp:panel id="panelTab" Runat="server" Visible="false">
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
							<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
								<ContentPane TargetUrl="./containers/container_Properties.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Key="InputForms" DefaultImage="/hc_v4/img/ed_inputforms.gif" Text="Input forms">
								<ContentPane TargetUrl="./containers/container_inputforms.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Key="Usage" DefaultImage="/hc_v4/img/ed_usage.gif" Text="Usage">
								<ContentPane TargetUrl="./containers/container_usage.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Key="ContainerDependencies" Text="Dependencies">
								<ContentPane TargetUrl="./containers/container_dependencies.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Key="PossibleValues" DefaultImage="/hc_v4/img/ed_alternatives.gif" Text="Possible values">
								<ContentPane TargetUrl="./containers/container_possiblevalues.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
						</Tabs>
					</igtab:ultrawebtab></td>
			</tr>
		</asp:panel>
	</table>
</asp:Content>
