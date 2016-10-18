//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'DateValueRenderer' in the code behind file in 'ui\admin\eventbrowser\EventBrowser.aspx.cs' is moved to this file.
//====================================================================


using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.WebSchedule;
using HyperCatalog.Business;


namespace HyperCatalog.UI.Admin.Architecture
 {


  public class DateValueRenderer : Infragistics.UltraChart.Resources.IRenderLabel
  {
    public string ToString(Hashtable Context)
    {
      string test = "test : ";
      foreach (string key in Context.Keys)
        test += key + ">" + Context[key] + " ";
      return test;
    }

  }

}