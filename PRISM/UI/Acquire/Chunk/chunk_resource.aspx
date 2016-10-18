<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Resource" validateRequest="false" CodeFile="Chunk_Resource.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk resource</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script language="javascript" type="text/javascript">
      top.window.focus();
      //alert(window.document.domain);
      var ILBText = "Intentionaly left blank";
      var toolBar;
      var damUrl='';
      function uwToolbar_Click(oToolbar, oButton, oEvent){
        var bt1 = oToolbar.Items[0]; // ilb
        var bt2 = oToolbar.Items[2]; // dam
        var cv = document.getElementById('ctl00_HOCP_txtValue');
        var cvImg = document.getElementById('ctl00_HOCP_txtImagePath');
			  if (oButton.Key=='ilb'){		
          if (oButton.getSelected()){
            cv.value = ILBText;
            cvImg.value = ILBText;
          }
          else{
            cv.value='';
            cvImg.value = '';
            cv.focus();
          }
			    return;			    
			  }
			  if (oButton.Key == 'dam'){
			    if (!bt1.getSelected()){
  			    var params = '&resource=';
	  		    if (cv.value != '' || !cv.disabled){
		  	      params += cv.value;
			      }
 	          params += "&openerfunc=top.opener.UpdateRes";
 	          damUrl = "../dam/QuickDAMImages.aspx?inframe=1&culture=<%= culture.Code%>";
	          var url = damUrl + params;
	          var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
            var damResource = OpenModalWindow(url,'DAM', 600, 800, 'yes');
        
  	        ///var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
    	      //var damResource = window.showModalDialog(url,window.self,"dialogWidth:800px;dialogHeight:600px;help:0;status:yes;resizable:yes;scroll:yes;");
    	      
            /*if (damResource){
              cv.value = damResource;
              cvImg.value = damResource;
            }*/
  	        oEvent.cancelPostBack = true;
  	        return;
			    }
			  }
		  }
		  function UpdateRes(damResource){
        var cv = document.getElementById('ctl00_HOCP_txtValue');
        var cvImg = document.getElementById('ctl00_HOCP_txtImagePath');
        cv.value = damResource;
        cvImg.value = damResource;
		    if (typeof(top.ChunkIsChanged == 'function')){     
          top.ChunkIsChanged();
        }
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
          <td>
            <fieldset>
              <legend><asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" AlternateText="status" ImageAlign="Middle"></asp:image>&nbsp;
                <asp:requiredfieldvalidator id="requiredValue" runat="server" ErrorMessage="Required" ControlToValidate="txtValue"
                  Display="Dynamic"></asp:requiredfieldvalidator></legend>
              <table cellspacing="0" cellpadding="0" border="0">
                <tr valign="top" bgcolor="#d6d3ce">
                  <td>
                    <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="20px" ImageDirectory=" "
                      CssClass="hc_toolbar2">
                      <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
                      <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                      <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                      <Items>
                        <igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionaly left blank"
                          Image="/hc_v4/img/ed_blank.gif">
                          <SelectedStyle BorderWidth="1px" BorderStyle="Inset"></SelectedStyle>
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="ilbSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton Key="dam" ToolTip="Browse the Digital Asset Manager" Text="" Image="/hc_v4/img/ed_dam.gif"></igtbar:TBarButton>
                      </Items>
                      <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
                    </igtbar:ultrawebtoolbar></td>
                </tr>
                <tr valign="top">
                  <td><asp:textbox id="txtValue" runat="server" Columns="80" Rows="5">Value</asp:textbox></td>
                </tr>
              </table>
              <div id="textcount"></div>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td><hc:chunkcomment id="ChunkComment1" runat="server"></hc:chunkcomment></td>
        </tr>
        <tr valign="top" style="height:1px">
          <td><hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
            <center><asp:label id="lbResult" runat="server" Font-Size="7pt">Result message</asp:label></center>
          </td>
        </tr>
      </table>
      <input id="txtCulture" type="hidden" name="txtCulture" runat="server"/>
      <input id="txtImagePath" type="hidden" name="txtImagePath" runat="server"/>
</asp:Content>