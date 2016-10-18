<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoadMap_CountryViewList.ascx.cs" Inherits="RoadMap_CountryViewList"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
	<style type="text/css">
    .re { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #336699; white-space: nowrap; color: black }
    .rl { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #FF6600; white-space: nowrap; color: black }
    .ro { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #000000; white-space: nowrap; color: white }
    .ri { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #FF0000; white-space: nowrap; color: black }
    .rrv { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #999999; white-space: nowrap; color: black }
    .rcv { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #009900; white-space: nowrap; color: black }
    .b { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; }
    .adfd { background-image: url(/hc_v4/img/AD.gif); background-repeat: no-repeat; background-position: center; }
    .rdod { background-image: url(/hc_v4/img/RD.gif); background-repeat: no-repeat; background-position: center; }
    .rdobadfd { background-image: url(/hc_v4/img/ARD.gif); background-repeat: no-repeat; background-position: center; }
  </style>
	<script type="text/javascript" language="javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent){}
	  var co = "";
	  function EditPLC(i, col, row, dg, c)
	  {
	    if (c!='' && c!=null){
	      co = c;
	    }
 	    var url = sRootUrl + "UI/Acquire/QDE/qde_CountryViewEditPLC.aspx?i=" + i + "&col=" + col + "&row=" + row + "&dg=" + dg + "&co=" + c
 	    var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
 	    url += '#target';
 	    winChunkEdit = OpenModalWindow(url,'plcwindow', 250, 450, 'yes', 'yes', 'no')
 	    return false;
 	   }
	  var color="darkblue";
	  function UpdatePLC(dgName, col, row, pid, pod, c)	  
	  {
	    var dg = igtbl_getGridById(dgName);	    
	    var commonStart = "<nobr><a href='javascript://' onclick='EditPLC(" + dg.Rows.getRow(row).getCellFromKey("ItemId").getValue() + ", " +
         col + "," +
         row + ",\"" +
         dgName + "\", \"" + co +"\");color=\"" + color +"\";'><u><font color=\"" + color +"\"'>";
      var commonEnd = "</font></u></a></nobr>";
	    var newUrlPID =  commonStart + pid + commonEnd;
      var newUrlPOD =  commonStart  + pod + commonEnd;
	    dg.Rows.getRow(row).getCell(col).getElement().innerHTML = newUrlPID;
	    dg.Rows.getRow(row).getCell(col+2).getElement().innerHTML = newUrlPOD;
	  }	
	  </script>

  <script type="text/javascript" language="javascript">
    document.body.scroll = "no";
  </script>
	<table class="main b">
		<tr valign="top" style="height:1px">
			<td class="b">
			  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<Items>
						<igtbar:TBarButton Key="Back" ToolTip="Back to the parameters" Text="Back" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator Key="BackSep" />
						<igtbar:TBarButton Key="Promote" ToolTip="Promote to country" Text="Promote" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator Key="PromoteSep" />
						<igtbar:TBarButton Key="GraphicView" ToolTip="Switch view graphic" Text="Switch view" Image="/hc_v4/img/LinkFrom.gif">
						  <DefaultStyle Width="100px"></DefaultStyle>
						</igtbar:TBarButton>
						<igtbar:TBSeparator Key="GraphicViewSep" />
						<igtbar:TBLabel Text="Filter"><DefaultStyle Width="40px" Font-Bold="True"></DefaultStyle></igtbar:TBLabel>
						<igtbar:TBCustom Width="150px" Key="filterField">
							<asp:TextBox CssClass="Search" Visible="True" runat="server" Width="150px" Id="txtFilter" MaxLength="50"></asp:TextBox>
						</igtbar:TBCustom>
						<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif"><DefaultStyle Width="25px"></DefaultStyle></igtbar:TBarButton>
					</Items>
				</igtbar:ultrawebtoolbar></td>
		</tr>				
		<tr valign="top" style="height: 1px">
		  <td class="b">
		    <asp:Label ID="lbResult" runat="server" CssClass="hc_success" Visible="false"></asp:Label>
		  </td>				
		</tr>
		<tr valign="top" style="height: 1px">
		  <td class="b">
		    <asp:Label ID="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
		  </td>				
		</tr>
	  <tr valign="top">
	    <td class="b">
		    <div id="divDg" style="OVERFLOW: auto; width: 100%; height:100%">
          <igtbl:UltraWebGrid id="dg" runat="server" Width="100%" Height="100%" OnInitializeRow="dg_InitializeRow">
						<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
							HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
							NoDataMessage="No input forms attached to this container">
							<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh" Height="30px">
								<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
							</HeaderStyleDefault>
							<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
							<RowAlternateStyleDefault CssClass="uga" Height="30px"></RowAlternateStyleDefault>
							<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd" Height="30px"></RowStyleDefault>
						</DisplayLayout>
					  <Bands>
						  <igtbl:UltraGridBand BaseTableName="ItemLevels" Key="ItemLevels" BorderCollapse="Collapse" DataKeyField="LevelId">
							  <Columns>
								  <igtbl:UltraGridColumn Key="ItemId" Hidden="True" BaseColumnName="ItemId"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="CountryCode" Hidden="True" BaseColumnName="CountryCode"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="CountryName" Hidden="True" BaseColumnName="CountryName"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="FullDateType" Hidden="True" BaseColumnName="FullDateType"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="ObsoleteDateType" Hidden="True" BaseColumnName="ObsoleteDateType"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="BlindDateType" Hidden="True" BaseColumnName="BlindDateType"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="AnnouncementDateType" Hidden="True" BaseColumnName="AnnouncementDateType"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="RemovalDateType" Hidden="True" BaseColumnName="RemovalDateType"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="BlindDate" Hidden="True" BaseColumnName="BlindDate"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="RDIsDifOB" Hidden="True" BaseColumnName="RDIsDifOB"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="ADIsDifFD" Hidden="True" BaseColumnName="ADIsDifFD"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="WorkflowStatus" Hidden="True" BaseColumnName="WorkflowStatus"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="Country" Key="Country" Width="150px"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="ItemName"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="SKU" Key="SKU" Width="80px" BaseColumnName="ItemSku"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="Status" Key="Status" Width="100px" BaseColumnName="Status"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="PID" Key="PID" Width="80px" BaseColumnName="FullDate" DataType="System.DateTime"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="PromotePID" Width="30px" Type="CheckBox" AllowUpdate="Yes" DataType="System.Boolean" AllowRowFiltering="false">
								    <CellStyle VerticalAlign="Middle" HorizontalAlign="Center"></CellStyle>
								    <Header Caption="P" Title="Check to promote"></Header>
								  </igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="POD" Key="POD" Width="80px" BaseColumnName="ObsoleteDate" DataType="System.DateTime"></igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn Key="PromotePOD" Width="30px" Type="CheckBox" AllowUpdate="Yes" DataType="System.Boolean" AllowRowFiltering="false">
								    <CellStyle VerticalAlign="Middle" HorizontalAlign="Center"></CellStyle>
								    <Header Caption="P" Title="Check to promote"></Header>
								  </igtbl:UltraGridColumn>
								  <igtbl:UltraGridColumn HeaderText="Outstanding" Key="Outstanding" Width="80px" AllowRowFiltering="false"></igtbl:UltraGridColumn>
							  </Columns>
						  </igtbl:UltraGridBand>
					  </Bands>
			    </igtbl:UltraWebGrid>
	      </div>
	    </td>
	  </tr>
	  <tr valign="bottom" height="20px">
	    <td>
	      <br />
	      <table class="hc_toolbartitle" width="100%" cellpadding="0" cellspacing="0" border="0">
	        <tr valign="top" height="20px">
	          <td width="10px">&nbsp;</td>
		        <td class="ri" width="30px" nowrap>&nbsp;</td>
		        <td nowrap>&nbsp;Invalid&nbsp;</td>
		        <td class="rrv" width="30px" nowrap>&nbsp;</td>
		        <td nowrap>&nbsp;Region validated&nbsp;</td>
		        <td class="rcv" width="30px" nowrap>&nbsp;</td>
		        <td nowrap>&nbsp;Country validated&nbsp;</td>
		        <td class="rl" width="30px" nowrap>&nbsp;</td>
		        <td nowrap>&nbsp;Live&nbsp;</td>
		        <td class="ro" width="30px" nowrap>&nbsp;</td>
		        <td nowrap>&nbsp;Obsolete&nbsp;</td>
		        <td class="re" width="30px" nowrap>&nbsp;</td>
		        <td nowrap>&nbsp;Excluded&nbsp;</td>
	          <td width="100%"></td>
	        </tr>
	      </table>
	    </td>
	  </tr>
	  <tr valign="bottom" height="20px">
	    <td>
	      <br />
	      <table width="100%" cellpadding="0" cellspacing="0" border="0">
	        <tr valign="top" height="20px">
	          <td width="10px">&nbsp;</td>
	          <td nowrap><img alt="announcement date <> full date" src="/hc_v4/img/AD.gif" /></td>
	          <td nowrap>&nbsp;announcement date &lt;&gt; PID&nbsp;</td>
	          <td nowrap><img alt="removal date <> obsolete date" src="/hc_v4/img/RD.gif" /></td>
	          <td nowrap>&nbsp;removal date &lt;&gt; POD&nbsp;</td>
	          <td width="100%" nowrap>&nbsp;</td>
	        </tr>
	      </table>
	    </td>
	  </tr>
	</table>
<asp:HiddenField ID="hiCacheRefresh" runat="server">
</asp:HiddenField>
