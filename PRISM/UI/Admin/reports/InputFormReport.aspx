
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="InputFormReport.aspx.cs" Inherits="UI_Admin_InputFormReport"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Input Form Report</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
        <br />
        <table  cellspacing='1' cellpadding='2' border='0' bgcolor='bbbbb0'>
            <tr>
                <td nowrap class='tabletitle' style="width: 227px; height: 45px;">
                    Select&nbsp; Input Form Name &nbsp;:<br />
                    <asp:ListBox ID="lbInputformName" EnableViewState = "true" AutoPostBack="false" runat="server" Height="104px" OnSelectedIndexChanged="lbInputformName_SelectedIndexChanged" On SelectionMode="Multiple" Width="328px"></asp:ListBox>
                    <asp:RequiredFieldValidator ID="reqInputForm" runat="server" ControlToValidate="lbInputformName" Text="Please select atleast one InputForm" Display="Static"></asp:RequiredFieldValidator>
                    &nbsp;<br />
                    [To Multi Select input form names use Ctrl key and then select]<br/>
                    &nbsp;<asp:Button ID="btnSubmit" runat="server" Font-Bold="True"
                        OnClick="btnSubmit_Click" Text="Generate Input Form List report" Width="312px" />
                    <br />
                    <asp:Button ID="BtnUsageReport" runat="server" Font-Bold="True" OnClick="BtnUsageReport_Click"
                        Text="Generate Input Form Usage Report" Width="312px" /></td>
            </tr>
        </table>
</asp:Content>