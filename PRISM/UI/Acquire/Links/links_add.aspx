<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Links.Links_addMethod" CodeFile="Links_add.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">New link</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="main" cellpadding="0" cellspacing="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolBarTitle" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbartitle">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBLabel Text="Title" Key="Title">
									<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
								</igtbar:TBLabel>
							</Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:auto">
				  <td>
					  <igtab:ultrawebtab id="webTab" runat="server" BorderStyle="Solid" BorderWidth="1px" BorderColor="#949878"
				      Height="101%" DummyTargetUrl="/hc_v4/pleasewait.htm" width="100%" SpaceOnRight="0" DynamicTabs="False"
				      ThreeDEffect="False" BarHeight="0" LoadAllTargetUrls="False" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
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
							  <igtab:Tab Key="Editor" DefaultImage="" Text="Editor">
									<ContentPane TargetUrl="./Links_Add/Links_add_editor.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Treeview" DefaultImage="" Text="Treeview">
									<ContentPane TargetUrl="./Links_Add/Links_add_treeview.aspx"></ContentPane>
								</igtab:Tab>
							</Tabs>
						</igtab:ultrawebtab>
					</td>
				</tr>
			</table>
</asp:Content>