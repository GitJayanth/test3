#region uses
using System;
using HyperComponents.Data.dbAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using HyperCatalog.Shared;
#endregion

namespace hc_homePageModules.ModuleDB
{
	/// <summary>
	/// This class allows to access to data for module 'Notifications'
	/// </summary>
  public class NotificationsDB : IDisposable
	{
		#region Private vars
		private Database _Db=null;
		private string _LastError=string.Empty;
		#endregion

    protected bool disposed = false;
    #region Constructor
		public NotificationsDB()
		{
			if (ConfigurationManager.AppSettings["NotifManagerConnectionString"]!=null)
			{
				_Db=new Database(ConfigurationManager.AppSettings["NotifManagerConnectionString"]);
			}
		}
		#endregion

		#region Public methods
		public DataSet GetNotifications(DateTime notifDate)
		{
			if (_Db!=null)
			{
				SqlParameter[] parameters={	new SqlParameter("@UserId", SessionState.User.Id ),
											new SqlParameter("@Day", 1),
											new SqlParameter("@Date", notifDate)};
        using (DataSet ds = _Db.RunSPReturnDataSet("NM_GetNotifications", string.Empty, parameters))
        {
          _Db.CloseConnection();
          if (_Db.LastError.Length > 0)
          {
            _LastError = _Db.LastError;
            return null;
          }
          return ds;
        }
			}
			return null;
		}
		#endregion
    ~NotificationsDB() 
    {
      Dispose(false);
    }
    public void Dispose()
    {
      Dispose(true);
      // stops the finalizer from being run 
      GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          if (_Db != null)
          {
            _Db.Dispose();
          }

        }
        // release unmanaged resource(s) held by this object
        disposed = true;
      }
    } 


	}
}
