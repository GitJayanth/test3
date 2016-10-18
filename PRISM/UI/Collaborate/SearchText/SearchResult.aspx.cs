#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Text;
using System.Text.RegularExpressions;
#endregion

#region History
/*=================Modification details===============/
 Mod#1 PRISM UI To PDB Changes
      Description: 
      Modfication Date:02/02/2013
      Modified by: Rekha Thomas*/
#endregion

namespace HyperCatalog.UI
{
    /// <summary>
    /// Description résumée de New_Search_Result.
    /// </summary>
    public partial class New_Search_Result : HCPage
    {
        #region Constants
        private const int MAX_EXCEL_SIZE = 65536;
        private const int MIN_TIMEOUT = 600;
        private const int MAX_SIZE = 100000;
        #endregion

        #region Declaration
        private int timeout = MIN_TIMEOUT;
        private HyperComponents.Data.dbAccess.Database dbObj;
        private HyperComponents.Data.dbAccess.DatabaseQuery dbQuery;
        OracleConnection oracleConn = new OracleConnection();
        private string oracleErrorMsg = string.Empty;

        #endregion

        #region Properties
        private string SQLItem
        {
            get
            {
                return (string)ViewState["SQLItem"];
            }
            set
            {
                ViewState["SQLItem"] = value;
            }
        }
        private string SQLContainer
        {
            get
            {
                return (string)ViewState["SQLContainer"];
            }
            set
            {
                ViewState["SQLContainer"] = value;
            }
        }
        private string SQLCulture
        {
            get
            {
                return (string)ViewState["SQLCulture"];
            }
            set
            {
                ViewState["SQLCulture"] = value;
            }
        }
        private string SQLContent
        {
            get
            {
                return (string)ViewState["SQLContent"];
            }
            set
            {
                ViewState["SQLContent"] = value;
            }
        }
        private bool Mandatory
        {
            get
            {
                return (bool)ViewState["Mandatory"];
            }
            set
            {
                ViewState["Mandatory"] = value;
            }
        }
        private Business.SearchQuery Query
        {
            get
            {
                return (Business.SearchQuery)ViewState["Query"];
            }
            set
            {
                ViewState["Query"] = value;
                if (value != null)
                {
                    SQLItem = value.SQLItem;
                    SQLContainer = value.SQLContainer;
                    SQLCulture = value.SQLCulture;
                    SQLContent = value.SQLContent;
                }
            }
        }
        private bool useCrystalDB
        {
            get
            {
                return (bool)ViewState["useCrystalDB"];
            }
            set
            {
                ViewState["useCrystalDB"] = value;
            }
        }
        private string appComponentName
        {
            get
            {
                /***** PRISM UI TO PDB CHANGES BY REKHA THOMAS*****/
                return useCrystalDB ? "Crystal_DB" : "PDB";
                /***** END PRISM UI TO PDB CHANGES BY REKHA THOMAS*****/
            }
        }
        /***********Start: Added for 3.5.02 ******************/
        private DataTable selectedCustomFilters
        {
            get
            {
                return ViewState["selectedCustomFilters"] != null ? (DataTable)ViewState["selectedCustomFilters"] : null;
            }
            set
            {
                ViewState["selectedCustomFilters"] = value;
            }
        }
        private DataTable selectedBusinessFilters
        {
            get
            {
                return ViewState["selectedBusinessFilters"] != null ? (DataTable)ViewState["selectedBusinessFilters"] : null;
            }
            set
            {
                ViewState["selectedBusinessFilters"] = value;
            }
        }
        private string SearchType
        {
            get
            {
                return ViewState["SearchType"] + "";
            }
            set
            {
                ViewState["SearchType"] = value;
            }
        }
        /***********End: Added for 3.5.02 ******************/

        /***********PRISM UI TO PDB CHANGES BY REKHA THOMAS ******/
        private DataTable SelectedCustomParameters
        {
            get
            {
                return ViewState["selectedCustomParameters"] != null ? (DataTable)ViewState["selectedCustomParameters"] : null;
            }
            set
            {
                ViewState["selectedCustomParameters"] = value;
            }
        }

        private DataTable SelectedBusinessParameters
        {
            get
            {
                return ViewState["selectedBusinessParameters"] != null ? (DataTable)ViewState["selectedBusinessParameters"] : null;
            }
            set
            {
                ViewState["selectedBusinessParameters"] = value;
            }
        }

        private string ProductName
        {
            get
            {
                return (string)ViewState["productName"];
            }
            set
            {
                ViewState["productName"] = value;
            }
        }

        private string ProductNumber
        {
            get
            {
                return (string)ViewState["productNumber"];
            }
            set
            {
                ViewState["productNumber"] = value;
            }
        }


        public string InputForm
        {
            get
            {
                return (string)ViewState["inputForm"];
            }
            set { ViewState["inputForm"] = value; }
        }


        private string ItemId1
        {
            get
            {
                return (string)ViewState["itemId1"];
            }
            set
            {
                ViewState["itemId1"] = value;
            }
        }


        public string ItemCreation
        {
            get
            {
                return (string)ViewState["itemCreation"];
            }
            set { ViewState["itemCreation"] = value; }
        }

        public string BlindDate
        {
            get
            {
                return (string)ViewState["blindDate"];
            }
            set { ViewState["blindDate"] = value; }
        }

        private string fullDate;

        public string FullDate
        {
            get
            {
                return (string)ViewState["fullDate"];
            }
            set { ViewState["fullDate"] = value; }
        }


        public string ObsoleteDate
        {
            get
            {
                return (string)ViewState["obsoleteDate"];
            }
            set { ViewState["obsoleteDate"] = value; }
        }

        public string RemovalDate
        {
            get
            {
                return (string)ViewState["removalDate"];
            }
            set { ViewState["removalDate"] = value; }
        }

        public string EndOfSupportDate
        {
            get
            {
                return (string)ViewState["endOfSupportDate"];
            }
            set { ViewState["endOfSupportDate"] = value; }
        }

        public string ContainerName
        {
            get
            {
                return (string)ViewState["containerName"];
            }
            set { ViewState["containerName"] = value; }
        }

        public string ContainerGroup
        {
            get
            {
                return (string)ViewState["containerGroup"];
            }
            set { ViewState["containerGroup"] = value; }
        }

        public string SubPath
        {
            get
            {
                return (string)ViewState["subPath"];
            }
            set { ViewState["subPath"] = value; }
        }

        public string FullPath
        {
            get
            {
                return (string)ViewState["fullPath"];
            }
            set { ViewState["fullPath"] = value; }
        }

        public string ContainerValue
        {
            get
            {
                return (string)ViewState["containerValue"];
            }
            set { ViewState["containerValue"] = value; }
        }

        public string ContentTouch
        {
            get
            {
                return (string)ViewState["contentTouch"];
            }
            set { ViewState["contentTouch"] = value; }
        }

        public string ItemClassList
        {
            get
            {
                return (string)ViewState["itemClassList"];
            }
            set { ViewState["itemClassList"] = value; }
        }


        public string ItemClassLevelList
        {
            get
            {
                return (string)ViewState["itemClassLevelList"];
            }
            set { ViewState["itemClassLevelList"] = value; }
        }

        public string ProductLineList
        {
            get
            {
                return (string)ViewState["productLineList"];
            }
            set { ViewState["productLineList"] = value; }
        }

        public string ContentStatusList
        {
            get
            {
                return (string)ViewState["contentStatusList"];
            }
            set { ViewState["contentStatusList"] = value; }
        }

        public string ContainerTypeList
        {
            get
            {
                return (string)ViewState["containerTypeList"];
            }
            set { ViewState["containerTypeList"] = value; }
        }


