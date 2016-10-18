#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using HyperCatalog.Shared;
#endregion

#region History
// create by Pervenche REY 19/09/2006
/*=============Modification Details====================================
      Mod#1 Tuned the Search functionality
      Description: QC#723
      Modfication Date:12/10/2007
      Modified by: Jothi*/
#endregion

public partial class UI_Admin_pls_pl_users : HCPage
{
  #region Declarations
  private string plCode = null;
  private CollectionView vPLUsers;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("ApplyChanges").Enabled = false;
    }
    #endregion
    try
    {
       if (Request["p"] != null)
        plCode = Request["p"];
        

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
  }

  private void UpdateDataView()
  {
      UserList plUsers = PL.GetByKey(plCode).Users;
      vPLUsers = new CollectionView(plUsers);

      UserList users = HyperCatalog.Business.User.GetAll("IsActive = 1");
      if (users != null)
      {
          dg.DataSource = users;
          Utils.InitGridSort(ref dg, false);
          dg.DataBind();

          if (dg.Rows.Count > 0)
          {
              dg.Visible = true;
              lbNoresults.Visible = false;
          }
          else
          {
              lbNoresults.Text = "No user found";
              dg.Visible = false;
              lbNoresults.Visible = true;
          }
      }
  }

  public void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "export")
    {
      Utils.ExportToExcel(dg, "PLUsers", "PLUsers");
    }
    if (btn == "applychanges")
    {
      ApplyChanges();
    }
  }

  public void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
      /******** QC#723 (rowDel is true if row got deleted )****************/
      bool rowDel = false;
    Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
    vPLUsers.ApplyFilter("Id", r.Cells.FromKey("UserId").Value, CollectionView.FilterOperand.Equals);
    if (vPLUsers.Count > 0)
    {
      TemplatedColumn col = (TemplatedColumn)r.Cells.FromKey("Select").Column;
      CheckBox cb = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
      cb.Checked = true;
    }
    if (!cbFilterAllUsers.Checked)
    {
      TemplatedColumn col = (TemplatedColumn)r.Cells.FromKey("Select").Column;
      CheckBox cb = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
      /******** QC#723 (setting rowDel to true)****************/
      if (!cb.Checked) { dg.Rows.Remove(r); rowDel = true; }
    
    }
    #region filter
    /******** QC#723 (No need to search in deleted rows.)****************/
    if (txtFilter.Text.Length > 0 && !rowDel)
    {
      string filter = txtFilter.Text.Trim().ToLower();
      string userName = r.Cells.FromKey("UserName").Value.ToString().ToLower();
      string orgName = r.Cells.FromKey("OrgName").Value.ToString().ToLower();
      string roleName = r.Cells.FromKey("RoleName").Value.ToString().ToLower();

      if ((userName.Length == 0 || userName.IndexOf(filter) < 0) &&
          (orgName.Length == 0 || orgName.IndexOf(filter) < 0) &&
          (roleName.Length == 0 || roleName.IndexOf(filter) < 0))
        dg.Rows.Remove(r);
      else
        UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }
    #endregion
  }

  public void cbFilterAllUsers_CheckedChanged(object sender, System.EventArgs e)
  {
    UpdateDataView();
  }

  private void ApplyChanges()
  {
    #region Remove all users attached to this PL
    using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 240))
    {
      dbObj.RunSQLQuery("DELETE FROM UserPLs WHERE PLCode = '" + plCode + "'");
    #endregion
      if (dbObj.LastError != string.Empty)
      {
        lbError.Text = "Error: impossible to remove all users - " + dbObj.LastError;
        lbError.CssClass = "hc_error";
        lbError.Visible = true;
      }
      else
      {
        for (int i = 0; i < dg.Rows.Count; i++)
        {
          TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
          CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
          if (cb.Checked)
          {
            using (User u = HyperCatalog.Business.User.GetByKey(Convert.ToInt32(dg.Rows[i].Cells.FromKey("UserId").Value)))
            {
              u.PLs.Add(PL.GetByKey(plCode));
              if (!u.Save(false, true))
              {
                lbError.Text = "Error: impossible to add the user " + u.FullName + " to the PL " + plCode;
                lbError.CssClass = "hc_error";
                lbError.Visible = true;
                break;
              }
            }
          }
        }
        UpdateDataView();
        lbError.Text = "Changes applied";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
      }
    }
  }
}
