<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Chunk.Chunk_Container" CodeFile="Chunk_Container.aspx.cs" %>
<%@ Register TagPrefix="hc" TagName="containerInfo" Src="~/UI/Acquire/Chunk/ContainerInfo.ascx"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk_Container</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <style>
    BODY{background-color:whitesmoke;}
  </style>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
    <hc:containerinfo id="ContainerInfoPanel" runat="server" Visible="true"></hc:containerinfo>
</asp:Content>
