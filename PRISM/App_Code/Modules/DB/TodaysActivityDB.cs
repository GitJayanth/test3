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
	/// This class allows to access to data for module 'TodaysActivity'
	/// </summary>
  public class TodaysActivityDB : IDisposable
	{
		#region Private vars
		private Database _Db=null;
		private string _LastError=string.Empty;
		#endregion

    protected bool disposed = false;
    #region Constructor
		public TodaysActivityDB()
		{
  		_Db=Utils.GetMainDB();
		}
		#endregion

		#region Public methods
		public DataTable GetTodaysActivity()
		{
      return GetTodaysActivityFromSession();
		}

		private DataTable GetTodaysActivityFromSession()
		{
      if (SessionState.TodaysActivity == null)
      {
        if (_Db != null)
        {
          using (DataSet ds = _Db.RunSQLReturnDataSet("Work_GetTodaysActivity"))
          {
            _Db.CloseConnection();
            SessionState.TodaysActivity = ds.Tables[0];
            return SessionState.TodaysActivity;
          }
        }
      }
      return SessionState.TodaysActivity;
		}
    
		#endregion
    ~TodaysActivityDB() 
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
