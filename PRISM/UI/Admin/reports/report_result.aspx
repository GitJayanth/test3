<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Report_Result" CodeFile="Report_Result.aspx.cs" %>
<%@ PreviousPageType VirtualPath="report_properties.aspx" %> 

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Report results</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

	<script>
    window.focus();
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
      if (oButton.Key=="Run") 
      {
        if (selectedQuery < 0){
          alert("Please, select a query first");
        }
        else{
          var url='reports/Report_Result.aspx?r=' + selectedQuery;
          open(url,'queryprocess','toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,width=600,height=600');
        }        
        oEvent.cancelPostBack = true;
      }
		}


	</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script>
    document.body.onload=function(){ResizeGrid('dg')};
  </script>
  <%--Removed the width propery form ultrawebtoolbar to fix moving button issue by Radha S--%>
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox id="txtFilter" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
									<DefaultStyle Width="25px"></DefaultStyle>
								</igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height: 1px">
				  <td>
				    <asp:Label ID="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
				  </td>
				</tr>
				<tr valign="top">
					<td>
						<br/>
						<asp:Label id="lbResume" runat="server">nbRows</asp:Label>
						<br/>
                        <%--Removed the css class in headerstyledefault and added inline property to fix gridline issue by Radha S--%>
						<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
							<DisplayLayout AllowSortingDefault="OnClient" Version="3.00"
								ViewType="OutlookGroupBy" AllowColumnMovingDefault="OnServer" HeaderClickActionDefault="SortSingle"
								AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" 
								SelectTypeColDefault="Single" NoDataMessage="">
								<Pager QuickPages="10" PageSize="200" AllowPaging="True"></Pager>
								<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true"> <%--CssClass="gh">--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CustomRules="table-layout:auto" CssClass="dataTable">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </FrameStyle>
								<ActivationObject BorderStyle="Outset" BorderWidth="2px"></ActivationObject>
								<GroupByBox ButtonConnectorStyle="Solid" ButtonConnectorColor="Navy" ShowBandLabels="IntermediateBandsOnly">
									<Style Cursor="Hand"></Style>
									<BandLabelStyle Font-Bold="True" ForeColor="White" BackColor="#093B7A"></BandLabelStyle>
								</GroupByBox>
								<SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand></igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
						<asp:PlaceHolder id="phGrids" runat="server"></asp:PlaceHolder>
					</td>
				</tr>
			</table>
</asp:Content>