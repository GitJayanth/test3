<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Links_list"
    CodeFile="Links_list.aspx.cs" %>

<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
    Compatibilities</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">
    <script type="text/javascript" language="javascript">
        var winLinkAdd = null;

        function ReloadPage() {
            if (document)
                document.location.reload();
        }

        function DisplayLinkFromOrLinkTo(itemId, linkTypeId, linkFromTo) {
            if (document)
                document.location = "Links_list.aspx?i=" + itemId + "&t=" + linkTypeId + "&f=" + linkFromTo;
        }

        function AddLink(itemId, linkFromTo, linkTypeId) {
            var url = "Links_add.aspx?i=" + itemId + "&t=" + linkTypeId + "&f=" + linkFromTo;
            winLinkAdd = OpenModalWindow(url, "lnk", 600, 500, "yes");
        }

        function uwToolbar_Click(oToolbar, oButton, oEvent) {
            if (oButton.Key == 'Add') {
                oEvent.cancelPostBack = true;
                AddLink(itemId, bLinkFrom, lType);
            }
            //Nisha verma commented the alert message to not to show the message when user hasnt selected anything and clicking on delete button(start)
                if (oButton.Key == 'Delete') 
                {
                    var check = false;
                    myForm = document.forms[0];
                    for (i = 0; i < myForm.length; i++)
                    {
                        if ((myForm.elements[i].id.indexOf('g_sd') != -1)) 
                        {
                         
                         if (!myForm.elements[i].disabled && myForm.elements[i].checked) 
                         {
                            check = true;
                            break;
                        }
                    }
                }
                if (check) 
                {
                    oEvent.cancelPostBack = !confirm("Are you sure you want to remove this link ?");
                }
           }
            //Nisha verma commented the alert message to not to show the message when user hasnt selected anything and clicking on delete button(end)  
                                                

            if (oButton.Key == 'ResumeInheritance')
                oEvent.cancelPostBack = !confirm("ResumeInheritance will delete all the localized links for this SKU. Are you sure you want to perform this opertaion?");
        }

        function RefreshTabs(key1, v1, key2, v2) {
            try {
                var webTab = parent.igtab_getTabById('webTab');
                if (webTab == null) {
                    webTab = parent.igtab_getTabById('ctl00_HOCP_webTab'); // Master Pages since Migration to .Net 2.0
                }
                var s = '';

                var tab1 = webTab.Tabs[key1];
                if (tab1) {
                    s = tab1.getText();
                    if (s.indexOf('(') > 0) {
                        s = s.substring(0, s.indexOf('('));
                    }
                    s = s + '(' + v1 + ')';
                    tab1.setText(s);
                }
                var tab2 = webTab.Tabs[key2];
                if (tab2) {
                    s = tab2.getText();
                    if (s.indexOf('(') > 0) {
                        s = s.substring(0, s.indexOf('('));
                    }
                    s = s + '(' + v2 + ')';
                    tab2.setText(s);
                }
            }
            catch (e) {
                window.status = e.name + ' - ' + e.message;
            }
        }

        function RefreshTabContent(count) {
            try {
                parent.ChangeTabText('tb_Content', count);
            }
            catch (e) {
                window.status = e.name + ' - ' + e.message;
            }
        }
        function UnloadGrid() {
            igtbl_unloadGrid("dg");
        }
    </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
    <script type="text/javascript" language="javascript">
        document.body.onunload = function () { if (winLinkAdd) { winLinkAdd.close(); } };
    </script>
    <%--Removed the width property from ultrawebtoolbar to fix the button issue by Radha S--%>
    <table class="main" style="width: 100%; height: 100%" cellspacing="0" cellpadding="0"
        border="0">
        <tr valign="top" style="height: 1px">
            <td>
                <igtbar:UltraWebToolbar ID="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="120px"
                     OnButtonClicked="uwToolbar_ButtonClicked">
                    <HoverStyle CssClass="hc_toolbarhover">
                    </HoverStyle>
                    <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click">
                    </ClientSideEvents>
                    <SelectedStyle CssClass="hc_toolbarselected">
                    </SelectedStyle>
                    <Items>
                    
                        <%--Code modified for Links Requirement - Links Tab Grid Format (PR668013) by Prachi on 10th Dec 2012 - start--%>
                        <%--<igtbar:TBLabel Text="Hardware list" Key="Title">
							<DefaultStyle Width="100px" Font-Bold="True" Height="20px"></DefaultStyle>
						</igtbar:TBLabel>--%>
                        <%--<igtbar:TBSeparator Key="LinkToFromSep"></igtbar:TBSeparator>--%>
                        <%--<igtbar:TBarButton Key="LinkToFrom" Text="Hardware" Image="/hc_v4/img/LinkTo.gif" ToolTip="To display the hardware list"></igtbar:TBarButton>--%>
                        <%--<igtbar:TBSeparator Key="AddSep"></igtbar:TBSeparator>--%>
                        <igtbar:TBarButton Key="Add" Text="Add Companions" Image="/hc_v4/img/ed_new.gif"
                            ToolTip="To add a new link">
                            <%--<DefaultStyle Width="80px"></DefaultStyle>--%>
                            <DefaultStyle Width="120px">
                            </DefaultStyle>
                        </igtbar:TBarButton>
                        <%--Code modified for Links Requirement - Links Tab Grid Format (PR668013) by Prachi on 10th Dec 2012 - end--%>
                        <igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton Key="Save" Text="Apply changes" Image="/hc_v4/img/ed_save.gif"
                            ToolTip="To save the new exclusions and/or new sort and/or the new DS recommended links">
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton Key="Delete" Text="Delete selected" Image="/hc_v4/img/ed_delete.gif"
                            ToolTip="To delete selected links">
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="ExportSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"
                            ToolTip="Export content in Excel">
                            <DefaultStyle Width="80px">
                            </DefaultStyle>
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="NonInheritedSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton ToggleButton="True" Key="NonInherited" ToolTip="To display only the non inherited links"
                            Image="/hc_v4/img/ed_links.gif" Text="Specific links">
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="DSRLinksSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton ToggleButton="True" Key="DSRLinks" ToolTip="To display only the DSR Links and modify sort order."
                            Image="/hc_v4/img/ed_OK.gif" Text="DSR Links">
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="SCRLinksSep"></igtbar:TBSeparator>
<igtbar:TBarButton ToggleButton="True" Key="SCRLinks" ToolTip="To display only the SCR Links and modify sort order." Image="/hc_v4/img/ed_OK.gif" Text="SCR Links"></igtbar:TBarButton>
                        <igtbar:TBSeparator Key="AtCountryLevelSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton ToggleButton="True" Key="AtCountryLevel" ToolTip="To display only all links at country levels"
                            Image="/hc_v4/img/ed_links.gif" Text="At country level">
                        </igtbar:TBarButton>
                        <igtbar:TBSeparator Key="ResumeInheritanceSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton Key="ResumeInheritance" ToolTip="To delete all the local links (eligible &amp; ineligible links) and resume inheritance"
                            Text="Resume Inheritance">
                        </igtbar:TBarButton>
                        <%--Code added for Links Requirement - Links Tab Grid Format (PR668013) by Prachi on 10th Dec 2012--%>
                        <igtbar:TBSeparator Key="LinkToFromSep"></igtbar:TBSeparator>
                        <igtbar:TBarButton Key="LinkToFrom" Text="List Hosts" ToolTip="To display the hosts list">
                        </igtbar:TBarButton>
                        <%--Code added for Links Requirement - Links Tab Grid Format (PR668013) by Prachi on 10th Dec 2012 - end--%>
                    </Items>
                    <DefaultStyle CssClass="hc_toolbardefault">
                    </DefaultStyle>
                </igtbar:UltraWebToolbar>
            </td>
        </tr>
        <tr valign="top" style="height: 1px">
            <td>
                <asp:Label ID="lbError" runat="server" CssClass="hc_error" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr valign="top" style="height: 1px">
            <td align="center">
                <asp:Label ID="lbResult" runat="server" CssClass="hc_success" Visible="false">No result</asp:Label>
            </td>
        </tr>
        <%--Code added for Links Requirement - Links Tab Grid Format (PR668013) by Prachi on 10th Dec 2012 - start--%>
        <tr id="tr1" runat="server" valign="middle" align="center" class="ptbgroup" style="height: 1px">
            <td id="tdList" runat="server" class="ptbgroup" title="Companions List">
            </td>
        </tr>
        <%--Code added for Links Requirement - Links Tab Grid Format (PR668013) by Prachi on 10th Dec 2012 - end--%>
        <tr valign="top" style="height: auto">
            <td>
                <div style="overflow: auto; width: 100%; height: 100%">
                    <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%" UseAccessibleHeader="False"
                        OnInitializeRow="dg_InitializeRow">
                        <%--AllowSortingDefault property modified to "Yes" for Links Requirement (PR668013) - Links Tab Grid Format by Prachi--%>
                        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
                            HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
                            CellClickActionDefault="RowSelect" NoDataMessage="No links available">
                            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh2">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                </BorderDetails>
                            </HeaderStyleDefault>
                            <FrameStyle Width="100%" CssClass="dataTable">
                            </FrameStyle>
                            <RowAlternateStyleDefault CssClass="uga">
                            </RowAlternateStyleDefault>
                            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                            </RowStyleDefault>
                            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
                                InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
                        </DisplayLayout>
                        <Bands>
                            <igtbl:UltraGridBand BaseTableName="Links" Key="Links" BorderCollapse="Collapse">
                                <Columns>
                                    <igtbl:UltraGridColumn Key="LinkId" BaseColumnName="LinkId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="SubItemId" BaseColumnName="SubItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="SubItemSort" BaseColumnName="SubItemSort" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="InheritedItemId" BaseColumnName="InheritedItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="ItemExcluded" BaseColumnName="ExcludedItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="CountryExcluded" BaseColumnName="ExcludedCountryCode"
                                        ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="UserId" BaseColumnName="UserId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="Family" BaseColumnName="FamilyName" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="CountryCode" BaseColumnName="CountryCode" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="FallbackCountryCode" BaseColumnName="FallbackCountryCode"
                                        ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="IsInherited" BaseColumnName="Inherited" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:TemplatedColumn Key="Select" Width="20px">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="True">
                                        </CellStyle>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="g_ca" onclick="javascript: return g_su(this);" runat="server">
                                            </asp:CheckBox>
                                        </HeaderTemplate>
                                        <CellTemplate>
                                            <asp:CheckBox ID="g_sd" onclick="javascript: return g_su(this);" runat="server">
                                            </asp:CheckBox>
                                        </CellTemplate>
                                    </igtbl:TemplatedColumn>
                                    <igtbl:UltraGridColumn HeaderText="Class" Key="Class" Width="100px" BaseColumnName="ClassName"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Sku" Key="ItemSku" Width="100%" BaseColumnName="ItemSKU"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" Width="200px" BaseColumnName="ItemName"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <%--Added for Links Requirement (PR668013) - Links Tab Grid Format to add Effective Date column by Prachi--%>
                                    <igtbl:UltraGridColumn HeaderText="Effective Date" Key="EffectiveDate" BaseColumnName="EffectiveDate"
                                        DataType="System.DateTime">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <%--Added for Links Requirement (PR668013) - Links Tab Grid Format to add Effective Date column by Prachi - end--%>
                                    <igtbl:UltraGridColumn HeaderText="Level" Key="InheritedLevel" Width="80px" BaseColumnName="InheritedLevel">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="ImageCountry" Width="60px" HeaderText="Catalog">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="E" Key="IsExcluded" Width="22px" Type="CheckBox"
                                        DataType="System.Boolean" BaseColumnName="Excluded" AllowUpdate="Yes">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                        </CellStyle>
                                        <Header Caption="E" Title="Excluded">
                                        </Header>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="DSR" Key="IsRecommended" Width="30px" Type="CheckBox"
                                        DataType="System.Boolean" BaseColumnName="Recommended" AllowUpdate="Yes">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                        </CellStyle>
                                        <Header Caption="DSR" Title="DS Recommended">
                                        </Header>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="SCR" Key="IsSCRecommended" Width="30px" Type="CheckBox" DataType="System.Boolean" BaseColumnName="SCRecommended" AllowUpdate="Yes">
<CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
</CellStyle><Header Caption="SCR" Title="SC Recommended"></Header>
</igtbl:UltraGridColumn>
                                </Columns>
                            </igtbl:UltraGridBand>
                        </Bands>
                    </igtbl:UltraWebGrid>
                    <igtbl:UltraWebGrid ID="dgNoBidi" runat="server" Width="100%" OnInitializeRow="dg_InitializeRow">
                        <%--AllowSortingDefault property added for Links Requirement (PR668013) - Links Tab Grid Format by Prachi--%>
                        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" HeaderClickActionDefault="SortSingle"
                            RowSelectorsDefault="No" CellClickActionDefault="RowSelect" Name="dgNoBidi" TableLayout="Fixed"
                            NoDataMessage="No data to display" AllowSortingDefault="Yes">
                            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
                                <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                </BorderDetails>
                            </HeaderStyleDefault>
                            <FrameStyle Width="100%" CssClass="dataTable">
                            </FrameStyle>
                            <RowAlternateStyleDefault CssClass="uga">
                            </RowAlternateStyleDefault>
                            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                            </RowStyleDefault>
                            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
                                InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
                        </DisplayLayout>
                        <Bands>
                            <igtbl:UltraGridBand BaseTableName="Links" Key="Links" BorderCollapse="Collapse">
                                <Columns>
                                    <igtbl:UltraGridColumn Key="ItemId" BaseColumnName="ItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="SubItemId" BaseColumnName="SubItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="InheritedItemId" BaseColumnName="InheritedItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="IsInherited" BaseColumnName="Inherited" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="ItemExcluded" BaseColumnName="ExcludedItemId" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="CountryExcluded" BaseColumnName="ExcludedCountryCode"
                                        ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="UserId" BaseColumnName="UserId" Hidden="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="CountryCode" BaseColumnName="CountryCode" ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="FallbackCountryCode" BaseColumnName="FallbackCountryCode"
                                        ServerOnly="True">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:TemplatedColumn Key="Select" Width="20px">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="True">
                                        </CellStyle>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="g_ca" onclick="javascript: return g_su(this);" runat="server">
                                            </asp:CheckBox>
                                        </HeaderTemplate>
                                        <CellTemplate>
                                            <asp:CheckBox ID="g_sd" onclick="javascript: return g_su(this);" runat="server">
                                            </asp:CheckBox>
                                        </CellTemplate>
                                    </igtbl:TemplatedColumn>
                                    <igtbl:UltraGridColumn HeaderText="Class" Key="Class" Width="100px" BaseColumnName="SubClassName"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Family" Key="Family" Width="100px" BaseColumnName="SubFamilyName"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Sku" Key="ItemSku" Width="100%" BaseColumnName="SubItemSKU"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" Width="200px" BaseColumnName="SubItemName"
                                        CellMultiline="Yes">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <%--Added for Links Requirement (PR668013) - Links Tab Grid Format to add Effective Date column by Prachi--%>
                                    <igtbl:UltraGridColumn HeaderText="Effective Date" Key="EffectiveDate" BaseColumnName="EffectiveDate"
                                        DataType="System.DateTime">
                                        <CellStyle Wrap="true">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <%--Added for Links Requirement (PR668013) - Links Tab Grid Format to add Effective Date column by Prachi - end--%>
                                    <igtbl:UltraGridColumn HeaderText="Level" Key="InheritedLevel" Width="80px" BaseColumnName="InheritedLevel">
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn Key="ImageCountry" Width="60px" HeaderText="Catalog">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                        </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="E" Key="IsExcluded" Width="22px" Type="CheckBox"
                                        DataType="System.Boolean" BaseColumnName="Excluded" AllowUpdate="Yes">
                                        <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                                        </CellStyle>
                                        <Header Caption="E" Title="Excluded">
                                        </Header>
                                    </igtbl:UltraGridColumn>
                                </Columns>
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
                    </igtbl:UltraWebGrid>
                </div>
            </td>
        </tr>
        <tr valign="bottom" style="height: 1px">
            <td>
                <div id="divToolbar">
                    <table class="hc_toolbartitle" style="height: 16px; width: 100%" cellspacing="0"
                        cellpadding="0" border="0">
                        <tr style="height: 100%">
                            <td>
                                <img alt="Inherited links" src="/hc_v4/img/ed_links_disabled.gif" />&nbsp;
                                <asp:Label ID="lbInheritedLnk" runat="server">Inherited link count : 0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <img alt="Specific links" src="/hc_v4/img/ed_links.gif" />&nbsp;
                                <asp:Label ID="lbSpecificLnk" runat="server">Specifiec link count : 0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbInhRCount" runat="server">DS Recommended inherited link count : 0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbNotInhRCount" runat="server">DS Recommended specifc link count : 0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbInhSCount" runat="server">SC Recommended inherited link count : 0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbNotInhSCount" runat="server">SC Recommended specifc link count : 0</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" name="action" />
    <input type="hidden" name="txtSortColPos" id="txtSortColPos" runat="server" value="16" />
</asp:Content>
