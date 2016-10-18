    /* A WebService retreiving the menus for an Item in a Tree View and a certain user. */    
    proxies.AppComponents = {
    url: wsRootUrl + "ajaxcontrols/AppComponents.asmx",
    ns: "http://www.hypercatalog.fr/webservices/"
    } // proxies.AppComponents

        /** Return Child nodes in XML Format */
      
      proxies.AppComponents.Check = function () { return(proxies.callSoap(arguments)); }
      proxies.AppComponents.Check.fname = "Check";
      proxies.AppComponents.Check.service = proxies.AppComponents;
      proxies.AppComponents.Check.action = "http://www.hypercatalog.fr/webservices/Check";
      proxies.AppComponents.Check.params = [
          "appComponentId:int"
        ];
      proxies.AppComponents.Check.rtype = [
          "CheckResult"
        ];
    