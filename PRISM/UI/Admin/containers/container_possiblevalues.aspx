<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="container_possiblevalues" CodeFile="container_possiblevalues.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Possible values</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'filter')
		    {     		 
		  
		        var sURL = window.document.URL.toString();
		        var arrParams = sURL.split("&");
                sURL  = arrParams[0] ;
		        if (txtFilterField != undefined)
               {
                  sURL = sURL + "&filter=" + encodeURI(txtFilterField.value)  ;
               } 
                if (culSelect != undefined)
               {
                
                 sURL = sURL + "&CultureList=" + culSelect.value ;
               }
            window.location= sURL;
		    //DoSearch();	
            oEvent.cancelPostBack = true;
            return;
        }
        if (oButton.Key == 'Export')
        {
          oEvent.cancelPostBack = false;
        }
		  } 
			</script>
</asp:Content>
<%--Removed width propery in UltraWebToolbar to fix the horizontal width issue By Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox runat="server" Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
									<DefaultStyle Width="25px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
                                <igtbar:TBLabel Text="Culture">
                                    <DefaultStyle Font-Bold="True" Width="50px">
                                    </DefaultStyle>
                                </igtbar:TBLabel>
                                <igtbar:TBCustom Key="CultureFilter" Width="250px">
                                    <asp:DropDownList ID="Culturelist" runat="Server" AutoPostBack="True" DataTextField="Name"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </igtbar:TBCustom>								
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">Error message</asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" TableLayout="Fixed" RowSelectorsDefault="No" Name="dg"
								CellClickActionDefault="RowSelect" NoDataMessage="No container dependencies">
								<%-- Fix for QC# 7384 by Radha S. Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                <%-- Fix for QC# 7386 by Rekha Thomas. Added borderdetails tag to fix the gridlines missing issue --%>
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
                                <%-- Fix for QC# 7384 by Radha S. Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --Start --%>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
                                <%-- Fix for QC# 7384 by Radha S. Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --End --%>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="ContainerDependencies" BorderCollapse="Collapse">
									<Columns>
										<igtbl:UltraGridColumn Width="50px" HeaderText="Count" Key="Count" BaseColumnName="Count"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Width="100%" HeaderText="Value" Key="Value" BaseColumnName="ChunkValue"></igtbl:UltraGridColumn>
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
</asp:Content>