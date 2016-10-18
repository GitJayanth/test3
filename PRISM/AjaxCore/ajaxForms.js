/// Ajax = Asynchronous JavaScript + XML (+ HTML)
/// Ajax framework for Internet Explorer (6.0, ...) and Firefox (1.0, ...)
/// by Matthias Hertel
/// More information on: http://ajaxaspects.blogspot.com/ and http://ajaxaspekte.blogspot.com/
/// -----
/// ajaxforms.js: Methods for handling Form data
/// 07.07.2005 creation.

// ----- global variable for ajaxForms functionality. -----

/// <summary>The root object for the ajaxForms functionality.</summary>
var ajaxForms = new Object();

// ----- ajaxForms implementation -----

///<summary>Walk all html INPUT, TEXTAREA and SELECT elements contained by the passed object
///and return the current values.</summary>
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
///<returns>The data of inside the form as XML Document.</returns>
ajaxForms.getData = function (obj) {
  var n, aList;
  var xData = ajaxForms._getXMLDOM("<data></data>");
  var xNode;

  obj = ajaxForms._getFormObj("ajaxForms.getData", obj);

  // get text & checked
  aList = obj.getElementsByTagName("INPUT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      if (aList[n].type == "text") {
        xNode = xData.documentElement.appendChild(xData.createElement(aList[n].name));
        xNode.appendChild(xData.createTextNode(aList[n].value));
      } else if (aList[n].type == "checkbox") {
        xNode = xData.documentElement.appendChild(xData.createElement(aList[n].name));
        xNode.appendChild(xData.createTextNode(aList[n].checked));
      } // if
    } // if
  } // for

  // get text
  aList = obj.getElementsByTagName("TEXTAREA");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      xNode = xData.documentElement.appendChild(xData.createElement(aList[n].name));
      xNode.appendChild(xData.createTextNode(aList[n].value));
    } // if
  } // for

  // get value of selected option
  aList = obj.getElementsByTagName("SELECT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      xNode = xData.documentElement.appendChild(xData.createElement(aList[n].name));
      xNode.appendChild(xData.createTextNode(aList[n].value));
    } // if
  } // for
  return(xData);
} // ajaxForms.getFormData


///<summary>This method transfers the values of the data into the corresponding INPUT, TEXTAREA and SELECT elements.
///All HTML elements of these types that have a name but no value will be cleared.</summary>
///<param name="data">The data for the form. This can be a XML string or an XML document.</param>
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
ajaxForms.setData = function(data, obj) {
  var n, aList;
  var xData, xElem, val;
  
  obj = ajaxForms._getFormObj("ajaxForms.setData", obj);

  if (data == null)
    throw new Error("ajaxForms.setData: Argument 'data' must be set.");
    
  if (typeof(data) == "string")
    xData = ajaxForms._getXMLDOM(data);
  else
    xData = data;

  // set text & checked
  aList = obj.getElementsByTagName("INPUT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      xElem = xData.documentElement.getElementsByTagName(aList[n].name);
      val = (((xElem.length == 0) || (xElem[0].firstChild == null)) ? "" : xElem[0].firstChild.nodeValue);
        
      if (aList[n].type == "text") {
        if (aList[n].value != val) aList[n].value = val;
      } else if (aList[n].type == "checkbox") {
        aList[n].checked = (val.toLowerCase() == "true");
      } // if
    } // if
  } // for

  // set text
  aList = obj.getElementsByTagName("TEXTAREA");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      xElem = xData.documentElement.getElementsByTagName(aList[n].name);
      val = (((xElem.length == 0) || (xElem[0].firstChild == null)) ? "" : xElem[0].firstChild.nodeValue);
      if (aList[n].value != val)
        aList[n].value = val;
    } // if
  } // for

  // set selected by value
  aList = obj.getElementsByTagName("SELECT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      xElem = xData.documentElement.getElementsByTagName(aList[n].name);
      val = (((xElem.length == 0) || (xElem[0].firstChild == null)) ? "" : xElem[0].firstChild.nodeValue);
      if (aList[n].value != val)
        aList[n].value = val;
    } // if
  } // for

  ajaxForms.clearErrors(obj);
} // ajaxForms.setData



///<summary>This method clears the values of the html elements of type INPUT, TEXTAREA and SELECT that have a name.</summary>
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
ajaxForms.clearData = function(obj) {
  var n, aList;
  
  obj = ajaxForms._getFormObj("ajaxForms.clearData", obj);

  // clear the text of fields and uncheck checkboxes.
  aList = obj.getElementsByTagName("INPUT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      if (aList[n].type == "text") {
        aList[n].value = "";
      } else if (aList[n].type == "checkbox") {
        aList[n].checked = false;
      } // if
    } // if
  } // for

  // clear the text of textareas
  aList = obj.getElementsByTagName("TEXTAREA");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      aList[n].value = "";
    } // if
  } // for

  // because there is no real clear with select objects, the select is reset to the default-value
  aList = obj.getElementsByTagName("SELECT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != ""))
      for (o = 0; o < aList[n].options.length; o++) {
        if (aList[n].options[o].defaultSelected) {
          aList[n].selectedIndex = o;
          break;
        } // if
      } // for
  } // for

  ajaxForms.clearErrors(obj);
} // ajaxForms.clearData


