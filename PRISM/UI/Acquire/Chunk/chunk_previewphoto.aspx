<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Reference Control="~/ui/acquire/chunk/containerinfo.ascx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_PreviewPhoto" CodeFile="Chunk_PreviewPhoto.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk preview photo</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <table cellspacing="0" cellpadding="1" width="100%" border="0">
        <tr valign="top">
          <td><asp:image id="imgPreview" runat="server" ImageUrl="/hc_v4/img/ed_notfound.gif"></asp:image></td>
          <asp:panel id="panelInfo" Runat="server">
    <td width="100%">
      <table class=datatable cellspacing=0 cellpadding=2 width="100%" 
        border=0>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id="Label13" runat="server">Library</asp:Label></td>
          <td class=ptb3>
<asp:Label id="lbLibrary" runat="server">info</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label3 runat="server">Name</asp:Label></td>
          <td 
          class=ptb3>
<asp:Label id=lbName runat="server">info</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label12 runat="server">Created on</asp:Label></td>
          <td class=ptb3>
<asp:Label id=lbCreatedOn runat="server">info</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label1 runat="server">Height</asp:Label></td>
          <td class=ptb3>
<asp:Label id=lbHeight runat="server">info</asp:Label>
<asp:Label id=Label7 runat="server">px</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label2 runat="server">Width</asp:Label></td>
          <td class=ptb3>
<asp:Label id=lbWidth runat="server">info</asp:Label>
<asp:Label id=Label6 runat="server">px</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label4 runat="server">Size</asp:Label></td>
          <td class=ptb3>
<asp:Label id=lbSize runat="server">info</asp:Label>
<asp:Label id=Label11 runat="server">Kb</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label5 runat="server">Vert. Resolution</asp:Label></td>
          <td class=ptb3>
<asp:Label id=lbResolutionV runat="server">info</asp:Label>
<asp:Label id=Label10 runat="server">dpi</asp:Label></td></tr>
        <TR>
          <td class=ptb1 width=100>
<asp:Label id=Label8 runat="server">Hor. Resolution</asp:Label></td>
          <td class=ptb3>
<asp:Label id=lbResolutionH runat="server">info</asp:Label>
<asp:Label id=Label9 runat="server">dpi</asp:Label></td></tr></table>
</td>
          </asp:panel></tr>
      </table>
</asp:Content>