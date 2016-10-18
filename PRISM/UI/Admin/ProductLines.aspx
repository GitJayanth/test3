<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.UIProductLines" CodeFile="ProductLines.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Product Lines and Businesses</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <style type="text/css">
  .org {font-size: medium;padding-left:5px;color:darkblue;}
  .group {font-size: small;padding-left:15px;color:orange}
  .gbu {font-size: x-small;padding-left:25px;color:green}
  .pl {font-size: xx-small;padding-left:35px;}
  </style>

  <script type="text/javascript" language="javascript">
  		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          oEvent.cancelPostBack = true;
        }
        if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
		  } 
  </script>

  <table class="main" style="width: 100%; height: 100%" cellspacing="0" cellpadding="0"
    border="0">
    <tr valign="top">
      <td class="sectionTitle">
        <asp:Label ID="lbTitle" runat="server">Product Lines and Businesses list</asp:Label></td>
    </tr>
    <asp:Panel ID="panelGrid" runat="server" Visible="True">
      <tr valign="top" style="height: 1px">
        <td>
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="25px"
           OnButtonClicked="uwToolbar_ButtonClicked" Height="24px">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <Items>
              <igtbar:TBarButton Key="Export" ToolTip="Export current view" Image="/hc_v4/img/ed_download.gif" Text="Export">
                <DefaultStyle Width="80px"></DefaultStyle>
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="Export"></igtbar:TBSeparator>
              <igtbar:TBarButton ToggleButton="True" Selected="false" Key="Active" ToolTip="Filter mandatory" Text="Show All">
                <DefaultStyle Width="80px" Font-Bold="True"></DefaultStyle>
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="ShowAll"></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" ToolTip="To add a new PL Code">
			    <DefaultStyle Width="80px"></DefaultStyle>
			  </igtbar:TBarButton>
            </Items>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr valign="top">
        <td>
            <asp:Label ID="lbPLs" runat="server">PLs Table</asp:Label>
        </td>
      </tr>
    </asp:Panel>
    <tr valign="top">
      <td>
						<igtab:ultrawebtab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
							BorderWidth="1px" BorderColor="#949878" width="100%" Height="101%" LoadAllTargetUrls="False" BarHeight="0"
							ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
							<DefaultTabStyle Height="25px" Font-Size="8pt" Font-Names="Microsoft Sans Serif" ForeColor="Black"
								BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="ig_tab_lightb2.gif"
								NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
          <Tabs>
            <igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties" Key="Properties">
              <ContentPane TargetUrl="./pls/pl_Properties.aspx" Visible="False">
              </ContentPane>
            </igtab:Tab>
            <igtab:Tab DefaultImage="/hc_v4/img/ed_users.gif" Text="Users" Key="Users">
              <ContentPane TargetUrl="./pls/pl_users.aspx" Visible="False">
              </ContentPane>
            </igtab:Tab>
            <igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="New PL" Key="AddPL">
              <ContentPane TargetUrl="./pls/PL_Add.aspx" Visible="False">
              </ContentPane>
            </igtab:Tab>
          </Tabs>
        </igtab:UltraWebTab>
      </td>
    </tr>
  </table>
</asp:Content>
