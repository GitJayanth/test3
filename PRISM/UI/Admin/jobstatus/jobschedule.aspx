<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HyperCatalog.master" CodeFile="jobschedule.aspx.cs" Inherits="UI_Admin_jobstatus_jobschedule" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Job Schedule</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script type="text/javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		    
		 } 
		  
	</script>
	<table class="main" cellspacing="0" cellpadding="0">
    <%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<IGTBAR:ULTRAWEBTOOLBAR id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
						<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
						<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
						<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
						<ITEMS>
							<IGTBAR:TBARBUTTON Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></IGTBAR:TBARBUTTON>
							<IGTBAR:TBSEPARATOR Key="AddSep"></IGTBAR:TBSEPARATOR>
							<IGTBAR:TBARBUTTON Text="Export" Image="/hc_v4/img/ed_download.gif" Key="export"></IGTBAR:TBARBUTTON>
							<IGTBAR:TBSEPARATOR></IGTBAR:TBSEPARATOR>
							
						</ITEMS>
						<CLIENTSIDEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIDEEVENTS>
					</IGTBAR:ULTRAWEBTOOLBAR></td>
			</tr>
			<tr valign="top">
				<td>
					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
                            <%--removed the css class and added the styles inline--Start%>--%>
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true">  <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="JobSchedule" Key="JobSchedule" BorderCollapse="Collapse" DataKeyField="JobName">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="AppComponentId" Key="AppComponentId" Width="120px" BaseColumnName="AppComponentId">
										
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="JobName" Width="100%" HeaderText="JobName" BaseColumnName="JobName" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
									
										
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="JobType" Key="JobType" Width="110px" BaseColumnName="JobType">			
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="ScheduledDay" Key="ScheduledDay" Width="120px" BaseColumnName="ScheduledDay">			
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="ScheduledTime (GMT) " Key="ScheduledTime" Width="120px" BaseColumnName="ScheduledTime">			
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="EstimatedFinishTime" Key="EstimatedFinishTime" Width="120px" BaseColumnName="EstimatedFinishTime">			
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="CutOffTime (GMT)" Key="CutOffTime" Width="120px" BaseColumnName="CutOffTime">			
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="IsActive" Key="IsActive" Width="100px" BaseColumnName="IsActive">			
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<center>
						<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label></center>
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
								<ContentPane TargetUrl="./jobscheduleedit.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							
						</Tabs>
					</igtab:ultrawebtab></td>
			</tr>
		</asp:panel>
	</table>
</asp:Content>
