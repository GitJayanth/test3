<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="IGSpellCheck.aspx.cs" Inherits="UI_Acquire_SpellChecker_IGSpellCheck"%>
<%@ Register TagPrefix="ig_spell" Namespace="Infragistics.WebUI.WebSpellChecker" Assembly="Infragistics2.WebUI.WebSpellChecker.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Spell checker</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
			<script type="text/javascript" src="../../../ajaxcontrols/Extensions.js"></script>
			<script type="text/javascript" language="javascript">
			  function AddWord(currentWord) {
  } // validateEMail
      function AjaxSubmitWord(aButton){
        addButton = aButton;
        ajax.Start(AddAction);
      }
  var AddAction = {
    delay: 0,
    prepare: function() { 
          addButton.disabled=true;
          var i = document.getElementById(aj_id).value;
          var c = document.getElementById(aj_cid).value;
          var u = document.getElementById(aj_uid).value;
          var un = document.getElementById(aj_un).value;
          var w = badWords.getItem(octl00_HOCP_WebSpellCheckerDialog1._dialog._currentWordIndex).getText();
          return '<option>'+
                   '<u>' + u + '</u>' +
                   '<un>' + un + '</un>' +
                   '<id>' + i + '</id>'+
                   '<c>' + c + '</c>' +
                   '<w>' + w + '</w>'+
                 '</option>';
    },
    call: proxies.UIExtensions.AddWordXML,

    finish: function (success) {
      // from the Validator Common JavaScript
      if (success == "1"){
        alert("Word added to the custom word list\nIt will be reviewed briefly");
      }
      else{
        alert("An unexcpected error occurred\nThe word couldn't be added");
      }
      addButton.disabled=false;
      octl00_HOCP_WebSpellCheckerDialog1._dialog.ignoreAll();
    },
    onException: proxies.alertException
  } // validateAction
  

			</script>
			   <style type="text/css">
   html, body, form {
	height: 100%;
	width: 100%;
	margin: 0px;
	padding: 0px;
}

body {
	background-color: #CCCCCC;
}

.igspell_documentTextPanel 
{
	border: 1px solid darkgray;
	position: absolute;
	padding:1px;
	height: 85px;
	width: 320px;
	left: 10px;
	top: 10px;
	overflow:auto;
	background-color:White;
	bottom: -1px;
}

/**********************************
 * Button Styles
 **********************************/
.igspell_button {
	width: 95px;
	height: 21px;
	position: absolute;
	left: 340px;
	font-family:Verdana, Arial, Helvetica, sans-serif;
	font-size: 10px;
	bottom: -1px;
}

.igspell_button:hover {

}
 
.igspell_ignoreButton {
	top: 17px;
}

.igspell_ignoreAllButton {
	top: 43px;
}

.igspell_changeButton {
	top: 206px;
}

.igspell_changeAllButton {
	top: 232px;
}

.igspell_finishButton 
{
	top: 258px;
}

.igspell_addButton {
	top: 68px;
}


/**************************
 Text Boxes
 **************************/

.igspell_changeToBox {
	position: absolute;
	left: 10px;
	top: 220px;
	width: 320px;
	height: 18px;
	border: 1px solid darkgray;
	bottom: -1px;
	padding:1px;
}

/**************************
 Select Boxes
 **************************/

.igspell_suggestions {
	position: absolute;
	left: 10px;
	top: 135px;
	width: 320px;
	height: 60px;
	border: 1px solid #CCCCCC;
	bottom: -1px;	
}

/**************************
 Labels
 **************************/
.igspell_label {
	font-weight: bold;
	font-size: xx-small;
	margin-bottom: 2px;
	position: absolute;
	left: 10px;
	color: #151C55;
	width: 87px;
	bottom: -1px;
}

.igspell_changeToLabel {
	top: 200px;
	left: 0px;
	padding-left: 10px;
	padding-top: 3px;
	margin-left: 0px;
	border-top: 1px dashed #999999;
	width: 100%;
}

.igspell_suggestionsLabel {
	top: 113px;
	left: 0px;
	padding-left: 10px;
	padding-top: 3px;	
	margin-left: 0px;
	border-top: 1px dashed #999999;
	width: 100%;
}

.igspell_notFoundLabel {
	top: 5px;
}

/*************************
Other
**************************/
.igspell_BadWord
{
	BORDER-BOTTOM: red 1px solid;	
	color: Green;
	font-weight:bold;
} 
   
   </style>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
    <div>
      <ig_spell:WebSpellCheckerDialog ID="WebSpellCheckerDialog1" runat="server" ShowAddButton="None" Width="274px">
        <Template>
         
            <div id="documentTextPanel" runat="server" class="igspell_documentTextPanel">
            </div>
            <span id="changeToLabel" runat="server" class="igspell_label igspell_changeToLabel">
              Change To:</span>
            <input id="changeToBox" runat="server" class="igspell_changeToBox" type="text" />
            <select id="suggestions" runat="server" class="igspell_suggestions" size="6">
            </select>
            <span id="suggestionsLabel" runat="server" class="igspell_label igspell_suggestionsLabel">
              Suggestions:</span>
            <button id="ignoreButton" runat="server" class="igspell_button igspell_ignoreButton">
              Ignore</button>
            <button id="ignoreAllButton" runat="server" class="igspell_button igspell_ignoreAllButton">
              Ignore All</button>
            <button id="addButton" runat="server" class="igspell_button igspell_addButton" style="visibility: ;
              display: ;">
              Add</button>
            <button id="changeButton" runat="server" class="igspell_button igspell_changeButton">
              Change</button>
            <button id="changeAllButton" runat="server" class="igspell_button igspell_changeAllButton">
              Change All</button>
            <button id="finishButton" runat="server" class="igspell_button igspell_finishButton">
              Finish</button>
          </div>
        </Template>
      </ig_spell:WebSpellCheckerDialog>
            <input type="button" class="igspell_button igspell_addButton" onclick="AjaxSubmitWord(this);"  value="Add" id="Button1"/>
      <script type="text/javascript" language="javascript">
      var badWords = octl00_HOCP_WebSpellCheckerDialog1._dialog.getBadWords();        
      var addButton;
      </script>
      <input type="hidden" id="aj_id" name="aj_id" runat="server"/>
      <input type="hidden" id="aj_cid" name="aj_cid" runat="server"/>
      <input type="hidden" id="aj_uid" name="aj_uid" runat="server"/>
      <input type="hidden" id="aj_un" name="aj_un" runat="server"/>
          </div>
      </asp:Content>