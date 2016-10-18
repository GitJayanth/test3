<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="qde_CountryViewEditPLC.aspx.cs" Inherits="HyperCatalog.UI.Acquire.QDE.qde_CountryViewEditPLC"%>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">PLC edit</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" language="javascript">
    
   var mPID = 'The ' + strPID + ' is always earlier than the ' + strPOD + '.';
   var mPOD = 'The ' + strPOD + ' is always later than the ' + strPID + '.';
   var mBlind = '<li>The ' + strBlind + ' can only be changed to a later date as given by the source.</li>' +
                '<li>The ' + strBlind + ' should be always before than ' + strPID + '.</li>';
   var mAnnouncement = '<li>The ' + strAnnouncement + ' can only be changed to a later date as given by the source.</li>' +
                       '<li> In the case no ' + strAnnouncement + ' is given, the ' + strAnnouncement + ' can only be set to a date which is maximum 30 days prior the ' + strPID + '.</li>';
   var mRemoval = 'The ' + strRemoval + ' can only occur after the ' + strPOD + '.';
    
  function DateAdd(timeU,byMany,dateObj)
  {
	  var millisecond=1;
	  var second=millisecond*1000;
	  var minute=second*60;
	  var hour=minute*60;
	  var day=hour*24;
	  var year=day*365;

	  var newDate;
	  var dVal=dateObj.valueOf();
	  switch(timeU) {
		  case "ms": newDate=new Date(dVal+millisecond*byMany); break;
		  case "s": newDate=new Date(dVal+second*byMany); break;
		  case "mi": newDate=new Date(dVal+minute*byMany); break;
		  case "h": newDate=new Date(dVal+hour*byMany); break;
		  case "d": newDate=new Date(dVal+day*byMany); break;
		  case "y": newDate=new Date(dVal+year*byMany); break;
	  }
	  return newDate;
  }
 	
 	function ReloadParent()
 	{
	  if (top.opener){
	    top.opener.document.getElementById("action").value = "reload";
      top.opener.document.forms[0].submit();
    }
    top.window.close();
	}
 	
 	var dg = '';
 	var col=0;
 	var row=0;
  function RefreshGrid()
 	{
	  if (top.opener && dg!=''){
      var obso = igdrp_getComboById(wdcObsoleteDate).getText();
      var pid = igdrp_getComboById(wdcPID).getText();
	    top.opener.UpdatePLC(dg, col, row, pid, obso);
    }
	}
  function uwToolbar_Click(oToolbar, oButton, oEvent)
	{
	  if (!MandatoryFieldMissing())
	  {
      if (oButton.Key == 'Save')
		  {
        var canSave = true;
        var obso = igdrp_getComboById(wdcObsoleteDate).getValue();
        var pid = igdrp_getComboById(wdcPID).getValue();
        var blind = igdrp_getComboById(wdcBlindDate).getValue();
        var remov = igdrp_getComboById(wdcRemovalDate).getValue();
        var ann = igdrp_getComboById(wdcAnnouncementDate).getValue();
        var _pid30 = DateAdd('d', -30, pid);

		    // Obsolete date rule. obso > pid
        if ((obso <= pid) && (obso != null) && (pid != null))
        {
          alert('The ' + strPOD + ' should be always later than the ' + strPID + '.');
          canSave = false;
        }
		    // Blind date rule.
		    // if regional date, region blind date > new blind date
        if ((pblind != null) && (blind != null) && (blindR == 'R') && (pblind > blind))
        {
          alert('The ' + strBlind + ' can only be changed to a later date as given by the source.');
          canSave = false;          
        }
        // blind <= pid
        if ((blind > pid) && (blind != null) && (pid != null))
        {
          alert('The ' + strBlind + ' should be always before than the ' + strPID + '.');
          canSave = false;
        }
        // Removal date rule. removal > obso
        if ((remov <= obso) && (remov != null) && (obso != null))
        {
          alert('The ' + strRemoval + ' can only occur after the ' + strPOD + '.');
          canSave = false;
        }
        // Announcement date rule. 
        // if regional date, region announcement date > new announcement date
        // if country date, announcement date >= (pid - 30 days)
        if ((pann != null) && (ann != null) && (annR == 'R') && (pann > ann))
        {
          alert('The ' + strAnnouncement + ' can only be changed to a later date as given by the source.');
          canSave = false;          
        }
        if ((ann != null) && (pann == null) && (ann < _pid30))
        {
          alert('In the case no ' + strAnnouncement + ' is given,\n\r the ' + strAnnouncement + ' can only be set to a date which is maximum 30 days prior the ' + strPID + '.');
          canSave = false;  
        }
        if (!canSave)
        {
          oEvent.cancelPostBack = true;
        }
        else{
          RefreshGrid();
        }
	    }
	  }
	  else
	  {
	    oEvent.cancelPostBack = true;
	  }
		if (oButton.Key == 'Close')
		{
			oEvent.cancelPostBack = true;
			window.close();
		}
	}

