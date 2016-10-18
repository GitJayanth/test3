<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" AutoEventWireup="true" CodeFile="RoadMap_CountryParameters.aspx.cs" Inherits="RoadMap_CountryParameters"%>
<%@ Register TagPrefix="iglbar" Namespace="Infragistics.WebUI.UltraWebListbar" Assembly="Infragistics2.WebUI.UltraWebListbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="hc" TagName="countryViewGraphic" Src="~/UI/Localize/RoadMap/RoadMap_CountryViewGraphic.ascx"%>
<%@ Register TagPrefix="hc" TagName="countryViewList" Src="~/UI/Localize/RoadMap/RoadMap_CountryViewList.ascx"%>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Roadmap</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" runat="Server">
  <script type="text/javascript" language="javascript">
    function CheckAll(list, cb)
    {
      if (list && cb)
      {
        for(var i = 0; i < list.options.length; i++)
        {
          list.options[i].selected = cb.checked;    
        }
      }
    }
    
    function CheckList(list, cb)
    {
      if (list && cb)
      {
        cb.checked = true;
        for(var i = 0; i < list.options.length; i++)
        {
          cb.checked = cb.checked && list.options[i].selected;   
        }
      }
    }
  
    function CheckAllStatus()
    {
      var elt1 = document.getElementById(lStatus);
      var elt2 = document.getElementById(cbStatus);
      CheckAll(elt1, elt2);
    }
  
    function CheckAllWorkflowStatus()
    {
      var elt1 = document.getElementById(lWorkflowStatus);
      var elt2 = document.getElementById(cbWorkflowStatus);
      CheckAll(elt1, elt2);
    }
    
    function CheckAllClass()
    {      
      var elt1 = document.getElementById(lClasses);
      var elt2 = document.getElementById(cbClasses);
      CheckAll(elt1, elt2);
    }
    
    function CheckListStatus()
    {
      var elt1 = document.getElementById(lStatus);
      var elt2 = document.getElementById(cbStatus);
      CheckList(elt1, elt2);
    }
    
    function CheckListWorkflowStatus()
    {
      var elt1 = document.getElementById(lWorkflowStatus);
      var elt2 = document.getElementById(cbWorkflowStatus);
      CheckList(elt1, elt2);
    }
    
    function CheckListClass()
    {
      var elt1 = document.getElementById(lClasses);
      var elt2 = document.getElementById(cbClasses);
      CheckList(elt1, elt2);
    }
  </script>
  <table class="main" cellpadding="0" cellspacing="0" border="0">
    <asp:panel id="pnlParameters" runat="server">
      <tr valign="top" style="height: 1px;">
        <td>
		      <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
				    <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				    <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				    <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				    <Items>
              <igtbar:TBarButton Image="/hc_v4/img/ed_savefinal.gif" Key="Submit" Text="Submit"
                ToolTip="Submit">
              </igtbar:TBarButton>
				    </Items>
			    </igtbar:ultrawebtoolbar>
			  </td>
			</tr>
      <tr valign="top">
        <td>
          <table border="0" cellpadding="2" cellspacing="0" style="width: 100%">
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label runat="server" ID="lbStatus">Product status</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:ListBox ID="lStatus" runat="server" SelectionMode="Multiple" onclick="CheckListStatus()" Rows="5"></asp:ListBox>
                    </td>
                    <td>
                      <asp:CheckBox ID="cbStatusAll" runat="server" Text="All" onclick="CheckAllStatus()" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label runat="server" ID="lbWorkflowStatus">Product workflow status</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:ListBox ID="lWorkflowStatus" runat="server" SelectionMode="Multiple" onclick="CheckListWorkflowStatus()" Rows="4"></asp:ListBox>
                    </td>
                    <td>
                      <asp:CheckBox ID="cbWorkflowStatusAll" runat="server" Text="All" onclick="CheckAllWorkflowStatus()" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label runat="server" ID="lbLD">Live date</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="2" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:DropDownList ID="ddlLD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                      <asp:Panel ID="pnlLD" runat="server">
                        <igsch:WebDateChooser ID="wdLD" runat="server" Width="90px" NullDateLabel="">
                          <EditStyle Font-Size="8pt"></EditStyle>
					                <CalendarLayout ShowTitle="False" ShowFooter="False">
                            <TodayDayStyle ForeColor="Red" />
                          </CalendarLayout>
                        </igsch:WebDateChooser>
                      </asp:Panel>
                    </td>
                    <td>
                      <asp:Label ID="lbMissingLD1" runat="server" CssClass="hc_error" Visible="false">*</asp:Label>
                    </td>
                  </tr>
                  <asp:Panel ID="pnlLDTo" runat="server">
                    <tr valign="top">
                      <td>
                        <asp:Label ID="lbLDTo" runat="server">to</asp:Label>
                      </td>
                      <td>
                        <igsch:WebDateChooser ID="wdLDTo" runat="server" Width="90px" NullDateLabel="">
                          <EditStyle Font-Size="8pt"></EditStyle>
					                <CalendarLayout ShowTitle="False" ShowFooter="False">
                            <TodayDayStyle ForeColor="Red" />
                          </CalendarLayout>
                        </igsch:WebDateChooser>
                      </td>
                      <td>
                        <asp:Label ID="lbMissingLD2" runat="server" CssClass="hc_error" Visible="false">*</asp:Label>
                      </td>
                    </tr>
                  </asp:Panel>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label runat="server" ID="lbOD">Obsolete date</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="2" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:DropDownList ID="ddlOD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                      <asp:Panel ID="pnlOD" runat="server">
                        <igsch:WebDateChooser ID="wdOD" runat="server" Width="90px" NullDateLabel="">
                          <EditStyle Font-Size="8pt"></EditStyle>
					                <CalendarLayout ShowTitle="False" ShowFooter="False">
                            <TodayDayStyle ForeColor="Red" />
                          </CalendarLayout>
                        </igsch:WebDateChooser>
                      </asp:Panel>
                    </td>
                    <td>
                      <asp:Label ID="lbMissingOD1" runat="server" CssClass="hc_error" Visible="false">*</asp:Label>
                    </td>
                  </tr>
                  <asp:Panel ID="pnlODTo" runat="server">
                    <tr valign="top">
                      <td>
                        <asp:Label ID="lbODTo" runat="server">to</asp:Label>
                      </td>
                      <td>
                        <igsch:WebDateChooser ID="wdODTo" runat="server" Width="90px" NullDateLabel="">
                          <EditStyle Font-Size="8pt"></EditStyle>
					                <CalendarLayout ShowTitle="False" ShowFooter="False">
                            <TodayDayStyle ForeColor="Red" />
                          </CalendarLayout>
                        </igsch:WebDateChooser>
                      </td>
                      <td>
                        <asp:Label ID="lbMissingOD2" runat="server" CssClass="hc_error" Visible="false">*</asp:Label>
                      </td>
                    </tr>
                  </asp:Panel>
                  <tr>
                    <asp:Label ID="lbErrOD" runat="server" CssClass="hc_error"></asp:Label></tr>
                </table>
               </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label ID="lbCountries" runat="server">Countries</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:DropDownList ID="ddlCountries" runat="server"> </asp:DropDownList>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label ID="lbClasses" runat="server">Classes</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:ListBox ID="lClasses" runat="server" SelectionMode="Multiple"  onclick="CheckListClass()"></asp:ListBox>
                    </td>
                    <td>
                       <asp:CheckBox ID="cbClassesAll" runat="server" Text="All" onclick="CheckAllClass()" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label ID="lbProductScope" runat="server">Product scope</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:RadioButton ID="rdMyProduct" runat="server" Text="Only products under my responsibility" GroupName="ProductScope" />
                    </td>
                  </tr>
                  <tr valign="top">
                    <td>
                      <asp:RadioButton ID="rdAllProduct" runat="server" Text="All products" GroupName="ProductScope" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr valign="top">
              <td class="EditLabelCell" style="width: 150px; font-weight: bold">
                <asp:Label ID="lbView" runat="server">View</asp:Label>
              </td>
              <td class="EditValueCell">
                <table cellpadding="0" cellspacing="0" border="0">
                  <tr valign="top">
                    <td>
                      <asp:RadioButton ID="rdList" runat="server" Text="List" GroupName="View" />
                    </td>
                  </tr>
                  <tr valign="top">
                    <td>
                      <asp:RadioButton ID="rdGraphic" runat="server" Text="Graphic" GroupName="View" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </asp:panel>
    <asp:Panel ID="pnlList" runat="server">
      <tr valign="top">
        <td>
          <hc:countryViewList id="vList" runat="server"></hc:countryViewList>
        </td>
      </tr>
    </asp:Panel>
    <asp:Panel ID="pnlGraphic" runat="server">
      <tr valign="top">
        <td>
          <hc:countryViewGraphic id="vGraphic" runat="server"></hc:countryViewGraphic>
        </td>
      </tr>
    </asp:Panel>
  </table>
</asp:Content>
