<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" 
  CodeFile="PLCReport.aspx.cs" Inherits="UI_Collaborate_PLCReport" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Missing PLC Report</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
	<script type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
		} 
		
  </script>

  <table class="main" cellspacing="0" cellpadding="0">
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
           
            <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                <%-- Fix for QC# 7386 by Nisha Verma Added borderdetails tag to fix the gridlines missing issue --%>
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
                                <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --Start --%>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
                                <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --End --%>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="PLCode" HeaderText="PL" Key="PL" Width="30px">
                  <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Class" HeaderText="Class" Key="Class" Width="200px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ProductNumber" HeaderText="Product Number" Key="PN" Width="100px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ProductName" HeaderText="Product Name" Key="Name" Width="100%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="true" >
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid><br />
        <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
        <input type="hidden" name="action" id="action">
      </td>
    </tr>
  </table>
</asp:Content>

