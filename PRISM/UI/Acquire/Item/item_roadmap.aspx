<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.ItemManagement.Item_Roadmap" CodeFile="Item_Roadmap.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ig_sched" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebSchedule.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="hc" TagName="progressbar" Src="~/tools/holoadingpage.ascx"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Roadmap</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<style type="text/css">
        .rc { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; height: 18px; width: 100%; cursor: hand}
        .rg { border-right: 0px; border-top: 1px solid white; border-left: 0px; border-bottom: 1px solid white; border-spacing: 0px; padding: 0px; margin: 1px; border-collapse: collapse; background-color: #FFFFFF; white-space: nowrap; }
        .rb { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; background-color: #FF8040; white-space: nowrap }
        .rl { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; background-color: #008000; white-space: nowrap }
        .ri { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; background-color: #FF0000; white-space: nowrap }
        .v { border-right: 0px; border-left: 0px; border-top: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; writing-mode: tb-rl; font-weight:bold; vertical-align: bottom;  border-bottom: solid 1px black }
        .b { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; }
        .mr { border-width: 0px; border-spacing: 0px; padding: 0px; margin: 0px; border-collapse: collapse; width: 3px }
        .bb { border-bottom: solid 1px black }
        .bs { border-bottom: solid 1px black; width: auto }
        .n { border-bottom: solid 1px black; width: 200px; font-size:xx-small }
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
  	  
  	  function UpdateVars(nbMonths, startDate)
  	  {
  	    hNbMonths.value = nbMonths;
  	    hStartDate.value = startDate;
  	  }
  	  window.onresize = RedimDiv;
  	  window.onload= InitDiv;
  	  var divDg;
  	  function InitDiv(){
  	    divDg = document.getElementById("divDg");
  	    RedimDiv();
  	  }
  	  function RedimDiv(){
  	    divDg.style.width = Window_Width()-2;
  	  }
  	  function Window_Width()
      {
        if (window.innerWidth) return window.innerWidth;
        else if (document.body && document.body.offsetWidth) return document.body.offsetWidth;
        else return 800;
      }

			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
  document.body.scroll = "no";
</script>
	<hc:progressbar id="hoprogress1" runat="server" SrcImage="/hc_v4/img/ed_wait.gif"/>
	<table class="main b">
		<tr valign="top" style="height:1px">
			<td class="b">
			  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<Items>
						<igtbar:TBarButton Key="Close" ToolTip="Close Window" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator Key="StartDateSep"></igtbar:TBSeparator>
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
				</igtbar:ultrawebtoolbar>
			</td>
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
            <asp:Repeater ID="rDates" runat="server">
              <HeaderTemplate>
				        <table class="b" style="width: 100%">
<asp:Label ID="lbHeader" runat="server"></asp:Label>
              </HeaderTemplate>
              <ItemTemplate>
<asp:Label ID="lbBar" runat="server"></asp:Label>
<asp:Label ID="lbBarPLC" runat="server"></asp:Label>
              </ItemTemplate>
              <FooterTemplate>
                </table>
              </FooterTemplate>
            </asp:Repeater>
        </div>
		  </td>
	  </tr>
	  <tr valign="bottom" style="height:1px">
      <td class="b">
        <table class="hc_toolbartitle" style="height:15px; width:100%" cellpadding="0" cellspacing="0" border="0">
          <tr valign="middle">
            <td style="width:30px">&nbsp;</td>
	          <td class="rb" style="width:30px; height:15px">&nbsp;</td><td style="width:50px">&nbsp;Blind</td>
	          <td class="rl" style="width:30px; height:15px">&nbsp;</td><td style="width:60px">&nbsp;Full/Live</td>
	          <td class="ri" style="width:30px; height:15px">&nbsp;</td><td style="width:80px">&nbsp;Invalid</td>
	          <td style="width: auto">&nbsp;</td>
          </tr>
        </table>
      </td>
	  </tr>
  </table>
</asp:Content>