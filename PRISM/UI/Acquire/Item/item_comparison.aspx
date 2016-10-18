<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.ItemManagement.item_Comparison" CodeFile="item_Comparison.aspx.cs" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Item Comparison</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script type="text/javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Close') 
		    {
					oEvent.cancelPostBack = true;
          window.close();
        }
		  } 		  
//		   function CompareWrtContainerType(i,l){
//		    top.resizeTo(800,500);
//		    window.location = "item_compare.aspx?m=&c=" + l + "&i=" + i
//		    }
      </script>
</asp:Content>
<%--<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:1px">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <CLIENTSIdEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIdEEVENTS>
              <ITEMS>
                  <igtbar:TBarButton Key="Close" ToolTip="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
                  <igtbar:TBSeparator></igtbar:TBSeparator>
                  <igtbar:TBarButton Text="Compare" Key="Compare" ToolTip="Compare" Image="/hc_v4/img/ed_dynamiccomparison.gif"></igtbar:TBarButton>
                  <igtbar:TBSeparator></igtbar:TBSeparator>
                  <igtbar:TBarButton Image="/hc_v4/img/ed_download.gif" Text="Export" Key="Export"></igtbar:TBarButton>
               <%-- <igtbar:TBarButton Image="/hc_v4/img/ed_cancel.gif" Text="Close" ToolTip="Close window" Key="Close"></igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Image="/hc_v4/img/ed_download.gif" Text="Export" Key="Export"></igtbar:TBarButton>
              </ITEMS>
            </igtbar:ultrawebtoolbar></td>
        </tr>--%>
        <asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tbody>
          <tr valign="top" style="height:1px">
            <td>
    <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
      <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
      <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
      <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
                <Items>
                  <igtbar:TBarButton Key="Close" ToolTip="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
                  <igtbar:TBSeparator></igtbar:TBSeparator>
                  <igtbar:TBarButton Text="Compare" Key="Compare" ToolTip="Compare" Image="/hc_v4/img/ed_dynamiccomparison.gif"></igtbar:TBarButton>
                  <igtbar:TBSeparator></igtbar:TBSeparator>
                  <igtbar:TBarButton Image="/hc_v4/img/ed_download.gif" Text="Export" Key="Export"></igtbar:TBarButton>
                </Items>
              </igtbar:ultrawebtoolbar>
            </td>
          </tr>
         <asp:Panel ID="panelSku" runat="server">
          <tr>
            <td>
								<table border="0">
									<tr valign="middle">
										<td>Container Type</td>
										<td>
									        <asp:dropdownlist id="ddlContainertype" runat="server" Width="211px" Height="71px">
                                            </asp:dropdownlist>											
                                        </td>
									</tr>
								</table>
								
            </td>
          </tr>
          </asp:Panel>
           <tr>
            <td>
              <igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
                <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free" EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No data">

       <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>

<FrameStyle Width="100%" CssClass="DataTable">
</FrameStyle>

<RowAlternateStyleDefault CssClass="uga">
</RowAlternateStyleDefault>

<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" Wrap="True" CssClass="ugd">
</RowStyleDefault>
  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>


</DisplayLayout>

<Bands>
<igtbl:UltraGridBand BaseTableName="Items" Key="Items" BorderCollapse="Collapse" DataKeyField="ItemId">
<Columns>
<igtbl:UltraGridColumn HeaderText="Id" Key="ItemId" IsBound="True" Width="30px" Hidden="True" BaseColumnName="ItemId">
<Header Caption="Id">
</Header>
</igtbl:UltraGridColumn>
                      <igtbl:TemplatedColumn Key="Select" Width="20px" FooterText="">
                        <CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
                        <HeaderTemplate>
                        <!--  <asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox> -->
                        </HeaderTemplate>
                        <CellTemplate>
                          <!-- <asp:CheckBox id="g_sd" onclick="return g_su(this);" runat="server"></asp:CheckBox> -->
                        </CellTemplate>
                        <Footer Key="Select" Caption="">
                          <RowLayoutColumnInfo OriginX="13"></RowLayoutColumnInfo> 
                        </Footer>
                        <Header Key="Select">
                          <RowLayoutColumnInfo OriginX="13"></RowLayoutColumnInfo>
                        </Header>
                      </igtbl:TemplatedColumn>
<igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" IsBound="True" Width="100%" BaseColumnName="ItemName">
<Header Caption="Name">
</Header>
</igtbl:UltraGridColumn>
</Columns>
</igtbl:UltraGridBand>
</Bands>
              </igtbl:UltraWebGrid>
            </td>
          </tr>
       <%-- <tr valign="top">
          <td>
            <igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
              <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" SelectTypeRowDefault="Single"
                ScrollBar="Never" IndentationDefault="20" RowSelectorsDefault="No"
                Name="dg" TableLayout="Fixed" NoDataMessage="No containers found">
                <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh2">
                  <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </HeaderStyleDefault>
                <FrameStyle Width="100%" CustomRules="table-layout:auto"></FrameStyle>
                <SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
                <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
              </DisplayLayout>
              <Bands>
                <igtbl:UltraGridBand></igtbl:UltraGridBand>
              </Bands>
            </igtbl:ultrawebgrid>
          </td>
        </tr>--%>
          </tbody>
      </table>
</asp:Content>