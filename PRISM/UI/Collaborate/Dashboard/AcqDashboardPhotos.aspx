<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="AcqDashboardPhotos.aspx.cs" Inherits="UI_Collaborate_Dashboard_AcqDashboardPhotos" %>
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
          Width="100%" OnInitializeRow="dgResults_InitializeRow">
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
          <Bands>
            <igtbl:UltraGridBand Key="Org" Indentation="15">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="OrgCode" Hidden="True" Key="OrgCode">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="OrgName" HeaderText="Organization/Group/GBU/PL" Key="OrgName" Width="315px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NbItems" HeaderText="Items count" Key="NbItems" Width="60px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170" HeaderText="# 190x170" Key="NBItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170Missing" HeaderText="missing 190x170" Key="NBItems_190x170Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170" HeaderText="% 190x170" Key="PercItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100" HeaderText="# 70x100 " Key="NBItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100Missing" HeaderText="missing 70x100" Key="NBItems_70x100Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100" HeaderText="% 70x100" Key="PercItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170series" HeaderText="# 190x170 series" Key="NBItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170seriesMissing" HeaderText="missing 190x170 series" Key="NBItems_190x170seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170series" HeaderText="% 190x170 series" Key="PercItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>                                
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100series" HeaderText="# 70x100 series" Key="NBItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100seriesMissing" HeaderText="missing 70x100 series" Key="NBItems_70x100seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100series" HeaderText="% 70x100 series" Key="PercItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn> 
              </Columns>
              <RowStyle CssClass="ugd" BackColor="#F5F5F5" />    
              <HeaderStyle Height="40px" />
            </igtbl:UltraGridBand>
            <igtbl:UltraGridBand Key="Group" Indentation="15">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="GroupCode" Hidden="True" Key="GroupCode">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="GroupName" HeaderText="Group" Key="GroupName" Width="300px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NbItems" HeaderText="Items count" Key="NbItems" Width="60px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170" HeaderText="# 190x170" Key="NBItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170Missing" HeaderText="missing 190x170" Key="NBItems_190x170Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170" HeaderText="% 190x170" Key="PercItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100" HeaderText="# 70x100 " Key="NBItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100Missing" HeaderText="missing 70x100" Key="NBItems_70x100Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100" HeaderText="% 70x100" Key="PercItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170series" HeaderText="# 190x170 series" Key="NBItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170seriesMissing" HeaderText="missing 190x170 series" Key="NBItems_190x170seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170series" HeaderText="% 190x170 series" Key="PercItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>                                
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100series" HeaderText="# 70x100 series" Key="NBItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100seriesMissing" HeaderText="missing 70x100 series" Key="NBItems_70x100seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100series" HeaderText="% 70x100 series" Key="PercItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn> 
              </Columns>
              <RowStyle CssClass="ugd" BackColor="#F1F9F9" />              
              <HeaderStyle Height="0px" />
            </igtbl:UltraGridBand>
            <igtbl:UltraGridBand Key="GBU" Indentation="15">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="GBUCode" Hidden="True" Key="GBUCode">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="GBUName" HeaderText="GBU" Key="GBUName" Width="285px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NbItems" HeaderText="Items count" Key="NbItems" Width="60px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170" HeaderText="# 190x170" Key="NBItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170Missing" HeaderText="missing 190x170" Key="NBItems_190x170Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170" HeaderText="% 190x170" Key="PercItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100" HeaderText="# 70x100 " Key="NBItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100Missing" HeaderText="missing 70x100" Key="NBItems_70x100Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100" HeaderText="% 70x100" Key="PercItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170series" HeaderText="# 190x170 series" Key="NBItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170seriesMissing" HeaderText="missing 190x170 series" Key="NBItems_190x170seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170series" HeaderText="% 190x170 series" Key="PercItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>                                
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100series" HeaderText="# 70x100 series" Key="NBItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100seriesMissing" HeaderText="missing 70x100 series" Key="NBItems_70x100seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100series" HeaderText="% 70x100 series" Key="PercItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn> 
              </Columns>
              <RowStyle CssClass="ugd" BackColor="#FAFCF1" />              
              <HeaderStyle Height="0px" />
            </igtbl:UltraGridBand>
            <igtbl:UltraGridBand Key="PL" Indentation="15">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="PLCode" Hidden="True" Key="PLCode">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PLName" HeaderText="PL" Key="PLName" Width="270px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NbItems" HeaderText="Items count" Key="NbItems" Width="60px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170" HeaderText="# 190x170" Key="NBItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170Missing" HeaderText="missing 190x170" Key="NBItems_190x170Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170" HeaderText="% 190x170" Key="PercItems_190x170" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100" HeaderText="# 70x100 " Key="NBItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100Missing" HeaderText="missing 70x100" Key="NBItems_70x100Missing" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100" HeaderText="% 70x100" Key="PercItems_70x100" Width="50px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170series" HeaderText="# 190x170 series" Key="NBItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_190x170seriesMissing" HeaderText="missing 190x170 series" Key="NBItems_190x170seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_190x170series" HeaderText="% 190x170 series" Key="PercItems_190x170series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>                                
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100series" HeaderText="# 70x100 series" Key="NBItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NBItems_70x100seriesMissing" HeaderText="missing 70x100 series" Key="NBItems_70x100seriesMissing" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PercItems_70x100series" HeaderText="% 70x100 series" Key="PercItems_70x100series" Width="70px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderStyle wrap="true"></HeaderStyle>
                </igtbl:UltraGridColumn> 
              </Columns>
              <RowStyle CssClass="ugd" BackColor="#FFFFFF" />              
              <HeaderStyle Height="0px" />
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
    </td>
  </tr>
</table>
</asp:Content>