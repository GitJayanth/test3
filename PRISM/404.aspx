<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Login._404" CodeFile="404.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">The page you requested cannot be found</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="2" border="0">
    <tr valign="top" style="height:1px">
      <td class="sectionTitle">
        <asp:label id="lbTitle" runat="server">We're very sorry!</asp:label></td>
    </tr>
    <tr valign="top">
      <td><br/>
        <B>The page you requested cannot be found</B><br/>
        <UL>
          <LI>
          If you typed the URL yourself, please make sure that the spelling is correct.
          <LI>
          If you clicked on a link to get here, there may be a problem with the link.
          <LI>
            Try using your browser's "Back" button or the "Return to previous page" link 
            below to choose a different link on that page, or use search to find what you 
            are looking for.
          </LI>
        </UL>
        We apologize for the inconvenience!
        <br/>
        <A onclick="history.go(-1)" href="#">Return to previous page</A>
      </td>
    </tr>
  </table>
</asp:Content>
