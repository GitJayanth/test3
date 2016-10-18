<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.TotallyEmpty.master" Inherits="qde_main" CodeFile="QDE_Main.aspx.cs" %>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" Runat="Server">
<FRAMESET name="framemain" cols="300,*" border="0" frameborder="yes" FRAMESPACING="1" TOPMARGIN="0"
  BORDERCOLOR="black" LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0">
  <FRAMESET name="frameleft" rows="25,21,*" border="0" frameborder="no" FRAMESPACING="0" TOPMARGIN="0"
    LEFTMARGIN="0" MARGINHEIGHT="0" BOTTOMMARGIN="0" MARGINWIdTH="0">
    <FRAME RunAt="server" id="frametoolbar" name="frametoolbar" src="QDE_ToolBar.aspx" SCROLLING="no"
      TOPMARGIN="0" LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0" FRAMEBORDER="0" BORDER="0" />
    <FRAME RunAt="server" id="frameitemscope" name="frameitemscope" src="QDE_ItemScope.aspx"
      SCROLLING="no" TOPMARGIN="0" LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0" FRAMEBORDER="0"
      BORDER="0" />
    <FRAME RunAt="server" id="frametv" name="frametv" src="QDE_TV.aspx?all=0" TOPMARGIN="0"
      LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0" FRAMEBORDER="0" BORDER="0" DESIGNTIMEDRAGDROP="9" />
  </FRAMESET>
  <FRAME RunAt="server" id="framecontent" name="framecontent" src="QDE_FormRoll.aspx" SCROLLING="no"
    FRAMEBORDER="no" BORDER="0" />
</FRAMESET>
</asp:Content>