<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Architecture.ReinstateSKU"
  CodeFile="ReinstateSKU.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Restoring Obsolete Products to Live State</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script language="javascript" type="text/javascript">
    function uwToolbar_Click(oToolbar, oButton, oEvent)
    {
        if (oButton.Key == 'filter')
	     {     		     
            DoSearch();	
            oEvent.cancelPostBack = true;
            return;
          }
        
        if (oButton.Key == 'Reinstate')
	    {
            oEvent.cancelPostBack = !confirm('Are you sure want to reinstate the selected SKU(s)?');
        }
    }

    function UnloadGrid()
    {
        igtbl_unloadGrid("dg");
    }
  </script>
			<table class="main" cellspacing="0" cellpadding="0" border="1">
			<tr valign="top" style="height:1px;">
					<td>
                    <%--Removed the width tag and Itemdefaultwidth to fix enlarge button by Radha S--%>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Reinstate" Text="Reinstate" Image="/hc_v4/img/ed_override.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="ReinstateSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
                                <igtbar:TBLabel Text="Search">
                                  <DefaultStyle Font-Bold="True">
                                  </DefaultStyle>
                                </igtbar:TBLabel>
                                <igtbar:TBCustom ID="TBCustom1" Key="SearchField" runat="server">
                                  <asp:TextBox ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
                                </igtbar:TBCustom>
                                <%--Removed the text name and passing as empty bcoz the text was not displaying in Production--%>
                                <igtbar:TBarButton key="filter" Text="" Image="/hc_v4/img/ed_search.gif">
                                  <DefaultStyle>
                                  </DefaultStyle>
                                </igtbar:TBarButton>
			                  </Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
			  <tr>
                  <td class="sectionTitle">
                    <asp:Label ID="lbTitle" runat="server" EnableViewState="false">List of live products eligible for reinstatement</asp:Label></td>
              </tr>
				<tr valign="top">
					<td>
						<asp:Label id="LbNbSKUs" Runat="server" Visible="False" ForeColor="Green" EnableViewState="false"></asp:Label>
	                    <asp:Label id="lblDwlReport" Runat="server" Visible="False" ForeColor="Green" EnableViewState="false"></asp:Label>				
					    <asp:LinkButton ID="lbDwlReport" runat="server" Text="click here" EnableViewState="false" Visible="false" OnClick="lbDwlReport_Click"></asp:LinkButton>
					</td>
				</tr>
		<tr valign="top">
      <td class="main">
        <asp:Label ID="lbMessage" runat="server" Text="lbMessage" Visible="false" EnableViewState="false"></asp:Label>
        <igtbl:UltraWebGrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%"
          OnInitializeRow="dg_InitializeRow">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True">
            </Pager>
            <%--removed the css class and added the styles inline--Start%>--%>
            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true">  <%--CssClass="gh">--%>
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="65.3%" CssClass="dataTable">
            </FrameStyle>
			<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:TemplatedColumn FooterText="" Key="Select" Width="50px">
                 <HeaderTemplate>
                   <asp:CheckBox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"  EnableViewState="true"></asp:CheckBox>
                 </HeaderTemplate>
                  <CellTemplate>
                    <asp:CheckBox ID="g_sd" onclick="javascript:return g_su(this);" runat="server" EnableViewState="true"></asp:CheckBox>
                  </CellTemplate>
                  <CellStyle VerticalAlign="Middle" Wrap="True">
                  </CellStyle>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" HeaderText="ItemId" Hidden="true">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Sku" HeaderText="Item Number"
                  Key="sku">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" HeaderText="Item Name"
                  Key="ItemName" Width="300px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ClassName" HeaderText="Class Name"
                  Key="ClassName" Width="200px">
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        <center>
          <asp:Label ID="lbNoresults" runat="server" Visible="false" ForeColor="Red" EnableViewState="false" Font-Bold="True">No results</asp:Label>
        </center>
      </td>
    </tr>
</table>
<asp:HiddenField ID="hfSKUList" runat="server" Visible="false" EnableViewState="true" />
<script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>
  <script>
    window.onload=g_i
  </script>
</asp:Content>
  
