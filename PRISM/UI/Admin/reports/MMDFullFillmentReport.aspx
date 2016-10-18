<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" 
  CodeFile="MMDFullFillmentReport.aspx.cs" Inherits="UI_Admin_MMDFullFillmentReport" %>
  <%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  MMDFullFillment Report</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
	<script type="text/javascript">
//		function uwToolbar_Click(oToolbar, oButton, oEvent){
//		  if (oButton.Key == 'Generate')
//		  {     		     
//		        DoSearch();	
//        oEvent.cancelPostBack = true;
//        return;
//      }
//		} 		
  </script>

  <table class="main" cellspacing="0" cellpadding="0">
    <tr valign="bottom">    
      <td align="right">      
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
          CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwToolbar_ButtonClicked" >
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
         <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBLabel Text="PLCode">
              <DefaultStyle Width="100px" Font-Bold="True"></DefaultStyle>
            </igtbar:TBLabel>
            <asp:DropDownList ID="ddlPlcode" runat="server" Width="129px" EnableViewState = "true" AutoPostBack="false" Height="64px">                         
            </asp:DropDownList>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Generate" Text="Generate" Image="/hc_v4/img/ed_OK.gif"></igtbar:TBarButton>
            </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>      
    </tr>
    <tr valign="top">
      <td class="main">
       <igtbl:UltraWebGrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
          <Bands>
           <igtbl:UltraGridBand DataKeyField="ProductLineNo">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ProductLineNo" HeaderText="ProductLineNo" Key="ProductLineNo" Width="10%">
                  <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Header Caption="ProductLineNo">
                    </Header>
                </igtbl:UltraGridColumn>              
                <igtbl:UltraGridColumn BaseColumnName="Product Number" HeaderText="Product Number" Key="Product Number" Width="10%">
                    <Header Caption="Product Number">
                        <RowLayoutColumnInfo OriginX="1" />
                    </Header>
                    <Footer>
                        <RowLayoutColumnInfo OriginX="1" />
                    </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Product Name" HeaderText="Product Name" Key="Product Name" Width="30%">
                    <Header Caption="Product Name">
                        <RowLayoutColumnInfo OriginX="2" />
                    </Header>
                    <Footer>
                        <RowLayoutColumnInfo OriginX="2" />
                    </Footer>
                </igtbl:UltraGridColumn>
              </Columns>
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
                <FilterOptions AllString="" EmptyString="" NonEmptyString="">
                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                        CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                        Font-Size="11px" Width="200px">
                        <Padding Left="2px" />
                    </FilterDropDownStyle>
                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                    </FilterHighlightRowStyle>
                </FilterOptions>
            </igtbl:UltraGridBand>
          </Bands>
	   <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True">
            </Pager>
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
            </ClientSideEvents>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
              
	   </DisplayLayout>
        </igtbl:UltraWebGrid>
        <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
        <input type="hidden" name="action" id="action">
      </td>
    </tr>
  </table>
</asp:Content>

