<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globalize.TM" CodeFile="TM.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Translation memory</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent){
			if (oButton.Key == 'filter')
			{
	      DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }		       
		}		  
		function WholeWord(cb){
		  document.getElementById("wholeword").value = cb.checked?"1":"0";
		}
	</script>
	<input type="hidden" id="wholeword" name="wholeword" value=""/>
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle" height="17">
				<asp:label id="lbTitle" runat="server">List of TM expressions</asp:label></td>
		</tr>
		<asp:panel id="panelgrid" Runat="server">
			<tr valign="top">
				<td>
  			  <!-- WARNING BUTTON ADD NOT USE FOR MOMENT -->
					<igtbar:UltraWebToolbar id="uwToolbar" runat="server" ItemWidthDefault="150px" CssClass="hc_toolbar"
						ImageDirectory=" ">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" visible="false"></igtbar:TBarButton>
						
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Search">
								<DefaultStyle Width="50px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="250px" Key="SearchField">
								<asp:TextBox Width="250px" ID="txtFilter" MaxLength="50" runat=server></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBCustom Width="150px" Key="match">
                  <asp:CheckBox ID="cbMatch" runat="server" AutoPostBack="false" Text="Match whole word"/>
								</igtbar:TBCustom>

						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:UltraWebToolbar></td>
			</tr>
			<tr valign="top">
				<td class="main">
					<CENTER>
						<asp:Label id="lbMessage" runat="server" visible="false">To find a TM Expression, please use the Search function</asp:Label><br/>
						<asp:Label id="lbNoresults" runat="server" visible="false" ForeColor="Red" Font-Bold="True">No results</asp:Label></CENTER>
					<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
							SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" EnableInternalRowsManagement="True"
							RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
							<Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True"></Pager>
						 <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand>
								<Columns>
									<igtbl:UltraGridColumn HeaderText="" Key="TMExpressionId" Hidden="True" BaseColumnName="Id">
										<Footer Key="TermId"></Footer>
										<Header Key="TermId" Caption=""></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="TMExpressionValue" Width="100%" BaseColumnName="Value" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Owner" Key="UserName" Width="150px" BaseColumnName="UserName">
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Modify date" Key="ModifyDate" Width="100px" BaseColumnName="ModifyDate">
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:ultrawebgrid></td>
			</tr>
		</asp:panel>
		<tr valign="top">
			<td>
				<igtab:UltraWebTab id="webTab" runat="server" Width="100%" Visible="False" BorderColor="#808080" BorderStyle="Solid"
					BorderWidth="1px" LoadAllTargetUrls="False" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%">
					<DefaultTabStyle Height="25px" BackColor="WhiteSmoke"></DefaultTabStyle>
					<RoundedImage SelectedImage="ig_tab_lightb2.gif" NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
					<Tabs>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
							<ContentPane TargetUrl="./TM/Expression_Properties.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
						<igtab:Tab DefaultImage="/hc_v4/img/ed_translate.gif" Text="Translations" Key="Translations">
							<ContentPane TargetUrl="./TM/Expression_Translations.aspx" Visible="False"></ContentPane>
						</igtab:Tab>
					</Tabs>
				</igtab:UltraWebTab></td>
		</tr>
	</table>
	<input type="hidden" name="action"> <input type="hidden" name="letter">
</asp:Content>
