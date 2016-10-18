<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="eventbrowserdetail" CodeFile="eventbrowserdetail.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Event Log Details</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
	    function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Close') {
          window.close();
          oEvent.cancelPostBack = true;
        }
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
      <table style="WIdTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
					<tr valign="top" height="1">
						<td class=sectionTitle><asp:Label id="propertiesTitleLbl" runat="server"></asp:Label></td>
					</tr>
					<tr valign="top" height="1">
						<td>
								<igtbar:ultrawebtoolbar id="eventsToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="eventsToolBar_ButtonClicked">
									<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
									<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
									<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
									<Items>
  									<igtbar:TBarButton Key="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
  									<igtbar:TBarButton Key="Refresh" Text="Refresh" Image="/hc_v4/img/ed_update.gif"></igtbar:TBarButton>
  									<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
										<igtbar:TBLabel Text=" ">
											<DefaultStyle Width="100%"></DefaultStyle>
										</igtbar:TBLabel>
									</Items>
                <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							</igtbar:ultrawebtoolbar>
						</td>
					</tr>
					<tr valign="top" height="1">
						<td>
							<FIELDSET><LEGEND>Information</LEGEND>
								<table cellspacing=0 cellpadding=0 width="100%" border=0>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											<asp:label id=lbStart Text="Start at" Runat="server"></asp:label>
										</td>
										<td class=ugd>
											<asp:Label id=txtStartValue runat="server"></asp:Label>
										</td>
										<td class=editLabelCell width=130>
											<asp:label id=lbDuration Text="Duration" Runat="server"></asp:label>
										</td>
										<td class=ugd>
											<asp:Label id=txtDurationValue runat="server"></asp:Label>
										</td>
									</tr>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											<asp:label id=lbComponent Text="Batch" Runat="server"></asp:label>
										</td>
										<td class=ugd>
											<asp:Label id=txtComponentValue runat="server"></asp:Label>
										</td>
										<td class=editLabelCell width=130>
											<asp:label id=lbUser2 Text="User" Runat="server"></asp:label>
										</td>
										<td class=ugd>
											<asp:Label id=txtUserValue runat="server"></asp:Label>
										</td>
									</tr>
									<tr valign=middle>
										<td class=editLabelCell width=130>
											<asp:label id=lbResult Text="Result" Runat="server"></asp:label>
										</td>
										<td class=ugd colspan=3>
											<asp:Label id=txtResultValue runat="server"></asp:Label>
										</td>
									</tr>
								</table>
							</FIELDSET> 
