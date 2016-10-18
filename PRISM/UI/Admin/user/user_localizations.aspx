<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="user_localizations" CodeFile="user_localizations.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
	      function uwToolbar_Click(oToolbar, oButton, oEvent){
		      if (oButton.Key == 'List') {
            back();
            oEvent.cancelPostBack = true;
          }
		    } 
			</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr height="1" valign="top">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar></td>
				</tr>
        <tr valign="top" style="height:1px">
					<td>
						<br/>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" SelectTypeRowDefault="Single"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" 
								NoDataMessage="No input forms attached to this container">
									 <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
								<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand BaseTableName="Cultures" Key="Cultures" BorderCollapse="Collapse" DataKeyField="CultureCode">
									<Columns>
									  <igtbl:UltraGridColumn Key="InScope" Hidden="True" BaseColumnName="InScope"></igtbl:UltraGridColumn>
									  <igtbl:templatedcolumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
											<cellstyle VerticalAlign="Top" WRAP="True" HorizontalAlign="Center"></cellstyle>
											<headertemplate>
												<asp:checkbox id="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:checkbox>
											</headertemplate>
											<celltemplate>
												<asp:checkbox id="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:checkbox>
											</celltemplate>
											<footer KEY="Select" caption=""></footer>
											<header KEY="Select"></header>
										</igtbl:templatedcolumn>
										<igtbl:UltraGridColumn HeaderText="Code" Key="CountryCode" width="40px" BaseColumnName="CountryCode">
											<CellStyle HorizontalAlign="Center" VerticalAlign="Middle"></CellStyle>
											<Header Caption="Code"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Code" Key="CultureCode" BaseColumnName="CultureCode" Hidden="true">
											<CellStyle HorizontalAlign="Center"></CellStyle>
											<Header Caption="Code"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="CultureName" Width="400px" BaseColumnName="CultureName">
											<CellStyle Wrap="True"></CellStyle>
											<Header Caption="Name"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="CultureTypeId" BaseColumnName="CultureTypeId" Hidden="true">
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
					</td>
				</tr>
			</table>
</asp:Content>