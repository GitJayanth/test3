<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Inputforms.inputforms_usage" CodeFile="InputForms_Usage.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Items</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
	<script>
		function dg_DblClickHandler(gridName, id, ev){
			var cell = igtbl_getCellById(id);
      var grid = igtbl_getGridById(gridName);
		}

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
<%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" >
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
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
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbError" CssClass="hc_error" Visible="false" Runat="server"></asp:label></td>
				</tr>
				<asp:Panel ID="pnlApplLevel" Runat="server">
					<tr valign="top" style="height:1px">
						<td>
							<table cellspacing="0" cellpadding="0" width="100%" align="right" border="0">
								<TR>
									<td>&nbsp;</td>
									<td class="gh" width="200">Applicable levels</td>
								</tr>
							</table>
						</td>
					</tr>
				</asp:Panel>
				<tr valign="top">
					<td>
						<igtbl:UltraWebGrid id="dg" runat="server" Width="100%" UseAccessibleHeader="False">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
								<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
                                <RowAlternateStyleDefault CssClass="uga">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
								<%--<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>--%>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="InputFormUsage" BorderCollapse="Collapse">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="Id" Key="ItemId" Width="60px" BaseColumnName="ItemId"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Item Name" Key="ItemName" Width="100%" BaseColumnName="ItemName">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Product Lines" Key="PLs" Width="100px" BaseColumnName="PLs">
										  <CellStyle Wrap="true"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="DefinedPLs" Hidden="true" BaseColumnName="DefinedPLs"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="LevelId" Key="LevelId" BaseColumnName="LevelId" ServerOnly="true"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="ItemPath" Key="ItemPath" BaseColumnName="ItemPath" ServerOnly="true"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Applicable level" Key="ApplicableLevel" BaseColumnName="ApplicableLevel"
											ServerOnly="true"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="1" Key="L1" Width="25px" BaseColumnName="L1">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="2" Key="L2" Width="25px" BaseColumnName="L2">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="3" Key="L3" Width="25px" BaseColumnName="L3">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="4" Key="L4" Width="25px" BaseColumnName="L4">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="5" Key="L5" Width="25px" BaseColumnName="L5">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="6" Key="L6" Width="25px" BaseColumnName="L6">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="7" Key="L7" Width="25px" BaseColumnName="L7">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="8" Key="L8" Width="25px" BaseColumnName="L8">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
						<CENTER>
							<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
						</CENTER>
					</td>
				</tr>
			</table>
</asp:Content>