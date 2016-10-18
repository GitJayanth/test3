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
	/// This class allows to access to data for module 'ComingTranslations'
	/// </summary>
  public class ComingTranslationsDB : IDisposable
	{
		#region Private vars
		private Database _Db=null;
		private string _LastError=string.Empty;
		#endregion

    protected bool disposed = false;
    #region Constructor
		public ComingTranslationsDB()
		{
  		_Db=Utils.GetMainDB();
		}
		#endregion

		#region Public methods
		public DataSet GetComingTranslations()
		{
      return GetComingTranslationsFromSession();
		}

		private DataSet GetComingTranslationsFromSession()
		{
      if (SessionState.ComingTRs == null)
      {
        if (_Db != null)
        {
          using (DataSet ds = _Db.RunSQLReturnDataSet("TR_ComingTRs " + SessionState.User.Id.ToString()))
          {
            _Db.CloseConnection();
            SessionState.ComingTRs = ds;
            return SessionState.ComingTRs;
          }
        }
      }
      return SessionState.ComingTRs;
		}
    
		#endregion
    ~ComingTranslationsDB() 
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
