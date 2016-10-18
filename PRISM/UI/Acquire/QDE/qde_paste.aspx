<%@ Reference Page="~/ui/acquire/qde.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_Paste" CodeFile="QDE_Paste.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Paste content</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
	<script type="text/javascript" language="javascript">
      function ReloadAndClose(){
	    	opener.document.getElementById("action").value = "reload";
        opener.document.forms[0].submit();
        window.close();
      }
		  ///////////////////////////////////////////////////////
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		  ///////////////////////////////////////////////////////
        if (oButton.Key == 'Cancel') {
          oEvent.cancelPostBack = true;
          window.close();
        }
		  }
	 function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }

			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbartitle" ItemWidthDefault="80px"
				width="100%">
				<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				<ClientSideEvents></ClientSideEvents>
				<Items>
					<igtbar:TBLabel Text="ItemName" Key="ItemName">
						<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
					</igtbar:TBLabel>
				</Items>
			</igtbar:ultrawebtoolbar><igtbar:ultrawebtoolbar id="Ultrawebtoolbar1" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px">
				<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				<ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></ClientSideEvents>
				<Items>
					<igtbar:TBarButton Key="Apply" ToolTip="Add Item" Text="Apply" Image="/hc_v4/img/ed_savefinal.gif"></igtbar:TBarButton>
					<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
					<igtbar:TBarButton Key="Cancel" ToolTip="Close window" Text="Cancel" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
				</Items>
			</igtbar:ultrawebtoolbar>
			<center>
				<b><asp:Label ID="lbBody" Runat="server">The following content will be created/updated</asp:Label></b>
				<br/>
				<asp:CheckBox id="cbTranslations" runat="server" Text="Check this box if you also want to copy the regionalizations & translations/localizations"/>
				<br/><br/>
				<igtbl:ultrawebgrid id="dg" runat="server" Height="250px" ImageDirectory="/ig_common/Images/">
					<DisplayLayout RowHeightDefault="20px" Version="4.00" BorderCollapseDefault="Separate" AllowRowNumberingDefault="Continuous"
						Name="dg" TableLayout="Fixed">
						<HeaderStyleDefault CssClass="gh"></HeaderStyleDefault>
						<FrameStyle BorderWidth="1px" Font-Size="xx-small" Font-Names="Verdana" BorderStyle="Solid"
							Height="250px"></FrameStyle>
						<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
						<RowStyleDefault CssClass="ugd"></RowStyleDefault>
					</DisplayLayout>
					<Bands>
						<igtbl:UltraGridBand>
							<Columns>
								<igtbl:UltraGridColumn HeaderText="Container" Key="Container" Width="100%">
									<CellStyle CssClass="ptb2"></CellStyle>
								</igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn HeaderText="Action" Key="Action" Width="150px">
									<CellStyle CssClass="ptb2"></CellStyle>
								</igtbl:UltraGridColumn>
							</Columns>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:ultrawebgrid>
			</center>
			<center><asp:label id="lbError" runat="server" CssClass="hc_error">Result message</asp:label></center>
</asp:Content>