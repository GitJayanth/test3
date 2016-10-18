<%@ Control Language="c#" Inherits="HyperCatalog.UI.Acquire.QDE.controlItemSort" CodeFile="controlItemSort.ascx.cs" %>
<table class="datatable" id="Table1" cellspacing="0" cellpadding="4" width="100%" border="0">
	<TR>
		<td width="20">
			<SELECT id="lstBoxSiblingItems" size="2" name="lstBoxSiblingItems" runat="server">
				<OPTION></OPTION>
			</SELECT>
		</td>
		<td valign="middle" align="left"><asp:button id="BtnUp" runat="server" Text=" Up " CausesValidation="False" onclick="BtnUp_Click"></asp:button><br/>
			<asp:button id="BtnDown" runat="server" Text="Down" CausesValidation="False" onclick="BtnDown_Click"></asp:button>
		</td>
	</tr>
</table>
