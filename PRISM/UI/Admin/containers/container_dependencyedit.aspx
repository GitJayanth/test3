<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="container_dependencyEdit" CodeFile="container_dependencyEdit.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Add container dependency</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
        else if (oButton.Key == 'close'){
					top.window.close();
					oEvent.cancelPostBack = false;
        }
		  }
		  
		  function UpdateParent(featureContainerId)
		  {
				if (opener != null)
					opener.location = "./container_dependencies.aspx?c="+featureContainerId;
				if (top != null && top.window != null)
					top.window.close();
		  } 		
		</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellpadding="0" cellspacing="0">
				<tr height="1" valign="top">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%"
							ImageDirectory=" ">
							<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
							<CLIENTSIDEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIDEEVENTS>
							<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
							<ITEMS>
								<IGTBAR:TBARBUTTON Image="/hc_v4/img/ed_save.gif" Text="Save" Key="save"></IGTBAR:TBARBUTTON>
								<IGTBAR:TBSEPARATOR Key="SaveSep"></IGTBAR:TBSEPARATOR>
								<IGTBAR:TBARBUTTON Image="/hc_v4/img/ed_cancel.gif" Text="Close" Key="close"></IGTBAR:TBARBUTTON>
							</ITEMS>
							<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<TR>
								<td class="editLabelCell" height="21"><asp:label id="Label1" runat="server">Techspec container</asp:label></td>
								<td class="ugd" height="21">
									<asp:DropDownList id="ddlTechspecContainers" runat="server" Width="180px"></asp:DropDownList>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>