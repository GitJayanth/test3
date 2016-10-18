#region uses
using System;
using HyperComponents.Data.dbAccess;
using System.Data;
using System.Data.SqlClient;
using HyperCatalog.Shared;
#endregion

namespace hc_homePageModules.ModuleDB
{
	/// <summary>
	/// This class allows to access to data for module 'Welcome'
	/// </summary>
  public class MenusDB : IDisposable
	{
		#region Private vars
		private Database _Db=null;
		private string _LastError=string.Empty;
		#endregion
    protected bool disposed = false; 

		#region Constructor
		public MenusDB()
		{
			_Db=Utils.GetMainDB();
		}
		#endregion

		#region Public methods
		public DataSet GetMenus()
		{
			return GetMenusFromSession();
		}
    private DataSet GetMenusFromSession()
    {
      if (SessionState.UserPopupMenus == null)
      {
        if (_Db != null)
        {
          SqlParameter[] parameters ={ new SqlParameter("@UserId", SessionState.User.Id.ToString()) };
          using (DataSet ds = _Db.RunSPReturnDataSet("_User_GetWelcomeLinks", string.Empty, parameters))
          {
            _Db.CloseConnection();
            SessionState.UserPopupMenus = ds;
            return SessionState.UserPopupMenus;
          }
        }
      }
      return SessionState.UserPopupMenus;
    }

		#endregion
    ~MenusDB() 
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
