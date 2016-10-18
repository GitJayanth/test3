//////////////////////////////////////////////////////////////////////////////////////////////
//---------TREE VIEW ACTIONS
//////////////////////////////////////////////////////////////////////////////////////////////
function doAction(action, id){
//////////////////////////////////////////////////////////////////////////////////////////////
  document.getElementById("menubox").style.display = 'none';
  switch (action){
    case ('addnew'):
      AP(id);
      break;
    case ('delete'):
      DP(id);
      break;
    case ('export'):
      EX(id);
      break;
    case ('roadmap'):
      RM(id);
      break;
    case ('LinkExport'):
      EP(id);
      break;
  //Added for Links Export for CAS by Radha S
    case ('ExportLinksCAS'):
      EL_CAS(id);
      break;
    case ('inputforms'):
      IF(id);
      break;
    case ('viewall'):
      AC(id);
      break;
    case ('preview'):
      PT(id);
      break;
  //Added to Generate preview for softroll by Radha S
  case ('preview for softroll'):
      PT_Softroll(id);
      break;
    case ('compare'):
      SC(id);
      break;
    case ('comparewith'):
      DC(id);
      break;
    case ('templates'):
      TP(id);
      break;
    case ('createroll'):
      CR(id);
      break; 
    case ('deleteroll'):
      DR(id);
      break;
    case ('cloneitem'):
      CI(id);
      break; 
    case ('moveitem'):
      MI(id);
      break; 
    case ('spellcheck'):
      CH(id);
      break; 
    case ('movestatusto'):
      MS(id);
      break; 
    case ('SummaryReport'):
      SR(id);
      break; 
  }
}

// Spell checker (CHARENSOL Mickael 05/05/06)
//////////////////////////////////////////////////////////////////////////////////////////////
function CH(id){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id >= 0){
    var url = 'QDE_SpellChecker.aspx?i='+id+'&c='+l;     
  	var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;"; 	
    OpenModalWindow(url, "ch", 500, 700, "no");
  }
}

// Move status to (CHARENSOL Mickael 05/05/06)
//////////////////////////////////////////////////////////////////////////////////////////////
function MS(id){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id >= 0){
    var url = 'QDE_MoveStatusTo.aspx?i='+id+'&l='+l;   
    OpenModalWindow(url, "ms", 300, 400, "no");
  }
}

// Management for move (REY Pervenche 22/06/06)
//////////////////////////////////////////////////////////////////////////////////////////////
function MI(id){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'QDE_MoveItem.aspx?i='+id+'&c='+l;   
    OpenModalWindow(url, "mi", 400, 600, "no", 0);
  }
}

// Management for clone (CHARENSOL Mickael 24/04/06)
//////////////////////////////////////////////////////////////////////////////////////////////
function CI(id){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'QDE_CloneItem.aspx?i='+id+'&c='+l;   
    OpenModalWindow(url, "ci", 400, 600, "no", 0);
  }
}

// Management for roll (CHARENSOL Mickael 15/03/06)
//////////////////////////////////////////////////////////////////////////////////////////////
function CR(id){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'QDE_Rolls.aspx?i='+id+'&a=create&c='+l; 
    OpenModalWindow(url, "cr", 200, 600, "no");
  }
}

//////////////////////////////////////////////////////////////////////////////////////////////
function DR(id){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var isOk=confirm("Are you sure you want to delete this soft roll ?");
    if (isOk)
    {
      if (parent) 
        parent.framecontent.location='QDE_FormRoll.aspx?i='+id+'&a=delete&c='+l;
    }
  }
}
// End of mangement for roll

