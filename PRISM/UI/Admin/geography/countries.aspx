<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Admin.Architecture.Countries" CodeFile="Countries.aspx.cs" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Countries</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <style>
    .lbl_msgInformation{font-weight:bold;color:green;}
    .lbl_msgWarning{font-weight:bold;color:orange;}
    .lbl_msgError{font-weight:bold;color:red;}
  </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <table style="width: 100%" cellspacing="0" cellpadding="0" border="0">
    <asp:Panel ID="panelGrid" runat="server">
      <tbody>
        <tr valign="top" style="height: 1px">
          <td>
            <igtbar:UltraWebToolbar ID="mainToolBar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px"
              Width="100%">
              <HoverStyle CssClass="hc_toolbarhover">
              </HoverStyle>
              <DefaultStyle CssClass="hc_toolbardefault">
              </DefaultStyle>
              <SelectedStyle CssClass="hc_toolbarselected">
              </SelectedStyle>
              <Items>
                <igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBLabel Text="Region">
                  <DefaultStyle Font-Bold="True" Width="80px">
                  </DefaultStyle>
                </igtbar:TBLabel>
                <igtbar:TBCustom Key="RegionFilter" Width="250px">
                  <asp:DropDownList ID="regionList" runat="Server" AutoPostBack="True" DataTextField="Name"
                    DataValueField="Code">
                  </asp:DropDownList>
                </igtbar:TBCustom>
                <igtbar:TBLabel Text=" ">
                  <DefaultStyle Width="100%">
                  </DefaultStyle>
                </igtbar:TBLabel>
              </Items>
            </igtbar:UltraWebToolbar>
          </td>
        </tr>
        <tr valign="top">
          <td>
            <asp:Label ID="mainMsgLbl" runat="server"></asp:Label>
            <igtbl:UltraWebGrid ID="countriesGrid" runat="server" Width="100%">
              <DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="OnClient" RowHeightDefault="100%"
                Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
                AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                CellClickActionDefault="RowSelect" NoDataMessage="No region defined" LoadOnDemand="Manual">
                <%--removed the css class and added the styles inline--Start%>--%>
                <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true">  <%--CssClass="gh">--%>
				    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
				</HeaderStyleDefault>
				<FrameStyle Width="100%" CssClass="dataTable">
                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                </FrameStyle>
                <%--removed the css class and added the styles inline--End%>--%>
                <SelectedRowStyleDefault CssClass="ugs">
                </SelectedRowStyleDefault>
                <RowAlternateStyleDefault CssClass="uga">
                </RowAlternateStyleDefault>
                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                </RowStyleDefault>
              </DisplayLayout>
              <Bands>
                <igtbl:UltraGridBand Key="Countries" BorderCollapse="Collapse" DataKeyField="Code">
                  <Columns>
                    <igtbl:TemplatedColumn HeaderText="Code" Key="CountryCode">
                      <CellTemplate>
                        <asp:LinkButton ID="CodeLink" OnClick="UpdateGridItem" runat="server">
														<img alt="" src='/hc_v4/img/flags/<%#Container.Text%>.gif' border=0 valign='middle'/>
														<%#Container.Text%>
                        </asp:LinkButton>
                      </CellTemplate>
                    </igtbl:TemplatedColumn>
                    <igtbl:UltraGridColumn HeaderText="Name" Key="Name" BaseColumnName="Name" Width="300">
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn HeaderText="Region" Key="Region" BaseColumnName="Region" Width="300">
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn Width="100%" Key="Empty">
                    </igtbl:UltraGridColumn>
                  </Columns>
                </igtbl:UltraGridBand>
              </Bands>
            </igtbl:UltraWebGrid></td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="panelProperties" runat="server" Visible="false">
      <tr valign="top" style="height: 1px">
        <td>
          <igtbar:UltraWebToolbar ID="propertiesToolBar" runat="server" CssClass="hc_toolbar"
            ItemWidthDefault="80px" Width="100%">
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <Items>
              <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Save" ToolTip="Save changes" Text="Save" Image="/hc_v4/img/ed_save.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Delete" ToolTip="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif">
              </igtbar:TBarButton>
              <igtbar:TBLabel Text=" ">
                <DefaultStyle Width="100%">
                </DefaultStyle>
              </igtbar:TBLabel>
            </Items>
          </igtbar:UltraWebToolbar>
          <tr valign="top">
            <td>
              <asp:Label ID="propertiesMsgLbl" runat="server"></asp:Label>
              <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="lbCountryName" runat="server" Text="Name"></asp:Label></td>
                  <td class="ugd">
                    <asp:TextBox ID="txtCountryNameValue" runat="server" Width="180px"></asp:TextBox></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="lbCountryCode" runat="server" Text="Code"></asp:Label></td>
                  <td class="ugd">
                    <asp:TextBox ID="txtCountryCodeValue" runat="server" Width="180px" MaxLength="2"
                      Enabled="false"></asp:TextBox></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="lbRegionCode" runat="server" Text="Region"></asp:Label></td>
                  <td class="ugd">
                    <asp:DropDownList ID="txtRegionCodeValue" runat="server" Width="180px">
                    </asp:DropDownList></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label1" runat="server" Text="Can Edit Content"></asp:Label></td>
                  <td class="uga">
                    <asp:CheckBox ID="cbEditContent" runat="server" /></td>
                </tr>
                <tr valign="middle" style="visibility:hidden; display:none;">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label2" runat="server" Text="Can Edit PLC"></asp:Label></td>
                  <td class="ugd">
                    <asp:CheckBox ID="cbEditPLC" runat="server" /></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label3" runat="server" Text="Can Edit Market Segments"></asp:Label></td>
                  <td class="ugd">
                    <asp:CheckBox ID="cbEditMS" runat="server" /></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label7" runat="server" Text="Can Edit Publishers"></asp:Label></td>
                  <td class="uga">
                    <asp:CheckBox ID="cbEditPublishers" runat="server" /></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label4" runat="server" Text="Can Edit Cross Sells"></asp:Label></td>
                  <td class="ugd">
                    <asp:CheckBox ID="cbEditCS" runat="server" /></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label5" runat="server" Text="Can Create Country Specific products"></asp:Label></td>
                  <td class="uga">
                    <asp:CheckBox ID="cbEditCP" runat="server" /></td>
                </tr>
                <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label6" runat="server" Text="Translation is driven by PLC"></asp:Label></td>
                  <td class="ugd">
                    <asp:CheckBox ID="cbTranslationPLC" runat="server" /></td>
                </tr>
                 <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label9" runat="server" Text="Fall Back To English"></asp:Label></td>
                  <td class="ugd">
                    <asp:CheckBox ID="cbFallBackToEnglish" runat="server" /></td>
                </tr>
                 <tr valign="middle">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label10" runat="server" Text="Publishable"></asp:Label></td>
                  <td class="ugd">
                    <asp:CheckBox ID="cbPublishable" runat="server" /></td>
                </tr>
                <tr id="Tr1" valign="middle" visible="false" runat="server">
                  <td class="editLabelCell" style="width: 130px">
                    <asp:Label ID="Label8" runat="server" Text="IsActive"></asp:Label></td>
                  <td class="uga">
                    <asp:CheckBox ID="cbIsActive" Checked="true" runat="server" /></td>
                </tr>
              </table>
              <tr valign="top">
                <td colspan="2" class="sectionTitle">
                  Passive approval trigger
      <tr>
        <td>
          <%--<igtbl:UltraWebGrid ID="dgPVGrid" runat="server" Width="100%" OnInitializeRow="dgPVGrid_InitializeRow">
            <DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="OnClient" RowHeightDefault="100%"
              Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
              AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
              CellClickActionDefault="RowSelect" NoDataMessage="No region defined" LoadOnDemand="Manual" ViewType="Hierarchical">
              <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                </BorderDetails>
              </HeaderStyleDefault>
              <FrameStyle Width="100%" CssClass="dataTable">
              </FrameStyle>
              <SelectedRowStyleDefault CssClass="ugs">
              </SelectedRowStyleDefault>
              <RowAlternateStyleDefault CssClass="uga">
              </RowAlternateStyleDefault>
              <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
              </RowStyleDefault>
            </DisplayLayout>
            <Bands>
             <igtbl:UltraGridBand Key="Class" Indentation="15">
              <Columns>
                <igtbl:UltraGridColumn BaseColumnName="ClassId" Hidden="True" Key="ClassId">
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn BaseColumnName="ProductTypeName" HeaderText="ProductTypeName" Key="ProductTypeName" Width="315px">
                </igtbl:UltraGridColumn>
              </Columns>
              <RowStyle CssClass="ugd" BackColor="WhiteSmoke" />    
              <HeaderStyle Height="22px" />
            </igtbl:UltraGridBand>
              <igtbl:UltraGridBand Key="Approval" BorderCollapse="Collapse" DataKeyField="ClassId">
                <Columns>
                  <igtbl:UltraGridColumn Key="ClassId" ServerOnly="true" BaseColumnName="Id">
                  </igtbl:UltraGridColumn>
                 <igtbl:UltraGridColumn Key="PassiveApprovalDay">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="ActiveApproval">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn Key="PLCode">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Name" Key="ClassName" BaseColumnName="Name" Width="300">
                    <CellStyle CssClass="ptb1"/>
                  </igtbl:UltraGridColumn>
                   <igtbl:UltraGridColumn HeaderText="PLCode" Key="PLCode" BaseColumnName="" Width="300">
                    <CellStyle CssClass="ptb1"/>
                  </igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn HeaderText="Trigger" Key="PvTrigger" Width="80">
                    <CellTemplate>
                      <igtxt:WebNumericEdit Enabled="true" Width="78" ID="wTrigger" runat="server" DataMode="Int"
                        NullText="0" MinValue="0" MaxValue="100" MaxLength="2">
                      </igtxt:WebNumericEdit>
                    </CellTemplate>
                  </igtbl:TemplatedColumn>
                  <igtbl:TemplatedColumn Key="pvActive" BaseColumnName="" HeaderText="Active Approval"
                    Width="120">
                    <CellStyle VerticalAlign="Top" HorizontalAlign="Center" Wrap="True">
                    </CellStyle>
                    <CellTemplate>
                      <asp:CheckBox ID="cbActive" runat="server"></asp:CheckBox>
                    </CellTemplate>
                  </igtbl:TemplatedColumn>
                </Columns>
              </igtbl:UltraGridBand>
            </Bands>
          </igtbl:UltraWebGrid>--%>
          <igtbl:UltraWebGrid ID="dgPVGrid" runat="server" OnInitializeLayout ="dgPVGrid_InitializeLayout" DisplayLayout-AllowUpdateDefault="Yes">
        <Bands>
            <igtbl:UltraGridBand>
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
                <FilterOptions AllString="" EmptyString="" NonEmptyString="">
                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                        CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                        Font-Size="11px" Width="200px">
                        <Padding Left="2px" />
                    </FilterDropDownStyle>
                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="#FFFFFF">
                    </FilterHighlightRowStyle>
                </FilterOptions>
            </igtbl:UltraGridBand>
        </Bands>
        <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" AllowDeleteDefault="Yes"
            AllowSortingDefault="OnClient" AllowUpdateDefault="Yes" BorderCollapseDefault="Separate"
            HeaderClickActionDefault="SortMulti" Name="UltraWebGrid2" RowHeightDefault="20px"
            RowSelectorsDefault="No" SelectTypeRowDefault="Extended" Version="4.00" ViewType="Hierarchical">
            <GroupByBox>
                <Style BackColor="ActiveBorder" BorderColor="Window"></Style>
            </GroupByBox>
            <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
            </GroupByRowStyleDefault>
            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
            </FooterStyleDefault>
            <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                <BorderDetails ColorLeft="Window" ColorTop="Window" />
                <Padding Left="3px" />
            </RowStyleDefault>
            <FilterOptionsDefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                    CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                    Font-Size="11px" Width="200px">
                    <Padding Left="2px" />
                </FilterDropDownStyle>
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="#FFFFFF">
                </FilterHighlightRowStyle>
            </FilterOptionsDefault>
            <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid" HorizontalAlign="Left">
                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
            </HeaderStyleDefault>
            <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
            </EditCellStyleDefault>
            <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid"
                BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Height="200px"
                Width="325px">
            </FrameStyle>
            <Pager>
                <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
            </Pager>
            <AddNewBox Hidden="False">
                <Style BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
            </AddNewBox>
        </DisplayLayout>
    </igtbl:UltraWebGrid>

    </asp:Panel>
  </table>
</asp:Content>
