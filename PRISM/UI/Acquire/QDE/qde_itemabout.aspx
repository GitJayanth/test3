<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_ItemAbout" CodeFile="QDE_ItemAbout.aspx.cs"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="hc" TagName="projectInfo" Src="~/ui/acquire/qde/controlProject.ascx"%>
<%@ Register TagPrefix="hc" TagName="plcInfo" Src="~/ui/acquire/qde/controlPLC.ascx"%>
<%@ Register TagPrefix="hc" TagName="sortInfo" Src="~/ui/acquire/qde/controlItemSort.ascx"%>

<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Content</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
  <script type="text/javascript" src="../../../ajaxcontrols/Extensions.js"></script>

	<script type="text/javascript" language="javascript">
    var winCrossSell;
    var winRoll;
    var winInstantTR;
    	
    function EditTR(i)
    {
      var url = "../../../redirect.aspx?p=UI/Globalize/TR/TR_Properties.aspx&tr=" + i;
      url += '#target';
      winChunkEdit = OpenModalWindow(url,'qde', 400, 500, 'yes', 'yes', 'no', 'no', 'yes')
    }
    function DoAction(action, panel)
    {
      if (action=='cancel')
      {
        __doPostBack(action, panel);
        return;
      }
      if (action=='deleteproj')
      {
        if (confirm('Are you sure you want to delete the project ?'))
        {
          __doPostBack(action, panel);
        }
        return;
      }
      if (action=='save')
      {
        if (typeof(Page_Validators) != 'undefined')
        {
          for( var i = 0; i < Page_Validators.length; ++i)
          {
            ValidatorEnable( Page_Validators[i], true);
          }
        }
      }  
      if (!MandatoryFieldMissing()){
        __doPostBack(action, panel);
      }
    }
    function ReloadMain()
    {
      if (top)
      {
        var prev = top.location;
        top.location = '/hc_v4/dummy.htm';
        top.location = prev;
      }
    }
    function ReloadTV(itemName, isRoll)
    {
      // reload item name
      if (top.qdeFrame.frametv && isRoll == 0)
      {
        var prev = top.qdeFrame.frametv.location;
        top.qdeFrame.frametv.location = '/hc_v4/dummy.htm';
        top.qdeFrame.frametv.location = "QDE_TV.aspx";
      }
      // reload item name
      if (itemName != '' && parent)
      {
        if (isRoll == 1 && parent.parent)
        {
          var name = "<img src='/hc_v4/img/ed_roll.gif'> "+itemName;
          parent.parent.UpdateItemName(name);
        }
        else
          parent.UpdateItemName(itemName);
      }
    }
    function openRoll(id)
    {
	      var url = 'QDE_Rolls.aspx?i='+id+'&a=modify'; 
  	    var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";     	
        winRoll = OpenModalWindow(url, "cr", 200, 600, "no");
    }
    function InstantTR(id, c){
	      //var url = 'QDE_InstantTR.aspx?i='+id; 
	      
	      //PCF Requirement -- Regional project Management (Deepak.S)
	      var url = 'QDE_InstantTR.aspx?i='+id+'&c='+c; 
	      
  	    var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";     	
        winInstantTR = OpenModalWindow(url, "itr", 200, 600, "no");
    }

    function OpenWhosWho(id, c)
    {
	      var url = 'qde_itemwhoswho.aspx?i='+id+'&c='+c; 
        winWhosWho = OpenModalWindow(url, "cr", 500, 500, 'yes');
    }
    // Added by Mickael CHARENSOL (30/06/2006)
    function OpenCrossSellPopup(itemId, isCrossSell)
    {
	    var url = 'QDE_CrossSell.aspx?i='+itemId+'&a='+isCrossSell;
	    var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";  
      winCrossSell = OpenModalWindow(url, "CrossSell", 200, 400, "no");
    }
    // Added by Mickael CHARENSOL (30/06/2006)
    function closeWindows()
    {
	    if (winCrossSell) winCrossSell.close();
	    if (winInstantTR) winInstantTR.close();
	    if (winRoll) winRoll.close();
    }
    
    function UpdateMS(cbMS)
    {
      var cb = document.getElementById(cbMS);
      if (cb)
        cb.checked = true;
    }
    
    function UpdateP(cbP)
    {
      var cb = document.getElementById(cbP);
      if (cb)
        cb.checked = true;
    }
    

    /**********************
    ** REFRESH LANGUAGES **
    **********************/
		var ItemWorkingTable = 
		{
      delay: 100,
      prepare: function() {
        var i = document.getElementById("aj_id").value; // itemId
        var l = document.getElementById("aj_cc").value; // itemId
        return '<option><id>' + i + '</id><l>' + l+ '</l></option>';
      },
      call: proxies.UIExtensions.RefreshItemWorkingTables,
      finish: function (xmlDoc) {
        UpdatePage(xmlDoc);
      },
      onException: proxies.alertException
    } // ItemWorkingTables
    function RefreshItemWorkingTable()
    { 
      ajax.Start(ItemWorkingTable); 
    }		  
	  function UpdatePage(xmlDoc)
	  {		
	    if (xmlDoc && xmlDoc.firstChild 
	        && xmlDoc.firstChild.childNodes 
	        && xmlDoc.firstChild.childNodes.length 
	        && xmlDoc.firstChild.childNodes.length == 1)
	    {
	      var currentRecord = xmlDoc.firstChild.childNodes[0];
        if (currentRecord)
        {
          var result = currentRecord.attributes[0].nodeValue;
          if (result.toLowerCase() == "true")
          {
            var appliedLanguages = currentRecord.attributes[1].nodeValue;
            var AppL = document.getElementById("tdAppL");
            if (AppL && appliedLanguages)
            {
              AppL.innerText = appliedLanguages;
            }
          }
          else
          {
            alert("result=" + currentRecord.attributes[2].nodeValue);
          }
        }
        else
        {
          alert("No response!!!");
        }
	  	}
    }
    
    /**********************
    ** REFRESH PUBLISHER **
    **********************/
    var PublishersWork = 
    {
      delay: 100,
      prepare: function() {
        var i = document.getElementById("aj_id").value; // itemId
        return '<option><id>' + i + '</id></option>';
      },
      call: proxies.UIExtensions.RefreshPublishers,
      finish: function (xmlDoc) {
        UpdatePublishers(xmlDoc);
      },
      onException: proxies.alertException
    } // PublishersWork

    function RefreshPublishers()
    { 
      ajax.Start(PublishersWork); 
    }
    		  
    function UpdatePublishers(xmlDoc)
    {		
      if (xmlDoc && xmlDoc.firstChild 
          && xmlDoc.firstChild.childNodes 
          && xmlDoc.firstChild.childNodes.length 
          && xmlDoc.firstChild.childNodes.length == 1)
      {
        var currentRecord = xmlDoc.firstChild.childNodes[0];
        if (currentRecord)
        {
          var result = currentRecord.attributes[0].nodeValue;
          if (result.toLowerCase() == "false")
          {
            alert(currentRecord.attributes[1].nodeValue);
          }
        }
        else
        {
          alert("Not response!!!");
        }
	    }
    }
    function UnloadGrid()
            {
                igtbl_unloadGrid("dg");
            }
  </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<!--onunload="closeWindows();"-->
