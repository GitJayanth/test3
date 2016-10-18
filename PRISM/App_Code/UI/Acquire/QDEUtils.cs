#region uses
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperCatalog.WebServices.TranslationWS;
//using DamWebService;
#endregion

/// <summary>
/// Summary description for QDEUtils
/// </summary>
public class QDEUtils
{
	public QDEUtils()
	{
		//
		// TODO: Add constructor logic here
		//
	}

  public static WSTranslation wsTranslationObj
  {
    get
    {
      if (HttpContext.Current.Cache["wsTranslationObj"] == null)
      {
        try
        {
          HttpContext.Current.Trace.Warn("QDEUtils.wsTranslationObj is being regenerated");
          HttpContext.Current.Cache.Add("wsTranslationObj", HyperCatalog.WebServices.WSInterface.Translation, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        }
        catch
        {

          HttpContext.Current.Trace.Warn("QDEUtils.wsTranslationObj IS NULL!!!"); 
          return null;
        }
      }
      HttpContext.Current.Trace.Warn("QDEUtils.wsTranslationObj is returned from cache");
      return (WSTranslation)HttpContext.Current.Cache["wsTranslationObj"];
    }
  }
  public static HyperCatalog.Business.Item GetItemIdFromRequest()
  {
    System.Int64 itemId = GetQueryItemIdFromRequest();
    HyperCatalog.Business.Item item;
    if (SessionState.CurrentItem != null && SessionState.CurrentItem.Id == itemId)
    {
      item = SessionState.CurrentItem;
      return item;
    }
    else
    {
      SessionState.CurrentItem = item = HyperCatalog.Business.Item.GetByKey(GetQueryItemIdFromRequest());
    }
    if (item != null && item.IsRoll)
    {
      itemId = item.RefItemId;
    }
    if (SessionState.TVAllItems)
    {
      SessionState.User.LastVisitedItemReadOnly = Convert.ToInt64(itemId);
    }
    else
    {
      SessionState.User.LastVisitedItem = Convert.ToInt64(itemId);
    }
    SessionState.User.QuickSave();

    return item;
  }
    //Added the below function to generate preview for Softroll by Radha S
  public static HyperCatalog.Business.Item GetItemIdFromRequest(Int64 itemId)
  {
      HyperCatalog.Business.Item item;
      if (SessionState.CurrentItem != null && SessionState.CurrentItem.Id == itemId)
      {
          item = SessionState.CurrentItem;
          return item;
      }
      else
      {
          SessionState.CurrentItem = item = HyperCatalog.Business.Item.GetByKey(itemId);
      }
      if (SessionState.TVAllItems)
      {
          SessionState.User.LastVisitedItemReadOnly = Convert.ToInt64(itemId);
      }
      else
      {
          SessionState.User.LastVisitedItem = Convert.ToInt64(itemId);
      }
      SessionState.User.QuickSave();

      return item;
  }

  public static HyperCatalog.Business.Culture UpdateCultureCodeFromRequest()
  {
    return UpdateCultureCodeFromRequest("c");
  }
  public static HyperCatalog.Business.Culture UpdateCultureCodeFromRequest(string requestField)
  {
    if (System.Web.HttpContext.Current.Request[requestField] != null)
    {
      if (SessionState.Culture != null)
      {
        if (SessionState.Culture.Code != System.Web.HttpContext.Current.Request[requestField].ToString())
        {
          HyperCatalog.Business.Culture cul = HyperCatalog.Business.Culture.GetByKey(System.Web.HttpContext.Current.Request[requestField].ToString());
          if (cul != null)
          {
            SessionState.Culture = cul;
          }
        }
      }
      else
      {
        HyperCatalog.Business.Culture cul = HyperCatalog.Business.Culture.GetByKey(System.Web.HttpContext.Current.Request[requestField].ToString());
        if (cul != null)
        {
          SessionState.Culture = cul;
        }
      }
    }
    return SessionState.Culture;
  }

  public static string GetQueryCultureCodeFromRequest(string requestField)
  {
    string cultureCode = SessionState.Culture != null?SessionState.Culture.Code:"";
    if (System.Web.HttpContext.Current.Request[requestField] != null)
    {
      cultureCode = System.Web.HttpContext.Current.Request[requestField].ToString();
    }
    return cultureCode;
  }
  
  public static string GetQueryCultureCodeFromRequest()
  {
    return GetQueryCultureCodeFromRequest("c");
  }

  public static System.Int64 GetQueryItemIdFromRequest()
  {
    System.Int64 itemId;
    if (System.Web.HttpContext.Current.Request["i"] != null)
    {
      itemId = Convert.ToInt64(System.Web.HttpContext.Current.Request["i"]);
    }
    else
    {
      if (SessionState.TVAllItems)
      {
        itemId = SessionState.User.LastVisitedItemReadOnly>=0?SessionState.User.LastVisitedItemReadOnly:0;
      }
      else
      {
        itemId = SessionState.User.LastVisitedItem>=0?SessionState.User.LastVisitedItem:0;
      }
    }
    HttpContext.Current.Trace.Warn("QDEUtils.GetQueryItemIdFromRequest returns " + itemId);

    return itemId;
  }

}
