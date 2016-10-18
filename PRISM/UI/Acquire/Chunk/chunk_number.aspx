<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Reference Control="~/ui/acquire/chunk/containerinfo.ascx" %>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Number" CodeFile="Chunk_Number.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk number</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script type="text/javascript" language="javascript">
      top.window.focus();
      var ILBText = "Intentionaly left blank";
      var toolBar;
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
			  if (oButton.Key=='ilb'){					  
			  // 1035 fix (Deepak S) START -- Unable to set ##BLANK##
			  /*
			    var cv = igedit_getById("txtValue");
			    cv.setNumber(null);
			    */
			    // 1035 fix (Deepak S) END -- Unable to set ##BLANK##
			    var cv = igedit_getById("ctl00_HOCP_txtValue");				    		    
			    cv.setNumber(0);			    
			    for(i = 0; i < cv.Element.all.length; i++){
            cv.Element.all(i).disabled = oButton.getSelected();            
          }
          if (!oButton.getSelected()){
            cv.setNumber(0);
            cv.focus();
          }
			    return;			    
			  }
		  }

	  
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <script id="Infragistics" type="text/javascript">
function txtValue_ValueChange(oEdit, oldValue, oEvent){
  if (oEdit.getValue() != oldValue && typeof(top.ChunkIsChanged == 'function')){     
    top.ChunkIsChanged();
  }
}
function txtValue_KeyPress(oEdit, keyCode, oEvent){
  if (typeof(top.ChunkIsChanged == 'function')){     
    top.ChunkIsChanged();
  }
}
function txtValue_Spin(oEdit, delta, oEvent){
  if (typeof(top.ChunkIsChanged == 'function')){     
    top.ChunkIsChanged();
  }
}
</script>
<script type="text/javascript" language="javascript">
		document.body.onload = function(){try{document.getElementById('txtValue').focus();}catch(e){}};
</script>
      <hc:chunkbuttonbar id="ChunkButtonBar" runat="server"></hc:chunkbuttonbar>
      <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:1px">
          <td>
            <fieldset>
              <legend>
                <asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" ImageAlign="Middle" AlternateText="status"></asp:image>&nbsp;
                <asp:requiredfieldvalidator id="requiredValue" runat="server" Display="Dynamic" ControlToValidate="txtValue" ErrorMessage="Please enter a value"></asp:requiredfieldvalidator></legend>
              <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr valign="top" bgColor="#d6d3ce">
                  <td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" width="100%" ImageDirectory=" " CssClass="hc_toolbar2">

                      <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
                      <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                      <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                      <Items>
                        <igtbar:TBarButton ToggleButton="True" Key="ilb" Text ="Blank" ToolTip="Force this chunk to be intentionaly left blank"
                          Image="/hc_v4/img/ed_blank.gif">
                          <SelectedStyle BorderWidth="1px" BorderStyle="Inset"></SelectedStyle>
                        </igtbar:TBarButton>
                        </Items>
                    </igtbar:ultrawebtoolbar></td>
                </tr>
                <tr valign="top">
                  <td>
                    <igtxt:WebNumericEdit id="txtValue" runat="server" Width="100px" HideEnterKey="True" HorizontalAlign="Left">
                      <SpinButtons DefaultTriangleImages="ArrowSmall" Display="OnRight" EnsureFocus="True"></SpinButtons>
                      <ClientSideEvents ValueChange="txtValue_ValueChange" KeyPress="txtValue_KeyPress" Spin="txtValue_Spin" />
                    </igtxt:WebNumericEdit>
                  </td>
                </tr>
              </table>
              <div id="textcount"></div>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <hc:chunkcomment id="ChunkComment1" runat="server"></hc:chunkcomment>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <hc:chunkmodifier id="ChunkModifier1" runat="server"></hc:chunkmodifier>
            <center>
              <asp:Label id="lbResult" runat="server" Font-Size="7pt">Result message</asp:Label></center>
          </td>
        </tr>
      </table>
</asp:Content>