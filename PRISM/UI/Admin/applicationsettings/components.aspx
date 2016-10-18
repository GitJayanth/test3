<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Admin.Architecture.Components" CodeFile="Components.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Application components</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
  <STYLE>
    .lbl_msgInformation{font-weight:bold;color:green;}
    .lbl_msgWarning{font-weight:bold;color:orange;}
    .lbl_msgError{font-weight:bold;color:red;}
  </STYLE>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
  <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
  <script type="text/javascript" src="../../../ajaxcontrols/AppComponents.js"></script>
  <script type="text/javascript"><!--

		function toolBar_Click(oToolbar, oButton, oEvent){
			if (oButton.Key=="List")
			{
			  Page_Validators = null;
			}
			else if (oButton.Key=="Consistency")
			{
			  oEvent.cancelPostBack = true;
			  var i=0;
			  if (oToolbar.clientId.length >= 11 && oToolbar.clientId.substr(oToolbar.clientId.length - 11,11).toLowerCase() == "maintoolbar")
			  {
      	  var grid = igtbl_getGridById("<%= componentsGrid.ClientID%>");
			    for (i=0;i<grid.Rows.length;i++)
			    {
			      ajax.Start(CheckAction,[grid.Rows.getRow(i).DataKey,grid.Rows.getRow(i).getCellFromKey("Status").Element]);
			    }
			  }
			  else if (oToolbar.clientId.length >= 17 && oToolbar.clientId.substr(oToolbar.clientId.length - 17,17).toLowerCase() == "propertiestoolbar")
			  {
			    var statusTR = document.getElementById("statusTR");
			    statusTR.style.display = '';
      	  ajax.Start(CheckAction,[<%= currentComponent!=null?currentComponent.Id.ToString():"null"%>,statusTR.children[1]]);
      	}
			}
		}

		function batchChanged(stepList,spanDescription)
		{
			if (spanDescription!=null)
			{
				spanDescription.innerHTML =	"";
				if (stepList.selectedIndex>0)
				{
					if (batchDescriptionArray!=null)
						spanDescription.innerHTML += batchDescriptionArray[stepList.selectedIndex-1] + "<br/>";
					if (batchServerArray!=null)
						spanDescription.innerHTML += batchServerArray[stepList.selectedIndex-1] + "<br/>";
/*					if (batchActionArray!=null)
						spanDescription.innerHTML += "Action: " + batchActionArray[stepList.selectedIndex-1] + "<br/>";*/
				}
				if (spanDescription.innerHTML == "")
					spanDescription.innerHTML =	"<br/><br/>";
			}
		}

	  var CheckAction = {
      delay: 100,
      queueMultiple: true,
      prepare: function(options){return options[0];},
      call: proxies.AppComponents.Check,
      finish: function (status,options) {
        onChecked(status,options[1]);
      },
      onException: proxies.alertException
    } // PopupAction
    
    function onChecked(status,output)
    {
      if (output == null)
        window.status = status;
      else
        output.innerHTML = status;
    }
		
