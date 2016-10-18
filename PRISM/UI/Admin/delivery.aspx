<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Admin.Architecture.Delivery"
  CodeFile="Delivery.aspx.cs" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">
  Delivery</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">

  <script type="text/javascript" language="javascript">
      function uwToolbar_Click(oToolbar, oButton, oEvent){
		    if (oButton.Key == 'filter')
		    {     		     
		      DoSearch();	
          oEvent.cancelPostBack = true;
          return;
        }
      if (typeof(Page_Validators) != 'undefined')
        {
          for( var i = 0; i < Page_Validators.length; ++i) {
            ValidatorEnable( Page_Validators[i], (oButton.Key == 'Save'));
          }
        }
				if (oButton.Key == 'Save') {
					oEvent.cancelPostBack = MandatoryFieldMissing();
				}        
		    if (oButton.Key == 'Delete') {
		      if (dg_nbItems_Checked==0)
		      {
	          alert('You must select at least one item');
	          oEvent.cancelPostBack = true;
	        }
	        else
	        {
            oEvent.cancelPostBack = !confirm("Are you sure you want to delete the selected packages ?");
          }
        }
		  } 
			function cg_up(){
	     var grid = igtbl_getGridById(dgClientId);
       var activeRow = grid.getActiveRow();
			  __doPostBack('groupup', activeRow.getIndex());
			}
			
			function cg_down(){
	     var grid = igtbl_getGridById(dgClientId);
       var activeRow = grid.getActiveRow();
			  __doPostBack('groupdown', activeRow.getIndex());
			}
		function UnloadGrid()
                {
                	igtbl_unloadGrid("dg");
            	}
  </script>

  <table class="main" cellspacing="0" cellpadding="0">
    <tr height="1">
      <td class="sectionTitle" style="height: 1px">
        <asp:Label ID="lbTitle" runat="server">Packages list</asp:Label></td>
    </tr>
    <asp:Panel ID="panelGrid" runat="server">
      <tr valign="top" style="height: 1px">
        <td>
          <igtbar:UltraWebToolbar ID="mainToolBar" runat="server" Width="100%" ItemWidthDefault="80px"
            CssClass="hc_toolbar" ImageDirectory=" ">
            <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
            <HoverStyle CssClass="hc_toolbarhover">
            </HoverStyle>
            <SelectedStyle CssClass="hc_toolbarselected">
            </SelectedStyle>
            <Items>
              <igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Delete" ToolTip="Delete selection" Text="Delete" Image="/hc_v4/img/ed_delete.gif">
              </igtbar:TBarButton>
              <igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
              <igtbar:TBarButton Key="Save" Text="Apply new sort" Image="/hc_v4/img/ed_save.gif">
              <DefaultStyle WIDTH="120px"></DefaultStyle>
              </igtbar:TBarButton>
              <igtbar:TBSeparator></igtbar:TBSeparator>
              <igtbar:TBarButton Key="DAM" ToolTip="Output packages" Text="Outputs" Image="/hc_v4/img/ed_outputtemplate.gif">
              </igtbar:TBarButton>
              <igtbar:TBLabel Text=" ">
                <DefaultStyle Width="100%">
                </DefaultStyle>
              </igtbar:TBLabel>
            </Items>
            <DefaultStyle CssClass="hc_toolbardefault">
            </DefaultStyle>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr valign="top">
        <td>
          <asp:Label ID="mainMsgLbl" runat="server"></asp:Label>
          <igtbl:UltraWebGrid ID="packagesGrid" runat="server" ImageDirectory="/ig_common/Images/"
            Width="100%">
            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
              SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No"
              Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
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
              <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
            </DisplayLayout>
            <Bands>
              <igtbl:UltraGridBand Key="Packages" BorderCollapse="Collapse" DataKeyField="Code">
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
                    <Footer Caption="">
                    </Footer>
                  </igtbl:TemplatedColumn>
                  <igtbl:UltraGridColumn Key="PackageId" Width="25px" HeaderText="Id" BaseColumnName="Id">
                  </igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn HeaderText="Name" Key="Name" Width="100%" BaseColumnName="Name">
                    <CellTemplate>
                      <asp:LinkButton ID="Linkbutton1" OnClick="UpdateGridItem" runat="server">
													<%#Container.Text%>
                      </asp:LinkButton>
                    </CellTemplate>
                  </igtbl:TemplatedColumn>
                  <igtbl:UltraGridColumn HeaderText="Filename" Key="FileName" Width="175px" BaseColumnName="FileName">
                  </igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn HeaderText="Region package" Key="RegionPackage" BaseColumnName="RegionPackage"
                    Width="100px">
                  </igtbl:TemplatedColumn>
                  <igtbl:UltraGridColumn HeaderText="Creator" BaseColumnName="UserName">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Active" Type="CheckBox" width="50px" BaseColumnName="IsActive">
                   <CellStyle HorizontalAlign="center"></CellStyle>
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
          <igtbar:UltraWebToolbar ID="propertiesToolBar" runat="server" Width="100%" ItemWidthDefault="80px"
            CssClass="hc_toolbar">
            <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
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
            </Items>
          </igtbar:UltraWebToolbar>
        </td>
      </tr>
      <tr valign="top">
        <td>
          <asp:Label ID="propertiesMsgLbl" runat="server"></asp:Label>
          <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr valign="middle">
              <td class="editLabelCell" valign="top" width="150">
                Active</td>
              <td class="ugd">
                <asp:CheckBox ID="cbActive" runat="server">
                </asp:CheckBox>
              </td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" valign="top" width="150">
                Package name</td>
              <td class="ugd">
                <asp:TextBox ID="txtName" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="errorMessage"
                  ErrorMessage="*" ControlToValidate="txtName" Enabled="false"></asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" valign="top" width="150">
                Package filename</td>
              <td class="ugd">
                <asp:TextBox ID="txtFileName" runat="server" Width="200px"></asp:TextBox>.zip
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="errorMessage"
                  ErrorMessage="*" ControlToValidate="txtFileName" Enabled="false"></asp:RequiredFieldValidator><br />
                <asp:CheckBox ID="cbTimeStamp" runat="server" Text="Include TimeStamp (YYYYMMDDHHMMSS)">
                </asp:CheckBox></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                FTP URL</td>
              <td class="ugd">
                <asp:TextBox ID="txtFTPURL" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="errorMessage"
                  ErrorMessage="*" ControlToValidate="txtFTPURL" Enabled="false"></asp:RequiredFieldValidator></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                FTP Login</td>
              <td class="ugd">
                <asp:TextBox ID="txtFTPLogin" runat="server" Width="200px"  MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="errorMessage"
                  ErrorMessage="*" ControlToValidate="txtFTPLogin" Enabled="false"></asp:RequiredFieldValidator></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                FTP Password</td>
              <td class="ugd">
                <asp:TextBox ID="txtFTPPassword" runat="server" Width="200px" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                FTP Upload Path</td>
              <td class="ugd">
                <asp:TextBox ID="txtUploadPath" runat="server" Width="200px" MaxLength="500"></asp:TextBox></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                FTP Mode</td>
              <td class="ugd">
                <asp:DropDownList ID="ddlFTPMode" runat="server">
                  <asp:ListItem Value="Passive" Selected="True">Passive</asp:ListItem>
                  <asp:ListItem Value="Active">Active</asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                FTP Port</td>
              <td class="ugd">
                <asp:TextBox ID="txtFTPPort" runat="server" Width="50px">21</asp:TextBox></td>
            </tr>
            <tr valign="middle">
              <td class="editLabelCell" style="width: 150px">
                Notification Email</td>
              <td class="ugd">
                <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email address"
                  Display="Dynamic" ValidationExpression="\b^[A-Za-z0-9](([_\.\-\!\#\$\%\&\'\*\+\\\/\=\?\^\`\{\|\}\~]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$\b"
                  ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
              </td>
            </tr>
            <tr>
              <td class="editLabelCell" valign="top" width="150">
                Region/Country Package</td>
              <td class="ugd">
                <asp:DropDownList ID="ddlRegionPackage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRegionPackage_SelectedIndexChanged">
                </asp:DropDownList></td>
            </tr>
                        <asp:Panel ID="panelFilter" runat="server" Visible="false">

            <tr>
              <td class="editLabelCell" valign="top" width="150">
                Products</td>
              <td class="ugd">
                        <asp:CheckBoxList ID="cboxFilter" AutoPostBack="False" runat="server">
          <asp:ListItem Value="PreReleased">Pre-released</asp:ListItem>
          <asp:ListItem Value="Live">Live</asp:ListItem>
          <asp:ListItem Value="Obsolete">Obsolete</asp:ListItem>
          <asp:ListItem Value="Fallback">Fallback english</asp:ListItem>
          </asp:CheckBoxList>
