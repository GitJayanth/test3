<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="SendFeedBack" CodeFile="SendFeedBack.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Send a feedback</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <style type="text/css">
    .Title { PADDING-LEFT: 3px; FONT-WEIGHT: bold; FONT-SIZE: 14pt; COLOR: white; FONT-FAMILY: Tahoma,Verdana,arial,sans-serif; BACKGROUND-COLOR: #808080 }
	</style>
    <script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Submit') {
		      if (document.getElementById("txtComment").value=='')
		      {
		      document.getElementById("lblValidate").innerHTML = 'A description is required to send this feedback';
		        oEvent.cancelPostBack = true;
		      }
		      else
		      {
		       document.getElementById("lblValidate").innerHTML='';
		      }
 		    }
		    if (oButton.Key == 'Close') {
        oEvent.cancelPostBack = true;
		    window.close()
        }
		  } 
		  function uwToolbar_InitializeToolbar(oToolbar, oEvent){
   	  }		  
	    	    
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <div id="service" style="BEHAVIOR: url(/hc_v4/js/webservice.htc)"></div>
      <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr valign=top>
          <td class="Title" colspan="2">Feedback</td>
        </tr>
        <tr valign=top>
          <td class="sectionTitle" colspan="2">Feedbacks help us improving quality and 
            useability of this application.</td>
        </tr>
        <tr valign="top" height="25">
          <td colspan="2"><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%"
              ImageDirectory=" ">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <Items>
                <igtbar:TBarButton Key="Submit" ToolTip="Send a feedback to support team." Text="Send" Image="/hc_v4/img/ed_savepublish.gif"></igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Close" ToolTip="Close the window." Text="Close" Image="/hc_v4/img/ed_reject.gif"></igtbar:TBarButton>
              </Items>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
            </igtbar:ultrawebtoolbar></td>
        </tr>
        <tr height="10">
          <td colspan="2"><br/>
          </td>
        </tr>
        <tr valign="top">
          <td width="550">
            <table width="550" border="0">
              <tr valign=top>
                <td width="100">Classification</td>
                <td width="60"><asp:dropdownlist id="ClassificationList" runat="server"></asp:dropdownlist></td>
                <td width=340>Please, only use <b>SERIOUS</b> and <b>CRITICAL</b> for circumstances like:</td>
              </tr>
              <tr valign=top>
                <td width="100"><b>User</b></td>
                <td ><asp:label id="Label1" runat="server">Label</asp:label></td>
                <td>
                  <ul>
                    <li>The system doesn't work at all</li>
                    <li>The system doesn't work as it should</li>
                  </ul>
                </td>
              </tr>
              <tr valign=top>
                <td width="100" height="20"><b>Date</b></td>
                <td height="20" colspan=2><asp:label id="Label2" runat="server">Label</asp:label></td>
              </tr>
              <tr valign=top>
                <td width="100"><b>Current page</b></td>
                <td nowrap colspan=2 width=530><asp:label id="txtCurrentPage" runat="server">Label</asp:label></td>
              </tr>
            </table>
          </td>
        </tr>
        <tr valign="top">
          <td colspan="2"><br/>
            <br/>
            <b>Type here your problem description.</b></td>
        </tr>
        <tr valign="top">
          <td colspan="2"><asp:textbox id="txtComment" runat="server" Rows="5" TextMode="MultiLine" Columns="100"></asp:textbox></td>
        </tr>
        <tr valign=top>
          <td class="ugb" colspan="2">
            <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" CssClass="errorMessage" ErrorMessage="*"
              ControlToValidate="txtComment" Display="Dynamic" InitialValue="" Width="100%"></asp:requiredfieldvalidator>
          </td>
        </tr>
        <tr valign=top>
          <td colspan="2"><asp:Label id="lblSubmitResult" runat="server" ForeColor="#009900"></asp:Label>
          <br/><font color="#ff0000"><div id="lblValidate" runat="server"></div></font></td>
        </tr>
      </table>
</asp:Content>