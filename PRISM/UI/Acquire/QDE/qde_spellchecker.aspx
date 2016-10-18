<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_SpellChecker" CodeFile="QDE_SpellChecker.aspx.cs"%>
<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Spell checker</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script language="javascript" type="text/jscript">
			var winEditChunk;
			
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
				if (oButton.Key == 'Cancel')
				{
					oEvent.cancelPostBack = true;
					window.close();
				}
			}
			
			var dgGrid;
		  var inputFormName = null;
		  
		  function ed(rowIndex, iId)
		  {
		    dgGrid = igtbl_getGridById(dgClientId);
    	  var url = 'QDE_Chunk.aspx?g='+dgClientId+'&r='+rowIndex+'&i='+iId+'&c='+cultureCode;   
    	  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	  url += '#target';	
				winEditChunk = OpenModalWindow(url, "chunkwindow",  360, 615, 'yes');
		  }
		  
		  function updateParent()
		  {
//				if (opener != null)
//					opener.parent.document.location.reload();
				if (winEditChunk) 
					winEditChunk.close();
		  }
		  
		  function UnloadGrid()
           	 {
                	igtbl_unloadGrid("dg");
           	 }
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript">
		document.body.onunload="updateParent";
  </script>
  <%--Removed the width tag to fix larger button issue by Radha S--%>
			<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbTitle" Runat="server" Width="100%" Font-Size="9pt" Font-Bold="True" CssClass="hc_toolbartitle">ItemName</asp:label></td>
				</tr>
				<tr valign="top" style="height:25px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
							<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
							<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
							<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
							<CLIENTSIDEEVENTS InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></CLIENTSIDEEVENTS>
							<ITEMS>
								<IGTBAR:TBARBUTTON Text="Analyze" Key="Analyze" Image="/hc_v4/img/ed_OK.gif" ToolTip="Analize"></IGTBAR:TBARBUTTON>
								<IGTBAR:TBSEPARATOR Key="AnalyzeSep"></IGTBAR:TBSEPARATOR>
								<IGTBAR:TBARBUTTON Text="Cancel" Key="Cancel" Image="/hc_v4/img/ed_cancel.gif" ToolTip="Close window"></IGTBAR:TBARBUTTON>
							</ITEMS>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbError" runat="server" CssClass="hc_error"></asp:label></td>
				</tr>
				<asp:Panel Runat="server" ID="pnlChildren">
					<tr valign="top" style="height:1px">
						<td>
							<table cellspacing="0" cellpadding="0" width="100%" border="0">
								<tr>
									<td class="editLabelCell" width="380">
										<asp:label id="lbWithChildren" runat="server">Select the check box included content for children of this node</asp:label></td>
									<td class="uga">
										<asp:checkbox id="cbWithChildren" runat="server"></asp:checkbox></td>
								</tr>
							</table>
						</td>
					</tr>
				</asp:Panel>
				<tr valign="top" style="height:1px">
					<td align="center"><asp:label id="lbResult" runat="server" CssClass="hc_success" Visible="False">No result</asp:label></td>
				</tr>
                <%--Removed the css class and added inline property to fix infragestic grid line issue by Radha S--%>
				<tr valign="top">
					<td>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
							<igtbl:ultrawebgrid id="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/">
								<DisplayLayout AutoGenerateColumns="False" Version="4.00" SelectTypeRowDefault="Single" BorderCollapseDefault="Separate"
									EnableInternalRowsManagement="True"  Name="dg" TableLayout="Fixed"
									NoDataMessage="No data to display" RowSelectorsDefault="No">
									<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<RowSelectorStyleDefault Width="30px" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</RowSelectorStyleDefault>
									<FrameStyle Width="100%">
                                        <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                    </FrameStyle>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
									<ActivationObject BorderColor="Orange"></ActivationObject>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand>
										<Columns>
											<igtbl:UltraGridColumn Key="Index" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemName" ServerOnly="True" BaseColumnName="ItemName">
                        <Header>
                          <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemNumber" ServerOnly="True" BaseColumnName="ItemNumber">
                        <Header>
                          <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ContainerId" Hidden="True" BaseColumnName="ContainerId">
                        <Header>
                          <RowLayoutColumnInfo OriginX="3" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="3" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId">
                        <Header>
                          <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsMandatory" Hidden="True" BaseColumnName="IsMandatory">
                        <Header>
                          <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="CultureCode" Hidden="True" BaseColumnName="CultureCode">
                        <Header>
                          <RowLayoutColumnInfo OriginX="6" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="6" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InputFormContainerId" Hidden="True" BaseColumnName="InputFormContainerId">
                        <Header>
                          <RowLayoutColumnInfo OriginX="7" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="7" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Rtl" Hidden="True" BaseColumnName="Rtl">
                        <Header>
                          <RowLayoutColumnInfo OriginX="8" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="8" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ReadOnly" BaseColumnName="ReadOnly" Hidden="True">
                        <Header>
                          <RowLayoutColumnInfo OriginX="9" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="9" />
                        </Footer>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="S" Key="Status" Width="15px" BaseColumnName="ChunkStatus">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="S">
                          <RowLayoutColumnInfo OriginX="10" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="10" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Container" Key="ContainerName" Width="170px" BaseColumnName="ContainerName"
												CellMultiline="Yes">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="Container">
                          <RowLayoutColumnInfo OriginX="11" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="11" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Value" Key="Value" Width="100%" AllowGroupBy="No" BaseColumnName="ChunkValue"
												CellMultiline="Yes">
												<CellStyle Wrap="True" CssClass="ptb3"></CellStyle>
                        <Header Caption="Value">
                          <RowLayoutColumnInfo OriginX="12" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="12" />
                        </Footer>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Error" Key="Error" Width="100px" AllowGroupBy="No" BaseColumnName="Error"
												CellMultiline="Yes">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <Header Caption="Error">
                          <RowLayoutColumnInfo OriginX="13" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="13" />
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
							</igtbl:ultrawebgrid></div>
					</td>
				</tr>
			</table>
</asp:Content>