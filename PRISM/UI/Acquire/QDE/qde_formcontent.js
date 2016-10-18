  var winChunkEdit = null;
  var winSpellCheck = null;
  var winMoveStatusTo = null;
  var winRegionalizable = null;
  
  var cultureCode = '';
  var iId = -1;
  var inputFormId = -1;
  var dgGrid = null;
  var inputFormName = null;
  //Added by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
  var containerLimit = 10;
  var isAutoTRButton = false;

  function uwToolbar_Click(oToolbar, oButton, oEvent)
  {
    switch (oButton.Key)
    {
      case 'filter':
		    oEvent.cancelPostBack = true;		  
		    DoSearch();	
        break;
      case 'TinyTM':
       //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
        if (dg_nbItems_Checked > 0 && dg_nbItems_Checked <= containerLimit)
        {
            oEvent.cancelPostBack = true;	
       	    var checkedContainers = '';
            var myForm = document.forms[0];
            var sep  = '';var j = 0;
            for (i=0; i<myForm.length; i++)
            {
                if ((myForm.elements[i].id.indexOf('g_sd')!=-1) && myForm.elements[i].checked)
                {
                    if (j>0) sep = ';'
                    checkedContainers += sep + myForm.elements[i].parentNode.parentNode.childNodes[4].innerHTML;
                    j++;
                }
            }
            dg_nbItems_Checked = 0;	  
            var url = '../../Globalize/TM_InstantTranslate.aspx?i=' + iId  + '&con=' + checkedContainers + '&c=' + cultureCode;
            var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
            winChunkEdit = OpenModalWindow(url,'TinyTM', 360, 615, 'yes');
        }
        else if (dg_nbItems_Checked > containerLimit)
        {
            alert('Auto TR can be performed for a maximum of ' + containerLimit + ' containers only. Please uncheck few containers and try again.');
            oEvent.cancelPostBack = true;		  
         }
        else
        {
            alert('Please select at least one container to Auto Translate');
            oEvent.cancelPostBack = true;
        }
        break;
      case 'Paste':
   		  oEvent.cancelPostBack = true;		  
   	    var url = 'QDE_Paste.aspx?i='+iId+'&c='+cultureCode;
        var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
        winChunkEdit = OpenModalWindow(url,'Paste', 360, 615, 'yes');
        break;
      case 'Delete':
        if (dg_nbItems_Checked > 0)
        {
   		  //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
          if(cultureCode != 'ww-en' && isAutoTRButton)
   		    oEvent.cancelPostBack = !confirm("Are you sure you want to delete selected chunks? \n\nNote: Delete will be performed on locally authored content only.");		   
   		  else
   		    oEvent.cancelPostBack = !confirm("Are you sure you want to delete selected chunks?");		   
	    }
	    else
	    {
	      alert('Please select at least one row');
	      oEvent.cancelPostBack = true;
	    }
        break;  	      
      case 'Copy':
        if (dg_nbItems_Checked == 0)
        {
   		    alert('Please select at least one row');
   		    oEvent.cancelPostBack = true;
   		  }
   		  else if(cultureCode != 'ww-en' && isAutoTRButton) // do this operation only for regions
   		  {    		  
   		  //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
   		    alert('Copy will be performed on locally authored only.');
   		  }
        break;
			case 'SpellCheck':
   		  oEvent.cancelPostBack = true;		
   			var url = 'QDE_SpellChecker.aspx?i='+iId + '&f='+inputFormId+'&c='+cultureCode;
				var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
				winSpellCheck = OpenModalWindow(url,'SpellCheck', 500, 700, 'no');
				break;
			case 'MoveStatusTo':
   		    oEvent.cancelPostBack = true;
        if (dg_nbItems_Checked == 0)
        {
   		    alert('Please select at least one row');
   		  }
   		  else
   		  {
   		    oEvent.cancelPostBack = true;		  
   		    var checkedItems = '';
          var myForm = document.forms[0];
          var sep  = '';var j = 0;
          for (i=0; i<myForm.length; i++)
          {
            if ((myForm.elements[i].id.indexOf('g_sd')!=-1) && myForm.elements[i].checked)
            {
              if (j>0) sep = ','
              checkedItems += sep + myForm.elements[i].parentNode.parentNode.childNodes[4].innerHTML;
              j++;
            }
          }
   		    var url = 'QDE_MoveStatusTo.aspx?f='+inputFormId + '&c=' + checkedItems + '&isATRButton=' + isAutoTRButton;
				  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
				  winMoveStatusTo = OpenModalWindow(url,'MoveStatusTo', 200, 400, 'no');
				}
				break;  	      
			case 'Regionalizable':
   		  oEvent.cancelPostBack = true;
			  var url = 'QDE_Regionalize.aspx?f='+inputFormId+'&c='+cultureCode;
			  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
			  winRegionalizable = OpenModalWindow(url,'MultiRegions', 500, 700, 'no');
				break;
    }
  }
  
  function ed(index)
  {
    dgGrid = igtbl_getGridById(dgClientId);
	  var url = 'qde_chunk.aspx?g='+dgClientId+'&r='+index+'&i='+iId+'&c='+cultureCode;
	  var windowArgs = "center=yes;help=no;status=no;resizable=no;edge=sunken;unadorned=yes;";
	  url += '#target';
	  winChunkEdit = OpenModalWindow(url,'chunkwindow', 360, 615, 'yes')
  }
  
  function closeWindows()
  {
		if (winChunkEdit) {winChunkEdit.close();}
		if (winSpellCheck) {winSpellCheck.close();}
		if (winMoveStatusTo) {winMoveStatusTo.close();}
		if (winRegionalizable) {winRegionalizable.close();}
  }
