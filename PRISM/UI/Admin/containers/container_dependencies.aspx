<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="container_dependencies" CodeFile="container_dependencies.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Dependencies</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
			var winInputForms = null;
			var winDependencies = null;
			function AddContainerDependency()
			{
				var fContainerId = document.getElementById("hFeatureContainerId").Value;
				var url = "Container_DependencyEdit.aspx?c="+fContainerId;
        winDependencies = OpenModalWindow(url, "AddDependency", 200, 550, "yes");
			}
			
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		    if (oButton.Key == 'List') 
		    {
          back();
          oEvent.cancelPostBack = true;
        }
        else if (oButton.Key == 'Add')
        {
					AddContainerDependency();
          oEvent.cancelPostBack = true;
        }
        else if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
        else if (oButton.Key == 'Export')
        {
          oEvent.cancelPostBack = false;
        }
        else if (oButton.Key == 'Delete')
        {
					if (dg_nbItems_Checked==0)
		      {
	          alert('You must select at least one item');
	          oEvent.cancelPostBack = true;
	        }
	        else
	        {
            oEvent.cancelPostBack = !confirm("Are you sure you want to delete selected items ?");
          }
        }
		  } 
		  function OpenPopupInputForms(containerId)
		  {
				var url = "container_inputforms.aspx?c="+containerId+"&b=1";
        winInputForms = OpenModalWindow(url, "InputForm", 300, 600, "Yes");
        if (winInputForms)
					winInputForms.focus();
		  }
		  
		  function BeforeClose()
		  {
				if (winInputForms) 
					winInputForms.close();
				if (winDependencies) 
					winDependencies.close();
		  }
			</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" language="javascript">
		document.body.onunload="BeforeClose";
	</script>
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" ToolTip="Delete selected" Text="Delete selected" Image="/hc_v4/img/ed_delete.gif">
									<DefaultStyle Width="120px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
									<DefaultStyle Width="25px"></DefaultStyle>
								</igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display" CellClickActionDefault="Edit">
								<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
                <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
 							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="ContainerDependencies" BorderCollapse="Collapse">
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
										<igtbl:UltraGridColumn Key="Id" ServerOnly="True" BaseColumnName="TechspecContainerId"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Tag" Key="Tag" Width="280px" BaseColumnName="TechspecTag" CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="250px" BaseColumnName="TechspecName" CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Definition" Key="Definition" Width="100%" BaseColumnName="TechspecDef"
											CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
						<CENTER>
							<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
						</CENTER>
					</td>
				</tr>
			</table>
			<input type="hidden" id="hFeatureContainerId">
</asp:Content>