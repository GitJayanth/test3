/// Change History
/// Modified 22-Oct-2007 Vinay Bhandari
///             - Changed to disable the exclude check box is parent is excluded.
/// Modified 31-Jan-2013 Dylan Fowles
///             - Modified for Links Management Sorting and Recommending
/// Modified 06-Dec-2013 Rekha Thomas
///             - Disabled Add Components, Apply Changes, Delete buttons for Bundles in Regional and Country level. Enabled only for ww-en.
/// Modified 03-Jan-2014 Rekha Thomas
///             - CR 7055, fix for "Can't eval uwToolbar_Click(oControl, ig_fireEvent.arguments[2], ig_fireEvent.arguments[3]) under links tab.

#region uses
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.WebDataInput;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
#endregion

/// <summary>
/// Links_list displays the grid containing the links
/// </summary>
public partial class Links_list : HCPage
{
    #region Declarations
    private bool bLinkFrom = true; // true: display the list of companion, false: display the list of hardware
    private HyperCatalog.Business.LinkType linkType = null;
    private int AccessoriesId, SuppliesId, ServicesId, CrossSellId, BundlesId; // Specific link types 

    private int inhLCount, notInhLCount; // count of links (inherited or not inherited)
    private int inhRLCount, notInhRLCount; // count of recommended links (inherited and not inherited)

    private int inhSCount, notInhSCount;
    private int inhSRCount, notInhSRCount;

    private string currentGroup = string.Empty;
    private string currentLinkGroup = string.Empty;
    private int groupCount = 0;
    private bool viewNonInherited = false;
    private bool viewAtCountryLevel = false;
    private bool viewDSRLinks = false;
    private bool viewSCRLinks = false;
    #endregion

    #region Constantes
    //Code modified for Links Requirement (PR668013) - to update the toolbar button text by Prachi on 10th Dec 2012
    private const string CST_COMPANION = "Add Hosts";
    private const string CST_HARDWARE = "Add Companions";
    private const string CST_HARDWARE_TOOLTIP = "To display the hosts list";
    private const string CST_COMPANION_TOOLTIP = "To display the companions list";
    private const string CST_COMPANION_LIST = "List Companions";
    private const string CST_HARDWARE_LIST = "List Hosts";
    private const int CST_LINKTYPE_CAREPACKS = 2;
    private const string CST_LINKTYPE_CAREPACKS_NAME = "Services";
    private const int CST_LINKTYPE_CROSSSELL = 4;
    private const string SP_LINK_UPDSCRECOMMENDED = "dbo._Link_UpdSCRecommended";
    #endregion

