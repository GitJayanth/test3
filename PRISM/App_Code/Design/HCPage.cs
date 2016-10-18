#region uses
using System;
using HyperCatalog.Shared;
#endregion

  /// <summary>
  /// Description résumée de HCPage.
  /// </summary>
public class HCPage : HyperPageDefault
{
  public bool ShowFrame
  {
    get { return false; }// ((HyperCatalog.MasterPage)this.Master).ShowFrame; }
    set { }//((HyperCatalog.MasterPage)this.Master).ShowFrame = value; }
  }

  private static string _connectionString;
  public override string ConnectionString
  {
    get
    {
      try
      {
        if (_connectionString == null)
        {
          HyperCatalog.Business.Debug.Trace("HCPage", "Trying to retrieve Crystal DB", HyperCatalog.Business.DebugSeverity.Medium);
          _connectionString = SessionState.CacheComponents["Crystal_DB"].ConnectionString;
          HyperCatalog.Business.Debug.Trace(String.Format("HCPage.ConnectionString={0}", _connectionString), HyperCatalog.Business.DebugSeverity.Medium);
        }
        return _connectionString;
      }
      catch (Exception exc)
      {
        HyperCatalog.Business.Debug.Trace("Error in HCPage [ConnectionString property]: " + exc.Message, HyperCatalog.Business.DebugSeverity.High);
        return string.Empty;
      }
    }
  }

  public static string LayoutURL
  {
    get
    {
      return "/hc_v4";
    }
  }
  public static HyperCatalog.WebServices.DocumentFactoryWS.DocumentFactory WSDocFactory
  {
    get
    {
      if (System.Web.HttpContext.Current.Cache["WSDocFactory"] == null)
      {
        try
        {
          HyperCatalog.WebServices.DocumentFactoryWS.DocumentFactory ws = HyperCatalog.WebServices.WSInterface.DocumentFactory;
          System.Web.HttpContext.Current.Cache.Add("WSDocFactory", ws, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        }
        catch
        {
          return null;
        }
      }
      return (HyperCatalog.WebServices.DocumentFactoryWS.DocumentFactory)System.Web.HttpContext.Current.Cache["WSDocFactory"];
    }
  }
  public static HyperCatalog.WebServices.PublicationManagerWS.Publication WSPubManager
  {
    get
    {
      if (System.Web.HttpContext.Current.Cache["WSPubManager"] == null)
      {
        try
        {
          HyperCatalog.WebServices.PublicationManagerWS.Publication ws = HyperCatalog.WebServices.WSInterface.Publication;
          System.Web.HttpContext.Current.Cache.Add("WSPubManager", ws, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        }
        catch
        {
          return null;
        }
      }
      return (HyperCatalog.WebServices.PublicationManagerWS.Publication)System.Web.HttpContext.Current.Cache["WSPubManager"];
    }
  }
  public static HyperCatalog.WebServices.DAMWS.DAMWebService WSDam
  {
    get
    {
      if (System.Web.HttpContext.Current.Cache["WSDam"] == null)
      {
        try
        {
          HyperCatalog.WebServices.DAMWS.DAMWebService ws = HyperCatalog.WebServices.WSInterface.DAM;
          System.Web.HttpContext.Current.Cache.Add("WSDam", ws, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        }
        catch
        {
          return null;
        }
      }
      return (HyperCatalog.WebServices.DAMWS.DAMWebService)System.Web.HttpContext.Current.Cache["WSDam"];
    }
  }
  public static HyperCatalog.WebServices.ContentProviderWS.ContentProvider WSContentProvider
  {
    get
    {
      if (System.Web.HttpContext.Current.Cache["WSContentProvider"] == null)
      {
        try
        {
          HyperCatalog.WebServices.ContentProviderWS.ContentProvider ws = HyperCatalog.WebServices.WSInterface.ContentProvider;
          System.Web.HttpContext.Current.Cache.Add("WSContentProvider", ws, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        }
        catch
        {
          return null;
        }
      }
      return (HyperCatalog.WebServices.ContentProviderWS.ContentProvider)System.Web.HttpContext.Current.Cache["WSContentProvider"];
    }
  }
  protected override void SavePageStateToPersistenceMedium(object viewState)
  {
    if (viewState != null)
    {
      try
      {
        if (SessionState.User != null)
          base.UserId = SessionState.User.Id.ToString();
        else
          base.UserId = Session.SessionID;
        base.SavePageStateToPersistenceMedium(viewState);
      }
      catch (Exception exc)
      {
        HyperCatalog.Business.Debug.Trace("Error in HCPage [SavePageStateToPersistenceMedium]: " + exc.Message, HyperCatalog.Business.DebugSeverity.High);
      }

    }
  }
  protected override object LoadPageStateFromPersistenceMedium()
  {
    try
    {
      if (SessionState.User != null)
        base.UserId = SessionState.User.Id.ToString();
      else
        base.UserId = Session.SessionID;
      return base.LoadPageStateFromPersistenceMedium();
    }
    catch (Exception exc)
    {
      HyperCatalog.Business.Debug.Trace("Error in HCPage [LoadPageStateFromPersistenceMedium]: " + exc.Message, HyperCatalog.Business.DebugSeverity.High);
      HyperCatalog.EventLogger.EventLogger.AddEventLog(SessionState.CacheComponents["CrystalUI"].Id, Guid.NewGuid(), -1, HyperCatalog.EventLogger.Severity.ERROR, "HCPage.LoadPageStateFromPersistenceMedium()", "", exc.Message);

      return null;
    }
  }
}

