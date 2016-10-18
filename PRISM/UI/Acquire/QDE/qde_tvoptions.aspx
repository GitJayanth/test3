<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="QDE_TVOptions" CodeFile="QDE_TVOptions.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">QDE_TVOptions</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">
		<script>
			function UpdateGrid()
			{
				if (parent)
				{
					parent.framecontent.location.reload();
				}
			}
		</script>
</asp:Content>
<%--Removed width tag from ultrawebtoolbar to fix the large button issue by Radha S--%>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
	<table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%">
		<tr valign="top" style="height:1px">
			<td>
				<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="60px">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
					<Items>
						<igtbar:TBarButton ToolTip="Apply changes" Image="/hc_v4/img/ed_save.gif" Key="Save" Text="Save"></igtbar:TBarButton>
					</Items>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="top" style="height:1px">
			<td>
				<asp:Label ID="lbError" Runat="server" CssClass="hc_error" Visible="False"></asp:Label>
			</td>
		</tr>
		<tr valign="top" style="height:auto">
			<td>
				<div style="OVERFLOW-Y: auto; WIDTH: 100%; HEIGHT: 100%">
					<table width="100%" cellpadding="2" cellspacing="0" border="0">
						<tr>
							<td class="ptb1" colspan=2>Input Forms</td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbComment" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbComment" runat="server"></asp:checkbox></td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbCulture" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbCulture" runat="server"></asp:checkbox></td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbToolbarStat" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbToolbarStat" runat="server"></asp:checkbox></td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbInheritanceMode" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbInheritanceMode" runat="server"></asp:checkbox></td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbShowLinkCount" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbShowLinkCount" runat="server"></asp:checkbox></td>
						</tr>
						<tr>
							<td class="ptb1" colspan=2>Tree view</td>
						</tr>
						<tr>
							<td class="hc_title">Show obsolete products</td>
							<td><asp:checkbox id="cbViewObsoletes" runat="server"></asp:checkbox></td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbShowTranslatableName" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbShowTranslatableName" runat="server"></asp:checkbox></td>
						</tr>
						<!--
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbShowShrinkedName" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbShowShrinkedName" runat="server"></asp:checkbox></td>
						</tr>
						<tr>-->
							<td class="ptb1" colspan=2>Chunk Window</td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbChunkMoveNext" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbChunkMoveNext" runat="server"></asp:checkbox></td>
						</tr>
						<tr valign="top">
							<td class="hc_title"><asp:Label ID="lbChunkShowSimplified" Runat="server"></asp:Label></td>
							<td><asp:checkbox id="cbChunkShowSimplified" runat="server"></asp:checkbox></td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
	</table>
</asp:Content>