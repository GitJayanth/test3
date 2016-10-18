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
using Infragistics.WebUI.UltraWebNavigator;
using HyperCatalog.Shared;
#endregion

public partial class PLWebTree : System.Web.UI.UserControl
{
  private bool _Expanded = true;
  public bool Expanded
  {
    get
    {
      return _Expanded;
    }
    set
    {
      _Expanded = value;
    }
  }

  #region Declarations

  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      BindTreeView();
    }
  }

  #region Data load & bind
  private void BindTreeView()
  {
    Trace.Write("PLWebTree", "BindTreeView Starts");
    // DataSet contains all classes
    using (Database dbObj = Utils.GetMainDB())
    {
        //using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT * FROM View_PLOrganizations WHERE IsActive = 1; SELECT * FROM View_PLGroups WHERE IsActive = 1; SELECT * FROM View_PLGBUs WHERE IsActive = 1; SELECT * FROM View_PLs WHERE IsActive = 1"))
       //Added by Seela 10/05/16
        string query = string.Empty;
        if (SessionState.CompanyName == "HPI")
        {
            query = "SELECT * FROM View_PLOrganizations where OrgCode <>54; SELECT * FROM View_PLGroups where OrgCode <>54 ; SELECT * FROM View_PLGBUs where OrgCode <>54 ; SELECT * FROM View_PLs where OrgCode <>54";
        }
        else
        {
            query = "SELECT * FROM View_PLOrganizations where OrgCode =54; SELECT * FROM View_PLGroups where OrgCode =54 ; SELECT * FROM View_PLGBUs where OrgCode =54 ; SELECT * FROM View_PLs where OrgCode =54";
        }
        //Added by Seela
        using (DataSet ds = dbObj.RunSQLReturnDataSet(query))
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
              try
              {
                ds.Relations.Add("OrgGroup",
                    ds.Tables[0].Columns["OrgCode"],
                    ds.Tables[1].Columns["OrgCode"]);
              }
              catch (System.Exception x)
              {
                lbError.CssClass = "hc_error";
                lbError.Text = "1" + x.Message;
                lbError.Visible = true;
              }

              try
              {
                ds.Relations.Add("GroupGBU",
                    ds.Tables[1].Columns["GroupCode"],
                    ds.Tables[2].Columns["GroupCode"]);
              }
              catch (System.Exception x)
              {
                lbError.CssClass = "hc_error";
                lbError.Text = "2" + x.Message;
                lbError.Visible = true;
              }

              try
              {
                ds.Relations.Add("GBUPL",
                    ds.Tables[2].Columns["GBUCode"],
                    ds.Tables[3].Columns["GBUCode"]);
              }
              catch (System.Exception x)
              {
                lbError.CssClass = "hc_error";
                lbError.Text = "3" + x.Message;
                lbError.Visible = true;
              }


              webTree.DataSource = ds;
              webTree.Levels[0].RelationName = "OrgGroup";
              webTree.Levels[0].ColumnName = "OrgName";
              webTree.Levels[0].LevelKeyField = "OrgCode";
              webTree.Levels[0].LevelClass = "org";
              webTree.Levels[1].RelationName = "GroupGBU";
              webTree.Levels[1].ColumnName = "GroupName";
              webTree.Levels[1].LevelKeyField = "GroupCode";
              webTree.Levels[1].LevelClass = "group";
              webTree.Levels[2].RelationName = "GBUPL";
              webTree.Levels[2].ColumnName = "GBUName";
              webTree.Levels[2].LevelKeyField = "GBUCode";
              webTree.Levels[2].LevelClass = "gbu";
              webTree.Levels[3].ColumnName = "PLName";
              webTree.Levels[3].LevelKeyField = "PLCode";
              webTree.Levels[3].LevelClass = "pl";
              webTree.DataMember = ds.Tables[0].TableName;
              webTree.DataBind();

              //webTree.Enabled = (HyperCatalog.Shared.SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_USERS) && !HyperCatalog.Shared.SessionState.User.IsReadOnly);

              if (_Expanded)
                webTree.ExpandAll();
            }
            catch (Exception e)
            {
              lbError.CssClass = "hc_error";
              lbError.Text = e.ToString();
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
    Trace.Write("PLWebTree", "BindTreeView Ends");
  }
  #endregion

  public PLList GetCheckedPLs()  
  {
    PLList result = new PLList();
    foreach (Node node in webTree.CheckedNodes)
    {
      if (node.Nodes.Count == 0)
      {
        PL pl = new PL(node.DataKey.ToString(), node.Text.Substring(5, node.Text.Length - 5),
          node.Parent.Parent.Parent.DataKey.ToString(),
          node.Parent.Parent.DataKey.ToString(),
          node.Parent.DataKey.ToString(), true);
        Trace.Warn("New PL added  code = " + node.DataKey.ToString() + " | name = " + node.Text.Substring(5, node.Text.Length - 5) + " | OrgCode = " +
          node.Parent.Parent.Parent.DataKey.ToString() + " | GroupCode=" +
          node.Parent.Parent.DataKey.ToString() + " | GBUCode=" + 
          node.Parent.DataKey.ToString());
        result.Add(pl);
      }
    }
    return result;
  }
  public void CheckPLs(PLList pls)  
  {
    foreach (Node org in webTree.Nodes)
      foreach (Node group in org.Nodes)
        foreach (Node gbu in group.Nodes)
          foreach (Node leaf in gbu.Nodes)
            foreach (PL pl in pls)
              if (leaf.DataKey.ToString() == pl.Code)
              {
                leaf.Checked = true;
                break;
              }
  }

  #region Events
  protected void webTree_NodeBound(object sender, Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs e)
  {
    //if (e.Node.Level == 3)
      e.Node.Text = "[" + e.Node.DataKey + "] " + e.Node.Text;
  }
  #endregion}
}
