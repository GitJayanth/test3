<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Report_Properties" CodeFile="Report_Properties.aspx.cs"  %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Report properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
    <script>
      var txtCodeElement;
      var txtReportId;
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
<%--Removed the width property to fix the moving button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
      <table class="main" id="Table2" cellspacing="0" cellpadding="0">
        <tr valign="top" style="height:1px">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
              <Items>
                <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Syntax" ToolTip="Check SQL syntax" Text="Syntax" Image="/hc_v4/img/ed_ok.gif"></igtbar:TBarButton>
                <igtbar:TBarButton Key="Run" Text="Run" Image="/hc_v4/img/ed_play.gif"></igtbar:TBarButton>
                <igtbar:TBSeparator Key="SyntaxSep"></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Save" ToolTip="Save report in library" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
                <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
              </Items>
            </igtbar:ultrawebtoolbar></td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <br/>
            <asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
          </td>
        </tr>
        <tr valign="top">
          <td>
            <asp:hyperlink id="hlCreator" runat="server"></asp:hyperlink>
          </td>
        <TR>
        <tr valign="top">
          <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
              <asp:Panel ID="panelId" runat="server">
                <tr valign="middle">
                  <td class="editLabelCell" width="108"><asp:label id="lbId" runat="server">Id</asp:label></td>
                  <td class="uga" width="522"><asp:textbox id="txtId" runat="server" Enabled="false" Width="30px"></asp:textbox></td>
                </tr>
              </asp:Panel>
              <asp:panel id="panelDesc" Runat="server">
                <tr valign="middle">
                  <td class="editLabelCell" width="108">
                    <asp:label id="Label1" runat="server">Name</asp:label></td>
                  <td class="ugd" width="522">
                    <asp:textbox id="txtName" runat="server" MaxLength="50" Columns="50"></asp:textbox>
                    <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" CssClass="errorMessage" ControlToValidate="txtName"
                      ErrorMessage="*"></asp:requiredfieldvalidator></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" width="108">
                    <asp:label id="Label2" runat="server">Description</asp:label></td>
                  <td class="uga" width="522">
                    <asp:textbox id="txtDescription" runat="server" Columns="50" Rows="4" TextMode="MultiLine"></asp:textbox>
                    <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="errorMessage" ControlToValidate="txtDescription"
                      ErrorMessage="*"></asp:requiredfieldvalidator>
                  </td>
                </tr>
              </asp:panel>
              <asp:panel id="panelSql" Runat="server">
                <tr valign="middle">
                  <td class="editLabelCell" valign="top" width="108">
                    <asp:label id="Label3" runat="server"> Code</asp:label></td>
                  <td class="ugd" width="522">
                    <asp:textbox id="txtCode" runat="server" Columns="80" Rows="8" TextMode="MultiLine"></asp:textbox>
                    <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="errorMessage" ControlToValidate="txtCode"
                      ErrorMessage="*"></asp:requiredfieldvalidator></td>
                </tr>
              </asp:panel>
              <asp:panel id="panelRoles" Runat="server">
                <tr valign="middle">
                  <td class="editLabelCell" width="100">
                    <asp:label id="lbRole" Runat="server">Role</asp:label></td>
                  <td class="ugd">
                    <asp:DataList id="rolesDataList" runat="server" repeatcolumns="4" repeatdirection="vertical" repeatlayout="table">
                      <ItemTemplate>
                        <asp:CheckBox id="rolename" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:CheckBox>
                        <asp:TextBox id="roleid" runat="server" Width="0px" Text='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>
                        </asp:TextBox>
                      </ItemTemplate>
                    </asp:DataList></td>
                </tr>
              </asp:panel></table>
          </td>
        </tr>
      </table>
</asp:Content>