<input type="hidden" name="action" id="action">
			<igmisc:webpanel id="wPanelIdentity" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img">
				<Header Text="Identity" TextAlignment="Left">
					<ExpandedAppearance>
            <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
					<ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
          <CollapsedAppearance>
            <Style CssClass="hc_webpanelcol"></Style>
          </CollapsedAppearance>
				</Header>
				<Template>
          <asp:Panel ID="PanelViewIdentity" Runat="server">
						<asp:label id="lbItemName" runat="server" CssClass="hc_itemname">Item name</asp:label>
						<asp:Panel ID="panelCrossSell" runat="server">
              <br/>
              <asp:label id="lbIsCrossSell" runat="server">IsCrossSell</asp:label>
              <asp:Button id="btnCrossSell" runat="server" Text="Apply" OnClick="btnCrossSell_Click"></asp:Button>
            </asp:Panel>
            <asp:Panel ID="panelCountrySpecific" runat="server">
              <br/>
              <asp:label id="lbCountrySpecific" runat="server"><b>Import this product into Master Catalogue</b></asp:label>
              <asp:Button id="btnCountrySpecific" runat="server" Text="Apply" OnClick="btnCountrySpecific_Click"></asp:Button>
            </asp:Panel>
            <asp:label id="lbError" runat="server"></asp:label>
            <br/> 
            <asp:label id="lbCreated" runat="server" Font-Italic="True">ItemCreationDate</asp:label>
            <asp:ImageButton ID="whoswho" ImageAlign="AbsBottom" ToolTip="Who's who" runat="server" ImageUrl="/hc_v4/img/ed_whoswho.gif"/>
