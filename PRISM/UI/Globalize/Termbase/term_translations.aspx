<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="term_translations"
  CodeFile="Term_Translations.aspx.cs" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Term translations</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script type="text/javascript">
		
			function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
					oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Delete') {
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
		
		  function SC(c){
    	  var url = 'Term_TranslationEdit.aspx?t='+ termid + '&c=' + c;
    	  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	  url += '#target';
    	  winContainerEdit = OpenModalWindow(url,'containerwindow', 400, 500, 'no')
    	}		
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <tr height="*" valign="bottom">
    <td align="right">
      <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
        CssClass="hc_toolbar" ImageDirectory=" ">
        <HoverStyle CssClass="hc_toolbarhover">
        </HoverStyle>
        <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
        </ClientSideEvents>
        <SelectedStyle CssClass="hc_toolbarselected">
        </SelectedStyle>
        <Items>
          <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator></igtbar:TBSeparator>
          <igtbar:TBarButton Key="Delete" ToolTip="Delete selected" Text="Delete selected"
            Image="/hc_v4/img/ed_delete.gif">
            <DefaultStyle Width="120px">
            </DefaultStyle>
          </igtbar:TBarButton>
        </Items>
        <DefaultStyle CssClass="hc_toolbardefault">
        </DefaultStyle>
      </igtbar:UltraWebToolbar>
    </td>
  </tr>
  <tr valign="top">
    <td>
      <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Error message</asp:Label>
      <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%">
        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
          Version="4.00" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free"
          EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
          NoDataMessage="No data to display">
          <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
            
          <FrameStyle Width="100%" CssClass="dataTable">
          </FrameStyle>
          <RowAlternateStyleDefault CssClass="uga">
          </RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
          </RowStyleDefault>
          <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
            InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
        </DisplayLayout>
        <Bands>
          <igtbl:UltraGridBand>
            <Columns>
              <igtbl:UltraGridColumn Key="TermId" Width="30px" Hidden="True" BaseColumnName="TermId">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn Key="LanguageCode" Width="10px" Hidden="True" BaseColumnName="LanguageCode">
              </igtbl:UltraGridColumn>
              <igtbl:TemplatedColumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
                <CellStyle VerticalAlign="Top" Wrap="True">
                </CellStyle>
                <HeaderTemplate>
                  <asp:CheckBox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                </HeaderTemplate>
                <CellTemplate>
                  <asp:CheckBox ID="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                </CellTemplate>
                <Footer Key="Select" Caption="">
                </Footer>
                <Header Key="Select">
                </Header>
              </igtbl:TemplatedColumn>
              <igtbl:UltraGridColumn HeaderText="Language" Key="Language" Width="100px" BaseColumnName="CultureName">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn Key="Rtl" Hidden="True" BaseColumnName="Rtl" AllowUpdate="No">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Value" Key="TermValue" Width="100%" Type="Custom"
                BaseColumnName="Value" CellMultiline="Yes">
                <CellStyle Wrap="True">
                </CellStyle>
                <Footer Key="TermValue">
                </Footer>
                <Header Key="TermValue" Caption="Value">
                </Header>
              </igtbl:UltraGridColumn>
            </Columns>
          </igtbl:UltraGridBand>
        </Bands>
      </igtbl:UltraWebGrid></td>
  </tr>
  <input type="hidden" name="action">
</asp:Content>
