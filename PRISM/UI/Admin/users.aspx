<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Users" CodeFile="Users.aspx.cs"%>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Users</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script>
		  function dg_BeforeSortColumnHandlerDateSelect(gridName, columnId){
		     var myCol = igtbl_getColumnById(columnId);
         return ((myCol.Key == "Active") || (myCol.Key == "LastLogin"));        
		  }
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
		} 
  </script>
  <table class="main" cellspacing="0" cellpadding="0" width="100%">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">PageTitle</asp:label>
			</td>
		</tr>
        <%--Removed width property from ultrawebtoolbar to fix moving icon issue by Radha S--%>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
                            <asp:HyperLink id="hyperlink1"  Text="HPP Admin" Font-Bold="false"  ForeColor="black" Font-Size="7pt" Target="_new" runat="server"/>
                            <igtbar:TBSeparator Key="Sep0"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox Width="150px" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<%--<igtbar:TBLabel Text="Locked users">
								<DefaultStyle Width="100px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>--%>
							<igtbar:TBCustom Width="150px" Key="filterLocked">
<%--								<asp:CheckBox id="cbFilterLock" runat="server" AutoPostBack="True"></asp:CheckBox>
--%>							</igtbar:TBCustom>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar>
			<tr valign="top">
				<td>

					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%" OnInitializeRow="dg_InitializeRow">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
                            <%--Removed css style and added inline property for grid by Radha S--%>
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="Users" Key="Users" BorderCollapse="Collapse" DataKeyField="Id">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="UserId" Key="Id" ServerOnly="True" BaseColumnName="Id"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Active" Key="Active" Width="45px" Type="CheckBox" BaseColumnName="IsActive"></igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="180px" HeaderText="Name" BaseColumnName="FullName" CellMultiline="Yes">
											<CellTemplate>
									   <asp:LinkButton id="ln" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
											</CellTemplate>
                  </igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Organization" Key="OrgName" Width="220px" BaseColumnName="OrgName"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Role" Key="Role" Width="220px" BaseColumnName="RoleName"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn AllowRowFiltering="False" HeaderText="Last Login" DataType="System.DateTime" Key="LastLogin" Width="130px" BaseColumnName="LastLogOnDate"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn AllowRowFiltering="False" HeaderText="#Logged" Key="Logged" Width="75px" BaseColumnName="LogCount"></igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
					</CENTER>
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
						<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="./User/user_Properties.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_notify.gif" Text="Notifications">
							<ContentPane TargetUrl="./User/user_notifications.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_translate.gif" Text="Catalogs" Key="Cultures">
							<ContentPane TargetUrl="./User/user_localizations.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_items.gif" Text="Items" Key="Items">
							<ContentPane TargetUrl="./User/user_PLs.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
</asp:Content>
