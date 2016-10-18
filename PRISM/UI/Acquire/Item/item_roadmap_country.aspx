<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.ItemManagement.Item_Roadmap_Country" CodeFile="Item_Roadmap_Country.aspx.cs"%>
<%@ Register TagPrefix="hc" TagName="countryViewGraphic" Src="~/UI/Localize/RoadMap/RoadMap_CountryViewGraphic.ascx"%>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Roadmap</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%">
    <tr valign="top">
      <td>
        <hc:countryViewGraphic id="vGraphic" runat="server"></hc:countryViewGraphic>
      </td>
    </tr>
  </table>
</asp:Content>
