<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Translate_content" CodeFile="Translate_content.aspx.cs" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Translate content</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script type="text/javascript">
			var winChunkEdit = null;
		
			function ed(gridName, index, cultureCode, itemId)
			{
			    var param = 'g='+gridName+'&r='+index+'&c='+cultureCode+'&i='+itemId;
				var url = '../Translate/Translate_EditChunk.aspx?'+param;
				var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
				url += '#target';
				winChunkEdit = OpenModalWindow(url,'chunkwindow', 370, 610, 'no');  
			}
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
			<table cellspacing="1" cellpadding="0" style="WIDTH: 100%; HEIGHT: 100%" border="0" class="main">
				<tr valign="top" style="height:1px">
					<td class="sectiontitle">
						<asp:Label id="lblTitleItem" runat="server"></asp:Label>
					</td>
				</tr>
				<tr valign="top" style="height:auto">
					<td>
						<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 100%">
							<igtbl:UltraWebGrid id="dg" runat="server" Width="100%">
								<DisplayLayout MergeStyles="False" AutoGenerateColumns="False" AllowSortingDefault="Yes" Version="4.00"
									HeaderClickActionDefault="SortSingle" AllowColSizingDefault="Free" EnableInternalRowsManagement="True"
									RowSelectorsDefault="No" Name="dg" TableLayout="Fixed" NoDataMessage="No data to display">
									<HeaderStyleDefault VerticalAlign="Middle" CssClass="gh">
										<BorderDetails StyleBottom="Solid" WidthRight="1pt" StyleRight="Solid" WidthBottom="1pt"></BorderDetails>
									</HeaderStyleDefault>
									<FrameStyle Width="100%"></FrameStyle>
									<ClientSideEvents BeforeSortColumnHandler="dg_BeforeSortColumnHandler" KeyDownHandler="g_kd"></ClientSideEvents>
									<ActivationObject BorderStyle="None" AllowActivation="False" BorderColor=""></ActivationObject>
								</DisplayLayout>
								<Bands>
									<igtbl:UltraGridBand BaseTableName="Chunks" Key="Chunks" BorderCollapse="Collapse" DataKeyField="Id">
										<Columns>
											<igtbl:UltraGridColumn Key="InputFormName" Hidden="True" BaseColumnName="InputFormName"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="InputFormContainerId" Hidden="True" BaseColumnName="InputFormContainerId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="ContainerId" Hidden="True" BaseColumnName="ContainerId"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn Key="Tag" Hidden="True" BaseColumnName="Tag"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="" Key="ChunkStatus" Width="14px" BaseColumnName="ChunkStatus"></igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Container" Key="ContainerName" IsBound="True" Width="40%" BaseColumnName="ContainerName" CellMultiline="Yes">
												<CellStyle VerticalAlign="Top" Wrap="True" CssClass="ptb1"></CellStyle>
												<Header Caption="Container"></Header>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Master value" Key="MasterValue" IsBound="True" Width="20%" BaseColumnName="MasterValue" CellMultiline="Yes">
												<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
												<Header Caption="Master value"></Header>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Value" Key="Value" IsBound="True" Width="20%" AllowGroupBy="No" BaseColumnName="ChunkValue" CellMultiline="Yes">
												<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
												<Header Caption="Value"></Header>
											</igtbl:UltraGridColumn>
											<igtbl:UltraGridColumn HeaderText="Comment" Key="Value" IsBound="True" Width="20%" AllowGroupBy="No" BaseColumnName="Comment" CellMultiline="Yes">
												<CellStyle VerticalAlign="Top" Wrap="True"></CellStyle>
												<Header Caption="Comment"></Header>
											</igtbl:UltraGridColumn>
										</Columns>
									</igtbl:UltraGridBand>
								</Bands>
							</igtbl:UltraWebGrid>
						</div>
					</td>
				</tr>
			</table>
</asp:Content>