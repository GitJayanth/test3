<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="hc_termbase.UI.TermBaseSettings"
  CodeFile="Setting_TermTypeCultures.aspx.cs" %>

<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Settings</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
  
			function uwToolbar_Click(oToolbar, oButton, oEvent){
				if (oButton.Key == 'List') {
					back();
					oEvent.cancelPostBack = true;
				}
				if (oButton.Key == 'Delete') {
					if (dg_nbItems_Checked==0)
		      {
	          alert('You must select at least one item');
	          oEvent.cancelPostBack = true;
	        }
	        else
	        {
            oEvent.cancelPostBack = !confirm("Are you sure?");
          }
				}
			} 
			
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table>
    <tr valign="bottom" height="*">
      <td>
        <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
          CssClass="hc_toolbar">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
          </ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBLabel Text="Type">
              <DefaultStyle Width="40px" Font-Bold="True">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom Width="85px" runat="server">
              <asp:DropDownList ID="DDL_TermTypeList" DataValueField="Code" DataTextField="Name"
                Width="80px" runat="server" AutoPostBack="True">
              </asp:DropDownList>
            </igtbar:TBCustom>
            <igtbar:TBLabel Text="Region">
              <DefaultStyle Width="50px" Font-Bold="True">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom Width="180px" runat="server">
              <asp:DropDownList ID="DDL_RegionList" DataValueField="Code" DataTextField="Name"
                Width="150px" runat="server" AutoPostBack="True">
              </asp:DropDownList>
            </igtbar:TBCustom>
            </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top">
      <td class="main">
        <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            NoDataMessage="No data to display">
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
              InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn Key="LanguageCode" Hidden="True" BaseColumnName="LanguageCode">
                </igtbl:UltraGridColumn>
                <igtbl:TemplatedColumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
                  <CellStyle VerticalAlign="Top" Wrap="True">
                  </CellStyle>
                  <Footer Key="Select" Caption="">
                  </Footer>
                  <Header Key="Select">
                  </Header>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn HeaderText="Language" Key="LanguageName" Width="250px" BaseColumnName="LanguageName">
                  <Footer Key="Language">
                  </Footer>
                  <Header Key="Language" Caption="Language">
                  </Header>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
      </td>
    </tr>
  </table>
</asp:Content>
