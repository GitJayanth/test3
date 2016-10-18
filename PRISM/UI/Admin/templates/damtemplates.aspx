<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.DAM.UI.Acquire.dam.DAMTemplates" CodeFile="damtemplates.aspx.cs" %>
<%@ Register TagPrefix="ucs" TagName="ResourceList" Src="./../../DAM/ResourceList2.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Templates</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<ucs:ResourceList id="resourceList" WorkspaceId="2" runat="server"></ucs:ResourceList>
</asp:Content>