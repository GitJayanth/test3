<%@ Control Language="c#" Inherits="HyperCatalog.UI.Acquire.QDE.controlPLC" CodeFile="controlPLC.ascx.cs" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<style type="text/css">
  .nav-loading{position:absolute;top:250px;left:400px;z-index:500;
               width:100px;height:50px;background:url(/hc_v4/img/loading-bg.png);
               font:bold 11px/50px Verdana,Geneva,Arial,Helvetica,sans-serif;
               text-align:center;color:#666;
               }
               .ro{font-style:italic;background-color:silver}
</style>
  <script type="text/javascript" language="javascript" src="controlplc.js"></script>
<script type="text/javascript" language="javascript">
		function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dg");
                 }
  </script>
<igsch:webdatechooser id="wd" Height="30px" Width="150px" runat="server" NullDateLabel=" ">
  <EditStyle Font-Size="8pt"></EditStyle>
  <CalendarLayout ShowTitle="False" ShowFooter="False"></CalendarLayout>
</igsch:webdatechooser>
<table border="0" cellspacing="0" cellpadding="0"  width="100%">
<tr valign="top"><td>
<%--Fix for QC# 7029 by Rekha Thomas. Added  OnUpdateCell event handler --%>
<igtbl:ultrawebgrid id="dg" Height="300px" Width="100%" runat="server" ImageDirectory="/hc_v4/ig/Images/"> <%--<OnUpdateCell="dg_UpdateCell">--%>
  <displaylayout NoDataMessage="No data to display" CellClickActionDefault="Edit" TableLayout="Fixed"
    Name="ctl00xdg" RowSelectorsDefault="No" SelectTypeCellDefault="Single" SelectTypeRowDefault="Single"
    Version="4.00" RowHeightDefault="18px" AllowSortingDefault="Yes" AutoGenerateColumns="False" BorderCollapseDefault="Separate" StationaryMargins="Header" UseFixedHeaders="True">
        <%-- Fix for QC# 7384 by Nisha Verma Removed the css class reference to gh. Wrote the styles directly in the headerStyleDefault tag to fix the issue --%>
                                <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
    <framestyle Height="300px" Width="100%" Cursor="Default">
  </framestyle>
    <clientsideevents AfterExitEditModeHandler="dg_AfterExitEditModeHandler" BeforeEnterEditModeHandler="dg_BeforeEnterEditModeHandler"></clientsideevents>
    <activationobject AllowActivation="False"></activationobject>
    <editcellstyledefault Font-Size="8pt" BorderStyle="None" Font-Names="Verdana" BorderWidth="0px"></editcellstyledefault>
   
   	<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd" BorderColor="LightGray">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt" />
                                </RowStyleDefault>
    <filteroptionsdefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
      <filterdropdownstyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
        CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px"
        Width="200px">
        <Padding Left="2px" />
      </filterdropdownstyle>
      <filterhighlightrowstyle BackColor="#151C55" ForeColor="White">
      </filterhighlightrowstyle>
    </filteroptionsdefault>
  </displaylayout>
  <bands>
    <igtbl:UltraGridBand Key="PLC" DataKeyField="ItemId"><%--Fix for QC# 7029 by Rekha Thomas. Added  DataKeyField --%>
      <columns>
        <igtbl:UltraGridColumn Width="20px" Key="ApplyChilds" HeaderText="">
          <Header Caption="">
          </Header>
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="240px" Key="Region" FooterText="" AllowUpdate="No" BaseColumnName="RegionName"
          Format="" EditorControlID="" HeaderText="Region">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="Err" HeaderText="" Width="0px" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="Blind" FooterText="" AllowUpdate="Yes" BaseColumnName="BlindDate"
          EditorControlID="wd" HeaderText="Blind" DataType="System.DateTime" Type="Custom">
          <cellstyle HorizontalAlign="Center"></cellstyle>
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="PID" FooterText="" AllowUpdate="Yes" BaseColumnName="FullDate"
          EditorControlID="wd" HeaderText="PID" DataType="System.DateTime" Type="Custom">
          <cellstyle HorizontalAlign="Center"></cellstyle>
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="POD" FooterText="" AllowUpdate="Yes" BaseColumnName="ObsoleteDate"
          EditorControlID="wd" HeaderText="POD" DataType="System.DateTime" Type="Custom">
          <cellstyle HorizontalAlign="Center"></cellstyle>
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="Announcement" FooterText="" AllowUpdate="Yes" BaseColumnName="AnnouncementDate"
          EditorControlID="wd" HeaderText="Announce" DataType="System.DateTime" Type="Custom">
          <cellstyle HorizontalAlign="Center"></cellstyle>      
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="Removal" FooterText="" AllowUpdate="Yes" BaseColumnName="RemovalDate"
          EditorControlID="wd" HeaderText="Remove" DataType="System.DateTime" Type="Custom">
          <cellstyle HorizontalAlign="Center"></cellstyle>
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="RegionType" FooterText="" BaseColumnName="RegionType" Format="" HeaderText="Type"
          Hidden="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="RegionCode" HeaderText="Code" Hidden="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="ParentCode" HeaderText="ParentCode" Hidden="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="Depth" HeaderText="Depth" Hidden="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="Upd" HeaderText="Upd" Hidden="True">
       </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="BlindLocked" BaseColumnName="BlindLocked" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="FullLocked" BaseColumnName="FullLocked" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="ObsoleteLocked" BaseColumnName="ObsoleteLocked" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="AnnouncementLocked" BaseColumnName="AnnouncementLocked" ServerOnly="True">
       </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="RemovalLocked" BaseColumnName="RemovalLocked" ServerOnly="True">
         </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="EndOfSupportLocked" BaseColumnName="EndOfSupportLocked" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="DiscontinueLocked" BaseColumnName="DiscontinueLocked" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="EndOfLifeLocked" BaseColumnName="EndOfLifeLocked" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="BlindDateType" BaseColumnName="BlindDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="FullDateType" BaseColumnName="FullDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="ObsoleteDateType" BaseColumnName="ObsoleteDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="AnnouncementDateType" BaseColumnName="AnnouncementDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="RemovalDateType" BaseColumnName="RemovalDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="EndOfSupportDateType" BaseColumnName="EndOfSupportDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="DiscontinueDateType" BaseColumnName="DiscontinueDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="EndOfLifeDateType" BaseColumnName="EndOfLifeDateType" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="Excluded" BaseColumnName="Excluded" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Key="ExcludeModifierId" BaseColumnName="ExcludeModifierId" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="EndOfSupport" BaseColumnName="EndOfSupportDate"
          DataType="System.DateTime" Type="Custom" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="Discontinue" BaseColumnName="DiscontinueDate"
          DataType="System.DateTime" Type="Custom" ServerOnly="True">
        </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn Width="90px" Key="EndOfLife" BaseColumnName="EndOfLifeDate"
          DataType="System.DateTime" Type="Custom" ServerOnly="True">
        </igtbl:UltraGridColumn>
      </columns>
    </igtbl:UltraGridBand>
  </bands>
</igtbl:ultrawebgrid>
    <div id="divWait" class="nav-loading" style="display:none;visibility:hidden"><center>
      <table border="0" style="height: 100%"><tr valign="middle"><td style="width: 1px"><img alt="" src="/hc_v4/img/ed_wait.gif" /></td><td>processing</td></tr></table>
      </center>
    </div>
    </td>
  </tr>
</table>