--></script>

  <table style="WIDTH: 100%; HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
	<asp:panel id="panelGrid" Runat="server" Visible="true">
		<tr valign="top" style="height:1px">
			<td>
				<igtbar:ultrawebtoolbar id="mainToolBar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator></igtbar:TBSeparator>
						<igtbar:TBarButton Key="Consistency" Text="Check" Image="/hc_v4/img/ed_build.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator></igtbar:TBSeparator>
						<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
						<igtbar:TBLabel Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
					<ClientSideEvents Click="toolBar_Click"/>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="top">
			<td>
				<igtbl:UltraWebGrid id="componentsGrid" runat="server" Width="100%" DataKeyField="Id">
					<DisplayLayout AutoGenerateColumns="False" LoadOnDemand="Manual" AllowSortingDefault="Yes" RowHeightDefault="100%"
						Version="4.00" SelectTypeRowDefault="Single" ViewType="Hierarchical" HeaderClickActionDefault="SortSingle"
						AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="componentsGrid" TableLayout="Fixed" CellClickActionDefault="RowSelect">
					<HeaderStyleDefault VerticalAlign="Middle" Font-Bold="true" Cursor="Hand" BackColor="LightGray" HorizontalAlign="Center" > <%--CssClass="gh"--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
						<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
						<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
						<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
						<SelectedRowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></SelectedRowStyleDefault>
					</DisplayLayout>
					<Bands>
						<igtbl:UltraGridBand DataKeyField="Id">
							<Columns>
								<igtbl:UltraGridColumn HeaderText="Id" Key="Id" BaseColumnName="Id" Width="25px"></igtbl:UltraGridColumn>
								<igtbl:TemplatedColumn HeaderText="Name" Key="Name" BaseColumnName="Name" IsBound="True" Width="200px">
									<CellTemplate>
										<asp:LinkButton id="NameLink" onclick="UpdateGridItem" runat="server">
											<%#Container.Text%>
										</asp:LinkButton>
									</CellTemplate>
								</igtbl:TemplatedColumn>
								<igtbl:UltraGridColumn HeaderText="Type" Key="Type" BaseColumnName="Type"></igtbl:UltraGridColumn>
								<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="450px" BaseColumnName="Description"></igtbl:UltraGridColumn>
								<igtbl:TemplatedColumn HeaderText="Status" Key="Status" Width="100%">
								  <CellTemplate></CellTemplate>
								</igtbl:TemplatedColumn>
							</Columns>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:UltraWebGrid>
			</td>
		</tr>
	</asp:panel>
	<asp:panel id="panelProperties" Visible="false" Runat="server">
		<tr valign="top" style="height:1px">
			<td>
				<igtbar:ultrawebtoolbar id="propertiesToolBar" runat="server" width="100%" ItemWidthDefault="80px" CssClass="hc_toolbar">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
						<igtbar:TBSeparator/>
						<igtbar:TBarButton Key="Save" ToolTip="Save changes" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
						<igtbar:TBarButton Key="Consistency" Text="Check" Image="/hc_v4/img/ed_build.gif"></igtbar:TBarButton>
						<igtbar:TBLabel Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
					<ClientSideEvents Click="toolBar_Click"></ClientSideEvents>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="top">
			<td>
				<asp:Label id="propertiesMsgLbl" style="font-weight:bold;" runat="server"></asp:Label>
				<table cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr valign="middle" id="idRow" runat="server">
						<td class="editLabelCell" style="width:130px">
							Id
						</td>
						<td class="ugd">
							<asp:label id="txtComponentIdValue" runat="server" width="180px"></asp:label>
						</td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" style="width:130px">
							Name
						</td>
						<td class="ugd">
							<asp:textbox id="txtComponentNameValue" runat="server" width="180px"></asp:textbox>
							<asp:RequiredFieldValidator id="valName" runat="server" ControlToValidate="txtComponentNameValue" ErrorMessage="Name field is mandatory." />
						</td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" style="width:130px">
							Description
						</td>
						<td class="ugd">
							<textarea id="txtComponentDescriptionValue" runat="server" style="vertical-align:top;" cols="51" rows="5"></textarea>
						</td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" style="width:130px">
							Version number
						</td>
						<td class="ugd">
							<asp:textbox id="txtComponentVersionNumberValue" runat="server" width="180px"></asp:textbox>
						</td>
					</tr>
					<tr valign="middle">
						<td class="editLabelCell" style="width:130px">
							Type
						</td>
						<td class="ugd">
							<asp:dropdownlist id="txtComponentTypeValue" onChange="typeChanged();" runat="server" width="180px">
								<asp:ListItem Text="Hardware" Value="HW"/>
								<asp:ListItem Text="Database" Value="DB"/>
								<asp:ListItem Text="Web User Interface" Value="UI"/>
								<asp:ListItem Text="Web Service" Value="WS"/>
								<asp:ListItem Text="Batch" Value="BA"/>
							</asp:dropdownlist>
						</td>
					</tr>
					<tr valign="middle" id="uriTR">
						<td class="editLabelCell" style="width:130px">
							URI
						</td>
						<td class="ugd">
							<asp:textbox id="txtComponentURIValue" runat="server" width="180px"></asp:textbox>
						</td>
					</tr>
					<tr valign="middle" id="loginTR">
						<td class="editLabelCell" style="width:130px">
							Login
						</td>
						<td class="ugd">
							<asp:textbox id="txtComponentLoginValue" runat="server" width="180px"></asp:textbox>
						</td>
					</tr>
					<tr valign="middle" id="passwordTR">
						<td class="editLabelCell" style="width:130px">
							Password
						</td>
						<td class="ugd">
							<input type="password" id="txtComponentPasswordValue" runat="server" width="180px"/>
						</td>
					</tr>
					<tr valign="middle" id="optionsTR">
						<td class="editLabelCell" style="width:130px">
							Options
						</td>
						<td class="ugd">
							<asp:textbox id="txtComponentOptionsValue" runat="server" width="180px"></asp:textbox>
						</td>
					</tr>
					<tr valign="middle" id="actionTR">
						<td class="editLabelCell" style="width:130px">
							Action
						</td>
						<td class="ugd">
							<asp:DropDownList id="ComponentActionTypeValue" runat="server" onchange="actionTypeChanged();">
								<asp:ListItem Text="Command line" Value="exe"></asp:ListItem>
								<asp:ListItem Text="SQL Query" Value="sql"></asp:ListItem>
								<asp:ListItem Text="SQL Stored Procedure" Value="sp"></asp:ListItem>
							</asp:Dropdownlist>
							<br/>
							<asp:textbox id="txtComponentActionValue" runat="server" width="180px"></asp:textbox>
						</td>
					</tr>
					<tr valign="middle" id="DBComponentTR">
						<td class="editLabelCell" style="width:130px">
							Database
						</td>
						<td class="ugd">
							<asp:dropdownlist id="txtComponentDBValue" onchange="batchChanged(this,document.getElementById('newDescriptionDB'));" runat="server"></asp:dropdownlist><br/>
							<span id="newDescriptionDB"><br/></span>
						</td>
					</tr>
					<tr valign="middle" id="HWComponentTR">
						<td class="editLabelCell" style="width:130px">
							Hardware
						</td>
						<td class="ugd">
							<asp:dropdownlist id="txtComponentHWValue" onchange="batchChanged(this,document.getElementById('newDescriptionHW'));" runat="server"></asp:dropdownlist><br/>
							<span id="newDescriptionHW"><br/></span>
						</td>
					</tr>
					<tr valign="middle" id="uiGroupByTR">
						<td class="editLabelCell" style="width:130px">
							Logs grouped by
						</td>
						<td class="ugd">
							<asp:textbox id="txtComponentGroupByValue" runat="server" width="180px"></asp:textbox>
						</td>
					</tr>
					<tr valign="middle" id="statusTR" style="display:none;">
						<td class="editLabelCell" style="width:130px">
							Status
						</td>
						<td class="ugd">
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</asp:panel>
	  </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HOJSBottom" Runat="Server">
		<script type="text/javascript"><!--
