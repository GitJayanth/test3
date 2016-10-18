<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" CodeFile="NewModifiedContentReport.aspx.cs" Inherits="UI_Collaborate_NewModifiedContentReport" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ucs" TagName="PLWebTree" Src="../Admin/PLWebTree.ascx" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
 New/Modified Content Report</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <script type="text/javascript" language="javascript">
        function OpenDetail(id, c, f, p)
      {
	      var url = 'NewModifiedContentDetail.aspx?i='+id+'&c='+c+'&f='+f+'&p='+p; 
        winDetail = OpenModalWindow(url, "cr", 500, 800, 'yes');
      }
  </script>

  <table class="main" cellspacing="0" cellpadding="0" width="100%">
    <tr valign="top">
      <td class="sectionTitle">
          <table border="0">
            <tr valign="middle">
              <td valign="bottom">
              <asp:Label ID="Label2" runat="server">Catalogs </asp:Label>
              <asp:DropDownList ID="DDL_Cultures" runat="server" Width="200px" DataTextField="Name" DataValueField="Code">
              </asp:DropDownList>
              </td>
              <td>Filter
              <asp:DropDownList ID="DDL_Filter" runat="server" Width="160px">
              <asp:ListItem Value="0">Full Content</asp:ListItem>
              <asp:ListItem Value="1">New/Modified content</asp:ListItem>
              <asp:ListItem Value="2">Proposed Content Updated</asp:ListItem>
              </asp:DropDownList>
              </td>
              <td>
              Include 
              <asp:DropDownList ID="DDL_Days" runat="server" Width="40px">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
              </asp:DropDownList> day(s)
              </td>
              <td align="left" valign="bottom"><asp:Button ID="btSearch" runat="server" Text="Generate" OnClick="btSearch_Click"></asp:Button></td>              
            </tr>
            <tr>
              <td colspan="5">
                <asp:Label ID="lbMessage" runat="server" CssClass="hc_error" Text="Label"></asp:Label></td>
            </tr>
          </table>
      </td>
    </tr>
    <tr valign="top">
      <td class="sectionTitle" >
        <asp:Label ID="Label1" runat="server">Results</asp:Label>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
          CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
            </igtbar:TBarButton>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
        </td>
     </tr>
     <tr valign="top">
        <td>
        <igmisc:webpanel id="wPanelPL" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img" Width="100%">
				<Header Text="PLSelection">
					<ExpandedAppearance>
                        <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
				    <ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
                    <CollapsedAppearance>
                        <Style CssClass="hc_webpanelcol"></Style>
                    </CollapsedAppearance>
				</Header>
          <Template>
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr valign="top">
                    <td class="editLabelCell" style="width: 80px">
                        <asp:Label ID="Label7" runat="server">Product Lines</asp:Label>
                    </td>
                    <td class="uga">
                       <ucs:PLWebTree id="PLTree" runat="server" Expanded="false"/>
                    </td>
                </tr>
            </table>
            </Template>
        </igmisc:webpanel>
        </td>
    </tr>
     <tr valign="top">
      <td>
        <igtbl:UltraWebGrid ID="dgResults" runat="server" ImageDirectory="/ig_common/Images/"
          Width="100%" OnInitializeRow="dgResults_InitializeRow">
          <DisplayLayout MergeStyles="False" ViewType="Hierarchical" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dgResults" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
             <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                <%-- Fix for QC# 7386 by Nisha Verma Added borderdetails tag to fix the gridlines missing issue --%>
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
                                <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --Start --%>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
                                <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the RowAlternateStyleDefault and adding BorderDetails  tag to fix the issue --End --%>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand Key="Average">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ClassId" Hidden="True" Key="ClassId">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ClassName" HeaderText="Class" Key="ClassName"
                  Width="200px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="True">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" HeaderText="Product Name" Key="ItemName"
                  Width="100%">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemNumber" HeaderText="Product Number" Key="ItemNumber" Width="120px">
                <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>
		<igtbl:UltraGridColumn Key="PLCode"  BaseColumnName="PLCode" HeaderText ="PLCode" CellMultiline="Yes" Width ="120px" >
		<CellStyle Wrap="true"  HorizontalAlign = "Center"></CellStyle>
    		</igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="WorkflowStatus" HeaderText="Workflow status" Key="WorkflowStatus" 
                  Width="100px">
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Workflow status">
                  </Header>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ContentStatus" HeaderText="Status" Key="ContentStatus"
                  Width="70px">
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SourceNewest" Hidden="True" Type="CheckBox" HeaderText="Source newest" Key="SourceNewest"
                  Width="50px">
                  <HeaderStyle Wrap="True" />
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                  <Header Caption="Source newest">
                  </Header>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ModifyDate" HeaderText="ModifyDate" Key="ModifyDate"
                  Width="100px">
                  <CellStyle HorizontalAlign="Center"></CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
      </td>
    </tr>
  </table>
</asp:Content>
