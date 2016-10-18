<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="AccessorieswithAttributes.aspx.cs" Inherits="Attribute" Title="Prism - Accessories with Attributes"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" Runat="Server">Accessories with Attributes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
<table>
    <tr>
        <td>
            <!--<h3>Accessories with Attributes</h3> -->
        </td>
    </tr> 
    
     <tr>
    <td colspan="2" rowspan="1" style="left: 0px; position: relative; top: 0px; height: 15px">
                          <asp:RadioButton ID="OrgName" runat="server" Checked="True" OnCheckedChanged="OrgName_CheckedChanged"
                              Text="OrgName" Width="75px" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="GroupName" runat="server" Text="GroupName" OnCheckedChanged="GroupName_CheckedChanged" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="GBUName" runat="server" OnCheckedChanged="GBUName_CheckedChanged"
                              Text="GBUName" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="PLName" runat="server" OnCheckedChanged="PLName_CheckedChanged"
                              Text="PLCode" AutoPostBack="True" GroupName="BizOptions" /></td>
    </tr>
    
    <tr>
        <td>
           <asp:Label ID = "BUType" runat="server" Width="111px" Visible="False">BusinessList</asp:Label>
        </td>
        <td>
           <asp:ListBox ID="BusinessList" runat="server" EnableViewState ="True" AutoPostBack = "True" SelectionMode="Multiple" Visible="False" Width="211px"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
           Regions
        </td>
        <td>
           <asp:DropDownList ID="ddlGeography" runat="server" Width="211px" Height="71px">                            
                        </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
         <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" Font-Size="Small" Height="28px"
                   Text="Generate Report" Width="178px" OnClick="btnSubmit_Click" Font-Names="Times New Roman" />
         </td>
          <td>
            <asp:Label ID="ErrorMsg" runat="server"></asp:Label>
        </td>
    </tr>
</table>

</asp:Content>
    
