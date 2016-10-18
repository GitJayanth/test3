<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.TotallyEmpty.master" Inherits="Translate_main" CodeFile="Translate_main.aspx.cs" %>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<FRAMESET name="framemain" cols="300,67%" border="1" frameborder="1" FRAMESPACING="1" TOPMARGIN="0"
	LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0" BORDERCOLOR="black">
	<FRAMESET name="framemaintv" rows="25,*" border="0" frameborder="0" FRAMESPACING="0" TOPMARGIN="0"
		LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0">
		<FRAME name="frametoolbar" src="Translate_toolbar.aspx"
			SCROLLING="no" TOPMARGIN="0" LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0" FRAMEBORDER="0" BORDER="0" />
		<FRAME name="frametv" src="Translate_tv.aspx?i=<%#itemId%>" TOPMARGIN="0" LEFTMARGIN="0" MARGINHEIGHT="0"
      MARGINWIdTH="0" FRAMEBORDER="1" BORDER="0" scrolling="auto" BORDERCOLOR="black" />
	</FRAMESET>
	<FRAMESET name="framemaintv" rows="25,*" border="0" frameborder="0" FRAMESPACING="0" TOPMARGIN="0"
		LEFTMARGIN="0" MARGINHEIGHT="0" MARGINWIdTH="0" BORDERCOLOR="black" />
	<FRAME name="framecontenttitle" src="Translate_contenttitle.aspx" TOPMARGIN="0" LEFTMARGIN="0"
		MARGINHEIGHT="0" MARGINWIdTH="0" FRAMEBORDER="yes" BORDER="0" scrolling="no" />
	<FRAME name="framecontent" src="Translate_content.aspx?i=<%#itemId%>" SCROLLING="no" FRAMEBORDER="no"
      BORDER="0" BORDERCOLOR="black">
</FRAMESET>
</FRAMESET>
</asp:Content>