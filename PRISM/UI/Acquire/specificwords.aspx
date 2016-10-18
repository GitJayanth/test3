<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Acquire.SpecificWords" CodeFile="SpecificWords.aspx.cs" Async="true"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Specific words</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" language="JavaScript" src="/hc_v4/js/spell.js" type="text/javascript"></script>
  <script type="text/javascript"language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>
  <script language="javascript" type="text/javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
		      if (oButton.Key == 'Approve')
		      {
		        if (dg_nbItems_Checked==0)
		        {
	            alert('You must select at least one item');
	            oEvent.cancelPostBack = true;
	          }
		      }
				  if (oButton.Key == 'Delete') {
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
				  if (oButton.Key == 'Import') {
    	      var url = 'SpellChecker/SpecificWords_Load.aspx';
    	      var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	      url += '#target';
    	      winContainerEdit = OpenModalWindow(url,'containerwindow', 200, 500, 'no')
			      oEvent.cancelPostBack = true;
			  } 
		  }
		function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dg");
           	 }
	</script>

<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle" height="17">
				<asp:label id="lbTitle" runat="server">List of words</asp:label>
			</td>
		</tr>
<!--
		<tr valign="top">
			<td class="sectionTitle" height="17">
				<asp:label id="lbTitle1" runat="server">List of words in </asp:label>&nbsp;
				<asp:DropDownList id="DDL_Cultures1" runat="server" AutoPostBack="True" DataTextField="Name" DataValueField="Code"></asp:DropDownList></td>
		</tr>-->
        <%--Removed the width property to fix enlarged button issue by Radha S--%>
			<asp:panel id="panelGrid" Runat="server">
				<tr valign="top">
					<td>
						<igtbar:UltraWebToolbar id="uwToolbar" runat="server" ImageDirectory=" " ItemWidthDefault="80px"
							CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Approve" Text="Approve" Image="/hc_v4/img/ed_accept.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Build" Text="Build" Image="/hc_v4/img/ed_build.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="BuildSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Import" Text="Import" Visible="False" Image="/hc_v4/img/ed_import.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Width="0px"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Text="Filter">
									<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="150px" Key="filterField">
									<asp:TextBox Width="150px" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
									<DefaultStyle Width="25px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBarButton ToggleButton="True" Key="NotApproved" SelectedImage="/hc_v4/img/ed_checked.jpg"
									Text="Not approved" Image="/hc_v4/img/ed_unchecked.jpg" Selected="True">
									<SelectedStyle ForeColor="Black"></SelectedStyle>
									<DefaultStyle Width="100px"></DefaultStyle>
								</igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:UltraWebToolbar></td>
				</tr>
				<tr valign="top">
					<td class="main">
						<asp:Label id="lbMessage" runat="server" Visible="False">error message</asp:Label>
						<igtbl:ultrawebgrid id="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
								SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" EnableInternalRowsManagement="True"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
								<Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True"></Pager>
								<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
                <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand>
									<Columns>
										<igtbl:TemplatedColumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
											<CellStyle VerticalAlign="Middle" Wrap="True"></CellStyle>
											<HeaderTemplate>
												<asp:CheckBox id="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
											</HeaderTemplate>
											<CellTemplate>
												<asp:CheckBox id="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
											</CellTemplate>
											<Footer Key="Select" Caption=""></Footer>
											<Header Key="Select"></Header>
										</igtbl:TemplatedColumn>
										<igtbl:TemplatedColumn Key="Text" Width="100%" HeaderText="" BaseColumnName="Text" CellMultiline="Yes">
											<CellTemplate>
												<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
													<%#Container.Text%>
												</asp:LinkButton>
											</CellTemplate>
											<Footer Key="Text">
												<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="Text" Caption="">
												<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
											</Header>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn Key="word" Hidden="True" BaseColumnName="Text">
											<Footer Key="word">
												<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="word">
												<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Owner" Key="UserName" Width="200px" BaseColumnName="SubmitterName">
											<Footer Key="UserName">
												<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="UserName" Caption="Owner">
												<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Submitted" Key="Submitted" Width="120px" DataType="System.DateTime"
											BaseColumnName="SubmitDate">
											<Footer Key="Submitted">
												<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="Submitted" Caption="Submitted">
												<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Approved" Key="Approved" Width="120px" DataType="System.DateTime" BaseColumnName="ApproveDate">
											<Footer Key="Approved">
												<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="Approved" Caption="Approved">
												<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid><br/>
						<center>
							<asp:Label id="lbNoresults" runat="server" ForeColor="Red" Font-Bold="True">No results</asp:Label></center>
					</td>
				</tr>
			</asp:panel>
			<asp:panel id="panelTabWord" Runat="server" visible="false">
				<tr valign="top">
					<td>
						<igtab:ultrawebtab id="webTab" runat="server" width="100%" BarHeight="0" ThreeDEffect="False" SpaceOnRight="0"
							DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%" BorderColor="#949878" BorderWidth="1px" BorderStyle="Solid">
							<DefaultTabStyle Height="21px" Font-Size="xx-small" ForeColor="Black" BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="ig_tab_winXP1.gif"
								NormalImage="ig_tab_winXP3.gif" HoverImage="ig_tab_winXP2.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
							<Tabs>
								<igtab:Tab Key="SpecificWord" DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
									<ContentPane TargetUrl="Word_properties.aspx" BorderStyle="None"></ContentPane>
								</igtab:Tab>
							</Tabs>
						</igtab:ultrawebtab></td>
				</tr>
			</asp:panel>
	</table>
</asp:Content>
