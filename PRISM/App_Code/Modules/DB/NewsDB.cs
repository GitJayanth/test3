#region uses
using System;
using HyperComponents.Data.dbAccess;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using HyperCatalog.Shared;
#endregion

namespace hc_homePageModules.ModuleDB
{
	/// <summary>
	/// This class allows to access to data for module 'NewsDB'
	/// </summary>
  public class NewsDB : IDisposable
	{
		#region Private vars
		private Database _Db=null;
		private string _LastError=string.Empty;
		#endregion
    protected bool disposed = false; 

		#region Properties
		public string LastError
		{
			get {return _LastError;}
		}
		#endregion

		#region Constructor
		public NewsDB()
		{
			_Db = new Database(SessionState.CacheComponents["HyperHelp_DB"].ConnectionString);
		}
		#endregion

		#region Public methods
		public DataSet GetNews()
		{
			if (_Db!=null)
			{
        using (DataSet dsNews = _Db.RunSQLReturnDataSet("Exec _WP_GetNews"))
        {
          _Db.CloseConnection();
          if (_Db.LastError.Length > 0)
          {
            _LastError = _Db.LastError;
            return null;
          }
          return dsNews;
        }
			}
			return null;
		}
		#endregion
    ~NewsDB() 
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
