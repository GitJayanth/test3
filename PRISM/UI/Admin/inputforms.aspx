<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.InputForms" CodeFile="InputForms.aspx.cs"%>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input forms</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <script language="javascript" type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
		    if (oButton.Key == 'Overview') 
		    {
          window.location = 'InputFormsOverview.aspx';
          oEvent.cancelPostBack = true;
        }
		}
		
		function Redirect(id)
		{
			document.getElementById("redirectId").value = id;
			document.forms[0].submit();
		} 
		
  </script>

  <input type="hidden" name="redirectId" id="cloneId" value="">
  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" width="100%"
    border="0">
    <tr>
      <td class="sectionTitle">
        <asp:Label ID="lbTitle" runat="server">Input forms list</asp:Label></td>
    </tr>
    <%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
    <asp:Panel ID="panelGrid" runat="server" Visible="True">
      <tr>
        <td>
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ImageDirectory=" " CssClass="hc_toolbar"
            ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <Items>
              <igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Overview" Text="Overview" Visible="False" Image="/hc_v4/img/ed_manage.gif">
                <DefaultStyle Width="120px">
                </DefaultStyle>
              </igtbar:TBarButton>
              <igtbar:TBSeparator Width="0px"></igtbar:TBSeparator>
              <igtbar:TBLabel Text="Filter">
                <DefaultStyle Width="40px" Font-Bold="True">
                </DefaultStyle>
              </igtbar:TBLabel>
              <igtbar:TBCustom ID="TBCustom1" Width="250px" Key="SearchField" runat="server">
                <asp:TextBox Width="250px" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
              </igtbar:TBCustom>
              <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                <DefaultStyle Width="25px">
                </DefaultStyle>
              </igtbar:TBarButton>
            </Items>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
          </igtbar:UltraWebToolbar>
          <tr valign="top">
            <td class="main">
              <igtbl:UltraWebGrid ID="dg" runat="server" Visible="False" Width="100%" OnInitializeRow="dg_InitializeRow">
                <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
                  SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No"
                  Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
                  <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                    </BorderDetails>
                  </HeaderStyleDefault>
                  <FrameStyle Width="100%" CssClass="dataTable">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                  </FrameStyle>
                  <RowAlternateStyleDefault CssClass="uga">
                  </RowAlternateStyleDefault>
                  <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                  </RowStyleDefault>
                </DisplayLayout>
                <Bands>
                  <igtbl:UltraGridBand BaseTableName="InputForms" Key="InputForms" BorderCollapse="Collapse"
                    DataKeyField="InputFormId">
                    <Columns>
                      <igtbl:UltraGridColumn HeaderText="Id" Key="Id" Width="25px" BaseColumnName="Id">
                      </igtbl:UltraGridColumn>
                      <igtbl:TemplatedColumn Key="Name" Width="250px" HeaderText="Name" BaseColumnName="Name"
                        CellMultiline="Yes">
                        <CellTemplate>
                          <asp:LinkButton ID="lnkEdit" OnClick="UpdateGridItem" runat="server">
												<%#DataBinder.Eval(Container, "Text")%>
                          </asp:LinkButton>
                        </CellTemplate>
                      </igtbl:TemplatedColumn>
                      <igtbl:UltraGridColumn HeaderText="Short name" Key="ShortName" Width="150px" BaseColumnName="ShortName">
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description">
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Active" Width="50px" Key="IsActive" Type="CheckBox"
                        DataType="System.Boolean" BaseColumnName="IsActive" AllowUpdate="No">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                      </igtbl:UltraGridColumn>
                    </Columns>
                  </igtbl:UltraGridBand>
                </Bands>
              </igtbl:UltraWebGrid>
              <center>
                <asp:Label ID="lbNoresults" runat="server" Visible="False" Font-Bold="True" ForeColor="Red">No results</asp:Label></center>
    </asp:Panel>
    <asp:Panel ID="panelTabs" runat="server" Visible="False">
      <tr valign="top">
        <td>
						<igtab:ultrawebtab id="webTab" runat="server" DummyTargetUrl="/hc_v4/pleasewait.htm" BorderStyle="Solid"
							BorderWidth="1px" BorderColor="#949878" width="100%" Height="101%" LoadAllTargetUrls="False" BarHeight="0"
							ThreeDEffect="False" DynamicTabs="False" SpaceOnRight="0" DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
							<DefaultTabStyle Height="25px" Font-Size="8pt" Font-Names="Microsoft Sans Serif" ForeColor="Black"
								BackColor="#FEFCFD">
								<Padding Bottom="0px" Top="1px"></Padding>
							</DefaultTabStyle>
							<RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="ig_tab_lightb2.gif"
								NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></RoundedImage>
							<SelectedTabStyle>
								<Padding Bottom="1px" Top="0px"></Padding>
							</SelectedTabStyle>
            <Tabs>
              <igtab:Tab DefaultImage="/hc_v4/img/ed_properties.gif" Text="Properties">
                <ContentPane TargetUrl="./inputforms/inputforms_properties.aspx" Visible="False"
                  BorderWidth="0px" BorderStyle="None">
                </ContentPane>
              </igtab:Tab>
              <igtab:Tab Key="Containers" DefaultImage="/hc_v4/img/ed_containers.gif" Text="Containers">
                <ContentPane TargetUrl="./inputforms/inputforms_containers.aspx" Visible="False"
                  BorderWidth="0px" BorderStyle="None">
                </ContentPane>
              </igtab:Tab>
              <igtab:Tab Key="Usage" DefaultImage="/hc_v4/img/ed_usage.gif" Text="Usage">
                <ContentPane TargetUrl="./containers/inputforms_usage.aspx" Visible="False">
                </ContentPane>
              </igtab:Tab>
              <igtab:Tab Key="PLs" DefaultImage="" Text="Product Lines">
                <ContentPane TargetUrl="./inputforms/inputforms_productlines.aspx" Visible="False">
                </ContentPane>
              </igtab:Tab>
              <igtab:Tab Key="DV" DefaultImage="" Text="Detailed view">
                <ContentPane TargetUrl="./inputforms/inputforms_detailledview.aspx" Visible="False">
                </ContentPane>
              </igtab:Tab>
            </Tabs>
          </igtab:UltraWebTab>
        </td>
      </tr>
    </asp:Panel>
  </table>
</asp:Content>
