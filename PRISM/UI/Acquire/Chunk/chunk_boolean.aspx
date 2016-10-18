<%@ Reference Page="~/ui/acquire/chunk/chunk.aspx" %>
<%@ Register TagPrefix="hc" TagName="chunkModifier" Src="Chunk_Modifier.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkcomment" Src="Chunk_Comment.ascx"%>
<%@ Register TagPrefix="hc" TagName="chunkbuttonbar" Src="Chunk_ButtonBar.ascx"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Boolean" CodeFile="Chunk_Boolean.aspx.cs" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk Text</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script>
      top.window.focus();
      var toolBar;
      var BlankValue;//ACQ10.0
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		   if (oButton.Key=='ilb')
			  {			  
			  //ACQ10.0 Starts
			    //var cvy = document.getElementById("rdYes");           // Commented
			    //var cvn = document.getElementById("rdNo");            // Commented
			    var cvy = document.getElementById("ctl00_HOCP_rdYes");
			    var cvn = document.getElementById("ctl00_HOCP_rdNo");
			    //ACQ10.0 Starts
                if (oButton.getSelected())
                  {
	  	            cvy.checked = false;
	  	            cvn.checked = false;
                    cvy.disabled = true;
                    cvn.disabled = true;
                  }
                else{
                    cvy.disabled = false;
                    cvn.disabled = false;
	  	            cvy.checked = false;
	  	            cvn.checked = false;
                  }
			    return;			    
			  }
		  }
		  //ACQ10.0 Starts
		  function disableRadioButton()
		  {
		        var cvy = document.getElementById("ctl00_HOCP_rdYes");
			    var cvn = document.getElementById("ctl00_HOCP_rdNo");
		        cvy.checked = false;
	  	        cvn.checked = false;
                cvy.disabled = true;
                cvn.disabled = true;
		  }
		  //ACQ10.0 End
		  function dc()
		  {
		    if (typeof(top.ChunkIsChanged == 'function')){     
            top.ChunkIsChanged();
        }
      }
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script>
    document.body.onload=function()
        {
            try
                {   //ACQ10.0 Starts
                    //document.getElementById('txtValue').focus(); //Commented
                    if (BlankValue == true)
                    disableRadioButton();
                    //ACQ10.0 Ends
                }
          catch(e){}
        };
  </script>
      <hc:chunkbuttonbar id="ChunkButtonBar" runat="server"></hc:chunkbuttonbar>
      <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
        <tr valign="top" style="height:1px">
          <td>
            <fieldset>
              <legend><asp:label id="Label2" runat="server">Value is</asp:label>&nbsp;<asp:label id="lbStatus" runat="server">Label</asp:label>&nbsp;<asp:image id="imgStatus" runat="server" AlternateText="status" ImageAlign="Middle"></asp:image>&nbsp;</legend>
              <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr valign="top" bgColor="#d6d3ce">
                  <td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" width="100%" ImageDirectory=" " CssClass="hc_toolbar2">
                      <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
                      <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
                      <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
                      <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
                      <SelectedStyle BorderWidth="1px" BorderStyle="Inset" ForeColor="Red" BackColor="Gainsboro"></SelectedStyle>
                      <Items>
                        <igtbar:TBarButton ToggleButton="True" Key="ilb" ToolTip="Force this chunk to be intentionaly left blank" Text="Blank" Image="/hc_v4/img/ed_blank.gif"></igtbar:TBarButton>
                      </Items>
                    </igtbar:ultrawebtoolbar></td>
                </tr>
                <tr valign="top">
                  <td>
                    <asp:RadioButton id="rdYes" runat="server" GroupName="rdYesNo" Text="Yes"></asp:RadioButton>
                    <asp:RadioButton id="rdNo" runat="server" GroupName="rdYesNo" Text="No"></asp:RadioButton></td>
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
        <tr valign="bottom" height="*">
          <td colspan="2"></td>
        </tr>
      </table>
</asp:Content>