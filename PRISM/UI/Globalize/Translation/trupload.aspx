<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="hc_termbase.UI.Globalize.TRUpload" CodeFile="TRUpload.aspx.cs"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">TR upload</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>
<script type="text/javascript">
function wBtProcess_Click(oButton, oEvent){
	if (dg_nbItems_Checked==0){
	  alert('You must select at least one item');
	  oEvent.cancel = true;
	}
 oButton.setEnabled(oEvent.cancel);
}
function wBtDelete_Click(oButton, oEvent){
	if (dg_nbItems_Checked==0){
	  alert('You must select at least one item');
	  oEvent.cancel = true;  
	}
	else{
	  oEvent.cancel = !confirm('Are you sure?');
	}
 oButton.setEnabled(oEvent.cancel);
}
window.onload=g_i;
	function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
</script>

  <table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top">
      <td class="sectionTitle" height="17">
        <asp:label id="lbTitle" runat="server">Upload a new TR</asp:label></td>
    </tr>
    <tr valign="top" style="height:1px">
      <td>
        <fieldset>
          <legend>TR Zip file</legend>
          <table border="0">
            <tr valign="middle">
              <td>
              <asp:FileUpload ID="txtTRUpload" runat="server" CssClass="hc_upload"/>                
                </td>                
              <td>
                <asp:Button id="btnUpload" runat="server" Text="-- Upload --" onclick="btnUpload_Click"></asp:Button>
                <asp:RegularExpressionValidator id="regTRFormat" runat="server" ErrorMessage="Only TR zip files are allowed"
                  Display="Dynamic" ValidationExpression="\b(.*\\)+[Tt][Rr][_][0-9]*\.[zZ][iI][pP]\b" ControlToValidate="txtTRUpload"></asp:RegularExpressionValidator></td>
            </tr>
          </table>
        </fieldset>
        <span id="bodyPre" runat="server"></span><br/>
      </td>
    </tr>
    <tr valign="top" style="height:1px">
      <td>
        <fieldset>
          <legend>Delete an Existing TR</legend>
          <asp:DropDownList ID="ddlCurrentTRs" runat="server" Width="100px">
          </asp:DropDownList>&nbsp; Comment &nbsp;<asp:TextBox ID="txtDeleteComment" runat="server" Width="200px"></asp:TextBox> <asp:Button id="BtnDeleteTR" runat="server" Text="Delete" OnClick="BtnDeleteTR_Click"></asp:Button>
          <asp:Label ID="txtTRMessage" runat="server" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr valign="top" style="HEIGHT: 17px">
      <td class="sectionTitle">
        <asp:label id="Label1" runat="server">Process awaiting TRs</asp:label></td>
    </tr>
    <tr class="hc_toolbar" style="HEIGHT: 26px" valign="top">
      <td>
        <igtxt:webimagebutton id="wBtProcess" runat="server" Text="Process" Width="80px" Height="16px" OnClick="wBtProcess_Click1" CausesValidation="False">
          <Alignments VerticalImage="Middle"></Alignments>
          <ClientSideEvents Click="wBtProcess_Click"></ClientSideEvents>
          <PressedAppearance>
            <Style CssClass="hc_toolbarselected"></Style>
          </PressedAppearance>
          <HoverAppearance>
            <Style CssClass="hc_toolbarhover"></Style>
          </HoverAppearance>
          <Appearance>
            <Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
            <Image Url="/hc_v4/img/ed_new.gif" AlternateText="Process"></Image>
          </Appearance>
        </igtxt:webimagebutton>
        <igtxt:webimagebutton id="wBtDelete" runat="server" Text="Delete selected" Width="120px" Height="16px" CausesValidation="False" OnClick="wBtDelete_Click1">
          <Alignments VerticalImage="Middle"></Alignments>
          <ClientSideEvents Click="wBtDelete_Click"></ClientSideEvents>
          <PressedAppearance>
            <Style CssClass="hc_toolbarselected"></Style>
          </PressedAppearance>
          <HoverAppearance>
            <Style CssClass="hc_toolbarhover"></Style>
          </HoverAppearance>
          <Appearance>
            <Style BorderWidth="0px" CssClass="hc_toolbardefault"></Style>
            <Image Url="/hc_v4/img/ed_delete.gif" AlternateText="Delete selected"></Image>
          </Appearance>
        </igtxt:webimagebutton>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <asp:Label id="lbMessage" runat="server" Visible="False">error message</asp:Label>
        <igtbl:ultrawebgrid id="dg" runat="server" Width="100%" OnInitializeRow="dg_InitializeRow">
          <DISPLAYLAYOUT NoDataMessage="No data to display" CellClickActionDefault="RowSelect" TableLayout="Fixed"
            Name="dg" RowSelectorsDefault="No" EnableInternalRowsManagement="True" HeaderClickActionDefault="SortSingle"
            SelectTypeRowDefault="Single" Version="4.00" AllowSortingDefault="Yes" AutoGenerateColumns="False"
            MergeStyles="False">
            <HEADERSTYLEDEFAULT CssClass="gh" HorizontalAlign="Left" VerticalAlign="Top" TextOverflow="Ellipsis">
              <BORDERDETAILS WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BORDERDETAILS>
            </HEADERSTYLEDEFAULT>
            <FRAMESTYLE Width="100%" CssClass="dataTable"></FRAMESTYLE>
            <ROWALTERNATESTYLEDEFAULT CssClass="uga"></ROWALTERNATESTYLEDEFAULT>
            <ROWSTYLEDEFAULT CssClass="ugd" VerticalAlign="Top" TextOverflow="Ellipsis" BorderWidth="1px"></ROWSTYLEDEFAULT>
					  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
          </DISPLAYLAYOUT>
          <BANDS>
            <igtbl:UltraGridBand>
              <COLUMNS>
                <igtbl:TemplatedColumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
                  <CellStyle VerticalAlign="Middle" Wrap="True"></CellStyle>
                  <HeaderTemplate>
                    <asp:CheckBox id="g_ca" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                  </HeaderTemplate>
                  <CellTemplate>
                    <asp:CheckBox id="g_sd" onclick="return g_su(this);" runat="server"></asp:CheckBox>
                  </CellTemplate>
                  <Footer Key="Select" Caption=""></Footer>
                  <Header Key="Select"></Header>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn HeaderText="Name" Key="FileName" Width="250px" BaseColumnName="name" CellMultiline="No"></igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Upload Date" Key="UploadDate" Width="120px" BaseColumnName="created"
                  CellMultiline="No"></igtbl:UltraGridColumn>
              </COLUMNS>
            </igtbl:UltraGridBand>
          </BANDS>
        </igtbl:ultrawebgrid></td>
      </tr>
  </table>
</asp:Content>
