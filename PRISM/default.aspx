<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="HyperCatalog.UI.Login._default" Title="Home Page"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Home page</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<script type="text/javascript" language="javascript">
	<!--	
		function uwtHomePage_Click(oToolbar, oButton, oEvent)
		{
			if (oButton.Key=="btnPageProperties")
				var win=open('HomePageProperties.aspx', 'Properties', 'width=300, height=300');
		}
	
		function show_hide_module(dataId, img)
		{
			var data=document.getElementById(dataId);
			if (data!=null)
			{
				if (data.style.display=='none')
				{
					data.style.visibility='visible';
					data.style.display='block';
					
					img.src="/hc_v4/img/homePage/button_less.gif";
				}
				else
				{
					data.style.visibility='hidden';
					data.style.display='none';
					
					img.src="/hc_v4/img/homePage/button_more.gif";
				}
			}
		}
	-->
	</script>
	  
	<table cellspacing="0" cellpadding="0" width="100%" border="0">
		<tr>
			<td>
				<table cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td>
							<igtbar:ultrawebtoolbar id="uwtHomePage" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px"
								Height="24px">
								<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
								<ClientSideEvents Click="uwtHomePage_Click"></ClientSideEvents>
								<Items>
									<igtbar:TBarButton Key="btnPageProperties" ToolTip="Home page properties" Text="&lt;b&gt;Properties&lt;/b&gt;"
										Image="/hc_v4/img/homePage/properties.gif">
										<DefaultStyle Width="90px"></DefaultStyle>
									</igtbar:TBarButton>
								</Items>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							</igtbar:ultrawebtoolbar>
						</td>
					</tr>
					<tr style="height: 5px"><td></td></tr>
					<tr style="height: auto">
						<td>
							<table cellpadding="0" cellspacing="5" border="0" style="width: 100%; height: 100%">
								<tr valign="top">
								  <td style="width: 170px" ID="leftPane" runat="server" Visible="false">
								  </td>
								  <td style="width: auto" ID="contentPane" runat="server" Visible="false">
								  </td>
								  <td style="width: 230px" ID="rightPane" runat="server" Visible="false">
								  </td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>

