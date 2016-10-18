<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true"
  CodeFile="TranslationReport.aspx.cs" Inherits="UI_Globalize_TranslationReport" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Translation dashboard</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <table class="main" cellspacing="0" cellpadding="0" width="100%">
    <tr valign="top">
      <td class="sectionTitle">
          <table border="0">
            <tr valign="middle">
              <td>
              <table><tr><td><asp:Label ID="lbStart" runat="server">Start date: </asp:Label>
                <igsch:WebDateChooser ID="StartDate" runat="server" NullDateLabel="" Value="">
                </igsch:WebDateChooser></td><td><asp:Label ID="lbEnd" runat="server">End date: </asp:Label>
                <igsch:WebDateChooser ID="EndDate" runat="server" NullDateLabel="">
                </igsch:WebDateChooser></td></tr></table>
              </td>
              <td></td>
            </tr>
            <tr>
              <td>
                <asp:CheckBoxList ID="cbType" runat="server" RepeatColumns="4">
                  <asp:ListItem Value="S">TR</asp:ListItem>
                  <asp:ListItem Value="M">MTR</asp:ListItem>
                  <asp:ListItem Value="C">CTR</asp:ListItem>
                  <asp:ListItem Value="E">Termbase</asp:ListItem>
                  </asp:CheckBoxList>
                </td>
                <td align="left"><asp:Button ID="btSearch" runat="server" Text="Generate" OnClick="btSearch_Click"></asp:Button></td>
            </tr>
            <tr>
              <td colspan="2">
                <asp:Label ID="lbMessage" runat="server" CssClass="hc_error" Text="Label"></asp:Label></td>
            </tr>
          </table>
      </td>
    </tr>
    <tr valign="top">
      <td class="sectionTitle" style="height: 100%">
        <asp:Label ID="Label1" runat="server">Results</asp:Label>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
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
        <igtbl:UltraWebGrid ID="dgResults" runat="server" ImageDirectory="/ig_common/Images/"
          Width="100%" OnInitializeRow="dgResults_InitializeRow">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dgResults" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display" SelectTypeCellDefault="Extended"
            SelectTypeColDefault="Extended" ViewType="Hierarchical">
           <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
           <FrameStyle Width="100%" CssClass="dataTable">
                                <%-- Fix for QC# 7386 by Nisha Verma Added borderdetails tag to fix the gridlines missing issue --%>
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </FrameStyle>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
            <FilterOptionsDefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
              <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px"
                Width="200px">
                <Padding Left="2px" />
              </FilterDropDownStyle>
              <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
              </FilterHighlightRowStyle>
            </FilterOptionsDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand Key="Average">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ClassId" Hidden="True" Key="ClassId">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ClassName" HeaderText="Class" Key="ClassName"
                  Width="200px">
                  <CellStyle CssClass="ptb1" Font-Bold="True"></CellStyle>
                  <Header Caption="Class">
                    <RowLayoutColumnInfo OriginX="1" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="1" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="AverageTAT" HeaderText="TAT (avg)" Key="AverageTAT"
                  Width="50px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="TAT (avg)">
                    <RowLayoutColumnInfo OriginX="2" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="2" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SumTChunks" HeaderText="Chunks (S)" Key="SumTChunks"
                  Width="50px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Chunks (S)">
                    <RowLayoutColumnInfo OriginX="3" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="3" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SumTWords" HeaderText="Translated words (S)"
                  Key="SumTWords" Width="80px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Translated words (S)">
                    <RowLayoutColumnInfo OriginX="4" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="4" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SumTWordsSentToTranslation" HeaderText="Translation sent words (S)"
                  Key="SumTWordsSentToTranslation" Width="85px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Translation sent words (S)">
                    <RowLayoutColumnInfo OriginX="5" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="5" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SumTWordsFilteredByTM" HeaderText="TM filtered words (S)"
                  Key="SumTWordsFilteredByTM" Width="80px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="TM filtered words (S)">
                    <RowLayoutColumnInfo OriginX="6" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="6" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="AverageTLanguages" HeaderText="Languages (avg)"
                  Key="AverageTLanguages" Width="80px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Languages (avg)">
                    <RowLayoutColumnInfo OriginX="7" />
                  </Header>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="7" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="AllLanguageCodes" CellMultiline="Yes" HeaderText="Language List"
                  Key="AllLanguageCodes" Width="100%">
                  <Header Caption="Language List">
                    <RowLayoutColumnInfo OriginX="8" />
                  </Header>
                  <CellStyle Wrap="True">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="8" />
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
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                </FilterHighlightRowStyle>
              </FilterOptions>
            </igtbl:UltraGridBand>
            <igtbl:UltraGridBand Key="Details" Indentation="0">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ClassId" Hidden="True" Key="ClassId">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ClassName" Hidden="True" Key="ClassName">
                  <Header>
                    <RowLayoutColumnInfo OriginX="1" />
                  </Header>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="1" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TRId" HeaderText="Project (Id)" Key="TRId"
                  Width="50px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Project (Id)">
                    <RowLayoutColumnInfo OriginX="2" />
                  </Header>
                  <CellStyle HorizontalAlign="Center" Font-Bold="true">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="2" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" CellMultiline="Yes" HeaderText="Item Name"
                  Key="Item Name" Width="150px">
                  <Header Caption="Item Name">
                    <RowLayoutColumnInfo OriginX="3" />
                  </Header>
                  <CellStyle Wrap="True">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="3" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Type" HeaderText="Type" Key="Type" Width="50px">
                  <Header Caption="Type">
                    <RowLayoutColumnInfo OriginX="4" />
                  </Header>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="4" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Status" HeaderText="Status" Key="Status" Width="80px">
                  <Header Caption="Status">
                    <RowLayoutColumnInfo OriginX="5" />
                  </Header>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="5" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="CreationDate" HeaderText="Created" Key="CreationDate"
                  Width="70px">
                  <Header Caption="Created">
                    <RowLayoutColumnInfo OriginX="6" />
                  </Header>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="6" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="CompletionDate" HeaderText="Completed" Key="CompletionDate"
                  Width="70px">
                  <Header Caption="Completed">
                    <RowLayoutColumnInfo OriginX="7" />
                  </Header>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="7" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TAT" HeaderText="TAT" Key="TAT" Width="40px">
                  <Header Caption="TAT">
                    <RowLayoutColumnInfo OriginX="8" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="8" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TChunks" HeaderText="Chunks" Key="TChunks"
                  Width="50px">
                  <Header Caption="Chunks">
                    <RowLayoutColumnInfo OriginX="9" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="9" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TWords" HeaderText="Words" Key="TWords" Width="50px">
                  <Header Caption="Words">
                    <RowLayoutColumnInfo OriginX="10" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="10" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TWordsSentToTranslation" HeaderText="Translation sent words"
                  Key="TWordsSentToTranslation" Width="80px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="Translation sent words">
                    <RowLayoutColumnInfo OriginX="11" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="11" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TWordsFilteredByTM" HeaderText="TM filtered words"
                  Key="TWordsFilteredByTM" Width="80px">
                  <HeaderStyle Wrap="True" />
                  <Header Caption="TM filtered words">
                    <RowLayoutColumnInfo OriginX="12" />
                  </Header>
                  <CellStyle HorizontalAlign="Right">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="12" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="TLanguages" HeaderText="Languages" Key="TLanguages"
                  Width="70px">
                  <Header Caption="Languages">
                    <RowLayoutColumnInfo OriginX="13" />
                  </Header>
                  <CellStyle HorizontalAlign="Center">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="13" />
                  </Footer>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="LanguageCodes" CellMultiline="Yes" HeaderText="Language List"
                  Key="LanguageCodes" Width="100%">
                  <Header Caption="Language List">
                    <RowLayoutColumnInfo OriginX="14" />
                  </Header>
                  <CellStyle Wrap="True">
                  </CellStyle>
                  <Footer>
                    <RowLayoutColumnInfo OriginX="14" />
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
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                </FilterHighlightRowStyle>
              </FilterOptions>
              <RowStyle CssClass="ugd" BackColor="#FAFCF1" />              
              <RowAlternateStyle CssClass="ugd" BackColor="#F1F9F9" />              
              
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        <asp:Label ID="lbNotes" Font-Size="XX-Small" ForeColor="gray" runat="server">Note: The aggregated values (sum/average) are computed only with Completed projects (not with the opened ones)</asp:Label>
      </td>
    </tr>
  </table>
</asp:Content>
