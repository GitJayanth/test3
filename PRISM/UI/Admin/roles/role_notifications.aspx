<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="role_Notifications" CodeFile="role_notifications.aspx.cs"%>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Notifications</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		  } 		
			</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
		<table class="main" cellspacing="0" cellpadding="0">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar"
						ImageDirectory=" ">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif">
							</igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar>
				</td>
			</tr>
			<tr valign="top">
				<td>
                <%--Removed the css property and added inline property in headerstyledefault to fix infragestic issue by Radha S--%>
					<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
								SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" 
								NoDataMessage="No input forms attached to this container">
								<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </FrameStyle>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </RowStyleDefault>
							</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="Notifications" Key="Notifications" BorderCollapse="Collapse" DataKeyField="NotificationId">
								<Columns>
									<igtbl:UltraGridColumn Key="InScope" Width="25px" Type="CheckBox" HeaderClickAction="Select" BaseColumnName="InScope"
											AllowUpdate="Yes">
											<Header ClickAction="Select"></Header>
										</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="NotificationId" Key="NotificationId" Hidden="True" BaseColumnName="NotificationId"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Name" Key="NotificationName" Width="200px" BaseColumnName="NotificationName"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="NotificationDescription"></igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:ultrawebgrid>
				</td>
			</tr>
			</table>
</asp:Content>