</td></tr>
<tr valign="top" height="*"><td>
<div>
       		    <igtbl:UltraWebGrid id="eventsGrid" runat="server" Width="100%" Height="100%" OnInitializeRow="eventsGrid_InitializeRow" Browser="Xml">
						  <DisplayLayout AutoGenerateColumns="False" AllowRowNumberingDefault="Continuous" RowHeightDefault="100%" Version="4.00" HeaderClickActionDefault="SortSingle" Name="eventsGrid" TableLayout="Fixed" CellClickActionDefault="NotSet" NoDataMessage="No event" XmlLoadOnDemandType="Virtual" LoadOnDemand="Xml" RowSelectorsDefault="Yes">
					<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
						    <FrameStyle Width="100%" Height="100%"/>
									<RowAlternateStyleDefault CssClass="uga"/>
										<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"/>
                <ActivationObject AllowActivation="False">
                </ActivationObject>
                    <RowSelectorStyleDefault Height="100%">
                    </RowSelectorStyleDefault>
                    <ClientSideEvents XmlVirtualScrollHandler="eventsGrid_XmlVirtualScrollHandler"></ClientSideEvents>                   
									</DisplayLayout>
 
									<Bands>
																    <igtbl:UltraGridBand BorderCollapse="Collapse" ColFootersVisible="Yes">

											<Columns>
												<igtbl:TemplatedColumn HeaderText="Occurred on" Key="Date" DataType="System.DateTime" Width="135px" BaseColumnName="DateTime">
                          <Header Caption="Occurred on">
                          </Header>
												</igtbl:TemplatedColumn>
												<igtbl:UltraGridColumn Key="JobId" BaseColumnName="JobId" ServerOnly="True">
                          <Header>
                            <RowLayoutColumnInfo OriginX="1" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="1" />
                          </Footer>
                        </igtbl:UltraGridColumn>
												<igtbl:TemplatedColumn Key="Severity" Width="20px" BaseColumnName="Severity">
													<CellTemplate>
														<img src='<%# severityImgSrc((HyperCatalog.Business.EventBrowser.Severities)Container.Value)%>' alt='<%# Container.Value%>'/>
													</CellTemplate>
                          <Header>
                            <RowLayoutColumnInfo OriginX="2" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="2" />
                          </Footer>
												</igtbl:TemplatedColumn>
												<igtbl:TemplatedColumn Key="EventId" BaseColumnName="EventId" ServerOnly="True">
                          <Header>
                            <RowLayoutColumnInfo OriginX="3" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="3" />
                          </Footer>
                        </igtbl:TemplatedColumn>
												<igtbl:UltraGridColumn HeaderText="Event" Key="EventName" Width="60px" BaseColumnName="EventName">
                          <Header Caption="Event">
                            <RowLayoutColumnInfo OriginX="4" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="4" />
                          </Footer>
                        </igtbl:UltraGridColumn>
												<igtbl:UltraGridColumn HeaderText="Geo" Key="GeoCode" Width="40px" BaseColumnName="GeoCode">
                          <Header Caption="Geo">
                            <RowLayoutColumnInfo OriginX="5" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="5" />
                          </Footer>
                        </igtbl:UltraGridColumn>
												<igtbl:TemplatedColumn HeaderText="ItemId" Key="ItemId" Width="50px" BaseColumnName="ItemId">
													<CellTemplate>
														<%# ((int)Container.Value)<0?"-":Container.Text%>
													</CellTemplate>
                          <Header Caption="ItemId">
                            <RowLayoutColumnInfo OriginX="6" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="6" />
                          </Footer>
												</igtbl:TemplatedColumn>
												<igtbl:TemplatedColumn HeaderText="OtherId" Key="OtherId" Width="52px" BaseColumnName="OtherId">
													<CellTemplate>
														<%# ((int)Container.Value)<0?"-":Container.Text%>
													</CellTemplate>
                          <Header Caption="OtherId">
                            <RowLayoutColumnInfo OriginX="7" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="7" />
                          </Footer>
												</igtbl:TemplatedColumn>
												<igtbl:UltraGridColumn HeaderText="Message" Key="Text" Width="200px" BaseColumnName="Text">
                          <Header Caption="Message">
                            <RowLayoutColumnInfo OriginX="8" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="8" />
                          </Footer>
                        </igtbl:UltraGridColumn>
												<igtbl:UltraGridColumn HeaderText="Source" Key="Source" Width="150px" BaseColumnName="Source">
                          <Header Caption="Source">
                            <RowLayoutColumnInfo OriginX="9" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="9" />
                          </Footer>
                        </igtbl:UltraGridColumn>
												<igtbl:UltraGridColumn HeaderText="Detail" Key="Detail" Width="100%" BaseColumnName="Detail">
                          <Header Caption="Detail">
                            <RowLayoutColumnInfo OriginX="10" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="10" />
                          </Footer>
                        </igtbl:UltraGridColumn>
												<igtbl:UltraGridColumn Key="Empty" Width="100%">
                          <Header>
                            <RowLayoutColumnInfo OriginX="11" />
                          </Header>
                          <Footer>
                            <RowLayoutColumnInfo OriginX="11" />
                          </Footer>
                        </igtbl:UltraGridColumn>
											</Columns>
                      <AddNewRow View="NotSet" Visible="NotSet">
                      </AddNewRow>
                      <FilterOptions AllString="" EmptyString="" NonEmptyString="">
                        <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                          CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px"
                          Width="200px">
                          <Padding Left="2px" />
                        </FilterDropDownStyle>
                        <FilterHighlightRowStyle BackColor="#151C55" ForeColor="#FFFFFF">
                        </FilterHighlightRowStyle>
                      </FilterOptions>
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