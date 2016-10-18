<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="role_Properties"
  CodeFile="role_properties.aspx.cs" %>

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">

  <script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'List') {
          back();
          oEvent.cancelPostBack = true;
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
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <tr valign="bottom" height="*">
    <td>
      <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" ItemWidthDefault="80px"
        CssClass="hc_toolbar">
        <HoverStyle CssClass="hc_toolbarhover">
        </HoverStyle>
        <DefaultStyle CssClass="hc_toolbardefault">
        </DefaultStyle>
        <SelectedStyle CssClass="hc_toolbarselected">
        </SelectedStyle>
        <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
        </ClientSideEvents>
        <Items>
          <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator></igtbar:TBSeparator>
          <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif">
          </igtbar:TBarButton>
          <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
          <igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif">
          </igtbar:TBarButton>
        </Items>
      </igtbar:UltraWebToolbar>
    </td>
  </tr>
  <tr valign="top" style="height: 1px">
    <td>
      <br />
      <asp:Label ID="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
    </td>
  </tr>
  <tr valign="top">
    <td>
      <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <asp:Panel ID="panelId" Visible="False" runat="server">
          <tbody>
            <tr valign="middle">
              <td class="editLabelCell">
                <asp:Label ID="Label1" runat="server">Id</asp:Label></td>
              <td class="ugd">
                <asp:TextBox ID="txtRoleId" runat="server" Width="30px" Enabled="false" MaxLength="1"></asp:TextBox></td>
            </tr>
        </asp:Panel>
        <tr valign="middle">
          <td class="editLabelCell">
            <asp:Label ID="Label3" runat="server">Name</asp:Label>
          </td>
          <td class="uga">
            <asp:TextBox ID="txtRoleName" runat="server" Visible="true" MaxLength="50" Columns="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" runat="server" CssClass="errorMessage"
              ErrorMessage="*" ControlToValidate="txtRoleName"></asp:RequiredFieldValidator>
          </td>
        </tr>
        <tr valign="top">
          <td class="editLabelCell">
            <asp:Label ID="Label5" runat="server">Description</asp:Label>
          </td>
          <td class="ugd">
            <asp:TextBox ID="txtDescription" runat="server" Visible="true" MaxLength="500" TextMode="MultiLine"
              Rows="2" Columns="60"></asp:TextBox>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</asp:Content>
