 <%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="UI_Globalize_TRReports_Report" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">TR reports</asp:Content>
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
		
		  
	  function EditTR(i)
	  {
 	    var url = "../../../redirect.aspx?p=UI/Globalize/TR/TR_Properties.aspx&tr=" + i;
 	    url += '#target';
 	    winChunkEdit = OpenModalWindow(url,'qde', 600, 600, 'yes', 'yes', 'no', 'no', 'yes')
 	   }
  </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
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
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
            </ClientSideEvents>
             <RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
           	<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="TRId" Key="TRId" HeaderText="Id" Width="50px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Type" Key="Type" HeaderText="Type" Width="100px">
                </igtbl:UltraGridColumn>
                                <igtbl:UltraGridColumn BaseColumnName="InstantTR" Type="CheckBox" Key="InstantTR" HeaderText="Instant" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>

                <igtbl:UltraGridColumn BaseColumnName="LevelId" Hidden="true" Key="LevelId">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="LevelName" HeaderText="Level" Key="Level">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PLCode" HeaderText="PL" Key="PL" Width="30px">
                  <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>
                 <igtbl:UltraGridColumn BaseColumnName="Name" HeaderText="Name" Key="Name" Width="100%">
                </igtbl:UltraGridColumn>
                <%--Deepak - Regional Project Management--%>
                <igtbl:UltraGridColumn BaseColumnName="RegionCode" HeaderText="Region" Key="RegionCode" Width="100px">
                  <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="BOT" HeaderText="BOT" Key="BOT">
                  <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="true">
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