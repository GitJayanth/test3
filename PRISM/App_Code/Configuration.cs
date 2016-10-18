#region uses
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using System.IO;
#endregion

namespace HyperCatalog.UI.Login
{
	/// <summary>
	/// This class allow to get or set home page properties for current user 
	/// </summary>
	public class Configuration
	{
		#region Constants
		// stored procedure names
		private const string CST_SP_GET_WEBPARTS_BY_USERID="HP_GetWebPartsByUserId";
		private const string CST_SP_ALL_WEBPARTS="HP_GetAllWebParts";
		private const string CST_SP_INIT_WEBPARTS="HP_InitWebParts";
		private const string CST_SP_SAVE_WEBPART="HP_SaveWebPart";
		private const string CST_SP_DELETE_WEBPART="HP_DeleteWebPart";
		private const string CST_SP_SAVE_USER_WEBPART="HP_SaveUserWebPart";

		// table name
		private const string CST_TABLE_USER_WEBPARTS="UserWebParts";

		// field names
		private const string CST_FIELD_USERID="UserId";
		private const string CST_FIELD_WEBPARTID="WebPartId";
		private const string CST_FIELD_SORT="Sort";
		private const string CST_FIELD_PANEID="PaneId";
		private const string CST_FIELD_CONTROL="Control";

		// parameter names
		private const string CST_PARAM_USERID="@UserId";
		private const string CST_PARAM_WEBPARTID="@WebPartId";
		private const string CST_PARAM_CONTROL="@Control";
		private const string CST_PARAM_CAPTION="@Caption";

		// session names
		private const string CST_SESSION_USER_WEBPARTS="UserWebParts";
		private const string CST_SESSION_WEBPARTS="WebParts";
		#endregion

		#region Static methods
		/// <summary>
		/// Load module properties of current user
		/// </summary>
		/// <returns>Returns DataSet object containing all module properties of current user</returns>
		public static DataSet LoadUserModules()
		{
			if (HttpContext.Current.Session[CST_SESSION_USER_WEBPARTS]==null)
			{
        using (Database db = Utils.GetMainDB())
        {
          SqlParameter[] parameters ={ new SqlParameter(CST_PARAM_USERID, SessionState.User.Id) };
          using (DataSet ds = db.RunSPReturnDataSet(CST_SP_GET_WEBPARTS_BY_USERID, string.Empty, parameters))
          {
            if (db.LastError.Length > 0)
            {
              Configuration.SaveUserModules(null);
              return null;
            }
            Configuration.SaveUserModules(ds);
            return ds;
          }
        }
			}
			else
				return (DataSet)HttpContext.Current.Session[CST_SESSION_USER_WEBPARTS];
		}

		/// <summary>
		/// Save module properties of current user
		/// </summary>
		/// <param name="ds">DataSet object to save</param>
		public static void SaveUserModules(DataSet ds)
		{
			// save in database
			if ((ds!=null) && (ds.Tables.Count>0))
			{
        using (Database db = Utils.GetMainDB())
        {
          System.Text.StringBuilder sbQuery = new System.Text.StringBuilder(string.Empty);

          if (ds.Tables[0].Rows.Count > 0)
          {
            // Delete old modules of current user
            sbQuery.Append("DELETE FROM ");
            sbQuery.Append(CST_TABLE_USER_WEBPARTS);
            sbQuery.Append(" WHERE ");
            sbQuery.Append(CST_FIELD_USERID);
            sbQuery.Append("=");
            sbQuery.Append((Int16)ds.Tables[0].Rows[0][CST_FIELD_USERID]);
            sbQuery.Append(";");

            // Insert new modules of current user
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
              DataRow currentDR = ds.Tables[0].Rows[i]; // current row

              // save current module
              sbQuery.Append("EXEC ");
              sbQuery.Append(CST_SP_SAVE_USER_WEBPART);
              sbQuery.Append(" ");
              sbQuery.Append((Int16)currentDR[CST_FIELD_USERID]);
              sbQuery.Append(", ");
              sbQuery.Append((Int16)currentDR[CST_FIELD_WEBPARTID]);
              sbQuery.Append(", ");
              sbQuery.Append((Int16)currentDR[CST_FIELD_SORT]);
              sbQuery.Append(", ");
              sbQuery.Append((Int16)currentDR[CST_FIELD_PANEID]);
              sbQuery.Append(";");
            }
            if (sbQuery.Length > 0)
            {
              db.RunSQL(sbQuery.ToString());
              if (db.LastError.Length > 0)
              {
                string lastError = db.LastError;
              }
            }
          }
        }
			}

