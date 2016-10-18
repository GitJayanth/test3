<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="ChunkWindow" CodeFile="chunk.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server"><%#container.Name%> (<%#container.Tag%>) properties</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language='javascript' src='..\QDE\qde_chunk.js'></script>
<script>
  top.window.document.title = document.title;
</script>
			<input id="chunkHeight" type="hidden" value="317" name="chunkHeight" runat="server">
			<input id="resourceHeight" type="hidden" value="520" name="resourceHeight" runat="server">
			<table class="main" style="WIDTH: 100%; HEIGHT: 100%" height="20" cellspacing="0" cellpadding="0"
				border="0">
				<tr height="1">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbartitle">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<Items>
								<igtbar:TBLabel Text="Action" Key="Action">
									<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="">
									<DefaultStyle Width="5px"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="Culture" ImageAlign="Right" Key="Culture">
									<DefaultStyle Width="180px" Font-Size="9pt" Font-Bold="True" TextAlign="Right"></DefaultStyle>
								</igtbar:TBLabel>
							</Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<igtab:ultrawebtab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
							BorderWidth="1px" BorderColor="#949878" width="100%" Height="101%" LoadAllTargetUrls="False" BarHeight="0"
							ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
							<DefaultTabStyle Height="21px" Font-Size="8pt" Font-Names="Microsoft Sans Serif" ForeColor="Black" BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/inf/images/ig_tab_winXP1.gif"
								NormalImage="/hc_v4/inf/images/ig_tab_winXP3.gif" HoverImage="/hc_v4/inf/images/ig_tab_winXP2.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
							<Tabs>
								<igtab:Tab Key="Chunk" DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
									<ContentPane TargetUrl="/hc_v4/dummy.htm"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Master" DefaultImage="/hc_v4/img/ed_master.gif" Text=" Fallback" Visible="False">
									<ContentPane TargetUrl="Chunk_master.aspx" BorderStyle="None"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Container" DefaultImage="/hc_v4/img/ed_containers.gif" Text=" Dictionary">
									<ContentPane TargetUrl="Chunk_Container.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="History" DefaultImage="/hc_v4/img/ed_history.gif" Text=" History" Visible="False">
									<ContentPane TargetUrl="Chunk_History.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Regionalization" DefaultImage="/hc_v4/img/ed_translate.gif" Text=" Regionalizations" Visible="False">
									<ContentPane TargetUrl="Chunk_Regionalizations.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Translation" DefaultImage="/hc_v4/img/ed_translate.gif" Text=" Translations" Visible="False">
									<ContentPane TargetUrl="Chunk_Localizations.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="PossibleValues" DefaultImage="/hc_v4/img/ed_possiblevalues.gif" Text=" Values" Visible="False">
									<ContentPane TargetUrl="Chunk_PossibleValues.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Inheritance" DefaultImage="/hc_v4/img/ed_inheritance.gif" Text=" Inheritance" Visible="False">
									<ContentPane TargetUrl="chunk_inheritance.aspx"></ContentPane>
								</igtab:Tab>
								<igtab:Tab Key="Image" DefaultImage="/hc_v4/img/ed_eye.gif" Text=" Preview" Visible="False">
									<ContentPane TargetUrl="chunk_previewphoto.aspx"></ContentPane>
								</igtab:Tab>
							</Tabs>
						</igtab:ultrawebtab>
					</td>
				</tr>
			</table>
</asp:Content>