<!-- RQ 4246 Dummy SKU  11/27/2007-->
            <br/> 
            <asp:label id="lbPLCModifiedBy" runat="server" Font-Italic="True">PLC Last modified by: NULL </asp:label>
<!-- RQ 4246 Dummy SKU  11/27/2007-->
          </asp:Panel>
          <asp:Panel ID="PanelEditIdentity" Runat="server">
						<script type="text/javascript" language="javascript">
							////////////////////////////////////////
							function CleanSku(text, re){
							////////////////////////////////////////
								return new String(text).toUpperCase().replace(re, '');
  						}
							var strRuleName, strRuleSku;
							//strRuleName = '';
	  					var normalSku = /([^0-9A-Z#-])/g;
						</script>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr valign="top">
								<td style="width: 90px; height: 18px"><b><asp:Label ID="label9" runat="server">Name</asp:Label></b></td>
								<td>
									<asp:textbox id="txtProductName" runat="server" Columns="70"></asp:textbox>
									<a onmouseover="doTooltip(event,strRuleName)" onmouseout="hideTip()" href="javascript://" style="border: 0px">
									  <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
									</a>
									<span id="textcount">&nbsp;</span>
									<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ControlToValidate="txtProductName"
										Display="Dynamic"></asp:requiredfieldvalidator>
								</td>
						<asp:panel id="PanelMasterName" Runat="server">
							<td style="width: 90px; height: 18px">
							  <b><asp:label id="lbMasterNameLabel" runat="server">Original</asp:label></b></td>
						  <td><asp:label id="lbMasterName" runat="server">Item name</asp:label><br/><br/></td>
					  </asp:panel>
              </tr>
            <asp:Panel ID="PanelItemType" Runat="Server">
						  <tr>
						    <td style="width: 90px; height: 18px"><b><asp:Label ID="label7" runat="server">Type</asp:Label></b></td> 
						    <td>
						      <table border="0" cellpadding="0" cellspacing="0">
						        <tr>
							        <td><asp:DropDownList ID="ddlItemType" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlItemType_SelectedIndexChanged"></asp:DropDownList></td>
						          <asp:panel id="PanelBundleMainComponent" Runat="server">
							          <td style="width: 90px; height: 18px"><b><asp:Label ID="label8" runat="server" Width="100px">&nbsp;Main Component</asp:Label></b></td>
							          <td>
								          <asp:DropDownList ID="ddlMainComponent" Runat="server"></asp:DropDownList>
							          </td>
					            </asp:panel>
					          </tr>
						      </table>
						    </td>
					    </tr>
					  </asp:Panel>
            <asp:panel id="PanelOption" Runat="server" Visible="false">
						  <tr valign="top">
                  <td valign="middle" style="height: 18px"><b><asp:Label ID="label13" runat="server">Option</asp:Label></b></td>
                   <td style="height: 18px">
								  <asp:Label ID="lbSku" runat="server"></asp:Label><asp:textbox id="txtOption" runat="server" Width="50px" Columns="6" MaxLength=6></asp:textbox>
								  <span id="Span1">&nbsp;</span>
								  <asp:requiredfieldvalidator id="RequiredFieldvalidator3" runat="server" ErrorMessage="Required" ControlToValidate="txtOption"
									  Display="Dynamic"></asp:requiredfieldvalidator>
							  </td>
						  </tr>
						</asp:panel>
            <asp:panel id="PanelSku" Runat="server">
						  <tr valign="top">
                  <td valign="middle" style="height: 18px"><b><asp:Label ID="label6" runat="server">Sku</asp:Label></b></td>
                   <td style="height: 18px">
								  <asp:textbox id="txtSku" runat="server" Width="112px" Columns="50" MaxLength="50"></asp:textbox>
								  <a onmouseover="doTooltip(event,strRuleSku)" onmouseout="hideTip()" href="javascript://" style="border: 0px">
								    <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top">
								  </a>
								  <span id="textcountSku">&nbsp;</span>
								  <asp:requiredfieldvalidator id="RequiredFieldvalidator2" runat="server" ErrorMessage="Required" ControlToValidate="txtSku"
									  Display="Dynamic"></asp:requiredfieldvalidator>
							  </td>
						  </tr>
						  <tr valign="top">
							  <td valign="middle" style="height: 18px"><b><asp:Label ID="labelPL" runat="server">Product Line</asp:Label></b></td>
							  <td style="height: 18px">
								  <asp:DropDownList id="ddlPL" runat="server" Width="112px">
								  </asp:DropDownList>
							  </td>
						  </tr>
						  <tr valign="top">
							  <td valign="middle" style="height: 18px"><b><asp:Label ID="label10" runat="server">Retail only</asp:Label></b></td>
							  <td style="height: 18px">
								  <asp:CheckBox id="cbRetailOnly" runat="server"></asp:CheckBox>
							  </td>
						  </tr>
					  </asp:panel>
					  <asp:Panel ID="PanelSort" runat="server">
              <tr>
						    <td><b><asp:Label ID="label11" runat="server">Ordering</asp:Label></b></td>
						    <td>
							    <hc:sortInfo id="SortInfoControl" runat="server"></hc:sortInfo>
						    </td>
					    </tr>
					  </asp:Panel>
            </table>
          </asp:Panel>
        </Template>
			</igmisc:webpanel>
			<igmisc:webpanel id="wPanelPLC" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img">
				<Header Text="Product Life Cycle" TextAlignment="Left">
					<ExpandedAppearance>
            <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
					<ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
          <CollapsedAppearance>
            <Style CssClass="hc_webpanelcol"></Style>
          </CollapsedAppearance>
				</Header>
				<Template>
					<asp:Label id="lbPLCInformation" runat="server">lbPLCInformation</asp:Label>
					<asp:Panel ID="PanelPLC" Runat="server">
						<hc:plcInfo id="PIC" runat="server"></hc:plcInfo>
					</asp:Panel>
				</Template>
			</igmisc:webpanel>
			<igmisc:webpanel id="wPanelProject" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img">
				<Header Text="Project information" TextAlignment="Left">
					<ExpandedAppearance>
            <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
					<ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
          <CollapsedAppearance>
            <Style CssClass="hc_webpanelcol"></Style>
          </CollapsedAppearance>
				</Header>
				<Template>
					<asp:Panel id="PanelViewProject" Runat="server">
						<table border='1' cellpadding='1' style='border-collapse:collapse;width:90%' cellspacing='0'>
							<tr valign="top">
								<td class='editLabelCell' style='font-weight:bold;width:130px'>
										<asp:Label id="Label1" runat="server">Project</asp:Label></td>
								<td class='editValueCell'>
									<asp:Label id="lbProjectInfo" runat="server">ItemProjectInformation</asp:Label></td>
							</tr>
							<asp:Panel ID="PanelProjectStatus" Runat="server">
								<tr valign="top">
									<td class='editLabelCell' style='font-weight:bold' >
											<asp:Label id="Label4" runat="server">Status</asp:Label></td>
									<td class='editValueCell'>
										<table border="0" cellpadding="0" cellspacing="0">
											<tr>
												<td>
													<asp:Label id="lbProjectStatus" runat="server">ItemProjectStatus</asp:Label>&nbsp;&nbsp;
													<asp:Button id="btnInstantTR" runat="server" CausesValidation="False" Text="--> Instant TR <--"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</asp:Panel>
							<asp:Panel ID="panelProjectTRs" Runat="server">
								<tr valign="top">
									<td class='editLabelCell' style='font-weight:bold' >
											<asp:Label id="Label5" runat="server">OnGoing TRs</asp:Label></td>
									<td class='editValueCell'>
										<asp:Label id="lbCurrentOpenedTRs" runat="server">Current Opened TRs</asp:Label></td>
								</tr>
							</asp:Panel>
							<asp:Panel ID="panelProjectLanguages" Runat="server">
								<tr valign="top">
									<td class='editLabelCell' style='font-weight:bold' >
											<asp:Label id="Label2" runat="server">Project languages</asp:Label></td>
									<td class='editValueCell'>
										<asp:Label id="lbProjectInfoLanguages" runat="server">ItemProjectInformation</asp:Label></td>
								</tr>
							</asp:Panel>
							<tr valign="top">
								<td class='editLabelCell' style='font-weight:bold' >
										<asp:Label id="Label3" runat="server">Applied languages</asp:Label></td>
								<td id="tdAppL" class='editValueCell'>
									<asp:Label id="lbProjectInfoApplicableLanguages" runat="server">ItemProjectInformation</asp:Label>
								</td>
							</tr>
						</table>
						<br/>
						<asp:label id="lbProjectCreationDate" runat="server" Font-Italic="True">ProjectCreationDate</asp:label>
					</asp:Panel>
					<asp:Panel ID="PanelEditProject" Runat="server">
						<hc:projectInfo id="ProjectInformationControl" runat="server"></hc:projectInfo>
					</asp:Panel>
				</Template>
			</igmisc:webpanel>
			<igmisc:webpanel id="wPanelMarket" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img">
				<Header Text="Marketing information" TextAlignment="Left">
					<ExpandedAppearance>
            <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
					<ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
          <CollapsedAppearance>
            <Style CssClass="hc_webpanelcol"></Style>
          </CollapsedAppearance>
				</Header>
				<Template>				
					<asp:Panel id="PanelViewMarketSegments" Runat="server">
					  <table border='1' cellpadding='1' style='border-collapse:collapse;width:90%' cellspacing='0'>
				   		<tr valign="top">
								<td class='editLabelCell' style='font-weight:bold;width:130px'><asp:Label id="Label12" runat="server">Defined market segments</asp:Label></td>
								<td class='editValueCell'><asp:Label id="lbDefinedMarketSegment" runat="server"></asp:Label></td>
							</tr>
							<tr valign="top">
								<td class='editLabelCell' style="font-weight:bold;width:130px"><asp:Label id="Label14" runat="server">Applied Market segments</asp:Label></td>
								<td class='editValueCell'><asp:Label id="lbAppliedMarketSegment" runat="server"></asp:Label></td>
							</tr>
					  </table>
					</asp:Panel>
					<asp:Panel id="PanelEditMarketSegments" Runat="server">
					  <table border="0" cellpadding="0" cellspacing="10">
              <tr valign="top">
                <td>
                  <asp:CheckBox runat="server" ID="cbNotInheritedMS" Text="Not inherited market segments by fallback and inheritance mechanisms" />
                </td>
              </tr>
						  <tr valign="top">
							  <td>
								  <igtbl:ultrawebgrid id="dg" runat="server">
									  <DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" HeaderClickActionDefault="SortSingle"
										  RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
                                          <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
										  <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true"> <%--CssClass="gh">--%>
											  <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
										  </HeaderStyleDefault>
										  <FrameStyle CssClass="dataTable">
                                            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                          </FrameStyle>
										  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
										  <ActivationObject AllowActivation="False"></ActivationObject>
										  <RowAlternateStyleDefault CssClass="uga">
                                            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                          </RowAlternateStyleDefault>
										  <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                          </RowStyleDefault>
									  </DisplayLayout>
									  <Bands>
										  <igtbl:UltraGridBand BaseTableName="" Key="" DataKeyField="">
											  <Columns>
												  <igtbl:UltraGridColumn Width="50px"></igtbl:UltraGridColumn>
												  <igtbl:UltraGridColumn HeaderText="Segment" Width="95px"></igtbl:UltraGridColumn>
												  <igtbl:UltraGridColumn HeaderText="Country" Width="50px"></igtbl:UltraGridColumn>
											  </Columns>
										  </igtbl:UltraGridBand>
									  </Bands>
								  </igtbl:ultrawebgrid>
							  </td>
						  </tr>
					  </table>
					</asp:Panel>
				</Template>
			</igmisc:webpanel>
      <igmisc:webpanel id="wPanelPublisher" runat="server" CssClass="hc_webpanel" ExpandEffect="None" ImageDirectory="/hc_v4/img">
				<Header Text="Exclusive publishers" TextAlignment="Left">
					<ExpandedAppearance>
            <Style CssClass="hc_webpanelexp"></Style>
					</ExpandedAppearance>
					<ExpansionIndicator AlternateText="Expand/Collapse" CollapsedImageUrl="ed_dt.gif" ExpandedImageUrl="ed_upt.gif"></ExpansionIndicator>
          <CollapsedAppearance>
            <Style CssClass="hc_webpanelcol"></Style>
          </CollapsedAppearance>
				</Header>
				<Template>				
					<asp:Panel id="PanelViewPublishers" Runat="server">
					  <asp:Label id="lbP" runat="server">Publishers</asp:Label>
					</asp:Panel>
					<asp:Panel id="PanelEditPublishers" Runat="server">
					  <table border="0" cellpadding="0" cellspacing="10">
              <tr valign="top">
                <td>
                  <asp:CheckBox runat="server" ID="cbNotInheritedP" Text="Not inherited publishers by fallback mechanism" />
                </td>
              </tr>
						  <tr valign="top">
							  <td>
								  <igtbl:ultrawebgrid id="dgEditPublisher" runat="server">
									  <DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" HeaderClickActionDefault="SortSingle"
										  RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
                                          <%--Removed the css class and added inline property to fix grid line issue by Radha S--%>
										  <HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" BackColor="LightGray" Cursor="Hand" Font-Bold="true"> <%--CssClass="gh">--%>
											  <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
										  </HeaderStyleDefault>
										  <FrameStyle CssClass="dataTable">
                                            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                          </FrameStyle>
										  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
										  <ActivationObject AllowActivation="False"></ActivationObject>
										  <RowAlternateStyleDefault CssClass="uga">
                                            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                          </RowAlternateStyleDefault>
										  <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                            <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                          </RowStyleDefault>
									  </DisplayLayout>
									  <Bands>
										  <igtbl:UltraGridBand BaseTableName="" Key="" DataKeyField="">
											  <Columns>
												  <igtbl:UltraGridColumn HeaderText="" Key="" Width="50px" BaseColumnName=""></igtbl:UltraGridColumn>
												  <igtbl:UltraGridColumn HeaderText="Publisher" Key="" Width="95px" BaseColumnName=""></igtbl:UltraGridColumn>
												  <igtbl:UltraGridColumn HeaderText="Country" Width="50px"></igtbl:UltraGridColumn>
											  </Columns>
										  </igtbl:UltraGridBand>
									  </Bands>
								  </igtbl:ultrawebgrid>
							  </td>
						  </tr>
					  </table>
					</asp:Panel>
				</Template>
			</igmisc:webpanel>
		<script src="/hc_v4/js/hc_tooltip.js" type="text/javascript"></script>
</asp:Content>