<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="InputForms_ContainersBatchAdd.aspx.cs" Inherits="UI_Admin_InputForms_InputForms_ContainersBatchAdd"%>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Container edit properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">		
		  function ReloadParent()
		  {
		    top.opener.document.getElementById("action").value = "reload";
        top.opener.document.forms[0].submit();
		  }
		  	  
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
        if (oButton.Key == 'Close'){
					window.close();
					oEvent.cancelPostBack = true;
        }
        if (oButton.Key == 'Add'){
          if (dg_nbItems_Checked == 0)
          {
   		      alert('Please, select at least one row');
   		      oEvent.cancelPostBack = true;
   		    }
        }
			}
      
      function Lock(obj){
        cb = obj.id;
        cb = cb.replace("ctl00_HOCP_dg_ci_0_4", "ctl00_HOCP_dg_ci_0_3");
        cb = cb.replace("_g_m", "_g_sd");
        if (document.getElementById(cb))
        {
          document.getElementById(cb).checked = obj.checked;
          if (obj.checked)
            dg_nbItems_Checked ++;
          else
            dg_nbItems_Checked --;
          
//          return g_su(cb);
        }
      }
			</script>
</asp:Content>
<%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<table class="main" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top" style="height:1px">
			<td>
				<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="25px" OnButtonClicked="uwToolbar_ButtonClicked">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="Add" Text="Add Selected" ToolTip="Add selected containers" Image="/hc_v4/img/ed_new.gif">
						<DEFAULTSTYLE WIDTH="120px"></DEFAULTSTYLE></igtbar:TBarButton>
						<igtbar:TBSeparator Key="CloseSep"></igtbar:TBSeparator>
						<igtbar:TBarButton Key="Close" Text="Close" ToolTip="Close window" Image="/hc_v4/img/ed_cancel.gif">
						<DEFAULTSTYLE WIDTH="80px"></DEFAULTSTYLE></igtbar:TBarButton>
              <igtbar:TBSeparator Key="FilterSep"></igtbar:TBSeparator>
              <igtbar:TBLabel Text="Filter">
                <DEFAULTSTYLE WIDTH="40px" FONT-BOLD="True"></DEFAULTSTYLE>
              </igtbar:TBLabel>
              <igtbar:TBCustom Width="150px" Key="filterField" runat="server">
                <ASP:TEXTBOX Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50" runat="server"></ASP:TEXTBOX>
              </igtbar:TBCustom>
              <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                <DEFAULTSTYLE WIDTH="25px"></DEFAULTSTYLE>
              </igtbar:TBarButton>
					</Items>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="middle" style="height:1px">
			<td align="left" class="editLabelCell" style="height: 1px"><b>Choose a Group: </b>&nbsp;<asp:DropDownList ID="ddlGroups" runat="server" OnSelectedIndexChanged="ddlGroups_SelectedIndexChanged" AutoPostBack="True"/><asp:Label ID="lbNbContainers" runat="server"></asp:Label><br /><br /></td>
		</tr>
		<tr valign="top" style="height:1px">
			<td align="center">
			  <asp:label id="lbResult" runat="server" CssClass="hc_success" Visible="False">No result</asp:label>
			</td>
		</tr>
		<tr valign="top" style="height:1px">
		  <td>
				  <asp:Label ID="lbError" Runat="server" Visible="False" CssClass="hc_error"></asp:Label>
		  </td>
		</tr>
		<tr valign="top">
			<td>
			<asp:Panel runat="server" ID="panelExplain" Visible="false">
			Select a container group in the drop down list. <br />
			The eligible containers will then be displayed in a grid and you'll be able to select the ones that apply for the current input form (e.g: respecting the input form type exclusion rules).<br /><br />
			For each of the container, you'll have to indicate if it is mandatory and if its type is either<li>normal</li><li>Single Choice</li><li>Multi Choice</li>
			</asp:Panel>
				<div id="divDg" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
					<igtbl:ultrawebgrid ID="dg"  runat="server" ImageDirectory="/ig_common/Images/" Width="100%" OnInitializeRow="dg_InitializeRow" Height="100%">
            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
              HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" ActivationObject-AllowActivation="false"
              CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
              <%--removed the css class and added the styles inline--Start%>--%>
							<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true">  <%--CssClass="gh">--%>
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                            </FrameStyle>
                            <%--removed the css class and added the styles inline--End%>--%>
              <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
						  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
              <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
            </DisplayLayout>
						<Bands>
							<igtbl:UltraGridBand BaseTableName="Containers" Key="Containers" BorderCollapse="Collapse" DataKeyField="Id">
								<Columns>
                  <igtbl:UltraGridColumn Hidden="True" Key="Index">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn BaseColumnName="Id" Hidden="True" Key="Id"></igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn BaseColumnName="LookupId" Hidden="True" Key="LookupId"></igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn FooterText="" Key="Select" Width="20px">
                    <CellTemplate>
											<asp:CheckBox id="g_sd" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                    </CellTemplate>
                    <HeaderTemplate>
											<asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                    </HeaderTemplate>
                    <CellStyle Wrap="True">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                  <igtbl:TemplatedColumn HeaderText="M" Key="Mandatory" Width="20px">
                    <CellTemplate>
											<asp:CheckBox id="g_m" runat="server" OnClick="Lock(this)"></asp:CheckBox>
                    </CellTemplate>
                    <CellStyle Wrap="True">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                <igtbl:ultragridcolumn HeaderText="R" Key="isReg" Width="25px" Type="CheckBox" BaseColumnName="Regionalizable"
                  AllowUpdate="No">
                  <cellstyle HorizontalAlign="Center"></cellstyle>
                </igtbl:ultragridcolumn>
                  <igtbl:UltraGridColumn BaseColumnName="Tag" Key="Tag" ServerOnly="True">
                    <Header>
                      <RowLayoutColumnInfo OriginX="4" />
                    </Header>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="4" />
                    </Footer>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn BaseColumnName="Id" Key="Id" ServerOnly="True">
                    <Header>
                      <RowLayoutColumnInfo OriginX="5" />
                    </Header>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="5" />
                    </Footer>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn BaseColumnName="Name" CellMultiline="Yes" HeaderText="Container"
                    Key="ContainerName" Width="100%">
                    <Header Caption="Container">
                      <RowLayoutColumnInfo OriginX="6" />
                    </Header>
                    <CellStyle Wrap="True">
                    </CellStyle>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="6" />
                    </Footer>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn AllowUpdate="Yes" HeaderText="Comment" Key="Comment" Width="150px">
                    <Header Caption="Comment">
                      <RowLayoutColumnInfo OriginX="7" />
                    </Header>
                    <Footer>
                      <RowLayoutColumnInfo OriginX="7" />
                    </Footer>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn AllowUpdate="Yes" HeaderText="Type" Key="Type" ServerOnly="true">
                  </igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn  HeaderText="Type" Key="ifcType" Width="150px">
                    <CellTemplate>
											<asp:DropDownList Width="150px" id="ddType" runat="server"></asp:DropDownList >
                    </CellTemplate>
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
				</div>
					<asp:dropdownlist id="DDL_InputType" runat="server"></asp:dropdownlist>
			</td>
		</tr>
	</table>
</asp:Content>