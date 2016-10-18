<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="expression_properties" CodeFile="Expression_Properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Expression properties</asp:Content>
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
		}		  
			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript">
    document.body.onload=function(){try{document.getElementById('txtValue').focus();}catch(e){}};
  </script>
			<tr valign="bottom" height="*">
				<td align="right">
					<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar"
						ImageDirectory=" ">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar></td>
			</tr>
			<table style="WIDTH: 100%; BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="0" border="0">
				<TBODY>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
								<legend>
									<asp:label id="lValue" runat="server">Value</asp:label>
								</legend>
								<table width="100%" border="0" cellpadding="0" cellspacing="0">
									<tr>
										<td width="95%">
											<asp:textbox id="txtTMValue" runat="server" TextMode="MultiLine" width="100%"></asp:textbox>
										</td>
										<td>&nbsp;
											<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtTMValue"></asp:RequiredFieldValidator>
										</td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
								<legend>
									<asp:label id="lComment" runat="server">Comment</asp:label>
								</legend>
								<asp:textbox id="txtComment" runat="server" Width="95%" Columns="60" TextMode="MultiLine" Rows="2"></asp:textbox>
							</fieldset>
						</td>
						</td><td></td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<FIELDSET><LEGEND>
									<asp:label id="lAuthor" runat="server">Author</asp:label>
								</LEGEND>
<asp:HyperLink id="hlCreator" runat="server">HyperLink</asp:HyperLink>&nbsp; 
      [ 
<asp:label id="lbOrganization" runat="server">Organization</asp:label>] - 
<asp:Label id="lbCreatedOn" Runat="server"></asp:Label></FIELDSET>
						</td>
					</tr>
				</TBODY>
			</table>
			<asp:Label id="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
</asp:Content>