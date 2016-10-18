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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Acquire
{
  /// <summary>
  /// Attach new input form or delete attached input form
  /// </summary>
  public partial class InputFormsAttach : HCPage
  {
    #region Declarations
    public HyperCatalog.Business.Item itemObj;
    private System.Int64 itemId;
    private ItemLevelList listLevel;
    protected int levelCount;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (SessionState.User.HasCapability(CapabilitiesEnum.ATTACH_INPUT_FORMS) ||
          SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY) ||
          SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS) ||
          SessionState.User.HasCapability(CapabilitiesEnum.DISPLAY_ITEMS))
      {
        try
        {
          itemObj = QDEUtils.GetItemIdFromRequest();
          itemId = itemObj.Id;          
        }
        catch
        {
          throw new ArgumentException("Item Id was not provided");
        }

        // List of level
        listLevel = new ItemLevelList();
        using (ItemLevelList itemLevelAll = ItemLevel.GetAll())
        {
          foreach (ItemLevel i in itemLevelAll)
          {
            if (i.Id > 0)
            {
              listLevel.Add(i);
            }
          }
        }
        levelCount = listLevel.Count;

        if (!Page.IsPostBack)
        {
          // Get applicable level list 
          ddlLevels.Items.Insert(0, new ListItem("-->Choose a level<--", "0"));
          int l = 0;
          for (int i = 1; i <= listLevel.Count - 1; i++)
          {
            if (listLevel[i].Id >= itemObj.LevelId)
            {
              l++;
              ddlLevels.Items.Insert(l, new ListItem("[" + listLevel[i].Id.ToString() + "] " + listLevel[i].Name.ToString(), listLevel[i].Id.ToString()));
            }
          }
          ddlLevels.Attributes.Add("onChange", "if (this.value=='0') return false;");

          // Add Level Columns to grid
          int totalWidth = 0;
          foreach (HyperCatalog.Business.ItemLevel lev in listLevel)
          {
            Infragistics.WebUI.UltraWebGrid.UltraGridColumn levCol = new UltraGridColumn("L" + lev.Id.ToString(), lev.Id.ToString(), ColumnType.NotSet, null);
            levCol.Width = Unit.Pixel(25);
            levCol.BaseColumnName = levCol.Key;
            dg.Bands[0].Columns.Add(levCol);
            totalWidth += 25;
          }
          int width = totalWidth;
          AppLevelTitle.Width = width.ToString();
          AppLevelTitle.DataBind();

          // Move column "delete" in last position
          dg.Bands[0].Columns.FromKey("Action").Move(dg.Bands[0].Columns.Count - 1);

          // Update Grid
          UpdateDataView();

          cbInputForms.Enabled = false;
          uwToolBar.Items.FromKeyButton("add").Enabled = false;
          uwToolBar.Items.FromKeyButton("applyAll").Enabled = false;
        }

        if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY) && !SessionState.User.HasCapability(CapabilitiesEnum.ATTACH_INPUT_FORMS))
        {
          ddlLevels.Enabled = false;
          cbInputForms.Enabled = false;
          uwToolBar.Items.FromKeyButton("add").Enabled = false;
          uwToolBar.Items.FromKeyButton("applyAll").Enabled = false;
        }
      }
      else
      {
        UITools.DenyAccess(DenyMode.Popup);
      }
    }

    /// <summary>
    /// Display data
    /// </summary>
    private void UpdateDataView()
    {
      // Get ItemName and ItemLevel of current item
      lItemName.Text = itemObj.Name;
      lItemLevel.Text = "[" + itemObj.LevelId.ToString() + "] " + itemObj.Level.Name + "&nbsp;";

      // Get input forms attached to the current item
      using (Database dbObj = Utils.GetMainDB())
      {
        DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_InputForm_GetAttached",
          new SqlParameter("@ItemId", itemObj.Id));
        dbObj.CloseConnection();

        if (dbObj.LastError == string.Empty)
        {
          dg.DataSource = ds;
          Utils.InitGridSort(ref dg, false);
          dg.DataBind();

          dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
          dg.Columns.Remove(dg.Columns.FromKey("__Spacer"));

          lnkExportCartography.Enabled = (dg.Rows.Count > 0);
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = dbObj.LastError;
          lbError.Visible = true;
        }
      }
    }
    private void UpdateCandidates()
    {
      lbError.Visible = false;

      if (ddlLevels.SelectedIndex > 0)
      {
        // Get Inputforms available for the level selected and item selected
        using (Database dbObj = Utils.GetMainDB())
        {

          // Sql query
          DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_InputForm_GetCandidatesByLevel", "InputForms",
            new SqlParameter("@ItemId", itemObj.Id),
            new SqlParameter("@LevelId", Convert.ToInt32(ddlLevels.SelectedValue)));
          dbObj.CloseConnection();

          if (dbObj.LastError.Length == 0)
          {
            if (ds != null)
            {
              cbInputForms.DataSource = ds;
              cbInputForms.DataTextField = "Name";
              cbInputForms.DataValueField = "InputFormId";
              cbInputForms.DataBind();

              if (cbInputForms.Rows.Count > 0)
              {
                cbInputForms.Enabled = true;
                cbInputForms.SelectedIndex = 0;

                // Show Add button
                uwToolBar.Items.FromKeyButton("add").Enabled = true;

                // Show or hide ApplyAll button
                if (!ddlLevels.SelectedValue.Equals(levelCount.ToString()))
                  uwToolBar.Items.FromKeyButton("applyAll").Enabled = true;
                else
                  uwToolBar.Items.FromKeyButton("applyAll").Enabled = false;
              }
              else
              {
                uwToolBar.Items.FromKeyButton("add").Enabled = false;
                uwToolBar.Items.FromKeyButton("applyAll").Enabled = false;

                lbError.Text = "No input form can be attached";
                lbError.CssClass = "hc_error";
                lbError.Visible = true;
              }
            }
          }
          else
          {
            // Error database
            lbError.CssClass = "hc_error";
            lbError.Text = dbObj.LastError;
            lbError.Visible = true;
          }
        }
      }
      // Refresh grid
      UpdateDataView();
    }

    private void Save()
    {
      bool isSaved = itemObj.AddInputForm(Convert.ToInt32(cbInputForms.DataValue), Convert.ToInt32(ddlLevels.SelectedValue.ToString()), SessionState.User.Id);
      if (!isSaved)
      {
        lbError.CssClass = "hc_errror";
        lbError.Text = Item.LastError;
        lbError.Visible = true;
      }
      else
      {
        // Refresh candidates
        UpdateCandidates();

        // Refresh frame content
        if (SessionState.CurrentItem != null)
          SessionState.CurrentItem.InputForms = null;
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadQDEContent", "<script>ReloadQDEContent(" + itemId.ToString() + ", '" + SessionState.Culture.Code + "');</script>");
      }
    }
    private void SaveAll()
    {
      int skuLevel = ItemLevel.GetSkuLevel().Id;
      for (int i = Convert.ToInt32(ddlLevels.SelectedValue); i <= levelCount; i++)
      {
        if (i <= skuLevel)
        {
          bool isSaved = itemObj.AddInputForm(Convert.ToInt32(cbInputForms.DataValue), i, SessionState.User.Id);
          if (!isSaved)
          {
            lbError.CssClass = "hc_errror";
            lbError.Text = Item.LastError;
            lbError.Visible = true;
            break;
          }
        }
      }
      // Refresh candidates
      UpdateCandidates();
      // Refresh fram content
      if (SessionState.CurrentItem != null)
        SessionState.CurrentItem.InputForms = null;
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadQDEContent", "<script>ReloadQDEContent(" + itemId.ToString() + ", '" + SessionState.Culture.Code + "');</script>");
    }


    /// <summary>
    /// Initialize datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      // Name of input form (name + long name)
      e.Row.Cells.FromKey("IFName").Text = e.Row.Cells.FromKey("IFName").Text + ": " + e.Row.Cells.FromKey("IFLongName").Text + " (#" + e.Row.Cells.FromKey("InputFormId").Text + ")";

      // Image "delete"
      TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Action").Column;
      ImageButton btImg = (ImageButton)((CellItem)col.CellItems[e.Row.Index]).FindControl("imgDel");
      btImg.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this relationship ?');");

      // Display by row (for level)
      for (int c = 1; c <= levelCount; c++)
      {
        string colName = "L" + c.ToString();

        // InputForm herited
        if (e.Row.Cells.FromKey("Herited").Text == "True")
        {
          e.Row.Style.ForeColor = Color.Gray;
          ((CellItem)col.CellItems[e.Row.Index]).Controls.Remove(btImg);
        }

        // Align X
        if (e.Row.Cells.FromKey(colName).Text != null)
        {
          e.Row.Cells.FromKey(colName).Style.HorizontalAlign = HorizontalAlign.Center;
        }
      }
    }
    /// <summary>
    /// Initialize InputForms checkbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cbInputForms_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      e.Row.Cells.FromKey("Name").Text = e.Row.Cells.FromKey("ShortName").Text + ": " + e.Row.Cells.FromKey("Name").Text;
    }
    /// <summary>
    /// Delete the relation between IF and the current item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Delete_Click(object sender, ImageClickEventArgs e)
    {
      if (sender != null)
      {
        Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.ImageButton)sender).Parent);
        int inputFormId = Convert.ToInt32(cellItem.Cell.Row.Cells.FromKey("InputFormId").Text);

        //Modified for the QC# 938
        if (itemObj.DeleteInputFormUser(inputFormId, SessionState.User.Id))
        //Modified for the QC# 938
        {
          // Refresh candidates
          UpdateCandidates();

          // Refresh frame content
          if (SessionState.CurrentItem != null)
            SessionState.CurrentItem.InputForms = null;
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadQDEContent", "<script>ReloadQDEContent(" + itemId.ToString() + ", '" + SessionState.Culture.Code + "');</script>");
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = Item.LastError;
          lbError.Visible = true;
        }
      }
    }
    /// <summary>
    /// Toolbar click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    protected void uwToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "add")
      {
        Save();
      }
      else if (btn == "applyall")
      {
        SaveAll();
      }
    }
    /// <summary>
    /// Change level
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLevels_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      UpdateCandidates();
    }

    protected void lnkExportCartography_Click(object sender, EventArgs e)
    {
      Tools.Export.ExportCartography(itemId, this.Page);
      UpdateDataView();
    }
  }
}