function Lock(obj, cb){
  if (document.getElementById(cb))
  {
    document.getElementById(cb).checked = obj.checked;
  }
}

function wdcPID_ValueChanged(oDropDown, newValue, oEvent){
  document.getElementById(cbPIDL).checked = true;
  document.getElementById(cboxPID).checked = true;
}
function wdcObsoleteDate_ValueChanged(oDropDown, newValue, oEvent){
  document.getElementById(cbObsoL).checked = true;
  document.getElementById(cboxObsoleteDate).checked = true;
}
function wdcBlindDate_ValueChanged(oDropDown, newValue, oEvent){
  document.getElementById(cbBlindL).checked = true;
  document.getElementById(cboxBlindDate).checked = true;
}
function wdcAnnouncementDate_ValueChanged(oDropDown, newValue, oEvent){
  document.getElementById(cbAnnL).checked = true;
  document.getElementById(cboxAnnouncementDate).checked = true;
}	
function wdcRemovalDate_ValueChanged(oDropDown, newValue, oEvent){
  document.getElementById(cbRemovL).checked = true;
  document.getElementById(cboxRemovalDate).checked = true;
}
</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

    <table style="width: 100%; height: 100%" border="0" cellpadding="0 " cellspacing="0">
      <tr height="1" valign="top">
        <td>
          <igtbar:UltraWebToolbar ID="uwToolbarTitle" runat="server" Width="100%" CssClass="hc_toolbartitle"
            BackgroundImage="" ImageDirectory="" ItemWidthDefault="">
            <Items>
              <igtbar:TBLabel Key="Sku" Text="Sku">
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
        <%--Removed width property to fix enlarge buttons by Radha S--%>
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
            CssClass="hc_toolbar" BackgroundImage="" ImageDirectory="" OnButtonClicked="uwToolbar_ButtonClicked">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
            <Items>
              <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/Img/ed_save.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="SaveSep" />
              <igtbar:TBarButton Key="Close" Text="Close" ToolTip="Close window" Image="/hc_v4/Img/ed_cancel.gif">
              </igtbar:TBarButton>
            </Items>
          </igtbar:UltraWebToolbar>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="wdcPID"
                  ErrorMessage="PID is mandatory" SetFocusOnError="True"></asp:RequiredFieldValidator>        </td>
      </tr>
      <tr valign="top">
        <td>
          <asp:Label ID="lError" runat="server" Text="lError" CssClass="hc_error"></asp:Label>
          <table style="border: 1px; border-color: black; border-collapse: collapse;" border="1"
            cellpadding="2" cellspacing="0">
            <thead height="1">
              <th class="gh" colspan="3">
              </th>
              <th class="gh" width="50px">
                <asp:Image ID="iLocked" runat="server" ImageUrl="/hc_v4/img/locked_status.gif" /></th>
              <th class="gh">
                <asp:Label ID="lPromote" runat="server" Text="Promote to country"></asp:Label></th>
            </thead>
            <tr valign="middle" height="1">
              <td class="ptb1" width="160" style="height: 1px">
                <a onmouseover="doTooltip(event, mPID)"
                  onmouseout="hideTip()" href="javascript://" style="border: 0px">
                  <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
                </a><asp:Label ID="lbPID" runat="server" Text="public introduction date"></asp:Label>
              </td>
              <td class="ptb3">
                <center><asp:Image ID="iRegionPID" runat="server" /></center></td>
              <td style="height: 1px" class="ptb3">
                <igsch:WebDateChooser ID="wdcPID" runat="server" NullDateLabel="">
                  <ClientSideEvents ValueChanged="wdcPID_ValueChanged" />
                  <CalendarLayout ShowFooter="False" ShowTitle="False">
                  </CalendarLayout>
                </igsch:WebDateChooser>
                </td>
              <td width="50px" class="ptb3">
                <center><asp:CheckBox ID="cbPIDL" runat="server" OnClick="Lock(this, cboxPID)"/></center></td>
              <td class="ptb3">
                <center><asp:CheckBox ID="cboxPID" runat="server"/></center>
                </td>
            </tr>
            <tr valign="middle" height="1">
              <td class="ptb1" width="160" style="height: 1px">
                <a onmouseover="doTooltip(event,mPOD)"
                  onmouseout="hideTip()" href="javascript://" style="border: 0px">
                  <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
                </a><asp:Label ID="lbPOD" runat="server" Text="obsolescence date"></asp:Label></td>
              <td class="ptb3">
                <center><asp:Image ID="iRegionObso" runat="server" /></center></td>
              <td class="ptb3">
                <igsch:WebDateChooser ID="wdcObsoleteDate" runat="server" NullDateLabel="">
                  <CalendarLayout ShowFooter="False" ShowTitle="False">
                  </CalendarLayout>
                  <ClientSideEvents ValueChanged="wdcObsoleteDate_ValueChanged" />
                </igsch:WebDateChooser>
              </td>
              <td width="50px" class="ptb3">
                <center><asp:CheckBox ID="cbObsoL" runat="server" OnClick="Lock(this, cboxObsoleteDate)"/></center></td>
              <td class="ptb3">
                <center><asp:CheckBox ID="cboxObsoleteDate" runat="server"/></center>
                </td>
            </tr>
            <tr valign="middle" height="1">
              <td class="ptb1" width="160" style="height: 1px">
                <a onmouseover="doTooltip(event,mBlind)" onmouseout="hideTip()" href="javascript://"
                  style="border: 0px">
                  <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
                </a><asp:Label ID="lbBlind" runat="server" Text="blind date"></asp:Label></td>
              <td class="ptb3">
                <center><asp:Image ID="iRegionBlind" runat="server" /></center></td>
              <td class="ptb3">
                <igsch:WebDateChooser ID="wdcBlindDate" runat="server" NullDateLabel="">
                  <CalendarLayout ShowFooter="False" ShowTitle="False">
                  </CalendarLayout>
                  <ClientSideEvents ValueChanged="wdcBlindDate_ValueChanged" />
                </igsch:WebDateChooser>
              </td>
              <td width="50px" class="ptb3">
                <center><asp:CheckBox ID="cbBlindL" runat="server" OnClick="Lock(this, cboxBlindDate)"/></center></td>
              <td class="ptb3">
                <center><asp:CheckBox ID="cboxBlindDate" runat="server"/></center>
                </td>
            </tr>
            <tr valign="middle" height="1">
              <td class="ptb1" width="160" style="height: 1px">
                <a onmouseover="doTooltip(event,mAnnouncement)"
                  onmouseout="hideTip()" href="javascript://" style="border: 0px">
                  <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
                </a><asp:Label ID="lbAnnouncement" runat="server" Text="announcement date"></asp:Label></td>
              <td class="ptb3">
                <center><asp:Image ID="iRegionAnnouncement" runat="server" /></center></td>
              <td class="ptb3">
                <igsch:WebDateChooser ID="wdcAnnouncementDate" runat="server" NullDateLabel="">
                  <CalendarLayout ShowFooter="False" ShowTitle="False">
                  </CalendarLayout>
                  <ClientSideEvents ValueChanged="wdcAnnouncementDate_ValueChanged" />
                </igsch:WebDateChooser>
              </td>
              <td width="50px" class="ptb3">
                <center><asp:CheckBox ID="cbAnnL" runat="server"  OnClick="Lock(this, cboxAnnouncementDate)"/></center></td>
              <td class="ptb3">
                <center><asp:CheckBox ID="cboxAnnouncementDate" runat="server"/></center>
                </td>
            </tr>
            <tr valign="middle" height="1">
              <td class="ptb1" width="160" style="height: 1px">
                <a onmouseover="doTooltip(event,mRemoval)"
                  onmouseout="hideTip()" href="javascript://" style="border: 0px">
                  <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
                </a><asp:Label ID="lbRemoval" runat="server" Text="removal date"></asp:Label></td>
              <td class="ptb3">
                <center><asp:Image ID="iRegionRemoval" runat="server" /></center></td>
              <td class="ptb3">
                <igsch:WebDateChooser ID="wdcRemovalDate" runat="server" NullDateLabel="">
                  <CalendarLayout ShowFooter="False" ShowTitle="False">
                  </CalendarLayout>
                  <ClientSideEvents ValueChanged="wdcRemovalDate_ValueChanged" />
                </igsch:WebDateChooser>
              </td>
              <td width="50px" class="ptb3">
                <center><asp:CheckBox ID="cbRemovL" runat="server" OnClick="Lock(this, cboxRemovalDate)"/></center></td>
              <td class="ptb3">
                <center><asp:CheckBox ID="cboxRemovalDate" runat="server"/></center></td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  <script src="/hc_v4/js/hc_tooltip.js" type="text/javascript"></script>
</asp:Content>