<%@ Reference Page="~/ui/acquire/qde/preview/preview.aspx" %>

<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Globalize.Preview"
  CodeFile="Preview.aspx.cs" %>

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="~/UI/Acquire/Chunk/Chunk_Modifier.ascx" %>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="~/UI/Acquire/Chunk/Chunk_Comment.ascx" %>
<%@ Register TagPrefix="hc" TagName="containerInfo" Src="~/UI/Acquire/Chunk/ContainerInfo.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  TR preview</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <hc:containerInfo ID="ContainerInfoPanel" runat="server" Visible="false"></hc:containerInfo>
  <table id="Table1" style="width: 100%; height: 100%" cellspacing="0" cellpadding="0"
    border="0">
    <asp:Panel ID="PanelChunk" runat="server">
    <tr height="1" valign="top">
          <td>
            <igtbar:UltraWebToolbar ID="uwToolbarTitle" runat="server" CssClass="hc_toolbartitle"
              Width="100%" BackgroundImage="" ImageDirectory="" ItemWidthDefault="">
              <Items>
                <igtbar:TBLabel Key="ItemName" Text="ItemName">
                  <DefaultStyle Font-Bold="True" Font-Size="9pt" TextAlign="Left">
                  </DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBLabel Key="Culture" Text="Culture" ImageAlign="Right">
                  <DefaultStyle Font-Bold="True" Font-Size="9pt" TextAlign="Right">
                  </DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBLabel Text="" ImageAlign="NotSet">
                  <DefaultStyle TextAlign="Right" Width="5px">
                  </DefaultStyle>
                </igtbar:TBLabel>
              </Items>
            </igtbar:UltraWebToolbar>
          </td>
        </tr>
      <tr valign="top">
        <td>
          <igtab:UltraWebTab ID="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
            BorderWidth="1px" BorderColor="#949878" Width="100%" Height="101%" LoadAllTargetUrls="False"
            BarHeight="0" ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="Scrollable"
            ImageDirectory="/hc_v4/inf/images/">
            <DefaultTabStyle Height="25px" Font-Size="8pt" Font-Names="Microsoft Sans Serif"
              ForeColor="Black" BackColor="#FEFCFD">
              <Padding Bottom="0px" Top="1px"></Padding>
            </DefaultTabStyle>
            <RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="ig_tab_lightb2.gif"
              NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
            <SelectedTabStyle>
              <Padding Bottom="1px" Top="0px"></Padding>
            </SelectedTabStyle>
            <Tabs>
              <igtab:Tab Text="ContainerName" DefaultImage="/hc_v4/img/ed_containers.gif">
									<ContentPane TargetUrl="/hc_v4/dummy.htm"></ContentPane>
              </igtab:Tab>
              <igtab:Tab Text="All Content" DefaultImage="/hc_v4/img/ed_properties.gif">
                <ContentPane TargetUrl="../../Acquire/QDE/qde_formcontent.aspx">
                </ContentPane>
              </igtab:Tab>
            </Tabs>
          </igtab:UltraWebTab>
        </td>
      </tr>
    </asp:Panel>
      
      <asp:Panel ID="PanelChunkDetail" runat="server">
        <tr valign="top" style="height: 1px">
          <td>
            <fieldset>
              <legend>
                <asp:Label ID="Label6" runat="server">Value is</asp:Label><font color="gray">
                  <asp:Label ID="lbStatus" runat="server">Label</asp:Label></font>&nbsp;
                <asp:Image ID="imgStatus" runat="server" AlternateText="status" ImageAlign="Middle">
                </asp:Image>&nbsp;</legend>
              <asp:TextBox ID="txtValue" runat="server" Columns="80" TextMode="MultiLine" Rows="4"></asp:TextBox>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" style="height: 1px">
          <td>
            <hc:chunkcomment ID="ChunkComment1" runat="server"></hc:chunkcomment>
          </td>
        </tr>
        <tr valign="top" style="height: 1px">
          <td>
            <hc:chunkModifier ID="ChunkModifier1" runat="server"></hc:chunkModifier>
            <center>
              &nbsp;</center>
          </td>
        </tr>
      </asp:Panel>
    <tr valign="bottom" height="*">
      <td align="right">
      </td>
    </tr>
  </table>
</asp:Content>
