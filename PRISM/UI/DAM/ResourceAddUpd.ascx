<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourceAddUpd.ascx.cs" Inherits="UI_DAM_ResourceAddUpd" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<script><!--
function clipBd(t){window.clipboardData.setData("Text",t);window.status="Link copied to clipboard: \""+t+"\"";return false;}
function typeUpd(l){if(l!=null){var pre="attrRow";for(var i=0;i<l.options.length;i++){var j=0;var tr=document.getElementById(pre+i+"_"+j);while(tr!=null){tr.style.display=l.options[i].selected?"":"none";tr=document.getElementById(pre+i+"_"+(++j));}}}}
function ref(f){var v=f.value;var i=v.lastIndexOf("\\",v.length-1);v=v.substring(i>0?i+1:0,v.lastIndexOf(".",v.length-1));var r=document.getElementById("<%=txtResourceNameValue.ClientID%>");if (r!=null && !r.disabled && r.value==""){r.value=v;}}
function cfmDel(r){return confirm("Do you really want to delete this variant for the resource "+r+" ?");}
function pTb_Click(oT,oB,oE){if(oB.Key=="Del"){oE.cancelPostBack=!(confirm("You are about to delete this resource and all its variants."));}}
//-->
</script>  <asp:Label id="propertiesMsgLbl" runat="server"></asp:Label>
	<igtbar:ultrawebtoolbar id="pTb" runat="server" CssClass="hc_toolbar" width="100%" ItemWidthDefault="70px" OnButtonClicked="pTb_ButtonClicked">
		<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
		<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
		<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
		<ITEMS>
			<igtbar:TBarButton Key="Save" Image="/hc_v4/img/ed_save.gif" Text="Save" ToolTip="Save properties changes"></igtbar:TBarButton>
			<igtbar:TBarButton Key="Del" ToolTip="Delete" Text="Delete" Image="/hc_v4/img/ed_delete.gif"></igtbar:TBarButton>
			<igtbar:TBSeparator Key="SepCRT"></igtbar:TBSeparator>
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
			  Library
		  </td>
			<td class="ugd">
				<asp:dropdownlist id="ddlResourceLibrary" runat="server" Enabled="false" DataTextField="Name" DataValueField="Id"></asp:dropdownlist></td>
		</tr>
		<tr valign="middle">
			<td class="editOptionalLabelCell" width="130">
				Name
			</td>
			<td class="ugd">
				<asp:TextBox id="txtResourceNameValue" runat="server" Width="330px"></asp:TextBox></td>
		</tr>
		<tr valign="middle">
			<td class="editOptionalLabelCell" width="130">
				Description
			</td>
			<td class="ugd"><TEXTAREA id="txtResourceDescriptionValue" style="VERTICAL-ALIGN: top" rows="5" cols="51"
					runat="server"></TEXTAREA>
			</td>
		</tr>
		<tr valign="middle">
			<td class="editOptionalLabelCell" width="130">
				Type
			</td>
			<td class="ugd">
				<asp:dropdownlist id="ddlResourceType" runat="server" DataValueField="Id" DataTextField="Name" onchange="typeUpd(this);"></asp:dropdownlist>
			</td>
		</tr>
		<asp:Panel id="variantProperties" runat="server">
			<tr valign="middle">
				<td class="editLabelCell" style="width:130px">
					Culture
				</td>
				<td class="ugd">
					<asp:Dropdownlist id="ddlCulture" runat="server" DataValueField="Code" DataTextField="Name"></asp:Dropdownlist>
			  </td>
			</tr>
			<asp:Repeater id="rptResourceTypes" runat="server" OnItemDataBound="rptResourceTypes_ItemDataBound">
				<ItemTemplate>
					<asp:Repeater id="rptAttributes" runat="server">
						<ItemTemplate>
							<tr id='<%# "attrRow" + ((System.Web.UI.WebControls.RepeaterItem)Container.Parent.Parent).ItemIndex + "_" + Container.ItemIndex%>' valign="middle">
								<td class="editLabelCell" style="text-transform:capitalize;" width="130">
									<%# Container.DataItem%>
								</td>
								<td class="ugd">
									<asp:Label id="lblAutomatic" runat="server" Text="Automatic" Visible="false"></asp:Label>
									<asp:TextBox ID="txtAttribute" Runat="Server" Visible="false"></asp:TextBox>
									<asp:DropDownList ID="ddlAttribute" Runat="Server" Visible="false"></asp:DropDownList>
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
		<asp:Panel id="resourceInformation" runat="server">
			<tr valign="middle">
				<td class="editLabelCell" style="width:130px">
					<asp:label id="lbResourceURI" Text="URI" Runat="server"></asp:label></td>
				<td class="ugd"><A id="txtResourceURIValue" runat="server"></A></td>
			</tr>
		</asp:Panel>
		</table>
  <script type="text/javascript"><!--
    typeUpd(document.getElementById('<%Response.Write(ddlResourceType.ClientID);%>'));
    //-->
  </script>
