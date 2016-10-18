<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Inputforms.InputForms_Properties" CodeFile="InputForms_Properties.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ucs" TagName="PLWebTree" Src="../PLWebTree.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" language="javascript">
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
 		    if (oButton.Key == 'Clone') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
		  }
		  
		    
		function Redirect(id)
		{
			top.Redirect(id);
		} 
  </script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving button issue  by Radha S--%>
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
              <igtbar:TBarButton Text="Save" Image="/hc_v4/img/ed_save.gif" Key="Save">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
              <igtbar:TBarButton Text="Delete" Image="/hc_v4/img/ed_delete.gif" ToolTip="Delete from library"
                Key="Delete">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="CloneSep"></igtbar:TBSeparator>
              <igtbar:TBarButton Text="Clone" Image="/hc_v4/img/ed_copy.gif" ToolTip="Clone the inputform"
                Key="Clone">
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
                  <asp:Panel ID="panelId" runat="server" Visible="False">
                    <tr valign="middle">
                      <td class="editLabelCell" style="width: 80px">
                        <asp:Label ID="Label1" runat="server">Id</asp:Label></td>
                      <td class="uga">
                        <asp:TextBox ID="txtInputFormId" runat="server" Width="30px" Enabled="False"></asp:TextBox></td>
                    </tr>
                  </asp:Panel>
                  <tr valign="middle">
                    <td class="editLabelCell" style="width: 80px">
                      <asp:Label ID="Label3" runat="server">Name</asp:Label></td>
                    <td class="ugd">
                      <asp:TextBox ID="txtInputFormName" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rv1" runat="server" CssClass="errorMessage" ControlToValidate="txtInputFormName"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                  </tr>
                  <tr valign="middle">
                    <td class="editLabelCell" style="width: 80px">
                      <asp:Label ID="Label2" runat="server">Short Name</asp:Label></td>
                    <td class="uga">
                      <asp:TextBox ID="txtInputFormShortName" runat="server" Visible="true" Width="150px" MaxLength="15"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rv2" runat="server" CssClass="errorMessage" ControlToValidate="txtInputFormShortName"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                  </tr>
                  <tr valign="middle">
                    <td class="editLabelCell" style="width: 80px">
                      <asp:Label ID="Label4" runat="server">Description</asp:Label></td>
                    <td class="ugd">
                      <asp:TextBox ID="txtDescription" runat="server" Visible="true" MaxLength="500" Columns="60"
                        Rows="2" TextMode="MultiLine"></asp:TextBox></td>
                  </tr>
                  <tr valign="middle">
                    <td class="editLabelCell" style="width: 80px">
                      <asp:Label ID="Label6" runat="server">Type</asp:Label></td>
                    <td class="uga">
                      <asp:DropDownList ID="ddlInputFormType" runat="server" DataTextField="Name" DataValueField="Code">
                      </asp:DropDownList>
                    </td>
                  </tr>
                  <tr valign="middle">
                    <td class="editLabelCell" style="width: 80px">
                      <asp:Label ID="Label5" runat="server">Active</asp:Label></td>
                    <td class="ugd">
                      <asp:CheckBox ID="cbIsActive" runat="server"></asp:CheckBox>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td>
                <asp:HyperLink ID="hlCreator" runat="server"></asp:HyperLink>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
</asp:Content>