///<summary>This method clears the values of the html elements of type INPUT, TEXTAREA and SELECT that have a name.</summary>
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
ajaxForms.resetData = function(obj) {
  var n, aList;
  
  obj = ajaxForms._getFormObj("ajaxForms.resetData", obj);

  // reset text & checked
  aList = obj.getElementsByTagName("INPUT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      if (aList[n].type == "text") {
        aList[n].value = aList[n].defaultValue;
      } else if (aList[n].type == "checkbox") {
        aList[n].checked = aList[n].defaultSelected;
      } // if
    } // if
  } // for

  // reset text
  aList = obj.getElementsByTagName("TEXTAREA");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != "")) {
      aList[n].value = aList[n].defaultValue;
    } // if
  } // for

  // reset selected
  aList = obj.getElementsByTagName("SELECT");
  for (n = 0; n < aList.length; n++) {
    if ((aList[n].name != null) && (aList[n].name != ""))
      for (o = 0; o < aList[n].options.length; o++) {
        if (aList[n].options[o].defaultSelected) {
          aList[n].selectedIndex = o;
          break;
        } // if
      } // for
  } // for

  ajaxForms.clearErrors(obj);
} // ajaxForms.resetData


///<summary>Parse an exception for an ArgumentException and set the error annotations</summary>
///<param name="ex">An exception object.</param>
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
ajaxForms.processException = function(ex, obj) {
  var txt = ex.description;
  if ((ex.message == "soap:Server") && (txt != null) && (txt.indexOf("System.ArgumentException: ") > 0)) {
    obj = ajaxForms._getFormObj("ajaxForms.processException", obj);
  
    txt = txt.substr(txt.indexOf("System.ArgumentException: ") + 26).split('\n');
    var param = txt[1].substr(txt[1].indexOf(':')+2);
    
    var aList = document.getElementsByTagName("SPAN");
    for (var n = 0; n < aList.length; n++) {
      if ((aList[n].className == "AJAXFORMEXCEPTION") && (aList[n].getAttribute("name") == param)) {
        aList[n].style.display = "inline";
        aList[n].innerText = txt[0]; // IE
        aList[n].textContent = txt[0]; // FF
        break;
      } // if
    } // for
  } else {
    proxies.alertException(ex);
  } // if
}


///<summary>This method clears the error annotations of the html elements of type SPAN
///that have the className AJAXFORMEXCEPTION.</summary>
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
ajaxForms.clearErrors = function(obj) {
  var n;
  var aList = document.getElementsByTagName("SPAN");
  for (n = 0; n < aList.length; n++) {
    if (aList[n].className == "AJAXFORMEXCEPTION") {
      aList[n].style.display = "none";
      aList[n].innerText = ""; // IE
      aList[n].textContent = ""; // FF
    } // if
  } // for
} // clearErrors


///<summary>private method for getting the html object containing the ajax form UI.</summary>
// txt the name of the public method
///<param name="obj">A reference or an id of the html object containing the ajax form UI.</param>
ajaxForms._getFormObj = function(txt, obj) {
  if (obj == null)
    throw new Error(txt + ": Argument 'obj' must be set.");

  // maybe an id of an object was passed.
  if (obj.constructor == String)
    obj = document.getElementById(obj);
  if (obj == null)
    throw new Error(txt + ": Argument 'obj' is unknown.");
  return(obj);
} // _getFormObj


///<summary>Get a browser specific implementation of the XMLDOM object, containing a XML document.</summary>
///<param name="xmlText">the xml document as string.</param>
ajaxForms._getXMLDOM = function (xmlText) {
  var obj = null;

  if (typeof(DOMParser) != "undefined") {
    // Gecko / Mozilla / Firefox
    var parser = new DOMParser();
    obj = parser.parseFromString(xmlText, "text/xml");

  } else {    
    // IE
    try {
      obj = new ActiveXObject("MSXML2.DOMDocument");
    } catch (e) { }

    if (obj == null) {
      try {
        obj = new ActiveXObject("Microsoft.XMLDOM");
      } catch (e) { }
    } // if
  
    if (obj != null) {
      obj.async = false;
      obj.validateOnParse = false;
    } // if
    obj.loadXML(xmlText);
  } // if
  return(obj);
} // _getXMLDOM

// ----- End -----

