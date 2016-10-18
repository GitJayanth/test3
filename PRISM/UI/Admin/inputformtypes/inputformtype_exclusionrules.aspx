<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="inputformtype_exclusionrules" CodeFile="inputformtype_exclusionrules.aspx.cs" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Exclusion rules</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
		  function SC(inputFormTypeCode, containerTypeCode)
		  {
    	  var url = "InputFormType_ExclusionRuleEdit.aspx?i="+inputFormTypeCode+"&c="+containerTypeCode;
        OpenModalWindow(url, "popupExclusionRule", 200, 550, "true");
		  }
		  var hInputFormTypeCodeId;

			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
		    if (oButton.Key == 'List') 
		    {
          back();
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Add') 
		    {
					var inputFormTypeCode = document.getElementById(hInputFormTypeCodeId).value;
					SC(inputFormTypeCode, '');
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Delete') 
		    {
          if (dg_nbItems_Checked==0)
		      {
	          alert('You must select at least one item');
	          oEvent.cancelPostBack = true;
	        }
	        else
	        {
            oEvent.cancelPostBack = !confirm("Are you sure?");
          }
        }
		  } 

		  var action;
		  function UpdateDocument()
		  {
		    if (document.getElementById(action))
		    {
		  	  document.getElementById(action).value="update";
				  document.forms[0].submit();
				}
		  }
	  </script>
</asp:Content>
<%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ImageDirectory=" " ItemWidthDefault="80px"
							CssClass="hc_toolbar">
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
								<igtbar:TBSeparator Key="ExportSep"></igtbar:TBSeparator>
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
					<td><igtbl:ultrawebgrid id="dg" runat="server" ImageDirectory="/ig_common/Images/" UseAccessibleHeader="False">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
								HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
								CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
                                <%--removed the css class and added the styles inline--Start%>--%>
							<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </FrameStyle>
                                <%--<%--removed the css class and added the styles inline--End--%>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand Key="ExclusionRules" BorderCollapse="Collapse">
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
										<igtbl:UltraGridColumn Key="InputFormTypeCode" Width="10px" Hidden="True" BaseColumnName="InputFormTypeCode"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Code" Key="ContainerTypeCode" Width="15%" BaseColumnName="ContainerTypeCode">
											<CellStyle HorizontalAlign="Center"></CellStyle>
											<Footer Key="ContainerTypeCode"></Footer>
											<Header Key="ContainerTypeCode" Caption="Code"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Container type" Key="ContainerType" Width="85%" BaseColumnName="ContainerTypeName">
											<Footer Key="ContainerType"></Footer>
											<Header Key="ContainerType" Caption="Container type"></Header>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
					</td>
				</tr>
			</table>
			<input type="hidden" id="hAction" runat="server" name="action" /> 
			<input type="hidden" id="hInputFormTypeCode" runat="server" name="inputFormTypeCode" />
</asp:Content>