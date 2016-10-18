using System;
using System.Data;
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.Schedule
{
	public class Schedule
	{
		#region Enum type
		public enum TypeDate :int {ACQUISITION=0, VALIDATION=1, TRNASLATION_GLOBAL=2, TRANSLATION_DETAILS=3, PUBLICATION=4};
		#endregion

		#region Privates vars
		private string _LastError = string.Empty;
		#endregion

		#region Properties
		public string LastError
		{
			get {return _LastError;}
		}
		#endregion

		#region Consts vars
		// SQL stored procedures
		private const string SP_RETRIEVE_PROJECTS = "PM_RetrieveProjects";

		// SQL parameters
		private const string CST_PARAM_SQL_USER_ID = "@UserId";
		private const string CST_PARAM_SQL_REQUIRED_ONLY = "@RequiredOnly";
		private const string CST_PARAM_SQL_ITEM_ID = "@SelectedItemId";
		private const string CST_PARAM_SQL_CULTURE_CODE = "@CultureCode";
		private const string CST_PARAM_SQL_TYPE = "@Type";

		// SQL columns names
		public const string CST_COLUMN_ITEM_ID = "ItemId";
		public const string CST_COLUMN_ITEM_NAME = "ItemName";
		public const string CST_COLUMN_CREATE_DATE = "CreateDate";
		public const string CST_COLUMN_EOA = "EOA";
		public const string CST_COLUMN_EOV = "EOV";
		public const string CST_COLUMN_BOT = "BOT";
		public const string CST_COLUMN_EOT = "EOT";
		public const string CST_COLUMN_BOP = "BOP";
		public const string CST_COLUMN_BOPP = "BOPP";
		public const string CST_COLUMN_EOP = "EOP";
		public const string CST_COLUMN_MISSING_COUNT = "MissingCount";
        //public const string CST_COLUMN_REJECTED_COUNT = "RejectedCount"; //Commented for alternate for CR 5096
		public const string CST_COLUMN_DRAFT_COUNT = "DraftCount";
		public const string CST_COLUMN_CULTURE_NAME = "CultureName";
		public const string CST_COLUMN_MISSING_TRANS_COUNT = "MissingTranslationCount";
		public const string CST_COLUMN_ITEM_NAME_AND_COUNT_MISSING = "ItemNameAndCountMissing";
		public const string CST_COLUMN_ITEM_NAME_AND_COUNT_MISSING_TRANS = "ItemNameAndCountMissingTrans";
		public const string CST_COLUMN_ITEM_NAME_AND_COUNT_DRAFT = "ItemNameAndCountDraft";
		public const string CST_COLUMN_IMG_PRIORITY = "ImgPriority";
		public const string CST_COLUMN_PRIORITY = "Priority"; // 0:Normal, 1:Urgent, 2:Late, 3:MissingDate

		// Const integer
		public const int CST_RANGE_TASK_VIEW = 45;
		public const int CST_DEPL_TIME_TASK_VIEW = 15;
		public const int CST_START_HOUR = 0;
		public const int CST_END_HOUR = 23;
		public const int CST_YEAR_INFINITY = 100;

		// Urgency colors
		public static readonly string[] ARRAY_COLORS = {"green", "orange", "black", "blue"};
		
		// ASPX parameters
		public const string CST_PARAM_START_DATE = "StartDate";
		public const string CST_PARAM_END_DATE = "EndDate";
		public const string CST_PARAM_ITEM_ID = "ItemId";

		// Labels
		public const string CST_TITLE_MY_PROJECTS = "My projects";
		public const string CST_TITLE_TRANSLATION_DETAILS = "Languages";
		public const string CST_ALT_START_PROJECT = "Move to start of project";
		public const string CST_ALT_END_PROJECT = "Move to end of project";

		// Viewstate keys
		public const string CST_KEY_SCHEDULE = "ScheduleKey";
		#endregion

		#region Constructor
		public Schedule(){}
		#endregion

		#region Publics methods
		/// <summary>
		/// Build dataset object for the creation of acquisition schedule
		/// </summary>
		/// <returns>Returns dataset object containing all projects with missing chunks (in master language), and dates</returns>
		public DataSet buildAcquSchedule()
		{
			DataSet ds = buildDataSet(-1, TypeDate.ACQUISITION);
			ds = updateInfinityDate(ds, CST_COLUMN_CREATE_DATE, CST_COLUMN_EOA);
			ds = addSpanTime(ds, CST_COLUMN_CREATE_DATE, CST_COLUMN_EOA);
			return ds;
		}

		/// <summary>
		/// Build dataset object for the creation of validation schedule
		/// </summary>
		/// <returns>Return dataset object containing all projects with missing chunks (in not final state and in master language) and dates</returns>
		public DataSet buildValSchedule()
		{
			DataSet ds = buildDataSet(-1, TypeDate.VALIDATION);
			ds = updateInfinityDate(ds, CST_COLUMN_EOA, CST_COLUMN_EOV);
			ds = addSpanTime(ds, CST_COLUMN_EOA, CST_COLUMN_EOV);
			return ds;
		}
		
		/// <summary>
		/// Build dataset object for the creation of translation schedule global
		/// </summary>
		/// <returns>Return dataset object containing all projects with missing chunks (in not final state and in language) and dates</returns>
		public DataSet buildGlobSchedule()
		{
			DataSet ds = buildDataSet(-1, TypeDate.TRNASLATION_GLOBAL);
			ds = updateInfinityDate(ds, CST_COLUMN_BOT, CST_COLUMN_EOT);
			ds = addSpanTime(ds, CST_COLUMN_BOT, CST_COLUMN_EOT);
			return ds;
		}

		public DataSet buildLocSchedule(int itemId)
		{
			DataSet ds = buildDataSet(itemId, TypeDate.TRANSLATION_DETAILS);
			ds = updateInfinityDate(ds, CST_COLUMN_BOT, CST_COLUMN_EOT);
			ds = addSpanTime(ds, CST_COLUMN_BOT, CST_COLUMN_EOT);
			return ds;
		}

		/// <summary>
		/// Build dataset object for the creation of translation schedule details
		/// </summary>
		/// <returns>Return dataset object containing all projects with missing chunks (in not final state and in locale) and dates</returns>
		public DataSet buildLocSchedule()
		{
			DataSet ds=null;
			return ds;
		}

		/// <summary>
		/// Build dataset object for the creation of publication schedule
		/// </summary>
		/// <returns>Return dataset object containing all projects with missing chunks and dates</returns>
		public DataSet buildPubSchedule()
		{
			DataSet ds=null;
			return ds;
		}

		/// <summary>
		/// Build dataset object for the creation of projects schedule
		/// </summary>
		/// <returns>Return dataset object containing all projects with missing chunks for each phases and dates</returns>
		public DataSet buildProjectSchedule()
		{
			DataSet ds=null;
			return ds;
		}
		#endregion

		#region Privates methods
		private DataSet buildDataSet(int itemId, TypeDate type)
		{
			SqlParameter[] parameters = {new SqlParameter(CST_PARAM_SQL_USER_ID, SessionState.User.Id), 
										 new SqlParameter(CST_PARAM_SQL_REQUIRED_ONLY, (byte)0),
										 new SqlParameter(CST_PARAM_SQL_ITEM_ID, itemId),
										 new SqlParameter(CST_PARAM_SQL_CULTURE_CODE, string.Empty),
										 new SqlParameter(CST_PARAM_SQL_TYPE, (int)type)};
			Database db = Utils.GetMainDB();
			DataSet ds = db.RunSPReturnDataSet(SP_RETRIEVE_PROJECTS, "ProjectDatesTable", parameters);
			if (db.LastError.Length > 0)
				_LastError = db.LastError;

			return ds;
		}

		private DataSet addSpanTime(DataSet ds, string startDateField, string endDateField)
		{
			// A COMPLETER
			if ((ds != null) && (ds.Tables.Count > 0))
			{
				for (int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					if (ds.Tables[0].Rows[i][startDateField] != DBNull.Value)
					{
						ds.Tables[0].Rows[i][startDateField] = ((DateTime)ds.Tables[0].Rows[i][startDateField]).Date;
						ds.Tables[0].Rows[i][startDateField] = ((DateTime)ds.Tables[0].Rows[i][startDateField]).Add(new TimeSpan(0,0,0));
					}
					if (ds.Tables[0].Rows[i][endDateField] != DBNull.Value)
					{
						ds.Tables[0].Rows[i][endDateField] = ((DateTime)ds.Tables[0].Rows[i][endDateField]).Date;
						ds.Tables[0].Rows[i][endDateField] = ((DateTime)ds.Tables[0].Rows[i][endDateField]).Add(new TimeSpan(23,59,59));
					}
				}
			}
			return ds;
		}

		private DataSet updateInfinityDate(DataSet ds, string startDateField, string endDateField)
		{
			if ((ds != null) && (ds.Tables.Count>0))
			{
				for (int i=ds.Tables[0].Rows.Count-1; i>=0; i--)
				{
					DataRow dr = ds.Tables[0].Rows[i];
					if (dr[startDateField]==DBNull.Value)
						ds.Tables[0].Rows.Remove(dr);
					else if (dr[endDateField]==DBNull.Value)
						dr[endDateField] = ((DateTime)dr[startDateField]).AddYears(CST_YEAR_INFINITY);
				}
			}
			return ds;
		}
		#endregion
	}
}
