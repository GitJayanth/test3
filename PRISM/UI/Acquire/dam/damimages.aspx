<%@ Page language="c#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.DAM.DAMImages" CodeFile="DAMImages.aspx.cs" %>
<%@ Register TagPrefix="ucs" TagName="ResourceList" Src="./../../DAM/ResourceList2.ascx" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" runat="server">Digital Asset Manager</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
	<igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderColor="#808080" BorderStyle="Solid"
		BorderWidth="1px" LoadAllTargetUrls="False" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%">
		<DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
		<ROUNDEDIMAGE FillStyle="LeftMergedWithCenter" NormalImage="\hc_v4\inf\Images\ig_tab_lightb1.gif" SelectedImage="\hc_v4\inf\Images\ig_tab_lightb2.gif"></ROUNDEDIMAGE>
		<TABS>
			<igtab:Tab Key="Resources" Text="Resources" DefaultImage="/hc_v4/img/ed_properties.gif">
				<CONTENTPANE TargetUrl="./ResourceList.aspx?w=1">
				</CONTENTPANE>
			</igtab:Tab>
			<igtab:Tab Key="Libraries" Text="Libraries" DefaultImage="/hc_v4/img/ed_properties.gif">
				<CONTENTPANE TargetUrl="./LibraryList.aspx?w=1">
				</CONTENTPANE>
			</igtab:Tab>
		</TABS>
	</igtab:ultrawebtab>
	<ucs:ResourceList id="resourceList" runat="server" Visible="false" WorkSpaceId="1"></ucs:ResourceList>
</asp:Content>