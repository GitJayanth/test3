<%@ Page language="c#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.DAM.Pages.ResourceList" CodeFile="ResourceList.aspx.cs"%>
<%@ Register TagPrefix="ucs" TagName="ResourceList" Src="./../../DAM/ResourceList2.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Libraries</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
	<ucs:ResourceList id="resourceList" runat="server"></ucs:ResourceList>
</asp:Content>