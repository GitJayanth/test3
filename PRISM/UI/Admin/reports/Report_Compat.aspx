<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="Report_Compat.aspx.cs" Inherits="Report_Compat" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" Runat="Server">
Compatibilities Report</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
<table>
    <tr>
        <td>
            <h3>Products With Compatibilities</h3>
        </td>
    </tr>
                     
                 
    <tr>
    <td colspan="2" rowspan="1" style="left: 0px; position: relative; top: 0px; height: 15px">
                          <asp:RadioButton ID="OrgName" runat="server" Checked="True" OnCheckedChanged="OrgName_CheckedChanged"
                              Text="OrgName" Width="75px" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="GroupName" runat="server" Text="GroupName" OnCheckedChanged="GroupName_CheckedChanged" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="GBUName" runat="server" OnCheckedChanged="GBUName_CheckedChanged"
                              Text="GBUName" AutoPostBack="True" GroupName="BizOptions" /></td>
    </tr>
                              <tr>
        <td>
            <asp:Label ID = "LinkType" runat="server" Width="111px">Linksdropdown</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="Linksdropdown" runat="server" EnableViewState = "true" AutoPostBack="false"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID = "BUType" runat="server" Width="111px" Visible="False">BusinessList</asp:Label>
        </td>
        <td>
            <asp:ListBox ID="BusinessList" runat="server" EnableViewState ="true" AutoPostBack = "false" SelectionMode="Multiple" Visible="False"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
        <asp:Label ID="Label1" runat="server" Width="111px">Region</asp:Label>
        </td>
        <td>
        <!--<asp:DropDownList ID="dropCultures" runat="server" EnableViewState="true" AutoPostBack="false"></asp:DropDownList>-->
        <asp:ListBox ID="CulturesList" runat="server" EnableViewState ="true" AutoPostBack = "false" SelectionMode="Multiple"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
        <asp:Button ID="submit1" runat="server" Text="Submit" OnClick="submit1_Click" />
        </td>
        <td>
            <asp:Label ID="ErrorMsg" runat="server" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
<hr />
<hr />
<table>
    <tr>
        <td>
            <h3>Products Without Compatibilities</h3>
        </td>
    </tr> 
       <tr>
    <td colspan="2" rowspan="1" style="left: 0px; position: relative; top: 0px; height: 15px">
                          <asp:RadioButton ID="OrgName1" runat="server" Checked="True" OnCheckedChanged="OrgName_CheckedChanged"
                              Text="OrgName" Width="75px" AutoPostBack="True" GroupName="BizOptions1" />
                          <asp:RadioButton ID="GroupName1" runat="server" Text="GroupName" OnCheckedChanged="GroupName_CheckedChanged" AutoPostBack="True" GroupName="BizOptions1" />
                          <asp:RadioButton ID="GBUName1" runat="server" OnCheckedChanged="GBUName_CheckedChanged"
                              Text="GBUName" AutoPostBack="True" GroupName="BizOptions1" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID = "BUType1" runat="server" Width="111px" Visible="False"></asp:Label>
        </td>
        <td>
            <asp:ListBox ID="BusinessList1" runat="server" EnableViewState ="true" AutoPostBack = "false" SelectionMode="Multiple" Visible="False"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
        <asp:Label ID="Label2" runat="server" Text="Region" Width="111px"></asp:Label>
        </td>
        <td>
        <asp:ListBox ID="CulturesList1" runat="server" EnableViewState ="true" AutoPostBack = "false" SelectionMode="Multiple"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
        <asp:Button ID="submit2" runat="server" Text="Submit" OnClick="submit2_Click" />
        </td>
            <td>
            <asp:Label ID="ErrorMsg1" runat="server"></asp:Label>
        </td>
    </tr>  
</table>
</asp:Content>

