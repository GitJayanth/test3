<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="jobscheduleedit.aspx.cs" Inherits="UI_Admin_jobstatus_jobscheduleedit" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" language="javascript">
  
		  function uwToolbar_Click(oToolbar, oButton, oEvent)
		  {
		    if (oButton.Key == 'List') 
		    {
                back();
                oEvent.cancelPostBack = true;
            }
            
//            if (oButton.Key == 'Save') 
//		    {
//		        var jt = document.getElementById('txtJobType');
//		        var jtValue= trim(jt)
//		        if (jtValue == 'Inbound' || jtValue == 'Operational' || jtValue == 'Outbound')
//		        {
//                
//                }
//                else
//                {
//                    jt.focus()
//                }
//            }
		  } 
		  
//		function trim(s)
//        {return rtrim(ltrim(s));}

//        function ltrim(s)
//        {
//	        var l=0;
//	        while(l < s.length && s[l] == ' ')
//	        {	l++; }
//	        return s.substring(l, s.length);
//        }

//        function rtrim(s)
//        {
//	        var r=s.length -1;
//	        while(r > 0 && s[r] == ' ')
//	        {	r-=1;	}
//	        return s.substring(0, r+1);
//        }
//        
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
				<tr valign="bottom" height="*">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
                                <igtbar:TBSeparator key="AddSep"></igtbar:TBSeparator>
                            <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
                            </Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>

				<tr valign="top">
				<asp:label id="lblMsg" runat="server" Visible="false" >Data Saved Successfully</asp:label>
					<td>
						<table cellspacing="0" cellpadding="0" border="0" width="100%">
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="Label1" runat="server">AppComponent ID</asp:label></td>
							<td class="uga">
							<asp:TextBox id="txtAppComponentId" runat="server" width="40%"></asp:TextBox>
							</td>
							<td class="ugd">
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbJobName" runat="server">Job Name</asp:label></td>
							<td class="uga">
							<asp:TextBox id="txtJobName" runat="server" width="40%"></asp:TextBox>
							</td>
							<td class="uga">
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbJobType" runat="server">Job Type</asp:label></td>
							<td class="uga">
                                <asp:DropDownList ID="cmbJobType" runat="server">
                                </asp:DropDownList>
							<!--<asp:TextBox id="txtJobType" runat="server" width="40%"></asp:TextBox>
							<asp:label id="lblJobTypeFormat" runat="server">(Inbound/Operational/Outbound)</asp:label>-->
							</td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbScheduleDay" runat="server">Scheduled Day</asp:label></td>
							<td class="uga">
							 <asp:DropDownList ID="cmbScheduledDay" runat="server">
                                </asp:DropDownList>
							<!--<asp:TextBox id="txtScheduleDay" runat="server" width="40%"></asp:TextBox>
							
							<asp:label id="lbScheduleDayFormat" runat="server">(Daily/Sun/Mon/Tue/Wed/Thu/Fri/Sat)</asp:label>--></td>
								</td>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbScheduleTime" runat="server">Scheduled Time (GMT) </asp:label></td>
							<td class="uga">
							<asp:TextBox id="txtScheduleTime" runat="server" width="40%"></asp:TextBox>
							<asp:label id="lbScheduleTimeFormat" runat="server">(HH:MM)</asp:label>
								</td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbEstimatedTime" runat="server">Estimated Time</asp:label>
							</td>
							<td class="uga">
							<asp:TextBox id="txtEstimatedTime" runat="server" width="40%"></asp:TextBox>
							<asp:label id="Label2" runat="server">(Job Duration in minute(s))</asp:label>
							</td>
						</tr>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbCutOffTime" runat="server">Cut Off Time (GMT) </asp:label></td>
							<td class="uga">
							<asp:TextBox id="txtCutOffTime" runat="server" width="40%"></asp:TextBox>
							<asp:label id="lbCutOffTimeFormat" runat="server">(HH:MM)</asp:label>
								</td>
						<tr valign="middle">
							<td class="editLabelCell" width="120">
								<asp:label id="lbIsActive" runat="server">IsActive</asp:label></td>
							<td class="uga">
							<asp:CheckBox ID="chIsActive" runat="server" />
								</td>
						</tr>
						</table>
					</td>
				</tr>
</asp:Content>