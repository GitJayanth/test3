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
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

/// <summary>
/// Display product list in selected user's scope
///		--> Return to the user list
///		--> Select products
/// </summary>
public partial class user_products : HCPage
{
    #region Declarations

    private HyperCatalog.Business.User user = null;
    private Database dbObj;

    #endregion

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
        this.webTree.DemandLoad += new Infragistics.WebUI.UltraWebNavigator.DemandLoadEventHandler(this.webTree_DemandLoad);
        this.webTree.NodeChecked += new Infragistics.WebUI.UltraWebNavigator.NodeCheckedEventHandler(this.webTree_NodeChecked);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            if (Request["u"] != null)
            {
                int userId = Convert.ToInt32(Request["u"]);
                ViewState["userId"] = userId.ToString();


                if (!Page.IsPostBack)
                {
                    InitTreeView();
                    webTree.Nodes[0].Expand(true);
                    UpdateDataView();
                }
            }
            else
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
        }
        catch
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
        }
    }

    private void webTree_DemandLoad(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
    {
        string itemId = e.Node.DataPath.ToString();
        e.Node.DataKey = itemId;
        if (e.Node.Nodes.Count == 0)
        {
            // Retrieve all children for a given parent
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._User_ScopeGetTV", "Items",
                  new SqlParameter("@UserId", -1),
                  new SqlParameter("@ParentId", itemId)))
                {
                    e.Node.DataBind(ds.Tables[0].DefaultView, "Items");
                    dbObj.CloseConnection();
                    e.Node.Expanded = true;

                    if (ds != null)
                        ds.Dispose();
                }
            }
        }
    }

    private void webTree_NodeChecked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeCheckedEventArgs e)
    {
        if (e.Node.Checked == true)
        {
            // Add item in the user's scope
            AddItemInScope(e.Node.DataKey.ToString());
            RecurseNodes(e.Node, true);
        }
        else
        {
            // Remove item in the user's scope
            RecurseNodes(e.Node, false);
            RemoveItemFromScope(e.Node.DataKey.ToString());
        }

        // Update grid
        UpdateDataView();
    }


    #region Private methods
    private void UpdateDataView()
    {
        if (user.Id > -1)
        {
            // DataSet contains all items in the scope of selected user
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._User_GetItems", "Items", new SqlParameter("@UserId", user.Id), new SqlParameter("@Company", SessionState.CompanyName)))
                {
                    dbObj.CloseConnection();
                    if (dbObj.LastError.Length > 0)
                    {
                        lbError.CssClass = "hc_error";
                        lbError.Text = dbObj.LastError;
                        lbError.Visible = true;
                    }
                    else
                    {
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                // Update grid
                                dg.DataSource = ds.Tables[0].DefaultView;
                                Utils.InitGridSort(ref dg);
                                dg.DataBind();

                                dg.Visible = true;

                                // Refresh count in the tab (count of selected item)
                                UITools.RefreshTab(Page, "Items", ds.Tables[0].Rows.Count);
                            }
                            else
                            {
                                lbError.CssClass = "hc_error";
                                lbError.Text = "Nothing in user scope...";
                                lbError.Visible = true;
                                dg.Visible = false;
                            }
                            ds.Dispose();
                        }
                    }
                }
            }
        }
    }
    private void InitTreeView()
    {
        if (user.Id > -1)
        {
            // DataSet contains all classes
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._User_ScopeGetTV", "Items", new SqlParameter("@UserId", user.Id)))
                {
                    dbObj.CloseConnection();

                    if (dbObj.LastError.Length > 0)
                    {
                        // Error
                        lbError.CssClass = "hc_error";
                        lbError.Text = dbObj.LastError;
                        lbError.Visible = true;
                    }
                    else
                    {
                        if (ds != null)
                        {
                            try
                            {
                                ds.Relations.Add("ItemChilds", ds.Tables[0].Columns["ItemId"], ds.Tables[1].Columns["ParentId"], false);

                                webTree.DataSource = ds.Tables[0].DefaultView;
                                webTree.Levels[0].RelationName = "ItemParents";
                                for (int i = 0; i < 8; i++)
                                {
                                    webTree.Levels[i].ColumnName = "ItemName";
                                    webTree.Levels[i].LevelKeyField = "ItemId";
                                    webTree.Levels[i].TargetFrameName = "ItemId";
                                    webTree.Levels[i].CheckboxColumnName = "InUserScope";
                                }
                                webTree.DataMember = "Items";
                                webTree.DataBind();

                                // Update treeview for expand all selected node
                                UpdateTV();
                            }
                            catch (Exception e)
                            {
                                lbError.CssClass = "hc_error";
                                lbError.Text = e.Message;
                                lbError.Visible = true;
                            }
                            finally
                            {
                                ds.Dispose();
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // Error
            lbError.CssClass = "hc_error";
            lbError.Text = "The current user doesn't exist";
            lbError.Visible = true;
        }

    }
    private void UpdateTV()
    {
        using (dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._User_GetItems", "Items", new SqlParameter("@UserId", user.Id), new SqlParameter("@Company", SessionState.CompanyName)))
            {
                dbObj.CloseConnection();
                if (dbObj.LastError.Length > 0)
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = dbObj.LastError;
                    lbError.Visible = true;
                }
                else
                {
                    if (ds != null)
                    {
                        // for each item
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int levelId = Convert.ToInt32(dr["LevelId"]);
                            if (levelId > 1)
                            {
                                // DataSet contains all parents of the given item
                                DataSet parentsDS = dbObj.RunSPReturnDataSet("dbo.QDE_GetParents", "Parents",
                                  new SqlParameter("@ItemId", Convert.ToInt64(dr["ItemId"])));
                                dbObj.CloseConnection();

                                // Expand each parent
                                for (int i = 0; i < parentsDS.Tables["Parents"].Rows.Count; i++)
                                {
                                    ExpandNode(parentsDS.Tables["Parents"].Rows[i]["ItemId"].ToString());
                                }

                                parentsDS.Dispose();
                            }
                        }
                    }
                }
            }
        }
    }
    private void ExpandNode(string nodeId)
    {
        Infragistics.WebUI.UltraWebNavigator.Node n = Utils.FindNodeInTree(webTree.Nodes, nodeId);
        if (n != null && !n.Expanded)
        {
            using (dbObj = Utils.GetMainDB())
            {
                // Retrieve all children
                DataSet ds = dbObj.RunSPReturnDataSet("dbo._User_ScopeGetTV", "Items",
                  new SqlParameter("@UserId", -1),
                  new SqlParameter("@ParentId", nodeId));
                n.DataBind(ds.Tables[0].DefaultView, "Items");
                dbObj.CloseConnection();
            }
            n.Expanded = true;
        }
    }

    private void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
    {
        if (e.Node != null)
        {
            string sItemId = e.Node.DataKey.ToString();
            e.Node.Text = e.Node.Text + " [#" + e.Node.DataKey.ToString() + "]";
            e.Node.DataPath = sItemId;

            // Retrieve the count of children, if the count is positive then the current node can be expanded
            e.Node.ShowExpand = Utils.GetCount(dbObj, String.Format("SELECT COUNT(*) FROM Items WHERE ParentId = {0}", sItemId)) > 0;

            using (HyperCatalog.Business.Item currentItem = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(sItemId)))
            {

                // Update checkbox
                if (user != null && user.HasItemInScope(currentItem.Id))
                    e.Node.Checked = true;

                if (e.Node.Parent != null)
                {
                    Infragistics.WebUI.UltraWebNavigator.Node p = e.Node.Parent;
                    if (p.Checked)
                    {
                        e.Node.Checked = true;
                        e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
                    }
                    else
                    {
                        e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;
                    }
                    if (e.Node.Checked)
                    {
                        while (p != null)
                        {
                            if (p.Level > 0)
                                p = p.Parent;
                            else
                                p = null;
                        }
                    }
                }

                if ((currentItem != null) && (currentItem.Level.SkuLevel))
                {
                    // Update Icon
                    e.Node.ImageUrl = "/hc_v4/img/type_" + currentItem.TypeId + ".png";
                }
            }
        }
    }

    private void AddItemInScope(string itemId)
    {
        // Add item in the scope
        lbError.Visible = false;
        lbError.Text = string.Empty;
        if (!user.AddItemInScope(Convert.ToInt64(itemId)))
        {
            lbError.CssClass = "hc_error";
            lbError.Text += "Error: Scope couldn't be updated<br/>";
            lbError.Visible = true;
        }
    }

    private void RemoveItemFromScope(string itemId)
    {
        // Remove item in the scope
        lbError.Visible = false;
        lbError.Text = string.Empty;
        if (!user.RemoveItemFromScope(Convert.ToInt64(itemId)))
        {
            lbError.CssClass = "hc_error";
            lbError.Text += "Error: Scope couldn't be updated<br/>";
            lbError.Visible = true;
        }
    }

    private void RecurseNodes(Infragistics.WebUI.UltraWebNavigator.Node n, bool check)
    {
        for (int i = 0; i < n.Nodes.Count; i++)
        {
            if (check)
            {
                if (n.Nodes[i].Checked)
                {
                    RemoveItemFromScope(n.Nodes[i].DataKey.ToString());
                }
                n.Nodes[i].CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
            }
            else
            {
                n.Nodes[i].CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;
            }
            RecurseNodes(n.Nodes[i], check);
            n.Nodes[i].Checked = check;
        }
    }
    #endregion
}
