proxies.UIExtensions = {
url: wsExtensionsUrl,
ns: "http://tempuri.org/"
} // proxies.UIExtensions

/** Return Child nodes in XML Format */

proxies.UIExtensions.GetChilds2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.GetChilds2.fname = "GetChilds2";
proxies.UIExtensions.GetChilds2.service = proxies.UIExtensions;
proxies.UIExtensions.GetChilds2.action = "http://tempuri.org/GetChilds2";
proxies.UIExtensions.GetChilds2.params = ["nodeId:int","userId:int","cultureCode","viewObsoletes:bool","showTranslated:bool","allItems:bool","doShrink:bool"];
proxies.UIExtensions.GetChilds2.rtype = ["GetChilds2Result:x"];

/** Return Child nodes in XML Format */

proxies.UIExtensions.GetChilds = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.GetChilds.fname = "GetChilds";
proxies.UIExtensions.GetChilds.service = proxies.UIExtensions;
proxies.UIExtensions.GetChilds.action = "http://tempuri.org/GetChilds";
proxies.UIExtensions.GetChilds.params = ["option"];
proxies.UIExtensions.GetChilds.rtype = ["GetChildsResult:x"];

proxies.UIExtensions.GetItemTemplates = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.GetItemTemplates.fname = "GetItemTemplates";
proxies.UIExtensions.GetItemTemplates.service = proxies.UIExtensions;
proxies.UIExtensions.GetItemTemplates.action = "http://tempuri.org/GetItemTemplates";
proxies.UIExtensions.GetItemTemplates.params = ["items"];
proxies.UIExtensions.GetItemTemplates.rtype = ["GetItemTemplatesResult:x"];

//Added by Radha

proxies.UIExtensions.GetItemTemplates_Pub = function () { return (proxies.callSoap(arguments)); }
proxies.UIExtensions.GetItemTemplates_Pub.fname = "GetItemTemplates_Pub";
proxies.UIExtensions.GetItemTemplates_Pub.service = proxies.UIExtensions;
proxies.UIExtensions.GetItemTemplates_Pub.action = "http://tempuri.org/GetItemTemplates_Pub";
proxies.UIExtensions.GetItemTemplates_Pub.params = ["items"];
proxies.UIExtensions.GetItemTemplates_Pub.rtype = ["GetItemTemplates_PubResult:x"];

proxies.UIExtensions.AddWordXML = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.AddWordXML.fname = "AddWordXML";
proxies.UIExtensions.AddWordXML.service = proxies.UIExtensions;
proxies.UIExtensions.AddWordXML.action = "http://tempuri.org/AddWordXML";
proxies.UIExtensions.AddWordXML.params = ["option"];
proxies.UIExtensions.AddWordXML.rtype = ["AddWordXMLResult"];

proxies.UIExtensions.SpellCheck = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.SpellCheck.fname = "SpellCheck";
proxies.UIExtensions.SpellCheck.service = proxies.UIExtensions;
proxies.UIExtensions.SpellCheck.action = "http://tempuri.org/SpellCheck";
proxies.UIExtensions.SpellCheck.params = ["text"];
proxies.UIExtensions.SpellCheck.rtype = ["SpellCheckResult:int"];

/** Return possible options for a node XML Format (popup menu) */
proxies.UIExtensions.GetOptions2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.GetOptions2.fname = "GetOptions2";
proxies.UIExtensions.GetOptions2.service = proxies.UIExtensions;
proxies.UIExtensions.GetOptions2.action = "http://tempuri.org/GetOptions2";
proxies.UIExtensions.GetOptions2.params = ["nodeId:int","userId:int","cultureCode"];
proxies.UIExtensions.GetOptions2.rtype = ["GetOptions2Result:x"];

/** Return possible options for a node XML Format (popup menu) */
proxies.UIExtensions.GetOptions = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.GetOptions.fname = "GetOptions";
proxies.UIExtensions.GetOptions.service = proxies.UIExtensions;
proxies.UIExtensions.GetOptions.action = "http://tempuri.org/GetOptions";
proxies.UIExtensions.GetOptions.params = ["option"];
proxies.UIExtensions.GetOptions.rtype = ["GetOptionsResult:x"];

/** Update Item Working Tables */
proxies.UIExtensions.RefreshItemWorkingTables2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshItemWorkingTables2.fname = "RefreshItemWorkingTables2";
proxies.UIExtensions.RefreshItemWorkingTables2.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshItemWorkingTables2.action = "http://tempuri.org/RefreshItemWorkingTables2";
proxies.UIExtensions.RefreshItemWorkingTables2.params = ["itemId:int","cultureCode"];
proxies.UIExtensions.RefreshItemWorkingTables2.rtype = ["RefreshItemWorkingTables2Result:x"];

