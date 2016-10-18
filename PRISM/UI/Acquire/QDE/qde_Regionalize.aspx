<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_Regionalize" CodeFile="qde_Regionalize.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Regionalize</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
			function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
        if (oButton.Key == 'Close')
        {
					oEvent.CancelPostBack = true;
					window.close();
        }
  	  }
  	  
  	  function UpdateGrid(itemId, inputFormId, cultureCode)
  	  {
  			if (opener != null)
  			{
  			  opener.document.location = 'QDE_formcontent.aspx?i='+itemId+'&f=IF_'+inputFormId+'&c='+cultureCode;
  			}
  			window.close();
  	  }
	  function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="main" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="25px" width="100%">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Duplicate" ToolTip="Regionalize selected content" Image="/hc_v4/img/ed_copy.gif" Text="Regionalize"><DefaultStyle Width="120px"></DefaultStyle></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DuplicateSep"></igtbar:TBSeparator>
								<%--Alternate for CR 5096(Removal of rejection functionality)
								<igtbar:TBarButton Key="Reject" ToolTip="Reject Master Content" Image="/hc_v4/img/ed_reject.gif" Text="Reject"><DefaultStyle Width="80px"></DefaultStyle></igtbar:TBarButton>
								<igtbar:TBSeparator Key="RejectSep"></igtbar:TBSeparator>--%>
								<igtbar:TBarButton Key="ilb" ToolTip="Force this chunk to be intentionaly left blank" Text="Blank" Image="/hc_v4/img/ed_blank.gif"><DefaultStyle Width="80px"></DefaultStyle></igtbar:TBarButton>						
								<igtbar:TBSeparator Key="ilbSep"></igtbar:TBSeparator>
								<%--Alternate for CR 5096(Removal of rejection functionality)--%>
								<igtbar:TBarButton Key="Close" ToolTip="Close window" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label ID="lbError" Runat="server" Visible="False" CssClass="hc_error"></asp:Label>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td align="center"><asp:label id="lbResult" runat="server" CssClass="hc_success" Visible="False">No result</asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
							<igtbl:ultrawebgrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
								<DisplayLayout AutoGenerateColumns="False" Version="4.00" SelectTypeRowDefault="Single" BorderCollapseDefault="Separate"
									EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<RowSelectorStyleDefault Width="30px" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</RowSelectorStyleDefault>
									<FrameStyle Width="100%"></FrameStyle>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
									<ActivationObject BorderColor="Orange"></ActivationObject>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BaseTableName="Roles" Key="Roles" BorderCollapse="Collapse" DataKeyField="Id">
										<Columns>
											<igtbl:UltraGridColumn Key="Index" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="S" Hidden="True" BaseColumnName="Status"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InputFormContainerId" Hidden="True" BaseColumnName="InputFormContainerId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsMandatory" Hidden="True" BaseColumnName="Mandatory"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Path" BaseColumnName="Path" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Rtl" BaseColumnName="Rtl" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ContainerId" Hidden="True" BaseColumnName="ContainerId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="CultureCode" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SourceCode" Hidden="True" BaseColumnName="CultureCode"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsResource" BaseColumnName="IsResource" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsBoolean" BaseColumnName="IsBoolean" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Inherited" Key="Inherited" Width="20px" BaseColumnName="Inherited" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ReadOnly" Hidden="True" BaseColumnName="ReadOnly"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Regionalizable" Hidden="True" BaseColumnName="Regionalizable"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Overrides" Hidden="True" BaseColumnName="Overrides"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Editable" Hidden="True" BaseColumnName="Editable"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="HasFallback" Hidden="True" BaseColumnName="HasFallback"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Globalizable" Hidden="True" BaseColumnName="_Globalizable"></igtbl:UltraGridColumn>
											<igtbl:TemplatedColumn Key="Select" Width="20px" FooterText="">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
												<HeaderTemplate>
													<asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox>
												</HeaderTemplate>
												<CellTemplate>
													<asp:CheckBox id="g_sd" onclick="return g_su(this);" Enabled="false" runat="server"></asp:CheckBox>
                                                    <asp:Label id="grp_lbl" Text="Hello" runat="server" Visible="false"></asp:Label>
												</CellTemplate>
											</igtbl:TemplatedColumn>
											<igtbl:UltraGridColumn HeaderText="M" Key="Mandatory" Width="15px">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="S" Key="Status" Width="15px" BaseColumnName="Status">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Container" Key="ContainerName" Width="170px" BaseColumnName="ContainerName"
												CellMultiline="Yes">
												<CellStyle Wrap="True" CssClass="ptb1"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Value" Key="Value" Width="100%" AllowGroupBy="No" BaseColumnName="Text"
												CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
										</Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
						</div>
					</td>
				</tr>
			</table>
</asp:Content>