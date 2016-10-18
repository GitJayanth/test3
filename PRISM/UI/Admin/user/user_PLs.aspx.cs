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
public partial class user_PLs : HCPage
{
  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
    }
    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
    }
    #endregion

    if (!Page.IsPostBack)
    {
      BindClasses();
    }
    lbTitle.Text = SessionState.ItemLevels[1].Name;
  }
  protected override void OnLoadComplete(EventArgs e)
  {
    base.OnLoadComplete(e);
    Debug.Trace("WEBUI","User checked PLs = " + SessionState.EditedUser.PLs.Count.ToString() + " items",DebugSeverity.Low);
    PLTree.CheckPLs(SessionState.EditedUser.PLs);
  }

  #region Data load & bind
  private void BindClasses()
  {
    using (ItemList iClasses = HyperCatalog.Business.Item.GetAll("LevelId=1"))
    {
      classes.DataSource = iClasses;
      classes.DataBind();
    }

    foreach (ListItem item in classes.Items)
    {
      item.Selected = SessionState.EditedUser.HasItemInScope(Convert.ToInt64(item.Value));
      item.Enabled = (SessionState.EditedUser.RoleId != 0);
      if (SessionState.EditedUser.RoleId == 0)
      {
        item.Selected = true;
      }

      if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS) || SessionState.User.IsReadOnly)
        item.Enabled = false;
    }
  }
  #endregion

  private void Export()
  {
    using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 3600))
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("_Report_GetGetFlatCategorizationPerUser", new SqlParameter("@UserId", SessionState.EditedUser.Id)))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != null && dbObj.LastError.Length > 0)
        {
          lbError.Text = dbObj.LastError;
          lbError.CssClass = "hc_error";
          lbError.Visible = true;
        }
        else
        {
          if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
          {
            Utils.ExportDataSetToExcel(this.Page, ds, Utils.CleanFileName(SessionState.EditedUser.FullName + " product coverage.xls"));
          }
          else
          {
            lbError.Text = "No product to export.";
            lbError.CssClass = "hc_error";
            lbError.Visible = true;
          }
        }
      }
    }
    
  }
  private void Save()
  {
    bool success = true;
    success = SessionState.EditedUser.ClearItemsScope();
    if (success)
    {
      int classesCount = 0;
      foreach (ListItem item in classes.Items)
      {
        if (item.Selected)
        {
          classesCount ++;
          success = success && SessionState.EditedUser.AddItemInScope(Convert.ToInt64(item.Value));
        }
      }
      if (SessionState.EditedUser.Id == HyperCatalog.Shared.SessionState.User.Id)
      {
        SessionState.User.Dispose();
        HyperCatalog.Shared.SessionState.User = SessionState.EditedUser;
      }

      PLList pls = PLTree.GetCheckedPLs();
      /*if (pls.Count > 0 && classesCount > 0)
      {*/
        SessionState.EditedUser.PLs.Clear();
        foreach (PL pl in pls)
        {
          SessionState.EditedUser.PLs.Add(pl);
        }
        success = success && SessionState.EditedUser.Save(false, true);
        if (success)
        {
          SessionState.EditedUser.UpdateItemScope();
          if (SessionState.EditedUser.Id == SessionState.User.Id)
          {
            HyperCatalog.Shared.SessionState.User = SessionState.EditedUser;
          }

          foreach (ListItem item in classes.Items)
          {
            item.Selected = SessionState.EditedUser.HasItemInScope(Convert.ToInt64(item.Value));
            item.Enabled = (SessionState.EditedUser.RoleId != 0);
            if (SessionState.EditedUser.RoleId == 0)
            {
              item.Selected = true;
            }

            if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS) || SessionState.User.IsReadOnly)
              item.Enabled = false;
          }

          lbError.Text = "Data saved! (the product type displayed are only relevant for the selected product lines)";
          lbError.CssClass = "hc_success";
          lbError.Visible = true;
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.User.LastError;
          lbError.Visible = true;
        }

        SessionState.EditedUser.Dispose();
      /*}
      else
      {
        if (pls.Count == 0 && classesCount > 0)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "You must select at least one Product Line.";
          lbError.Visible = true;
        }
        else if (pls.Count > 0 && classesCount == 0)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "You must select at least one Classe.";
          lbError.Visible = true;
        }
      }*/
    }
    else
    {
      lbError.CssClass = "hc_error";
      lbError.Text = HyperCatalog.Business.User.LastError;
      lbError.Visible = true;
    }
  }

  #region Events
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "save")
      Save();
    if (btn == "export")
      Export();
  }
  #endregion
}
