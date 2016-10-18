<%@ Control Language="c#" Inherits="hc_homePageModules.uc_welcome" CodeFile="uc_welcome.ascx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="MM" Namespace="HyperCatalog.MasterModules" Assembly="HyperCatalog.MasterModules" %>
<MM:mastermodule id="Module" runat="server" ModuleName="Welcome" ModuleTitle="Welcome">
	<igtbl:ultrawebgrid id="uwgWelcome" runat="server" Width="100%" OnInitializeRow="dg_InitializeRow">
		<DisplayLayout AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00" RowSelectorsDefault="No"
			Name="xctl0uwgWelcome" TableLayout="Fixed" NoDataMessage="No counter to display">
			<HeaderStyleDefault Height="0px"></HeaderStyleDefault>
			<FrameStyle Width="100%" BorderStyle="None" CssClass="dataTable"></FrameStyle>
			<ActivationObject AllowActivation="False"></ActivationObject>
			<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="0px" CssClass="ugd"></RowStyleDefault>
		</DisplayLayout>
		<Bands>
			<igtbl:UltraGridBand>
				<Columns>
					<igtbl:UltraGridColumn Key="Text" BaseColumnName="Text" Width="100%" >
						<CellStyle Wrap="True"></CellStyle>
						<Footer Key=""></Footer>
						<Header Key=""></Header>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn Key="URL" BaseColumnName="Url" ServerOnly="true">
					</igtbl:UltraGridColumn>
				</Columns>
			</igtbl:UltraGridBand>
		</Bands>
	</igtbl:ultrawebgrid>
</MM:mastermodule>
