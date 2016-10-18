<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="userprofile_password" CodeFile="userprofile_password.aspx.cs" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Password</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
    <script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          oEvent.cancelPostBack = true;
          back();
        }
		    if (oButton.Key == 'Save') {
		      if (rate>52){
            oEvent.cancelPostBack = MandatoryFieldMissing();
          }
          else{
            alert("Your password is weak");
            oEvent.cancelPostBack = true;
          }
        }
		  }
		  
		  var lc = /[a-z]{1}/; // lowercase letters
			var uc = /[A-Z]{1}/; // uppercase letters
			var nm = /[0-9]{1}/; // numbers
			var sc = /.[!,@,#,$,%,^,&,*,?,_,~]/; // Special Char
      var rate = 0;
		  
		  function getRate(string)
		  {
		    var isTooShort = string.length < 6;
		    var isFair = string.length >= 8;
		    rate = 0;
		    if (!isTooShort)
		    {
	  	    rate += string.match(nm)?10:0;
		      rate += string.match(uc)?26:0;
		      rate += string.match(lc)?26:0;
		      rate += string.match(sc)?11:0;
		    }
		    return rate;
		  }
		  
		  function updateStrength(pwd)
		  {
		    //top.window.status = pwd;
		    if (pwd.length == 0)
		    {
		      rateRubber.childNodes[0].innerHTML = "&nbsp;";
	        rateRubber.childNodes[0].className = "";
	        rateRubber.childNodes[1].className = "";
	        rateRubber.childNodes[2].className = "";
		      return;
		    }
		    var rate = getRate(pwd);
		    if (rate<53)
		    {
		      rateRubber.childNodes[0].innerText = "Weak";
	        rateRubber.childNodes[0].className = "weak";
	        rateRubber.childNodes[1].className = "";
	        rateRubber.childNodes[2].className = "";
        }
        else if (rate<63)
        {
		      rateRubber.childNodes[0].innerText = "Good";
          rateRubber.childNodes[0].className = "good";
          rateRubber.childNodes[1].className = "good";
          rateRubber.childNodes[2].className = "";
        }
        else
        {
		      rateRubber.childNodes[0].innerText = "Strong";
          rateRubber.childNodes[0].className = "strong";
	        rateRubber.childNodes[1].className = "strong";
	        rateRubber.childNodes[2].className = "strong";
		    }
		  }
    </script>
    <style>
      .weak
      {
        background-color:#FFFF99;
        color:#FF3333;
        font-weight:bold;
      }
      .good
      {
        background-color:#FFCC99;
        color:#996600;
        font-weight:bold;
      }
      .strong
      {
        background-color:#CCFFCC;
        color:#009900;
        font-weight:bold;
      }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr height="1" valign="top">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <CLIENTSIdEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIdEEVENTS>
              <ITEMS>
                <igtbar:TBarButton Text="Save" Image="/hc_v4/img/ed_save.gif" Key="Save"></igtbar:TBarButton>
              </ITEMS>
            </igtbar:ultrawebtoolbar></td>
        </tr>
      </table>
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr valign="top" style="height:1px">
          <td><br/>
            <asp:label id="lbInfo" runat="server">To change your password, please provide the following information 
        and then click <B>Save.</B></asp:label><br/>
          </td>
        </tr>
        <tr valign="top">
          <td><br/>
            <table id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
              <tr>
                <td class="editLabelCell" style="width:130px">
                  <asp:label id="lbOldPassword" runat="server">Old password</asp:label></td>
                <td class="ugd">
                  <asp:textbox id="txtOldPassword" runat="server" Text="" width="180px" TextMode="Password"></asp:textbox>
                  <asp:requiredfieldvalidator id="Requiredfieldvalidator10" runat="server" ControlToValidate="txtOldPassword"
                    ErrorMessage="*" CssClass="errorMessage"></asp:requiredfieldvalidator>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="width:130px">
                  <asp:label id="lbNewPassword" runat="server">New password</asp:label></td>
                <td class="uga">
                  <asp:textbox id="txtNewPassword" runat="server" Text="" width="180px" TextMode="Password" onkeyup="updateStrength(this.value);"></asp:textbox>
                  <asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="*"
                    CssClass="errorMessage"></asp:requiredfieldvalidator>
                  <br/>Six-character minimum
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="width:130px">
                  <asp:label id="lbSafety" runat="server">Safety rate</asp:label></td>
                <td class="ugd">
                  <table cellpadding="0" cellspacing="0">
                    <tr style="background-color:whitesmoke;" id="rateRow">
                      <td class="" style="width:59px;border-left:solid 1px black;border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td>
                      <td style="width:59px;border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td>
                      <td style="width:59px;border-right:solid 1px black;border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td>
                    </tr>
                  </table>
                  <script type="text/javascript" language="javascript">
              	    var rateRubber = document.getElementById("rateRow");
                  </script>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="width:130px">
                  <asp:label id="lbConfirm" runat="server">Confirm new password</asp:label></td>
                <td class="uga">
                  <asp:textbox id="txtNewPasswordConfirm" runat="server" Text="" width="180px" TextMode="Password"></asp:textbox>
                  <asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" ControlToValidate="txtNewPasswordConfirm"
                    ErrorMessage="*" CssClass="errorMessage"></asp:requiredfieldvalidator>
                </td>
              </tr>
            </table>
            <br/>
            <asp:label id="lbError" runat="server" CssClass="hc_error" Visible="false">Label</asp:label>
          </td>
        </tr>
      </table>
</asp:Content>