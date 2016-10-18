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
#endregion

namespace HyperCatalog.UI.Publish
{

  /// <summary>
  /// Attach new template or delete attached template
  /// </summary>	
  public partial class TemplateAttach : HCPage
  {
    #region Declarations

    //protected HyperCatalog.MasterPages.MasterPage IFAttach;


    public HyperCatalog.Business.Item itemObj;
    private System.Int64 itemId;
    private ItemLevelList listLevel;
    protected System.Web.UI.WebControls.DropDownList DropDownList1;
    protected int levelCount;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      InitializeComponent();
      this.ddlLevels.SelectedIndexChanged += new System.EventHandler(this.ddlLevels_SelectedIndexChanged);
      base.OnInit(e);
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.uwToolBar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolBar_ButtonClicked);
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TEMPLATES) ||
          SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS) ||
          SessionState.User.HasCapability(CapabilitiesEnum.DISPLAY_ITEMS))
      {
        try
        {
          itemObj = QDEUtils.GetItemIdFromRequest();
        }
        catch
        {
          throw new ArgumentException("Item Id was not provided");
        }

        // List of level
        listLevel = new ItemLevelList();
        foreach (ItemLevel i in ItemLevel.GetAll())
        {
          if (i.Id > 0)
          {
            listLevel.Add(i);
          }
          if (i.SkuLevel)
          {
            break;
          }
        }
        levelCount = listLevel.Count;

        if (!Page.IsPostBack)
        {
          //Initalize templates calling webservice once and put value in cache
          GetTemplates();
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

          cbTemplates.Enabled = false;
          uwToolBar.Items.FromKeyButton("add").Enabled = false;
          uwToolBar.Items.FromKeyButton("applyAll").Enabled = false;
        }
        if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TEMPLATES))
        {
          ddlLevels.Enabled = false;
          cbTemplates.Enabled = false;
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
        string sSql = "_Item_Template_GetAttached " + itemObj.Id;
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sSql, ""))
        {
          dbObj.CloseConnection();

          if (dbObj.LastError == string.Empty)
          {
            dg.DataSource = ds;
            Utils.InitGridSort(ref dg, false);
            dg.DataBind();
            dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = "An error of database is occurred.";
            lbError.Visible = true;
          }
        }
      }
    }

    private void UpdateCandidates()
    {
      // Get Inputforms available for the level selected and item selected
      try
      {
        cbTemplates.Rows.Clear();
        TemplateList tList = GetTemplates();
        // Remove Templates already affected for current level
        for (int i = 0; i < tList.Count; i++)
        {
          foreach (UltraGridRow dr in dg.Rows)
          {
            if (dr.Cells.FromKey("TemplateName").Text == tList[i].Name && dr.Cells.FromKey("L" + ddlLevels.SelectedValue.ToString()).Text != string.Empty)
            {
              tList.Remove(i);
              i--;
              break;
            }
          }
        }
        if (tList.Count > 0)
        {
          cbTemplates.DataSource = tList;
          cbTemplates.DataValueField = "Name";
          cbTemplates.DataTextField = "Name";
          cbTemplates.DataBind();
          cbTemplates.Enabled = true;
          cbTemplates.SelectedIndex = 0;
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
        }
      }
      catch (Exception ex)
      {
        // Error web service
        lbError.CssClass = "hc_error";
        lbError.Text = ex.Message;
        lbError.Visible = true;
      }
    }

    private TemplateList GetTemplates()
    {
      if (Cache["Templates"] == null)
      {
        Cache.Add("Templates", Template.GetAll(), null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
      }
      return (TemplateList)Cache["Templates"];
    }

    private void Save()
    {
      bool isSaved = itemObj.AddTemplate(cbTemplates.DisplayValue.ToString(), Convert.ToInt32(ddlLevels.SelectedValue.ToString()), SessionState.User.Id);
      if (!isSaved)
      {
        lbError.CssClass = "hc_errror";
        lbError.Text = Item.LastError;
        lbError.Visible = true;
      }
      else
      {
        // Refresh candidates
        UpdateDataView();
        UpdateCandidates();
        if (SessionState.CurrentItem != null)
          SessionState.CurrentItem.InputForms = null;

      }
    }

    private void SaveAll()
    {
      for (int i = Convert.ToInt32(ddlLevels.SelectedValue); i <= levelCount; i++)
      {
        bool isSaved = itemObj.AddTemplate(cbTemplates.DataValue.ToString(), i, SessionState.User.Id);
        if (!isSaved)
        {
          lbError.CssClass = "hc_errror";
          lbError.Text = Item.LastError;
          lbError.Visible = true;

          break;
        }
      }
      // Refresh candidates
      UpdateDataView();
      if (SessionState.CurrentItem != null)
        SessionState.CurrentItem.InputForms = null;
      UpdateCandidates();
    }


    /// <summary>
    /// Initialize datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      // Image "delete"
      TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Action").Column;
      ImageButton btImg = (ImageButton)((CellItem)col.CellItems[e.Row.Index]).FindControl("imgDel");
      btImg.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this relation ?');");

      // Display by row (for level)
      for (int c = 1; c <= levelCount; c++)
      {
        string colName = "L" + c.ToString();

        // Template herited
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
    /// Delete the relation between IF and the current item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Delete_Click(object sender, ImageClickEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.ImageButton)sender).Parent);
      string templateName = cellItem.Cell.Row.Cells.FromKey("TemplateName").Text;
      if (itemObj.DeleteTemplate(templateName))
      {
        // Refresh candidates
        UpdateCandidates();
        if (SessionState.CurrentItem != null)
          SessionState.CurrentItem.InputForms = null;
        UpdateDataView();
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = Item.LastError;
        lbError.Visible = true;
      }
    }

    /// <summary>
    /// Toolbar click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void uwToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
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
    private void ddlLevels_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      UpdateCandidates();
    }
  }
}
