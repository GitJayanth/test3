<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="container_usage" CodeFile="container_usage.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Usage</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script>
		function dg_DblClickHandler(gridName, id, ev){
			var cell = igtbl_getCellById(id);
      var grid = igtbl_getGridById(gridName);
		}

		
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
		    if (oButton.Key == 'filter')
		    {     		 
		  
		        var sURL = window.document.URL.toString();
		        var arrParams = sURL.split("&");
                sURL  = arrParams[0] ;
		        if (txtFilterField != undefined)
               {
                  sURL = sURL + "&filter=" + encodeURI(txtFilterField.value)  ;
               } 
                if (culSelect != undefined)
               {
                
                 sURL = sURL + "&CultureList=" + culSelect.value ;
               }
            window.location= sURL;
		    //DoSearch();	
            oEvent.cancelPostBack = true;
            return;
        }
		  if (oButton.Key == 'List') {
				back();
				oEvent.cancelPostBack = true;
      }
		} 

  </script>
</asp:Content>
<%--Removed width propery in UltraWebToolbar to fix the horizontal width issue By Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
    <table class="main" cellspacing="0" cellpadding="0">
      <tr valign="top" style="height: 1px">
        <td>
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
            CssClass="hc_toolbar">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
            </ClientSideEvents>
            <Items>
              <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBLabel Text="Filter">
                <DefaultStyle Width="40px" Font-Bold="True">
                </DefaultStyle>
              </igtbar:TBLabel>
              <igtbar:TBCustom Width="150px" Key="filterField">
                <asp:TextBox CssClass="Search" Width="150px" ID="txtFilter" MaxLength="50"></asp:TextBox>
              </igtbar:TBCustom>
              <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                <DefaultStyle Width="25px">
                </DefaultStyle>
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBLabel Text="Culture">
                  <DefaultStyle Font-Bold="True" Width="50px">
                  </DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBCustom Key="CultureFilter" Width="250px">
                  <asp:DropDownList ID="CultureList" runat="Server" AutoPostBack="True" DataTextField="Name"
                    DataValueField="Code">
                  </asp:DropDownList>
                </igtbar:TBCustom>     
            </Items>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr valign="top">
        <td>
          <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%">
            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
              SelectTypeRowDefault="Extended" HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No"
              Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container"
              AllowColSizingDefault="Fixed" AllowDeleteDefault="No" AllowUpdateDefault="No"
              BorderCollapseDefault="Separate" RowHeightDefault="20px" Version="4.00" ViewType="OutlookGroupBy">
              <%-- Fix for QC# 7384 and QC#7386 by Radha S. Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --Start --%>
              <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </HeaderStyleDefault>
              <FrameStyle Width="100%" CssClass="dataTable">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
              </FrameStyle>
              <RowSelectorStyleDefault Width="30px" CssClass="gh">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </RowSelectorStyleDefault>
              <RowAlternateStyleDefault CssClass="uga">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
              </RowAlternateStyleDefault>
              <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
              </RowStyleDefault>
              <%-- Fix for QC# 7384 by Radha S. Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --End --%>
            </DisplayLayout>
            <Bands>
              <igtbl:UltraGridBand Key="Usages" BorderCollapse="Collapse">
                <Columns>
                  <igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" ServerOnly="True">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="ItemStatus" BaseColumnName="ItemStatus" ServerOnly="True">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="S" Key="ChunkStatus" IsBound="True" Width="20px" BaseColumnName="Status">
                  </igtbl:UltraGridColumn>
                   <igtbl:UltraGridColumn HeaderText="ChunkStatus" Key="ST" Hidden="true"  Width="16px" BaseColumnName="Status">
                 </igtbl:UltraGridColumn>
                 
                  <igtbl:UltraGridColumn HeaderText="Class" Key="ClassName" Width="280px" BaseColumnName="ClassName"
                    CellMultiline="Yes">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Item name" Key="ItemName" Width="280px" BaseColumnName="ItemName"
                    CellMultiline="Yes">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="ItemSku" Width="40px" BaseColumnName="ItemSku" ServerOnly="True">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Level" Key="Level" Width="70px" BaseColumnName="ItemLevelId">
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Value" Key="Value" Width="100%" BaseColumnName="Text"
                    CellMultiline="Yes">
                    <CellStyle Wrap="True">
                    </CellStyle>
                  </igtbl:UltraGridColumn>
                </Columns>
              </igtbl:UltraGridBand>
            </Bands>
          </igtbl:UltraWebGrid>
          <center>
            <asp:Label ID="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
          </center>
        </td>
      </tr>
    </table>
</asp:Content>