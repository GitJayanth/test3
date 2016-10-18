<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.chunk_master" CodeFile="chunk_master.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk master</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		document.body.oncontextmenu = function(){return false;};
</script>
			<table id="Table1" style="WIDTH: 100%;HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td>
						<fieldset>
							<legend><asp:label id="Label6" runat="server">Value is</asp:label><font color="gray">
									<asp:label id="lbStatus" runat="server">Label</asp:label></font>&nbsp;<asp:image id="imgStatus" runat="server" ImageAlign="Middle" AlternateText="status"></asp:image>&nbsp;</legend>
							<asp:textbox id="txtValue" runat="server" Rows="4" TextMode="MultiLine" Columns="72"></asp:textbox>
							<asp:image id="imgPreview" runat="server" ImageUrl="/hc_v4/img/ed_notfound.gif"></asp:image>
						</fieldset>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<hc:chunkcomment id="ChunkComment1" runat="server"></hc:chunkcomment>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
						<center>&nbsp;</center>
					</td>
				</tr>
				<tr valign="bottom" style="height: auto">
					<td align="right"></td>
				</tr>
			</table>
</asp:Content>