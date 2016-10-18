<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="LinksReport.aspx.cs" Inherits="UI_Collaborate_LinksReport" Title="Untitled Page" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Links Report</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
	
  <table class="main" cellspacing="0" cellpadding="0">
    <tr valign="bottom" height="*">
      <td align="right">
        <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" Width="100%" ItemWidthDefault="80px"
          CssClass="hc_toolbar" ImageDirectory=" " BackgroundImage="" OnButtonClicked="uwToolbar_ButtonClicked">
          <HoverStyle CssClass="hc_toolbarhover">
          </HoverStyle>
         <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" ></ClientSideEvents>
          <SelectedStyle CssClass="hc_toolbarselected">
          </SelectedStyle>
          <Items>
          <igtbar:TBCustom ID="TBCustom1" runat="server" Width="50px">
            <asp:label ID= "Label1" Text="Country" runat="server" Width="50px">
            </asp:label>
           </igtbar:TBCustom>
          <igtbar:TBCustom ID="TBCustom2" runat="server" Width="160px">
              <asp:DropDownList ID="DDL_Countries" runat="server"  Width="150px">
              </asp:DropDownList>
            </igtbar:TBCustom>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBCustom ID="TBCustom6" runat="server" Width="50px">
            <asp:label ID= "Label2" Text="Class" runat="server" Width="50px">
            </asp:label>
           </igtbar:TBCustom>
            <igtbar:TBCustom ID="TBCustom3" runat="server" Width="160px">
              <asp:DropDownList ID="Prod_Type" runat="server"  Width="150px">
              </asp:DropDownList>
              </igtbar:TBCustom>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBCustom ID="TBCustom7" runat="server" Width="50px">
            <asp:label ID= "Label3" Text="LinkType" runat="server" Width="50px">
            </asp:label>
           </igtbar:TBCustom>
             <igtbar:TBCustom ID="TBCustom4" runat="server" Width="100px">
              <asp:DropDownList ID="Link_Type" runat="server"  Width="90px">
              </asp:DropDownList>
              </igtbar:TBCustom>
              <igtbar:TBSeparator></igtbar:TBSeparator> 
            <igtbar:TBCustom ID="TBCustom8" runat="server" Width="60px">
                <asp:label ID= "Label4" Text="LimitValue" runat="server" Width="60px">
                </asp:label>
                </igtbar:TBCustom>
              
              <igtbar:TBCustom ID="TBCustom5" runat="server" Width="60px">
              <asp:DropDownList ID="Links_Num" runat="server" Width="50px">
              </asp:DropDownList>
              </igtbar:TBCustom>
            <igtbar:TBSeparator></igtbar:TBSeparator>
               
            <igtbar:TBarButton key="Generate" Text="Generate" Image="/hc_v4/img/dam_ok.png" >
              </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
            </igtbar:TBarButton>
            <igtbar:TBSeparator></igtbar:TBSeparator>
            <igtbar:TBarButton Key="Anomalies" Text="Anomalies" Image="/hc_v4/img/ed_download.gif">
            </igtbar:TBarButton>
          </Items>
          <DefaultStyle CssClass="hc_toolbardefault">
          </DefaultStyle>
        </igtbar:UltraWebToolbar>
      </td>
    </tr>

    <tr valign="top">
    <td align="center">
    <fieldset id="optionsFieldset"><legend>Filters</legend>
		 <table cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr valign="middle">
			    <td class="editLabelCell" style="width:130px">
			      PLC Status (*)
			    </td>
			    <td class="ugd">
                    <asp:checkboxlist id="statusFilter" runat="server" RepeatColumns="5">
                         <asp:ListItem Text="Live" Value="Live" Selected="true"></asp:ListItem>
                    </asp:checkboxlist>
        
		         </td>
		    </tr>
		    <tr valign="middle">
		         <td class="editLabelCell" style="width:130px">
			          Include inheritance/Fallback
			        </td>
                 <td class="ugd">
                     <asp:CheckBox ID="inheritanceFilter" Text="InheritedLinks" runat="server" />
                 </td>
            </tr>
	    </table>
        </fieldset>
    </td>
    </tr>
   
    <tr valign="top" height="1">
      <td>
        <asp:Label ID="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>
      </td>
    </tr>
    <tr valign="top">
      <td class="main">
        <igtbl:UltraWebGrid ID="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%"
          OnInitializeRow="dg_InitializeRow">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            CellClickActionDefault="RowSelect" NoDataMessage="No data to display" AllowRowNumberingDefault ="Continuous" ScrollBar="Auto">
            <Pager PageSize="50" PagerAppearance="Top" StyleMode="ComboBox" AllowPaging="True" >
            </Pager>
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd">
            </ClientSideEvents>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
              <igtbl:UltraGridColumn BaseColumnName="ItemId" Key="ItemId" Hidden="true" >
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SubItemId" Key="SubItemId" Hidden="true" >
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PLCode" HeaderText="PLCode" Key="PLCode" Width="50px" AllowResize="Free">
                </igtbl:UltraGridColumn>
               <igtbl:UltraGridColumn BaseColumnName="ProductLevel" HeaderText="ProductLevel" Key="Level_Name" Width="75px" AllowResize="Free">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="HostName" HeaderText="HostName" Key="LinkFrom_ItemName" Width="200px" AllowResize="Free">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="HostSKUNumber" HeaderText="HostSKUNumber" Key="LinkFrom_Itemnumber" Width="150px" AllowResize="Free">
                  </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="CompanionSKUName" HeaderText="CompanionSKUName" Key="LinkTo_ItemName" Width="200px" AllowResize="Free">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="CompanionSKUNumber" HeaderText="CompanionSKUNumber" Key="LinkTo_Itemnumber" Width="150px" AllowResize="Free">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="PLCStatus" HeaderText="PLCStatus" Key="ItemStatus" Width="75px" AllowResize="Free">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="SortOrder" HeaderText="SortOrder" Key="SubItemSort" Width="75px" AllowResize="Free">
                </igtbl:UltraGridColumn>                
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
       
        </igtbl:UltraWebGrid>
        <input type="hidden" name="action" id="action">
      </td>
    </tr>
  </table>
</asp:Content>



