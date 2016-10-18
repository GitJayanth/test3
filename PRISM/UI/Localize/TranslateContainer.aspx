<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="TranslateContainer.aspx.cs" Inherits="Default2"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" Runat="Server">TR List</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
  <table style="WIDTH: 100%; HEIGHT: 100%" cellspacing=0 cellpadding=0 border=1>
		<tr valign=top height=1>
	    <td colspan="2" >
				<igtbar:ultrawebtoolbar id=mainToolBar runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBLabel Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign=top>
			<td width="25%" height="90%">
        <iframe id="contents" name="contents" src="TRList.aspx"  scrolling="auto"/>
  			    Your browser does not allow local frames.
	    </td>
	    <td width="75%" height="90%">
  	   	<iframe id="main" name="main" src="TranslateContent.aspx" scrolling="auto"/>
            Your browser does not allow local frames.
      </td>
		</tr>
  </table> 
</asp:Content>