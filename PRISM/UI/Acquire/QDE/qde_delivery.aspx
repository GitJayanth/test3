<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_Delivery" CodeFile="QDE_Delivery.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Delivery</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script language="javascript" type="text/javascript">
		    function uwToolbar_Click(oToolbar, oButton, oEvent)
		    {
		      if (oButton.Key == 'print')
		      {     		      
		        oEvent.cancelPostBack = true;		  
		        document.getElementById("divDg").style.overflow ='visible';  
		        document.body.scroll = "yes";
  	        this.window.print();
		      }
		      else if (oButton.Key == 'filter')
		      {     		     
		        DoSearch();	
            oEvent.cancelPostBack = true;
            return;
		      }
  	    }
		function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
            function UnloadGrid()
            {
                igtbl_unloadGrid("dgI");
            }
			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
     <script language="javascript" type="text/javascript">
  		  document.body.scroll = "no";
			</script>
			<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td>
					  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="25px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<hoverstyle CssClass="hc_toolbarhover"></hoverstyle>
							<selectedstyle CssClass="hc_toolbarselected"></selectedstyle>
              <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
 							<items>
								<igtbar:TBLabel Text="Filter">
									<defaultstyle Font-Bold="True" Width="40px"></defaultstyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Key="filterField" Width="150px">
									<asp:TextBox runat="server" ID="txtFilter" CssClass="Search" Width="150px" MaxLength="50"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator />
								<igtbar:TBarButton Key="Export" ToolTip="Export current view" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="ExportSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="print" Image="/hc_v4/img/ed_print.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Key="mpd">
								<DefaultStyle Width="100%" TextAlign="Right" Font-Bold="true"></DefaultStyle></igtbar:TBLabel>
							</items>
							<defaultstyle CssClass="hc_toolbardefault"></defaultstyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top">
					<td>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
                        <%--Modified the code to fix infragestic issue--%>
						<igtbl:ultrawebgrid id="dgChunks" runat="server" ImageDirectory="/ig_common/Images/" Width="100%" OnInitializeRow="dg_InitializeRow">
								<DisplayLayout ReadOnly="PrintingFriendly" NoDataMessage="No data to display" TableLayout="Fixed" Name="dg" RowSelectorsDefault="No"
									EnableInternalRowsManagement="True" BorderCollapseDefault="Separate" Version="4.00" AutoGenerateColumns="False">
									<HeaderStyleDefault  VerticalAlign="Middle"> <%--CssClass="gh"--%>
										<BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
									</HeaderStyleDefault>
									<RowSelectorStyleDefault CssClass="gh" Width="30px">
										<BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
									</RowSelectorStyleDefault>
                                    <FrameStyle Width="100%">
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
                                    </FrameStyle>
									<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand Key="Roles" DataKeyField="Id" BorderCollapse="Collapse" BaseTableName="Roles">
										<Columns>
											<igtbl:UltraGridColumn Key="Index" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="S" Hidden="True" BaseColumnName="Status"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InputFormContainerId" Hidden="True" BaseColumnName="InputFormContainerId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsMandatory" Hidden="True" BaseColumnName="IsMandatory"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Path" BaseColumnName="Path" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Rtl" BaseColumnName="Rtl" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ContainerId" Hidden="True" BaseColumnName="ContainerId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="SourceCode" Hidden="True" BaseColumnName="CultureCode"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="CultureCode" Hidden="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsResource" BaseColumnName="IsResource" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="IsBoolean" BaseColumnName="IsBoolean" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Inherited" Width="20px" Hidden="True" BaseColumnName="Inherited" HeaderText="Inherited"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ReadOnly" Hidden="True" BaseColumnName="ReadOnly"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Overrides" Hidden="True" BaseColumnName="Overrides"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Editable" Hidden="True" BaseColumnName="Editable"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="HasFallback" Hidden="True" BaseColumnName="HasFallback"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Globalizable" Hidden="True" BaseColumnName="_Globalizable"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Mandatory" Width="15px" HeaderText="M">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Status" Width="15px" BaseColumnName="ChunkStatus" HeaderText="S">
												<CellStyle CssClass="ptb1" Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ContainerName" Width="170px" BaseColumnName="ContainerName" HeaderText="Container" CellMultiline="Yes">
												<CellStyle CssClass="ptb1" Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Value" Width="100%" BaseColumnName="ChunkValue" HeaderText="Value" CellMultiline="Yes" AllowGroupBy="No">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Group" Hidden="True"></igtbl:UltraGridColumn>
										</Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
							<br/>
							<asp:Label id="labelLinks" runat="server" CssClass="hc_pagetitle" Width="100%">Links</asp:Label>
                            <%--Modified the code to fix infragestic issue--%>
							<igtbl:ultrawebgrid id="dgLinks" runat="server" Width="100%" ImageDirectory="/ig_common/Images/" OnInitializeRow="dgLinks_InitializeRow">
								<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
									HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dgI" TableLayout="Fixed" NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true"> <%--CssClass="gh">--%>
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<FrameStyle Width="100%" CssClass="dataTable">
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
                                    </FrameStyle>
									<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
									<ActivationObject AllowActivation="False"></ActivationObject>
									<RowAlternateStyleDefault> <%--CssClass="uga">--%>
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
                                    </RowAlternateStyleDefault>
									<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
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
											<igtbl:UltraGridColumn HeaderText="Class" Key="Class" Width="100px" CellMultiline="Yes">
											  <CellStyle Wrap="true"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="SKU" Key="SKU" Width="100%" CellMultiline="Yes">
											  <CellStyle Wrap="true"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="400px" CellMultiline="Yes">
											  <CellStyle Wrap="true"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="DSR" Key="IsRecommended" Width="22px" Type="CheckBox" DataType="System.Boolean" BaseColumnName="Recommended" AllowUpdate="No">
												<CellStyle VerticalAlign="Middle" HorizontalAlign="Center"></CellStyle>
												<Header Key="IsRecommended" Caption="DSR" Title="DS Recommended"></Header>
											</igtbl:UltraGridColumn>
									  </Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
							<br/>
							<asp:Label id="labelPLC" runat="server" CssClass="hc_pagetitle" Width="100%">PLC</asp:Label>
                            <%--Modified the code to fix infragestic issue--%>
							<igtbl:ultrawebgrid id="dgPLC" runat="server" Width="100%" ImageDirectory="/ig_common/Images/">
								<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
									HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dgI" TableLayout="Fixed"
									NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" Cursor="Hand" BackColor="LightGray"> <%--CssClass="gh">--%>
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<FrameStyle Width="100%" CssClass="dataTable">
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
                                    </FrameStyle>
									<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
									<ActivationObject AllowActivation="False"></ActivationObject>
									<RowAlternateStyleDefault> <%--CssClass="uga">--%>
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
                                    </RowAlternateStyleDefault>
									<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                        <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
                                    </RowStyleDefault>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BaseTableName="PLC" Key="Links" BorderCollapse="Collapse">
										<Columns>
											<igtbl:UltraGridColumn Key="PID" Width="150px" BaseColumnName="LiveDate">
											<CellStyle HorizontalAlign="Center"></CellStyle></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="POD" Width="150px" BaseColumnName="ObsoleteDate">
											<CellStyle HorizontalAlign="Center"></CellStyle></igtbl:UltraGridColumn>
										</Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
							<br />
							<br />
						</div>
					</td>
				</tr>
			</table>
</asp:Content>