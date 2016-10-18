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
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebNavigator;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Xml;
using System.Text;
#endregion

/// <summary>
/// Description résumée de QDE_TV.
/// </summary>
public partial class QDE_TV : HCPage
{
  private Database dbObj;
  protected string currentNodeId = string.Empty;
  private long lastVisitedItem = -1;
  private DataSet ds=null;

  public int SkuLevel
  {
    get { return (int)ViewState["SkuLevel"]; }
    set { ViewState["SkuLevel"] = value; }

  }

  #region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    //
    // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
    //
    InitializeComponent();
    base.OnInit(e);
  }

  /// <summary>
  /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
  /// le contenu de cette méthode avec l'éditeur de code.
  /// </summary>
  private void InitializeComponent()
  {
    this.webTree.NodeBound += new Infragistics.WebUI.UltraWebNavigator.NodeBoundEventHandler(this.webTree_NodeBound);

  }
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region "Extra JS Code added because of Empty Master Template Added on 09/22/2006"
    string strObsolete = SessionState.User.ViewObsoletes ? "1" : "0";
    string strAllItems = SessionState.TVAllItems ? "1" : "0";
    string strShowTranslatedNames = (bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value ? "1" : "0";
    string strShrink = (bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_SHRINKED_NAMES).Value ? "1" : "0";
    Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "initvars", "<input type='hidden' id='aj_id' name='aj_id'/>" +
    "<input type='hidden' id='aj_uid' name='aj_uid'/>" +
    "<input type='hidden' id='aj_cc' name='aj_cc'/>" +
    "<input type='hidden' id='aj_obs' name='aj_obs'/>" +
    "<input type='hidden' id='aj_tra' name='aj_tra'/>" +
    "<input type='hidden' id='aj_shrink' name='aj_shrink'/>" +
    "<input type='hidden' id='aj_all' name='aj_all'/>" +
     "<input type='hidden' id='aj_company' name='aj_company'/>" +
    "<script>" +
    "var aj_uid = document.getElementById(\"aj_uid\");" +
    "var aj_cc = document.getElementById(\"aj_cc\");" +
    "var aj_obs = document.getElementById(\"aj_obs\");" +
    "var aj_tra = document.getElementById(\"aj_tra\");" +
    "var aj_shrink = document.getElementById(\"aj_shrink\");" +
    "var aj_all = document.getElementById(\"aj_all\");" +
    "var aj_company = document.getElementById(\"aj_company\");" +
    "aj_uid.value = " + SessionState.User.Id.ToString() + ";" +
    "aj_cc.value = '" + SessionState.Culture.Code + "';" +
    "aj_obs.value = " + strObsolete + ";" +
    "aj_all.value = " + strAllItems + ";" +
    "aj_tra.value = " + strShowTranslatedNames + ";" +
    "aj_company.value = '" + SessionState.CompanyName + "';" +
    "aj_shrink.value = " + strShrink + ";</script>");
    #endregion
    //Get the node requested, or the root if this is the real Page_Load      
    if (Request["all"] != null)
    {
      Trace.Warn("Request['all'].ToString(): " + Request["all"].ToString());
      SessionState.TVAllItems = Request["all"].ToString() == "1";
    }
    QDEUtils.UpdateCultureCodeFromRequest();
    QDEUtils.GetItemIdFromRequest();

    Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "initvars2", "<script>aj_cc.value = '" + SessionState.Culture.Code + "';</script>");
    lastVisitedItem = SessionState.TVAllItems ? SessionState.User.LastVisitedItemReadOnly : SessionState.User.LastVisitedItem;
    if (Request["i"] != null)
    {
      lastVisitedItem = Convert.ToInt64(Request["i"].ToString());
    }
    Trace.Warn("ItemId = " + lastVisitedItem.ToString() + " | AllItems = " + SessionState.TVAllItems.ToString());

    if (!Page.IsPostBack)
    {
      RetrieveSkuLevel();
      InitTreeView();
    }
    // Right Test to add
    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "language", "<script>l='" + SessionState.Culture.Code + "';skulevel=" + SkuLevel + ";</script>");
  }
  private void RetrieveSkuLevel()
  {
    using (ItemLevelList levels = ItemLevel.GetAll())
    {
      foreach (ItemLevel l in levels)
      {
        if (l.SkuLevel)
        {
          SkuLevel = l.Id;
          break;
        }
      }
    }
  }
  private void InitTreeView()
  {
    using (dbObj = Utils.GetMainDB())
    {
        using (ds = dbObj.RunSPReturnDataSet("dbo.QDE_GetTV", "",
        new SqlParameter("@UserId", SessionState.TVAllItems ? -1 : SessionState.User.Id),
        new SqlParameter("@CultureCode", HyperCatalog.Shared.SessionState.Culture.Code),
        new SqlParameter("@ParentId", -1),
        new SqlParameter("@RetrieveItemInfo", 0),
        new SqlParameter("@RetrieveObsolete", SessionState.User.ViewObsoletes),
        new SqlParameter("@RetrieveCultureItemNames", (bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value),
        new SqlParameter("@ReadOnly", SessionState.TVAllItems),
        new SqlParameter("@Company", SessionState.CompanyName)
        ))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          Debug.Trace("PRISM.UI",dbObj.LastError, DebugSeverity.High);
        }

        try
        {
          ds.Relations.Add("ItemChilds", ds.Tables[0].Columns["ItemId"], ds.Tables[1].Columns["ParentId"], false);
        }
        catch (System.Exception x)
        {
          Response.Write("step1->" + x.Message);
          Response.End();
        }
        try
        {
          webTree.DataSource = ds.Tables[0].DefaultView;
          webTree.Levels[0].RelationName = "ItemChilds";
          webTree.Levels[0].ColumnName = "ItemName";
          webTree.Levels[0].LevelKeyField = "ItemId";
          webTree.Levels[0].TargetFrameName = "IsProject";
          webTree.Levels[0].ImageColumnName = "Icon";
          using (ItemLevelList itemLevelAll = ItemLevel.GetAll())
          {
            int maxLevels = itemLevelAll.Count + 1;
            for (int i = 1; i < maxLevels; i++)
            {
              webTree.Levels[i].ColumnName = "ItemName";
              webTree.Levels[i].LevelKeyField = "ItemId";
              webTree.Levels[i].TargetFrameName = "IsProject";
              webTree.Levels[i].ImageColumnName = "ItemTypeId";
              webTree.Levels[i].TargetUrlName = "IsTopValue"; // Used for TopValue
            }
          }
          webTree.DataMember = ds.Tables[0].TableName;
          webTree.DataBind();
          ds.Dispose();
          webTree.Nodes[0].Expanded = true;
          if (SessionState.TVAllItems)
          {
            webTree.Nodes[0].Tag = 0;
          }
        }
        catch (System.Exception x)
        {
          Response.Write("step2->" + x.ToString());
          Response.End();
        }
      }
    }
  }

  private void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
  {
    //*************************************************************
    // Flag node if it has childs
    //*************************************************************
    try
    {
      if (e.Node.DataKey != null)
      {
        //Response.Write("<font size=1 face=verdana>Table Count = " + ds.Tables.Count.ToString());
        //Response.Write("<br>DataKey = " + e.Node.DataKey.ToString());
        DataTable dtChildren = e.Node.DataKey.ToString() == "-1" ? ds.Tables[0] : ds.Tables.Count > 1 ? ds.Tables[1] : ds.Tables[0];
        //Response.Write("<br>Rows Count = " + dtChildren.Rows.Count.ToString());
        //Response.Write("<br>");
        
        DataRow dr = dtChildren.Select("ItemId = " + e.Node.DataKey.ToString())[0];
        e.Node.ShowExpand = Convert.ToInt32(dr["ChildCount"]) > 0;

        //*************************************************************
        // if node is project, put in different style
        //*************************************************************
        if (Convert.ToBoolean(dr["IsProject"]))
        {
          e.Node.CssClass = "hc_pnode";
          e.Node.HoverClass = "hc_opnode";
          e.Node.HiliteClass = "hc_spnode";
        }
        e.Node.TargetFrame = e.Node.Text;
        //*************************************************************
        // Copy Item Id to node Tag so that Client script can access it
        //*************************************************************
        e.Node.Tag = e.Node.DataKey.ToString();
        if (e.Node.ShowExpand && e.Node.Nodes.Count == 0)
        {
          e.Node.Tag += "|needsLoadOnDemand";
          Node loadNode = e.Node.Nodes.Add("<img src='/hc_v4/img/ed_wait.gif'/>Loading...");
          loadNode.ImageUrl = "/hc_v4/ig/s_blank.gif";
          e.Node.Expanded = false;
        }
        if (e.Node.Parent != null)
        {
          string tag = e.Node.Parent.Tag.ToString();
          if (tag.IndexOf("|needsLoadOnDemand") > 0)
          {
            e.Node.Parent.Tag = tag.Replace("|needsLoadOnDemand", String.Empty);
            if (e.Node.Parent.Nodes[0].Text.EndsWith("Loading..."))
            {
              e.Node.Parent.Nodes.RemoveAt(0);
            }
          }
        }
        //*************************************************************
        // if node has a roll, put image
        //*************************************************************
        if (Convert.ToBoolean(dr["HasRoll"]))
        {
          e.Node.Text = "<img src='/hc_v4/img/ed_roll.gif'> " + e.Node.Text;
        }
        //*************************************************************
        // if node is country specific
        //*************************************************************
        if (Convert.ToBoolean(dr["IsCountrySpecific"]))
        {
          e.Node.Text = "<img src='/hc_v4/img/M.gif'> " + e.Node.Text;
        }
        // Add status for obsolete and future
        string status = dr["Status"].ToString();
        e.Node.ImageUrl = e.Node.SelectedImageUrl = "/hc_v4/ig/s_" + status + ".gif";

        // If level Is Sku level, update Icon
        int levelId = Convert.ToInt32(dr["LevelId"]);
        if (levelId == SkuLevel)
        {
          e.Node.ImageUrl = "/hc_v4/img/type_" + dr["ItemTypeId"].ToString() + ".png";
          if (e.Node.TargetUrl == "1") // Top Value
          {
            e.Node.ImageUrl = "/hc_v4/img/type_1.png";
          }
          e.Node.SelectedImageUrl = e.Node.ImageUrl;
          switch (status.ToLower())
          {
            case ("o"):
              e.Node.Text = "<font color=gray>[O] </font>" + e.Node.Text;
              break;
            case ("f"):
              e.Node.Text = "<font color=green>[F] </font>" + e.Node.Text;
              break;
            case ("e"):
              e.Node.Text = "<font color=red>[E] </font>" + e.Node.Text;
              break;
          }
        }
        else
        {
          if (levelId > SkuLevel)
          {
            e.Node.SelectedImageUrl = e.Node.ImageUrl = "/hc_v4/img/option.png";
          }
        }

        //*************************************************************
        // Item deleted
        //*************************************************************
        if (Convert.ToInt32(dr["PMDeleted"])>0)
          e.Node.Style.CustomRules += "text-decoration:line-through;";

        //*************************************************************
        // Clean Nodes text (remove common text with parents)
        //*************************************************************
        Infragistics.WebUI.UltraWebNavigator.Node n = e.Node;
        //// Moueen Code for SMO
        #region COMPANY COLUMN
     
        if (e.Node.Level == 1)
        {
            string company = string.Empty;
            int defaultCompany = 0;
            using (Database dbObj = Utils.GetMainDB())
            {
                using (SqlDataReader rs = dbObj.RunSPReturnRS("[GetCompanyInfoFromItemId]",
                  new SqlParameter("@ItemId", e.Node.DataKey),
                  new SqlParameter("@AlwaysDefault", defaultCompany)))
                {
                    if ((dbObj.LastError == string.Empty) && (rs.HasRows))
                    {
                        rs.Read();
                        company = rs["Company_Info"].ToString();

                    }
                }
            }

            if (!string.IsNullOrEmpty(company))
            {
                e.Node.Text = company + "-" + e.Node.Text;
            }
        }
        #endregion
        ds.Dispose();
        if ((bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_SHRINKED_NAMES).Value)
        {
          Trace.Warn("Before Clean = " + n.Text);
          Utils.CleanNodeText(ref n);
          Trace.Warn("After Clean = " + n.Text);
        }
      }
    }
    catch (Exception ex)
    {
      Response.Write(ex.ToString());
      Response.End();
    }
  }

  protected void webTree_Load(object sender, System.EventArgs e)
  {
    if (lastVisitedItem >= 0)
    {
      string nodeId = lastVisitedItem.ToString();
      // if the last visited item is roll
      HyperCatalog.Business.Item itemTemp=null;
      try
      {
        itemTemp = SessionState.CurrentItem;
        if ((itemTemp != null) && (itemTemp.IsRoll))
        {
          nodeId = itemTemp.RefItemId.ToString();
        }
        Node n = Utils.FindNodeInTree(webTree.Nodes, nodeId);
        if (n == null)
        {
          Trace.Warn("Search parents for itemId: " + lastVisitedItem.ToString());
          using (DataSet parentsDS = dbObj.RunSPReturnDataSet("dbo.QDE_GetParents", "Parents", new SqlParameter("@ItemId", lastVisitedItem.ToString())))
          {
            dbObj.CloseConnection();
            for (int i = 0; i < parentsDS.Tables["Parents"].Rows.Count; i++)
            {
              Trace.Warn("ParentId: " + parentsDS.Tables["Parents"].Rows[i]["ItemId"].ToString());
              ExpandNode(parentsDS.Tables["Parents"].Rows[i]["ItemId"].ToString());
            }
          }
          n = Utils.FindNodeInTree(webTree.Nodes, nodeId);
        }
        if (n != null)
        {
          webTree.SelectedNode = n;
          ExpandNode(nodeId);
          webTree.SelectedNode.Expand(false);
        }
        else if (webTree.Nodes.Count > 0 && webTree.Nodes[0].Nodes.Count > 0)
        {
          bool doSave = false;
          if (SessionState.User.LastVisitedItemReadOnly != Convert.ToInt64(webTree.Nodes[0].Nodes[0].DataKey))
          {
            lastVisitedItem = SessionState.User.LastVisitedItemReadOnly = Convert.ToInt64(webTree.Nodes[0].Nodes[0].DataKey);
          }
          else
          {
            if (SessionState.User.LastVisitedItem != Convert.ToInt64(webTree.Nodes[0].Nodes[0].DataKey))
            {
              lastVisitedItem = SessionState.User.LastVisitedItem = Convert.ToInt64(webTree.Nodes[0].Nodes[0].DataKey);
              doSave = true;
            }
          }
          if (doSave)
            SessionState.User.QuickSave();
        }
      }
      finally
      {
        if (itemTemp != null)
        {
          itemTemp.Dispose();
        }
      }
      if (lastVisitedItem == 0)
      {
        webTree.Nodes[0].Expand(true);
      }
    }
  }
  private void ExpandNode(string nodeId)
  {
    Trace.Warn("-->ExpandNode : " + nodeId);
    Node n = Utils.FindNodeInTree(webTree.Nodes, nodeId);
    if (n != null && !n.Expanded)
    {
      using (dbObj = Utils.GetMainDB())
      {
        using (ds = dbObj.RunSPReturnDataSet("dbo.QDE_GetTV", "Items",
          new SqlParameter("@UserId", SessionState.TVAllItems ? -1 : SessionState.User.Id),
          new SqlParameter("@CultureCode", HyperCatalog.Shared.SessionState.Culture.Code),
          new SqlParameter("@ParentId", nodeId),
          new SqlParameter("@RetrieveObsolete", SessionState.User.ViewObsoletes),
          new SqlParameter("@RetrieveCultureItemNames", (bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value),
        new SqlParameter("@ReadOnly", SessionState.TVAllItems),
        new SqlParameter("@Company", SessionState.CompanyName)))
        {
          if (dbObj.LastError == string.Empty)
          {
            try
            {
              n.DataBind(ds.Tables[0].DefaultView, "Items");
              ds.Dispose();
              n.Expanded = true;
            }
            catch (Exception ex)
            {
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + Utils.jsReplace("Error in ExpandNode for parent [#" + nodeId + "]: " + ex.ToString()) + "');", true);
            }
          }
          else
          {
           // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + Utils.jsReplace("Error in ExpandNode for parent [#" + nodeId + "]: " + dbObj.LastError) + "');", true);
          }
          dbObj.CloseConnection();
        }
      }
    }
    else {
      Trace.Warn("-->ExpandNode : node not found!");
    }
  }
  /*private void webTree_DemandLoad(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
  {
    if (e.Node.Nodes.Count == 0)
    {
      string itemId = e.Node.DataKey.ToString();
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo.QDE_GetTV", "Items",
        new SqlParameter("@UserId", SessionState.TVAllItems ? -1 : SessionState.User.Id),
        new SqlParameter("@CultureCode", HyperCatalog.Shared.SessionState.Culture.Code),
        new SqlParameter("@ParentId", itemId),
      new SqlParameter("@RetrieveObsolete", SessionState.User.ViewObsoletes),
      new SqlParameter("@RetrieveCultureItemNames", (bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value),
      new SqlParameter("@ReadOnly", SessionState.TVAllItems)))
      {
        dbObj.CloseConnection();
        e.Node.DataBind(ds.Tables[0].DefaultView, "Items");
        ds.Dispose();
        e.Node.Expanded = true;
        currentNodeId = String.Format("{0}{1}", webTree.ID, e.Node.GetIdString());
        Page.DataBind();
      }
    }
  }*/
}
