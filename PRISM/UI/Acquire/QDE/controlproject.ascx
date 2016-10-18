<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Control Language="c#" Inherits="HyperCatalog.UI.Acquire.QDE.controlProject" CodeFile="controlProject.ascx.cs"%>


<script type="text/javascript" language="javascript">
    //Added by Prabhu for ACQ 3.0 (PCF1: Regional Project Management) -- 19/May/09
    function BOTDateCheck(oDateChooser, newValue, oEvent)
    {
        var d1_str = document.getElementById('<%= wdBot.ClientID %>').value; 
        var d2_str = document.getElementById('<%= wdEov.ClientID %>').value; 
        
        var dt1   = parseInt(d1_str.split('-')[2]); 
        var mon1  = parseInt(d1_str.split('-')[1]);
        var yr1   = parseInt(d1_str.split('-')[0]); 
        var dt2   = parseInt(d2_str.split('-')[2]);
        var mon2  = parseInt(d2_str.split('-')[1]);
        var yr2   = parseInt(d2_str.split('-')[0]);
        var BOTDate = new Date(yr1, mon1, dt1); 
        var EORDate = new Date(yr2, mon2, dt2);
        
        if (BOTDate < EORDate) 
        {
            alert('Your are setting BOT date less than EOR date!!!');
        }
    } 
    
    function ValidateEOR(source, args) 
    {
        var BORDate = document.getElementById('<%= wdEoa.ClientID %>').value; 
        var EORDate = document.getElementById('<%= wdEov.ClientID %>').value; 

        if ((EORDate != null || EORDate != "") && (BORDate == null || BORDate == ""))
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    function ValidateBOT(source, args) 
    {
        var EORDate = document.getElementById('<%= wdEov.ClientID %>').value; 
        var BOTDate = document.getElementById('<%= wdBot.ClientID %>').value; 

        if ((BOTDate != null || BOTDate != "") && (EORDate == null || EORDate == ""))
            args.IsValid = false;
        else
            args.IsValid = true;
    }
</script>
<asp:linkbutton id="lnkOverride" CausesValidation="False" Visible="False" runat="server" onclick="lnkOverride_Click">
	<br/>
	<center><b>[Override and Create a new Project]</b></center><br/>
		<br/>
</asp:linkbutton>
<table cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr valign="top">
		<td style="WIDTH:30%">
			<table cellspacing="0" cellpadding="1" width="100%" border="0">
				<tr valign="top">
					<td class="SectionSubSubTitle" style="WIDTH:30%" colspan="3"><b>Dates</b></td>
				</tr>
				<asp:Panel ID="panelBOP" Runat="server">
					<tr valign="top">
						<td class="ptb1" valign="top"  nowrap="nowrap" style="width:50%">Beginning of Project (BOP)</td>
						<td class="ptb3" align="center" style="width:25%">
							<igsch:webdatechooser id="wdBop" runat="server" Enabled="false" Width="100px" ReadOnly="True">
								<EditStyle Font-Size="8pt"></EditStyle>
								<CalendarLayout FooterFormat="Today: {0:d}" MaxDate="" ShowTitle="False" ShowFooter="False"></CalendarLayout>
							</igsch:webdatechooser></td>						
					</tr>
				</asp:Panel>
				<asp:Panel ID="panelEOA" Runat="server">
					<tr valign="top">
						<td class="ptb1" valign="top" nowrap="nowrap" style="width:50%">Beginning of Regionalization (BOR)</td>
						<td class="ptb3" align="center" style="width:135px">
							<igsch:webdatechooser id="wdEoa" runat="server" Width="100px" NullDateLabel=" ">
								<EditStyle Font-Size="8pt"></EditStyle>
								<CalendarLayout FooterFormat="Today: {0:d}" MaxDate="" ShowTitle="False" ShowFooter="False"></CalendarLayout>
							</igsch:webdatechooser></td>
					</tr>
				</asp:Panel>
				<asp:Panel ID="panelEOV" Runat="server">
					<tr valign="top">
						<td class="ptb1" nowrap="nowrap" style="width:50%">End of Regionalization (EOR)</td>
						<td class="ptb3" align="center" style="width:135px">
							<igsch:webdatechooser id="wdEov" runat="server" Width="100px" NullDateLabel=" ">
								<EditStyle Font-Size="8pt"></EditStyle>
								<CalendarLayout FooterFormat="Today: {0:d}" MaxDate="" ShowTitle="False" ShowFooter="False"></CalendarLayout>
							</igsch:webdatechooser></td>
							
					</tr>
				</asp:Panel>
				<asp:Panel ID="panelBOT" Runat="server">
					<tr valign="top">
						<td class="ptb1" valign="top" nowrap="nowrap" style="width:50%">Beginning of Translation (BOT)</td>
						<td class="ptb3" align="center" style="width:135px">
							<igsch:webdatechooser id="wdBot" runat="server" Width="100px" NullDateLabel=" ">
							<ClientSideEvents ValueChanged="BOTDateCheck" />
								<EditStyle Font-Size="8pt"></EditStyle>
								<CalendarLayout FooterFormat="Today: {0:d}" MaxDate="" ShowTitle="False" ShowFooter="False"></CalendarLayout>
							</igsch:webdatechooser></td>
					</tr>
				</asp:Panel>
				<asp:Panel ID="panelEOT" Runat="server" Visible="False">
					<tr valign="top">
						<td class="ptb1" nowrap="nowrap" style="width:50%">End of Translation (EOT)</td>
						<td class="ptb3" align="center" style="width:135px">
								<igsch:webdatechooser id="wdEot" Enabled="false" runat="server" Width="100px" NullDateLabel=" ">
								<EditStyle Font-Size="8pt"></EditStyle>
								<CalendarLayout FooterFormat="Today: {0:d}" MaxDate="" ShowTitle="False" ShowFooter="False"></CalendarLayout>
							</igsch:webdatechooser></td>							
					</tr>
				</asp:Panel>
			</table>
			<asp:comparevalidator id="cvEoa" runat="server" Display="None" ControlToValidate="wdEoa" ErrorMessage="Beginning of Regionalization must be after the Project Creation date" CssClass="hc_error" ForeColor=" " Type="Date" ControlToCompare="wdBop" Operator="GreaterThanEqual" Enabled="false"></asp:comparevalidator>      
			<asp:comparevalidator id="cvEov" runat="server" Display="None" ControlToValidate="wdEov" ErrorMessage="EOR must be after BOR" CssClass="hc_error" ForeColor=" " Type="Date" ControlToCompare="wdEoa" Operator="GreaterThanEqual" Enabled="false"></asp:comparevalidator>															
			<asp:comparevalidator id="cvBot" runat="server" Display="None" ControlToValidate="wdBot" ErrorMessage="BOT must be after BOR" CssClass="hc_error" ForeColor=" " Type="Date" ControlToCompare="wdEoa" Operator="GreaterThanEqual" Enabled="false"></asp:comparevalidator>
			<asp:CustomValidator id="valEov" runat="server" Display="None" ControlToValidate="wdEov" ErrorMessage ="BOR date cannot be NULL" CssClass="hc_error" ForeColor=" " ClientValidationFunction="ValidateEOR" Enabled="true"></asp:CustomValidator>
			<asp:CustomValidator id="valBot" runat="server" Display="None" ControlToValidate="wdBot" ErrorMessage ="EOR date cannot be NULL" CssClass="hc_error" ForeColor=" " ClientValidationFunction="ValidateBOT" Enabled="true"></asp:CustomValidator>
			<asp:ValidationSummary id="pVal" runat="server" Font-Bold="true" DisplayMode="List"/>
		</td>
		<td style="WIDTH:70%">
			<table cellspacing="0" cellpadding="1" width="100%" border="0">
				<tr valign="top">
					<asp:Panel ID="panelScopeTitle" Runat="server">
						<td class="SectionSubSubTitle"><b>List of predefined scopes</b></td>
					</asp:Panel>
				</tr>
				<asp:Panel ID="panelScopeLanguages" Runat="server">
					<tr>
						<td class="ptb3" style="WIDTH: 100%" valign="top" rowspan="10"><!-- LANGUAGE SCOPE -->
							<table class="ptb2" cellspacing="1" cellpadding="0" width="100%" border="0">
								<tr valign="middle">
									<td style="WIDTH: 235px">
										<asp:dropdownlist id="ddlScopes" runat="server" Width="232px" DataTextField="Name" DataValueField="Id"></asp:dropdownlist></td>
									<td style="WIDTH: 100%;" align="left">
										<igtxt:webimagebutton id="wBtnAdd" runat="server" CausesValidation="False" Text="" OnClick="wBtnAdd_Click">
											<Alignments VerticalImage="Middle"></Alignments>
											<PressedAppearance>
												<Style CssClass="hc_toolbarselected">
												</Style>
											</PressedAppearance>
											<HoverAppearance>
												<Style CssClass="hc_toolbarhover">
												</Style>
											</HoverAppearance>
											<Appearance>
												<Style BorderWidth="0px" CssClass="hc_toolbardefault">
												</Style>
												<Image Url="/hc_v4/img/ed_new.gif" AlternateText="Add the languages defined in this scope to the current selection"></Image>
											</Appearance>
										</igtxt:webimagebutton>&nbsp;
										<igtxt:webimagebutton id="wBtnReplace" runat="server" CausesValidation="False" Text="" OnClick="wBtnReplace_Click">
											<Alignments VerticalImage="Middle"></Alignments>
											<PressedAppearance>
												<Style CssClass="hc_toolbarselected">
												</Style>
											</PressedAppearance>
											<HoverAppearance>
												<Style CssClass="hc_toolbarhover">
												</Style>
											</HoverAppearance>
											<Appearance>
												<Style BorderWidth="0px" CssClass="hc_toolbardefault">
												</Style>
												<Image Url="/hc_v4/img/ed_refresh.gif" AlternateText="Replace the current selection with the languages defined in this scope"></Image>
											</Appearance>
										</igtxt:webimagebutton></td>
								</tr>
							</table>
							<asp:checkboxlist id="cblLanguageScope" runat="server" Width="100%" DataTextField="Name" DataValueField="Code"
								RepeatColumns="3" cellspacing="0" cellpadding="0"></asp:checkboxlist></td>
					</tr>
				</asp:Panel>
			</table>
		</td>
	</tr>
</table>
