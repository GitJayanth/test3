<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="userprofile_identification" CodeFile="userprofile_identification.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Identification</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
    <script type="text/javascript" language="javascript">
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		  } 
    </script>
</asp:Content>
<%--To fix QC 7691 - Removed width property form ultrawebtoolbar by Radha S --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr valign="top" style="height: 1px">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <CLIENTSIdEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIdEEVENTS>
              <ITEMS>
                <igtbar:TBarButton Text="Save" Image="/hc_v4/img/ed_save.gif" Key="Save"></igtbar:TBarButton>
              </ITEMS>
            </igtbar:ultrawebtoolbar>
          </td>
        </tr>
       <tr valign="top">
        <td>
          <asp:label id="lbError" runat="server" Visible="false"></asp:label>
        </td>
        </tr>
        <tr valign="top">
          <td>
            <table id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
              <tr>
                <td class="editLabelCell">
                  <asp:label id="lbPseudo" runat="server">Pseudo</asp:label></td>
                <td class="ugd">
                  <asp:textbox id="txtPseudo" ReadOnly ="true"  runat="server" Text="<%# currentUser.Pseudo %>" width="180px">
                  </asp:textbox>
                  <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" CssClass="errorMessage" ErrorMessage="*"
                    ControlToValidate="txtPseudo"></asp:requiredfieldvalidator>
                </td>
              </tr>
              <tr valign="middle">
                <td class="editLabelCell">
                  <asp:label id="lbFirstName" runat="server">First name</asp:label></td>
                <td class="ugd">
                  <asp:textbox id="txtUserId" runat="server" Visible="false" Text="<%# currentUser.Id %>" width="0px">
                  </asp:textbox>
                  <asp:textbox id="txtRoleId" runat="server" Visible="false" Text="" width="0px"></asp:textbox>
                  <asp:textbox id="txtOrgId" runat="server" Visible="false" Text="<%# currentUser.OrgId %>" width="0px">
                  </asp:textbox>
                  <asp:textbox id="txtFirstName" runat="server" ReadOnly ="true" Text="<%# currentUser.FirstName %>" width="180px">
                  </asp:textbox>
                  <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="errorMessage" ErrorMessage="*"
                    ControlToValidate="txtFirstName"></asp:requiredfieldvalidator>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell">
                  <asp:label id="lbLastName" runat="server">Last name</asp:label></td>
                <td class="ugd">
                  <asp:textbox id="txtLastName" ReadOnly ="true" runat="server" Text="<%# currentUser.LastName %>" width="180px">
                  </asp:textbox>
                  <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="errorMessage" ErrorMessage="*"
                    ControlToValidate="txtLastName"></asp:requiredfieldvalidator>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="height: 23px">
                  <asp:label id="lbEmail" runat="server">Email</asp:label></td>
                <td class="ugd" style="height: 23px">
                  <asp:textbox id="txtEmail"  ReadOnly ="true" runat="server" Text="<%# currentUser.Email %>" width="180px"></asp:textbox>
                  <asp:RegularExpressionValidator id="regEmail" runat="server" ErrorMessage="Invalid email address"
                    Display="Dynamic" ValidationExpression="\b^[A-Za-z0-9](([_\.\-\!\#\$\%\&\'\*\+\\\/\=\?\^\`\{\|\}\~]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$\b" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                  <asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" CssClass="errorMessage" ErrorMessage="*"
                    ControlToValidate="txtEmail"></asp:requiredfieldvalidator>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="height: 23px">
                  <asp:label id="Label2" runat="server">Additional emails</asp:label></td>
                <td class="ugd" style="height: 23px">
                  <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                      <td>
                        <asp:textbox id="txtEmail2" runat="server" Text="<%# currentUser.Email2 %>" width="180px"></asp:textbox>
                        <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email address"
                        Display="Dynamic" ValidationExpression="\b^[A-Za-z0-9](([_\.\-\!\#\$\%\&\'\*\+\\\/\=\?\^\`\{\|\}\~]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$\b" ControlToValidate="txtEmail2"></asp:RegularExpressionValidator>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:textbox id="txtEmail3" runat="server" Text="<%# currentUser.Email3 %>" width="180px"></asp:textbox>
                         <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid email address"
                        Display="Dynamic" ValidationExpression="\b^[A-Za-z0-9](([_\.\-\!\#\$\%\&\'\*\+\\\/\=\?\^\`\{\|\}\~]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$\b" ControlToValidate="txtEmail3"></asp:RegularExpressionValidator>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="height: 23px">
                  <asp:label id="Label1" runat="server">Time Zone</asp:label></td>
                <td class="ugd" style="height: 23px">
                  <asp:DropDownList id="DDL_TimeZone" runat="server"></asp:DropDownList></td>
              </tr>
              <tr>
                <td class="editLabelCell" style="height: 23px">
                  <asp:label id="Label3" runat="server">Format date</asp:label></td>
                <td class="ugd" style="height: 23px">
                  <asp:DropDownList id="DDLFormatDate" runat="server">
                  <asp:ListItem Value="MM/dd/yyyy">MM/dd/yyyy</asp:ListItem>
                  <asp:ListItem Value="dd/MM/yyyy">dd/MM/yyyy</asp:ListItem>
                  <asp:ListItem Value="yyyy/MM/dd">yyyy/MM/dd</asp:ListItem>
                  <asp:ListItem Value="MM-dd-yyyy">MM-dd-yyyy</asp:ListItem>
                  <asp:ListItem Value="dd-MM-yyyy">dd-MM-yyyy</asp:ListItem>
                  <asp:ListItem Value="yyyy-MM-dd">yyyy-MM-dd</asp:ListItem>
                  </asp:DropDownList>
                </td>
              </tr>
              <tr>
                <td class="editLabelCell" style="height: 23px">
                  <asp:label id="Label4" runat="server">Format time</asp:label></td>
                <td class="ugd" style="height: 23px">
                  <asp:DropDownList id="DDLFormatTime" runat="server">
                  <asp:ListItem Value="HH:mm:ss">HH:mm:ss (24 hours)</asp:ListItem>
                  <asp:ListItem Value="hh:mm:ss">hh:mm:ss</asp:ListItem>
                  <asp:ListItem Value="h:m:s">h:m:s</asp:ListItem></asp:DropDownList></td>
              </tr>
            </table>
           </td>
         </tr>
      </table>
</asp:Content>