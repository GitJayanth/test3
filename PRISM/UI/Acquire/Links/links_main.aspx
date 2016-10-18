<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="links_main" CodeFile="Links_main.aspx.cs" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">List</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		//document.body.oncontextmenu = function(){return false;};
</script>
			<asp:Label ID="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
			<igtab:ultrawebtab id="webTab" runat="server" BorderStyle="Solid" BorderWidth="1px" BorderColor="#949878"
				Height="100%" DummyTargetUrl="/hc_v4/pleasewait.htm" width="100%" SpaceOnRight="0" DynamicTabs="False"
				ThreeDEffect="False" BarHeight="0" LoadAllTargetUrls="False" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
				<DefaultTabStyle Height="21px" Font-Size="xx-small" Font-Names="Microsoft Sans Serif" ForeColor="Black" BackColor="#FEFCFD">
					<Padding Bottom="0px" Top="1px"></Padding>
				</DefaultTabStyle>
				<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/inf/images/ig_tab_winXP1.gif"
					NormalImage="/hc_v4/inf/images/ig_tab_winXP3.gif" HoverImage="/hc_v4/inf/images/ig_tab_winXP2.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
				<SelectedTabStyle>
					<Padding Bottom="1px" Top="0px"></Padding>
				</SelectedTabStyle>
				<Tabs></Tabs>
			</igtab:ultrawebtab>
</asp:Content>