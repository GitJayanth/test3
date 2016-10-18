<%@ Control Language="c#" Inherits="HyperCatalog.UI.Acquire.Chunk.ContainerInfo" CodeFile="ContainerInfo.ascx.cs" %>
<fieldset>
	<legend>
		<asp:label id="Label1" runat="server">Container</asp:label>
	</legend>
	<table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr class="ugd" valign="top">
			<td noWrap width="150" height="*"><B><asp:label id="Label11" runat="server">Group</asp:label></B></td>
			<td><%#container.Group.Name%>&nbsp;</td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap><B><asp:label id="Label2" runat="server">Tag</asp:label></B></td>
			<td><%#container.Tag%></td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap><B><asp:label id="Label13" runat="server">Readable name</asp:label></B></td>
			<td><%#container.Name%></td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap><B><asp:label id="Label3" runat="server">Definition</asp:label></B></td>
			<td><%#container.Definition%>&nbsp;</td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap><B><asp:label id="Label10" runat="server">Label</asp:label></B></td>
			<td><%#container.Label%></td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap><B><asp:label id="Label4" runat="server">Business rule</asp:label></B></td>
			<td><%#container.EntryRule%>&nbsp;</td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap><B><asp:label id="Label5" runat="server">Data type</asp:label></B></td>
			<td><%#container.DataType.Name%>&nbsp;</td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap><B><asp:label id="Label6" runat="server">Max length</asp:label></B></td>
			<td><%#container.MaxLength%>&nbsp;</td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap><B><asp:label id="Label7" runat="server">Sample</asp:label></B></td>
			<td><%#container.Sample%>&nbsp;</td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap><B><asp:label id="Label8" runat="server">Type</asp:label></B></td>
			<td><%#container.ContainerType.Name%>&nbsp;</td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap height="*"><B><asp:label id="Label9" runat="server">Inheritance method</asp:label></B></td>
			<td><%#HyperCatalog.Business.InheritanceMethod.GetByKey(container.InheritanceMethodId).Name %>&nbsp;</td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap height="*"><B><asp:label id="Label14" runat="server">Regionalizable</asp:label></B></td>
			<td><%#container.Regionalizable%>&nbsp;</td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap height="*"><B><asp:label id="Label15" runat="server">Translatable</asp:label></B></td>
			<td><%#container.Translatable%>&nbsp;</td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap height="*"><B><asp:label id="Label12" runat="server">Localizable</asp:label></B></td>
			<td><%#container.Localizable%>&nbsp;</td>
		</tr>
		<tr class="ugd" valign="top">
			<td noWrap height="*"><B><asp:label id="Label16" runat="server">Read only</asp:label></B></td>
			<td><%#container.ReadOnly%>&nbsp;</td>
		</tr>
		<tr class="uga" valign="top">
			<td noWrap height="*"><B><asp:label id="Label17" runat="server">Publishable</asp:label></B></td>
			<td><%#container.Publishable%>&nbsp;</td>
		</tr>
	</table>
</fieldset>
