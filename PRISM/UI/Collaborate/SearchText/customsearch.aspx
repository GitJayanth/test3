<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.CustomSearch" CodeFile="CustomSearch.aspx.cs" EnableEventValidation="false"%>

<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igcmbo" Namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics2.WebUI.WebCombo.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ASCX" TagName="RangeFilter" Src="RangeFilter.ascx" %>
<%@ Register TagPrefix="ucs" TagName="PLWebTree" Src="../../Admin/PLWebTree.ascx" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Custom search</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
      <script>
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'Options') {
        oEvent.cancelPostBack = true;
        var optionsFieldset = document.getElementById('optionsFieldset');
        optionsFieldset.style.display=(optionsFieldset.style.display==''?'none':'');
      }
    }
    
//		
//		document.onkeypress = checkkey;
//    function checkkey(keyStroke)
//    {
//      if (event.keyCode == 13) 
//      {     
//        event.returnValue = false;
//        event.cancelBubble = true; 
//      }                              
//    }

  </script>
  <table class="main" cellspacing="0" cellpadding="0" border="0">
    <tr valign="top" style="height:1px">
      <td colspan="2">
        <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" Height="26px">
          <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
          <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
          <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
          <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
          <Items>
            <igtbar:TBarButton Key="Search" ToolTip="Proceed to search" Text="Search" Image="/hc_v4/img/ed_search.gif">
              <DefaultStyle height="20px"/>
            </igtbar:TBarButton>
					  <igtbar:TBarButton Key="Options" ToolTip="Result options" Text="Options" ToggleButton="True" Image="/hc_v4/img/ed_options.gif"></igtbar:TBarButton>
          </Items>
          <DefaultStyle BorderWidth="1px" BorderStyle="None" BackgroundImage="/ig_common/webtoolbar2/ig_tb_back03.gif"></DefaultStyle>
        </igtbar:ultrawebtoolbar>
      </td>
    </tr>
    <tr valign="top">
      <td>
        <fieldset id="optionsFieldset" style="display:none;"><legend>Options</legend>
			    <table cellspacing="0" cellpadding="0" width="100%" border="0">
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
					      Data source
					    </td>
					    <td class="ugd">
					      <asp:DropDownList ID="dbList" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="dbList_SelectedIndexChange" > 
					      <%--PRISM UI TO PDB CHANGES BY REKHA THOMAS--%>
					        <asp:ListItem Text="PDB (default)" Value="0" Selected="true" ></asp:ListItem>
					      <%--END PRISM UI TO PDB CHANGES BY REKHA THOMAS--%>
					        <asp:ListItem Text="Live data" Value="1"></asp:ListItem>
					      </asp:DropDownList>
					    </td>
				    </tr>
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
					      Fields to display
					    </td>
					    <td class="ugd">
                <asp:checkboxlist id="fieldFilter" runat="server" RepeatColumns="4">
                  <asp:ListItem Text="ProductType" Value="ProductType" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Culture" Value="Culture" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Level" Value="LevelId" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="ProductName" Value="ProductName" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Item status" Value="Status" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="xmlname" Value="xmlname" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Container" Value="ContainerName" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Value" Value="ChunkValue" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Status" Value="ChunkStatus" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Modified" Value="ModifiedDate" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Inherited" Value="IsInherited" Selected="true"></asp:ListItem>
                </asp:checkboxlist>
					    </td>
				    </tr>
			    </table>
        </fieldset>
        <table class="datatable" width="100%">
          <TR>
            <td valign="top">
              <asp:Label ID="message" runat="server" Visible="false" ForeColor="red" Font-Bold="true"></asp:Label>
              <fieldset><legend>Custom search</legend>
                <div style="width:100%;" class="sectionSubTitle">new filter</div>
                  <ASCX:RangeFilter id="newFilter" runat="server" FilterAdded="AddFilter"></ASCX:RangeFilter>
                <br/>
                <div style="width=100%;" class="sectionSubTitle">applied filters</div>
                <asp:Repeater id="paramList" runat="server" >
                  <ItemTemplate>
                    <b><asp:Label id="parameterName" runat="server" Text='<%# filterNames[Container.ItemIndex] %>'></asp:Label></b>
                    <asp:dropdownlist id="operator" runat="server" Visible='<%# ((ArrayList)filters2[Container.ItemIndex]).Count>1 %>'>
                    <%--PRISM UI TO PDB CHANGES BY REKHA THOMAS--%>
                    <%--  <asp:ListItem Text="--Please select--" Value=""/>--%>
                      <asp:ListItem Text="Match all" Value="and"/>
                      <asp:ListItem Text="Match one at least" Value="or"/>
                      <asp:ListItem Text="Match none" Value="nor"/>
                    <%--END PRISM UI TO PDB CHANGES BY REKHA THOMAS--%>
                    </asp:dropdownlist>
                    
                    <table cellpadding="0" cellspacing="0" width="100%" style="background-color:white;" id="multipleValuesTable">
                      <tr>
                        <td style="border-left:1px solid gainsboro;border-bottom:1px solid gainsboro;">
                          <asp:Repeater id="filterList2" runat="server" DataSource='<%# filters2[Container.ItemIndex] %>'>
                            <ItemTemplate>
                              <ASCX:RangeFilter id="filter2" runat="server" 
                                Filter='<%# Container.DataItem%>'
                                FilterRemoved="newFilter_FilterRemoved">
                              </ASCX:RangeFilter>
                            </ItemTemplate>
                          </asp:Repeater>
                        </td>
                      </tr>
                    </table>
                  </ItemTemplate>
                </asp:Repeater>
                <asp:Panel id="emptyPanel" runat="server" visible='<%# filters2.Count==0%>'>
                  <font style="color:gray;">empty</font>
                </asp:Panel>
              </fieldset>
            </td>
          </tr><tr>
            <td valign="top">
              <style type="text/css">
              <!--
                A.highlightableArrow:hover
                {
                  color:red;
                  text-decoration:none;
                }
                A.highlightedArrow:hover
                {
                  color:blue;
                  text-decoration:none;
                }
                TR.over,TD.over
                {
                  background-color:#eeeeee;
                }
              -->
              </style>
              <script language="javascript">
                <!--
                var currentList, currentHref, innerHtmlBackup;
                var customizationPanel;

                function applyFilter()
                {
                  var checkboxes=currentList.getElementsByTagName("input");
                  var values="";
                  for (var i=0;i<checkboxes.length;i++)
                    values+=checkboxes[i].checked?", "+checkboxes[i].nextSibling.innerText:"";
                  var innerHTML=currentHref.innerHTML;
                  if (values.length>0)
                  {
                    currentHref.innerHTML="Filtered "+innerHTML.substr(innerHTML.length-6,6);
                    currentHref.title=currentHref.alt=values.substr(2,values.length-2);
                  }
                  else
                  {
                    currentHref.innerHTML="No filter "+innerHTML.substr(innerHTML.length-6,6);
                    currentHref.title=currentHref.alt="No filter";
                  }
                  hide(document.getElementById('CustomizationPanel'),currentHref);
                  return false;
                }
                
                function resetFilter()
                {
                  var checkboxes=currentList.getElementsByTagName("input");
                  for (var i=0;i<checkboxes.length;i++)
                    checkboxes[i].checked=false;
                  var innerHTML=currentHref.innerHTML;
                  currentHref.innerHTML="No filter "+innerHTML.substr(innerHTML.length-6,6);
                  currentHref.title=currentHref.alt="No filter";
                  //hide(document.getElementById('CustomizationPanel'),currentHref);
                  return false;
                }
                
                function resetPLs()
                {
                  resetCheckedNodes();
                  return false;
                }
                
                function show(customizationPanel,ahref,list)
                {
                  if (innerHtmlBackup==null)
                    innerHtmlBackup = customizationPanel.innerHTML;

                  currentHref = ahref;
                  var innerHTML=currentHref.innerHTML;
                  currentHref.className='highlightedArrow';
                  currentHref.innerHTML=innerHTML.substr(0,innerHTML.length-4)+'--'+innerHTML.substr(innerHTML.length-4,4);

                  document.getElementById('<%= resetChoice.ClientID %>').style.display = '';
                  list.style.display='';
                  list.parentElement.style.display='';
                  currentList=list;
                  
                  customizationPanel.innerHTML = ahref.parentElement.previousSibling.innerHTML;
                }
                
                function hide(customizationPanel,ahref)
                {
                  var innerHTML=ahref.innerHTML;
                  ahref.className='highlightableArrow';
                  ahref.innerHTML=innerHTML.substr(0,innerHTML.length-6)+innerHTML.substr(innerHTML.length-4,4);
                  currentHref = null;
                  
                  document.getElementById('<%= resetChoice.ClientID %>').style.display = 'none';
                  currentList.style.display='none';
                  currentList.parentElement.style.display='none';
                  currentList=null;

                  customizationPanel.innerHTML = innerHtmlBackup;
                }
                
                function ShowHide(ahref,listName)
                {
                  var list=document.getElementById(listName);

                  var toshow=(currentHref!=ahref);
                  if (currentHref!=null)
                    applyFilter();
                  if (toshow)
                    show(customizationPanel,ahref,list);
                }
                
                function tdOver(td)
                {
                  td.parentElement.className='over';
                  if (customizationPanel)
                    customizationPanel.parentElement.className='over';
                }
                
                function tdOut(td)
                {
                  td.parentElement.className='';
                  if (customizationPanel)
                    customizationPanel.parentElement.className='';
                }
                //-->
              </script>
              <fieldset><legend><a name="business">Business filters</a></legend>
                <table cellspacing="0" cellpadding="0">
                  <TR>
                    <td height="22px" width="120" style="cursor:hand;" onMouseOver="tdOver(this)" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + itemClassList.ClientID + "\");"%>'><B>
                      <asp:Label ID="lbClassName" runat="server" Text="Product Class"></asp:Label></B></td>
                    <td height="22px" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + itemClassList.ClientID + "\");"%>'>
                      <a id="itemClassAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                    <td rowspan="9" style="border-left:1px solid gainsboro;vertical-align:top;padding:10px;">
                      <div id="CustomizationPanel">
                        <span style="color:gray;">Select a filter</span>
                      </div>
                      <div style="height:250px;width:300px;display:none;overflow-y:scroll;border-top:solid 1px gray;border-right:solid 1px white;border-bottom:solid 1px white;border-left:solid 1px gray;">
                        <div id="plDiv" style="display:none;"><ucs:PLWebTree id="plList" runat="server" /></div>
                      </div>
                      <div style="display:none;">
                        <asp:checkboxlist id="itemClassList" runat="server" style="display:none" RepeatColumns="3" DataValueField="Id" DataTextField="Name"></asp:checkboxlist>
                        <asp:checkboxlist id="itemLevelList" runat="server" style="display:none" RepeatColumns="3" DataValueField="Id" DataTextField="Name"></asp:checkboxlist>
                        <asp:checkboxlist id="regionCountryList" runat="server" style="display:none" RepeatColumns="8" DataValueField="Code" DataTextField="Name" ></asp:checkboxlist>
                        <asp:checkboxlist id="containerStatusList" runat="server" style="display:none" RepeatColumns="1"></asp:checkboxlist>
                        <asp:checkboxlist id="containerTypeList" runat="server" style="display:none" RepeatColumns="2" DataValueField="Code" DataTextField="Name"></asp:checkboxlist>
                        <asp:checkboxlist id="itemStatusList" runat="server" style="display:none" RepeatColumns="1"></asp:checkboxlist>
                        <asp:checkboxlist id="itemTypeList" runat="server" style="display:none" RepeatColumns="2" DataValueField="Id" DataTextField="Name"></asp:checkboxlist>
                        <div id="productWithoutDiv" style="display:none;">
                          <asp:CheckBox ID="prodWOcompat" runat="server" Text="without compatibility"/><BR />
                          <asp:CheckBox ID="prodWOPL" runat="server" Text="without PL"/><BR />
                          <asp:CheckBox ID="prodExpToProv" runat="server" Text="exported to Provisioner"/>
                        </div>
                      </div>
                      <asp:Button id="resetChoice" runat="server" Text="Reset" style="display:none;"></asp:Button>
                    </td>
                  </tr>
                  <TR>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + itemLevelList.ClientID + "\");"%>'><B>Hierarchy level</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + itemLevelList.ClientID + "\");"%>'>
                      <a id="itemLevelAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <TR>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick="ShowHide(this.nextSibling.firstChild,'plDiv');"><B>Product line</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick="ShowHide(this.firstChild,'plDiv');">
                      <a id="plAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <TR>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + containerStatusList.ClientID + "\");"%>'><B>Content status</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + containerStatusList.ClientID + "\");"%>'>
                      <a id="containerStatusAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <TR>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + containerTypeList.ClientID + "\");"%>'><B>Container type</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + containerTypeList.ClientID + "\");"%>'>
                      <a id="containerTypeAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <TR>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + regionCountryList.ClientID + "\");"%>'><B>Catalogs</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + regionCountryList.ClientID + "\");"%>'>
                      <a id="regionCountryAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <tr>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + itemStatusList.ClientID + "\");"%>'><B>Node status</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + itemStatusList.ClientID + "\");"%>'>
                      <a id="itemStatusAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <tr>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.nextSibling.firstChild,\"" + itemTypeList.ClientID + "\");"%>'><B>Item type</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick='<%= "ShowHide(this.firstChild,\"" + itemTypeList.ClientID + "\");"%>'>
                      <a id="itemTypeAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <tr>
                    <td height="22" width="120" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick="ShowHide(this.nextSibling.firstChild,'productWithoutDiv');"><B>Product...</B></td>
                    <td height="22" width="150" style="cursor:hand;" onMouseOver="tdOver(this);" onMouseOut="tdOut(this);" onClick="ShowHide(this.firstChild,'productWithoutDiv');">
                      <a id="productWithoutAnchor" runat="server" href="#business" style="font-weight:bold;" class="highlightableArrow" title="No filter">No filter &gt;</a>
                    </td>
                  </tr>
                  <tr><td height="75px" colspan="2">&nbsp;</td></tr>
                </table>
              </fieldset>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</asp:Content>
<asp:Content ID="JSBottom" ContentPlaceHolderID="HOJSBottom" runat="server">
    <script language=javascript>
  customizationPanel=document.getElementById('CustomizationPanel');
</script>
</asp:Content>