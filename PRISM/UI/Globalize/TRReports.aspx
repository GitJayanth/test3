<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="TRReports.aspx.cs" Inherits="UI_Globalize_TRReports" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Translation follow up</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table class="main" cellspacing="0" cellpadding="0">
			<tr valign="top">
				<td>
					<igtab:ultrawebtab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
							BorderWidth="1px" BorderColor="#949878" width="100%" Height="101%" LoadAllTargetUrls="False" BarHeight="0"
							ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="SingleRow" ImageDirectory="/hc_v4/inf/images/">
							<DefaultTabStyle Height="21px" Font-Size="8pt" Font-Names="Microsoft Sans Serif" ForeColor="Black"
								BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/inf/images/ig_tab_winXP1.gif"
								NormalImage="/hc_v4/inf/images/ig_tab_winXP3.gif" HoverImage="/hc_v4/inf/images/ig_tab_winXP2.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
						<Tabs>
							<igtab:Tab Text="Upcoming TRs (next 30 days)">
								<ContentPane TargetUrl="./TRReports/Report.aspx?r=1" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Text="TR in progress">
								<ContentPane TargetUrl="./TRReports/Report.aspx?r=2" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab Text="Projects without BOT">
								<ContentPane TargetUrl="./TRReports/Report.aspx?r=3" Visible="False"></ContentPane>
							</igtab:Tab>
						</Tabs>
					</igtab:UltraWebTab></td>
			</tr>
  </table>
</asp:Content>

