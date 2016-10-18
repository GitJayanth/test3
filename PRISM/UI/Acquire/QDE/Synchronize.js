function ReloadAndClose()
{ 
  if (opener)
    opener.ReloadFrames(window);
  else
    window.close();  
}

var count;
var maxCount;
var lbMessage;
var message;

/**************************
** REFRESH ITEM LANGUAGE **
**************************/
var ItemLanguageWork = 
{
  delay: 100,
  prepare: function() {
    var i = document.getElementById("aj_id").value; // itemId
    return '<option><id>' + i + '</id></option>';
  },
  call: proxies.UIExtensions.RefreshItemLanguage,
  finish: function (xmlDoc) {
    UpdateItemLanguage(xmlDoc);
  },
  onException: proxies.alertException
} // ItemLanguageWork

function RefreshItemLanguage()
{ 
  ajax.Start(ItemLanguageWork); 
}
		  
function UpdateItemLanguage(xmlDoc)
{		
  if (xmlDoc && xmlDoc.firstChild 
      && xmlDoc.firstChild.childNodes 
      && xmlDoc.firstChild.childNodes.length 
      && xmlDoc.firstChild.childNodes.length == 1)
  {
    var currentRecord = xmlDoc.firstChild.childNodes[0];
    if (currentRecord)
    {
      var result = currentRecord.attributes[0].nodeValue;
      if (result.toLowerCase() == "true")
      {
        count ++;
        if (count == maxCount)
        {
          if (lbMessage && message)
          {
            lbMessage.innerText = message;
          }
          ReloadAndClose();
        }
      }
      else
      {
        alert(currentRecord.attributes[1].nodeValue);
      }
    }
    else
    {
      alert("Not response!!!");
    }
	}
}

/****************************
** REFRESH MARKET SEGMENTS **
****************************/
var MarketSegmentsWork = 
{
  delay: 100,
  prepare: function() {
    var i = document.getElementById("aj_id").value; // itemId
    return '<option><id>' + i + '</id></option>';
  },
  call: proxies.UIExtensions.RefreshMarketSegments,
  finish: function (xmlDoc) {
    UpdateMarketSegments(xmlDoc);
  },
  onException: proxies.alertException
} // MarketSegmentsWork

function RefreshMarketSegments()
{ 
  ajax.Start(MarketSegmentsWork); 
}
		  
function UpdateMarketSegments(xmlDoc)
{		
  if (xmlDoc && xmlDoc.firstChild 
      && xmlDoc.firstChild.childNodes 
      && xmlDoc.firstChild.childNodes.length 
      && xmlDoc.firstChild.childNodes.length == 1)
  {
    var currentRecord = xmlDoc.firstChild.childNodes[0];
    if (currentRecord)
    {
      var result = currentRecord.attributes[0].nodeValue;
      if (result.toLowerCase() == "true")
      {
        count ++;
        if (count == maxCount)
        {
          if (lbMessage && message)
          {
            lbMessage.innerText = message;
          }
          ReloadAndClose();
        }
      }
      else
      {
        alert(currentRecord.attributes[1].nodeValue);
      }
    }
    else
    {
      alert("Not response!!!");
    }
	}
}

/**********************
** REFRESH PUBLISHER **
**********************/
var PublishersWork = 
{
  delay: 100,
  prepare: function() {
    var i = document.getElementById("aj_id").value; // itemId
    return '<option><id>' + i + '</id></option>';
  },
  call: proxies.UIExtensions.RefreshPublishers,
  finish: function (xmlDoc) {
    UpdatePublishers(xmlDoc);
  },
  onException: proxies.alertException
} // PublishersWork

function RefreshPublishers()
{ 
  ajax.Start(PublishersWork); 
}
		  
function UpdatePublishers(xmlDoc)
{		
  if (xmlDoc && xmlDoc.firstChild 
      && xmlDoc.firstChild.childNodes 
      && xmlDoc.firstChild.childNodes.length 
      && xmlDoc.firstChild.childNodes.length == 1)
  {
    var currentRecord = xmlDoc.firstChild.childNodes[0];
    if (currentRecord)
    {
      var result = currentRecord.attributes[0].nodeValue;
      if (result.toLowerCase() == "true")
      {
        count ++;
        if (count == maxCount)
        {
          if (lbMessage && message)
          {
            lbMessage.innerText = message;
          }
          ReloadAndClose();
        }
      }
      else
      {
        alert(currentRecord.attributes[1].nodeValue);
      }
    }
    else
    {
      alert("Not response!!!");
    }
	}
}