</td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="panelCreator" runat="server" Visible="false">
              <tr valign="top" style="height: 1px">
                <td colspan="2">
                  <asp:HyperLink ID="hlCreator" runat="server">HyperLink</asp:HyperLink>&nbsp;[
                  <asp:Label ID="lbOrganization" runat="server">Organization</asp:Label>]
                </td>
              </tr>
            </asp:Panel>
          </table>
<%--          <br />
          <igtbl:UltraWebGrid ID="dgPackageCountries" runat="server" ImageDirectory="/ig_common/Images/"
            Width="100%" Visible="false">
            <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="OnClient"
              SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No"
              Name="dg" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No input forms attached to this container">
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
            </DisplayLayout>
            <Bands>
              <igtbl:UltraGridBand Key="PackageCountries" BorderCollapse="Collapse" DataKeyField="Name">
                <Columns>
                  <igtbl:UltraGridColumn HeaderText="Country" Key="CountryCode" Hidden="true" BaseColumnName="Code">
                  </igtbl:UltraGridColumn>
                  <igtbl:UltraGridColumn HeaderText="Country" Key="CountryName" Width="100%" BaseColumnName="Name">
                  </igtbl:UltraGridColumn>
                  <igtbl:TemplatedColumn HeaderText="Pre-released" Key="PreReleased">
                    <CellTemplate>
                      <asp:CheckBox ID="cbPreReleased" runat="server"></asp:CheckBox>
                    </CellTemplate>
                    <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                  <igtbl:TemplatedColumn HeaderText="Live" Key="Live">
                    <CellTemplate>
                      <asp:CheckBox ID="cbLive" runat="server"></asp:CheckBox>
                    </CellTemplate>
                    <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                  <igtbl:TemplatedColumn HeaderText="Obsolete" Key="Obsolete">
                    <CellTemplate>
                      <asp:CheckBox ID="cbObsolete" runat="server"></asp:CheckBox>
                    </CellTemplate>
                    <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                  <igtbl:TemplatedColumn HeaderText="Fallback english" Key="Fallback">
                    <CellTemplate>
                      <asp:CheckBox ID="cbFallback" runat="server"></asp:CheckBox>
                    </CellTemplate>
                    <CellStyle VerticalAlign="Middle" HorizontalAlign="Center">
                    </CellStyle>
                  </igtbl:TemplatedColumn>
                </Columns>
              </igtbl:UltraGridBand>
            </Bands>
          </igtbl:UltraWebGrid>--%>
        </td>
      </tr>
    </asp:Panel>
    <asp:TextBox ID="txtPackageId" runat="server" Visible="false">
    </asp:TextBox></table>
  <input type="hidden" name="action"> <input type="hidden" name="txtSortColPos" id="txtSortColPos" runat="server" value="7">
  <script language="javascript" src="/hc_v4/js/hypercatalog_grid.js"></script>

  <script>
    window.onload=g_i
  </script>

</asp:Content>
