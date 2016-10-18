<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="container_Properties" CodeFile="container_properties.aspx.cs" validateRequest="false"%>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ajax" TagName="Lookup" Src="~/ajaxcontrols/LookUp.ascx"%>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
			<script type="text/javascript" src="../../../ajaxcontrols/LookupStaticTerm.js"></script>
			<script type="text/javascript" language="javascript">
			var strInheritanceMethodInfo='';
			
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
				if (oButton.Key == 'List') 
				{
					UpdateTabs(-1);
					oEvent.cancelPostBack = true;
				}
				if (oButton.Key == 'Save') {
					oEvent.cancelPostBack = MandatoryFieldMissing();
				}
				if (oButton.Key == 'Delete') 
				{
					oEvent.cancelPostBack = !confirm("Are you sure you want to delete this container ?");
				}
				if (oButton.Key == 'RePublish')
		        {
		            if (confirm('Do you really want to Republish ?'))
		                {
                        oEvent.needPostBack = true;
                        } 
                    else 
                        {
                            oEvent.needPostBack = false;
                        }
               }
			} 
			var groupIndex;
			
		  function UpdateTabsAfterUpdate(containerId)
			{
				if (parent)
				{
					if (containerId > -1)
						parent.document.location = "../Containers.aspx?c="+containerId+"&u=1";
					else
						parent.document.location = "../Containers.aspx?u=1";
				}
			}
			
			function UpdateTabs(containerId)
			{
				if (parent)
				{
					if (containerId > -1)
						parent.document.location = "../Containers.aspx?c="+containerId;
					else
						parent.document.location = "../Containers.aspx";
				}
			}
			</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="110px" CssClass="hc_toolbar" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" ToolTip="Delete from library" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
                                <igtbar:TBSeparator Key="RePublishSep"></igtbar:TBSeparator>
                                <igtbar:TBarButton Key="RePublish"  ToolTip="PDB Republish"  Text="PDB Republish" Image="/hc_v4/img/ed_refresh.gif"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td>
									<asp:hyperlink id="hlCreator" runat="server"></asp:hyperlink>
									<asp:hyperlink id="hlModifier" runat="server"></asp:hyperlink>
								</td>
							</tr>
							<tr>
								<td>
									<table cellspacing="0" cellpadding="0" width="100%" border="0">
									  <tr>
										  <td class="editLabelCell" width="150px" valign="top"><asp:label id="Label7" runat="server">Container group</asp:label></td>
                      <td class="uga">
                        <asp:DropDownList ID="ddlContainerGroups" runat="server" DataTextField="Path" Width="400px">
                        </asp:DropDownList>
								      </td>
							      </tr>
							      <asp:panel id="panelId" Runat="server" Visible="False">
								      <tr valign="middle">
									      <td class="editLabelCell" width="150px"><asp:Label id="Label13" runat="server">ContainerId</asp:Label></td>
									      <td class="ugd"><asp:textbox id="txtContainerId" runat="server" width="50px" Enabled="false"></asp:textbox></td>
								      </tr>
							      </asp:panel>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="label1" runat="server">Tag</asp:label></td>
								      <td class="uga">
									      <asp:textbox id="txtTag" runat="server" width="180px" MaxLength="50"></asp:textbox>
									      <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" CssClass="errorMessage" ErrorMessage="*"
										      ControlToValidate="txtTag">
										    </asp:requiredfieldvalidator>
										    <asp:RegularExpressionValidator id="regTag" runat="server" ErrorMessage="Invalid tag (tag can contain only lower case characters, numerics and points)"
                          Display="Dynamic" ValidationExpression="[.a-z0-9_]+" ControlToValidate="txtTag">
                        </asp:RegularExpressionValidator>
								      </td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label2" runat="server">Name</asp:label></td>
								      <td class="ugd">
									      <asp:textbox id="txtContainerName" runat="server" width="272px" MaxLength="80"></asp:textbox>
									      <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="errorMessage" ErrorMessage="*"
										      ControlToValidate="txtContainerName"></asp:requiredfieldvalidator></td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label19" runat="server">Label</asp:label></td>
								      <td class="uga">
									      <ajax:Lookup ID="cbLabel" runat="server" lookupservice="LookupStaticTerm" pageproperty="Value" nosubmit="true" width="400" />
									      <asp:TextBox id="txtLabel" runat="server" Width="400" Visible="False" Enabled="False"></asp:TextBox>
									      <asp:requiredfieldvalidator id="rvLabel" runat="server" CssClass="errorMessage" ErrorMessage="*"  EnableClientScript="false"
										      ControlToValidate="txtLabel"></asp:requiredfieldvalidator>
								      </td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label3" runat="server">Definition</asp:label></td>
								      <td class="ugd">
									      <asp:textbox id="txtDefinition" runat="server" Visible="true" Columns="60" rows="2" TextMode="MultiLine"
										      MaxLength="300"></asp:textbox>
									      <asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="errorMessage" ErrorMessage="*"
										      ControlToValidate="txtDefinition"></asp:requiredfieldvalidator></td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label14" runat="server">Business rule</asp:label></td>
								      <td class="uga">
									      <asp:textbox id="txtEntryRule" runat="server" Visible="true" Columns="60" rows="2" TextMode="MultiLine"
										      MaxLength="800"></asp:textbox>
									      <asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" CssClass="errorMessage" ErrorMessage="*"
										      ControlToValidate="txtEntryRule"></asp:requiredfieldvalidator></td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label4" runat="server">Example</asp:label></td>
								      <td class="ugd">
									      <asp:textbox id="txtSample" runat="server" Visible="true" Columns="60" rows="2" TextMode="MultiLine"
										      MaxLength="1000"></asp:textbox>
									      <asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" CssClass="errorMessage" ErrorMessage="*"
										      ControlToValidate="txtsample"></asp:requiredfieldvalidator></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px"><asp:label id="Label6" runat="server">Data type</asp:label></td>
								      <td class="uga"><asp:dropdownlist id="dlDataType" runat="server" width="180px" DataTextField="Name" DataValueField="Code" AutoPostBack="True" OnSelectedIndexChanged="dlDataType_SelectedIndexChanged"></asp:dropdownlist></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px"><asp:label id="Label5" runat="server">Container type</asp:label></td>
								      <td class="ugd"><asp:dropdownlist id="dlContainerType" runat="server" width="180px" DataTextField="Name" DataValueField="Code"></asp:dropdownlist></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 18px"><asp:label id="Label11" runat="server">Lookup table</asp:label></td>
								      <td class="uga"><asp:dropdownlist id="dlLookupField" runat="server" width="180px" DataTextField="Name" DataValueField="Id"></asp:dropdownlist>
								      <asp:label id="lbLookupField" runat="server" Visible="false">(not relevant for this data type)</asp:label></td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label17" runat="server">Input mask</asp:label></td>
								      <td class="ugd"><asp:textbox id="txtInputMask" runat="server" Visible="true" MaxLength="100" Width="320px"></asp:textbox>
								      <asp:label id="lbInputMask" runat="server" Visible="false">(not relevant for this data type)</asp:label></td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label9" runat="server">Max length</asp:label></td>
								      <td class="uga"><igtxt:webnumericedit id="txtMaxLength" runat="server" DataMode="Int" MinValue="0" MaxValue="3500">
										      <ClientSideEvents ValueChange="txtMaxLength_ValueChange"></ClientSideEvents>
									      </igtxt:webnumericedit>
									      (0 if not relevant)
									      <asp:label id="lbMaxLength" runat="server" Visible="false">(not relevant for this data type)</asp:label>
								      </td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label20" runat="server">Translatable</asp:label></td>
								      <td class="ugd" style="height: 16px"><asp:checkbox id="cbTranslatable" runat="server"></asp:checkbox></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label23" runat="server">Regionalizable</asp:label></td>
								      <td class="uga" style="height: 16px"><asp:checkbox id="cbRegionalizable" runat="server"></asp:checkbox></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label16" runat="server">Localizable</asp:label></td>
								      <td class="ugd" style="height: 16px"><asp:checkbox id="cbLocalizable" runat="server"></asp:checkbox></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label10" runat="server">Publishable</asp:label></td>
								      <td class="uga" style="height: 16px"><asp:checkbox id="cbPublishable" runat="server"></asp:checkbox></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label21" runat="server">UI Read only</asp:label></td>
								      <td class="ugd" style="height: 16px"><asp:checkbox id="cbReadOnly" runat="server" AutoPostBack="True" oncheckedchanged="cbReadOnly_CheckedChanged"></asp:checkbox></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label22" runat="server">Keep if obsolete</asp:label></td>
								      <td class="uga" style="height: 16px"><asp:checkbox id="cbKeepIfObsolete" runat="server"></asp:checkbox></td>
							      </tr>
							       <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label18" runat="server">MMD</asp:label></td>
								      <td class="uga" style="height: 16px"><asp:checkbox id="cbMMD" runat="server"></asp:checkbox></td>
							      </tr>
							       <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label24" runat="server">MECD</asp:label></td>
								      <td class="uga" style="height: 16px"><asp:checkbox id="cbMECD" runat="server"></asp:checkbox></td>
							      </tr>
							       <tr>
								      <td class="editLabelCell" width="150px" style="height: 16px"><asp:label id="Label25" runat="server">CDM</asp:label></td>
								      <td class="uga" style="height: 16px"><asp:checkbox id="cbCDM" runat="server"></asp:checkbox></td>
							      </tr>
							      <tr>
								      <td class="editLabelCell" width="150px" style="height: 25px"><asp:label id="Label12" runat="server">Inheritance method</asp:label></td>
								      <td class="ugd" style="height: 25px">
								        <asp:dropdownlist id="dlInheritanceMethod" runat="server" width="180px" DataTextField="Name" DataValueField="Id"></asp:dropdownlist>
								        <a onmouseover="doTooltip(event,strInheritanceMethodInfo)" onmouseout="hideTip()" href="javascript://" style="border: 0px">
								          <img class="middle" src="/hc_v4/img/ed_info.gif" alt="" style="border: 0px; vertical-align: top" />
								        </a></td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="Label15" runat="server">Sort</asp:label></td>
								      <td class="uga">
									      <igtxt:webnumericedit id="txtSort" runat="server" DataMode="Text" MinValue="0" MaxValue="999" ValueText="0" MaxLength="3"></igtxt:webnumericedit>
									      <asp:regularexpressionvalidator id="Regularexpressionvalidator2" runat="server" ErrorMessage="Max length must be a valid number"
										      ControlToValidate="txtMaxLength" ValidationExpression="^[-+]?\d*"></asp:regularexpressionvalidator></td>
							      </tr>
							      <tr valign="middle" style="visibility:hidden;display:none">
								      <td class="editLabelCell" width="150px"><asp:label id="Label8" runat="server">WWXPath</asp:label></td>
								      <td class="uga">
									      <asp:textbox id="tbWWXPath" runat="server" Visible="true" Width="350px" MaxLength="500"></asp:textbox>
								      </td>
							      </tr>
							      <tr valign="middle">
								      <td class="editLabelCell" width="150px"><asp:label id="segmentLbl" runat="server">Segment</asp:label></td>
								      <td class="ugd">
									      <asp:DropDownList id="ddlSegments" runat="server" DataTextField="SegmentName" DataValueField="SegmentId">
										      <asp:ListItem Value="None">None</asp:ListItem>
										      <asp:ListItem Value="reseller">Reseller</asp:ListItem>
										      <asp:ListItem Value="globalanonymous">Global anonymous</asp:ListItem>
										      <asp:ListItem Value="consumer">Consumer</asp:ListItem>
									      </asp:DropDownList>
								      </td>
							      </tr>
						      </table>
					      </td>
				      </tr>
			      </table>
			    </td>
	      </tr>
	    </table>
		<script src="/hc_v4/js/hc_tooltip.js" type="text/javascript"></script>
</asp:Content>