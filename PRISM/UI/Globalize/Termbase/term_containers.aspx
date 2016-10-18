<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="hc_termbase.UI.Globalize.Termbase.Term_Containers"
  CodeFile="Term_Containers.aspx.cs" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Term translations</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script type="text/javascript">
		
			function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
					oEvent.cancelPostBack = true;
        }
      }
		
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <tr height="*" valign="bottom">
    <td align="right">
      <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
        CssClass="hc_toolbar" ImageDirectory=" ">
        <HoverStyle CssClass="hc_toolbarhover">
        </HoverStyle>
        <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
        </ClientSideEvents>
        <SelectedStyle CssClass="hc_toolbarselected">
        </SelectedStyle>
        <Items>
          <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
          </igtbar:TBarButton>
        </Items>
        <DefaultStyle CssClass="hc_toolbardefault">
        </DefaultStyle>
      </igtbar:UltraWebToolbar>
    </td>
  </tr>
  <tr valign="top">
    <td>
      <asp:Label ID="lbError" runat="server" Width="100%" Visible="False">Error message</asp:Label>
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
          <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
          </ClientSideEvents>
        </DisplayLayout>
        <Bands>
          <igtbl:UltraGridBand>
            <Columns>
              <igtbl:UltraGridColumn Key="ContainerId" Width="10px" Hidden="True" BaseColumnName="ContainerId">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Tag" Key="Tag" Width="200px" BaseColumnName="Tag">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Name" Key="ContainerName" Width="100%" BaseColumnName="Name">
              </igtbl:UltraGridColumn>
            </Columns>
          </igtbl:UltraGridBand>
        </Bands>
      </igtbl:UltraWebGrid></td>
  </tr>
</asp:Content>
