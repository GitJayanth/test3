#region uses
using System;
using System.Xml;
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
using Infragistics.WebUI.UltraWebNavigator;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebToolbar;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

/// <summary>
/// This class allows adding link
/// </summary>
public partial class Links_add_treeview : HCPage
{
    #region Declarations
    protected string currentNodeId = string.Empty;
    //private HyperCatalog.Business.Item item = null;
    private Int64 itemId = -1;
    private int linkTypeId = -1;
    private bool isBidirectional = false;
    private bool bLinkFrom = false;
    private Database dbObj = Utils.GetMainDB();
    private int SKuLevelId = -1;
    private DataSet ds = null;
    //Adding Export funtionality as part of PR665368 - Export Button by Nisha Verma on 18th jan 2013
    private int configLimit = 0;
    private int NbSKUs = 0;
    private int colErrorIndex = 0;
    private bool isItemObs = false;

    private const int LINKTYPE_BUNDLE = 3;
    #endregion

  #region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    txtFilter.AutoPostBack = false;
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");  
       InitializeComponent();
    base.OnInit(e);
      
  }
     private void InitializeComponent()
  {
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

  }
  #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
        UITools.CheckConnection(Page);
        colErrorIndex = dg.Columns.FromKey("Error").Index;
        if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
        {
            RetrieveSkuLevel(); // Retrieve Id of the SKU level
            try
            {
                // get properties
                if (Request["f"] != null)
                    bLinkFrom = Request["f"].ToString().Equals("1");
                if (Request["f"] != null)
                    linkTypeId = Convert.ToInt32(Request["t"]);
                if (Request["f"] != null)
                    itemId = Convert.ToInt64(Request["i"]);

                //Code added for Links Requirement (PR658943) - to set the min and max date of Effective Date calendar by Prachi on 15th Jan 2013 - start
                dateValue.MinDate = SessionState.User.FormatUtcDate(DateTime.UtcNow);
                dateValue.MaxDate = SessionState.User.FormatUtcDate(DateTime.UtcNow).AddYears(dateValue.CalendarLayout.DropDownYearsNumber);
                //Code added for Links Requirement (PR658943) - to set the min and max date of Effective Date calendar by Prachi on 15th Jan 2013 - end

                if (linkTypeId > -1)
                {
                    using (LinkType linkType = HyperCatalog.Business.LinkType.GetByKey(linkTypeId))
                    {
                        isBidirectional = linkType.IsBidirectional;
                        configLimit = linkType.GetConfigLimit(itemId, Convert.ToByte(bLinkFrom));
                    }
                    isItemObs = IsItemObsolete();
                }

                if (rblInput.SelectedValue.ToUpper().Equals("NAME"))
                {
                    ddlLevel.Enabled = true;
                    lbLevel.Enabled = true;
                }
                else
                {
                    ddlLevel.Enabled = false;
                    lbLevel.Enabled = false;
                }

                if (!Page.IsPostBack)
                {
                    if (Request["filter"] != null)
                    {
                        txtFilter.Text = Request["filter"].ToString();
                        if (Request["filter"].ToString().Trim() != string.Empty)
                        {
                            DisplaySearchResults();
                        }
                        else
                        {
                            UpdateDataView();
                        }
                    }
                    else
                    {
                        UpdateDataView();
                    }
                    //Code added for Links Requirement (PR658943) - to set today's date as Effective Date in the calendar by Prachi on 15th Jan 2013
                    dateValue.Value = SessionState.User.FormatUtcDate(DateTime.UtcNow);
                    LoadLevelDDL();

                    if (!bLinkFrom)  //Adding a Host; so disable the 'Include Obsolete' checkbox
                    {
                        CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
                        cb.Enabled = false;
                    }

                    if (configLimit == 1) //If limit is Product Type, display the "Load from Level 1" checkbox
                    {
                        UITools.ShowToolBarSeparator(Ultrawebtoolbar2, "LoadSep");
                        Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible = true;
                    }
                    else
                    {
                        UITools.HideToolBarSeparator(Ultrawebtoolbar2, "LoadSep");
                        Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible = false;
                    }
                }
            }
            catch (FormatException fe)
            {
                UITools.DenyAccess(DenyMode.Popup);
            }
        }
        else
        {
            UITools.DenyAccess(DenyMode.Popup);
        }
    }
    private void UpdateDataView()
    {
        // Initialize treeview
        InitTreeView();
    }

    #region TreeView
    private void InitTreeView()
    {
        lbError.Visible = false;
        bool showObsolete;
        bool level1Flag;

        if (bLinkFrom) //Adding companion, so considering the correct checked state of Include Obsolete checkbox
        {
            CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
            showObsolete = cb.Checked;
        }
        else
        {
            showObsolete = true;  //Adding Host, so by default including Obsolete Items
        }

        if (Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible)
        {
            CheckBox checkLoad = (CheckBox)Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").FindControl("chkLoad");
            level1Flag = checkLoad.Checked;
        }
        else
            level1Flag = false;

        // get possible links
        //ds = Link.GetTreeView(linkTypeId, Convert.ToByte(bLinkFrom), SessionState.Culture.Code, -1, itemId, Convert.ToByte(showObsolete), Convert.ToByte(level1Flag));
        //Added by venkata 06/10/16
        ds = GetTreeView(linkTypeId, Convert.ToByte(bLinkFrom), SessionState.Culture.Code, -1, itemId, Convert.ToByte(showObsolete), Convert.ToByte(level1Flag));
        //end

        if (ds == null || ds.Tables.Count == 0)
        {
            lbError.CssClass = "hc_error";
            lbError.Text = "Error in retrieving the Dataset";
            lbError.Visible = true;

            return;
        }

        if (ds.Tables.Count > 1) // two tables (root and children)
        {
            // Add relation (between itemId and parentId)
            try
            {
                ds.Relations.Add("ItemChilds", ds.Tables[0].Columns["ItemId"], ds.Tables[1].Columns["ParentId"], false);
            }
            catch (System.Exception x)
            {
                lbError.CssClass = "hc_error";
                lbError.Text = x.Message;
                lbError.Visible = true;

                return;
            }

            // Update treeview
            try
            {
                webTree.DataSource = ds.Tables[0].DefaultView;
                webTree.Levels[0].RelationName = "ItemChilds";
                webTree.Levels[0].ColumnName = "ItemName";
                webTree.Levels[0].LevelKeyField = "ItemId";
                webTree.Levels[0].TargetFrameName = "ItemName";
                using (ItemLevelList itemLevelAll = ItemLevel.GetAll())
                {
                    int maxLevels = itemLevelAll.Count + 1;
                    for (int i = 1; i < maxLevels; i++)
                    {
                        webTree.Levels[i].ColumnName = "ItemName";
                        webTree.Levels[i].LevelKeyField = "ItemId";
                        webTree.Levels[i].TargetFrameName = "ItemName";
                    }
                }

                webTree.DataMember = ds.Tables[0].TableName;
                webTree.DataBind();
                webTree.Nodes[0].Expanded = true;
            }
            catch (System.Exception x)
            {
                lbError.CssClass = "hc_error";
                lbError.Text = x.Message;
                lbError.Visible = true;

                return;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }
        else
        {
            lbError.CssClass = "hc_error";
            lbError.Text = "No candidates";
            lbError.Visible = true;
        }
    }

    // Added by Venkata 06/10/16
    public DataSet GetTreeView(int linkTypeId, byte linkFrom, string cultureCode, Int64 parentId, Int64 itemId, byte showObs, byte level1Flag)
    {
        DataSet ds = null;
        using (Database dbObj = Utils.GetMainDB())
        {
            ds = dbObj.RunSPReturnDataSet("dbo._Link_GetTv", "Items",
              new SqlParameter("@LinkTypeId", linkTypeId),
              new SqlParameter("@LinkFromTo", linkFrom),
              new SqlParameter("@CultureCode", cultureCode),
              new SqlParameter("@ParentId", parentId),
              new SqlParameter("@ItemId", itemId),
              new SqlParameter("@IncObsolete", showObs),
              new SqlParameter("@Level1Flag", level1Flag),
              new SqlParameter("@Company", SessionState.CompanyName));
            dbObj.CloseConnection();
            if (dbObj.LastError != null && dbObj.LastError.Length > 0)
            {
                throw new DataException("SQLDataAccessLayer: GetTreeView-> " + dbObj.LastError);
            }
            return ds;
        }
    }
    //Added by Venkata end

    protected void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
    {
        //*************************************************************
        // Flag node if it has childs
        //*************************************************************
        if (e.Node.DataKey != null)
        {
            //Code added for Links Requirement (PR664195) to show/hide obsolete items by Prachi on 12th Dec 2012
            bool showObsolete;
            if (bLinkFrom)
            {
                CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
                showObsolete = cb.Checked;
            }
            else
            {
                showObsolete = true;
            }

            //Code added for Links Requirement (PR658940) - to enable the checkbox above the SKU Level by Prachi on 11th Dec 2012
            e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;

            Int64 currentItemId = Convert.ToInt64(e.Node.DataKey);

            DataTable dtChildren = e.Node.DataKey.ToString() == "-1" ? ds.Tables[0] : ds.Tables.Count > 1 ? ds.Tables[1] : ds.Tables[0];
            DataRow dr = dtChildren.Select("ItemId = " + currentItemId.ToString())[0];

            // Display or not image to expand node
            e.Node.ShowExpand = Convert.ToBoolean(dr["HasChild"]);

            // Update image
            e.Node.ImageUrl = "/hc_v4/ig/s_l.gif";
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scroll", "scroll('" + String.Format("{0}{1}", webTree.UniqueID.Replace("$", ""), e.Node.GetIdString()) + "');", true);

            // Add checkbox
            bool isSkuLevel = Convert.ToBoolean(dr["IsSkuLevel"]);
            bool isMainComponent = Convert.ToBoolean(dr["IsMainComponent"]);
            int typeId = Convert.ToInt32(dr["TypeId"]);
            //Code added for Links Requirement (PR664195) to display obsolete icon by Prachi on 12th Dec 2012
            bool isObsolete = Convert.ToBoolean(dr["IsObsolete"]);
            //Code modified for Links Requirement (PR658940) - to enable the checkbox above the SKU Level by Prachi on 11th Dec 2012 - start
            bool isLinked = Convert.ToBoolean(dr["IsLinked"]);
            int levelId = Convert.ToInt32(dr["LevelId"]);
            int limit = configLimit;
            if (Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible)
            {
                CheckBox checkLoad = (CheckBox)Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").FindControl("chkLoad");
                //If retrieved Limit is Product Type and User has not checked the "Load from Product Level" checkbox, then consider the limit as the default value
                if (limit == 1) //For Product Type Level
                {
                    if (!checkLoad.Checked)
                    {
                        if (bLinkFrom) //Companion
                        {
                            limit = Convert.ToInt32(ApplicationSettings.Parameters["Link_ProductCompanionLimit"].Value);
                        }
                        else   //Host
                        {
                            limit = Convert.ToInt32(ApplicationSettings.Parameters["Link_ProductHostLimit"].Value);
                        }
                    }
                }
            }
            if (levelId >= limit)  //Code modified for Links Requirement (PR658940) - to enable the checkbox above the SKU Level by Prachi on 11th Dec 2012 - end
            {
                e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;

                //Code added for Links Requirement (PR658940) - to check the nodes if parent node is already checked by Prachi on 9th Jan 2013 - start
                Node parentNode = e.Node.Parent;
                if (parentNode.Checked)
                {
                    e.Node.Checked = true;
                }
                //Code added for Links Requirement (PR658940) - to check the nodes if parent node is already checked by Prachi on 9th Jan 2013 - end

                //Code modified for Links Requirement (PR658940) - to disable the checkbox if the item has already been linked by Prachi on 10th Jan 2013
                if (isLinked)
                {
                    e.Node.Enabled = false;
                    e.Node.Checked = true;
                    e.Node.ShowExpand = false;
                }

                // Check if current item is selected item
                if (currentItemId == itemId)
                {
                    e.Node.Enabled = false;
                    e.Node.Checked = false;

                    e.Node.Style.CssClass = "hc_error";
                    e.Node.Style.Font.Bold = true;
                    e.Node.Style.Font.Italic = true;
                    e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
                }

                // If link type is bundle and current SKU is main component 
                if (isMainComponent)
                {
                    e.Node.Enabled = false;
                    e.Node.Checked = true;

                    e.Node.Style.CssClass = "hc_error";
                    e.Node.Style.Font.Bold = true;
                    e.Node.Style.Font.Italic = true;
                    e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
                }
                //Code added for Links Requirement (PR664195) - to not allow the user to select obsolete item if selected host/companion is already obsolete by Prachi on 16th Jan 2013
                //If selected item on left hand side is obsolete and current item in treeview is also obsolete
                if (showObsolete)
                {
                    if (isObsolete && isItemObs)
                    {
                        e.Node.Enabled = false;
                        e.Node.Checked = false;
                        e.Node.ShowExpand = false;
                    }
                }
            }

            //Code added for Links Requirement (PR658940) - to enable the checkbox above the SKU Level by Prachi on 11th Dec 2012 
            // If level is Sku level, update Icon
            if (isSkuLevel)
            {
                e.Node.ImageUrl = "/hc_v4/img/type_" + typeId.ToString() + ".png";
                e.Node.ShowExpand = false;
            }

            //Code added for Links Requirement (PR664195) - to display obsolete icon and show/hide obsolete items by Prachi on 12th Dec 2012
            if (isObsolete)
            {
                if (showObsolete)
                {
                    e.Node.ImageUrl = e.Node.SelectedImageUrl = "/hc_v4/ig/s_o.gif";
                }
            }
        }

        //*************************************************************
        // Copy Item Id to node Tag so that Client script can access it
        //*************************************************************
        e.Node.Tag = e.Node.DataKey.ToString();

        //*************************************************************
        // Clean Nodes text (remove common text with parents)
        //*************************************************************
        Infragistics.WebUI.UltraWebNavigator.Node n = e.Node;
        if ((bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_SHRINKED_NAMES).Value)
        {
            Utils.CleanNodeText(ref n);
        }
    }
    protected void webTree_DemandLoad(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
    {
        bool showObsolete;
        bool level1Flag;
        if (bLinkFrom)
        {
            CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
            showObsolete = cb.Checked;
        }
        else
        {
            showObsolete = true;
        }
        if (Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible)
        {
            CheckBox checkLoad = (CheckBox)Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").FindControl("chkLoad");
            level1Flag = checkLoad.Checked;
        }
        else
            level1Flag = false;

        // Get children
        //ds = Link.GetTreeView(linkTypeId, Convert.ToByte(bLinkFrom), SessionState.Culture.Code, Convert.ToInt64(e.Node.DataKey), itemId, Convert.ToByte(showObsolete), Convert.ToByte(level1Flag));
        //Added by Venkata 07-10-16
        ds = GetTreeView(linkTypeId, Convert.ToByte(bLinkFrom), SessionState.Culture.Code, Convert.ToInt64(e.Node.DataKey), itemId, Convert.ToByte(showObsolete), Convert.ToByte(level1Flag));
        //end 
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                e.Node.DataBind(ds.Tables[0].DefaultView, "Items");

                e.Node.Expanded = true;
                currentNodeId = String.Format("{0}{1}", webTree.UniqueID.Replace("$", ""), e.Node.GetIdString());
                Page.DataBind();

                if (ds != null)
                    ds.Dispose();
            }
        }
    }

    private void ApplyTVChanges()
    {
        lbError.Visible = false;
        if (webTree != null)
        {
            bool checkFlag;
            XmlDocument t_SourceTreeData = new XmlDocument();
            this.webTree.WriteXmlDoc(t_SourceTreeData, true, false);
            XmlNodeList t_TreeNodeList = t_SourceTreeData.SelectNodes("//Node[Checked]");

            XmlNode node = null;
            XmlNode node1 = null;
            //Code added for Links Requirement (PR658940) - to consider only SKU Level Items for Link creation by Prachi on 10th Jan 2013
            string ItemList = "";
            DataSet ds = null;
            if (bLinkFrom)
            {
                CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
                checkFlag = cb.Checked;
            }
            else
            {
                checkFlag = true;
            }

            bool areSaved = false;
            int invalidCnt = 0;
            for (int i = 0; i < t_TreeNodeList.Count; i++)
            {
                //Code modified for Links Requirement (PR658940) - to consider only SKU Level Items for Link creation by Prachi on 10th Jan 2013
                node1 = t_TreeNodeList[i].SelectSingleNode("./Enabled");
                if (node1 == null)
                {
                    node = t_TreeNodeList[i].SelectSingleNode("./DataKey");

                    if (node != null)
                    {
                        ItemList += node.InnerText + ",";
                    }
                }
            }
            ItemList = ItemList.TrimEnd(',');
            ////QC 7705 Nisha Verma added the if code for IE error when not selecting anything and clicking on apply changes
            if (t_TreeNodeList.Count != 0)
            {
                ds = Link.GetEligibleSKUs(itemId, ItemList, linkTypeId, Convert.ToByte(bLinkFrom), SessionState.Culture.Code, Convert.ToByte(checkFlag));
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if ((Convert.ToBoolean(row["IsRelevant"])) && (Convert.ToInt32(row["SameLink"]) != 2))
                        {
                            System.Int64 mainId = itemId;
                            System.Int64 subId = Convert.ToInt64(row["ItemId"]);

                            if (!bLinkFrom) // LinkTo -- Adding a Host
                            {
                                mainId = subId;
                                //Modified for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item
                                subId = Convert.ToInt64(row["MainItemId"]);
                            }
                            //prachi 5/5/2016 CR#6914
                            if (string.IsNullOrEmpty(Convert.ToString(dateValue.Value)))
                            {

                                dateValue.Value = DateTime.Now.ToUniversalTime().Date;
                            }
                            //prachi 5/5/2016 CR#6914
                        
                            Link lnk = new Link(mainId, subId, linkTypeId, SessionState.Culture.CountryCode, -1, SessionState.User.Id, DateTime.UtcNow, Convert.ToDateTime(dateValue.Value).ToUniversalTime().Date);
                            if (!lnk.Save())
                            {
                                lbError.CssClass = "hc_error";
                                lbError.Text = Link.LastError;
                                lbError.Visible = true;

                                areSaved = false;
                                return;
                            }
                            else
                                areSaved = true;
                        }
                        else if ((Convert.ToBoolean(row["IsRelevant"])) && (Convert.ToInt32(row["SameLink"]) == 2))
                        {
                            invalidCnt++;
                        }
                        else if (!Convert.ToBoolean(row["IsRelevant"]) && (Convert.ToInt32(row["SameLink"]) == 1))
                        {
                            invalidCnt++;
                        }
                    }
                }

                if (areSaved)
                {
                    lbError.CssClass = "hc_success";
                    lbError.Text = "Data saved";
                    lbError.Visible = true;

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>ReloadParent();</script>");
                }
                else
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = "Error in Link creation";
                    lbError.Visible = true;
                }
            }

            ////QC 7705 Nisha Verma commented the code for IE error when not selecting anything and clicking on apply changes
            //if (invalidCnt == ds.Tables[0].Rows.Count)  //All links already exist and cannot be created
            //{
            //    lbError.CssClass = "hc_error";
            //    lbError.Text = "Link cannot be created as the link already exists";
            //    lbError.Visible = true;
            //}
        }
    }
    #endregion

    #region Search
    //Code modified for Links Requirement (PR658943) - to allow for searches of node names in addition to SKUs on 26th Jan 2013
    private void DisplaySearchResults()
    {
        //Adding Export funtionality as part of PR665368 - Export Button by Nisha Verma on 18th jan 2013(Enabling the export button) 
        Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = true;
        lbError.Visible = false;
        dg.Visible = false;

        if (txtFilter.Text.Trim().Length == 0)
        {
            webTree.Visible = true; // display treeview
            rblInput.SelectedIndex = 0;
            ddlLevel.Enabled = false;
            lbLevel.Enabled = false;
            LbNbSKUs.Visible = false;
            Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = false;
            if (configLimit == 1) //If limit is Product Type, display the "Load from Level 1" checkbox
            {
                UITools.ShowToolBarSeparator(Ultrawebtoolbar2, "LoadSep");
                Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible = true;
            }
        }
        else
        {
            webTree.Visible = false; // display grid
            UITools.HideToolBarSeparator(Ultrawebtoolbar2, "LoadSep");
            Ultrawebtoolbar2.Items.FromKeyCustom("LoadLevel1").Visible = false;

            string cleanFilter = txtFilter.Text.Trim().Replace("'", "''").ToLower();
            cleanFilter = cleanFilter.Replace("[", "[[]");
            cleanFilter = cleanFilter.Replace("_", "[_]");
            cleanFilter = cleanFilter.Replace("%", "[%]");

            bool showObsolete;
            if (bLinkFrom)
            {
                CheckBox cb = (CheckBox)uwToolbar.Items.FromKeyCustom("chkObs").FindControl("chkObsolete");
                showObsolete = cb.Checked;
            }
            else
            {
                showObsolete = true;
            }

            string inputType = rblInput.SelectedValue.ToUpper();
            int levelId;
            if (inputType.Equals("NAME"))
                levelId = Convert.ToInt32(ddlLevel.SelectedValue);
            else
                levelId = 7; //In case of "SKU" selected in Input Type

            DataSet ds = Link.LinkAnalyze(itemId, SessionState.Culture.Code, cleanFilter, Convert.ToByte(bLinkFrom), linkTypeId, ",", Convert.ToByte(showObsolete), inputType, levelId);

            if (ds == null || ds.Tables.Count == 0)
            {
                lbError.CssClass = "hc_error";
                lbError.Text = "Error in retrieving the Dataset";
                lbError.Visible = true;
            }
            else
            {
                // initialize
                NbSKUs = 0;
                // Display grid
                if (ds != null)
                {
                    dg.DataSource = ds.Tables[0];
                    dg.DataBind();
                    dg.Visible = true;

                    ds.Dispose();
                }

                if (inputType.Equals("NAME"))
                {
                    LoadLevelDDL();
                    lbLevel.Enabled = true;
                    ddlLevel.Enabled = true;
                    ddlLevel.Items.Remove("Disabled");
                    ddlLevel.SelectedValue = Convert.ToString(levelId);
                }

                // User can or cannot save
                bool canProceed = false;
                bool isHeaderChecked = false;
                if (txtFilter.Text.Length > 0)
                    txtFilter.Text = txtFilter.Text.Substring(0, txtFilter.Text.Length);

                foreach (UltraGridRow r in dg.Rows)
                {
                    if ((r.Cells[colErrorIndex].Text == string.Empty) || (Convert.ToInt32(r.Cells.FromKey("SameLink").Value) == 2 && Convert.ToBoolean(r.Cells.FromKey("IsRelevant").Text)))
                    {
                        canProceed = true;
                        break;
                    }
                }

                TemplatedColumn colH = (TemplatedColumn)dg.Columns.FromKey("Select");
                CheckBox cbH = (CheckBox)colH.HeaderItem.FindControl("g_ca");
                if (canProceed)
                {
                    Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = true;
                    cbH.Enabled = true;
                }
                else
                {
                    Ultrawebtoolbar2.Items.FromKeyButton("Export").Enabled = true;
                    cbH.Enabled = false;
                }

                //selecting/unselecting the header checkbox
                foreach (UltraGridRow r in dg.Rows)
                {
                    CheckBox cbItem = (CheckBox)((CellItem)colH.CellItems[r.Index]).FindControl("g_sd");
                    if (!cbItem.Enabled && cbItem.Checked)
                    {
                        isHeaderChecked = true;
                    }
                    else if (cbItem.Enabled)
                    {
                        if (!cbItem.Checked)
                        {
                            isHeaderChecked = false;
                            break;
                        }
                        else
                            isHeaderChecked = true;
                    }
                }
                if (isHeaderChecked)
                    cbH.Checked = true;

                // Count of SKUs to save
                if (NbSKUs > 0)
                    LbNbSKUs.Text = NbSKUs.ToString() + "/" + dg.Rows.Count.ToString() + " component(s) found. Please make a selection and Click Apply changes.";
                else
                    LbNbSKUs.Text = "The filter provided 0 possible components. One component at least is required. Please retry or cancel.";

                LbNbSKUs.Text = "<br>" + LbNbSKUs.Text;
                LbNbSKUs.Visible = true;
            }
        }
    }

    //Code modified for Links Requirement (PR658943) - to allow for searches of node names in addition to SKUs on 26th Jan 2013
    private void ApplyGridChanges()
    {
        // *************************************************************************
        // update multiple lines on the grid
        // *************************************************************************
        lbError.Visible = false;

        bool areSaved = false;
        if (dg != null)
        {
            TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");

            for (int i = 0; i < dg.Rows.Count; i++)
            {
                // Search checkbox for each row
                int isRelevant = Convert.ToInt32(dg.Rows[i].Cells.FromKey("IsRelevant").Value);
                int SameValidLink = Convert.ToInt32(dg.Rows[i].Cells.FromKey("SameLink").Value);
                CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");

                if (((isRelevant == 1) || (SameValidLink == 2)) && cb.Enabled && cb.Checked)
                {
                    System.Int64 mainId = itemId;
                    System.Int64 subId = Convert.ToInt64(dg.Rows[i].Cells.FromKey("ItemId").Value);
                    if (!bLinkFrom) // LinkTo -- Adding a Host
                    {
                        mainId = subId;
                        //Modified for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item
                        subId = Convert.ToInt64(dg.Rows[i].Cells.FromKey("MainItemId").Value);
                    }

                    //prachi 5/5/2016 CR#6914
                    if (string.IsNullOrEmpty(Convert.ToString(dateValue.Value)))
                    {

                        dateValue.Value = DateTime.Now.ToUniversalTime().Date;
                    }
                    //prachi 5/5/2016 CR#6914
                    Link lnk = new Link(mainId, subId, linkTypeId, SessionState.Culture.CountryCode, -1, SessionState.User.Id, DateTime.UtcNow, Convert.ToDateTime(dateValue.Value).ToUniversalTime().Date);
                    if (!lnk.Save())
                    {
                        lbError.CssClass = "hc_error";
                        lbError.Text = Link.LastError;
                        lbError.Visible = true;

                        areSaved = false;
                        return;
                    }
                    else
                        areSaved = true;

                    //cb.Enabled = false;
                }
            }

            if (areSaved)
            {
                lbError.CssClass = "hc_success";
                lbError.Text = "Data saved!";
                lbError.Visible = true;

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>ReloadParent();</script>");
            }
        }
    }

    //Code modified for Links Requirement (PR658943) - to allow for searches of node names in addition to SKUs on 26th Jan 2013
    protected void dg_InitializeRow(object sender, RowEventArgs e)
    {
        string inputType = rblInput.SelectedValue.ToUpper();

        TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");

        if (e.Row.Cells.FromKey("IsRelevant").Value != null)
        {
            cb.Enabled = Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Text);
            if (Convert.ToInt16(e.Row.Cells.FromKey("FlagExists").Value) == 1 && Convert.ToInt16(e.Row.Cells.FromKey("SameLink").Value) == 1)
            {
                cb.Checked = Convert.ToBoolean(e.Row.Cells.FromKey("FlagExists").Text);
            }
        }

        //Added for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item - start
        if (!bLinkFrom) // for links From -- Adding a Host
        {
            dg.Columns.FromKey("ClassName").Hidden = true;
            dg.Columns.FromKey("MainItemPath").Hidden = false;
            dg.Columns.FromKey("ItemPath").Hidden = true;
            dg.Columns.FromKey("ItemName").Hidden = false;
        }
        else  // for links To -- Adding a Companion
        {
            dg.Columns.FromKey("ClassName").Hidden = false;
            dg.Columns.FromKey("MainItemPath").Hidden = true;
        }
        //Added for Links Requirement (PR664364) - to add Host from Companions - Companion should always be SKU Level Item - end

        if (inputType.Equals("SKU"))
        {
            dg.Columns.FromKey("ItemPath").Hidden = true;
            dg.Columns.FromKey("ItemName").Hidden = false;

            // If the proposed Sku is incorrect, change the cell style
            if (e.Row.Cells[colErrorIndex].Value != null && e.Row.Cells[colErrorIndex].Value.ToString().Length > 0)
            {
                if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value))
                {
                    e.Row.Cells.FromKey("ItemName").Style.CssClass = "hc_error";
                }

                if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value) && Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) != 2)
                {
                    e.Row.Cells.FromKey("ItemName").Text = "<b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells[colErrorIndex].Value.ToString();
                }
                else
                {
                    e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "] - " + e.Row.Cells[colErrorIndex].Value.ToString() + "</b>";
                }

                if (Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) == 2 && Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Text))
                    NbSKUs++;
            }
            else
            {
                e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b>";
                NbSKUs++;
            }
        }
        else if (inputType.Equals("NAME"))
        {
            if (bLinkFrom)  //Adding a companion
            {
                dg.Columns.FromKey("ItemName").Hidden = true;
                dg.Columns.FromKey("ItemPath").Hidden = false;
            }

            // If the Sku is incorrect, change the cell style
            if (e.Row.Cells[colErrorIndex].Value != null && e.Row.Cells[colErrorIndex].Value.ToString().Length > 0)
            {
                if (bLinkFrom)
                {
                    if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value))
                    {
                        e.Row.Cells.FromKey("ItemPath").Style.CssClass = "hc_error";
                    }

                    if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value) && Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) != 2)
                    {
                        if (e.Row.Cells.FromKey("ItemNumber").Value.ToString().Length > 0)
                        {
                            e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells[colErrorIndex].Value.ToString();
                        }
                        else
                        {
                            e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " - " + e.Row.Cells[colErrorIndex].Value.ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "] - " + e.Row.Cells[colErrorIndex].Value.ToString() + "</b>";
                    }
                }
                else
                {
                    if (!Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value))
                    {
                        e.Row.Cells.FromKey("ItemName").Style.CssClass = "hc_error";
                    }
                    else if (Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Value) && Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) == 2)
                    {
                        e.Row.Cells.FromKey("ItemName").Style.Font.Bold = true;
                    }

                    if (e.Row.Cells.FromKey("ItemNumber").Value != null && e.Row.Cells.FromKey("ItemNumber").Value.ToString().Length > 0)
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells[colErrorIndex].Value.ToString();
                    }
                    else
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " - " + e.Row.Cells[colErrorIndex].Value.ToString();
                    }
                }

                if (Convert.ToInt32(e.Row.Cells.FromKey("SameLink").Value) == 2 && Convert.ToBoolean(e.Row.Cells.FromKey("IsRelevant").Text))
                    NbSKUs++;
            }
            else
            {
                if (bLinkFrom)
                    e.Row.Cells.FromKey("ItemPath").Text = e.Row.Cells.FromKey("ItemPath").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b>";
                else
                {
                    if (e.Row.Cells.FromKey("ItemNumber").Value != null && e.Row.Cells.FromKey("ItemNumber").Value.ToString().Length > 0)
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " <b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b>";
                    }
                    else
                    {
                        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text;
                    }
                }
                NbSKUs++;
            }
        }
    }
    #endregion

    private void RetrieveSkuLevel()
    {
        ItemLevelList levels = ItemLevel.GetAll();
        foreach (ItemLevel l in levels)
        {
            if (l.SkuLevel)
            {
                SKuLevelId = l.Id;
                break;
            }
        }
    }

    protected void uwToolbar_ButtonClicked(object sender, ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "save")
        {
            if (dg.Visible)
                ApplyGridChanges();
            else
                ApplyTVChanges();
        }
    }

    //Code added for Links Requirement (PR664195) - to show/hide obsolete items based on "Include Obsolete" checkbox by Prachi on 13th Dec 2012
    protected void chkObsolete_CheckedChanged(object sender, EventArgs e)
    {
        if (webTree.Visible)
        {
            UpdateDataView();
        }
    }

    protected void chkLoad_CheckedChanged(object sender, EventArgs e)
    {
        UpdateDataView();
    }

    //Code Added for Links Requirement (PR658943) - to load the Level dropdown on 11th Jan 2013 - start
    private void LoadLevelDDL()
    {
        Database dbObj;
        using (dbObj = Utils.GetMainDB())
        {

            using (DataSet ds = dbObj.RunSQLReturnDataSet("select LevelId, LevelName from dbo.ItemLevels where LevelId between " + configLimit + " and 7"))
            {
                dbObj.CloseConnection();
                ddlLevel.DataSource = ds.Tables[0];
                ddlLevel.DataTextField = "LevelName";
                ddlLevel.DataValueField = "LevelId";
                ddlLevel.DataBind();

            }
        }
    }
    //Code Added for Links Requirement (PR658943) - to load the Level dropdown on 11th Jan 2013 - end

    //Code added for Links Requirement (PR664195) - to find out if selected host/companion is obsolete by Prachi on 16th Jan 2013 - start
    private bool IsItemObsolete()
    {
        int r = 0;
        using (HyperCatalog.Business.Item item = HyperCatalog.Business.Item.GetByKey(itemId))
        {
            r = item.IsObsolete(SessionState.Culture.Code);
        }
        if (r == 1)
            return true;
        else
            return false;
    }
    //Code added for Links Requirement (PR664195) - to find out if selected host/companion is obsolete by Prachi on 16th Jan 2013 - start

    //Adding Export funtionality as part of PR665368 - Export Button by Nisha Verma on 21 st dec
    protected void uwToolbar1_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn1 = be.Button.Key.ToLower();

        if (btn1 == "export")
        {
            Utils.ExportToExcel(dg, txtFilter.Text + SessionState.CurrentItem.Name.ToString(), txtFilter.Text + SessionState.CurrentItem.Name.ToString());
        }
    }

    protected void uwToolbarFilter_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();

        if (btn.Equals("filter"))
        {
            DisplaySearchResults();
        }
    }
}
