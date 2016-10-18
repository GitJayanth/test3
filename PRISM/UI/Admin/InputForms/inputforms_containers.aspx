<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="InputForms_Containers" CodeFile="InputForms_Containers.aspx.cs" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input form containers</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
    <script type="text/javascript" language="javascript">
      var winContainerEdit = null;
     
		  function SC(c){
    	  var url = 'InputForms_ContainersEdit.aspx?f='+ifid+'&c='+ c;
    	  //winContainerEdit = OpenModalWindow(url,'containerwindow', 430, 650, 'no'); //#ACQ8.20
    	  winContainerEdit = OpenModalWindow(url,'containerwindow', 430, 750, 'no');
		  }

		  function SCB(){
    	  var url = 'InputForms_ContainersBatchAdd.aspx?f='+ifid
    	  winContainerEdit = OpenModalWindow(url,'containerwindow', 430, 650,"yes", 1);
		  }

			function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Add') {
		      SC(-1)
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'BatchAdd') {
		      SCB()
          oEvent.cancelPostBack = true;
        }
		    if (oButton.Key == 'Delete') {
		      if (dg_nbItems_Checked == 0)
          {
   		      alert('Please, select at least one row');
   		      oEvent.cancelPostBack = true;
   		    }
   		    else{
            oEvent.cancelPostBack = !confirm("Are you sure you want to delete selected item ?");
          }
        }
		  } 

			function cg_up(){
	     var grid = igtbl_getGridById(dgClientId);
       var activeRow = grid.getActiveRow();
			  __doPostBack('groupup', activeRow.getIndex());
			}
			
			function cg_down(){
	     var grid = igtbl_getGridById(dgClientId);
       var activeRow = grid.getActiveRow();
			  __doPostBack('groupdown', activeRow.getIndex());
			}
			function cg_sup(){
			  if (confirm("Are you sure you want to remove this container group ?"))
			  {
	       var grid = igtbl_getGridById(dgClientId);
         var activeRow = grid.getActiveRow();
			    __doPostBack('groupdel', activeRow.getIndex());
			  }
			}
      </script>
