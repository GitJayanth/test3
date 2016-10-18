<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.SearchText" CodeFile="SearchText.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Advanced search</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table style="WIDTH: 100%;HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td>
                <igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderColor="#808080" BorderStyle="Solid"
					BorderWidth="1px" LoadAllTargetUrls="False" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%">
					<DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
					<ROUNDEDIMAGE FillStyle="LeftMergedWithCenter" NormalImage="\hc_v4\inf\Images\ig_tab_lightb1.gif" SelectedImage="\hc_v4\inf\Images\ig_tab_lightb2.gif"></ROUNDEDIMAGE>
					<TABS>
						<igtab:Tab Text="Build a search query" Key="Custom">
							<CONTENTPANE TargetUrl="./SearchText/CustomSearch.aspx"></CONTENTPANE>
						</igtab:Tab>
						<igtab:Tab Text="Perform existing search" Key="Templates">
							<CONTENTPANE TargetUrl="./SearchText/TemplateSearch.aspx">
							</CONTENTPANE>
						</igtab:Tab>
					</TABS>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
</asp:Content>
