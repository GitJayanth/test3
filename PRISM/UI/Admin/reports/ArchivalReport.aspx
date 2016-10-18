<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" 
  CodeFile="ArchivalReport.aspx.cs" Inherits="UI_Acquire_ArchivalReport" %>

<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc1" %>
  <%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Archival Report</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
	<script type="text/javascript">
//		function uwToolbar_Click(oToolbar, oButton, oEvent){
//		  if (oButton.Key == 'Generate')
//		  {     		     
//		    DoSearch();	
//        oEvent.cancelPostBack = true;
//        return;
//      }
//		} 		
  </script>

  <table class="main" cellspacing="0" cellpadding="0">
    <tr valign="bottom" height="*">    
      <td align="right">      
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
          CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="" >
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
         <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
              <igtbar:TBCustom ID="TBCustom1" runat="server" Width="">
           <asp:Button ID="btnExport" runat="server" Font-Bold="True" Font-Size="Small" Height="24px"
            Text="Export" Width="77px" OnClick="btnExport_Click" Font-Names="Times New Roman" />
              </igtbar:TBCustom>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBLabel Text="From Date">
              <DefaultStyle Width="80px" Font-Bold="True"></DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom ID="TBCustom2" Width="120px" Key="FromDate" runat="server">
              <igsch:WebDateChooser ID="FrmDate" runat="server" Editable="False">
              </igsch:WebDateChooser>
            </igtbar:TBCustom>
            <igtbar:TBLabel Text="To Date">
              <DefaultStyle Width="50px" Font-Bold="True">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom ID="TBCustom3" Width="120px" Key="ToDate" runat="server">
              <igsch:WebDateChooser ID="ToDate" runat="server" Editable="False">
              </igsch:WebDateChooser>              
            </igtbar:TBCustom>
            <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBCustom ID="TBCustom4" runat="server" Width="">
            <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" Font-Size="Small" Height="24px"
            Text="Apply" Width="77px" OnClick="btnSubmit_Click" Font-Names="Times New Roman" /> 
              </igtbar:TBCustom>
            <igtbar:TBLabel Text=" ">
				<DefaultStyle Width="100%"></DefaultStyle>
			</igtbar:TBLabel>
            </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>      
    </tr>
    <tr valign="top">
      <td class="main">
        <igtbl:UltraWebGrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
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
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="NOTIFICATION DATE" HeaderText="NOTIFICATION DATE" Key="NOTIFICATION DATE" Width="10%">
                  <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>              
                <igtbl:UltraGridColumn BaseColumnName="EVENT" HeaderText="EVENT" Key="EVENT" Width="5%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ITEM NUMBER" HeaderText="ITEM NUMBER" Key="ITEM NUMBER" Width="10%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ITEM NAME" HeaderText="ITEM NAME" Key="ITEM NAME" Width="10%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="COUNTRY CODE" HeaderText="COUNTRY CODE" Key="COUNTRY CODE" Width="10%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ARCHIVED DATE" HeaderText="ARCHIVED DATE" Key="ARCHIVED DATE" Width="10%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="OBSOLETE DATE" HeaderText="OBSOLETE DATE" Key="OBSOLETE DATE" Width="10%">
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

