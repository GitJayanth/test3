<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.master" Inherits="HyperCatalog.UI.Delivery.DeliveryOutputs" CodeFile="deliveryOutputs.aspx.cs" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Delivery packages</asp:Content><asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">
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

protected string sizeStr(HyperCatalog.Business.DAM.FileVersion file)
{
	return file==null?"-":convertBytes((long)file.BinarySize);
}


</script>
<STYLE>
.lbl_msgInformation { FONT-WEIGHT: bold; COLOR: green }
.lbl_msgWarning { FONT-WEIGHT: bold; COLOR: orange }
.lbl_msgError { FONT-WEIGHT: bold; COLOR: red }
</STYLE>
  <style>
      .editOptionalLabelCell{
        border-right: 1pt double;
        border-top: 1pt double;
        font-size: 8pt;
        border-left: 1pt double;
        border-bottom: silver 1pt double;
        padding-left: 1px;
        padding-right: 10px;
        color: black;
        background-color: lightgrey;
      }
  </style>
  <script>
    function showhide(href, divId)
    {
      var div = document.getElementById(divId);
      var show = (div.style.display == 'none');
      div.style.display = (show?'':'none');
      href.innerText = (show?'- Previous versions':'+ Previous versions');
    }
  </script>
	<table style="WIDTH: 100%; HEIGHT: 100%" cellspacing=0 cellpadding=0 border=0>
		<tr valign=top>
			<td>
				<igtbar:ultrawebtoolbar id=mtb runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" width="100%" OnButtonClicked="mtb_ButtonClicked">
					<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
					<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
					<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
					<Items>
						<igtbar:TBarButton Key="List" ToolTip="Back to delivery packages" Text="List" Image="/hc_v4/img/ed_back.gif"></igtbar:TBarButton>
						<igtbar:TBLabel Text=" ">
							<DefaultStyle Width="100%"></DefaultStyle>
						</igtbar:TBLabel>
					</Items>
				</igtbar:ultrawebtoolbar>
			  
	      <asp:DataGrid id="DataGrid1" runat="server" Width="100%" BorderColor="LightGray" DataKeyField="Id"
		      AllowSorting="True" PageSize="500" AllowPaging="True" EnableViewState="true" AutoGenerateColumns="False"
		      ShowFooter="False" GridLines="Both" cellspacing="0" cellpadding="1">
		      <ItemStyle CssClass="ugd" />
		      <HeaderStyle Height="15px" VerticalAlign="Top" CssClass="gh" BorderStyle="Solid" BorderWidth="1pt"></HeaderStyle>
		      <SelectedItemStyle CssClass="ugo"></SelectedItemStyle>
		      <AlternatingItemStyle CssClass="uga"></AlternatingItemStyle>
		      <EditItemStyle HorizontalAlign="Left"></EditItemStyle>
		      <FooterStyle BorderWidth="0px"></FooterStyle>
		      <PagerStyle Visible="False" HorizontalAlign="Center" Height="15px" BackColor="#cccccc" Mode="NumericPages"></PagerStyle>
		      <Columns>
			      <asp:TemplateColumn SortExpression="Name">
				      <ItemStyle VerticalAlign="Top" Width="40px"></ItemStyle>
				      <ItemTemplate>
					      <a href="<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).URI%>"><img src="<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).URI%>?thumbnail=1&size=40" width="40" height="40" border="0"/></a>
				      </ItemTemplate>
			      </asp:TemplateColumn>
			      <asp:TemplateColumn SortExpression="Name">
				      <ItemStyle VerticalAlign="Top" Width="350px"></ItemStyle>
				      <HeaderTemplate>
					      Name
				      </HeaderTemplate>
				      <ItemTemplate>
					      <a href="<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).URI%>"><%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).Name %></a>
					      <BR />
					      <asp:Panel ID="pnlFiles" runat="server" Visible='<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).MasterVariant.Files.Count > 0 %>'>
					        <a href="#" onclick='<%# "showhide(this,&quot;" + Container.FindControl("pnlOlderFiles").ClientID + "&quot;);"%>'>+ Previous versions</a>
					        <asp:Panel ID="pnlOlderFiles" runat="server" OnInit="pnlOlderFiles_OnInit">
					          <asp:Repeater ID="olderFiles" runat="server" DataSource='<%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).MasterVariant.Files %>'>
					            <ItemTemplate>
					              &nbsp;&nbsp;<a href='<%# ((HyperCatalog.Business.DAM.FileVersion)Container.DataItem).URI%>'><%# ((HyperCatalog.Business.DAM.FileVersion)Container.DataItem).CreatedOn%></a>
					            </ItemTemplate>
					            <SeparatorTemplate><br /></SeparatorTemplate>
					          </asp:Repeater>
					        </asp:Panel>
					      </asp:Panel>
				      </ItemTemplate>
			      </asp:TemplateColumn>
			      <asp:TemplateColumn>
				      <HeaderStyle HorizontalAlign="left"></HeaderStyle>
				      <HeaderTemplate>
					      Last refresh date
				      </HeaderTemplate>
				      <ItemStyle VerticalAlign="top" HorizontalAlign="left" Width="125px"></ItemStyle>
				      <ItemTemplate>
					      <%# ((HyperCatalog.Business.DAM.Resource)Container.DataItem).MasterVariant.CurrentFile.CreatedOn%>
				      </ItemTemplate>
			      </asp:TemplateColumn>
			      <asp:TemplateColumn>
				      <HeaderStyle HorizontalAlign="left"></HeaderStyle>
				      <HeaderTemplate>
					      Size
				      </HeaderTemplate>
				      <ItemStyle VerticalAlign="top" HorizontalAlign="left"></ItemStyle>
				      <ItemTemplate>
					      <%# sizeStr(((HyperCatalog.Business.DAM.Resource)Container.DataItem).MasterVariant.CurrentFile)%>
				      </ItemTemplate>
			      </asp:TemplateColumn>
		      </Columns>
	      </asp:DataGrid>
	      <asp:DataGrid id="documents" runat="server" Width="100%" BorderColor="LightGray" 
	        AllowSorting="True" PageSize="500" AllowPaging="True" EnableViewState="true" AutoGenerateColumns="False"
		      ShowFooter="False" GridLines="Both" cellspacing="0" cellpadding="1" OnItemCreated="documents_ItemCreated">
		      <ItemStyle CssClass="ugd" />
		      <HeaderStyle Height="15px" VerticalAlign="Top" CssClass="gh" BorderStyle="Solid" BorderWidth="1pt"></HeaderStyle>
		      <SelectedItemStyle CssClass="ugo"></SelectedItemStyle>
		      <AlternatingItemStyle CssClass="uga"></AlternatingItemStyle>
		      <EditItemStyle HorizontalAlign="Left"></EditItemStyle>
		      <FooterStyle BorderWidth="0px"></FooterStyle>
		      <PagerStyle Visible="False" HorizontalAlign="Center" Height="15px" BackColor="#cccccc" Mode="NumericPages"></PagerStyle>
		      <Columns>
					  <asp:TemplateColumn>
						  <ItemStyle VerticalAlign="top" HorizontalAlign="left"></ItemStyle>
				      <ItemStyle VerticalAlign="Top" Width="40px"></ItemStyle>
						  <ItemTemplate>
						  </ItemTemplate>
					  </asp:TemplateColumn>
					  <asp:TemplateColumn>
				      <HeaderTemplate>
					      Name
				      </HeaderTemplate>
				      <ItemStyle VerticalAlign="Top" Width="350px"></ItemStyle>
						  <ItemTemplate>
							  <asp:HyperLink id="lnkEdit" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, "ResourceName").ToString()%>'></asp:HyperLink>
					      <BR />
					      <asp:Panel ID="pnlFiles" runat="server">
					        <a href="#" onclick='<%# "showhide(this,&quot;" + Container.FindControl("pnlOlderFiles").ClientID + "&quot;);"%>'>+ Previous versions</a>
					        <asp:Panel ID="pnlOlderFiles" runat="server" OnInit="pnlOlderFiles_OnInit">
					          <asp:Repeater ID="olderFiles" runat="server">
					            <ItemTemplate>
					              &nbsp;&nbsp;<a href='<%# damHandlerURL + CurrentLibrary.Name + "/" + ((System.Data.DataRow)Container.DataItem)["FileId"] + ((System.Data.DataRow)Container.DataItem)["FileExtension"]%>' target="_blank"><%# ((System.Data.DataRow)Container.DataItem)["FileCreatedOn"]%></a>
					            </ItemTemplate>
					            <SeparatorTemplate><br /></SeparatorTemplate>
					          </asp:Repeater>
					        </asp:Panel>
					      </asp:Panel>
						  </ItemTemplate>
					  </asp:TemplateColumn>
					  <asp:TemplateColumn>
				      <HeaderStyle HorizontalAlign="left"></HeaderStyle>
				      <HeaderTemplate>
					      Last refresh date
				      </HeaderTemplate>
				      <ItemStyle VerticalAlign="top" HorizontalAlign="left" Width="125px"></ItemStyle>
				      <ItemTemplate>
					      
				      </ItemTemplate>
			      </asp:TemplateColumn>
			      <asp:TemplateColumn>
				      <HeaderStyle HorizontalAlign="left"></HeaderStyle>
				      <HeaderTemplate>
					      Size
				      </HeaderTemplate>
				      <ItemStyle VerticalAlign="top" HorizontalAlign="left"></ItemStyle>
				      <ItemTemplate>
					      
				      </ItemTemplate>
			      </asp:TemplateColumn>
		      </Columns>
	      </asp:DataGrid>
			</td>
		</tr>
  </table>
</asp:Content>