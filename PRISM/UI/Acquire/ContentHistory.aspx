<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" CodeFile="ContentHistory.aspx.cs" Inherits="UI_Acquire_ContentHistory"%>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ucs" TagName="PLWebTree" Src="../Admin/PLWebTree.ascx" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Content History</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
<script type="text/javascript">
  var stdToolBar = null;
	function mainToolBar_Click(oToolbar, oButton, oEvent)
	{
		if (oButton.Key=="Advanced")
		{
			oEvent.cancelPostBack=true;
			stdToolBar = oToolbar;
			showHideAdvancedToolBar();
		}
		if (oButton.Key=="Today")
		{
			oToolbar.Items.fromKey("LastWeek").setSelected(false);
			oToolbar.Items.fromKey("Yesterday").setSelected(false);
		}
		if (oButton.Key=="Yesterday")
		{
			oToolbar.Items.fromKey("LastWeek").setSelected(false);
			oToolbar.Items.fromKey("Today").setSelected(false);
		}
		if (oButton.Key=="LastWeek")
		{
			oToolbar.Items.fromKey("Today").setSelected(false);
			oToolbar.Items.fromKey("Yesterday").setSelected(false);
		}
	}
	function advToolBar_Click(oToolbar, oButton, oEvent)	
	{
		stdToolBar.Items.fromKey("Today").setSelected(false);
		stdToolBar.Items.fromKey("Yesterday").setSelected(false);
        stdToolBar.Items.fromKey("LastWeek").setSelected(false);
    }
	
	function showHideAdvancedToolBar()
	{
		var toolbar = igtbar_getToolbarById('<%= advancedToolBar.ClientID %>');
		if (toolbar!=null)
			toolbar.Element.style.display = toolbar.Element.style.display=='none'?'':'none';
	}
  var label=null;
  var timeoutID=0;
