
<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" 
 Inherits="HyperCatalog.UI.Termbase.term_properties" CodeFile="Term_Properties.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Term properties</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
			<script type="text/javascript" language="javascript">
  
			function uwToolbar_Click(oToolbar, oButton, oEvent){
				if (oButton.Key == 'List') {
					back();
					oEvent.cancelPostBack = true;
				}
				if (oButton.Key == 'Save') {
					oEvent.cancelPostBack = MandatoryFieldMissing();
				}
				if (oButton.Key == 'Delete') {
					oEvent.cancelPostBack = !confirm("Are you sure?");
				}
			    } 
		    function Redirect(id)
  		        {
			        top.Redirect(id);
		        }
		
			</script> 
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript">
    document.body.onload=function(){try{document.getElementById('txtValue').focus();}catch(e){}};
    function ValidateType(obj)
	    {
		    var Value = document.getElementById('<%= DDL_TermTypeList.ClientID %>').options[document.getElementById('<%= DDL_TermTypeList.ClientID %>').selectedIndex].value;
		    document.getElementById('<%= IsTranslatable.ClientID %>').disabled = false;
		    if (Value=='S')
		    {
    		    document.getElementById('<%= IsTranslatable.ClientID %>').disabled = true;
    		    document.getElementById('<%= IsTranslatable.ClientID %>').checked = true;
		    }	
		} 
		
  </script>
			<table style="WIDTH: 100%; BORDER-COLLAPSE: collapse" cellspacing="0" cellpadding="0" border="0">
			    <tr valign="bottom" style="height:1px">
				    <td align="right">
					    <igtbar:ultrawebtoolbar id="uwToolbar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
						    <HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
						    <ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
						    <SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
						    <Items>
							    <igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
							    <igtbar:TBSeparator></igtbar:TBSeparator>
							    <igtbar:TBarButton Key="Save" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
							    <igtbar:TBSeparator Key="SepDelete"></igtbar:TBSeparator>
							    <igtbar:TBarButton Key="Delete" ToolTip="Delete the chunk" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
						    </Items>
						    <DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					    </igtbar:ultrawebtoolbar></td>
			    </tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
								<legend>
									<asp:label id="lType" runat="server">Type</asp:label>
								</legend>
								<asp:DropDownList id="DDL_TermTypeList" DataTextField="Name" DataValueField="Code" runat="server" onchange = "ValidateType(this);"></asp:DropDownList>
							</fieldset>
						</td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
								<legend>
									<asp:label id="lValue" runat="server">Value</asp:label>
								</legend>
								<asp:label id="lUsage" runat="server"> Used 0 time(s)</asp:label>
								<table width="100%" border="0" cellpadding="0" cellspacing="0">
									<tr>
										<td style="width:95%">
											<asp:textbox id="txtTermValue" runat="server" TextMode="MultiLine" width="100%"></asp:textbox>
										</td>
										<td>&nbsp;
											<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtTermValue"></asp:RequiredFieldValidator>
										</td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
								<legend>
									<asp:label id="lComment" runat="server">Comment</asp:label>
								</legend>
								<asp:textbox id="txtComment" runat="server" Width="95%" Columns="60" TextMode="MultiLine" Rows="2"></asp:textbox>
							</fieldset>
						</td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset id="fUsage" runat="server">
								<legend>
									<asp:label id="lUsageIF" runat="server">Usage</asp:label>
								</legend>
        <igtbl:UltraWebGrid ID="dg" runat="server" Width="100%">
          <DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes"
            Version="4.00" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free"
            EnableInternalRowsManagement="True" RowSelectorsDefault="No" Name="dg" TableLayout="Fixed"
            NoDataMessage="No data to display">
            <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
              <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt">
              </BorderDetails>
            </HeaderStyleDefault>
            <FrameStyle Width="100%" CssClass="dataTable">
            </FrameStyle>
            <RowAlternateStyleDefault CssClass="uga">
            </RowAlternateStyleDefault>
            <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
            </RowStyleDefault>
            <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"
              InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
          </DisplayLayout>
          <Bands>
            <igtbl:UltraGridBand>
              <Columns>
                <igtbl:UltraGridColumn Key="IFName" BaseColumnName="InputFormName" Width="400px">
                <Header Caption="Inputform"></Header>
                <CellStyle Wrap="true"></CellStyle>
                </igtbl:UltraGridColumn>
                <igtbl:UltraGridColumn Key="ContainerName" BaseColumnName="ContainerName" Width="100%">
                <Header Caption="Container"></Header>
                </igtbl:UltraGridColumn>
              </Columns>
            </igtbl:UltraGridBand>
          </Bands>
        </igtbl:UltraWebGrid>								
							</fieldset>
						</td>
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
							  <legend>
							  <asp:label id="Label2" runat="server">Translatable</asp:label>
							  	</legend>
							  	<asp:CheckBox ID="IsTranslatable" runat="server" onclick="javascript:ValidateType(this);"/> <asp:Label id="Label1" Runat="server">IsTranslatable</asp:Label>
                             </fieldset>
						</td>									
					</tr>
					<tr valign="top" style="height:1px">
						<td>
							<fieldset>
                               <legend>
									<asp:label id="lAuthor" runat="server">Author</asp:label>
								</legend>
                                <asp:HyperLink id="hlCreator" runat="server">HyperLink</asp:HyperLink>    
                            <asp:label id="lbOrganization" runat="server"> [Organization] - </asp:label>
                            <asp:Label id="lbCreatedOn" Runat="server"></asp:Label>
                            </fieldset>
                        
						</td>
					</tr>
			</table>
			<asp:Label id="lbMessage" runat="server" Width="100%" Visible="False">Message</asp:Label>

</asp:Content>