<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RedirectReportsToBoss.aspx.cs" Inherits="UI_Admin_reports_RedirectReportsToBoss" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Redirect Reports to BoE</title>
    <link href="/hc_v4/css/Output.css" type="text/css" rel="STYLESHEET"/>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
    <td>
    <img src="/hc_v4/img/hp_Logo.gif" border="0" 
        style="height: 40px; width: 40px"></img>
    </td>
    <td>
        <asp:Label id="Label1" runat="server" Text="This PRISM report is available in the BoE Reporting System" Font-Names="Arial" Font-Size="10 pt" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    </table>
    <div>
    <%--<asp:Label id="BossLinkLabel" runat="server" Text="This PRISM report is available in the BOSS Reporting System" Font-Names="Arial" Font-Size="10 pt" Font-Bold="true">
    </asp:Label>--%>
    <br />
    <div id="divTable" runat="server"></div>
    <br />
    <asp:HyperLink ID="AccessLink" runat="server" NavigateUrl="http://ent212.sharepoint.hp.com/teams/IMSEMEA/Content%20Management/Forms/AllItems.aspx?RootFolder=%2Fteams%2FIMSEMEA%2FContent%20Management%2FPrism%20UAT%2FBOSS%5FReporting&FolderCTID=0x0120006F880A6F53755042AAB6269FB3038E0D&View=%7bB65AEB8F-4E1D-428E-B89E-A19DCD7111D7%7d" Text="Instructions to request BoE access and BoE reports documentation" ForeColor="Navy" Target="_blank"></asp:HyperLink>
    <%--<a id="AccessLink" href="http://ent212.sharepoint.hp.com/teams/IMSEMEA/Content%20Management/Forms/AllItems.aspx?RootFolder=%2Fteams%2FIMSEMEA%2FContent%20Management%2FPrism%20UAT%2FBOSS%5FReporting&FolderCTID=0x0120006F880A6F53755042AAB6269FB3038E0D&View=%7bB65AEB8F-4E1D-428E-B89E-A19DCD7111D7%7d">Instructions to request BoE access and BoE reports documentation</a>--%>
    <br />
    </div>
    </form>
</body>
</html>
