<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.DataTypes" CodeFile="DataTypes.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Data types</asp:Content>
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
	<table class="main" cellspacing="0" cellpadding="0">
		<TR>
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">Data types list</asp:label></td>
			<asp:panel id="panelGrid" runat="server" Visible="True">
            <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
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
				<tr valign="top">
					<td>
						<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
								<%--<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>--%>
                                <%-- Fix for QC# 7384 by Rekha Thomas. Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
								<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand BaseTableName="DataTypes" Key="DataTypes" BorderCollapse="Collapse" DataKeyField="DataTypeCode">
									<Columns>
										<igtbl:TemplatedColumn Key="Id" Width="40px" HeaderText="Code" BaseColumnName="DataTypeCode" CellMultiline="Yes">
											<CellTemplate>
												<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
													<%#Container.Text%>
												</asp:LinkButton>
											</CellTemplate>
											<Footer Key="Id"></Footer>
											<Header Key="Id" Caption="Code"></Header>
										</igtbl:TemplatedColumn>
										<igtbl:TemplatedColumn Key="Name" Width="80px" HeaderText="Name" BaseColumnName="DataType" CellMultiline="Yes">
											<CellTemplate>
												<asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
													<%#Container.Text%>
												</asp:LinkButton>
											</CellTemplate>
											<Footer Key="Name"></Footer>
											<Header Key="Name" Caption="Name"></Header>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="300px" BaseColumnName="Description">
											<CellStyle Wrap="True"></CellStyle>
											<Footer Key="Description"></Footer>
											<Header Key="Description" Caption="Description"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Example" Key="Example" Width="150px" BaseColumnName="Example">
											<CellStyle Wrap="True"></CellStyle>
											<Footer Key="Example"></Footer>
											<Header Key="Example" Caption="Example"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Comment" Key="Comment" Width="100%" BaseColumnName="Comment">
											<CellStyle Wrap="True"></CellStyle>
											<Footer Key="Comment"></Footer>
											<Header Key="Comment" Caption="Comment"></Header>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
						<CENTER>
							<asp:Label id="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label></CENTER>
					</td>
				</tr>
			</asp:panel>
		<TR>
			<td>
				<igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderWidth="1px" BorderColor="#808080"
					BorderStyle="none" LoadAllTargetUrls="False" visible="false" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%">
							<DefaultTabStyle Height="21px" Font-Size="8pt" Font-Names="Microsoft Sans Serif" ForeColor="Black"
								BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/inf/images/ig_tab_winXP1.gif"
								NormalImage="/hc_v4/inf/images/ig_tab_winXP3.gif" HoverImage="/hc_v4/inf/images/ig_tab_winXP2.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
					<TABS>
						<igtab:Tab Text="Properties" DefaultImage="/hc_v4/img/ed_properties.gif">
							<CONTENTPANE TargetUrl="./datatypes/datatype_Properties.aspx"></CONTENTPANE>
						</igtab:Tab>
						<igtab:Tab Text="Containers" DefaultImage="/hc_v4/img/ed_containers.gif">
							<CONTENTPANE TargetUrl="./datatypes/datatype_containers.aspx"></CONTENTPANE>
						</igtab:Tab>
					</TABS>
				</igtab:ultrawebtab></td>
		</tr>
	</table>
</asp:Content>