<%@ Reference Page="~/ui/globalize/termbase/term_translationedit.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globalize.Termbase" CodeFile="Termbase.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Termbase</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script type="text/javascript">
		function Redirect(id)
		{
			document.getElementById("redirectId").value = id;
			document.forms[0].submit();
		} 
  	function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
		  if (oButton.Key=="Export")
		  {
			  oEvent.cancelPostBack=true;
			  showHideAdvancedToolBar();
		  }
	  }
  	
	  function showHideAdvancedToolBar()
	  {
		  var toolbar = igtbar_getToolbarById('<%= advancedToolBar.ClientID %>');
		  if (toolbar!=null)
		  {
		    var display = toolbar.Element.style.display=='none'?'':'none';;
			  toolbar.Element.style.display = display;
			  var errorMsgLbl = document.getElementById('<%= errorMsg.ClientID %>');
			  if (errorMsgLbl != null)
			    errorMsgLbl.style.display = display;
			}
  	}
//-->		
		  			  
	</script>
	  <input type="hidden" name="redirectId" id="cloneId" value="">
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top">
			<td class="sectionTitle">
				<asp:label id="lbTitle" runat="server" Height="17px">List of terms</asp:label></td>
		</tr>
		<asp:panel id="panelGrid" Runat="server">
			<tr valign="top">
				<td>
					<igtbar:UltraWebToolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
         <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif" ToggleButton="true"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Type">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="80px">
								<asp:DropDownList id="DDL_TermTypeList" DataTextField="Name" DataValueField="Code" Width="80px" runat="server"
									AutoPostBack="True"></asp:DropDownList>
							</igtbar:TBCustom>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Filter">
								<DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox id="txtFilter" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="All" Text="All">
								<DefaultStyle Width="20px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="[0-9]" Text="0-9">
								<DefaultStyle Width="25px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="A" Text="A">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="B" Text="B">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="C" Text="C">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="D" Text="D">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="E" Text="E">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="F" Text="F">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="G" Text="G">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="H" Text="H">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="I" Text="I">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="J" Text="J">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="K" Text="K">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="L" Text="L">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="M" Text="M">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="N" Text="N">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="O" Text="O">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="P" Text="P">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="Q" Text="Q">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="R" Text="R">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="S" Text="S">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="T" Text="T">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="U" Text="U">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="V" Text="V">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="W" Text="W">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="X" Text="X">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="Y" Text="Y">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBarButton Key="Z" Text="Z">
								<DefaultStyle Width="10px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Settings" Text="Settings" Image="/hc_v4/img/ed_parameters.gif"></igtbar:TBarButton>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:UltraWebToolbar>
					<igtbar:ultrawebtoolbar id=advancedToolBar runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="advancedToolBar_ButtonClicked">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="TotalExport" Text="Complete export" Image="/hc_v4/img/ed_download.gif">
								<DefaultStyle Width="120px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBLabel Text="Validation report:">
								<DefaultStyle Font-Bold="True" Width="120px"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBLabel Text="since">
								<DefaultStyle Width="50px"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="120px" Key="startDate">
								<igsch:WebDateChooser id="startDate" runat="server" NullDateLabel="Not set" Editable="false"></igsch:WebDateChooser>
							</igtbar:TBCustom>
							<igtbar:TBarButton Key="RunValidationReport" Text="Apply" Image="/hc_v4/img/ed_build.gif"></igtbar:TBarButton>
							<igtbar:TBCustom>
							  <a onmouseover="doTooltip(event,'<table width=&quot;350px&quot;><tr><td>To download the report file, you may be required to modify Internet Explorer settings:<BR/><li>Under menu: &quot;Tools&quot;/&quot;Internet options...&quot;</li><li>Go to the &quot;Security&quot; tab.</li><li>Click on &quot;Custom Level...&quot; button.</li><li>In &quot;Downloads&quot; section, enable &quot;Automatic prompting for file downloads&quot;.</li></td></tr></table>')" onmouseout="hideTip()" href="javascript://" style="border: 0px">
			            <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
			          </a>
							</igtbar:TBCustom>
							<igtbar:TBLabel Text=" ">
								<DefaultStyle Width="100%"></DefaultStyle>
							</igtbar:TBLabel>
						</Items>
						<ClientSideEvents InitializeToolbar = "showHideAdvancedToolBar();"/>
					</igtbar:ultrawebtoolbar>
					<asp:Label id="errorMsg" runat="server" ForeColor="Red" Font-Bold="True" Visible="false"></asp:Label>
			  </td>
			</tr>
			<tr valign="top">
				<td class="main">
					<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
							SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" EnableInternalRowsManagement="True"
							RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
							<Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True"></Pager>
							 <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						</DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand>
								<Columns>
									<igtbl:UltraGridColumn HeaderText="" Key="TermId" Hidden="True" BaseColumnName="Id">
										<Footer Key="TermId"></Footer>
										<Header Key="TermId" Caption=""></Header>
									</igtbl:UltraGridColumn>
									<igtbl:TemplatedColumn Key="Name" Width="100%" HeaderText="" BaseColumnName="Value" CellMultiline="Yes">
										<CellTemplate>
											<asp:LinkButton id="Linkbutton1" onclick="UpdateGridItem" runat="server">
												<%#Container.Text%>
											</asp:LinkButton>
										</CellTemplate>
										<CellStyle Wrap="true"></CellStyle>
									</igtbl:TemplatedColumn>
									<igtbl:UltraGridColumn HeaderText="Type" Key="TermTypeName" Width="80px" BaseColumnName="TermTypeName" CellMultiline="Yes">
										<CellStyle HorizontalAlign="Center" Wrap="True"></CellStyle>
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Owner" Key="UserName" Width="150px" BaseColumnName="UserName">
										<CellStyle HorizontalAlign="Center"></CellStyle>
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									</igtbl:UltraGridColumn>
									<igtbl:UltraGridColumn HeaderText="Modify date" Key="ModifyDate" Width="100px" BaseColumnName="ModifyDate">
										<CellStyle HorizontalAlign="Center"></CellStyle>
										<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									</igtbl:UltraGridColumn>
								</Columns>
							</igtbl:UltraGridBand>
						</Bands>
					</igtbl:ultrawebgrid><br/>
					<center>
						<asp:Label id="lbNoresults" runat="server" ForeColor="Red" Font-Bold="True">No results</asp:Label>
					</center>
				</td>
			</tr>
		</asp:panel>
		<asp:panel id="panelTabTerm" Runat="server" visible="false">
			<tr valign="top">
				<td>
					<igtab:UltraWebTab id="webTab" runat="server" Width="100%" Height="100%" DummyTargetUrl="/hc_v4/pleasewait.htm"
						LoadAllTargetUrls="False" BorderWidth="1px" BorderStyle="Solid" BorderColor="Gray">
						<DefaultTabStyle Height="25px" BackColor="WhiteSmoke"></DefaultTabStyle>
						<RoundedImage SelectedImage="ig_tab_lightb2.gif" NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
						<Tabs>
							<igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
								<ContentPane TargetUrl="./Termbase/Term_properties.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab DefaultImage="/hc_v4/img/ed_translate.gif" Text="Translations" Key="Translations">
								<ContentPane TargetUrl="./Termbase/Term_translations.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
							<igtab:Tab DefaultImage="/hc_v4/img/ed_containers.gif" Text="Containers">
								<ContentPane TargetUrl="./Termbase/Term_containers.aspx" Visible="False"></ContentPane>
							</igtab:Tab>
						</Tabs>
					</igtab:UltraWebTab></td>
			</tr>
		</asp:panel>
		<asp:panel id="panelTabSettings" Runat="server" visible="false">
			<tr valign="top">
				<td>
					<igtab:UltraWebTab id="webTabSettings" runat="server" Width="100%" Height="100%" DummyTargetUrl="/hc_v4/pleasewait.htm"
						LoadAllTargetUrls="False" BorderWidth="1px" BorderStyle="Solid" BorderColor="Gray">
						<DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
						<ROUNDEDIMAGE FillStyle="LeftMergedWithCenter" NormalImage="ig_tab_lightb1.gif" SelectedImage="ig_tab_lightb2.gif"></ROUNDEDIMAGE>
						<TABS>
							<igtab:Tab Text="TermType languages" DefaultImage="/hc_v4/img/ed_properties.gif">
								<CONTENTPANE TargetUrl="./Termbase/Setting_TermTypeCultures.aspx"></CONTENTPANE>
							</igtab:Tab>
						</TABS>
					</igtab:UltraWebTab></td>
			</tr>
		</asp:panel></table>
		<script src="/hc_v4/js/hc_tooltip.js" type="text/javascript"></script>
</asp:Content>
