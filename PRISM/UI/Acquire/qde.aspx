<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDEFrame" CodeFile="qde.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server"><div id="QDEtitle">Enter Content</div></asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" Runat="Server">
  <IFRAME id="qdeFrame" 
  style="width:100%;height:99%;margin:0px;padding:0px;" frameborder="0" scrolling="auto" name="qdeFrame" src="qde/qde_main.aspx">[Your user agent does not support iframes or is currently configured not to display iframes. If you're using Opera 6+, you can enable iframe with File/Preferences...Alt+P/Page style/Enable inline frames.]</IFRAME>
</asp:Content>
