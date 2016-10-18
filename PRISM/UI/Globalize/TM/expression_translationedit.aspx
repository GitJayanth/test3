<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="hc_termbase.UI.Globalize.TM.Expression_TranslationEdit" CodeFile="Expression_TranslationEdit.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">TM expression translation edit</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script>
			    function ReloadParent() {
			        top.opener.document.getElementById("action").value = "reload";
			        top.opener.document.forms[0].submit();
			    }

			    function uwToolbar_Click(oToolbar, oButton, oEvent) {
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
			<tr height="1" valign="top">
				<td><igtbar:ultrawebtoolbar id="uwToolbarTitle" runat="server" ImageDirectory=" " CssClass="hc_toolbar" ItemWidthDefault="80px">
						
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBLabel Text="Action" Key="Action">
								<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
							</igtbar:TBLabel>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar></td>
			</tr>
            <%--Removed width property from ultrawebtoolbar by Radha To fix moving icon issue--%>
			<tr height="*" valign="bottom">
				<td align="left"><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="Delete" ToolTip="Delete the chunk" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
						</Items>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					</igtbar:ultrawebtoolbar>
					<FIELDSET>
						<LEGEND>
							<asp:label id="Label2" runat="server">Master</asp:label>
						</LEGEND>
						<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
							<TR>
								<td width="95%"><asp:textbox id="txtTMExpressionValueMaster" runat="server" width="95%" ReadOnly="True" TextMode="MultiLine"
										Height="80px"></asp:textbox></td>
							</tr>
						</table>
					</FIELDSET>
				</td>
				<FIELDSET>
					<LEGEND>
						<asp:label id="lbLanguage" runat="server">
							<%# languageCode %>
						</asp:label>
					</LEGEND>
					<table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
						<TR>
							<td width="95%"><asp:textbox id="txtTMExpressionValue" runat="server" width="95%" TextMode="MultiLine" Height="80px"></asp:textbox></td>
						</tr>
					</table>
				</FIELDSET>
				<FIELDSET><LEGEND>
						<asp:label id="Label11" runat="server">Author</asp:label>
					</LEGEND>
				<asp:hyperlink id="hlCreator" runat="server">HyperLink</asp:hyperlink>&nbsp; [
				<asp:label id="lbOrganization" runat="server">Organization</asp:label>] -
				<asp:Label id="lbCreatedOn" Runat="server"></asp:Label></FIELDSET>
				<asp:Label id="lbMessage" runat="server" Width="100%" Visible="False">message</asp:Label>
</tr>
</asp:Content>