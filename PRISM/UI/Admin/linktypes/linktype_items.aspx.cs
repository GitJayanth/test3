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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using System.Data.SqlClient;
using System.Xml;
using HyperCatalog.Shared;

/// <summary>
/// LinkType tab contains items
/// </summary>
public partial class linktype_items : HCPage
{
    #region Declarations

    private bool linkFrom = true; // by default, display the list of companions 
    private int linkTypeId = -1;
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
        this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
        this.webTree.NodeBound += new Infragistics.WebUI.UltraWebNavigator.NodeBoundEventHandler(this.webTree_NodeBound);
        this.webTree.DemandLoad += new Infragistics.WebUI.UltraWebNavigator.DemandLoadEventHandler(this.webTree_DemandLoad);
        if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL)
          && !HyperCatalog.Shared.SessionState.User.IsReadOnly)
        {
            this.webTree.NodeChecked += new Infragistics.WebUI.UltraWebNavigator.NodeCheckedEventHandler(this.webTree_NodeChecked);
        }
    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            // Get parameters (Id of link type)
            if (Request["t"] != null)
                linkTypeId = Convert.ToInt32(Request["t"]);
            LinkType lt = LinkType.GetByKey(linkTypeId);

            if (lt != null)
            {
                linkFrom = uwToolbar.Items.FromKeyButton("LinkToFrom").Text.Equals("Display companions");

                if (!Page.IsPostBack)
                {
                    if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL)
                        && !HyperCatalog.Shared.SessionState.User.IsReadOnly)
                    {
                        UpdateTitles(); // Update grid title and tree title
                        InitTreeView(); // Init tree view
                        pnlTreeView.Visible = pnlTitleTreeView.Visible = true;
                    }
                    else
                    {
                        pnlTreeView.Visible = pnlTitleTreeView.Visible = false;
                    }
                    ExpandNodes(); // Expand nodes in tree view if a node is checked
                    UpdateDataView(); // Update grid
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

    #region Tree methods
    private void InitTreeView()
    {
        webTree.ClearAll();

        if (linkTypeId >= 0)
        {
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._LinkType_GetTv", "Items",
                  new SqlParameter("@LinkTypeId", linkTypeId),
                  new SqlParameter("@ParentId", -1),
                  new SqlParameter("@LinkFromTo", Convert.ToByte(!linkFrom)),
                  new SqlParameter("@Company", SessionState.CompanyName)))
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
                            try
                            {
                                ds.Relations.Add("ItemChilds", ds.Tables[0].Columns["ItemId"], ds.Tables[1].Columns["ParentId"], false);

                                webTree.DataSource = ds.Tables[0].DefaultView;
                                webTree.Levels[0].RelationName = "ItemParents";
                                for (int i = 0; i < 8; i++)
                                {
                                    webTree.Levels[i].ColumnName = "ItemName";
                                    webTree.Levels[i].LevelKeyField = "ItemId";
                                    webTree.Levels[i].CheckboxColumnName = "InLinkTypeScope";
                                }
                                webTree.DataMember = "Items";
                                webTree.DataBind();
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
    }
    private void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
    {
        if (e.Node != null)
        {
            e.Node.ShowExpand = Utils.GetCount(dbObj, "SELECT COUNT(*) FROM Items WHERE ParentId = " + e.Node.DataKey.ToString()) > 0;
            if (e.Node.Parent != null)
            {
                if (e.Node.Parent.Checked)
                {
                    // remove checkbox
                    e.Node.Checked = true;
                    e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
                }
                else
                {
                    // add checkbox
                    e.Node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;
                }

                if (e.Node.Checked)
                {
                    Infragistics.WebUI.UltraWebNavigator.Node p = e.Node.Parent;
                    while (p != null)
                    {
                        if (p.Level > 0)
                            p = p.Parent;
                        else
                            p = null;
                    }
                }
            }

            // Update image
            e.Node.ImageUrl = "/hc_v4/ig/s_l.gif";

            HyperCatalog.Business.Item currentItem = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(e.Node.DataKey));
            if ((currentItem != null) && (currentItem.Level.SkuLevel))
            {
                // Update Icon
                e.Node.ImageUrl = "/hc_v4/img/type_" + currentItem.TypeId + ".png";
            }
        }
    }
    private void webTree_NodeChecked(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeCheckedEventArgs e)
    {
        if (!HyperCatalog.Shared.SessionState.User.IsReadOnly
          && HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
        {
            if (e.Node.Checked == true)
            {
                if (!AddItem(e.Node.DataKey.ToString()))
                {
                    e.Node.Checked = false;
                    return;
                }
                RecurseNodes(e.Node, true);
            }
            else
            {
                RecurseNodes(e.Node, false);
                if (!RemoveItem(e.Node.DataKey.ToString()))
                {
                    e.Node.Checked = true;
                    return;
                }
            }
            UpdateDataView();
        }
        else
        {
            e.Node.Enabled = false;
        }
    }
    private void webTree_DemandLoad(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
    {
        lbError.Visible = false;

        if (e.Node.Nodes.Count == 0)
        {
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._LinkType_GetTv", "Items",
                  new SqlParameter("@LinkTypeId", linkTypeId),
                  new SqlParameter("@ParentId", e.Node.DataKey.ToString()),
                  new SqlParameter("@LinkFromTo", Convert.ToByte(!linkFrom)),
                  new SqlParameter("@Company", SessionState.CompanyName)))
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
                        e.Node.DataBind(ds.Tables[0].DefaultView, "Items");
                        e.Node.Expanded = true;

                        if (ds != null)
                            ds.Dispose();

                        Page.DataBind();
                    }
                }
            }
        }
        UpdateDataView();
    }
    #endregion

    #region Grid methods
    private void UpdateDataView()
    {
        if (linkTypeId >= 0)
        {
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._LinkType_GetItems", "Items",
                  new SqlParameter("@LinkTypeId", linkTypeId),
                  new SqlParameter("@LinkFromTo", Convert.ToByte(!linkFrom)),
                      new SqlParameter("@Company", SessionState.CompanyName)))
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
                            dg.DataSource = ds.Tables[0].DefaultView;
                            Utils.InitGridSort(ref dg, true);
                            dg.DataBind();

                            dg.Visible = true;

                            // Refresh title tab
                            UITools.RefreshTab(Page, "Items", ds.Tables[0].Rows.Count);

                            ds.Dispose();
                        }
                    }
                }
            }
        }
    }
    #endregion

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn.Equals("linktofrom"))
        {
            linkFrom = !uwToolbar.Items.FromKeyButton("LinkToFrom").Text.Equals("Display companions");
            if (!linkFrom)
            {
                uwToolbar.Items.FromKeyButton("LinkToFrom").Text = "Display host"; // Update button
                lblTitleTree.Text = "Possible companions";// Update tree title
                lblTitleGrid.Text = "Link type scope (Companions)"; // Update grid title
            }
            else
            {
                uwToolbar.Items.FromKeyButton("LinkToFrom").Text = "Display companions"; // Update button
                //Code modified for Links Requirement (PR668013) - to change 'Hardware' to 'Host' by Prachi on 10th Dec 2012
                lblTitleTree.Text = "Possible host";// Update tree title
                lblTitleGrid.Text = "Link type scope (Host)"; // Update grid title
            }

            InitTreeView(); // update treeview
            ExpandNodes();
            UpdateDataView(); // update grid
        }
    }

    #region Private methods
    private void UpdateTitles()
    {
        if (!linkFrom)
        {
            // list of possible companions
            lblTitleTree.Text = "Possible companions";
            lblTitleGrid.Text = "Link type scope (Companions)";
        }
        else
        {
            // list of possible hardware
            //Code modified for Links Requirement (PR668013) - to change 'Hardware' to 'Host' by Prachi on 10th Dec 2012 - start
            //lblTitleTree.Text = "Possible hardware";
            //lblTitleGrid.Text = "Link type scope (Hardware)";
            lblTitleTree.Text = "Possible host";
            lblTitleGrid.Text = "Link type scope (Host)";
            //Code modified for Links Requirement (PR668013) - to change 'Hardware' to 'Host' by Prachi on 10th Dec 2012 - end
        }
    }
    private void RecurseNodes(Infragistics.WebUI.UltraWebNavigator.Node n, bool check)
    {
        foreach (Infragistics.WebUI.UltraWebNavigator.Node node in n.Nodes)
        {
            if (check)
            {
                if (node.Checked)
                {
                    RemoveItem(node.DataKey.ToString());
                }
                node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.False;
            }
            else
            {
                node.CheckBox = Infragistics.WebUI.UltraWebNavigator.CheckBoxes.True;
            }
            RecurseNodes(node, check);
            node.Checked = check;
        }
    }
    private bool AddItem(string itemId)
    {
        lbError.Visible = false;
        using (LinkType lt = LinkType.GetByKey(linkTypeId))
        {

            if ((itemId != null) && (lt != null))
            {
                if (!lt.AddItem(Convert.ToInt64(itemId), linkFrom))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = LinkType.LastError;
                    lbError.Visible = true;

                    return false;
                }
                else
                    return true;
            }
        }
        return false;
    }
    private bool RemoveItem(string itemId)
    {
        lbError.Visible = false;
        using (LinkType lt = LinkType.GetByKey(linkTypeId))
        {

            if ((itemId != null) && (lt != null))
            {
                if (!lt.RemoveItem(Convert.ToInt64(itemId), linkFrom))
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = LinkType.LastError;
                    lbError.Visible = true;

                    return false;
                }
                else
                    return true;
            }
        }
        return false;
    }
    private void ExpandNodes()
    {
        using (dbObj = Utils.GetMainDB())
        {

            using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._LinkType_GetItems", "Items",
              new SqlParameter("@LinkTypeId", linkTypeId),
              new SqlParameter("@LinkFromTo", Convert.ToByte(!linkFrom)),
              new SqlParameter("@Company", SessionState.CompanyName)))
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
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            long currentItemId = Convert.ToInt64(dr["ItemId"]);
                            Infragistics.WebUI.UltraWebNavigator.Node n = Utils.FindNodeInTree(webTree.Nodes, currentItemId.ToString());

                            if (n == null)
                            {
                                DataSet parentsDS = dbObj.RunSPReturnDataSet("dbo.QDE_GetParents", "Parents", new SqlParameter("@ItemId", currentItemId));
                                dbObj.CloseConnection();
                                for (int i = 0; i < parentsDS.Tables["Parents"].Rows.Count; i++)
                                {
                                    ExpandNode(parentsDS.Tables["Parents"].Rows[i]["ItemId"].ToString());
                                }
                                parentsDS.Dispose();
                                n = Utils.FindNodeInTree(webTree.Nodes, currentItemId.ToString());
                            }
                        }
                        ds.Dispose();
                    }
                }
            }
        }
    }
    private void ExpandNode(string nodeId)
    {
        Infragistics.WebUI.UltraWebNavigator.Node n = Utils.FindNodeInTree(webTree.Nodes, nodeId);
        if (n != null)
        {
            using (dbObj = Utils.GetMainDB())
            {
                using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._LinkType_GetTv", "Items",
                  new SqlParameter("@LinkTypeId", linkTypeId),
                  new SqlParameter("@ParentId", nodeId),
                  new SqlParameter("@LinkFromTo", Convert.ToByte(!linkFrom)),
                  new SqlParameter("@Company", SessionState.CompanyName)))
                {
                    dbObj.CloseConnection();
                    n.DataBind(ds.Tables[0].DefaultView, "Items");
                    ds.Dispose();
                    n.Expanded = true;
                }
            }
        }
    }
    #endregion
}