			// save in session
			if (HttpContext.Current.Session[CST_SESSION_USER_WEBPARTS]==null)
				HttpContext.Current.Session.Add(CST_SESSION_USER_WEBPARTS, ds);
			else
				HttpContext.Current.Session[CST_SESSION_USER_WEBPARTS]=ds;

			// free memory
			if (ds!=null)
				ds.Dispose();
		}

		/// <summary>
		/// Load module definitions from Session or database
		/// </summary>
		/// <returns>Returns DataSet object containing all module definitions</returns>
		public static DataSet LoadModuleDefs()
		{
      if (HttpContext.Current.Session[CST_SESSION_WEBPARTS] == null)
      {
        using (Database db = Utils.GetMainDB())
        {

          SqlParameter[] parameters ={ };
          DataSet ds = db.RunSPReturnDataSet(CST_SP_ALL_WEBPARTS, string.Empty, parameters);
          if (db.LastError.Length > 0)
          {
            SaveModuleDefs(null);
            return null;
          }
          SaveModuleDefs(ds);
          return ds;
        }
      }
      else
        return (DataSet)HttpContext.Current.Session[CST_SESSION_WEBPARTS];
		}

		/// <summary>
		/// Save DataSet object containing all module definitions ,in Session
		/// </summary>
		/// <param name="ds">DataSet object to save</param>
		public static void SaveModuleDefs(DataSet ds)
		{
			// save in session
			if (HttpContext.Current.Session[CST_SESSION_WEBPARTS]==null)
				HttpContext.Current.Session.Add(CST_SESSION_WEBPARTS, ds);
			else
				HttpContext.Current.Session[CST_SESSION_WEBPARTS]=ds;
			
			// free memory
			if (ds!=null)
				ds.Dispose();
		}

		/// <summary>
		/// Retrieve module definition from module identifier
		/// </summary>
		/// <param name="moduleDefId">Module identifier</param>
		/// <returns>Returns DataRow object containing module definition</returns>
		public static DataRow GetModulesDefinition(int moduleDefId)
		{ 
			// DataRow containing properties
			DataRow drResult=null;
			// Load data for module definitioon
			DataSet dsModules=Configuration.LoadModuleDefs();
			if ((dsModules!=null) && (dsModules.Tables.Count>0))
			{
				for(int i=0; i<dsModules.Tables[0].Rows.Count; i++)
				{
					if (moduleDefId==((Int16)dsModules.Tables[0].Rows[i][CST_FIELD_WEBPARTID]))
					{
						drResult=dsModules.Tables[0].Rows[i];
						break;
					}
				}
			}
			// free memory
			if (dsModules!=null)
				dsModules.Dispose();

			return drResult;
		}		
		
		/// <summary>
		/// Retrieve module properties from module identifier
		/// </summary>
		/// <param name="moduleDefId">Module identifier</param>
		/// <returns>Returns DataRow object containing module properties</returns>
		public static DataRow GetModuleProperties(int moduleDefId)
		{	
			// DataRow containing properties
			DataRow drResult=null;
			// Load data for module properties
			DataSet dsUserModules=Configuration.LoadUserModules();
			if ((dsUserModules!=null) && (dsUserModules.Tables.Count>0))
			{
				for (int i=0; i<dsUserModules.Tables[0].Rows.Count; i++)
				{
					if (moduleDefId==((Int16)dsUserModules.Tables[0].Rows[i][CST_FIELD_WEBPARTID]))
					{
						drResult=dsUserModules.Tables[0].Rows[i];
						break;
					}
				}
			}
			// free memory
			if (dsUserModules!=null)
				dsUserModules.Dispose();

			return drResult;
		}

