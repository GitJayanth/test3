#region uses
using System;
using HyperComponents.Data.dbAccess;
using System.Data;
using System.Data.SqlClient;
#endregion

namespace hc_homePageModules.ModuleDB
{
	/// <summary>
	/// This class allows to access to data for module 'Counters'
	/// </summary>
  public class CountersDB : IDisposable
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
		public CountersDB()
		{
			_Db=Utils.GetMainDB();
		}
		#endregion

		#region Public methods
		public DataSet GetCounters() 
		{
			if (_Db!=null)
			{
				SqlParameter[] parameters={};
        if (System.Web.HttpContext.Current.Cache["appCounters"]==null){
        System.Web.HttpContext.Current.Cache.Add("appCounters", _Db.RunSQLReturnDataSet("WP_GetCounters"), null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
            System.Web.Caching.CacheItemPriority.Normal, null);
        }
        using (DataSet ds = (DataSet)System.Web.HttpContext.Current.Cache["appCounters"])
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
    ~CountersDB() 
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
