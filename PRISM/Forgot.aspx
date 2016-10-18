<%@ Page Language="C#" Inherits="HyperCatalog.UI.Forgot" CodeFile="Forgot.aspx.cs"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>[<%= (string)HyperCatalog.Shared.SessionState.CacheParams["AppName"].Value%>] - Login page</title>
    <link href="/hc_v4/css/HomePage.css" type="text/css" rel="STYLESHEET"/>
      <script type="text/javascript">
      if (top.window.location != window.location){
       top.window.location = window.location;
      } 
      if (opener){
        opener.location = window.location;
        window.close;
      }
if (top != self) {
    top.location = self.location
}
      </script>
  </head>
  <body class="colorFFFFFFbg" style="margin:0px;" onload="document.getElementById('txtLogin').focus();">
    <form name="aspnetForm" id="aspnetForm" method="post" runat="server">
      <!--stopindex-->
      <!-- Begin Top Navigation Area -->
      <table cellspacing="0" cellpadding="0" width="740" border="0">
        <tr class="decoration">
          <td class="table_home_2"></td>
          <td class="countryInd" align="right" width="285"></td>
          <td align="right">English</td>
        </tr>
      </table>
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
          <td valign="top" align="left" bgColor="#666666">
            <table cellspacing="0" cellpadding="0" width="720" border="0">
              <tr>
                <td><a href="http://welcome.hp.com/country/uk/en/welcome.html"><img alt="HP.com United Kigdom home" src="/hc_v4/img/hpweb_1-2_topnav_home.gif" width="100"
                      border="0"></a></td>
                <td class="colorE7E7E7bg"></td>
                <td><a href="http://welcome.hp.com/country/uk/en/prodserv.html"><img alt="Products and Services" src="/hc_v4/img/hpweb_1-2_topnav_prdsrv.gif" width="166"
                      border="0"></a></td>
                <td class="colorE7E7E7bg"></td>
                <td><a href="http://welcome.hp.com/country/uk/en/support.html"><img alt="Support and Drivers" src="/hc_v4/img/hpweb_1-2_topnav_supprt.gif" width="163"
                      border="0"></a></td>
                <td class="colorE7E7E7bg"></td>
                <td><a href="http://welcome.hp.com/country/uk/en/solutions.html"><img alt="Solutions" src="/hc_v4/img/hpweb_1-2_topnav_slutns.gif" width="143" border="0"></a></td>
                <td class="colorE7E7E7bg"></td>
                <td><a href="http://welcome.hp.com/country/uk/en/howtobuy.html"><img alt="How to Buy" src="/hc_v4/img/hpweb_1-2_topnav_buy.gif" width="143" border="0"></a></td>
                <td class="colorE7E7E7bg"></td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
      <!-- End Top Navigation Area -->
      <!-- Begin Search Area -->
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
          <td valign="top" align="left" bgColor="#e7e7e7">
            <table height="50" cellspacing="0" cellpadding="0" width="740" border="0">
              <tr>
                <td valign="top" width="20" height="50"></td>
                <td align="left" height="50"><A class="smallbld" href="mailto:">» Support</A>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
      <!-- End Search Area -->
      <!--startindex-->
      <!-- Begin Page Title Area -->
      <table cellspacing="0" cellpadding="0" width="840" border="0">
        <tr>
          <td valign="middle" align="center" width="200" height="70"><img height="55" src="/hc_v4/img/hp_logo.gif" width="64" border="0"></td>
          <td width="10">&nbsp;</td>
          <td valign="middle" align="left">
            <h1><%= (string)HyperCatalog.Shared.SessionState.CacheParams["AppName"].Value%>: Request for a new password</h1>
          </td>
          <!--stopindex--></tr>
      </table>
      <!-- End Page Title Area -->
      <!-- Begin Left Navigation and Content Area.  To increase width of content area, modify width of table below. -->
      <table height="65%" width="740">
        <tr>
          <!-- Start Left Navigation -->
          <td valign="top" width="200">
            <table width="200">
              <tr class="colorDCDCDCbg">
                <td valign="middle" align="left">
                  <h2>&nbsp;</h2>
                </td>
              </tr>
            </table>
            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr>
                <td width="10"></td>
                <td><br/>
                  &raquo; <asp:linkbutton id="linkLogin" runat="server" ForeColor="#003366" CausesValidation="False">Back to login form</asp:linkbutton><br/>
                  <br/>
                </td>
                <td width="10"></td>
              </tr>
              <tr>
                <td width="10"></td>
                <td class="colorCCCCCCbg" height="1"></td>
                <td width="10"></td>
              </tr>
              <tr>
                <td width="10"></td>
                <td><br/>
                  <br/>
                </td>
                <td width="10"></td>
              </tr>
            </table>
          </td>
          <!-- End Left Navigation -->
          <!-- Begin Gutter Cell 10px Wide -->
          <td width="10">&nbsp;</td>
          <!-- End Gutter Cell 10px Wide -->
          <!--startindex-->
          <!-- Start Content Area.  To increase width of content area, modify width of table cell below. -->
          <td valign="top" align="left">
            <br/>
            <br/>
            <table width="100%">
              <tr>
                <td class="home_label" valign="middle" width="100">Login:&nbsp;</td>
                <td valign="bottom" width="210"><asp:textbox id="txtLogin" tabIndex="1" runat="server" cssclass="home_field" MaxLength="50" width="200"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtLogin"
                    CssClass="errorMessage" ErrorMessage="Login mandatory"> *</asp:requiredfieldvalidator></td>
                <td valign="bottom" align="left" rowSpan="2"><asp:imagebutton id="validateBtn" tabIndex="3" runat="server" ImageUrl="/hc_v4/img/bt_arrow_grey.gif"></asp:imagebutton></td>
              </tr>
              <tr>
                <td valign="middle" width="100">&nbsp;</td>
                <td valign="bottom" width="210">
                  <div align="center" width="100"><U></U>&nbsp;</div>
                </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td valign="middle" colspan="3"></td>
              </tr>
              <tr>
                <td valign="middle" colspan="3"><asp:label id="lbError" runat="server" CssClass="errorClass" Visible="False" Width="100%">error message</asp:label></td>
              </tr>
              <tr>
                <td valign="middle" colspan="3">
                  <asp:Label id="lbConfirm" runat="server" Width="100%" Visible="False">lbConfirm</asp:Label></td>
              </tr>
            </table>
          </td>
          <!-- End Content Area -->
          <!--stopindex--></tr>
      </table>
      <!-- End Left Navigation and Content Area -->
      <!-- Begin Footer Area -->
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr class="decoration">
          <td class="color666666bg"></td>
        </tr>
        <tr>
          <td valign="top" align="left">
            <table cellspacing="0" cellpadding="0" width="740" border="0">
              <tr class="decoration">
                <td colspan="4"></td>
              </tr>
              <tr>
                <td align="center" width="33%"><a class="udrlinesmall" href="http://welcome.hp.com/country/uk/en/privacy.html">Privacy 
                    statement</a></td>
                <td align="center" width="33%"><a class="udrlinesmall" href="http://welcome.hp.com/country/uk/en/termsofuse.html">Using 
                    this site means you accept its terms</a></td>
                <td align="center" width="33%"><!-- Activate Feedback Link Here --> <!-- a href="Insert_Link_Here" class="udrlinesmall">Feedback to [site name]</a -->  <!-- End Activate Feedback Link Here --></td>
              </tr>
              <tr class="decoration">
                <td colspan="4"></td>
              </tr>
              <tr>
                <td class="small" align="center" colspan="4">© 2004 Hewlett-Packard Development 
                  Company, L.P.</td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
      <!-- End Footer Area -->
      <!-- Begin METRICS Javascript -->
      <script language="JavaScript" src="/js/metrics.js" type="text/javascript"></script>
      <!-- End METRICS Javascript --></form>
  </body>
</html>
