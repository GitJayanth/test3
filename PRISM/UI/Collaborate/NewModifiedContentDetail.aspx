<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true"
  CodeFile="NewModifiedContentDetail.aspx.cs" Inherits="UI_Collaborate_NewModifiedContentDetail" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  New/Modified Content</asp:Content>
  <asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" language="javascript">
    
  function uwToolbar_Click(oToolbar, oButton, oEvent)
	{
		if (oButton.Key == 'Close')
		{
			oEvent.cancelPostBack = true;
			window.close();
		}
	}
	
	function OpenChunkWindow(containerId)
	{
    var url = '../Acquire/Chunk/Chunk.aspx?i='+itemId+'&c='+cultureCode+'&d='+containerId; 
    winChunk = OpenModalWindow(url, "chunkWindow", 340, 600, 'yes');
	}
	
</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table style="width: 100%; height: 100%" border="0" cellpadding="0" cellspacing="0">
    <tr height="1" valign="top">
      <td>
        <igtbar:UltraWebToolbar ID="uwToolbarTitle" runat="server" CssClass="hc_toolbartitle"
          Width="100%" BackgroundImage="" ImageDirectory="" ItemWidthDefault="">
          <Items>
            <igtbar:TBLabel Key="ItemName" Text="ItemName">
              <DefaultStyle Font-Bold="True" Font-Size="9pt" TextAlign="Left">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBLabel Key="Culture" Text="Culture" ImageAlign="Right">
              <DefaultStyle Font-Bold="True" Font-Size="9pt" TextAlign="Right">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBLabel Text="" ImageAlign="NotSet">
              <DefaultStyle TextAlign="Right" Width="5px">
              </DefaultStyle>
            </igtbar:TBLabel>
          </Items>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr height="1" valign="top">
      <td>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%"
          CssClass="hc_toolbar" BackgroundImage="" ImageDirectory="" ItemWidthDefault="25px">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
          <Items>
            <igtbar:TBarButton Key="Close" ToolTip="Close" Image="/hc_v4/Img/ed_cancel.gif">
            </igtbar:TBarButton>
          </Items>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top">
    <td>
           <asp:Label ID="lError" runat="server" Text="lError" CssClass="hc_error"></asp:Label><br />
      <asp:Label ID="lContent" runat="server" Text="Content"></asp:Label>
          <igtbl:UltraWebGrid ID="dgContent" runat="server" ImageDirectory="/ig_common/Images/" Width="100%" OnInitializeRow="dgResults_InitializeRow">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
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
            <igtbl:UltraGridBand Key="Details">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ContainerId" Key="ContainerId" Hidden="true">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ContainerName" HeaderText="Container" Key="ContainerName" 
                  Width="100%">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NewValue" HeaderText="New value" Key="NewValue" 
                  Width="200px" CellMultiline="Yes">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                                <igtbl:UltraGridColumn BaseColumnName="OldValue" HeaderText="Old value" Key="OldValue" 
                  Width="200px" CellMultiline="Yes">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>

                <igtbl:UltraGridColumn BaseColumnName="ContentStatus" HeaderText="Status" Key="ContentStatus"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="FallbackCode" HeaderText="Fallback" Key="FallbackCode"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ModifyDate" HeaderText="ModifyDate" Key="ModifyDate"
                  Width="100px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        <asp:Label ID="lLinks" runat="server" Text="Links"></asp:Label>
          <igtbl:UltraWebGrid ID="dgLinks" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
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
            <igtbl:UltraGridBand Key="Details">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="LinkItemNumber" HeaderText="Link from" Key="LinkItemNumber" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="LinkSubItemNumber" HeaderText="Link to" Key="LinkSubItemNumber" >
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="LinkTypeName" HeaderText="Link type" Key="LinkTypeName" 
                  Width="120px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SubItemSort" HeaderText="Sort order" Key="SubItemSort" 
                  Width="100%">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Bidirectional" Type="CheckBox" HeaderText="Bi" Key="Bidirectional"
                  Width="50px">
                  <HeaderStyle Wrap="true" />
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                
                <igtbl:UltraGridColumn BaseColumnName="Recommended" Type="CheckBox" HeaderText="DSR" Key="Recommended"
                  Width="50px">
                  <HeaderStyle Wrap="true" />
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ContentStatus" HeaderText="Status" Key="ContentStatus"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="FallbackCountryCode" HeaderText="Fallback" Key="FallbackCode"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ModifyDate" HeaderText="ModifyDate" Key="ModifyDate"
                  Width="100px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        <asp:Label ID="lMarketSegments" runat="server" Text="Market segments"></asp:Label>
          <igtbl:UltraWebGrid ID="dgMarketSegments" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
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
            <igtbl:UltraGridBand Key="Details">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" HeaderText="Item name" Key="ItemName" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemNumber" HeaderText="Item PN" Key="ItemNumber" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NewValue" HeaderText="New value" Key="NewValue" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="OldValue" HeaderText="Old value" Key="OldValue" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ContentStatus" HeaderText="Status" Key="ContentStatus"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="FallbackCountryCode" HeaderText="Fallback" Key="FallbackCode"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ModifyDate" HeaderText="ModifyDate" Key="ModifyDate"
                  Width="100px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        <asp:Label ID="lPublishers" runat="server" Text="Publishers"></asp:Label>
          <igtbl:UltraWebGrid ID="dgPublishers" runat="server" ImageDirectory="/ig_common/Images/" Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" SelectTypeRowDefault="Single"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
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
            <igtbl:UltraGridBand Key="Details">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" HeaderText="Item name" Key="ItemName" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemNumber" HeaderText="Item PN" Key="ItemNumber" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="NewValue" HeaderText="New value" Key="NewValue" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="OldValue" HeaderText="Old value" Key="OldValue" 
                  Width="120px">
                  <HeaderStyle Wrap="true" />
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ContentStatus" HeaderText="Status" Key="ContentStatus"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="FallbackCountryCode" HeaderText="Fallback" Key="FallbackCode"
                  Width="70px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ModifyDate" HeaderText="ModifyDate" Key="ModifyDate"
                  Width="100px">
                  <CellStyle HorizontalAlign="center"></CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
    </td>
    </tr>
  </table>
</asp:Content>
