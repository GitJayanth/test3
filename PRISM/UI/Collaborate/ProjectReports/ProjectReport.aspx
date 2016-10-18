<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true"
  CodeFile="ProjectReport.aspx.cs" Inherits="UI_Globalize_ProjectReports_ProjectReport" %>

<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Project Reports</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
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
<%--Removed width property from UltraWebToolbar to fix moving icon issue by Radha S--%>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <tr valign="top">
			<td>
				<asp:label id="lbTitle" runat="server"></asp:label></td>
		</tr>
  <tr valign="bottom" height="*">
    <td align="right">
      <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
        CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwToolbar_ButtonClicked">
        <HoverStyle CssClass="hc_toolbarhover">
        </HoverStyle>
         <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
        <SelectedStyle CssClass="hc_toolbarselected">
        </SelectedStyle>
        <Items>
          <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator></igtbar:TBSeparator>
          <igtbar:TBLabel Text="Search">
            <DefaultStyle Width="50px" Font-Bold="True">
            </DefaultStyle>
          </igtbar:TBLabel>
          <igtbar:TBCustom ID="TBCustom1" Width="250px" Key="SearchField" runat="server">
            <asp:TextBox Width="250px" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
          </igtbar:TBCustom>
          <igtbar:TBarButton key="filter" Image="/hc_v4/img/ed_search.gif">
            <DefaultStyle Width="25px">
            </DefaultStyle>
          </igtbar:TBarButton>
        </Items>
        <DefaultStyle CssClass="hc_toolbardefault">
        </DefaultStyle>
      </igtbar:UltraWebToolbar>
    </td>
  </tr>

  <tr valign="top">
    <td class="main">
      <igtbl:UltraWebGrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%"
        OnInitializeRow="dg_InitializeRow">
        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
          Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
          EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
          CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
          <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True">
          </Pager>
          <%--Removed css style and added inline property for grid by Radha S--%>
          <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
            </BorderDetails>
          </HeaderStyleDefault>
          <FrameStyle Width="100%" CssClass="dataTable">
          </FrameStyle>
          <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
          </ClientSideEvents>
          <RowAlternateStyleDefault CssClass="uga">
            <BorderDetails StyleBottom="Solid" WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" />
          </RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            <BorderDetails StyleBottom="Solid" WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" />
          </RowStyleDefault>
        </DisplayLayout>
        <Bands>
          <igtbl:UltraGridBand>
            <Columns>
              <igtbl:UltraGridColumn BaseColumnName="LevelId" Hidden="true" Key="LevelId">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="LevelName" HeaderText="Level" Key="Level" Width="150px">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="PLCode" HeaderText="PL" Key="PL" Width="30px">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="Class" HeaderText="Class" Key="Class" Width="150px">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="Name" HeaderText="Product Name" Key="Name" Width="100%">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="Regions" HeaderText="Region Name" Key="Regions" Width="200px">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="ProjectDate" HeaderText="ProjectDate" Key="ProjectDate">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="true" Width="100px">
              </igtbl:UltraGridColumn>
               <%-- Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 21/May/09 --%>
              <igtbl:UltraGridColumn BaseColumnName="CultureCode" Key="CultureCode" Hidden="true" Width="100px">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn BaseColumnName="IsInitialized" Key="IsInitialized" HeaderText="Visible in region" Type="CheckBox">
               <CellStyle HorizontalAlign="Center"></CellStyle>
              </igtbl:UltraGridColumn>
            </Columns>
          </igtbl:UltraGridBand>
        </Bands>
      </igtbl:UltraWebGrid><br />
    </td>
  </tr>
  <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
  <input type="hidden" name="action" id="action">
</asp:Content>
