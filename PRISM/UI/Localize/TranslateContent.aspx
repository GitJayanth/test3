<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="TranslateContent.aspx.cs" Inherits="UI_Localize_TranslateContent" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1" runat="server">
 <script language="javascript" type="text/javascript">
            var _pinned = true;
            
            function toggleDockPanel()
		    {
		    var main = parent.document.getElementById('main');
			var content = parent.document.getElementById('contents');
			var hideImage=document.getElementById('theDockPanelImage');
		    if (_pinned)
		    {
			content.setAttribute("width","0");
			main.setAttribute("width","100%");
            main.setAttribute("left","0");
            hideImage.src='./Images/pushPinClose.gif';

            }
            else
            {
            content.setAttribute("width","25%");
			main.setAttribute("width","75%");
            main.setAttribute("left","25%");
            hideImage.src='./Images/pushPin.gif';
            }
            _pinned = !_pinned;
			}
         </script>
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="WIDTH: 100%; HEIGHT: 100%" cellspacing=0 cellpadding=0 border=0>
			<tr valign=top height=1>
			    <TD style="BACKGROUND-IMAGE: url(./images/onyxBar.gif)" align="left" width="2">
			        <IMG id="theDockPanelImage" onclick="toggleDockPanel();" src="./images/pushPin.gif">
			    </td>
		    <td>
				<igtbar:ultrawebtoolbar id=mainToolBar runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="submit" Text="Save" Image="/hc_v4/img/ed_save.gif"></igtbar:TBarButton>
						<igtbar:TBLabel Key="CurrentFile" Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign=top>
			<td colspan="2">
        <igtbl:UltraWebGrid ID="MyWebGrid" runat="server" Height="100%" Width="100%" OnInitializeLayout="MyWebGrid_InitializeLayout">
            <Bands>
                <igtbl:UltraGridBand>
                    <AddNewRow View="NotSet" Visible="NotSet">
                    </AddNewRow>
                    <FilterOptions AllString="" EmptyString="" NonEmptyString="">
                        <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                            CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                            Font-Size="11px" Width="200px">
                            <Padding Left="2px" />
                        </FilterDropDownStyle>
                        <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                        </FilterHighlightRowStyle>
                    </FilterOptions>
                </igtbl:UltraGridBand>
            </Bands>
	        <DisplayLayout RowHeightDefault="100%" Version="4.00" SelectTypeRowDefault="Single" HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="MyWebGrid" TableLayout="Fixed" CellClickActionDefault="RowSelect" NoDataMessage="No event"
	          AllowUpdateDefault="Yes" >
		        <HeaderStyleDefault VerticalAlign="Middle" CssClass="gh"/>
		        <FrameStyle Width="100%" CssClass="dataTable"/>
		        <RowAlternateStyleDefault CssClass="uga"/>
		        <RowStyleDefault TextOverflow="Ellipsis" VerticalAlign="Top" BorderWidth="1px" CssClass="ugd"/>
            <FilterOptionsDefault AllString="(All)" EmptyString="(Empty)" NonEmptyString="(NonEmpty)">
                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                    CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                    Font-Size="11px" Width="200px">
                    <Padding Left="2px" />
                </FilterDropDownStyle>
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                </FilterHighlightRowStyle>
            </FilterOptionsDefault>
	        </DisplayLayout>
        </igtbl:UltraWebGrid>
                <asp:TextBox ID="EditTranslated" runat="server" Rows="3" TextMode="MultiLine" Visible="False"></asp:TextBox></td>
		</tr>
  </table>
    </div>
    </form>
</body>
</html>

	