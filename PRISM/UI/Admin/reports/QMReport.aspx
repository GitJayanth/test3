<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="QMReport.aspx.cs" Inherits="QMReport" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HOPT" Runat="Server">
</asp:Content>--%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Quality Monitor Report</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HOCP" Runat="Server">

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


    <script type="text/javascript">
        function SetId(ids)
	    {
	       document.getElementById("HF1").value=ids.id;
	              
		}
		function frmLoad()
		{
		  var ctls=document.getElementById("HF1").value;
          document.getElementById(ctls).checked=false;  
		  //document.getElementById("ch").visible=false;
		}
    </script>

      
              <TABLE width="50%" border="0" cellpadding="5" cellspacing="3">
                  <tr>
                      <td colspan="2" rowspan="" style="left: 0px; position: relative; top: 0px; height: 16px">
                          <asp:Label ID="Label2" runat="server" Text="Select Options" Width="146px"></asp:Label></td>
                  </tr>
                  <tr>
                      <td colspan="2" rowspan="" style="left: 0px; position: relative; top: 0px; height: 15px">
                          <asp:RadioButton ID="OrgName" runat="server" Checked="True" OnCheckedChanged="OrgName_CheckedChanged"
                              Text="OrgName" Width="75px" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="GroupName" runat="server" Text="GroupName" OnCheckedChanged="GroupName_CheckedChanged" AutoPostBack="True" GroupName="BizOptions" />
                          <asp:RadioButton ID="GBUName" runat="server" OnCheckedChanged="GBUName_CheckedChanged"
                              Text="GBUName" AutoPostBack="True" GroupName="BizOptions" /></td>
                  </tr>
                
                <tr>
                    <td style="width: 7%">
                        <asp:Label ID="Label1" runat="server" Text="Select OrgName:" Width="84px"></asp:Label></td>
                    <td style="width: 14%">
                        <asp:ListBox ID="LstBoxBusinessUnitName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="LstBoxBusinessUnitName_SelectedIndexChanged" Width="100%" Height="90" SelectionMode="Multiple" ></asp:ListBox></td>
                       
                </tr>
                <tr>
                    <td style="width: 7%"><asp:Label ID="lblRegionName" runat="server" EnableViewState="false">Select Region:</asp:Label></td>
                    <td style="width: 14%"><asp:ListBox ID="LstBoxRegionName" runat="server" AutoPostBack="true" OnSelectedIndexChanged = "LstBoxRegionName_SelectedIndexChanged" Width="100%" SelectionMode="Multiple"></asp:ListBox></td>
                </tr>
                <tr>
                    <td style="width: 7%"><asp:Label ID="lblOptions" runat="server" EnableViewState="false">Select Options:</asp:Label></td>
                    <td style="width: 14%">
                        <asp:RadioButton ID="rbtnInputForm" runat="server" GroupName="viewOptions" OnCheckedChanged="rbtnInputForm_CheckedChanged" Text="InputForm View" AutoPostBack="True" /><br/>
                        <asp:RadioButton ID="rbtnContainer" runat="server" GroupName="viewOptions" OnCheckedChanged="rbtnContainer_CheckedChanged" Text="Container View" AutoPostBack="True" />
                    </td>
                </tr>
                <asp:Panel ID="pnlInputForm" runat="server" Visible="false">
                    <tr>
                        <td width="25%"><asp:Label ID="lblInputFormName" runat="server" EnableViewState="false">Select Input Form:</asp:Label></td>
                        <td width="25%">
                            <asp:ListBox ID="lstboxInputFormName" Height="180" runat="server" SelectionMode="Multiple" AutoPostBack="true" EnableViewState="true" Width="100%" OnSelectedIndexChanged = "lstboxInputFormName_SelectedIndexChanged"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" align="center" colspan=2><asp:Button id="btnNext" runat="server" Text="Next" OnClick="btnNext_Click"></asp:Button></td>        
                         
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlContainer" runat="server" Visible="false">
                    <tr>
                        <td width="25%"><asp:Label ID="lblContainer" runat="server" EnableViewState="false">Container's Readable Name:</asp:Label></td>
                        <td width="25%">
                            <asp:TextBox ID="txtContainer" runat="server"></asp:TextBox>
                            <asp:Label ID="wildcardmsg" runat="server" EnableViewState="false" >WildCards accepted</asp:Label><br /></td>
                                              
                    </tr>
                   <tr>
                        <td width="100%" align="center" colspan="2"><asp:Button id="btnSearchCont" runat="server" Text="Search" OnClick="btnSearchCont_Click"></asp:Button></td>        
                    </tr>
                </asp:Panel>
         
                 <asp:Panel ID="pnlSearchRes" runat="server" Visible="true" >
                    <tr>
                        <td width="100%" colspan="2" align="center">
                          <igtbl:UltraWebGrid ID="uwgSearchRes" runat="server" OnInitializeLayout="uwgSearchRes_InitializeLayout" Visible = "false">
                            </igtbl:UltraWebGrid>
                        </td>
                    </tr>
                 </asp:Panel>
               
            </TABLE>
             <br />
</asp:Content>


