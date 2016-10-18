<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="InputForms_ContainersEdit" CodeFile="InputForms_ContainersEdit.aspx.cs"%>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Edit input form container</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
<script language=javascript type="text/javascript">
  var curTab;
  function webTab_InitializeTabs(oWebTab)
  {
	  curTab = oWebTab;
  }
</script>
	<table class="main" height="20" cellspacing="0" cellpadding="0">
		<tr valign="top" style="height:1px">
			<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ImageDirectory=" " CssClass="hc_toolbar" ItemWidthDefault="80px"
					width="100%">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBLabel Text="Action" Key="Action">
							<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="top">
			<td>
			  <igtab:ultrawebtab id="webTab" runat="server" width="100%" BarHeight="0" ThreeDEffect="False" SpaceOnRight="0"
					DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%" BorderColor="#949878" BorderWidth="1px" BorderStyle="Solid"
					LoadAllTargetUrls="false" DynamicTabs="true">
					<DefaultTabStyle Height="25px" Font-Size="XX-Small" ForeColor="Black" BackColor="#FEFCFD">
						<Padding Bottom="0px" Top="1px"></Padding>
					</DefaultTabStyle>
					<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="ig_tab_lightb2.gif"
						NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
					<SelectedTabStyle>
						<Padding Bottom="1px" Top="0px"></Padding>
					</SelectedTabStyle>
          <ClientSideEvents InitializeTabs="webTab_InitializeTabs" />
					<Tabs>
						<igtab:Tab Key="Properties" DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="InputForms_ContainersEditProperties.aspx" BorderStyle="None"></ContentPane>
						</igtab:Tab>
						<igtab:Tab Key="PossibleValues" DefaultImage="/hc_v4/img/ed_possiblevalues.gif" Text=" Values" Visible="False">
							<ContentPane TargetUrl="InputForms_ContainersEditValues.aspx"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab>
			</td>
		</tr>
	</table>
</asp:Content>