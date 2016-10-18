<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_InputMask" CodeFile="Chunk_InputMask.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk Inputmask</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script>
      top.window.focus();
      var ILBText = "Intentionaly left blank";
      var toolBar;
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
			  if (oButton.Key=='ilb'){		
			    var cv = igedit_getById("txtValue");
			    //ACQ10.0 starts
			    if(oButton.getSelected())
			    {
                    Disable (true);     
                }
			    else
			    {
			         Disable(false);
			    }
			    //ACQ10.0 Ends
			    if (!oButton.getSelected())
			    {
			    cv.disabled =false;
			    }
			    else
			    {
			    cv.disabled =true;
			    }
			    for(i = 0; i < cv.Element.all.length; i++){
            isBlankChunk = cv.Element.all(i).disabled = oButton.getSelected();
          }
          if (!oButton.getSelected()){
            cv.setValue('');
            cv.focus();
          }
			    return;			    
			  }
		  }

//ACQ10.0 starts
function Disable(bvariable)
{
    var RegValidator = document.getElementById('ctl00_HOCP_requiredValue');	//ACQ10.0
    RegValidator.enabled = !bvariable;//ACQ10.0
    myForm = document.forms[0];
        for (i=0; i<myForm.length; i++)
        {
          if ((myForm.elements[i].id.indexOf('txtValue')!=-1))
             {
              myForm.elements[i].disabled = bvariable;
              if (bvariable == true)
                myForm.elements[i].value=ILBText;                
             }  
         }
}
//ACQ10.0 Ends
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <script id="Infragistics" type="text/javascript">
<!--
function txtValue_ValueChange(oEdit, oldValue, oEvent){
  if (typeof(top.ChunkIsChanged == 'function')){     
    top.ChunkIsChanged();
  }
}
// -->
</script>
<script type="text/javascript" language="javascript">
		document.body.onload = function(){try{Disable(isBlankChunk);document.getElementById('txtValue').focus();}catch(e){}};
</script>
      <hc:chunkbuttonbar id="ChunkButtonBar" runat="server"></hc:chunkbuttonbar>
      <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:1px">
          <td>
            <fieldset>
              <legend>
                <asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" ImageAlign="Middle" AlternateText="status"></asp:image>&nbsp;
                <asp:requiredfieldvalidator id="requiredValue" runat="server" Display="Dynamic" ControlToValidate="txtValue"
                  ErrorMessage="Required"></asp:requiredfieldvalidator></legend>
              <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr valign="top" bgColor="#d6d3ce">
                  <td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="20px" width="100%" ImageDirectory=" "
                      CssClass="hc_toolbar2">
                      <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
                      <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
                      <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                      <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                      <Items>
                        <igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionally left blank"
													Image="/hc_v4/img/ed_blank.gif">
													<SelectedStyle BorderWidth="1px" BorderStyle="Inset"></SelectedStyle>
												</igtbar:TBarButton>
                      </Items>
                    </igtbar:ultrawebtoolbar></td>
                </tr>
                <tr valign="top">
                  <td>
                    <igtxt:WebMaskEdit id="txtValue" runat="server" Width="100%" DisplayMode="Mask" InputMask="(###) ###-####">
                      <ClientSideEvents ValueChange="txtValue_ValueChange" />
                    </igtxt:WebMaskEdit>
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