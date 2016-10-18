<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="Report_NodeLevelCompat.aspx.cs" Inherits="Report_NodeLevelCompat" Title="Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
<%--PR659863 -To checked whether the host items is associated with any links. If no then hidden filed is updated--%>
<script language = "javascript" type ="text/javascript">
    function excelDownload() {
        var con = confirm("There are no links associated with this host. Do you want to create empty spreadsheet for this host?");
        if (con) {

            document.getElementById('<%=hfcon.ClientID %>').value = "yes";

            __doPostBack();
        }
        else {
            document.getElementById('<%=hfcon.ClientID %>').value = "no";
            window.close();
        }
    }
</script>
<asp:HiddenField ID="hfcon" runat="server" Value = "no"/>
<table>
    <tr>
        <td>
            <asp:Label ID="Result" runat="server"></asp:Label>
        </td>
    </tr>
</table>
</asp:Content>
