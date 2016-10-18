<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Translate_contenttitle" CodeFile="Translate_contenttitle.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Translate content title</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript">
    document.body.oncontextmenu = function(){return false;};
  </script>
			<table cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="selectlanguage">
						&nbsp;<img src="/hc_v4/img/btn_left_arrow.gif" onclick="HideShowTV(parent.framemain, this)"
							border="0" align="top" style="MARGIN-LEFT:6px;MARGIN-RIGHT:6px" title="Hide Treeview">
						&nbsp;Translate chunks
					</td>
				</tr>
			</table>
</asp:Content>