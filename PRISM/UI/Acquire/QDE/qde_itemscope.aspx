<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_ItemScope" CodeFile="QDE_ItemScope.aspx.cs"%>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Instant TR creation</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script type="text/javascript">
      ////////////////////////////////////////////////////////////////////////////////////////////// 
      function GI(webTabId, all, itemId){
      //////////////////////////////////////////////////////////////////////////////////////////////
        if (parent) 
        {
          setTimeout("parent.framecontent.location='/hc_v4/pleasewait.htm'", 10);
          setTimeout("parent.frametv.location='/hc_v4/pleasewait.htm'", 10);
          setTimeout("parent.framecontent.location='QDE_FormRoll.aspx?i=" + itemId + "'", 100);
          setTimeout("parent.frametv.location='QDE_TV.aspx?all=" + all + "'", 100);
        }
      }
      //////////////////////////////////////////////////////////////////////////////////////////////
      function GO(webTabId){
      //////////////////////////////////////////////////////////////////////////////////////////////
        if (parent) 
        {
          setTimeout("parent.frametv.location='/hc_v4/pleasewait.htm'", 10);
          setTimeout("parent.frametv.location='QDE_TVOptions.Aspx'", 100);
        }
      }
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:auto">
          <td>
            <igtab:ultrawebtab id="webTab" runat="server" SpaceOnRight="0" DynamicTabs="False" ThreeDEffect="False"
              BarHeight="0" Height="101%" width="100%" BorderColor="#949878" BorderWidth="1px" BorderStyle="Solid"
              DummyTargetUrl="/hc_v4/pleasewait.htm" AutoPostBack="True" OnTabClick="webTab_TabClick">
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
                <igtab:Tab Key="My" Text="My Items">
                <ContentPane TargetUrl="/hc_v4/dummy.htm"></ContentPane>
                </igtab:Tab>
                <igtab:Tab Key="All" Text="All Items">
                <ContentPane TargetUrl="/hc_v4/dummy.htm"></ContentPane>
                </igtab:Tab>
                <igtab:Tab Key="Options" Text="Options">
                <ContentPane TargetUrl="/hc_v4/dummy.htm"></ContentPane>
                </igtab:Tab>
              </Tabs>
            </igtab:ultrawebtab>
            <asp:Label id="lbScript" runat="server"></asp:Label></td>
        </tr>
      </table>
</asp:Content>