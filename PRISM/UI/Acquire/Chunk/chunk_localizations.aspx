<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Chunk_Localizations" CodeFile="Chunk_Localizations.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk localizations</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		function UnloadGrid()
            	{
                	igtbl_unloadGrid("dg");
            	}

		//document.body.oncontextmenu = function(){return false;};
</script>
      <table style="WIdTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top">
          <td>
            <igtbl:ultrawebgrid id="dg" runat="server" Width="100%"  ImageDirectory="/ig_common/Images/">
            <%--modified the code to fix infragestic issue by Radha S--%>
              <DisplayLayout StationaryMargins="Header" AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00"
                SelectTypeCellDefault="Single" cellpaddingDefault="2" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                NoDataMessage="No Possible values">
                <Pager QuickPages="10" PageSize="50" AllowPaging="True"></Pager>
                <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true"> <%--CssClass="gh">--%>
                  <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </HeaderStyleDefault>
                <FrameStyle Width="100%" CssClass="dataTable">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </FrameStyle>
                <SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
                <RowAlternateStyleDefault CssClass="uga">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </RowAlternateStyleDefault>
                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </RowStyleDefault>
              </DisplayLayout>
        <Bands>
          <igtbl:UltraGridBand Key="CultureCode" BorderCollapse="Collapse" DataKeyField="CultureCode">
            <Columns>
              <igtbl:UltraGridColumn HeaderText="" Key="Status" IsBound="True" Width="20px" BaseColumnName="Status"
                AllowUpdate="No"></igtbl:UltraGridColumn>
	      <igtbl:UltraGridColumn HeaderText="TRId" Key="TRId" IsBound="True" Width="50px" BaseColumnName="TRId"
                AllowUpdate="No"></igtbl:UltraGridColumn>	
              <igtbl:UltraGridColumn HeaderText="Culture" Key="CultureCode" IsBound="True" Width="50px" BaseColumnName="CultureCode"
                AllowUpdate="No"></igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="rtl" Key="rtl" IsBound="True" Width="100px" ServerOnly="True" BaseColumnName="Rtl" AllowUpdate="No"></igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn HeaderText="Value" Key="Value" IsBound="True" Width="100%" Type="Custom" BaseColumnName="Text"
                      AllowUpdate="No" CellMultiline="Yes">
                      <SelectedCellStyle CssClass="ugs"></SelectedCellStyle>
                      <CellStyle Wrap="True"></CellStyle>
                      <Footer Key="Value">
                        <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                      </Footer>
                      <Header Key="Value" Caption="Value">
                        <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                      </Header>
                    </igtbl:UltraGridColumn>
            </Columns>
          </igtbl:UltraGridBand>
        </Bands>
      </igtbl:ultrawebgrid>
      </td>
      </tr>
      </table>
</asp:Content>