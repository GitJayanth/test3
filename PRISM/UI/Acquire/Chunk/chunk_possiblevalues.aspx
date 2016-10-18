<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Chunk_PossibleValues" CodeFile="Chunk_PossibleValues.aspx.cs" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk possible values</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		document.body.scroll = "no";
</script>
      <table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:1px">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <Items>
                <igtbar:TBLabel Text="Filter">
                  <DefaultStyle Width="45px" Font-Bold="True"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBCustom Width="550px" Key="FilterList" runat="server">
                  <igcmbo:WebCombo id="uwDDL_Filters" runat="server" Height="18px" Width="520px"
                    DataTextField="Name" Editable="True" BackColor="White" BorderColor="LightGray" SelBackColor="49, 106, 197" DropImage1="/ig_common/WebGrid3/ig_cmboDownXP1.bmp"
                    DropImage2="/ig_common/WebGrid3/ig_cmboDownXP2.bmp" OnInitializeRow="uwDDL_Filters_InitializeRow" OnSelectedRowChanged="uwDDL_Filters_SelectedRowChanged" Version="4.00">
                    <Columns>
                      <igcmbo:UltraGridColumn Key="Id" Hidden="True" BaseColumnName="Id"></igcmbo:UltraGridColumn>
                      <igcmbo:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="Name"></igcmbo:UltraGridColumn>
                      <igcmbo:UltraGridColumn Key="ItemName" Hidden="True" BaseColumnName="ItemName"></igcmbo:UltraGridColumn>
                      <igcmbo:UltraGridColumn Key="LevelName" Hidden="True" BaseColumnName="LevelName"></igcmbo:UltraGridColumn>
                    </Columns>
                    <DropDownLayout DropdownWidth="540px" DropdownHeight="80px" ColHeadersVisible="No" AutoGenerateColumns="False"
                      BorderCollapse="Separate" RowHeightDefault="17px" RowSelectors="No">
                      <RowStyle BorderWidth="0px" BorderColor="Gray" BorderStyle="None" BackColor="White">
                        <BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
                      </RowStyle>
                      <SelectedRowStyle ForeColor="White" BackColor="DarkBlue"></SelectedRowStyle>
                      <FrameStyle Width="540px" Cursor="Default" BorderWidth="2px" Font-Size="XX-Small" Font-Names="Verdana"
                        BorderStyle="Ridge" BackColor="White" Height="80px"></FrameStyle>
                      <HeaderStyle BackColor="LightGray" BorderStyle="Solid">
                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                      </HeaderStyle>
                    </DropDownLayout>
                    <ExpandEffects ShadowColor="LightGray"></ExpandEffects>
                  </igcmbo:WebCombo>
                </igtbar:TBCustom>
              </Items>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
            </igtbar:ultrawebtoolbar>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <asp:Label id="LbSubSet" Height="18px" runat="server" Visible="False">Only a subset of most recent possible values was returned</asp:Label>
          </td>
        </tr>
        <asp:Panel id="panelGrid" runat=server>
        <tr valign="top" style="height:auto">
          <td>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
              <igtbl:ultrawebgrid id="dgPossibleValues" runat="server" Width="100%" Height="100%" OnInitializeRow="dgPossibleValues_InitializeRow">
              <%--modified the code to fix infragestic issue by Radha S--%>
                <DisplayLayout StationaryMargins="Header" AutoGenerateColumns="False" RowHeightDefault="100%"
                  SelectTypeCellDefault="Single" cellpaddingDefault="2" RowSelectorsDefault="No" Name="dgPossibleValues" TableLayout="Fixed"
                  NoDataMessage="No Possible values">
                  <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" BackColor="LightGray" Cursor="Hand" HorizontalAlign="Center"> <%--CssClass="gh">--%>
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
                  <igtbl:UltraGridBand Key="ProductBand" BorderCollapse="Collapse" DataKeyField="ProductBand">
                    <Columns>
                      <igtbl:UltraGridColumn Key="Status" IsBound="True" Width="20px" BaseColumnName="Status" AllowUpdate="No"></igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Product" Key="Product" IsBound="True" Width="200px" BaseColumnName="ItemName" AllowUpdate="No">
                        <CellStyle Wrap="True"></CellStyle>
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Value" Key="Value" IsBound="True" Width="100%" Type="Custom" BaseColumnName="Text" AllowUpdate="No" CellMultiline="Yes">
                        <SelectedCellStyle CssClass="ugs"></SelectedCellStyle>
                        <CellStyle Wrap="True"></CellStyle>
                      </igtbl:UltraGridColumn>
                    </Columns>
                  </igtbl:UltraGridBand>
                </Bands>
              </igtbl:ultrawebgrid>
            </div>
          </td>
        </tr>
        </asp:Panel>
      </table>
</asp:Content>