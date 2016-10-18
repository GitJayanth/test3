<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_Chunk" CodeFile="QDE_Chunk.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk Edit</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" language='javascript' src='qde_chunk.js'></script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <table style="WIDTH:100%;HEIGHT:100%" border="0" cellpadding="0 " cellspacing="0">
        <tr style="height: auto">
          <td><iframe id="chunkFrame" border="0" frameborder="0" scrolling="no" src="/hc_v4/dummy.htm" style="WIDTH:100%;HEIGHT:100%"></iframe>
          </td>
        </tr>
        <tr style="height: 1px">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="20px" CssClass="hc_toolbar">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
              <Items>
                <igtbar:TBLabel Text="Input Form" Key="IfName">
                  <DefaultStyle Width="100%" Font-Bold="True" TextAlign="Left"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBLabel Text="Chunk x/n" Key="ChunkCount">
                  <DefaultStyle Width="100px" TextAlign="Right"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBLabel Text="">
                  <DefaultStyle Width="5px"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBarButton Key="Prev" ToolTip="Previous" Image="/hc_v4/Img/ed_prev.gif"></igtbar:TBarButton>
                <igtbar:TBarButton Key="Next" ToolTip="Next" Image="/hc_v4/Img/ed_next.gif"></igtbar:TBarButton>
                <igtbar:TBarButton Key="Close" ToolTip="Close" Image="/hc_v4/Img/ed_cancel.gif"></igtbar:TBarButton>
              </Items>
            </igtbar:ultrawebtoolbar>
          </td>
        </tr>
      </table>
      <input type="hidden" id="curpos" name="curpos" value="" />
</asp:Content>