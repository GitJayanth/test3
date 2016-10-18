<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.InputFormsAttach" CodeFile="InputFormsAttach.aspx.cs" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input form attachment</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" Runat="Server">
		<script>
		function ReloadQDEContent(itemId, cultureCode)
		{
			// Reload framecontent
			if (opener)
			{
			  var url = 'QDE/QDE_FormRoll.aspx?i=' +itemId + '&c=' +cultureCode;
			  opener.parent.framecontent.document.location = url;
			}
		}
		function UnloadGrid()
           	 {
               		 igtbl_unloadGrid("dg");
           	 }

		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" Runat="Server">
<script type="text/javascript" language="javascript">
  document.body.onload = function(){this.focus();};
</script>
			<table cellpadding="0" cellspacing="0">
				<tr class="selectlanguage">
					<td>&nbsp;<asp:label id="lItemName" runat="server">CurrentItemName</asp:label>
          </td>
					<td align="right"><asp:label id="lItemLevel" runat="server">CurrentItemLevel</asp:label>&nbsp;</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td colspan=2>
						<igtbar:ultrawebtoolbar id="uwToolBar" runat="server" ImageDirectory=" " CssClass="hc_toolbar" ItemWidthDefault="80px"
							width="100%" OnButtonClicked="uwToolBar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBLabel Text="Level to apply">
									<DefaultStyle Width="100px"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" runat="server">
									<asp:DropDownList id="ddlLevels" runat="server" AutoPostBack="True" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlLevels_SelectedIndexChanged"></asp:DropDownList>
								</igtbar:TBCustom>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBLabel Text="Input form">
									<DefaultStyle Width="100px"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="250px" runat="server">
									<igcmbo:WebCombo Runat="server"
										Height="16px" Width="250px" BorderColor="LightGray" ID="cbInputForms" OnInitializeRow="cbInputForms_InitializeRow" Version="4.00">
										<ExpandEffects ShadowColor="Transparent"></ExpandEffects>
										<Columns>
											<igtbl:UltraGridColumn HeaderText="Id" Key="InputFormId" ServerOnly="True" BaseColumnName="InputFormId">
                        <header caption="Id"></header>
                      </igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="Name"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ShortName" ServerOnly="True" BaseColumnName="ShortName"></igtbl:UltraGridColumn>
										</Columns>
										<DropDownLayout DropdownWidth="450px" BorderCollapse="Separate" RowSelectors="No" RowHeightDefault="17px" AutoGenerateColumns="False" ColHeadersVisible="No" Version="4.00">
											<RowStyle BorderWidth="0px" BorderColor="Gray" BorderStyle="None" BackColor="White">
												<BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
											</RowStyle>
											<SelectedRowStyle ForeColor="White" BackColor="DarkBlue"></SelectedRowStyle>
											<HeaderStyle BorderStyle="Solid" BackColor="LightGray">
												<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
											</HeaderStyle>
											<FrameStyle Width="450px" Cursor="Default" BorderWidth="2px" Font-Size="XX-Small" Font-Names="Verdana"
												BorderStyle="Ridge" BackColor="White" Height="130px"></FrameStyle>
										</DropDownLayout>
									</igcmbo:WebCombo>
								</igtbar:TBCustom>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="add" ToolTip="Attach input form to current level" Text="Apply"
									Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="applyAll" ToolTip="Attach input form to current level and below"
									Text="Apply all" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td colspan=2>
						<asp:Label ID="lbError" Runat="server" CssClass="hc_error" Visible="false"></asp:Label>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td colspan=2>
						<table align="right" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td>&nbsp;</td>
								<td id="AppLevelTitle" runat="server" class="gh" align="center">Applicable levels</td>
								<td width="24">&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr valign="top">
					<td colspan="2">
						<igtbl:ultrawebgrid id="dg" runat="server" Width="100%" UseAccessibleHeader="False" OnInitializeRow="dg_InitializeRow">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
								<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand>
									<Columns>
										<igtbl:UltraGridColumn Key="IFLongName" ServerOnly="True" BaseColumnName="IFLongName"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="InputFormId" ServerOnly="True" BaseColumnName="InputFormId"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="Herited" ServerOnly="True" BaseColumnName="Herited"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Level" Key="ItemLevelId" Width="50px" BaseColumnName="ItemLevelId">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Item name" Key="ItemName" Width="100%" BaseColumnName="ItemName">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Input form name" Key="IFName" Width="250px" BaseColumnName="IFShortName">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Active" Width="50px" Key="IsActive" Type="CheckBox" DataType="System.Boolean"
											BaseColumnName="IsActive" AllowUpdate="No">
											<CellStyle HorizontalAlign="Center"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:TemplatedColumn Key="Action" Width="25px" BaseColumnName="">
											<CellStyle HorizontalAlign="Center"></CellStyle>
											<CellTemplate>
												<asp:ImageButton id="imgDel" onclick="Delete_Click" style="cursor:pointer" runat="server" ImageUrl="/hc_v4/img/ed_delete.gif"
													ToolTip="Delete"></asp:ImageButton>
											</CellTemplate>
										</igtbl:TemplatedColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>						
					</td>
				</tr>
				<tr valign="bottom"><td align="center" style="height: 19px" colspan=2>
          <b><asp:LinkButton ID="lnkExportCartography" runat="server" OnClick="lnkExportCartography_Click" Font-Size="x-Small" ToolTip="Click here to create the Excel cartography report">>> Export Cartography <<</asp:LinkButton></b></td></tr>
			</table>
</asp:Content>