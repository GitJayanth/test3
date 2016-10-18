<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true"
  CodeFile="DailyActivity.aspx.cs" Inherits="UI_Collaborate_DailyActivities" %>

<%@ Register TagPrefix="igchart" Namespace="Infragistics.WebUI.UltraWebChart" Assembly="Infragistics2.WebUI.UltraWebChart.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Daily activity</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">

  <script type="text/javascript">
  
  function DisplayByDay(d, c)
  {
    alert(d);
    alert(c);
  
  }  
   function chart30Days_ClientOnMouseClick(this_ref, row, column, value, row_label, column_label, evt_type, layer_id){
    window.status = "You have selected (Row:"+row+", Column:"+column+", Value:"+value+", Row Label:"+row_label+", Column Label:"+column_label+")";
  }

  </script>

  <table class="main" cellspacing="0" cellpadding="0">
    <tr valign="bottom" height="*">
      <td align="right">
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
          CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBCustom runat="server" Width="200px">
              <asp:DropDownList ID="DDL_Cultures" runat="server" AutoPostBack="True" Width="200px"
                DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="DDL_Cultures_SelectedIndexChanged">
              </asp:DropDownList>
            </igtbar:TBCustom>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top">
      <td class="main">
        <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label><br />
        <center>
        <igchart:UltraChart ID="chart30Days" runat="server" BackColor="White"
          Version="6.1" ForeColor="Black" EmptyChartText="Data Not Available."
          OnChartDataClicked="chart30Days_ChartDataClicked" Width="700px">
          <Border CornerRadius="5"></Border>
          <ColorModel AlphaLevel="150" ModelStyle="CustomSkin">
            <Skin ApplyRowWise="False">
              <PEs>
              	
              </PEs>
            </Skin>
          </ColorModel>
          <Axis>
            <Y LineEndCapStyle="Flat" Visible="True" TickmarkInterval="20" LineThickness="1"
              Extent="30" TickmarkStyle="Smart">
              <Labels ItemFormatString="&lt;DATA_VALUE:00.##&gt;" VerticalAlign="Center" Font="Verdana, 7pt"
                Orientation="Horizontal" HorizontalAlign="Far">
                <SeriesLabels HorizontalAlign="Far" FormatString="" Orientation="VerticalLeftFacing"
                  VerticalAlign="Center">
                </SeriesLabels>
              </Labels>
              <MajorGridLines AlphaLevel="255" DrawStyle="Dot" Color="Gainsboro" Visible="True"
                Thickness="1"></MajorGridLines>
              <MinorGridLines AlphaLevel="255" DrawStyle="Dot" Color="LightGray" Visible="False"
                Thickness="1"></MinorGridLines>
            </Y>
            <Y2 Visible="False" TickmarkInterval="0">
              <Labels ItemFormatString="&lt;DATA_VALUE:00.##&gt;" VerticalAlign="Center" Orientation="VerticalLeftFacing"
                HorizontalAlign="Near">
                <SeriesLabels HorizontalAlign="Near" FormatString="" Orientation="VerticalLeftFacing"
                  VerticalAlign="Center">
                </SeriesLabels>
              </Labels>
              <MajorGridLines AlphaLevel="255" DrawStyle="Dot" Color="Gainsboro" Visible="True"
                Thickness="1"></MajorGridLines>
              <MinorGridLines AlphaLevel="255" DrawStyle="Dot" Color="LightGray" Visible="False"
                Thickness="1"></MinorGridLines>
            </Y2>
            <X2 Visible="False" TickmarkInterval="0">
              <Labels ItemFormatString="&lt;ITEM_LABEL&gt;" VerticalAlign="Center" Orientation="VerticalLeftFacing"
                HorizontalAlign="Far">
                <SeriesLabels HorizontalAlign="Center" Orientation="Horizontal" VerticalAlign="Center">
                </SeriesLabels>
              </Labels>
              <MajorGridLines AlphaLevel="255" DrawStyle="Dot" Color="Gainsboro" Visible="True"
                Thickness="1"></MajorGridLines>
              <MinorGridLines AlphaLevel="255" DrawStyle="Dot" Color="LightGray" Visible="False"
                Thickness="1"></MinorGridLines>
            </X2>
            <X LineEndCapStyle="Flat" Visible="True" TickmarkInterval="0" LineThickness="1">
              <Labels VerticalAlign="Center" Font="Verdana, 7pt" Orientation="VerticalLeftFacing"
                HorizontalAlign="Near" ItemFormatString="">
                <SeriesLabels HorizontalAlign="Center" Orientation="VerticalLeftFacing" VerticalAlign="Center">
                </SeriesLabels>
              </Labels>
              <MajorGridLines AlphaLevel="255" DrawStyle="Dot" Color="Gainsboro" Visible="True"
                Thickness="1"></MajorGridLines>
              <MinorGridLines AlphaLevel="255" DrawStyle="Dot" Color="LightGray" Visible="False"
                Thickness="1"></MinorGridLines>
            </X>
            <Z TickmarkInterval="0" Visible="True">
              <MinorGridLines AlphaLevel="255" Color="LightGray" DrawStyle="Dot" Thickness="1"
                Visible="False" />
              <MajorGridLines AlphaLevel="255" Color="Gainsboro" DrawStyle="Dot" Thickness="1"
                Visible="True" />
              <Labels HorizontalAlign="Near" ItemFormatString="" Orientation="Horizontal" VerticalAlign="Center">
                <SeriesLabels HorizontalAlign="Center" Orientation="Horizontal" VerticalAlign="Center">
                </SeriesLabels>
              </Labels>
            </Z>
            <Z2 TickmarkInterval="0" Visible="True">
              <MinorGridLines AlphaLevel="255" Color="LightGray" DrawStyle="Dot" Thickness="1"
                Visible="False" />
              <MajorGridLines AlphaLevel="255" Color="Gainsboro" DrawStyle="Dot" Thickness="1"
                Visible="True" />
              <Labels HorizontalAlign="Near" ItemFormatString="" Orientation="Horizontal" VerticalAlign="Center">
                <SeriesLabels HorizontalAlign="Center" Orientation="Horizontal" VerticalAlign="Center">
                </SeriesLabels>
              </Labels>
            </Z2>
          </Axis>
          <Tooltips FormatString="&lt;ITEM_LABEL&gt;: &lt;DATA_VALUE:00.##&gt;" />
          <ClientSideEvents ClientOnMouseClick="chart30Days_ClientOnMouseClick" />
          <Effects>
            <Effects>
            	
            </Effects>
          </Effects>
          <DeploymentScenario FilePath="" ImageType="Jpeg" ImageURL="" Scenario="Session" />
        </igchart:UltraChart></center>
        <br />
        <asp:Label ID="lbByDay" runat="server" Width="100%" Visible="False"></asp:Label>
        <igtbl:UltraWebGrid ID="dgByDay" runat="server" ImageDirectory="/ig_common/Images/"
          Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%">
            </FrameStyle>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="DateTimeStamp" Key="Date" Hidden="true">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="UserId" Key="UserId" Hidden="true">
                </igtbl:UltraGridColumn>
                <igtbl:TemplatedColumn BaseColumnName="FullName" HeaderText="Name" Key="HeaderText"
                  Width="200px">
                  <CellTemplate>
                    <asp:LinkButton ID="linkUser" runat="server" OnClick="DisplayByUser" Text='<% #Container.Value %>'></asp:LinkButton></CellTemplate>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn BaseColumnName="RoleName" HeaderText="Role" Key="RoleName"
                  Width="150px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ChunkCount" HeaderText="ChunksCount" Key="ChunksCount"
                  Width="100px">
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid><br />
        <asp:Label ID="lbByUser" runat="server" Width="100%" Visible="False"></asp:Label>
        <igtbl:UltraWebGrid ID="dgByUser" runat="server" ImageDirectory="/ig_common/Images/"
          Width="100%" OnInitializeRow="dgByUser_InitializeRow" OnSortColumn="dgByUser_SortColumn" OnGroupColumn="dgByUser_GroupColumn" OnUnGroupColumn="dgByUser_UnGroupColumn">
          <DisplayLayout MergeStyles="False" ViewType="OutlookGroupBy" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dgByUser" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="True">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ClassName" HeaderText="Class" Key="ClassName"
                  Width="150px" CellMultiline="Yes">
                  <CellStyle Wrap="True">
                  </CellStyle>
                  <Header Caption="Class">
                    <RowLayoutColumnInfo OriginX="1" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="1" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" HeaderText="Item" Key="ItemName"
                  Width="150px" CellMultiline="Yes">
                  <CellStyle Wrap="True">
                  </CellStyle>
                  <Header Caption="Item">
                    <RowLayoutColumnInfo OriginX="2" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="2" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Tag" HeaderText="Container" Key="Tag" Width="150px">
                  <Header Caption="Container">
                    <RowLayoutColumnInfo OriginX="3" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="3" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ChunkValue" HeaderText="Value" Key="Value"
                  Width="100%" CellMultiline="Yes">
                  <CellStyle Wrap="True">
                  </CellStyle>
                  <Header Caption="Value">
                    <RowLayoutColumnInfo OriginX="4" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="4" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ChunkStatus" HeaderText="S" Key="Status" Width="25px">
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Header Caption="S">
                    <RowLayoutColumnInfo OriginX="5" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="5" />
                  </Footer>
                </igtbl:UltraGridColumn>
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
        </igtbl:UltraWebGrid>
      </td>
    </tr>
  </table>
</asp:Content>
