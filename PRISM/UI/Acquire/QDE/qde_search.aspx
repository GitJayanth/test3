<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="qde_search" CodeFile="QDE_Search.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Create roll</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
      <script>
      function DoSearch(){
        var elem=document.getElementById?document.getElementById('sField'):bw.ie4?document.all['sField']:0;
        if (elem.value==''){alert('search field cannot be null');return false;}
        parent.frames[3].location='QDE_search.aspx?f=' + escape(elem.value);
        elem.value='';
      }
      </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <style>
    BODY{BACKGROUND: whitesmoke; }
  </style>
  <script type"text/javascript" language="javascript">
    document.body.oncontextmenu = function(){return false;};
  </script>
      <asp:panel id="panelSearch" Runat="server">
        <table height="20" cellspacing="0" cellpadding="0" width="100%" border="0">
          <TR>
            <td class="cartouche_top" colspan="2"><img height="5" src="/hc_v4/img/cartouche_center_back.gif" width="37"></td>
          </tr>
          <tr class="cartouche_bottom" valign="middle" align="left">
            <td noWrap width="50"><B>Search</B></td>
            <td>
              <asp:TextBox id="sField" runat="server" Width="120" MaxLength="255"></asp:TextBox><A onclick="DoSearch(this)" href="#"><img alt="Search" src="/hc_v4/img/ed_find.gif" align="middle" border="0"></A>
            </td>
          </tr>
        </table>
      </asp:panel><asp:panel id="panelResult" Runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
          <tr class="selectlanguage">
            <td class="selectlanguage">&nbsp;
              <asp:Label id="lbSearchDetail" runat="server">Search results for</asp:Label></td>
          </tr>
        </table>
        <table style="WIdTH: 100%" height="100%" cellspacing="0" cellpadding="2" border="1">
          <tr valign="top">
            <TH height="15">
              <asp:Label id="lbSearchResults" Runat="server"></asp:Label><br/>
            </TH>
          <tr valign="top">
            <td width="100%">
              <div style="OVERFLOW: auto; WIdTH: 100%; HEIGHT: 100%">
                <igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
                  <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" ColHeadersVisibleDefault="No" Version="4.00"
                    SelectTypeRowDefault="Single" EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg"
                    TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="Nothing match your search">
                    <Pager QuickPages="15" PageSize="25" Alignment="Center" AllowPaging="True"></Pager>
                    <ClientSideEvents MouseOverHandler="dg_MouseOverHandler" MouseOutHandler="dg_MouseOutHandler"></ClientSideEvents>
          <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
          </HeaderStyleDefault>
          <FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
          <SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
          <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
                  </DisplayLayout>
                  <Bands>
                    <igtbl:UltraGridBand BaseTableName="Items" Key="Items" BorderCollapse="Collapse" DataKeyField="ItemId">
                      <Columns>
                        <igtbl:UltraGridColumn HeaderText="ItemId" Key="ItemId" IsBound="True" Hidden="True" BaseColumnName="ItemId"></igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" IsBound="True" Width="100%" BaseColumnName="ItemName"></igtbl:UltraGridColumn>
                      </Columns>
                    </igtbl:UltraGridBand>
                  </Bands>
                </igtbl:UltraWebGrid></div>
            </td>
          </tr>
        </table>
      </asp:panel>
</asp:Content>