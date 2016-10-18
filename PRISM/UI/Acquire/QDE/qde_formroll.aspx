<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_FormRoll" CodeFile="QDE_FormRoll.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input forms</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
		function UpdatePage(itemId)
		{
			if (parent)
			{
				parent.location = 'QDE_Main.aspx?i='+itemId;
	    }
		}
		
		var itemNameId;
		function UpdateItemName(name)
		{
		  if (itemNameId)
		    document.getElementById(itemNameId).innerHTML = name;
		}
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
<asp:Panel ID="pInfo" runat="server">
				<tr style="vertical-align: middle; height: 1px" align="left">
					<td class="selectlanguage">
					  &nbsp;<img src="/hc_v4/img/btn_left_arrow.gif" onclick="HideShowTV(parent.framemain, this);" style="margin-left: 6px; margin-right:6px; border-width: 0px; text-align: left" alt="Hide Treeview" />
					  &nbsp;<asp:Label ID="lbItemName" runat="server"></asp:Label>
				  </td>
					<td class="selectlanguage" align="right"><%#itemLevelName%></td>
				</tr>
				<tr style="height: 1px; vertical-align: top">
					<td>
						<asp:Label ID="lbError" Runat="server" CssClass="hc_error" Visible="false"></asp:Label>
					</td>
				</tr>
				</asp:Panel>
				<tr valign="top" style="height:auto">
					<td colspan="2">
						<igtab:ultrawebtab id="webTab" runat="server" TextOrientation="Vertical" TabOrientation="RightTop"
							BorderWidth="1px" SpaceOnRight="0" DynamicTabs="False" ThreeDEffect="False" BarHeight="0" LoadAllTargetUrls="False"
							BorderColor="#949878" BorderStyle="Solid" DummyTargetUrl="/hc_v4/pleasewait.htm" 
							Width="100%" Height="100%" ImageDirectory="/hc_v4/inf/images/" AutoPostBack="True">
							<DefaultTabStyle Width="22px" Height="100px" Font-Size="xx-Small" ForeColor="Black" BackgroundImage="/hc_v4/img/ig_tab_winXP3bis_r.gif">
								<Padding Left="0px" Right="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/img/ig_tab_winXP1_r.gif"
								NormalImage="/hc_v4/img/ig_tab_winXP3_r.gif" HoverImage="/hc_v4/img/ig_tab_winXP2_r.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle BackgroundImage="/hc_v4/img/ig_tab_winXP1_rbis.gif">
								<Padding Left="1px" Right="0px"></Padding>
							</SelectedTabStyle>
							<Tabs>
							</Tabs>
						</igtab:ultrawebtab>
					</td>
				</tr>
			</table>
</asp:Content>