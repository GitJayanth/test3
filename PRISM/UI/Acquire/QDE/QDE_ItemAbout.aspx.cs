#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
    /// <summary>
    /// Description r�sum�e de QDE_ItemAbout.
    /// </summary>
    public partial class QDE_ItemAbout : HCPage
    {
        #region Declarations
        private HyperCatalog.Business.Chunk localizedProductNameChunk;
        protected int itemLevelId;
        protected System.Int64 itemId;
        protected string itemName, itemLevelName;
        #endregion

        #region Properties
        private bool isUserItem
        {
            get { return (bool)ViewState["isUserItem"]; }
            set { ViewState["isUserItem"] = value; }
        }
        private bool isMinimizedItem
        {
            get { return (bool)ViewState["isMinimizedItem"]; }
            set { ViewState["isMinimizedItem"] = value; }
        }
        private bool isFrozenItem
        {
            get { return (bool)ViewState["isFrozenItem"]; }
            set { ViewState["isFrozenItem"] = value; }
        }
        #endregion

        #region Constantes
        private const int ITEM_TYPE_BUNDLE = 3;
        private const string APPLIED_LANGUAGES = "Applied languages are being computed...<img src='/hc_v4/img/ed_wait.gif'/>";
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {

            SessionState.QDETab = "tb_info"; // information
            QDEUtils.UpdateCultureCodeFromRequest();
            RetrieveItem();

            if (SessionState.CurrentItem.IsCountrySpecific && SessionState.Culture.Type == CultureType.Locale)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "redirect", "<script>document.location='QDE_ItemCountryAbout.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code + "'</script>");
            }
            else
            {
                #region "Extra JS Code added because of Empty Master Template Added on 11/21/2006"
                string script = string.Empty;
                script += "<input type='hidden' id='aj_id' name='aj_id'/>";
                script += "<input type='hidden' id='aj_cc' name='aj_cc'/>";
                script += "<script>";
                script += "document.getElementById(\"aj_id\").value = " + SessionState.CurrentItem.Id.ToString() + ";";
                script += "document.getElementById(\"aj_cc\").value = '" + SessionState.Culture.Code + "';";
                script += "</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "initvars", script);
                #endregion

                if (!Page.IsPostBack)
                {
                    isUserItem = SessionState.CurrentItemIsUserItem;
                    isMinimizedItem = SessionState.CurrentItem.IsMinimizedByCulture(SessionState.Culture.Code);
                    isFrozenItem = SessionState.CurrentItem.IsFrozenByCulture(SessionState.Culture.Code);
                    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 20/May/09 -- Check
                    SessionState.CurrentItem.Milestones = null;
                    UpdateDataView();
                }
                else
                {
                    // action after delete TR in TR properties window 
                    if (Request["action"] != null && Request["action"].ToString().ToLower() == "reload")
                    {
                        isUserItem = SessionState.CurrentItemIsUserItem;
                        isMinimizedItem = SessionState.CurrentItem.IsMinimizedByCulture(SessionState.Culture.Code);
                        isFrozenItem = SessionState.CurrentItem.IsFrozenByCulture(SessionState.Culture.Code);
                        //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 20/May/09 -- Check
                        SessionState.CurrentItem.Milestones = null;
                        UpdateDataView();
                    }
                    Infragistics.WebUI.Misc.WebPanel wPanel = null;
                    switch (Request.Form["__EVENTTARGET"].ToString())
                    {
                        case "deleteproj":
                            if (InitDeleteProject())
                            {
                                RetrieveItem();
                                InitViewProject();

                                wPanel = wPanelProject;
                                wPanel.Header.Text = wPanel.Header.Text.Replace("save", "edit");
                                wPanel.Header.Text = wPanel.Header.Text.Replace("&nbsp;<a href=\"javascript:DoAction('deleteproj','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[delete]</span></a>", string.Empty);
                                wPanel.Header.Text = wPanel.Header.Text.Replace("&nbsp;<a href=\"javascript:DoAction('cancel','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[cancel]</span></a>", string.Empty);
                                wPanel.PanelStyle.BackColor = Color.WhiteSmoke;
                                wPanel.Expanded = true;
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTV", "<script>ReloadTV('', 0);</script>");
                            }
                            break;
                        case "edit":
                        case "view":
                            RetrieveItem();
                            switch (Request.Form["__EVENTARGUMENT"].ToString())
                            {
                                case "identity":
                                    wPanel = wPanelIdentity;
                                    InitEditIdentity();
                                    break;
                                case "project":
                                    wPanel = wPanelProject;
                                    InitEditProject();
                                    break;
                                case "market":
                                    wPanel = wPanelMarket;
                                    InitEditMarketSegments();
                                    break;
                                case "plc":

                                    /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */
                                    if (Request.Form["__EVENTTARGET"].ToString() == "edit")
                                    {
                                        wPanel = wPanelPLC;
                                        InitEditPLC();
                                    }
                                    else if (Request.Form["__EVENTTARGET"].ToString() == "view")
                                    {
                                        wPanel = wPanelPLC;
                                        InitViewDummyPLC();
                                    }
                                    /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */

                                    break;
                                case "publisher":
                                    wPanel = wPanelPublisher;
                                    InitEditPublishers();
                                    break;
                            }
                            wPanel.Header.Text = wPanel.Header.Text.Replace("edit", "save");
                            wPanel.Header.Text = wPanel.Header.Text.Replace("<a href=\"javascript:DoAction('view','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[view]</span></a>", "<span id=view></span>");
                            wPanel.Header.Text = wPanel.Header.Text.Replace("&nbsp;<a href=\"javascript:DoAction('deleteproj','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[delete]</span></a>", "<span id=deleteproj></span>");

                            if (!Request.Form["__EVENTTARGET"].ToString().Equals("view"))
                                wPanel.Header.Text = wPanel.Header.Text + "&nbsp;";
                            wPanel.Header.Text = wPanel.Header.Text + "<a href=\"javascript:DoAction('cancel','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[cancel]</span></a>";
                            wPanel.PanelStyle.BackColor = Color.White;
                            wPanel.Expanded = true;

                            UpdateAppliedLanguages();
                            break;
                        case "cancel":
                            RetrieveItem();
                            switch (Request.Form["__EVENTARGUMENT"].ToString())
                            {
                                case "identity":
                                    wPanel = wPanelIdentity;
                                    InitViewIdentity();
                                    break;
                                case "project":
                                    wPanel = wPanelProject;
                                    InitViewProject();
                                    break;
                                case "market":
                                    wPanel = wPanelMarket;
                                    InitViewMarketSegments();
                                    break;
                                case "plc":
                                    wPanel = wPanelPLC;
                                    InitViewPLC();
                                    break;
                                case "publisher":
                                    wPanel = wPanelPublisher;
                                    InitViewPublishers();
                                    break;
                            }
                            wPanel.Header.Text = wPanel.Header.Text.Replace("save", "edit");
                            wPanel.Header.Text = wPanel.Header.Text.Replace("<span id=view></span>", "<a href=\"javascript:DoAction('view','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[view]</span></a>");
                            wPanel.Header.Text = wPanel.Header.Text.Replace("&nbsp;<a href=\"javascript:DoAction('cancel','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[cancel]</span></a>", string.Empty);
                            wPanel.Header.Text = wPanel.Header.Text.Replace("<a href=\"javascript:DoAction('cancel','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[cancel]</span></a>", string.Empty);

                            //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 20/May/09 -- Check
                            if (Request.Form["__EVENTARGUMENT"].ToString() == "project" && (SessionState.Culture.Type == CultureType.Regionale)
                              && SessionState.CurrentItem != null && !SessionState.CurrentItem.Milestones.Inherited
                              && SessionState.CurrentItem.Milestones.CreationDate != null)
                            {
                                wPanel.Header.Text = wPanel.Header.Text.Replace("<span id=deleteproj></span>", "&nbsp;<a href=\"javascript:DoAction('deleteproj','project')\"><span class='hc_webpanela'>[delete]</span></a>");
                            }
                            wPanel.PanelStyle.BackColor = Color.WhiteSmoke;
                            wPanel.Expanded = true;

                            UpdateAppliedLanguages();
                            break;
                        case "save":
                            {
                                bool canSwitch = false;
                                switch (Request.Form["__EVENTARGUMENT"].ToString())
                                {
                                    case "identity":
                                        if (InitSaveIdentity())
                                        {
                                            if (!(SessionState.CurrentItem.IsCountrySpecific && SessionState.Culture.Type == CultureType.Master))
                                                canSwitch = true;
                                            InitViewIdentity();
                                            UpdateAppliedLanguages();
                                        }
                                        wPanel = wPanelIdentity;
                                        break;
                                    case "project":
                                        if (InitSaveProject())
                                        {
                                            canSwitch = true;
                                            InitViewProject();
                                        }
                                        wPanel = wPanelProject;
                                        break;
                                    case "market":
                                        if (InitSaveMarketSegments())
                                        {
                                            canSwitch = true;
                                            InitViewMarketSegments();
                                            UpdateAppliedLanguages();
                                        }
                                        wPanel = wPanelMarket;
                                        break;
                                    case "plc":
                                        if (InitSavePLC())
                                        {
                                            canSwitch = true;
                                            InitViewPLC();

                                            /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */
                                            InitViewPLCModifier();
                                            /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */

                                        }
                                        wPanel = wPanelPLC;
                                        break;
                                    case "publisher":
                                        if (InitSavePublishers())
                                        {
                                            canSwitch = true;
                                            InitViewPublishers();
                                            UpdateAppliedLanguages();
                                        }
                                        wPanel = wPanelPublisher;
                                        break;
                                }
                                if (canSwitch)
                                {
                                    SessionState.CurrentItem = Item.GetByKey(SessionState.CurrentItem.Id);
                                    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
                                    SessionState.CurrentItem.RegionCode = SessionState.Culture.Code;
                                    if (SessionState.CurrentItem.Milestones.Inherited)
                                        SessionState.CurrentItem.Milestones.InheritedItem.RegionCode = SessionState.Culture.Code;
                                    itemId = SessionState.CurrentItem.Id;
                                    wPanel.Header.Text = wPanel.Header.Text.Replace("save", "edit");
                                    wPanel.Header.Text = wPanel.Header.Text.Replace("&nbsp;<a href=\"javascript:DoAction('cancel','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[cancel]</span></a>", string.Empty);
                                    wPanel.Header.Text = wPanel.Header.Text.Replace("<a href=\"javascript:DoAction('cancel','" + Request.Form["__EVENTARGUMENT"].ToString() + "')\"><span class='hc_webpanela'>[cancel]</span></a>", string.Empty);

                                    if (SessionState.Culture.Type == CultureType.Regionale)
                                        Trace.Warn(SessionState.CurrentItem.Milestones.Inherited.ToString());
                                    // fix for QC 1779 - Prabhu
                                    if (SessionState.CurrentItem.Milestones.BeginningOfRegionalization.HasValue && (SessionState.Culture.Type == CultureType.Regionale))//Prabhu - 5/Mar/08 - Fix to prevent blank screen in Information tab when editing whose Project is set at one of its decendents
                                        Trace.Warn(SessionState.CurrentItem.Milestones.BeginningOfRegionalization.Value.ToLongTimeString());  
                                    Trace.Warn(wPanel.Header.Text);

                                    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 20/May/09 -- Check
                                    if (Request.Form["__EVENTARGUMENT"].ToString() == "project" && (SessionState.Culture.Type == CultureType.Regionale)
                                      && SessionState.CurrentItem != null && !SessionState.CurrentItem.Milestones.Inherited
                                      && SessionState.CurrentItem.Milestones.CreationDate != null)
                                    {
                                        wPanel.Header.Text = wPanel.Header.Text.Replace("<span id=deleteproj></span>", "&nbsp;<a href=\"javascript:DoAction('deleteproj','project')\"><span class='hc_webpanela'>[delete]</span></a>");
                                    }
                                    wPanel.PanelStyle.BackColor = Color.WhiteSmoke;
                                    wPanel.Expanded = true;
                                }

                                break;
                            }
                    }
                }
                HidePanels();
            }
        }


        private void UpdateAppliedLanguages()
        {
            #region Applied languages
            using (LanguageList applicableLanguages = SessionState.CurrentItem.GetItemApplicableLanguages())
            {
                if (applicableLanguages.Count > 0)
                {
                    lbProjectInfoApplicableLanguages.Text = string.Empty;

                    using (HyperCatalog.Business.LanguageList languages = HyperCatalog.Business.Language.GetByCulture(HyperCatalog.Shared.SessionState.Culture.Code, string.Empty))
                    {
                        for (int i = 0; i < applicableLanguages.Count; i++)
                        {
                            // if current culture is a region then the language of this region are only displayed
                            if (ContainsLanguage(languages, applicableLanguages[i]))
                                lbProjectInfoApplicableLanguages.Text = lbProjectInfoApplicableLanguages.Text + applicableLanguages[i].Code + ", ";
                        }
                    }

                    if (lbProjectInfoApplicableLanguages.Text.Length > 0)
                        lbProjectInfoApplicableLanguages.Text = lbProjectInfoApplicableLanguages.Text.Substring(0, lbProjectInfoApplicableLanguages.Text.Length - 2);
                    else
                        lbProjectInfoApplicableLanguages.Text = "None";
                }
            }
            #endregion
        }
        private bool IsSuperiorToSKULevel()
        {
            if (SessionState.CurrentItem != null && HyperCatalog.Shared.SessionState.SkuLevel != null)
            {
                return SessionState.CurrentItem.LevelId > HyperCatalog.Shared.SessionState.SkuLevel.Id;
            }
            return false;
        }
        private Item RetrieveItem()
        {
            #region "Retrieve Item"
            using (HyperCatalog.Business.Item curItem = QDEUtils.GetItemIdFromRequest())
            {
                itemId = curItem.Id;
                localizedProductNameChunk = null;
                if (SessionState.Culture.Type != CultureType.Master)
                {
                    localizedProductNameChunk = HyperCatalog.Business.Chunk.GetByKey(itemId, 1, SessionState.Culture.Code);
                }
                return curItem;
            }
            #endregion
        }
        private void UpdateDataView()
        {
            //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
            SessionState.CurrentItem.RegionCode = SessionState.Culture.Code;
            if (SessionState.CurrentItem.Milestones.InheritedItem != null)
                SessionState.CurrentItem.Milestones.InheritedItem.RegionCode = SessionState.Culture.Code;

            if (SessionState.CurrentItem != null)
            {
				//Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009
                SessionState.CurrentItem.Translations = null;
                #region Design interface
                InitViewIdentity();
                //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
                if (SessionState.Culture.Type == CultureType.Regionale)
                {
                    InitViewProject();
                }
                InitViewMarketSegments();
                InitViewPLC();
                InitViewPublishers();

                /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */
                InitViewPLCModifier();
                /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */

                #endregion

                #region customize screen depending on user capabilities
                #region "Panel identity"
                if (isUserItem
                  && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                  && (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS) || SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEM_TYPE_TO_CHANGE_IN_BUNDLE))
                  && !isMinimizedItem)
                {
                    wPanelIdentity.Header.Text = wPanelIdentity.Header.Text + " " + "<a href=\"javascript:DoAction('edit','identity')\"><span class='hc_webpanela'>[edit]</span></a>";
                }
                #endregion
                #region "Panel project"
                if (isUserItem && (SessionState.Culture.Type == CultureType.Regionale)
                    && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                    && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PROJECTS)
                    && !isMinimizedItem
                    && !IsSuperiorToSKULevel())
                {
                    wPanelProject.Header.Text = wPanelProject.Header.Text + " " + "<a href=\"javascript:DoAction('edit','project')\"><span class='hc_webpanela'>[edit]</span></a>";
                    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -- Check
                    if (!SessionState.CurrentItem.Milestones.Inherited && SessionState.CurrentItem.Milestones.CreationDate != null)
                        wPanelProject.Header.Text = wPanelProject.Header.Text + "&nbsp;<a href=\"javascript:DoAction('deleteproj','project')\"><span class='hc_webpanela'>[delete]</span></a>";
                    else
                        wPanelProject.Header.Text = wPanelProject.Header.Text + "<span id=deleteproj></span>";
                }
                else
                {
                    // Check
                    if ((SessionState.Culture.Type == CultureType.Regionale) && (SessionState.CurrentItem.Milestones.Inherited || SessionState.CurrentItem.Milestones.BeginningOfRegionalization.HasValue))
                        wPanelProject.Header.Text = wPanelProject.Header.Text + " " + "<a href=\"javascript:DoAction('view','project')\"><span class='hc_webpanela'>[view]</span></a>";
                }
                #endregion
                #region "Panel PLC"
                //Modified for CR 4516/4537 - Prabhu
                /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */
                //if (isUserItem
                //      && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                //      && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PLC)
                //      && !isMinimizedItem)
                if (isUserItem
                      && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                      && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PLC))
                    //&& SessionState.Culture.Code == "ww-en")
                {
                    int PLCUserId = PLC.PLCUser(SessionState.CurrentItem.Id);
                    if (PLCUserId > 0)
                    {
                        wPanelPLC.Header.Text = wPanelPLC.Header.Text + " " + "<a href=\"javascript:DoAction('edit','plc')\"><span class='hc_webpanela'>[edit]</span></a>";
                        //if (PLCUserId != string.Empty)
                        //{
                        //    wPanelPLC.Header.Text = wPanelPLC.Header.Text + "&nbsp;<a href=\"javascript:DoAction('deleteplc')\"><span class='hc_webpanela'>[delete]</span></a>";
                        //}
                    }
                    else
                    {

                        wPanelPLC.Header.Text = wPanelPLC.Header.Text + " " + "<a href=\"javascript:DoAction('view','plc')\"><span class='hc_webpanela'>[view]</span></a>";

                    }
                }
                else
                {

                    wPanelPLC.Header.Text = wPanelPLC.Header.Text + " " + "<a href=\"javascript:DoAction('view','plc')\"><span class='hc_webpanela'>[view]</span></a>";
                }
                //wPanelPLC.Header.Text = wPanelPLC.Header.Text + " " + "<a href=\"javascript:DoAction('view','plc')\"><span class='hc_webpanela'>[view]</span></a>";

                /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */

                #endregion
                #region "Panel Market Segment"
                if (isUserItem
                  && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                  && SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT)
                  && !isMinimizedItem)
                {
                    wPanelMarket.Header.Text = wPanelMarket.Header.Text + " " + "<a href=\"javascript:DoAction('edit','market')\"><span class='hc_webpanela'>[edit]</span></a>";
                }
                #endregion
                #region "Panel Publisher"
                if (isUserItem
                  && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                  && SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT)
                  && !isMinimizedItem
                  && SessionState.CurrentItem.Level.SkuLevel)
                {
                    wPanelPublisher.Header.Text = wPanelPublisher.Header.Text + " " + "<a href=\"javascript:DoAction('edit','publisher')\"><span class='hc_webpanela'>[edit]</span></a>";
                }
                #endregion
                #endregion
            }
        }
        private void HidePanels()
        {
            // Add by Mickael CHARENSOL (05/07/2006) for roll management
            if (SessionState.CurrentItem.IsRoll)
            {
                wPanelMarket.Visible = wPanelPublisher.Visible = wPanelPLC.Visible = false;
                PanelSort.Visible = false;
            }
            // Add by Mickael CHARENSOL (05/09/2006) for country specific
            if (SessionState.CurrentItem.IsCountrySpecific && SessionState.Culture.Type == CultureType.Master)
            {
                wPanelMarket.Visible = wPanelPublisher.Visible = wPanelPLC.Visible = wPanelProject.Visible = false;
            }
            //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
            if (SessionState.Culture.Type == CultureType.Master)
            {
                wPanelProject.Visible = false;
            }

            // Add by Mickael CHARENSOL (10/10/2006) for item at Option level
            if (IsSuperiorToSKULevel())
            {
                wPanelMarket.Visible = false;
                wPanelPublisher.Visible = false;
            }
            // Add by Mickael CHARENSOL (15/01/2006) for publisher (only sku level)
            if (!SessionState.CurrentItem.Level.SkuLevel)
            {
                wPanelPublisher.Visible = false;
            }
        }

        #region Project
        private void InitViewProject()
        {
            PanelViewProject.Visible = true;
            btnInstantTR.Visible = PanelProjectStatus.Visible = PanelEditProject.Visible = panelProjectLanguages.Visible = panelProjectTRs.Visible = false;
            //Modified by Sateesh for PCF1: Regional Project Management--Moving IsProject,TranslationMode properties to region level-- 18/June/09
            SessionState.CurrentItem.RegionCode = SessionState.Culture.Code;
            lbProjectInfoLanguages.Text = "None";
            lbProjectInfoLanguages.Visible = SessionState.CurrentItem.TranslationMode == TRClassTranslationMode.Standard;

            //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 20/May/09 -- Check
            //if (SessionState.CurrentItem.Milestones != null && SessionState.CurrentItem.Milestones.BeginningOfRegionalization.HasValue)
            if (SessionState.CurrentItem.Milestones != null && SessionState.CurrentItem.Milestones.CreationDate != null)
            {
                lbProjectInfo.CssClass = string.Empty;

                if (!SessionState.CurrentItem.Milestones.Inherited)
                {
                    lbProjectInfo.Text = "Yes";
                    if (SessionState.CurrentItem.Translations.Count > 0 && SessionState.CurrentItem.TranslationMode == TRClassTranslationMode.Standard)
                    {
                        lbProjectInfoLanguages.Text = string.Empty;

                        //Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009
                        HyperCatalog.Business.LanguageList languages = HyperCatalog.Business.Language.GetByCulture(HyperCatalog.Shared.SessionState.Culture.Code, string.Empty);
                        foreach (HyperCatalog.Business.Language lang in SessionState.CurrentItem.Translations)
                        {
                            // if current culture is a region then the language of this region are only displayed
                            if (ContainsLanguage(languages, lang))
                                lbProjectInfoLanguages.Text = lbProjectInfoLanguages.Text + lang.Code + ", ";
                        }

                        if (lbProjectInfoLanguages.Text.Length > 0)
                            lbProjectInfoLanguages.Text = lbProjectInfoLanguages.Text.Substring(0, lbProjectInfoLanguages.Text.Length - 2);
                        else
                            lbProjectInfoLanguages.Text = "None";

                        panelProjectLanguages.Visible = !SessionState.CurrentItem.Milestones.Inherited;
                    }
                    if (SessionState.CurrentItem.GetItemApplicableLanguages().Count > 0) // Languages can come from PLC or PL
                    {
                        if (SessionState.User.HasCapability(CapabilitiesEnum.INSTANT_TRANSLATE))
                        {
                            btnInstantTR.Visible = true;
                            //btnInstantTR.Attributes.Add("OnClick", "InstantTR(" + SessionState.CurrentItem.Id.ToString() + ");return false;");
                            
                            //PCF Requirement -- Regional project Management (Deepak.S)
                            btnInstantTR.Attributes.Add("OnClick", "InstantTR(" + SessionState.CurrentItem.Id.ToString() + ",'" + SessionState.Culture.Code + "');return false;");
                        }
                    }
                }
                else
                {
                    lbProjectInfo.Text = "Part of [" + SessionState.CurrentItem.Milestones.InheritedItem.FullName + "] project set at level [" + SessionState.CurrentItem.Milestones.InheritedItem.LevelId + "]";
                }

                PanelProjectStatus.Visible = true;
                lbProjectStatus.Text = SessionState.CurrentItem.Milestones.Status.ToString();
                if (lbProjectStatus.Text == "Region_Acquisition_Non_Initialized")
                {
                    lbProjectStatus.Text += ". Beginning of Regionalization (BOR) date has not reached for this item.";
                    lbProjectStatus.Font.Bold = true;
                    lbProjectStatus.ForeColor = Color.Red;
                }
                else
                {
                    lbProjectStatus.Font.Bold = false;
                    lbProjectStatus.ForeColor = Color.Black;
                }

                lbProjectCreationDate.Text = "Created on " + SessionState.User.FormatUtcDate(SessionState.CurrentItem.Milestones.CreationDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + " by <a href='mailto:" + UITools.GetDisplayEmail(SessionState.CurrentItem.Milestones.Creator.Email) + "?subject=" + SessionState.CurrentItem.FullName + " [#" + SessionState.CurrentItem.Id.ToString() + "]' title='Send Mail'>" + SessionState.CurrentItem.Milestones.Creator.FullName + " [" + SessionState.CurrentItem.Milestones.Creator.Organization.Name + "]</a>";
                lbProjectCreationDate.Visible = true;
                if (SessionState.CurrentItem.Milestones.TRs != null)
                {
                    if (SessionState.CurrentItem.Milestones.TRs.Count > 0)
                    {
                        lbCurrentOpenedTRs.Text = string.Empty;
                        panelProjectTRs.Visible = true;
                        foreach (TR tr in SessionState.CurrentItem.Milestones.TRs)
                        {
                            // PCF Requirement -- Only TRs applicable to that region should be displayed
                            if (SessionState.Culture.Code == tr.RegionCode)
                            {
                                lbCurrentOpenedTRs.Text += tr.Type.ToString() + ":<b><a href='javascript://' onclick=\"EditTR(" + tr.Id.ToString() + ")\">" + tr.Id.ToString() + "</b></a> ";
                                if (tr.InstantTR)
                                {
                                    lbCurrentOpenedTRs.Text += "<font color=red>{Instant TR}</font>";
                                }
                                lbCurrentOpenedTRs.Text += " [" + SessionState.User.FormatUtcDate(tr.CreationDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + "]<br>";
                            }
                        }
                        // PCF Requirement -- Only TRs applicable to that region should be displayed
                        if (lbCurrentOpenedTRs.Text == string.Empty)
                        {
                            lbCurrentOpenedTRs.Text += "None";
                        }
                    }
                }
            }
            else
            {
                if (SessionState.CurrentItem.Milestones.Inherited)
                {
                    lbProjectInfo.Text = "This item has projects in its descendants.";
                    lbProjectInfo.CssClass = string.Empty;
                    lbProjectCreationDate.Visible = false;
                }
                else
                {
                    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
                    lbProjectInfo.Text = "This item is not part of any project.";
                    lbProjectInfo.CssClass = "hc_error";
                    lbProjectCreationDate.Visible = false;
                }
            }

            if (!lbProjectInfoApplicableLanguages.Text.Equals(APPLIED_LANGUAGES))
            {
                lbProjectInfoApplicableLanguages.Text = "None";

                LanguageList applicableLanguages = SessionState.CurrentItem.GetItemApplicableLanguages();
                if (applicableLanguages.Count > 0)
                {
                    lbProjectInfoApplicableLanguages.Text = string.Empty;

                    HyperCatalog.Business.LanguageList languages = HyperCatalog.Business.Language.GetByCulture(HyperCatalog.Shared.SessionState.Culture.Code, string.Empty);
                    foreach (HyperCatalog.Business.Language lang in applicableLanguages)
                    {
                        // if current culture is a region then the language of this region are only displayed
                        if (ContainsLanguage(languages, lang))
                            lbProjectInfoApplicableLanguages.Text = lbProjectInfoApplicableLanguages.Text + lang.Code + ", ";
                    }

                    if (lbProjectInfoApplicableLanguages.Text.Length > 0)
                        lbProjectInfoApplicableLanguages.Text = lbProjectInfoApplicableLanguages.Text.Substring(0, lbProjectInfoApplicableLanguages.Text.Length - 2);
                    else
                        lbProjectInfoApplicableLanguages.Text = "None";
                }
            }
        }
        private void InitEditProject()
        {
            RetrieveItem();
            lbProjectInfoLanguages.Visible = true;
            btnInstantTR.Visible = false;
            PanelEditProject.Visible = true;
            ProjectInformationControl.Item = SessionState.CurrentItem;

            if (IsSuperiorToSKULevel()
              || !isUserItem
              || !SessionState.User.HasCultureInScope(SessionState.Culture.Code)
              || !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PROJECTS))
            {
                ProjectInformationControl.CanOverrideProject = false;
                ProjectInformationControl.Enabled = false;
            }
        }
        private bool InitDeleteProject()
        {
            RetrieveItem();
            if (SessionState.CurrentItem.Milestones.Delete(SessionState.User.Id))
            {
                //SessionState.CurrentItem.Milestones = null;
                Debug.Trace("[ItemUpdateWorkingTablesAsync] START", DebugSeverity.Low);

                SessionState.CurrentItem.Milestones = null;
                SessionState.CurrentItem.Dispose();

                lbProjectInfoApplicableLanguages.Text = APPLIED_LANGUAGES;

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateItemWorkingTable", "RefreshItemWorkingTable();", true);
                return true;
            }
            else
            {
                string msg = "'The project cannot be deleted!'";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert(" + msg + ");", true);
                if (ItemDates.LastError != string.Empty)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error2", "alert(\"" + ItemDates.LastError.Replace("\n", ".") + "\");", true);
                }
                return false;
            }
        }
        private bool InitSaveProject()
        {
            if (ProjectInformationControl.Enabled)
            {
                RetrieveItem();
                //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
                SessionState.CurrentItem.Milestones.RegionCode = SessionState.Culture.Code;
                bool isNewProject = SessionState.CurrentItem.Milestones.Inherited ? true : false;
                SessionState.CurrentItem.Milestones.BeginningOfRegionalization = (DateTime?)ProjectInformationControl.Eoa;
                SessionState.CurrentItem.Milestones.EndOfRegionalization = (DateTime?)ProjectInformationControl.Eov;
                SessionState.CurrentItem.Milestones.BeginningOfTranslation = (DateTime?)ProjectInformationControl.Bot;
                SessionState.CurrentItem.Milestones.EndOfTranslation = (DateTime?)ProjectInformationControl.Eot;
                if (SessionState.Culture.Type != CultureType.Master)
                {
                    // clear all languages of scope 
                    string filter = "LanguageCode <> dbo.GetLanguageCode('" + HyperCatalog.Shared.SessionState.Culture.Code + "') AND LanguageCode NOT IN (SELECT LanguageCode FROM Cultures Cu, Countries C WHERE Cu.CountryCode = C.CountryCode AND C.PLCDrivenTranslation = 1)";
                    LanguageList languages = Language.GetByCulture(HyperCatalog.Shared.SessionState.Culture.Code, filter);
                    if (languages != null)
                    {
                        int index = -1;
                        foreach (Language l in languages)
                        {
                            index = RetrieveIndex(l, SessionState.CurrentItem.Translations);
                            if (index > -1 && index < SessionState.CurrentItem.Translations.Count)
                            {
                                SessionState.CurrentItem.Translations.Remove(index);
                            }
                        }
                    }
                }
                else
                {
                    SessionState.CurrentItem.Translations.Clear();
                }
                SessionState.CurrentItem.IsProject = true;
                ListItemCollection lg = ProjectInformationControl.Languages;
                foreach (ListItem obj in lg)
                {
                    SessionState.CurrentItem.Translations.Add(HyperCatalog.Business.Language.GetByKey(obj.Value));
                }

                if (SessionState.CurrentItem.Save(SessionState.User.Id, false, false, false, true))
                {
                    SessionState.CurrentItem.Milestones = null;
                    SessionState.CurrentItem.Dispose();

                    lbProjectInfoApplicableLanguages.Text = APPLIED_LANGUAGES;

                    Debug.Trace("[ItemUpdateWorkingTablesAsync] START", DebugSeverity.Low);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateItemWorkingTable", "RefreshItemWorkingTable();", true);

                    if (SessionState.Culture.Type == HyperCatalog.Business.CultureType.Regionale)
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Warning", "alert(\"The Beginning of translation is modified for scope's languages\");", true);

                    if (isNewProject) // || ProjectInformationControl.IsNewProject)
                    {
                        // Reload Catego to show the new project
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTV", "ReloadTV('', 0);", true);
                    }
                    return true;
                }
                else
                {
                    string msg = "'The Item cannot be saved!'";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(" + msg + ");</script>");
                    if (Item.LastError != string.Empty || ItemDates.LastError != string.Empty)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error2", "<script>alert(\"" + Item.LastError.Replace("\n", ".") + "|" + ItemDates.LastError.Replace("\n", ".") + "\");</script>");
                    }
                    return false;
                }
            }
            return true;
        }

        private bool ContainsLanguage(LanguageList languages, Language language)
        {
            if (languages != null && language != null)
            {
                foreach (Language l in languages)
                {
                    if (l != null && l.Code.Equals(language.Code))
                        return true;
                }
            }
            return false;
        }
        private int RetrieveIndex(Language language, LanguageList languageList)
        {
            int index = -1;
            if (language != null && languageList != null)
            {
                for (int i = 0; i < languageList.Count; i++)
                {
                    Language l = languageList[i];
                    if (l.Code == language.Code)
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }
        #endregion
        #region market segment
        private void InitViewMarketSegments()
        {
            PanelViewMarketSegments.Visible = true;
            PanelEditMarketSegments.Visible = false;

            string fontFallback1 = "<font style='color:blue;font-style:italic;'>";
            string fontFallback2 = "<font style='color:red;font-style:italic;'>";
            string endFont = "</font>";

            string countryCode = string.Empty;
            string fallback1CountryCode = string.Empty;
            string fallback2CountryCode = string.Empty;
            if (SessionState.Culture != null)
            {
                countryCode = SessionState.Culture.CountryCode;
                if (SessionState.Culture.Fallback != null)
                {
                    fallback1CountryCode = SessionState.Culture.Fallback.CountryCode;
                    if (SessionState.Culture.Fallback.Fallback != null)
                        fallback2CountryCode = SessionState.Culture.Fallback.Fallback.CountryCode;
                }
            }

            #region Build market segment view
            #region Defined market segments
            string defMS = string.Empty;
            ItemMarketSegmentList definedItemMarketSegments = ItemMarketSegment.GetDefinedByItem(SessionState.CurrentItem.Id, countryCode);
            if (definedItemMarketSegments != null)
            {
                foreach (ItemMarketSegment ims in definedItemMarketSegments)
                {
                    if (ims.MarketSegment != null)
                    {
                        if (defMS.Length > 0)
                            defMS += ", ";
                        if (ims.CountryCode == countryCode)
                            defMS += ims.MarketSegment.Name;
                        else if (ims.CountryCode == fallback1CountryCode)
                            defMS += fontFallback1 + ims.MarketSegment.Name + endFont;
                        else if (ims.CountryCode == fallback2CountryCode)
                            defMS += fontFallback2 + ims.MarketSegment.Name + endFont;
                    }
                }
            }
            if (defMS.Length > 0)
                lbDefinedMarketSegment.Text = defMS;
            else
                lbDefinedMarketSegment.Text = "None";
            #endregion
            #region Applied market segments
            string appMS = string.Empty;
            ItemMarketSegmentList appliedItemMarketSegments = ItemMarketSegment.GetAppliedByItem(SessionState.CurrentItem.Id, countryCode);
            if (appliedItemMarketSegments != null)
            {
                foreach (ItemMarketSegment ims in appliedItemMarketSegments)
                {
                    if (appMS.Length > 0)
                        appMS += ", ";
                    if (ims.CountryCode == countryCode)
                        appMS += ims.MarketSegment.Name;
                    else if (ims.CountryCode == fallback1CountryCode)
                        appMS += fontFallback1 + ims.MarketSegment.Name + endFont;
                    else if (ims.CountryCode == fallback2CountryCode)
                        appMS += fontFallback2 + ims.MarketSegment.Name + endFont;
                }
            }
            if (appMS.Length > 0)
                lbAppliedMarketSegment.Text = appMS;
            else
                lbAppliedMarketSegment.Text = "None";
            #endregion
            #endregion
        }
        private void InitEditMarketSegments()
        {
            PanelViewMarketSegments.Visible = false;
            PanelEditMarketSegments.Visible = true;

            #region Build market segment edition
            dg.Rows.Clear();
            using (MarketSegmentList marketSegments = MarketSegment.GetAll())
            {
                ItemMarketSegmentList itemMarketSegments = ItemMarketSegment.GetDefinedByItem(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode);
                if (itemMarketSegments != null && itemMarketSegments.Count > 0)
                {
                    foreach (ItemMarketSegment ims in itemMarketSegments)
                    {
                        if (ims != null && ims.CountryCode == SessionState.Culture.CountryCode)
                        {
                            cbNotInheritedMS.Checked = true;
                            break;
                        }
                    }
                }
                if (SessionState.Culture.Type == CultureType.Master)
                    cbNotInheritedMS.Text = "Not inherited market segments by inheritance mechanism";

                if (itemMarketSegments != null && marketSegments != null)
                {
                    string segmentChecked = string.Empty;
                    string countryCode = string.Empty;
                    foreach (MarketSegment ms in marketSegments)
                    {
                        segmentChecked = string.Empty;
                        countryCode = string.Empty;
                        foreach (ItemMarketSegment sms in itemMarketSegments)
                        {
                            if (ms.Id == sms.MarketSegmentId)
                            {
                                countryCode = "<img alt='" + sms.CountryCode + "' src='/hc_v4/img/flags/" + sms.CountryCode + ".gif' />";
                                segmentChecked = " CHECKED";
                                break;
                            }
                        }
                        UltraGridRow newRow = new UltraGridRow(new object[] { ms.Name, "<center><input type='checkbox' onclick='UpdateMS(\"" + cbNotInheritedMS.ClientID + "\")' name='m_" + ms.Id.ToString() + "' id='m_" + ms.Id.ToString() + "'" + segmentChecked + "></center>", "<center>" + countryCode + "</center>" });
                        dg.Rows.Add(newRow);
                    }
                }
            #endregion
            }
        }
        private bool InitSaveMarketSegments()
        {
            RetrieveItem();
            bool isSavedMarketSegment = true;

            #region Retrieve market segment
            ItemMarketSegment imSpec = new ItemMarketSegment(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode, -1, false, SessionState.User.Id);
            imSpec.Delete(SessionState.User.Id);

            using (MarketSegmentList marketSegments = MarketSegment.GetAll())
            {
                ItemMarketSegmentList msl = ItemMarketSegment.GetAppliedByItem(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode);
                if (marketSegments != null && msl != null)
                {
                    int index = 0;
                    while (index < msl.Count)
                    {
                        ItemMarketSegment ms = msl[index];
                        if (ms != null && ms.CountryCode == SessionState.Culture.CountryCode)
                            ms.Delete(SessionState.User.Id);
                        msl.Remove(index);
                    }
                    int imsCount = 0;
                    for (int i = 0; i < marketSegments.Count; i++)
                    {
                        MarketSegment ms = marketSegments[i];
                        if (Request["m_" + ms.Id.ToString()] != null)
                        {
                            imsCount++;

                            ItemMarketSegment ims = new ItemMarketSegment(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode, ms.Id, Request["m_p_" + ms.Id.ToString()] != null, SessionState.User.Id);
                            if (!ims.Save(SessionState.User.Id))
                            {
                                isSavedMarketSegment = false;
                                break;
                            }
                        }
                    }
                    if (isSavedMarketSegment)
                    {
                        if (imsCount == 0 && cbNotInheritedMS.Checked)
                        {
                            ItemMarketSegment ims = new ItemMarketSegment(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode, -1, false, SessionState.User.Id);
                            if (!ims.Save(SessionState.User.Id))
                                isSavedMarketSegment = false;
                        }

                        if (isSavedMarketSegment)
                            ItemMarketSegment.UpdateAllItemMarketSegments(SessionState.CurrentItem.Id);
                    }
                }

                if (isSavedMarketSegment)
                {
                    HyperCatalog.Shared.SessionState.CurrentItem.Dispose();

                    return true;
                }
                else
                {
                    string msg = "'The Item cannot be saved! - ";
                    if (!isSavedMarketSegment)
                        msg += ItemMarketSegment.LastError;
                    msg += "'";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(" + msg + ");</script>");
                    return false;
                }
            }
            #endregion
        }
        #endregion
        #region publishers
        private void InitViewPublishers()
        {
            PanelViewPublishers.Visible = true;
            PanelEditPublishers.Visible = false;

            string fontFallback1 = "<font style='color:blue;font-style:italic;'>";
            string fontFallback2 = "<font style='color:red;font-style:italic;'>";
            string endFont = "</font>";

            string countryCode = string.Empty;
            string fallback1CountryCode = string.Empty;
            string fallback2CountryCode = string.Empty;
            if (SessionState.Culture != null)
            {
                countryCode = SessionState.Culture.CountryCode;
                if (SessionState.Culture.Fallback != null)
                {
                    fallback1CountryCode = SessionState.Culture.Fallback.CountryCode;
                    if (SessionState.Culture.Fallback.Fallback != null)
                        fallback2CountryCode = SessionState.Culture.Fallback.Fallback.CountryCode;
                }
            }

            #region Build publisher view
            if (SessionState.CurrentItem.Level.SkuLevel)
            {
                lbP.Text = "<br>";
                lbP.Text += "<table border='1' cellpadding='1' style='border-collapse:collapse;width:90%' cellspacing='0'>";

                #region Publishers
                string sPublishers = string.Empty;
                ItemPublisherList ipl = ItemPublisher.GetPublishersByItemId(SessionState.CurrentItem.Id, countryCode);
                lbP.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Publishers</span></td>";

                if (ipl != null)
                {
                    string name = string.Empty;
                    string regionCode = string.Empty;
                    foreach (ItemPublisher ip in ipl)
                    {
                        if (ip != null)
                        {
                            if (ip.Publisher != null)
                            {
                                name = ip.Publisher.Name;
                                regionCode = ip.CountryCode;

                                if (sPublishers.Length > 0)
                                    sPublishers += ", ";
                                if (regionCode == countryCode)
                                    sPublishers += name;
                                else if (regionCode == fallback1CountryCode)
                                    sPublishers += fontFallback1 + name + endFont;
                                else if (regionCode == fallback2CountryCode)
                                    sPublishers += fontFallback2 + name + endFont;
                            }
                        }
                    }
                }

                if (sPublishers.Length > 0)
                    lbP.Text += "<td class='editValueCell'>" + sPublishers + "</td>";
                else
                    lbP.Text += "<td class='editValueCell'>None</td>";
                lbP.Text += "</tr>";
                #endregion

                lbP.Text += "</table>";
                lbP.Visible = true;
            }
            else
            {
                lbP.Visible = false;
            }
            #endregion
        }
        private void InitEditPublishers()
        {
            PanelViewPublishers.Visible = false;
            PanelEditPublishers.Visible = true;

            #region Build publishers edition
            if (SessionState.CurrentItem.Level.SkuLevel)
            {
                dgEditPublisher.Rows.Clear();
                string filterPublishers = string.Empty;
                if (SessionState.Culture.Type == CultureType.Locale)
                {
                    filterPublishers = "ExcludeFromCountries = 0";
                }
                using (PublisherList publishers = Publisher.GetAll(filterPublishers))
                {
                    ItemPublisherList ipl = ItemPublisher.GetPublishersByItemId(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode);
                    if (ipl != null && ipl.Count > 0)
                    {
                        foreach (ItemPublisher ip in ipl)
                        {
                            if (ip != null && ip.CountryCode == SessionState.Culture.CountryCode)
                            {
                                cbNotInheritedP.Checked = true;
                                break;
                            }
                        }
                    }

                    string regionCode = string.Empty;
                    string publisherChecked = string.Empty;
                    int curPublisherId = -1;
                    foreach (Publisher p in publishers)
                    {
                        publisherChecked = string.Empty;
                        regionCode = string.Empty;
                        if (ipl != null)
                        {
                            foreach (ItemPublisher ip in ipl)
                            {
                                curPublisherId = ip.PublisherId;
                                if (p.Id == curPublisherId)
                                {
                                    regionCode = "<img alt='" + ip.CountryCode + "' src='/hc_v4/img/flags/" + ip.CountryCode + ".gif' />";
                                    publisherChecked = " CHECKED";
                                    break;
                                }
                            }
                        }
                        UltraGridRow newRow = new UltraGridRow(new object[] { p.Name, "<center><input type='checkbox' onclick='UpdateP(\"" + cbNotInheritedP.ClientID + "\")' name='p_" + p.Id.ToString() + "' id='p_" + p.Id.ToString() + "'" + publisherChecked + "></center>", "<center>" + regionCode + "</center>" });
                        dgEditPublisher.Rows.Add(newRow);
                    }

                    if (SessionState.Culture.Type == CultureType.Master)
                        cbNotInheritedP.Visible = false;
                }
            }
            else
            {
                dgEditPublisher.Visible = false;
                cbNotInheritedP.Visible = false;
            }
            #endregion
        }
        private bool InitSavePublishers()
        {
            RetrieveItem();
            bool isSavedPublishers = true;

            #region Retrieve publishers
            if (SessionState.CurrentItem.Level.SkuLevel)
            {
                using (PublisherList publishers = Publisher.GetAll())
                {
                    if (publishers != null)
                    {
                        // Delete all publishers
                        foreach (Publisher p in publishers)
                        {
                            if (p != null)
                            {
                                isSavedPublishers = SessionState.CurrentItem.DeletePublisher(p.Id, SessionState.Culture.CountryCode, SessionState.User.Id);
                                if (!isSavedPublishers)
                                    break;
                            }
                        }
                        SessionState.CurrentItem.DeletePublisher(-1, SessionState.Culture.CountryCode, SessionState.User.Id);

                        // Add new publishers
                        int ipCount = 0;
                        foreach (Publisher p in publishers)
                        {
                            if (Request["p_" + p.Id.ToString()] != null)
                            {
                                ipCount++;
                                isSavedPublishers = SessionState.CurrentItem.SavePublisher(p.Id, SessionState.Culture.CountryCode, SessionState.User.Id);
                                if (!isSavedPublishers)
                                    break;
                            }
                        }

                        if (isSavedPublishers)
                        {
                            if (ipCount == 0 && cbNotInheritedP.Checked)
                                isSavedPublishers = SessionState.CurrentItem.SavePublisher(-1, SessionState.Culture.CountryCode, SessionState.User.Id);
                            ItemPublisher.UpdateAllItemPublishers(SessionState.CurrentItem.Id);
                        }
                    }
                }
            }
            #endregion

            if (isSavedPublishers)
            {
                HyperCatalog.Shared.SessionState.CurrentItem.Dispose();
                HyperCatalog.Shared.SessionState.User.ItemCulturesRelevant = null;

                return true;
            }
            else
            {
                string msg = "'The Item cannot be saved! - ";
                if (!isSavedPublishers)
                    msg += Item.LastError;
                msg += "'";

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert('" + msg + "');</script>");
                return false;
            }
        }
        #endregion
        #region Identity Panel
        private void InitViewIdentity()
        {
            PanelViewIdentity.Visible = true;
            panelCrossSell.Visible = panelCountrySpecific.Visible = lbCountrySpecific.Visible = lbIsCrossSell.Visible = btnCountrySpecific.Visible = btnCrossSell.Visible = false;
            PanelEditIdentity.Visible = PanelSku.Visible = PanelOption.Visible = PanelMasterName.Visible = wPanelPLC.Visible = PanelItemType.Visible = PanelBundleMainComponent.Visible = false;
            lbError.Visible = false;

            lbItemName.Text = "<table border='1' cellpadding='1' style='border-collapse:collapse;width:90%' cellspacing='0'>";
            lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Item name</span></td><td class='editValueCell'>";

            string name = SessionState.CurrentItem.Name;
            if (SessionState.Culture.Type != CultureType.Master)
            {
                if (localizedProductNameChunk == null)
                    localizedProductNameChunk = HyperCatalog.Business.Chunk.GetByKey(SessionState.CurrentItem.Id, 1, SessionState.Culture.Code);
                if (localizedProductNameChunk != null)
                    name = localizedProductNameChunk.Text;
            }

            if (SessionState.CurrentItem.Sku != string.Empty)
            {
                if (SessionState.CurrentItem.NodeOID.ToString() != null && SessionState.CurrentItem.NodeOID.ToString() != "-1")
                    lbItemName.Text += "<b>[" + SessionState.CurrentItem.Sku + "]</b> - " + name + " (#" + SessionState.CurrentItem.Id.ToString() + ") [PM OID: " + SessionState.CurrentItem.NodeOID.ToString() + "]</td></tr>";
                else
                    lbItemName.Text += "<b>[" + SessionState.CurrentItem.Sku + "]</b> - " + name + " (#" + SessionState.CurrentItem.Id.ToString() + ")</td></tr>";

                PanelSku.Visible = true;
                wPanelPLC.Visible = SessionState.CurrentItem.Level.SkuLevel;

                #region Item type
                lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Type</span></td>";
                lbItemName.Text += "<td class='editValueCell'>" + SessionState.CurrentItem.Type.Name + "</td></tr>";
                #endregion
                #region Retail only
                if (SessionState.CurrentItem.IsDeal)
                    lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Retail only</span></td><td class='editValueCell'>Yes</td></tr>";
                #endregion
                #region Product is minimized
                if (isMinimizedItem)
                    lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Product has been minimized</span></td><td class='editValueCell'>Yes</td></tr>";
                #endregion
                #region Product is frozen
                if (isFrozenItem)
                    lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Product has been frozen</span></td><td class='editValueCell'>Yes</td></tr>";
                #endregion
                #region Reference item
                if (SessionState.CurrentItem.RefItem != null)
                    lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Reference Item</span></td><td class='editValueCell'>" + SessionState.CurrentItem.RefItem.FullName + "</td></tr>";
                else if (SessionState.CurrentItem.RefItem == null && SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.BUNDLE))
                    lbItemName.Text = lbItemName.Text + "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Reference Item</span></td><td class='editValueCell'>none</td></tr>";
                #endregion
                #region Cross sell
                // Create or delete automatically
                if (SessionState.CurrentItem.IsCrossSellEnable()
                  && isUserItem
                  && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                  && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL)
                  && SessionState.CurrentItem.TypeId != ((int)ItemTypesEnum.BUNDLE))
                {
                    if (!SessionState.CurrentItem.IsCrossSell)
                        lbIsCrossSell.Text = "<br>Promote the <b>Cross Sell</b>";
                    else
                        lbIsCrossSell.Text = "<br>Remove the <b>Cross Sell</b>";
                    //Disabling Cross-Sell Promotion as a Part of req 4220
                    //panelCrossSell.Visible = lbIsCrossSell.Visible = btnCrossSell.Visible = true;
                    panelCrossSell.Visible = lbIsCrossSell.Visible = btnCrossSell.Visible = false;
                }
                #endregion
            }
            else
            {
                if (SessionState.CurrentItem.NodeOID.ToString() != null && SessionState.CurrentItem.NodeOID.ToString() != "-1")
                    lbItemName.Text += name + " (#" + SessionState.CurrentItem.Id.ToString() + ") [PM OID: " + SessionState.CurrentItem.NodeOID.ToString() + "]</td></tr>";
                else
                    lbItemName.Text += name + " (#" + SessionState.CurrentItem.Id.ToString() + ")</td></tr>";
            }

            #region ProductLines
            string plText = string.Empty, plChildText;
            if (SessionState.CurrentItem.Sku != string.Empty)
            {
                plText += SessionState.CurrentItem.ProductLineCode;
            }
            else
            {
                if (SessionState.CurrentItem.ChildProductLines.Count > 0)
                {
                    foreach (PL pl in SessionState.CurrentItem.ChildProductLines)
                    {
                        plText += pl.Code + ", ";
                    }
                    plText = plText.Substring(0, plText.Length - 2); // Remove leading ", "        

                }
            }
            if (plText != string.Empty)
            {
                lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold'><span>ProductLine</span></td><td class='editValueCell'>" + plText + "</td></tr>";
            }
            #endregion
            #region Country specific
            if (SessionState.CurrentItem.IsCountrySpecific)
            {
                lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold'><span>Country specific</span></td>";
                lbItemName.Text += "<td class='editValueCell'>Yes (" + SessionState.CurrentItem.CountrySpecific.Name + ")</td></tr>";
            }
            #endregion
            #region IsInitialized flag
            if (SessionState.Culture.Type == CultureType.Master)
            {
                // IsInialized flag
                lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold'><span>Initialized</span></td>";
                if (SessionState.CurrentItem.IsInitialized)
                    lbItemName.Text += "<td class='editValueCell'>Yes</td></tr>";
                else
                    lbItemName.Text += "<td class='hc_notinitialized'>No (Not visible at region level)</td></tr>";
            }
            #endregion
            #region Soft roll
            if (SessionState.CurrentItem.IsRoll && SessionState.CurrentItem.ItemRoll != null)
            {
                lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold'><span>Soft roll</span></td>";
                if (isUserItem
                  && SessionState.User.HasCultureInScope(SessionState.Culture.Code))
                    lbItemName.Text += "<td class='editValueCell'>Replacement date: <a href='javascript:openRoll(" + SessionState.CurrentItem.Id + ");' title='Modify the replacement date'>" + SessionState.CurrentItem.ItemRoll.ReplacementDate.Value.ToString(SessionState.User.FormatDate) + "</a></td></tr>";
                else
                    lbItemName.Text += "<td class='editValueCell'>Replacement date: " + SessionState.CurrentItem.ItemRoll.ReplacementDate.Value.ToString(SessionState.User.FormatDate) + "</td></tr>";
            }
            #endregion
            #region Deleted region
            if (SessionState.CurrentItem.PMDeleted.HasValue)
            {
                lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold'><span>Deleted product</span></td>";
                lbItemName.Text += "<td class='editValueCell'>Yes</td></tr>";
            }
            #endregion
            #region CountrySpecific
            if (SessionState.CurrentItem.IsCountrySpecific
              && SessionState.Culture.Type == CultureType.Master
              && isUserItem
              && SessionState.User.HasCultureInScope(SessionState.Culture.Code))
                panelCountrySpecific.Visible = lbCountrySpecific.Visible = btnCountrySpecific.Visible = true;
            #endregion

            //// Moueen Code for the SMO
            #region Company 
            string company = string.Empty;
   
            int defaultCompany = 0;
            using (Database dbObj = Utils.GetMainDB())
            {
                using (SqlDataReader rs = dbObj.RunSPReturnRS("[GetCompanyInfoFromItemId]",
                  new SqlParameter("@ItemId", SessionState.CurrentItem.Id),
                  new SqlParameter("@AlwaysDefault", defaultCompany)))
                {
                    if ((dbObj.LastError == string.Empty) && (rs.HasRows))
                    {
                        rs.Read();
                        company = rs["Company_Info"].ToString();

                    }
                }
            }

            lbItemName.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold'><span>Company</span></td><td class='editValueCell'>" + company + "</td></tr>";
            #endregion

            lbItemName.Text += "</table>";

            lbCreated.Text = "Created on " + SessionState.User.FormatUtcDate(SessionState.CurrentItem.CreateDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + " by <a href='mailto:" + UITools.GetDisplayEmail(SessionState.CurrentItem.Creator.Email) + "?subject=" + SessionState.CurrentItem.FullName + " [#" + SessionState.CurrentItem.Id.ToString() + "]' title='Send Mail'>" + SessionState.CurrentItem.Creator.FullName + " [" + SessionState.CurrentItem.Creator.Organization.Name + "]</a>";
            whoswho.Attributes.Add("OnClick", "OpenWhosWho(" + SessionState.CurrentItem.Id.ToString() + ",'" + SessionState.Culture.Code + "');return false;");
        }
        private void InitEditIdentity()
        {
            RetrieveItem();
            PanelEditIdentity.Visible = true;
            PanelViewIdentity.Visible = false;

            PanelItemType.Visible = false;
            PanelSku.Visible = false;
            PanelSort.Visible = false;
            if (SessionState.Culture.Type == CultureType.Master || SessionState.CurrentItem.IsCountrySpecific)
            {
                PanelSort.Visible = true;
                if (SessionState.CurrentItem.Level.SkuLevel)
                {
                    PanelItemType.Visible = true;
                    PanelSku.Visible = true;
                }
                if (IsSuperiorToSKULevel())
                {
                    txtSku.Text = string.Empty;
                    PanelOption.Visible = true;
                    lbSku.Text = SessionState.CurrentItem.Parent.Sku + " ";
                    txtOption.Text = SessionState.CurrentItem.Sku.Replace(SessionState.CurrentItem.Parent.Sku, string.Empty).Trim();

                }
            }

            using (HyperCatalog.Business.Container containerName = HyperCatalog.Business.Container.GetByKey(1))
            { // ItemName
                using (HyperCatalog.Business.Container containerSku = HyperCatalog.Business.Container.GetByKey(2))
                {
                    // ItemSku
                    #region Retrieve Item Name and Sku containers
                    string maxLenItemName = containerName.MaxLength.ToString();
                    string maxLenSku = containerSku.MaxLength.ToString();
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "rulename", "<script>strRuleName = '" + containerName.EntryRule.ToString() + "';</script>");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "rulesku", "<script>strRuleSku = '" + containerSku.EntryRule.ToString() + "';</script>");
                    #endregion
                    #region InitValues
                    txtProductName.Text = SessionState.CurrentItem.Name;
                    txtSku.Text = SessionState.CurrentItem.Sku;
                    cbRetailOnly.Checked = SessionState.CurrentItem.IsDeal;
                    #endregion
                    #region Init Text fields
                    txtProductName.TextMode = TextBoxMode.SingleLine;
                    if (Convert.ToInt32(maxLenItemName) > 0)
                    {
                        txtProductName.MaxLength = Convert.ToInt32(maxLenItemName);
                        txtProductName.Attributes.Add("onkeyup", "TrackCount(this,'textcount'," + maxLenItemName + ")");
                        txtProductName.Attributes.Add("onfocus", "TrackCount(this,'textcount'," + maxLenItemName + ")");
                        txtProductName.Attributes.Add("onkeypress", "LimitText(this," + maxLenItemName + ")");
                    }
                    txtSku.TextMode = TextBoxMode.SingleLine;
                    string cleanSKUJs = @"this.value=CleanSku(this.value, normalSku);"; // normalSku is defined in the HTML page
                    txtSku.Attributes.Add("onKeyPress", cleanSKUJs);
                    txtSku.Attributes.Add("onBlur", cleanSKUJs);
                    if (Convert.ToInt32(maxLenSku) > 0)
                    {
                        txtSku.MaxLength = Convert.ToInt32(maxLenSku);
                        txtSku.Attributes.Add("onKeyUp", cleanSKUJs + "TrackCount(this,'textcountSku'," + maxLenSku + ")");
                        txtSku.Attributes.Add("onFocus", "TrackCount(this,'textcountSku'," + maxLenSku + ")");
                        txtSku.Attributes.Add("onKeyPress", cleanSKUJs + "LimitText(this," + maxLenSku + ");");
                    }
                    #endregion
                }
            }
            #region Regionalized or translated name
            // If Culture is not master, show the original product name value
            if (SessionState.Culture.Type != CultureType.Master)
            {
                lbMasterName.Text = SessionState.CurrentItem.Name;
                txtProductName.Text = string.Empty;
                if (localizedProductNameChunk != null)
                {
                    txtProductName.Text = localizedProductNameChunk.Text;
                }
                if (SessionState.Culture.Language.Rtl)
                {
                    txtProductName.CssClass = "hc_rtledit";
                }
                PanelMasterName.Visible = true;
            }
            #endregion
            #region Sort
            SortInfoControl.Item = SessionState.CurrentItem.Parent;
            SortInfoControl.SelectItem(SessionState.CurrentItem.Id);
            #endregion
            if (SessionState.CurrentItem.Level.SkuLevel)
            {
                using (ItemList siblingItems = SessionState.CurrentItem.GetPossibleMainComponents(SessionState.Culture.Code))
                {
                    #region Init item type
                    // Retrieve all item type
                    string filter = string.Empty;
                    if (SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.STANDARD))
                    {
                        if (siblingItems == null || siblingItems.Count == 0 || SessionState.CurrentItem.IsRoll || SessionState.CurrentItem.GetRoll() != null)
                        {
                            // standard --> top value, third party, customer specific, standard
                            filter += " ItemTypeId NOT IN (" + ((int)ItemTypesEnum.BUNDLE).ToString() + ")";
                        }
                        // else standard --> top value, third party, customer specific, bundle, standard
                    }
                    else if (SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.CUSTOMER_SPECIFIC)
                             || SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.THIRD_PARTY)
                             || SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.TOP_VALUE))
                    //|| SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.BUNDLE))
                    {
                        // customer specific --> customer specific, standard
                        // third party --> third party, standard
                        // top value --> top value, standard
                        // bundle --> bundle, standard
                        filter += " ItemTypeId IN (" + SessionState.CurrentItem.TypeId.ToString() + ", " + ((int)ItemTypesEnum.STANDARD).ToString() + ")";
                    }
                    else
                    {
                        // other case
                        filter += " ItemTypeId IN (" + SessionState.CurrentItem.TypeId.ToString() + ")";
                    }

                    using (ItemTypeList types = ItemType.GetAll(filter))
                    {
                        ddlItemType.DataSource = types;
                        ddlItemType.DataTextField = "Name";
                        ddlItemType.DataValueField = "Id";
                        ddlItemType.DataBind();
                        ddlItemType.Enabled = (ddlItemType.Items.Count > 1);

                        // Select type for this current item
                        string sItemTypeId = SessionState.CurrentItem.TypeId.ToString();
                        foreach (ListItem i in ddlItemType.Items)
                        {
                            if (i.Value.Equals(sItemTypeId))
                            {
                                i.Selected = true;
                                break;
                            }
                        }
                        #region Main component for bundle
                        // Display main component if necessary
                        ddlMainComponent.Items.Clear();
                        if (SessionState.CurrentItem.TypeId != ((int)ItemTypesEnum.BUNDLE))
                            ddlMainComponent.Items.Add(new ListItem("<---- Please Select ------>", ""));
                        if (siblingItems != null)
                        {
                            if (siblingItems.Count > 0)
                                siblingItems.Sort("Sort");

                            foreach (Item lstItem in siblingItems)
                            {
                                ListItem newItem = new ListItem(lstItem.FullName, lstItem.Id.ToString());
                                if (newItem.Text.Length > 50)
                                    newItem.Text = newItem.Text.Substring(0, 49) + "...";
                                ddlMainComponent.Items.Add(newItem);
                            }
                        }
                        ddlMainComponent.SelectedIndex = 0;
                        #endregion
                    }
                    // Display main component
                    DisplayMainComponent();
                    #endregion
                }
                #region Product Line
                PLList PLs = SessionState.User.PLs;
                using (CollectionView activePLs = new CollectionView(PLs))
                {
                    activePLs.ApplyFilter("IsActive", true, CollectionView.FilterOperand.Equals);
                    ddlPL.DataSource = activePLs;
                    ddlPL.DataTextField = "Code";
                    ddlPL.DataValueField = "Code";
                    ddlPL.DataBind();
                }
                //ddlPL.Items.Insert(0, new ListItem("", ""));
                if (SessionState.CurrentItem != null && SessionState.CurrentItem.ProductLineCode != null && ddlPL.Items.FindByValue(SessionState.CurrentItem.ProductLineCode) != null)
                {
                    ddlPL.SelectedValue = SessionState.CurrentItem.ProductLineCode;
                }
                #endregion

                if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS)
                  && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEM_TYPE_TO_CHANGE_IN_BUNDLE))
                {
                    txtProductName.Enabled = txtSku.Enabled = ddlPL.Enabled =
                      cbRetailOnly.Enabled = SortInfoControl.Enabled = false;
                }
            }
            //UI changes for HAA - Prabhu
            //Disabling few identity data as a part of PM Intergration of Crystal in 5.0
            if (SessionState.Culture.Type == CultureType.Master && SessionState.CurrentItem.NodeOID.ToString() != null && SessionState.CurrentItem.NodeOID.ToString() != "-1")
            {
                PanelOption.Visible = false;
                txtProductName.ReadOnly = true;
                txtSku.ReadOnly = true;
                ddlPL.Enabled = false;
                label11.Visible = false; // fix for QC 1776 - Prabhu
                SortInfoControl.Visible = false; // fix for QC 1776 - Prabhu
            }
            else if (SessionState.Culture.Type == CultureType.Regionale && SessionState.CurrentItem.NodeOID.ToString() != null && SessionState.CurrentItem.NodeOID.ToString() != "-1")
            {
                PanelOption.Visible = false;
                txtSku.ReadOnly = true;
                ddlPL.Enabled = false;
                label11.Visible = false; // fix for QC 1776 - Prabhu
                SortInfoControl.Visible = false; // fix for QC 1776 - Prabhu
            }
        }
        private void DisplayMainComponent()
        {
            PanelBundleMainComponent.Visible = false;
            if (ddlItemType.SelectedValue == Convert.ToString((int)ItemTypesEnum.BUNDLE) && ddlMainComponent.Items.Count > 0)
            {
                if (SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.BUNDLE) && SessionState.CurrentItem.RefItemId > 0)
                {
                    ListItem selectedItem = ddlMainComponent.Items.FindByValue(SessionState.CurrentItem.RefItemId.ToString());
                    if (selectedItem != null && ddlMainComponent.Items.IndexOf(selectedItem) >= 0)
                    {
                        ddlMainComponent.SelectedIndex = ddlMainComponent.Items.IndexOf(selectedItem);
                    }
                }
                ddlMainComponent.Visible = true;
                PanelBundleMainComponent.Visible = true;
            }
        }
        private bool InitSaveIdentity()
        {
            RetrieveItem();
            bool doSave = false;
            #region Item name for master culture
            if (localizedProductNameChunk == null && SessionState.CurrentItem.Name != txtProductName.Text && SessionState.Culture.Type == CultureType.Master)
            {
                SessionState.CurrentItem.Name = txtProductName.Text;
                doSave = true;
            }
            #endregion
            #region Item name for region
            if (localizedProductNameChunk == null && SessionState.CurrentItem.Name != txtProductName.Text
              && SessionState.Culture.Type != CultureType.Master && !SessionState.CurrentItem.IsCountrySpecific)
            {
                if (SessionState.CurrentItem.SaveItemName(txtProductName.Text, SessionState.Culture.Code, SessionState.User.Id))
                {
                    string itemName = txtProductName.Text;
                    if (SessionState.CurrentItem.Sku != null && SessionState.CurrentItem.Sku.Length > 0)
                        itemName = "[" + SessionState.CurrentItem.Sku + "] " + itemName;
                    if (itemName.Length > 50)
                        itemName = itemName.Substring(0, 49) + "...";
                    string hasRoll = "0";
                    if (SessionState.CurrentItem.IsRoll || SessionState.CurrentItem.GetRoll() != null)
                        hasRoll = "1";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTV", "<script>ReloadTV('" + itemName + "', " + hasRoll + ");</script>");
                    return true;
                }
                else
                {
                    string msg = "'The Item cannot be saved!";
                    msg += "'";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(" + msg + ");</script>");
                    return false;
                }
            }
            else if (localizedProductNameChunk != null && localizedProductNameChunk.Text != txtProductName.Text)
            {
                localizedProductNameChunk.Text = txtProductName.Text;
                if (localizedProductNameChunk.Save(SessionState.User.Id))
                {
                    string itemName = localizedProductNameChunk.Text;
                    if (SessionState.CurrentItem.Sku != null && SessionState.CurrentItem.Sku.Length > 0)
                        itemName = "[" + SessionState.CurrentItem.Sku + "] - " + itemName;
                    if (itemName.Length > 50)
                        itemName = itemName.Substring(0, 49) + "...";
                    string hasRoll = "0";
                    if (SessionState.CurrentItem.IsRoll || SessionState.CurrentItem.GetRoll() != null)
                        hasRoll = "1";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTV", "<script>ReloadTV('" + itemName + "', " + hasRoll + ");</script>");
                    return true;
                }
                else
                {
                    string msg = "'The Item cannot be saved!";
                    msg += "'";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(" + msg + ");</script>");
                    return false;
                }
            }
            #endregion
            #region Product number for master culture
            if (PanelSku.Visible)
            {
                if (SessionState.CurrentItem.Sku != txtSku.Text && !IsSuperiorToSKULevel())
                {
                    SessionState.CurrentItem.Sku = txtSku.Text;
                    doSave = true;
                }
                if (SessionState.CurrentItem.ProductLineCode != ddlPL.Text)
                {
                    SessionState.CurrentItem.ProductLineCode = ddlPL.Text;
                    doSave = true;
                }
                if (SessionState.CurrentItem.IsDeal != cbRetailOnly.Checked)
                {
                    SessionState.CurrentItem.IsDeal = cbRetailOnly.Checked;
                    doSave = true;
                }
            }
            if (PanelOption.Visible)
            {
                string newSku = lbSku.Text.Trim() + " " + txtOption.Text.Trim();
                if (SessionState.CurrentItem.Sku != newSku)
                {
                    SessionState.CurrentItem.Sku = newSku;
                    doSave = true;
                }
            }
            #endregion
            #region Item type for master culture
            // Item type is updated
            if (ddlItemType.Visible && !ddlItemType.SelectedValue.Equals(SessionState.CurrentItem.TypeId.ToString()))
            {
                SessionState.CurrentItem.TypeId = Convert.ToInt32(ddlItemType.SelectedValue);
                SessionState.CurrentItem.Type = null;
                if ((SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.BUNDLE)) && (ddlMainComponent.SelectedIndex > 0))
                {
                    SessionState.CurrentItem.RefItemId = Convert.ToInt32(ddlMainComponent.SelectedValue);
                }
                else if ((SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.BUNDLE)) && (ddlMainComponent.SelectedIndex == 0))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "<script>alert('Select main component');</script>");
                    return false;
                }
                doSave = true;
            }
            else if (SessionState.CurrentItem.TypeId == ((int)ItemTypesEnum.BUNDLE))
            {
                if (ddlMainComponent.Visible)
                {
                    SessionState.CurrentItem.RefItemId = Convert.ToInt32(ddlMainComponent.SelectedValue);
                }
                doSave = true;
            }
            #endregion
            #region Sort for master culture country
            if (SortInfoControl.HasChanges)
            {
                SortInfoControl.SaveSort();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTV", "<script>ReloadTV('', 0);</script>");
                doSave = true;
            }
            #endregion
            if (doSave)
            {
                if (SessionState.CurrentItem.Save(SessionState.User.Id, true, false, false, true))
                {
                    if (SessionState.CurrentItem.TypeId != ((int)ItemTypesEnum.BUNDLE) && !SessionState.CurrentItem.IsRoll)
                        SessionState.CurrentItem.RefItemId = -1;

                    #region Refresh new item name
                    string itemName = SessionState.CurrentItem.FullName;
                    if (itemName.Length > 50)
                        itemName = itemName.Substring(0, 49) + "...";
                    string hasRoll = "0";
                    if (SessionState.CurrentItem.IsRoll || SessionState.CurrentItem.GetRoll() != null)
                        hasRoll = "1";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RefreshTV", "<script>ReloadTV('" + itemName + "', " + hasRoll + ");</script>");
                    #endregion

                    HyperCatalog.Shared.SessionState.CurrentItem.Dispose();
                    return true;
                }
                else
                {
                    string msg = "'The Item cannot be saved!";
                    if (PanelSku.Visible)
                    {
                        msg += "\\nThis Sku may already exist";
                    }
                    msg += "'";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(" + msg + ");</script>");
                    return false;
                }
            }
            return true;
        }

        protected void ddlItemType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Display main component
            DisplayMainComponent();
        }
        protected void btnCrossSell_Click(object sender, System.EventArgs e)
        {
            RetrieveItem();
            if (SessionState.CurrentItem != null)
            {
                // IsCrossSell = true: create cross sell links, false: delete cross sell links
                SessionState.CurrentItem.IsCrossSell = !SessionState.CurrentItem.IsCrossSell;

                if (SessionState.CurrentItem.IsCrossSell)
                {
                    // Promote Cross Sell
                    if (!Link.AddDelCrossSell(SessionState.CurrentItem.Id, SessionState.CurrentItem.IsCrossSell))
                    {
                        lbError.CssClass = "hc_error";
                        lbError.Text = Link.LastError;
                        lbError.Visible = true;
                    }
                    else
                    {
                        // Update label and button for Cross Sell
                        lbIsCrossSell.Text = "<br><br>Remove the <b>Cross Sell</b>";

                        // Save flag IsCrossSell
                        if (SessionState.CurrentItem.Save(SessionState.User.Id, true))
                        {
                            HyperCatalog.Shared.SessionState.CurrentItem.Dispose();
                            lbError.CssClass = "hc_success";
                            lbError.Text = "All Cross Sell are created!";
                            lbError.Visible = true;
                        }
                    }
                }
                else
                {
                    // Remove Cross Sell
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenPopup", "<script>OpenCrossSellPopup(" + SessionState.CurrentItem.Id + ", '" + SessionState.CurrentItem.IsCrossSell.ToString() + "')</script>");
                }
            }
        }
        protected void btnCountrySpecific_Click(object sender, EventArgs e)
        {
            if (SessionState.CurrentItem.Import(SessionState.User.Id))
            {
                lbError.CssClass = "hc_error";
                lbError.Text = Item.LastError;
                lbError.Visible = true;
            }
            else
            {
                SessionState.CurrentItem = null;
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadMain", "<script>ReloadMain();</script>");
            }
        }
        #endregion
        #region PLC
        private void InitViewPLC()
        {
            lbPLCInformation.Visible = true;
            lbPLCInformation.CssClass = "";
            PanelPLC.Visible = false;
            if (SessionState.CurrentItem.PLCDates.Count > 0)
            {
                lbPLCInformation.Text = "PLC is set for [";
                using (HyperCatalog.Business.CountryList countries = HyperCatalog.Business.Country.GetByCulture(HyperCatalog.Shared.SessionState.Culture.Code, string.Empty))
                {
                    foreach (HyperCatalog.Business.PLC plc in SessionState.CurrentItem.PLCDates)
                    {
                        if (HyperCatalog.Shared.SessionState.Culture.Code == "ww-en")
                            lbPLCInformation.Text = lbPLCInformation.Text + plc.CountryCode + ", ";
                        else if (ContainsCountry(countries, plc.CountryCode))
                            lbPLCInformation.Text = lbPLCInformation.Text + plc.CountryCode + ", ";
                    }
                }

                if (HyperCatalog.Shared.SessionState.Culture.Code != "ww-en" && SessionState.CurrentItem.NodeOID > 0)
                    lbPLCInformation.Text = lbPLCInformation.Text + HyperCatalog.Shared.SessionState.Culture.CountryCode + ", ";

                if (lbPLCInformation.Text.Length > 0)
                    lbPLCInformation.Text = lbPLCInformation.Text.Substring(0, lbPLCInformation.Text.Length - 2) + "]";
                else
                {
                    lbPLCInformation.Text = "No PLC for this Item!";
                    lbPLCInformation.CssClass = "hc_error";
                }
            }
            else
            {
                lbPLCInformation.Text = "No PLC for this Item!";
                lbPLCInformation.CssClass = "hc_error";
            }
        }

        /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */

        private void InitViewPLCModifier()
        {
            string str;
            str = PLC.PLCLastModifierUser(SessionState.CurrentItem.Id, SessionState.Culture.CountryCode);
            if (SessionState.CurrentItem.Level.SkuLevel)
            {
                if (str.Length > 0)
                {
                    if (str == "Administrator  SYSTEM")
                        lbPLCModifiedBy.Text = "PLC Last modified by: PMG";
                    else
                        lbPLCModifiedBy.Text = "PLC Last modified by:" + str;
                }
            }
            else
                lbPLCModifiedBy.Visible = false;

        }

        

        
        private void InitEditPLC()
        {


            RetrieveItem();
            //wPanelPLC.Header.Text = wPanelPLC.Header.Text.Replace("save", "edit");
            lbPLCInformation.Visible = false;
            PanelPLC.Visible = true;
            PIC.Item = SessionState.CurrentItem;
            if (!SessionState.User.HasCultureInScope(SessionState.Culture.Code)
              || !isUserItem
             || !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PLC))
                //|| SessionState.Culture.Code != "ww-en")
            {
                PIC.Enabled = false;

            }

        }



        private void InitViewDummyPLC()
        {


            RetrieveItem();
            //wPanelPLC.Header.Text = wPanelPLC.Header.Text.Replace("save", "edit");
            lbPLCInformation.Visible = false;
            PanelPLC.Visible = true;
            PIC.Item = SessionState.CurrentItem;
            //if (!SessionState.User.HasCultureInScope(SessionState.Culture.Code)
            //  || !isUserItem
            // || !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PLC) || SessionState.Culture.Code != "ww-en" || PLCReadonly == 1)
            //{
            PIC.Enabled = false;

            //}

        }

        /* RQ 4246 Noverber 27, 2007 Dummy Sku PLC Modifier Label Starts from here */


        private bool InitSavePLC()
        {
            RetrieveItem();
            string msg = string.Empty;
            switch (PIC.ValidatePLC())
            {
                case PLCErrorEnum.CorruptedData:
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(\"The PLC values you've provided can't be saved\\nSee tooltips for more info.\");</script>");
                    return false;
                
                // Commented below code for Dummy SKU PLC
                //case PLCErrorEnum.NoDates:
                    //msg = "'You Must provide at least one valid date'"; ;
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "<script>alert(" + msg + ");</script>");
                    //return false;
                // Commented below code for Dummy SKU PLC
                
                default: //  PLCErrorEnum.None
                    SessionState.CurrentItem.PLCDates.Clear();
                    foreach (PLC plc in PIC.AddUpdatePLCList)
                    {
                        SessionState.CurrentItem.PLCDates.Add(plc);
                    }
                    PIC.AddUpdatePLCList.Dispose();
                    foreach (PLC plc in PIC.DeletedPLCList)
                    {
                        plc.ItemId = SessionState.CurrentItem.Id;
                        plc.Delete(SessionState.User.Id);
                    }
                    PIC.DeletedPLCList.Dispose();

                    if (SessionState.CurrentItem.Save(SessionState.User.Id, false, true, false, true))
                    {
                        foreach (PLC plc in PIC.UnchangedPLCList)
                        {
                            SessionState.CurrentItem.PLCDates.Add(plc);
                        }
                        PIC.UnchangedPLCList.Dispose();
                        Item.UpdateAllItemStatuses(SessionState.CurrentItem.Id);
                        HyperCatalog.Shared.SessionState.CurrentItem.Dispose();

                        lbProjectInfoApplicableLanguages.Text = APPLIED_LANGUAGES;

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateItemPublishers", "RefreshPublishers();", true);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateItemWorkingTable", "RefreshItemWorkingTable();", true);

                        return true;
                    }
                    else
                    {
                        msg = "'PLC cannot be saved:" + Item.LastError.Replace(Environment.NewLine, " - ").Replace("'", "\"") + "!'";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert(" + msg + ");", true);

                        foreach (PLC plc in PIC.UnchangedPLCList)
                        {
                            SessionState.CurrentItem.PLCDates.Add(plc);
                        }

                        PIC.UnchangedPLCList.Dispose();
                        return false;
                    }
            }
        }

        private bool ContainsCountry(CountryList countries, string countryCode)
        {
            return countries[countryCode] != null;
        }
        #endregion
    }
}

