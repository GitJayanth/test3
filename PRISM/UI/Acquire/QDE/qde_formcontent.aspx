<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.qde_formcontent" CodeFile="qde_formcontent.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Content</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript" language="javascript" src="qde_formcontent.js"></script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <script id="Infragistics" type="text/javascript">
<!--

var curText;
var curCell;
function dg_AfterEnterEditModeHandler(gridName, cellId){
  curCell = igtbl_getCellById(cellId);
	curText = curCell.Element.innerHTML;
}function dg_AfterExitEditModeHandler(gridName, cellId){
  curCell.Element.innerHTML = curText;
}
function dg_EditKeyDownHandler(gridName, cellId, key){
  if(!(event.ctrlKey && key == 67))
  {
    event.returnValue=false;
    event.cancel = true;
  }
}
	function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
           
// -->
</script>
  <script type="text/javascript" language="javascript">
		  document.body.onunload = "closeWindows";
 		  document.body.scroll = "no";
  </script>
			<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="25px" width="100%" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBLabel Text="Complete list" Key="lbListType">
									<DefaultStyle Width="180px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBSeparator Key="ListTypeSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Copy" ToolTip="Copy selected content" Image="/hc_v4/img/ed_copy.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="CopySep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Paste" ToolTip="Paste content" Image="/hc_v4/img/ed_paste.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="PasteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="SpellCheck" ToolTip="Spell check" Image="/hc_v4/img/ed_spellcheck.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SpellCheckSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="MoveStatusTo" ToolTip="Move status to..." Image="/hc_v4/img/Sf.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="MoveStatusToSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" ToolTip="Delete selected content" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" ToolTip="Export current view" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="ExportSep"></igtbar:TBSeparator>
								<%-- Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09 --%>
								<igtbar:TBarButton Key="TinyTM" ToolTip="Auto Translate selected content" Image="/hc_v4/img/ed_map.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="TinyTMSep"></igtbar:TBSeparator>
								<igtbar:TBarButton ToggleButton="True" Key="MandatoryOnly" ToolTip="Filter mandatory" Image="/hc_v4/img/m.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="RegionOnlySep"></igtbar:TBSeparator>
								<igtbar:TBarButton ToggleButton="True" Key="RegionalizableOnly" ToolTip="Filter regionalizable" Image="/hc_v4/img/ed_filterr.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="RegionSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Regionalizable" ToolTip="Regionalize" Image="/hc_v4/img/ed_localize.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Text=" ">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox runat="server" Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
							<igtbl:ultrawebgrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%" OnInitializeRow="dg_InitializeRow">
								<DisplayLayout AutoGenerateColumns="False" Version="4.00" BorderCollapseDefault="Separate"
									EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<RowSelectorStyleDefault Width="30px" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</RowSelectorStyleDefault>
									<FrameStyle Width="100%"></FrameStyle>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler" AfterEnterEditModeHandler="dg_AfterEnterEditModeHandler" AfterExitEditModeHandler="dg_AfterExitEditModeHandler" EditKeyDownHandler="dg_EditKeyDownHandler"></ClientSideEvents>
									<ActivationObject AllowActivation="False" BorderColor="Orange"></ActivationObject>
                  <FilterOptionsDefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                      CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px"
                      Width="200px">
                      <Padding Left="2px" />
                    </FilterDropDownStyle>
                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                    </FilterHighlightRowStyle>
                  </FilterOptionsDefault>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BaseTableName="Roles" Key="Roles" BorderCollapse="Collapse" DataKeyField="Id">
										<Columns>
											<igtbl:TemplatedColumn Key="Select" Width="20px" FooterText="">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
												<HeaderTemplate>
													<asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox>
												</HeaderTemplate>
												<CellTemplate>
													<asp:CheckBox id="g_sd" onclick="return g_su(this);" Enabled="false" runat="server"></asp:CheckBox>
                                                    <asp:Label id="grp_lbl" Text="Hello" runat="server" Visible="false"></asp:Label>
												</CellTemplate>
                        <Footer Caption="">
                        </Footer>
											</igtbl:TemplatedColumn>
											<igtbl:UltraGridColumn Key="Index" Hidden="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="S" ServerOnly="True" BaseColumnName="Status">
                        <Header>
                          <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InputFormContainerId" Hidden="True" BaseColumnName="InputFormContainerId">
                        <Header>
                          <RowLayoutColumnInfo OriginX="3" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="3" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsMandatory" Hidden="True" BaseColumnName="Mandatory">
                        <Header>
                          <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Path" BaseColumnName="Path" ServerOnly="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Rtl" BaseColumnName="Rtl" ServerOnly="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="6" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="6" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ContainerId" Hidden="True" BaseColumnName="ContainerId">
                        <Header>
                          <RowLayoutColumnInfo OriginX="7" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="7" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="CultureCode" ServerOnly="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="8" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="8" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="sc" ServerOnly="True" BaseColumnName="CultureCode">
                        <Header>
                          <RowLayoutColumnInfo OriginX="9" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="9" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="cc" ServerOnly="True" BaseColumnName="CountryCode">
                        <Header>
                          <RowLayoutColumnInfo OriginX="10" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="10" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemId" ServerOnly="True" BaseColumnName="ItemId">
                        <Header>
                          <RowLayoutColumnInfo OriginX="11" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="11" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsResource" BaseColumnName="IsResource" ServerOnly="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="12" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="12" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsBoolean" BaseColumnName="IsBoolean" ServerOnly="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="13" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="13" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsRegionalizable" BaseColumnName="Regionalizable" ServerOnly="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="14" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="14" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn AllowUpdate="Yes" HeaderText="Inherited" Key="Inherited" Width="20px" BaseColumnName="Inherited" Hidden="True">
                        <Header Caption="Inherited">
                          <RowLayoutColumnInfo OriginX="15" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="15" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ReadOnly" Hidden="True" BaseColumnName="ReadOnly">
                        <Header>
                          <RowLayoutColumnInfo OriginX="16" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="16" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Overrides" Hidden="True" BaseColumnName="Overrides">
                        <Header>
                          <RowLayoutColumnInfo OriginX="17" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="17" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Editable" Hidden="True" BaseColumnName="Editable">
                        <Header>
                          <RowLayoutColumnInfo OriginX="18" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="18" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn AllowUpdate="Yes" HeaderText="HasFallback" Key="hasFallback" Hidden="True" BaseColumnName="hasFallback">
                        <Header Caption="HasFallback">
                          <RowLayoutColumnInfo OriginX="19" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="19" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Globalizable" Hidden="True" BaseColumnName="_Globalizable">
                        <Header>
                          <RowLayoutColumnInfo OriginX="20" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="20" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="M" Key="Mandatory" Width="15px">
												<CellStyle Wrap="True"></CellStyle>
                        <Header Caption="M">
                          <RowLayoutColumnInfo OriginX="21" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="21" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="S" Key="Status" Width="15px" BaseColumnName="Status">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="S">
                          <RowLayoutColumnInfo OriginX="22" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="22" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Container" AllowUpdate="Yes" Key="ContainerName" Width="170px" BaseColumnName="ContainerName"
												CellMultiline="Yes">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="Container">
                          <RowLayoutColumnInfo OriginX="23" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="23" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Value" AllowUpdate="Yes" Key="Value" Width="100%" AllowGroupBy="No" BaseColumnName="Text"
												CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
                        <Header Caption="Value">
                          <RowLayoutColumnInfo OriginX="24" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="24" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="H" Key="InheritanceMethodId" Width="17px" BaseColumnName="InheritanceMethodId" CellMultiline="No">												
											<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="H">
                          <RowLayoutColumnInfo OriginX="25" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="25" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Comment" Key="Comment" Width="150px" AllowGroupBy="No" BaseColumnName="Comment"
												CellMultiline="Yes" ServerOnly="True">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="Comment">
                          <RowLayoutColumnInfo OriginX="26" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="26" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Culture" Key="Country" Width="45px" AllowGroupBy="No">
												<CellStyle Wrap="True" CssClass="ptb1" horizontalAlign="Center"></CellStyle>
												<Header Caption="Culture" Title="Language/Region">
                          <RowLayoutColumnInfo OriginX="27" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="27" />
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
                      <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                      </FilterHighlightRowStyle>
                    </FilterOptions>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
						</div>
					</td>
				</tr>
				<tr valign="bottom">
					<td height="1">
						<div id="divToolbar">
							<table class="hc_toolbartitle" style="height:25px;width:100%" cellspacing="0" cellpadding="0" border="0">
								<tr style="height:25px;vertical-align:middle">
									<td>&nbsp;Total:
										<asp:label id="txtTotal" runat="server">0</asp:label>&nbsp;&nbsp;
										<img height="11" src="/hc_v4/img/SF.gif" width="11"  style="vertical-align:top" alt=""/>&nbsp;Final :
										<asp:label id="txtnbFinal" runat="server" ToolTip="Inherited value count + non inherited value count">0</asp:label>&nbsp;&nbsp;
										<img height="11" src="/hc_v4/img/SD.gif" width="11"  style="vertical-align:top" alt=""/>&nbsp;Draft :
										<asp:label id="txtNbDraft" runat="server" ToolTip="Inherited value count + non inherited value count">0</asp:label>&nbsp;&nbsp;
										<img height="11" src="/hc_v4/img/SM.gif" width="11"  style="vertical-align:top" alt=""/>&nbsp;Missing :
										<asp:label id="txtNbMissing" runat="server">0</asp:label>
										<%--Alternate for CR 5096(Removal of rejection functionality)
										<asp:label id="txtNbRejected" runat="server">0</asp:label>--%></td>
								</tr>
							</table>
							<table class="hc_toolbartitle" style="height:25px;width:100%" cellspacing="0" cellpadding="0" border="0">
								<tr style="height:25px;vertical-align:middle">
									<td>
										<img src="/hc_v4/img/M.gif" style="vertical-align:top" alt=""/>&nbsp;Mandatory:
										<asp:label id="txtTotalMandatory" runat="server">0</asp:label>
								  </td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
			</table>
			<input id="action" type="hidden" name="action"/>
</asp:Content>