//////////////////////////////////////////////////////////////////////////////////////////////
function AP(id){// Add a new Product Child
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'QDE_AddItem.aspx?i='+id+'&c='+l;      	
    OpenModalWindow(url, "ap", 450, 800, "no");
  }
}
function ReloadFrames(w){
  w.close();
  parent.framecontent.location = '/hc_v4/dummy.htm';
  parent.framecontent.location='QDE_Formroll.aspx';
  window.location = '/hc_v4/dummy.htm';
  window.location = 'QDE_TV.aspx';
}
//////////////////////////////////////////////////////////////////////////////////////////////
function DP(id){// Delete a Product
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'QDE_DelItem.aspx?i='+id+'&c='+l;      	
    OpenModalWindow(url, "dp", 200, 600, "no");
  }     	  
}
//////////////////////////////////////////////////////////////////////////////////////////////
function EX(id){// Export a product for offline acquisition
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'QDE_Export.aspx?i='+id+'&c='+l;
	  OpenModalWindow(url, "popup", 350, 810, "yes");
  }
}      
//////////////////////////////////////////////////////////////////////////////////////////////
function RM(id){// Roadmap
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = '../Item/Item_Roadmap.aspx?i='+id+'&c='+l;
    OpenModalWindow(url, "popup", 500, 800, "yes", 1);
  }
}
//////////////////////////////////////////////////////////////////////////////////////////////
function EP(id) //Link - ExportReport
//////////////////////////////////////////////////////////////////////////////////////////////
{
    var url = '../../Admin/Reports/Report_NodeLevelCompat.aspx?i='+id+'&c='+l+'&export=linkExport';
    OpenModalWindow(url, "popup", 500, 800, "yes", 1);
}
/////////////////////////////////////////////////////////////////////////////////////////////
//Funtion to CAS export for Links added by Radha
function EL_CAS(id)//ExportReports for CAS Links
{
    var url = '../../Admin/Reports/Report_NodeLevelCompat.aspx?i='+id+'&c='+l+'&export=CASExport';
    OpenModalWindow(url, "popup", 500, 800, "yes", 1);
}
/////////////////////////////////////////////////////////////////////////////////////////////
function AC(id){// All chunks for a product
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = '../Item/Item_allChunks.aspx?i='+id+'&c='+l;
	  OpenModalWindow(url, "popup", 700, 700, "yes", 1);
  }
}      
//////////////////////////////////////////////////////////////////////////////////////////////
function IF(id){// Input forms for a product
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = '../InputFormsAttach.aspx?i='+id+'&c='+l;
	  OpenModalWindow(url, "popup", 350, 810, "yes");
  }
}      
//////////////////////////////////////////////////////////////////////////////////////////////
function TP(id){// Templates for a product
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = '../../Publish/TemplateAttach.aspx?i='+id+'&c='+l;
	  OpenModalWindow(url, "popup", 350, 803, "yes");
  }
}      
//////////////////////////////////////////////////////////////////////////////////////////////
function SC(id){// Static comparison
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = '../Item/Item_Comparison.aspx?m=s&i='+id+'&c='+l;
  	OpenModalWindow(url, "popup", "500", "800", "yes", 1);
  }
}      
//////////////////////////////////////////////////////////////////////////////////////////////
function DC(id){// Dynamic comparison
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = '../Item/Item_CompareWith.aspx?i='+id+'&c='+l;
  	OpenModalWindow(url, "popup", "250", "600", "yes", 1);
  }
}      
//////////////////////////////////////////////////////////////////////////////////////////////
function SR(id){// Summary Report Ramachandran
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
   var url = 'QDE_SummaryReport_Initial.aspx?i='+id+'&c='+l;
  OpenModalWindow(url, "popup", 800, 800, "yes", 1);
  }
}   
//////////////////////////////////////////////////////////////////////////////////////////////
function PT(id){// preview
//////////////////////////////////////////////////////////////////////////////////////////////
  if (id>=0){
    var url = 'preview/Preview.aspx?i='+id+'&c='+l+'&preview=original';
    //open(url, 'PreviewTemplateMenu','toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=0,width=600,height=100')
  	OpenModalWindow(url, "popupPreview", 250, 650, "no");
  }
}

//////////////////////////////////////////////////////////////////////////////////////////////
// preview for softroll - Added by Radha S
//////////////////////////////////////////////////////////////////////////////////////////////
function PT_Softroll(id) {
    if (id >= 0) {
        var url = 'preview/Preview.aspx?i=' + id + '&c=' + l + '&preview=softroll';
        //open(url, 'PreviewTemplateMenu','toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=0,resizable=0,width=600,height=100')
        OpenModalWindow(url, "popupPreview", 250, 650, "no");
    }
}     
		

