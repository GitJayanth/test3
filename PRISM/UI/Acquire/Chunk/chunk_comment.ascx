<%@ Control Language="c#" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Comment" CodeFile="Chunk_Comment.ascx.cs" %>
<fieldset>
  <legend>
    <asp:label id="Label10" runat="server">Comment</asp:label>&nbsp;
    <asp:RegularExpressionValidator Enabled="false" ID="regValidateHTMLComment" Display="Dynamic" ControlToValidate="txtComment" runat="server" ErrorMessage="HTML tags are not allowed!"></asp:RegularExpressionValidator>
    </legend>
  <asp:textbox id="txtComment" runat="server" Columns="80" Rows="2" TextMode="MultiLine"></asp:textbox>
</fieldset>
