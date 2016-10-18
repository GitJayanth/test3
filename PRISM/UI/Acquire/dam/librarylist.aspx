<%@ Page language="c#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.DAM.Pages.LibraryList" CodeFile="LibraryList.aspx.cs" %>
<%@ Register TagPrefix="ucs" TagName="LibraryList" Src="~/UI/DAM/librarylist.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Libraries</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
	<ucs:LibraryList id="libraryList" runat="server"></ucs:LibraryList>
</asp:Content>