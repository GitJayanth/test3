#region using
using System;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Xml;
using System.IO;
using System.Text;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.Shared
{
    [Serializable]
    public class SessionState
    {

        public static string AppName
        {
            get
            {
                if (HttpContext.Current.Session["AppName"] == null)
                {
                    HttpContext.Current.Session["AppName"] = SessionState.CacheParams["AppName"].Value;
                }
                return HttpContext.Current.Session["AppName"] != null ? HttpContext.Current.Session["AppName"].ToString() : "PRISM";
            }
        }
        public static ItemLevel SkuLevel
        {
            get
            {
                if (HttpContext.Current.Session["SkuLevel"] == null)
                {
                    HttpContext.Current.Session["SkuLevel"] = HyperCatalog.Business.ItemLevel.GetSkuLevel();
                }
                return (ItemLevel)HttpContext.Current.Session["SkuLevel"]; ;
            }
        }

        public static DataTable UIMenuItems
        {
            get
            {
                if (HttpContext.Current.Session["UIMenuItems"] == null)
                {
                    using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
                    {
                        UIMenuItems = dbObj.RunSPReturnDataSet("dbo._User_GetMenus",
                          new System.Data.SqlClient.SqlParameter("@UserId", SessionState.User.Id.ToString())).Tables[0];
                        dbObj.CloseConnection();
                    }
                }
                return (DataTable)HttpContext.Current.Session["UIMenuItems"]; ;
            }
            set
            {
                HttpContext.Current.Session["UIMenuItems"] = value;
            }
        }
        public static User User
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session["CurrentUser"] == null)
                    {
                        try
                        {
                            HyperCatalog.Business.User user = HyperCatalog.Business.User.GetByKey(HttpContext.Current.User.Identity.Name);
                            if (user != null)
                            {
                                HttpContext.Current.Session["CurrentUser"] = user;
                                SessionState.Culture = HyperCatalog.Shared.SessionState.MasterCulture;
                            }
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    if (HttpContext.Current.Session["CurrentUser"] != null)
                    {
                        return (User)HttpContext.Current.Session["CurrentUser"];
                    }
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
        }

        public static User EditedUser
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session["EditedUser"] != null)
                    {
                        return (User)HttpContext.Current.Session["EditedUser"];
                    }
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["EditedUser"] = value;
            }
        }

        public static Container QDEContainer
        {
            get
            {
                if (HttpContext.Current.Session["QDEContainer"] != null)
                {
                    return (Container)HttpContext.Current.Session["QDEContainer"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["QDEContainer"] = value;
            }
        }
        public static Chunk QDEChunk
        {
            get
            {
                if (HttpContext.Current.Session["QDEChunk"] != null)
                {
                    return (Chunk)HttpContext.Current.Session["QDEChunk"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["QDEChunk"] = value;
            }
        }

        public static string QDETab
        {
            get
            {
                if (HttpContext.Current.Session["QDETab"] != null)
                {
                    return HttpContext.Current.Session["QDETab"].ToString();
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["QDETab"] = value;
            }
        }

        public static HyperCatalog.Business.Culture MasterCulture
        {
            get
            {
                if (HttpContext.Current.Session["MasterCulture"] == null)
                {
                    HttpContext.Current.Session["MasterCulture"] = HyperCatalog.Business.Culture.GetMasterCulture();
                }
                if (HttpContext.Current.Session["MasterCulture"] != null)
                {
                    return (Culture)HttpContext.Current.Session["MasterCulture"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["MasterCulture"] = value;
            }
        }
        public static string GroupId
        {
            get
            {
                if (HttpContext.Current.Session["GroupId"] != null)
                {
                    return HttpContext.Current.Session["GroupId"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["GroupId"] = value;
            }
        }
        public static Culture Culture
        {
            get
            {
                if (HttpContext.Current.Session["Culture"] != null)
                {
                    return (Culture)HttpContext.Current.Session["Culture"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["Culture"] = value;
                /*try
                {
                  System.Globalization.CultureInfo ci0 = System.Globalization.CultureInfo.CreateSpecificCulture(value.Code);
                  if (ci0 != null)
                  {
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci0;
                  }
                  else
                  {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
                    ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                  }
                }
                catch
                {
                  System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
                  ci.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
                  System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                }*/
            }
        }

        public static Item CurrentItem
        {
            get
            {
                if (HttpContext.Current.Session["CurrentItem"] != null)
                {
                    return (Item)HttpContext.Current.Session["CurrentItem"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["CurrentItem"] = value;
                if (value != null)
                {
                    HttpContext.Current.Session["CurrentItemId"] = ((Item)value).Id;
                    HttpContext.Current.Session["CurrentItemIsUserItem"] = SessionState.User.HasItemInScope(((Item)value).Id);
                }
                else
                {
                    HttpContext.Current.Session["CurrentItemIsUserItem"] = false;
                    HttpContext.Current.Session["CurrentItemId"] = null;
                }
            }
        }
        public static bool CurrentItemIsUserItem
        {
            get { return HttpContext.Current.Session["CurrentItemIsUserItem"] != null ? (bool)HttpContext.Current.Session["CurrentItemIsUserItem"] : false; }
        }

        /// <summary>
        /// Specific Word - FilterWord
        /// </summary>
        public static string swFilterWord
        {
            get
            {
                if (HttpContext.Current.Session["swFilterWord"] != null)
                {
                    return HttpContext.Current.Session["swFilterWord"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["swFilterWord"] = value;
            }
        }
        /// <summary>
        /// Specific Word - Not approved
        /// </summary>
        public static bool swNotApproved
        {
            get
            {
                if (HttpContext.Current.Session["swNotApproved"] != null)
                {
                    return (bool)HttpContext.Current.Session["swNotApproved"];
                }
                return false;
            }
            set
            {
                HttpContext.Current.Session["swNotApproved"] = value;
            }
        }
        /// <summary>
        /// Specific Word - PageIndexWord
        /// </summary>
        public static string swPageIndexWord
        {
            get
            {
                if (HttpContext.Current.Session["swPageIndexWord"] != null)
                {
                    return HttpContext.Current.Session["swPageIndexWord"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["swPageIndexWord"] = value;
            }
        }
        /// <summary>
        /// TermBase - TermType
        /// </summary>
        public static string tTermType
        {
            get
            {
                if (HttpContext.Current.Session["tTermType"] != null)
                {
                    return HttpContext.Current.Session["tTermType"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["tTermType"] = value;
            }
        }
        /// <summary>
        /// TermBase - letter
        /// </summary>
        public static string tLetter
        {
            get
            {
                if (HttpContext.Current.Session["tLetter"] != null)
                {
                    return HttpContext.Current.Session["tLetter"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["tLetter"] = value;
            }
        }
        /// <summary>
        /// TermBase - FilterTerm
        /// </summary>
        public static string tFilterTerm
        {
            get
            {
                if (HttpContext.Current.Session["tFilterTerm"] != null)
                {
                    return HttpContext.Current.Session["tFilterTerm"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["tFilterTerm"] = value;
            }
        }
        /// <summary>
        /// TermBase - PageIndexTerm
        /// </summary>
        public static string tPageIndexTerm
        {
            get
            {
                if (HttpContext.Current.Session["tPageIndexTerm"] != null)
                {
                    return HttpContext.Current.Session["tPageIndexTerm"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["tPageIndexTerm"] = value;
            }
        }
        /// <summary>
        /// TM - FilterExpression
        /// </summary>
        public static string tmFilterExpression
        {
            get
            {
                if (HttpContext.Current.Session["tmFilterExpression"] != null)
                {
                    return HttpContext.Current.Session["tmFilterExpression"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["tmFilterExpression"] = value;
            }
        }

        /// <summary>
        /// TM - PageIndexExpression
        /// </summary>
        public static string tmPageIndexExpression
        {
            get
            {
                if (HttpContext.Current.Session["tmPageIndexExpression"] != null)
                {
                    return HttpContext.Current.Session["tmPageIndexExpression"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["tmPageIndexExpression"] = value;
            }
        }

        public static DataTable TodaysActivity
        {
            get
            {
                if (HttpContext.Current.Cache["TodaysActivity"] != null)
                {
                    return (DataTable)HttpContext.Current.Cache["TodaysActivity"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Cache.Add("TodaysActivity", value, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
            }
        }

        public static DataSet ComingTRs
        {
            get
            {
                if (HttpContext.Current.Session["ComingTRs"] != null)
                {
                    return (DataSet)HttpContext.Current.Session["ComingTRs"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["ComingTRs"] = value;
            }
        }
        public static DataSet UserPopupMenus
        {
            get
            {
                if (HttpContext.Current.Session["UserPopupMenus"] != null)
                {
                    return (DataSet)HttpContext.Current.Session["UserPopupMenus"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["UserPopupMenus"] = value;
            }
        }


        public static HyperCatalog.Shared.Defs.Clipboard Clipboard
        {
            get
            {
                if (HttpContext.Current.Session["Clipboard"] == null)
                {
                    HttpContext.Current.Session["Clipboard"] = new HyperCatalog.Shared.Defs.Clipboard();
                }
                return (HyperCatalog.Shared.Defs.Clipboard)HttpContext.Current.Session["Clipboard"];
            }
            set
            {
                HttpContext.Current.Session["Clipboard"] = value;
            }
        }

        // Add feature ContainerId (CHARENSOL Mickael 14/10/2005)
        public static string FeatureContainerId
        {
            get
            {
                if (HttpContext.Current.Session["FeatureContainerId"] != null)
                {
                    return HttpContext.Current.Session["FeatureContainerId"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["FeatureContainerId"] = value;
            }
        }

        // Add notification date (CHARENSOL Mickael 16/06/2006)
        public static DateTime? NotificationDate
        {
            get
            {
                if (HttpContext.Current.Session["NotificationDate"] != null)
                {
                    return (DateTime?)HttpContext.Current.Session["NotificationDate"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["NotificationDate"] = value;
            }
        }

        /// <summary>
        /// Treeview is showing all items
        /// </summary>
        public static bool TVAllItems
        {
            get
            {
                if (HttpContext.Current.Session["TVAllItems"] != null)
                {
                    return (bool)HttpContext.Current.Session["TVAllItems"];
                }
                return false;
            }
            set
            {
                HttpContext.Current.Session["TVAllItems"] = value;
            }
        }

        // Add CompanyName (Venkata Jayanth 9/7/2016)
        public static string CompanyName
        {
            get
            {
                if (HttpContext.Current.Session["CompanyName"] != null)
                {
                    return HttpContext.Current.Session["CompanyName"].ToString();
                }
                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["CompanyName"] = value;
            }
        }

        public static HyperCatalog.Business.ItemLevelList ItemLevels
        {
            get
            {
                if (HttpContext.Current.Cache["ItemLevels"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("ItemLevels", HyperCatalog.Business.ItemLevel.GetAll(), null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (HyperCatalog.Business.ItemLevelList)HttpContext.Current.Cache["ItemLevels"];
            }
        }

        public static HyperCatalog.Business.ApplicationParameterCollection CacheParams
        {
            get
            {
                if (HttpContext.Current.Cache["AppParameters"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HyperCatalog.Business.ApplicationSettings.Reset();
                    HttpContext.Current.Cache.Add("AppParameters", HyperCatalog.Business.ApplicationSettings.Parameters, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                    HttpContext.Current.Cache.Add("AppComponents", HyperCatalog.Business.ApplicationSettings.Components, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }

                return (HyperCatalog.Business.ApplicationParameterCollection)HttpContext.Current.Cache["AppParameters"];
            }
        }
        public static HyperCatalog.Business.ApplicationComponentCollection CacheComponents
        {
            get
            {
                if (HttpContext.Current.Cache["AppComponents"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HyperCatalog.Business.ApplicationSettings.Reset();
                    HttpContext.Current.Cache.Add("AppParameters", HyperCatalog.Business.ApplicationSettings.Parameters, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                    HttpContext.Current.Cache.Add("AppComponents", HyperCatalog.Business.ApplicationSettings.Components, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (HyperCatalog.Business.ApplicationComponentCollection)HttpContext.Current.Cache["AppComponents"];
            }
        }
        public static string AppDBVersion
        {
            get
            {
                if (HttpContext.Current.Cache["AppDBVersion"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HyperCatalog.DataAccessLayer.SqlDataAccessLayer db = new HyperCatalog.DataAccessLayer.SqlDataAccessLayer();
                    HttpContext.Current.Cache.Add("AppDBVersion", db.GetComponentVersion(HyperCatalog.DataAccessLayer.SqlDataAccessLayer.appSettings_ConnectionString), null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                    db = null;
                }
                return HttpContext.Current.Cache["AppDBVersion"].ToString();
            }
        }
        public static UserList AppUsers
        {
            get
            {
                if (HttpContext.Current.Cache["AppUsers"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppUsers", HyperCatalog.Business.User.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (UserList)HttpContext.Current.Cache["AppUsers"];
            }
        }
        public static InputFormList AppInputForms
        {
            get
            {
                if (HttpContext.Current.Cache["AppInputForms"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppInputForms", HyperCatalog.Business.InputForm.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (InputFormList)HttpContext.Current.Cache["AppInputForms"];
            }
        }
        public static ContainerList AppContainers
        {
            get
            {
                if (HttpContext.Current.Cache["AppContainers"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppContainers", HyperCatalog.Business.Container.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (ContainerList)HttpContext.Current.Cache["AppContainers"];
            }
        }
        public static ContainerGroupList AppContainersGroups
        {
            get
            {
                if (HttpContext.Current.Cache["AppContainersGroups"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppContainersGroups", HyperCatalog.Business.ContainerGroup.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (ContainerGroupList)HttpContext.Current.Cache["AppContainersGroups"];
            }
        }
        public static OrganizationList AppOrganizations
        {
            get
            {
                if (HttpContext.Current.Cache["AppOrganizations"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppOrganizations", HyperCatalog.Business.Organization.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (OrganizationList)HttpContext.Current.Cache["AppOrganizations"];
            }
        }
        public static RoleList AppRoles
        {
            get
            {
                if (HttpContext.Current.Cache["AppRoles"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppRoles", HyperCatalog.Business.Role.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (RoleList)HttpContext.Current.Cache["AppRoles"];
            }
        }
        public static InheritanceMethodList AppInheritanceMethods
        {
            get
            {
                if (HttpContext.Current.Cache["AppInheritanceMethods"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppInheritanceMethods", HyperCatalog.Business.InheritanceMethod.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (InheritanceMethodList)HttpContext.Current.Cache["AppInheritanceMethods"];
            }
        }
        public static LookupGroupList AppLookupGroups
        {
            get
            {
                if (HttpContext.Current.Cache["AppLookupGroups"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppLookupGroups", HyperCatalog.Business.LookupGroup.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (LookupGroupList)HttpContext.Current.Cache["AppLookupGroups"];
            }
        }
        public static ContainerTypeList AppContainerTypes
        {
            get
            {
                if (HttpContext.Current.Cache["AppContainerTypes"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppContainerTypes", HyperCatalog.Business.ContainerType.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (ContainerTypeList)HttpContext.Current.Cache["AppContainerTypes"];
            }
        }
        public static DataTypeList AppDataTypes
        {
            get
            {
                if (HttpContext.Current.Cache["AppDataTypes"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    HttpContext.Current.Cache.Add("AppDataTypes", HyperCatalog.Business.DataType.GetAll(), null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration,
                      System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                return (DataTypeList)HttpContext.Current.Cache["AppDataTypes"];
            }
        }
        public static DataTable AppContainerSegments
        {
            get
            {
                if (HttpContext.Current.Cache["AppContainerSegments"] == null) // If the parameters are not yet in cache (IIS restart)
                {
                    using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
                    {
                        HttpContext.Current.Cache["AppContainerSegments"] = dbObj.RunSQLReturnDataSet("SELECT SegmentId, SegmentName FROM ContainerSegments", "ContainerSegments").Tables[0];
                    }
                }
                return (DataTable)HttpContext.Current.Cache["AppContainerSegments"];
            }
        }
        public static void ClearAppUsers() { HttpContext.Current.Cache.Remove("AppUsers"); }
        public static void ClearAppDBVersion() { HttpContext.Current.Cache.Remove("AppDBVersion"); }
        public static void ClearAppComponents() { HttpContext.Current.Cache.Remove("AppComponents"); }
        public static void ClearAppParameters() { HttpContext.Current.Cache.Remove("AppParameters"); }
        public static void ClearAppInputForms() { HttpContext.Current.Cache.Remove("AppInputForms"); }
        public static void ClearAppContainers() { HttpContext.Current.Cache.Remove("AppContainers"); }
        public static void ClearAppOrganizations() { HttpContext.Current.Cache.Remove("AppOrganizations"); }
        public static void ClearAppRoles() { HttpContext.Current.Cache.Remove("AppRoles"); }
        public static void ClearAppContainerGroups() { HttpContext.Current.Cache.Remove("AppContainersGroups"); }
        public static void ClearAppInheritanceMethods() { HttpContext.Current.Cache.Remove("AppInheritanceMethods"); }
        public static void ClearAppLookupGroups() { HttpContext.Current.Cache.Remove("AppLookupGroups"); }
        public static void ClearAppContainerTypes() { HttpContext.Current.Cache.Remove("AppContainerTypes"); }
        public static void ClearAppDataTypes() { HttpContext.Current.Cache.Remove("AppDataTypes"); }
        public static bool CheckVersion()
        {
            return HyperCatalog.Business.Settings.DBVersion.CompareTo(new Version(SessionState.AppDBVersion)) == 0;
        }

        private SessionState()
        {
        }
    }
}
