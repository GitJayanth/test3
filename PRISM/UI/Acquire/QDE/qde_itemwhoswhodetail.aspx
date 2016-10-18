<%@ Reference Page="~/ui/acquire/qde.aspx" %>

<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" CodeFile="qde_itemwhoswhodetail.aspx.cs"
  Inherits="HyperCatalog.UI.Acquire.QDE.qde_itemwhoswhodetail" %>

<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Item Who's who</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Close') 
		    {
					oEvent.cancelPostBack = true;
          window.close();
        }
		  } 
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table cellspacing="0" cellpadding="0" border="0" width="100%" class="main">
    <tr class="selectlanguage" style="height: 1px">
      <td>
        &nbsp;<asp:Label ID="lItemName" runat="server">CurrentItemName</asp:Label>
      </td>
      <td align="right">
        <asp:Label ID="lItemLevel" runat="server">CurrentItemLevel</asp:Label>&nbsp;</td>
    </tr>
    <tr valign="top" style="height: 1px">
      <td colspan="2">
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
          CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar">
          </ClientSideEvents>
          <Items>
            <igtbar:TBarButton Image="/hc_v4/img/ed_cancel.gif" Text="Close" ToolTip="Close window"
              Key="Close">
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
          </Items>
        </igtbar:UltraWebToolbar>
    </tr>
    <tr style="height: 1px">
      <td>
        &nbsp;<asp:Label ID="lUserName" runat="server">UserName</asp:Label>
      </td>
    </tr>
    <tr valign="top">
      <td colspan="2">
      <div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
        <igtbl:UltraWebGrid ID="dg" width="100%" runat="server" ImageDirectory="/ig_common/Images/" OnInitializeRow="dg_InitializeRow">
          <DisplayLayout NoDataMessage="No data to display" TableLayout="Fixed" Name="dg" RowSelectorsDefault="No"
            EnableInternalRowsManagement="True" BorderCollapseDefault="Separate" Version="4.00"
            AutoGenerateColumns="False">
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%"></FrameStyle>
                <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand Key="Chunks" BorderCollapse="Collapse" BaseTableName="Chunks">
              <Columns>
                <igtbl:UltraGridColumn Key="Status" Width="25" BaseColumnName="ChunkStatus">
                <Header Caption="S"></Header>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="ContainerName" width="150" BaseColumnName="Container" CellMultiline="Yes">
<CellStyle Wrap="True"></CellStyle>
                <Header Caption="Name"></Header>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="ChunkValue" width="100%" BaseColumnName="ChunkValue" CellMultiline="Yes">
                <CellStyle Wrap="True"></CellStyle>
                <Header Caption="Value"></Header>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="ModifyDate" BaseColumnName="ModifyDate" CellMultiline="Yes">
                <CellStyle Wrap="True"></CellStyle>
                <Header Caption="Modify date"></Header>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        </div>
      </td>
    </tr>
  </table>
  <asp:Label ID="lbError" runat="server" Visible="false"></asp:Label>
</asp:Content>
