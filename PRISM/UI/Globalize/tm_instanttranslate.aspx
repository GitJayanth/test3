<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Main.Globalize.TM_InstantTranslate" CodeFile="TM_InstantTranslate.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">TinyTM on demand</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" Runat="Server">
  <script>
    function uwToolbar_Click(oToolbar, oButton, oEvent)
    {
        if (oButton.Key == 'Close') 
        {
            oEvent.cancelPostBack = true;
            window.close();
        }
    }
		  
	//Added by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign) -- 13/Jun/09	  
    function ResetParentCheckBox()
    { 
        var myForm = parent.opener.document.forms[0];
        for (i=0; i<myForm.length; i++)
        {
            if ((myForm.elements[i].id.indexOf('g_sd')!=-1) && myForm.elements[i].checked)
            {
                myForm.elements[i].checked = false;
            }
        }	  
    }
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript">
    document.body.onunload = "UpdateParentWithoutClose";
  </script>
<igtbar:ultrawebtoolbar id=uwToolbar runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbartitle">
        <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
        <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
        <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
        <Items>
          <igtbar:TBLabel Text="ItemName" Key="ItemName">
            <DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
          </igtbar:TBLabel>
        </Items>
      </igtbar:ultrawebtoolbar>
      <igtbar:ultrawebtoolbar id=Ultrawebtoolbar1 runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar" ImageDirectory=" ">
<HoverStyle CssClass="hc_toolbarhover">
</HoverStyle>

<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
</ClientSideEvents>

<SelectedStyle CssClass="hc_toolbarselected">
</SelectedStyle>

<Items>
<igtbar:TBarButton Key="Close" ToolTip="Close window" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
<igtbar:TBSeparator Key="CloseSep" ></igtbar:TBSeparator>
<igtbar:TBarButton Key="Export" ToolTip="Export a detailed Report of which chunks have been translated" Text="TM Report" Image="/hc_v4/img/ed_download.gif">
</igtbar:TBarButton>
</Items>

<DefaultStyle CssClass="hc_toolbardefault">
</DefaultStyle>
</igtbar:ultrawebtoolbar>
<asp:label id=lbError runat="server" Visible="False" ForeColor="Red">Label</asp:label>
<asp:label id=lblWarningMessage runat="server" Visible="False">Label</asp:label>
<igtbl:ultrawebgrid id=dg runat="server" Width="100%">
        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
          HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
          NoDataMessage="No data to display">
          <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
          </HeaderStyleDefault>
          <FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
          <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
          <ActivationObject AllowActivation="False"></ActivationObject>
          <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
        </DisplayLayout>
        <Bands>
          <igtbl:UltraGridBand Key="ChunksBand" BorderCollapse="Collapse" DataKeyField="CultureCode">
            <Columns>
              <igtbl:UltraGridColumn HeaderText="Code" Key="CultureCode" Width="50px" Hidden="True" BaseColumnName="CultureCode">
                <Footer Key="CultureCode"></Footer>
                <Header Key="CultureCode" Caption="Code"></Header>
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Language" Key="CultureName" Width="200px" BaseColumnName="CultureName">
                <CellStyle Font-Bold="True"></CellStyle>
                <Footer Key="CultureName"></Footer>
                <Header Key="CultureName" Caption="Language"></Header>
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Created" Key="Created" Width="80px" BaseColumnName="NbInsert">
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Updated" Key="Updated" Width="80px" BaseColumnName="NbUpdate">
              </igtbl:UltraGridColumn>
            </Columns>
          </igtbl:UltraGridBand>
        </Bands>
      </igtbl:ultrawebgrid>
      <br /><br />
      <%-- Added by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign) -- 21/May/09 --%>
      <igtbl:ultrawebgrid id=dgContainer runat="server" Width="80%">
        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
          HeaderClickActionDefault="SortSingle" EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dgContainer" TableLayout="Fixed"
          NoDataMessage="No data to display">
          <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
          </HeaderStyleDefault>
          <FrameStyle Width="80%" CssClass="dataTable"></FrameStyle>
          <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
          <ActivationObject AllowActivation="False"></ActivationObject>
          <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
        </DisplayLayout>
        <Bands>
          <igtbl:UltraGridBand Key="TRChunksBand" BorderCollapse="Collapse" DataKeyField="ContainerName">
            <Columns>
              <igtbl:UltraGridColumn HeaderText="Container Name" Key="ContainerName" Width="70%" BaseColumnName="ContainerName">
                <Footer Key="ContainerName"></Footer>
                <Header Key="ContainerName" Caption="Container Name"></Header>
              </igtbl:UltraGridColumn>
              <igtbl:UltraGridColumn HeaderText="Translatable Chunk Count" Key="TRChunkCount" Width="30%" BaseColumnName="TRChunkCnt">
              </igtbl:UltraGridColumn>
            </Columns>
          </igtbl:UltraGridBand>
        </Bands>
      </igtbl:ultrawebgrid>
</asp:Content>