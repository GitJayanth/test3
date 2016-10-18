<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.ItemManagement.item_allChunks" CodeFile="item_allChunks.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">All Chunks</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script>
    function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
		    if (oButton.Key == 'Close') {
          window.close();
          oEvent.cancelPostBack = true;
        }        
		    if (oButton.Key == 'Print') {
          window.print();
          oEvent.cancelPostBack = true;
        }        
		  } 
    var l;
    
		function dg_DblClickHandler(gridName, cellId){
		  var cell = igtbl_getCellById(cellId);
		  var c = cell.Row.getCellFromKey('ContainerId').getValue();	  
		  var i = cell.Row.getCellFromKey('ItemId').getValue();	  
		  var args = EditChunk(c, i, l);
  		if (args != null)
	    {
        if (parent){
          var obj = parent.document.getElementById("allChunks");
          var s = obj.document.allChunks.document.location;
          
          // Force reload (becoz of modal window behaviour!)
          obj.document.allChunks.document.location = '/hc_v4/dummy.htm';
          obj.document.allChunks.document.location = s;
        }
        else{
          location.reload();
        }
      }      
		}
  </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr valign="top" style="height: 1px">
        <td colspan="4">
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" RenderAnchors="True" CssClass="hc_toolbar"
            ItemWidthDefault="80px">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <Items>
              <igtbar:TBarButton Key="Close" ToolTip="Close window" Text="Close" Image="/hc_v4/img/ed_cancel.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Print" ToolTip="Print" Text="Print" Image="/hc_v4/img/ed_print.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBLabel Text="Filter">
                <DefaultStyle Width="40px" Font-Bold="True">
                </DefaultStyle>
              </igtbar:TBLabel>
              <igtbar:TBCustom Width="150px" Key="filterField">
                <asp:TextBox Width="150px" ID="txtFilter" MaxLength="50"></asp:TextBox>
              </igtbar:TBCustom>
              <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                <DefaultStyle Width="25px">
                </DefaultStyle>
              </igtbar:TBarButton>
            </Items>
            <DefaultStyle BorderWidth="1px" BorderStyle="None" BackgroundImage="/ig_common/webtoolbar2/ig_tb_back03.gif"
              Height="22px" CssClass="hc_toolbardefault">
            </DefaultStyle>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr class="sectionTitle" valign="middle">
        <td align="left" style="width: 1px">
          &nbsp;
        </td>
        <td align="center" width="100%">
          All chunks for
          <asp:Label ID="lbItemName" runat="server">item name</asp:Label></td>
        <td nowrap align="right">
          <asp:Label ID="lbcultureName" runat="server" ForeColor="Black">CultureName</asp:Label></td>
      </tr>
    </table>
    <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%">
      <DisplayLayout AutoGenerateColumns="False" ViewType="OutlookGroupBy" AllowSortingDefault="OnClient"
        RowHeightDefault="20px" Version="4.00" HeaderClickActionDefault="SortSingle" BorderCollapseDefault="Separate"
        EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
        CellClickActionDefault="RowSelect" NoDataMessage="No chunks to proofread">
        
          <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
    <FrameStyle Width="100%" CssClass="dataTable">
        </FrameStyle>
           <RowAlternateStyleDefault CssClass="uga">
   
                              </RowAlternateStyleDefault>
           	<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
      </DisplayLayout>
      <Bands>
        <igtbl:UltraGridBand BaseTableName="chunks" Key="chunks" DataKeyField="ContainerId">
          <Columns>
            <igtbl:UltraGridColumn Key="ContainerId" Hidden="True" BaseColumnName="ContainerId">
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId">
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn Key="L" ServerOnly="true" BaseColumnName="LevelId">
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn Key="rtl" ServerOnly="True" BaseColumnName="Rtl">
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn HeaderText="ChunkStatus" Key="S" IsBound="True" Width="100px" BaseColumnName="ChunkStatus">
            </igtbl:UltraGridColumn>
             <igtbl:UltraGridColumn HeaderText="ChunkStatus" Key="ST" Hidden="true"  Width="16px" BaseColumnName="ChunkStatus">
            </igtbl:UltraGridColumn>

            <igtbl:UltraGridColumn HeaderText="Name" Key="IN" IsBound="True" Width="210px"
              BaseColumnName="ItemName" CellMultiline="Yes">
              <CellStyle Wrap="True">
              </CellStyle>
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn HeaderText="Container Name" Key="CN" IsBound="True"
              Width="120px" BaseColumnName="ContainerName" CellMultiline="Yes">
              <CellStyle Wrap="True">
              </CellStyle>
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn HeaderText="Container Group" Key="CG" Width="180px" IsBound="True" BaseColumnName="Path" CellMultiline="Yes">
              <CellStyle Wrap="True">
              </CellStyle>
            </igtbl:UltraGridColumn>
            <igtbl:UltraGridColumn HeaderText="Chunk Value" Key="V" IsBound="True" Width="100%"
              BaseColumnName="ChunkValue" CellMultiline="Yes">
              <CellStyle Wrap="True">
              </CellStyle>
            </igtbl:UltraGridColumn>
            
          </Columns>
        </igtbl:UltraGridBand>
      </Bands>
    </igtbl:UltraWebGrid>
    <center>
      <asp:Label ID="lbResult" CssClass="hc_success" runat="server">No result</asp:Label>
    </center>
</asp:Content>