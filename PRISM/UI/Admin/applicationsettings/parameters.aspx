<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" ValidateRequest="false" Inherits="HyperCatalog.UI.Admin.Architecture.Parameters" CodeFile="Parameters.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Application parameters</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <STYLE>
  .lbl_msgInformation{font-weight:bold;color:green;}
  .lbl_msgWarning{font-weight:bold;color:orange;}
  .lbl_msgError{font-weight:bold;color:red;}
  </STYLE>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <table style="WIDTH: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top" style="height:1px">
      <td>
        <igtbar:ultrawebtoolbar id="mainToolBar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
          <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
          <Items>
            <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
			<igtbar:TBLabel Text=" ">
				<DefaultStyle Width="100%"></DefaultStyle>
			</igtbar:TBLabel>
          </Items>
        </igtbar:ultrawebtoolbar></td>
    </tr>
    <tr valign="top">
      <td>
		<asp:Label id="msgLbl" style="font-weight:bold;" runat="server"></asp:Label>
		<asp:Repeater id="parameters" runat="server">
			<HeaderTemplate>
				<table cellspacing="0" cellpadding="0" width="100%" border="0">
			</HeaderTemplate>
			<ItemTemplate>
				<tr valign="middle" runat="server" Visible='<%# applicationParameters[(string)Container.DataItem].UIVisible != HyperCatalog.Business.ApplicationParameter.UIVisibility.Hidden%>'>
					<td class="editLabelCell" width="130" height="22px">
						<asp:label id="lbParameterName" Runat="server" Text='<%# Container.DataItem%>'></asp:label>
					</td>
					<td class="ugd">
						<asp:label id="lbParameterValue" runat="server" Text='<%# applicationParameters[(string)Container.DataItem].Value%>' Visible='<%# applicationParameters[(string)Container.DataItem].UIVisible == HyperCatalog.Business.ApplicationParameter.UIVisibility.ReadOnly%>'></asp:label>
						<asp:textbox id="txtParameterValue" runat="server" width="180px" Text='<%# applicationParameters[(string)Container.DataItem].Value%>' Visible='<%# applicationParameters[(string)Container.DataItem].UIType != HyperCatalog.Business.ApplicationParameter.ValueType.DateTime && applicationParameters[(string)Container.DataItem].UIVisible == HyperCatalog.Business.ApplicationParameter.UIVisibility.Full%>'></asp:textbox>
                        <asp:label id="UTCcomment" runat="server" Text=" (UTC)" Visible='<%# (applicationParameters[(string)Container.DataItem].Value!=null && applicationParameters[(string)Container.DataItem].UIType == HyperCatalog.Business.ApplicationParameter.ValueType.DateTime)%>'></asp:label>
						<asp:label id="lbUnit" runat="server" Text='<%# applicationParameters[(string)Container.DataItem].UIUnit%>'></asp:label>
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
      </td>
    </tr>
  </table>
</asp:Content>