        public string CultureList
        {
            get
            {
                return (string)ViewState["cultureList"];
            }
            set { ViewState["cultureList"] = value; }
        }


        public string NodeStatusList
        {
            get
            {
                return (string)ViewState["nodeStatusList"];

            }
            set { ViewState["nodeStatusList"] = value; }
        }



        public string ItemTypeList
        {
            get
            {
                return (string)ViewState["itemTypeList"];
            }
            set { ViewState["itemTypeList"] = value; }
        }

        public bool? ProdWOPL
        {
            get { return (bool)ViewState["prodWOPL"]; }
            set { ViewState["prodWOPL"] = value; }
        }

        public bool ProdExpToProv
        {
            get { return (bool)ViewState["prodExpToProv"]; }
            set { ViewState["prodExpToProv"] = value; }
        }


        public bool JoinItems
        {
            get
            {
                return (bool)ViewState["joinItems"];
            }
            set { ViewState["joinItems"] = value; }
        }

        public bool JoinPLC
        {
            get
            {
                return (bool)ViewState["joinPLC"];
            }
            set { ViewState["joinPLC"] = value; }
        }

        public bool ProdWOcompat
        {
            get
            {
                return (bool)ViewState["prodWOcompat"];
            }
            set { ViewState["prodWOcompat"] = value; }
        }


        public int InstanceId
        {
            get
            {
                return (int)ViewState["instanceId"];
            }
            set
            {
                ViewState["instanceId"] = value;
            }
        }

        public string ConnString
        {
            get
            {
                return (string)ViewState["connString"];
            }
            set
            {
                ViewState["connString"] = value;
            }
        }

        //[BrowsableAttribute(false)]
        //private int commandTimeout;
        //public override int CommandTimeout 
        //{
        //    get
        //    {
        //        return commandTimeout;
        //    }
        //    set
        //    {
        //        commandTimeout = value;
        //    }

        //}

        /***********END PRISM UI TO PDB CHANGES ******/


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string CultureCode = SessionState.Culture.Code;
            string CountryCode = SessionState.Culture.Code.Substring(0, 2).ToUpper();
            LblError.Visible = false;
            lblNameValid.Visible = false;
            lblDesValid.Visible = false;
            //reloadLink.Visible = false;
            useCrystalDB = !(Request["cdb"] == null || Request["cdb"] == "0");
            if (useCrystalDB == true)
            {
                dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents[appComponentName].ConnectionString, 1800);
                //dbObj = new HyperComponents.Data.dbAccess.Database("Data Source= THOMAREK1\\SQLSERVER2008; Initial Catalog= Crystal; Integrated Security=SSPI");
            }
            else
            {
                oracleConn.ConnectionString = SessionState.CacheParams["PDBConnectionString"].Value.ToString();

                //oracleConn.ConnectionString = "Server = (DESCRIPTION = (SDU = 32768)(enable = broken) (LOAD_BALANCE = yes) (ADDRESS = (PROTOCOL = TCP)(HOST = gcu90440.houston.hp.com)(PORT = 1526)) (CONNECT_DATA = (SERVICE_NAME = prismi)));User Id = Pdb; Password = 08n0aA2$Cg3c";
            }

            //  Changes incorporated for Crystal-Gemstone convergence ( Deepa , 15/03/15)
            if (CultureCode.ToUpper() == "US-EN" || CultureCode.ToUpper() == "NA-EN" || CultureCode.ToUpper() == "WW-EN")
            {
                InstanceId = 1;
            }
            else
            {
                InstanceId = 2;
            }
            //  Deepa , 15/03/15 - End


            HyperCatalog.Business.Debug.Trace("UI.Architecture", "DB: " + appComponentName, Business.DebugSeverity.Low);

            if (!IsPostBack)
            {
                /***********PRISM UI TO PDB CHANGES  ******/
                //SubPath = (string)Session["SubPath"];
                //FullPath = (string)Session["FullPath"];
                Mandatory = (bool)Session["Mandatory"];
                Session.Remove("SubPath");
                Session.Remove("FullPath");
                Session.Remove("Mandatory");
                /***********END PRISM UI TO PDB CHANGES  ******/
                if (Session["SQLItem"] != null) // this session is filled at templateSearch.aspx when the user wants to view a saved report.
                {
                    Query = (Business.SearchQuery)Session["SearchQuery"];
                    SQLItem = (string)Session["SQLItem"];
                    SQLContainer = (string)Session["SQLContainer"];
                    SQLCulture = (string)Session["SQLCulture"];
                    SQLContent = (string)Session["SQLContent"];

                    Session.Remove("SQLItem");
                    Session.Remove("SQLContainer");
                    Session.Remove("SQLCulture");
                    Session.Remove("SQLContent");
                    Session.Remove("SearchQuery");

                }
                /***********Start: Added for 3.5.02 ******************/
                selectedCustomFilters = Session["selectedCustomFilters"] != null ? (DataTable)Session["selectedCustomFilters"] : null;
                selectedBusinessFilters = Session["selectedBusinessFilters"] != null ? (DataTable)Session["selectedBusinessFilters"] : null;
                SearchType = Session["SearchType"] + "";
                Session.Remove("selectedCustomFilters");
                Session.Remove("selectedBusinessFilters");
                Session.Remove("SearchType");
                /***********End: Added for 3.5.02 ******************/

                /***********PRISM UI TO PDB CHANGES BY REKHA THOMAS******/
                SelectedCustomParameters = Session["SelectedCustomParameters"] != null ? (DataTable)Session["SelectedCustomParameters"] : null;
                Session.Remove("SelectedCustomParameters");
                SelectedBusinessParameters = Session["SelectedBusinessParameters"] != null ? (DataTable)Session["SelectedBusinessParameters"] : null;
                Session.Remove("SelectedBusinessParameters");
                /***********END PRISM UI TO PDB CHANGES  ******/

                HyperCatalog.Business.Debug.Trace("UI.Architecture", "SQLItem " + SQLItem, Business.DebugSeverity.High);
                HyperCatalog.Business.Debug.Trace("UI.Architecture", "SQLContainer " + SQLContainer, Business.DebugSeverity.High);
                HyperCatalog.Business.Debug.Trace("UI.Architecture", "SQLCulture " + SQLCulture, Business.DebugSeverity.High);
                HyperCatalog.Business.Debug.Trace("UI.Architecture", "SQLContent " + SQLContent, Business.DebugSeverity.High);

                if (Request["cols"] != null)
                {
                    foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn column in dg.Columns)
                        column.Hidden = true;

                    foreach (string columnKey in Request["cols"].Split(','))
                    {
                        Infragistics.WebUI.UltraWebGrid.UltraGridColumn column = dg.Columns.FromKey(columnKey);
                        if (column != null)
                            column.Hidden = false;
                        if (columnKey == "ProductName")
                        {
                            column = dg.Columns.FromKey(columnKey + "Link");
                            if (column != null)
                                column.Hidden = false;
                        }
                    }
                }
                dg.Columns.FromKey("ModifiedDate").Format = SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime;

                dg.Visible = false;
                lRecordcount.Visible = false;
                lRecordcount.Text = string.Empty;

                BindGrid();

