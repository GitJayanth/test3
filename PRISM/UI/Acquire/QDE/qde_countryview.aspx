<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_CountryView"
    CodeFile="QDE_Countryview.aspx.cs" %>

<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc1" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
    Content</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

    <script type="text/javascript" language="javascript">

      var itemId = null;
			var cultureCode = null;
			
			// Add by mickael CHARENSOL
			function AddLink()
      {
        var url = "../Links/Links_add.aspx?i="+itemId+"&t=4&f=1";
        winLinkAdd = OpenModalWindow(url, "lnk", 600, 500, "yes");
      }
			// end
			
      function uwToolbar_Click(oToolbar, oButton, oEvent)
      {
        if (oButton.Key == 'EditPLC')
				{
				  oEvent.cancelPostBack = true;
    	    var url = 'qde_CountryViewEditPLC.aspx?i=' + itemId + '&c=' + cultureCode;
    	    var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	    url += '#target';
    	    winChunkEdit = OpenModalWindow(url,'chunkwindow', 250, 450, 'yes')
    	  }
        if (oButton.Key == 'Validate')
				{
				  oEvent.cancelPostBack = !confirm('are you sure you want to validate this product ?');
    	  }
    	  if (oButton.Key == 'Exclude')
				{
				  oEvent.cancelPostBack = !confirm('are you sure you want to exclude this product ?');
    	  }
        if (oButton.Key == 'Unexclude')
				{
				  oEvent.cancelPostBack = !confirm('are you sure you want to unexclude this product ?');
    	  }
    	  if (oButton.Key == 'Add') 
		    {
          oEvent.cancelPostBack = true;
		      AddLink();
        }
		    if (oButton.Key == 'Delete')
		    {
          oEvent.cancelPostBack = !confirm("Are you sure you want to delete this product ?");
        }
           if (oButton.Key == 'ResumeInheritance')
            oEvent.cancelPostBack = !confirm("ResumeInheritance will delete all the localized links for this SKU. Are you sure you want to perform this opertaion?");
      }
      function uwToolbar_Click2(oToolbar, oButton, oEvent)
      {
      if (oButton.Key == 'legend')
      {
        var url = 'qde_countryviewlegend.htm'; 
        winLegend = OpenModalWindow(url, "legend", 120, 200, 'yes');
        oEvent.cancelPostBack = true;
        return;
      }
		  if (oButton.Key == 'filter')
		  {     		     
		    DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
		  
	}
			  

      function OpenWhosWho(id, c)
      {
	      var url = 'qde_itemwhoswho.aspx?i='+id+'&c='+c; 
        winWhosWho = OpenModalWindow(url, "cr", 500, 500, 'yes');
      }
		  function ed(gridName, index)
		  {
  	    dgGrid = igtbl_getGridById(gridName);
    	  var url = 'qde_chunk.aspx?g='+gridName+'&r='+index+'&i='+itemId+'&c='+cultureCode;;
    	  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	  url += '#target';
    	  winChunkEdit = OpenModalWindow(url, 'chunkwindow', 360, 615, 'yes')
		  }

		  function jp(id)
		  {
        if (top.qdeFrame && top.qdeFrame.frameitemscope)
        {
					top.qdeFrame.frameitemscope.location = "qde_itemscope.aspx?i=" + id;
        }      	
        else
        {
          top.window.location = 'qde_countryview.aspx?i=' + id;
        }
        return false;
		  }
		  
		      
		function UpdateMS(cbMS)
    {
      var cb = document.getElementById(cbMS);
      if (cb)
      {
        cb.checked = true;
      }
    }
    
    function UpdateP(cbP)
    {
      var cb = document.getElementById(cbP);
      if (cb)
      {
      cb.checked = true;
      }
    }
    function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
    function UnloadGrid()
    {
      igtbl_unloadGrid("dgUPC");
    }
    function UnloadGrid()
    {
      igtbl_unloadGrid("dgl");
    }
    function UnloadGrid()
    {
      igtbl_unloadGrid("dgCrossSell");
    }
    
    </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

    <script type="text/javascript" language="javascript">
		  document.body.onunload = "closeWindows";
 		  document.body.scroll = "no";
    </script>

    <table border="0" style="width: 100%; height: 100%" cellspacing="0" cellpadding="0">
        <tr valign="top">
            <td colspan="2">
                <div id="divDg" style="overflow: auto; width: 100%; height: 100%">
                    <table class="main" style="width: 100%; background-color: White" cellspacing="0"
                        cellpadding="0" border="0">
                        <asp:Panel ID="pInfo" runat="server">
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="background-color: White">
                                        <tr valign="top" style="height: 1px">
                                            <td colspan="3">
                                            <%--Removed the width tag to fix enlarged button by Radha S--%>
                                                <igtbar:UltraWebToolbar ID="uwToolbarExport" runat="server" ItemWidthDefault="75px"
                                                    Height="24px" CssClass="hc_toolbar" BackgroundImage="" ImageDirectory="" OnButtonClicked="uwToolbarExport_ButtonClicked">
                                                    <HoverStyle CssClass="hc_toolbarhover">
                                                    </HoverStyle>
                                                    <DefaultStyle CssClass="hc_toolbardefault">
                                                    </DefaultStyle>
                                                    <SelectedStyle CssClass="hc_toolbarselected">
                                                    </SelectedStyle>
                                                    <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                                                    <Items>
                                                        <igtbar:TBarButton Key="OverviewExport" Text="Export" Image="/hc_v4/Img/ed_download.gif">
                                                        </igtbar:TBarButton>
                                                    </Items>
                                                </igtbar:UltraWebToolbar>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="width: 170px" align="right">
                                                <asp:Image ID="imgProduct" runat="server" BorderStyle="None" BorderWidth="0px"></asp:Image>
                                            </td>
                                            <td align="right" style="height: 100%; width: 100%">
                                                <table width="300px" border="0" style="height: 100%">
                                                    <tr valign="top" style="height: 1px">
                                                        <td>
                                                            <asp:Label ID="lbItemInfo" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                    <%--Removed the width tag to fix enlarged button by Radha S--%>
                                                        <td>
                                                            <igtbar:UltraWebToolbar ID="uwToolbarStatus" runat="server" ItemWidthDefault="90px"
                                                                CssClass="hc_toolbar" BackgroundImage="" ImageDirectory="" TextAlign="Right"
                                                                OnButtonClicked="uwToolbarStatus_ButtonClicked">
                                                                <HoverStyle CssClass="hc_toolbarhover">
                                                                </HoverStyle>
                                                                <DefaultStyle CssClass="hc_toolbardefault">
                                                                </DefaultStyle>
                                                                <SelectedStyle CssClass="hc_toolbarselected">
                                                                </SelectedStyle>
                                                                <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                                                                <Items>
                                                                    <igtbar:TBarButton Key="Unexclude" Text="Un-exclude" Image="/hc_v4/Img/ed_OK.gif">
                                                                    </igtbar:TBarButton>
                                                                    <igtbar:TBSeparator Key="UnexcludeSep" />
                                                                    <igtbar:TBarButton Key="Validate" Text="Validate" Image="/hc_v4/Img/ed_OK.gif">
                                                                    </igtbar:TBarButton>
                                                                    <igtbar:TBSeparator Key="ValidateSep" />
                                                                    <igtbar:TBarButton Key="Exclude" Text="Exclude" Image="/hc_v4/Img/ed_reject.gif">
                                                                    </igtbar:TBarButton>
                                                                </Items>
                                                            </igtbar:UltraWebToolbar>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="bottom">
                                                            <asp:Label ID="lbRoll" runat="server" Visible="false">Roll</asp:Label>
                                                            <br />
                                                            <asp:Label ID="lbCreated" runat="server" Font-Italic="True">ItemCreationDate</asp:Label>
                                                            <asp:ImageButton ID="whoswho" ImageAlign="AbsBottom" ToolTip="Who's who" runat="server"
                                                                ImageUrl="/hc_v4/img/ed_whoswho.gif" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                            <asp:Panel ID="pPLC" runat="server">
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="labelPLC" runat="server" CssClass="hc_pagetitle" Width="100%">Product Life Cycle</asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top" style="height: 1px">
                                <%--Removed the width tag from UltraWebToolbar to fix enlarge button issue by Radha S--%>
                                    <td>
                                        <igtbar:UltraWebToolbar ID="uwToolbarPLC" runat="server" ItemWidthDefault="50px"
                                            Height="24px" CssClass="hc_toolbar" BackgroundImage="" ImageDirectory="">
                                            <HoverStyle CssClass="hc_toolbarhover">
                                            </HoverStyle>
                                            <DefaultStyle CssClass="hc_toolbardefault">
                                            </DefaultStyle>
                                            <SelectedStyle CssClass="hc_toolbarselected">
                                            </SelectedStyle>
                                            <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                                            <Items>
                                                <igtbar:TBarButton Key="EditPLC" Text="Edit" Image="/hc_v4/Img/ed_edit.gif">
                                                </igtbar:TBarButton>
                                            </Items>
                                        </igtbar:UltraWebToolbar>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="lbErrorPLC" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pGridPLC" Visible="true" runat="server">
                                            <table style='border-collapse: collapse; width: 100%' cellspacing='0' bgcolor="white"
                                                border="1">
                                                <tr valign="top" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 130px'>
                                                        <asp:Label ID="lbPID" runat="server" Text="public introduction date"></asp:Label>
                                                    </td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lPID" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iPID" runat="server" />
                                                        <asp:Label ID="lPIDPMT" runat="server" Visible="false" Font-Bold="true" ForeColor="red"> (PMT)</asp:Label></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbPOD" runat="server" Text="obsolescence date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lObso" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iObso" runat="server" />
                                                        <asp:Label ID="lObsoPMT" runat="server" Visible="false" Font-Bold="true" ForeColor="red"> (PMT)</asp:Label></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbBlind" runat="server" Text="blind date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lBlind" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iBlind" runat="server" /></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbAnnouncement" runat="server" Text="announcement date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lAnnoun" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iAnnoun" runat="server" /></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbRemoval" runat="server" Text="removal date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lRemov" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iRemov" runat="server" /></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbSupport" runat="server" Text="end of support date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lSupport" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iSupport" runat="server" /></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbEOL" runat="server" Text="end of life date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lEOL" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iEOL" runat="server" /></td>
                                                </tr>
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        <asp:Label ID="lbDiscontinue" runat="server" Text="discontinue date"></asp:Label></td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lDiscontinue" runat="server">Label</asp:Label>
                                                        <asp:Image ID="iDiscontinue" runat="server" /></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        <asp:Panel ID="pBundlePLC" runat="server">
                                            <igtbl:UltraWebGrid ID="dgBundlePLC" runat="server" Width="100%" ImageDirectory="/ig_common/Images/"
                                                OnInitializeRow="dgBundlePLC_InitializeRow">
                                                <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                                                    TableLayout="Fixed" NoDataMessage="No data to display">
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
                                                    <igtbl:UltraGridBand BaseTableName="BundlePLC" Key="BundlePLC" BorderCollapse="Collapse">
                                                        <Columns>
                                                            <igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" Hidden="true">
                                                            </igtbl:UltraGridColumn>
                                                            <igtbl:UltraGridColumn Key="ItemNumber" BaseColumnName="ItemNumber" Width="100%"
                                                                HeaderText="Components" CellMultiline="Yes">
                                                                <CellStyle Wrap="true">
                                                                </CellStyle>
                                                            </igtbl:UltraGridColumn>
                                                            <igtbl:UltraGridColumn Key="PID" BaseColumnName="PID" Width="100px" HeaderText="product introduction date">
                                                                <CellStyle HorizontalAlign="Center">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="true" />
                                                            </igtbl:UltraGridColumn>
                                                            <igtbl:UltraGridColumn Key="POD" BaseColumnName="POD" Width="100px" HeaderText="obsolescence date">
                                                                <CellStyle HorizontalAlign="Center">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="true" />
                                                            </igtbl:UltraGridColumn>
                                                            <igtbl:UltraGridColumn Key="BlindDate" BaseColumnName="BlindDate" Width="100px" HeaderText="blind date">
                                                                <CellStyle HorizontalAlign="Center">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="true" />
                                                            </igtbl:UltraGridColumn>
                                                            <igtbl:UltraGridColumn Key="AnnDate" BaseColumnName="AnnouncementDate" Width="100px"
                                                                HeaderText="announcement date">
                                                                <CellStyle HorizontalAlign="Center">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="true" />
                                                            </igtbl:UltraGridColumn>
                                                            <igtbl:UltraGridColumn Key="RemDate" BaseColumnName="RemovalDate" Width="100px" HeaderText="removal date">
                                                                <CellStyle HorizontalAlign="Center">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="true" />
                                                            </igtbl:UltraGridColumn>
                                                        </Columns>
                                                    </igtbl:UltraGridBand>
                                                </Bands>
                                            </igtbl:UltraWebGrid>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <%--Removed the width tag from UltraWebToolbar to fix enlarge button issue by Radha S--%>
                            <asp:Panel ID="PanelMarketSegments" runat="server">
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="lbMS" runat="server" CssClass="hc_pagetitle" Width="100%">Market segments</asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top" style="height: 1px">
                                    <td>
                                        <igtbar:UltraWebToolbar ID="uwtoolbarMS" runat="server" CssClass="hc_toolbar" Height="24px"
                                            ItemWidthDefault="50px" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwtoolbarMS_ButtonClicked">
                                            <HoverStyle CssClass="hc_toolbarhover">
                                            </HoverStyle>
                                            <SelectedStyle CssClass="hc_toolbarselected">
                                            </SelectedStyle>
                                            <Items>
                                                <igtbar:TBarButton Key="save" Text="Save" Image="/hc_v4/Img/ed_save.gif">
                                                </igtbar:TBarButton>
                                            </Items>
                                            <DefaultStyle CssClass="hc_toolbardefault">
                                            </DefaultStyle>
                                        </igtbar:UltraWebToolbar>
                                    </td>
                                </tr>
                                <tr valign="top" style="height: 1px">
                                    <td>
                                        <asp:Label ID="lbErrorMS" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style='border-collapse: collapse; width: 100%' cellspacing='0' bgcolor="white"
                                            border="1">
                                            <asp:Panel ID="panelDefinedMS" runat="server">
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        Defined market segments</td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lbDefinedMS" runat="server"></asp:Label></td>
                                                </tr>
                                            </asp:Panel>
                                            <tr valign="middle" style="height: 1px">
                                                <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                    Applied market segments</td>
                                                <td class='editValueCell'>
                                                    <asp:Label ID="lbAppliedMS" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="10">
                                            <tr valign="top">
                                                <td>
                                                    <asp:CheckBox runat="server" ID="cbNotInheritedMS" Text="Not inherited market segments by fallback and inheritance mechanisms" />
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <igtbl:UltraWebGrid ID="dgMS" runat="server">
                                                        <DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
                                                            HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                                                            NoDataMessage="No data to display">
                                                            <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
                                                            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" Cursor="Hand" BackColor="LightGray"> <%--CssClass="gh">--%>
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </HeaderStyleDefault>
                                                            <FrameStyle CssClass="dataTable">
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>                                                            
                                                            </FrameStyle>
                                                            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
                                                            </ClientSideEvents>
                                                            <ActivationObject AllowActivation="False">
                                                            </ActivationObject>
                                                            <RowAlternateStyleDefault CssClass="uga">
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </RowAlternateStyleDefault>
                                                            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </RowStyleDefault>
                                                        </DisplayLayout>
                                                        <Bands>
                                                            <igtbl:UltraGridBand BaseTableName="" Key="" DataKeyField="">
                                                                <Columns>
                                                                    <igtbl:UltraGridColumn Width="50px">
                                                                    </igtbl:UltraGridColumn>
                                                                    <igtbl:UltraGridColumn HeaderText="Segment" Width="95px">
                                                                    </igtbl:UltraGridColumn>
                                                                    <igtbl:UltraGridColumn HeaderText="Country" Width="50px">
                                                                    </igtbl:UltraGridColumn>
                                                                </Columns>
                                                            </igtbl:UltraGridBand>
                                                        </Bands>
                                                    </igtbl:UltraWebGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <%--Removed the width tag to fix enlarge button by Radha S--%>
                            <asp:Panel ID="PanelPublishers" runat="server">
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="lbP" runat="server" CssClass="hc_pagetitle" Width="100%">Exclusive publishers</asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top" style="height: 1px">
                                    <td>
                                        <igtbar:UltraWebToolbar ID="uwtoolbarP" runat="server" CssClass="hc_toolbar" Height="24px"
                                            ItemWidthDefault="50px" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwtoolbarP_ButtonClicked">
                                            <HoverStyle CssClass="hc_toolbarhover">
                                            </HoverStyle>
                                            <SelectedStyle CssClass="hc_toolbarselected">
                                            </SelectedStyle>
                                            <Items>
                                                <igtbar:TBarButton Key="save" Text="Save" Image="/hc_v4/Img/ed_save.gif">
                                                </igtbar:TBarButton>
                                            </Items>
                                            <DefaultStyle CssClass="hc_toolbardefault">
                                            </DefaultStyle>
                                        </igtbar:UltraWebToolbar>
                                    </td>
                                </tr>
                                <tr valign="top" style="height: 1px">
                                    <td>
                                        <asp:Label ID="lbErrorP" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style='border-collapse: collapse; width: 100%' cellspacing='0' bgcolor="white"
                                            border="1">
                                            <asp:Panel ID="panelDefinedP" runat="server">
                                                <tr valign="middle" style="height: 1px">
                                                    <td class='editLabelCell' style='font-weight: bold; width: 200px'>
                                                        Selected publishers</td>
                                                    <td class='editValueCell'>
                                                        <asp:Label ID="lbDefinedP" runat="server"></asp:Label></td>
                                                </tr>
                                            </asp:Panel>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="10">
                                            <tr valign="top">
                                                <td>
                                                    <asp:CheckBox runat="server" ID="cbNotInheritedP" Text="Not inherited publishers by fallback mechanism" />
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td>
                                                    <igtbl:UltraWebGrid ID="dgP" runat="server">
                                                        <DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
                                                            HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                                                            NoDataMessage="No data to display">
                                                            <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
                                                            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" Cursor="Hand" BackColor="LightGray"> <%--CssClass="gh">--%>
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </HeaderStyleDefault>
                                                            <FrameStyle CssClass="dataTable">
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </FrameStyle>
                                                            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
                                                            </ClientSideEvents>
                                                            <ActivationObject AllowActivation="False">
                                                            </ActivationObject>
                                                            <RowAlternateStyleDefault CssClass="uga">
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </RowAlternateStyleDefault>
                                                            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                                </BorderDetails>
                                                            </RowStyleDefault>
                                                        </DisplayLayout>
                                                        <Bands>
                                                            <igtbl:UltraGridBand BaseTableName="" Key="" DataKeyField="">
                                                                <Columns>
                                                                    <igtbl:UltraGridColumn HeaderText="" Key="" Width="50px" BaseColumnName="">
                                                                    </igtbl:UltraGridColumn>
                                                                    <igtbl:UltraGridColumn HeaderText="Publisher" Key="" Width="95px" BaseColumnName="">
                                                                    </igtbl:UltraGridColumn>
                                                                    <igtbl:UltraGridColumn HeaderText="Country" Width="50px">
                                                                    </igtbl:UltraGridColumn>
                                                                </Columns>
                                                            </igtbl:UltraGridBand>
                                                        </Bands>
                                                    </igtbl:UltraWebGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel ID="pContent" runat="server">
                            <tr valign="top">
                                <td>
                                    <asp:Label ID="lbContent" runat="server" CssClass="hc_pagetitle" Width="100%">Content</asp:Label>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                    <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" CssClass="hc_toolbar" Height="24px"
                                        ItemWidthDefault="25px" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwToolbarContent_ButtonClicked">
                                        <HoverStyle CssClass="hc_toolbarhover">
                                        </HoverStyle>
                                        <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click2">
                                        </ClientSideEvents>
                                        <SelectedStyle CssClass="hc_toolbarselected">
                                        </SelectedStyle>
                                        <Items>
                                            <igtbar:TBarButton Key="legend" runat="server" Text="Legend" Image="/hc_v4/img/ed_help.gif">
                                                <DefaultStyle Width="80px">
                                                </DefaultStyle>
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator></igtbar:TBSeparator>
                                            <igtbar:TBLabel Text="Filter">
                                                <DefaultStyle Font-Bold="True" Width="40px">
                                                </DefaultStyle>
                                            </igtbar:TBLabel>
                                            <igtbar:TBCustom Width="150px" Key="filterField" runat="server">
                                                <asp:TextBox Width="150px" CssClass="Search" ID="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
                                            </igtbar:TBCustom>
                                            <igtbar:TBarButton Image="/hc_v4/img/ed_search.gif" Key="filter">
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator></igtbar:TBSeparator>
                                            <igtbar:TBarButton ToggleButton="True" Key="OnlyLocalizable" SelectedImage="/hc_v4/img/ed_checked.jpg"
                                                Text="Localizable content only" Image="/hc_v4/img/ed_unchecked.jpg">
                                                <SelectedStyle ForeColor="Black">
                                                </SelectedStyle>
                                                <DefaultStyle Width="180px">
                                                </DefaultStyle>
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator></igtbar:TBSeparator>
                                            <igtbar:TBarButton ToggleButton="True" Key="SingleView" Text="Single view">
                                                <DefaultStyle Width="130px">
                                                </DefaultStyle>
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator></igtbar:TBSeparator>
                                            <igtbar:TBarButton Key="Export" ToolTip="Export current view" Image="/hc_v4/img/ed_download.gif">
                                            </igtbar:TBarButton>
                                        </Items>
                                        <DefaultStyle CssClass="hc_toolbardefault">
                                        </DefaultStyle>
                                    </igtbar:UltraWebToolbar>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                    <asp:Label ID="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/"
                                        OnInitializeRow="dg_InitializeRow">
                                        <DisplayLayout NoDataMessage="No data to display" TableLayout="Fixed" Name="dg" RowSelectorsDefault="No"
                                            EnableInternalRowsManagement="True" BorderCollapseDefault="Separate" Version="4.00"
                                            AutoGenerateColumns="False">
                                            <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
                                            <HeaderStyleDefault HorizontalAlign="Center" BackColor="LightGray" Font-Bold="true" VerticalAlign="Middle" Cursor="Default"> <%--CssClass="gh"--%>
                                                <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid">
                                                </BorderDetails>
                                            </HeaderStyleDefault>
                                        </DisplayLayout>
                                        <Bands>
                                            <igtbl:UltraGridBand Key="Roles" DataKeyField="Id" BorderCollapse="Collapse" BaseTableName="Roles">
                                                <Columns>
                                                    <igtbl:UltraGridColumn Key="Index" Hidden="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="CId" BaseColumnName="CultureTypeId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Tag" BaseColumnName="Tag" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Path" BaseColumnName="Path" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Rtl" BaseColumnName="Rtl" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ContainerId" Hidden="true" BaseColumnName="ContainerId">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ModifierId" Hidden="true" BaseColumnName="ModifierId">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="CultureCode" Hidden="true" BaseColumnName="CultureCode">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ContainerType" ServerOnly="True" BaseColumnName="ContainerType">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="IsResource" BaseColumnName="IsResource" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ReadOnly" ServerOnly="True" BaseColumnName="ReadOnly">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ParentId" Hidden="True" BaseColumnName="ParentId">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="LevelId" BaseColumnName="LevelId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="SourceCode" BaseColumnName="CultureCode" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="IsBoolean" BaseColumnName="IsBoolean" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="IsResource" BaseColumnName="IsResource" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="IsMandatory" BaseColumnName="IsMandatory" Hidden="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="InputFormContainerId" BaseColumnName="InputFormContainerId"
                                                        Hidden="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="IsBoolean" BaseColumnName="IsBoolean" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Inherited" ServerOnly="True" BaseColumnName="Inherited">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="InheritanceMethodId" ServerOnly="True" BaseColumnName="InheritanceMethodId">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ReadOnly" ServerOnly="True" BaseColumnName="ReadOnly">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Overrides" ServerOnly="True" BaseColumnName="Overrides">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Localizable" ServerOnly="True" BaseColumnName="Localizable">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="HasFallback" ServerOnly="True" BaseColumnName="HasFallback">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Globalizable" ServerOnly="True" BaseColumnName="_Globalizable">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ContainerGroup" BaseColumnName="Path" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ItemName" BaseColumnName="ItemName" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ContainerName" BaseColumnName="ContainerName" Hidden="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Label" Width="200px" BaseColumnName="ContainerLabel"
                                                        HeaderText="Container" CellMultiline="Yes">
                                                        <CellStyle Wrap="True" CssClass="ptb1">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Value" Width="100%" BaseColumnName="ChunkValue" HeaderText="Value"
                                                        CellMultiline="Yes" AllowGroupBy="No">
                                                        <CellStyle Wrap="True">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                </Columns>
                                            </igtbl:UltraGridBand>
                                        </Bands>
                                    </igtbl:UltraWebGrid>
                                    <br />
                                </td>
                            </tr>
                            <asp:Panel ID="pUPC" runat="server">
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="label1" runat="server" CssClass="hc_pagetitle" Width="100%">Available options</asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top" style="height: 1px">
                                    <td>
                                        <asp:Label ID="lbErrorUPC" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        <igtbl:UltraWebGrid ID="dgUPC" runat="server" Width="100%" UseAccessibleHeader="False">
                                            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                                                Name="dgUPC" TableLayout="Fixed" NoDataMessage="No data to display">
                                                <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" Cursor="Hand" BackColor="LightGray"> <%--CssClass="gh">--%>
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                    </BorderDetails>
                                                </HeaderStyleDefault>
                                                <FrameStyle Width="100%" CssClass="dataTable">
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                    </BorderDetails>
                                                </FrameStyle>
                                                <RowAlternateStyleDefault CssClass="uga">
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                    </BorderDetails>
                                                </RowAlternateStyleDefault>
                                                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                    </BorderDetails>
                                                </RowStyleDefault>
                                            </DisplayLayout>
                                            <Bands>
                                                <igtbl:UltraGridBand BaseTableName="UPC" Key="UPC" BorderCollapse="Collapse">
                                                    <Columns>
                                                        <igtbl:UltraGridColumn HeaderText="UPC Code" Key="UPCCode" BaseColumnName="UPC" Width="150px">
                                                        </igtbl:UltraGridColumn>
                                                        <igtbl:UltraGridColumn HeaderText="Option code" Key="OptionCode" BaseColumnName="Opt"
                                                            Width="150px">
                                                        </igtbl:UltraGridColumn>
                                                        <igtbl:UltraGridColumn HeaderText="Option description" Key="OptionDescription" BaseColumnName="Description"
                                                            Width="100%" CellMultiline="Yes">
                                                            <CellStyle Wrap="true">
                                                            </CellStyle>
                                                        </igtbl:UltraGridColumn>
                                                    </Columns>
                                                </igtbl:UltraGridBand>
                                            </Bands>
                                        </igtbl:UltraWebGrid>
                                        <br />
                                    </td>
                                </tr>
                            </asp:Panel>
                        </asp:Panel>
                          <!-- added by kalai-->
                        <asp:Panel ID="pLinks" runat="server">
                            <%--<tr valign="top">
                                <td>
                                    <asp:Label ID="lbErrorTab" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                    <igtab:UltraWebTab ID="webTab" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                        BorderColor="#949878" Height="100%" DummyTargetUrl="/hc_v4/pleasewait.htm" Width="100%"
                                        SpaceOnRight="0" DynamicTabs="False" ThreeDEffect="False" BarHeight="0" LoadAllTargetUrls="False"
                                        DisplayMode="Scrollable" ImageDirectory="/hc_v4/inf/images/">
                                        <DefaultTabStyle Height="21px" Font-Size="xx-small" Font-Names="Microsoft Sans Serif"
                                            ForeColor="Black" BackColor="#FEFCFD">
                                            <Padding Bottom="0px" Top="1px"></Padding>
                                        </DefaultTabStyle>
                                        <RoundedImage LeftSideWidth="7" RightSideWidth="6" ShiftOfImages="2" SelectedImage="/hc_v4/inf/images/ig_tab_winXP1.gif"
                                            NormalImage="/hc_v4/inf/images/ig_tab_winXP3.gif" HoverImage="/hc_v4/inf/images/ig_tab_winXP2.gif"
                                            FillStyle="LeftMergedWithCenter"></RoundedImage>
                                        <SelectedTabStyle>
                                            <Padding Bottom="1px" Top="0px"></Padding>
                                        </SelectedTabStyle>
                                        <Tabs>
                                        </Tabs>
                                    </igtab:UltraWebTab>
                                </td>
                            </tr>--%>
                            <tr valign="top">
                                <td>
                                    <asp:Label ID="lbLinks" runat="server" CssClass="hc_pagetitle" Width="100%">Links</asp:Label>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                <%--Removed the width tag to fix enlarged button by Radha S--%>
                                    <igtbar:UltraWebToolbar ID="Ultrawebtoolbar1" runat="server" CssClass="hc_toolbar"
                                        ItemWidthDefault="120px" Height="24px" OnButtonClicked="Ultrawebtoolbar1_ButtonClicked">
                                        <HoverStyle CssClass="hc_toolbarhover">
                                        </HoverStyle>
                                        <SelectedStyle CssClass="hc_toolbarselected">
                                        </SelectedStyle>
                                        <Items>
                                            <igtbar:TBarButton Key="Save" Text="Apply changes" Image="/hc_v4/img/ed_save.gif"
                                                ToolTip="To save the new exclusions">
                                            </igtbar:TBarButton>
                                        </Items>
                                        <DefaultStyle CssClass="hc_toolbardefault">
                                        </DefaultStyle>
                                    </igtbar:UltraWebToolbar>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                    <asp:Label ID="lbErrorLinks" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <igtbl:UltraWebGrid ID="dgl" runat="server" Width="100%" UseAccessibleHeader="False"
                                        OnInitializeRow="dgl_InitializeRow">
                                        <%--Removed the css class and added inline properties to fix infragestic grid line by Radha S--%>
                                        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                                            Name="dgl" TableLayout="Fixed" NoDataMessage="No data to display">
                                            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="Gainsboro" Font-Bold="true"> <%--CssClass="gh2">--%>
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </HeaderStyleDefault>
                                            <FrameStyle Width="100%" CssClass="dataTable">
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </FrameStyle>
                                            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
                                            </ClientSideEvents>
                                            <ActivationObject AllowActivation="False">
                                            </ActivationObject>
                                            <RowAlternateStyleDefault CssClass="uga">
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </RowAlternateStyleDefault>
                                            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </RowStyleDefault>
                                        </DisplayLayout>
                                        <Bands>
                                            <igtbl:UltraGridBand BaseTableName="Links" Key="Links" BorderCollapse="Collapse">
                                                <Columns>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="LinkTypeId" BaseColumnName="LinkTypeId"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="LinkFrom" BaseColumnName="LinkFrom" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ItemId" BaseColumnName="ItemId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="InheritedItemId" BaseColumnName="InheritedItemId"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="SubItemId" BaseColumnName="SubItemId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="InheritedSubItemId" BaseColumnName="InheritedSubItemId"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="Family" BaseColumnName="ItemFamilyName"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="SubFamily" BaseColumnName="SubItemFamilyName"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ClassName" BaseColumnName="ItemClassName"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="SubClassName" BaseColumnName="SubItemClassName"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ItemName" BaseColumnName="ItemName" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ItemSKU" BaseColumnName="ItemSKU" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="SubItemName" BaseColumnName="SubItemName"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="SubItemSKU" BaseColumnName="SubItemSKU"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="Bidirectional" BaseColumnName="Bidirectional"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="LinkFrom" BaseColumnName="LinkFrom" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="IsInherited" BaseColumnName="IsInherited"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="CountryCode" BaseColumnName="CountryCode"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="FallbackCountryCode" BaseColumnName="FallbackCountryCode"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ExcludedCountry" BaseColumnName="ExcludedCountry"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ExcludedItem" BaseColumnName="ExcludedItem"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="Class" Key="Class" Width="150px" CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="SKU" Key="SKU" Width="100%" CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="300px" CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ImageCountry" Width="60px" HeaderText="Country">
                                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="E" Key="IsExcluded" Width="22px" Type="CheckBox"
                                                        DataType="System.Boolean" BaseColumnName="IsExcluded" AllowUpdate="Yes">
                                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                                        </CellStyle>
                                                        <Header Caption="E" Title="Excluded">
                                                        </Header>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="R" Key="IsRecommended" Width="22px" Type="CheckBox"
                                                        DataType="System.Boolean" BaseColumnName="Recommended" AllowUpdate="No">
                                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                                        </CellStyle>
                                                        <Header Caption="R" Title="Recommended">
                                                        </Header>
                                                    </igtbl:UltraGridColumn>
                                                </Columns>
                                            </igtbl:UltraGridBand>
                                        </Bands>
                                    </igtbl:UltraWebGrid>
                                    <br />
                                </td>
                            </tr>
                        </asp:Panel>
                      
                        <!-- Kalai code ends here-->
                        <asp:Panel ID="pCross" runat="server">
                            <tr valign="top" style="height: 1px">
                                <td>
                                    <asp:Label ID="lbCrossSell" runat="server" CssClass="hc_pagetitle" Width="100%">Cross Sell</asp:Label>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                <%--Removed the width tag to fix enlarged button by Radha S--%>
                                    <igtbar:UltraWebToolbar ID="Ultrawebtoolbar2" runat="server" CssClass="hc_toolbar"
                                        ItemWidthDefault="120px" Height="24px" OnButtonClicked="Ultrawebtoolbar2_ButtonClicked">
                                        <HoverStyle CssClass="hc_toolbarhover">
                                        </HoverStyle>
                                        <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                                        <SelectedStyle CssClass="hc_toolbarselected">
                                        </SelectedStyle>
                                        <Items>
                                            <igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" ToolTip="To add cross sell">
                                                <DefaultStyle Width="80px">
                                                </DefaultStyle>
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
                                            <igtbar:TBarButton Key="Save" Text="Apply changes" Image="/hc_v4/img/ed_save.gif"
                                                ToolTip="To save the new exclusions">
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
                                            <igtbar:TBarButton Key="Delete" Text="Delete selected" Image="/hc_v4/img/ed_delete.gif"
                                                ToolTip="To delete selected cross sell">
                                            </igtbar:TBarButton>
                                            <igtbar:TBSeparator Key="ResumeInheritanceSep"></igtbar:TBSeparator>
                                            <igtbar:TBarButton Key="ResumeInheritance" ToolTip="To delete all the local links (eligible & ineligible links) and resume inheritance"
                                                Text="Resume Inheritance">
                                                <DefaultStyle Width="180px">
                                                </DefaultStyle>
                                            </igtbar:TBarButton>
                                        </Items>
                                        <DefaultStyle CssClass="hc_toolbardefault">
                                        </DefaultStyle>
                                    </igtbar:UltraWebToolbar>
                                </td>
                            </tr>
                            <tr valign="top" style="height: 1px">
                                <td>
                                    <asp:Label ID="lbErrorCrossSell" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                <%--Removed the css class and added inline properties to fix infragestic grid line by Radha S--%>
                                    <igtbl:UltraWebGrid ID="dgCrossSell" runat="server" Width="100%" OnInitializeRow="dgCrossSell_InitializeRow">
                                        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                                            Name="dgCrossSell" TableLayout="Fixed" NoDataMessage="No data to display">
                                            <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="Gainsboro" Font-Bold="true"> <%--CssClass="gh2">--%>
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </HeaderStyleDefault>
                                            <FrameStyle Width="100%" CssClass="dataTable">
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </FrameStyle>
                                            <RowAlternateStyleDefault CssClass="uga">
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </RowAlternateStyleDefault>
                                            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                                </BorderDetails>
                                            </RowStyleDefault>
                                            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
                                                InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
                                        </DisplayLayout>
                                        <Bands>
                                            <igtbl:UltraGridBand BaseTableName="Links" Key="Links" BorderCollapse="Collapse">
                                                <Columns>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ItemId" BaseColumnName="ItemId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="SubItemId" BaseColumnName="SubItemId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="InheritedItemId" BaseColumnName="InheritedItemId"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="IsInherited" BaseColumnName="IsInherited"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="UserId" BaseColumnName="ModifierId" ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ExcludedCountry" BaseColumnName="ExcludedCountry"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="ExcludedItem" BaseColumnName="ExcludedItem"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="CountryCode" BaseColumnName="CountryCode"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Hidden="true" Key="FallbackCountryCode" BaseColumnName="FallbackCountryCode"
                                                        ServerOnly="True">
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:TemplatedColumn Key="Select" Width="20px">
                                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="True">
                                                        </CellStyle>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="g_ca" onclick="javascript: return g_su(this);" runat="server"></asp:CheckBox>
                                                        </HeaderTemplate>
                                                        <CellTemplate>
                                                            <asp:CheckBox ID="g_sd" onclick="javascript: return g_su(this);" runat="server"></asp:CheckBox>
                                                        </CellTemplate>
                                                    </igtbl:TemplatedColumn>
                                                    <igtbl:UltraGridColumn Key="Class" Width="100px" BaseColumnName="SubItemClassName"
                                                        CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="Family" Width="120px" BaseColumnName="SubItemFamilyName"
                                                        CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="Sku" Key="ItemSku" Width="100%" BaseColumnName="SubItemSKU"
                                                        CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" Width="300px" BaseColumnName="SubItemName"
                                                        CellMultiline="Yes">
                                                        <CellStyle Wrap="true">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn Key="ImageCountry" Width="60px" HeaderText="Country">
                                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                                        </CellStyle>
                                                    </igtbl:UltraGridColumn>
                                                    <igtbl:UltraGridColumn HeaderText="E" Key="IsExcluded" Width="22px" Type="CheckBox"
                                                        DataType="System.Boolean" BaseColumnName="IsExcluded" AllowUpdate="Yes">
                                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                                        </CellStyle>
                                                        <Header Caption="E" Title="Excluded">
                                                        </Header>
                                                    </igtbl:UltraGridColumn>
                                                </Columns>
                                            </igtbl:UltraGridBand>
                                        </Bands>
                                    </igtbl:UltraWebGrid>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr style="height: 10px">
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" name="action" id="action" />
    <input type="hidden" name="txtSortColPos" id="txtSortColPos" runat="server" value="15" />
</asp:Content>
