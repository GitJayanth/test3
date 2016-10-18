<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="SpecificWord_Properties"
  CodeFile="Word_properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Term properties</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">

  <script type="text/javascript">

		function uwToolbar_Click(oToolbar, oButton, oEvent){
			if (oButton.Key == 'List') {
					back();
					oEvent.cancelPostBack = true;
				}
       if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		    if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure you want to delete this chunk ?");
        }
		}
  </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <script type="text/javascript" language="javascript">
    document.body.onload=function(){try{document.getElementById('txtValue').focus();}catch(e){}};
  </script>

  <tr valign="bottom" height="*">
    <td align="right">
      <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
        CssClass="hc_toolbar">
        <HoverStyle CssClass="hc_toolbarhover">
        </HoverStyle>
        <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
        </ClientSideEvents>
        <SelectedStyle CssClass="hc_toolbarselected">
        </SelectedStyle>
        <Items>
          <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator Key="Sep"></igtbar:TBSeparator>
          <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
          <igtbar:TBarButton Key="Delete" ToolTip="Delete the chunk" Text="Delete" Image="/hc_v4/img/ed_delete.gif">
          </igtbar:TBarButton>
        </Items>
        <DefaultStyle CssClass="hc_toolbardefault">
        </DefaultStyle>
      </igtbar:UltraWebToolbar>
    </td>
  </tr>
  <tr>
    <td>
      <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">message</asp:Label>
      <table style="width: 100%; border-collapse: collapse" cellspacing="0" cellpadding="0"
        border="0">
        <tr valign="top" height="1">
          <td>
            &nbsp;
          </td>
        </tr>
        <tr valign="top" height="1">
          <td>
            <fieldset>
              <legend>
                <asp:Label ID="Label2" runat="server">Value</asp:Label>
              </legend>
              <asp:TextBox ID="txtValue" runat="server" Width="95%"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                ControlToValidate="txtValue" Display="Dynamic"></asp:RequiredFieldValidator>
            </fieldset>
          </td>
        </tr>
        <tr valign="top" height="1">
          <td>
            <fieldset>
              <legend>
                <asp:Label ID="Label3" runat="server">Comment</asp:Label>
              </legend>
              <asp:TextBox ID="txtComment" runat="server" Width="95%" Columns="60" TextMode="MultiLine"
                Rows="2"></asp:TextBox>
            </fieldset>
          </td>
        </tr>
        <asp:Panel ID="panelSubmitter" runat="server" Visible="False">
          <tr valign="top" height="1">
            <td>
              <fieldset>
                <legend>
                  <asp:Label ID="Label11" runat="server">Submitted by</asp:Label>
                </legend>
                <asp:HyperLink ID="hlSubmitter" runat="server">HyperLink</asp:HyperLink>&nbsp; [
                <asp:Label ID="lbOrganizationSubmitter" runat="server">Organization</asp:Label>]
                -
                <asp:Label ID="lbSubmittedOn" runat="server"></asp:Label>
              </fieldset>
            </td>
          </tr>
        </asp:Panel>
        <asp:Panel ID="panelApprover" runat="server" Visible="False">
          <tr valign="top" height="1">
            <td>
              <fieldset>
                <legend>
                  <asp:Label ID="Label1" runat="server">Approved by</asp:Label>
                </legend>
                <asp:HyperLink ID="hlApprover" runat="server">HyperLink</asp:HyperLink>&nbsp; [
                <asp:Label ID="lbOrganizationApprover" runat="server">Organization</asp:Label>]
                -
                <asp:Label ID="lbApprovedOn" runat="server"></asp:Label>
              </fieldset>
            </td>
          </tr>
        </asp:Panel>
      </table>
    </td>
  </tr>
</asp:Content>
