<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globalize.TRScopes" CodeFile="TRScopes.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Language scopes</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
		    if (oButton.Key == 'list') {
          back();
          oEvent.cancelPostBack = true;
        }
		    else if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		    else if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
        else if ((oButton.Key == 'Export') || (oButton.Key == 'Add') || (oButton.Key == 'filter')){
					oEvent.cancelPostBack = false;
        }
		  } 
	</script>
	<table class="main" cellspacing="0" cellpadding="0">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">PageTitle</asp:label></td>
		</tr>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<IGTBAR:ULTRAWEBTOOLBAR id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
						<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
						<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
						<ITEMS>
							<IGTBAR:TBARBUTTON Text="Add" Image="/hc_v4/img/ed_new.gif" Key="Add"></IGTBAR:TBARBUTTON>
							<IGTBAR:TBSEPARATOR></IGTBAR:TBSEPARATOR>
							<IGTBAR:TBARBUTTON Text="Export" Image="/hc_v4/img/ed_download.gif" Key="export"></IGTBAR:TBARBUTTON>
							<IGTBAR:TBSEPARATOR></IGTBAR:TBSEPARATOR>
							<IGTBAR:TBLABEL Text="Filter">
								<DEFAULTSTYLE Width="40px" Font-Bold="True"></DEFAULTSTYLE>
							</IGTBAR:TBLABEL>
							<IGTBAR:TBCUSTOM Width="150px" Key="filterField">
								<asp:TextBox id="txtFilter" runat="server" Width="150px" MaxLength="50" cssClass="Search"></asp:TextBox>
							</IGTBAR:TBCUSTOM>
							<IGTBAR:TBARBUTTON Image="/hc_v4/img/ed_search.gif" Key="filter">
								<DEFAULTSTYLE Width="25px"></DEFAULTSTYLE>
							</IGTBAR:TBARBUTTON>
						</ITEMS>
						<CLIENTSIDEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIDEEVENTS>
					</IGTBAR:ULTRAWEBTOOLBAR></td>
			</tr>
			<tr valign="top">
				<td>
					<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
							<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="TRScopes" Key="TRScopes" BorderCollapse="Collapse" DataKeyField="TRScopeId">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="40px" BaseColumnName="Id" ServerOnly="true">
										<Footer Key="Id"></Footer>
										<Header Key="Id" Caption="Id"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="150px" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<Footer Key="Name">
											<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="Name" Caption="Name">
											<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
										</Header>
										
									</igtbl:TemplatedColumn>
									<%--Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009--%>
									<igtbl:UltraGridColumn HeaderText="Region" Key="Icon" Width="50%" BaseColumnName="RegionCode"></igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Comment" Key="Comment"  Width="100%" BaseColumnName="Comment"></igtbl:UltraGridColumn>
									
									
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid></td>
			</tr>
		</asp:panel>
		<asp:panel id="panelEdit" runat="server" Visible="False">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolBarEdit" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<Items>
							<igtbar:TBarButton Key="list" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
						</Items>
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<tr valign="top" style="height:1px">
				<td><br/>
					<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label></td>
			</tr>
			<tr valign="top">
				<td>
					<table cellspacing="0" cellpadding="0" width="100%">
						<asp:panel id="PanelId" RunAt="server">
							<TBODY>
								<tr valign="middle">
									<td class="editLabelCell" width="55">
										<asp:label id="lbTRScopeId" runat="server">Id</asp:label></td>
									<td class="ugd">
										<IGTXT:WEBNUMERICEDIT id="wneScopeId" runat="server" width="50px" Enabled="False" ValueText="-1" MinValue="0"
											DataMode="Int"></IGTXT:WEBNUMERICEDIT></td>
								</tr>
						</asp:panel>
                        						
						<tr valign="middle">
							<td class="editLabelCell" width="55">
								<asp:label id="label1" runat="server">Name</asp:label></td>
							<td class="uga">
								<asp:textbox id="txtName" runat="server" Visible="true" width="180px"></asp:textbox>
								<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" CssClass="errorMessage" ErrorMessage="*"
									ControlToValidate="txtName"></asp:requiredfieldvalidator></td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="55">
								<asp:label id="Label2" runat="server">Comment</asp:label></td>
							<td class="uga">
								<asp:textbox id="txtComment" runat="server" Visible="true" width="180px"></asp:textbox></td>
						</tr>

                        <%--Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009--%>
                        <asp:panel id="PanelRegionDDL" RunAt="server">				
						<tr valign="middle">
						    <td class="editLabelCell" width="55"> 
						        <asp:Label id ="Label3" RunAt="server">Region</asp:Label></td>
						    <td class="uga">
						        <asp:DropDownList id="ddRegions" RunAt="server" Visible="true" width="180px" OnSelectedIndexChanged="ddRegions_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
						</tr>
						</asp:panel>
						<asp:panel id="PanelRegionReadonly" RunAt="server">
							<TBODY>
								<tr valign="middle">
									<td class="editLabelCell" width="55">
										<asp:label id="Label5" runat="server">Region</asp:label></td>
									<td class="ugd">
										<IGTXT:WEBTEXTEDIT id="wteRegionCode" runat="server" width="180px" Enabled="False" DataMode="Text"></IGTXT:WEBTEXTEDIT></td>
								</tr>
						</asp:panel>
						<%--Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009--%>
						<tr valign="middle">
							<td class="editLabelCell" width="55">Languages</td>
							<td class="ugd">
								<asp:checkboxlist id="cblLanguageScope" runat="server" Width="100%" DataValueField="Code" DataTextField="Name"
									cellpadding="0" cellspacing="0" RepeatColumns="4"></asp:checkboxlist></td>
						</tr>
					</table>
				</td>
			</tr>
		</asp:panel></table>
</asp:Content>
