<%@ Control Language="c#" Inherits="HyperCatalog.UI.DAM.UserControls.CustomPager" CodeFile="custompager.ascx.cs" %>
<table border="0" cellspacing="0" cellpadding="0" width="100%" bgColor="#cccccc" align="center">
	<TR>
		<asp:Repeater id="customPager" runat="server" DataSource='<%# GroupPages%>'>
			<HeaderTemplate>
				<td width="25px">
					<asp:ImageButton id="prevBtn" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# "/hc_v4/img/ed_prev.GIF"%>' Visible='<%# (PageGroupIndex-1) >= 0%>' CommandName="GotoGroup" CommandArgument='<%# PageGroupIndex-1%>' OnCommand="pagerClick"></asp:ImageButton>
				</td>
				<td align="center" Height="15px" bgColor="#cccccc">
			</HeaderTemplate>
			<ItemTemplate>
				<asp:LinkButton id="pageBtn" runat="server" Visible='<%# ((int)Container.DataItem) != CurrentPageIndex%>' Text='<%# ((int)Container.DataItem)+1%>' Style="font-weight:bold;color:white;" CommandName="GotoPage" CommandArgument='<%# Container.DataItem%>' OnCommand="pagerClick"></asp:LinkButton>
				<asp:Label id="pageLbl" runat="server" Visible='<%# ((int)Container.DataItem) == CurrentPageIndex%>' Text='<%# ((int)Container.DataItem)+1%>' Style="font-weight:bold;color:#BFF4B8;"></asp:Label>
			</ItemTemplate>
			<SeparatorTemplate> </SeparatorTemplate>
			<FooterTemplate>
				<td width="25px">
					<asp:ImageButton id="nextBtn" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# "/hc_v4/img/ed_next.GIF"%>' Visible='<%# (PageGroupIndex+1) * PageGroupCount < PageCount%>' CommandName="GotoGroup" CommandArgument='<%# PageGroupIndex+1%>' OnCommand="pagerClick"></asp:ImageButton>
				</td>
			</FooterTemplate>
		</asp:Repeater>
		<td>
			<asp:Dropdownlist id="pageList" runat="server" AutoPostBack="True" DataSource='<%# AllPages%>'></asp:Dropdownlist>
		</td>
	</tr>
</table>
