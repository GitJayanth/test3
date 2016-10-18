<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="linktype_itemtypes" CodeFile="linktype_itemtypes.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Item type</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
		    if (oButton.Key == 'List') 
		    {
          back();
          oEvent.cancelPostBack = true;
        }
		    else if (oButton.Key == 'Apply') 
		    {
          oEvent.cancelPostBack = false;
        }
		  } 
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving button issue  by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td colspan="2"><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="120px" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
									<DefaultStyle Width="80px" Height="20px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Apply" Text="Apply change" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td colspan="2">
						<asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<IGTBL:ULTRAWEBGRID id="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/" UseAccessibleHeader="False" OnInitializeRow="dg_InitializeRow">
							<DISPLAYLAYOUT MERGESTYLES="False" AUTOGENERATECOLUMNS="False" SELECTTYPEROWDEFAULT="Single" ROWSELECTORSDEFAULT="No"
								NAME="dg" TABLELAYOUT="Fixed" CELLCLICKACTIONDEFAULT="RowSelect" NODATAMESSAGE="No input forms attached to this container">
								<%-- Fix for QC# 7384 by Rekha Thomas. Added borderdetails tag to fix the bold issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh" QC#7384--%>
									    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
						        </HeaderStyleDefault>
								<FRAMESTYLE WIDTH="100%" CSSCLASS="dataTable"></FRAMESTYLE>
								<ROWALTERNATESTYLEDEFAULT CSSCLASS="uga"></ROWALTERNATESTYLEDEFAULT>
								<ROWSTYLEDEFAULT TEXTOVERFLOW="Ellipsis" VERTICALALIGN="Top" BORDERWIDTH="1px" CSSCLASS="ugd"></ROWSTYLEDEFAULT>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
							</DISPLAYLAYOUT>
							<BANDS>
								<IGTBL:ULTRAGRIDBAND BaseTableName="InputFormContainers" Key="InputFormContainerId" BorderCollapse="Collapse"
									DataKeyField="InputFormContainerId">
									<COLUMNS>
										<IGTBL:ULTRAGRIDCOLUMN Key="IsLinked" Width="20px" BaseColumnName="IsLinked" ServerOnly="true"></IGTBL:ULTRAGRIDCOLUMN>
										<IGTBL:TEMPLATEDCOLUMN Key="Select" Width="20px" BaseColumnName="" FooterText="">
											<CELLSTYLE VERTICALALIGN="Top" WRAP="True" HorizontalAlign="Center"></CELLSTYLE>
											<HEADERTEMPLATE>
												<ASP:CHECKBOX id="g_ca" onclick="javascript:return g_su(this);" runat="server"></ASP:CHECKBOX>
											</HEADERTEMPLATE>
											<CELLTEMPLATE>
												<ASP:CHECKBOX id="g_sd" onclick="javascript:return g_su(this);" runat="server"></ASP:CHECKBOX>
											</CELLTEMPLATE>
											<FOOTER KEY="Select" CAPTION=""></FOOTER>
											<HEADER KEY="Select"></HEADER>
										</IGTBL:TEMPLATEDCOLUMN>
										<IGTBL:ULTRAGRIDCOLUMN HeaderText="Id" Key="ItemTypeId" Width="50px" BaseColumnName="ItemTypeId"></IGTBL:ULTRAGRIDCOLUMN>
										<IGTBL:ULTRAGRIDCOLUMN HeaderText="Name" Key="ItemType" Width="200px" BaseColumnName="ItemTypeName"></IGTBL:ULTRAGRIDCOLUMN>
									</COLUMNS>
								</IGTBL:ULTRAGRIDBAND>
							</BANDS>
						</IGTBL:ULTRAWEBGRID>
						<CENTER>
							<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
						</CENTER>
					</td>
				</tr>
			</table>
</asp:Content>