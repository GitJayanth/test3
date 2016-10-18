<%@ Control Language="c#" Inherits="HyperCatalog.UI.Login.Design.TopMenu" CodeFile="TopMenu.ascx.cs"%>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<ignav:ultrawebmenu Width="450px" XPSpacerImage="/hc_v4/img/menuspacer.gif" WebMenuStyle="XPClient" CssClass="mg"
  Cursor="hand" Height="25px" id="wm" runat="server"
  SubMenuImage="/hc_v4/img/ed_right.gif" EnhancedRendering="false">
  <ItemStyle CssClass="mgi"></ItemStyle>
  <HoverItemStyle CssClass="mgho"></HoverItemStyle>
  <SeparatorStyle CssClass="mgs"></SeparatorStyle>
  <Levels>
    <ignav:Level Index="0"></ignav:Level>
  </Levels>
</ignav:ultrawebmenu>



