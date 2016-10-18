<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_CloneItem" CodeFile="QDE_CloneItem.aspx.cs" Async="True"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Clone item</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
			function uwToolbar_Click(oToolbar, oButton, oEvent){
				if (oButton.Key == 'Clone') {
					oEvent.cancelPostBack = MandatoryFieldMissing();
				}
				else if (oButton.Key == 'Cancel') {
				  oEvent.cancelPostBack = true
					window.close();
				}
			} 
			
			function ReloadAndClose()
      { 
        if (opener)
          opener.ReloadFrames(window)
        else
          window.close();    
      }
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="main" cellspacing="0" cellpadding="0">
					<tr valign="top" style="height: 25px">
						<td><asp:label id="lbTitle" CssClass="hc_toolbartitle" Font-Bold="True" Font-Size="9pt" Width="100%"
								Runat="server">ItemName</asp:label></td>
					</tr>
					<tr valign="top" style="height:1px">
						<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="uwToolbar_ButtonClicked">
								<HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
								<DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
								<SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
								<CLIENTSIDEEVENTS InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></CLIENTSIDEEVENTS>
								<ITEMS>
									<IGTBAR:TBARBUTTON Text="Clone" Key="Clone" Image="/hc_v4/img/ed_savefinal.gif" ToolTip="Clone item"></IGTBAR:TBARBUTTON>
									<IGTBAR:TBSEPARATOR Key="CloneSep"></IGTBAR:TBSEPARATOR>
									<IGTBAR:TBARBUTTON Text="Close" Key="Cancel" Image="/hc_v4/img/ed_cancel.gif" ToolTip="Close window"></IGTBAR:TBARBUTTON>
								</ITEMS>
							</igtbar:ultrawebtoolbar></td>
					</tr>
					<tr valign="top" style="height:1px">
						<td><asp:label id="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:label></td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<table cellspacing="0" cellpadding="0" width="100%" border="0">
								<tr>
									<td class="editLabelCell" width="120"><asp:label id="lbCloneCount" runat="server" Font-Bold="True"></asp:label></td>
									<td class="ugd"><igtxt:webnumericedit id="wneCloneCount" runat="server" MinValue="1" DataMode="Int" ValueText="1"></igtxt:webnumericedit><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="errorMessage" ControlToValidate="wneCloneCount"
											ErrorMessage="*"></asp:requiredfieldvalidator></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr valign="top" style="height: 20px" bgcolor="white">
						<td><asp:label id="lbParents" runat="server" Font-Bold="True"></asp:label></td>
					</tr>
					<tr valign="top">
						<td><asp:repeater id="rPossibleParents" runat="server" OnItemDataBound="rPossibleParents_ItemDataBound" >
								<HeaderTemplate>
									<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
										<table width="100%" cellpadding="0" cellspacing="0" border="0" style="OVERFLOW: auto">
								</HeaderTemplate>
								<ItemTemplate>
									<tr style="height: 20px">
										<td width="20px"><asp:Label ID="lbRabioButton" Runat="server" Visible="False"></asp:Label>
										<td>
										<td><asp:Label ID="lbParent" Runat="server"></asp:Label></td>
									</tr>
								</ItemTemplate>
								<FooterTemplate>
			          </table>
			        </div>    
			          </FooterTemplate>
			        </asp:repeater>
			      </td>
			    </tr>
			  </table>
</asp:Content>