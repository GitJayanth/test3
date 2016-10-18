<%@ Reference Control="~/ui/dam/custompager.ascx" %>
<%@ Control language="c#" Inherits="HyperCatalog.UI.DAM.Controls.LibraryList" CodeFile="librarylist.ascx.cs" CodeFileBaseClass="HyperCatalog.UI.DAM.UserControl" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ucs" TagName="CustomPager" Src="./CustomPager.ascx" %>
<%@ Register TagPrefix="hcx" Namespace="HyperCatalog.UI.WebControls"%>
<script language="C#" runat=server>
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
    if (text != null)
      return Utils.CReplace(text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
    return text;
  }

</script>
	<script type="text/javascript"><!--

			function templateToolbar_Click(oToolbar, oButton, oEvent)
			{
				if (oButton.Key == "Del")
				{
					igtbl_getGridById('internLibraryGrid').deleteSelectedRows();
					return false;
				}
			}
	--></script>
<STYLE>
.lbl_msgInformation{font-weight:bold;color:green;}
.lbl_msgWarning{font-weight:bold;color:orange;}
.lbl_msgError{font-weight:bold;color:red;}
</STYLE>
		<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
			<TR>
				<td valign="top" colspan="2">
					<igtbar:ultrawebtoolbar id="mTb" runat="server" ItemWidthDefault="80px" width="100%" CssClass="hc_toolbar">
						<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						<Items>
							<igtbar:TBarButton Key="Add" Text="Add" Image="/hc_v4/img/ed_new.gif" ToolTip="Add a new library" ToggleButton="False"></igtbar:TBarButton>
							<igtbar:TBLabel Text=" ">
								<DefaultStyle Width="100%"></DefaultStyle>
							</igtbar:TBLabel>
							<igtbar:TBLabel Text="Filter">
								<DEFAULTSTYLE Width="40px" Font-Bold="True"></DEFAULTSTYLE>
							</igtbar:TBLabel>
							<igtbar:TBCustom Width="150px" Key="filterField">
								<asp:TextBox id="txtFilter" Width="150px" MaxLength="50" Runat="Server"></asp:TextBox>
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
			<TR>
				<td valign="top" height="100%" width="20%">
					<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr valign="top">
							<td class="main">
								<ucs:CustomPager id="librariesPagerTop" runat="server"></ucs:CustomPager>
								<asp:DataGrid id="libraryGrid" runat="server" Width="100%" cellpadding="0" cellspacing="0"
									GridLines="Both" ShowFooter="False" ShowHeader="False" AutoGenerateColumns="False" EnableViewState="true"
									AllowPaging="True" PageSize="50" DataKeyField="LibraryId" BorderColor="LightGray">
									<ItemStyle CssClass="ugd" HorizontalAlign="center"/>
									<HeaderStyle Height="15px" VerticalAlign="Middle" CssClass="gh" BorderStyle="Solid" BorderWidth="1pt"></HeaderStyle>
									<SelectedItemStyle CssClass="ugo"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="uga"></AlternatingItemStyle>
									<EditItemStyle HorizontalAlign="Left"></EditItemStyle>
									<FooterStyle BorderWidth="0px"></FooterStyle>
									<PagerStyle Visible="false" HorizontalAlign="Center" Height="15px" BackColor="#cccccc" Mode="NumericPages"></PagerStyle>
									<Columns>
										<asp:TemplateColumn Visible="false">
											<ItemStyle VerticalAlign="middle" Width="20px"></ItemStyle>
											<HeaderTemplate>
												<asp:CheckBox id="selectAll" runat="server" Visible="false"></asp:CheckBox>
											</HeaderTemplate>
											<ItemTemplate>
												<asp:CheckBox id="selectThis" runat="server" Checked='<%# Container.ItemIndex==((DataGrid)Container.Parent.Parent).SelectedIndex%>'></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<ItemStyle VerticalAlign="top" HorizontalAlign="left"></ItemStyle>
											<ItemTemplate>
												<asp:LinkButton id="lnkEdit" onclick="ChooseLibrary" runat="server">
													<%#highlightFilter(DataBinder.Eval(Container.DataItem, "LibraryName").ToString(),filter)%>
												</asp:LinkButton>
												<br/>
												<span style="font-size:80%;font-style:italic;margin-left:5px;">
													<%#highlightFilter(WrapString(DataBinder.Eval(Container.DataItem, "LibraryDescription").ToString(), 30),filter)%>
												</span>
											</ItemTemplate>
											<EditItemTemplate>
												<asp:Textbox id="editLibraryName" runat="server" Width="200px" Text='<%#DataBinder.Eval(Container.DataItem,"LibraryName")%>'></asp:Textbox>
												<textarea id="editLibraryDescription" runat="server" style="vertical-align:top;" cols="32" rows="3"><%#DataBinder.Eval(Container.DataItem, "LibraryDescription")%></textarea>
											</EditItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:DataGrid>
								<ucs:CustomPager id="librariesPagerBottom" runat="server"></ucs:CustomPager>
							</td>
						</tr>
					</table>
				</td>
				<td valign="top" width="80%" style="padding:2px;">
					<asp:Label id="propertiesMsgLbl" runat="server"></asp:Label>
					<fieldset><legend><asp:Label id="propertiesLbl" runat="server" Text="Properties"></asp:Label></legend>
						<igtbar:ultrawebtoolbar id="pTb" runat="server" ItemWidthDefault="60px" width="100%" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents Click="templateToolbar_Click"></ClientSideEvents>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Save" ToolTip="Save changes" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
				        <igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
				        <igtbar:TBarButton Key="Delete" ToolTip="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Text=" ">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
							</Items>
						</igtbar:ultrawebtoolbar>
						<table cellspacing=0 cellpadding=0 width="100%" border=0>
							<tr valign=middle>
								<td class=editLabelCell width=130>
									<asp:label id=lbName Text="Name" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:TextBox id=txtNameValue runat="server"></asp:TextBox>
									<asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtNameValue" ErrorMessage="Name field is mandatory."></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr valign=middle>
								<td class=editLabelCell width=130>
									<asp:label id=lbDescription Text="Description" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<textarea id="txtDescriptionValue" runat="server" style="vertical-align:top;" cols="51" rows="5"></textarea>
								</td>
							</tr>
							<tr valign=middle>
								<td class=editLabelCell width=130>
									<asp:label id=lbType Text="Type" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:Dropdownlist id="txtTypeValue" OnChange="typeChanged(this);" Runat="server">
										<asp:ListItem Text="Internal" Value="1"></asp:ListItem>
										<asp:ListItem Text="External" Value="0"></asp:ListItem>
									</asp:Dropdownlist>
								</td>
							</tr>
							<tr valign=middle id="versioningRow" style="display:none;">
								<td class=editLabelCell width=130>
									<asp:label id=lbVersioning Text="Versioning" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:Checkbox id="txtVersionValue" OnClick="versioningChanged(this);" Text="Enabled" Runat="server"></asp:Checkbox>
								</td>
							</tr>
							<tr valign=middle id="maxVersionsRow" style="display:none;">
								<td class=editLabelCell width=130>
									<asp:label id=lbMaxVersions Text="Max number of versions" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:Checkbox id="maxVersionsUnlimited" OnClick="maxVersionsUnlimitedChanged(this);" Text="Unlimited" Runat="server"></asp:Checkbox>
									<igtxt:WebNumericEdit id="maxVersionsValue" ValueText="0" MinValue="0" runat="server" Width="25px"></igtxt:WebNumericEdit>
								</td>
							</tr>
							<tr valign=middle id="storageRow" style="display:none;">
								<td class=editLabelCell width=130>
									<asp:label id=lbStorage Text="Storage" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:Dropdownlist id="txtStorageValue" OnChange="storageChanged(this);" Runat="server">
										<asp:ListItem Text="Database" Value="Database"></asp:ListItem>
										<asp:ListItem Text="File system" Value="FileSystem"></asp:ListItem>
									</asp:Dropdownlist>
								</td>
							</tr>
							<tr valign=middle id="pathRow" style="display:none;">
								<td class=editLabelCell width=130>
									<asp:label id=lbPath Text="Path" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:TextBox id=txtPathValue runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr valign=middle>
								<td class=editLabelCell width=130>
									<asp:label id=lbResourceType Text="Handled types" Runat="server"></asp:label>
								</td>
								<td class=ugd>
									<asp:checkBoxlist id="txtResourceTypeValue" runat="server" DataTextField="Name" DataValueField="Id" RepeatDirection="horizontal"></asp:checkBoxlist>
									<asp:Repeater id="resourceTypeBoxes" Visible="false" runat="server">
										<ItemTemplate>
											<asp:checkBox id="txtResourceTypeValue2" runat="server" OnClick='<%# "selType(" + Container.ItemIndex + ",this.checked);"%>' Text='<%# DataBinder.Eval(Container.DataItem,"Name")%>' Value='<%# DataBinder.Eval(Container.DataItem,"Id")%>'></asp:checkBox>
										</ItemTemplate>
									</asp:Repeater>
									<hcx:CheckBoxGroupedList id="txtResourceTypeValue3" runat="server" Visible="false">
										<Groups>
											<hcx:ListItemGroup Text="All" Value="All"></hcx:ListItemGroup>
										</Groups>
									</hcx:CheckBoxGroupedList>
								</td>
							</tr>
						</table>
					</fieldset>
					<asp:Panel id="libOptions" runat="server" visible="false">
					<fieldset><legend>Options</legend>
						<igtbar:ultrawebtoolbar id="oTb" runat="server" ItemWidthDefault="60px" width="100%" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents Click="templateToolbar_Click"></ClientSideEvents>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="Save" ToolTip="Save changes" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBLabel Text=" ">
									<DefaultStyle Width="100%"></DefaultStyle>
								</igtbar:TBLabel>
							</Items>
						</igtbar:ultrawebtoolbar>
						<table cellspacing=0 cellpadding=0 width="100%" border=0>
							<asp:Repeater id="resourceTypesAttributes" runat="server" Visible="false">
								<ItemTemplate>
									<asp:Repeater id="variantAttributes" runat="server">
										<ItemTemplate>
											<tr id='<%# "attrRow" + ((System.Web.UI.WebControls.RepeaterItem)Container.Parent.Parent).ItemIndex + "_" + Container.ItemIndex%>' valign="middle">
												<td class="editLabelCell" style="width:130px">
													<%# Container.DataItem%>
												</td>
												<td class="ugd">
													<asp:checkBoxlist id="attrValueBox" runat="server"></asp:checkBoxlist>
													Default value: <asp:dropdownlist id="attrValueList" runat="server"></asp:dropdownlist>
												</td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</ItemTemplate>
							</asp:Repeater>
						</table>
					</fieldset>
					</asp:Panel>
				</td>
			</tr>
		</table>

	<script>
		function typeChanged(typeList)
		{
			var versioningRow = document.getElementById("versioningRow");
			var maxVersionsRow = document.getElementById("maxVersionsRow");
			var storageRow = document.getElementById("storageRow");
			var pathRow = document.getElementById("pathRow");
			if (versioningRow!=null && maxVersionsRow!=null && storageRow!=null && pathRow!=null)
			{
				var visible = (typeList.options[typeList.selectedIndex].value == "1");
				versioningRow.style.display = visible?"":"none";
				maxVersionsRow.style.display = visible?"":"none";
				storageRow.style.display = visible?"":"none";
				pathRow.style.display = visible?"":"none";
				
				if (visible)
				{
					var txtStorageValue = document.getElementById("<%= txtStorageValue.ClientID%>");
					if (txtStorageValue!=null)
						storageChanged(txtStorageValue);
					var txtVersionValue = document.getElementById("<%= txtVersionValue.ClientID%>");
					if (txtVersionValue!=null)
						versioningChanged(txtVersionValue);
				}
			}
		}
		
		function versioningChanged(versioningList)
		{
			var maxVersionsRow = document.getElementById("maxVersionsRow");
			if (maxVersionsRow!=null)
				maxVersionsRow.style.display = versioningList.checked?"":"none";
			var maxVersionsUnlimited = document.getElementById("<%= maxVersionsUnlimited.ClientID%>");
			if (maxVersionsUnlimited!=null)
				maxVersionsUnlimitedChanged(maxVersionsUnlimited);
		}
		
		function maxVersionsUnlimitedChanged(maxVersionChk)
		{
		  var edit = igedit_getById('<%= maxVersionsValue.ClientID%>');
		  if (edit!=null)
			  edit.setEnabled(!maxVersionChk.checked);
		}
		
		function storageChanged(storageList)
		{
			var pathRow = document.getElementById("pathRow");
			if (pathRow!=null)
			{
				var visible = (storageList.options[storageList.selectedIndex].value == "FileSystem");
				pathRow.style.display = visible?"":"none";
			}
		}
		
		function selType(typeId,checked)
		{
			var pre="attrRow";
			var j=0;
			var tr=document.getElementById(pre+typeId+"_"+j);
			while(tr!=null)
			{
				tr.style.display=checked?"":"none";
				tr=document.getElementById(pre+typeId+"_"+(++j));
			}
		}

		function typeUpd(l)
		{
			if(l!=null)
			{
				var pre="attrRow";
				for(var i=0;i<l.options.length;i++)
				{
					var j=0;
					var tr=document.getElementById(pre+i+"_"+j);
					while(tr!=null)
					{
						tr.style.display=l.options[i].selected?"":"none";
						tr=document.getElementById(pre+i+"_"+(++j));
					}
				}
			}
		}
		
	</script>