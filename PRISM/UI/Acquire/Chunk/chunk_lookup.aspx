<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Lookup" validateRequest="false" CodeFile="Chunk_Lookup.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk lookup</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script type="text/javascript" language="javascript">
      top.window.focus();
      var rowIndex = -1;
      var ILBText = "Intentionaly left blank";

		  ////////////////////////////////////////////////////////////
		  function grid_InitializeLayoutHandler(gridName){
		  ////////////////////////////////////////////////////////////
		    if (rowIndex>0){
          igtbl_scrollToView(gridName,igtbl_getElementById(gridName + 'r_' + rowIndex));
        }
		  }
		  function cg_up(){
	     var grid = igtbl_getGridById(dgClientId);
       var activeRow = grid.getActiveRow();
			  __doPostBack('rowup', activeRow.getIndex());
			}
			
			function cg_down(){
	     var grid = igtbl_getGridById(dgClientId);
       var activeRow = grid.getActiveRow();
			  __doPostBack('rowdown', activeRow.getIndex());
			}
			
			function uwToolbar_Click(oToolbar, oButton, oEvent){}
		
        function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <script id="Infragistics" type="text/javascript">
<!--
function dc(){
  if (typeof(top.ChunkIsChanged == 'function')){     
    top.ChunkIsChanged();
  }
}
function dg_AfterCellUpdateHandler(gridName, cellId){
  dc();
}// -->
</script>
<script type="text/javascript" language="javascript">
		document.body.onload = function(){try{document.getElementById('txtValue').focus();}catch(e){}};
</script>
      <hc:chunkbuttonbar id="ChunkButtonBar" runat="server"></hc:chunkbuttonbar>
      <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:1px">
          <td>
            <fieldset>
              <legend>
                <asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" ImageAlign="Middle" AlternateText="status"></asp:image>&nbsp;
              </legend>
              <%--Removed the css class and provided inline property in UltraWebGrid by Radha S--%>
              <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr valign="top">
                  <td>
                  <%-- QC 7028 Fix by Rekha Thomas. Added OnUpdateCell event handler--%>
                    <igtbl:UltraWebGrid id="dg" runat="server" Width="100%" Height="100%" OnUpdateCell="dg_UpdateCell"> 
											<DisplayLayout StationaryMargins="Header" AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00"
												cellpaddingDefault="2" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data">
												<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true"> <%--CssClass="gh">--%>
													<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
												</HeaderStyleDefault>
												<FrameStyle Width="100%" Height="130px">
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                                </FrameStyle>
												<RowAlternateStyleDefault CssClass="uga">
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                                </RowAlternateStyleDefault>
												<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                                </RowStyleDefault>
      								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
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
                      <%--Start QC 7028 Fix by Rekha Thomas. removed Key="Values" and added Datakeyfield--%>
                        <igtbl:UltraGridBand Key="Id" BorderCollapse="Collapse" DataKeyField="Id">
                          <Columns>
                          
                            <igtbl:UltraGridColumn HeaderText="Check" Key="InScope" Width="50px" type="CheckBox" AllowUpdate="Yes">
                             <Header>
                                <RowLayoutColumnInfo OriginX="1" />
                                </Header>
                                <Footer>
                                <RowLayoutColumnInfo OriginX="1" />
                                </Footer>
                            </igtbl:UltraGridColumn>

							<%--<igtbl:UltraGridColumn Key="InScope" Width="25px" Type="CheckBox" AllowUpdate="Yes" AllowGroupBy="Yes" IsBound="true"> --%>
                             <%--HeaderClickAction="Select" --%>
							  <%--  <Header ClickAction="Select">
                                <RowLayoutColumnInfo OriginX="1" />
                                </Header>
                                <Footer>
                                <RowLayoutColumnInfo OriginX="1" />
                                </Footer>
					        </igtbl:UltraGridColumn>--%>

                            <%-- End QC 7028 Fix by Rekha Thomas. removed Key="Values" and added Datakeyfield--%>

                            <igtbl:UltraGridColumn HeaderText="" Key="cChoose" Width="20px">
                              <Header Caption="">
                                <RowLayoutColumnInfo OriginX="2" />
                              </Header>
                              <Footer>
                                <RowLayoutColumnInfo OriginX="2" />
                              </Footer>
                            </igtbl:UltraGridColumn>
														<igtbl:UltraGridColumn HeaderText="Value" Key="Value" IsBound="True" Width="300px" Type="Custom" BaseColumnName="Text"
															AllowUpdate="No" CellMultiline="Yes">
															<CellStyle Wrap="True"></CellStyle>
															<Header Caption="Value">
                                <RowLayoutColumnInfo OriginX="3" />
                              </Header>
                              <Footer>
                                <RowLayoutColumnInfo OriginX="3" />
                              </Footer>
														</igtbl:UltraGridColumn>
														<igtbl:UltraGridColumn HeaderText="Comment" Key="Comment" IsBound="True" Width="100%" BaseColumnName="Comment" CellMultiline="Yes">
															<CellStyle Wrap="True"></CellStyle>
                              <Header Caption="Comment">
                                <RowLayoutColumnInfo OriginX="4" />
                              </Header>
                              <Footer>
                                <RowLayoutColumnInfo OriginX="4" />
                              </Footer>
                            </igtbl:UltraGridColumn>

                            <igtbl:UltraGridColumn HeaderText="Id" Key="Id" IsBound="True" Hidden="True" BaseColumnName="Id">
                              <Header Caption="Id"></Header>
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
                    </igtbl:UltraWebGrid>
                  </td>
                </tr>
                <tr valign="bottom" bgColor="#d6d3ce">
                  <td>
										<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="20px" width="100%" ImageDirectory=" ">
											<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
											<ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
											<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
											<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
											<Items>
											  <igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionally left blank"
													Image="/hc_v4/img/ed_blank.gif">
													<SelectedStyle BorderWidth="1px" BorderStyle="Inset"></SelectedStyle>
												</igtbar:TBarButton>
											</Items>
                    </igtbar:ultrawebtoolbar>
                  </td>
                </tr>
              </table>
              <div id="textcount"></div>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <hc:chunkcomment id="ChunkComment1" runat="server"></hc:chunkcomment>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
            <center>
              <asp:Label id="lbResult" runat="server" Font-Size="7pt">Result message</asp:Label></center>
          </td>
        </tr>
      </table>
     <input type="hidden" name="txtSortColPos" id="txtSortColPos" runat="server" value="5"/>
</asp:Content>