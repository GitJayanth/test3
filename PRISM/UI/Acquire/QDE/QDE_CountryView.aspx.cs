/// Change History
/// Modified 22-Oct-2007 Vinay Bhandari
///             - Changed to disable the exclude check box is parent is excluded.
/// Modified 25-Oct-2007 Deepak S
///             - Included the Freeze/Minimize information in status field


#region Uses
using System;
using System.Xml;
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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
    /// <summary>
    /// Description résumée de QDE_CountryView.
    /// </summary>
    public partial class QDE_CountryView : HCPage
    {
        #region Constantes
        private const int CROSS_SELL_ID = 4;
        #endregion

        #region Declarations
        private string view = "all";
        private Item item;
        private int levelId;

        private string currentContainerGroup = string.Empty;
        private string currentContainerType = string.Empty;
        private string currentItemName = string.Empty;

        //Added for Enhancement for the BUG#70135 
        private string inputFormId;
        //Added for Enhancement for the BUG#70135 

        private int groupContainerTypeCount = 0;
        private int groupContainerGroupCount = 0;
        private int groupItemNameCount = 0;
        bool linkTypesAreInitialized = false;

        private string typeMessaging = "Messaging";
        private string photoContainerTag = SessionState.CacheParams["Localization_PhotoDisplayed"].Value.ToString();

        private int SuppliesId, ServicesId, CrossSellId, BundlesId; // Specific link types 
        #endregion

        #region Code généré par le Concepteur Web Form
        override protected void OnInit(EventArgs e)
        {
            txtFilter.AutoPostBack = false;
            base.OnInit(e);
        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["view"] != null)
            {
                view = Request["view"].ToLower().ToString();
            }
            if (Request["filter"] != null)
            {
                txtFilter.Text = Request["filter"].ToString();
            }

            // Added for Enhancement for the BUG#70135

            if (Request["f"] != null)
            {
                inputFormId = Request["f"].ToString(); // for example: IF_177
                inputFormId = inputFormId.Substring(3, inputFormId.Length - 3);
            }

            if (view.ToLower() == "all")
            {
                UITools.HideToolBarButton(uwToolbar, "Export");
            }
            if (view.ToLower() == "info")
            {

                uwToolbarExport.Visible = false;

            }

            // Added for Enhancement for the BUG#70135 




            using (item = QDEUtils.GetItemIdFromRequest())
            {
                levelId = item.LevelId;
                QDEUtils.UpdateCultureCodeFromRequest();

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitVars", "<script>itemId='" + item.Id + "';cultureCode='" + SessionState.Culture.Code + "';</script>");
                if (!Page.IsPostBack || Request["action"] != string.Empty)
                {
                    int linksStartLevel = Convert.ToInt32(HyperCatalog.Business.ApplicationSettings.Parameters["LinksStartLevel"].Value.ToString());
                    pInfo.Visible = pContent.Visible = pLinks.Visible = pCross.Visible = (view.ToLower() == "all");

                    //if (item.LevelId < linksStartLevel)
                    //    pLinks.Visible = pCross.Visible = false;

                    uwToolbar.Items.FromKeyButton("SingleView").Pressed(SessionState.tmFilterExpression == "1");
                    uwToolbar.Items.FromKeyButton("OnlyLocalizable").Pressed(SessionState.tmPageIndexExpression == "1");

                    Trace.Warn("*************** Start ******************");
                    SessionState.QDETab = view;
                    if ((view.ToLower() == "info") || (view.ToLower() == "all"))
                    {
                        pInfo.Visible = true;
                        Trace.Warn("~~~~~~~~~~ DisplayItemInfo");
                        DisplayItemInfo();
                        Trace.Warn("~~~~~~~~~~ RetrievePLC");
                        RetrievePLC();
                        Trace.Warn("~~~~~~~~~~ RetrieveMarketSegments");
                        RetrieveMarketSegments();
                        Trace.Warn("~~~~~~~~~~ RetrievePublishers");
                        RetrievePublishers();
                    }
                    if ((view.ToLower() == "content") || (view.ToLower() == "all"))
                    {
                        pContent.Visible = true;
                        Trace.Warn("~~~~~~~~~~ UpdateDataView");
                        UpdateDataView(); // only Localizable content
                        Trace.Warn("~~~~~~~~~~ RetrieveUPC");
                        RetrieveUPC();
                    }
                    if (!item.IsRoll)
                    {
                        //Modified for Links Requirement QC6373 - to display correctly the 'Overview' Tab by Prachi on 19th Feb 2013
                        //if (((view.ToLower() == "links") || (view.ToLower() == "all")) && item.Level.Id >= linksStartLevel)
                        if (view.ToLower() == "links" && item.Level.Id >= linksStartLevel)
                        {
                            //Kalai Links start Here
                            pLinks.Visible = true;
                           // pnlTypes.Visible = true;
                           // InitTabs();
                            Response.Redirect("../Links/Links_main.aspx");
                            Trace.Warn("~~~~~~~~~~ UpdateLinksDataView");
                          // UpdateLinksDataView(); // Display links     
                        }
                        //if (((view.ToLower() == "cross") || (view.ToLower() == "all")) && item.Level.Id >= linksStartLevel)
                        //{
                        //    pCross.Visible = true;
                        //    Trace.Warn("~~~~~~~~~~ UpdateCrossSellDataView");
                        //    UpdateCrossSellDataView(); // Display cross sell
                        //}

                        //Kalai Links end here

                        //Added for Links Requirement QC6373 - to display correctly the 'Overview' Tab by Prachi on 19th Feb 2013
                        if (view.ToLower() == "all" && item.Level.Id >= linksStartLevel)
                        {
                            pLinks.Visible = true;
                            Trace.Warn("~~~~~~~~~~ UpdateLinksDataView");
                            UpdateLinksDataView(); // Display links

                            pCross.Visible = true;
                            Trace.Warn("~~~~~~~~~~ UpdateCrossSellDataView");
                            UpdateCrossSellDataView(); // Display cross sell
                        }
                    }
                    else
                    {
                        dgCrossSell.Visible = lbCrossSell.Visible = Ultrawebtoolbar2.Visible =
                          dgl.Visible = lbLinks.Visible = Ultrawebtoolbar1.Visible =
                          dgMS.Visible = dgP.Visible = uwtoolbarMS.Visible = lbMS.Visible =
                          cbNotInheritedMS.Visible = cbNotInheritedP.Visible =
                          uwtoolbarP.Visible = lbP.Visible = false;
                    }
                    Trace.Warn("*************** End ******************");
                }
                else
                {
                    // action after changes in PLC edit window 
                    if (Request["action"] != null && Request["action"].ToString().ToLower() == "reload")
                    {
                        RetrievePLC();
                    }
                }

                #region Check buttons if item frozen or minmized
                if (item.IsMinimizedByCulture(SessionState.Culture.Code)
                  || item.IsFrozenByCulture(SessionState.Culture.Code))
                {
                    uwToolbarStatus.Visible = Ultrawebtoolbar1.Visible = Ultrawebtoolbar2.Visible = false;
                }
                #endregion

                #region Check button "save" for market segments and publishers
                if (!SessionState.CurrentItemIsUserItem
                  || !SessionState.User.HasCapability(CapabilitiesEnum.LOCALIZE_CHUNKS)
                  || item.IsMinimizedByCulture(SessionState.Culture.Code)
                  || item.IsFrozenByCulture(SessionState.Culture.Code))
                {
                    uwtoolbarMS.Visible = uwtoolbarP.Visible = dgP.Visible = dgMS.Visible =
                      cbNotInheritedP.Visible = cbNotInheritedMS.Visible = false;
                    panelDefinedMS.Visible = panelDefinedP.Visible = true;
                }
                else
                {
                    UITools.ShowToolBarButton(uwtoolbarMS, "save");
                    UITools.ShowToolBarButton(uwtoolbarP, "save");
                    panelDefinedMS.Visible = panelDefinedP.Visible = false;
                    dgMS.Visible = dgP.Visible = cbNotInheritedMS.Visible = cbNotInheritedP.Visible = true;
                    if (!SessionState.Culture.Country.CanLocalizeMarketSegments)
                    {
                        lbErrorP.CssClass = "hc_error";
                        lbErrorP.Text = "The country '" + SessionState.Culture.Country.Name + "' cannot localize market segments";
                        uwtoolbarMS.Visible = dgMS.Visible = cbNotInheritedMS.Visible = false;
                        panelDefinedMS.Visible = true;

                    }
                    if (!SessionState.Culture.Country.CanLocalizePublishers)
                    {
                        lbErrorP.CssClass = "hc_error";
                        lbErrorP.Text = "The country '" + SessionState.Culture.Country.Name + "' cannot localize publishers";
                        uwtoolbarP.Visible = dgP.Visible = cbNotInheritedP.Visible = false;
                        panelDefinedP.Visible = true;
                    }
                }
                #endregion

                #region User read only
                if (SessionState.User.IsReadOnly)
                {
                    uwToolbarStatus.Items.FromKeyButton("Exclude").Enabled =
                      uwToolbarStatus.Items.FromKeyButton("Validate").Enabled =
                      uwToolbarStatus.Items.FromKeyButton("Unexclude").Enabled =
                      uwtoolbarMS.Items.FromKeyButton("save").Enabled =
                      uwtoolbarP.Items.FromKeyButton("save").Enabled =
                      Ultrawebtoolbar1.Items.FromKeyButton("Save").Enabled =
                      Ultrawebtoolbar2.Items.FromKeyButton("Add").Enabled =
                      Ultrawebtoolbar2.Items.FromKeyButton("Save").Enabled =
                      Ultrawebtoolbar2.Items.FromKeyButton("Delete").Enabled =
                      Ultrawebtoolbar2.Items.FromKeyButton("ResumeInheritance").Enabled = false;
                }
                #endregion

                txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");
            }
        }

        #region "Event methods"
        /// <summary>
        /// Initalize row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            string filter = txtFilter.Text.Trim();
            int currentLevelId = Convert.ToInt32(e.Row.Cells.FromKey("LevelId").Text);
            bool keep = true;
            bool isInherited = Convert.ToBoolean(e.Row.Cells.FromKey("Inherited").Value);
            int inheritanceMethodId = Convert.ToInt32(e.Row.Cells.FromKey("InheritanceMethodId").Text);
            bool isResource = Convert.ToBoolean(e.Row.Cells.FromKey("IsResource").Value);
            bool isBoolean = Convert.ToBoolean(e.Row.Cells.FromKey("IsBoolean").Value);
            bool readOnly = Convert.ToBoolean(e.Row.Cells.FromKey("ReadOnly").Value);
            bool Localizable = Convert.ToBoolean(e.Row.Cells.FromKey("Localizable").Value);
            string containerType = e.Row.Cells.FromKey("ContainerType").Text;
            string containerName = e.Row.Cells.FromKey("Label").Text;
            int containerId = Convert.ToInt32(e.Row.Cells.FromKey("ContainerId").Value);

            UltraGridCell vCell = e.Row.Cells.FromKey("Value");
            #region Display photo
            string photo = vCell.Text;
            string fullPath = "/hc_v4/img/ed_notfound.gif";
            // if chunk is resource, try do display it
            if (isResource && vCell.Text != string.Empty)
            {
                if (photo == HyperCatalog.Business.Chunk.BlankValue)
                {
                    vCell.Text = HyperCatalog.Business.Chunk.BlankText;
                    vCell.Style.CustomRules = string.Empty;
                }
                else
                {
                    try
                    {
                        fullPath = Business.ApplicationSettings.Components["DAM_Provider"].URI + "/" + photo + "?DAM_avoid404=1&DAM_culture=" + SessionState.Culture.Code;
                        vCell.Text = "<a href='" + fullPath + "' target='_blank'><img src='" + fullPath + "&thumbnail=1&size=40' title='" + vCell.Text + "' border=0/></a>";
                    }
                    catch (Exception ex)
                    {
                        vCell.Text = "<img src='" + fullPath + "' title='An exception occurred: " + ex.Message + "' border=0/>";
                        Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
                    }
                }
            }
            #endregion

            //#region Display Global photo
            //if (e.Row.Cells.FromKey("Tag").Text.IndexOf(photoContainerTag) >= 0)
            //{
            //  imgProduct.ImageUrl = fullPath;
            //  imgProduct.ToolTip = photo;
            //}
            //#endregion

            #region filter
            if (filter != string.Empty)
            {
                keep = false;
                foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in e.Row.Cells)
                {
                    if (!c.Column.Hidden && c.Value != null && c.Text != HyperCatalog.Business.Chunk.BlankValue
                         && (!isResource) && (c.Column.Key == "Label" || c.Column.Key == "Value"))
                    {
                        if (c.Text.ToLower().IndexOf(filter.ToLower()) >= 0)
                        {
                            c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
                            keep = true;
                        }
                    }
                }
            }
            #endregion
            if (!keep)
            {
                e.Row.Delete();
            }
            else
            {
                #region Update Index column (to navigate in chunk list)
                containerName = e.Row.Cells.FromKey("Label").Text;
                e.Row.Cells.FromKey("Index").Value = e.Row.Index;
                #endregion

                #region If RTL languages, ensure correct display
                if ((bool)e.Row.Cells.FromKey("Rtl").Value)
                    vCell.Style.CustomRules = "direction: rtl;";//unicode-bidi:bidi-override;";
                #endregion

                string linkLevel = currentLevelId.ToString();
                #region Add HyperLink for LevelId < skuLevel
                if (currentLevelId != levelId)
                {
                    string pId = e.Row.Cells.FromKey("ParentId").Text;
                    string tvAll = SessionState.TVAllItems ? "1" : "0";
                    linkLevel = "<a href='javascript://' onclick='jp(" + pId + ")'>" + linkLevel + "</a>";
                }
                #endregion

                #region Add LevelId to Label
                e.Row.Cells.FromKey("Label").Text = "[" + linkLevel + "] ";
                #endregion

                #region Container type
                if (currentContainerType != containerType)
                {
                    currentItemName = string.Empty;
                    currentContainerType = containerType;
                    groupContainerTypeCount++;
                }
                #endregion

                #region ContainerType = Messaging --> Group by ItemName
                if (containerType.ToLower() == typeMessaging.ToLower())
                {
                    string ItemName = e.Row.Cells.FromKey("ItemName").Text;
                    if (currentItemName != ItemName)
                    {
                        currentItemName = ItemName;
                        groupItemNameCount++;
                    }
                }
                #endregion

                #region ContainerGroup
                string containerGroup = e.Row.Cells.FromKey("Path").Text;
                if (currentContainerGroup != containerGroup)
                {
                    currentContainerGroup = containerGroup;
                    groupContainerGroupCount++;
                }
                #endregion
                // Use only for debugging purpose
                //vCell.Text = vCell.Text + "[containerType=" + groupContainerTypeCount.ToString() + ", containerGroup" + groupContainerGroupCount.ToString() + ", ItemNameCount=" + groupItemNameCount.ToString() + "]";

                #region "Display Edit Link in Container Name"
                //product number CANNOT be localized, even with ful privileges
                if ((Localizable || SessionState.User.HasCapability(CapabilitiesEnum.LOCALIZE_EVERYTHING)) && containerId != 2 && (!readOnly) && ((currentLevelId == levelId) || (inheritanceMethodId == 1) || (inheritanceMethodId == 2)))
                {
                    int rowIndex = e.Row.Index;
                    rowIndex += groupContainerTypeCount + groupItemNameCount + groupContainerGroupCount;
                    string containerNameLabel = "<a href='javascript://' onclick=\"ed('" + dg.ClientID + "', " + rowIndex.ToString() + ");\">" + containerName + "</a>";
                    e.Row.Cells.FromKey("Label").Text = e.Row.Cells.FromKey("Label").Text + containerNameLabel;
                }
                else
                {
                    e.Row.Cells.FromKey("Label").Text = e.Row.Cells.FromKey("Label").Text + containerName;
                }
                #endregion

                if (Localizable)
                {
                    e.Row.Cells.FromKey("Label").Text = e.Row.Cells.FromKey("Label").Text + " <img src='/hc_v4/img/l.gif'/>";
                }


                // Update Item column
                if (e.Row.Cells.FromKey("ItemId").Text != item.Id.ToString())
                    e.Row.Cells.FromKey("ItemId").Value = item.Id.ToString();

                vCell.Style.CssClass = "ptb3"; // by default
                vCell.Style.Wrap = true; // by default

                HyperCatalog.Business.CultureType cType = HyperCatalog.Business.Culture.GetCultureTypeFromInteger(Convert.ToInt32(e.Row.Cells.FromKey("CId").Value));
                #region Change font if already localized
                if (cType == HyperCatalog.Business.CultureType.Master)
                {
                    vCell.Style.ForeColor = Color.Red; // fallback master
                }
                if (cType == HyperCatalog.Business.CultureType.Regionale)
                {
                    vCell.Style.ForeColor = Color.Blue; // fallback region
                }
                if (cType == HyperCatalog.Business.CultureType.Locale)
                {
                    if (e.Row.Cells.FromKey("ModifierId").Value.ToString() == "0")
                    {
                        vCell.Style.ForeColor = Color.Green; // translated
                    }
                    else
                    {
                        vCell.Style.ForeColor = Color.Black; // Localized
                    }
                }
                #endregion
                // Ensure multiline is kept
                if (vCell.Value != null && !isResource)
                {
                    // BLANK Value is replace by Readable sentence
                    if (vCell.Text == HyperCatalog.Business.Chunk.BlankValue)
                    {
                        vCell.Text = HyperCatalog.Business.Chunk.BlankText;
                        vCell.Style.CustomRules = string.Empty;
                    }
                    else
                    {
                        if (isBoolean && vCell.Text != string.Empty)
                        {
                            vCell.Text = Convert.ToBoolean(vCell.Value) ? "Yes" : "No";
                        }
                        else
                        {
                            vCell.Text = UITools.HtmlEncode(vCell.Text);
                        }
                    }
                }
            }
        }
        protected void dgBundlePLC_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            e.Row.Cells.FromKey("ItemNumber").Text = "<a href='javascript://' onclick='jp(" + e.Row.Cells.FromKey("ItemId").Text + ")'>" + e.Row.Cells.FromKey("ItemNumber").Text + "</a>";

        }
        protected void uwtoolbarMS_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            switch (btn)
            {
                case "save":
                    {
                        SaveMS();
                        break;
                    }
            }
        }
        protected void uwtoolbarP_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            switch (btn)
            {
                case "save":
                    {
                        SaveP();
                        break;
                    }
            }
        }
        /// <summary>
        /// Action on Content Toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="be"></param>
        protected void uwToolbarContent_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            switch (btn)
            {
                case "onlylocalizable":
                    {
                        SessionState.tmPageIndexExpression = be.Button.Selected ? "1" : "0";
                        UpdateDataView();
                        break;
                    }
                case "singleview":
                    {
                        SessionState.tmFilterExpression = be.Button.Selected ? "1" : "0";
                        uwToolbar.Items.FromKeyButton("OnlyLocalizable").Pressed(true);
                        SessionState.tmPageIndexExpression = "1";
                        UpdateDataView();
                        break;
                    }

                //Added for Enhancement for the BUG#70135
                case "export":
                    {
                        int ifId = Convert.ToInt32(inputFormId);
                        ExportUtils.ExportGridOfFrameContentCountry(item.Id, uwToolbar.Items.FromKeyButton("OnlyLocalizable").Selected, uwToolbar.Items.FromKeyButton("SingleView").Selected, txtFilter.Text, this, dgUPC);
                        break;
                    }
                //Added for Enhancement for the BUG#70135

            }
        }
        /// <summary>
        /// Action on Status Toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="be"></param>
        protected void uwToolbarStatus_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            switch (btn)
            {
                case "unexclude":
                    {
                        if (UpdateStatus("R", SessionState.Culture.Code))
                        {
                            //Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009
                            Item.UpdateWorkingTables(item.Id,SessionState.Culture.Code, true);
                        }
                        break;
                    }
                case "validate":
                    {
                        using (PLC itemPLC = PLC.GetByKey(item.Id, SessionState.Culture.CountryCode))
                        {
                            if (itemPLC != null &&
                               (itemPLC.FullDate != null || itemPLC.BlindDate != null || itemPLC.AnnouncementDate != null))
                            {
                                UpdateStatus("C", SessionState.Culture.Code);
                                RetrievePLC();
                            }
                            else
                            {
                                Page.RegisterClientScriptBlock("alert", "<script>alert('You can not Validate this product, because the PLC is missing or invalid');</script>");
                            }
                        }
                        break;
                    }
                case "exclude":
                    {
                        if (UpdateStatus("E", SessionState.Culture.Code))
                        {
                            //Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009
                            Item.UpdateWorkingTables(item.Id,SessionState.Culture.Code, true);
                        }
                        break;
                    }
            }
        }

        //Added for Enhancement for the BUG#70135

        protected void uwToolbarExport_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            switch (btn)
            {
                case "overviewexport":
                    {
                        string fallback1CountryCode = string.Empty;
                        string fallback2CountryCode = string.Empty;
                        string countryCode = string.Empty;

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
                        ExportUtils.ExportGridOfFrameOverview(item.Id, countryCode, fallback1CountryCode, fallback2CountryCode, uwToolbar.Items.FromKeyButton("OnlyLocalizable").Selected, uwToolbar.Items.FromKeyButton("SingleView").Selected, txtFilter.Text, this, dgl, dgUPC, dgCrossSell);

                        break;
                    }

            }
        }

        //Added for Enhancement for the BUG#70135



        #endregion
        /// <summary>
  /// Retrieve all available links types in the database to build the form
  /// </summary>
  //private void InitTabs()
  //{
  //  lbErrorTab.Visible = false;

  //  Infragistics.WebUI.UltraWebTab.Tab newTab;
  //  webTab.Tabs.Clear();

  //  bool withLinkCount = false;
  //  if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT) != null
  //    && SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT).Value)
  //  {
  //    withLinkCount = true;
  //  }

  //  using (DataSet ds = SessionState.CurrentItem.GetLinkTypesCount(SessionState.Culture.Code, -1, withLinkCount))
  //  {
  //    if (Item.LastError.Length == 0)
  //    {
  //      if (ds != null && ds.Tables.Count > 0)
  //      {
  //        foreach (DataRow dr in ds.Tables[0].Rows)
  //        {
  //          string linkTypeId = dr["LinkTypeId"].ToString();
  //          string linkTypeName = dr["LinkTypeName"].ToString();
  //          string nbLinks = dr["NbLinks"].ToString();
  //          string linkFrom = dr["LinkFrom"].ToString();
  //          if (linkFrom.Equals("True"))
  //            linkFrom = "1";
  //          else
  //            linkFrom = "0";

  //          string icon = dr["Icon"].ToString();
  //          // Update title of the new tab
  //          string tabName = linkTypeName;
  //          if (withLinkCount)
  //          {
  //            tabName = tabName + " (" + nbLinks + ")";
  //          }
  //          if (SessionState.Culture.Type == CultureType.Master && linkTypeName == "CrossSell") //Added if else loop to disable CrossSell in Master - Prabhu - 8 Oct 07
  //          {
  //          }
  //          else
  //          {
  //              // Create new tab for this links
  //              newTab = new Infragistics.WebUI.UltraWebTab.Tab(tabName);
  //              // Update key of the new tab
  //              newTab.Key = "tb_" + linkTypeId;
  //              // Update image of the new tab
  //              if (icon.Length > 0)
  //                  newTab.DefaultImage = "/hc_v4/img/" + icon;
  //              // Update URL of the new tab
  //              newTab.ContentPane.TargetUrl = "Links_list.aspx?i=" + SessionState.CurrentItem.Id.ToString() + "&t=" + linkTypeId + "&f=" + linkFrom + "&c=" + SessionState.Culture.Code;
  //              // Add the new tab
  //              webTab.Tabs.Add(newTab);
  //          }
  //        }
  //      }

  //      // Tab name
  //      string tabContentName = "All links";
  //      if (withLinkCount)
  //      {
  //        tabContentName = tabContentName + " (" + Link.GetLinksCount(SessionState.CurrentItem.Id, SessionState.Culture.Code) + ")";
  //      }
  //      // Add Content tab (contains all links applicable at this node)
  //      newTab = new Infragistics.WebUI.UltraWebTab.Tab(tabContentName);
  //      // Update image of the new tab
  //      newTab.DefaultImage = "/hc_v4/img/ed_links.gif";
  //      // Update key of the new tab
  //      newTab.Key = "tb_Content";
  //      // Update URL of the new tab
  //      newTab.ContentPane.TargetUrl = "Links_content.aspx?i=" + SessionState.CurrentItem.Id.ToString();
  //      // Add the new tab
  //      webTab.Tabs.Add(newTab);

  //      webTab.Visible = true;
  //    }
  //  }
  //}

        #region "Private methods"
        /// <summary>
        /// Retrieve Item info
        /// </summary>
        private void DisplayItemInfo()
        {
            #region Display Global photo
            imgProduct.Visible = false;
            using (Database dbObj = Utils.GetMainDB())
            {
                using (SqlDataReader rs = dbObj.RunSPReturnRS("[_Item_GetChunk]",
                  new SqlParameter("@ItemId", item.Id),
                  new SqlParameter("@Tag", photoContainerTag),
                  new SqlParameter("@CultureCode", SessionState.Culture.Code)))
                {
                    if ((dbObj.LastError == string.Empty) && (rs.HasRows))
                    {
                        rs.Read();
                        string photo = rs["ChunkValue"].ToString();
                        string fullPath = "/hc_v4/img/ed_notfound.gif";
                        try
                        {
                            fullPath = Business.ApplicationSettings.Components["DAM_Provider"].URI + "/" + photo + "?DAM_avoid404=1&DAM_culture=" + SessionState.Culture.Code;
                            imgProduct.ImageUrl = fullPath;
                            imgProduct.ToolTip = photo;
                            imgProduct.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message);
                            Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
                        }
                        finally
                        {
                            rs.Close();
                        }
                    }
                }
            }
            #endregion

            lbItemInfo.Text = "<table border='1' cellpadding='1' style='border-collapse:collapse;width:100%' cellspacing='0'>";
            if (item.Sku != string.Empty)
            {
                lbItemInfo.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Type</span></td>";
                lbItemInfo.Text += "<td class='editValueCell'>" + item.Type.Name + "</td></tr>";

                // Retail only
                if (item.IsDeal)
                    lbItemInfo.Text = lbItemInfo.Text + "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Retail only</span></td><td class='editValueCell'>Yes</td></tr>";

                // Reference item
                if (item.RefItem != null)
                    lbItemInfo.Text = lbItemInfo.Text + "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Reference Item</span></td><td class='editValueCell'>" + item.RefItem.FullName + "</td></tr>";
            }
            #region ProductLines
            string plText = string.Empty, plChildText;
            if (item.Sku != string.Empty)
            {
                plText += item.ProductLineCode;
            }
            else
            {
                if (item.ChildProductLines.Count > 0)
                {
                    foreach (PL pl in item.ChildProductLines)
                    {
                        plText += pl.Code + ", ";
                    }
                    plText = plText.Substring(0, plText.Length - 2); // Remove leading ", "        

                }
            }
            if (plText != string.Empty)
            {
                lbItemInfo.Text = lbItemInfo.Text + "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>ProductLine</span></td><td class='editValueCell'>" + plText + "</td></tr>";
            }
            #endregion

            #region Product status
            uwToolbarStatus.Visible = false;
            if (item.Sku != string.Empty)
            {
                string classStatus = "hc_initialized";
                string s = item.GetWorkflowStatus(SessionState.Culture.Code);

                if (SessionState.CurrentItemIsUserItem)
                {
                    if (s.StartsWith("R"))
                    {
                        classStatus = "hc_regionvalidated";
                        uwToolbarStatus.Visible = uwToolbarStatus.Items.FromKeyButton("Validate").Visible = uwToolbarStatus.Items.FromKeyButton("Exclude").Visible = true;
                        uwToolbarStatus.Items.FromKeySeparator("ValidateSep").Width = 8;
                        uwToolbarStatus.Items.FromKeyButton("Unexclude").Visible = false;
                    }
                    if (s.StartsWith("C"))
                    {
                        classStatus = "hc_countryvalidated";
                        uwToolbarStatus.Visible = uwToolbarStatus.Items.FromKeyButton("Exclude").Visible = true;
                        uwToolbarStatus.Items.FromKeySeparator("ValidateSep").Width = uwToolbarStatus.Items.FromKeySeparator("UnexcludeSep").Width = 0;
                        uwToolbarStatus.Items.FromKeyButton("Validate").Visible = uwToolbarStatus.Items.FromKeyButton("Unexclude").Visible = false;
                    }
                    if (s.StartsWith("E"))
                    {
                        classStatus = "hc_excluded";
                        uwToolbarStatus.Items.FromKeyButton("Validate").Visible = uwToolbarStatus.Items.FromKeyButton("Exclude").Visible = false;
                        uwToolbarStatus.Items.FromKeySeparator("ValidateSep").Width = uwToolbarStatus.Items.FromKeySeparator("UnexcludeSep").Width = 0;
                        uwToolbarStatus.Visible = uwToolbarStatus.Items.FromKeyButton("Unexclude").Visible = true;
                    }
                }


                lbItemInfo.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Workflow</span></td><td class='editValueCell'>";
                lbItemInfo.Text += "<span class='" + classStatus + "'>" + s + "</span>";
            }
            lbItemInfo.Text += "<tr valign='top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>Status</span></td><td class='editValueCell'>";
            lbItemInfo.Text += "<span>" + item.GetPLCStatus(SessionState.Culture.CountryCode) + "</span>";

            // QC 1054 fix START-- CRYS:Minimized or content frozen message is no longer available in the UI
            if (item.IsMinimizedByCountry(SessionState.Culture.CountryCode))
            {
                lbItemInfo.Text += "(Minimized)";
            }
            else if (item.IsFrozenByCountry(SessionState.Culture.CountryCode))
            {
                lbItemInfo.Text += "(Frozen)";
            }
            // QC 1054 fix END-- CRYS:Minimized or content frozen message is no longer available in the UI


            //Added by Radha for QC6182 - Display PMaster OID in the top right of the country catalog
            lbItemInfo.Text += "<tr valign= 'top'><td class='editLabelCell' style='font-weight:bold;width:130px'><span>PM OID</span></td><td class='editValueCell'>";
            lbItemInfo.Text += "<span>" + SessionState.CurrentItem.NodeOID.ToString() + "</span>";

            #endregion
            lbItemInfo.Text += "</td></tr></table>";

            #region Roll
            if (item.IsRoll && item.ItemRoll != null)
            {
                if (SessionState.CurrentItemIsUserItem)
                    lbRoll.Text = "<br><span class='hc_roll'>Soft roll</span> (<i>replacement date: <a href='javascript:openRoll(" + item.Id + ");' title='Modify the replacement date'>" + item.ItemRoll.ReplacementDate.Value.ToString(SessionState.User.FormatDate) + "</i></a>)";
                else
                    lbRoll.Text = "<br><span class='hc_roll'>Soft roll</span> (<i>replacement date: " + item.ItemRoll.ReplacementDate.Value.ToString(SessionState.User.FormatDate) + "</a>)";
                lbRoll.Visible = true;
            }
            #endregion
            lbCreated.Text = "Created on " + SessionState.User.FormatUtcDate(item.CreateDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime) + "<br/>by <a href='mailto:" + UITools.GetDisplayEmail(item.Creator.Email) + "?subject=" + item.FullName + " [#" + item.Id.ToString() + "]' title='Send Mail'>" + item.Creator.FullName + " [" + item.Creator.Organization.Name + "]</a>";
            whoswho.Attributes.Add("OnClick", "OpenWhosWho(" + item.Id.ToString() + ",'" + SessionState.Culture.Code + "');return false;");
        }
        /// <summary>
        /// Retrieve PLC
        /// </summary>
        private void RetrievePLC()
        {
            #region Update PLC Label
            lbPID.Text = UITools.PIDLabel;
            lbPOD.Text = UITools.PODLabel;
            lbBlind.Text = UITools.BlindLabel;
            lbAnnouncement.Text = UITools.AnnouncementLabel;
            lbSupport.Text = UITools.SupportLabel;
            lbEOL.Text = UITools.EOLLabel;
            lbRemoval.Text = UITools.RemovalLabel;
            lbDiscontinue.Text = UITools.DiscontinueLabel;
            #endregion
            pBundlePLC.Visible = false;
            int PLCUserId = PLC.PLCUser(SessionState.CurrentItem.Id);
            if (item.Level.SkuLevel) // PLC only at SKU Level
            {
                using (PLC itemPLC = PLC.GetByKey(item.Id, SessionState.Culture.CountryCode))
                {

                    if (itemPLC != null)
                    {
                        pPLC.Visible = true;
                        lAnnoun.Text = GetProperShortDate(itemPLC.AnnouncementDate);
                        iAnnoun.ImageUrl = itemPLC.AnnouncementDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iAnnoun.Visible = (((itemPLC.AnnouncementLocked) || (itemPLC.AnnouncementDateType == 'R')) && (itemPLC.AnnouncementDate != null));
                        lBlind.Text = GetProperShortDate(itemPLC.BlindDate);
                        iBlind.ImageUrl = itemPLC.BlindDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iBlind.Visible = (((itemPLC.BlindLocked) || (itemPLC.BlindDateType == 'R')) && (itemPLC.BlindDate != null));
                        lPID.Text = GetProperShortDate(itemPLC.FullDate);
                        iPID.ImageUrl = itemPLC.FullDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iPID.Visible = (((itemPLC.FullLocked) || (itemPLC.FullDateType == 'R')) && (itemPLC.FullDate != null));
                        lObso.Text = GetProperShortDate(itemPLC.ObsoleteDate);
                        iObso.ImageUrl = itemPLC.ObsoleteDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iObso.Visible = (((itemPLC.ObsoleteLocked) || (itemPLC.ObsoleteDateType == 'R')) && (itemPLC.ObsoleteDate != null));
                        lRemov.Text = GetProperShortDate(itemPLC.RemovalDate);
                        iRemov.ImageUrl = itemPLC.RemovalDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iRemov.Visible = (((itemPLC.RemovalLocked) || (itemPLC.RemovalDateType == 'R')) && (itemPLC.RemovalDate != null));
                        lSupport.Text = GetProperShortDate(itemPLC.EndOfSupportDate);
                        iSupport.ImageUrl = itemPLC.EndOfSupportDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iSupport.Visible = (((itemPLC.EndOfSupportLocked) || (itemPLC.EndOfSupportDateType == 'R')) && (itemPLC.EndOfSupportDate != null));
                        lEOL.Text = GetProperShortDate(itemPLC.EndOfLifeDate);
                        iEOL.ImageUrl = itemPLC.EndOfLifeDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iEOL.Visible = (((itemPLC.EndOfLifeLocked) || (itemPLC.EndOfLifeDateType == 'R')) && (itemPLC.EndOfLifeDate != null));
                        lDiscontinue.Text = GetProperShortDate(itemPLC.DiscontinueDate);
                        iDiscontinue.ImageUrl = itemPLC.DiscontinueDateType == 'R' ? "/hc_v4/img/flags/" + itemPLC.Country.MainRegionCode + ".gif" : "/hc_v4/img/locked_status.gif";
                        iDiscontinue.Visible = (((itemPLC.DiscontinueLocked) || (itemPLC.DiscontinueDateType == 'R')) && (itemPLC.DiscontinueDate != null));
                        //Change for CR 4516 - Prabhu
                        if (PLCUserId > 0 && SessionState.CurrentItemIsUserItem 
                            && SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_PLC)
                            && SessionState.User.HasCultureInScope(SessionState.Culture.Code)
                            )
                        {
                            uwToolbarPLC.Items.FromKeyButton("EditPLC").Visible = true;
                        }
                        else
                        {
                            uwToolbarPLC.Items.FromKeyButton("EditPLC").Visible = false;
                        }
                        #region "Ugly PMT code"
                        // Easy Content awesome requirement
                        // Display a warning when PMT date is more recent and different from current obso and full date
                        if (itemPLC.ObsoleteLocked || itemPLC.FullLocked)
                        {
                            bool bShowFullPMT = false, bShowObsoletePMT = false;
                            using (Database dbObj = new Database(SessionState.CacheComponents["Inbound_DB"].ConnectionString))
                            {
                                if (dbObj != null)
                                {
                                    using (DataSet dsPMT = dbObj.RunSQLReturnDataSet("Select FileTimeStamp FROM Interfaces WHERE InterfaceId = 3"))
                                    {
                                        if (dsPMT != null && dsPMT.Tables.Count == 1 && dsPMT.Tables[0].Rows.Count == 1 && dsPMT.Tables[0].Rows[0][0] != DBNull.Value)
                                        {
                                            DateTime? pmtFileDate = Utils.PMTConvertToDate(dsPMT.Tables[0].Rows[0][0].ToString());
                                            if (pmtFileDate != null)
                                            {
                                                using (DataSet ds = dbObj.RunSPReturnDataSet("QDE_GetPMTInfo", new SqlParameter("@CountryCode", SessionState.Culture.CountryCode)
                                                , new SqlParameter("@LanguageCode", SessionState.Culture.LanguageCode)
                                                , new SqlParameter("@Sku", item.Sku)))
                                                {
                                                    if (ds != null && ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
                                                    {
                                                        DateTime? pmtFull = HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GetProperDate(ds.Tables[0].Rows[0]["FullDate"]);
                                                        DateTime? pmtObso = HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GetProperDate(ds.Tables[0].Rows[0]["ObsoleteDate"]);
                                                        bShowFullPMT = pmtFull.HasValue && itemPLC.FullLocked && pmtFileDate > itemPLC.FullModifyDate && itemPLC.FullDate != pmtFull;
                                                        bShowObsoletePMT = pmtObso.HasValue && itemPLC.ObsoleteLocked && pmtFileDate > itemPLC.ObsoleteModifyDate && itemPLC.ObsoleteDate != pmtObso;
                                                    }
                                                    else
                                                    {
                                                        Trace.Warn("Error running [Select * FROM PMT WHERE CountryCode = '" + SessionState.Culture.CountryCode + "' AND LanguageCode = '" + SessionState.Culture.LanguageCode + "' AND ProductNumber = '" + item.Sku + "'] query :" + dbObj.LastError);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Trace.Warn("Error : pmtFileDate return invalid datetime");
                                            }
                                        }
                                        else
                                        {
                                            Trace.Warn("Error : Interface Id (#3) not found");
                                        }
                                    }
                                }
                                else
                                {
                                    Trace.Warn("Error : Cannot connect to Inbound DB [" + SessionState.CacheComponents["Inbound_DB"].ConnectionString + "]");
                                }
                            }
                            lPIDPMT.Visible = bShowFullPMT;
                            lObsoPMT.Visible = bShowObsoletePMT;
                        }
                        #endregion
                        if (item.TypeId == (int)ItemTypesEnum.BUNDLE)
                        {
                            lbErrorPLC.Visible = false;
                            pBundlePLC.Visible = true;
                            using (Database dbObj = Utils.GetMainDB())
                            {
                                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._PLC_CheckPLCforBundle",
                                  new SqlParameter("@ItemId", item.Id),
                                  new SqlParameter("@CountryCode", SessionState.Culture.CountryCode),
                                  new SqlParameter("@PID", itemPLC.FullDate),
                                  new SqlParameter("@POD", itemPLC.ObsoleteDate)))
                                {
                                    dbObj.CloseConnection();
                                    if (dbObj.LastError == string.Empty && ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[1].Rows[0]["errorMessage"].ToString() != string.Empty)
                                        {
                                            lbErrorPLC.Text = "Warning PLC - " + ds.Tables[1].Rows[0]["errorMessage"].ToString();
                                            lbErrorPLC.Visible = true;
                                        }
                                        dgBundlePLC.DataSource = ds.Tables[0].DefaultView;
                                        dgBundlePLC.DataBind();
                                        dgBundlePLC.Columns.FromKey("PID").Format = SessionState.User.FormatDate;
                                        dgBundlePLC.Columns.FromKey("POD").Format = SessionState.User.FormatDate;
                                        dgBundlePLC.Columns.FromKey("BlindDate").Format = SessionState.User.FormatDate;
                                        dgBundlePLC.Columns.FromKey("AnnDate").Format = SessionState.User.FormatDate;
                                        dgBundlePLC.Columns.FromKey("RemDate").Format = SessionState.User.FormatDate;
                                    }
                                    else { lbErrorPLC.Text = "_PLC_CheckPLCforBundle - error - " + dbObj.LastError; lbErrorPLC.Visible = true; }
                                }
                            }
                        }
                        if (!SessionState.CurrentItemIsUserItem)
                        {
                            uwToolbarPLC.Items.FromKeyButton("EditPLC").Visible = false;
                        }
                    }
                    else
                    {
                        lbErrorPLC.Text = "No PLC available";
                        lbErrorPLC.Visible = true;
                        pGridPLC.Visible = false;
                    }
                }
            }
            else
            {
                pPLC.Visible = false;
                uwToolbarPLC.Items.FromKeyButton("EditPLC").Visible = false;
            }
        }
        /// <summary>
        /// Retrieve UPC
        /// </summary>
        private void RetrieveUPC()
        {
            if (item.Level.SkuLevel) // UPC information only at SKU Level
            {
                using (Database dbObj = Utils.GetMainDB())
                {
                    using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT UPC, Opt, Description FROM UPC WHERE ItemId=@ItemId AND CountryCode=@CountryCode", "UPC", Database.NewSqlParameter("@ItemId", SqlDbType.BigInt, item.Id), Database.NewSqlParameter("@CountryCode", SqlDbType.VarChar, 10, SessionState.Culture.CountryCode)))
                    {
                        dbObj.CloseConnection();
                        if (dbObj.LastError == string.Empty)
                        {
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                pUPC.Visible = false;
                            }
                            else
                            {
                                dgUPC.DataSource = ds.Tables[0];
                                dgUPC.DataBind();
                                pUPC.Visible = true;
                            }
                        }
                        else
                        {
                            lbErrorUPC.Text = "ERROR - Impossible to found UPC" + dbObj.LastError;
                            lbErrorUPC.CssClass = "hc_error";
                            lbErrorUPC.Visible = true;
                        }
                    }
                }
            }
            else
            {
                pUPC.Visible = false;
            }
        }
        /// <summary>
        /// Display data into the grid
        /// </summary>
        private void UpdateDataView()
        {
            lbContent.Visible = (view != "content");
            dg.Rows.Clear();
            using (Database dbObj = Utils.GetMainDB())
            {
                string sSql = "EXECUTE dbo.QDE_GetItemCountryView " + item.Id + ", '" + SessionState.Culture.Code + "', ";

                // Localizable or all content
                if (uwToolbar.Items.FromKeyButton("OnlyLocalizable").Selected)
                    sSql += " 1, "; // display only localizable content
                else
                    sSql += " 0, "; // display all content

                // Single or aggregated view
                if (uwToolbar.Items.FromKeyButton("SingleView").Selected)
                    sSql += " 1"; // display single view
                else
                    sSql += " 0"; // display aggregated view

                using (DataSet ds = dbObj.RunSQLReturnDataSet(sSql, ""))
                {
                    dbObj.CloseConnection();
                    if (dbObj.LastError == string.Empty)
                    {
                        dg.DataSource = ds.Tables[0];
                        dg.DataBind();
                    }
                    else
                    {
                        lbError.Text = dbObj.LastError;
                        lbError.Visible = true;
                    }
                    currentContainerGroup = string.Empty;
                    currentContainerType = string.Empty;
                    currentItemName = string.Empty;
                    InitializeChunksGridGrouping();
                    imgProduct.Visible = imgProduct.ImageUrl != string.Empty;

                    ds.Dispose();
                }
            }
        }
        /// <summary>
        /// Get short date
        /// </summary>
        /// <param name="d">date (null or not)</param>
        /// <returns>Return empty string if the date is null else the short date</returns>
        private string GetProperShortDate(DateTime? d)
        {
            return d.HasValue ? d.Value.ToString(SessionState.User.FormatDate) : string.Empty;
        }
        /// <summary>
        /// Group chunks into the grid
        /// </summary>
        private void InitializeChunksGridGrouping()
        {
            int i = 0;
            groupContainerTypeCount = 0;
            groupContainerGroupCount = 0;
            groupItemNameCount = 0;
            while (i < dg.Rows.Count)
            {
                #region Group by ContainerType
                string containerType = dg.Rows[i].Cells.FromKey("ContainerType").Value.ToString();
                if (i == 0 || currentContainerType != containerType)
                {
                    currentContainerType = containerType;
                    currentItemName = string.Empty;
                    dg.Rows.Insert(i, new UltraGridRow());
                    UltraGridRow groupRow = dg.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
                    foreach (UltraGridCell cell in groupRow.Cells)
                    {
                        cell.Style.CssClass = string.Empty;
                    }
                    dg.Rows[i].Style.CssClass = "ptbgroup";
                    UltraGridCell groupCell = groupRow.Cells.FromKey("Label");
                    groupCell.Text = containerType;
                    groupCell.ColSpan = 2;
                    i++;
                }
                #endregion
                #region Group by ItemName if container type = "messaging"
                if (containerType.ToLower() == typeMessaging.ToLower())
                {
                    #region ItemName
                    string itemName = dg.Rows[i].Cells.FromKey("ItemName").Value.ToString();
                    if (i == 0 || currentItemName != itemName)
                    {
                        currentItemName = itemName;
                        dg.Rows.Insert(i, new UltraGridRow());
                        UltraGridRow groupRow = dg.Rows[i];
                        UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row          
                        foreach (UltraGridCell cell in groupRow.Cells)
                        {
                            cell.Style.CssClass = string.Empty;
                        }
                        dg.Rows[i].Style.CssClass = "ptb4";
                        UltraGridCell groupCell = groupRow.Cells.FromKey("Label");
                        groupCell.Text = itemName;
                        groupCell.ColSpan = 2;
                        i++;
                    }
                    #endregion
                }
                #endregion
                #region Group by ContainerGroup
                string containerGroup = dg.Rows[i].Cells.FromKey("ContainerGroup").Value.ToString();
                if (i == 0 || currentContainerGroup != containerGroup)
                {
                    currentContainerGroup = containerGroup;
                    dg.Rows.Insert(i, new UltraGridRow());
                    UltraGridRow groupRow = dg.Rows[i];
                    UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
                    foreach (UltraGridCell cell in groupRow.Cells)
                    {
                        cell.Style.CssClass = string.Empty;
                    }
                    dg.Rows[i].Style.CssClass = "ptb5";
                    UltraGridCell groupCell = groupRow.Cells.FromKey("Label");
                    groupCell.Text = containerGroup;
                    groupCell.ColSpan = 2;
                    i++;
                }
                #endregion
                i++;
            }
        }
        /// <summary>
        /// Update the status of the item
        /// </summary>
        /// <param name="Status"></param>
        private bool UpdateStatus(string s, string c)
        {
            if (item.UpdateWorkflowStatus(s, c, SessionState.User.Id))
            {
                DisplayItemInfo();
                return true;
            }
            else { lbError.Visible = true; return false; }

        }
        #endregion

        #region "Market segments & Publishers"
        private void RetrieveMarketSegments()
        {
            PanelMarketSegments.Visible = true;

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

            if (!SessionState.Culture.Country.CanLocalizeMarketSegments
              || item.IsFrozenByCulture(SessionState.Culture.Code)
              || item.IsMinimizedByCulture(SessionState.Culture.Code))
            {
                #region Market segments view
                string defMS = string.Empty;
                using (ItemMarketSegmentList definedItemMarketSegments = ItemMarketSegment.GetDefinedByItem(item.Id, countryCode))
                {
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
                }
                if (defMS.Length > 0)
                    lbDefinedMS.Text = defMS;
                else
                    lbDefinedMS.Text = "None";
                #endregion
            }
            else
            {
                #region Market segments edit
                dgMS.Rows.Clear();
                using (MarketSegmentList marketSegments = MarketSegment.GetAll())
                {
                    using (ItemMarketSegmentList itemMarketSegments = ItemMarketSegment.GetDefinedByItem(item.Id, SessionState.Culture.CountryCode))
                    {
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

                        if (itemMarketSegments != null && marketSegments != null)
                        {
                            bool canLocalizeMS = SessionState.Culture.Country.CanLocalizeMarketSegments;

                            string segmentChecked = string.Empty;
                            string canLocalize = string.Empty;
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
                                    if (!canLocalizeMS)
                                        canLocalize = " disabled='disabled'";
                                }
                                UltraGridRow newRow = new UltraGridRow(new object[] { ms.Name, "<center><input type='checkbox' onclick='UpdateMS(\"" + cbNotInheritedMS.ClientID + "\")' name='m_" + ms.Id.ToString() + "' id='m_" + ms.Id.ToString() + "'" + segmentChecked + canLocalize + "></center>", "<center>" + countryCode + "</center>" });
                                dgMS.Rows.Add(newRow);
                            }
                        }
                    }
                }
                #endregion
            }

            #region Applied market segments
            countryCode = SessionState.Culture.CountryCode;
            string appMS = string.Empty;
            using (ItemMarketSegmentList appliedItemMarketSegments = ItemMarketSegment.GetAppliedByItem(item.Id, countryCode))
            {

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
            }
            if (appMS.Length > 0)
                lbAppliedMS.Text = appMS;
            else
                lbAppliedMS.Text = "None";
            #endregion
        }
        private void RetrievePublishers()
        {
            PanelPublishers.Visible = true;

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

            if (item.Level.SkuLevel)
            {
                if (!SessionState.Culture.Country.CanLocalizePublishers
                  || item.IsFrozenByCulture(SessionState.Culture.Code)
                  || item.IsMinimizedByCulture(SessionState.Culture.Code))
                {
                    #region Publishers view
                    string sPublishers = string.Empty;
                    using (ItemPublisherList ipl = ItemPublisher.GetPublishersByItemId(item.Id, countryCode))
                    {

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
                    }

                    if (sPublishers.Length > 0)
                        lbDefinedP.Text += sPublishers;
                    else
                        lbDefinedP.Text += "None";

                    #endregion
                }
                else
                {
                    #region Publishers edit
                    dgP.Rows.Clear();
                    string filterPublishers = string.Empty;
                    if (SessionState.Culture.Type == CultureType.Locale)
                        filterPublishers = "ExcludeFromCountries = 0";
                    using (PublisherList publishers = Publisher.GetAll(filterPublishers))
                    {
                        using (ItemPublisherList ipl = ItemPublisher.GetPublishersByItemId(item.Id, SessionState.Culture.CountryCode))
                        {

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

                            if (ipl != null && publishers != null)
                            {
                                bool canLocalizeP = SessionState.Culture.Country.CanLocalizeMarketSegments;

                                string regionCode = string.Empty;
                                string publisherChecked = string.Empty;
                                string canLocalize = string.Empty;
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
                                            if (!canLocalizeP)
                                                canLocalize = " disabled='disabled'";
                                        }
                                    }
                                    UltraGridRow newRow = new UltraGridRow(new object[] { p.Name, "<center><input type='checkbox' onclick='UpdateP(\"" + cbNotInheritedP.ClientID + "\")' name='p_" + p.Id.ToString() + "' id='p_" + p.Id.ToString() + "'" + publisherChecked + "></center>", "<center>" + regionCode + "</center>" });
                                    dgP.Rows.Add(newRow);
                                }
                            }
                        }
                        if (SessionState.Culture.Type == CultureType.Master)
                            cbNotInheritedP.Visible = false;
                    }
                    #endregion
                }
            }
            else
            {
                PanelPublishers.Visible = false;
            }
        }
        private void SaveMS()
        {
            bool isSavedMarketSegment = true;

            #region Retrieve market segments
            using (MarketSegmentList marketSegments = MarketSegment.GetAll())
            {

                ItemMarketSegment imSpec = new ItemMarketSegment(item.Id, SessionState.Culture.CountryCode, -1, false, SessionState.User.Id);
                imSpec.Delete(SessionState.User.Id);

                using (ItemMarketSegmentList msl = ItemMarketSegment.GetAppliedByItem(item.Id, SessionState.Culture.CountryCode))
                {
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

                                ItemMarketSegment ims = new ItemMarketSegment(item.Id, SessionState.Culture.CountryCode, ms.Id, Request["m_p_" + ms.Id.ToString()] != null, SessionState.User.Id);
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
                                ItemMarketSegment ims = new ItemMarketSegment(item.Id, SessionState.Culture.CountryCode, -1, false, SessionState.User.Id);
                                if (!ims.Save(SessionState.User.Id))
                                    isSavedMarketSegment = false;
                            }

                            if (isSavedMarketSegment)
                            {
                                ItemMarketSegment.UpdateAllItemMarketSegments(item.Id);
                                RetrieveMarketSegments();
                            }
                        }
                    }
                }
            #endregion

                if (!isSavedMarketSegment)
                {
                    string msg = "'The Item cannot be saved! - ";
                    if (!isSavedMarketSegment)
                        msg += ItemMarketSegment.LastError;
                    msg += "'";

                    lbErrorMS.Text = msg;
                    lbErrorMS.CssClass = "hc_error";
                    lbErrorMS.Visible = true;
                }
            }
        }
        private void SaveP()
        {
            bool isSavedPublishers = true;

            #region Retrieve publishers
            if (item.Level.SkuLevel)
            {
                string filterPublishers = string.Empty;
                if (SessionState.Culture.Type == CultureType.Locale)
                {
                    filterPublishers = "ExcludeFromCountries = 0";
                }
                using (PublisherList publishers = Publisher.GetAll(filterPublishers))
                {

                    if (publishers != null)
                    {
                        // Delete all publishers
                        foreach (Publisher p in publishers)
                        {
                            if (p != null)
                            {
                                isSavedPublishers = item.DeletePublisher(p.Id, SessionState.Culture.CountryCode, SessionState.User.Id);
                                if (!isSavedPublishers)
                                    break;
                            }
                        }
                        item.DeletePublisher(-1, SessionState.Culture.CountryCode, SessionState.User.Id);

                        // Add new publishers
                        int ipCount = 0;
                        foreach (Publisher p in publishers)
                        {
                            if (Request["p_" + p.Id.ToString()] != null)
                            {
                                ipCount++;
                                isSavedPublishers = item.SavePublisher(p.Id, SessionState.Culture.CountryCode, SessionState.User.Id);
                                if (!isSavedPublishers)
                                    break;
                            }
                        }

                        if (isSavedPublishers)
                        {
                            if (ipCount == 0 && cbNotInheritedP.Checked)
                                isSavedPublishers = item.SavePublisher(-1, SessionState.Culture.CountryCode, SessionState.User.Id);
                            ItemPublisher.UpdateAllItemPublishers(item.Id);
                            RetrievePublishers();
                        }
                    }
                }
            #endregion

                if (!isSavedPublishers)
                {
                    string msg = "'The Item cannot be saved! - ";
                    if (!isSavedPublishers)
                        msg += Item.LastError;
                    msg += "'";

                    lbErrorP.Text = msg;
                    lbErrorP.CssClass = "hc_error";
                    lbErrorP.Visible = true;
                }
            }
        }
        #endregion

        #region "Links methods"
        private void UpdateLinksDataView()
        {
            #region "Apply button"
            if (CanApplyLinks(-1))
            {
                // display button "Apply changes"
                UITools.ShowToolBarButton(Ultrawebtoolbar1, "Save");
            }
            else
            {
                UITools.HideToolBarButton(Ultrawebtoolbar1, "Save");
            }
            #endregion

            #region "Hide all components"
            // hide grids
            Ultrawebtoolbar1.Visible = false;
            dgl.Visible = false;

            // hide labels
            lbErrorLinks.Visible = false;
            lbLinks.Visible = false;
            #endregion
            Trace.Warn("   -> RetrieveLinkType() Begin");
            RetrieveLinkType();
            Trace.Warn("   -> RetrieveLinkType() Ends");

            if (item != null)
            {
                #region "Display links"
                Trace.Warn("   -> Link.GetContent Begin");
                using (DataSet ds = Link.GetContent(item.Id, SessionState.Culture.Code, -2, true))
                {// without cross sell
                    Trace.Warn("   -> Link.GetContent Ends");
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Trace.Warn("   -> dgl Databind Begins");
                            //Fix for QC 2256 - All links disappear in ALL country catalogs - Prabhu R S
                            dgl.DataSource = ds;
                            dgl.DataBind();
                            dgl.DisplayLayout.AllowSortingDefault = AllowSorting.No; // no sort
                            InitializeLinksGridGrouping();
                            Trace.Warn("   -> dgl Databind Ends");

                            dgl.Visible = true;
                            Ultrawebtoolbar1.Visible = true;
                            lbLinks.Visible = true;
                        }
                        else
                        {
                            lbErrorLinks.CssClass = "hc_success";
                            lbErrorLinks.Text = "No records";
                            lbErrorLinks.Visible = true;
                        }
                    }
                    else
                    {
                        // Error
                        lbErrorLinks.CssClass = "hc_error";
                        lbErrorLinks.Text = HyperCatalog.Business.Item.LastError;
                        lbErrorLinks.Visible = true;
                    }
                }
                #endregion
                //dgl.Columns.FromKey("Class").Header.Caption = SessionState.ItemLevels[1].Name;
                lbLinks.Visible = (view != "links");
            }
        }
        private void updateCheckbox(UltraGridRow r)
        {
            bool isInherited = false;
            bool isExcluded = false;
            int linkTypeId = -1;
            if (r.Cells.FromKey("IsInherited") != null && r.Cells.FromKey("IsInherited").Value != null)
                isInherited = Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value);
            if (r.Cells.FromKey("IsExcluded") != null && r.Cells.FromKey("IsExcluded").Value != null)
                isExcluded = Convert.ToBoolean(r.Cells.FromKey("IsExcluded").Value);
            if (r.Cells.FromKey("LinkTypeId") != null && r.Cells.FromKey("LinkTypeId").Value != null)
                linkTypeId = Convert.ToInt32(r.Cells.FromKey("LinkTypeId").Value);

            // Exclusion checkbox
            if (r.Cells.FromKey("IsExcluded") != null && !r.Cells.FromKey("IsExcluded").Column.Hidden)
            {
                if (isInherited)
                {
                    System.Int64 inheritedItemId = -1;
                    System.Int64 subItemId = -1;
                    System.Int64 itemId = -1;
                    string countryCode = string.Empty;
                    string fallbackCountryCode = string.Empty;
                    if (r.Cells.FromKey("InheritedItemId") != null && r.Cells.FromKey("InheritedItemId").Value != null)
                        inheritedItemId = Convert.ToInt64(r.Cells.FromKey("InheritedItemId").Value);
                    if (r.Cells.FromKey("SubItemId") != null && r.Cells.FromKey("SubItemId").Value != null)
                        subItemId = Convert.ToInt64(r.Cells.FromKey("SubItemId").Value);
                    if (r.Cells.FromKey("CountryCode") != null && r.Cells.FromKey("CountryCode").Value != null)
                        countryCode = r.Cells.FromKey("CountryCode").Value.ToString();
                    if (r.Cells.FromKey("FallbackCountryCode") != null && r.Cells.FromKey("FallbackCountryCode").Value != null)
                        fallbackCountryCode = r.Cells.FromKey("FallbackCountryCode").Value.ToString();
                    if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null)
                        itemId = Convert.ToInt64(r.Cells.FromKey("ItemId").Value);

                    if (inheritedItemId >= 0 && subItemId >= 0 && itemId >= 0 && countryCode.Length > 0)
                    {
                        bool isExclusion = Link.IsLinkExclusion(inheritedItemId, subItemId, fallbackCountryCode, linkTypeId, itemId, countryCode);
                        if (!isExclusion && isExcluded)
                        {
                            r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
                        }
                    }
                }

                int userId = -1;
                bool linkFrom = false;
                string excludedCountry = string.Empty;
                Int64 excludedItem = -1;
                if (r.Cells.FromKey("UserId") != null && r.Cells.FromKey("UserId").Value != null)
                    userId = Convert.ToInt32(r.Cells.FromKey("UserId").Value);
                if (r.Cells.FromKey("LinkFrom") != null && r.Cells.FromKey("LinkFrom").Value != null)
                    linkFrom = Convert.ToBoolean(r.Cells.FromKey("LinkFrom").Value);
                if (r.Cells.FromKey("ExcludedCountry") != null && r.Cells.FromKey("ExcludedCountry").Value != null)
                    excludedCountry = r.Cells.FromKey("ExcludedCountry").Value.ToString();
                if (r.Cells.FromKey("ExcludedItem") != null && r.Cells.FromKey("ExcludedItem").Value != null)
                    excludedItem = Convert.ToInt64(r.Cells.FromKey("ExcludedItem").Value);

                if (!linkFrom
                  || (isInherited && isExcluded && (excludedCountry != SessionState.Culture.CountryCode || excludedItem != item.Id))
                  || (userId == 0 && isExcluded)
                  || SessionState.User.IsReadOnly
                  || !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS)
                  || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkTypeId == CrossSellId)
                  || (SessionState.Culture.Type != CultureType.Locale))
                {
                    r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
                }
            }
        }
        private void InitializeLinksGridGrouping()
        {
            int i = 0;
            int groupLinkTypeCount = 0;
            int groupLinkFromCount = 0;
            int groupFamilyCount = 0;
            bool currentLinkFrom = false;
            string currentFamily = string.Empty;
            LinkType currentLinkType = null;
            bool newLinkType = true;
            bool newLinkFrom = true;

            int indexMax = dgl.Columns.Count - 1;
            int index = 0;

            while (i < dgl.Rows.Count)
            {
                int linkTypeId = Convert.ToInt32(dgl.Rows[i].Cells.FromKey("LinkTypeId").Value);
                using (currentLinkType = LinkType.GetByKey(linkTypeId))
                {
                    if (i == 0 || (currentLinkType != null && currentLinkType.Id != linkTypeId))
                    {
                        #region Group by LinkType
                        newLinkType = true;
                        dgl.Rows.Insert(i, new UltraGridRow());
                        UltraGridRow groupRow = dgl.Rows[i];
                        UltraGridCell groupCellMax = groupRow.Cells[indexMax]; // initialize all cells for this row

                        UltraGridCell groupCell = null;
                        index = groupRow.Cells.FromKey("Class").Column.Index;
                        groupCell = groupRow.Cells[index];
                        groupCell.Style.HorizontalAlign = HorizontalAlign.Left;
                        groupCell.Style.CssClass = "ptbgroup";
                        groupCell.Style.CustomRules = "";
                        using (LinkType lt = HyperCatalog.Business.LinkType.GetByKey(linkTypeId))
                        {
                            groupCell.Text = lt.Name;
                        }
                        groupCell.ColSpan = indexMax - index + 1;

                        i++;
                        #endregion
                    }
                    if (currentLinkType != null && currentLinkType.IsBidirectional && dgl.Rows[i].Cells.FromKey("LinkFrom") != null)
                    {
                        #region Group by LinkFrom
                        bool linkFrom = Convert.ToBoolean(dgl.Rows[i].Cells.FromKey("LinkFrom").Value);
                        if (newLinkType || currentLinkFrom != linkFrom)
                        {
                            currentLinkFrom = linkFrom;
                            newLinkFrom = true;
                            newLinkType = false;
                            dgl.Rows.Insert(i, new UltraGridRow());
                            UltraGridRow groupRow = dgl.Rows[i];
                            UltraGridCell groupCellMax = groupRow.Cells[indexMax]; // initialize all cells for this row    

                            UltraGridCell groupCell = null;
                            index = groupRow.Cells.FromKey("Class").Column.Index;
                            groupCell = groupRow.Cells[index];
                            groupCell.Style.HorizontalAlign = HorizontalAlign.Left;
                            groupCell.Style.CssClass = "ptb4";
                            groupCell.Style.CustomRules = "";
                            groupCell = groupRow.Cells.FromKey("Class");
                            if (linkFrom)
                                groupCell.Text = "Companion list";
                            else
                                //Code modified to change 'Hardware' to 'Host'
                                //groupCell.Text = "Hardware list";
                                groupCell.Text = "Host list";

                            groupCell.ColSpan = indexMax - index + 1;
                            i++;
                        }
                        #endregion

                        #region Group by Family
                        if (dgl.Rows[i].Cells.FromKey("Family") != null && dgl.Rows[i].Cells.FromKey("Family").Value != null)
                        {
                            string family = string.Empty;
                            if (linkFrom) // Companion list
                                family = dgl.Rows[i].Cells.FromKey("SubFamily").Value.ToString();
                            else // Hardware list
                                family = dgl.Rows[i].Cells.FromKey("Family").Value.ToString();
                            if (newLinkType || newLinkFrom || currentFamily != family)
                            {
                                currentFamily = family;
                                newLinkType = false;
                                newLinkFrom = false;
                                dgl.Rows.Insert(i, new UltraGridRow());
                                UltraGridRow groupRow = dgl.Rows[i];
                                UltraGridCell groupCellMax = groupRow.Cells[indexMax]; // initialize all cells for this row

                                UltraGridCell groupCell = null;
                                index = groupRow.Cells.FromKey("Class").Column.Index;
                                groupCell = groupRow.Cells[index];
                                groupCell.Style.HorizontalAlign = HorizontalAlign.Left;
                                groupCell.Style.CssClass = "ptb5";
                                groupCell.Style.CustomRules = "";
                                groupCell = groupRow.Cells.FromKey("Class");
                                index = groupCell.Column.Index;
                                groupCell.Text = family;
                                groupCell.ColSpan = indexMax - index + 1;
                                i++;
                            }
                        }
                        #endregion
                    }

                    #region bidirectional link type
                    if (dgl.Rows[i].Cells.FromKey("Bidirectional") != null && dgl.Rows[i].Cells.FromKey("Bidirectional").Value != null)
                    {
                        bool isBidirectional = Convert.ToBoolean(dgl.Rows[i].Cells.FromKey("Bidirectional").Value);
                        if (!isBidirectional && dgl.Rows[i].Cells.FromKey("Name") != null) // Cross sell, Bundle, ...
                        {
                            dgl.Rows[i].Cells.FromKey("Name").ColSpan = 3;
                        }
                    }
                    #endregion
                    i++;
                }
            }
        }
        protected void dgl_InitializeRow(object sender, RowEventArgs e)
        {
            // update checkbox
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

            if (e.Row.Cells.FromKey("LinkFrom") != null)
            {
                bool linkFrom = Convert.ToBoolean(e.Row.Cells.FromKey("LinkFrom").Value);
                if (!linkFrom) // Hardware list
                {
                    if (e.Row.Cells.FromKey("Name") != null && e.Row.Cells.FromKey("ItemName") != null && e.Row.Cells.FromKey("ItemName").Value != null)
                        e.Row.Cells.FromKey("Name").Text = e.Row.Cells.FromKey("ItemName").Value.ToString();
                    if (e.Row.Cells.FromKey("SKU") != null && e.Row.Cells.FromKey("ItemSKU") != null && e.Row.Cells.FromKey("ItemSKU").Value != null)
                        e.Row.Cells.FromKey("SKU").Text = e.Row.Cells.FromKey("ItemSKU").Value.ToString();
                    if (e.Row.Cells.FromKey("Class") != null && e.Row.Cells.FromKey("ClassName") != null && e.Row.Cells.FromKey("ClassName").Value != null)
                        e.Row.Cells.FromKey("Class").Text = e.Row.Cells.FromKey("ClassName").Value.ToString();
                }
                else // Companion list
                {
                    if (e.Row.Cells.FromKey("Name") != null && e.Row.Cells.FromKey("SubItemName") != null && e.Row.Cells.FromKey("SubItemName").Value != null)
                        e.Row.Cells.FromKey("Name").Text = e.Row.Cells.FromKey("SubItemName").Value.ToString();
                    if (e.Row.Cells.FromKey("SKU") != null && e.Row.Cells.FromKey("SubItemSKU") != null && e.Row.Cells.FromKey("SubItemSKU").Value != null)
                        e.Row.Cells.FromKey("SKU").Text = e.Row.Cells.FromKey("SubItemSKU").Value.ToString();
                    if (e.Row.Cells.FromKey("Class") != null && e.Row.Cells.FromKey("SubClassName") != null && e.Row.Cells.FromKey("SubClassName").Value != null)
                        e.Row.Cells.FromKey("Class").Text = e.Row.Cells.FromKey("SubClassName").Value.ToString();
                }

                if (e.Row.Cells.FromKey("Name") != null)
                    e.Row.Cells.FromKey("Name").Style.Wrap = true;
                if (e.Row.Cells.FromKey("Class") != null)
                    e.Row.Cells.FromKey("Class").Style.Wrap = true;
                if (e.Row.Cells.FromKey("SKU") != null)
                    e.Row.Cells.FromKey("SKU").Style.Wrap = true;
            }
        }
        private string buildExclusionList()
        {
            System.Text.StringBuilder exclusionList = new System.Text.StringBuilder(string.Empty);

            UltraWebGrid grid = null;
            if (dgl != null && dgl.Visible)
                grid = dgl;

            if (grid != null)
            {
                // Get all rows for sort and exclusion
                foreach (UltraGridRow r in grid.Rows)
                {
                    if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null)
                    {
                        // new exclusion
                        if (exclusionList.Length > 0)
                            exclusionList.Append("|");

                        exclusionList.Append(r.Cells.FromKey("InheritedItemId").Value.ToString());
                        exclusionList.Append(",");
                        exclusionList.Append(r.Cells.FromKey("SubItemId").Value.ToString());
                        exclusionList.Append(",");
                        exclusionList.Append(r.Cells.FromKey("LinkTypeId").Value.ToString());
                        exclusionList.Append(",'");
                        exclusionList.Append(r.Cells.FromKey("FallbackCountryCode").Value.ToString());
                        exclusionList.Append("',");
                        exclusionList.Append(r.Cells.FromKey("ItemId").Value.ToString());
                        exclusionList.Append(",'");
                        exclusionList.Append(SessionState.Culture.CountryCode);
                        exclusionList.Append("',");
                        exclusionList.Append(Convert.ToByte(r.Cells.FromKey("IsExcluded").Value));
                    }
                }
            }
            return exclusionList.ToString();
        }
        protected void Ultrawebtoolbar1_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            if (btn == "save")
            {
                Apply();
            }
        }
        private void Apply()
        {
            lbErrorLinks.Visible = false;

            string exclusionList = buildExclusionList();

            // Update link exclusions
            if (exclusionList.Length > 0)
            {

                // save new exclusions
                if (!HyperCatalog.Business.Link.SaveNewExclusion(exclusionList, SessionState.User.Id))
                {
                    lbErrorLinks.CssClass = "hc_error";
                    lbErrorLinks.Text = HyperCatalog.Business.Link.LastError;
                    lbErrorLinks.Visible = true;
                    return;
                }
            }

            lbErrorLinks.CssClass = "hc_success";
            lbErrorLinks.Text = "New data saved!";
            lbErrorLinks.Visible = true;
        }
        private void RetrieveLinkType()
        {
            if (!linkTypesAreInitialized)
            {
                // Retrieve link types
                using (LinkTypeList linkTypes = LinkType.GetAll())
                {
                    SuppliesId = GetLinkType(linkTypes, "Supplies");
                    ServicesId = GetLinkType(linkTypes, "Services");
                    CrossSellId = GetLinkType(linkTypes, "CrossSell");
                    BundlesId = GetLinkType(linkTypes, "Bundles");
                    linkTypesAreInitialized = true;
                }
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
        #endregion

        #region "Cross sell methods"
        private void UpdateCrossSellDataView()
        {
            lbCrossSell.Visible = (view != "cross");
            Trace.Warn("   -> RetrieveLinkType() Begin");
            RetrieveLinkType();
            Trace.Warn("   -> RetrieveLinkType() Ends");
            #region "Hide all components"
            // hide buttons
            UITools.HideToolBarButton(Ultrawebtoolbar2, "Add");
            UITools.HideToolBarSeparator(Ultrawebtoolbar2, "DeleteSep");
            UITools.HideToolBarButton(Ultrawebtoolbar2, "Delete");
            UITools.HideToolBarSeparator(Ultrawebtoolbar2, "SaveSep");
            UITools.HideToolBarButton(Ultrawebtoolbar2, "Save");
            UITools.HideToolBarSeparator(Ultrawebtoolbar2, "ResumeInheritanceSep");
            UITools.HideToolBarButton(Ultrawebtoolbar2, "ResumeInheritance");

            // hide grid
            dgCrossSell.Visible = false;
            // hide labels
            lbErrorCrossSell.Visible = false;
            #endregion

            bool hasButton = false;
            #region "Add button"
            if (CanManageLinks(CROSS_SELL_ID))
            {
                UITools.ShowToolBarButton(Ultrawebtoolbar2, "Add");
                hasButton = true;
                Ultrawebtoolbar2.Items.FromKeyButton("Add").DefaultStyle.Width = Unit.Pixel(80);
            }
            #endregion
            #region "Apply button"
            if (CanApplyLinks(CROSS_SELL_ID))
            {
                // display button "Apply changes"
                if (hasButton)
                    UITools.ShowToolBarSeparator(Ultrawebtoolbar2, "SaveSep");
                UITools.ShowToolBarButton(Ultrawebtoolbar2, "Save");
                hasButton = true;
            }
            #endregion

            dgCrossSell.Columns.FromKey("Class").Header.Caption = SessionState.ItemLevels[1].Name;
            dgCrossSell.Columns.FromKey("Family").Header.Caption = SessionState.ItemLevels[3].Name;
            if (item != null)
            {
                using (DataSet ds = Link.GetContent(item.Id, SessionState.Culture.Code, CROSS_SELL_ID, true))
                {
                    #region "Display links no bidirectional (CrossSell, Bundles)"
                    // all links for cross sell
                    if (ds != null)
                    {
                        dgCrossSell.DataSource = ds;
                        Utils.InitGridSort(ref dgCrossSell, false);
                        dgCrossSell.DataBind();
                        Utils.EnableIntelligentSort(ref dgCrossSell, Convert.ToInt32(txtSortColPos.Value));
                        dgCrossSell.DisplayLayout.AllowSortingDefault = AllowSorting.No;

                        if (dgCrossSell.Rows.Count == 0)
                        {
                            UITools.HideToolBarSeparator(Ultrawebtoolbar2, "DeleteSep");
                            UITools.HideToolBarButton(Ultrawebtoolbar2, "Delete");
                            UITools.HideToolBarSeparator(Ultrawebtoolbar2, "SaveSep");
                            UITools.HideToolBarButton(Ultrawebtoolbar2, "Save");
                        }
                        dgCrossSell.Visible = true;
                    }
                    else
                    {
                        // Error
                        lbErrorCrossSell.CssClass = "hc_error";
                        lbErrorCrossSell.Text = HyperCatalog.Business.Link.LastError;
                        lbErrorCrossSell.Visible = true;
                    }
                    #endregion

                    #region "Delete button"
                    // header for select All
                    TemplatedColumn colH = null;
                    if (dgCrossSell.Visible)
                        colH = (TemplatedColumn)dgCrossSell.Columns.FromKey("Select");

                    if (colH != null)
                    {
                        CheckBox cbH = null;
                        if (colH.HeaderItem != null && colH.HeaderItem.FindControl("g_ca") != null)
                            cbH = (CheckBox)colH.HeaderItem.FindControl("g_ca");

                        // possible deletion
                        if ((!AreAllAutoLinks()) && CanManageLinks(CROSS_SELL_ID))
                        {
                            if (hasButton)
                                UITools.ShowToolBarSeparator(Ultrawebtoolbar2, "DeleteSep");
                            UITools.ShowToolBarButton(Ultrawebtoolbar2, "Delete");
                            UITools.ShowToolBarSeparator(Ultrawebtoolbar2, "ResumeInheritanceSep");
                            UITools.ShowToolBarButton(Ultrawebtoolbar2, "ResumeInheritance");
                            if (cbH != null)
                                cbH.Enabled = true;
                            hasButton = true;
                        }
                        else
                        {
                            if (cbH != null)
                                cbH.Enabled = false;
                        }
                    }
                }
                    #endregion
            }
        }
        private void updateCheckboxCrossSell(UltraGridRow r)
        {

            bool isInherited = false;
            bool isExcluded = false;
            int linkTypeId = 4;
            string countryCode = string.Empty;
            if (r.Cells.FromKey("IsInherited") != null && r.Cells.FromKey("IsInherited").Value != null)
                isInherited = Convert.ToBoolean(r.Cells.FromKey("IsInherited").Value);
            if (r.Cells.FromKey("IsExcluded") != null && r.Cells.FromKey("IsExcluded").Value != null)
                isExcluded = Convert.ToBoolean(r.Cells.FromKey("IsExcluded").Value);
            if (r.Cells.FromKey("LinkTypeId") != null && r.Cells.FromKey("LinkTypeId").Value != null)
                linkTypeId = Convert.ToInt32(r.Cells.FromKey("LinkTypeId").Value);
            if (r.Cells.FromKey("CountryCode") != null && r.Cells.FromKey("CountryCode").Value != null)
                countryCode = r.Cells.FromKey("CountryCode").Value.ToString();

            if (r.Cells.FromKey("Name") != null)
                r.Cells.FromKey("Name").Style.Wrap = true;

            // Exclusion checkbox
            if (r.Cells.FromKey("IsExcluded") != null && !r.Cells.FromKey("IsExcluded").Column.Hidden)
            {
                if (isInherited)
                {
                    System.Int64 inheritedItemId = -1;
                    System.Int64 subItemId = -1;
                    System.Int64 itemId = -1;

                    if (r.Cells.FromKey("InheritedItemId") != null && r.Cells.FromKey("InheritedItemId").Value != null)
                        inheritedItemId = Convert.ToInt64(r.Cells.FromKey("InheritedItemId").Value);
                    if (r.Cells.FromKey("SubItemId") != null && r.Cells.FromKey("SubItemId").Value != null)
                        subItemId = Convert.ToInt64(r.Cells.FromKey("SubItemId").Value);
                    if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null)
                        itemId = Convert.ToInt64(r.Cells.FromKey("ItemId").Value);

                    if (inheritedItemId >= 0 && subItemId >= 0 && itemId >= 0 && countryCode.Length > 0)
                    {
                        bool isExclusion = Link.IsLinkExclusion(inheritedItemId, subItemId, countryCode, linkTypeId, itemId, SessionState.Culture.CountryCode);
                        if (!isExclusion && isExcluded)
                        {
                            r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
                        }
                    }
                }

                int userId = -1;
                bool linkFrom = false;
                string excludedCountry = string.Empty;
                Int64 excludedItem = -1;
                if (r.Cells.FromKey("UserId") != null && r.Cells.FromKey("UserId").Value != null)
                    userId = Convert.ToInt32(r.Cells.FromKey("UserId").Value);
                if (r.Cells.FromKey("ExcludedCountry") != null && r.Cells.FromKey("ExcludedCountry").Value != null)
                    excludedCountry = r.Cells.FromKey("ExcludedCountry").Value.ToString();
                if (r.Cells.FromKey("ExcludedItem") != null && r.Cells.FromKey("ExcludedItem").Value != null)
                    excludedItem = Convert.ToInt64(r.Cells.FromKey("ExcludedItem").Value);

                if ((isInherited && isExcluded && (excludedCountry != SessionState.Culture.CountryCode || excludedItem != item.Id))
                  || (userId == 0 && isExcluded)
                  || SessionState.User.IsReadOnly
                  || !SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS)
                  || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkTypeId == CrossSellId)
                  || (SessionState.Culture.Type != CultureType.Locale))
                {
                    r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
                }
                // Vinay changed to block exclusion if it happens at parent locale or series.
                if (isExcluded && (excludedCountry != SessionState.Culture.CountryCode || excludedItem != item.Id))
                    r.Cells.FromKey("IsExcluded").AllowEditing = AllowEditing.No;
            }

            // Deleted checkbox
            if (r.Cells.FromKey("UserId") != null && r.Cells.FromKey("UserId").Value != null && r.Cells.FromKey("Select") != null)
            {
                int userId = Convert.ToInt32(r.Cells.FromKey("UserId").Value);
                if (userId == 0 || isInherited || (countryCode != SessionState.Culture.CountryCode)) // not delete automatic links or inherited links 
                {
                    TemplatedColumn col = (TemplatedColumn)r.Cells.FromKey("Select").Column;
                    if (col != null)
                    {
                        CheckBox cb = (CheckBox)((CellItem)col.CellItems[r.Index]).FindControl("g_sd");
                        if (cb != null)
                        {
                            cb.Enabled = false;
                        }
                    }
                }
            }
        }
        protected void dgCrossSell_InitializeRow(object sender, RowEventArgs e)
        {
            // Update exclusion, recommended checkbox
            updateCheckboxCrossSell(e.Row);

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
                    e.Row.Cells.FromKey("ItemName").Style.Font.Italic = true;
                    e.Row.Cells.FromKey("ItemSku").Style.Font.Italic = true;
                    e.Row.Cells.FromKey("Class").Style.Font.Italic = true;
                    if (e.Row.Cells.FromKey("Family") != null)
                        e.Row.Cells.FromKey("Family").Style.Font.Italic = true;
                }
            }
        }
        private void ApplyCrossSell()
        {
            lbErrorCrossSell.Visible = false;

            #region "Exclusion links"
            string exclusionList = buildExclusionListForCrossSell();

            // Update link exclusions
            if (exclusionList.Length > 0)
            {
                // save new exclusions
                if (!HyperCatalog.Business.Link.SaveNewExclusion(exclusionList.ToString(), SessionState.User.Id))
                {
                    lbErrorCrossSell.CssClass = "hc_error";
                    lbErrorCrossSell.Text = HyperCatalog.Business.Link.LastError;
                    lbErrorCrossSell.Visible = true;
                    return;
                }
            }
            #endregion
            #region "Sort links"
            string sortList = buildSortList();

            // Update link exclusions
            if (sortList.Length > 0)
            {
                // save new sort links
                if (!HyperCatalog.Business.Link.SaveNewSort(sortList.ToString(), SessionState.User.Id))
                {
                    lbErrorCrossSell.CssClass = "hc_error";
                    lbErrorCrossSell.Text = HyperCatalog.Business.Link.LastError;
                    lbErrorCrossSell.Visible = true;
                    return;
                }
            }
            #endregion

            lbErrorCrossSell.CssClass = "hc_success";
            lbErrorCrossSell.Text = "New data saved!";
            UpdateCrossSellDataView();
            lbErrorCrossSell.Visible = true;
        }
        private string buildSortList()
        {
            System.Text.StringBuilder sortList = new System.Text.StringBuilder(string.Empty);

            UltraWebGrid grid = null;
            if (dgCrossSell != null && dgCrossSell.Visible)
                grid = dgCrossSell;

            if (grid != null)
            {
                // Get all rows for sort and exclusion
                foreach (UltraGridRow r in grid.Rows)
                {
                    // new sort
                    if (sortList.Length > 0)
                        sortList.Append("|");

                    sortList.Append(r.Cells.FromKey("InheritedItemId").Value.ToString());
                    sortList.Append(",");
                    sortList.Append(r.Cells.FromKey("SubItemId").Value.ToString());
                    sortList.Append(",");
                    sortList.Append(CROSS_SELL_ID);
                    sortList.Append(",'");
                    sortList.Append(r.Cells.FromKey("FallbackCountryCode").Value.ToString());
                    sortList.Append("',");
                    sortList.Append(r.Cells.FromKey("ItemId").Value.ToString());
                    sortList.Append(",'");
                    sortList.Append(SessionState.Culture.CountryCode);
                    sortList.Append("',");
                    sortList.Append(r.Index);
                }
            }
            return sortList.ToString();
        }
        private string buildExclusionListForCrossSell()
        {
            System.Text.StringBuilder exclusionList = new System.Text.StringBuilder(string.Empty);

            UltraWebGrid grid = null;
            if (dgCrossSell != null && dgCrossSell.Visible)
                grid = dgCrossSell;

            if (grid != null)
            {
                // Get all rows for sort and exclusion
                foreach (UltraGridRow r in grid.Rows)
                {
                    if (r.Cells.FromKey("ItemId") != null && r.Cells.FromKey("ItemId").Value != null)
                    {
                        // new exclusion
                        if (exclusionList.Length > 0)
                            exclusionList.Append("|");

                        exclusionList.Append(r.Cells.FromKey("InheritedItemId").Value.ToString());
                        exclusionList.Append(",");
                        exclusionList.Append(r.Cells.FromKey("SubItemId").Value.ToString());
                        exclusionList.Append(",");
                        exclusionList.Append(CROSS_SELL_ID);
                        exclusionList.Append(",'");
                        exclusionList.Append(r.Cells.FromKey("FallbackCountryCode").Value.ToString());
                        exclusionList.Append("',");
                        exclusionList.Append(r.Cells.FromKey("ItemId").Value.ToString());
                        exclusionList.Append(",'");
                        exclusionList.Append(SessionState.Culture.CountryCode);
                        exclusionList.Append("',");
                        if (r.Cells.FromKey("IsExcluded").IsEditable())
                            exclusionList.Append(Convert.ToByte(r.Cells.FromKey("IsExcluded").Value));
                        else
                            exclusionList.Append(Convert.ToByte(false));
                    }
                }
            }
            return exclusionList.ToString();
        }
        private void DeleteCrossSell()
        {
            lbErrorCrossSell.Visible = false;
            if (item != null)
            {
                UltraWebGrid grid = null;
                if (dgCrossSell.Visible)
                    grid = dgCrossSell;

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
                                System.Int64 mainId = item.Id;
                                System.Int64 subId = Convert.ToInt64(grid.Rows[i].Cells.FromKey("SubItemId").Value);

                                // Get country code
                                string countryCode = grid.Rows[i].Cells.FromKey("CountryCode").Value.ToString();

                                //get link
                                using (Link lnk = Link.GetByKey(mainId, subId, CROSS_SELL_ID, countryCode))
                                {
                                    if (lnk != null)
                                    {
                                        if (!lnk.Delete(HyperCatalog.Shared.SessionState.User.Id))
                                        {

                                            lbErrorCrossSell.Text = Link.LastError;
                                            lbErrorCrossSell.CssClass = "hc_error";
                                            lbErrorCrossSell.Visible = true;

                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    UpdateCrossSellDataView();
                }
            }
        }

        private void ResumeInheritanceCrossSell()
        {
            lbErrorCrossSell.Visible = false;
            bool chk = true;
            if (item != null)
            {
                UltraWebGrid grid = null;
                if (dgCrossSell.Visible)
                    grid = dgCrossSell;

                if (grid != null)
                {
                    // Delete the clear blindly all links (eligible & ineligible links) 
                    if (!Link.ResumeInheritance(Convert.ToInt64(SessionState.CurrentItem.Id.ToString()), CROSS_SELL_ID, SessionState.Culture.CountryCode.ToString(), Convert.ToInt32(SessionState.User.Id.ToString())))
                    {
                        chk = false;
                        lbError.Text = Link.LastError;
                        lbError.CssClass = "hc_error";
                        lbError.Visible = true;
                        return;
                    }
                }
                if (chk)
                {
                    lbErrorCrossSell.CssClass = "hc_success";
                    lbErrorCrossSell.Text = "New data saved!";
                    UpdateCrossSellDataView();
                    lbErrorCrossSell.Visible = true;
                }

            }
        }

        protected void Ultrawebtoolbar2_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            if (btn == "save")
            {
                ApplyCrossSell();
            }
            else if (btn == "delete")
            {
                DeleteCrossSell();
            }
            else if (btn == "resumeinheritance")
            {
                ResumeInheritanceCrossSell();
            }
        }
        private bool AreAllAutoLinks()
        {
            UltraWebGrid grid = null;
            if (dgCrossSell.Visible)
                grid = dgCrossSell;

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
        #endregion

        private bool CanManageLinks(int linkTypeId)
        {
            if (SessionState.CurrentItemIsUserItem)
            {
                // Culture
                if ((SessionState.Culture.Type == CultureType.Master)
                  || (SessionState.Culture.Type == CultureType.Locale && item.IsCountrySpecific)
                  || (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanLocalizeCrossSells && linkTypeId == CrossSellId))
                {
                    // Capability
                    if ((SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
                      || (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkTypeId == CrossSellId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CanApplyLinks(int linkTypeId)
        {
            if (SessionState.CurrentItemIsUserItem)
            {
                // Culture
                if ((SessionState.Culture.Type == CultureType.Master)
                  || (SessionState.Culture.Type == CultureType.Regionale)
                  || (SessionState.Culture.Type == CultureType.Locale && linkTypeId != CrossSellId)
                  || (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanLocalizeCrossSells && linkTypeId == CrossSellId))
                {
                    // Capability
                    if ((SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
                      || (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CROSS_SELL) && linkTypeId == CrossSellId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
