<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Control language="c#" Inherits="HyperCatalog.UI.DAM.Controls.ResourceList" CodeFile="resourcelist.ascx.cs" CodeFileBaseClass="HyperCatalog.UI.DAM.UserControl" %>
<script language="C#" runat="server">
private string WrapString(string longText, int maxLength)
{
	if (longText!=null && longText.Length>maxLength)
	{
		if (maxLength>4)
			maxLength = maxLength-4;

		int lastSpace = longText.Substring(0,maxLength).LastIndexOf(' ');
		string wrappedStr = lastSpace>0?longText.Substring(0,lastSpace):longText.Substring(0,maxLength);
		return wrappedStr + (maxLength>0?" [...]":"");
	}
	return longText;
}

private string highlightFilter(string text, string filter)
{
    if (text!=null)
		return Utils.CReplace(text,filter,"<font color=red><b>" + filter + "</b></font>",1);
    return text;
}

private string sizeStr(HyperCatalog.Business.DAM.FileVersion file)
{
	return file==null?"-":convertBytes((long)file.BinarySize);
}

private static string convertBytes(long bytes)
{
    //Converts bytes into a readable 
    if (bytes >= 1073741824)
    return String.Format(Convert.ToString(bytes/1024/1024/1024),"#0.00") + " GB" ;
    else if (bytes >= 1048576)
    return String.Format(Convert.ToString(bytes/1024/1024),"#0.00") + " MB" ;
    else if (bytes >= 1024)
    return String.Format(Convert.ToString(bytes/1024),"#0.00") + " KB" ;
    else if (bytes > 0 && bytes < 1024)
    return bytes + " Bytes" ;
    else
    return "0 Byte" ;
}
</script>
<script><!--
function clipBd(t){window.clipboardData.setData("Text",t);window.status="Link copied to clipboard: \""+t+"\"";return false;}
function typeUpd(l){if(l!=null){var pre="attrRow";for(var i=0;i<l.options.length;i++){var j=0;var tr=document.getElementById(pre+i+"_"+j);while(tr!=null){tr.style.display=l.options[i].selected?"":"none";tr=document.getElementById(pre+i+"_"+(++j));}}}}
function ref(f){var v=f.value;var i=v.lastIndexOf("\\",v.length-1);v=v.substring(i>0?i+1:0,v.lastIndexOf(".",v.length-1));var r=document.getElementById("<%=txtResourceNameValue.ClientID%>");if (r!=null && !r.disabled && r.value==""){r.value=v;}}
function cfmDel(r){return confirm("Do you really want to delete this variant for the resource "+r+" ?");}
function pTb_Click(oT,oB,oE){if(oB.Key=="Del"){oE.cancelPostBack=!(confirm("You are about to delete this resource and all its variants."));}}
//-->
</script>
<STYLE>
.lbl_msgInformation { FONT-WEIGHT: bold; COLOR: green }
.lbl_msgWarning { FONT-WEIGHT: bold; COLOR: orange }
.lbl_msgError { FONT-WEIGHT: bold; COLOR: red }
</STYLE>
<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
	<TR>
		<td valign="top" colspan="3">
			<igtbar:ultrawebtoolbar id="mTb" runat="server" ItemWidthDefault="80px" width="100%" CssClass="hc_toolbar">
				<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
				<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
				<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
				<Items>
					<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" ToolTip="Add a new resource"
						ToggleButton="False"></igtbar:TBarButton>
					<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
					<igtbar:TBSeparator></igtbar:TBSeparator>
					<igtbar:TBLabel Key="LibraryListLbl" Text="Library">
						<DefaultStyle Font-Bold="True" Width="60px"></DefaultStyle>
					</igtbar:TBLabel>
					<igtbar:TBCustom Width="220px" Key="libraryList">
						<asp:DropDownList Width="220px" ID="libraryList" AutoPostBack="True"></asp:DropDownList>
					</igtbar:TBCustom>
					<igtbar:TBLabel Text=" ">
						<DefaultStyle Width="100%"></DefaultStyle>
					</igtbar:TBLabel>
					<igtbar:TBLabel Text="Filter">
						<DEFAULTSTYLE Width="50px" Font-Bold="True"></DEFAULTSTYLE>
					</igtbar:TBLabel>
					<igtbar:TBCustom Width="100px" Key="filterField">
						<asp:TextBox id="txtResourceFilter" Width="100px" MaxLength="90" Runat="Server"></asp:TextBox>
					</igtbar:TBCustom>
					<igtbar:TBarButton Key="Filter" Image="/hc_v4/img/ed_find.gif" ToolTip="Apply search filter on the list">
						<DEFAULTSTYLE Width="25px"></DEFAULTSTYLE>
					</igtbar:TBarButton>
					<igtbar:TBarButton Key="Reload" Image="/hc_v4/img/ed_refresh.gif" ToolTip="Reset list">
						<DEFAULTSTYLE Width="25px"></DEFAULTSTYLE>
					</igtbar:TBarButton>
				</Items>
			</igtbar:ultrawebtoolbar>
		</td>
	</tr>
	<tr>
		<td id="minimalCell" runat="server" valign="top" height="100%">
			<igtbl:UltraWebGrid id="rGd" runat="server" Width="100%" Height="100%">
				<DisplayLayout ColHeadersVisibleDefault="No" AutoGenerateColumns="False" AllowSortingDefault="No"
					RowHeightDefault="100%" Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
					AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="rGd" TableLayout="Fixed" CellClickActionDefault="RowSelect"
					NoDataMessage="No resource">
					<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
						<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
					</HeaderStyleDefault>
					<FrameStyle Width="100%" CssClass="dataTable"></FrameStyle>
					<SelectedRowStyleDefault CssClass="ugs"></SelectedRowStyleDefault>
					<RowAlternateStyleDefault CssClass="uga"></RowAlternateStyleDefault>
					<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"></RowStyleDefault>
				</DisplayLayout>
				<Bands>
					<igtbl:UltraGridBand BorderCollapse="Collapse" DataKeyField="Id">
						<Columns>
							<igtbl:UltraGridColumn Key="Id" BaseColumnName="Id" ServerOnly="true"></igtbl:UltraGridColumn>
							<igtbl:TemplatedColumn Key="Thumbnail" BaseColumnName="URI">
								<CellTemplate>
									<a href="<%# HyperCatalog.Business.DAM.Resource.GetByKey(CurrentLibrary.Id,(int)Container.Cell.Row.DataKey).URI%>"><img src="<%# HyperCatalog.Business.DAM.Resource.GetByKey(CurrentLibrary.Id,(int)Container.Cell.Row.DataKey).URI%>?thumbnail=1&size=40" width="40" height="40" border="0"/></a>
								</CellTemplate>
							</igtbl:TemplatedColumn>
							<igtbl:TemplatedColumn Key="Name" BaseColumnName="Name" HeaderText="Name" Width="100%">
								<CellTemplate>
									<asp:LinkButton id="lnkEdit" onclick="SelectResource_Click" runat="server">
										<%# highlightFilter(Container.Text,resourceFilter)%>
									</asp:LinkButton>
									<br/>
									<span style="font-size:80%;font-style:italic;margin-left:5px;">
										<%# highlightFilter(WrapString(Container.Cell.Row.Cells.FromKey("Description").Text,30),resourceFilter)%>
									</span>
								</CellTemplate>
							</igtbl:TemplatedColumn>
							<igtbl:UltraGridColumn Key="Description" BaseColumnName="Description" ServerOnly="true"></igtbl:UltraGridColumn>
						</Columns>
					</igtbl:UltraGridBand>
				</Bands>
			</igtbl:UltraWebGrid>
		</td>
		<asp:Panel id="emptyLibPanel" runat="server" Visible="false"><td valign="top" colspan="2"><asp:Label id="emptyLibMsgLbl" runat="server" CssClass="lbl_msgWarning" Text="No resource to be displayed."></asp:Label></td></asp:Panel>
		<asp:Panel id="completePanel" runat="server">
			<td valign="top" width="4"></td>
			<td valign="top" width="80%">
				<asp:label id="resourceLbl" runat="server" CssClass="sectionSubTitle" width="100%"></asp:label>
				<asp:Label id="propertiesMsgLbl" runat="server"></asp:Label>
				<FIELDSET>
					<LEGEND>
						<asp:Label id="resourcePropertiesLbl" runat="server" Text="Information"></asp:Label>
					</LEGEND>
					<igtbar:ultrawebtoolbar id="pTb" runat="server" CssClass="hc_toolbar" width="100%" ItemWidthDefault="70px">
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<ITEMS>
							<igtbar:TBarButton Key="Save" Image="/hc_v4/img/ed_save.gif" Text="Save" ToolTip="Save properties changes"></igtbar:TBarButton>
							<igtbar:TBarButton Key="Del" ToolTip="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
							<igtbar:TBSeparator Key="SepAdd"></igtbar:TBSeparator>
							<igtbar:TBarButton Key="CRT" Text="CRT" Image="/hc_v4/img/ed_print.gif" DisabledImage="/hc_v4/img/ed_print_disabled.gif"
								ToolTip="Produce CRT of selected variants">
								<DefaultStyle Width="60px"></DefaultStyle>
							</igtbar:TBarButton>
							<igtbar:TBLabel Text=" ">
								<DefaultStyle Width="100%"></DefaultStyle>
							</igtbar:TBLabel>
						</ITEMS>
						<ClientSideEvents Click="pTb_Click"></ClientSideEvents>
					</igtbar:ultrawebtoolbar>
					<table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr valign="middle">
							<td class="editOptionalLabelCell" width="130">
								<asp:label id="lbResourceLibrary" Text="Library" Runat="server"></asp:label></td>
							<td class="ugd">
								<asp:dropdownlist id="txtResourceLibraryValue" runat="server" Enabled="false"></asp:dropdownlist></td>
						</tr>
						<tr valign="middle">
							<td class="editOptionalLabelCell" width="130">
								<asp:label id="lbResourceName" Text="Name" Runat="server"></asp:label></td>
							<td class="ugd">
								<asp:TextBox id="txtResourceNameValue" runat="server" Width="330px"></asp:TextBox></td>
						</tr>
						<tr valign="middle">
							<td class="editOptionalLabelCell" width="130">
								<asp:label id="lbResourceDescription" Text="Description" Runat="server"></asp:label></td>
							<td class="ugd"><TEXTAREA id="txtResourceDescriptionValue" style="VERTICAL-ALIGN: top" rows="5" cols="51"
									runat="server"></TEXTAREA>
							</td>
						</tr>
						<tr valign="middle">
							<td class="editOptionalLabelCell" width="130">
								<asp:label id="lbResourceType" Text="Type" Runat="server"></asp:label></td>
							<td class="ugd">
								<asp:dropdownlist id="txtResourceTypeValue" runat="server" DataValueField="Id" DataTextField="Name"
									onchange="typeUpd(this);"></asp:dropdownlist></td>
						</tr>
						<asp:Panel id="variantAddProperties" runat="server">
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px">
									<asp:label id="lbVariantCulture" Text="Culture" Runat="server"></asp:label></td>
								<td class="ugd">
									<asp:Dropdownlist id="txtVariantCultureValue" runat="server" DataValueField="Code" DataTextField="Name"></asp:Dropdownlist></td>
							</tr>
							<asp:Repeater id="resourceTypesAttributes" runat="server">
								<ItemTemplate>
									<asp:Repeater id="variantAttributes" runat="server">
										<ItemTemplate>
											<tr id='<%# "attrRow" + ((System.Web.UI.WebControls.RepeaterItem)Container.Parent.Parent).ItemIndex + "_" + Container.ItemIndex%>' valign="middle">
												<td class="editLabelCell" style="text-transform:capitalize;" width="130">
													<%# Container.DataItem%>
												</td>
												<td class="ugd">
													<asp:Label id="attrMsg" runat="server" Text="Automatic" Visible="false"></asp:Label>
													<asp:TextBox ID="attrValueBox" Runat="Server" Visible="false"></asp:TextBox>
													<asp:DropDownList ID="attrValueList" Runat="Server" Visible="false"></asp:DropDownList>
												</td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</ItemTemplate>
							</asp:Repeater>
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px">
									<asp:label id="lbFileBinary" Text="File" Runat="server"></asp:label></td>
								<td class="ugd"><input id="txtFileBinaryValue" type="file" onchange="javascript:ref(this);" runat="server"></INPUT>
								</td>
							</tr>
						</asp:Panel>
						<asp:Panel id="basicResourceInformation" runat="server">
							<tr valign="middle">
								<td class="editLabelCell" style="width:130px">
									<asp:label id="lbResourceURI" Text="URI" Runat="server"></asp:label></td>
								<td class="ugd"><A id="txtResourceURIValue" runat="server"></A></td>
							</tr>
						</asp:Panel></table>
				</FIELDSET>
				<asp:Panel id="variantsPanel" runat="server">
					<FIELDSET>
						<LEGEND>Variants</LEGEND>
						<igtbar:ultrawebtoolbar id="vTb" runat="server" CssClass="hc_toolbar" width="100%" ItemWidthDefault="80px">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ITEMS>
								<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" ToolTip="Add a new variant" ToggleButton="False"></igtbar:TBarButton>
								<igtbar:TBLabel Text=" ">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="Filter">
									<DEFAULTSTYLE Width="50px" Font-Bold="True"></DEFAULTSTYLE>
								</igtbar:TBLabel>
								<igtbar:TBCustom Width="100px" Key="filterField">
									<asp:TextBox id="txtVariantFilter" Width="100px" MaxLength="90" Runat="Server"></asp:TextBox>
								</igtbar:TBCustom>
								<igtbar:TBarButton Key="Filter" Image="/hc_v4/img/ed_find.gif" ToolTip="Apply search filter on the list">
									<DEFAULTSTYLE Width="25px"></DEFAULTSTYLE>
								</igtbar:TBarButton>
								<igtbar:TBarButton Key="Reload" Image="/hc_v4/img/ed_refresh.gif" ToolTip="Reset list">
									<DEFAULTSTYLE Width="25px"></DEFAULTSTYLE>
								</igtbar:TBarButton>
							</ITEMS>
						</igtbar:ultrawebtoolbar>
						<asp:DataGrid id="variantsGrid" runat="server" Width="100%" BorderColor="LightGray" DataKeyField="Id"
							AllowSorting="True" PageSize="5" AllowPaging="True" EnableViewState="true" AutoGenerateColumns="False"
							ShowFooter="False" GridLines="Both" cellspacing="0" cellpadding="1">
							<ItemStyle CssClass="ugd" />
							<HeaderStyle Height="15px" VerticalAlign="Top" CssClass="gh" BorderStyle="Solid" BorderWidth="1pt"></HeaderStyle>
							<SelectedItemStyle CssClass="ugo"></SelectedItemStyle>
							<AlternatingItemStyle CssClass="uga"></AlternatingItemStyle>
							<EditItemStyle HorizontalAlign="Left"></EditItemStyle>
							<FooterStyle BorderWidth="0px"></FooterStyle>
							<PagerStyle Visible="False" HorizontalAlign="Center" Height="15px" BackColor="#cccccc" Mode="NumericPages"></PagerStyle>
							<Columns>
								<asp:TemplateColumn Visible="false">
									<HeaderTemplate>
										<asp:CheckBox id="selectAll" runat="server" Visible="false" OnClick="javascript:SelectAll(this);"></asp:CheckBox>
									</HeaderTemplate>
									<ItemStyle VerticalAlign="Top" HorizontalAlign="center" Width="20px"></ItemStyle>
									<ItemTemplate>
										<asp:CheckBox id="selectThis" runat="server" OnClick='<%# "javascript:SelectThis(this," + Container.ItemIndex + "," + variantsGrid.Items.Count + ");" %>' Checked='<%# Container.ItemIndex==((DataGrid)Container.Parent.Parent).SelectedIndex%>'>
										</asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="center"></HeaderStyle>
									<HeaderTemplate>
									</HeaderTemplate>
									<ItemStyle VerticalAlign="Top" HorizontalAlign="center" Width="20px"></ItemStyle>
									<ItemTemplate>
										<img id="flagImg" Runat="Server" Alt="Default variant" Visible='<%# ((HyperCatalog.Business.DAM.Variant)Container.DataItem).IsMaster%>' src='<%#"/hc_v4/img/ed_cross.gif"%>'/>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Name">
									<ItemStyle VerticalAlign="Top" Width="40px"></ItemStyle>
									<ItemTemplate>
										<a href="<%# ((HyperCatalog.Business.DAM.Variant)Container.DataItem).URI%>"><img src="<%# ((HyperCatalog.Business.DAM.Variant)Container.DataItem).URI%>&thumbnail=1&size=40" width="40" height="40" border="0"/></a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="center"></HeaderStyle>
									<HeaderTemplate>
										Culture
									</HeaderTemplate>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<%#((HyperCatalog.Business.Culture)DataBinder.Eval(Container.DataItem,"Culture")).Name%>
									</ItemTemplate>
									<EditItemTemplate>
										Culture:<br/>
										<asp:DropdownList id="editVariantCulture" runat="server" Enabled='<%# !((HyperCatalog.Business.DAM.Variant)Container.DataItem).IsMaster%>' DataSource='<%#HyperCatalog.Business.Culture.GetAll()%>' DataTextField="Name" DataValueField="Code" SelectedValue='<%#((HyperCatalog.Business.Culture)DataBinder.Eval(Container.DataItem,"Culture")).Code%>'>
										</asp:DropdownList>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="center"></HeaderStyle>
									<HeaderTemplate>
										Attributes
									</HeaderTemplate>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:repeater id="attributesGrid" Runat="Server" DataSource='<%#((HyperCatalog.Business.DAM.Variant)Container.DataItem).Attributes%>'>
											<ItemTemplate><%# (string)Container.DataItem%>:&nbsp;<%# ((HyperCatalog.Business.DAM.VariantAttributeSet)((System.Web.UI.WebControls.Repeater)Container.Parent).DataSource)[(string)Container.DataItem]%></ItemTemplate>
											<SeparatorTemplate><BR /></SeparatorTemplate>
										</asp:repeater>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DataGrid id="editVariantAttributes" runat="server" Visible="false" DataSource='<%#DataBinder.Eval(Container.DataItem,"Attributes")%>' Width="100%" cellpadding="2" cellspacing="1" GridLines="None" ShowHeader="False" ShowFooter="False" AutoGenerateColumns="False">
											<SelectedItemStyle ForeColor="GhostWhite" BackColor="DarkSlateBlue"></SelectedItemStyle>
											<Columns>
												<asp:TemplateColumn>
													<ItemStyle Width="25px"></ItemStyle>
													<ItemTemplate>
														<asp:Label id="editVariantAttributeName" runat="server" Text='<%# Container.DataItem%>'>
														</asp:Label>:
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn>
													<ItemTemplate>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:DataGrid>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false">
									<HeaderStyle HorizontalAlign="center"></HeaderStyle>
									<HeaderTemplate>
										Versions
									</HeaderTemplate>
									<ItemStyle VerticalAlign="top" HorizontalAlign="center"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton id="lnkVersions" runat="server">
										#<%#((HyperCatalog.Business.DAM.FileVersionCollection)DataBinder.Eval(Container.DataItem,"Files")).Count%>
									</asp:LinkButton>
										<br/>
										<asp:ImageButton id="viewVersionsBtn" runat="server" ToolTip="View versions" AlternateText="View versions" ImageUrl='<%#"/hc_v4/img/dam_history.png"%>'>
										</asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="center"></HeaderStyle>
									<HeaderTemplate>
										Size
									</HeaderTemplate>
									<ItemStyle VerticalAlign="top" HorizontalAlign="center"></ItemStyle>
									<ItemTemplate>
										<%# sizeStr(((HyperCatalog.Business.DAM.Variant)Container.DataItem).CurrentFile)%>
									</ItemTemplate>
									<EditItemTemplate>
										File:<br/>
										<input id="editVariantFileValue" type="file" runat="server">
										<table border="0" cellpadding="2" cellspacing="0" id="editVariantDatesPnl" runat="server" Visible='<%# CurrentLibrary.VersioningEnabled%>'>
											<TR>
												<td>
													<asp:Label id="editVariantBOPLbl" runat="server" Text="BOP:"></asp:Label>
													<igsch:WebDateChooser id="editVariantBOPValue" runat="server" NullDateLabel="Undefined"></igsch:WebDateChooser>
												</td>
												<td>
													<asp:Label id="editVariantEOPLbl" runat="server" Text="EOP:"></asp:Label>
													<igsch:WebDateChooser id="editVariantEOPValue" runat="server" NullDateLabel="Undefined"></igsch:WebDateChooser>
												</td>
											</tr>
										</table>
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn visible="false">
									<HeaderStyle HorizontalAlign="center"></HeaderStyle>
									<HeaderTemplate>
										File Properties
									</HeaderTemplate>
									<ItemStyle VerticalAlign="top"></ItemStyle>
									<ItemTemplate>
										<!---<% /* # RenderSpecificProperties((HyperCatalog.Business.DAM.SpecificResource)DataBinder.Eval(Container.DataItem,"Specific"))*/%>--->
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn>
									<ItemStyle VerticalAlign="Top" HorizontalAlign="center" Width="15px"></ItemStyle>
									<ItemTemplate>
										<table border="0">
											<tr>
												<td runat="server">
													<asp:ImageButton id="editVariant" runat="server" EnableViewState="True" onclick="EditVariant_Click" ToolTip="Edit variant information" AlternateText="Edit variant information" ImageUrl='<%#"/hc_v4/img/ed_edit.gif"%>'>
													</asp:ImageButton></td>
												<td runat="server" Visible='<%# !((HyperCatalog.Business.DAM.Variant)Container.DataItem).IsMaster%>'>
													<asp:ImageButton id="deleteVariant" runat="server" onclick="DeleteVariant_Click" EnableViewState="True" ToolTip="Delete this variant" AlternateText="Delete this variant" ImageUrl='<%#"/hc_v4/img/ed_delete.gif"%>'>
													</asp:ImageButton></td>
												<td><a onClick="javascript:return clipBd(this.href);" href="<%# ((HyperCatalog.Business.DAM.Variant)Container.DataItem).URI%>"><img src="/hc_v4/img/ed_download.gif" Alt="Copy link location" border="0" /></a></td>
											</tr>
										</table>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:ImageButton id="validateEditVariant" runat="server" onclick="ValidateEditVariant_Click" EnableViewState="True" AlternateText="Save changes" ImageUrl='<%#"/hc_v4/img/ed_ok.gif"%>'>
										</asp:ImageButton>
										<asp:ImageButton id="cancelEditVariant" runat="server" onclick="CancelEditVariant_Click" EnableViewState="True" AlternateText="Cancel" ImageUrl='<%#"/hc_v4/img/ed_reject.gif"%>'>
										</asp:ImageButton>
									</EditItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
					</FIELDSET>
				</asp:Panel>
			</td>
		</asp:Panel>
	</tr>
</table>
<script type="text/javascript"><!--
typeUpd(document.getElementById('<%Response.Write(txtResourceTypeValue.ClientID);%>'));
//-->
</script>
