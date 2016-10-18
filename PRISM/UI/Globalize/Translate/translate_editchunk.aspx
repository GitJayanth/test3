<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="Translate_EditChunk" CodeFile="Translate_EditChunk.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Chunk edit</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
		<script>
      		window.focus();
			var toolBar = null;
			var gridObject = null;
			var rowIndex = null;
			var row = null;
			var chunkFrame = null;
			var containerId = null;
			var itemId = null;
			var inputFormContainerId = null;
			var cultureCode = null;
			var nbGridLines = null;
			var mustReload = false;
			var gridName = '';
			
			///////////////////
			function InitVars()
			///////////////////
			{
				chunkFrame = document.getElementById("ChunkFrame");       
				gridObject=opener.igtbl_getGridById(gridName);
				nbGridLines =  gridObject.Rows.length;
				NavigateRow();
			}
			
			//////////////////////
			function NavigateRow()
			//////////////////////
			{
				row = gridObject.Rows.getRow(rowIndex);
				containerId = row.getCellFromKey("ContainerId").getValue();
				//cultureCode = row.getCellFromKey("CultureCode").getValue();
				//itemId = row.getCellFromKey("ItemId").getValue();
				inputFormContainerId = row.getCellFromKey("InputFormContainerId").getValue();
				var url = '../../Acquire/Chunk/Chunk.aspx?c='+containerId+'&i='+itemId+'&l='+cultureCode
				if (inputFormContainerId != null)
					url += '&ifcid='+inputFormContainerId;
				var curLine = rowIndex + 1;
				toolBar.Items[0].Element.innerText = row.getCellFromKey("InputFormName").getValue();
				toolBar.Items[1].Element.innerText = 'Chunk ' + curLine + '/' + nbGridLines;
    			chunkFrame.src = url;
			}
			
			//////////////////////////////////
			function UpdateGrid(status, value)
			//////////////////////////////////
			{
				row.getCellFromKey("ChunkStatus").Element.innerHTML = "<img src='/hc_v4/img/S" + status + ".gif'></img>";
				row.getCellFromKey("Value").setValue(value);
				//if ((status == "R") || (status == "D") || (status == "F")) // Commented for alternate for CR 5096
				if ((status == "D") || (status == "F")) // status draft or final
				{
					// refresh webtree and grid
					opener.parent.frames[1].document.getElementById("SelectedItem").value=""+itemId;
					mustReload = true;
				}
				NavigateRow();
			}

			///////////////////////////////////////////////////
			function uwToolbar_Click(oToolbar, oButton, oEvent)
			///////////////////////////////////////////////////
			{
				if (!opener)
				{
					window.close();
					return;
				}
				if (oButton.Key == 'Prev') 
				{
					if (rowIndex -1 >= 0)
					{
						rowIndex--;
						NavigateRow();
					}
				}
				else
				{
					if (oButton.Key == 'Next') 
					{
						if (rowIndex + 1 <= gridObject.Rows.length-1)
						{
							rowIndex++;
							NavigateRow();
						}
					}
					else
					{
						if (oButton.Key == 'Close')
							window.close();
					}
				}
				oEvent.cancelPostBack = true;
			}
		
			//////////////////////////////////////////////////////
			function uwToolbar_InitializeToolbar(oToolbar, oEvent)
			//////////////////////////////////////////////////////
			{
				toolBar = oToolbar;
				InitVars();
			}
		
			///////////////////////////
			function testOpenerReload()
			///////////////////////////
			{
				if (mustReload)
					opener.parent.frames[1].document.forms[0].submit(); // refresh webtree
			}
		</script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
  <script type="text/javascript" language="javascript">
    document.body.onunload = "testOpenerReload";
  </script>
			<table style="WIDTH:100%;HEIGHT:100%" border="0" cellpadding="0" cellspacing="0">
				<tr height="*">
					<td><iframe id="chunkFrame" border="0" FRAMEBORDER="0" SCROLLING="no" src="/hc_v4/dummy.htm" style="WIDTH:100%;HEIGHT:100%"></iframe>
					</td>
				</tr>
				<tr height="1">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" width="100%" ItemWidthDefault="20px" CssClass="hc_toolbar">
							<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
							<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
							<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
							<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
							<Items>
								<igtbar:TBLabel Text="Input Form" Key="IfName">
									<DefaultStyle Width="100%" Font-Bold="True" TextAlign="Left"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="Chunk x/n" Key="ChunkCount">
									<DefaultStyle Width="100px" TextAlign="Right"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBLabel Text="">
									<DefaultStyle Width="5px"></DefaultStyle>
								</igtbar:TBLabel>
								<igtbar:TBarButton Key="Prev" ToolTip="Previous" Image="/ig_common/Images/CorporateSilver/prev_up.gif"></igtbar:TBarButton>
								<igtbar:TBarButton Key="Next" ToolTip="Next" Image="/ig_common/Images/CorporateSilver/next_up.gif"></igtbar:TBarButton>
								<igtbar:TBarButton Key="Close" ToolTip="Close" Image="/hc_v4/Img/ed_cancel.gif"></igtbar:TBarButton>
							</Items>
						</igtbar:ultrawebtoolbar>
					</td>
				</tr>
			</table>
</asp:Content>