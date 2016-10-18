<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Page Language="vb" AutoEventWireup="false" Inherits="HyperHelp.TechnicalDocumentation"
  CodeFile="TechnicalDocumentation.aspx.vb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
  <title>TechnicalDocumentation</title>
  <link href="css/HyperHelp.css" rel="stylesheet"/>
</head>
<body class="hh_body">
  <form id="Form1" method="post" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%">
      <!-- PANEL OBJECTS -------------------------------------------------------------------------- -->
      <asp:Panel ID="pObjectsList" runat="server">
        <tbody>
          <tr valign="top">
            <td class="hh_Title">
              <asp:Label ID="Label1" runat="server">Database objects documentation</asp:Label></td>
          </tr>
          <tr>
            <td>
              <igtbar:UltraWebToolbar ID="uwToolBar" runat="server" ImageDirectory=" "  BackgroundImage="/ig_common/images/ig_tb_back03.gif"
                ItemWidthDefault="80px" CssClass="hh_toolbar">
                <HoverStyle CssClass="hh_toolbarhover">
                </HoverStyle>
                <SelectedStyle CssClass="hh_toolbarselected">
                </SelectedStyle>
                <Items>
                  <igtbar:TBLabel Text="Select an object">
                    <DefaultStyle Width="110px" TextAlign="Left">
                    </DefaultStyle>
                  </igtbar:TBLabel>
                  <igtbar:TBCustom Width="150px">
                    <asp:DropDownList runat="server" DataTextField="Name" DataValueField="Type" Width="150px" AutoPostBack="True"
                      CssClass="hh_dropdown" ID="dpObjectsList" OnSelectedIndexChanged="OnSelChange">
                    </asp:DropDownList>
                  </igtbar:TBCustom>
                  <igtbar:TBarButton Key="Report" Text="Report" Visible="False" Image="img/report.gif">
                  </igtbar:TBarButton>
              
                  <igtbar:TBarButton Key="reIndex" Visible="False" Text="reIndex" Image="img/reindex.gif">
                  </igtbar:TBarButton>
                 
                  <igtbar:TBarButton Key="refreshDependence" Visible="False" Text="Refresh dependence"
                    Image="img/refresh.gif">
                    <DefaultStyle Width="170px">
                    </DefaultStyle>
                  </igtbar:TBarButton>
                  
                  <igtbar:TBLabel Text=" " Key="lbResult">
                    <DefaultStyle Width="350px" TextAlign="Left">
                    </DefaultStyle>
                  </igtbar:TBLabel>
                </Items>
                <DefaultStyle CssClass="hh_toolbardefault">
                </DefaultStyle>
              </igtbar:UltraWebToolbar>
            </td>
          </tr>
          <tr>
            <td>
              <asp:Panel ID="pDBObjects" runat="server">
                <igtbl:UltraWebGrid ID="gridDBObject" runat="server">
                  <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                    Name="gridDBObject" TableLayout="Fixed" NoDataMessage="No data to display">
             
                     <HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" >
                      <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                      </BorderDetails>
                    </HeaderStyleDefault>
                    <FrameStyle CssClass="hh_dataTable">
                    </FrameStyle>
                    <ActivationObject AllowActivation="False">
                    </ActivationObject>
                    <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                    </RowAlternateStyleDefault>
                    <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                    </RowStyleDefault>
                  </DisplayLayout>
                  <Bands>
                    <igtbl:UltraGridBand>
                      <Columns>
                        <igtbl:TemplatedColumn Key="Name" Width="300px" HeaderText="Name" BaseColumnName="Name"
                          CellMultiline="Yes">
                          <CellTemplate>
                            <asp:LinkButton ID="lnkEdit" OnClick="UpdateGridItem" runat="server">
																<%#DataBinder.Eval(Container, "Text")%>
                            </asp:LinkButton>
                          </CellTemplate>
                          <Footer Key="Name">
                          </Footer>
                          <Header Key="Name" Caption="Name">
                          </Header>
                        </igtbl:TemplatedColumn>
                        <igtbl:UltraGridColumn HeaderText="Dependence" Key="Dependence" Width="100px" BaseColumnName="Dependence">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Footer Key="Dependence">
                          </Footer>
                          <Header Key="Dependence" Caption="Dependence">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Function" Key="objFunction" Width="150px" BaseColumnName="objFunction">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Footer Key="objFunction">
                          </Footer>
                          <Header Key="objFunction" Caption="Function">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Description" Key="description" Width="100%" BaseColumnName="Description">
                          <CellStyle Wrap="True">
                          </CellStyle>
                          <Footer Key="description">
                          </Footer>
                          <Header Key="description" Caption="Description">
                          </Header>
                        </igtbl:UltraGridColumn>
                      </Columns>
                    </igtbl:UltraGridBand>
                  </Bands>
                </igtbl:UltraWebGrid>
              </asp:Panel>
              <asp:Panel ID="pJobs" runat="server">
                <igtbl:UltraWebGrid ID="gridJobs" runat="server" Width="100%">
                  <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                    Name="gridJobs" TableLayout="Fixed" NoDataMessage="No data to display">
                    <HeaderStyleDefault VerticalAlign="Middle" CssClass="hh_gridHeader">
                      <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                      </BorderDetails>
                    </HeaderStyleDefault>
                    <FrameStyle CssClass="hh_dataTable">
                    </FrameStyle>
                    <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                    </RowAlternateStyleDefault>
                    <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                    </RowStyleDefault>
                  </DisplayLayout>
                  <Bands>
                    <igtbl:UltraGridBand>
                      <Columns>
                        <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="Name">
                          <Footer Key="Name">
                          </Footer>
                          <Header Key="Name" Caption="Name">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Status" Key="Status" Width="100px" BaseColumnName="Status">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Footer Key="Status">
                          </Footer>
                          <Header Key="Status" Caption="Status">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Last run" Key="Lastrun" Width="100px" BaseColumnName="Lastrun">
                          <Footer Key="Lastrun">
                          </Footer>
                          <Header Key="Lastrun" Caption="Last run">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Next run" Key="Nextrun" Width="100px" BaseColumnName="Nextrun">
                          <Footer Key="Nextrun">
                          </Footer>
                          <Header Key="Nextrun" Caption="Next run">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Planified" Key="Planified" Width="80px" Type="CheckBox"
                          BaseColumnName="Planified">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Footer Key="Planified">
                          </Footer>
                          <Header Key="Planified" Caption="Planified">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Activate" Key="Activate" Width="80px" Type="CheckBox"
                          BaseColumnName="Activate">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Footer Key="Activate">
                          </Footer>
                          <Header Key="Activate" Caption="Activate">
                          </Header>
                        </igtbl:UltraGridColumn>
                      </Columns>
                    </igtbl:UltraGridBand>
                  </Bands>
                </igtbl:UltraWebGrid>
              </asp:Panel>
              <asp:Panel ID="pFiles" runat="server">
                <igtbl:UltraWebGrid ID="gridFiles" runat="server" Width="100%">
                  <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                    Name="gridFiles" TableLayout="Fixed" NoDataMessage="No data to display">
                    <HeaderStyleDefault VerticalAlign="Middle" CssClass="hh_gridHeader">
                      <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                      </BorderDetails>
                    </HeaderStyleDefault>
                    <FrameStyle CssClass="hh_dataTable">
                    </FrameStyle>
                    <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                    </RowAlternateStyleDefault>
                    <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                    </RowStyleDefault>
                  </DisplayLayout>
                  <Bands>
                    <igtbl:UltraGridBand>
                      <Columns>
                        <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="300px" BaseColumnName="Name">
                          <Header Key="Name" Caption="Name">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Function" Key="objFunction" Width="100px" BaseColumnName="objFunction">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Header Key="objFunction" Caption="Function">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description">
                          <CellStyle HorizontalAlign="Center" Wrap="True">
                          </CellStyle>
                          <Header Key="Description" Caption="Description">
                          </Header>
                        </igtbl:UltraGridColumn>
                      </Columns>
                    </igtbl:UltraGridBand>
                  </Bands>
                </igtbl:UltraWebGrid>
              </asp:Panel>
              <asp:Panel ID="pDlls" runat="server">
                <igtbl:UltraWebGrid ID="gridDlls" runat="server" Width="100%">
                  <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                    Name="gridDlls" TableLayout="Fixed" NoDataMessage="No data to display">
                    <HeaderStyleDefault VerticalAlign="Middle" CssClass="hh_gridHeader">
                      <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                      </BorderDetails>
                    </HeaderStyleDefault>
                    <FrameStyle CssClass="hh_dataTable">
                    </FrameStyle>
                    <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                    </RowAlternateStyleDefault>
                    <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                    </RowStyleDefault>
                  </DisplayLayout>
                  <Bands>
                    <igtbl:UltraGridBand>
                      <Columns>
                        <igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="300px" BaseColumnName="Name">
                          <Header Key="Name" Caption="Name">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Function" Key="objFunction" Width="100px" BaseColumnName="objFunction">
                          <CellStyle HorizontalAlign="Center">
                          </CellStyle>
                          <Header Key="objFunction" Caption="Function">
                          </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description">
                          <CellStyle HorizontalAlign="Center" Wrap="True">
                          </CellStyle>
                          <Header Key="Description" Caption="Description">
                          </Header>
                        </igtbl:UltraGridColumn>
                      </Columns>
                    </igtbl:UltraGridBand>
                  </Bands>
                </igtbl:UltraWebGrid>
              </asp:Panel>
            </td>
          </tr>
      </asp:Panel>
      <asp:Panel ID="pObjectDetails" runat="server">
        <tr valign="top">
          <td class="hh_Title">
            <asp:Label ID="title" runat="server">Application objects documentation - </asp:Label></td>
        </tr>
        <tr>
          <td>
            <igtbar:UltraWebToolbar ID="uwToolBarObjDetail" runat="server" ItemWidthDefault="80px"
              CssClass="hh_toolbar">
              <HoverStyle CssClass="hh_toolbarhover">
              </HoverStyle>
              <DefaultStyle CssClass="hh_toolbardefault">
              </DefaultStyle>
              <SelectedStyle CssClass="hh_toolbarselected">
              </SelectedStyle>
              <Items>
                <igtbar:TBarButton Key="List" Text="List" Image="img/ed_back.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator Key="Separator1"></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Save" Text="Save" Image="img/ed_save.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator Key="Separator2"></igtbar:TBSeparator>
                <igtbar:TBLabel Text="" Key="lbResultObj">
                  <DefaultStyle TextAlign="Left" Width="200px">
                  </DefaultStyle>
                </igtbar:TBLabel>
              </Items>
            </igtbar:UltraWebToolbar>
          </td>
        </tr>
        <tr>
          <td>
            <br>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
              <tr>
                <td class="hh_cellTitle">
                  <asp:Label ID="Label10" runat="server">Name</asp:Label></td>
                <td class="hh_gridCell">
                  <asp:TextBox ID="txtName" runat="server" ReadOnly="true" Enabled="False"></asp:TextBox></td>
              </tr>
              <tr>
                <td class="hh_cellTitle">
                  <asp:Label ID="Label2" runat="server">Type</asp:Label></td>
                <td class="hh_gridCellAlter">
                  <asp:TextBox ID="txtType" runat="server" ReadOnly="true" Enabled="False"></asp:TextBox></td>
              </tr>
              <tr>
                <td class="hh_cellTitle">
                  <asp:Label ID="Label3" runat="server">Description</asp:Label></td>
                <td class="hh_gridCell">
                  <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" Rows="4" Columns="60"></asp:TextBox></td>
              </tr>
              <tr>
                <td class="hh_cellTitle">
                  <asp:Label ID="Label4" runat="server">Function</asp:Label></td>
                <td class="hh_gridCellAlter">
                  <asp:DropDownList ID="lboxFunction" runat="server">
                  </asp:DropDownList></td>
              </tr>
              <tr>
                <td class="hh_cellTitle">
                  <asp:Label ID="lTempTable" runat="server">Temporary table</asp:Label></td>
                <td class="hh_gridCell">
                  <asp:CheckBox ID="cboxSecondarytable" runat="server"></asp:CheckBox></td>
              </tr>
            </table>
            <br>
            <asp:Panel ID="Panel1" runat="server">
              <table id="gridDependance" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                  <asp:Panel ID="pFields" runat="server">
                    <td valign="top" width="35%">
                      <asp:Label ID="Label5" runat="server" CssClass="hh_subTitle" Width="100%">Fields</asp:Label><br>
                      <igtbl:UltraWebGrid ID="gridStruture" runat="server">
                        <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                          Name="gridStruture" TableLayout="Fixed" NoDataMessage="No data to display">
                      	<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
                          <FrameStyle CssClass="dataTable">
                          </FrameStyle>
                          <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                          </RowAlternateStyleDefault>
                          <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                          </RowStyleDefault>
                        </DisplayLayout>
                        <Bands>
                          <igtbl:UltraGridBand>
                            <Columns>
                              <igtbl:UltraGridColumn HeaderText="FieldName" Key="FieldName" Width="100%" BaseColumnName="FieldName">
                                <Header Caption="FieldName">
                                </Header>
                              </igtbl:UltraGridColumn>
                              <igtbl:UltraGridColumn HeaderText="Type" Key="Type" Width="100px" BaseColumnName="Type">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <Header Caption="Type">
                                </Header>
                              </igtbl:UltraGridColumn>
                              <igtbl:UltraGridColumn HeaderText="Length" Key="Length" Width="100px" BaseColumnName="Length">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <Header Caption="Length">
                                </Header>
                              </igtbl:UltraGridColumn>
                            </Columns>
                          </igtbl:UltraGridBand>
                        </Bands>
                      </igtbl:UltraWebGrid></td>
                    <td width="20">
                    </td>
                  </asp:Panel>
                  <td valign="top">
                    <asp:Label ID="lDependence" runat="server" CssClass="hh_subTitle" Width="100%">Dependence</asp:Label><br>
                    <table cellspacing="0" cellpadding="0">
                      <tr>
                        <asp:Panel ID="pGridDBObjDep1" runat="server">
                          <td valign="top" width="350">
                            <igtbl:UltraWebGrid ID="gridDBObjDep1" runat="server">
                              <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                                Name="gridDBObjDep1" TableLayout="Fixed" NoDataMessage="No data to display">
                                <HeaderStyleDefault VerticalAlign="Middle" CssClass="hh_gridHeader">
                                  <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
                                  </BorderDetails>
                                </HeaderStyleDefault>
                                <FrameStyle CssClass="hh_dataTable">
                                </FrameStyle>
                                <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                                </RowAlternateStyleDefault>
                                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                                </RowStyleDefault>
                              </DisplayLayout>
                              <Bands>
                                <igtbl:UltraGridBand>
                                  <Columns>
                                    <igtbl:UltraGridColumn Width="25px" BaseColumnName="img">
                                      <CellStyle HorizontalAlign="Center">
                                      </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="depends on" Width="100%" BaseColumnName="DBobj">
                                      <CellStyle HorizontalAlign="Left" Wrap="True">
                                      </CellStyle>
                                      <Header Caption="depends on">
                                      </Header>
                                    </igtbl:UltraGridColumn>
                                  </Columns>
                                </igtbl:UltraGridBand>
                              </Bands>
                            </igtbl:UltraWebGrid></td>
                          <td width="20">
                          </td>
                        </asp:Panel>
                        <asp:Panel ID="pGridDBObjDep2" runat="server">
                          <td valign="top" width="400">
                            <igtbl:UltraWebGrid ID="gridDBObjDep2" runat="server">
                              <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                                Name="gridDBObjDep2" TableLayout="Fixed" NoDataMessage="No data to display">
                             	<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
                                <FrameStyle CssClass="hh_dataTable">
                                </FrameStyle>
                                <RowAlternateStyleDefault CssClass="hh_gridCellAlter">
                                </RowAlternateStyleDefault>
                                <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="hh_gridCell">
                                </RowStyleDefault>
                              </DisplayLayout>
                              <Bands>
                                <igtbl:UltraGridBand>
                                  <Columns>
                                    <igtbl:UltraGridColumn Width="25px" BaseColumnName="img">
                                      <CellStyle HorizontalAlign="Center">
                                      </CellStyle>
                                    </igtbl:UltraGridColumn>
                                    <igtbl:UltraGridColumn HeaderText="objects depending on" Width="100%" BaseColumnName="DBobj">
                                      <CellStyle HorizontalAlign="Left" Wrap="True">
                                      </CellStyle>
                                      <Header Caption="objects depending on">
                                      </Header>
                                    </igtbl:UltraGridColumn>
                                  </Columns>
                                </igtbl:UltraGridBand>
                              </Bands>
                            </igtbl:UltraWebGrid></td>
                        </asp:Panel>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </td>
        </tr>
      </asp:Panel>
      <!-- PANEL REPORT -------------------------------------------------------------------------- -->
      <asp:Panel ID="pReport" runat="server">
        <tr valign="top">
          <td class="hh_title" height="20">
            <asp:Label ID="Label8" runat="server">Database objects report</asp:Label></td>
        </tr>
        <tr>
          <td>
            <igtbar:UltraWebToolbar ID="uwToolBarReport" runat="server" Width="100%" ItemWidthDefault="80px"
              CssClass="hh_toolbar">
              <HoverStyle CssClass="hh_toolbarhover">
              </HoverStyle>
              <DefaultStyle CssClass="hh_toolbardefault">
              </DefaultStyle>
              <SelectedStyle CssClass="hh_toolbarselected">
              </SelectedStyle>
              <Items>
                <igtbar:TBarButton Key="List" Text="List" Image="img/ed_back.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBLabel Text="" Key="lberror">
                  <DefaultStyle TextAlign="Left">
                  </DefaultStyle>
                </igtbar:TBLabel>
              </Items>
            </igtbar:UltraWebToolbar>
          </td>
          <tr>
            <td>
              <br>
              <asp:Panel ID="REPORT" runat="server">
              </asp:Panel>
            </td>
          </tr>
      </asp:Panel>
    </table>
  </form>
</body>
</html>
