<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="AcqDashboardChunks.aspx.cs" Inherits="UI_Collaborate_Dashboard_AcqDashboardChunks" %>

<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Acquisition Dashboard</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<table class="main">
  <tr valign="top" style="height:1px">
    <td>
      <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
        CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwToolbar_ButtonClicked">
        <HoverStyle CssClass="hc_toolbarhover">
        </HoverStyle>
        <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
        <SelectedStyle CssClass="hc_toolbarselected">
        </SelectedStyle>
        <Items>
          <igtbar:TBCustom ID="TBCustom1" Width="200px" runat="server">
          <asp:DropDownList ID="DDL_Cultures" runat="server" Width="200px" DataTextField="Name" DataValueField="Code" AutoPostBack="True" OnSelectedIndexChanged="DDL_Cultures_SelectedIndexChanged">
              </asp:DropDownList>
          </igtbar:TBCustom>
          <igtbar:TBSeparator />
          <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator />
          <igtbar:TBarButton Key="Expand" Text="Expand all" Image="/hc_v4/img/ed_items.gif">
            <DefaultStyle Width="100px"></DefaultStyle>
          </igtbar:TBarButton>
        </Items>
        <DefaultStyle CssClass="hc_toolbardefault">
        </DefaultStyle>
      </igtbar:UltraWebToolbar>
    </td>
  </tr>
  <tr valign="top" style="height:1px">
			<td>
				<asp:label id="lbTitle" runat="server">report</asp:label>
				<asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
			</td>
		</tr>  
  <tr valign="top">
    <td>
        <igtbl:UltraWebGrid ID="dgResults" runat="server" ImageDirectory="/ig_common/Images/"
          Width="100%" OnInitializeRow="dgResults_InitializeRow" EnableViewState="false">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" 
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dgResults" TableLayout="Fixed"
            NoDataMessage="No data to display" SelectTypeCellDefault="Extended"
            SelectTypeColDefault="Extended" ViewType="Hierarchical">
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
        </igtbl:UltraWebGrid>
    </td>
  </tr>
</table>
</asp:Content>
