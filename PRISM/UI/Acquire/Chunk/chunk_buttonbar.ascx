<%@ Control Language="c#" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_ButtonBar" CodeFile="Chunk_ButtonBar.ascx.cs" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<script type="text/javascript"><!--
    var wngMsg = 'Warning, this chunk is currently in translation!\n          It will need to be re-translated!\n                        Continue?';
		function wBtDelete_Click(oButton, oEvent){
	    oEvent.cancel = !confirm('are you sure you want to delete this chunk ?');
		}
		function wBtFinal_Click(oButton, oEvent){
		if (typeof(respectMaxLen)!="undefined" && !respectMaxLen)
 	    {
	      oEvent.cancel = true;
	      alert('Chunk is over the length limit!');
 	    }
 	    else
 	    {
 	        //code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix - start
 	        if(!Page_ValidateHTMLTags())
		    {
		      oEvent.cancel=true;
		      return;
		    }
		    //code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix - end
		  
		    if (!isInTR || confirm(wngMsg)){
		      finalButton = oButton;
	        Page_CheckSpelling();
	        genericEvent = oEvent;
	        oEvent.cancel = true;
	      }
	      else{
	        oEvent.cancel = true;
	      }
	    }
		}
		function wBtDraft_Click(oButton, oEvent){
    	//code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix - start
    	if(!Page_ValidateHTMLTags())
		  oEvent.cancel=true;
		//code added on 17th November 2011 for Chunk Edit Validation for XSS Vulnerability Fix - end  
		
 	    if (typeof(respectMaxLen)!="undefined" && !respectMaxLen)
 	    {
	      oEvent.cancel = true;
	      alert('Chunk is over the length limit!');
 	    }
 	    if (isInTR && !confirm(wngMsg)){
 	      oEvent.cancel = true;
 	    }
		}
		if (typeof(top.ChunkUnchanged) == 'function'){
		  top.ChunkUnchanged();
		}
		
		//code added on 24th Nov 2011 for Chunk Edit Validation (XSS Vulnerability Fix) for Country Catalog - start
		function wBtFinal_Click1(oButton, oEvent)
		{
		   if(!Page_ValidateHTMLTags())
		      oEvent.cancel=true;
		}
		//code added on 24th Nov 2011 for Chunk Edit Validation (XSS Vulnerability Fix) for Country Catalog - end
		
