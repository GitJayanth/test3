window.focus();
var chunkHasUnsavedChanges = false;
var toolBar = null;
var gridObject = null;
var gridDiv = null;
var rowIndex = null;
var saveRowIndex = -1;
var row = null;
var chunkFrame = null;
var containerId = null;
var itemId = null;
var inputFormContainerId = null;
var cultureCode = null;
var nbGridLines = null;
var mustReload = false;
var mandatory = false;
var countryCode = '';
var cultureName = '';
var culId = 0;
  
///////////////////////////////////////////////////////
  function InitVars(){
///////////////////////////////////////////////////////
    chunkFrame = document.getElementById("ChunkFrame");        
    gridObject = opener.dgGrid;
    gridDiv  = opener.divDg;
    
    toolBar.Items[0].Element.innerText = opener.inputFormName?opener.inputFormName:'';
    
    nbGridLines = opener.nbCellsWithIndex;
  	if (movenext)
	    MoveNext();

    NavigateRow();
  }
///////////////////////////////////////////////////////
function NavigateRow(){
///////////////////////////////////////////////////////
  row = gridObject.Rows.getRow(rowIndex);
  containerId = row.getCellFromKey("ContainerId").getValue();
	//cultureCode = row.getCellFromKey("CultureCode").getValue();
	cultureCode = culCode;
	//itemId = row.getCellFromKey("ItemId").getValue();
	itemId = iId;
	inputFormContainerId = row.getCellFromKey("InputFormContainerId").getValue();
	if (culId == 2 && containerId > 3) // Country
    mandatory = true;
	else
	  mandatory = row.getCellFromKey("IsMandatory").getValue();
	  
	document.getElementById("curpos").value = rowIndex;
  var url = '../chunk/chunk.aspx?d='+containerId+'&i='+itemId+'&l='+cultureCode+'&ifcid='+inputFormContainerId+'&m='+mandatory;
  var curLine = 1 + parseInt(row.getCellFromKey("Index").getValue());
  toolBar.Items[1].Element.innerText = 'Chunk ' + curLine + '/' + nbGridLines;
  chunkFrame.src = url;  
  chunkHasUnsavedChanges = false;
  //row.Element.scrollIntoView();
  //row.activate();
}
function MovePrev(index){
  if (!chunkHasUnsavedChanges || confirm('You have un-commited changes, proceed anyway?')){
  	var index = parseInt(gridObject.Rows.getRow(rowIndex).getCellFromKey("Index").getValue());
    if (index - 1 >= 0)
    {
      rowIndex--;
			while (gridObject.Rows.getRow(rowIndex).getCellFromKey("Index").getValue() == null)
			{
				// if previous row is a group label
				rowIndex--;
			}
      NavigateRow();
    }
  }
}
function MoveNext(index){
  if (!chunkHasUnsavedChanges || confirm('You have un-commited changes, proceed anyway?')){
	  var index = parseInt(gridObject.Rows.getRow(rowIndex).getCellFromKey("Index").getValue());
    if (index + 1 <= nbGridLines - 1)
    {
      rowIndex++;
			while (gridObject.Rows.getRow(rowIndex).getCellFromKey("Index").getValue() == null)
			{
				// if previous row is a group label
				rowIndex++;
			}
      NavigateRow();
    }
  }
}
///////////////////////////////////////////////////////
function uwToolbar_Click(oToolbar, oButton, oEvent){
///////////////////////////////////////////////////////
	if (!opener){
		window.close();
		return;
	}	
	if (oButton.Key == 'Prev') 
	{
    MovePrev();
  }
  else if (oButton.Key == 'Next') 
	{
	  MoveNext();
  }
  else if (oButton.Key == 'Close') 
  {
    window.close();
  }
  oEvent.cancelPostBack = true;
}
///////////////////////////////////////////////////////
function uwToolbar_InitializeToolbar(oToolbar, oEvent){
///////////////////////////////////////////////////////
	toolBar = oToolbar;
	InitVars();
}
///////////////////////////////////////////////////////
function UpdateGrid(status, value){
///////////////////////////////////////////////////////
  var saverow = gridObject.Rows.getRow(saveRowIndex);
  if (saverow){
  if (saverow.getCellFromKey("Status")) 
    saverow.getCellFromKey("Status").Element.className = "ptb1 S" + status;
  if (saverow.getCellFromKey("Value") && saverow.getCellFromKey("Value").Element) 
  {
    saverow.getCellFromKey("Value").Element.innerHTML = ConvertBR(Trim(value));
    saverow.getCellFromKey("Value").Element.style.color = "";
    saverow.getCellFromKey("Value").Element.style.fontStyle = "";
    var curStyle = saverow.getCellFromKey("Value").Element.className;

    //Modified the If condition to check the css class name
    if (curStyle.indexOf("overw") != -1)
    {
      saverow.getCellFromKey("Value").Element.className = "ptb3";
      saverow.getCellFromKey("Overrides").setValue(true);
      //saverow.getCellFromKey("HasFallback").setValue(true);  // QC :2758 UAT: Java error when moving ILB to final for URL container Balakumar
      saverow.getCellFromKey("hasFallback").setValue(true); // QC :2758 UAT: Java error when moving ILB to final for URL container Balakumar
    }
  }
  if (saverow.getCellFromKey("Country") && saverow.getCellFromKey("Country").Element)
  {
    saverow.getCellFromKey("Country").Element.innerHTML = "<img alt='" + cultureName + "' src='/hc_v4/img/flags/" + countryCode + ".gif'>";
  }
  if (saverow.getCellFromKey("Select") != null)
  {
	  for(i = 0; i < saverow.getCellFromKey("Select").Element.all.length; i++)
	  {
	    saverow.getCellFromKey("Select").Element.all(i).disabled = value=='';
		}
  }
  if (saverow.getCellFromKey("Overrides") != null)
  {
		if (saverow.getCellFromKey("Overrides").getValue()==true && value==''){
			mustReload = true;
		}
	}
	}
}
function ReloadParent()
{
	try{
   opener.document.getElementById("action").value = "reload";
   opener.document.forms[0].submit();
   window.close();
  }
  catch(e)
  {
  }
}
///////////////////////////////////////////////////////
function testOpenerReload(){
///////////////////////////////////////////////////////	
	try{
	  if (mustReload){
		  opener.document.getElementById("action").value = "reload";
      opener.document.forms[0].submit();
    }
  }
  catch(e)
  {
  }
}

///////////////////////////////////////////////////////
function SaveCurrentRow(){
///////////////////////////////////////////////////////
  saveRowIndex = rowIndex;
}
///////////////////////////////////////////////////////
function ChunkIsChanged(){
///////////////////////////////////////////////////////
  chunkHasUnsavedChanges = true;
}
