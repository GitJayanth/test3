<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="chunk_inheritance" CodeFile="chunk_inheritance.aspx.cs" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk Inheritance</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
        function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
</script>  
<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top">
      <td>
        <igtbl:ultrawebgrid id="dg" runat="server" Width="100%" EnableViewState="False" Height="100%" OnInitializeRow="dg_InitializeRow">
        <%--modified the code to fix infragestic issue by Radha S--%>
          <DisplayLayout StationaryMargins="Header" AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00"
            cellpaddingDefault="2" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data">
            <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign ="Center"> <%--CssClass="gh">--%>
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" Height="100%">
                <BorderDetails StyleBottom="Solid" WidthBottom ="1pt" StyleRight="Solid" WidthRight="1pt" />
            </FrameStyle>
            <SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
            <RowAlternateStyleDefault CssClass="uga">
                <BorderDetails StyleBottom="Solid" WidthBottom ="1pt" StyleRight="Solid" WidthRight="1pt" />
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                <BorderDetails StyleBottom="Solid" WidthBottom ="1pt" StyleRight="Solid" WidthRight="1pt" />
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
					  <igtbl:UltraGridBand Key="ProductBand">
              <Columns>
                <igtbl:UltraGridColumn HeaderText="Level" Key="Level" Width="40px" BaseColumnName="LevelId">
                  <CellStyle HorizontalAlign="Center" Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Product" Key="Product" Width="300px" BaseColumnName="ItemName">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Value" Key="Value" Width="100%" BaseColumnName="ChunkValue" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Culture" Key="Culture" Width="45px">
									<CellStyle horizontalAlign="Center"></CellStyle>
								</igtbl:UltraGridColumn>
							  <igtbl:UltraGridColumn Key="CountryCode" BaseColumnName="CountryCode" ServerOnly="True"></igtbl:UltraGridColumn>
							  <igtbl:UltraGridColumn Key="CultureCode" BaseColumnName="CultureCode" ServerOnly="True"></igtbl:UltraGridColumn>
							  <igtbl:UltraGridColumn Key="InScope" BaseColumnName="InScope" ServerOnly="True"></igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:ultrawebgrid>
      </td>
    </tr>
    <tr valign="bottom" height="1">
      <td align="right">
      </td>
    </tr>
  </table>
</asp:Content>