		/// <summary>
		/// Synchronize module definitions between database and directory 'Modules'
		/// </summary>
		/// <param name="directoryName">Directory name containing all module files</param>
		public static void SynchronizeModuleDefs(string directoryName)
		{
			if (Directory.Exists(directoryName))
			{
				string[] fileNames=Directory.GetFiles(directoryName, "uc_*.ascx");

				// Search if modules are added in directory
				for (int i=0; i<fileNames.Length; i++)
				{
					string moduleName=string.Empty; // Module name

					// current file name
					string currentFileName=fileNames[i];
					StreamReader sr=new StreamReader(currentFileName);

					bool isFound=false;
					string line=string.Empty;
					while (((line=sr.ReadLine())!= null) && (!isFound)) 
					{
						// Search line containing '<MM:mastermodule runat="server" ModuleName="Module_Name" />'
						if (line.IndexOf("<MM:mastermodule")>=0)
						{
							isFound=true;
							// get value for attribute 'name'
							moduleName=getParamValue(line, "ModuleName");
						}
					}
					// close stream
					sr.Close();

					// Synchronize with database
					if (currentFileName!=string.Empty)
						SynchronizeModule(new FileInfo(currentFileName).Name, moduleName);
				}

				// Search if modules are deleted in directory
				DataSet ds=LoadModuleDefs();
				if ((ds!=null) && (ds.Tables.Count>0))
				{
					for (int i=ds.Tables[0].Rows.Count-1; i>=0; i--)
					{
						DataRow currentDR=ds.Tables[0].Rows[i];
						string currentSource=currentDR[CST_FIELD_CONTROL].ToString();
						int j=0;
						while ((j<fileNames.Length) && (!currentSource.Equals(new FileInfo(fileNames[j]).Name)))
							j++;
						// If doesn't exist then the module is deleted in database
						if (j==fileNames.Length)
						{
							DeleteModuleDef(Convert.ToInt32(currentDR[CST_FIELD_WEBPARTID].ToString()));
							ds.Tables[0].Rows.Remove(currentDR);
						}
					}
				}
				if (ds!=null)
				{
					SaveModuleDefs(ds);
					ds.Dispose();
				}
			}
		}

		/// <summary>
		/// Init modules for current user if necessary
		/// </summary>
		public static void InitModules()
		{
			DataSet ds=LoadUserModules();
			if ((ds!=null) && (ds.Tables.Count>0) && (ds.Tables[0].Rows.Count==0))
			{
        using (Database db = Utils.GetMainDB())
        {
          SqlParameter[] parameters ={ new SqlParameter(CST_PARAM_USERID, SessionState.User.Id) };
          int result = db.RunSPReturnInteger(CST_SP_INIT_WEBPARTS, parameters);
        }
			}
			SaveUserModules(null);
			if (ds!=null)
				ds.Dispose();
		}
		#endregion
	
		#region Private static methods
		/// <summary>
		/// Retrieve attribute value
		/// </summary>
		/// <param name="line">Line containing attribute</param>
		/// <param name="paramName">Attribute name</param>
		/// <returns>Returns attribute value</returns>
		private static string getParamValue(string line, string paramName)
		{
			string paramValue=string.Empty;

			// search attribute begin
			int iBeg=line.IndexOf(paramName);
			if (iBeg>0)
			{
				// search attribute end
				int iEnd=line.IndexOf(" ", iBeg);
				if (iEnd<=iBeg)
					iEnd=line.IndexOf("/", iBeg);
				if (iEnd<=iBeg)
					iEnd=line.IndexOf(">", iBeg);

				if (iEnd>iBeg)
				{
					// get attribute
					string param=line.Substring(iBeg, iEnd-iBeg);
					string[] paramTab=param.Split(new char[] {'='});
					if ((paramTab!=null) && (paramTab.Length==2))
					{
						// get attribute value
						paramValue=paramTab[1]; 
						paramValue=paramValue.Replace("\"", "");
					}
				}
			}
			return paramValue;
		}

		/// <summary>
		/// Synchronize module with database
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="moduleName">Module name</param>
		private static void SynchronizeModule(string fileName, string moduleName)
		{
      using (Database db = Utils.GetMainDB())
      {
        SqlParameter[] parameters ={	new SqlParameter(CST_PARAM_CONTROL, fileName),
										new SqlParameter(CST_PARAM_CAPTION, moduleName)};
        int result = db.RunSPReturnInteger(CST_SP_SAVE_WEBPART, parameters);
      }
		}

		/// <summary>
		/// Delete module in database
		/// </summary>
		/// <param name="moduleDefId">Module definition identifier</param>
		private static void DeleteModuleDef(int moduleDefId)
		{
      using (Database db = Utils.GetMainDB())
      {
        SqlParameter[] parameters ={ new SqlParameter(CST_PARAM_WEBPARTID, moduleDefId) };
        int result = db.RunSPReturnInteger(CST_SP_DELETE_WEBPART, parameters);
      }
		}
		#endregion
	}
}
