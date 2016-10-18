//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\ui\acquire\qde\Stub_qde_additem_aspx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_QDE_AddItem' in file 'ui\acquire\qde\qde_additem.aspx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================


using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;


namespace HyperCatalog.UI.Acquire.QDE
 {


abstract public class QDE_AddItem :  HCPage
{
    abstract public StepEnum Step
    {
      get;
      set;
    }
    abstract public int StepNumber
    {
      get;
      set;
    }
    abstract public int NbSteps
    {
      get;
      set;
    }
    abstract public bool IsNewSku
    {
      get;
      set;
    }
    abstract public int SkuLevel
    {
      get;
      set;
    }
    abstract public int MainComponentSort
    {
      get;
      set;
    }
    abstract public int NbErrorsInMainComponentPLC
    {
      get;
      set;
    }
    abstract public int NbFatalErrorsInMainComponentPLC
    {
      get;
      set;
    }
    abstract public int NbErrorsInComponentsPLC
    {
      get;
      set;
    }
    abstract public int NbFatalErrorsInComponentsPLC
    {
      get;
      set;
    }
    public enum StepEnum : int
    {
      None = 0,
      MainInfo = 1,
      SortInfo = 2,
      BundleInfo = 3,
      PLCInfo = 4,
      CheckBundlePLC = 5,
      Project = 6,
      Summarize = 7
    }
    public enum BundleComponentsErrorEnum : int
    {
      None = 0,
      BundleItem = -1,
      MainComponent = -2,
      MissingSku = -3
    }
    public enum PLCBundleErrorEnum : int
    {
      None = 0,
      ExcludedInCountry = -1,
      FullDateAfterBundlePID = -2,
      FullDateAfterBundlePOD = -3,
      ObsoleteDateBeforeBundlePOD = -4,
      ObsoleteDateBeforeBundlePID = -5,
      NotRelevantInCountry = -6
  }

}



}
