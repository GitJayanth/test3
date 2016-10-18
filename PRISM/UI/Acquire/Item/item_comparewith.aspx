<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.ItemManagement.item_CompareWith" CodeFile="item_CompareWith.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Compare with</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
    <script type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'Close') {
      window.close();
      oEvent.cancelPostBack = true;
      }
		} 
		  function DoCompare(i,l,preference,ContainerTypeCode){
		    top.resizeTo(800,500);
		    window.location = "item_Compare.aspx?m=&c=" + l + "&i=" + i + "&preference=" + preference + "&ContainerTypeCode=" + ContainerTypeCode
		  }
		  function txtCompareComponents_TextChanged(oEdit, newText, oEvent){
		    oEdit.setText(CleanSku(newText, listSku));
  		}		
		  function txtCompareComponents_Blur(oEdit, text, oEvent){
		    oEdit.setText(CleanSku(text, listSku));
		  }		  
		  function CleanSku(text, re){
		    return new String(text).toUpperCase().replace(re, '');
  		}
      var strRuleName, strRuleSku;
  		var listSku = /([^0-9A-Z#-,-])/g;
    </script>
</asp:Content>
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
                </Items>
              </igtbar:ultrawebtoolbar>
            </td>
          </tr>
          <asp:Panel ID="panelSku" runat="server">
          <tr>
            <td>
								<table border="0">
									<tr valign="middle">
										<td>Provide the Categories/Skus, separated with commas:</td>
										<td>
											<igtxt:WebTextEdit id="txtCompareComponents" runat="server" Width="200px" HideEnterKey="True" MaxLength="50">
												<ClientSideEvents TextChanged="txtCompareComponents_TextChanged" Blur="txtCompareComponents_Blur"></ClientSideEvents>
											</igtxt:WebTextEdit>
</td>
									</tr>
									<tr valign="middle">
										<td>Container Type</td>
										<td>
									        <asp:dropdownlist id="ddlContainertype" runat="server" Width="211px" Height="71px">
                                            </asp:dropdownlist>											
                                        </td>
									</tr>
								</table>
								or select them in the grid below
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
    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
</FrameStyle>

<RowAlternateStyleDefault CssClass="uga">
    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
</RowAlternateStyleDefault>

<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" Wrap="True" CssClass="ugd">
    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
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
                          <asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server" Enabled ="false"></asp:CheckBox>
                        </HeaderTemplate>
                        <CellTemplate>
                          <asp:CheckBox id="g_sd" onclick="return g_su(this);" runat="server"></asp:CheckBox>
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
        </tbody>
      </table>
</asp:Content>