<%@ Reference Page="~/ui/admin/capabilities.aspx" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Analysis" CodeFile="Analyses.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Analysis</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script>
      var s = top.location;
      var toolBar;
      var selectedAnalyze=-1;
      function back(){
        top.location = s;
      }
		
		function dg_CellClickHandler(gridName, cellId, button){
			var cell = igtbl_getCellById(cellId);
			selectedAnalyze = cell.Row.getCellFromKey('Id').getValue();
     }

		function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
      if (oButton.Key=="Run") 
      {
        if (selectedAnalyze < 0){
          alert("Please, select an analyze first");
          oEvent.cancelPostBack = true;
        }
        else{
          var url='Analysis/Analysis_Results.aspx?d=' + selectedAnalyze;
          open(url,'queryprocess','toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,width=800,height=600');
        }        
        oEvent.cancelPostBack = true;
      }      
		}		

  </script>
  <table class="main" style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" border="0">
    <tr height="1">
      <td class="sectionTitle">
        <asp:label id="lbTitle" runat="server">Analysis List</asp:label></td>
    </tr>
    <asp:panel id="panelGrid" Runat="server">
      <tr valign="top" style="height:1px">
        <td height="26">
          <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
            <HOVERSTYLE CssClass="hc_toolbarhover"></HOVERSTYLE>
            <DEFAULTSTYLE CssClass="hc_toolbardefault"></DEFAULTSTYLE>
            <SELECTEDSTYLE CssClass="hc_toolbarselected"></SELECTEDSTYLE>
            <ITEMS>
              <igtbar:TBarButton Image="/hc_v4/img/ed_new.gif" Text="Add" Key="Add"></igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Image="/hc_v4/img/ed_play.gif" Text="Run" Key="Run"></igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBLabel Text="Filter">
                <DEFAULTSTYLE Font-Bold="True" Width="40px"></DEFAULTSTYLE>
              </igtbar:TBLabel>
              <igtbar:TBCustom Key="filterField" Width="150px">
                <asp:TextBox id="txtFilter" runat="server" Width="150px" MaxLength="50" cssClass="Search"></asp:TextBox>
              </igtbar:TBCustom>
              <igtbar:TBarButton Image="/hc_v4/img/ed_search.gif" Key="filter">
                <DEFAULTSTYLE Width="25px"></DEFAULTSTYLE>
              </igtbar:TBarButton>
            </ITEMS>
            <CLIENTSIDEEVENTS Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar"></CLIENTSIDEEVENTS>
          </igtbar:ultrawebtoolbar></td>
      </tr>
      <tr valign="top" style="height:auto">
        <td>
          <igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
            <DisplayLayout AllowUpdateDefault="Yes" NoDataMessage="No data to display" TableLayout="Fixed"
              Name="dg" Version="4.00" AutoGenerateColumns="False" MergeStyles="False">
              <HEADERSTYLEDEFAULT CssClass="gh" VerticalAlign="Middle">
                <BorderDetails WidthBottom="1pt" StyleRight="Solid" WidthRight="1pt" StyleBottom="Solid"></BorderDetails>
              </HEADERSTYLEDEFAULT>
              <FRAMESTYLE CssClass="dataTable" Width="100%"></FRAMESTYLE>
              <CLIENTSIDEEVENTS CellClickHandler="dg_CellClickHandler"></CLIENTSIDEEVENTS>
              <SELECTEDROWSTYLEDEFAULT CssClass="ugs"></SELECTEDROWSTYLEDEFAULT>
              <ROWALTERNATESTYLEDEFAULT CssClass="uga"></ROWALTERNATESTYLEDEFAULT>
              <ROWSTYLEDEFAULT CssClass="ugd" VerticalAlign="Top" BorderWidth="1px" TextOverflow="Ellipsis"></ROWSTYLEDEFAULT>
            </DisplayLayout>
            <Bands>
              <igtbl:UltraGridBand Key="Containers">
                <COLUMNS>
                  <igtbl:UltraGridColumn Key="Id" Width="10px" BaseColumnName="Id" Hidden="True" IsBound="True" HeaderText="Id">
                    <FOOTER Key="Id"></FOOTER>
                    <HEADER Key="Id" Caption="Id"></HEADER>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="Name" Width="150px" BaseColumnName="Name" IsBound="True" HeaderText="Name"
                    CellMultiline="Yes">
                    <CELLSTYLE Wrap="True"></CELLSTYLE>
                  </igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn Key="NameLink" Width="150px" BaseColumnName="Name" HeaderText="Name" CellMultiline="Yes">
                    <CELLTEMPLATE>
                      <asp:LinkButton id="lnkEdit" onclick="UpdateGridItem" runat="server">
                        <%#DataBinder.Eval(Container, "Text")%>
                      </asp:LinkButton>
                    </CELLTEMPLATE>
                    <FOOTER Key="Name"></FOOTER>
                    <HEADER Key="Name" Caption="Name"></HEADER>
                  </igtbl:TemplatedColumn>
                  <igtbl:UltraGridColumn Key="Description" Width="100%" BaseColumnName="Description" IsBound="True" HeaderText="Description"
                    CellMultiline="Yes">
                    <CELLSTYLE Wrap="True"></CELLSTYLE>
                    <FOOTER Key="Description"></FOOTER>
                    <HEADER Key="Description" Caption="Description"></HEADER>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="Owner" Width="150px" BaseColumnName="Owner" IsBound="True" HeaderText="Owner"
                    CellMultiline="Yes">
                    <CELLSTYLE Wrap="True"></CELLSTYLE>
                    <FOOTER Key="Owner"></FOOTER>
                    <HEADER Key="Owner" Caption="Owner"></HEADER>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="CreateDate" Width="90px" BaseColumnName="CreateDate" HeaderText="Created on">
                    <FOOTER Key="CreateDate"></FOOTER>
                    <HEADER Key="CreateDate" Caption="Created on"></HEADER>
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="LastRun" Width="90px" BaseColumnName="LastRunDate" HeaderText="Last run on"
                    CellMultiline="Yes">
                    <CELLSTYLE Wrap="True"></CELLSTYLE>
                    <FOOTER Key="LastRun"></FOOTER>
                    <HEADER Key="LastRun" Caption="Last run on"></HEADER>
                  </igtbl:UltraGridColumn>
                </COLUMNS>
              </igtbl:UltraGridBand>
            </Bands>
          </igtbl:ultrawebgrid></td>
      </tr>
    </asp:panel>
    <tr valign="top">
      <td>
        <igtab:ultrawebtab id="webTab" runat="server" Width="100%" BorderColor="#808080" BorderStyle="Solid"
          LoadAllTargetUrls="False" visible="false" DummyTargetUrl="/hc_v4/pleasewait.htm" Height="100%" BorderWidth="1px">
          <DEFAULTTABSTYLE Height="25px" BackColor="WhiteSmoke"></DEFAULTTABSTYLE>
          <ROUNDEDIMAGE SelectedImage="ig_tab_lightb2.gif" NormalImage="ig_tab_lightb1.gif" FillStyle="LeftMergedWithCenter"></ROUNDEDIMAGE>
          <TABS>
            <igtab:Tab Text="Properties" DefaultImage="/hc_v4/img/ed_properties.gif" Key="Properties">
              <CONTENTPANE TargetUrl="./Analysis/Analysis_Properties.aspx"></CONTENTPANE>
            </igtab:Tab>
            <igtab:Tab Text="Roles" DefaultImage="/hc_v4/img/ed_roles.gif" Key="Roles">
              <CONTENTPANE TargetUrl="./Analysis/Analysis_Roles.aspx"></CONTENTPANE>
            </igtab:Tab>
          </TABS>
        </igtab:ultrawebtab></td>
    </tr>
  </table>
</asp:Content>
