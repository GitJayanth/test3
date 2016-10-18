<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_History" CodeFile="Chunk_History.aspx.cs" %>


<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Chunk_History</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <style type="text/css">
    fieldset1 { BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid }
    legend1 { BORDER-RIGHT: green 1px solid; PADDING-RIGHT: 0.5em; BORDER-TOP: green 1px solid; PADDING-LEFT: 0.5em; FONT-SIZE: 90%; PADDING-BOTTOM: 0.2em; BORDER-LEFT: green 1px solid; COLOR: black; PADDING-TOP: 0.2em; BORDER-BOTTOM: green 1px solid; TEXT-ALIGN: right }
    </style>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top">
      <td height="100%">
        <div style="overflow: auto; width: 100%; height: 100%">
          <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
              <td>
                <asp:Repeater ID="repHistory" runat="server">
                  <ItemTemplate>
                    <fieldset style="margin-bottom: 6px; margin-top: 2px;">
                      <legend>
                        <asp:Label ID="Label6" runat="server"><%# DataBinder.Eval(Container.DataItem, "User.FirstName") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "User.LastName") %> - (<%# HyperCatalog.Shared.SessionState.User.FormatUtcDate((DateTime)DataBinder.Eval(Container.DataItem, "ModifyDate"), true, HyperCatalog.Shared.SessionState.User.FormatDate + ' ' + HyperCatalog.Shared.SessionState.User.FormatTime)%>) - <i>Status was [<%# DataBinder.Eval(Container.DataItem, "Status")%>]</asp:Label></i>&nbsp;
                      </legend>
                      <fieldset style="border: 1px solid gray; color: black; background: white; padding: 2px;
                        margin: 2px;" title="Comment: <%# DataBinder.Eval(Container.DataItem, "Comment") %>">
                        <table border="0">
                          <tr valign="top">
                            <td>
                              <img src='/hc_v4/img/S<%# HyperCatalog.Business.Chunk.GetStatusFromEnum((HyperCatalog.Business.ChunkStatus)DataBinder.Eval(Container.DataItem, "Status")) %>.gif'
                                align="right" /></td>
                            <td>
                              <i>
                                <%# CheckValue(DataBinder.Eval(Container.DataItem, "Text")) %>
                              </i>
                            </td>
                          </tr>
                        </table>
                      </fieldset>
                    </fieldset>
                  </ItemTemplate>
                </asp:Repeater>
            </tr>
          </table>
        </div>
      </td>
    </tr>
  </table>
</asp:Content>
