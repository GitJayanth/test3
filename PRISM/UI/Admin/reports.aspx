<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Reports" CodeFile="Reports.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Reports</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script>
		var selectedQuery = -1;
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
      if (oButton.Key=="Run") 
      {
        if (selectedQuery < 0)
        {
          alert("Please, select a query first");
        }
        else
        {
          var url='reports/Report_Result.aspx?r=' + selectedQuery;
          open(url,'queryprocess','toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,width=800,height=600');
        }        
        oEvent.cancelPostBack = true;
      }
		}
		
  	function dg_CellClickHandler(gridName, cellId, button){
			var cell = igtbl_getCellById(cellId);
			selectedQuery = cell.Row.getCellFromKey('Id').getValue();
    }
	</script>
	<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr height="1">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">Report List</asp:label></td>
		</tr>
		<asp:panel id="panelGrid" Runat="server">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Run" Text="Run" Image="/hc_v4/img/ed_play.gif"></igtbar:TBarButton>
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
			<tr valign="top" style="height:auto">
				<td>
					<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="Yes" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
                            <%--removed the css class and added the styles inline--Start%>--%>
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true">  <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </FrameStyle>
                            <%--removed the css class and added the styles inline--End%>--%>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							<ClientSideEvents CellClickHandler="dg_CellClickHandler" />
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand Key="Containers">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="ContainerId" Key="Id" Width="10px" Hidden="True" BaseColumnName="Id">
										<Footer Key="Id"></Footer>
										<Header Key="Id" Caption="ContainerId"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="150px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name">
											<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="Name" Caption="Name">
											<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
										</Header>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description"
										CellMultiline="Yes">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="Description">
											<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="Description" Caption="Description">
											<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
										</Header>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Owner" Key="Owner" Width="150px" BaseColumnName="" CellMultiline="Yes">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="Owner">
											<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="Owner" Caption="Owner">
											<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
										</Header>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Created on" Key="CreateDate" Width="140px" BaseColumnName="CreateDate">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="CreateDate">
											<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="CreateDate" Caption="Created on">
											<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
										</Header>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Last run on" Key="LastRun" Width="140px" BaseColumnName="LastRunDate"
										CellMultiline="Yes">
										<CellStyle Wrap="True"></CellStyle>
										<Footer Key="LastRun">
											<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="LastRun" Caption="Last run on">
											<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
										</Header>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:ultrawebgrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" ForeColor="Red" Font-Bold="True" Visible="False">No results</asp:Label></CENTER>
				</td>
			</tr>
		</asp:panel>
		<tr valign="top">
			<td>
				<igtab:ultrawebtab id="webTab" runat="server" Width="100%" Height="100%" DummyTargetUrl="/hc_v4/pleasewait.htm"
					visible="false" LoadAllTargetUrls="False" BorderWidth="1px" BorderStyle="Solid" BorderColor="#808080">
					<DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
					<ROUNDEDIMAGE FillStyle="LeftMergedWithCenter" NormalImage="ig_tab_lightb1.gif" SelectedImage="ig_tab_lightb2.gif"></ROUNDEDIMAGE>
					<TABS>
						<igtab:Tab Text="Properties" DefaultImage="/hc_v4/img/ed_properties.gif">
							<CONTENTPANE TargetUrl="./reports/report_Properties.aspx"></CONTENTPANE>
						</igtab:Tab>
						<igtab:Tab Text="Roles" DefaultImage="/hc_v4/img/ed_roles.gif">
							<CONTENTPANE TargetUrl="./reports/report_Properties.aspx"></CONTENTPANE>
						</igtab:Tab>
					</TABS>
				</igtab:ultrawebtab></td>
		</tr>
	</table>
</asp:Content>
