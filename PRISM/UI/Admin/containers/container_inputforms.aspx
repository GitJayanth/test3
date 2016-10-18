<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="container_inputforms" CodeFile="container_inputforms.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">InputForms</asp:Content>
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
<%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<asp:Panel ID="pnlTitle" Visible="false" Runat="server">
					<TBODY>
						<tr valign="top" style="height:1px">
							<td>
								<igtbar:ultrawebtoolbar id="uwtoolbarTitle" runat="server" CssClass="hc_toolbartitle" ItemWidthDefault="80px"
									width="100%">
									<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
									<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
									<Items>
										<igtbar:TBLabel Text="Action" Key="Action">
											<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
										</igtbar:TBLabel>
										<igtbar:TBLabel Text="">
											<DefaultStyle Width="5px"></DefaultStyle>
										</igtbar:TBLabel>
									</Items>
									<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
								</igtbar:ultrawebtoolbar></td>
						</tr>
				</asp:Panel>
                <%--Removed width propery in UltraWebToolbar to fix the horizontal width issue By Radha S--%>
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="ListSep"></igtbar:TBSeparator>
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
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<igtbl:UltraWebGrid id="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/" Height="100px"
							UseAccessibleHeader="False" DisplayLayout-ActivationObject-AllowActivation="true">
							<DisplayLayout AllowDeleteDefault="Yes" MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
								 SelectTypeRowDefault="Extended" AllowColumnMovingDefault="OnServer" HeaderClickActionDefault="SortMulti"
								BorderCollapseDefault="Separate" AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="dg"
								TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container"
								AllowUpdateDefault="Yes" RowHeightDefault="100%" Version="4.00"> <%--RowHeightDefault="10px"--%>
								<HeaderStyleDefault  VerticalAlign="Middle" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" Font-Bold="true" Height="20px" > <%--TextOverflow="Ellipsis" CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<%--<GroupByRowStyleDefault BorderColor="Window" BackColor="Control"></GroupByRowStyleDefault>--%>
								<%--<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"/>
                                </FrameStyle>--%>
								<RowAlternateStyleDefault   BorderWidth="1px" BorderStyle="Solid" BorderColor="Gray" BackColor="White"></RowAlternateStyleDefault> <%--CssClass="uga"--%>
								<RowStyleDefault   VerticalAlign="Top" BorderWidth="1px" BorderStyle="Solid"  BorderColor="Gray" BackColor="White"></RowStyleDefault><%--CssClass="ugd" TextOverflow="Ellipsis"--%>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="InputForms" BorderCollapse="Collapse">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="InputFormId" Key="InputFormId" Hidden="True" BaseColumnName="InputFormId"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="200px" BaseColumnName="Name"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Type" Key="InputType" Width="100px" BaseColumnName="InputType"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Mandatory" Key="Mandatory" Width="80px" Type="CheckBox" DataType="System.Boolean"
											BaseColumnName="IsMandatory" AllowUpdate="No">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:UltraWebGrid>
						<center>
							<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
						</center>
					</td>
				</tr>
				</TBODY>
			</table>
</asp:Content>