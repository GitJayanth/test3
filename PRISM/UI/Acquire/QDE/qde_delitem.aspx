<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_DelItem" CodeFile="QDE_DelItem.aspx.cs" Async="True"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Delete item</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
	<script>
    var isProcessing = false;
    function ReloadAndClose(){
      if (opener){
        opener.location = '/hc_v4/dummy.htm';
        opener.parent.framecontent.location = '/hc_v4/dummy.htm';
        opener.location = 'QDE_TV.aspx';
        opener.parent.framecontent.location = 'QDE_FormRoll.aspx';
      }
      window.close();
    }
	  ///////////////////////////////////////////////////////
	  function uwToolbar_Click(oToolbar, oButton, oEvent){
	  ///////////////////////////////////////////////////////
      if (isProcessing) 
      {
        alert('Please wait...');
        oEvent.cancelPostBack = true;
      }
      else
      {
        if (oButton.Key == 'Apply') 
        {
          var td = document.getElementById("tdDeletedItem");
          if (td)
          {
            td.style.visibility = "visible";
            td.style.display = "block";
          }
          isProcessing = true;
        }
        if (oButton.Key == 'Cancel') 
        {
          oEvent.cancelPostBack = true;
          window.close();
        }
      }
    }
	</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
	<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbartitle">
		<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
		<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
		<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
		<ClientSideEvents></ClientSideEvents>
		<Items>
			<igtbar:TBLabel Text="ItemName" Key="ItemName">
				<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
			</igtbar:TBLabel>
		</Items>
	</igtbar:ultrawebtoolbar><igtbar:ultrawebtoolbar id="Ultrawebtoolbar1" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
		<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
		<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
		<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
		<ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
		<Items>
			<igtbar:TBarButton Key="Apply" ToolTip="Delete Item" Text="Apply" Image="/hc_v4/img/ed_savefinal.gif"></igtbar:TBarButton>
			<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
			<igtbar:TBarButton Key="Cancel" ToolTip="Close window" Text="Cancel" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
		</Items>
	</igtbar:ultrawebtoolbar>
	<table class="datatable" cellspacing="0" cellpadding="0" width="100%" border="0">
		<tr>
			<td align="center">
				<table border="0">
					<tr>
						<td>
							<center><b>[WARNING]</b></center>
							<br/>
							Are you sure you want to delete this node?
							<ul>
								<asp:Repeater id="rConsequences" runat="server">
									<ItemTemplate>
										<li>
											<%# DataBinder.Eval(Container.DataItem, "Label") %>
										</li>
									</ItemTemplate>
								</asp:Repeater>
							</ul>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<center>
	  <asp:label id="lbError" runat="server" CssClass="hc_error">Result message</asp:label>
	</center>
</asp:Content>