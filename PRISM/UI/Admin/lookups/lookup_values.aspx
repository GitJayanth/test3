<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Lookup_Values" CodeFile="Lookup_Values.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtxt" Namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics2.WebUI.WebDataInput.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Lookups values</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
      var object;
		  function SC(){
			  object = document.getElementById('ctl00$HOCP$txtValue');
			  specialCharacter(object);
			  return false;
			}

			function uwToolbar_Click(oToolbar, oButton, oEvent){
		        if (oButton.Key == 'filter')
		        {     		     
		            DoSearch();	
                    oEvent.cancelPostBack = true;
                    return;
                }
		        if (oButton.Key == 'List') {
                back();
                oEvent.cancelPostBack = true;
                }
		        if (oButton.Key == 'Delete') {
                    if (dg_nbItems_Checked==0)
		            {
	                alert('You must select at least one item');
	                oEvent.cancelPostBack = true;
	                }
	                else
	                {
                    oEvent.cancelPostBack = !confirm("Are you sure?");
                    }
                }
                var Checked =0;
                if (oButton.Key == 'Save')
                {
                myForm = document.forms[0];
                    for (i=0; i<myForm.length; i++)
                    {
                    if ((myForm.elements[i].id.indexOf('IsTranslateRows')!=-1))
                        {
                            if (!(myForm.elements[i].disabled) && myForm.elements[i].checked)
                            Checked =Checked+1;
                        }  
                    }
                }
                if (Checked>0)
                //oEvent.cancelPostBack = !confirm("Setting IsTranslate to Yes to a term value\n will be impacting all existing Inputform ?");                
                oEvent.cancelPostBack = !confirm("Setting IsTranslate to Yes to a term value\n will impact all existing Inputform(s).\nWould you like to continue ?");                
		     } 
		  
		     ///#ACQ8.20 Start
        //////////////////////////////////
        function IsTranslateHeader_Click(obj)
        //////////////////////////////////
        //grid: select/unselect all
        {
            var chkVal = obj.checked;
            var idVal = obj.id;
            // user is selecting/unselecting one checkbox
            
            if (chkVal)
            {
                myForm = document.forms[0];
                for (i=0; i<myForm.length; i++)
                {
                if ((myForm.elements[i].id.indexOf('IsTranslateRows')!=-1))
                    {
                        if (!myForm.elements[i].disabled)
                            myForm.elements[i].checked = true;
                    }  
                 }
            }
            else
            {
                myForm = document.forms[0];
                for (i=0; i<myForm.length; i++)
                {
                if ((myForm.elements[i].id.indexOf('IsTranslateRows')!=-1))
                    {
                        if (!myForm.elements[i].disabled)
                            myForm.elements[i].checked = false;
                    }  
                 }
            }
        }
		</script>
