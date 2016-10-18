<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" CodeFile="eventbrowser.aspx.cs" Inherits="EventBrowser"%>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="System.Web.UI" tagprefix="cc1" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Event logs</asp:Content>
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
  function eventsGrid_XmlVirtualScrollHandler(gridName, topRowNo)
  {
      var grid=igtbl_getGridById(gridName);
      label.style.display="";
      label.style.left=grid.event.x - 50;
      label.style.top=grid.event.y - grid.MainGrid.offsetHeight;
      //window.status = grid.event.y + ', ' + grid.MainGrid.offsetHeight + ' -> ' + label.style.top; 
      label.innerHTML= topRowNo +"/" + totalRows; 
      if(timeoutID>0)
          window.clearTimeout(timeoutID);
      timeoutID=window.setTimeout("label.style.display='none'",grid.VirtualScrollDelay*3);
  }

</script>
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing=0 cellpadding=0 border=0>
						<tr valign=top height=1>
					    <td>
							<igtbar:ultrawebtoolbar id="mainToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="mainToolBar_ButtonClicked">
								<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
								<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
								<Items>
									<igtbar:TBLabel Text="Filter on" Key="filter">
										<DefaultStyle Font-Bold="True" Width="80px"></DefaultStyle>
									</igtbar:TBLabel>
									<igtbar:TBCustom ID="TBCustom1" Width="200px" Key="componentList" runat="server">
										<asp:DropDownList runat="server" Width="200px" ID="componentList" AutoPostBack="True"></asp:DropDownList>
									</igtbar:TBCustom>
									<igtbar:TBSeparator Key="filterSep"></igtbar:TBSeparator>
									<igtbar:TBarButton Key="LastWeek" Text="Last week" Image="/hc_v4/img/event_lastweek.png" ToggleButton="True">
										<DefaultStyle Width="100px"></DefaultStyle>
									</igtbar:TBarButton>
									<igtbar:TBarButton Key="Yesterday" Text="Yesterday" Image="/hc_v4/img/event_yesterday.png" ToggleButton="True"></igtbar:TBarButton>
									<igtbar:TBarButton Key="Today" Text="Today" Image="/hc_v4/img/event_today.png" ToggleButton="True"></igtbar:TBarButton>
									<igtbar:TBarButton Key="Advanced" Text="Advanced" Image="/hc_v4/img/igmenu_scrolldown.gif"></igtbar:TBarButton>
									<igtbar:TBSeparator></igtbar:TBSeparator>
									<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
									<igtbar:TBLabel Text=" ">
										<DefaultStyle Width="100%"></DefaultStyle>
									</igtbar:TBLabel>
								</Items>
								<ClientSideEvents Click="mainToolBar_Click" InitializeToolbar="mainToolBar_InitializeToolbar"></ClientSideEvents>
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
									<igtbar:TBarButton Key="ApplyDates" Text="Apply" Image="/hc_v4/img/ed_OK.gif">
                                    </igtbar:TBarButton>
									<igtbar:TBLabel Text=" ">
										<DefaultStyle Width="100%"></DefaultStyle>
									</igtbar:TBLabel>
								</Items>
								<ClientSideEvents InitializeToolbar="showHideAdvancedToolBar();" Click="advToolBar_Click"/>
							</igtbar:ultrawebtoolbar>
						</td>
					</tr>
					<tr valign=top>
						<td Height="100%">						
						<asp:Label ID="lError" runat="server" Text="errorMessage" Visible="false"></asp:Label>
            <div>
    		    <igtbl:UltraWebGrid id="eventsGrid" runat="server" Height="100%" OnInitializeRow="eventsGrid_InitializeRow" Browser="Xml" >
						  <DisplayLayout AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00" HeaderClickActionDefault="SortSingle" Name="eventsGrid" TableLayout="Fixed" CellClickActionDefault="NotSet" NoDataMessage="No event" XmlLoadOnDemandType="Virtual" LoadOnDemand="Xml" RowSelectorsDefault="No">
						 	<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
						    <%--<FrameStyle Width="100%" Height="100%"/>--%>
						    <RowAlternateStyleDefault CssClass="uga">
          </RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
          </RowStyleDefault>
                <ActivationObject AllowActivation="False">
                </ActivationObject>
                <ClientSideEvents XmlVirtualScrollHandler="eventsGrid_XmlVirtualScrollHandler"></ClientSideEvents>                   
					    </DisplayLayout>
					    <Bands >
						    <igtbl:UltraGridBand BorderCollapse="Collapse" ColFootersVisible="Yes">
                <Columns>
								  <igtbl:UltraGridColumn Key="StartDate" BaseColumnName="StartDate" HeaderText="Occurred on" Width="200px" DataType="System.DateTime"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="Component" BaseColumnName="AppComponentName" HeaderText="Batch" Width="200px"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="Duration" HeaderText="duration"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="Result" HeaderText="Result" BaseColumnName="Status"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="Events" HeaderText="Events"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="JobId" BaseColumnName="JobId" ServerOnly="true"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="HasInfos" BaseColumnName="HasInfos" ServerOnly="true"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="HasWarnings" BaseColumnName="HasWarnings" ServerOnly="true"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="HasErrors" BaseColumnName="HasErrors" ServerOnly="true"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="EndsWithFailure" BaseColumnName="EndsWithFailure" ServerOnly="true"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="EndDate" BaseColumnName="EndDate" ServerOnly="true"></igtbl:UltraGridColumn>
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
