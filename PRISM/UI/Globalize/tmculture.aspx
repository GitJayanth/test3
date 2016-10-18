<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Globalize.TMCulture" CodeFile="TMCulture.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Fix translation memory</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <script type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
			if (oButton.Key == 'filter')
			{
	      DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }		       
		}

   function SC(id, l){
    	  var url = 'TM/Expression_TranslationEdit.aspx?e='+ id + '&l=' + l;
    	  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	  url += '#target';
    	  winContainerEdit = OpenModalWindow(url,'containerwindow', 400, 500, 'no')
    	}				
		
  </script>

  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top">
      <td Class="sectionTitle" height="17">
        <asp:Label ID="lbTitle" runat="server" >List of expressions in</asp:Label>&nbsp;
        <asp:DropDownList Font-Size="x-small" ID="DDL_Languages" runat="server" AutoPostBack="True" DataTextField="Name"
            DataValueField="Code" OnSelectedIndexChanged="DDL_Languages_SelectedIndexChanged">
          </asp:DropDownList>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="150px"
          ImageDirectory=" ">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
          </ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Save" Text="Apply changes" Visible="False" Image="/hc_v4/img/ed_save.gif">
              <DefaultStyle Width="125px" Height="20px">
              </DefaultStyle>
            </igtbar:TBarButton>
       
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBLabel Text="Search">
              <DefaultStyle Width="50px" Font-Bold="True">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom Width="250px" Key="SearchField">
              <asp:TextBox Width="250px" ID="txtFilter" MaxLength="50"></asp:TextBox>
            </igtbar:TBCustom>
            <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
              <DefaultStyle Width="25px">
              </DefaultStyle>
            </igtbar:TBarButton>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top">
      <td class="main">
        <center>
          <asp:Label ID="lbInfo" runat="server" Visible="false">To find a TM Expression, please use the Search function</asp:Label><br />
          <asp:Label ID="lbNoResults" runat="server" Visible="false" Font-Bold="True" ForeColor="Red">No results</asp:Label></center>
        <asp:Label ID="lbMessage" runat="server" Visible="false">Message</asp:Label>
        <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True">
            </Pager>
             <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
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
                <igtbl:UltraGridColumn Key="TMExpressionId" Hidden="True" BaseColumnName="TMExpressionId">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="CultureCode" Hidden="True" BaseColumnName="CultureCode">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="Rtl" Hidden="True" BaseColumnName="Rtl">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Master" Key="TMExpressionValue" Width="50%" BaseColumnName="Value"
                  CellMultiline="Yes">
                  <CellStyle Wrap="True">
                  </CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="TranslatedValue" HeaderText="Translation" Width="50%"
                  BaseColumnName="TranslatedValue" CellMultiline="Yes">
                  <CellStyle Wrap="True">
                  </CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:TemplatedColumn Hidden="True" Key="Value" Width="100%">
                  <CellStyle VerticalAlign="Top" Wrap="True">
                  </CellStyle>
                  <CellTemplate>
                    <asp:TextBox ID="TXTChangedValue" runat="server" TextMode="MultiLine" Rows="5" Columns="80"></asp:TextBox>
                  </CellTemplate>
                </igtbl:TemplatedColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
        <input type="hidden" name="action">
      </td>
    </tr>
  </table>
</asp:Content>
