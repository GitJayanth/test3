<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.SearchResult" CodeFile="TemplateSearch.aspx.cs" %>
<%@ Register TagPrefix="igtab" Namespace="Infragistics.WebUI.UltraWebTab" Assembly="Infragistics2.WebUI.UltraWebTab.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Saved search</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">

  <script type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent){
		  if (oButton.Key == 'Options') {
        oEvent.cancelPostBack = true;
        var optionsFieldset = document.getElementById('optionsFieldset');
        optionsFieldset.style.display=(optionsFieldset.style.display==''?'none':'');
      }
		}
		
		document.onkeypress = checkkey;
    function checkkey(keyStroke)
    {
      if (event.keyCode == 13) 
      {     
        event.returnValue = false;
        event.cancelBubble = true; 
      }                              
    }

  </script>
  <%--Removed the width tag to fix moving icon issue by Radha S--%>
<table style="WIDTH: 100%; HEIGHT: 100%;" cellspacing="0" cellpadding="0" border="0">
		<tr valign="top" style="height:1px">
			<td>
				<igtbar:ultrawebtoolbar id="mainToolBar" runat="server" ItemWidthDefault="80px" CssClass="hc_toolbar">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
          <ClientSideEvents Click="uwToolbar_Click"></ClientSideEvents>
					<Items>
					  <igtbar:TBarButton Key="Options" ToolTip="Result options" Text="Options" ToggleButton="True" Image="/hc_v4/img/ed_options.gif"></igtbar:TBarButton>
					</Items>
				</igtbar:ultrawebtoolbar>
			</td>
		</tr>
		<tr valign="top">
			<td>
        <fieldset id="optionsFieldset" style="display:none;"><legend>Options</legend>
			    <table cellspacing="0" cellpadding="0" width="100%" border="0">
				    <tr valign="middle">
					    <td class="editLabelCell" style="width:130px">
					      Fields to display
					    </td>
					    <td class="ugd">
                <asp:checkboxlist id="fieldFilter" runat="server" RepeatColumns="4">
                  <asp:ListItem Text="ProductType" Value="ProductType" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Culture" Value="Culture" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Level" Value="LevelId" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="ProductName" Value="ProductName" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Item status" Value="Status" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="xmlname" Value="xmlname" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Container" Value="ContainerName" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Value" Value="ChunkValue" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Status" Value="ChunkStatus" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Modified" Value="ModifiedDate" Selected="true"></asp:ListItem>
                  <asp:ListItem Text="Inherited" Value="IsInherited" Selected="true"></asp:ListItem>
                </asp:checkboxlist>
					    </td>
				    </tr>
			    </table>
        </fieldset>
				<igtbl:UltraWebGrid id="templatesGrid" runat="server" Width="100%" DataKeyField="Id">
					<DisplayLayout AutoGenerateColumns="False" LoadOnDemand="Manual" AllowSortingDefault="Yes" RowHeightDefault="100%"
						Version="4.00" SelectTypeRowDefault="Single" ViewType="Hierarchical" HeaderClickActionDefault="SortSingle"
						AllowColSizingDefault="Free" RowSelectorsDefault="No" Name="componentsGrid" TableLayout="Fixed"
						CellClickActionDefault="RowSelect">
						<HeaderStyleDefault VerticalAlign="Middle" HorizontalAlign="Center" Font-Bold="true" BackColor="LightGray" Cursor="Hand"> <%--CssClass="gh">--%>
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
					</DisplayLayout>
					<Bands>
						<igtbl:UltraGridBand DataKeyField="Name">
							<Columns>
								<igtbl:TemplatedColumn HeaderText="Name" Key="Name" BaseColumnName="Name" Width="200px">
									<CellTemplate>
										<asp:LinkButton id="NameLink" onclick="UpdateGridItem" runat="server">
											<%#Container.Text%>
										</asp:LinkButton>
									</CellTemplate>
								</igtbl:TemplatedColumn>
								<igtbl:TemplatedColumn HeaderText="Visibility" Key="Visibility" BaseColumnName="IsPublic">
								  <CellTemplate>
										<%#((bool)Container.Value)?"Public":"Private"%>
									</CellTemplate>
								</igtbl:TemplatedColumn>
								<igtbl:UltraGridColumn HeaderText="Description" Key="Description" Width="100%" BaseColumnName="Description"></igtbl:UltraGridColumn>
                                <%--CHANGES MADE ALONG WITH PRISM UI TO PDB CHANGES BY REKHA THOMAS--%>
                               <igtbl:TemplatedColumn HeaderText="Data Source" Key="DBComponentId" BaseColumnName="DBComponentId" Width="100px">
								  <CellTemplate>
										<%# Container.Value.Equals(1)?"Crystal":Container.Value.Equals(33)?"PDB":"DWH" %>
									</CellTemplate>
								</igtbl:TemplatedColumn>								
								<igtbl:TemplatedColumn HeaderText="Delete" Key="DBComponentId" BaseColumnName="DBComponentId" Width="100px">
								<CellStyle HorizontalAlign="Center"></CellStyle>
								  <CellTemplate>
										<asp:Button ID="DeleteButton" runat="server" Text="Delete" Width="50px" Height="20px" OnClick="DeleteButton_Click" OnClientClick="return confirm('Are you sure you want to delete this record?')" />
									</CellTemplate>
								</igtbl:TemplatedColumn>
								 <%--END CHANGES MADE ALONG WITH PRISM UI TO PDB CHANGES BY REKHA THOMAS--%>
							</Columns>
						</igtbl:UltraGridBand>
					</Bands>
				</igtbl:UltraWebGrid>
			</td>
		</tr>
  </table>
</asp:Content>