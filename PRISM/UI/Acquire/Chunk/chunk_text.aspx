<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ig_spell" Namespace="Infragistics.WebUI.WebSpellChecker" Assembly="Infragistics2.WebUI.WebSpellChecker.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Text" validateRequest="false" CodeFile="Chunk_Text.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk text</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
  <script type="text/javascript" src="../../../ajaxcontrols/Extensions.js"></script>
	<script type="text/javascript" language="javascript">
  top.window.focus();
  var ILBText = "Intentionaly left blank";
  var toolBar;
  var genericEvent = null;
	var finalButton = null;
 function SpellCheck()
 {
   octl00_HOCP_WebSpellChecker1.checkSpelling(document.getElementById('ctl00_HOCP_txtValue').value, myReturnFunc);
 }

 function myReturnFunc(checkedText)
 {
   document.getElementById('ctl00_HOCP_txtValue').value = checkedText;
 }

  function uwToolbar_Click(oToolbar, oButton, oEvent){
    var bt1 = oToolbar.Items[0]; // spell
    var bt2 = oToolbar.Items[2]; // char
    var bt3 = oToolbar.Items[4]; // ilb
    var RegValidator = document.getElementById('ctl00_HOCP_regValidate');	//ACQ10.0
	  if (oButton.Key=='spell'){
	    if (!bt3.getSelected()){
	      SpellCheck();
	    }
	    return;
	  }
	  if (oButton.Key=='chars'){						  
	    if (!bt3.getSelected()){
        specialCharacter(document.getElementById('ctl00_HOCP_txtValue'));
      }
	    return;
	  }
	  if (oButton.Key=='ilb'){	
	  RegValidator.enabled = false;//ACQ10.0
      var cv = document.getElementById('ctl00_HOCP_txtValue');        
      BtnIlb = oButton;
      if (oButton.getSelected()){
        cv.value=ILBText;
        isBlankChunk = cv.disabled=true;
      }
      else{
        //RegValidator.enabled = true;//ACQ10.0
        RegValidator.enabled = <%=Convert.ToString(!container.DataType.RegularExpression.Equals(string.Empty)).ToLower()%>; //QC :2740 //ACQ10.0
        isBlankChunk = cv.disabled=false;
        cv.value='';
        cv.focus();
      }
	    return;
	    
	  }
  }

  var SpellAction = {
    delay: 0,
    prepare: function() { 
          return document.getElementById('ctl00_HOCP_txtValue').value;
    },
    call: proxies.UIExtensions.SpellCheck,

    finish: function (nb) {
      if (nb > 0){                
        if (confirm('The system found ' + nb + ' spelling error(s)\nProceed anyway?')){
          try{finalButton._post=new Date();__doPostBack(finalButton._uniqueID,"4");}catch(ex){}
        }
        return false;
	    }	    
	    genericEvent.needPostBack = true;
      try{finalButton._post=new Date();__doPostBack(finalButton._uniqueID,"4");}catch(ex){}
	    return true;
    },
    onException: proxies.alertException
  } // validateAction

  ///////////////////////////////////////
  function Page_CheckSpelling(){
  ///////////////////////////////////////
  //ACQ10.0 Starts
  //This is to check if the Value is empty or not and return accordingly
    var cv = document.getElementById('ctl00_HOCP_txtValue') 
    if (cv.value=='' ||cv.value ==null || (cv.value.trim().length()) <=0)
    {
    //QC 2740 Starts
    var requiredValue = document.getElementById('ctl00_HOCP_requiredValue');
    requiredValue.validate();
    //QC 2740 Ends
    cv.focus()
    return false;
    }
    //ACQ10.0 Ends
    return ajax.Start(SpellAction);
  }
  
  
  //function added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix
  ///////////////////////////////////////
  function Page_ValidateHTMLTags(){
  ///////////////////////////////////////
  //This is to check for the presence of HTML tags in Chunk Edit
    var cv = document.getElementById('ctl00_HOCP_txtValue');
    var commentVal=document.getElementById('ctl00_HOCP_ChunkComment1_txtComment');
    //code added on 30th Nov 2011
    var chunkVal = cv.value.toLowerCase();
    var comment=commentVal.value.toLowerCase();
    var regHTML = document.getElementById('ctl00_HOCP_regValidateHTML');
    var regHTML_Comment = document.getElementById('ctl00_HOCP_ChunkComment1_regValidateHTMLComment');
    var keywords='<%=keyword %>';
    if (!(keywords == null || keywords==''))
    {
        var re=new RegExp(keywords);
        if (chunkVal.match(re) && comment.match(re))
        {
            ValidatorEnable(regHTML, true); 
            ValidatorEnable(regHTML_Comment, true);
            return false;
        }
        else if (chunkVal.match(re))
        {
            ValidatorEnable(regHTML, true);
            ValidatorEnable(regHTML_Comment, false);
            return false;
        }
        else if (comment.match(re))
        {
            ValidatorEnable(regHTML_Comment, true);
            ValidatorEnable(regHTML, false);
            return false;
        }     
        else
        {
            ValidatorEnable(regHTML, false);
            ValidatorEnable(regHTML_Comment, false);
        }
    }
    return true;
 }
  
  </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		//document.body.oncontextmenu = function(){return false;};
