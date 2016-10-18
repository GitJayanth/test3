<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Links_add_treeview" CodeFile="Links_add_treeview.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">New link</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript" language="javascript">
			function Scroll(nodeId)
			{ 
				try
				{
					if (document.getElementById(nodeId))
						document.getElementById(nodeId).scrollIntoView(); 
				}
				catch (e){}
			}
			
			function ReloadParent()
			{
				top.opener.document.getElementById("action").value = "reload";
				top.opener.document.forms[0].submit();
				top.window.close();
			}
			
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
			    if (oButton.Key == 'Close')
                {
					parent.close();
					oEvent.cancelPostBack = true;
                }
			}

	function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dg");
             }

             //Function added for Links Requirement (PR658940) - to enable the checkbox above the SKU Level by Prachi on 11th Dec 2012
             function checkNodes(treeId, nodeId, bChecked) {
                 var selectedNode = igtree_getNodeById(nodeId);
                 var parentNode = selectedNode.getParent();
                 var childNodes = selectedNode.getChildNodes();
                 var siblingNode = "";
                 var checked = 0;
                 var count = 0;
                 var str = nodeId.split("_");

                 //Uses the selected nodeId to build the Id for the first node object in a group of children so that each sibling can be looped through to find its checked status.
                 for (var i = 0; i < (str.length - 1); i++) {
                     siblingNode = siblingNode + str[i] + "_";
                 }
                 siblingNode = siblingNode + "1"
                 siblingNode = igtree_getNodeById(siblingNode);

                 //Loop through siblingNodes and counts how many are currently checked..
                 while (siblingNode != null) {
                     if (siblingNode.getChecked() == true) {
                         checked++
                     }
                     count++
                     siblingNode = siblingNode.getNextSibling();
                 }

                 //In case of a node being checked
                 if (bChecked) {
                     //If the number of checked siblings == the number of siblings then parent nodes are checked if they are not already checked.
                     //It checkes if the parents are checked to prevent an infinite loop.
                     parentNode = selectedNode.getParent();
                     if (parentNode != null) {
                         if (parentNode.hasCheckbox()) {
                             if (count == checked && parentNode.getChecked() != true) {
                                 parentNode.setChecked(true);
                             }
                         }
                     }

                     //Finally all children are checked for the node. This cascades down, so there is no need to navigate down.
                     for (n in childNodes) {
                         if (childNodes[n].getChecked() != bChecked && childNodes[n].getEnabled() == true) {
                             childNodes[n].setChecked(bChecked);
                         }
                     }
                 }

                 //In case of node being unchecked
                 else if (!bChecked) {
                     var uncheckFlag = 0;
                     checked = 0;
                     count = 0;
                     selectedNode = igtree_getNodeById(nodeId);
                     parentNode = selectedNode.getParent();

                     //To uncheck the parent node if even a single child node is unchecked
                     if (parentNode != null) {
                         if (parentNode.hasCheckbox()) {
                             var allChildNodes = parentNode.getChildNodes();
                             for (n in allChildNodes) {
                                 if (allChildNodes[n].getChecked() == false) {
                                     uncheckFlag = 1;
                                 }
                             }
                             if (uncheckFlag == 1) {
                                 parentNode.setChecked(false);
                             }
                         }
                     }

                     //To uncheck all the child nodes if parent node is unchecked
                     childNodes = selectedNode.getChildNodes();
                     if (childNodes != null) {
                         for (n in childNodes) {
                             if (childNodes[n].getChecked() == true) {
                                 checked++;
                             }
                             count++;
                         }
                     }
                     if (count == checked) {
                         for (n in childNodes) {
                             if (childNodes[n].getChecked() != bChecked && childNodes[n].getEnabled() == true) {
                                 childNodes[n].setChecked(bChecked);
                             }
                         }
                     }
                 }
             }

             //Function Added for Links Requirement (PR658943) - to enable/disable Level Dropdown based on the selection in Input radio button on 11th Jan 2013 - start
             function GetInputValue() {
                 var radioButtons = document.getElementById('<%=rblInput.ClientID %>');
                 var level = document.getElementById('<%=lbLevel.ClientID %>');
                 var ddl = document.getElementById('<%=ddlLevel.ClientID %>');

                 var inputs = radioButtons.getElementsByTagName("input");
                 for (var i = 0; i < inputs.length; i++) {
                     if (inputs[i].checked) {
                         if (inputs[i].value == "Sku") {
                             level.disabled = true;
                             ddl.disabled = true;

                             //to add '[Disabled]' as the selected value in the Level dropdown
                             var option = document.createElement("Option");
                             option.value = "Disabled";

                             option.innerHTML = "[Disabled]";
                             ddl.insertBefore(option, ddl.firstChild);
                             ddl.value = "Disabled";
                         }
                         else if (inputs[i].value == "Name") {
                             level.disabled = false;
                             ddl.disabled = false;

                             //to remove '[Disabled]' from the Level dropdown
                             ddl.removeChild(ddl.firstChild);
                         }
                     }
                 }
             }

             function dispLoading() {
                 var str = '<%=webTree.ClientID %>';
                 var str1 = str.replace(new RegExp('_', 'g'), '');
                 var tree = igtree_getTreeById(str1);
                 var nodes = tree.getNodes();

                 //Removing the existing nodes from Tree and adding "Loading ..." node
                 for (var i = 0; i < nodes.length; i++) {
                     var node = nodes[i];
                     var cnodes = node.getChildNodes();
                     var clen = cnodes.length;
                     for (var ci = 0; ci < clen; ci++) {
                         cnodes[ci].setExpanded(false);
                         cnodes[ci].element.style.display = 'none';
                     }
                     //node.setExpanded(false);
                     node.element.style.display = 'none';
                 }

                 tree.insertRoot(0, "<img src='/hc_v4/img/ed_wait.gif'/> Loading ...");
                 var rootNode = tree.getNodes()[0];
                 if (rootNode) {
                     rootNode.setTag("Loading");
                     rootNode.setExpanded(false);
                 }
                 //__doPostBack();
             }

             function reloadTreeObs() {
                 var chkFlag = false;
                 var myForm = document.forms[0];
                 for (i = 0; i < myForm.length; i++) {
                     if ((myForm.elements[i].id.indexOf('g_ca') != -1)) {
                         chkFlag = true;  // This means that 'dg' is visible and 'webTree' is invisible
                     }
                 }
                 if (!chkFlag) {   //If 'webTree' is visible, then only display the "Loading..." node in tree
                     dispLoading();
                 }
                 //__doPostBack();
             }
			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<script type="text/javascript" language="javascript">
		document.body.onload = function(){try{Scroll('<%#currentNodeId%>');}catch(e){}};

