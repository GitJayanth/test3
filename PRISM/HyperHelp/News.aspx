<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Page Language="vb" AutoEventWireup="false" Inherits="HyperHelp.News"
  CodeFile="News.aspx.vb" %>

<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
  <title>News</title>
  <link href="css/hyperhelp.css" rel="stylesheet" />

  <script>
		  function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'Delete') {
          oEvent.cancelPostBack = !confirm("Are you sure?");
        }
		  } 
  </script>

</head>
<body class="hh_body">
  <form id="Form1" method="post" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
      <tr>
        <td class="hh_Title">
          <asp:Label ID="lbTitle" runat="server">News</asp:Label>
        </td>
      </tr>
      <asp:Panel ID="pnNews" Width="100%" runat="server" Visible="True">
        <tr valign="top">
          <td>
            <igtbar:UltraWebToolbar ID="uwToolBar" runat="server" CssClass="hh_toolbar" ItemWidthDefault="80px">
                          <HoverStyle CssClass="hh_toolbarhover">
              </HoverStyle>
              <DefaultStyle CssClass="hh_toolbardefault">
              </DefaultStyle>
              <SelectedStyle CssClass="hh_toolbarselected">
              </SelectedStyle>
              <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
              <Items>
                <igtbar:TBarButton Key="Add" Text="Add" Image="img/ed_new.gif">
                </igtbar:TBarButton>
              </Items>
            </igtbar:UltraWebToolbar>
          </td>
        </tr>
        <tr valign="top">
          <td>
            <p>
              <igtbl:UltraWebGrid ID="dgNews" runat="server">
                <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" Version="4.00" RowSelectorsDefault="No"
                  Name="dgNews" TableLayout="Fixed" NoDataMessage="No data to display">
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
                      <igtbl:UltraGridColumn Key="Id" Hidden="True" BaseColumnName="NewsId">
                        <Footer Key="Id">
                        </Footer>
                        <Header Key="Id">
                        </Header>
                      </igtbl:UltraGridColumn>
                      <igtbl:TemplatedColumn Key="Title" Width="200px" HeaderText="Name" BaseColumnName="Title"
                        CellMultiline="Yes">
                        <CellStyle Wrap="True">
                        </CellStyle>
                        <CellTemplate>
                          <asp:LinkButton ID="lnkEdit" OnClick="UpdateGridItem" runat="server">
															<%#DataBinder.Eval(Container, "Text")%>
                          </asp:LinkButton>
                        </CellTemplate>
                        <Header Key="Name" Caption="Name">
                        </Header>
                      </igtbl:TemplatedColumn>
                      <igtbl:UltraGridColumn HeaderText="Short description" Key="" Width="150px" BaseColumnName="ShortDescription">
                        <CellStyle Wrap="True">
                        </CellStyle>
                        <Footer Key="">
                        </Footer>
                        <Header Key="" Caption="Short description">
                        </Header>
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Description" Key="" Width="100%" BaseColumnName="Description">
                        <CellStyle Wrap="True">
                        </CellStyle>
                        <Footer Key="">
                        </Footer>
                        <Header Key="" Caption="Description">
                        </Header>
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Date" Key="" Width="100px" Format="dd/MM/yyyy"
                        BaseColumnName="Date">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                        <Footer Key="">
                        </Footer>
                        <Header Key="" Caption="Date">
                        </Header>
                      </igtbl:UltraGridColumn>
                      <igtbl:UltraGridColumn HeaderText="Author" Key="" Width="150px" BaseColumnName="Author">
                        <Footer Key="">
                        </Footer>
                        <Header Key="" Caption="Author">
                        </Header>
                      </igtbl:UltraGridColumn>
                    </Columns>
                  </igtbl:UltraGridBand>
                </Bands>
              </igtbl:UltraWebGrid><br>
              <asp:Label ID="lbNoNews" runat="server" CssClass="hh_message">There is no news...</asp:Label>
              <asp:Label ID="lbErrorNews" runat="server" CssClass="hh_errormessage">[ERROR] Please contact the support</asp:Label></p>
          </td>
        </tr>
      </asp:Panel>
      <asp:Panel ID="pnEditNews" Width="100%" runat="server" Visible="False">
        <tr valign="top">
          <td>
            <igtbar:UltraWebToolbar ID="uwToolBar2" runat="server" CssClass="hh_toolbar" ItemWidthDefault="80px">
              <HoverStyle CssClass="hh_toolbarhover">
              </HoverStyle>
              <DefaultStyle CssClass="hh_toolbardefault">
              </DefaultStyle>
              <SelectedStyle CssClass="hh_toolbarselected">
              </SelectedStyle>
              <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
              <Items>
                <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="img/ed_back.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Save" Text="Save" Image="img/ed_save.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
                <igtbar:TBarButton Key="Delete" Text="Delete" Image="img/ed_delete.gif">
                </igtbar:TBarButton>
                <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
              </Items>
            </igtbar:UltraWebToolbar>
            <table cellspacing="0" cellpadding="0" width="100%">
              <tr>
                <td class="hh_cellTitle" width="200">
                  <asp:Label ID="lbTitleNews" runat="server">Title</asp:Label></td>
                <td class="hh_gridCell">
                  <asp:TextBox ID="txtTitle" runat="server" Columns="50" Text="<%# pageTitle %>">
                  </asp:TextBox></td>
              </tr>
              <tr>
                <td class="hh_cellTitle" width="200">
                  <asp:Label ID="lbShortdescription" runat="server">Short description</asp:Label></td>
                <td class="hh_gridCellAlter">
                  <asp:TextBox ID="txtShortdescription" runat="server" Columns="50" Text="<%# shortDescription %>">
                  </asp:TextBox></td>
              </tr>
              <tr>
                <td class="hh_cellTitle" width="200">
                  <asp:Label ID="lbDescription" runat="server">Text</asp:Label></td>
                <td class="hh_gridCell">
                  <asp:TextBox ID="txtdescription" runat="server" Columns="80" Text="<%# description %>"
                    Rows="5" TextMode="MultiLine">
                  </asp:TextBox></td>
              </tr>
              <tr>
                <td width="200">
                </td>
                <td>
                  <asp:Label ID="Label1" runat="server" CssClass="smallgray" Text="<%# author %>">
                  </asp:Label></td>
              </tr>
            </table>
            <asp:Label ID="lbError" runat="server" CssClass="hh_ErrorMessage"></asp:Label>
            <asp:TextBox ID="txtNewsID" runat="server" Visible="False" Text="<%# nId %>">
            </asp:TextBox></td>
        </tr>
      </asp:Panel>
    </table>
  </form>
</body>
</html>
