<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true"
    CodeFile="qde_countryviewmain.aspx.cs" Inherits="UI_Acquire_QDE_qde_countryviewain"
    SmartNavigation="False" %>

<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
    <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="middle" style="height: 1px">
            <td class="selectlanguage" style="font-size: x-small; width: 934px;">
                &nbsp;<img alt="" title="Hide Treeview" style="vertical-align: middle; border: 0px;
                    margin-left: 6px; margin-right: 6px" onclick="HideShowTV(parent.framemain, this)"
                    src="/hc_v4/img/btn_left_arrow.gif" />&nbsp;
                <asp:Label ID="lTitle" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="selectlanguage" align="right" style="width: 71px">
                <asp:Label ID="lLevel" runat="server" Text="Label"></asp:Label></td>
        </tr>
        <tr valign="top" style="height: auto">
            <td colspan="2">
                <igtab:UltraWebTab ID="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm"
                    BorderStyle="Solid" BorderWidth="1px" BorderColor="#949878" Width="100%" Height="100%"
                    LoadAllTargetUrls="False" BarHeight="0" ThreeDEffect="False" DynamicTabs="False"
                    SpaceOnRight="0" DisplayMode="SingleRow" ImageDirectory="/hc_v4/inf/images/">
                    <DefaultTabStyle Height="21px" Font-Size="8pt" Font-Names="Microsoft Sans Serif"
                        ForeColor="Black" BackColor="#FEFCFD">
                        <Padding Bottom="0px" Top="1px"></Padding>
                    </DefaultTabStyle>
                    <RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/inf/images/ig_tab_winXP1.gif"
                        NormalImage="/hc_v4/inf/images/ig_tab_winXP3.gif" HoverImage="/hc_v4/inf/images/ig_tab_winXP2.gif"
                        FillStyle="LeftMergedWithCenter"></RoundedImage>
                    <SelectedTabStyle>
                        <Padding Bottom="1px" Top="0px"></Padding>
                    </SelectedTabStyle>
                    <Tabs>
                        <igtab:Tab Text=" Information" Key="info" DefaultImage="/hc_v4/img/ed_about.gif">
                            <ContentPane TargetUrl="./qde_countryview.aspx?view=info" Visible="True">
                            </ContentPane>
                        </igtab:Tab>
                        <igtab:Tab Text=" Content" Key="content" DefaultImage="/hc_v4/img/ed_content.gif">
                            <ContentPane TargetUrl="./qde_countryview.aspx?view=content" Visible="True">
                            </ContentPane>
                        </igtab:Tab>
                        <igtab:Tab Text=" Links" Key="links" DefaultImage="/hc_v4/img/ed_links.gif">
                            <ContentPane TargetUrl="./qde_countryview.aspx?view=links" Visible="True">
                            </ContentPane>
                        </igtab:Tab>
                        <%--<igtab:Tab Text=" Cross Sell" Key="cross" DefaultImage="/hc_v4/img/ed_cross.gif">
								<ContentPane TargetUrl="./qde_countryview.aspx?view=cross" Visible="True"></ContentPane>
							</igtab:Tab>--%>
                        <igtab:Tab Text=" Overview" Key="all">
                            <ContentPane TargetUrl="./qde_countryview.aspx?view=all" Visible="True">
                            </ContentPane>
                        </igtab:Tab>
                        <igtab:Tab Text=" PDBView" Key="tb_PDBVIew">
                            <ContentPane TargetUrl="./qde_PDBView.aspx" Visible="True">
                            </ContentPane>
                        </igtab:Tab>
                    </Tabs>
                </igtab:UltraWebTab>
            </td>
        </tr>
    </table>
</asp:Content>
