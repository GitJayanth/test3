<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globilize.Translate" CodeFile="Translate.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Translate Content</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<IFRAME id="translateFrame" style="BORDER-RIGHT: 0px; BORDER-TOP: 0px; MARGIN: 0px; BORDER-LEFT: 0px; WIDTH: 100%; BORDER-BOTTOM: 0px; HEIGHT: 100%"
		border="0" name="missingFrame" src="Translate/Translate_main.aspx?i=<%#itemId%>">[Your user agent 
    does not support iframes or is currently configured not to display iframes. If 
    you're using Opera 6+, you can enable iframe with File/Preferences...Alt+P/Page 
    style/Enable inline frames.]</IFRAME>
</asp:Content>