//////////////////////////////////////////////////////////////////////////////////////////////
function Scroll(nodeId){ 
//////////////////////////////////////////////////////////////////////////////////////////////
  try{
    if (document.getElementById(nodeId)){ 
      document.getElementById(nodeId).scrollIntoView(); 
    }
  }
  catch (e){}
}
//////////////////////////////////////////////////////////////////////////////////////////////
function GI(itemId,cultureCode){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (parent) parent.framecontent.location='QDE_FormRoll.aspx?i=' +itemId + '&c=' +cultureCode;
}
//////////////////////////////////////////////////////////////////////////////////////////////
// TREEVIEW load on demand
//////////////////////////////////////////////////////////////////////////////////////////////
//This function will be called from with in the ajax response.  We need to take the response and load it.
//////////////////////////////////////////////////////////////////////////////////////////////
function LoadDemandNodes(xmlNodes , node){
//////////////////////////////////////////////////////////////////////////////////////////////
	if (node)
	{ //Loop through the response nodes, and load them into the tree and/or the grid.
	  var defaultClass = node.WebTree.defaultNodeClassName;
	  defaultClass = "hc_node";
	  // Remove the image since the load on demand recreate its automatically
	  if (xmlNodes && xmlNodes.childNodes && xmlNodes.childNodes.length)
	  {
	    var childList = xmlNodes;
	    if (childList.childNodes.length > 30) // Too many items to load. prefered way is reload
	    {
	      itemId = document.getElementById("aj_id").value;
	      try{GI(itemId,l);}catch (e){}
	      window.location = "qde_tv.aspx?i=" + itemId;
	      return;
	    }
	    for(var i=0;i<childList.childNodes.length;i++){
		    var currentRecord = childList.childNodes[i];		   		
		    //Looks like we need to load these nodes, so let's do it.
		    //First we'll create a new Tree Node based on the data in our XML node.
		    var itemId = new String(currentRecord.selectSingleNode("Id").text); // ItemId
		    var name = new String(currentRecord.selectSingleNode("N").text);// ItemName: Accessories
		    var levelId = new String(currentRecord.selectSingleNode("L").text); // LevelId: O or L
		    var status = new String(currentRecord.selectSingleNode("S").text); // Status: O or L
		    var deleted = new String(currentRecord.selectSingleNode("D").text); // PMDeleted: 0 or 1
		    var hasRoll = new String(currentRecord.selectSingleNode("R").text);// HasRoll: 0 or 1
		    var isCountrySpecific = new String(currentRecord.selectSingleNode("CS").text);// IsCountrySpecific: 0 or 1
		    var isSku = new String(currentRecord.selectSingleNode("PN").text);// IsSku: 0 or 1
		    var hasChildren = new String(currentRecord.selectSingleNode("C").text);// ChildCount
		    var isProject = new String(currentRecord.selectSingleNode("P").text);// IsProject: 0 or 1
		    var type = new String(currentRecord.selectSingleNode("T").text);// ItemTypeId: 0, 1, 2, 3, 4

		    var statusText = "";
        var rollImg = "";
        var deletedFontBeg = "";
        var deletedFontEnd = "";
        var isCountrySpec = "";
    		
		    // create new child
		    var newChild = node.addChild(name, defaultClass);
		    newChild.element.childNodes[2].setAttribute("src", "/hc_v4/ig/s_" + status +  ".gif");
    		
        if (newChild != null)
        {
          newChild.setTargetFrame(name);
			    // node is project
			    if (isProject.toLowerCase() == 'true' || isProject.toLowerCase() == '1')
			    {
				    newChild.setClass('hc_pnode');
				    newChild.setHoverClass('hc_opnode');
				    newChild.setHiliteClass('hc_spnode');
			    }
    		
    	    // has roll
    	    if (hasRoll.toLocaleLowerCase() == 'true' || hasRoll.toLocaleLowerCase() == '1')
    	    {
    		    rollImg = "<img alt='' src='/hc_v4/img/ed_roll.gif'/> ";
    	    }
        	
    	    // is country specific
    	    if (isCountrySpecific.toLocaleLowerCase() == 'true' || isCountrySpecific.toLocaleLowerCase() == '1')
    	    {
    		    isCountrySpec = "<img alt='' src='/hc_v4/img/M.gif'/> ";
    	    }
        	
    	    // deleted item
			    if (deleted.toLowerCase() == 'true' || deleted.toLowerCase() == '1')
			    {
				    deletedFontBeg = "<span style='text-decoration:line-through'>";
				    deletedFontEnd = "</span>";
			    }

			    // has children
			    if(hasChildren != "0" && isSku == "0")
			    {		
				    var loadNode = newChild.addChild("<img src='/hc_v4/img/ed_wait.gif'/>Loading ...");
				    loadNode.element.childNodes[2].setAttribute("src", "/hc_v4/ig/s_blank.gif");
    				
				    newChild.needsLoadOnDemand = true;
				    newChild.element.childNodes[1].setAttribute("imgType","exp");
				    newChild.element.setAttribute("isLoad","1");
				    newChild.setTag(itemId + "|needsLoadOnDemand");
				    newChild.getElement().style.padding = "2px";
				    newChild.setExpanded(false);
			    }
			    else // not children
			    {				    

				    newChild.setTag(itemId);
				    if (isSku != "0")
				    {
					    // sku icon
					    newChild.element.childNodes[2].setAttribute("src", "/hc_v4/img/type_" + type + ".png");    		      
					    // status item
					    if (status.toLowerCase() == 'o')
					    {
						    statusText = "<font color='gray'>[O] </font>";
					    }
					    if (status.toLowerCase() == 'f')
					    {
						    statusText = "<font color='green'>[F] </font>";
					    }
					    if (status.toLowerCase() == 'e')
					    {
						    statusText = "<font color='red'>[E] </font>";
					    }
					    if(hasChildren == "1"){
                var loadNode = newChild.addChild("<img src='/hc_v4/img/ed_wait.gif'/>Loading ...");
				        loadNode.element.childNodes[2].setAttribute("src", "/hc_v4/ig/s_blank.gif");
    				
				        newChild.needsLoadOnDemand = true;
     				    newChild.element.setAttribute("isLoad","1");
				        newChild.setTag(itemId + "|needsLoadOnDemand");
				        newChild.getElement().style.padding = "2px";
				        newChild.setExpanded(false);
				      }
    		    }
    		    else{
    		      if (levelId > skulevel)
    		      {
    		        newChild.element.childNodes[2].setAttribute("src", "/hc_v4/img/option.png");
    		      }
    		    }
			    }
    			
			    // update text
			    // Change after .Net 2.0 Migration
          newChild.getElement().innerHTML = StringReplace(newChild.getElement().innerHTML, newChild.getText(),deletedFontBeg+statusText+rollImg+isCountrySpec+newChild.getText()+deletedFontEnd); 
		    }
	    }	
	  }					
	  //Clear the needsLoadOnDemand flag, we will cache these nodes from now on.
    if (node.element.getAttribute("isLoad")!="1" && !ig_csom.IsIE)
    {
      node.element.removeChild(node.element.childNodes[1]);
    }
	  node.needsLoadOnDemand=false;
	}
}

