<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" AutoEventWireup="true" CodeFile="pl_users.aspx.cs" Inherits="UI_Admin_pls_pl_users" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Members</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script>
	  function uwToolbar_Click(oToolbar, oButton, oEvent){
	    if (oButton.Key == 'filter')
	    {     		     
	      DoSearch();	
        oEvent.cancelPostBack = true;
        return;
      }
	    if (oButton.Key == 'List') {
        back();
        oEvent.cancelPostBack = true;
      }
      if (oButton.Key == 'Add' || oButton.Key == 'Remove')
			{
      oEvent.cancelPostBack = !confirm('are you sure?');
      }
	  } 
  </script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix enlarged button issue by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
    <table class="main" cellspacing="0" cellpadding="0">
      <tr valign="top" style="height: 1px">
        <td>
          <igtbar:UltraWebToolbar ID="uwToolbar" runat="server"
            CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
            </ClientSideEvents>
            <Items>
              <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="ApplyChanges" Text="Apply changes" Image="/hc_v4/img/ed_save.gif">
              <%--<DefaultStyle Width="120px">
                </DefaultStyle>--%>
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBLabel Text="Filter">
                <DefaultStyle Font-Bold="True">
                </DefaultStyle>
              </igtbar:TBLabel>
              <igtbar:TBCustom Key="filterField" runat="server">
                <asp:TextBox runat="server" CssClass="Search" Width="150px" ID="txtFilter" MaxLength="50"></asp:TextBox>
              </igtbar:TBCustom>
              <igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                <%--<DefaultStyle Width="25px">
                </DefaultStyle>--%>
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBLabel Text="All users">
                <DefaultStyle Font-Bold="True">
                </DefaultStyle>
              </igtbar:TBLabel>
              <igtbar:TBCustom Key="filterAllUsers" runat="server">
                <asp:CheckBox ID="cbFilterAllUsers" runat="server" AutoPostBack="True" OnCheckedChanged="cbFilterAllUsers_CheckedChanged">
                </asp:CheckBox>
              </igtbar:TBCustom>
            </Items>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr valign="top" style="height: 1px">
        <td>
          <br />
          <asp:Label ID="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
        </td>
      </tr>
      <tr valign="top" height="*">
        <td>
          <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%" OnInitializeRow="dg_InitializeRow">
            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
              SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No"
              Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect">
              <%--Removed the css property and added inline property By Radha to fix infragestic grid line issue--%>
              <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </HeaderStyleDefault>
              <FrameStyle Width="100%" CssClass="dataTable">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </FrameStyle>
              <RowAlternateStyleDefault CssClass="uga">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </RowAlternateStyleDefault>
              <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </RowStyleDefault>
              <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
            </DisplayLayout>
            <Bands>
              <igtbl:UltraGridBand Key="Users" BorderCollapse="Collapse" DataKeyField="UserId">
                <Columns>
                  <igtbl:TemplatedColumn FooterText="" Key="Select" Width="20px">
                    <CellTemplate>
                      <asp:CheckBox ID="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                    </CellTemplate>
                    <HeaderTemplate>
                      <asp:CheckBox ID="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
                    </HeaderTemplate>
                    <CellStyle VerticalAlign="Middle" Wrap="True">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                  <igtbl:UltraGridColumn HeaderText="UserId" Key="UserId" ServerOnly="True" BaseColumnName="Id">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Name" Key="UserName" Width="100%" BaseColumnName="FullName">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Organization" Key="OrgName" Width="200px" BaseColumnName="OrgName">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Role" Key="RoleName" Width="200px" BaseColumnName="RoleName">
                  </igtbl:UltraGridColumn>
                </Columns>
              </igtbl:UltraGridBand>
            </Bands>
          </igtbl:UltraWebGrid>
          <center>
            <asp:Label ID="lbNoresults" runat="server" Visible="False" ForeColor="Red" Font-Bold="True">No results</asp:Label>
          </center>
        </td>
      </tr>
    </table>

    <script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

    <script>
    window.onload=g_i
    </script>
</asp:Content>