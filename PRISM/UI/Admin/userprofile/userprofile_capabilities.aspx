<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="userprofile_capabilities" CodeFile="userprofile_capabilities.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" language="javascript">
    document.body.oncontextmenu=function(){return false;};
  </script>
  <%--To fix QC 7691 - Removed width property form ultrawebtoolbar by Radha S --%>
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <TBODY>
          <tr valign="top" style="height:1px">
            <td>
               <igtbar:ultrawebtoolbar id=uwToolbar runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
                 <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
                <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
                <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
                <Items>
                  <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
                </Items>
              </igtbar:ultrawebtoolbar>
            </td>
          </tr>
          <TR>
            <td>
              <igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
                <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
                  SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free"
                  RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No data">
                  <%--Removed css class and added inline property by Radha S--%>
                  <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand"  BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                  </HeaderStyleDefault>
                  <FrameStyle Width="100%" CssClass="dataTable">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                  </FrameStyle>
                  <RowAlternateStyleDefault CssClass="uga">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                  </RowAlternateStyleDefault>
                  <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" Wrap="True" CssClass="ugd">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                  </RowStyleDefault>
                </DisplayLayout>
                <Bands>
                  <igtbl:UltraGridBand BorderCollapse="Collapse" DataKeyField="Id">
                    <Columns>
                      <igtbl:UltraGridColumn HeaderText="Id" Key="Id" ServerOnly="true" IsBound="True" BaseColumnName="Id"></igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Name" Key="Name" IsBound="True" Width="150px" BaseColumnName="Name"></igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Description" Key="Description" IsBound="True" Width="100%" BaseColumnName="Description"></igtbl:UltraGridColumn>
                    </Columns>
                  </igtbl:UltraGridBand>
                </Bands>
              </igtbl:UltraWebGrid>
            </td>
          </tr>
      </table>
</asp:Content>