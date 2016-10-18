<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="Links_content.aspx.cs" Inherits="Links_content" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Links content</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dg");
                  }
                </script>   

<%--Removed width property from ultrawebtoolbar to fix moving icon issue by Radha S--%>
 	<table class="main" style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td>
					  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif" ToolTip="Export content in Excel"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:label></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td align="center"><asp:label id="lbResult" runat="server" CssClass="hc_success" Visible="false">No result</asp:label></td>
				</tr>
				<tr valign="top" style="height:auto">
					<td>
						<div style="overflow:auto; WIDTH:100%; HEIGHT:100%">
							<igtbl:ultrawebgrid id="dg" runat="server" Width="100%" UseAccessibleHeader="False" OnInitializeRow="dg_InitializeRow">
                            <%--Modified the code to fix infragestic issue--%>
								<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
									RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
									NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" BackColor ="LightGray" Cursor="Hand" Font-Bold="true" HorizontalAlign="Center"> <%--CssClass="gh2">--%>
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<FrameStyle Width="100%" CssClass="dataTable">
                                        <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                    </FrameStyle>
									<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
									<ActivationObject AllowActivation="False"></ActivationObject>
									<RowAlternateStyleDefault CssClass="uga">
                                        <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                    </RowAlternateStyleDefault>
									<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                        <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                    </RowStyleDefault>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BaseTableName="Links" Key="Links" BorderCollapse="Collapse">
										<Columns>
											<igtbl:UltraGridColumn Key="LinkTypeId" BaseColumnName="LinkTypeId" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="LinkFrom" BaseColumnName="LinkFrom" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InheritedItemId" BaseColumnName="InheritedItemId" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SubItemId" BaseColumnName="SubItemId" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InheritedSubItemId" BaseColumnName="InheritedSubItemId" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Family" BaseColumnName="ItemFamilyName" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SubFamily" BaseColumnName="SubItemFamilyName" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ClassName" BaseColumnName="ItemClassName" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SubClassName" BaseColumnName="SubItemClassName" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemName" BaseColumnName="ItemName" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemSKU" BaseColumnName="ItemSKU" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SubItemName" BaseColumnName="SubItemName" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SubItemSKU" BaseColumnName="SubItemSKU" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Bidirectional" BaseColumnName="Bidirectional" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsExcluded" BaseColumnName="IsExcluded" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Class" Key="Class" Width="150px" CellMultiline="Yes">
											  <CellStyle Wrap="true"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="SKU" Key="SKU" Width="100px"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" CellMultiline="Yes">
											  <CellStyle Wrap="true"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="DSR" Key="IsRecommended" Width="22px" Type="CheckBox" DataType="System.Boolean"
												BaseColumnName="Recommended" AllowUpdate="No">
												<CellStyle VerticalAlign="Middle" HorizontalAlign="Center"></CellStyle>
												<Footer Key="IsRecommended"></Footer>
												<Header Key="IsRecommended" Caption="R" Title="DS Recommended"></Header>
											</igtbl:UltraGridColumn>
									  </Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
            </div>
					</td>
				</tr>
				<tr valign="bottom" style="height:1px">
					<td>
			      <div id="divToolbar">
						  <table class="hc_toolbartitle" style="height:15px; width:100%" cellspacing="0" cellpadding="0" border="0">
							  <tr style="height:100%">
								  <td>
								    <img alt="Links" src="/hc_v4/img/ed_links.gif" />&nbsp;
									  <asp:label id="lbLCount" runat="server">Link count : 0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									  <asp:label id="lbRCount" runat="server">DS Recommended link count : 0</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								  </td>
							  </tr>
						  </table>
				    </div>
					</td>
				</tr>
			</table>
</asp:Content>