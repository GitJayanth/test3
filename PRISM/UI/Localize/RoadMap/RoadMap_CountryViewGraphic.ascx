<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RoadMap_CountryViewGraphic.ascx.cs" Inherits="RoadMap_CountryViewGraphic" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
  <style type="text/css">
    .rc { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; height: 18px; width: 100%; cursor: hand}
    .rg { border-right: 0px; border-top: 1px solid white; border-left: 0px; border-bottom: 1px solid white; border-spacing: 0px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #FFFFFF; white-space: nowrap; }
    .re { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #336699; white-space: nowrap; color: black }
    .rl { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #FF6600; white-space: nowrap; color: black }
    .ro { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #000000; white-space: nowrap; color: white }
    .ri { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #FF0000; white-space: nowrap; color: black }
    .rrv { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #999999; white-space: nowrap; color: black }
    .rcv { border-width: 1px; border-spacing: 1px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #009900; white-space: nowrap; color: black }
    .v { border-right: 0px; border-left: 0px; border-top: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; writing-mode: tb-rl; font-weight:bold; vertical-align: bottom;  border-bottom: solid 1px black; white-space: nowrap; }
    .b { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; }
    .mr { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; width: 3px }
    .bb { border-bottom: solid 1px black }
    .bs { border-bottom: solid 1px black; width: auto }
    .n { border-bottom: solid 1px black; width: 200px; font-size:xx-small; font-weight: bold }
    .h { border-bottom: solid 1px black; width: 200px; font-size:x-small; font-weight: bold; text-align: center; vertical-align: bottom}
    .p { border-width: 1px; border-style: solid; border-color: black; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; height: 18px }
  </style>
	<script type="text/javascript" language="javascript" src="/hc_v4/js/hc_tooltip.js"></script>
	<script type="text/javascript" language="javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
      if (oButton.Key == 'Close')
      {
				oEvent.cancelPostBack = true;
				window.close();
      }
		}
	  
//	  function UpdateVars(nbMonths, startDate)
//	  {
//	    hNbMonths.value = nbMonths;
//	    hStartDate.value = startDate;
//	  }
	  window.onresize = RedimDiv;
	  window.onload= InitDiv;
	  var divDg;
	  function InitDiv()
	  {
	    divDg = document.getElementById("divDg");
	    RedimDiv();
	  }
	  function RedimDiv()
	  {
	    divDg.style.width = Window_Width()-2;
	  }
	  function Window_Width()
    {
      if (window.innerWidth) return window.innerWidth;
      else if (document.body && document.body.offsetWidth) return document.body.offsetWidth;
      else return 800;
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
						<igtbar:TBarButton Key="Close" ToolTip="Close window" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator Key="CloseSep"></igtbar:TBSeparator>
						<igtbar:TBarButton Key="Back" ToolTip="Back to the parameters" Text="Back" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator Key="BackSep"></igtbar:TBSeparator>
						<igtbar:TBarButton Key="ListView" ToolTip="Switch view list" Text="Switch view" Image="/hc_v4/img/LinkFrom.gif">
						  <DefaultStyle Width="100px"></DefaultStyle>
						</igtbar:TBarButton>
						<igtbar:TBSeparator Key="ListViewSep" />
						<igtbar:TBLabel Key="lbStartDate" Text="Selected date">
							<DefaultStyle Width="100px" Font-Bold="True"></DefaultStyle>
						</igtbar:TBLabel>
						<igtbar:TBCustom ID="TBCustom1" Width="95px" Key="wdStartDate" runat="server">
						  <igsch:webdatechooser id="wdCalendar" runat="server" Width="90px" NullDateLabel="">
						    <EditStyle Font-Size="8pt"></EditStyle>
						    <CalendarLayout ShowTitle="False" ShowFooter="False">
                  <TodayDayStyle ForeColor="Red" />
                </CalendarLayout>
					    </igsch:webdatechooser>
						</igtbar:TBCustom>
						<igtbar:TBLabel Key="lbPeriod" Text=" for: ">
							<DefaultStyle Width="30px" Font-Bold="True"></DefaultStyle>
						</igtbar:TBLabel>
						<igtbar:TBCustom Width="45px" Key="wdPeriod" runat="server">
							<asp:DropDownList id="DDL_Period" runat="server">
								<asp:ListItem Value="6">6</asp:ListItem>
								<asp:ListItem Value="9">9</asp:ListItem>
								<asp:ListItem Value="12">12</asp:ListItem>
								<asp:ListItem Value="15">15</asp:ListItem>
								<asp:ListItem Value="18">18</asp:ListItem>
								<asp:ListItem Value="24">24</asp:ListItem>
							</asp:DropDownList>
						</igtbar:TBCustom>
						<igtbar:TBLabel Text=" months">
							<DefaultStyle Width="50px" Font-Bold="True"></DefaultStyle>
						</igtbar:TBLabel>
						<igtbar:TBarButton Key="Show" ToolTip="Show" Text="Show" Image="/hc_v4/img/ed_roadmap.gif"></igtbar:TBarButton>
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
		    <div id="divDg" style="OVERFLOW: auto; width:800px; height:100%">
          <asp:Repeater ID="rDates" runat="server" OnItemDataBound="rDates_ItemDataBound">
            <HeaderTemplate><table class="b" style="width: 100%"><asp:Label ID="lbHeader" runat="server"></asp:Label></HeaderTemplate>
            <ItemTemplate><asp:Label ID="lbBar" runat="server"></asp:Label></ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
          </asp:Repeater>
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