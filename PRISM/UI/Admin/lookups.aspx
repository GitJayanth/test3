<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Lookups" CodeFile="Lookups.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Lookup tables</asp:Content>
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
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
		<tr>
			<td class="sectionTitle"><asp:label id="lbTitle" runat="server">Lookup tables list</asp:label></td>
		</tr>
        <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr>
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<items>
							<igtbar:TBarButton Text="Add" Key="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Text="Export" Key="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<defaultstyle Width="40px" Font-Bold="True"></defaultstyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox id="txtFilter" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<defaultstyle Width="25px"></defaultstyle>
							</igtbar:TBarButton>
						</items>
						<clientsideevents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></clientsideevents>
					</igtbar:ultrawebtoolbar>
			<tr valign="top">
				<td class="main">
                <%--Removed the css and added inline property to fix grid line issue by Radha S--%>
					<igtbl:UltraWebGrid id="dg" runat="server" Visible="False" Width="100%" OnInitializeRow="dg_InitializeRow">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="LookupGroup" Key="LookupGroup" BorderCollapse="Collapse" DataKeyField="GroupId">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="Id" Key="GroupId" Width="25px" BaseColumnName="Id"></igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="GroupName" Width="150px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
												<%#DataBinder.Eval(Container, "Text")%>
											</asp:LinkButton>
										</CellTemplate>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Comment" Key="Comment" Width="500px" BaseColumnName="Comment"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="MultiChoice" Key="MultiChoice" Width="100px" Type="CheckBox" DataType="System.Boolean"
										BaseColumnName="MultiChoice">
										<CellStyle HorizontalAlign="Center"></CellStyle>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<center>
						<asp:Label id="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label>
					</center>
		</asp:panel>
		<asp:panel id="panelTabs" runat="server" Visible="False">
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
							<igtab:Tab Key="Properties" DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
								<ContentPane TargetUrl="./lookups/Lookup_Properties.aspx" Visible="False" BorderWidth="0px" BorderStyle="None"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Key="Values" DefaultImage="/hc_v4/img/ed_possiblevalues.gif" Text=" Values">
								<ContentPane TargetUrl="./lookups/Lookup_Values.aspx" Visible="False" BorderWidth="0px" BorderStyle="None"></ContentPane>
							</igtab:Tab>
						</Tabs>
					</igtab:UltraWebTab></td>
			</tr>
		</asp:panel>
  </table>
</asp:Content>
