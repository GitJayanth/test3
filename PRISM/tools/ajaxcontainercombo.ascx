<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Control Language="c#" Inherits="HyperCatalog.Web.Components.AjaxContainerCombo" CodeFile="AjaxContainerCombo.ascx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<script>
</script>
<igcmbo:webcombo id="WebCombo1" style="Z-INDEX: 200" DBackColor="White" DataTextField="Tag" DisplayValue="Tag"
  DataValueField="Id" ComboTypeAhead="None" Editable="True" Version="3.00" BorderColor="LightGray"
  Height="16px" ForeColor="Black" Width="202px" BackColor="White" BorderWidth="1px" BorderStyle="solid"
  runat="server" ToolTip="Please make a choice">
  <Columns>
    <igtbl:UltraGridColumn HeaderText="Tag" Key="Tag" Width="10px" Hidden="True" BaseColumnName="">
      <Footer Key="Tag"></Footer>
      <Header Key="Tag" Caption="Tag"></Header>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="">
      <Footer Key="Name">
        <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
      </Footer>
      <Header Key="Name" Caption="Name">
        <RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
      </Header>
    </igtbl:UltraGridColumn>
  </Columns>
  <ClientSideEvents EditKeyUp="WebCombo1_EditKeyUp" InitializeCombo="WebCombo1_InitializeCombo" AfterSelectChange="WebCombo1_AfterSelectChange"></ClientSideEvents>
  <DropDownLayout DropdownWidth="302px" ColFootersVisible="Yes" RowSelectors="No" RowHeightDefault="17px"
    HeaderClickAction="Select" AutoGenerateColumns="False" DropdownHeight="" ColHeadersVisible="No"
    GridLines="None">
    <RowStyle Cursor="Hand" BorderWidth="0px" BorderStyle="None" BackColor="White"></RowStyle>
    <SelectedRowStyle ForeColor="White" BackColor="DarkBlue"></SelectedRowStyle>
    <HeaderStyle BorderStyle="Solid" BackColor="LightGray">
      <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
    </HeaderStyle>
    <FrameStyle Width="302px" Cursor="Default" BorderWidth="1px" BorderColor="LightGray" BorderStyle="Solid"
      BackColor="White"></FrameStyle>
  </DropDownLayout>
  <ExpandEffects ShadowColor=""></ExpandEffects>
</igcmbo:webcombo>