</script>
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
						<tr valign=top height=1>
					    <td>
							<igtbar:ultrawebtoolbar id="mainToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="mainToolBar_ButtonClicked">
								<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
								<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
								<Items>
									<igtbar:TBarButton Key="MineOnly" Text="Only my events" Image="/hc_v4/img/ed_unchecked.jpg" SelectedImage="/hc_v4/img/ed_checked.jpg" ToggleButton="true">
									   <DefaultStyle Width="120px"></DefaultStyle>
									</igtbar:TBarButton>
									<igtbar:TBSeparator Key="filterSep"></igtbar:TBSeparator>
									<igtbar:TBarButton Key="LastWeek" Text="Last week" ToggleButton="true" Image="/hc_v4/img/event_lastweek.png">
										<DefaultStyle Width="100px"></DefaultStyle>
									</igtbar:TBarButton>
									<igtbar:TBarButton Key="Yesterday" Text="Yesterday" ToggleButton="true"  Image="/hc_v4/img/event_yesterday.png" ></igtbar:TBarButton>
									<igtbar:TBarButton Key="Today" Text="Today"  ToggleButton="true" Image="/hc_v4/img/event_today.png"></igtbar:TBarButton>
									<igtbar:TBarButton Key="Advanced" Text="Advanced" Image="/hc_v4/img/igmenu_scrolldown.gif"></igtbar:TBarButton>
									<igtbar:TBSeparator></igtbar:TBSeparator>
									<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
									<igtbar:TBLabel Text=" ">
										<DefaultStyle Width="100%"></DefaultStyle>
									</igtbar:TBLabel>
									
								</Items>
								<ClientSideEvents Click="mainToolBar_Click"></ClientSideEvents>
							</igtbar:ultrawebtoolbar>
							<igtbar:ultrawebtoolbar id="advancedToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="advancedToolBar_ButtonClicked">
								<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
								<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
								<Items>
									<igtbar:TBLabel Text="Between">
										<DefaultStyle Font-Bold="True" Width="80px"></DefaultStyle>
									</igtbar:TBLabel>
									<igtbar:TBCustom Width="120px" Key="afterDate">
										<igsch:WebDateChooser id="startDate" runat="server" Editable="false"></igsch:WebDateChooser>
									</igtbar:TBCustom>
									<igtbar:TBLabel Text="and">
										<DefaultStyle Font-Bold="True" Width="50px"></DefaultStyle>
									</igtbar:TBLabel>
									<igtbar:TBCustom Width="120px" Key="beforeDate">
										<igsch:WebDateChooser id="endDate" runat="server" Editable="false"></igsch:WebDateChooser>
									</igtbar:TBCustom>
									<igtbar:TBarButton Key="ApplyDates" Text="Apply" Image="/hc_v4/img/ed_OK.gif"></igtbar:TBarButton>
									<igtbar:TBLabel Text=" (maximum 1 month in the past)">
										<DefaultStyle Width="100%" Font-Italic="true" TextAlign="Left"></DefaultStyle>
									</igtbar:TBLabel>
								</Items>
								<ClientSideEvents InitializeToolbar = "showHideAdvancedToolBar();" Click="advToolBar_Click"/>
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
					<tr valign=top>
						<td Height="100%">						
						<asp:Label ID="lError" runat="server" Text="errorMessage" Visible="false"></asp:Label>
                        <asp:label id="lRecordcount" runat="server" Visible="false">NbRecords</asp:label>
                        <asp:Panel id="tooManyRowsPanel" runat="server" Visible="false">
                          There are too many rows to export in formatted excel, you can:
                          <asp:Panel id="XLSPanel" runat="server" Visible="false">
                          <LI><asp:LinkButton ID="XLSLink" runat="server" Text="download the result in Excel format" OnClick="excelLink_Click"></asp:LinkButton></LI>
                          </asp:Panel>
                          <asp:Panel id="CSVPanel" runat="server">
                          <LI><asp:LinkButton ID="CSVLink" runat="server" Text="download the result in CSV format" OnClick="csvLink_Click"></asp:LinkButton></LI>
                          </asp:Panel>
                        </asp:Panel>
                        <div>
           <igtbl:UltraWebGrid ID="eventsGrid" runat="server" Width="100%" Height="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True">
            </Pager>
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
											    <igtbl:UltraGridColumn Key="Action" BaseColumnName="ActionType" HeaderText="Action" Width="130px">
                            <Header Caption="Action">
                            </Header>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="CultureCode" BaseColumnName="CultureCode" ServerOnly="True" Width="0px">
                            <Header>
                              <RowLayoutColumnInfo OriginX="1" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="1" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" ServerOnly="True">
                            <Header>
                              <RowLayoutColumnInfo OriginX="2" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="2" />
                            </Footer>
                          </igtbl:UltraGridColumn>
                          <igtbl:UltraGridColumn Key="PLCode" BaseColumnName="PLCode" HeaderText ="PLCode" Width="100px" >
                            <Header Caption="PLCode">
                              <RowLayoutColumnInfo OriginX="3" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="3" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="ItemNumber" BaseColumnName="ItemNumber" HeaderText="Product Number" Width="100px">
                            <Header Caption="Product Number">
                              <RowLayoutColumnInfo OriginX="4" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="4" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="ItemDeleted" BaseColumnName="Deleted" Width="100px" ServerOnly="True">
                            <Header>
                              <RowLayoutColumnInfo OriginX="5" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="5" />
                            </Footer>
                          </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="ItemName" Width="100%" HeaderText="Item" BaseColumnName="ItemName" CellMultiline="Yes">
                  <Header Caption="Item">
                    <RowLayoutColumnInfo OriginX="6" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="6" />
                  </Footer>
                </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="ItemStatus" BaseColumnName="ItemStatus" HeaderText="Product Status" Width="40px">
                            <Header Caption="Product Status">
                              <RowLayoutColumnInfo OriginX="7" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="7" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:TemplatedColumn Key="Geography" HeaderText="Geography">
                            <Header Caption="Geography">
                              <RowLayoutColumnInfo OriginX="8" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="8" />
                            </Footer>
                          </igtbl:TemplatedColumn>
											    <igtbl:TemplatedColumn Key="Culture" BaseColumnName="CultureName" ServerOnly="True">
                            <Header>
                              <RowLayoutColumnInfo OriginX="9" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="9" />
                            </Footer>
                          </igtbl:TemplatedColumn>
											    <igtbl:UltraGridColumn Key="Country" BaseColumnName="CountryName" ServerOnly="True">
                            <Header>
                              <RowLayoutColumnInfo OriginX="10" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="10" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="Language" BaseColumnName="LanguageName" ServerOnly="True">
                            <Header>
                              <RowLayoutColumnInfo OriginX="11" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="11" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="Date" BaseColumnName="DateTime" HeaderText="Occurred on" Width="130px" >
                            <Header Caption="Occurred on">
                              <RowLayoutColumnInfo OriginX="12" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="12" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Key="UserName" BaseColumnName="UserName" HeaderText="User" Width="170px">
                            <Header Caption="User">
                              <RowLayoutColumnInfo OriginX="13" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="13" />
                            </Footer>
                          </igtbl:UltraGridColumn>
											    <igtbl:UltraGridColumn Width="100%" Key="Empty" ServerOnly="True">
                            <Header>
                              <RowLayoutColumnInfo OriginX="14" />
                            </Header>
                            <Footer>
                              <RowLayoutColumnInfo OriginX="14" />
                            </Footer>
                          </igtbl:UltraGridColumn>
										    </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        

<asp:Label id="Label1"  runat="server" Height="21px" Width="50px" BackColor="#FFFFC0" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" EnableViewState="False" style="position:relative;">0</asp:Label>
</div>
						</td>
					</tr>
</table>
<script type="text/javascript" language="javascript">
  label=document.getElementById('<%=Label1.ClientID%>');
  label.style.display="none";
</script>
</asp:Content>
