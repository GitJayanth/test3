<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="user_notifications" CodeFile="user_notifications.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Notifications</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          oEvent.cancelPostBack = true;
          back();
        }
		  }
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving icon issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr height="1" valign="top">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Text="Save" Image="/hc_v4/img/ed_save.gif" Key="Save"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top">
					<td>
      			<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
		
							<igtbl:ultrawebgrid ID="dg"  runat="server" ImageDirectory="/ig_common/Images/" Width="100%" OnInitializeRow="dg_InitializeRow" Height="200px">
                <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
                  HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                  CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
                  <%--Removed css style and added inline property for grid by Radha S--%>
                  <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                  </HeaderStyleDefault>
                  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler"></ClientSideEvents>
                  <FrameStyle Width="100%" CssClass="dataTable">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                  </FrameStyle>
                  <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
                  <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
                </DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BorderCollapse="Collapse" DataKeyField="Id">
										<Columns>
                      <igtbl:UltraGridColumn BaseColumnName="Id" Hidden="True" Key="Id">
                        <Header>
                          <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn BaseColumnName="AllowNotificationInAdvance" Key="AllowNotificationInAdvance" ServerOnly="true"/>
                      <igtbl:TemplatedColumn FooterText="" Key="Select" Width="20px">
                        <CellTemplate>
													<asp:CheckBox id="g_sd" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                        </CellTemplate>
                        <HeaderTemplate>
													<asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                        </HeaderTemplate>
                        <Header>
                          <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <CellStyle Wrap="True">
                        </CellStyle>
                        <Footer Caption="">
                          <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                      </igtbl:TemplatedColumn>
                      <igtbl:UltraGridColumn BaseColumnName="Name" Key="Name">
                        <Header Caption="Name">
                          <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn BaseColumnName="Description" HeaderText="Description" Key="Description" Width="100%">
                        <Header Caption="Description">
                          <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                          <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                      </igtbl:UltraGridColumn>
                      <igtbl:TemplatedColumn HeaderText="Delay" Key="Delay" Width="150px">
                        <Header Caption="Type"></Header>
                        <CellTemplate>
										      <asp:DropDownList Width="150px" id="ddDelay" runat="server"></asp:DropDownList>
                        </CellTemplate>
                      </igtbl:TemplatedColumn>
										</Columns>
                    <FilterOptions AllString="" EmptyString="" NonEmptyString="">
                      <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                        CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px"
                        Width="200px">
                        <Padding Left="2px" />
                      </FilterDropDownStyle>
                      <FilterHighlightRowStyle BackColor="#151C55" ForeColor="#FFFFFF">
                      </FilterHighlightRowStyle>
                    </FilterOptions>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
			

			    </td>
			  </tr>
			</table>
</asp:Content>