--></script>
<table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
	<tbody>
		<tr valign="bottom" style="HEIGHT:26px" class="hc_toolbar">
			<td colspan="2">
				<table border="0">
					<tr>
						<td>&nbsp;<asp:Label id="lReadOnly" runat="server" Visible="False" height="18" CssClass="hc_error">Read Only attribute</asp:Label>					
						<asp:Label id="lNonRegionalizable" runat="server" Visible="False" height="18" CssClass="hc_error">Non regionalizable attribute</asp:Label>
						<asp:Label id="lNonEditableForCountry" runat="server" Visible="False" height="18" CssClass="hc_error">This country cannot localize content</asp:Label>
						</td>
						
								<%--Alternate for CR 5096(Removal of rejection funtionality)
								<td><igtxt:webimagebutton id="wbtReject" CausesValidation="false" runat="server" Text="Reject" Height="16px" Width="57px" OnClick="wbtReject_Click">
								<Alignments VerticalImage="Middle"></Alignments>
								<PressedAppearance>
									<Style CssClass="hc_toolbarselected"></Style>
								</PressedAppearance>
								<HoverAppearance>
									<Style CssClass="hc_toolbarhover"></Style>
								</HoverAppearance>
								<Appearance>
									<Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
									<Image Url="/hc_v4/img/ed_reject.gif" AlternateText="Reject Master value"></Image>
								</Appearance>
							</igtxt:webimagebutton>
						</td>
						<asp:Panel ID="panelSepReject" Visible="True" Runat="server">
							<td><span style="MARGIN-LEFT: 4px; WIDTH: 1px; BACKGROUND-COLOR: #848284"></span><span style="WIDTH: 1px; BACKGROUND-COLOR: #ffffff"></span></td>
						</asp:Panel>--%>
								<td><igtxt:webimagebutton id="wbtCopy" CausesValidation="false" runat="server" Text="Copy Master" Height="16px" Width="95px" OnClick="wbtCopy_Click">
								<Alignments VerticalImage="Middle"></Alignments>
								<PressedAppearance>
									<Style CssClass="hc_toolbarselected"></Style>
								</PressedAppearance>
								<HoverAppearance>
									<Style CssClass="hc_toolbarhover"></Style>
								</HoverAppearance>
								<Appearance>
									<Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
									<Image Url="/hc_v4/img/ed_copy.gif" AlternateText="Copy Master value"></Image>
								</Appearance>
							</igtxt:webimagebutton>
						</td>
						<asp:Panel ID="panelSepCopy" Visible="True" Runat="server">
							<td><span style="MARGIN-LEFT: 4px; WIDTH: 1px; BACKGROUND-COLOR: #848284"></span><span style="WIDTH: 1px; BACKGROUND-COLOR: #ffffff"></span></td>
						</asp:Panel>
						<td>
							<igtxt:webimagebutton id="wBtDraft" runat="server" Text="Draft" Height="16px" Width="57px">
								<Alignments VerticalImage="Middle"></Alignments>
								<PressedAppearance>
									<Style CssClass="hc_toolbarselected"></Style>
								</PressedAppearance>
								<HoverAppearance>
									<Style CssClass="hc_toolbarhover"></Style>
								</HoverAppearance>
								<Appearance>
									<Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
									<Image Url="/hc_v4/img/ed_savedraft.gif" AlternateText="Save as Draft"></Image>
								</Appearance>
							</igtxt:webimagebutton>
						</td>
						<td><igtxt:webimagebutton id="wBtFinal" runat="server" Text="Final" Height="16px" Width="57px">
								<Alignments VerticalImage="Middle"></Alignments>
								<PressedAppearance>
									<Style CssClass="hc_toolbarselected"></Style>
								</PressedAppearance>
								<HoverAppearance>
									<Style CssClass="hc_toolbarhover"></Style>
								</HoverAppearance>
								<Appearance>
									<Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
									<Image Url="/hc_v4/img/ed_saveFinal.gif" AlternateText="Save as Final"></Image>
								</Appearance>
							</igtxt:webimagebutton>
						</td>
						<td><igtxt:webimagebutton id="wBtDelete" runat="server" Text="Delete" Height="16px" CausesValidation="False"
								Width="57px">
								<Alignments VerticalImage="Middle"></Alignments>
								<ClientSideEvents Click="wBtDelete_Click"></ClientSideEvents>
								<PressedAppearance>
									<Style CssClass="hc_toolbarselected">
									</Style>
								</PressedAppearance>
								<HoverAppearance>
									<Style CssClass="hc_toolbarhover">
									</Style>
								</HoverAppearance>
								<Appearance>
									<Style CssClass="hc_toolbardefault">
									</Style>
									<Image Url="/hc_v4/img/ed_Delete.gif" AlternateText="Delete chunk"></Image>
								</Appearance>
							</igtxt:webimagebutton>
						</td>

						<asp:Panel ID="PanelSep" Visible="True" Runat="server">
							<td><span style="MARGIN-LEFT: 4px; WIDTH: 1px; BACKGROUND-COLOR: #848284"></span><span style="WIDTH: 1px; BACKGROUND-COLOR: #ffffff"></span></td>
						</asp:Panel>
						<td>
							<asp:CheckBox id="cbLockTranslations" runat="server" Text="Lock translations"></asp:CheckBox>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</tbody>
</table>
<asp:Label ID="txtInTranslation" Visible="false" CssClass="hc_error" runat="server">Currently under translations</asp:Label>