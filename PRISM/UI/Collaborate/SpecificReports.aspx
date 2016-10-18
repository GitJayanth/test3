<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true"
  CodeFile="SpecificReports.aspx.cs" Inherits="UI_Collaborate_SpecificReports" %>

<%@ Register TagPrefix="cc1" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Specific Reports</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <table class="main" cellspacing="0" cellpadding="0">
    <tr valign="top" height="*">
      <td>
        <table width="100%" class="dataTable" border="1" cellspacing="0" cellpadding="0">
          <tr valign="top">
            <th width="20">
              Action</th>
            <th width="200">
              Name</th>
            <th width="*">
              Description</th>
            <th width="200">
              Parameters</th>
          </tr>
          <tr valign="top" class="ugd">
            <td>
              <asp:ImageButton ID="export" runat="server" ImageUrl="/hc_v4/img/ed_download.gif" OnClick="ExtractFlatCatego"/>
            </td>
            <td>
              Extract flat categorization in CSV file
            </td>
            <td>
              Extract of the full database, with all the product numbers with their 6 upper levels.
              <br />
              It will be in this file as many lines as product numbers.
              <br /><br />
              Once you click on the report you want, it could take up to 4 minutes to generate it. <br />
              When you finish to download the file. Unzip it, then use the "open with..." windows feature to open this file with Miscrosoft Excel.
            </td>
            <td>
              <asp:CheckBox ID="cbWithObso" runat="server" Text="Include obsolete products"/><br />
              <asp:CheckBox ID="cbWithPL" runat="server" Text="Include product lines"/><BR />
              <asp:CheckBox ID="cbWithPLC" runat="server" Text="Include plc"/>
            </td>
          </tr>
        </table>
        <asp:Label ID="lNotes" runat="server">Note: The extracted report is in CSV format.<br />If you wish to open it in Excel, please save the file on your desktop; then right click on the file, choose 'open with...' and select Excel.<br />Once the file is loaded in Excel, you can use 'File | Save as...' to create a real Excel file</asp:Label><br />
        <asp:Label ID="lbError" runat="server"></asp:Label></td>
    </tr>
  </table>
</asp:Content>
