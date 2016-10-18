<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="term"
  CodeFile="Term_TranslationEdit.aspx.cs" %>

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Term translation edit</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
			function ReloadParent(){
		    top.opener.document.getElementById("action").value = "reload";
        top.opener.document.forms[0].submit();
		  }
		  
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		   if (oButton.Key == 'Cancel') {
          oEvent.cancelPostBack = true;
			    window.close();
        }
       if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		    if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
		}		  
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table>
    <tr height="1" valign="top">
      <td>
        <igtbar:UltraWebToolbar ID="uwToolbarTitle" runat="server" ImageDirectory=" " CssClass="hc_toolbar"
          ItemWidthDefault="80px" Width="100%">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBLabel Text="Action" Key="Action">
              <DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left">
              </DefaultStyle>
            </igtbar:TBLabel>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>
    <tr height="*" valign="bottom">
      <td align="left">
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Delete" ToolTip="Delete the chunk" Text="Delete" Image="/hc_v4/img/ed_delete.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator Key="SepCancel"></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Cancel" ToolTip="Cancel" Text="Cancel" Image="/hc_v4/img/ed_cancel.gif">
            </igtbar:TBarButton>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
          <ClientSideEvents Click="uwToolbar_Click" />
        </igtbar:UltraWebToolbar>
        <fieldset>
          <legend>
            <asp:Label ID="Label2" runat="server">Master</asp:Label>
          </legend>
          <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
              <td width="95%">
                <asp:TextBox ID="txtTermValueMaster" runat="server" Width="95%" ReadOnly="True" TextMode="MultiLine"
                  Height="80px"></asp:TextBox></td>
            </tr>
          </table>
        </fieldset>
      <fieldset>
        <legend>
          <asp:Label ID="lbLanguage" runat="server">
							<%# languageCode %>
          </asp:Label>
        </legend>
        <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
          <tr>
            <td width="95%">
              <asp:TextBox ID="txtTermValue" runat="server" Width="95%" TextMode="MultiLine" Height="80px"></asp:TextBox>
            </td>
            <td>
              &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                ControlToValidate="txtTermValue"></asp:RequiredFieldValidator>
            </td>
          </tr>
        </table>
      </fieldset>
      <fieldset>
        <legend>
          <asp:Label ID="Label11" runat="server">Author</asp:Label>
        </legend>
        <asp:HyperLink ID="hlCreator" runat="server">HyperLink</asp:HyperLink>&nbsp; [
        <asp:Label ID="lbOrganization" runat="server">Organization</asp:Label>] -
        <asp:Label ID="lbCreatedOn" runat="server"></asp:Label></fieldset>
      <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">message</asp:Label>
      </td>
    </tr>
  </table>
</asp:Content>
