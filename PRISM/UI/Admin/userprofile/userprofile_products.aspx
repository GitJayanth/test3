<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="userprofile_products" CodeFile="userprofile_products.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Products</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script>
		function dg_DblClickHandler(gridName, id, ev){
			var cell = igtbl_getCellById(id);
      var grid = igtbl_getGridById(gridName);
		}

		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
		} 
  </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" language="javascript">
    document.body.oncontextmenu=function(){return false;};
  </script>
  <%--Removed width tag from ultrawebtoolbar by Radha S--%>
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
                  <igtbar:TBSeparator></igtbar:TBSeparator>
                  <igtbar:TBLabel Text="Filter">
                    <DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
                  </igtbar:TBLabel>
                  <igtbar:TBCustom Width="150px" Key="filterField">
                    <asp:TextBox cssClass=Search Width="150px" Id="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
                  </igtbar:TBCustom>
                  <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                    <DefaultStyle Width="25px"></DefaultStyle>
                  </igtbar:TBarButton>
                </Items>
              </igtbar:ultrawebtoolbar>
            </td>
          </tr>
          <tr valign="top">
            <td>
                <asp:Label id="lbNoresults" runat="server" Visible="false"></asp:Label>
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
                  <igtbl:UltraGridBand BaseTableName="UserItems" Key="UserItems" BorderCollapse="Collapse" DataKeyField="ItemId">
                    <Columns>
                      <igtbl:UltraGridColumn HeaderText="Id" Key="ItemId" IsBound="True" Width="50px" BaseColumnName="Id"></igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" IsBound="True" Width="100%" BaseColumnName="Name"></igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Level" Key="LevelName" IsBound="True" Width="100px" BaseColumnName="LevelName"></igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn Key="LevelId" BaseColumnName="LevelId" Hidden="true"></igtbl:UltraGridColumn>
                    </Columns>
                  </igtbl:UltraGridBand>
                </Bands>
              </igtbl:UltraWebGrid>
            </td>
          </tr>
      </table>
</asp:Content>