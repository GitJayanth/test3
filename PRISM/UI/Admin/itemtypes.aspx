<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.ItemTypes" CodeFile="ItemTypes.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Item types</asp:Content>
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
        else if ((oButton.Key == 'Export') || (oButton.Key == 'Add') || (oButton.Key == 'Filter')){
					oEvent.cancelPostBack = false;
        }
		  } 
		  
	</script>
	<table class="main" cellspacing="0" cellpadding="0">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server">PageTitle</asp:label></td>
		</tr>
        <%--Removed the width property to fix enlarge button issue by Radha S--%>
		<asp:panel id="panelGrid" runat="server" Visible="True">
			<tr valign="top" style="height:1px">
				<td>
					<IGTBAR:ULTRAWEBTOOLBAR id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
						<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
						<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
						<ITEMS>
							<IGTBAR:TBARBUTTON Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></IGTBAR:TBARBUTTON>
							<IGTBAR:TBSEPARATOR Key="AddSep"></IGTBAR:TBSEPARATOR>
							<IGTBAR:TBARBUTTON Text="Export" Image="/hc_v4/img/ed_download.gif" Key="export"></IGTBAR:TBARBUTTON>
							<IGTBAR:TBSEPARATOR></IGTBAR:TBSEPARATOR>
							<IGTBAR:TBLABEL Text="Filter">
								<DEFAULTSTYLE Width="40px" Font-Bold="True"></DEFAULTSTYLE>
							</IGTBAR:TBLABEL>
							<IGTBAR:TBCUSTOM Width="150px" Key="filterField">
								<asp:TextBox runat="server" id="txtFilter" runat="server" Width="150px" MaxLength="50" cssClass="Search"></asp:TextBox>
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
							<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="ItemTypes" Key="ItemTypes" BorderCollapse="Collapse" DataKeyField="ItemTypeId">
								<Columns>
									<igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="40px" BaseColumnName="Id">
										<Footer Key="Id"></Footer>
										<Header Key="Id" Caption="Id"></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="100%" HeaderText="Name" BaseColumnName="Name" CellMultiline="Yes">
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
									<igtbl:UltraGridColumn HeaderText="Icon" Key="Icon" Width="110px" BaseColumnName="Icon">
										<Footer Key="Icon">
											<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
										</Footer>
										<Header Key="Icon" Caption="Icon">
											<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
										</Header>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:UltraWebGrid>
					<CENTER>
						<asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label></CENTER>
				</td>
			</tr>
		</asp:panel>
		<asp:panel id="panelEdit" runat="server" Visible="False">
			<tr valign="top" style="height:1px">
				<td>
					<igtbar:ultrawebtoolbar id="uwToolBarEdit" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<Items>
							<igtbar:TBarButton Key="list" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
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
						<tr valign="middle">
							<td class="editLabelCell" width="55">
								<asp:label id="lbItemTypeId" runat="server">Id</asp:label></td>
							<td class="ugd">
								<IGTXT:WEBNUMERICEDIT id="wneTypeId" runat="server" width="50px" DataMode="Int" MinValue="0" ValueText="-1"></IGTXT:WEBNUMERICEDIT></td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="55">
								<asp:label id="label1" runat="server">Name</asp:label></td>
							<td class="uga">
								<asp:textbox id="txtTypeName" runat="server" Visible="true" width="180px"></asp:textbox>
								<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" CssClass="errorMessage" ControlToValidate="txtTypeName"
									ErrorMessage="*"></asp:requiredfieldvalidator></td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="55">
								<asp:label id="Label3" runat="server">Icon</asp:label></td>
							<td class="ugd">
								<asp:textbox id="txtIcon" runat="server" Visible="true" width="180px"></asp:textbox></td>
						</tr>
					</table>
				</td>
			</tr>
		</asp:panel>
	</table>
</asp:Content>
