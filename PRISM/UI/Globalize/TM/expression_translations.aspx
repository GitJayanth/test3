<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Expression_Translation" CodeFile="Expression_Translations.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">TM expression translations</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript">
		
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
					oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
      }
      
			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

		<table border="0" cellpadding="0" cellspacing="0" class="datatable">
			<tr valign="bottom" style="height:auto">
				<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Save" Text="Apply changes" Image="/hc_v4/img/ed_save.gif">
							<DefaultStyle Width="150px"></DefaultStyle></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Delete" Text="Delete selected" Image="/hc_v4/img/ed_delete.gif">
								<DefaultStyle Width="120px"></DefaultStyle>
							</igtbar:TBarButton>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<tr valign="top">
				<td>
					<asp:label id="lbMessage" runat="server" Width="100%" Visible="false">Message</asp:label><br/>
					<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" EnableInternalRowsManagement="True"
							RowSelectorsDefault="No" SelectTypeCellDefault="Single" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
						 <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
						  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
              <FilterOptionsDefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                  CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px"
                  Width="200px">
                  <Padding Left="2px" />
                </FilterDropDownStyle>
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="#FFFFFF">
                </FilterHighlightRowStyle>
              </FilterOptionsDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand>
								<Columns>
									<igtbl:UltraGridColumn Key="ExpressionId" Width="30px" Hidden="True" BaseColumnName="Id">
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn Key="LanguageCode" Width="10px" Hidden="True" BaseColumnName="LanguageCode">
										<Footer>
                      <RowLayoutColumnInfo OriginX="1" />
                    </Footer>
										<Header>
                      <RowLayoutColumnInfo OriginX="1" />
                    </Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Select" Width="20px" FooterText="">
										<CellStyle VerticalAlign="Middle" Wrap="True"></CellStyle>
										<HeaderTemplate>
											<asp:CheckBox id="g_ca" onclick="javascript:return g_su(this);"
												runat="server"></asp:CheckBox>
										</HeaderTemplate>
										<CellTemplate>
											<asp:CheckBox id="g_sd" onclick="javascript:return g_su(this);"
												runat="server"></asp:CheckBox>
										</CellTemplate>
										<Footer Caption="">
                      <RowLayoutColumnInfo OriginX="2" />
                    </Footer>
										<Header>
                      <RowLayoutColumnInfo OriginX="2" />
                    </Header>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Language" Key="Language" Width="100px" BaseColumnName="LanguageName">
										<CellStyle VerticalAlign="Middle"></CellStyle>
                    <Header Caption="Language">
                      <RowLayoutColumnInfo OriginX="3" />
                    </Header>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="3" />
                    </Footer>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn Key="Rtl" Hidden="True" BaseColumnName="Rtl">
										<Footer>
                      <RowLayoutColumnInfo OriginX="4" />
                    </Footer>
										<Header>
                      <RowLayoutColumnInfo OriginX="4" />
                    </Header>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn ServerOnly="True" Key="TMExpressionValue" BaseColumnName="Value">
                    <Header>
                      <RowLayoutColumnInfo OriginX="5" />
                    </Header>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="5" />
                    </Footer>
                  </igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn HeaderText="Value" Key="Value" width="100%">
										<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
										<CellTemplate>
											<asp:TextBox id="TXTChangedValue" runat="server" TextMode="MultiLine" Rows="3" Columns="80"></asp:TextBox>
										</CellTemplate>
										<Header Caption="Value">
                      <RowLayoutColumnInfo OriginX="6" />
                    </Header>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="6" />
                    </Footer>
									</igtbl:TemplatedColumn>
								</Columns>
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
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