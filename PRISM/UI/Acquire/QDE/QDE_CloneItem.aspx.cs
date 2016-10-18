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
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// QDE_CloneItem class clones item
  /// </summary>
  public partial class QDE_CloneItem : HCPage
  {
    #region Declarations
    public HyperCatalog.Business.Item itemObj;
    private Database dbObj;
    #endregion

    // Use to call web service in asynchronous method
    //delegate bool ItemUpdateWorkingTables(System.Int64 itemId, bool isSku);

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Retrieve Item information
      try
      {
        if (Request["i"] != null)
          itemObj = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(Request["i"]));
        else if (QDEUtils.GetItemIdFromRequest() != null)
          itemObj = QDEUtils.GetItemIdFromRequest();

        if (Request["c"] != null)
          QDEUtils.UpdateCultureCodeFromRequest();
      }
      catch
      {
        throw new ArgumentException("Item Id was not provided");
      }
      #endregion

      if (itemObj != null)
      {
        #region Capability
        if ((SessionState.User.IsReadOnly) && (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS)) &&
          (itemObj.LevelId < Convert.ToInt32(SessionState.CacheParams["Item_CloneMaxLevel"].Value)))
        {
          UITools.JsCloseWin();
        }
        #endregion
        if (HyperCatalog.Business.ApplicationParameter.IOHeirachyStatus() == 1)
        {
            UITools.JsCloseWin("The Product Hierarchy Refresh job is in progress.  User cannot perform this action.  Please try once the job completes!");

        }
        try
        {
          if (!Page.IsPostBack)
          {
            UpdateDataView();
          }
        }
        catch
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>window.Close();</script>");
        }
      }
    }
    private void UpdateDataView()
    {
      if (itemObj != null)
      {
        lbTitle.Text = "Clone product - " + itemObj.FullName;
        lbCloneCount.Text = "Number of clone";
        lbParents.Text = "You are copying a product which level is " + itemObj.Level.Name + ".</br> The program is showing to you the different possible destination parents to copy it.";
        lbError.Visible = false;
        if (itemObj.Id >= 0)
        {
          // list of possible parents
          using (dbObj = Utils.GetMainDB())
          {
            if (dbObj != null)
            {
              dbObj.TimeOut = 300;
              using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_GetPossibleParentsForCloningMoving", "Items",
                new SqlParameter("@ItemId", itemObj.Id),
                new SqlParameter("@UserId", SessionState.User.Id)))
              {
                dbObj.CloseConnection();
                if (ds != null)
                {
                  if (dbObj.LastError.Length > 0)
                  {
                    // Error
                    lbError.CssClass = "hc_error";
                    lbError.Text = dbObj.LastError;
                    lbError.Visible = true;
                  }
                  else
                  {
                    // display list
                    rPossibleParents.DataSource = ds;
                    rPossibleParents.DataBind();
                  }

                }
              }
            }
          }
        }
      }
      else
      {
        // Error;
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>window.Close();</script>");
      }
    }
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn.Equals("clone"))
      {
        Clone();
      }
    }
    private void Clone()
    {
      int cloneCount = wneCloneCount.ValueInt;
      System.Int64 parentId = Convert.ToInt64(Request.Form["Parent"]);
      if (parentId > 0)
      {
        using (dbObj = Utils.GetMainDB())
        {
          if (dbObj != null)
          {
            dbObj.TimeOut = 300;
            int newItemId = dbObj.RunSPReturnInteger("dbo._Item_CloneProduct",
               new SqlParameter("@UserId", SessionState.User.Id),
               new SqlParameter("@ItemId", itemObj.Id),
               new SqlParameter("@ParentId", parentId),
               new SqlParameter("@CloneCount", cloneCount));
            dbObj.CloseConnection();

            if (newItemId < 0) // error
            {
              lbError.CssClass = "hc_error";
              lbError.Text = dbObj.LastError;
              lbError.Visible = true;
            }
            else
            {
              if (itemObj != null)
              {
                using (Item parentItem = Item.GetByKey(parentId))
                {
                  QDEUtils.wsTranslationObj.ItemUpdateWorkingTables(parentItem.Id, parentItem.Level.SkuLevel);
                }

                SessionState.User.LastVisitedItem = newItemId;
                SessionState.User.QuickSave();
                SessionState.CurrentItem = Item.GetByKey(newItemId);
                if (itemObj.Level != null && itemObj.Level.SkuLevel)
                {
                  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UpdateTV", "<script>alert('The system has created dummy skus. You must change them.');ReloadAndClose();</script>");
                }
                else
                {
                  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UpdateTV", "<script>ReloadAndClose();</script>");
                }
              }
              {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UpdateTV", "<script>ReloadAndClose();</script>");
              }
            }
          }
          else // dbObj is null
          {
            lbError.CssClass = "hc_error";
            lbError.Text = "dbObj is null";
            lbError.Visible = true;
          }
        }
      }
      else // parentId = 0
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "missingParent", "<script>alert('Missing parent');</script>");
      }
    }
    protected void rPossibleParents_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
      Label lbRadioButton = (Label)e.Item.FindControl("lbRabioButton");
      Label lbParent = (Label)e.Item.FindControl("lbParent");

      DataRowView dr = (DataRowView)e.Item.DataItem;
      if (dr != null)
      {
        int levelId = Convert.ToInt32(dr["LevelId"]);
        bool canClone = Convert.ToBoolean(dr["CanClone"]);
        string name = dr["ItemName"].ToString();
        System.Int64 id = Convert.ToInt64(dr["ItemId"]);

        if (lbRadioButton != null && canClone)
        {
          lbRadioButton.Text = "<input type='radio' name='Parent' value='" + id.ToString() + "' />";
          lbRadioButton.Visible = true;
        }

        if (lbParent != null)
        {
          lbParent.Text = "|";
          for (int i = 0; i < levelId; i++)
          {
            lbParent.Text += "--";
          }
          lbParent.Text += name;
        }
      }
    }
  }
}
