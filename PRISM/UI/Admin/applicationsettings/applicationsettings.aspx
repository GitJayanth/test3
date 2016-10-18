<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Architecture.ApplicationSettings" CodeFile="ApplicationSettings.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
	<table cellpadding="0" cellspacing="0" border="0" width="100%"><TR><td class="hc_pagetitle">
	Application settings
	</td>
	<td style="text-align:right;"><asp:ImageButton id="backButton" runat="server" visible="False" ImageUrl="/hc_v4/img/ed_back.gif" ToolTip="Back"></asp:ImageButton></td>
	</tr></table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table style="WIDTH: 100%;HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td>
				<igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderColor="#808080" BorderStyle="Solid"
					BorderWidth="1px" LoadAllTargetUrls="False" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%">
					<DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
					<ROUNDEDIMAGE FillStyle="LeftMergedWithCenter" NormalImage="\hc_v4\inf\Images\ig_tab_lightb1.gif" SelectedImage="\hc_v4\inf\Images\ig_tab_lightb2.gif"></ROUNDEDIMAGE>
					<TABS>
						<igtab:Tab Text="Parameters" Key="Parameters" DefaultImage="/hc_v4/img/ed_properties.gif">
							<CONTENTPANE TargetUrl="./Parameters.aspx">
							</CONTENTPANE>
						</igtab:Tab>
						<igtab:Tab Text="Components" Key="Components" DefaultImage="/hc_v4/img/ed_properties.gif">
							<CONTENTPANE TargetUrl="./Components.aspx">
							</CONTENTPANE>
						</igtab:Tab>
					</TABS>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
</asp:Content>
