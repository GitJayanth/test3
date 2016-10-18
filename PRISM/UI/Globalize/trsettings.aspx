<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globalize.TRSettings" CodeFile="TRSettings.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="iglbar" Namespace="Infragistics.WebUI.UltraWebListbar" Assembly="Infragistics2.WebUI.UltraWebListbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Translation settings</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script>
    function ConfirmPLChange(prevType){
      /*if (prevType == 'PL'){
        return confirm('Are you sure? This will erase all PL information');
      } */     
      return true;
    }
  </script>
  <table class="main" cellspacing="0" cellpadding="0" width="100%">
    <tr valign="top">
      <td class="sectionTitle">
        <asp:label id="lbTitle" runat="server">Business class options</asp:label></td>
    </tr>
    <tr valign="top">
      <td><!-- Begin option table -->
        <table height="100%" width="100%">
          <tr valign="top"> <!-- Begin left pane -->
            <td noWrap borderColor="black" width="30%">
              <iglbar:UltraWebListbar id="uwClassList" runat="server" Height="100%" BarWidth="100%" Width="100%" ViewType="ExplorerBar"
                GroupSpacing="15px" BorderStyle="None" ItemIconStyle="Square">
                <DefaultGroupStyle Cursor="Default" BackColor="WhiteSmoke"></DefaultGroupStyle>
                <DefaultItemStyle CssClass="hc_simpletoolbar"></DefaultItemStyle>
                <DefaultItemHoverStyle CssClass="hc_toolbar"></DefaultItemHoverStyle>
                <DefaultGroupHeaderAppearance>
                  <CollapsedAppearance ExpansionIndicatorImage="downarrows_white.gif">
                    <Style CssClass="hc_toolbar"></Style>
                  </CollapsedAppearance>
                  <HoverAppearance>
                    <Style CssClass="hc_toolbar"></Style>
                  </HoverAppearance>
                  <ExpandedAppearance ExpansionIndicatorImage="uparrows_white.gif">
                    <Style CssClass="hc_toolbar"></Style>
                  </ExpandedAppearance>
                </DefaultGroupHeaderAppearance>
                <Groups>
                  <iglbar:Group Key="classes" TextAlign="Left" Tag="" Text="Classes" UserControlUrl="">
                    <HeaderAppearance>
                      <CollapsedAppearance ExpansionIndicatorImage="downarrows_white.gif"></CollapsedAppearance>
                      <ExpandedAppearance ExpansionIndicatorImage="uparrows_white.gif"></ExpandedAppearance>
                    </HeaderAppearance>
                  </iglbar:Group>
                  <iglbar:Group Key="others" TextAlign="Left" Tag="" Text="Others" UserControlUrl="">
                    <HeaderAppearance>
                      <CollapsedAppearance ExpansionIndicatorImage="downarrows_white.gif"></CollapsedAppearance>
                      <ExpandedAppearance ExpansionIndicatorImage="uparrows_white.gif"></ExpandedAppearance>
                    </HeaderAppearance>
                  </iglbar:Group>
                </Groups>
                <DefaultItemSelectedStyle Cursor="Hand" Font-Bold="True" CssClass="hc_toolbartitle"></DefaultItemSelectedStyle>
              </iglbar:UltraWebListbar></td> <!-- End left pane --> <!-- Begin right pane -->
              <script type="text/javascript">document.getElementById("uwClassList").style.overflow = "visible"</script>
            <td class="ptb3" width="*">
              <IGTBAR:ULTRAWEBTOOLBAR id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
                <HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
                <DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
                <SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
                <ITEMS>
                  <IGTBAR:TBARBUTTON Text="Save" Key="save" Image="/hc_v4/img/ed_save.gif"></IGTBAR:TBARBUTTON>
                  <IGTBAR:TBLabel Text="" Key="lbl_Error"></IGTBAR:TBLabel>
                </ITEMS>
                <CLIENTSIDEEVENTS InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIDEEVENTS>
              </IGTBAR:ULTRAWEBTOOLBAR>
              <div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 95%">
                <FIELDSET>
                  <LEGEND>
                    <asp:label id="Label2" runat="server">Identity</asp:label>
                  </LEGEND>
                  <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr valign="top">
                      <td width="150" height="21"><B>
                          <asp:label id="Label3" runat="server">Name</asp:label></B></td>
                      <td width="20%" height="21"><B>
                          <asp:label id="lbItemName" runat="server">Item Name</asp:label></B></td>
                          <td>
                          <%--Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009--%>
                          <td width="50%"> <asp:Label id="lbError" runat="server"></asp:Label></td>
                        <asp:panel id="PanelRegionDDL" RunAt="server">				
						
						    <td width="55"> <B>
						        <asp:Label id ="LabelRegion" RunAt="server">Region</asp:Label></B>
						    </td>
						    <td>
						        <asp:Panel ID="pnl_ClassPanel" runat="server"><asp:DropDownList id="ddClassRegions" RunAt="server" Visible="true" width="180px" OnSelectedIndexChanged="ddClassRegions_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></asp:Panel>
						    
						        <asp:Panel ID="pnl_TermsPanel" runat="server"><asp:DropDownList id="ddTermsRegions" RunAt="server" Visible="true" width="180px" OnSelectedIndexChanged="ddTermsRegions_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></asp:Panel>
						    </td>
						</asp:panel>
						
						<%--Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009--%>
                          </td>
                    </tr>
                    <asp:panel id="panelTranslationMode" runat="server">
                      <tr valign="top">
                        <td height="21"><B>
                            <asp:label id="Label4" runat="server">Translation Mode</asp:label></B></td>
                        <td>
                          <asp:RadioButtonList id="rdTranslationMode" runat="server" cellspacing="2" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" cellpadding="2" RepeatColumns="2">
                            <asp:ListItem Value="Standard">Standard</asp:ListItem>
                            <asp:ListItem Value="PL">Product Line</asp:ListItem>
                          </asp:RadioButtonList>&nbsp;
                          <asp:Button id="btnChangePL" runat="server" Text="Change >>" onclick="btnChangePL_Click"></asp:Button></td>
                          
                      </tr>
                    </asp:panel></table>
                </FIELDSET>
                <asp:panel id="panelPLSettings" runat="server">
                  <FIELDSET>
                    <LEGEND>
                      <asp:label id="Label5" runat="server">PL settings</asp:label>
                    </LEGEND>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                      <tr valign="top">
                        <td width="150" height="21"><B>
                            <asp:label id="Label10" runat="server">PL</asp:label>: &nbsp;
                            <asp:DropDownList id="ddPL" runat="server" DataTextField="Code" DataValueField="Code" AutoPostBack="True" onselectedindexchanged="ddPL_SelectedIndexChanged"></asp:DropDownList></B></td>
                        <td>
                          <asp:CheckBoxList id="cbPLLanguages" runat="server" RepeatColumns="4" DataTextField="Name" DataValueField="Code">
                            <asp:ListItem Value="pt">pt</asp:ListItem>
                            <asp:ListItem Value="fr">fr</asp:ListItem>
                          </asp:CheckBoxList></td>
                      </tr>
                    </table>
                  </FIELDSET>
                </asp:panel>
                <FIELDSET>
                  <LEGEND>
                    <asp:label id="Label1" runat="server">MTR Days</asp:label>
                  </LEGEND>
                  <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr valign="top">
                      <td>
                        <asp:CheckBoxList id="cbMTRDays" runat="server" RepeatDirection="Horizontal" RepeatColumns="16">
                          <asp:ListItem Value="1">1</asp:ListItem>
                          <asp:ListItem Value="2">2</asp:ListItem>
                        </asp:CheckBoxList></td>
                    </tr>
                  </table>
                </FIELDSET>
                <asp:panel id="panelTermLanguages" runat="server">
                  <FIELDSET>
                    <LEGEND>
                      <asp:label id="Label9" runat="server">Term Languages</asp:label>
                    </LEGEND>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                      <tr valign="top">
                        <td>
                          <asp:CheckBoxList id="cbTermLanguages" runat="server" RepeatColumns="4" DataTextField="Name" DataValueField="Code">
                            <asp:ListItem Value="pt">pt</asp:ListItem>
                            <asp:ListItem Value="fr">fr</asp:ListItem>
                          </asp:CheckBoxList></td>
                      </tr>
                    </table>
                  </FIELDSET>
                </asp:panel>
                <asp:panel id="panelCTRSettings" runat="server">
                  <FIELDSET>
                    <LEGEND>
                      <asp:label id="Label6" runat="server">CTR Days</asp:label>
                    </LEGEND>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                      <tr valign="top">
                        <td>
                          <asp:CheckBoxList id="cbCTRDays" runat="server" RepeatDirection="Horizontal" RepeatColumns="16">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="2">2</asp:ListItem>
                          </asp:CheckBoxList></td>
                      </tr>
                    </table>
                  </FIELDSET>
                  <FIELDSET>
                    <LEGEND>
                      <asp:label id="Label12" runat="server">CTR Languages</asp:label>
                    </LEGEND>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                      <tr valign="top">
                        <td>
                          <asp:CheckBoxList id="cbCTRLanguages" runat="server" RepeatColumns="4" DataTextField="Name" DataValueField="Code">
                            <asp:ListItem Value="pt">pt</asp:ListItem>
                            <asp:ListItem Value="fr">fr</asp:ListItem>
                          </asp:CheckBoxList></td>
                      </tr>
                    </table>
                  </FIELDSET>
                </asp:panel></div>
            </td> <!-- End right pane --></tr>
        </table> <!-- End option table --></td>
    </tr>
  </table>
</asp:Content>
