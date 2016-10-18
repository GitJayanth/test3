<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Translate_tvaspx" CodeFile="Translate_tv.aspx.cs" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Treeview</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript">
			function webTree_AfterNodeSelectionChange(treeId, nodeId)
			{
				var item = igtree_getNodeById(nodeId);
				top.window.status = item.getTargetFrame();
				var tag = item.getTag();	
				
				if (parent) parent.frames[3].location = "Translate_content.aspx?i="+tag;
				if (parent) parent.frames[2].location = "Translate_contentTitle.aspx";
				return false;
			}
			
			function loadGrid(itemId)
			{
				if (parent) parent.frames[3].location = "Translate_content.aspx?i="+itemId;
				if (parent) parent.frames[2].location = "Translate_contentTitle.aspx";
				return false;
			}
			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table border="0" cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td>
						<ignav:UltraWebTree id="webTree" runat="server" ExpandImage="ig_treePlus2.gif" CollapseImage="ig_treeMinus2.gif"
							Indentation="12" ImageDirectory="/ig_common/WebNavigator31/" WebTreeTarget="ClassicTree" RenderAnchors="True"
							HoverClass="HoverClass" HiliteClass="HiliteClass" DefaultItemClass="DefaultItemClass" CssClass="ptb3">
							<SELECTEDNODESTYLE BackColor="#E5FFE5" ForeColor="Black" BorderStyle="Solid" BorderColor="Black" BorderWidth="1px"
								Cursor="Default">
								<PADDING Right="0px" Top="0px" Left="1px" Bottom="0px"></PADDING>
							</SELECTEDNODESTYLE>
							<NODESTYLE BorderStyle="Solid" BorderColor="#F0F0F0" BorderWidth="0px" Cursor="Default">
								<PADDING Right="0px" Top="0px" Left="1px" Bottom="0px"></PADDING>
							</NODESTYLE>
							<HOVERNODESTYLE BackColor="Silver" ForeColor="Black" BorderStyle="Solid" BorderColor="Gray" BorderWidth="1px"
								Cursor="Default">
								<PADDING Right="0px" Top="0px" Left="1px" Bottom="0px"></PADDING>
							</HOVERNODESTYLE>
							<LEVELS>
								<IGNAV:LEVEL Index="0"></IGNAV:LEVEL>
								<IGNAV:LEVEL Index="1"></IGNAV:LEVEL>
							</LEVELS>
							<STYLES>
								<IGNAV:STYLE CssClass="DefaultItemClass" BorderStyle="Solid" BorderColor="#F0F0F0" BorderWidth="0px"
									Cursor="Default">
									<PADDING Right="0px" Top="0px" Left="1px" Bottom="0px"></PADDING>
								</IGNAV:STYLE>
								<IGNAV:STYLE CssClass="HiliteClass" BackColor="#E5FFE5" ForeColor="Black" BorderStyle="Solid"
									BorderColor="Black" BorderWidth="1px" Cursor="Default">
									<PADDING Right="0px" Top="0px" Left="1px" Bottom="0px"></PADDING>
								</IGNAV:STYLE>
								<IGNAV:STYLE CssClass="HoverClass" BackColor="Silver" ForeColor="Black" BorderStyle="Solid" BorderColor="Gray"
									BorderWidth="1px" Cursor="Default">
									<PADDING Right="0px" Top="0px" Left="1px" Bottom="0px"></PADDING>
								</IGNAV:STYLE>
							</STYLES>
							<CLIENTSIDEEVENTS AfterNodeSelectionChange="webTree_AfterNodeSelectionChange"></CLIENTSIDEEVENTS>
						</ignav:UltraWebTree>
					</td>
				</tr>
			</table>
			<input type="hidden" id="SelectedItem" name="SelectedItem" runat="server" value="-1">
</asp:Content>