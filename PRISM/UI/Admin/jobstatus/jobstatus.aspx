<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="jobstatus.aspx.cs" Inherits="UI_Admin_jobstatus_jobstatus" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Job Status</asp:Content>
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
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
						<tr valign="top" style="height:1px">
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
								<ClientSideEvents Click="mainToolBar_Click" ></ClientSideEvents>
                                <%--InitializeToolbar="mainToolBar_InitializeToolbar"--%>
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
									<igtbar:TBLabel Text=" ">
										<DefaultStyle Width="100%"></DefaultStyle>
									</igtbar:TBLabel>
								</Items>
								<ClientSideEvents InitializeToolbar="showHideAdvancedToolBar();" Click="advToolBar_Click"/>
							</igtbar:ultrawebtoolbar>
						</td>
					</tr>
					<tr valign="top">
						<td height="100%">						
						<asp:Label ID="lError" runat="server" Text="errorMessage" Visible="false"></asp:Label>
            <div>
    		    <igtbl:UltraWebGrid id="eventsGrid" runat="server" Width="100%" OnInitializeRow="eventsGrid_InitializeRow"  Browser="Xml" >

        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
          Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
          EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
          CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
          <%--removed the css class and added the styles inline--Start%>--%>
          <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true">  <%--CssClass="gh">--%>
            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
            </BorderDetails>
          </HeaderStyleDefault>
          <FrameStyle Width="100%" CssClass="dataTable">
          </FrameStyle>
          <ClientSideEvents KeyDownHandler="g_kd"></ClientSideEvents>
          <%--BeforeSortColumnHandler="dg_BeforeSortColumnHandler" removed from the above tag since it is not defined in the page and throwing error.--%>
          <RowAlternateStyleDefault CssClass="uga">
          </RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
          </RowStyleDefault>
          </DisplayLayout>
					    

					    <Bands >
						    <igtbl:UltraGridBand  ColFootersVisible="Yes">
                <Columns>
                                  <igtbl:UltraGridColumn Key="JobName" BaseColumnName="JOBNAME" HeaderText="JobName" Width="150"></igtbl:UltraGridColumn>
                                  <igtbl:UltraGridColumn Key="JobId" BaseColumnName="JOBID" HeaderText="Job Id" Width="300"></igtbl:UltraGridColumn>
                                  <igtbl:UltraGridColumn Key="StartDate" BaseColumnName="START" HeaderText="Start Date (GMT)" Width="150"></igtbl:UltraGridColumn>
                                  <igtbl:UltraGridColumn Key="EndDate" BaseColumnName="END" HeaderText="End Date (GMT)" Width="150"></igtbl:UltraGridColumn>
                                  <igtbl:UltraGridColumn Key="Status" BaseColumnName="Status" HeaderText="Status" Width="50"></igtbl:UltraGridColumn>
                                  <igtbl:UltraGridColumn Key="Comment" BaseColumnName="Comment" HeaderText="Comment" Width="450"></igtbl:UltraGridColumn>
                                  <igtbl:UltraGridColumn Key="FileName" BaseColumnName="FILENAME" HeaderText="File Name" Width="400"></igtbl:UltraGridColumn>
                                  
							  </Columns>
						  </igtbl:UltraGridBand>
					  </Bands>
					</igtbl:UltraWebGrid>

</div>
						</td>
					</tr>
</table>
</asp:Content>
