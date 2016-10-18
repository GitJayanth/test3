<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Login.HomePageProperties" CodeFile="HomePageProperties.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" Runat="Server">
			<table height="100%" cellspacing="0" cellpadding="0" width="100%">
				<tr bgColor="#b2b2b2" height="1">
					<td class="hc_pagetitle" width="99%">Properties</td>
				</tr>
				<tr height="1">
					<td><igtbar:ultrawebtoolbar id="uwtProperties" runat="server" Width="100%" Height="24px" CssClass="hc_toolbar"
							ItemWidthDefault="80px" OnButtonClicked="uwtProperties_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<Items>
								<igtbar:TBarButton Key="btnSave" ToolTip="Save properties" Image="/hc_v4/img/homePage/disk.gif">
									<DefaultStyle Width="20px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBLabel Text="" Key="lblSpace">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBarButton Key="btnCancel" ToolTip="Close window" Image="/hc_v4/img/homePage/ed_cancel.gif">
									<DefaultStyle Width="20px"></DefaultStyle>
								</igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr bgColor="whitesmoke">
					<td><igtbl:ultrawebgrid id="uwgModules" runat="server" Width="100%" Height="100%" OnInitializeRow="uwgModules_InitializeRow">
							<DisplayLayout AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00" cellpaddingDefault="2"
								RowSelectorsDefault="No" Name="uwgModules" TableLayout="Fixed" NoDataMessage="No counter to display">
								<HeaderStyleDefault Width="150px" VerticalAlign="Middle" HorizontalAlign="Left" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" Height="100%" CssClass="dataTable"></FrameStyle>
								<ActivationObject AllowActivation="False"></ActivationObject>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="0px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand>
									<Columns>
										<igtbl:UltraGridColumn Key="WebPartId" Hidden="True" BaseColumnName="WebPartId">
											<Footer Key="WebPartId"></Footer>
											<Header Key="WebPartId"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="IsPresent" Width="10%" Type="CheckBox" BaseColumnName="IsPresent" AllowUpdate="Yes">
											<Footer Key="IsPresent"></Footer>
											<Header Key="IsPresent"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Caption" Key="Caption" Width="35%" BaseColumnName="Caption">
											<Footer Key="Caption"></Footer>
											<Header Key="Caption" Caption="Caption"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:TemplatedColumn Key="PaneName" Width="40%" HeaderText="Pane name" BaseColumnName="">
											<CellTemplate>
												<asp:DropDownList id="ddlPaneList" runat="server" Width="100%"></asp:DropDownList>
											</CellTemplate>
											<Footer Key="PaneName"></Footer>
											<Header Key="PaneName" Caption="Pane name"></Header>
										</igtbl:TemplatedColumn>
										<igtbl:TemplatedColumn Key="Sort" Width="15%" HeaderText="Sort" BaseColumnName="">
											<CellTemplate>
												<igtxt:WebNumericEdit id="wneModuleOrder" runat="server" Width="100%" DataMode="Int"></igtxt:WebNumericEdit>
											</CellTemplate>
											<Footer Key="Sort"></Footer>
											<Header Key="Sort" Caption="Sort"></Header>
										</igtbl:TemplatedColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
					</td>
				</tr>
			</table>
</asp:Content>