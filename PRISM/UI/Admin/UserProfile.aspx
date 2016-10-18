<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.UserProfile" CodeFile="UserProfile.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">My profile</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <igtab:UltraWebTab id="webTab" runat="server" DynamicTabs="False" SpaceOnRight="0" BarHeight="0" BorderWidth="1px"
    BorderStyle="Solid" BorderColor="#949878" Height="100%" ThreeDEffect="False" DummyTargetUrl="/hc_v4/pleasewait.htm"
    Width="100%" LoadAllTargetUrls="False">
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
      <igtab:TabSeparator Tag="preset">
        <Style Width="2px"></Style>
      </igtab:TabSeparator>
      <igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="  Identification">
        <ContentPane TargetUrl="userprofile/userprofile_identification.aspx" BorderWidth="0px" BorderStyle="None"></ContentPane>
      </igtab:Tab>
<%--      <igtab:Tab DefaultImage="/hc_v4/img/ed_password.gif" Key="Password" Text="  Password">
        <ContentPane TargetUrl="userprofile/userprofile_password.aspx" BorderWidth="0px" BorderStyle="None"></ContentPane>
      </igtab:Tab>--%>
      <igtab:Tab DefaultImage="/hc_v4/img/ed_notify.gif" Text="  Notifications">
        <ContentPane TargetUrl="userprofile/userprofile_notifications.aspx" BorderWidth="0px" BorderStyle="None"></ContentPane>
      </igtab:Tab>
      <igtab:Tab DefaultImage="/hc_v4/img/ed_items.gif" Text="  Items">
        <ContentPane TargetUrl="userprofile/userprofile_products.aspx" BorderWidth="0px" BorderStyle="None"></ContentPane>
      </igtab:Tab>
      <igtab:Tab DefaultImage="/hc_v4/img/ed_capabilities.gif" Text="  Capabilities">
        <ContentPane TargetUrl="userprofile/userprofile_capabilities.aspx" BorderWidth="0px" BorderStyle="None"></ContentPane>
      </igtab:Tab>
      <igtab:Tab DefaultImage="/hc_v4/img/ed_translate.gif" Text="  Catalogs">
        <ContentPane TargetUrl="userprofile/userprofile_Localizations.aspx" BorderWidth="0px" BorderStyle="None"></ContentPane>
      </igtab:Tab>
    </Tabs>
  </igtab:UltraWebTab>
</asp:Content>
