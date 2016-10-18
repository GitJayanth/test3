/** $Header: /Web36/WebObjects/P2Trace.js 6     15.07.04 17:06 Mh $
 * ---------------------------------------------------------------
 * Copyright by AP AG 2000-2004
 * This is a trace utility for client site tracing and timing.
 * ---------------------------------------------------------------
*/
var _trace = {
 _window: null, // window to output the trace text
 _indent: "", 

 Init: function() {
    if (this._window == null) {
      this._window = window.open("/hc_v4/dummy.htm", "_blank", "left=8,height=160,width=800,top=540,location=no,resizable=yes");
      this._window.document.write("<html><body scroll=yes>");
      this._window.document.write("<pre>");
      window.document.focus();
      
      if (window.navigator.userAgent.indexOf("MSIE ") > 0) {
        window.attachEvent("onunload", _trace._close);
      } else {
        window.addEventListener("unload", this._close, false);
      }
    }
   
  }, // Init
  
  _close: function() {
    if ((_trace._window != null) && (_trace._window.closed == false))
      _trace._window.close();
  }, // _close
  
  
  /// Write the text to the trace window.
  Write: function(txt) {
    if ((this._window != null) && (this._window.closed != true)) {
      txt = String(txt).replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
      this._window.document.write("\n" + this._indent + txt);
      this._window.scroll(0, 30000);
    } // if
  }, // Write

  // trace open and close of functions or blocks
  Open: function(txt, p) {
    if (p == null) p = "";
    this.Write("[" + txt + "] (" + p + ")");
    this._indent += "..";
  }, // Open

  Close: function(txt, ret) {
    if (ret == null) ret = "";
    this._indent = this._indent.substr(2);
    this.Write("[" + txt + "] <-" + ret);
  }, // Close

  ///<summary>display details about an object.</summary> 
  Obj : function(obj) {
    var s = "[obj]:";

    if (obj.constructor == String)
      s = "\"" + obj + "\"";
    else if (obj.constructor == Array)
      s += " _ARRAY";
    else if (typeof(obj) == "function")
      s += " [function]" + obj;

    else if ((typeof(XMLSerializer) != "undefined") && (obj.constructor == XMLDocument)) {
      s = "[XMLDocument]:\n" + (new XMLSerializer()).serializeToString(obj.firstChild);
      this.Write(s); return;

    } else if ((obj.constructor == null) && (typeof(obj) == "object") && (obj.xml != null)) {
      s = "[XML]:\n" + obj.xml;
      this.Write(s); return;
    }
    
    for (p in obj) {
      try {
        if (obj[p] == null) {
          s += "\n" + String(p) + " (...)";

        } else if (typeof(obj[p]) == "function") {
          s += "\n" + String(p) + " [function]";

        } else if (obj[p].constructor == Array) {
          s += "\n" + String(p) + " [ARRAY]: " + obj[p];
          for (n = 0; n < obj[p].length; n++)
            s += "\n  " + n + ": " + obj[p][n];

        } else {
          s += "\n" + String(p) + " [" + typeof(obj[p]) + "]: " + obj[p];
        } // if
      } catch (e) { s+= e;}
    } // for
    this.Write(s);
  } // Obj
}