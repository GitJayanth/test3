<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="user_properties" CodeFile="user_properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript">
			var noSavePassword;
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		    if (oButton.Key == 'List') {
		        back();
          oEvent.cancelPostBack = false;
        }
//		    if (oButton.Key == 'Save') {
//		      if (rate > 52 || noSavePassword == 1)
//		      {
//            oEvent.cancelPostBack = MandatoryFieldMissing();
//          }
//          else
//          {
//            alert("Your password is weak");
//            oEvent.cancelPostBack = true;
//          }
//        }
		    if (oButton.Key == 'Delete') {
		        oEvent.cancelPostBack = !confirm("Are you sure?");
		        back();
        }
        if (oButton.Key == 'Unlock'){
          oEvent.cancelPostBack = false;
        }
		  } 
		  
		  var lc = /[a-z]{1}/; // lowercase letters
			var uc = /[A-Z]{1}/; // uppercase letters
			var nm = /[0-9]{1}/; // numbers
			var sc = /.[!,@,#,$,%,^,&,*,?,_,~]/; // Special Char
      var rate = 0;
		  
//		  function getRate(string)
//		  {
//		    var isTooShort = string.length < 6;
//		    var isFair = string.length >= 8;
//		    rate = 0;
//		    if (!isTooShort)
//		    {
//	  	    rate += string.match(nm)?10:0;
//		      rate += string.match(uc)?26:0;
//		      rate += string.match(lc)?26:0;
//		      rate += string.match(sc)?11:0;
//		    }
//		    return rate;
//		  }
		  
////		  function updateStrength(pwd)
////		  {
////		    noSavePassword = 0;
////		    //top.window.status = pwd;
////		    if (pwd.length == 0)
////		    {
////		      rateRubber.childNodes[0].innerHTML = "&nbsp;";
////	        rateRubber.childNodes[0].className = "";
////	        rateRubber.childNodes[1].className = "";
////	        rateRubber.childNodes[2].className = "";
////		      return;
////		    }
//		    var rate = getRate(pwd);
//		    if (rate<53)
//		    {
//		      rateRubber.childNodes[0].innerText = "Weak";
//	        rateRubber.childNodes[0].className = "weak";
//	        rateRubber.childNodes[1].className = "";
//	        rateRubber.childNodes[2].className = "";
//        }
//        else if (rate<63)
//        {
//		      rateRubber.childNodes[0].innerText = "Good";
//          rateRubber.childNodes[0].className = "good";
//          rateRubber.childNodes[1].className = "good";
//          rateRubber.childNodes[2].className = "";
//        }
//        else
//        {
//		      rateRubber.childNodes[0].innerText = "Strong";
//          rateRubber.childNodes[0].className = "strong";
//	        rateRubber.childNodes[1].className = "strong";
//	        rateRubber.childNodes[2].className = "strong";
//		    }
//		  }
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
<%--Removed the width property from ultrawebtoolbar to fix moving icon issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
					  <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="UnlockSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Unlock" ToolTip="Unlock user" Text="Unlock" Image="/hc_v4/img/unlocked_status.gif"></igtbar:TBarButton>
							
								<igtbar:TBarButton Key="SendInfo" Text="Send informations"  Visible="false" Image="/hc_v4/img/send-mail.gif">
									<DefaultStyle Width="150px"></DefaultStyle>
								</igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" border="0">
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbFirstName" Runat="server">First name</asp:label></td>
								<td class="ugd" style="width: 100%"><asp:textbox id="txtFirstName" runat="server" width="180px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="errorMessage" ControlToValidate="txtFirstName"
										ErrorMessage="*"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbLastName" Runat="server">Last name</asp:label></td>
								<td class="uga"><asp:textbox id="txtLastName" runat="server" width="180px"></asp:textbox></td>
								<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="errorMessage" ControlToValidate="txtLastName"
									ErrorMessage="*"></asp:requiredfieldvalidator></tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width: 130px; height: 23px"><asp:label id="Label1" Runat="server">Email</asp:label></td>
								<td class="ugd" style="height: 23px"><asp:textbox id="txtEmail" runat="server" width="180px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" CssClass="errorMessage" ControlToValidate="txtEmail"
										ErrorMessage="*"></asp:requiredfieldvalidator>
										<asp:RegularExpressionValidator id="regEmail" runat="server" ErrorMessage="Invalid email address"
                    Display="Dynamic" ValidationExpression="\b^[A-Za-z0-9](([_\.\-\!\#\$\%\&\'\*\+\\\/\=\?\^\`\{\|\}\~]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$\b" ControlToValidate="txtEmail"></asp:RegularExpressionValidator></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="Label2" Runat="server">Active</asp:label></td>
								<td class="uga"><asp:checkbox id="cbIsActive" runat="server"></asp:checkbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="Label4" Runat="server">ReadOnly</asp:label></td>
								<td class="ugd"><asp:checkbox id="cbIsReadOnly" runat="server"></asp:checkbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbPseudo" Runat="server">Pseudo</asp:label></td>
								<td class="uga"><asp:textbox id="txtPseudo" runat="server" width="180px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" CssClass="errorMessage" ControlToValidate="txtPseudo"
										ErrorMessage="*"></asp:requiredfieldvalidator></td>
							</tr>
							<tr valign="middle">
<%--								<td class="editLabelCell" style="width:130px"><asp:label id="lbPassword" Runat="server">Password</asp:label></td>
--%>								<td class="ugd">
								  <table cellpadding="0" cellspacing="0" border="0">
								    <tr>
								      <%--<td colspan="2">
								        <asp:textbox id="txtPassword" runat="server" Text="" width="180px" TextMode="Password" onkeyup="updateStrength(this.value);"></asp:textbox>
                        <asp:requiredfieldvalidator id="RequiredFieldValidator6" runat="server" CssClass="errorMessage" ControlToValidate="txtPassword" ErrorMessage="*"></asp:requiredfieldvalidator>
								        (Six-character minimum)
								        <asp:Button ID="resetPwdBtn" runat="server" Text="reset" OnClick="ResetPassword"></asp:Button>
                        <asp:CheckBox id="useDefaultPwd" runat="server" Text="default" Checked="false"/>
								      </td>
								    </tr>
								    <tr>
								      <td>Safety rate</td>
								      <td>--%>
                       <%-- <table cellpadding="0" cellspacing="0">
								          <tr style="background-color:whitesmoke;" id="rateRow">
                            <td style="width:59px;border-left:solid 1px black;border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td>
                            <td style="width:59px;border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td>
                            <td style="width:59px;border-right:solid 1px black;border-top:solid 1px black;border-bottom:solid 1px black;">&nbsp;</td>
                          </tr>
                        </table>--%>
                        <script type="text/javascript" language="javascript">
              	          var rateRubber = document.getElementById("rateRow");
                        </script>
                      </td>
								    </tr>
								  </table>
								</td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbOrganization" Runat="server">Organization</asp:label></td>
								<td class="uga"><asp:dropdownlist id="cbOrgs" runat="server" DataValueField="Id" DataTextField="Name"></asp:dropdownlist></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="height: 23px"><asp:label id="Label3" runat="server">Time Zone</asp:label></td>
								<td class="ugd" style="height: 23px"><asp:dropdownlist id="DDL_TimeZone" runat="server"></asp:dropdownlist></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbRole" Runat="server">Role</asp:label></td>
								<td class="uga"><asp:dropdownlist id="ddlRoles" runat="server" DataValueField="Id" DataTextField="Name"></asp:dropdownlist></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbFormatDate" Runat="server">Date format</asp:label></td>
								<td class="ugd"><asp:dropdownlist id="ddlFormatDate" runat="server">
                  <asp:ListItem Value="MM/dd/yyyy">MM/dd/yyyy</asp:ListItem>
                  <asp:ListItem Value="dd/MM/yyyy">dd/MM/yyyy</asp:ListItem>
                  <asp:ListItem Value="yyyy/MM/dd">yyyy/MM/dd</asp:ListItem>
                  <asp:ListItem Value="MM-dd-yyyy">MM-dd-yyyy</asp:ListItem>
                  <asp:ListItem Value="dd-MM-yyyy">dd-MM-yyyy</asp:ListItem>
                  <asp:ListItem Value="yyyy-MM-dd">yyyy-MM-dd</asp:ListItem>
                </asp:dropdownlist></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px"><asp:label id="lbFormatTime" Runat="server">Time format </asp:label></td>
								<td class="uga"><asp:dropdownlist id="ddlFormatTime" runat="server">
                  <asp:ListItem Value="HH:mm:ss">HH:mm:ss (24 hours)</asp:ListItem>
                  <asp:ListItem Value="hh:mm:ss">hh:mm:ss</asp:ListItem>
                  <asp:ListItem Value="h:m:s">h:m:s</asp:ListItem>
                </asp:dropdownlist></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
<%--      <script>var passBox=document.getElementById('<%= txtPassword.ClientID %>');passBox.value='<%= emptyPassword %>';noSavePassword = 1;</script>
--%></asp:Content>