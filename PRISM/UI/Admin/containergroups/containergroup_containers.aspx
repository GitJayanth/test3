<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="containergroup_containers" CodeFile="containergroup_containers.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Containers</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		  } 
			</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
                <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
					<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
									<DefaultStyle Width="25px"></DefaultStyle>
								</igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top">
					<td><igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container" ActivationObject-BorderDetails-StyleBottom="Dotted" ActivationObject-BorderDetails-StyleRight="Solid">
                                <%-- Fix for QC# 7384 by Rekha Thomas. Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
								<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<%--<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>--%>
								<FrameStyle Width="100%" CssClass="dataTable">
                                 <%-- Fix for QC# 7386 by Rekha Thomas. Added borderdetails tag to fix the gridlines missing issue --%>
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
                                 <%-- Fix for QC# 7386 by Radha S. Added borderdetails tag to fix the gridlines missing issue --%>
                                <RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" StyleRight="Solid" WidthRight="1px" WidthBottom ="1px" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="Containers" BorderCollapse="Collapse">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="Tag" Key="Tag" Width="130px" BaseColumnName="Tag"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="250px" BaseColumnName="Name"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Entry" Key="DataType" Width="60px" BaseColumnName="DataTypeName"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Type" Key="ContainerType" Width="100px" BaseColumnName="ContainerTypeName"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Definition" Key="Definition" Width="100%" BaseColumnName="Definition" CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
						<CENTER>
							<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
						</CENTER>
					</td>
				</tr>
			</table>
</asp:Content>