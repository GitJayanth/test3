<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="inputformtype_exclusionruleEdit" CodeFile="inputformtype_exclusionruleEdit.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Edit exclusion rule</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
        if (oButton.Key == 'Close'){
					top.window.close();
					oEvent.cancelPostBack = true;
        }
		  } 
		  
		  function UpdateParentWithClose(){
		    opener.UpdateDocument();
		    window.close();
		  }
	  </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellpadding="0" cellspacing="0">
				<tr valign="top">
					<td class="sectionTitle">
						<asp:Label id="lblTitle" runat="server"></asp:Label>
					</td>
				</tr>
				<tr height="1" valign="top">
					<td>
                    <%--Removed the width property to fix enlarge buttons by Radha S--%>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px"
							ImageDirectory=" ">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Save" Text="Add" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:label id="lbError" runat="server" CssClass="hc_error" Visible="false">Error message</asp:label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
					  <igtbl:ultrawebgrid id="dg" runat="server" ImageDirectory="/ig_common/Images/" UseAccessibleHeader="False">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" SelectTypeRowDefault="Single"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No container type">
								<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="ExclusionRules" BorderCollapse="Collapse">
									<Columns>
										<igtbl:TemplatedColumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
											<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
											<HeaderTemplate>
												<asp:CheckBox id="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
											</HeaderTemplate>
											<CellTemplate>
												<asp:CheckBox id="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
											</CellTemplate>
											<Footer Key="Select" Caption=""></Footer>
											<Header Key="Select"></Header>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn HeaderText="Container code" Key="Code" Width="25%" BaseColumnName="Code"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Container type" Key="Name" Width="75%" BaseColumnName="Name"></igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
						</div>
					</td>
				</tr>
			</table>
</asp:Content>