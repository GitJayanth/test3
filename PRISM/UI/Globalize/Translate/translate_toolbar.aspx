<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Translate_toolbar" CodeFile="Translate_toolbar.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Toolbar</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script>
			function UpdateFrames()
			{
				parent.frames[1].document.forms[0].submit();        
			}
			</script>
	</head>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="selectlanguage" width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
				<tr valign="middle">
					<td align="right">
						<asp:DropDownList id="DDL_Cultures" runat="server" Width="99%" DataTextField="Name" DataValueField="Code"
							AutoPostBack="True" onselectedindexchanged="DDL_Cultures_SelectedIndexChanged"></asp:DropDownList>
					</td>
				</tr>
			</table>
</asp:Content>