/******* </DATA> **********/
/******* <AJAX> **********/
//Initiate the XMLHTTPRequest, and hook up the callback to load the response.
var LookupAction = {
  delay: 100,
  prepare: function(node) {
    var i = document.getElementById("aj_id").value;
    var u = document.getElementById("aj_uid").value;
    var l = document.getElementById("aj_cc").value;
    var o = document.getElementById("aj_obs").value;
    var t = document.getElementById("aj_tra").value;
    var a = document.getElementById("aj_all").value;
    var s = document.getElementById("aj_shrink").value;
    var c = document.getElementById("aj_company").value;
    return '<option><id>' + i + '</id><u>' + u + '</u><l>' + l + '</l><o>' + o + '</o><t>' + t + '</t><a>' + a + '</a><s>' + s + '</s><c>' + c + '</c></option>';
  },
  call: proxies.UIExtensions.GetChilds,
  finish: function (xmlDoc, node) {
   ProcessQDEResponse(xmlDoc, node);
   },
  onException: proxies.alertException
} // LookupAction

//////////////////////////////////////////////////////////////////////////////////////////////
function getNodeOnDemand(node){
//////////////////////////////////////////////////////////////////////////////////////////////
  if (node.getTag() != ''){
    document.getElementById("aj_id").value = StringReplace(node.getTag(), "|needsLoadOnDemand", "");
    ajax.Start(LookupAction, node);
  }
}
function ProcessQDEResponse(xmlDoc, aNode){
  aNode.setTag(StringReplace(aNode.getTag(), "|needsLoadOnDemand", ""));   
  LoadDemandNodes(xmlDoc.firstChild,aNode);  
  aNode.getFirstChild().remove()
}
/******* </AJAX> **********/       
