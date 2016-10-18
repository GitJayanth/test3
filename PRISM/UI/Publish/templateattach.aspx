<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Publish.TemplateAttach" CodeFile="TemplateAttach.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Attach a template</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" language="javascript">
    document.body.onload=function(){document.body.focus();}; 
  </script>
  <STYLE>
    BODY{BACKGROUND:whitesmoke;}
  </STYLE>
      <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr class="selectlanguage">
          <td>&nbsp;<asp:label id="lItemName" runat="server">CurrentItemName</asp:label></td>
          <td align="right"><asp:label id="lItemLevel" runat="server">CurrentItemLevel</asp:label>&nbsp;</td>
        </tr>
      </table>
      <table class="main" cellpadding="0" cellspacing="0">
        <tr valign="top" style="height:1px">
          <td>
            <igtbar:ultrawebtoolbar id="uwToolBar" runat="server" ImageDirectory=" " CssClass="hc_toolbar" ItemWidthDefault="80px"
              width="100%">
              <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
              <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
              <Items>
                <igtbar:TBLabel Text="Level to apply">
                  <DefaultStyle Width="100px"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBCustom Width="150px">
                  <asp:DropDownList id="ddlLevels" runat="server" AutoPostBack="True" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                </igtbar:TBCustom>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBLabel Text="Template">
                  <DefaultStyle Width="100px"></DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBCustom Width="250px">
                  <igcmbo:WebCombo BorderStyle="Solid" TypeAhead=none DisplayValueColumn="Name" BorderWidth="1px" Editable="False" BackColor="White" Height="16px"
                    ForeColor="Black" Width="250px" BorderColor="LightGray" ID="cbTemplates" DBackColor="White">
                    <ExpandEffects ShadowColor="Transparent"></ExpandEffects>
                    <Columns>
                      <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="Name">
                        <Footer Key="Template">
                          <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                        </Footer>
                        <Header Key="Template" Caption="Name">
                          <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
                        </Header>
                      </igtbl:UltraGridColumn>
                    </Columns>
                    <DropDownLayout DropdownWidth="450px" BorderCollapse="Separate" RowSelectors="No" RowHeightDefault="17px"
                      HeaderClickAction="Select" AutoGenerateColumns="False" DropdownHeight="" ColHeadersVisible="No">
                      <RowStyle BorderWidth="0px" BorderColor="Gray" BorderStyle="None" BackColor="White">
                        <BorderDetails WidthLeft="0px" WidthTop="0px"></BorderDetails>
                      </RowStyle>
                      <SelectedRowStyle ForeColor="White" BackColor="DarkBlue"></SelectedRowStyle>
                      <HeaderStyle BorderStyle="Solid" BackColor="LightGray">
                        <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                      </HeaderStyle>
                      <FrameStyle Width="300px" Cursor="Default" BorderWidth="2px" Font-Size="xx-small" Font-Names="Verdana"
                        BorderStyle="Ridge" BackColor="White"></FrameStyle>
                    </DropDownLayout>
                  </igcmbo:WebCombo>
                </igtbar:TBCustom>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Key="add" ToolTip="Attach to all Templates of the applicable level" Text="Apply"
                  Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Key="applyAll" ToolTip="Attach to all Templates of all levels underneath applicable level"
                  Text="Apply all" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
              </Items>
              <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
            </igtbar:ultrawebtoolbar>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <asp:Label ID="lbError" Runat="server" CssClass="hc_error" Visible="false"></asp:Label>
          </td>
        </tr>
        <tr valign="top" style="height:1px">
          <td>
            <table align="right" cellpadding="0" cellspacing="0" border="0">
              <tr>
                <td>&nbsp;</td>
                <td id="AppLevelTitle" runat="server" class="gh" align="center">Applicable levels</td>
                <td width="24">&nbsp;</td>
              </tr>
            </table>
          </td>
        </tr>
        <tr valign="top">
          <td>
            <igtbl:ultrawebgrid id="dg" runat="server" Width="100%" ImageDirectory="/ig_common/Images/" UseAccessibleHeader="False">
              <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient" SelectTypeRowDefault="Single"
                HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                CellClickActionDefault="RowSelect" NoDataMessage="No Templates attached to this container">
                <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
                  <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </HeaderStyleDefault>
                <FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
                <RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
              </DisplayLayout>
              <Bands>
                <igtbl:UltraGridBand>
                  <Columns>
                    <igtbl:UltraGridColumn Key="Herited" Hidden="True" BaseColumnName="Herited"></igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn HeaderText="Level" Key="ItemLevelId" Width="50px" BaseColumnName="ItemLevelId">
                      <CellStyle HorizontalAlign="Center"></CellStyle>
                      <Footer Key="ItemLevelId"></Footer>
                      <Header Key="ItemLevelId" Caption="Level"></Header>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn HeaderText="Item Name" Key="ItemName" Width="100%" BaseColumnName="ItemName">
                      <CellStyle Wrap="True"></CellStyle>
                      <Footer Key="ItemPath"></Footer>
                      <Header Key="ItemPath" Caption="Item Name"></Header>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn HeaderText="Name" Key="TemplateName" Width="250px" BaseColumnName="TemplateName">
                      <CellStyle Wrap="True"></CellStyle>
                      <Footer Key="IFName"></Footer>
                      <Header Key="IFName" Caption="Name"></Header>
                    </igtbl:UltraGridColumn>
                    <igtbl:TemplatedColumn Key="Action" Width="25px" BaseColumnName="">
                      <CellStyle HorizontalAlign="Center"></CellStyle>
                      <CellTemplate>
                        <asp:ImageButton id="imgDel" onclick="Delete_Click" style="cursor:pointer" runat="server" ImageUrl="/hc_v4/img/ed_delete.gif"
                          ToolTip="Delete"></asp:ImageButton>
                      </CellTemplate>
                      <Footer Key="Action"></Footer>
                      <Header Key="Action"></Header>
                    </igtbl:TemplatedColumn>
                  </Columns>
                </igtbl:UltraGridBand>
              </Bands>
            </igtbl:ultrawebgrid>
          </td>
        </tr>
      </table>
</asp:Content>