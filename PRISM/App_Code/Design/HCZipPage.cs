#region uses
using System;
using HyperCatalog.Shared;
using HyperComponents.Web.SmartTrace;
#endregion

  /// <summary>
  /// Description résumée de HCZipPage.
  /// </summary>
public class HCZipPage:HyperPageZipDB
{
  private static string _connectionString;
  public override string ConnectionString
  {
    get
    {
      if (_connectionString==null)
        _connectionString = SessionState.CacheComponents["Crystal_DB"].ConnectionString;
      return _connectionString;
    }
  }


  protected override void SavePageStateToPersistenceMedium(object viewState)
  {
    if (viewState!=null)
    {
      if (SessionState.User!=null)
        base.UserId = SessionState.User.Id.ToString();
      else
        base.UserId = Session.SessionID;
      base.SavePageStateToPersistenceMedium(viewState);
    }
  }

  protected override object LoadPageStateFromPersistenceMedium()
  {
    try
    {
      if (SessionState.User!=null)
        base.UserId = SessionState.User.Id.ToString();
      else
        base.UserId = Session.SessionID;
      return base.LoadPageStateFromPersistenceMedium();
    }
    catch
    {
      return null;
    }
  }
}
