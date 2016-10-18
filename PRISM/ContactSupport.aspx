<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Login._ContactSupport" CodeFile="ContactSupport.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">The page you requested cannot be found</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="2" border="0">
    <tr valign="top" style="height:1px">
      <td class="sectionTitle">
        <asp:label id="lbTitle" runat="server">Contact Support</asp:label></td>
    </tr>
    <tr valign="top">
      <td><br/>
      <ul>
        <li><a onclick="window.close();" target="_blank" href="<%=HyperCatalog.Shared.SessionState.CacheParams["ApplicationSupportLink"].Value%>"><B><%=HyperCatalog.Shared.SessionState.CacheParams["ApplicationSupportLinkTitle"].Value%></B>: <img border="0" src="/hc_v4/img/external_link.gif" align="texttop"></a>
                    <br />
            <b>Note</b>: You can contact the team in case of issues with: 
            <ul>
<li>wrong application behaviour</li>
<li>response time</li>
</ul>

</li>
            <br /><br />
        <B><li><a onclick="window.close();" target="_blank" href="<%=HyperCatalog.Shared.SessionState.CacheParams["ContentQualitySupportLink"].Value%>"><%=HyperCatalog.Shared.SessionState.CacheParams["ContentQualitySupportLinkTitle"].Value%></B>: 
            <img border="0" src="/hc_v4/img/external_link.gif" align="texttop"></a>
            <br />
            <b>Note</b>: You can contact the team in case of issues with: 
            <ul>
<li>timeliness</li>
<li>accuracy/correctness</li>
<li>completeness</li>
<li>consistency</li>
<li>content model update</li></ul>
            </li>
            </ul>
            </li>
      </td>
    </tr>
  </table>
</asp:Content>
