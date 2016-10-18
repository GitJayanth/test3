<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Admin.Architecture.ConfigFile" CodeFile="configFile.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ucs" TagName="Location" Src="configFile.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Configuration file</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script>
    function showHideFieldset(fieldset)
    {
      var l = fieldset.childNodes.length;
      if (l > 1)
        for (i=1;i<l;i++)
          fieldset.childNodes[i].style.display=(fieldset.childNodes[i].style.display==''?'none':'');
    }
  </script>
  <STYLE>
    .lbl_msgInformation{font-weight:bold;color:green;}
    .lbl_msgWarning{font-weight:bold;color:orange;}
    .lbl_msgError{font-weight:bold;color:red;}
    PRE {font-size:xx-small;}
  </STYLE>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <table style="WIDTH: 100%; HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
	<asp:panel id="panelProperties" Visible="true" Runat="server">
		<tr valign="top" style="height:1px">
			<td>
				<igtbar:ultrawebtoolbar id="mainToolBar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="mainToolBar_ButtonClicked">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="Save" ToolTip="Save changes" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator />
					  <igtbar:TBLabel Key="LibraryListLbl" Text="Application">
						  <DefaultStyle Font-Bold="True" Width="80px"></DefaultStyle>
					  </igtbar:TBLabel>
					  <igtbar:TBCustom Width="220px" Key="appList">
						  <asp:DropDownList Width="220px" ID="ddlApps" AutoPostBack="True" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlApps_SelectedIndexChanged"></asp:DropDownList>
					  </igtbar:TBCustom>
						<igtbar:TBLabel Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="top">
			<td>
				<asp:Label id="propertiesMsgLbl" style="font-weight:bold;" runat="server"></asp:Label>
	      <ucs:Location id="mainLocation" runat="server" configuration='<%# configuration %>'></ucs:Location>
			</td>
		</tr>
	</asp:panel>
	  </table>

</asp:Content>
