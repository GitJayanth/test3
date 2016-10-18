<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Help" CodeFile="Help.aspx.cs"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Help</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <IFRAME id="frmHelp" src="/hc_v4/pleasewait.htm" style="WIDTH: 100%; HEIGHT: 100%;Border:0px;Margin:0px" name="frmHelp" runat="server"></IFRAME>
</asp:Content>
