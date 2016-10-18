<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" CodeFile="Search.aspx.cs" Inherits="HyperCatalog.UI.Search" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Item Search</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
    <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top" style="height:1px">
      <td>
      <%--Removed the width property from ultrawebtoolbar to fix enlarged button by Radha S--%>
        <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
          </Items>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
        </igtbar:ultrawebtoolbar></td>
    </tr>
    <tr valign="top">
      <td>
        
        <table width="100%" border="0">
          <tr>
            <td nowrap><asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Label</asp:label><asp:label id="lbRecordcount" runat="server" Visible="False">NbRecords</asp:label></td>
            <td width="100%"><asp:DropDownList id="DDL_Cultures" runat="server" DataTextField="Name" DataValueField="Code" AutoPostBack="True" onselectedindexchanged="DDL_Cultures_SelectedIndexChanged"></asp:DropDownList></td>
          </tr>
        </table>
        <igtbl:ultrawebgrid id="dg" runat="server" Visible="False" Width="100%" OnInitializeRow="dg_InitializeRow">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
            HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free" EnableInternalRowsManagement="false"
            RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
            <Pager QuickPages="10" PageSize="35" PagerAppearance="Both" StyleMode="PrevNext" AllowPaging="True">
              <Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray"></Style>
            </Pager>
            <%--Removed the css property and add inline properties to fix infragestic grid line issue by Radha S--%>
            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </FrameStyle>
            <ClientSideEvents KeyDownHandler="g_kd"></ClientSideEvents>
            <ActivationObject AllowActivation="False"></ActivationObject>
            <RowAlternateStyleDefault CssClass="uga">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="10px" BaseColumnName="ItemId" ServerOnly="True">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Level" Key="LevelId" Width="50px" BaseColumnName="LevelId">
                  <CellStyle HorizontalAlign="Center" Wrap="True"></CellStyle>
                  <Footer Key="LevelId"></Footer>
                  <Header Key="LevelId" Caption="Level"></Header>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="" Key="IsRoll" Width="15px" BaseColumnName="IsRoll" ServerOnly="true">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="" Key="Status" Width="15px" BaseColumnName="Status" ServerOnly="true">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="Sku" Width="100px" HeaderText="Item" ServerOnly="true" BaseColumnName="Sku"></igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="FullName" Width="80%" HeaderText="Item" BaseColumnName="ItemName" CellMultiline="Yes"></igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Company" Key="COMPANY" Width="20%" BaseColumnName="Company">
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:ultrawebgrid></td>
    </tr>
  </table>
</asp:Content>
