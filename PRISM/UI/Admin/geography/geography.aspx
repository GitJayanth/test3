<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Architecture.Geography" CodeFile="Geography.aspx.cs" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Geography</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table style="WIDTH: 100%;HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td>
				<igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderColor="#808080" BorderStyle="Solid"
					BorderWidth="1px" LoadAllTargetUrls="False" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%">
					<DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
					<ROUNDEDIMAGE FillStyle="LeftMergedWithCenter" NormalImage="\hc_v4\inf\Images\ig_tab_lightb1.gif" SelectedImage="\hc_v4\inf\Images\ig_tab_lightb2.gif"></ROUNDEDIMAGE>
					<TABS>
						<igtab:Tab Text="Regions" Key="Regions" DefaultImage="/hc_v4/img/ed_properties.gif">
							<CONTENTPANE TargetUrl="./Regions.aspx">
							</CONTENTPANE>
						</igtab:Tab>
						<igtab:Tab Text="Countries" Key="Countries" DefaultImage="/hc_v4/img/ed_properties.gif">
							<CONTENTPANE TargetUrl="./Countries.aspx">
							</CONTENTPANE>
						</igtab:Tab>
					</TABS>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
</asp:Content>
