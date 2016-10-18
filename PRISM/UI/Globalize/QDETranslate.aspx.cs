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
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using System.Collections.Specialized;
using System.Text;
using System.Data.SqlTypes;
using System.Collections.Specialized;
using HyperComponents.Data.dbAccess;
using HyperComponents.Security.Cryptography;
#endregion

/*Created By:
 * 
 * Modified By: Jothi  Date:30/7/2007
 *Details of change:  QC defect #749. Searching the LastVisitedItem in all the relavant countries.  
 * 
 * 
 * 
 * 
 */
namespace HyperCatalog.UI.Globalize
{
    public partial class QDETranslate : HCPage
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            UITools.ShowSearchBar(this);
            /********** Start: Updated for QC defect #749 ****************/
            // Below line is commented for for QC defect #749 
            //UITools.FindUserFirstCulture(CultureType.Locale)
            bool found = SessionState.Culture != null && SessionState.Culture.Type == CultureType.Locale;
            ItemStatus status = ItemStatus.Unknown;
            bool isEligible = false;
            // Getting the Relavant Culture for current User and Item.
            if (!found)
            {
                using (Database dbObj = Utils.GetMainDB())
                {
                    
                    DatabaseQuery dbQuery = dbObj.CreateSPQuery("_User_GetItemRelavantCultures",
                    new SqlParameter("@UserId", SessionState.User.Id),
                    new SqlParameter("@ItemId", SessionState.User.LastVisitedItem));
                    dbQuery.AddOutputParameters(new SqlParameter("@CultureCode", SqlDbType.VarChar,10));
                    dbQuery.AddOutputParameters(new SqlParameter("@CultureName", SqlDbType.VarChar,80));
                    dbQuery.AddOutputParameters(new SqlParameter("@FallbackCode", SqlDbType.VarChar,10));
                    dbQuery.AddOutputParameters(new SqlParameter("@CultureTypeId", SqlDbType.TinyInt));
                    dbQuery.AddOutputParameters(new SqlParameter("@Sort", SqlDbType.SmallInt));
                    dbQuery.AddOutputParameters(new SqlParameter("@CultureId", SqlDbType.Int));
                    dbQuery.AddOutputParameters(new SqlParameter("@LanguageCode", SqlDbType.VarChar,3));
                    dbQuery.AddOutputParameters(new SqlParameter("@CountryCode", SqlDbType.VarChar,3));
                    dbQuery.AddOutputParameters(new SqlParameter("@Status", SqlDbType.Char,1));
                    dbQuery.Run();
                    
                    if (dbObj.LastError != null && dbObj.LastError.Length == 0)
                    {
                        
                      HyperCatalog.Business.Culture objCul =
                      new HyperCatalog.Business.Culture(
                      Convert.ToString(dbQuery.GetParameterValue("@CultureCode")).Trim(),
                      Convert.ToString(dbQuery.GetParameterValue("@CultureName")).Trim(),
                      Convert.ToString(dbQuery.GetParameterValue("@FallbackCode")).Trim(),
                      HyperCatalog.Business.Culture.GetCultureTypeFromInteger(Convert.ToInt32(dbQuery.GetParameterValue("@CultureTypeId"))),
                      Convert.ToInt32(dbQuery.GetParameterValue("@Sort")),
                      Convert.ToInt32(dbQuery.GetParameterValue("CultureId")),
                      Convert.ToString(dbQuery.GetParameterValue("@LanguageCode")).Trim(),
                      Convert.ToString(dbQuery.GetParameterValue("@CountryCode")).Trim());
                      status = Item.GetStatusFromString(Convert.ToString(dbQuery.GetParameterValue("@Status")).Trim().ToUpper());

                      found = true;
                      SessionState.Culture = objCul;
		if (objCul.Code == null ||objCul.Code.Length==0)
                          isEligible = false;
                      else isEligible = true;
                      dbObj.CloseConnection();
                    }
                    else
                    {
                        dbObj.CloseConnection();
                        throw new DataException("SQLDataAccessLayer: ItemEligibleInCountry-> " + dbObj.LastError);
                    }
                }
            }
            else
            {
                
                if (Item.GetByKey(SessionState.User.LastVisitedItem).GetWorkflowStatus(SessionState.Culture.Code) == ItemWorkflowStatus.Initialized.ToString()
                     || Item.GetByKey(SessionState.User.LastVisitedItem).GetWorkflowStatus(SessionState.Culture.Code) == ItemWorkflowStatus.AcquisitionCompleted.ToString()
                     || Item.GetByKey(SessionState.User.LastVisitedItem).GetWorkflowStatus(SessionState.Culture.Code) == ItemWorkflowStatus.Unknown.ToString())
                    isEligible = false;
                else isEligible = Item.IsEligible(SessionState.User.LastVisitedItem, SessionState.Culture.CountryCode, ref status);

            }
            if (!found)
            {
                // User has not locales in its scope
                UITools.DenyAccess(DenyMode.Standard);
            }
            else
            {
                //ItemStatus status = ItemStatus.Unknown;
                //bool isEligible = Item.IsEligible(SessionState.User.LastVisitedItem, SessionState.Culture.CountryCode, ref status);
                /********** End: Updated for QC defect #749 ****************/
                //Response.Write("Current IS " + SessionState.User.LastVisitedItem.ToString());
                //Response.Write(" - ELIGIBLE = " + isEligible.ToString() + "<br/>");
                if (isEligible)
                {
                    if (status == ItemStatus.Obsolete && !SessionState.User.ViewObsoletes)
                    {
                        SessionState.User.ViewObsoletes = true;
                        
                    }
                    SessionState.User.QuickSave();
                }
                else
                { // Find the first eligible Item for the user.
                    //  08/12/2009 QC 2692 - Modified by Sateesh -- The workflow Status 'R'/'C' only should be  visible in locales
                    SessionState.User.LastVisitedItem = SessionState.User.GetFirstCountryItem(SessionState.Culture.CountryCode, SessionState.Culture.Code);
                    //Response.Write("CHANGED TO " + SessionState.User.LastVisitedItem.ToString());
                    Item.IsEligible(SessionState.User.LastVisitedItem, SessionState.Culture.CountryCode, ref status);
                    if (status == ItemStatus.Obsolete && !SessionState.User.ViewObsoletes)
                    {
                        SessionState.User.ViewObsoletes = true;
                    }
                    SessionState.User.QuickSave();
                }
                qdeFrame.Attributes["src"] = Page.ResolveUrl("~/UI/Acquire/QDE/QDE_Main.aspx?g=2");
            }

        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}