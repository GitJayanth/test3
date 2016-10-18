<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Preview" CodeFile="Preview.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Preview</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script>
      window.focus();
      var id = 2660;
      //////////////////////////////////////////////////////////////////////////////////////////////
      function PT(){// preview
      //////////////////////////////////////////////////////////////////////////////////////////////
        if (id>=0){
          var l = document.all.dlLocalizations.value;
          var t = document.all.dlTemplates.value.toLowerCase();
          var n = document.all.dlTemplates.options[document.all.dlTemplates.selectedIndex].text;
          var params = 'i='+id + '&t=' + t+ '&l=' + l;
          var nHtml = n;
          if (nHtml.toLowerCase().indexOf('html')!=-1){
            params += '&out=html';
          }          
          var r = (Math.random() + Math.random()) * Math.random();
          params += "&z=" + r;
        	var url = 'PreviewTemplate.aspx?' + params;
        	
	        //OpenModalWindow(url, "Preview", 800, 700, "yes");
        	open(url, 'Preview','toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,resizable=1,width=700,height=600, left=0,top=0');
	        //window.showModalDialog(url, null, "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;dialogHeight:800px;dialogWidth:1000px;");
     	  }
      }      
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <style>
    BODY{BACKGROUND: #efebde;}
  </style>
  <script type="text/javascript" language="javascript">
    document.body.oncontextmenu=function(){return false;};
  </script>
      <table id="tblPreview" height="20" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
        <tr valign="middle" align="left">
          <td colspan="4">
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <Items>
                <igtbar:TBLabel Text="Item" Key="Item">
                  <DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBLabel Text="">
                  <DefaultStyle Width="5px"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBLabel Text="Culture" ImageAlign="Right" Key="Culture">
                  <DefaultStyle Width="180px" Font-Size="9pt" Font-Bold="True" TextAlign="Right"></DefaultStyle>
                </igtbar:TBLabel>
              </Items>
            </igtbar:ultrawebtoolbar>
          </td>
        </tr>
        <tr>
          <td class="editLabelCell">
			<asp:label id="Label1" runat="server">Localization</asp:label>&nbsp;
          </td>
          <td class="ugd">
            <asp:dropdownlist id="dlLocalizations" runat="server" DataTextField="Name" DataValueField="Code"></asp:dropdownlist>&nbsp;
          </td>
        </tr>
        <tr>
          <td class="editLabelCell">
            <asp:label id="Label2" runat="server">Template</asp:label>&nbsp;
          </td>
          <td class="ugd">
            <asp:dropdownlist id="dlTemplates" runat="server" DataTextField="TemplateNameAndType" DataValueField="TemplateFile"></asp:dropdownlist>&nbsp;&nbsp;
          </td>
        </tr>
        <tr>
          <td class="editLabelCell">
			<asp:label id="Label3" runat="server">Color</asp:label>&nbsp;
          </td>
          <td class="ugd">
            <asp:dropdownlist id="dlColors" runat="server"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
          </td>
        </tr>
        <tr>
          <td class="editLabelCell">
			<asp:label id="Label4" runat="server">Resolution</asp:label>&nbsp;
          </td>
          <td class="ugd">
            <asp:dropdownlist id="dlResolutions" runat="server">
				<asp:ListItem text="Preview" value="preview"></asp:ListItem>
				<asp:ListItem text="Low resolution" value="lowres"></asp:ListItem>
				<asp:ListItem text="High resolution" value="hires"></asp:ListItem>
            </asp:dropdownlist>
          </td>
        </tr>
        <tr>
          <td class="editLabelCell">
          </td>
          <td class="ugd">
            <asp:Button id="BtnGenerate" runat="server" Text="Go" onclick="BtnGenerate_Click"></asp:Button>
          </td>
        </tr>
      </table>
      <asp:Panel ID ="pnlMsg" runat="server">
      <br/>
      1 - <b>S</b>elect language and then template material<br/>
      2 - <b>P</b>ress the "Go" button<br/>
      3 - <b>Y</b>ou can then use the <i>File/Save as</i> menu item to save the file 
      on your local drive<br/>
      <br/>
      </asp:Panel>
      <asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
</asp:Content>