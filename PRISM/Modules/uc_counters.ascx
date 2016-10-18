<%@ Register TagPrefix="MM" Namespace="HyperCatalog.MasterModules" Assembly="HyperCatalog.MasterModules" %>
<%@ Control Language="c#" Inherits="hc_homePageModules.uc_counters" CodeFile="uc_counters.ascx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<MM:mastermodule id="Module" runat="server" ModuleName="Counters" ModuleTitle="Counters">
	<igtbl:UltraWebGrid id="uwgCounters" runat="server">
		<DisplayLayout AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00" RowSelectorsDefault="No"
			Name="xctl0uwgCounters" TableLayout="Fixed" NoDataMessage="No counter to display">
			<HeaderStyleDefault Height="0px"></HeaderStyleDefault>
			<FrameStyle Width="100%" BorderStyle="None" CssClass="dataTable"></FrameStyle>
			<ActivationObject AllowActivation="False"></ActivationObject>
			<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="0px" CssClass="ugd"></RowStyleDefault>
		</DisplayLayout>
		<Bands>
			<igtbl:UltraGridBand>
				<Columns>
					<igtbl:UltraGridColumn Key="" Width="50%" BaseColumnName="CounterName">
						<CellStyle HorizontalAlign="Left"></CellStyle>
						<Footer Key=""></Footer>
						<Header Key=""></Header>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn Key="" Width="50%" BaseColumnName="Counter">
						<CellStyle HorizontalAlign="Right"></CellStyle>
						<Footer Key=""></Footer>
						<Header Key=""></Header>
					</igtbl:UltraGridColumn>
				</Columns>
			</igtbl:UltraGridBand>
		</Bands>
	</igtbl:UltraWebGrid>
</MM:mastermodule>