</script>
<%--Removed the width tag from ultrawebtoolbar to fix enlarge button issue by Radha S--%>
			<table class="main" cellspacing="0" cellpadding="0" border="2">
				<tr valign="top" style="height:1px">
						<td><igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
									<igtbar:TBarButton Key="Save" Text="Apply changes" Image="/hc_v4/img/ed_save.gif">
										<DefaultStyle Width="120px"></DefaultStyle>
									</igtbar:TBarButton>
									<igtbar:TBSeparator></igtbar:TBSeparator>
									<igtbar:TBCustom Width="120px" Key="chkObs">
                                        <asp:CheckBox ID="chkObsolete" runat="server" Text="Include Obsolete" onclick = "reloadTreeObs()"
                                            OnCheckedChanged="chkObsolete_CheckedChanged" AutoPostBack="True" />
                                    </igtbar:TBCustom>
                                    <igtbar:TBSeparator></igtbar:TBSeparator>
                                    <igtbar:TBarButton Key="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif">
                                    </igtbar:TBarButton>
								</Items>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							</igtbar:ultrawebtoolbar></td>
					</tr>

                    <%--Code Added for Links Requirement (PR658943) - to allow for searches of node names in addition to SKUs on 11th Jan 2013 - start--%>
                     <tr valign="top" style="height:1px">
				     <td>
						<table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr valign="top">
                                <td width="70" class="editLabelCell" valign="middle">
                                    <asp:Label runat="server" ID="lbInput" Text="Input"></asp:Label>
                                </td>

                                <td class="uga">
                                    <asp:RadioButtonList runat="server" ID="rblInput" RepeatDirection="Horizontal" onclick="GetInputValue()">
                                        <asp:ListItem Text="Sku" Value="Sku" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Name" Value="Name"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                </tr>

                                <tr valign="top" id="LevelRow" runat="server">
                                <td class="editLabelCell" width="70">
                                    <asp:Label runat="server" ID="lbLevel" Text="Level" Enabled="false"></asp:Label>
                                </td>
                    
                                <td class="uga">
                                    <asp:DropDownList ID="ddlLevel" runat="server" Enabled="false">                             
                                    </asp:DropDownList>
        
                                    <script type="text/javascript">
                                        var radioButtons = document.getElementById('<%=rblInput.ClientID %>');
                                        var ddl = document.getElementById('<%=ddlLevel.ClientID %>');
                                        var inputs = radioButtons.getElementsByTagName("input");

                                        for (var i = 0; i < inputs.length; i++) {
                                            if (inputs[i].checked) {
                                                if (inputs[i].value == "Sku") {
                                                    var option = document.createElement("Option");
                                                    option.value = "Disabled";

                                                    option.innerHTML = "[Disabled]";
                                                    ddl.insertBefore(option, ddl.firstChild);
                                                    ddl.value = "Disabled";
                                                }
                                            }
                                        }
                                    </script>
                                </td>
                                </tr>
                                
                                <tr>
                                <td width="70" class="editLabelCell">
									<asp:Label ID="lbFilter" Runat="server">Filter</asp:Label>
								</td>

                                <td class="uga" style="vertical-align:middle">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                        <td class="uga" style="width:170px; vertical-align:middle">
                                            <asp:TextBox cssClass="Search" Width="150px" Id="txtFilter" MaxLength="50" runat="server"></asp:TextBox>
                                        </td>

                                         <td class="uga" style="vertical-align:middle" align="left">
                                <igtbar:ultrawebtoolbar id="uwToolbarFilter" runat="server" CssClass="hc_toolbar" ItemWidthDefault="20px" width="20px" ToolTip="Filter" OnButtonClicked="uwToolbarFilter_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbarFilter_InitializeToolbar"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
									<igtbar:TBarButton Key="filter" Image="/hc_v4/img/ed_search.gif">
                                    </igtbar:TBarButton>
                                    </Items>
                                    <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
                                    </igtbar:ultrawebtoolbar>
                                    </td>
                                </tr>
                                </table>
                                </td>
                            </tr>
                            <%--Code added for Links Requirement (PR658943) - to add Effective Date Calendar by Prachi on 15th Jan 2013 - start--%>
                        <tr valign="top">
                            <td class ="editLabelCell" width="80">
                            <asp:Label runat="server" ID="lblEffectiveDate" Text="Effective Date"></asp:Label>
                            </td>
                    
                            <td class="uga" style="vertical-align:middle">                                
                            <igsch:webdatechooser id="dateValue" runat="server" Font-Size="xx-small" Height="8px" Width="90px" NullDateLabel=" " AutoPostBack-CalendarMonthChanged="True" AutoPostBack-ValueChanged="True">
                                <CalendarLayout cellspacing="1" PrevMonthImageUrl="/hc_v4/inf/Images/ig_cal_grayP.gif" ShowTitle="False" NextMonthImageUrl="/hc_v4/inf/Images/ig_cal_grayN.gif">
                                    <DayStyle BorderWidth="0px" BorderColor="White" BorderStyle="Solid" BackColor="#D0D0D0"></DayStyle>
                                    <FooterStyle Height="12pt" BorderWidth="1px" Font-Size="XX-Small" BorderColor="#D8D8D8" BorderStyle="Solid" ForeColor="#303030" BackColor="#B0B0B0">
                                        <BorderDetails ColorBottom="0, 96, 96, 96" ColorRight="0, 96, 96, 96"></BorderDetails>
                                    </FooterStyle>
                                    <SelectedDayStyle Font-Bold="True" ForeColor="White" BackColor="#606060"></SelectedDayStyle>
                                    <OtherMonthDayStyle ForeColor="Gray"></OtherMonthDayStyle>
                                    <NextPrevStyle Width="10px" BorderWidth="1px" BorderColor="#D8D8D8" BorderStyle="Solid" BackColor="#B0B0B0">
                                        <BorderDetails ColorBottom="0, 112, 112, 112" ColorRight="0, 112, 112, 112" WidthBottom="2px"></BorderDetails>
                                    </NextPrevStyle>
                                    <CalendarStyle BorderWidth="1px" Font-Size="XX-Small" Font-Names="Tahoma" BorderColor="#505050" BorderStyle="Solid"
                                       ForeColor="Black" BackColor="#B0B0B0"></CalendarStyle>
                                    <WeekendDayStyle ForeColor="#505050"></WeekendDayStyle>
                                    <TodayDayStyle Font-Bold="True" BackColor="Silver"></TodayDayStyle>
                                    <DayHeaderStyle Height="1pt" Font-Size="XX-Small" Font-Italic="True" BorderStyle="None" BackColor="#B0B0B0"></DayHeaderStyle>
                                    <TitleStyle Height="16px" Font-Size="XX-Small" Font-Bold="True" ForeColor="White" BackColor="#909090">
                                        <BorderDetails StyleBottom="Solid" ColorBottom="0, 128, 128, 128" WidthBottom="1px"></BorderDetails>
                                    </TitleStyle>
                                </CalendarLayout>
                                <DropButton ImageUrl2="/hc_v4/img/igsch_xpbluedn.gif" ImageUrl1="/hc_v4/img/igsch_xpblueup.gif"></DropButton>
                            </igsch:webdatechooser>                              
                            </td>
                            </tr>
                            <%--Code added for Links Requirement (PR658943) - to add Effective Date Calendar by Prachi on 15th Jan 2013 - end--%>
						</table>
					</td>
				</tr>

                <tr valign="top" style="height:1px">
					<td>
						<asp:Label id="LbNbSKUs" Runat="server" Visible="False" ForeColor="Green"></asp:Label>
					</td>
				</tr>
                <%--Code Added for Links Requirement (PR658943) - to allow for searches of node names in addition to SKUs on 11th Jan 2013 - end--%>

                   <!--	Adding Export Functionality for free editor (PR665368 - Export Button)start by Nisha Verma on  18th jan 2013-->
				<tr valign="top" style="height:1px">
					<td>
				        <igtbar:ultrawebtoolbar id="Ultrawebtoolbar2" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="uwToolbar1_ButtonClicked" Height="17px">
                            <HoverStyle CssClass="hc_toolbarhover">
                            </HoverStyle>
                            <ClientSideEvents Click="uwToolbar_Click" InitializeToolbar="uwToolbar_InitializeToolbar" />
                            <SelectedStyle CssClass="hc_toolbarselected">
                            </SelectedStyle>
                            <Items>
                                <igtbar:TBarButton Key="Export" Text="Export" Enabled="False" Image="/hc_v4/img/ed_download.gif" ToolTip="Export content in Excel">
                                <DefaultStyle Width="80px"></DefaultStyle>
                                </igtbar:TBarButton>

                                <igtbar:TBSeparator Key="LoadSep"></igtbar:TBSeparator>
                                <igtbar:TBCustom width = "240px" Key="LoadLevel1">
                                        <asp:CheckBox ID="chkLoad" runat="server" Text="Enable TreeView Selection from Level 1" onclick = "dispLoading()"
                                            OnCheckedChanged="chkLoad_CheckedChanged" AutoPostBack="True" />
                                    </igtbar:TBCustom>
                            </Items>
                            <DefaultStyle CssClass="hc_toolbar1default">
                            </DefaultStyle>
                        </igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<%--		
				Adding Export Functionality for free editor (PR665368 - Export Button)end by Nisha Verma on  18th jan 2013--%>
                    
                    <tr valign="top" style="height:1px">
						<td><asp:label id="lbError" runat="server" CssClass="hc_error" Visible="False">LabelError</asp:label></td>
					</tr>
					<tr valign="top" style="height:auto">
						<td>
							<ignav:ultrawebtree id="webTree" runat="server" LoadOnDemand="Manual" InitialExpandDepth="1" 
							WebTreeTarget="HierarchicalTree" Indentation="10" ExpandImage="/hc_v4/ig/ig_treeXPPlus.gif" 
							CollapseImage="/hc_v4/ig/ig_treeXPMinus.gif" OnDemandLoad="webTree_DemandLoad" 
							OnNodeBound="webTree_NodeBound">
                            <%--Code added for Links Requirement (PR658940) - to enable the checkbox above the SKU Level by Prachi on 11th Dec 2012--%>
                            <ClientSideEvents NodeChecked="checkNodes" />
								<SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
								<NodeStyle CssClass="hc_node"></NodeStyle>
								<HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
								<NodePaddings Top="2px"></NodePaddings>
								<Levels>
									<ignav:Level Index="0"></ignav:Level>
									<ignav:Level Index="1"></ignav:Level>
								</Levels>
								<AutoPostBackFlags NodeExpanded="True" NodeChecked="False"></AutoPostBackFlags>
							</ignav:ultrawebtree>
						</td>
					</tr>
					<tr valign="top">
						<td>
							<%--<asp:label id="lbRecordcount" runat="server" Visible="False">NbRecords</asp:label><br/>--%>
							<igtbl:ultrawebgrid id="dg" runat="server" Visible="False" Width="325px" Height="200px" OnInitializeRow="dg_InitializeRow">
								<DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
									HeaderClickActionDefault="SortSingle" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
									<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler_links"></ClientSideEvents>
									<ActivationObject AllowActivation="False"></ActivationObject>
									<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
									<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BaseTableName="Items" Key="Items" BorderCollapse="NotSet" DataKeyField="ItemId">
										<Columns>
                                            <igtbl:TemplatedColumn Key="Select" Width="22px">
												<CellStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="True"></CellStyle>
                                                <HeaderTemplate>
											        <asp:CheckBox id="g_ca" onclick="javascript: return g_su_links(this);" runat="server"></asp:CheckBox>
										        </HeaderTemplate>
												<CellTemplate>
													<asp:CheckBox id="g_sd" onclick="javascript: return g_su_links(this);" runat="server"></asp:CheckBox>
												</CellTemplate>
                							</igtbl:TemplatedColumn>
                                            <%--Columns added for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item - start--%>
                                            <igtbl:UltraGridColumn HeaderText="MainItemId" Key="MainItemId" Width="50px" Hidden="True" BaseColumnName="MainItemId" ServerOnly="True"></igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="Companion ItemName" Key="MainItemPath" Width="180px" BaseColumnName="MainItemPath" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
                                            <%--Columns added for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item - end--%>
											<igtbl:UltraGridColumn HeaderText="ItemId" Key="ItemId" Width="50px" Hidden="True" BaseColumnName="ItemId" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Class" Key="ClassName" Width="95px" BaseColumnName="ClassName" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Sku" Key="ItemNumber" Width="40px" BaseColumnName="ItemNumber" ServerOnly="True"
												CellMultiline="No">
												<CellStyle Font-Bold="True"></CellStyle>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Name" Key="ItemName" Width="240px" BaseColumnName="ItemName" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="NameInput" Key="NameInput" Width="250px" BaseColumnName="NameInput" CellMultiline="Yes" Hidden="true" ServerOnly="true">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="Path -> ItemName" Key="ItemPath" Width="240px" BaseColumnName="ItemPath" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="FlagExists" Key="FlagExists" Width="50px" BaseColumnName="FlagExists"
												ServerOnly="True"></igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="IsRelevant" Key="IsRelevant" Width="50px" BaseColumnName="IsRelevant"
												ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="SameLink" Key="SameLink" Width="50px" BaseColumnName="SameLink"
												ServerOnly="True"></igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="Error" Key="Error" BaseColumnName="Error" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Status" Key="Status" Width="100%" BaseColumnName="Status" ServerOnly="True" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
										</Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
						</td>
					</tr>
			</table>
</asp:Content>