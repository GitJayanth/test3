<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HyperCatalog.master"
  CodeFile="countryproducts.aspx.cs" Inherits="UI_Localize_countryproducts"%>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  <% switch(pageAction)
     {
       case "o":
       case "O": Response.Write("Products nearly obsolete"); break;
       case "1": Response.Write("Country top values"); break;
       case "3": Response.Write("Country bundles"); break;
       default:
         Response.Write("New product introduction"); break;
   }
  %>
</asp:Content>
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
      if (oButton.Key == 'Validate' || oButton.Key == 'Exclude')
			{
        oEvent.cancelPostBack = !confirm('are you sure?');
      }
    }

	  
	  function EditPLC(i, col, row, dg)
	  {
 	    var url = "../../redirect.aspx?p=" + 
 	     escape("UI/Acquire/QDE/qde_CountryViewEditPLC.aspx?i=" + i + "&col=" + col + "&row=" + row + "&dg=" + dg + "&c=" +curCulture);
 	    var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
 	    url += '#target';
 	    winChunkEdit = OpenModalWindow(url,'plcwindow', 250, 450, 'yes', 'yes', 'no')
 	    return false;
 	   }
	  
	  function UpdatePLC(dgName, col, row, pid, pod)	  
	  {
	    var dg = igtbl_getGridById(dgName);	    
	    var newUrl = "<a href='javascript://' onclick='EditPLC(" + dg.Rows.getRow(row).getCellFromKey("ItemId").getValue() + ", " + curCulture + ", " 
         col + "," +
         row + ",\"" +
         dg.Id + "\")'>" + pid +"</a>";
	    dg.Rows.getRow(row).getCell(col).getElement().innerHTML = newUrl;
	    dg.Rows.getRow(row).getCell(col+1).getElement().innerHTML = pod;
	  }
	  function r(id){
	    var w = window.open(sRootUrl +'redirect.aspx?i='+id+'&c='+curCulture + '&p=UI/Globalize/qdetranslate.aspx','_popup','toolbar=1,location=1,directories=1,status=1,scrollbars=1,resizable=1,copyhistory=0,menuBar=1,width=1000,height=800');
	    w.focus();
	    return false;

	  }
	  function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
  </script>

  <table style="width: 100%; height: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top">
      <td class="sectionTitle">
        <asp:DropDownList Font-Size="x-Small" ID="DDL_Cultures" runat="server" AutoPostBack="True"
          DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="DDL_Cultures_SelectedIndexChanged">
        </asp:DropDownList>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px"
          ImageDirectory=" " OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
          </ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Validate" Text="Validate selected" Image="/hc_v4/img/ed_OK.gif">
              <DefaultStyle Width="150px" Height="20px">
              </DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Exclude" Text="Exclude selected" Image="/hc_v4/img/ed_reject.gif">
              <DefaultStyle Width="150px" Height="20px">
              </DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBLabel Text="Search">
              <DefaultStyle Width="50px" Font-Bold="True">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom Width="250px" Key="SearchField" runat="server">
              <asp:TextBox Width="250px" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
            </igtbar:TBCustom>
            <igtbar:TBarButton key="filter" Image="/hc_v4/img/ed_search.gif">
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
        &nbsp;<asp:Label ID="lbMessage" runat="server" Text="lbMessage" Visible="false"></asp:Label>
        <igtbl:UltraWebGrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%"
          OnInitializeRow="dg_InitializeRow">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display">
            <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True">
            </Pager>
            <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
								  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:TemplatedColumn FooterText="" Key="Select" Width="20px">
                  <CellTemplate>
                    <asp:CheckBox ID="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                  </CellTemplate>
                  <HeaderTemplate>
                    <asp:CheckBox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                  </HeaderTemplate>
                  <CellStyle VerticalAlign="Middle" Wrap="True">
                  </CellStyle>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="True">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="Sku" HeaderText="Sku"
                  Key="sku">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ItemName" HeaderText="Name"
                  Key="ItemName" Width="300px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ClassName" HeaderText="Class"
                  Key="ClassName" Width="150px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="pid" Format="System.String"
                  HeaderText="PID" Key="PID" Width="100px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="pod" Format="System.String"
                  HeaderText="POD" Key="POD" Width="100px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="RemovalDate" Format="System.String"
                  HeaderText="Removal date" Key="Removal" Width="100px">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ContentStatus" HeaderText="Content status"
                  Key="ContentStatus">
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid><br />
        <center>
          <asp:Label ID="lbNoresults" runat="server" ForeColor="Red" Font-Bold="True">No results</asp:Label>
        </center>
      </td>
    </tr>
  </table>
<input type="hidden" name="action" id="action" />
  <script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

  <script>
    window.onload=g_i
  </script>

</asp:Content>
