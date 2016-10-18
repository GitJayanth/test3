<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PLWebTree.ascx.cs" Inherits="PLWebTree" %>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

			<script type="text/javascript">
function resetCheckedNodes()
{
  alert(igtree_getTreeById('<%# webTree.ID%>'));
}
function setNodesChecked(nNodesCollection, bChecked)
{
    if (nNodesCollection)
    {
        var iCount = nNodesCollection.length ;
        for (var iTemp = 0 ; iTemp < iCount ; iTemp++)
        {
                var oNode = nNodesCollection[iTemp];

                oNode.setChecked(bChecked);

                setNodesChecked(oNode.getChildNodes(),bChecked);
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
		  
			</script>
  <STYLE type="text/css">
/*  .org {font-size: medium;}
  .group {font-size: small;}
  .gbu {font-size: x-small;}
  .pl {font-size: xx-small;}*/
</STYLE>
						<asp:Label id="lbError" runat="server" Visible="False" CssClass="hc_error">Error message</asp:Label>
              <ignav:ultrawebtree id="webTree" runat="server" ExpandImage="ig_treeXPPlus.gif" CollapseImage="ig_treeXPMinus.gif"
                Indentation="10" ImageDirectory="/hc_v4/ig/" WebTreeTarget="HierarchicalTree" InitialExpandDepth="0"
                CheckBoxes="True" OnNodeBound="webTree_NodeBound">
                <SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
                <NodeStyle CssClass="hc_node"></NodeStyle>
                <HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
                <NodePaddings Top="0px"></NodePaddings>
                  <Levels>
                      <ignav:Level Index="0"></ignav:Level>
                      <ignav:Level Index="1"></ignav:Level>
                  </Levels>
                  <NodeMargins Top="2px"></NodeMargins>
                  <Styles>
                      <ignav:Style Cursor="Hand" ForeColor="Black" BackColor="OldLace" CssClass="HiliteClass"></ignav:Style>
                      <ignav:Style BorderWidth="1px" BorderColor="DarkGray" BorderStyle="Solid" BackColor="Gainsboro"
                          CssClass="Hover">
                          <Padding Bottom="2px" Left="2px" Top="2px" Right="2px"></Padding>
                      </ignav:Style>
                      <ignav:Style ForeColor="White" BackColor="#333333" CssClass="SelectClass">
                          <Padding Bottom="2px" Left="2px" Top="2px" Right="2px"></Padding>
                      </ignav:Style>
                  </Styles>
                <ClientSideEvents NodeChecked="webTree_NodeChecked" />
              </ignav:UltraWebTree>