function typeChanged()
{

	var uriTR = document.getElementById("uriTR");
	var loginTR = document.getElementById("loginTR");
	var passwordTR = document.getElementById("passwordTR");
	var optionsTR = document.getElementById("optionsTR");
	var actionTR = document.getElementById("actionTR");
	var uiGroupByTR = document.getElementById("uiGroupByTR");
	var DBComponentTR = document.getElementById("DBComponentTR");
	var HWComponentTR = document.getElementById("HWComponentTR");
	if (txtComponentTypeValue!=null)
	{
		var typeValue = txtComponentTypeValue.options[txtComponentTypeValue.selectedIndex].value;

		setVisibility(uriTR,true);
		setVisibility(loginTR,false);
		setVisibility(passwordTR,false);
		setVisibility(optionsTR,false);
		setVisibility(actionTR,false);
		setVisibility(uiGroupByTR,false);
		setVisibility(DBComponentTR,false);
		setVisibility(HWComponentTR,false);
		switch (typeValue)
		{
			case "BA":
				setVisibility(optionsTR,true);
				setVisibility(actionTR,true);
				setVisibility(uiGroupByTR,true);
				actionTypeChanged();
				break;
			case "DB":
				setVisibility(uriTR,false);
				setVisibility(loginTR,true);
				setVisibility(passwordTR,true);
				setVisibility(optionsTR,true);
				setVisibility(HWComponentTR,true);
				break;
			case "HW":
				setVisibility(loginTR,true);
				setVisibility(passwordTR,true);
				break;
		}
	}
}

function actionTypeChanged()
{
	
	var DBComponentTR = document.getElementById("DBComponentTR");
	var HWComponentTR = document.getElementById("HWComponentTR");
	if (ComponentActionTypeValue!=null)
	{
		var actionTypeValue = ComponentActionTypeValue.options[ComponentActionTypeValue.selectedIndex].value;
		switch (actionTypeValue)
		{
			case "exe":
				setVisibility(DBComponentTR,false);
				setVisibility(HWComponentTR,false);
				break;
			default:
				setVisibility(DBComponentTR,true);
				setVisibility(HWComponentTR,false);
				break;
		}
	}
}

function setVisibility(element,show)
{
	if (element!=null)
		element.style.display = show?"":"none";
}

typeChanged();
//-->
</script></asp:Content>
