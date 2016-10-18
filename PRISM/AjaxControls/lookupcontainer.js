    /* A WebService retreiving the names of containers starting with a specific string. */   
    proxies.LookupContainer = {
    url: wsRootUrl + "ajaxcontrols/lookupcontainer.asmx",
    ns: "http://www.hypercatalog.fr/webservices/"
    } // proxies.LookupContainer

      proxies.LookupContainer.Lookup_StartWith = function () { return(proxies.callSoap(arguments)); }
      proxies.LookupContainer.Lookup_StartWith.fname = "Lookup_StartWith";
      proxies.LookupContainer.Lookup_StartWith.service = proxies.LookupContainer;
      proxies.LookupContainer.Lookup_StartWith.action = "http://www.hypercatalog.fr/webservices/Lookup_StartWith";
      proxies.LookupContainer.Lookup_StartWith.params = [
          "prefix"
        ];
      proxies.LookupContainer.Lookup_StartWith.rtype = [
          "Lookup_StartWithResult"
        ];
    
        /** Return Lookup entries that start with the given prefix. */
      
      proxies.LookupContainer.GetPrefixedEntries = function () { return(proxies.callSoap(arguments)); }
      proxies.LookupContainer.GetPrefixedEntries.fname = "GetPrefixedEntries";
      proxies.LookupContainer.GetPrefixedEntries.service = proxies.LookupContainer;
      proxies.LookupContainer.GetPrefixedEntries.action = "http://www.hypercatalog.fr/webservices/GetPrefixedEntries";
      proxies.LookupContainer.GetPrefixedEntries.params = [
          "prefix"
        ];
      proxies.LookupContainer.GetPrefixedEntries.rtype = [
          "GetPrefixedEntriesResult:x"
        ];
    