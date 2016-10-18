<%@ Register TagPrefix="hc" TagName="plcInfo" Src="controlPLC.ascx"%>
<%@ Register TagPrefix="hc" TagName="sortInfo" Src="controlItemSort.ascx"%>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.Migrated_QDE_AddItem" CodeFile="QDE_AddItem.aspx.cs"%> 
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Add Item Wizard</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
  <script type="text/javascript" src="../../../ajaxcontrols/Extensions.js"></script>
  <script type="text/javascript" src="./Synchronize.js"></script>
	<style type="text/css">
	  .LstSelectedItem { BACKGROUND-COLOR: orange }
  </style>
	<script type="text/javascript" language="javascript">
	//QC2828 Starts
	function TrackCountQDEPage(fieldObj,countFieldName,maxChars)
	{
	 var countField = document.getElementById(countFieldName);
   if (countField){
     var prevText = countField.innerHTML;
     var diff = maxChars - fieldObj.value.length;
     if (diff >=0 && diff < maxChars/10){;
       diff = '<font color=red>' + diff + '</font>';
     }
     if (diff < 0){ 
       countField.innerHTML = '<b><font color=red>Over the length limit (' + fieldObj.value.length + 'chars, max: ' + maxChars +' chars)!!</font></b>';
     }  
     else{
       countField.innerHTML = '<b>' + diff + '</b> char(s) left';
     }
    }
	}
	//QC2828 Ends
	
    function ReloadAndClose()
    { 
      if (opener)
        opener.ReloadFrames(window);
      else
        window.close();   
    }
    
    function UpdateMS(cbMS)
    {
      var cb = document.getElementById(cbMS);
      if (cb)
        cb.checked = true;
    }
    
	  var bundlePLCWarning = false;
	  var bundlePLCError = false;
	  var isProcessing = false;
	  ///////////////////////////////////////////////////////
	  function uwToolbar_Click(oToolbar, oButton, oEvent){
	  ///////////////////////////////////////////////////////
      if (isProcessing) 
      {
        alert('Please wait...');
        oEvent.cancelPostBack = true;
      }
      else{
        if (typeof(Page_Validators) != 'undefined')
        {
          for( var i = 0; i < Page_Validators.length; ++i) {
            ValidatorEnable( Page_Validators[i], (oButton.Key == 'Apply' || oButton.Key == 'Next'));
          }
        }
	      if (oButton.Key == 'Apply' || oButton.Key == 'Next') {
          oEvent.cancelPostBack = MandatoryFieldMissing();
          if (bundlePLCError){
            alert('The system detected PLC errors\nYou cannot proceed');
            oEvent.cancelPostBack = true;
          }
          else{
            if (bundlePLCWarning){
              oEvent.cancelPostBack = !confirm('The system detected PLC inconsistencies\nAre you sure you want to proceed anyway?');
            }
          }
          isProcessing = !oEvent.cancelPostBack;
        }
        else if (oButton.Key == 'Cancel')
        {
          oEvent.cancelPostBack = true;
          window.close();
	      }
	       else if (oButton.Key == 'Prev')
        {
          oEvent.cancelPostBack = false;
	      }
	    }
	  }
    ////////////////////////////////////////
	  function CleanSku(text, re){
    ////////////////////////////////////////
	    return new String(text).toUpperCase().replace(re, '');
		}
    var strRuleName, strRuleSku;
  	var normalSku = /([^0-9A-Z#-])/g;
		var listSku = /([^0-9A-Z#\-\,])/g;

	function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dgPLCMainReport");
              }
        function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dgPLCComponentsReport");
              }
  </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
	<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbartitle" ItemWidthDefault="80px"
		width="100%">
		<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
		<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
		<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
		<ClientSideEvents></ClientSideEvents>
		<Items>
			<igtbar:TBLabel Text="ItemName" Key="ItemName">
				<DefaultStyle Width="100%" Font-Size="9pt" Font-Bold="True" TextAlign="Left"></DefaultStyle>
			</igtbar:TBLabel>
		</Items>
	</igtbar:ultrawebtoolbar>
	<igtbar:ultrawebtoolbar  id="Ultrawebtoolbar1" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="Ultrawebtoolbar1_ButtonClicked">
		<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
		<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
		<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
		<Items>
			<igtbar:TBarButton Key="Prev" ToolTip="Return to previous Step" Text="Previous" Image="/hc_v4/img/ed_Prev.gif"></igtbar:TBarButton>
			<igtbar:TBarButton ImageAlign="Right" Key="Next" ToolTip="Go to next Step" Text="Next" Image="/hc_v4/img/ed_Next.gif"></igtbar:TBarButton>
			<igtbar:TBarButton Key="Apply" ToolTip="Add Item" Text="Apply" Image="/hc_v4/img/ed_savefinal.gif"></igtbar:TBarButton>
			<igtbar:TBSeparator Key="ApplySep"></igtbar:TBSeparator>
			<igtbar:TBarButton Key="Cancel" ToolTip="Close window" Text="Cancel" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
		</Items>
		<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
	</igtbar:ultrawebtoolbar>
	<table class="datatable" cellspacing="0" cellpadding="4" width="100%" border="0">
		<tr>
			<td class="sectionTitle" colspan="2"><b><asp:label id="lbWizardTitle" Runat="server">Summary</asp:label></b></td>
		</tr>
	</table>
	<!-- Panel Wizard -->
	<asp:panel id="PanelSummary" Runat="server">
		<br/>
		<div style="font-size:x-small">
		<asp:Label id="lbSummary" Runat="server">Summary Text</asp:Label>
		</div>
		<br/>
		<br/>
		<asp:Label id="Label1" Runat="server">Press the apply button to create your product.</asp:Label>
		<br/>
		<asp:Label id="Label2" Runat="server">This window will be closed automatically. </asp:Label>
	</asp:panel>
	
	<!-- Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
	<!-- Since project will be maintained from Regions, it cannot be added while item creation -->
	<%-- 
	<asp:panel id="PanelProject" Runat="server">
		<hc:projectInfo id="ProjectInformationControl" runat="server"></hc:projectInfo>
	</asp:panel>
	--%>
	<asp:panel id="PanelBundlePLC" Runat="server">
		<table cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td class="SectionSubSubTitle">
					<asp:Label id="lbBundleMainComponent" Runat="server">Main Component Name</asp:Label></td>
			</tr>
		</table>
		<igtbl:ultrawebgrid id="dgPLCMainReport" runat="server" Height="120px" ImageDirectory="/hc_v4/ig/Images/"
			Width="100%" OnInitializeRow="dgPLCMainReport_InitializeRow">
			<DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" AllowRowNumberingDefault="Continuous"
				Name="dgPLCMainReport" TableLayout="Fixed" CellClickActionDefault="Edit" NoDataMessage="No data to display">
				<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
					<BorderDetails StyleBottom="Solid" WidthRight="0pt" StyleRight="Solid" WidthBottom="0pt"></BorderDetails>
				</HeaderStyleDefault>
				<FrameStyle Width="100%" Height="120px" CssClass="dataTable"></FrameStyle>
				<ActivationObject AllowActivation="False"></ActivationObject>
				<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px"></RowStyleDefault>
			</DisplayLayout>
			<Bands>
				<igtbl:UltraGridBand BaseTableName="Items" Key="Items" DataKeyField="ItemId">
					<Columns>
						<igtbl:UltraGridColumn HeaderText="Error" Key="Error" DataType="System.Int32" BaseColumnName="Error" ServerOnly="True">
							<Footer Key="Error"></Footer>
							<Header Key="Error" Caption="Error"></Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Sku" Key="ItemNumber" Width="40px" BaseColumnName="ItemNumber" ServerOnly="True">
							<Footer Key="ItemNumber">
								<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="ItemNumber" Caption="Sku">
								<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Country" Key="Country" Width="200px" BaseColumnName="CountryCode">
							<Footer Key="Country">
								<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Country" Caption="Country">
								<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="CountryName" Key="CountryName" BaseColumnName="CountryName" ServerOnly="True">
							<Footer Key="CountryName">
								<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="CountryName" Caption="CountryName">
								<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="" Key="Status" Width="30px" BaseColumnName="" CellMultiline="Yes">
							<CellStyle HorizontalAlign="Center" Wrap="True"></CellStyle>
							<Footer Key="Status">
								<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Status" Caption="">
								<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="" CellMultiline="Yes">
							<CellStyle Wrap="True"></CellStyle>
							<Footer Key="Description">
								<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Description" Caption="Description">
								<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
					</Columns>
				</igtbl:UltraGridBand>
			</Bands>
		</igtbl:ultrawebgrid>
		<br/>
		<table cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td class="SectionSubSubTitle">
					<asp:Label id="lbBundleComponents" Runat="server">Components</asp:Label></td>
			</tr>
		</table>
		<igtbl:ultrawebgrid id="dgPLCComponentsReport" runat="server" Height="120px" ImageDirectory="/hc_v4/ig/Images/"
			Width="100%" OnInitializeRow="dgPLCComponentsReport_InitializeRow">
			<DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" AllowRowNumberingDefault="Continuous"
				Name="dgPLCComponentsReport" TableLayout="Fixed" CellClickActionDefault="Edit" NoDataMessage="No data to display">
				<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
					<BorderDetails StyleBottom="Solid" WidthRight="0pt" StyleRight="Solid" WidthBottom="0pt"></BorderDetails>
				</HeaderStyleDefault>
				<FrameStyle Width="100%" Height="120px" CssClass="dataTable"></FrameStyle>
				<ActivationObject AllowActivation="False"></ActivationObject>
				<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px"></RowStyleDefault>
			</DisplayLayout>
			<Bands>
				<igtbl:UltraGridBand BaseTableName="Items" Key="Items" DataKeyField="ItemId">
					<Columns>
						<igtbl:UltraGridColumn HeaderText="Error" Key="Error" DataType="System.Int32" BaseColumnName="Error" ServerOnly="True">
							<Footer Key="Error"></Footer>
							<Header Key="Error" Caption="Error"></Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Sku" Key="ItemNumber" Width="40px" BaseColumnName="ItemNumber" ServerOnly="True">
							<Footer Key="ItemNumber">
								<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="ItemNumber" Caption="Sku">
								<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Name" Key="Name" Width="300px" BaseColumnName="ItemName" CellMultiline="Yes">
							<CellStyle Wrap="True"></CellStyle>
							<Footer Key="Name">
								<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Name" Caption="Name">
								<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Country" Key="Country" MergeCells="True" Width="200px" BaseColumnName="CountryCode">
							<Footer Key="Country">
								<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Country" Caption="Country">
								<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="CountryName" Key="CountryName" BaseColumnName="CountryName" ServerOnly="True">
							<Footer Key="CountryName">
								<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="CountryName" Caption="CountryName">
								<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="" Key="Status" Width="18px" BaseColumnName="" CellMultiline="Yes">
							<CellStyle HorizontalAlign="Center" Wrap="True"></CellStyle>
							<Footer Key="Status">
								<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Status" Caption="">
								<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
						<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="" CellMultiline="Yes">
							<CellStyle Wrap="True"></CellStyle>
							<Footer Key="Description">
								<RowLayoutColumnInfo OriginX="6"></RowLayoutColumnInfo>
							</Footer>
							<Header Key="Description" Caption="Description">
								<RowLayoutColumnInfo OriginX="6"></RowLayoutColumnInfo>
							</Header>
						</igtbl:UltraGridColumn>
					</Columns>
				</igtbl:UltraGridBand>
			</Bands>
		</igtbl:ultrawebgrid>
	</asp:panel>
	<asp:panel id="PanelMainInfo" Runat="server">
		<table class="datatable" cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<!-- Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
	                    <!-- Since project will be maintained from Regions, it cannot be added while item creation -->
	                    <%-- 
						<asp:panel id="panelProjectYesOrNo" Runat="server">
						  <tr valign="top">
							  <td class="editLabelCell" style="width: 150px; height: 21px"><b>Project</b></td>
							  <td class="ugd" style="height: 21px">
								  <asp:RadioButtonList id="rdProject" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
									  <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
									  <asp:ListItem Value="Yes">Yes</asp:ListItem>
								  </asp:RadioButtonList>
							  </td>
						  </tr>
						</asp:panel>
						--%>
					  <tr>
						  <td class="editLabelCell" style="width: 150px; height: 15px"><b>Level</b></td>
						  <td class="ugd" style="height: 15px">
							  <asp:dropdownlist id="cbLevels" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="True" onselectedindexchanged="cbLevels_SelectedIndexChanged"></asp:dropdownlist>
						  </td>
					  </tr>
						<tr valign="top">
							<td class="editLabelCell" style="width: 150px; height: 21px"><b>Name</b></td>
							<td class="ugd" style="height: 21px">
								<asp:textbox id="txtProductName" runat="server" Columns="50" MaxLength="500"></asp:textbox>
								  <a onmouseover="doTooltip(event,strRuleName)" onmouseout="hideTip()" href="javascript://" style="border-width: 0px">
									  <img alt="" class="middle" src="/hc_v4/img/ed_info.gif" border="0" style="vertical-align: top" />
									</a>
								<span id="textcount">&nbsp;</span>
								<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtProductName"
									ErrorMessage="Required"></asp:requiredfieldvalidator>
							</td>
						</tr>
						<asp:panel id="panelOption" Runat="server" Visible="false">
							<tr valign="top">
								<td class="editLabelCell" style="width: 150px; height: 18px"><b>Option</b></td>
								<td class="ugd" style="height: 18px">
									<asp:Label ID="lbSku" runat="server"></asp:Label><asp:textbox id="txtOption" runat="server" Width="50px" Columns="6" MaxLength=6></asp:textbox>
									<span id="Span1">&nbsp;</span>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" Display="Dynamic" ControlToValidate="txtOption"
										ErrorMessage="Required"></asp:requiredfieldvalidator></td>
							</tr>
            </asp:panel>
						<asp:panel id="panelSku" Runat="server">
							<tr valign="top">
								<td class="editLabelCell" style="width: 150px; height: 18px"><b>Sku</b></td>
								<td class="ugd" style="height: 18px">
									<asp:textbox id="txtSku" runat="server" Width="112px" Columns="50" MaxLength="50"></asp:textbox>
									<a onmouseover="doTooltip(event,strRuleSku)" onmouseout="hideTip()" href="javascript://" style="border-width: 0px">
									  <img alt="" class="middle" src="/hc_v4/img/ed_info.gif" border="0" style="vertical-align: top" />
									</a>
									<span id="textcountSku">&nbsp;</span>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Display="Dynamic" ControlToValidate="txtSku"
										ErrorMessage="Required"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="editLabelCell" style="width: 150px"><b>Type</b></td>
								<td class="ugd">
									<asp:dropdownlist id="cbItemType" runat="server" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="cbItemType_SelectedIndexChanged" AutoPostBack="true"></asp:dropdownlist></td>
							</tr>
				      <tr valign="top">
				        <td class="editLabelCell" style="width: 150px; height: 21px"><b>Product Line</b></td>
					      <td class="ugd" style="height: 21px">
						      <asp:DropDownList id="ddlPL" runat="server"></asp:DropDownList>
									<asp:requiredfieldvalidator id="rvPL" runat="server" Display="Dynamic" ControlToValidate="ddlPL"
									  ErrorMessage="PL is Required"></asp:requiredfieldvalidator>
									  <asp:Label ID="txtPLInfo" runat="Server" Visible="false">PL code will be replicated from main component</asp:Label>
					      </td>
				      </tr>
				      <asp:Panel ID="panelPublishers" runat="server">
				        <tr valign="top">
					        <td class="editLabelCell" style="width: 150px; height: 21px"><b>Publishers</b></td>
					        <td class="ugd" style="height: 21px">
						        <asp:CheckBoxList id="cblPublishers" Runat="server" DataTextField="Name" DataValueField="Id" RepeatColumns="2"
							        cellpadding="2"></asp:CheckBoxList>
					        </td>
				        </tr>
				      </asp:Panel>
			      </asp:panel>
			      <asp:Panel ID="panelMarketSegments" runat="server">
			        <tr valign="top">
				        <td class="editLabelCell" style="width: 150px; height: 21px"><b>Market Segment</b></td>
				        <td class="ugd" style="height: 21px">
				          <asp:CheckBox runat="server" ID="cbNotInheritedMS" Text="Not inherited market segments" />
					        <asp:CheckBoxList id="cblMarketSegments" runat="server" DataTextField="Name" DataValueField="Id" RepeatColumns="2" cellpadding="2" />
						    </td>
			        </tr>
			      </asp:Panel>
			      <asp:Panel ID="panelRetailOnly" runat="server">
				      <tr valign="top">
					      <td class="editLabelCell" style="width: 150px; height: 21px"><b>Retail only</b></td>
					      <td class="ugd" style="height: 21px">&nbsp;
						      <asp:CheckBox ID="cbRetailOnly" runat="server" cellpadding="2"></asp:CheckBox>
					      </td>
				      </tr>
			      </asp:Panel>
		      </table>
				</td>
			</tr>
		</table>
	</asp:panel>
	<asp:panel id="PanelPLC" Runat="server">
	<asp:Label ID="lblNote" runat="server"><br><font color='red' style='font-weight:bold; font-size:14px; font-style:italic;'>Note:</font> Product Life Cycle information is now imported into Crystal via PMG. So user cannot provide PLC dates.<br></asp:Label><br />
	  <hc:plcInfo id="PLCInformationControl" runat="server"></hc:plcInfo>
	</asp:panel>
	<asp:panel id="PanelSortInfo" Runat="server">
		<hc:sortInfo id="SortInfoControl" runat="server"></hc:sortInfo>
	</asp:panel>
	<asp:panel id="PanelBundle" Runat="server">
		<table class="datatable" cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td class="editLabelCell"><b>Main component</b></td>
			</tr>
			<tr>
				<td class="ugd" style="height: 19px">
					<asp:dropdownlist id="cbMainComponent" runat="server" DataTextField="Name" DataValueField="Id" AutoPostBack="True" onselectedindexchanged="cbMainComponent_SelectedIndexChanged"></asp:dropdownlist>
			  </td>
			</tr>
			<asp:panel id="panelBundleComponents" Runat="server" Visible="False">
				<tr>
					<td class="editLabelCell"><b>Components of the bundle</b></td>
				</tr>
				<tr>
					<td>
						<script type="text/javascript" language="javascript">
              ////////////////////////////////////////
              function txtBundleComponents_TextChanged(oEdit, newText, oEvent){
              ////////////////////////////////////////
                oEdit.setText(CleanSku(newText, listSku));
	            }		
              ////////////////////////////////////////
              function txtBundleComponents_Blur(oEdit, text, oEvent){
              ////////////////////////////////////////
                oEdit.setText(CleanSku(text, listSku));
              }		  
            </script>
						<table border="0">
							<tr valign="middle">
								<td>Provide the skus, separated with commas:</td>
								<td>
									<igtxt:WebTextEdit id="txtBundleComponents" runat="server" Width="200px" HideEnterKey="True" MaxLength="50">
										<ClientSideEvents TextChanged="txtBundleComponents_TextChanged" Blur="txtBundleComponents_Blur"></ClientSideEvents>
									</igtxt:WebTextEdit>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="txtBundleComponents"
										ErrorMessage="Required"></asp:requiredfieldvalidator></td>
								<td>
									<asp:Button id="BtnAnalyzeComponents" runat="server" Text="Analyze" onclick="BtnAnalyzeComponents_Click"></asp:Button>
								</td>
							</tr>
						</table>
						<igtbl:ultrawebgrid id="dgComponents" runat="server" ImageDirectory="/hc_v4/ig/Images/" Width="100%"
							Visible="False" OnInitializeRow="dgComponents_InitializeRow">
							<DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" HeaderClickActionDefault="SortSingle"
								RowSelectorsDefault="No" Name="dgComponents" TableLayout="Fixed" NoDataMessage="No data to display">
								<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
								<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
								<ActivationObject AllowActivation="False"></ActivationObject>
								<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand BaseTableName="Items" Key="Items" BorderCollapse="NotSet" DataKeyField="ItemId">
									<Columns>
										<igtbl:UltraGridColumn HeaderText="ItemId" Key="ItemId" Width="50px" Hidden="True" BaseColumnName="ItemId">
											<Footer Key="ItemId"></Footer>
											<Header Key="ItemId" Caption="ItemId"></Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Class" Key="ClassName" Width="95px" BaseColumnName="ClassName" CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
											<Footer Key="ClassName">
												<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="ClassName" Caption="Class">
												<RowLayoutColumnInfo OriginX="1"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Sku" Key="ItemNumber" Width="40px" BaseColumnName="ItemNumber" ServerOnly="True"
											CellMultiline="No">
											<CellStyle Font-Bold="True"></CellStyle>
											<Footer Key="ItemNumber">
												<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="ItemNumber" Caption="Sku">
												<RowLayoutColumnInfo OriginX="2"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" Width="250px" BaseColumnName="ItemName" CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
											<Footer Key="ItemName">
												<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="ItemName" Caption="Name">
												<RowLayoutColumnInfo OriginX="3"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="IsRelevant" Key="IsRelevant" Width="50px" BaseColumnName="IsRelevant"
											ServerOnly="True">
											<Footer Key="IsRelevant">
												<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="IsRelevant" Caption="IsRelevant">
												<RowLayoutColumnInfo OriginX="4"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Error" Key="Error" BaseColumnName="Error" ServerOnly="True">
											<Footer Key="Error">
												<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="Error" Caption="Error">
												<RowLayoutColumnInfo OriginX="5"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn HeaderText="Status" Key="Status" Width="100%" BaseColumnName="Status" ServerOnly="True"
											CellMultiline="Yes">
											<CellStyle Wrap="True"></CellStyle>
											<Footer Key="Status">
												<RowLayoutColumnInfo OriginX="6"></RowLayoutColumnInfo>
											</Footer>
											<Header Key="Status" Caption="Status">
												<RowLayoutColumnInfo OriginX="6"></RowLayoutColumnInfo>
											</Header>
										</igtbl:UltraGridColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
						<asp:Label id="LbNbComponents" Runat="server" ForeColor="Green">_NbBundleComponents</asp:Label>
					</td>
				</tr>
			</asp:panel>
		</table>
	</asp:panel>
	<center>
	  <asp:label id="lbError" runat="server" CssClass="hc_error">Result message</asp:label>
	</center>
	<br />
	<br />
  <table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
      <td align="center" id="tdCreatedItem"><asp:Label ID="lbCreatedItem" runat="server" Visible="false">Item is being created...<img src='/hc_v4/img/ed_wait.gif'/></asp:Label></td>
    </tr>
  </table>
	<script src="/hc_v4/js/hc_tooltip.js" type="text/javascript"></script>
</asp:Content>