</script>
			<hc:chunkbuttonbar id="ChunkButtonBar" runat="server"></hc:chunkbuttonbar>
			<table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td style="width: 765px">
						<fieldset>
							<legend><asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" AlternateText="status" ImageAlign="Middle"></asp:image>&nbsp; 
					<%--  
                     * Fix for QC 2238: 01 Triage PRIS: List data type does not allow save as empty
                     * Fixed by Prabhu
                     * Date: 28 Jan 09
                     * Release: PRISM 7.0.01
                     * Code Change History:
                     * requiredfieldvalidator removed.
                       <asp:requiredfieldvalidator id="requiredValue" runat="server" ErrorMessage="Required" ControlToValidate="txtValue"
									Display="Dynamic" ></asp:requiredfieldvalidator>
									Adding the same below so '' cannot be authored via UI from PCF
									--%>
									<asp:requiredfieldvalidator id="requiredValue" Enabled="true" runat="server" ErrorMessage="Please enter a value!" ControlToValidate="txtValue"
									Display="Dynamic" ></asp:requiredfieldvalidator> 
									<asp:RegularExpressionValidator Enabled="false" ID="regValidate" Display="Dynamic" ControlToValidate="txtValue" runat="server" ErrorMessage="Invalid Format!"></asp:RegularExpressionValidator>
									<asp:RegularExpressionValidator Enabled="false" ID="regValidateHTML" Display="Dynamic" ControlToValidate="txtValue" runat="server" ErrorMessage="HTML tags are not allowed!"></asp:RegularExpressionValidator>
									</legend>
							<table cellspacing="0" cellpadding="0" border="0">
								<tr valign="top" bgColor="#d6d3ce">
									<td>
										<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="20px"  ImageDirectory=" ">
											<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
											<ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
											<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
											<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
											<Items>
												<igtbar:TBarButton Key="spell" ToolTip="Launch the spell checker" Image="/hc_v4/img/ed_spellcheck.gif"></igtbar:TBarButton>
												<igtbar:TBSeparator Key="spellsep"></igtbar:TBSeparator>
												<igtbar:TBarButton Key="chars" ToolTip="Insert a special character at current cursor position" Image="/hc_v4/img/ed_specialchars.gif"></igtbar:TBarButton>
												<igtbar:TBSeparator Key="ilbSep"></igtbar:TBSeparator>
												<igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionally left blank"
													Image="/hc_v4/img/ed_blank.gif">
													<SelectedStyle BorderWidth="1px" BorderStyle="Inset"></SelectedStyle>
												</igtbar:TBarButton>
											</Items>
										</igtbar:ultrawebtoolbar></td>
								</tr>
								<tr valign="top">                  
									<td><asp:textbox id="txtValue" runat="server" Columns="80" Rows="5" TextMode="MultiLine">Value</asp:textbox></td>
								</tr>
							</table>
							<div id="textcount"></div>
						</fieldset>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td style="width: 765px"><hc:chunkcomment id="ChunkComment1" runat="server"></hc:chunkcomment></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td style="width: 765px"><hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
						<center><asp:label id="lbResult" runat="server" Font-Size="7pt">Result message</asp:label></center>
					</td>
				</tr>
			</table>
			<input id="txtCulture" type="hidden">
      <ig_spell:WebSpellChecker ID="WebSpellChecker1" runat="server" TextComponentId="txtValue" WebSpellCheckerDialogPage="../SpellChecker/IGSpellCheck.Aspx">
        <SpellOptions AllowWordsWithDigits="True" IncludeUserDictionaryInSuggestions="True">
          <PerformanceOptions ConsiderationRange="80" />
        </SpellOptions> 
        <DialogOptions ShowNoErrorsMessage="True" WindowWidth="440" WindowHeight="395"/>
      </ig_spell:WebSpellChecker>
</asp:Content>