</asp:Content>
<%--Removed the width property from ultrawebtoolbar to fix moving button issue  by Radha S--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
			<table class="main" cellspacing="0" cellpadding="0">
				<tr valign="top" style="height:1px">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="120px"
							ImageDirectory=" ">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<Items>
								<igtbar:TBarButton Key="List" ToolTip="Back to list" Text="List" Image="/hc_v4/img/ed_back.gif">
									<DefaultStyle Width="80px"></DefaultStyle>
								</igtbar:TBarButton>
								<igtbar:TBSeparator Key="ExportSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Export" Text="Export" Image="/hc_v4/img/ed_download.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="SaveSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Save" Text="Apply changes" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
								<igtbar:TBSeparator Key="DeleteSep"></igtbar:TBSeparator>
								<igtbar:TBarButton Key="Delete" ToolTip="Delete selected" Text="Delete selected" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
							</Items>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr height="1">
					<td>
						<igtbl:ultrawebgrid id="dg" runat="server" Width="100%">
							<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle"
								RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display" CellClickActionDefault="Edit">
                                <%--Removed the css class and added inline property to fix grid line issue By Radha S--%>
								<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Cursor="Hand" BackColor="LightGray" Font-Bold="true"> <%--CssClass="gh">--%>
									<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
								</HeaderStyleDefault>
								<FrameStyle Width="100%" CssClass="dataTable">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </FrameStyle>
								<RowAlternateStyleDefault CssClass="uga">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </RowAlternateStyleDefault>
								<RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd">
                                    <BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
                                </RowStyleDefault>
							  <ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd" InitializeLayoutHandler="dg_InitializeLayoutHandler"></ClientSideEvents>
							</DisplayLayout>
							<Bands>
								<igtbl:UltraGridBand AllowSorting="No">
									<Columns>
										<igtbl:TemplatedColumn Key="Select" Width="20px" BaseColumnName="" FooterText="">
											<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
											<HeaderTemplate>
												<asp:CheckBox id="g_ca" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
											</HeaderTemplate>
											<CellTemplate>
												<asp:CheckBox id="g_sd" onclick="javascript:return g_su(this);" runat="server"></asp:CheckBox>
											</CellTemplate>
											<Footer Key="Select" Caption=""></Footer>
											<Header Key="Select"></Header>
										</igtbl:TemplatedColumn>
										<igtbl:UltraGridColumn Key="LookupValueId" BaseColumnName="Id" ServerOnly="True"></igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="TermValue" HeaderText="Value" BaseColumnName="Text" Width="230px" AllowUpdate="Yes">
											<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:UltraGridColumn Key="Comment" Width="300" HeaderText="Comment" BaseColumnName="Comment" AllowUpdate="Yes">
											<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
										</igtbl:UltraGridColumn>
										<igtbl:TemplatedColumn Key="IsTranslateRow" Width="120px" FooterText="" BaseColumnName="IsTranslatable">
                                        <CellStyle VerticalAlign="Top" Wrap="True">
                                        </CellStyle>
                                        <HeaderTemplate>
                                          <asp:CheckBox ID="IsTranslateHeader" onclick ="javascript:IsTranslateHeader_Click(this);" runat="server" AutoPostBack=false></asp:CheckBox>IsTranslatable
                                        </HeaderTemplate>
                                        <CellTemplate>
                                          <asp:CheckBox ID="IsTranslateRows" runat="server" AutoPostBack=false Enabled=<%# !Convert.ToBoolean(Container.Cell.Text) %> Checked=<%# Convert.ToBoolean(Container.Cell.Text) %> ></asp:CheckBox>
                                        </CellTemplate>
                                        </igtbl:TemplatedColumn>
									</Columns>
								</igtbl:UltraGridBand>
							</Bands>
						</igtbl:ultrawebgrid>
					</td>
				</tr>
				<tr valign="top">
					<td>
						<table style="BORDER-RIGHT:1px solid; BORDER-TOP:1px solid; BORDER-LEFT:1px solid; BORDER-BOTTOM:1px solid"
							cellpadding="0" cellspacing="1" width="100%" class="datatable">
							<tr align="left">
								<td width="18" nowrap>&nbsp;</td>
								<td width="230"><asp:textbox MaxLength="250" id="txtValue" runat="server" Width="230px"></asp:textbox></td>
								<td width="200"><asp:textbox MaxLength="100" id="txtComment" runat="server" Width="300"></asp:textbox>&nbsp;</td>
								<td valign="middle" align="center" width="60px">
								    <asp:imagebutton id="btAddRow" runat="server" ImageAlign="Middle" ImageUrl="/hc_v4/img/ed_new.gif" />
								    <%--Nisha Verma Modfified the tool tip for QC 6433--%>
								    <img alt="Insert a special character for value" src="/hc_v4/img/ed_specialchars.gif" style="cursor: hand" onclick="SC()" />
								</td>
                <td>&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<input type="hidden" name="action"> 
			<input id="txtSortColPos" type="hidden" value="5" name="txtSortColPos" runat="server">
</asp:Content>