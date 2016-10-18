<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="AcquisitionDashboard.aspx.cs" Inherits="UI_Collaborate_AcquisitionDashboard" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Acquisition dashboard</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table class="main" cellspacing="0" cellpadding="0">
			<tr valign="top">
				<td>
					<igtab:UltraWebTab id="webTab" runat="server" Width="100%" Height="100%" DummyTargetUrl="/hc_v4/pleasewait.htm"
						LoadAllTargetUrls="False" BorderWidth="1px" BorderStyle="Solid" BorderColor="Gray">
						<DefaultTabStyle Height="25px" BackColor="WhiteSmoke"></DefaultTabStyle>
						<RoundedImage SelectedImage="ig_tab_lightb2.gif" NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
						<Tabs>
							<igtab:Tab Text="Items" Key="Items">
								<ContentPane TargetUrl="./Dashboard/AcqDashboardItems.aspx?d=1" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Text="Chunks" Key="Chunks">
								<ContentPane TargetUrl="./Dashboard/AcqDashboardChunks.aspx?d=1" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Text="Photos" Key="Photos">
								<ContentPane TargetUrl="./Dashboard/AcqDashboardPhotos.aspx?d=1" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Text="Product activity" Key="MonthlySkus">
								<ContentPane TargetUrl="./Dashboard/AcqDashboardMonthlySkus.aspx?d=1" Visible="False"></ContentPane>
							</igtab:Tab>

						</Tabs>
					</igtab:UltraWebTab></td>
			</tr>
  </table> 
</asp:Content>