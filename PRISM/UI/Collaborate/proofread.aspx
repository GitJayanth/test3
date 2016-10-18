<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Collaborate.Proofread" CodeFile="Proofread.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Proofread Master Content</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<IFRAME id="pFrame" style="BORDER-RIGHT: 0px; BORDER-TOP: 0px; MARGIN: 0px; BORDER-LEFT: 0px; WIDTH: 100%; BORDER-BOTTOM: 0px; HEIGHT: 100%"
		border="0" name="pFrame" src="Proofread/ProofRead_main.aspx?i=<%#itemId%>">[Your user 
		agent does not support iframes or is currently configured not to display 
		iframes. If you're using Opera 6+, you can enable iframe with 
		File/Preferences...Alt+P/Page style/Enable inline frames.]</IFRAME>
</asp:Content>
