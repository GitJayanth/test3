<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="Gemstone_reports.aspx.cs" Inherits="Gemstone_reports" Title="Gemstone Reports" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" Runat="Server">
Gemstone Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">
<table>
    <tr>
        <td style="width: 259px">
            <h3>Delivery Report</h3>
        </td>
    </tr> 
    
    <tr>
        <td style="width: 259px; height: 76px">
            <asp:Label ID = "Message" runat="server" Text="Please click the button to generate the Delivery report" Width="239px" Height="29px" Font-Bold="True" Font-Size="Larger"></asp:Label>
            <br />
            <br />
            <br />
             <asp:Button ID="Submit" runat="server" Text="Generate Report" OnClick="submit1_Click" Width="219px" Font-Bold="True" Font-Size="Larger" /></td>
        <td style="height: 76px">
            &nbsp;</td>
    </tr>
    
</table>
    <br />
    <br />
    <br />
<table style="width: 968px; height: 79px">
          <tr>
        <td>
            <h3>PM Inbound Report</h3>
        </td>
    </tr> 
            <tr>
                <td style="width: 261px">
                    <strong><span style="font-size: 10pt; font-family: Arial">Business Unit &nbsp; </span></strong>&nbsp;<asp:DropDownList ID="ddlBusinessUnit"
                        runat="server" Width="129px" EnableViewState = "true" AutoPostBack="false" Height="64px">                         
                    </asp:DropDownList></td>
                <td style="width: 107px">
                    <span style="font-family: Arial"><strong><span style="font-size: 10pt">
                        <span>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                            &nbsp;
                            <br />
                            <span style="font-size: 10pt; font-family: Arial">
                        Date </span></span><span style="font-family: Arial">
                                &nbsp;<br />
                            </span></span></strong></span>
                </td>
                <td style="width: 207px">
                    &nbsp;<igsch:WebDateChooser ID="DCDate" runat="server" Height="1px" Width="130px">
        </igsch:WebDateChooser>
                </td>
                <td style="width: 135px">
                    <strong><span style="font-size: 11pt; font-family: Arial"></span></strong></td>
                <td>
                    <br />
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 261px">
                </td>
                <td style="width: 107px">
                </td>
                <td style="width: 207px">
                </td>
                <td style="width: 135px">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 261px; height: 13px">
                </td>
                <td style="width: 107px; height: 13px">
                </td>
                <td style="width: 207px; height: 13px">
                </td>
                <td style="width: 135px; height: 13px">
                </td>
                <td style="height: 13px">
                </td>
            </tr>
        </table>
        <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" Font-Size="Small" Height="27px"
            Text="Generate Report" Width="210px" OnClick="btnSubmit_Click" Font-Names="Times New Roman" /><br />
        <br/>
        <br />
         <asp:Label ID="ErrorMsg" runat="server" Visible="False"></asp:Label>
</asp:Content>

