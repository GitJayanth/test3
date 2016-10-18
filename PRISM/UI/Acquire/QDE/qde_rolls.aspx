<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_Rolls" CodeFile="QDE_Rolls.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Create roll</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
				if (oButton.Key == 'Cancel') 
        {
					oEvent.cancelPostBack = true;
          window.close();
        }
        else if (oButton.Key == 'Apply')
        {
	  			oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		  }
		  
		  function UpdateContentCreation(itemId)
		  {
		  	if ((opener) && (opener.parent))
		  	{
		  		opener.parent.location = 'QDE_Main.aspx?i='+itemId;
				}
				window.close();
		  }
		  
		  function UpdateContentModification(itemId)
		  {
		    if (opener)
		  	{
		  	  opener.document.location = 'QDE_itemabout.aspx?i='+itemId;
				}
				window.close();
		  }
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" height="25">
					<td>
						<asp:label id="lbTitle" Runat="server" Width="100%" Font-Size="9pt" Font-Bold="True" CssClass="hc_toolbartitle">ItemName</asp:label>
					</td>
				</tr>
				<tr valign="top" height="25">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" width="100%" ItemWidthDefault="80px">
							<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
							<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
							<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
							<CLIENTSIDEEVENTS InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></CLIENTSIDEEVENTS>
							<ITEMS>
								<IGTBAR:TBARBUTTON Text="Apply" Key="Apply" Image="/hc_v4/img/ed_savefinal.gif" ToolTip="Create roll"></IGTBAR:TBARBUTTON>
								<IGTBAR:TBSEPARATOR Key="ApplySep"></IGTBAR:TBSEPARATOR>
								<IGTBAR:TBARBUTTON Text="Cancel" Key="Cancel" Image="/hc_v4/img/ed_cancel.gif" ToolTip="Close window"></IGTBAR:TBARBUTTON>
							</ITEMS>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td><asp:label id="lbError" runat="server" CssClass="hc_error">lbError</asp:label></td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<TR>
								<td>
									<table cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr valign="top">
											<td class="editLabelCell" width="120">
												<asp:label id="lbReplacementDate" runat="server" Font-Bold="True"></asp:label></td>
											<td class="ugd">
												<igsch:webdatechooser id="wdcReplacementDate" runat="server" Width="150px" NullDateLabel="">
													<EditStyle Width="150px" Font-Size="xx-small"></EditStyle>
													<CalendarLayout ShowTitle="False" ShowFooter="False"></CalendarLayout>
												</igsch:webdatechooser>
											</td>
										</tr>
										<tr valign="top">
											<td class="editLabelCell" width="120">
												<asp:Label id="lbComment" runat="server" Font-Bold="True"></asp:Label></td>
											<td class="uga" colspan="2">
												<asp:TextBox id="txtComment" runat="server" Width="290px" TextMode="MultiLine" Rows="5"></asp:TextBox></td>
												<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Comment field should not be empty" ControlToValidate="txtComment"></asp:RequiredFieldValidator>
										</tr>
									</table>
								</td>
							</tr>
							<tr valign="top">
								<td>
									<asp:HyperLink id="hlCreator" runat="server" Visible="False" Font-Italic="True"></asp:HyperLink></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:Content>