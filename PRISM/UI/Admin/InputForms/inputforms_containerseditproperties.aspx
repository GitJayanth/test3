<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="InputForms_ContainersEditProperties" CodeFile="InputForms_ContainersEditProperties.aspx.cs" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ajax" TagName="Lookup" Src="~/ajaxcontrols/LookUp.ascx"%>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Container edit properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
		<script type="text/javascript" src="../../../ajaxcontrols/LookupContainer.js"></script>
			<style type="text/css">
				.ld1 { }
				.ld2 { COLOR: green; TEXT-ALIGN: right }
				</style>
			<script type="text/javascript" language="javascript">		
		  function ReloadParent()
		  {
		    top.opener.document.getElementById("action").value = "reload";
        top.opener.document.forms[0].submit();
		  }

		  
		  function DisplayTab(ifcId, ifId, IsVisible)
		  {
				var webTab = parent.curTab;
				// Display possible values tab
  			var tabPoss = webTab.Tabs['PossibleValues'];  			
  			if (tabPoss != null)
  			{
  			  try
  			  {
			      tabPoss.setVisible(IsVisible == 1);
  			    if (IsVisible == 1)
  			    {
			        tabPoss.setTargetUrl('InputForms_ContainersEditValues.aspx?c='+ifcId+'&f='+ifId);
			      }
			    }
			    catch (ex)
			    {;
			    }
			  }
				var tabProp = webTab.Tabs['Properties'];
				tabProp.setTargetUrl('InputForms_ContainersEditProperties.aspx?c='+ifcId+'&f='+ifId);
		    if (ifcId == -1)
		    {
					document.forms[0].submit();
		    }		    				
		  }

		  
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
		    if (oButton.Key == 'Save') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
        }
		    if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
        if (oButton.Key == 'Close'){
					parent.close();
					oEvent.cancelPostBack = true;
        }
			}
	
			</script>
</asp:Content>
<%--Removed the width property to fix enlarge button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellpadding="0" cellspacing="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<br/>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr valign="middle">
								<td class="editLabelCell" width="80"><asp:label id="Label2" runat="server">Value Type</asp:label></td>
								<td class="uga"><asp:dropdownlist id="DDL_InputType" runat="server"></asp:dropdownlist></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="80" height="19"><asp:label id="Label8" runat="server">Container tag</asp:label></td>
								<td class="ugd" height="19">
									<ajax:Lookup ID="cbContainers" runat="server" lookupservice="LookupContainer" pageproperty="Tag"
										nosubmit="true" Width="400" />
									<asp:TextBox id="txtContainer" runat="server" Width="400" Visible="False" Enabled="False"></asp:TextBox>
								</td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="80"><asp:label id="Label3" runat="server">Comment</asp:label></td>
								<td class="uga"><asp:textbox id="txtComment" runat="server" Width="296px" MaxLength="1000"></asp:textbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="80"><asp:label id="Label4" runat="server">Mandatory</asp:label></td>
								<td class="ugd"><asp:checkbox id="chkMandatory" runat="server" Visible="true"></asp:checkbox></td>
							</tr>
							<tr valign="middle">
								<td class="editLabelCell" width="80"><asp:label id="Label1" runat="server">Regionalizable</asp:label></td>
								<td class="ugd"><asp:checkbox id="cbRegionalizable" runat="server" Visible="true" Enabled="false"></asp:checkbox></td>
							</tr>
						</table>
						<asp:HyperLink id="hlCreator" runat="server">HyperLink</asp:HyperLink>
						<asp:Label ID="lbOpening" Runat="server">&nbsp; [</asp:Label>
						<asp:label id="lbOrganization" runat="server">Organization</asp:label>
						<asp:Label ID="lbClosing" Runat="server">] -</asp:Label>
						<asp:Label id="lbCreatedOn" Runat="server"></asp:Label>
					</td>
				</tr>
			</table>
</asp:Content>