/** Update Item Working Tables */
proxies.UIExtensions.RefreshItemWorkingTables = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshItemWorkingTables.fname = "RefreshItemWorkingTables";
proxies.UIExtensions.RefreshItemWorkingTables.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshItemWorkingTables.action = "http://tempuri.org/RefreshItemWorkingTables";
proxies.UIExtensions.RefreshItemWorkingTables.params = ["options"];
proxies.UIExtensions.RefreshItemWorkingTables.rtype = ["RefreshItemWorkingTablesResult:x"];

/** Update Status */
proxies.UIExtensions.RefreshStatus2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshStatus2.fname = "RefreshStatus2";
proxies.UIExtensions.RefreshStatus2.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshStatus2.action = "http://tempuri.org/RefreshStatus2";
proxies.UIExtensions.RefreshStatus2.params = ["itemId:int"];
proxies.UIExtensions.RefreshStatus2.rtype = ["RefreshStatus2Result:x"];

/** Update Status */
proxies.UIExtensions.RefreshStatus = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshStatus.fname = "RefreshStatus";
proxies.UIExtensions.RefreshStatus.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshStatus.action = "http://tempuri.org/RefreshStatus";
proxies.UIExtensions.RefreshStatus.params = ["options"];
proxies.UIExtensions.RefreshStatus.rtype = ["RefreshStatusResult:x"];

/** Update item language */
proxies.UIExtensions.RefreshItemLanguage2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshItemLanguage2.fname = "RefreshItemLanguage2";
proxies.UIExtensions.RefreshItemLanguage2.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshItemLanguage2.action = "http://tempuri.org/RefreshItemLanguage2";
proxies.UIExtensions.RefreshItemLanguage2.params = ["itemId:int"];
proxies.UIExtensions.RefreshItemLanguage2.rtype = ["RefreshItemLanguage2Result:x"];

/** Update item language */
proxies.UIExtensions.RefreshItemLanguage = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshItemLanguage.fname = "RefreshItemLanguage";
proxies.UIExtensions.RefreshItemLanguage.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshItemLanguage.action = "http://tempuri.org/RefreshItemLanguage";
proxies.UIExtensions.RefreshItemLanguage.params = ["options"];
proxies.UIExtensions.RefreshItemLanguage.rtype = ["RefreshItemLanguageResult:x"];

/** Update market segments */
proxies.UIExtensions.RefreshMarketSegments2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshMarketSegments2.fname = "RefreshMarketSegments2";
proxies.UIExtensions.RefreshMarketSegments2.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshMarketSegments2.action = "http://tempuri.org/RefreshMarketSegments2";
proxies.UIExtensions.RefreshMarketSegments2.params = ["itemId:int"];
proxies.UIExtensions.RefreshMarketSegments2.rtype = ["RefreshMarketSegments2Result:x"];

/** Update market segments */
proxies.UIExtensions.RefreshMarketSegments = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshMarketSegments.fname = "RefreshMarketSegments";
proxies.UIExtensions.RefreshMarketSegments.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshMarketSegments.action = "http://tempuri.org/RefreshMarketSegments";
proxies.UIExtensions.RefreshMarketSegments.params = ["options"];
proxies.UIExtensions.RefreshMarketSegments.rtype = ["RefreshMarketSegmentsResult:x"];

/** Update publishers */
proxies.UIExtensions.RefreshPublishers2 = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshPublishers2.fname = "RefreshPublishers2";
proxies.UIExtensions.RefreshPublishers2.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshPublishers2.action = "http://tempuri.org/RefreshPublishers2";
proxies.UIExtensions.RefreshPublishers2.params = ["itemId:int"];
proxies.UIExtensions.RefreshPublishers2.rtype = ["RefreshPublishers2Result:x"];

/** Update publishers */
proxies.UIExtensions.RefreshPublishers = function () { return(proxies.callSoap(arguments)); }
proxies.UIExtensions.RefreshPublishers.fname = "RefreshPublishers";
proxies.UIExtensions.RefreshPublishers.service = proxies.UIExtensions;
proxies.UIExtensions.RefreshPublishers.action = "http://tempuri.org/RefreshPublishers";
proxies.UIExtensions.RefreshPublishers.params = ["options"];
proxies.UIExtensions.RefreshPublishers.rtype = ["RefreshPublishersResult:x"];

