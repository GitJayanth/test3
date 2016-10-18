<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="Inputforms_productlines.aspx.cs" Inherits="UI_Admin_InputForms_Inputforms_productlines" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ucs" TagName="PLWebTree" Src="../PLWebTree.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">ProductLines</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" language="javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
        }
   	  }
   	  		  
   	  function RefreshTabs(key1, v1, key2, v2)
		  {
				try
				{
					var webTab = parent.igtab_getTabById('webTab');
		      if (webTab == null){
		        webTab = parent.igtab_getTabById('ctl00_HOCP_webTab'); // Master Pages since Migration to .Net 2.0
          }
				  var s = '';
				  
					var tab1 = webTab.Tabs[key1];
					if (tab1)
					{
						s = tab1.getText();
						if (s.indexOf('(')>0)
						{
							s = s.substring(0, s.indexOf('('));
						}
						s = s + '('+v1+')';
						tab1.setText(s);
					}
					var tab2 = webTab.Tabs[key2];
					if (tab2)
					{
						s = tab2.getText();
						if (s.indexOf('(')>0)
						{
							s = s.substring(0, s.indexOf('('));
						}
						s = s + '('+v2+')';
						tab2.setText(s);
					}
				}
				catch(e)
				{
					window.status = e.name+' - '+e.message;
				}
		  } 
  </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
    <table class="main" cellspacing="0" cellpadding="0">
      <tr style="height: 1px" valign="top">
        <td>
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
            CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
            <Items>
              <igtbar:TBarButton Text="List" Image="/hc_v4/img/ed_back.gif" ToolTip="Back to list"
                Key="List">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
              <igtbar:TBarButton Text="ApplyChanges" Image="/hc_v4/img/ed_save.gif" Key="ApplyChanges">
              <DefaultStyle Width=120px></DefaultStyle>
              </igtbar:TBarButton>
            </Items>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr valign="top" style="height: 1px">
        <td>
          <asp:Label ID="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
        </td>
      </tr>
      <tr valign="top">
        <td>
          <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
              <td>
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                  <tr valign="top">
                    <td class="editLabelCell" style="width: 80px">
                      <asp:Label ID="Label7" runat="server">Product Lines</asp:Label></td>
                    <td class="uga">
                       <ucs:PLWebTree id="PLTree" runat="server" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
</asp:Content>