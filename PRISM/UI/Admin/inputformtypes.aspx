<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.InputFormTypes" CodeFile="InputFormTypes.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input form types</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script>
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
    }
	</script>
  <script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

	<table class="main" cellspacing="0" cellpadding="0">
		<tr height="1">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">Input form types list</asp:label></td>
		</tr>
        <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Save" Text="Apply new sort" Image="/hc_v4/img/ed_save.gif">
								<DefaultStyle Width="120px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox cssClass="Search" Width="150px" Id="txtFilter" MaxLength="50"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
						</Items>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<tr valign="top" style="height:1px">
				<td>
					<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label></td>
			</tr>
			<tr valign="top">
				<td>
					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/" UseAccessibleHeader="False">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
							<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="ContainerTypes" Key="InputFormTypes" BorderCollapse="Collapse" DataKeyField="InputFormTypeCode">
								<Columns>
									<igtbl:UltraGridColumn Key="Code" Width="30px" Hidden="True" BaseColumnName="Code"></igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Id" Width="40px" HeaderText="Code" BaseColumnName="Code" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Id"></Footer>
										<Header Key="Id" Caption="Code"></Header>
									</igtbl:TemplatedColumn>
									<igtbl:TemplatedColumn Key="Name" Width="100%" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name"></Footer>
										<Header Key="Name" Caption="Name"></Header>
									</igtbl:TemplatedColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label></CENTER>
				</td>
			</tr>
		</asp:panel>
		<TR>
			<td>
				<igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderStyle="none" BorderColor="#808080"
					BorderWidth="1px" DummyTargetUrl="/hc_v4/pleasewait.htm" visible="false" LoadAllTargetUrls="False" Height="100%">
					<DefaultTabStyle Height="25px" BackColor="WhiteSmoke"></DefaultTabStyle>
					<RoundedImage SelectedImage="ig_tab_lightb2.gif" NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
					<Tabs>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="./ContainerTypes/inputform_properties.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab Key="ExclusionRules" Text="Exclusion rules">
							<ContentPane TargetUrl="./ContainerTypes/inputform_exclusionrules.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:ultrawebtab></td>
		</tr>
	</table>
	<input id="txtSortColPos" type="hidden" value="3" name="txtSortColPos" runat="server">
</asp:Content>
