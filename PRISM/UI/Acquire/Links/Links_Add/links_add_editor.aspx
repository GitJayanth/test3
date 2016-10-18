<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.Links.Links_editor" CodeFile="Links_add_editor.aspx.cs" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">New link</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript"language="javascript">			
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			{
			    if (oButton.Key == 'Close') {
			        oEvent.cancelPostBack = true;
			        parent.close();
			    }

			    if (oButton.Key == 'Analyze') {
			        if (document.getElementById('<%=txtSKUs.ClientID %>').value == "") {
			            alert("Please enter SKUList or NameList");
			            oEvent.cancelPostBack = true;
			        }
			    }   
			}
			
  		function ReloadParent()
			{
				parent.opener.document.getElementById("action").value = "reload";
				parent.opener.document.forms[0].submit();
				parent.close();
			}
		function UnloadGrid()
          	  {
                	igtbl_unloadGrid("dg");
             }

             //Code Added for Links Requirement (PR658943) - to enable/disable Level Dropdown based on the selection in Input radio button on 18th Dec 2012
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

             function clickHandler(obj) {
                 g_su_links(obj);
                 var chkId = obj.id;
                 var chkVal = obj.checked;
                 var toolbar = igtbar_getToolbarById('<%=uwToolbar.UniqueID %>');
                 if (chkId.indexOf('g_ca') != -1) {
                     toolbar.Items.fromKey("Save").setEnabled(chkVal);
                 }
                 else if (chkId.indexOf('g_sd') != -1) {
                     if (chkVal) {
                         toolbar.Items.fromKey("Save").setEnabled(chkVal);
                     }
                     else {
                         myForm = document.forms[0];
                         for (i = 0; i < myForm.length; i++) {
                             if ((myForm.elements[i].id.indexOf('g_sd') != -1)) {
                                 if (!myForm.elements[i].disabled && myForm.elements[i].checked) {
                                     toolbar.Items.fromKey("Save").setEnabled(true);
                                     break;
                                 }
                                 else {
                                     toolbar.Items.fromKey("Save").setEnabled(false);
                                 }
                             }
                         }
                     }
                 }
                 var cnt = document.getElementById('<%=AddHostCnt.ClientID %>').value;
                 var msg = document.getElementById('<%=lbMsg.ClientID %>');
                 if (cnt != '-1') {  //i.e. Adding a Host
                     myForm = document.forms[0];
                     for (i = 0; i < myForm.length; i++) {
                         if ((myForm.elements[i].id.indexOf('g_ca') != -1)) {
                             if (!myForm.elements[i].disabled && myForm.elements[i].checked) {
                                 //msg.value = 'You are trying to create ' + cnt + ' links. These links will only be manageable at product level';
                                 msg.style.visibility = 'visible';
                             }
                             else {
                                 msg.style.visibility = 'hidden';
                             }
                         }
                     }
                 }
                 else {
                     msg.style.visibility = 'hidden';
                 }
             }
			</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table class="main" cellspacing="0" cellpadding="0" border="2">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="uwToolbar_ButtonClicked">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Analyze" Text="Analyze" Image="/hc_v4/img/ed_OK.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="AnalyzeSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif" DisabledImage ="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
                                <%--Code Added for Links Requirement (PR664195) - to add "Include Obsolete" checkbox by Prachi on 17th Dec 2012 - start--%>
                                    <igtbar:TBCustom Width="120px" Key="chkObs">
                                       <asp:CheckBox ID="chkObsolete" runat="server" Text="Include Obsolete" />
                                    </igtbar:TBCustom>
                                    <igtbar:TBSeparator></igtbar:TBSeparator>
                                    <%--Code Added for Links Requirement (PR664195) - to add "Include Obsolete" checkbox by Prachi on 17th Dec 2012 - end--%>
								<igtbar:TBarButton Key="Close" Text="Close" Image="/hc_v4/img/ed_cancel.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar></td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <%--Code Added for Links Requirement (PR658943) - to add input type radio buttons and Level dropdown on 18th Dec 2012 - start--%>
                            <tr valign="top">
                                <td width="70" class="editLabelCell">
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

                            <%--Code Added for Links Requirement (PR658943) - to add input type radio buttons and Level dropdown on 18th Dec 2012 - end--%>

							<tr valign="top">
								<td width="70" class="editLabelCell">
									<asp:Label ID="lbSKUList" Runat="server">List</asp:Label>
								</td>
								<td class="ugd">
									<asp:TextBox id="txtSKUs" runat="server" Width="100%" TextMode="MultiLine" Rows="5"></asp:TextBox>
									<%--	Code commented to add custom validation for txtSKUs text box--%>
									<%--<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="txtSKUs"
										ErrorMessage="*"></asp:requiredfieldvalidator>--%>
								</td>
							</tr>
							<tr valign="top">
								<td width="70" class="editLabelCell">
									<asp:Label ID="lbSeparator" Runat="server">Separator</asp:Label>
								</td>
								<td class="uga">
									<asp:RadioButtonList id="rbList" runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Value="," Selected="True">,</asp:ListItem>
										<asp:ListItem Value=";">;</asp:ListItem>
										<asp:ListItem Value="|">|</asp:ListItem>
										<asp:ListItem Value="R">CR</asp:ListItem>
									</asp:RadioButtonList>
								</td>
							</tr>

                            <%--Code added for Links Requirement (PR658943) - to add Effective Date Calendar by Prachi on 15th Jan 2013 - start--%>
                            <tr valign="top">
                            <td class ="editLabelCell" width="80">
                            <asp:Label runat="server" ID="lbEffDate" Text="Effective Date"></asp:Label>
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
                <%--		
				Adding Export Functionality for free editor (PR665368 - Export Button)start by Nisha Verma on 21st dec 2012--%>
				<tr valign="top" style="height:5px">
					<td>
				        <igtbar:ultrawebtoolbar id="Ultrawebtoolbar2" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="Ultrawebtoolbar2_ButtonClicked" Height="20px">
                            <HoverStyle CssClass="hc_toolbarhover_Export">
                            </HoverStyle>
                            <SelectedStyle CssClass="hc_toolbarselected">
                            </SelectedStyle>
                            <Items>
                                <igtbar:TBarButton Key="Export" Text="Export" Enabled="False" Image="/hc_v4/img/ed_download.gif" ToolTip="Export content in Excel">
                                <DefaultStyle Width="80px"></DefaultStyle>
                                </igtbar:TBarButton>
                                                    
                                <igtbar:TBCustom Key = "AddHostMsg" Width = "100%">
                                <asp:Label ID = "lbMsg" runat = "server" CssClass = "hc_error" style="visibility:hidden" Text ="Add Host Message" Height = "20px"></asp:Label>
                                </igtbar:TBCustom>                               
                            </Items>
                            <DefaultStyle CssClass="hc_toolbar1default">
                            </DefaultStyle>
                        </igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<%--		
				Adding Export Functionality for free editor (PR665368 - Export Button)end by Nisha Verma on 21st dec 2012--%>
				<tr valign="top">
					<td>
						<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
							<igtbl:ultrawebgrid id="dg" runat="server" ImageDirectory="/ig_common/Images/" Width="100%" Visible="False" OnInitializeRow="dg_InitializeRow">
								<DisplayLayout AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00" HeaderClickActionDefault="SortSingle"
									RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
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
                                            <%--Columns added for Links Requirement (PR658943) - to provide checkbox for selecting the items by Prachi on 20th Jan 2013--%>
                                            <igtbl:TemplatedColumn Key="Select" Width="22px">
												<CellStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="True"></CellStyle>
                                                <HeaderTemplate>
											        <asp:CheckBox id="g_ca" onclick="javascript: return clickHandler(this);" runat="server"></asp:CheckBox>
										        </HeaderTemplate>
												<CellTemplate>
													<asp:CheckBox id="g_sd" onclick="javascript: return clickHandler(this);" runat="server"></asp:CheckBox>
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
                                            <%--Columns added for Links Requirement (PR658943) - to allow analyze for higher level Node Names by Prachi on 19th Jan 2013 - start--%>
                                            <igtbl:UltraGridColumn HeaderText="NameInput" Key="NameInput" Width="250px" BaseColumnName="NameInput" CellMultiline="Yes" Hidden="true" ServerOnly="true">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="Path -> ItemName" Key="ItemPath" Width="240px" BaseColumnName="ItemPath" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn HeaderText="FlagExists" Key="FlagExists" Width="50px" BaseColumnName="FlagExists"
												ServerOnly="True"></igtbl:UltraGridColumn>
                                            <%--Columns added for Links Requirement (PR658943) - to allow analyze for higher level Node Names by Prachi on 19th Jan 2013 - end--%>
											<igtbl:UltraGridColumn HeaderText="IsRelevant" Key="IsRelevant" Width="50px" BaseColumnName="IsRelevant"
												ServerOnly="True"></igtbl:UltraGridColumn>
											<%--Columns added for Links Requirement (PR666775) - to check the Link Creation Limits by Phani on 22nd Jan 2013 - Start--%>
                                            <igtbl:UltraGridColumn HeaderText="SameLink" Key="SameLink" Width="50px" BaseColumnName="SameLink"
												ServerOnly="True"></igtbl:UltraGridColumn>
                                            <%--Columns added for Links Requirement (PR666775) - to check the Link Creation Limits by Phani on 22nd Jan 2013 - End--%>
                                            <igtbl:UltraGridColumn HeaderText="Error" Key="Error" BaseColumnName="Error" ServerOnly="True"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Status" Key="Status" Width="100%" BaseColumnName="Status" ServerOnly="True" CellMultiline="Yes">
												<CellStyle Wrap="True"></CellStyle>
											</igtbl:UltraGridColumn>
										</Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:ultrawebgrid>
						</div>
                        <asp:HiddenField ID = "AddHostCnt" runat = "server" />
					</td>
				</tr>
			</table>
</asp:Content>