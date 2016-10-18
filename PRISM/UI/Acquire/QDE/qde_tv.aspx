<%@ Page Language="C#" MasterPageFile="~/HyperCatalog.Empty.master" Inherits="QDE_TV" smartNavigation="False" CodeFile="QDE_TV.aspx.cs"%>
<%@ Register TagPrefix="ignav" Namespace="Infragistics.WebUI.UltraWebNavigator" Assembly="Infragistics2.WebUI.UltraWebNavigator.v11.1, Version=11.1.20111.2238, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="PageTitle" ContentPlaceHolderID="HOPT" runat="server">QDE_TV</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HOJS" runat="server">
      <style type="text/css">
        .menu { FONT-SIZE: 10px; BACKGROUND: whitesmoke; CURSOR: pointer; COLOR: black; HEIGHT: 20px }
        .menuh { FONT-SIZE: 10px; BACKGROUND: #ff8000; CURSOR: pointer; COLOR: white; HEIGHT: 20px }</style>
      <script type="text/javascript" src="../../../ajaxcore/ajax.js"></script>
      <script type="text/javascript" src="../../../ajaxcontrols/Extensions.js"></script>
      <script type="text/javascript" language="javascript" src="qde_tv.js"></script>
      <script type="text/javascript" language="javascript">
		  var _tree;
      var _oPopup = window.createPopup();
		  /***** TREEVIEW EVENTS******/
		  var PopupAction = {
        delay: 100,
        prepare: function() {
          var i = document.getElementById("aj_id").value;
          var u = document.getElementById("aj_uid").value;
          var l = document.getElementById("aj_cc").value;
          return '<option><id>' + i + '</id><u>' + u + '</u><l>' + l+ '</l></option>';
        },
        call: proxies.UIExtensions.GetOptions,
        finish: function (xmlDoc) {
          webTree_BuildMenu(xmlDoc);
        },
        onException: proxies.alertException
      } // PopupAction

		  var menu, curEventX, curEventY, nbItems, clickedItem;
		  var curEvent, curEventButton;
		  
		  function HandleClick(e){
		    curEvent = e;
		    if (!curEvent){
		      curEvent = window.event;
  		    curEventX = curEvent.x;
          curEventY = curEvent.y;
		    }
		    else{
		      curEventX = curEvent.clientX;
          curEventY = curEvent.clientY;
		    }
		    curEventButton = curEvent.button;
		    if (curEventButton != 2)
        {
          document.getElementById("menubox").style.display = 'none';
        }
		  }
		  function webTree_NodeClick(treeId, nodeId,iBtn){
		    // IE/Firefox Compatibility
		    if (window.event) {
		      HandleClick(window.event);		      
		    }
		    window.scrollBy(-300, 0);
        menu = document.getElementById("menubox");
        menu.style.display = 'none';
		    if (iBtn==2 || curEventButton==2)
        {
		      clickedItem=igtree_getNodeById(nodeId);
		      if (clickedItem.getTag() != '')
          {
		        var itemId = StringReplace(clickedItem.getTag(), "|needsLoadOnDemand", "");
            if (itemId != '-1')
            {
  		        document.getElementById("aj_id").value = itemId;
              menu.style.position="absolute";
              ajax.Start(PopupAction);
            }
          }
        }		  
      }
      window.onblur = HidePopup;
      function HidePopup(){
        document.getElementById("menubox").style.display = 'none';
      }
      function webTree_BuildMenu(xmlDoc)
		  {				    
		    var nbItems = 0;
		    if (xmlDoc && xmlDoc.firstChild && xmlDoc.firstChild.childNodes && xmlDoc.firstChild.childNodes.length)
		    {
           nbItems = xmlDoc.firstChild.childNodes.length;		  	
        }
        var height;
		    var maxItems = 9;
		    var maxLen = 0;
        var content = '<HTML><HEAD><TITLE>Tree Popup</TITLE>' + 
                      '<LINK href="/hc_v4/css/hypercatalog.css" rel="stylesheet">'+
                      '</HEAD><BODY>' + 
                      '<DIV CLASS="ptb" style="border-right: 1px outset Black;border-top: 1px outset Black;border-left: 1px outset Black;border-bottom: 1px outset Black;background-color: Silver;text-align: left;  font-family : Verdana, Geneva, Arial, Helvetica, sans-serif;  font-size : 10px;  font-weight : normal;  border : thin outset;">';
		    for(var i=0;i<nbItems;i++)
		    {
		      var currentRecord = xmlDoc.firstChild.childNodes[i];
		      var curName = currentRecord.attributes[1].nodeValue;
		      if (curName.length > maxLen){
		        maxLen = curName.length;
		      }
          var s = "<div style=\"cursor:hand;color:black;font-size:10px;background:#C0C0C0;font-family:verdana,arial;\" " + 
                  "onmouseover=\"this.style.color='white';this.style.background='#000080';\" onmouseout=\"this.style.color='black';this.style.background='#C0C0C0';\">" + 
                  "<span onclick=\"parent.doAction('" +
                  currentRecord.attributes[2].nodeValue + "', " +
                  currentRecord.attributes[0].nodeValue + ")\">" +
                  "<img src='/hc_v4/img/" + currentRecord.attributes[3].nodeValue +
                  "'>&nbsp;" + curName+ "</span></div>";
          content = content + s;          
		    } 
		    content = content + "</DIV></BODY></HTML>";
        _oPopup.document.body.innerHTML= content; 
        window.status = nbItems;
		    if (nbItems > 0)
		    {
          var h = nbItems * 19 + 2;
          var l = maxLen * 8 + 13;
          _oPopup.show(curEventX, 5, l, h, clickedItem.Element);
		    }
		  }

		  function webTree_AfterNodeSelectionChange(treeId, nodeId){
        document.getElementById("menubox").style.display = 'none';
		    var item =igtree_getNodeById(nodeId);
        var itemId = StringReplace(item.getTag(), "|needsLoadOnDemand", "");              
        if (itemId != '-1'){
          top.window.status = item.getTargetFrame();
          GI(itemId,l);
        }
		    return false;
		  }
      function webTree_NodeExpand(treeId, nodeId){        
        var node = igtree_getNodeById(nodeId);
        //node.WebTree.Enabled = false;
        document.getElementById("menubox").style.display = 'none';			    
        node.needsLoadOnDemand = node.getTag().indexOf("needsLoadOnDemand")>1;
        if(node.needsLoadOnDemand){       
          getNodeOnDemand(node);
        }
        /*else
        {
          node.WebTree.Enabled = true;
        }*/
      }
		function webTree_InitializeTree(treeId){
			_tree=igtree_getTreeById(treeId);
			document.body.scroll="auto"; 
			_tree.defaultNodeClassName=_tree.getNodes()[0].getClass();
			window.scrollBy(-300, 0);
		}</script></asp:content>
<asp:Content ID="Main" ContentPlaceHolderID="HOCP" runat="server">
<div id="menubox" style="BORDER-RIGHT:silver 1px solid; BORDER-TOP:silver 1px solid; DISPLAY:none; Z-INDEX:1000; BORDER-LEFT:silver 1px solid; WIDTH:130px; COLOR:blue; BORDER-BOTTOM:silver 1px solid;BACKGROUND: whitesmoke;"></div>
      <script type="text/javascript" language="javascript">
		document.body.oncontextmenu = function(){HandleClick();return false;};
		document.body.onclick = "HandleClick";
		document.body.onload = function(){try{Scroll('<%#currentNodeId%>');}catch(e){}};
		//document.body. = "BORDER-RIGHT: 0px; BORDER-TOP: 0px; MARGIN: 0px 0px 0px 1px; BORDER-LEFT: 1px; BORDER-BOTTOM: 0px; BACKGROUND-COLOR: #fefcfd";
		document.body.bgColor = "#ffffff";
</script>
      <ignav:ultrawebtree id="webTree" runat="server" ExpandImage="/hc_v4/ig/ig_treeXPPlus.gif" CollapseImage="/hc_v4/ig/ig_treeXPMinus.gif"
        Indentation="10" ImageDirectory="/hc_v4/ig/" WebTreeTarget="HierarchicalTree" InitialExpandDepth="1"
        LoadOnDemand="Manual" DefaultSelectedImage="/hc_v4/ig/s_l.gif" DefaultImage="/hc_v4/ig/s_l.gif" onload="webTree_Load">
        <SelectedNodeStyle CssClass="hc_snode"></SelectedNodeStyle>
        <NodeStyle CssClass="hc_node"></NodeStyle>
        <HoverNodeStyle CssClass="hc_onode"></HoverNodeStyle>
        <NodePaddings Top="0px"></NodePaddings>
        <Levels>
          <ignav:Level Index="0"></ignav:Level>
          <ignav:Level Index="1"></ignav:Level>
        </Levels>
        <AutoPostBackFlags NodeExpanded="False" NodeChecked="False"></AutoPostBackFlags>
        <ClientSideEvents AfterNodeSelectionChange="webTree_AfterNodeSelectionChange" NodeExpand="webTree_NodeExpand"
          NodeClick="webTree_NodeClick" InitializeTree="webTree_InitializeTree"></ClientSideEvents>
      </ignav:ultrawebtree> 
</asp:Content>