    #region Enum types
    private enum LinkFromTo
    {
        FROM = 1,
        TO = 2,
        FROMTO = 3
    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
        {
            try
            {
                QDEUtils.GetItemIdFromRequest();
                QDEUtils.UpdateCultureCodeFromRequest();
                if (Request["t"] != null)
                {
                    int linkTypeId = Convert.ToInt32(Request["t"]);
                    linkType = HyperCatalog.Business.LinkType.GetByKey(linkTypeId);
                }
                if (Request["f"] != null)
                    bLinkFrom = ((Request["f"].ToString()).Equals("1"));

                // Retrieve link types
                RetrieveLinkType();

                dg.Visible = linkType.IsBidirectional;
                dgNoBidi.Visible = !linkType.IsBidirectional;

                if (!Page.IsPostBack || Request["action"] != string.Empty)
                    UpdateDataView();
                // Start Fix for CR7055 by Rekha Thomas. Can't eval() error
                else
                {
                    string varLinkFromTo = "var bLinkFrom=0;";
                    if (bLinkFrom)
                        varLinkFromTo = "var bLinkFrom=1;";

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "linkVars", "<script>" + varLinkFromTo + " var itemId=" + SessionState.CurrentItem.Id.ToString() + "; var lType=" + linkType.Id.ToString() + ";</script>");
                }
                // End Fix for CR7055 by Rekha Thomas. Can't eval() error
            }
            catch (FormatException fe)
            {
                lbError.Text = fe.ToString();
                lbError.CssClass = "hc_error";
                lbError.Visible = true;
                return;
            }
        }
        else
        {
            UITools.DenyAccess(DenyMode.Frame);
        }
        dg.EnableViewState = dg.Visible;
        dgNoBidi.EnableViewState = dgNoBidi.Visible;
        if (!dg.Visible)
        {
            dg.DisplayLayout.ReadOnly = ReadOnly.LevelZero;
        }
        if (!dgNoBidi.Visible)
        {
            dgNoBidi.DisplayLayout.ReadOnly = ReadOnly.LevelZero;
        }
    }
    private void UpdateDataView()
    {
        #region "Hide all components"
        // Hide buttons Delete and Apply changes
        UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
        UITools.HideToolBarButton(uwToolbar, "Save");
        UITools.HideToolBarSeparator(uwToolbar, "ExportSep");
        UITools.HideToolBarButton(uwToolbar, "Export");

        // hide grids
        dg.Visible = false;
        dgNoBidi.Visible = false;
        //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
        tr1.Visible = false;

        // hide labels
        lbError.Visible = false;
        lbResult.Visible = false;
        #endregion

        // check if companion or hardware exists 
        if (CheckLinks())
        {
            if (SessionState.CurrentItem != null && linkType != null)
            {
                HyperCatalog.Business.LinkList links = null;
                try
                {
                    // Display columns (hardware or companion)
                    updateColumns();

                    if (!linkType.IsBidirectional)
                    {
                        #region "Display links no bidirectional (CrossSell, Bundles)"
                        // Fix for QC 1118
                        //links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom);
                        if (!viewAtCountryLevel)
                        {
                            if (viewNonInherited)
                                links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom, false, false); // only the non inherited links
                            else
                                links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom); // all links (inherited and non inherited)
                        }
                        else
                            links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom, true); // only the links at country level

                        // all links 
                        if (links != null)
                        {
                            if (links.Count > 0)
                            {
                                links.Sort("SubItemSort");
                                dgNoBidi.DataSource = links;
                                Utils.InitGridSort(ref dgNoBidi, false);
                                dgNoBidi.DataBind();
                                // Sorting is only available when viewing DSR Links
                                if (viewDSRLinks == true)
                                    Utils.EnableIntelligentSort(ref dgNoBidi, Convert.ToInt32(txtSortColPos.Value));

                                // Sorting is now available when viewing SCR Links added by Charu Kalra
                                if (viewSCRLinks == true)
                                    Utils.EnableIntelligentSort(ref dgNoBidi, Convert.ToInt32(txtSortColPos.Value));


                                dgNoBidi.DisplayLayout.AllowSortingDefault = AllowSorting.Yes;

                                dgNoBidi.Visible = true;
                                //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
                                tr1.Visible = true;
                                tdList.InnerText = "Companions List";
                            }
                            else
                            {
                                // No links
                                lbResult.CssClass = "hc_success";
                                lbResult.Text = "Empty";
                                lbResult.Visible = true;
                                dgNoBidi.Visible = false;
                                //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
                                tr1.Visible = false;
                            }
                        }
                        else
                        {
                            // Error
                            lbError.CssClass = "hc_error";
                            lbError.Text = HyperCatalog.Business.Item.LastError;
                            lbError.Visible = true;
                            dgNoBidi.Visible = false;
                            //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
                            tr1.Visible = false;
                        }
                        #endregion
                    }
                    else
                    {
                        #region "Display links bidirectional (Accessories, Supplies, Services, ...)"
                        // get all links not inherited for this link type and item
                        if (!viewAtCountryLevel)
                        {
                            if (viewNonInherited)
                                links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom, false, false); // only the non inherited links
                            else
                                links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom); // all links (inherited and non inherited)
                        }
                        else
                            links = SessionState.CurrentItem.GetLinks(SessionState.Culture.Code, linkType.Id, bLinkFrom, true); // only the links at country level

                        // Display grid containing links
                        if (links != null)
                        {
                            if (links.Count > 0)
                            {
                                // Order by SubItemSort if viewing DSRLinks, else SubItemSKU
                                if (bLinkFrom && !viewDSRLinks)
                                    links.Sort("SubItemSort"); //Sorting will be done by ItemSort
                                //links.Sort("SubItemSKU");
                                else if (bLinkFrom && viewDSRLinks)
                                {
                                    dg.Columns.Band.SortedColumns.Clear();
                                    for (int i = 0; i < links.Count; )
                                    {
                                        if (links[i].Recommended == false)
                                            links.Remove(i);
                                        else
                                            i++;
                                    }
                                    links = SortLinks(links);
                                }
                                //-------------------------------------
                                if (bLinkFrom && !viewSCRLinks)
                                    links.Sort("SubItemSort"); //Sorting will be done by ItemSort
                                // links.Sort("SubItemSKU");
                                else if (bLinkFrom && viewSCRLinks)
                                {
                                    dg.Columns.Band.SortedColumns.Clear();
                                    for (int i = 0; i < links.Count; )
                                    {
                                        if (links[i].SCRecommended == false)
                                            links.Remove(i);
                                        else
                                            i++;
                                    }
                                    links = SortLinks(links);
                                }
                                //----------------------------------------
                                dg.DataSource = links;
                                Utils.InitGridSort(ref dg, false);
                                dg.DataBind();

                                if (!viewAtCountryLevel && viewDSRLinks == true)
                                {
                                    Utils.EnableIntelligentSort(ref dg, Convert.ToInt32(txtSortColPos.Value));
                                }
                                //Code added for SCR Links Sorting by charu kalra
                                else if (!viewAtCountryLevel && viewSCRLinks == true)
                                {
                                    Utils.EnableIntelligentSort(ref dg, Convert.ToInt32(txtSortColPos.Value));
                                }
                                else
                                {
                                    dg.Columns.Remove(dg.Columns.FromKey("s_a"));
                                }
                                //Code modified for Links Requirement (PR668013) - Links Tab Grid Format to allow column sorting by Prachi
                                //dg.DisplayLayout.AllowSortingDefault = AllowSorting.No; // no sort
                                dg.DisplayLayout.AllowSortingDefault = (!viewDSRLinks ? AllowSorting.Yes : AllowSorting.No);
                                //Code added for SCR Links Sorting by charu kalra
                                dg.DisplayLayout.AllowSortingDefault = (!viewSCRLinks ? AllowSorting.Yes : AllowSorting.No);

                                dg.Visible = true;


                                //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
                                tr1.Visible = true;
                            }
                            else
                            {
                                // No links
                                lbResult.CssClass = "hc_success";
                                lbResult.Text = "No records";
                                lbResult.Visible = true;
                                dg.Visible = false;
                                //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
                                tr1.Visible = false;
                            }
                        }
                        else
                        {
                            // Error
                            lbError.CssClass = "hc_error";
                            lbError.Text = HyperCatalog.Business.Item.LastError;
                            lbError.Visible = true;
                            dg.Visible = false;
                            //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
                            tr1.Visible = false;
                        }
                        #endregion
                    }

                    // Update count
                    updateCount();
                    // Update buttons in tool bar
                    updateButtons();
                    // Update toolbar in footer
                    updateToolBarStat();
                    // Update title for tab
                    updateTitleTab();

                    // Init vars (linkFrom, itemId, linkTypeId)
                    string varLinkFromTo = "var bLinkFrom=0;";
                    if (bLinkFrom)
                        varLinkFromTo = "var bLinkFrom=1;";

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "linkVars", "<script>" + varLinkFromTo + " var itemId=" + SessionState.CurrentItem.Id.ToString() + "; var lType=" + linkType.Id.ToString() + ";</script>");
                }
                finally
                {
                    if (links != null) links.Dispose();
                }
            }
        }
        else
        {
            dg.Visible = dgNoBidi.Visible = false;
            uwToolbar.Visible = false;
            //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
            tr1.Visible = false;
            lbError.CssClass = "hc_error";
            lbError.Text = "This link type is not correctly setted.";
            lbError.Visible = true;
        }
    }

    #region Persistence methods
    private void Delete()
    {
        lbError.Visible = false;

        if (SessionState.CurrentItem != null)
        {
            UltraWebGrid grid = null;
            if (dg.Visible)
                grid = dg;
            else if (dgNoBidi.Visible)
                grid = dgNoBidi;

            //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
            tr1.Visible = true;

            if (grid != null)
            {
                // Delete multiple containers on the grid
                int index = -1;
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    if (grid.Rows[i].Cells.FromKey("ItemId") != null && grid.Rows[i].Cells.FromKey("ItemId").Value != null)
                    {
                        index = index + 1;
                        TemplatedColumn col = (TemplatedColumn)grid.Rows[i].Cells.FromKey("Select").Column;
                        CheckBox cb = (CheckBox)((CellItem)col.CellItems[index]).FindControl("g_sd");

                        if (cb.Enabled && cb.Checked)
                        {

                            // for links To
                            System.Int64 mainId = -1;
                            System.Int64 subId = -1;
                            if (!bLinkFrom) // for links From
                            {
                                mainId = Convert.ToInt64(grid.Rows[i].Cells.FromKey("ItemId").Value);
                                subId = SessionState.CurrentItem.Id;
                            }
                            else // for links To
                            {
                                mainId = SessionState.CurrentItem.Id;
                                subId = Convert.ToInt64(grid.Rows[i].Cells.FromKey("SubItemId").Value);
                            }
                            // Get country code
                            string countryCode = grid.Rows[i].Cells.FromKey("CountryCode").Value.ToString();

                            //get link
                            int newInt = 0;
                            newInt = linkType.Id;
                            Link lnk = Link.GetByKey(mainId, subId, linkType.Id, countryCode);
                            if (lnk != null)
                            {
                                if (!lnk.Delete(HyperCatalog.Shared.SessionState.User.Id))
                                {
                                    lbError.Text = Link.LastError;
                                    lbError.CssClass = "hc_error";
                                    lbError.Visible = true;

                                    return;
                                }
                            }
                        }
                    }
                }

                UpdateDataView();
            }
        }
    }

    private void ResumeInheritance()
    {
        lbError.Visible = false;
        if (SessionState.CurrentItem != null)
        {
            // Delete the clear blindly all links (eligible & ineligible links) 
            if (!Link.ResumeInheritance(Convert.ToInt64(SessionState.CurrentItem.Id.ToString()), linkType.Id, SessionState.Culture.CountryCode.ToString(), Convert.ToInt32(SessionState.User.Id.ToString())))
            {
                lbError.Text = Link.LastError;
                lbError.CssClass = "hc_error";
                lbError.Visible = true;
                return;
            }
            UpdateDataView();
        }
    }

    private void Apply()
    {
        lbError.Visible = false;
        viewDSRLinks = uwToolbar.Items.FromKeyButton("DSRLinks").Selected;

        viewSCRLinks = uwToolbar.Items.FromKeyButton("SCRLinks").Selected;



        #region "Link exclusions"
        if ((dg != null && dg.Visible) || (dgNoBidi != null && dgNoBidi.Visible))
        {
            //Code added for Links Requirement (PR668013) - to add a title above webgrid by Prachi on 10th Dec 2012
            tr1.Visible = true;

            // build link exclusions
            List<LinkExclusion> exclusionList = buildExclusionList();

            // Update link exclusions
            if (exclusionList.Count > 0)
            {
                // save new exclusions
                if (!HyperCatalog.Business.LinkExclusion.SaveExclusions(SessionState.User.Id, exclusionList))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = HyperCatalog.Business.LinkExclusion.LastError;
                    lbError.Visible = true;
                    return;
                }
            }
        }



        #endregion
        #region "Recommended links"
        if (dg != null && dg.Visible)
        {
            // build recommended list
            string recommendedList = buildRecommendedList();

            string screcommendedList = buildSCRecommendedList();
            // Update recommended links
            if (recommendedList.Length > 0)
            {
                // save new recommended links
                if (!HyperCatalog.Business.Link.SaveNewRecommended(recommendedList.ToString(), SessionState.User.Id))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = HyperCatalog.Business.Link.LastError;
                    lbError.Visible = true;
                    return;
                }
            }

            if (screcommendedList.Length > 0)
            {
                // save new screcommended links
                if (!HyperCatalog.Business.Link.SaveNewSCRecommended(screcommendedList.ToString(), SessionState.User.Id))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = HyperCatalog.Business.Link.LastError;
                    lbError.Visible = true;
                    return;
                }
            }

        }
        #endregion
        #region "Sort links"
        //Sort only occurs when viewing DSR Links
        if (((dg != null && dg.Visible) || (dgNoBidi != null && dgNoBidi.Visible)) && viewDSRLinks)
        {
            string sortList = buildSortList();

            // Update link exclusions
            if (sortList.Length > 0)
            {
                // save new sort links
                if (!HyperCatalog.Business.Link.SaveNewSort(sortList.ToString(), SessionState.User.Id))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = HyperCatalog.Business.Link.LastError;
                    lbError.Visible = true;
                    return;
                }
            }
        }

        if (((dg != null && dg.Visible) || (dgNoBidi != null && dgNoBidi.Visible)) && viewSCRLinks)
        {
            string sortList = buildSortList();

            // Update link exclusions
            if (sortList.Length > 0)
            {
                // save new sort links
                if (!HyperCatalog.Business.Link.SaveNewSort(sortList.ToString(), SessionState.User.Id))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = HyperCatalog.Business.Link.LastError;
                    lbError.Visible = true;
                    return;
                }
            }
        }
        #endregion

        UpdateDataView();

        lbError.CssClass = "hc_success";
        lbError.Text = "New data saved!";
        lbError.Visible = true;

        // Update toolbar in footer
        updateCount();
        updateToolBarStat();
    }
    #endregion
    #region Event methods
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        // Update exclusion, recommended checkbox
        updateCheckbox(e.Row);

        // Retrieve country code
        if (e.Row.Cells.FromKey("FallbackCountryCode") != null && e.Row.Cells.FromKey("ImageCountry") != null)
        {
            string countryCode = string.Empty;
            if (e.Row.Cells.FromKey("FallbackCountryCode") != null && e.Row.Cells.FromKey("FallbackCountryCode").Value != null)
                countryCode = e.Row.Cells.FromKey("FallbackCountryCode").ToString();

            // Update image for current country
            if (countryCode.Length > 0 && e.Row.Cells.FromKey("ImageCountry") != null)
                e.Row.Cells.FromKey("ImageCountry").Text = "<img title=\"" + countryCode + "\" src=\"/hc_v4/img/flags/" + countryCode.ToLower() + ".gif\">";
        }

        // Inherited link
        if (e.Row.Cells.FromKey("IsInherited") != null && e.Row.Cells.FromKey("IsInherited").Value != null)
        {
            if (Convert.ToBoolean(e.Row.Cells.FromKey("IsInherited").Value))
            {
                e.Row.Cells.FromKey("InheritedLevel").Style.Font.Italic = true;
                e.Row.Cells.FromKey("ItemName").Style.Font.Italic = true;
                e.Row.Cells.FromKey("ItemName").Style.Wrap = true;
                e.Row.Cells.FromKey("ItemSku").Style.Font.Italic = true;
                e.Row.Cells.FromKey("ItemSku").Style.Wrap = true;
                e.Row.Cells.FromKey("Class").Style.Font.Italic = true;
                e.Row.Cells.FromKey("Class").Style.Wrap = true;
                //Code added for Links Requirement (PR668013) - Links Tab Grid Format to add Effective Date column by Prachi
                e.Row.Cells.FromKey("EffectiveDate").Style.Font.Italic = true;
                e.Row.Cells.FromKey("EffectiveDate").Style.Wrap = true;
                if (e.Row.Cells.FromKey("Family") != null)
                {
                    e.Row.Cells.FromKey("Family").Style.Font.Italic = true;
                    e.Row.Cells.FromKey("Family").Style.Wrap = true;
                }
            }
        }

        if (dg.Visible)
        {
            // Link group
            string linkGroup = e.Row.Cells.FromKey("Family").Text;
            if (currentLinkGroup != linkGroup)
            {
                currentLinkGroup = linkGroup;
                groupCount++;
            }
        }
    }
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "delete")
        {
            Delete();
        }
        if (btn == "resumeinheritance")
        {
            ResumeInheritance();
        }
        else if (btn == "save")
        {
            Apply();
        }
        else if (btn == "export")
        {
            //To export Cross sell Bug #7336 -- Prabhu
            if (dgNoBidi.Visible)
                Utils.ExportToExcel(dgNoBidi, linkType.Name + SessionState.CurrentItem.Name.ToString(), linkType.Name + SessionState.CurrentItem.Name.ToString());
            else
                Utils.ExportToExcel(dg, linkType.Name + SessionState.CurrentItem.Name.ToString(), linkType.Name + SessionState.CurrentItem.Name.ToString());

        }
        else if (btn == "linktofrom")
        {
            string f = "1";
            if (bLinkFrom)
                f = "0";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LinkFromTo", "<script>DisplayLinkFromOrLinkTo(" + SessionState.CurrentItem.Id.ToString() + ", " + linkType.Id.ToString() + ", " + f + ");</script>");
        }
        else if (btn == "noninherited")
        {
            viewNonInherited = be.Button.Selected;
            UpdateDataView();
        }
        else if (btn == "atcountrylevel")
        {
            viewAtCountryLevel = be.Button.Selected;
            UpdateDataView();
        }
        // Filters DSR Links
        else if (btn == "dsrlinks")
        {
            viewDSRLinks = be.Button.Selected;
            UpdateDataView();
        }
        // Filter SCR Links
        else if (btn == "scrlinks")
        {
            viewSCRLinks = be.Button.Selected;
            UpdateDataView();
        }
    }
    #endregion

    #region Private methods
    private bool CheckLinks()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._LinkType_GetItems", "Items",
              new SqlParameter("@LinkTypeId", linkType.Id),
             new SqlParameter("@LinkFromTo", Convert.ToByte(bLinkFrom)),
              new SqlParameter("@Company", SessionState.CompanyName)))
            {
                dbObj.CloseConnection();
                if (dbObj.LastError != null && dbObj.LastError.Length > 0)
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = dbObj.LastError;
                    lbError.Visible = true;
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        bool isOk = (ds.Tables[0].Rows.Count > 0);
                        ds.Dispose();
                        return isOk;
                    }
                }
            }
        }
        return false;
    }

    private void updateCount()
    {
        // init vars
        inhLCount = notInhLCount = inhRLCount = notInhRLCount = 0;

        inhSCount = notInhSCount = inhSRCount = notInhSRCount = 0;

        UltraWebGrid grid = null;
        if (dg != null && dg.Visible)
            grid = dg;
        else if (dgNoBidi != null && dgNoBidi.Visible)
            grid = dgNoBidi;

        if (grid != null)
        {
            // loops on link list
            foreach (UltraGridRow r in grid.Rows)
            {
                if (Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value) == true)
                {
                    inhLCount++;
                    inhSCount++;
                }
                else
                {
                    notInhLCount++;
                    notInhSCount++;
                }
                if (r.Cells.FromKey("IsRecommended") != null && Convert.ToBoolean(r.Cells.FromKey("IsRecommended").Value) == true)
                {
                    if (Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value) == true)
                        inhRLCount++;
                    else
                        notInhRLCount++;
                }
                if (r.Cells.FromKey("IsSCRecommended") != null && Convert.ToBoolean(r.Cells.FromKey("IsSCRecommended").Value) == true)
                {
                    if (Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value) == true)
                        inhSRCount++;
                    else
                        notInhSRCount++;
                }
            }
        }
    }
    private void updateButtons()
    {
        bool hasButton = false;

        if (linkType.IsBidirectional)
            hasButton = true;

        // Hide all buttons by default
        UITools.HideToolBarSeparator(uwToolbar, "LinkToFromSep");
        UITools.HideToolBarButton(uwToolbar, "LinkToFrom");
        //Code commented for Links Requirement (PR668013) - to modify the toolbar by Prachi on 10th Dec 2012
        //UITools.HideToolBarLabel(uwToolbar, "Title");
        UITools.HideToolBarSeparator(uwToolbar, "NonInheritedSep");
        UITools.HideToolBarButton(uwToolbar, "NonInherited");
        UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
        UITools.HideToolBarButton(uwToolbar, "Delete");
        //Code commented for Links Requirement (PR668013) - to modify the toolbar by Prachi on 10th Dec 2012
        //UITools.HideToolBarSeparator(uwToolbar, "AddSep");
        UITools.HideToolBarButton(uwToolbar, "Add");
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
        UITools.HideToolBarButton(uwToolbar, "Save");
        UITools.HideToolBarSeparator(uwToolbar, "ExportSep");
        UITools.HideToolBarButton(uwToolbar, "Export");
        UITools.HideToolBarSeparator(uwToolbar, "AtCountryLevelSep");
        UITools.HideToolBarButton(uwToolbar, "AtCountryLevel");
        UITools.HideToolBarSeparator(uwToolbar, "DSRLinksSep");
        UITools.HideToolBarButton(uwToolbar, "DSRLinks");
        UITools.HideToolBarSeparator(uwToolbar, "ResumeInheritanceSep");
        UITools.HideToolBarButton(uwToolbar, "ResumeInheritance");
        UITools.HideToolBarSeparator(uwToolbar, "SCRLinksSep");
        UITools.HideToolBarButton(uwToolbar, "SCRLinks");


        #region "LinkFromTo button"
        if ((linkType != null) && (SessionState.CurrentItem != null) && (linkType.IsBidirectional))
        {
            //Code modified for Links Requirement (PR668013) - for Links Tab UI changes by Prachi on 20th Dec 2012
            int iLinkFromTo = SessionState.CurrentItem.IsLinkFromTo(linkType.Id); // 1 : hardware, 2 : companion, 3 : hardware or companion
            if (iLinkFromTo == (int)LinkFromTo.FROMTO)
            {
                uwToolbar.Items.FromKeyButton("LinkToFrom").DefaultStyle.Width = Unit.Pixel(120);
                if (!bLinkFrom)
                {
                    //Code modified for Links Requirement (PR668013) - to update the toolbar button text by Prachi on 10th Dec 2012
                    uwToolbar.Items.FromKeyButton("LinkToFrom").Text = CST_COMPANION_LIST;
                    uwToolbar.Items.FromKeyButton("LinkToFrom").ToolTip = CST_COMPANION_TOOLTIP;
                    uwToolbar.Items.FromKeyButton("Add").Text = CST_COMPANION;
                    tdList.InnerText = "Hosts List";
                }
                else
                {
                    //Code modified for Links Requirement (PR668013) - to update the toolbar button text by Prachi on 10th Dec 2012
                    uwToolbar.Items.FromKeyButton("LinkToFrom").Text = CST_HARDWARE_LIST;
                    uwToolbar.Items.FromKeyButton("LinkToFrom").ToolTip = CST_HARDWARE_TOOLTIP;
                    uwToolbar.Items.FromKeyButton("Add").Text = CST_HARDWARE;
                    tdList.InnerText = "Companions List";
                }

                // Show button
                UITools.ShowToolBarButton(uwToolbar, "LinkToFrom");
                UITools.ShowToolBarSeparator(uwToolbar, "LinkToFromSep");
                hasButton = true;
            }
            //Code added for Links Requirement - to make the "Add Host" functioanlity available at higher levels by Prachi on 10th Feb 2013
            else if (iLinkFromTo == (int)LinkFromTo.TO)  // Item is a Possible Companion and Host can be added to it
            {
                if (!bLinkFrom)
                {
                    uwToolbar.Items.FromKeyButton("Add").Text = CST_COMPANION;
                    tdList.InnerText = "Hosts List";
                    //Code added for Links Requirement - to make the "Add Host" functioanlity not available at country catalogue for services by Nisha Verma on 27thth Feb 2013
                    //Added ServicesId as part of QC6396
                    if (linkType.Id == ServicesId)
                    {
                        if (SessionState.Culture.Type == CultureType.Master || SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale)
                        {
                            uwToolbar.Items.FromKeyButton("Add").Enabled = false;
                        }
                    }
                    //Code added for Links Requirement - to make the "Add Host" functioanlity not available at country catalogue for services by Nisha Verma on 27thth Feb 2013        

                    //Start QC 7423 - Rekha Thomas - Disable Add Companions, Add hosts etc from Bundles tab.
                    if (linkType.Id == BundlesId)
                    {
                        if (SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale)
                        {
                            uwToolbar.Items.FromKeyButton("Add").Enabled = false;
                            uwToolbar.Items.FromKeyButton("Save").Enabled = false;
                        }
                    }
                    //End QC 7423 - Rekha Thomas
                }
                else
                {
                    uwToolbar.Items.FromKeyButton("Add").Text = CST_HARDWARE;
                    tdList.InnerText = "Companions List";
                }
            }
        }
        #endregion

        #region "Add button"
        if (CanManageLinks() && !viewAtCountryLevel)
            if (CanManageLinks())
                if (!viewAtCountryLevel)
                {
                    //Code commented for Links Requirement (PR668013) - to modify the toolbar by Prachi on 10th Dec 2012
                    /*
                    if (hasButton)
                        UITools.ShowToolBarSeparator(uwToolbar, "AddSep");
                     */
                    UITools.ShowToolBarButton(uwToolbar, "Add");
                    hasButton = true;
                    //Code modified for Links Requirement (PR668013) - to modify the toolbar by Prachi on 10th Dec 2012
                    uwToolbar.Items.FromKeyButton("Add").DefaultStyle.Width = Unit.Pixel(130);
                }
        //Code added for Links Requirement - to make the "Add" functioanlity not available at country catalogue for services by Nisha Verma on 27thth Feb 2013
        //Added ServicesId as part of QC6396
        if (linkType.Id == ServicesId)
        {
            if (SessionState.Culture.Type == CultureType.Master || SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale)
            {
                uwToolbar.Items.FromKeyButton("Add").Enabled = false;
            }
        }
        //Code added for Links Requirement - to make the "Add" functioanlity not available at country catalogue for services by Nisha Verma on 27thth Feb 2013

        //Start QC 7423 - Rekha Thomas - to make the "Add" functioanlity not available at country and Region catalogues for Bundles
        if (linkType.Id == BundlesId)
        {
            if (SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale)
            {
                uwToolbar.Items.FromKeyButton("Add").Enabled = false;
                uwToolbar.Items.FromKeyButton("Save").Enabled = false;
            }
        }
        //End QC 7423 - Rekha Thomas
        #endregion

        #region "Apply button"
        if (bLinkFrom && !viewAtCountryLevel)
        {
            if ((notInhLCount + inhLCount > 0) && CanApplyLinks())
            {
                // display button "Apply changes"
                if (hasButton)
                    UITools.ShowToolBarSeparator(uwToolbar, "SaveSep");
                UITools.ShowToolBarButton(uwToolbar, "Save");
                hasButton = true;
            }
        }
        #endregion

        #region "Delete button"
        // header for select All
        if (!viewAtCountryLevel)
        {
            TemplatedColumn colH = null;
            if (dg.Visible)
                colH = (TemplatedColumn)dg.Columns.FromKey("Select");
            else if (dgNoBidi.Visible)
                colH = (TemplatedColumn)dgNoBidi.Columns.FromKey("Select");

            if (colH != null)
            {
                CheckBox cbH = (CheckBox)colH.HeaderItem.FindControl("g_ca");

                // possible deletion
                if ((notInhLCount > 0) && (!AreAllAutoLinks()) && CanManageLinks())
                {
                    if (hasButton)
                        UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
                    UITools.ShowToolBarButton(uwToolbar, "Delete");
                    if (linkType.Id == 4)
                    {
                        if (hasButton)
                            UITools.ShowToolBarSeparator(uwToolbar, "ResumeInheritanceSep");
                        UITools.ShowToolBarButton(uwToolbar, "ResumeInheritance");
                    }
                    cbH.Enabled = true;
                    hasButton = true;
                }
                else
                {
                    cbH.Enabled = false;
                }
            }
        }
        //Code added for Links Requirement - to make the "Delete and select" functioanlity not available at country catalogue for services by Nisha Verma on 27thth Feb 2013
        //Added ServicesId as part of QC6396
        if (linkType.Id == ServicesId)
        {

            if (SessionState.Culture.Type == CultureType.Master || SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale)
            {
                TemplatedColumn colH = null;
                if (dg.Visible)
                    colH = (TemplatedColumn)dg.Columns.FromKey("Select");
                if (colH != null)
                {
                    CheckBox cbH = (CheckBox)colH.HeaderItem.FindControl("g_ca");
                    cbH.Enabled = false;
                }
                uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
            }
        }
        //Code added for Links Requirement - to make the "Delete and select" functioanlity not available at country catalogue for services by Nisha Verma on 27thth Feb 2013

        //Start QC 7423 - Rekha Thomas - To make the "Delete and select" functioanlity not available at country and region level for Bundles
        if (linkType.Id == BundlesId)
        {

            if (SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale)
            {
                TemplatedColumn colH = null;
                if (dg.Visible)
                    colH = (TemplatedColumn)dg.Columns.FromKey("Select");
                if (colH != null)
                {
                    CheckBox cbH = (CheckBox)colH.HeaderItem.FindControl("g_ca");
                    cbH.Enabled = false;
                }
                uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
            }
        }
        //End QC 7423 - Rekha Thomas
        #endregion

        #region "Inherited button"
        if (notInhLCount > 0 && !viewAtCountryLevel && !viewDSRLinks)
        {
            // Exist inherited links
            if (hasButton)
                UITools.ShowToolBarSeparator(uwToolbar, "NonInheritedSep");
            UITools.ShowToolBarButton(uwToolbar, "NonInherited");
            hasButton = true;
        }
        #endregion

        #region "Export button"
        if (notInhLCount + inhLCount > 0)
        {
            if (hasButton)
                UITools.ShowToolBarSeparator(uwToolbar, "ExportSep");
            UITools.ShowToolBarButton(uwToolbar, "Export");
            hasButton = true;
        }
        #endregion

        #region "At country level button"
        //if (!(SessionState.Culture.Type == CultureType.Locale) && linkType.Id == CST_LINKTYPE_CAREPACKS)
        if (!(SessionState.Culture.Type == CultureType.Locale) && linkType.Name == CST_LINKTYPE_CAREPACKS_NAME)
        {
            if (hasButton)
                UITools.ShowToolBarSeparator(uwToolbar, "AtCountryLevelSep");
            UITools.ShowToolBarButton(uwToolbar, "AtCountryLevel");
            hasButton = true;
        }
        #endregion

        #region "DSR Links Button"
        // Enables DSR Link button.
        if (CanManageLinks() && !viewNonInherited && bLinkFrom == true && !viewAtCountryLevel &&
            SessionState.Culture.Type != CultureType.Master &&
            (linkType.Id == AccessoriesId || linkType.Id == SuppliesId))
        {
            if (hasButton)
                UITools.ShowToolBarSeparator(uwToolbar, "DSRLinksSep");
            UITools.ShowToolBarButton(uwToolbar, "DSRLinks");
            hasButton = true;
        }
        #endregion

        #region "SCR Links Button"
        // Enables SCR Link button.
        if (CanManageLinks() && !viewNonInherited && bLinkFrom == true && !viewAtCountryLevel &&
            SessionState.Culture.Type != CultureType.Master &&
            (linkType.Id == AccessoriesId || linkType.Id == SuppliesId))
        {
            if (hasButton)
                UITools.ShowToolBarSeparator(uwToolbar, "SCRLinksSep");
            UITools.ShowToolBarButton(uwToolbar, "SCRLinks");
            hasButton = true;
        }
        #endregion

        uwToolbar.Visible = hasButton;
    }
    private void updateTitleTab()
    {
        if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT) != null
          && SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT).Value)
        {
            // Update tab title
            string tabKey = string.Empty;
            int tabCount = -1;
            if (linkType.Id == SuppliesId || linkType.Id == ServicesId) // Supplies or Services (to promote or remove cross sell)
            {
                // Update Cross Sell links count if flag 'IsCrossSell' is 1
                using (DataSet ds = SessionState.CurrentItem.GetLinkTypesCount(SessionState.Culture.Code, CrossSellId))
                {
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count == 1))
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        tabKey = "tb_" + dr["LinkTypeId"].ToString();
                        tabCount = Convert.ToInt32(dr["NbLinks"]);
                    }
                }
                if (tabKey.Length == 0)// Update count of current link type 
                    UITools.RefreshTab(this.Page, "tb_" + linkType.Id.ToString(), inhLCount + notInhLCount);
                else // Update count of current link type and cross sell (necessary for automatic creation)
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTabs", "<script>RefreshTabs('" + tabKey + "', " + tabCount.ToString() + ", 'tb_" + linkType.Id.ToString() + "', " + (inhLCount + notInhLCount).ToString() + ")</script>");
            }
            else
            {
                // Update count of current link type 
                UITools.RefreshTab(this.Page, "tb_" + linkType.Id.ToString(), inhLCount + notInhLCount);
            }

            // Refresh tab content on postback
            if (Page.IsPostBack)
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "UpdateContentTab", "<script>RefreshTabContent(" + Link.GetLinksCount(SessionState.CurrentItem.Id, SessionState.Culture.Code).ToString() + ");</script>");
            }
        }
    }
    private void updateToolBarStat()
    {
        lbInheritedLnk.Text = "Inherited link count: " + inhLCount.ToString();
        lbSpecificLnk.Text = "Specific link count: " + notInhLCount.ToString();
        lbInhRCount.Text = "DS Recommended inherited link count: " + inhRLCount.ToString();
        lbNotInhRCount.Text = "DS Recommended specific link count: " + notInhRLCount.ToString();

        lbInhSCount.Text = "SC Recommended inherited link count: " + inhSRCount.ToString();
        lbNotInhSCount.Text = "SC Recommended specific link count: " + notInhSRCount.ToString();

        //if (linkType.Id == CrossSellId && notInhLCount < 1) // Hide ResumeInheritance buvtton if the crosssell does not have local links - Prabhu
        //{
        //    UITools.HideToolBarSeparator(uwToolbar, "ResumeInheritanceSep");
        //    UITools.HideToolBarButton(uwToolbar, "ResumeInheritance");
        //}

        lbInhRCount.Visible = lbNotInhRCount.Visible = (SessionState.Culture.Type == CultureType.Regionale);

        lbInhSCount.Visible = lbNotInhSCount.Visible = (SessionState.Culture.Type == CultureType.Regionale);

        if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT) != null)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateToolbarStat", "<script>hideDiv('divToolbar', '" + (!SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT).Value).ToString() + "');</script>");
        }
    }
    private void updateCheckbox(UltraGridRow r)
    {
        bool isInherited = false;
        bool isExcluded = false;
        string countryExcluded = string.Empty;
        string countryCode = string.Empty;
        string fallbackCountryCode = string.Empty;
        Int64 itemExcluded = -1;
        System.Int64 itemId = -1;
        if (r.Cells.FromKey("IsInherited") != null && r.Cells.FromKey("IsInherited").Value != null)
            isInherited = Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value);
        if (r.Cells.FromKey("IsExcluded") != null && r.Cells.FromKey("IsExcluded").Value != null)
            isExcluded = Convert.ToBoolean(r.Cells.FromKey("IsExcluded").Value);
        if (r.Cells.FromKey("ItemExcluded") != null && r.Cells.FromKey("ItemExcluded").Value != null)
            itemExcluded = Convert.ToInt64(r.Cells.FromKey("ItemExcluded").Value);
        if (r.Cells.FromKey("CountryExcluded") != null && r.Cells.FromKey("CountryExcluded").Value != null)
            countryExcluded = r.Cells.FromKey("CountryExcluded").Value.ToString();
        if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null)
            itemId = Convert.ToInt64(r.Cells.FromKey("ItemId").Value.ToString().Trim());
        if (r.Cells.FromKey("CountryCode") != null && r.Cells.FromKey("CountryCode").Value != null)
            countryCode = r.Cells.FromKey("CountryCode").Value.ToString();
        //FallbackCountryCode

        //Added by Kalai
        if (r.Cells.FromKey("FallbackCountryCode") != null && r.Cells.FromKey("FallbackCountryCode").Value != null)
            fallbackCountryCode = r.Cells.FromKey("FallbackCountryCode").Value.ToString();
        //Kalai code ends here
        // Exclusion checkbox
        if (r.Cells.FromKey("IsExcluded") != null && !r.Cells.FromKey("IsExcluded").Column.ServerOnly)
        {
            if (isInherited)
            {
                System.Int64 inheritedItemId = -1;
                System.Int64 subItemId = -1;
                if (r.Cells.FromKey("InheritedItemId") != null && r.Cells.FromKey("InheritedItemId").Value != null)
                    inheritedItemId = Convert.ToInt64(r.Cells.FromKey("InheritedItemId").Value);
                if (r.Cells.FromKey("SubItemId") != null && r.Cells.FromKey("SubItemId").Value != null)
                    subItemId = Convert.ToInt64(r.Cells.FromKey("SubItemId").Value);
                if (r.Cells.FromKey("CountryCode") != null && r.Cells.FromKey("CountryCode").Value != null)
                    countryCode = r.Cells.FromKey("CountryCode").Value.ToString();

                if (inheritedItemId >= 0 && subItemId >= 0 && itemId >= 0 && countryCode.Length > 0)
                {
                    if (!isExcluded || (countryCode == countryExcluded && itemId == itemExcluded))
                        r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.Yes;
                    else
                        r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
                }
            }
            // Added by Vinay to block parent exclusions 
            if (isExcluded && (itemId != itemExcluded || !countryCode.Equals(countryExcluded)))
                r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;

            int userId = -1;
            if (r.Cells.FromKey("UserId") != null && r.Cells.FromKey("UserId").Value != null)
                userId = Convert.ToInt32(r.Cells.FromKey("UserId").Value);

            if (!bLinkFrom
        || viewAtCountryLevel
                || (userId == 0 && isExcluded)
                || SessionState.User.IsReadOnly
                || !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS)
                || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkType.Id == CrossSellId))
            {
                if (SessionState.Culture.Type == CultureType.Locale && !SessionState.Culture.Country.CanLocalizeCrossSells && linkType.Id == CrossSellId)
                    r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
                if (viewAtCountryLevel)
                    r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
            }
        }

        // Recommended checkbox
        if (r.Cells.FromKey("IsRecommended") != null
            && !SessionState.User.IsReadOnly
            && !viewAtCountryLevel
            && (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS)
                || r.Cells.FromKey("InheritedLevel").Value.ToString() != SessionState.CurrentItem.Level.Name.ToString()
                || (SessionState.Culture.Type == CultureType.Locale && r.Cells.FromKey("FallBackCountryCode").Value.ToString() != SessionState.Culture.CountryCode))
            && (!(SessionState.Culture.Type == CultureType.Locale && SessionState.CurrentItem.IsCountrySpecific))
            && (linkType.Id == AccessoriesId || linkType.Id == SuppliesId))
            r.Cells.FromKey("IsRecommended").AllowEditing = AllowEditing.No;

        // SC Recommended checkbox
        if (r.Cells.FromKey("IsSCRecommended") != null
            && !SessionState.User.IsReadOnly
            && !viewAtCountryLevel
            && (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS)
                || r.Cells.FromKey("InheritedLevel").Value.ToString() != SessionState.CurrentItem.Level.Name.ToString()
                || (SessionState.Culture.Type == CultureType.Locale && r.Cells.FromKey("FallBackCountryCode").Value.ToString() != SessionState.Culture.CountryCode))
            && (!(SessionState.Culture.Type == CultureType.Locale && SessionState.CurrentItem.IsCountrySpecific))
            && (linkType.Id == AccessoriesId || linkType.Id == SuppliesId))
            r.Cells.FromKey("IsSCRecommended").AllowEditing = AllowEditing.No;

        // Deleted checkbox
        if (r.Cells.FromKey("UserId") != null && r.Cells.FromKey("UserId").Value != null
      && r.Cells.FromKey("Select") != null)
        {
            int userId = Convert.ToInt32(r.Cells.FromKey("UserId").Value);
            if (userId == 0 || isInherited || countryCode != fallbackCountryCode)//Kalai added countryCode != fallbackCountryCode   // not delete automatic links or inherited links 
            {
                TemplatedColumn col = (TemplatedColumn)r.Cells.FromKey("Select").Column;
                if (col != null)
                {
                    CheckBox cb = (CheckBox)((CellItem)col.CellItems[r.Index]).FindControl("g_sd");
                    if (cb != null)
                        cb.Enabled = false;
                }
            }
        }
    }
    private void updateColumns()
    {
        if (linkType.IsBidirectional)
        {
            if (bLinkFrom) // display companions list
            {
                dg.Columns.FromKey("Class").BaseColumnName = "SubClassName";
                dg.Columns.FromKey("Family").BaseColumnName = "SubFamilyName";
                dg.Columns.FromKey("ItemSku").BaseColumnName = "SubItemSKU";
                dg.Columns.FromKey("ItemName").BaseColumnName = "SubItemName";

                if ((linkType.Id == AccessoriesId || linkType.Id == SuppliesId)
                    && (SessionState.Culture.Type == CultureType.Regionale || SessionState.Culture.Type == CultureType.Locale))
                {
                    dg.Columns.FromKey("IsRecommended").ServerOnly = false;
                    dg.Columns.FromKey("IsSCRecommended").ServerOnly = false;
                }
                else
                {
                    dg.Columns.FromKey("IsRecommended").ServerOnly = true;
                    dg.Columns.FromKey("IsSCRecommended").ServerOnly = true;
                }
            }
            else // display hardware
            {
                dg.Columns.FromKey("Class").BaseColumnName = "ClassName";
                dg.Columns.FromKey("Family").BaseColumnName = "FamilyName";
                dg.Columns.FromKey("ItemSku").BaseColumnName = "ItemSKU";
                dg.Columns.FromKey("ItemName").BaseColumnName = "ItemName";
                dg.Columns.FromKey("IsRecommended").ServerOnly = true;
                dg.Columns.FromKey("IsSCRecommended").ServerOnly = true;
                dg.Columns.FromKey("InheritedLevel").ServerOnly = true;
            }
            //Kalai added
            //if (SessionState.Culture.Type == CultureType.Regionale
            //      || (SessionState.Culture.Type == CultureType.Locale && SessionState.CurrentItem.IsCountrySpecific))
            //    dg.Columns.FromKey("Select").ServerOnly = false;
            //else
            //    dg.Columns.FromKey("Select").ServerOnly = true;
            //KAlai added

            //Kalai added    || (SessionState.Culture.Type == CultureType.Regionale) || (SessionState.Culture.Type == CultureType.Locale)
            if ((SessionState.Culture.Type == CultureType.Master)
                || (SessionState.Culture.Type == CultureType.Regionale)
                || (SessionState.Culture.Type == CultureType.Locale)
                || (SessionState.Culture.Type == CultureType.Regionale && linkType.Id == CrossSellId) //Added by Prabhu for Cross-Sell requirement.
              || ((SessionState.Culture.Type == CultureType.Locale) && (linkType.Id == CrossSellId) && SessionState.Culture.Country.CanLocalizeCrossSells)
              || ((SessionState.Culture.Type == CultureType.Locale) && SessionState.CurrentItem.IsCountrySpecific))
            {
                dg.Columns.FromKey("Select").ServerOnly = false;
            }
            else
            {
                dg.Columns.FromKey("Select").ServerOnly = true;
            }

            // If the DSRLinks button is toggled Excluded column is hidden.
            dg.Columns.FromKey("IsExcluded").ServerOnly = viewDSRLinks;
        }
        else
        {
            if (linkType.Id == BundlesId)
                dgNoBidi.Columns.FromKey("IsExcluded").ServerOnly = true;

            if ((SessionState.Culture.Type == CultureType.Master)
                 || (SessionState.Culture.Type == CultureType.Regionale)
                || (SessionState.Culture.Type == CultureType.Locale)
                || (SessionState.Culture.Type == CultureType.Regionale && linkType.Id == CrossSellId) //Added by Prabhu for Cross-Sell requirement.
              || ((SessionState.Culture.Type == CultureType.Locale) && (linkType.Id == CrossSellId) && SessionState.Culture.Country.CanLocalizeCrossSells)
              || ((SessionState.Culture.Type == CultureType.Locale) && SessionState.CurrentItem.IsCountrySpecific))
            {
                dgNoBidi.Columns.FromKey("Select").ServerOnly = viewAtCountryLevel;
            }
            else
            {
                dgNoBidi.Columns.FromKey("Select").ServerOnly = true;
            }
        }
    }

    private void RetrieveLinkType()
    {
        // Retrieve link types
        using (LinkTypeList linkTypes = LinkType.GetAll())
        {
            AccessoriesId = GetLinkType(linkTypes, "Accessories");
            SuppliesId = GetLinkType(linkTypes, "Supplies");
            ServicesId = GetLinkType(linkTypes, "Services");
            CrossSellId = GetLinkType(linkTypes, "CrossSell");
            BundlesId = GetLinkType(linkTypes, "Bundles");
        }
    }
    private int GetLinkType(LinkTypeList linkTypes, string linkTypeName)
    {
        // Retrieve Id of the link type switch its name
        foreach (LinkType linkType in linkTypes)
        {
            if (linkType.Name.Equals(linkTypeName))
            {
                return linkType.Id;
            }
        }
        return -1;
    }
    private bool AreAllAutoLinks()
    {
        UltraWebGrid grid = null;
        if (dg.Visible)
            grid = dg;
        else if (dgNoBidi.Visible)
            grid = dgNoBidi;

        if (grid != null)
        {
            foreach (UltraGridRow r in grid.Rows)
            {
                // links is not inherited and is created by system user (0)
                if (r.Cells.FromKey("UserId") != null
                    && r.Cells.FromKey("UserId").Value != null
                    && Convert.ToInt32(r.Cells.FromKey("UserId").Value) > 0
                    && !Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private string buildSortList()
    {
        System.Text.StringBuilder sortList = new System.Text.StringBuilder(string.Empty);

        UltraWebGrid grid = null;
        if (dg != null && dg.Visible)
            grid = dg;
        else if (dgNoBidi != null && dgNoBidi.Visible)
            grid = dgNoBidi;

        if (grid != null)
        {
            int count = 0;
            // Get all rows for sort and exclusion
            foreach (UltraGridRow r in grid.Rows)
            {
                // new sort
                if (sortList.Length > 0)
                    sortList.Append("|");

                sortList.Append("'");
                sortList.Append(r.Cells.FromKey("LinkId").Value.ToString());
                sortList.Append("',");
                sortList.Append(r.Cells.FromKey("ItemId").Value.ToString());
                sortList.Append(",'");
                sortList.Append(SessionState.Culture.CountryCode);
                sortList.Append("',");

                if (viewDSRLinks)
                {
                    if (Convert.ToBoolean(r.Cells.FromKey("IsRecommended").Value) == true)
                        sortList.Append(count++ + 1);
                    else
                        sortList.Append(0);
                }
                if (viewSCRLinks)
                {
                    if (Convert.ToBoolean(r.Cells.FromKey("IsSCRecommended").Value) == true)
                        sortList.Append(count++ + 1);
                    else
                        sortList.Append(0);
                }
            }
        }

        return sortList.ToString();
    }


    private List<LinkExclusion> buildExclusionList()
    {

        List<LinkExclusion> exclusionList = new List<LinkExclusion>();

        UltraWebGrid grid = null;
        if (dg != null && dg.Visible)
            grid = dg;
        else if (dgNoBidi != null && dgNoBidi.Visible)
            grid = dgNoBidi;

        if (grid != null)
        {
            // Get all rows for sort and exclusion
            foreach (UltraGridRow r in grid.Rows)
            {
                UltraGridCell cellItemId = r.Cells.FromKey("ItemId");
                if (cellItemId != null && cellItemId.Value != null)
                {
                    UltraGridCell cellIsExcluded = r.Cells.FromKey("IsExcluded");
                    if (cellIsExcluded.IsEditable() && cellIsExcluded.DataChanged)
                    {
                        // inheritedItemId is the itemid of the link which could be inherited
                        Int64 linkItemId = Int64.Parse(r.Cells.FromKey("InheritedItemId").Value.ToString());
                        Int64 linkSubItemId = Int64.Parse(r.Cells.FromKey("SubItemId").Value.ToString());
                        int linkTypeId = linkType.Id;

                        // fallbackCountryCode is the countrycode of the link
                        String linkCountryCode = r.Cells.FromKey("FallbackCountryCode").Value.ToString();

                        // this is the itemid of the item the user is viewing
                        Int64 itemId = Int64.Parse(r.Cells.FromKey("ItemId").Value.ToString());

                        // this is the status of the excluded checkbox
                        int isExcluded = ((cellIsExcluded.Value != null && cellIsExcluded.Value.ToString().Equals("True")) ? 1 : 0);

                        // set the countrycode to be the countrycode where the exclusion occurred. If there is no existing exclusion then set to the 
                        // countrycode the user is currently viewing
                        String exclCountryCode = r.Cells.FromKey("CountryExcluded").Value.ToString();
                        if (exclCountryCode != null && exclCountryCode.Trim().Length == 0)
                            exclCountryCode = SessionState.Culture.CountryCode;

                        LinkExclusion le = new LinkExclusion(linkItemId, linkCountryCode, linkSubItemId, linkTypeId, itemId, exclCountryCode, isExcluded);
                        exclusionList.Add(le);
                    }
                }
            }
        }
        return exclusionList;
    }

    private string buildRecommendedList()
    {
        System.Text.StringBuilder recommendedLinkList = new System.Text.StringBuilder(string.Empty);

        if (dg != null && dg.Visible)
        {
            // Get all rows for recommendation
            foreach (UltraGridRow r in dg.Rows)
            {
                if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null && (r.Cells.FromKey("isRecommended").IsEditable() == true || viewDSRLinks == true))
                {
                    // new recommended links
                    if (recommendedLinkList.Length > 0)
                        recommendedLinkList.Append("|");

                    recommendedLinkList.Append("'");
                    recommendedLinkList.Append(r.Cells.FromKey("LinkId"));
                    recommendedLinkList.Append("',");
                    recommendedLinkList.Append(r.Cells.FromKey("ItemId").Value.ToString());
                    recommendedLinkList.Append(",'");
                    recommendedLinkList.Append(SessionState.Culture.CountryCode);
                    recommendedLinkList.Append("',");
                    recommendedLinkList.Append(Convert.ToByte(r.Cells.FromKey("IsRecommended").Value));
                }
            }
        }
        return recommendedLinkList.ToString();
    }

    private string buildSCRecommendedList()
    {
        System.Text.StringBuilder recommendedLinkList = new System.Text.StringBuilder(string.Empty);
        if (dg != null && dg.Visible)
        {
            // Get all rows for screcommendatio
            foreach (UltraGridRow r in dg.Rows)
            {
                if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null && (r.Cells.FromKey("isSCRecommended").IsEditable() == true || viewSCRLinks == true))
                {
                    // new recommended links
                    if (recommendedLinkList.Length > 0)
                        recommendedLinkList.Append("|");
                    recommendedLinkList.Append("'");
                    recommendedLinkList.Append(r.Cells.FromKey("LinkId"));
                    recommendedLinkList.Append("',");
                    recommendedLinkList.Append(r.Cells.FromKey("ItemId").Value.ToString());
                    recommendedLinkList.Append(",'");
                    recommendedLinkList.Append(SessionState.Culture.CountryCode);
                    recommendedLinkList.Append("',");
                    recommendedLinkList.Append(Convert.ToByte(r.Cells.FromKey("IsSCRecommended").Value));
                }
            }
        }
        return recommendedLinkList.ToString();
    }

    private bool CanManageLinks()
    {
        if (!SessionState.User.IsReadOnly && SessionState.CurrentItemIsUserItem)
        {
            // Culture
            //Kalai added   (SessionState.Culture.Type == CultureType.Regionale) || (SessionState.Culture.Type == CultureType.Locale)
            if ((SessionState.Culture.Type == CultureType.Master)
                || (SessionState.Culture.Type == CultureType.Regionale)
                || (SessionState.Culture.Type == CultureType.Locale)
              || (SessionState.Culture.Type == CultureType.Regionale && linkType.Id == CrossSellId) //Added by Prabhu for Cross-Sell requirement.
              || (SessionState.Culture.Type == CultureType.Locale && SessionState.CurrentItem.IsCountrySpecific)
              || (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanLocalizeCrossSells && linkType.Id == CrossSellId))
            {
                // Capability
                if ((SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
                  || (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkType.Id == CrossSellId))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool CanApplyLinks()
    {
        if (!SessionState.User.IsReadOnly && SessionState.CurrentItemIsUserItem)
        {
            // Culture
            if ((SessionState.Culture.Type == CultureType.Master)
              || (SessionState.Culture.Type == CultureType.Regionale)
              || (SessionState.Culture.Type == CultureType.Locale)
              || (SessionState.Culture.Type == CultureType.Locale && linkType.Id != CrossSellId)
              || (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanLocalizeCrossSells && linkType.Id == CrossSellId))
            {
                // Capability
                if ((SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
                  || (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkType.Id == CrossSellId))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void InitializeLinksGridGrouping()
    {
        int i = 0;
        groupCount = 0;
        int indexMax = dg.Columns.Count - 1;
        while (i < dg.Rows.Count)
        {
            if (dg.Rows[i].Cells.FromKey("Family").Value != null)
            {
                string linkGroup = dg.Rows[i].Cells.FromKey("Family").Value.ToString();
                if (i == 0 || currentLinkGroup != linkGroup)
                {
                    currentLinkGroup = linkGroup;
                    dg.Rows.Insert(i, new UltraGridRow());
                    UltraGridRow groupRow = dg.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[indexMax]; // initialize all cells for this row

                    UltraGridCell groupCell = null;
                    int index = 0;
                    if (groupRow.Cells.FromKey("Select").Column.ServerOnly)
                        index = groupRow.Cells.FromKey("Class").Column.Index;
                    else
                        index = groupRow.Cells.FromKey("Select").Column.Index;
                    groupCell = groupRow.Cells[index];
                    groupCell.Style.HorizontalAlign = HorizontalAlign.Left;
                    groupCell.Style.CssClass = "ptbgroup";
                    groupCell.Style.CustomRules = "";
                    groupCell.Text = linkGroup;
                    groupCell.ColSpan = indexMax - index + 1;
                }
                i++;
            }
            else
            {
                dg.Rows[i].Delete();
            }
            //i++;
        }
    }

    /// <summary>
    /// Accepts LinkList object as argument and returns list sorted by 
    ///	SubItemSort. If SubItemSort = 0 it is moved to the bottom of the list.
    /// </summary>
    /// <param name="links">LinkList to be sorted.</param>
    /// <returns>LinkList</returns>
    private LinkList SortLinks(LinkList links)
    {
        LinkList linksTemp = new LinkList();
        for (int i = 0; i < links.Count; )
        {
            if (links[i].SubItemSort == 0)
            {
                linksTemp.Add(links[i]);
                links.RemoveAt(i);
            }
            else
                i++;
        }

        links.Sort("SubItemSort");
        linksTemp.Sort("SubItemSKU");

        for (int i = 0; i < linksTemp.Count; i++)
        {
            links.Add(linksTemp[i]);
        }

        return links;
    }
    #endregion
}
