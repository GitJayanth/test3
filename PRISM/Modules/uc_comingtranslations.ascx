<%@ Register TagPrefix="MM" Namespace="HyperCatalog.MasterModules" Assembly="HyperCatalog.MasterModules" %>
<%@ Control Language="c#" Inherits="hc_homePageModules.uc_comingtranslations" CodeFile="uc_comingtranslations.ascx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<MM:mastermodule id="Module" runat="server" ModuleName="Translations" ModuleTitle="Products to be translated (30 days)">
	<igtbl:UltraWebGrid id="uwgTranslations" runat="server" OnInitializeRow="dg_InitializeRow">
								<DisplayLayout AutoGenerateColumns="False" Version="4.00" SelectTypeRowDefault="Single" BorderCollapseDefault="Separate"
									EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
							<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh2">
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" ></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
								</DisplayLayout>
		<Bands>
			<igtbl:UltraGridBand>
				<Columns>
					<igtbl:UltraGridColumn Key="ItemId" Width="20" BaseColumnName="ItemId" ServerOnly="true">
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn Key="LevelId" Width="20" BaseColumnName="LevelId">
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn HeaderText="Product" Key="ItemName" Width="200" BaseColumnName="ItemName">
						<CellStyle Wrap="true"></CellStyle>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn HeaderText="BOT" Key="BOT" Width="70" BaseColumnName="BOT">
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn ServerOnly="true" Key="ItemNumber" Width="20" BaseColumnName="ItemNumber">
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn ServerOnly="true" Key="NbDays" Width="20" BaseColumnName="NbDaysBeforeBOT">
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn HeaderText="Languages" Key="Languages" Width="200" BaseColumnName="LanguageCodes">
						<CellStyle Wrap="true"></CellStyle>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn HeaderText="Draft" Key="Draft" Width="50" BaseColumnName="NbDraftChunks">
						<CellStyle HorizontalAlign="Center"></CellStyle>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn HeaderText="Missing" Key="Missing" Width="50" BaseColumnName="NbMissingFixedChunks">
						<CellStyle HorizontalAlign="Center"></CellStyle>
					</igtbl:UltraGridColumn>
				</Columns>
			</igtbl:UltraGridBand>
		</Bands>
	</igtbl:UltraWebGrid>
	<br /><br />
</MM:mastermodule>
