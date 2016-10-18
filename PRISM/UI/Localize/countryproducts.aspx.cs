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
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebGrid;
#endregion

public partial class UI_Localize_countryproducts : HCPage
{
  #region Declarations
  private string _pageAction;
  protected string pageAction
  {
    get
    {
      if (_pageAction == null && Request["t"] != null)
        _pageAction = Request["t"].ToString();
      return _pageAction;
    }
  }
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request["filter"] != null)
    {
      txtFilter.Text = Request["filter"].ToString();
    }
    lbMessage.Visible = false;    
    #region Check Capabilities
    if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.LOCALIZE_PRODUCTS)))
    {
      uwToolbar.Items.FromKeyButton("Validate").Enabled = uwToolbar.Items.FromKeyButton("Exclude").Enabled = false;
    }
    if (SessionState.User.Cultures.Count == 0)
    {
      lbMessage.Text = "Your profile is not correctly set. Contact an administrator to assign the correct culture to your profile.";
      lbMessage.CssClass = "hc_error";
      lbMessage.Visible = true;
      DDL_Cultures.Visible = false;
      return;
    }
    #endregion

    if (!Page.IsPostBack)
    {
      #region Load Cultures list
      /// Retrieve all Users Cultures, and keep only primary
      CultureList dsCultures = SessionState.User.Cultures;
      CollectionView locales = new CollectionView(dsCultures);
      locales.ApplyFilter("Type", CultureType.Locale, CollectionView.FilterOperand.Equals);
      locales.ApplySort("Name", System.ComponentModel.ListSortDirection.Ascending);
      if (locales.Count > 0)
      {
        DDL_Cultures.DataSource = locales;
        DDL_Cultures.DataBind();
        if (SessionState.Culture.Type != CultureType.Locale)
        {
          SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(DDL_Cultures.Items[0].Value);
        }
        else
        {
          DDL_Cultures.Items.FindByValue(SessionState.Culture.Code).Selected = true;
        }
        DisplayData();
      }
      else
      {
        // User has no primary cultures in its scope
        lbMessage.Text = "Your profile is not correctly set. Contact an administrator to assign the correct culture to your profile.";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
        DDL_Cultures.Visible = false;
        return;
      }
      #endregion
    }
    else
    {
      // action after changes in PLC edit window 
      if (Request["action"] != null && Request["action"].ToString().ToLower() == "reload")
      {
        DisplayData();
      }
    }
    txtFilter.AutoPostBack = false;
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "initCulture", "<script>var curCulture = '" + SessionState.Culture.Code + "';</script>");
  }

  /// <summary>
  /// When Culture change, refresh data
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void DDL_Cultures_SelectedIndexChanged(object sender, EventArgs e)
  {
    SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(DDL_Cultures.SelectedValue);
    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "refreshCulture", "<script>var curCulture = '" + SessionState.Culture.Code + "';</script>");
    DisplayData();
    txtFilter.Text = string.Empty;
  }

  /// <summary>
  /// Display data into the grid
  /// </summary>
  private void DisplayData()
  {
    using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 600))
    {
      DataSet ds=null;
      try
      {
        if (pageAction == "O" || pageAction == "o")
        {
          ds = dbObj.RunSPReturnDataSet("_Item_GetProductsNearlyObso",
            new SqlParameter("@CultureCode", SessionState.Culture.Code),
            new SqlParameter("@UserId", SessionState.User.Id),
            new SqlParameter("@ProductObsoletePrevDelay", Convert.ToInt32(SessionState.CacheParams["ProductObsolete_PrevDelay"].Value)),
            new SqlParameter("@ContentStatusDelay", Convert.ToInt32(SessionState.CacheParams["Country_ContentStatusDelay"].Value)),
            new SqlParameter("@Company", SessionState.CompanyName));
        }
        else
        {
          string itemTypeId = pageAction; // If the action is not "O", we are passing the type
          ds = dbObj.RunSPReturnDataSet("_Item_GetProductsToValidate",
            new SqlParameter("@CultureCode", SessionState.Culture.Code),
            new SqlParameter("@UserId", SessionState.User.Id),
            new SqlParameter("@ItemTypeId", itemTypeId),
           new SqlParameter("@ContentStatusDelay", Convert.ToInt32(SessionState.CacheParams["Country_ContentStatusDelay"].Value)),
            new SqlParameter("@Company", SessionState.CompanyName));
        }
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] " + dbObj.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        else
        {
          #region Results
          if (ds.Tables[0].Rows.Count > 0)
          {
            dg.DataSource = ds;
            lbNoresults.Visible = false;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            dg.Columns.FromKey("PID").Format = SessionState.User.FormatDate;
            dg.Columns.FromKey("POD").Format = SessionState.User.FormatDate;
            dg.Columns.FromKey("Removal").Format = SessionState.User.FormatDate;
            UITools.UpdatePLCGridHeader(dg);

            dg.Visible = true;
            uwToolbar.Items.FromKeyButton("Export").Enabled = true;
          }
          #endregion
          #region No result
          else
          {
            lbNoresults.Text = "No product found";
            lbNoresults.Visible = true;
            uwToolbar.Items.FromKeyButton("Export").Enabled = false;
            dg.Visible = false;
          }
          #endregion
          dg.Columns.FromKey("ClassName").Header.Caption = SessionState.ItemLevels[1].Name;
        }
      }
      finally
      {
        if (ds != null) ds.Dispose();
      }
    }
  }

  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("ItemName");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cNum = e.Row.Cells.FromKey("sku");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cStatus = e.Row.Cells.FromKey("ContentStatus");

    #region Search colorization
    string search = txtFilter.Text.Trim();
    if (search != string.Empty)
    {
      if ((cName.Text.ToLower().IndexOf(search.ToLower())) >= 0 || (cNum.Text.ToLower().IndexOf(search.ToLower()) >= 0) || (cStatus.Text.ToLower().IndexOf(search.ToLower()) >= 0))
      {
        cName.Text = Utils.CReplace(cName.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
        cNum.Text = Utils.CReplace(cNum.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
        cStatus.Text = Utils.CReplace(cStatus.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
      }
      else
      {
        e.Row.Delete();
        return;
      }
    }
    #endregion

    cNum.Text = "<a href='javascript://' onclick='r(" + e.Row.Cells.FromKey("ItemId").Text + ");'>" + cNum.Text + "</a>";
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cd = e.Row.Cells.FromKey("POD");
    //-- Pod formatting
    cd = e.Row.Cells.FromKey("POD");
    cd.Text = SessionState.User.FormatUtcDateForGrid(cd.Value);
    //-- Removal formatting
    cd = e.Row.Cells.FromKey("Removal");
    cd.Text = cd.Value != null ? SessionState.User.FormatUtcDateForGrid(cd.Value) : cd.Text;
    //-- Pid formatting
    cd = e.Row.Cells.FromKey("PID");
    cd.Text = SessionState.User.FormatUtcDateForGrid(cd.Value);
    cd.Text = "<a href='javascript://' onclick='EditPLC(" + e.Row.Cells.FromKey("ItemId").Text +
     "," + e.Row.Cells.FromKey("PID").Column.Index.ToString() +
     "," + e.Row.Index.ToString() +
     ",&quot;" + dg.ClientID + "&quot;" +
     ");'>" + cd.Text + "</a>";
  }
  /// <summary>
  /// Action for toolbar buttons
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="be"></param>
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
      string btn = be.Button.Key.ToLower();
      #region Validate products
      if (btn == "validate")
      {
        ApplyChanges("C");
      }
      #endregion
      #region Exclude products
      if (btn == "exclude")
      {
        ApplyChanges("E");
      }
      #endregion
      #region Search
      if ( btn == "search")
      {
        DisplayData();
      }
      #endregion
      #region Export
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "ProductsToValidate", "ProductsToValidate");
      }
      #endregion
  }

  private void ApplyChanges(string s)
  {
    for (int i = 0; i < dg.Rows.Count; i++)
    {
      TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
      CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
      if (cb.Checked)
      {
        Item item = Item.GetByKey(Convert.ToInt64(dg.Rows[i].Cells.FromKey("ItemId").Value));
        if (!item.UpdateWorkflowStatus(s, DDL_Cultures.SelectedValue, SessionState.User.Id))
        {
          lbMessage.Text = "Error: impossible to update workflowStatus for the bundle " + item.Name;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
          break;
        }
      }
    }
    DisplayData();
    lbMessage.Text = "Changes applied";
    lbMessage.CssClass = "hc_success";
    lbMessage.Visible = true;
  }

}
