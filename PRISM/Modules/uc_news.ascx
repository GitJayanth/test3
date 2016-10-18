<%@ Register TagPrefix="MM" Namespace="HyperCatalog.MasterModules" Assembly="HyperCatalog.MasterModules" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Control Language="c#" Inherits="hc_homePageModules.uc_news" CodeFile="uc_news.ascx.cs" %>
<MM:mastermodule id="Module" runat="server" ModuleName="News" ModuleTitle="News">
	<igtbl:UltraWebGrid id="uwgNews" runat="server" OnInitializeRow="uwgNews_InitializeRow">
		<DisplayLayout AutoGenerateColumns="False" RowHeightDefault="100%" Version="4.00" RowSelectorsDefault="No"
			Name="xctl0uwgNews" TableLayout="Fixed" NoDataMessage="No news to display">
			<HeaderStyleDefault Height="0px"></HeaderStyleDefault>
			<FrameStyle Width="100%" BorderStyle="None" CssClass="dataTable"></FrameStyle>
			<ActivationObject AllowActivation="False"></ActivationObject>
			<SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
			<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="0px" CssClass="ugd"></RowStyleDefault>
		</DisplayLayout>
		<Bands>
			<igtbl:UltraGridBand> 
				<Columns>
					
					<igtbl:UltraGridColumn Key="CreateDate" Width="80px" BaseColumnName="Date">
						<CellStyle Wrap="True"></CellStyle>
						<Footer Key="Label"></Footer>
						<Header Key="Label" Caption="News"></Header>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn Key="ShortDescription" Width="100%" BaseColumnName="ShortDescription">
						<CellStyle Wrap="True"></CellStyle>
					</igtbl:UltraGridColumn>
					<igtbl:UltraGridColumn Key="NewsId" hidden="true" BaseColumnName="NewsId">
					</igtbl:UltraGridColumn>

				</Columns>
			</igtbl:UltraGridBand>
		</Bands>
	</igtbl:UltraWebGrid></MM:mastermodule>
