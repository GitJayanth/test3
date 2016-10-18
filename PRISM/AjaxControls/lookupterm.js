    /* A WebService retreiving the names of Terms starting with a specific string. */
    proxies.LookupTerm = {
    url: wsRootUrl + "ajaxcontrols/lookupterm.asmx",
    ns: "http://www.hypercatalog.fr/webservices/"
    } // proxies.LookupTerm

      proxies.LookupTerm.Lookup_StartWith = function () { return(proxies.callSoap(arguments)); }
      proxies.LookupTerm.Lookup_StartWith.fname = "Lookup_StartWith";
      proxies.LookupTerm.Lookup_StartWith.service = proxies.LookupTerm;
      proxies.LookupTerm.Lookup_StartWith.action = "http://www.hypercatalog.fr/webservices/Lookup_StartWith";
      proxies.LookupTerm.Lookup_StartWith.params = [
          "prefix"
        ];
      proxies.LookupTerm.Lookup_StartWith.rtype = [
          "Lookup_StartWithResult"
        ];
    
        /** Return Lookup entries that start with the given prefix. */
      
      proxies.LookupTerm.GetPrefixedEntries = function () { return(proxies.callSoap(arguments)); }
      proxies.LookupTerm.GetPrefixedEntries.fname = "GetPrefixedEntries";
      proxies.LookupTerm.GetPrefixedEntries.service = proxies.LookupTerm;
      proxies.LookupTerm.GetPrefixedEntries.action = "http://www.hypercatalog.fr/webservices/GetPrefixedEntries";
      proxies.LookupTerm.GetPrefixedEntries.params = [
          "prefix"
        ];
      proxies.LookupTerm.GetPrefixedEntries.rtype = [
          "GetPrefixedEntriesResult:x"
        ];
    