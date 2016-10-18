<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.New_Search_Result" CodeFile="SearchResult.aspx.cs"%>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"%>
<%@ Register TagPrefix="hc" TagName="progressbar" Src="~/tools/holoadingpage.ascx"%>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ASCX" TagName="RangeFilter" Src="RangeFilter.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Search results</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript" >
  
        
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'Save') {
        oEvent.cancelPostBack = true;
        document.getElementById('savingForm').style.display='';
       
       
      }
      if (oButton.Key == 'Refresh')
      {
        oEvent.cancelPostBack = !window.confirm("Depending on your query, it could take a long to run.");
      }
		}
		
		document.onkeypress = checkkey;
    function checkkey(keyStroke)
    {
      if (event.keyCode == 13) 
      {     
        event.returnValue = false;
        event.cancelBubble = true; 
      }                              
    }
    
    function g_p(i,c,ct)
    {
      var p = (ct=="2"?"./UI/Globalize/QDETranslate.aspx":"./UI/Acquire/qde.aspx");
      window.open("../../../Redirect.aspx?p=" + p + "&i=" + i + "&c=" + c);
    }

  </script>
	    <table class="main" style="width: 100%;" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top" style="height:1px">
      <td colspan="2">
        <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
          <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
          <Items>
            <igtbar:TBarButton Key="Save" Tooltip="Enter details to save the search" Text="Details" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
            <igtbar:TBSeparator Key="DeleteSep" />
            <igtbar:TBarButton Key="Delete" Tooltip="Delete query" Text="Delete" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
            <igtbar:TBSeparator />
            <igtbar:TBarButton Key="Refresh" Tooltip="Run the search again" Text="Refresh" Image="/hc_v4/img/ed_build.gif"></igtbar:TBarButton>
            <igtbar:TBSeparator />
            <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif" ToolTip="Export "></igtbar:TBarButton>
          </Items>
          <DefaultStyle BorderWidth="1px" BorderStyle="None" BackgroundImage="/ig_common/webtoolbar2/ig_tb_back03.gif"
            height="22px"></DefaultStyle>
        </igtbar:ultrawebtoolbar></td>
    </tr>
    <tr>
      <td>
        <asp:Label ID="lbError" runat="server"></asp:Label>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <fieldset id="savingForm" style="display:none;"><legend>Query saving data</legend>
			    <table cellspacing="0" cellpadding="0" width="100%" border="0">
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
						    <asp:label id="lbQueryName" Runat="server" Text="Name"></asp:label>
					    </td>
					    <td class="ugd">
						    <asp:TextBox id="txtQueryName" runat="server" width="220px"></asp:TextBox>
                            <asp:Label ID="lblNameValid" runat="server" Text="Label" Width="163px"></asp:Label></td>
				    </tr>
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
						    <asp:label id="lbQueryDescription" Runat="server" Text="Description"></asp:label>
					    </td>
					    <td class="ugd">
						    <TextArea id="txtQueryDescription" runat="server" rows="5" cols="40"></TextArea>
                            <asp:Label ID="lblDesValid" runat="server" Text="Label"></asp:Label></td>
				    </tr>
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
						    <asp:label id="lbQueryVisibility" Runat="server" Text="Level"></asp:label>
					    </td>
					    <td class="ugd">
						    <asp:RadioButtonList id="txtQueryVisibility" runat="server">
						      <asp:ListItem Text="Private" Value="private" Selected/>
						      <asp:ListItem Text="Public" Value="public"/>
						    </asp:RadioButtonList>
					    </td>
				    </tr>
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
					      &nbsp;						    
					    </td>
					    <td class="ugd">
						    <asp:Button id="saveBtn" runat="server" Text="Save" OnClick="saveBtn_Click" ></asp:Button>
						    
					    </td>
				    </tr>
				  </table>
        </fieldset>
        <asp:label id="LblError" runat="server" ForeColor="Red" Visible="False">Label</asp:label>
        <br/>
        <asp:label id="lRecordcount" runat="server">NbRecords</asp:label><br/>
        <br/>
        <asp:Panel id="tooManyRowsPanel" runat="server" Visible="false">
          There are too many rows to be displayed in this window, you can:
          <asp:Panel id="XLSPanel" runat="server" Visible="false">
          <LI><asp:LinkButton ID="XLSLink" runat="server" Text="download the result in Excel format" OnClick="excelLink_Click"></asp:LinkButton></LI>
          </asp:Panel>
          <asp:Panel id="CSVPanel" runat="server">
          <LI><asp:LinkButton ID="CSVLink" runat="server" Text="download the result in CSV format" OnClick="csvLink_Click"></asp:LinkButton></LI>
          </asp:Panel>
        </asp:Panel>
        <igtbl:UltraWebGrid id="dg" runat="server" OnGroupColumn="dg_GroupColumn">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" Version="4.00"
            ViewType="OutlookGroupBy" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="xctl0dg" TableLayout="Fixed" CellClickActionDefault="RowSelect"
            NoDataMessage="No data to display">
         <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
            <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand BaseTableName="SearchText" Key="SearchText" BorderCollapse="Collapse" DataKeyField="ChunkId">
              <Columns>
                <igtbl:UltraGridColumn HeaderText="rtl" Key="rtl" Hidden="True" BaseColumnName="Rtl" ServerOnly="True"/>
                <igtbl:UltraGridColumn HeaderText="ChunkId" Key="ChunkId" Hidden="True" BaseColumnName="ChunkId"/>
                <igtbl:UltraGridColumn HeaderText="ProductType" Key="ProductType" Width="200px" BaseColumnName="ProductType" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Culture" Key="Culture" Width="50px" BaseColumnName="Culture"/>
                <igtbl:UltraGridColumn Key="CultureTypeId" BaseColumnName="CultureTypeId" Hidden="true" ServerOnly="true"/>
                <igtbl:UltraGridColumn HeaderText="Level" Key="LevelId" Width="50px" BaseColumnName="LevelId"/>
                <igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" Hidden="true"/>
                 <igtbl:UltraGridColumn HeaderText="ProductName" Key="ProductName" Width="160px" BaseColumnName="ProductName" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:TemplatedColumn HeaderText="ProductName" Key="ProductNameLink" Width="160px" BaseColumnName="ProductName" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                  <CellTemplate>
                    <a id="ProductNameLink" href="#" onclick='<%# "javascript:g_p("+ Container.Cell.Row.Cells.FromKey("ItemId").Value + ",&quot;"+ Container.Cell.Row.Cells.FromKey("Culture").Value + "&quot;,"+ Container.Cell.Row.Cells.FromKey("CultureTypeId").Value + ");"%>'><%# Container.Text%></a>
                  </CellTemplate>
                </igtbl:TemplatedColumn>
                <igtbl:UltraGridColumn HeaderText="Item status" Key="Status" Width="70px" BaseColumnName="Status"/>
                <igtbl:UltraGridColumn HeaderText="xmlname" Key="xmlname" Width="150px" BaseColumnName="xmlname"/>
                <igtbl:UltraGridColumn HeaderText="Container" Key="ContainerName" Width="150px" BaseColumnName="ContainerName" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Value" Key="ChunkValue" Width="300px" BaseColumnName="ChunkValue" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn HeaderText="Status" Key="ChunkStatus" Width="50px" BaseColumnName="ChunkStatus"/>
                <igtbl:UltraGridColumn HeaderText="Inherited" Key="IsInherited" BaseColumnName="IsInherited" Type="CheckBox"/>
                <igtbl:UltraGridColumn HeaderText="Modified" Key="ModifiedDate" Width="100px" BaseColumnName="ModifiedDate" CellMultiline="Yes">
                  <CellStyle Wrap="True"></CellStyle>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>
      </td>
    </tr>
  </table>
</asp:Content>