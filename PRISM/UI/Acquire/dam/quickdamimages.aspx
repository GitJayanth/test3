<%@ Page language="c#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.DAM.QuickDAMImages" CodeFile="QuickDAMImages.aspx.cs" %>
<%@ Register TagPrefix="ucs" TagName="ResourceList" Src="./../../DAM/ResourceList2.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOTOP" Runat="Server"><base  target="_self"/></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
	<ucs:ResourceList id="resourceList" DisplayMode="Minimal" WorkspaceId="1" runat="server"></ucs:ResourceList>
</asp:Content>