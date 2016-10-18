    /* A WebService retreiving the names of static terms starting with a specific string. */   
    proxies.LookupStaticTerm = {
    url: wsRootUrl + "ajaxcontrols/lookupstaticterm.asmx",
    ns: "http://www.hypercatalog.fr/webservices/"
    } // proxies.LookupStaticTerm

      proxies.LookupStaticTerm.Lookup_StartWith = function () { return(proxies.callSoap(arguments)); }
      proxies.LookupStaticTerm.Lookup_StartWith.fname = "Lookup_StartWith";
      proxies.LookupStaticTerm.Lookup_StartWith.service = proxies.LookupStaticTerm;
      proxies.LookupStaticTerm.Lookup_StartWith.action = "http://www.hypercatalog.fr/webservices/Lookup_StartWith";
      proxies.LookupStaticTerm.Lookup_StartWith.params = [
          "prefix"
        ];
      proxies.LookupStaticTerm.Lookup_StartWith.rtype = [
          "Lookup_StartWithResult"
        ];
    
        /** Return Lookup entries that start with the given prefix. */
      
      proxies.LookupStaticTerm.GetPrefixedEntries = function () { return(proxies.callSoap(arguments)); }
      proxies.LookupStaticTerm.GetPrefixedEntries.fname = "GetPrefixedEntries";
      proxies.LookupStaticTerm.GetPrefixedEntries.service = proxies.LookupStaticTerm;
      proxies.LookupStaticTerm.GetPrefixedEntries.action = "http://www.hypercatalog.fr/webservices/GetPrefixedEntries";
      proxies.LookupStaticTerm.GetPrefixedEntries.params = [
          "prefix"
        ];
      proxies.LookupStaticTerm.GetPrefixedEntries.rtype = [
          "GetPrefixedEntriesResult:x"
        ];
    