                if (Query != null)
                {
                    txtQueryName.Text = Query.Name;
                    txtQueryDescription.Value = Query.Description;
                    txtQueryVisibility.SelectedValue = Query.IsPublic ? "public" : "private";

                    // uwToolbar.Items.FromKeyButton("Save").Text = "Modify";
                    uwToolbar.Items.FromKeyButton("Save").ToolTip = "Modify Details";
                    UITools.ShowToolBarButton(uwToolbar, "Delete");
                    UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
                }
                else
                {
                    UITools.HideToolBarButton(uwToolbar, "Delete");
                    UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
                }
            }
        }



        private void BindGrid()
        {
            try
            {
                bool isCorrect = false;
                /*********** PRISM UI TO PDB CHANGES BY REKHA *********/
                if (useCrystalDB == true)
                {
                    using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT TOP 1 * FROM DBVersion ORDER BY VersionDate DESC"))
                    {
                        isCorrect = dbObj.LastError == "" && ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1 && Business.Settings.DBVersion != null && !(ds.Tables[0].Rows[0]["VersionNumber"] is DBNull) && Business.Settings.DBVersion.CompareTo(new Version((string)ds.Tables[0].Rows[0]["VersionNumber"])) == 0;
                    }
                    if (isCorrect)
                    {
                        if (Query == null)
                        {
                            RunNewReport(); // this function has partition for Oracle and SQL SP
                        }
                        else
                        {
                            RunSavedReport();
                        }
                    }
                }
                else if (useCrystalDB == false)
                {
                    //oracle dbVersion.
                    //using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT TOP 1 * FROM DBVersion ORDER BY VersionDate DESC"))
                    //{
                    //    isCorrect = dbObj.LastError == "" && ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1 && Business.Settings.DBVersion != null && !(ds.Tables[0].Rows[0]["VersionNumber"] is DBNull) && Business.Settings.DBVersion.CompareTo(new Version((string)ds.Tables[0].Rows[0]["VersionNumber"])) == 0;
                    //}
                    isCorrect = true;
                    if (isCorrect)
                    {
                        if (Query == null)
                        {
                            RunNewReport();
                        }
                        else
                        {
                            RunSavedReport();
                        }
                    }
                    // for testing
                }
                /***********END PRISM UI TO PDB CHANGES BY REKHA *********/
                else
                {
                    dg.Visible = false;
                    lRecordcount.Visible = false;
                    UITools.HideToolBarButton(uwToolbar, "Export");
                    LblError.Visible = true;
                    LblError.Text = "Error: DataWarehouse is not synchronized with the live database. Please contact support.";
                }
            }
            catch (Exception e)
            {
                if (dbObj != null)
                {
                    if (dbObj.TimeOut >= MIN_TIMEOUT)
                    {
                        LblError.Visible = true;
                        LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";
                        // reloadLink.Visible = true;
                    }
                    if (dbObj.LastError.Contains("Request timed out"))
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";

                    }
                    if (dbObj.LastError.Contains("Timeout"))
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";

                    }

                    else
                    {
                        LblError.Visible = true;
                        LblError.Text = "Error: Your search query has consumed too much time, it has been timed out.";
                    }
                }
                else
                {
                    //PDB Error display
                    LblError.Visible = true;
                    LblError.Text = "Error: Query not formatted correctly.";
                    if (!string.IsNullOrEmpty(oracleErrorMsg))
                    {
                        LblError.Text = "Error: " + oracleErrorMsg;
                    }
                }
            }
        }

        /***********PRISM UI TO PDB CHANGES BY REKHA THOMAS ******/

        /// <summary>
        /// Function to split a string into half: This function is written to reduce the PDB query size issue > 4KB
        /// parameters : value1, value2
        /// </summary>
        /// 

        public string splitString(string value1, out string value2)
        {
            string inputString = value1;
            int stringLength = inputString.Length;
            int midPosition = 0;

            if (stringLength % 2 == 0)
            {
                midPosition = stringLength / 2;
                value1 = inputString.Substring(0, midPosition);
                value2 = inputString.Substring(midPosition);
            }
            else
            {
                midPosition = (stringLength + 1) / 2;
                value1 = inputString.Substring(0, midPosition);
                value2 = inputString.Substring(midPosition);
            }
            return value1;
        }


        /// <summary>
        /// Function to get Nth index of a character
        /// parameters : inputString, searchChar and n (nth Occurence)
        /// </summary>
        /// 
        public int GetNthIndex(string inputString, string searchString, int n)
        {
            int count = 0;
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i].ToString() == searchString)
                {
                    count++;
                    if (count == n)
                        return i;
                }
            }
            return -1;
        }

        #region saved reports
        private void RunSavedReport()
        {
            // this function has separate code for Oracle and SQL Server
            if (useCrystalDB == true)
            {
                String strSQL = "EXEC SearchText_GetResult "
                            + HyperCatalog.Shared.SessionState.User.Id.ToString()
                            + ", '" + SQLItem + "'"
                            + ", '" + SQLContainer + "'"
                            + ", '" + SQLCulture + "'"
                            + ", '" + SQLContent + "'"
                            + ", " + (Mandatory ? "1" : "0")
                            + ", " + (useCrystalDB ? "1" : "0")
                            ;
                dbObj.TimeOut = timeout;

                using (DataSet ds = dbObj.RunSPReturnDataSet("SearchText_GetResult",
                new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id.ToString()),
                new SqlParameter("@ItemSQL", string.IsNullOrEmpty(SQLItem) ? (object)DBNull.Value : SQLItem.Replace("''", "'")),
                new SqlParameter("@ContainerSQL", string.IsNullOrEmpty(SQLContainer) ? (object)DBNull.Value : SQLContainer.Replace("''", "'")),
                new SqlParameter("@CultureSQL", string.IsNullOrEmpty(SQLCulture) ? (object)DBNull.Value : SQLCulture.Replace("''", "'")),
                new SqlParameter("@FilterSQL", string.IsNullOrEmpty(SQLContent) ? (object)DBNull.Value : SQLContent.Replace("''", "'")),
                new SqlParameter("@Mandatory", Mandatory),
                new SqlParameter("@UseCrystalDB", useCrystalDB)))
                {
                    HyperCatalog.Business.Debug.Trace("UI.Architecture", "Query: " + strSQL, Business.DebugSeverity.High);
                    dbObj.CloseConnection();
                    if (dbObj.LastError == string.Empty)
                    {
                        Session["SearchDataSet"] = ds;
                        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
                        UpdateDataView();
                    }
                    else if (dbObj.LastError.StartsWith("Timeout"))
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        if (timeout >= MIN_TIMEOUT)
                        {
                            LblError.Visible = true;
                            LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";
                            // reloadLink.Visible = true;
                        }
                        else
                        {
                            LblError.Visible = true;
                            LblError.Text = "Error: Your search query has consumed too much time, it has been timed out.";
                        }
                    }
                    else
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        LblError.Visible = true;
                        LblError.Text = "Error: " + dbObj.LastError.ToString();
                    }
                }
            }// end Crystal If
            else if (useCrystalDB == false)
            {
                DataSet oraDS = new DataSet();
                DataTable oraDT = new DataTable();
                string oraSql = string.Empty;
                try
                {
                    //oraSql = "select * from ( table (PDB.HP_PDB_AS(" + InstanceId + "," + SQLCulture + ",to_clob('" + SQLItem + "'))))";
                    string value1, value2;
                    value1 = splitString(SQLItem, out value2);
                    oraSql = "select * from ( table (PDB.HP_PDB_AS(" + InstanceId + "," + SQLCulture + ",to_clob('" + value1 + "'),to_clob('" + value2 + "'))))";
                    OracleCommand oraCmd = new OracleCommand(oraSql, oracleConn);
                    oracleConn.Open();
                    oraCmd.CommandText = oraSql;
                    OracleDataReader oraReader = oraCmd.ExecuteReader();
                    oraDT.Load(oraReader);
                    oraDS.Tables.Add(oraDT);
                    oraReader.Close();
                }

                catch (OracleException ex)
                {
                    //LblError.Visible = true;
                    //LblError.Text = "Warning: Database Error: " + ex.Message.ToString();
                    oracleErrorMsg = ex.Message.ToString();
                }
                finally
                {
                    oracleConn.Close();
                }

                {
                    HyperCatalog.Business.Debug.Trace("UI.Architecture", "Query: " + oraSql, Business.DebugSeverity.High);

                    if (oracleErrorMsg == string.Empty)
                    {
                        Session["SearchDataSet"] = oraDS;
                        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
                        UpdateDataView();
                    }
                    else if (oracleErrorMsg.Contains("Timeout"))
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        if (timeout >= MIN_TIMEOUT)
                        {
                            LblError.Visible = true;
                            LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";
                            // reloadLink.Visible = true;
                        }
                        else
                        {
                            LblError.Visible = true;
                            LblError.Text = "Error: Your search query has consumed too much time, it has been timed out.";
                        }
                    }
                    else
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        LblError.Visible = true;
                        LblError.Text = "Error: " + oracleErrorMsg.ToString();
                    }
                }
                // Oracle cmd.commandtimeout is 0 by default, i.e; no limit. Sql cmd.commandtimeout is 30s by default.
            }
        }
        #endregion run saved report

        #region new reports
        private void RunNewReport()
        {
            string strNewSQL = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (SelectedCustomParameters != null && SelectedCustomParameters.Rows.Count > 0)
            {
                foreach (DataRow dRow in SelectedCustomParameters.Rows)
                {
                    int i = 0;
                    string matchOperator = string.Empty;
                    switch (dRow.ItemArray[i].ToString())
                    {
                        case "productName":
                            if (string.IsNullOrEmpty(ProductName))
                            {
                                ProductName = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ProductName = string.Format(" {0} " + ProductName + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "productNumber":
                            if (string.IsNullOrEmpty(ProductNumber))
                            {
                                ProductNumber = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ProductNumber = string.Format(" {0} " + ProductNumber + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "inputForm":
                            if (string.IsNullOrEmpty(InputForm))
                            {
                                InputForm = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    InputForm = string.Format(" {0} " + InputForm + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "ItemId":
                            if (string.IsNullOrEmpty(ItemId1))
                            {
                                ItemId1 = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ItemId1 = string.Format(" {0} " + ItemId1 + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "itemCreation":
                            if (string.IsNullOrEmpty(ItemCreation))
                            {
                                ItemCreation = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ItemCreation = string.Format(" {0} " + ItemCreation + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "blindDate":
                            //BlindDate = dRow.ItemArray[i + 1].ToString();
                            //if (useCrystalDB == true)
                            //{
                            if (string.IsNullOrEmpty(BlindDate))
                            {
                                BlindDate = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    BlindDate = string.Format(" {0} " + BlindDate + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "fullDate":
                            if (string.IsNullOrEmpty(FullDate))
                            {
                                FullDate = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    FullDate = string.Format(" {0} " + FullDate + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "obsoleteDate":
                            if (string.IsNullOrEmpty(ObsoleteDate))
                            {
                                ObsoleteDate = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ObsoleteDate = string.Format(" {0} " + ObsoleteDate + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "removalDate":
                            if (string.IsNullOrEmpty(RemovalDate))
                            {
                                RemovalDate = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    RemovalDate = string.Format(" {0} " + RemovalDate + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "endOfSupportDate":
                            if (string.IsNullOrEmpty(EndOfSupportDate))
                            {
                                EndOfSupportDate = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    EndOfSupportDate = string.Format(" {0} " + EndOfSupportDate + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "containerName":
                            //ContainerName = dRow.ItemArray[i + 1].ToString();
                            if (string.IsNullOrEmpty(ContainerName))
                            {
                                ContainerName = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ContainerName = string.Format(" {0} " + ContainerName + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "containerGroup":
                            if (string.IsNullOrEmpty(ContainerGroup))
                            {
                                ContainerGroup = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ContainerGroup = string.Format(" {0} " + ContainerGroup + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "subPath":
                            if (string.IsNullOrEmpty(SubPath))
                            {
                                SubPath = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    SubPath = string.Format(" {0} " + SubPath + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "fullPath":
                            if (string.IsNullOrEmpty(FullPath))
                            {
                                FullPath = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    FullPath = string.Format(" {0} " + FullPath + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;

                        case "containerValue":
                            //ContainerValue = dRow.ItemArray[i + 1].ToString();
                            if (string.IsNullOrEmpty(ContainerValue))
                            {
                                ContainerValue = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                if (!string.IsNullOrEmpty(matchOperator))
                                {
                                    ContainerValue = string.Format(" {0} " + ContainerValue + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                }
                            }
                            break;
                        case "contentTouch":
                            if (string.IsNullOrEmpty(ContentTouch))
                            {
                                ContentTouch = dRow.ItemArray[i + 1].ToString();
                            }
                            else
                            {
                                if (useCrystalDB == true)
                                {
                                    matchOperator = dRow.ItemArray[i + 2].ToString() + "#";
                                    if (!string.IsNullOrEmpty(matchOperator))
                                    {
                                        ContentTouch = string.Format(" {0} " + ContentTouch + " {1} " + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                    }
                                }
                                else
                                {
                                    matchOperator = "'" + dRow.ItemArray[i + 2].ToString() + "#'";
                                    string piping = "||";
                                    if (!string.IsNullOrEmpty(matchOperator))
                                    {
                                        ContentTouch = string.Format(" {0} " + piping + ContentTouch + piping + " {1} " + piping + " {2} ", matchOperator, matchOperator, dRow.ItemArray[i + 1].ToString());
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            if (SelectedBusinessParameters != null && SelectedBusinessParameters.Rows.Count > 0)
            {
                foreach (DataRow row in SelectedBusinessParameters.Rows)
                {
                    int i = 0;

                    switch (row.ItemArray[i].ToString())
                    {
                        case "itemClassList":
                            ItemClassList = row.ItemArray[i + 1].ToString();
                            break;
                        case "itemClassLevelList":
                            ItemClassLevelList = row.ItemArray[i + 1].ToString();
                            break;
                        case "productLineList":
                            ProductLineList = row.ItemArray[i + 1].ToString();
                            break;
                        case "contentStatusList":
                            ContentStatusList = row.ItemArray[i + 1].ToString();
                            break;
                        case "containerTypeList":
                            ContainerTypeList = row.ItemArray[i + 1].ToString();
                            break;
                        case "cultureList":
                            CultureList = row.ItemArray[i + 1].ToString();
                            break;
                        case "nodeStatusList":
                            NodeStatusList = row.ItemArray[i + 1].ToString();
                            break;
                        case "itemTypeList":
                            ItemTypeList = row.ItemArray[i + 1].ToString();
                            break;
                        case "prodWOPL":
                            ProdWOPL = Boolean.Parse(row.ItemArray[i + 1].ToString());
                            break;
                        case "prodExpToProv":
                            ProdExpToProv = Boolean.Parse(row.ItemArray[i + 1].ToString());
                            break;
                        case "joinItems":
                            JoinItems = Boolean.Parse(row.ItemArray[i + 1].ToString());
                            break;
                        case "joinPLC":
                            JoinPLC = Boolean.Parse(row.ItemArray[i + 1].ToString());
                            break;
                        case "prodWOcompat":
                            ProdWOcompat = Boolean.Parse(row.ItemArray[i + 1].ToString());
                            break;
                        default:
                            break;
                    }
                }
            }

            if (useCrystalDB == false)
            {
                // code for running PDB SP
                string resultQueryPDB = string.Empty;
                oracleConn.Open();
                OracleCommand cmd = new OracleCommand("PDB.HP_PDB_SAVE_AS", oracleConn);
                cmd.CommandType = CommandType.StoredProcedure;
                //InstanceId = 1; // Hard coded for testing.
                cmd.Parameters.Add(new OracleParameter("p_instance_id", InstanceId));
                cmd.Parameters.Add(new OracleParameter("p_product_name", string.IsNullOrEmpty(ProductName) ? (object)DBNull.Value : ProductName));
                cmd.Parameters.Add(new OracleParameter("p_product_number", string.IsNullOrEmpty(ProductNumber) ? (object)DBNull.Value : ProductNumber));
                cmd.Parameters.Add(new OracleParameter("p_container_name", string.IsNullOrEmpty(ContainerName) ? (object)DBNull.Value : ContainerName));
                cmd.Parameters.Add(new OracleParameter("p_container_value", string.IsNullOrEmpty(ContainerValue) ? (object)DBNull.Value : ContainerValue));
                cmd.Parameters.Add(new OracleParameter("p_container_group", string.IsNullOrEmpty(ContainerGroup) ? (object)DBNull.Value : ContainerGroup));
                cmd.Parameters.Add(new OracleParameter("p_container_sub_path", string.IsNullOrEmpty(SubPath) ? (object)DBNull.Value : SubPath));
                cmd.Parameters.Add(new OracleParameter("p_container_full_path", string.IsNullOrEmpty(FullPath) ? (object)DBNull.Value : FullPath));
                cmd.Parameters.Add(new OracleParameter("p_content_touched", string.IsNullOrEmpty(ContentTouch) ? (object)DBNull.Value : ContentTouch));
                cmd.Parameters.Add(new OracleParameter("p_input_form", string.IsNullOrEmpty(InputForm) ? (object)DBNull.Value : InputForm));
                cmd.Parameters.Add(new OracleParameter("p_node_created", string.IsNullOrEmpty(ItemCreation) ? (object)DBNull.Value : ItemCreation));
                cmd.Parameters.Add(new OracleParameter("p_itemid", string.IsNullOrEmpty(ItemId1) ? (object)DBNull.Value : ItemId1));
                cmd.Parameters.Add(new OracleParameter("p_selective_availability_dt", string.IsNullOrEmpty(BlindDate) ? (object)DBNull.Value : BlindDate));
                cmd.Parameters.Add(new OracleParameter("p_general_availability_dt", string.IsNullOrEmpty(FullDate) ? (object)DBNull.Value : FullDate));
                cmd.Parameters.Add(new OracleParameter("p_end_of_sales_dt", string.IsNullOrEmpty(ObsoleteDate) ? (object)DBNull.Value : ObsoleteDate));
                cmd.Parameters.Add(new OracleParameter("p_removal_dt", string.IsNullOrEmpty(RemovalDate) ? (object)DBNull.Value : RemovalDate));
                cmd.Parameters.Add(new OracleParameter("p_end_of_support_dt", string.IsNullOrEmpty(EndOfSupportDate) ? (object)DBNull.Value : EndOfSupportDate));
                cmd.Parameters.Add(new OracleParameter("p_mandatory", Convert.ToInt16(Mandatory)));
                cmd.Parameters.Add(new OracleParameter("p_product_type", string.IsNullOrEmpty(ItemClassList) ? (object)DBNull.Value : ItemClassList));
                cmd.Parameters.Add(new OracleParameter("p_hierarchy_level", string.IsNullOrEmpty(ItemClassLevelList) ? (object)DBNull.Value : ItemClassLevelList));
                cmd.Parameters.Add(new OracleParameter("p_product_line", string.IsNullOrEmpty(ProductLineList) ? (object)DBNull.Value : ProductLineList));
                cmd.Parameters.Add(new OracleParameter("p_content_status", string.IsNullOrEmpty(ContentStatusList) ? (object)DBNull.Value : ContentStatusList));
                cmd.Parameters.Add(new OracleParameter("p_container_type", string.IsNullOrEmpty(ContainerTypeList) ? (object)DBNull.Value : ContainerTypeList));
                cmd.Parameters.Add(new OracleParameter("p_catalog", string.IsNullOrEmpty(CultureList) ? (object)DBNull.Value : CultureList));
                cmd.Parameters.Add(new OracleParameter("p_node_status", string.IsNullOrEmpty(NodeStatusList) ? (object)DBNull.Value : NodeStatusList));
                cmd.Parameters.Add(new OracleParameter("p_item_type", string.IsNullOrEmpty(ItemTypeList) ? (object)DBNull.Value : ItemTypeList));
                cmd.Parameters.Add(new OracleParameter("p_product_without_comp", Convert.ToInt16(ProdWOcompat)));
                cmd.Parameters.Add(new OracleParameter("p_product_without_PL", Convert.ToInt16(ProdWOPL)));
                cmd.Parameters.Add(new OracleParameter("p_product_exportedto_prov", Convert.ToInt16(ProdExpToProv)));


                OracleParameter result = new OracleParameter();
                result.OracleType = OracleType.Clob;
                result.Direction = ParameterDirection.ReturnValue;
                result.Size = 100000;
                cmd.Parameters.Add(result);
                cmd.ExecuteNonQuery();
                resultQueryPDB = ((System.Data.OracleClient.OracleLob)(result.Value)).Value.ToString();
                cmd.Dispose();
                string value1, value2;
                value1 = splitString(resultQueryPDB, out value2);
                // Calling the second function
                OracleCommand command = oracleConn.CreateCommand();
                string sql = "select * from ( table (PDB.HP_PDB_AS(" + InstanceId + "," + CultureList + ",to_clob('" + value1 + "'),to_clob('" + value2 + "'))))";
                command.CommandText = sql;
                OracleDataReader reader = command.ExecuteReader();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                if (reader.HasRows == true)
                {
                    dt.Load(reader);
                    ds.Tables.Add(dt);
                }
                SQLItem = resultQueryPDB;
                SQLCulture = CultureList;
                SQLContainer = null;
                SQLContent = null;
                {
                    HyperCatalog.Business.Debug.Trace("UI.Architecture", "Query: " + resultQueryPDB, Business.DebugSeverity.High);
                    //dbObj.CloseConnection();
                    reader.Close();
                    oracleConn.Close();
                    command.Dispose();
                    //if (dbObj.LastError == string.Empty)
                    if (ds == null || ds.Tables.Count == 0)
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = true;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        if (ds.Tables.Count == 0)
                        {
                            //LblError.Visible = true;
                            lRecordcount.Text = "Record Count 0";
                            //LblError.Text = "Warning: Your search query returned no records. Please refine your search.";
                        }
                        else if (timeout >= MIN_TIMEOUT)
                        {
                            LblError.Visible = true;
                            LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";
                            // reloadLink.Visible = true;
                        }
                        else
                        {
                            LblError.Visible = true;
                            LblError.Text = "Error: Your search query has consumed too much time, it has been timed out.";
                        }
                    }
                    else if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        Session["SearchDataSet"] = ds;
                        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
                        UpdateDataView();
                    }
                    //else if (dbObj.LastError.StartsWith("Timeout"))

                    else
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        LblError.Visible = true;
                        LblError.Text = "Error: " + dbObj.LastError.ToString();
                    }
                }
            }// end if

            else if (useCrystalDB == true)
            {
                DataSet ds = new DataSet();
                strNewSQL = "EXEC SearchText_GetNewReportResults"
                              + HyperCatalog.Shared.SessionState.User.Id.ToString()
                              + ", " + (Mandatory ? "1" : "0")
                              + ", " + (useCrystalDB ? "1" : "0")
                              + ", '" + (string.IsNullOrEmpty(ProductName) ? (object)DBNull.Value : ProductName) + "'"
                                + ", '" + (string.IsNullOrEmpty(ProductNumber) ? (object)DBNull.Value : ProductNumber) + "'"
                                + ", '" + (string.IsNullOrEmpty(InputForm) ? (object)DBNull.Value : InputForm) + "'"
                                + ", '" + (string.IsNullOrEmpty(ItemId1) ? (object)DBNull.Value : ItemId1) + "'"
                                + ", '" + (string.IsNullOrEmpty(ItemCreation) ? (object)DBNull.Value : ItemCreation) + "'"
                                + ", '" + (string.IsNullOrEmpty(BlindDate) ? (object)DBNull.Value : BlindDate) + "'"
                                + ", '" + (string.IsNullOrEmpty(FullDate) ? (object)DBNull.Value : FullDate) + "'"
                                + ", '" + (string.IsNullOrEmpty(ObsoleteDate) ? (object)DBNull.Value : ObsoleteDate) + "'"
                                + ", '" + (string.IsNullOrEmpty(RemovalDate) ? (object)DBNull.Value : RemovalDate) + "'"
                                + ", '" + (string.IsNullOrEmpty(EndOfSupportDate) ? (object)DBNull.Value : EndOfSupportDate) + "'"
                                + ", '" + (string.IsNullOrEmpty(ContainerName) ? (object)DBNull.Value : ContainerName) + "'"
                                + ", '" + (string.IsNullOrEmpty(ContainerGroup) ? (object)DBNull.Value : ContainerGroup) + "'"
                                + ", '" + (string.IsNullOrEmpty((string)Session["SubPath"]) ? (object)DBNull.Value : SubPath) + "'"
                                + ", '" + (string.IsNullOrEmpty((string)Session["FullPath"]) ? (object)DBNull.Value : FullPath) + "'"
                                + ", '" + (string.IsNullOrEmpty(ContentTouch) ? (object)DBNull.Value : ContentTouch) + "'"
                                + ", '" + (string.IsNullOrEmpty(ContainerValue) ? (object)DBNull.Value : ContainerValue) + "'"
                                + ", '" + (string.IsNullOrEmpty(ItemClassList) ? (object)DBNull.Value : ItemClassList) + "'"
                                + ", '" + (string.IsNullOrEmpty(ItemClassLevelList) ? (object)DBNull.Value : ItemClassLevelList) + "'"
                                + ", '" + (string.IsNullOrEmpty(ProductLineList) ? (object)DBNull.Value : ProductLineList) + "'"
                                + ", '" + (string.IsNullOrEmpty(ContentStatusList) ? (object)DBNull.Value : ContentStatusList) + "'"
                                + ", '" + (string.IsNullOrEmpty(ContainerTypeList) ? (object)DBNull.Value : ContainerTypeList) + "'"
                                + ", '" + (string.IsNullOrEmpty(CultureList) ? (object)DBNull.Value : CultureList) + "'"
                                + ", '" + (string.IsNullOrEmpty(NodeStatusList) ? (object)DBNull.Value : NodeStatusList) + "'"
                                + ", '" + (string.IsNullOrEmpty(ItemTypeList) ? (object)DBNull.Value : ItemTypeList) + "'"
                                + ", '" + ((bool)ProdWOPL ? "1" : "0") + "'"
                                + ", '" + ((bool)ProdWOcompat ? "1" : "0") + "'"
                                + ", '" + ((bool)JoinItems ? "1" : "0") + "'"
                                + ", '" + ((bool)JoinPLC ? "1" : "0") + "'"
                                ;

                dbObj.TimeOut = timeout;
                dbObj.OpenConnection();
                dbQuery = dbObj.CreateSPQuery("SearchText_GetNewReportResults",
                new SqlParameter("@UserId", int.Parse(HyperCatalog.Shared.SessionState.User.Id.ToString())),
                new SqlParameter("@Mandatory", Mandatory),
                new SqlParameter("@UseCrystalDB", useCrystalDB),
                new SqlParameter("@ProductName", string.IsNullOrEmpty(ProductName) ? (object)DBNull.Value : ProductName.Replace("''", "'")),
                new SqlParameter("@ProductNumber", string.IsNullOrEmpty(ProductNumber) ? (object)DBNull.Value : ProductNumber.Replace("''", "'")),
                new SqlParameter("@InputForm", string.IsNullOrEmpty(InputForm) ? (object)DBNull.Value : InputForm.Replace("''", "'")),
                new SqlParameter("@ItemId1", string.IsNullOrEmpty(ItemId1) ? (object)DBNull.Value : ItemId1.Replace("''", "'")),
                new SqlParameter("@ItemCreation", string.IsNullOrEmpty(ItemCreation) ? (object)DBNull.Value : ItemCreation),
                new SqlParameter("@BlindDate", string.IsNullOrEmpty(BlindDate) ? (object)DBNull.Value : BlindDate),
                new SqlParameter("@FullDate", string.IsNullOrEmpty(FullDate) ? (object)DBNull.Value : FullDate),
                new SqlParameter("@ObsoleteDate", string.IsNullOrEmpty(ObsoleteDate) ? (object)DBNull.Value : ObsoleteDate),
                new SqlParameter("@RemovalDate", string.IsNullOrEmpty(RemovalDate) ? (object)DBNull.Value : RemovalDate),
                new SqlParameter("@EndOfSupportDate", string.IsNullOrEmpty(EndOfSupportDate) ? (object)DBNull.Value : EndOfSupportDate),
                new SqlParameter("@ContainerName", string.IsNullOrEmpty(ContainerName) ? (object)DBNull.Value : ContainerName.Replace("''", "'")),
                new SqlParameter("@ContainerGroup", string.IsNullOrEmpty(ContainerGroup) ? (object)DBNull.Value : ContainerGroup.Replace("''", "'")),
                new SqlParameter("@SubPath", string.IsNullOrEmpty(SubPath) ? (object)DBNull.Value : SubPath.Replace("''", "'")),
                new SqlParameter("@FullPath", string.IsNullOrEmpty(FullPath) ? (object)DBNull.Value : FullPath.Replace("''", "'")),
                new SqlParameter("@ContentTouch", string.IsNullOrEmpty(ContentTouch) ? (object)DBNull.Value : ContentTouch.Replace("''", "'")),
                new SqlParameter("@ContainerValue", string.IsNullOrEmpty(ContainerValue) ? (object)DBNull.Value : ContainerValue.Replace("''", "'")),
                new SqlParameter("@ItemClassList", string.IsNullOrEmpty(ItemClassList) ? (object)DBNull.Value : ItemClassList.Replace("''", "'")),
                new SqlParameter("@ItemClassLevelList", string.IsNullOrEmpty(ItemClassLevelList) ? (object)DBNull.Value : ItemClassLevelList.Replace("''", "'")),
                new SqlParameter("@ProductLineList", string.IsNullOrEmpty(ProductLineList) ? (object)DBNull.Value : ProductLineList.Replace("''", "'")),
                new SqlParameter("@ContentStatusList", string.IsNullOrEmpty(ContentStatusList) ? (object)DBNull.Value : ContentStatusList.Replace("''", "'")),
                new SqlParameter("@ContainerTypeList", string.IsNullOrEmpty(ContainerTypeList) ? (object)DBNull.Value : ContainerTypeList.Replace("''", "'")),
                new SqlParameter("@CultureList", string.IsNullOrEmpty(CultureList) ? (object)DBNull.Value : CultureList.Replace("''", "'")),
                new SqlParameter("@NodeStatusList", string.IsNullOrEmpty(NodeStatusList) ? (object)DBNull.Value : NodeStatusList.Replace("''", "'")),
                new SqlParameter("@ItemTypeList", string.IsNullOrEmpty(ItemTypeList) ? (object)DBNull.Value : ItemTypeList.Replace("''", "'")),
                new SqlParameter("@ProdWOPL", (bool)ProdWOPL),
                    //cmd.Parameters.Add(new SqlParameter("@ProdWOPL", SqlDbType.Bit)).Value = (ProdWOPL == null) ? (object)DBNull.Value : ProdWOPL;
                    //new SqlParameter("@ProdExpToProv", ProdExpToProv),
                new SqlParameter("@ProdWOcompat", (bool)ProdWOcompat),
                new SqlParameter("@JoinItems", (bool)JoinItems),
                new SqlParameter("@JoinPLC", (bool)JoinPLC)
                );
                dbQuery.AddOutputParameters(new SqlParameter("@ItemSQL", SqlDbType.NVarChar, MAX_SIZE));
                dbQuery.AddOutputParameters(new SqlParameter("@ContainerSQL", SqlDbType.NVarChar, MAX_SIZE));
                dbQuery.AddOutputParameters(new SqlParameter("@FilterSQL", SqlDbType.NVarChar, MAX_SIZE));
                dbQuery.AddOutputParameters(new SqlParameter("@CultureSQL", SqlDbType.NVarChar, MAX_SIZE));

                ds = dbQuery.RunAndReturnDataSet();

                SQLItem = Convert.ToString(dbQuery.GetParameterValue("@ItemSQL")).Trim();
                SQLContainer = Convert.ToString(dbQuery.GetParameterValue("@ContainerSQL")).Trim();
                SQLContent = Convert.ToString(dbQuery.GetParameterValue("@FilterSQL")).Trim();
                SQLCulture = Convert.ToString(dbQuery.GetParameterValue("@CultureSQL")).Trim();

                //for testing with local sql server instance:

                //ConnString = "Data Source= THOMAREK1\\SQLSERVER2008; Initial Catalog= Crystal; Integrated Security=SSPI";
                //dbObj.OpenConnection(ConnString);
                //SqlConnection sqlConn = new SqlConnection(ConnString);
                //sqlConn.Open();
                //SqlCommand cmd;
                ////DataSet ds = new DataSet();
                //cmd = new SqlCommand("SearchText_GetNewReportResults", sqlConn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandTimeout = 300;
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)).Value = int.Parse(HyperCatalog.Shared.SessionState.User.Id.ToString());
                //cmd.Parameters.Add(new SqlParameter("@Mandatory", SqlDbType.Bit)).Value = Mandatory;
                //cmd.Parameters.Add(new SqlParameter("@UseCrystalDB", SqlDbType.Bit)).Value = useCrystalDB;
                //cmd.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ProductName) ? (object)DBNull.Value : ProductName.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ProductNumber", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ProductNumber) ? (object)DBNull.Value : ProductNumber.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@InputForm", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(InputForm) ? (object)DBNull.Value : InputForm.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ItemId1", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ItemId1) ? (object)DBNull.Value : ItemId1.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ItemCreation", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ItemCreation) ? (object)DBNull.Value : ItemCreation;
                //cmd.Parameters.Add(new SqlParameter("@BlindDate", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(BlindDate) ? (object)DBNull.Value : BlindDate;
                //cmd.Parameters.Add(new SqlParameter("@FullDate", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(FullDate) ? (object)DBNull.Value : FullDate;
                //cmd.Parameters.Add(new SqlParameter("@ObsoleteDate", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ObsoleteDate) ? (object)DBNull.Value : ObsoleteDate;
                //cmd.Parameters.Add(new SqlParameter("@RemovalDate", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(RemovalDate) ? (object)DBNull.Value : RemovalDate;
                //cmd.Parameters.Add(new SqlParameter("@EndOfSupportDate", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(EndOfSupportDate) ? (object)DBNull.Value : EndOfSupportDate;

                //cmd.Parameters.Add(new SqlParameter("@ContainerName", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ContainerName) ? (object)DBNull.Value : ContainerName.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ContainerGroup", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ContainerGroup) ? (object)DBNull.Value : ContainerGroup.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@SubPath", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(SubPath) ? (object)DBNull.Value : SubPath.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@FullPath", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(FullPath) ? (object)DBNull.Value : FullPath.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ContentTouch", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ContentTouch) ? (object)DBNull.Value : ContentTouch;
                //cmd.Parameters.Add(new SqlParameter("@ContainerValue", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ContainerValue) ? (object)DBNull.Value : ContainerValue.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ItemClassList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ItemClassList) ? (object)DBNull.Value : ItemClassList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ItemClassLevelList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ItemClassLevelList) ? (object)DBNull.Value : ItemClassLevelList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ProductLineList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ProductLineList) ? (object)DBNull.Value : ProductLineList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ContentStatusList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ContentStatusList) ? (object)DBNull.Value : ContentStatusList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ContainerTypeList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ContainerTypeList) ? (object)DBNull.Value : ContainerTypeList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@CultureList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(CultureList) ? (object)DBNull.Value : CultureList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@NodeStatusList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(NodeStatusList) ? (object)DBNull.Value : NodeStatusList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ItemTypeList", SqlDbType.NVarChar)).Value = string.IsNullOrEmpty(ItemTypeList) ? (object)DBNull.Value : ItemTypeList.Replace("''", "'");
                //cmd.Parameters.Add(new SqlParameter("@ProdWOPL", SqlDbType.Bit)).Value = (ProdWOPL == null) ? (object)DBNull.Value : ProdWOPL;
                //cmd.Parameters.Add(new SqlParameter("@ProdWOcompat", SqlDbType.Bit)).Value = ProdWOcompat;
                //cmd.Parameters.Add(new SqlParameter("@JoinItems", SqlDbType.Bit)).Value = JoinItems;
                //cmd.Parameters.Add(new SqlParameter("@JoinPLC", SqlDbType.Bit)).Value = JoinPLC;
                //SqlParameter parm1 = cmd.Parameters.Add("@ItemSQL", SqlDbType.NVarChar, 100000);
                //parm1.Direction = ParameterDirection.Output;
                //SqlParameter parm2 = cmd.Parameters.Add("@ContainerSQL", SqlDbType.NVarChar, 100000);
                //parm2.Direction = ParameterDirection.Output;
                //SqlParameter parm3 = cmd.Parameters.Add("@FilterSQL", SqlDbType.NVarChar, 100000);
                //parm3.Direction = ParameterDirection.Output;
                //SqlParameter parm4 = cmd.Parameters.Add("@CultureSQL", SqlDbType.NVarChar, 100000);
                //parm4.Direction = ParameterDirection.Output;
                //da.Fill(ds);

                //End for testing with local sql server instance:


                {
                    HyperCatalog.Business.Debug.Trace("UI.Architecture", "Query: " + strNewSQL, Business.DebugSeverity.High);
                    dbObj.CloseConnection();
                    if (dbObj.LastError == string.Empty)
                    {
                        Session["SearchDataSet"] = ds;
                        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
                        UpdateDataView();
                    }
                    else if (dbObj.LastError.StartsWith("Timeout"))
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        if (timeout >= MIN_TIMEOUT)
                        {
                            LblError.Visible = true;
                            LblError.Text = "Warning: Your search query is consuming a lot of time.<BR/>Please refine your search by <a href=\"#\" onclick=\"window.close();\">providing more filters</a>";
                            // reloadLink.Visible = true;
                        }
                        else
                        {
                            LblError.Visible = true;
                            LblError.Text = "Error: Your search query has consumed too much time, it has been timed out.";
                        }
                    }
                    else
                    {
                        dg.Visible = false;
                        lRecordcount.Visible = false;
                        UITools.HideToolBarButton(uwToolbar, "Export");
                        LblError.Visible = true;
                        LblError.Text = "Error: " + dbObj.LastError.ToString();
                    }
                }
            } // end else
        }
        #endregion run new reports
        /***********END PRISM UI TO PDB CHANGES BY REKHA THOMAS ******/

        private void UpdateDataView()
        {
            DataSet ds = (DataSet)Session["SearchDataSet"];
            if (ds != null)
            {
                lRecordcount.Text = "<b>Recordcount : </b>" + ds.Tables[0].Rows.Count.ToString();
                lRecordcount.Visible = true;
                if (ds.Tables[0].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count > Convert.ToInt32(SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value))
                    {
                        tooManyRowsPanel.Visible = true;
                        UITools.HideToolBarButton(uwToolbar, "Export");

                        XLSPanel.Visible = (ds.Tables[0].Rows.Count <= MAX_EXCEL_SIZE);
                    }
                    else
                    {
                        bool hideLink = false;
                        Infragistics.WebUI.UltraWebGrid.UltraGridColumn col;
                        col = dg.Columns.FromKey("ProductName");
                        col.ServerOnly = col.Hidden || !hideLink;
                        col = dg.Columns.FromKey("ProductNameLink");
                        col.ServerOnly = col.Hidden || hideLink;

                        dg.DataSource = ds;
                        Utils.InitGridSort(ref dg, true);
                        dg.DataBind();
                        LblError.Visible = false;
                        dg.Visible = true;
                        UITools.ShowToolBarButton(uwToolbar, "Export");
                    }
                }
                else { dg.Visible = false; }
            }
        }

        #region events
        protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            switch (be.Button.Key.ToLower())
            {
                case "delete":
                    //QC 1058- Unable to delete a saved Query
                    // Modified bt Kanthi.J for 4.7.00.004 build
                    // Removed query from session and hence refreshing the parent page

                    if (Query != null)
                    {
                        SearchQuery qry = Shared.SessionState.User.SearchQueries[Query.Name];
                        if (qry != null)
                            Shared.SessionState.User.SearchQueries.Remove(qry);
                        if (Query.Delete())
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "closewindow", "<script>window.opener.location.href = window.opener.location.href;window.close();</script>");
                    }
                    break;
                case "run":
                    lRecordcount.Visible = true;
                    BindGrid();
                    break;
                case "refresh":
                    BindGrid();
                    break;
                default:
                    excelLink_Click(null, null);
                    break;
            }
        }
        protected void saveBtn_Click(object sender, EventArgs e)
        {
            lbError.Visible = false;

            if (txtQueryName.Text.ToString().Trim() == string.Empty)
            {
                lblNameValid.Text = "Please enter Name to save the query";
                lblNameValid.Visible = true;
                lblNameValid.ForeColor = Color.Red;
                LblError.Text = "Query Not Saved";
                LblError.Visible = true;
                RegisterStartupScript("gg", "<script type='text/javascript' language='javascript' >document.getElementById('savingForm').style.display='';</script>");
                return;

            }
            if (txtQueryDescription.Value.ToString().Trim() == string.Empty)
            {
                lblDesValid.Text = "Please enter Description to save the query";
                lblDesValid.Visible = true;
                lblDesValid.ForeColor = Color.Red;
                LblError.Text = "Query Not Saved";
                LblError.Visible = true;
                lbError.CssClass = "hc_error";
                RegisterStartupScript("gg", "<script type='text/javascript' language='javascript' >document.getElementById('savingForm').style.display='';</script>");

                return;

            }
            if (useCrystalDB == false)
            {
                SQLContainer = string.Empty;
                SQLContent = string.Empty;
            }
            bool isNew = (Query == null);
            if (isNew)
            {
                //Testing if save for Oracle PDB is not working because of so much data in SQLItem. 
                //SQLItem = "TestItem";
                //SQLCulture = "TestCulture";

                Query = new Business.SearchQuery(Shared.SessionState.User.Id, txtQueryName.Text, txtQueryDescription.Value, txtQueryVisibility.SelectedValue == "public", SQLItem, SQLContainer, SQLCulture, SQLContent, Mandatory, Business.ApplicationSettings.Components[appComponentName].Id);
                if (Query.Save())
                {
                    Shared.SessionState.User.SearchQueries.Add(Query);
                    lbError.CssClass = "hc_success";
                    lbError.Text = "Query saved!";
                    lbError.Visible = true;
                    // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "closewindow", "<script>window.opener.location.href = window.opener.location.href;window.close();</script>");
                }
                else
                {
                    Query = null;
                    lbError.CssClass = "hc_error";
                    lbError.Text = Business.SearchQuery.LastError;
                    lbError.Visible = true;
                }
            }
            else
            {
                string oldquery = Query.Name;
                if (!Query.Update(txtQueryName.Text, txtQueryDescription.Value, txtQueryVisibility.SelectedValue == "public"))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = Business.SearchQuery.LastError;
                    lbError.Visible = true;
                }
                else
                {

                    Query.Name = txtQueryName.Text;
                    Query.Description = txtQueryDescription.Value;


                    SearchQuery qryOld = Shared.SessionState.User.SearchQueries[oldquery];
                    if (qryOld != null)
                    {
                        Shared.SessionState.User.SearchQueries.Remove(qryOld);
                    }

                    Shared.SessionState.User.SearchQueries.Add(Query);


                    lbError.CssClass = "hc_success";
                    lbError.Text = "Query updated!";
                    lbError.Visible = true;
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "closewindow", "<script>window.opener.location.href = window.opener.location.href;window.close();</script>");
                }
            }
        }
        protected void csvLink_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["SearchDataSet"];
            System.Text.StringBuilder sb = Utils.ExportDataTableToCSV(ds.Tables[0]);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] contentBytes = encoding.GetBytes(sb.ToString());

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
            Response.ContentType = "application/text";
            //Fix for CR 5109 - Prabhu R S
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=\"SearchResults.csv\"; " +
                                  "size=" + sb.Length.ToString() + "; " +
                                  "creation-date=" + DateTime.Now.ToString("R") + "; " +
                                  "modification-date=" + DateTime.Now.ToString("R") + "; " +
                                  "read-date=" + DateTime.Now.ToString("R"));

            EnableViewState = false;
            Response.OutputStream.Write(contentBytes, 0, Convert.ToInt32(contentBytes.Length));
            Response.Flush();
            Response.End();
        }
        protected void excelLink_Click(object sender, EventArgs e)
        {
            if (dg.Visible == true)
            {
                DataTable objDTCustomFilters = selectedCustomFilters;
                DataTable objDTBusinessFilters = selectedBusinessFilters;
                Utils.ExportToExcelFromGrid(dg, "Search Results", "Search Results", Page, objDTBusinessFilters, objDTCustomFilters, "Search Results", SearchType);
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<TABLE border=\"1\">");
                DataSet ds = (DataSet)Session["SearchDataSet"];
                bool isHeader = true;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (isHeader)
                    {
                        sb.Append("<TR>");
                        foreach (DataColumn col in ds.Tables[0].Columns)
                            sb.Append("<TH style=\"font-weight:bold;\">" + col.Caption + "</TH>");
                        sb.Append("</TR>");
                        isHeader = false;
                    }

                    sb.Append("<TR>");
                    foreach (DataColumn col in ds.Tables[0].Columns)
                        sb.Append("<TD>" + UITools.HtmlEncode(row[col].ToString()) + "</TD>");
                    sb.Append("</TR>");
                }
                sb.Append("</TABLE>");
                string stream = sb.ToString();

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("content-disposition", "attachment; filename=SearchResults.xls");
                Response.ContentType = "application/vnd.ms-excel";
                //Fix for CR 5109 - Prabhu R S
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Length", System.Text.Encoding.Default.GetByteCount(stream).ToString());

                EnableViewState = false;
                Response.Write(stream);
                Response.End();
            }
        }

        protected void dg_GroupColumn(object sender, Infragistics.WebUI.UltraWebGrid.ColumnEventArgs e)
        {
            UpdateDataView();
        }
        #endregion

    }
}
