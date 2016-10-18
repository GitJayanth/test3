<%@ Reference Page="~/ui/acquire/qde/qde_additem.aspx" %>
<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_MoveStatusTo" CodeFile="QDE_MoveStatusTo.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Move status to</title>
		<meta http-equiv="Page-Enter" content="blendTrans(Duration=0.1)"/>
		<meta http-equiv="Page-Exit" content="blendTrans(Duration=0.1)"/>
		<link href="/hc_v4/css/hypercatalog.css" rel="stylesheet"/>
		<script type="text/javascript" language="javascript" src="/hc_v4/js/hypercatalog.js"></script>
		<script type="text/javascript" language="javascript">
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
				if (oButton.Key == 'Cancel')
				{
					oEvent.cancelPostBack = true;
					window.close();
				}
			}
			
			function UpdateAndClose(isClosed,ifId)
			{				
				if (opener != null){
				 if (ifId < 0 && opener.parent != null)
					 opener.parent.document.location = "QDE_main.aspx";
				 else
				 {
           opener.document.getElementById("action").value = "reload";
           opener.document.forms[0].submit();
				 }
			  }
				if (isClosed = 1)
					window.close();
			}
			
			function CheckedChanged()
			{
				var cbMastrerPub = document.getElementById('cbMasterPublishing');
				if (cbMastrerPub != null)
				{
					if (cbMastrerPub.checked)
					{
						divCalendar.style.visibility = 'visible';
					}
					else
					{
						divCalendar.style.visibility = 'hidden';
					}
				}
			}
			
			function Initialize()
			{
				CheckedChanged();
			}
		</script>
	</head>
	<body style="BORDER-RIGHT: 0px; BORDER-TOP: 0px; BORDER-LEFT: 0px; BORDER-BOTTOM: 0px"
		scroll="no" onload='Initialize();'>
		<form id="Form1" method="post" runat="server">
		<asp:label id="lblWarningMessage" runat="server" Visible="False">Label</asp:label>
    	<asp:Panel ID="panelMATF" Runat="server">
			<table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbTitle" Runat="server" Width="100%" Font-Size="9pt" Font-Bold="True" CssClass="hc_toolbartitle">ItemName</asp:label></td>
				</tr>
				<tr valign="top" height="25">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" width="100%" ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Save" ToolTip="Move status to..." Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Cancel" ToolTip="Close window" Text="Cancel" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Text="">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="Culture" ToolTip="CultureCode" ImageAlign="Right" Key="Culture">
									<DefaultStyle Width="180px" Font-Size="9pt" Font-Bold="True" TextAlign="Right"></DefaultStyle>
								</igtbar:TBLabel>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbError" runat="server" CssClass="hc_error">lbError</asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td width="200" class="editLabelCell"><asp:label id="lbStatus" runat="server">Select the status</asp:label></td>
								<td class="uga" colspan="2">
									<asp:RadioButton id="rbDraft" runat="server" GroupName="rbStatus" Text="Draft" AutoPostBack="True" OnCheckedChanged="rbDraft_CheckedChanged"></asp:RadioButton>
									<asp:RadioButton id="rbFinal" runat="server" GroupName="rbStatus" Text="Final" AutoPostBack="True" OnCheckedChanged="rbFinal_CheckedChanged"></asp:RadioButton></td>
							</tr>
							<asp:Panel ID="pnlChildren" Runat="server">
								<tr>
									<td class="editLabelCell" width="200">
										<asp:label id="lbWithChildren" runat="server">Select the check box if you wish to include the children</asp:label></td>
									<td class="ugd" colspan="2" id="tdChildren" runat="server">
										<asp:checkbox id="cbWithChildren" runat="server"></asp:checkbox></td>
								</tr>
							</asp:Panel>
							<asp:Panel ID="pnlSoftRoll" Runat="server">
								<tr>
									<td class="editLabelCell" width="200">
										<asp:label id="lbSoftRoll" runat="server">The current node has soft roll. Select original node or soft roll to apply this function.</asp:label></td>
									<td colspan="2" class="uga" id="tdSoftRoll" runat="server">
									  <asp:RadioButton id="rdOriginal" runat="server" GroupName="rbRoll" Text="Original" Checked="true"></asp:RadioButton>
									  <asp:RadioButton id="rdSoftRoll" runat="server" GroupName="rbRoll" Text="Soft roll"></asp:RadioButton>
									</td>
								</tr>
							</asp:Panel>
							<asp:Panel ID="pnlMasterPublishing" Runat="server">
								<tr>
									<td class="editLabelCell" width="200">
										<asp:label id="lbMasterPublishing" runat="server">Select the check box to also publish the product</asp:label></td>
									<td class="uga" id="tdMasterPublishing" runat="server">
										<asp:checkbox id="cbMasterPublishing" runat="server"></asp:checkbox></td>
									<td class="uga" align="right" id="tdCalendar" runat="server">
										<div id="divCalendar">
											<igsch:webdatechooser id="wdMasterPublishing" runat="server" Width="90px" NullDateLabel=" ">
												<EditStyle Font-Size="8pt"></EditStyle>
												<CalendarLayout FooterFormat="Today: {0:d}" MaxDate="" ShowTitle="False" ShowFooter="False"></CalendarLayout>
											</igsch:webdatechooser>
										</div>
									</td>
								</tr>
							</asp:Panel>
							<asp:Panel ID="pnlForceTranslationToDraft" Runat="server">
								<tr>
									<td class="editLabelCell" width="200">
										<asp:label id="lbForceTranslation" runat="server">Force translation to draft</asp:label></td>
									<td colspan="2" class="ugd" id="tdForceTranslation" runat="server">
										<asp:checkbox id="cbForceTranslation" runat="server"></asp:checkbox></td>
								</tr>
							</asp:Panel>
							<asp:Panel ID="pnlReport" Runat="server">
								<tr>
									<td class="editLabelCell" width="200">
										<asp:label id="lbReport" runat="server">Select the check box to export the report in excel</asp:label></td>
									<td colspan="2" class="ugd" id="tdReport" runat="server">
										<asp:checkbox id="cbReport" runat="server"></asp:checkbox></td>
								</tr>
							</asp:Panel>
						</table>
					</td>
				</tr>
				<tr>
				    <td>
				        <asp:Label ID="lblChkBoxAlert" runat="server" EnableViewState="false" Visible="false"></asp:Label>
				    </td>
				</tr>
			</table>
			  </asp:Panel>
		</form>
	</body>
</html>
