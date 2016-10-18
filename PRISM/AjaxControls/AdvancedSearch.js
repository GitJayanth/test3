proxies.AdvancedSearch = {
url: wsRootUrl + "ajaxcontrols/AdvancedSearch.asmx",
ns: "http://www.hypercatalog.fr/webservices/"
} // proxies.AdvancedSearch

proxies.AdvancedSearch.GetContainerGroups = function () { return(proxies.callSoap(arguments)); }
proxies.AdvancedSearch.GetContainerGroups.fname = "GetContainerGroups";
proxies.AdvancedSearch.GetContainerGroups.service = proxies.AdvancedSearch;
proxies.AdvancedSearch.GetContainerGroups.action = "http://www.hypercatalog.fr/webservices/GetContainerGroups";
proxies.AdvancedSearch.GetContainerGroups.params = ["parentContainerGroupId:int"];
proxies.AdvancedSearch.GetContainerGroups.rtype = ["GetContainerGroupsResult"];