<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="userprofile_notifications" CodeFile="userprofile_notifications.aspx.cs" %>
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
		    if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		  }
			</script>
</asp:Content>
<%--Removed width tag from ultrawebtoolbar by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table class="main" cellspacing="0" cellpadding="0" width="100%" border="0">
		<tr valign="top" style="height: 1px">
			<td>
				<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
					<Items>
						<igtbar:TBarButton Text="Save" Image="/hc_v4/img/ed_save.gif" Key="Save"></igtbar:TBarButton>
					</Items>
				</igtbar:ultrawebtoolbar></td>
		</tr>
		<tr valign="top" style="height: 1px">
			<td>
  			<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
  	  </td>
  	</tr>
  	<tr valign="top">
  	  <td>
				<igtbl:ultrawebgrid ID="dg" runat="server" OnInitializeRow="dg_InitializeRow" Width="100%">
			    <DisplayLayout MergeStyles="False" AutoGenerateColumns="False"
				    RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" ActivationObject-AllowActivation="false"
				    NoDataMessage="No input forms attached to this container">
                    <%--Removed css class and added inline property by Radha S--%>
				    <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand"  BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
					    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
				    </HeaderStyleDefault>
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
                <igtbl:UltraGridColumn BaseColumnName="Id" Hidden="True" Key="Id"></igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="AllowNotificationInAdvance" Key="AllowNotificationInAdvance" ServerOnly="True"></igtbl:UltraGridColumn>
                <igtbl:TemplatedColumn Key="Select" Width="20px">
                  <CellTemplate>
										<asp:CheckBox id="g_sd" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                  </CellTemplate>
                  <HeaderTemplate>
										<asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                  </HeaderTemplate>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn BaseColumnName="Name" HeaderText="Name" Key="Name"></igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Description" HeaderText="Description" Key="Description" Width="100%"></igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Delay" Key="Delay" ServerOnly="True"></igtbl:UltraGridColumn>
                <igtbl:TemplatedColumn HeaderText="Type" Key="nDelay" Width="150px">
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
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                </FilterHighlightRowStyle>
              </FilterOptions>
              <AddNewRow View="NotSet" Visible="NotSet">
              </AddNewRow>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:ultrawebgrid>
	    </td>
	  </tr>
	</table>
</asp:Content>