</asp:Content>
<%--Removed width property in ultrawebtoolbar by Radha S to fix the horizantal width issue--%>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
  <table cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr valign="top" style="height:1px">
      <td>
        <igtbar:UltraWebToolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ImageDirectory=" "
          ItemWidthDefault="80px">
          <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
          <Items>
            <igtbar:TBarButton Image="/hc_v4/img/ed_back.gif" Key="List" Text="List" ToolTip="Back to list">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="AddSep" />
            <igtbar:TBarButton Image="/hc_v4/img/ed_new.gif" Key="Add" Text="Add">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="AddSep2" />
            <igtbar:TBarButton Image="/hc_v4/img/ed_new.gif" Key="BatchAdd" Text="Batch Add">
              <DefaultStyle Width="120px">
              </DefaultStyle></igtbar:TBarButton>
            <igtbar:TBSeparator Key="DeleteSep" />
            <igtbar:TBarButton Image="/hc_v4/img/ed_delete.gif" Key="Delete" Text="Delete selected"
              ToolTip="Delete selected">
              <DefaultStyle Width="120px">
              </DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="SaveSep" />
            <igtbar:TBarButton Image="/hc_v4/img/ed_save.gif" Key="Save" Text="Save container sorting">
              <DefaultStyle Width="250px"></DefaultStyle>
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="ExportSep" />
            <igtbar:TBarButton Image="/hc_v4/img/ed_download.gif" Key="Export" Text="Export">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="FilterSep" />
            <igtbar:TBLabel Text="Filter">
              <DefaultStyle Font-Bold="True" Width="40px">
              </DefaultStyle>
            </igtbar:TBLabel>
            <igtbar:TBCustom Width="150px" Key="filterField" runat="server">
              <asp:TextBox Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
            </igtbar:TBCustom>
            <igtbar:TBarButton Image="/hc_v4/img/ed_search.gif" Key="filter">
              <DefaultStyle Width="25px">
              </DefaultStyle>
            </igtbar:TBarButton>
          </Items>
          <DefaultStyle CSSCLASS="hc_toolbardefault"></DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr valign="top" style="height:1px">
      <td>
        <asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</ASP:LABEL>
      </td>
    </tr>
    <tr>
      <td>
        <igtbl:UltraWebGrid id="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/" UseAccessibleHeader="False">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
            HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" Font-Bold="true" BackColor ="LightGray"> <%--CssClass="gh">--%>
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
            </FrameStyle>
            <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
					  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
          </DisplayLayout>
          <bands>
          <%--Modified the code to fix QC-7030 - Added label tag by Radha S--%>
            <igtbl:ultragridband Key="InputFormContainerId" BorderCollapse="Collapse">
              <Columns>
                <igtbl:ultragridcolumn Key="InputFormContainerId" Width="30px" BaseColumnName="Id" ServerOnly="True"></igtbl:ultragridcolumn>
                <igtbl:templatedcolumn Key="Select" Width="20px" BaseColumnName="">
                  <cellstyle VerticalAlign="Top" Wrap="True"></cellstyle>
                  <headertemplate>
                    <asp:checkbox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:checkbox>
                    <asp:Label id="grp_lbl" Text="Hello" runat="server" Visible="false"></asp:Label>
                  </headertemplate>
                  <celltemplate>
                    <asp:checkbox ID="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:checkbox>
                    <asp:Label id="grp_lbl" Text="Hello" runat="server" Visible="false"></asp:Label>
                  </celltemplate>
                  <footer Key="Select" caption=""></footer>
                  <header Key="Select"></header>
                </igtbl:templatedcolumn>
                <igtbl:ultragridcolumn Width="170px" Key="Tag" HeaderText="Tag" BaseColumnName="ContainerTag" ServerOnly="True">
                  <cellstyle Wrap="True"></cellstyle>
                </igtbl:ultragridcolumn>
                <igtbl:ultragridcolumn Width="400px" Key="ContainerName" HeaderText="Container" BaseColumnName="ContainerName">
                  <cellstyle Wrap="True"></cellstyle>
                </igtbl:ultragridcolumn>
                <igtbl:ultragridcolumn HeaderText="Comment" Key="Comment" Width="100%" BaseColumnName="Comment">
                  <cellstyle Wrap="True"></cellstyle>
                </igtbl:ultragridcolumn>
                <igtbl:ultragridcolumn HeaderText="R" Key="isReg" Width="25px" Type="CheckBox" BaseColumnName="Regionalizable"
                  AllowUpdate="No">
                  <cellstyle HorizontalAlign="Center"></cellstyle>
                </igtbl:ultragridcolumn>
                <igtbl:ultragridcolumn HeaderText="M" Key="isMandatory" Width="25px" Type="CheckBox" BaseColumnName="Mandatory"
                  AllowUpdate="No">
                  <cellstyle HorizontalAlign="Center"></cellstyle>
                </igtbl:ultragridcolumn>
                <igtbl:ultragridcolumn HeaderText="Type" Key="InputTypeName" Width="100px" BaseColumnName="Type">
                  <cellstyle Wrap="True"></cellstyle>
                </igtbl:ultragridcolumn>
                <igtbl:ultragridcolumn Key="ContainerGroupPath" Hidden="true" BaseColumnName="ContainerGroupPath"></igtbl:ultragridcolumn>
              </Columns>
            </igtbl:ultragridband>
          </bands>
        </igtbl:UltraWebGrid>
        <center>
          <asp:Label id="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
        </center>
      </td>
    </tr>
  </table>
  <input type="hidden" name="action"> <input type="hidden" name="txtSortColPos" id="txtSortColPos" runat="server" value="7">
</asp:Content>