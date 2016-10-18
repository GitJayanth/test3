<%@ Reference Page="~/ui/globalize/termbase/term_translationedit.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Acquire.SystemWords" CodeFile="SystemWords.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">System Words</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script type="text/javascript">
	
		function uwToolbar_Click(oToolbar, oButton, oEvent){
			if (oButton.Key == 'Add') {
    	  var url = 'SpellChecker/SystemWords_Load.aspx';
    	  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	  url += '#target';
    	  winContainerEdit = OpenModalWindow(url,'containerwindow', 200, 500, 'no')
			  oEvent.cancelPostBack = true;
			}
		}	
		  			  
	</script>
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle" height="17">List of dictionaries available</td>
		</tr>
		<tr valign="top">
			<td>
				<igtbar:UltraWebToolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%"
					ImageDirectory=" ">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="Add" Text="Add" Visible="False" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
					</Items>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				</igtbar:UltraWebToolbar></td>
		</tr>
		<tr valign="top">
			<td class="main">
				<asp:Label id="lbMessage" runat="server" Visible="False">Message</asp:Label><br/>
				<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
					<DisplayLayout ReadOnly="LevelOne" MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
						Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No"
						Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
						<Pager QuickPages="10" PageSize="50" PagerAppearance="Top" StyleMode="QuickPages" AllowPaging="True">
							<Style Font-Size="xx-small" HorizontalAlign="Right" CssClass="ptb3"></Style>
						</Pager>
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
								<igtbl:UltraGridColumn HeaderText="Language" Key="Locale" Width="200px" BaseColumnName="LanguageCode"></igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn HeaderText="words count" Key="WordsCount" Width="100px" BaseColumnName="Count"></igtbl:UltraGridColumn>
							</Columns>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:ultrawebgrid><br/>
				<CENTER>
					<asp:Label id="lbNoresults" runat="server" Font-Bold="True" ForeColor="Red">No results</asp:Label></CENTER>
			</td>
		</tr>
	</table>
	<input type="hidden" name="action"/>
</asp:Content>