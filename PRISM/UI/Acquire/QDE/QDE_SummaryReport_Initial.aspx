<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="HyperCatalog.UI.Acquire.QDE.QDE_SummaryReport_Initial" CodeFile="QDE_SummaryReport_Initial.aspx.cs"%>

<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="System.Web.UI" TagPrefix="cc1" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbar" Namespace="Infragistics.WebUI.UltraWebToolbar" Assembly="Infragistics2.WebUI.UltraWebToolbar.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">Summary Report</asp:Content>
<asp:Content ID="Javascript" ContentPlaceHolderID="HOJS" runat="server">
	<script type="text/javascript">
		function uwToolbar_Click(oToolbar, oButton, oEvent)
		{
			if (oButton.Key == 'Clone') 
			{
				oEvent.cancelPostBack = MandatoryFieldMissing();
			}
			else if (oButton.Key == 'Cancel') 
			{
			  oEvent.cancelPostBack = true
				window.close();
			}
		} 

    function setNodesChecked(nNodesCollection, bChecked)
    {
        if (nNodesCollection)
        {
            var iCount = nNodesCollection.length ;
            var bApplytoChild = true;
            for (var iTemp = 0 ; iTemp < iCount ; iTemp++)
            {
              var oNode = nNodesCollection[iTemp];
              if(oNode.getChecked() == bChecked)
              {
               bApplytoChild = false;
               iTemp = iCount // getting out of the Loop
              }
           }   
           if(bApplytoChild)
           {
            for (var iTemp = 0 ; iTemp < iCount ; iTemp++)
            {
                    var oNode = nNodesCollection[iTemp];

                    oNode.setChecked(bChecked);

                    setNodesChecked(oNode.getChildNodes(),bChecked);
               }
               }
        }
    }

    function webTree_NodeChecked(treeId, nodeId, bChecked)
    {
        var oNode = igtree_getNodeById(nodeId);

        if (oNode)
        {
            setNodesChecked(oNode.getChildNodes(),bChecked);
        }
    }

    var cbFilterID;
    function cbCheckedChanged()
    {
      if (cbFilterID)
      {
        var isSelected = document.getElementById(cbFilterID).checked;
        var url = document.location.toString();
        var index = url.indexOf('&r=');
        if (index >= 0)
        {
          url = url.substring(0, index);
        }
        document.location = url + '&r='+isSelected;
      }
    }
	</script> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOCP" Runat="Server">

			<table class="main" cellspacing="0" cellpadding="0">
				<tr height="1" valign="top">
					<td>
						<igtbar:ultrawebtoolbar id="uwToolbar" runat="server" CssClass="hc_toolbar" ItemWidthDefault="80px" OnButtonClicked="uwToolbar_ButtonClicked">
								<HoverStyle CssClass="hc_toolbarhover"></HoverStyle>
								<DefaultStyle CssClass="hc_toolbardefault"></DefaultStyle>
								<SelectedStyle CssClass="hc_toolbarselected"></SelectedStyle>
								<ClientSideEvents InitializeToolbar="uwToolbar_InitializeToolbar" Click="uwToolbar_Click"></ClientSideEvents>
								<Items>
									<igtbar:TBarButton Text="Submit" Key="Submit" Image="/hc_v4/img/ed_download.gif" ToolTip="Generate Summary Report"></igtbar:TBarButton>
									<igtbar:TBSeparator Key="SubmitSep"></igtbar:TBSeparator>
									<igtbar:TBarButton Text="Close" Key="Cancel" Image="/hc_v4/img/ed_cancel.gif" ToolTip="Close window"></igtbar:TBarButton>
									<igtbar:TBSeparator Key="CloseSep"></igtbar:TBSeparator>
									<igtbar:TBLabel Key="lbSoftRoll" Text="Soft roll" ToolTip="Display soft roll">
                    <DefaultStyle Width="60px">
                    </DefaultStyle>
                  </igtbar:TBLabel>
									<igtbar:TBCustom ID="TBCustom1" Key="SoftRoll" Width="30px" runat="server">
					          <asp:CheckBox ID="cbFilter" runat="server" onclick="cbCheckedChanged();" />
									</igtbar:TBCustom>
								</Items>
							</igtbar:ultrawebtoolbar>
					</td>
				</tr>
				<tr valign="top" style="height:1px">
					<td>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
					</td>
				</tr>
				<tr valign="top" height="100%">
					<td>
      <ignav:ultrawebtree id="webTree" runat="server" ExpandImage="/hc_v4/ig/ig_treeXPPlus.gif" CollapseImage="/hc_v4/ig/ig_treeXPMinus.gif"
        Indentation="10" ImageDirectory="/hc_v4/ig/" WebTreeTarget="HierarchicalTree" InitialExpandDepth="1"
        CheckBoxes=true LoadOnDemand="Manual" DefaultSelectedImage="/hc_v4/ig/s_l.gif" DefaultImage="/hc_v4/ig/s_l.gif" >
        <SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
        <NodeStyle CssClass="hc_node"></NodeStyle>
        <HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
        <NodePaddings Top="0px"></NodePaddings>
        <Levels>
          <ignav:Level Index="0"></ignav:Level>
          <ignav:Level Index="1"></ignav:Level>
        </Levels>
        <AutoPostBackFlags NodeExpanded="False" NodeChecked="False"></AutoPostBackFlags>
        <ClientSideEvents NodeChecked="webTree_NodeChecked" />
      </ignav:ultrawebtree>

             </td>
				</tr>
			</table>
</asp:Content>