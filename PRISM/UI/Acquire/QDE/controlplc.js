var isUpdating = false;		       
var preValue = null;
function Push(cell, value, updCell){
  //ClearValues
  if (cell.isEditable()){
    cell.setValue(value);
    //ClearStyle
    cell.Element.style.backgroundColor = '';
    updCell.setValue('u');
  }
}
function ClearPLCRow(row, bIndex, fIndex, oIndex, aIndex, rIndex, updCellIndex){
  var updCell = row.getCell(updCellIndex);
  Push(row.getCell(bIndex), null, updCell);
  Push(row.getCell(fIndex), null, updCell);
  Push(row.getCell(oIndex), null, updCell);
  Push(row.getCell(aIndex), null, updCell);
  Push(row.getCell(rIndex), null, updCell);
}
function ShowWait(){
  document.getElementById("divWait").style.display = "block";
  document.getElementById("divWait").style.visibility = "";
  window.status = "Please, wait...";
}
function HideWait(){
  window.status = "Ready";
  document.getElementById("divWait").style.display = "none";
  document.getElementById("divWait").style.visibility = "hidden";
}
function CC(gridId, rowNumber){ // Clear Current PLC
  if (confirm('Are you sure?')){
    ShowWait();
    setTimeout("CC2('" + gridId + "', " + rowNumber +")", 100);
  }
}
function CC2(gridId, rowNumber){ // Clear Current PLC
  var grid = igtbl_getGridById(gridId);
  var row = grid.Rows.getRow(rowNumber);
  var blindCell = row.getCellFromKey("Blind");
  var fullCell = row.getCellFromKey("PID");
  var obsoleteCell = row.getCellFromKey("POD");
  var announcementCell = row.getCellFromKey("Announcement");
  var removalCell = row.getCellFromKey("Removal");
  var updCell = row.getCellFromKey("Upd");
  ClearPLCRow(row, blindCell.Index, fullCell.Index, obsoleteCell.Index, announcementCell.Index, removalCell.Index, updCell.Index);
  HideWait();
}

function CSC(gridId, rowNumber){ // ClearSubCountries
  if (isUpdating){
    return;
  }
  if (confirm('Are you sure?')){
	  isUpdating = true;		 
    ShowWait();
    setTimeout("CSC2('" + gridId + "', " + rowNumber +")", 100);
  }
}
function CSC2(gridId, rowNumber){ // ClearSubCountries
  var grid = igtbl_getGridById(gridId);
  var row = grid.Rows.getRow(rowNumber);
  var blindCell = row.getCellFromKey("Blind");
  var fullCell = row.getCellFromKey("PID");
  var obsoleteCell = row.getCellFromKey("POD");
  var announcementCell = row.getCellFromKey("Announcement");
  var removalCell = row.getCellFromKey("Removal");
  var depthCell = row.getCellFromKey("Depth");
  var depth = depthCell.getValue();
  var updCell = row.getCellFromKey("Upd");
  maxLength = grid.Rows.length;
  var i = rowNumber + 1;
  var stopRecurse = false;
  while (i < maxLength && !stopRecurse){
    var childRow = grid.Rows.getRow(i);
    var type = childRow.getCellFromKey("RegionType").getValue();
    stopRecurse = childRow.getCellFromKey("Depth").getValue() <= depth;
    if (!stopRecurse){
      // ClearValues
      ClearPLCRow(childRow, blindCell.Index, fullCell.Index, obsoleteCell.Index, announcementCell.Index, removalCell.Index, updCell.Index);
    }
    i++;
  }
  ClearPLCRow(row, blindCell.Index, fullCell.Index, obsoleteCell.Index, announcementCell.Index, removalCell.Index, updCell.Index);
  HideWait();
  isUpdating = false;		 
}

function dg_BeforeEnterEditModeHandler(gridName, cellId){
  var cell = igtbl_getCellById(cellId);
  prevValue = cell.getValue();
}


function dg_AfterExitEditModeHandler(gridName, cellId){
 if (isUpdating){
  return;
 }
 var cell = igtbl_getCellById(cellId);
 var type = cell.Row.getCellFromKey("RegionType").getValue();
 if (type == 'R'){
   isUpdating = true;
   ShowWait();
   setTimeout("PropageChilds('" + gridName + "', '" + cellId +"')", 100);
 }
 else{
   cell.Row.getCellFromKey("Upd").setValue('u');
 }
}
function PropageChilds(gridName, cellId){	   
 var cell = igtbl_getCellById(cellId);
 if (!cell) return;
 cell.Element.style.backgroundColor = ''
 var type = cell.Row.getCellFromKey("RegionType").getValue();
 if (type == 'R'){
   var grid = igtbl_getGridById(gridName);
   var maxLength = grid.Rows.length;
   var i = cell.Row.getIndex() + 1;
   var date = cell.getValue();
   if (date == null || date == '') // Only if we are deleting !
     stopRecurse = !confirm('Are you sure you want to clean the date?');         
   else
     stopRecurse =false;
   var updCell = cell.Row.getCellFromKey("Upd");
   var updCellIndex = updCell.Index;
   var depthCell = cell.Row.getCellFromKey("Depth");
   var depth = depthCell.getValue();
   while (i < maxLength && !stopRecurse){
     var childRow = grid.Rows.getRow(i);
     var type = childRow.getCellFromKey("RegionType").getValue();
     stopRecurse = childRow.getCell(depthCell.Index).getValue() <= depth;
     if (!stopRecurse){
       if (type == 'R'){
         Push(childRow.getCell(cell.Index), null, childRow.getCell(updCellIndex));
       }
       else // country
         {
           Push(childRow.getCell(cell.Index),date, childRow.getCell(updCellIndex));
         }
       }
       i++;
     }		   
     Push(cell, null, updCell);
  }		
  HideWait();
  isUpdating = false;
}

