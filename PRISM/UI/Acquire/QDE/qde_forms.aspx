<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.qde_forms" smartNavigation="False" CodeFile="QDE_Forms.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input forms</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script id="Infragistics" type="text/javascript" language="javascript">
		var itemNameId;
		function UpdateItemName(name)
		{
		  if (itemNameId)
		    document.getElementById(itemNameId).innerHTML = name;
		}
  </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table style="WIDTH:100%;HEIGHT:100%" cellspacing="0" cellpadding="0" border="0">
				<asp:Panel ID="pnlTitle" Runat="server">
						<tr valign="middle" align="left" style="height:20px">
							<td class="selectlanguage">&nbsp;<img alt="" title="Hide Treeview" style="vertical-align:top;border:0px;MARGIN-LEFT: 6px; MARGIN-RIGHT: 6px" onclick="HideShowTV(parent.framemain, this)"
									src="/hc_v4/img/btn_left_arrow.gif"/>
									&nbsp;<asp:Label ID="lbItemName" runat="server"></asp:Label></td>
							<td class="selectlanguage" align="right"><%#itemLevelName%></td>
						</tr>
				</asp:Panel>
				<tr valign="top" style="height:auto">
					<td colspan="2">
						<igtab:ultrawebtab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
							BorderWidth="1px" BorderColor="#949878" width="100%" Height="100%" LoadAllTargetUrls="False" BarHeight="0"
							ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
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
								<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Forms">
									<ContentPane TargetUrl="qde_content.aspx"></ContentPane>
								</igtab:Tab>
							</Tabs>
						</igtab:ultrawebtab>						
					</td>
				</tr>
			</table>
</asp:Content>