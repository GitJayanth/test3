<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.DAM.DAMGeneric" CodeFile="damgeneric.aspx.cs" %>
<%@ Register TagPrefix="ucs" TagName="ResourceList" Src="ResourceList2.ascx" %>
	<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server"><%= Request["t"]%></asp:Content><asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
	<ucs:ResourceList id="resourceList" WorkspaceId='<%# Request["w"]!=null?Convert.ToInt32(Request["w"]):-1%>' runat="server"></ucs:ResourceList>
</asp:Content>