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
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;

public partial class Links_content : HCPage
{
  #region Declarations
  private HyperCatalog.Business.Item item = null;
  private int LCount; // count of links 
  private int RLCount; // count of recommended links 
  private string currentGroup = string.Empty;
  private string currentLinkGroup = string.Empty;
  private int groupCount = 0;
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
    {
      try
      {
        // get parameters
        if (Request["i"] != null)
        {
          System.Int64 itemId = Convert.ToInt64(Request["i"]);
          item = HyperCatalog.Business.Item.GetByKey(itemId);
        }

        if (!Page.IsPostBack)
          UpdateDataView();
      }
      catch (FormatException fe)
      {
        lbError.Text = fe.ToString();
        lbError.CssClass = "hc_error";
        lbError.Visible = true;
        return;
      }
    }
    else
    {
      UITools.DenyAccess(DenyMode.Frame);
    }
  }
  private void UpdateDataView()
  {
    #region "Hide all components"
    // hide grids
    dg.Visible = false;

    // hide labels
    lbError.Visible = false;
    lbResult.Visible = false;
    #endregion

    if (item != null)
    {
      #region "Display links"
      DataSet ds = Link.GetContent(item.Id, SessionState.Culture.Code, -1, true);
      if (ds != null)
      {
        if (ds.Tables[0].Rows.Count > 0)
        {
          dg.DataSource = ds;
          dg.DataBind();
          dg.DisplayLayout.AllowSortingDefault = AllowSorting.No; // no sort
          InitializeLinksGridGrouping();

          if (SessionState.Culture.Type == CultureType.Regionale)
          {
            dg.Columns.FromKey("IsRecommended").Hidden = false;
          }
          else
            dg.Columns.FromKey("IsRecommended").Hidden = true;
          dg.Visible = true;
        }
        else
        {
          // No links
          lbResult.CssClass = "hc_success";
          lbResult.Text = "No result";
          lbResult.Visible = true;
        }
      }
      else
      {
        // Error
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Item.LastError;
        lbError.Visible = true;
      }
      #endregion

      // Update count
      updateCount(ds);
      // Update toolbar in footer
      updateToolBarStat();

      if (ds != null)
        ds.Dispose();

      if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT) != null
        && SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT).Value)
      {
        UITools.RefreshTab(this.Page, "tb_Content", Link.GetLinksCount(item.Id, SessionState.Culture.Code));
      }
    }
  }

  #region Event methods
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    if (e.Row.Cells.FromKey("IsExcluded") != null && Convert.ToBoolean(e.Row.Cells.FromKey("IsExcluded").Value))
    {
      e.Row.Delete();
    }
    else
    {
      // Retrieve country code
      if (e.Row.Cells.FromKey("CountryCode") != null && e.Row.Cells.FromKey("ImageCountry") != null)
      {
        string countryCode = string.Empty;
        if (e.Row.Cells.FromKey("CountryCode") != null && e.Row.Cells.FromKey("CountryCode").Value != null)
          countryCode = e.Row.Cells.FromKey("CountryCode").ToString();

        // Update image for current country
        if (countryCode.Length > 0 && e.Row.Cells.FromKey("ImageCountry") != null)
          e.Row.Cells.FromKey("ImageCountry").Text = "<img title=\"" + countryCode + "\" src=\"/hc_v4/img/flags/" + countryCode.ToLower() + ".gif\">";
      }

      if (e.Row.Cells.FromKey("LinkFrom") != null)
      {
        bool linkFrom = Convert.ToBoolean(e.Row.Cells.FromKey("LinkFrom").Value);
        if (!linkFrom) // Hardware list
        {
          if (e.Row.Cells.FromKey("Name") != null && e.Row.Cells.FromKey("ItemName") != null && e.Row.Cells.FromKey("ItemName").Value != null)
            e.Row.Cells.FromKey("Name").Text = e.Row.Cells.FromKey("ItemName").Value.ToString();
          if (e.Row.Cells.FromKey("SKU") != null && e.Row.Cells.FromKey("ItemSKU") != null && e.Row.Cells.FromKey("ItemSKU").Value != null)
            e.Row.Cells.FromKey("SKU").Text = e.Row.Cells.FromKey("ItemSKU").Value.ToString();
          if (e.Row.Cells.FromKey("Class") != null && e.Row.Cells.FromKey("ClassName") != null && e.Row.Cells.FromKey("ClassName").Value != null)
            e.Row.Cells.FromKey("Class").Text = e.Row.Cells.FromKey("ClassName").Value.ToString();
        }
        else // Companion list
        {
          if (e.Row.Cells.FromKey("Name") != null && e.Row.Cells.FromKey("SubItemName") != null && e.Row.Cells.FromKey("SubItemName").Value != null)
            e.Row.Cells.FromKey("Name").Text = e.Row.Cells.FromKey("SubItemName").Value.ToString();
          if (e.Row.Cells.FromKey("SKU") != null && e.Row.Cells.FromKey("SubItemSKU") != null && e.Row.Cells.FromKey("SubItemSKU").Value != null)
            e.Row.Cells.FromKey("SKU").Text = e.Row.Cells.FromKey("SubItemSKU").Value.ToString();
          if (e.Row.Cells.FromKey("Class") != null && e.Row.Cells.FromKey("SubClassName") != null && e.Row.Cells.FromKey("SubClassName").Value != null)
            e.Row.Cells.FromKey("Class").Text = e.Row.Cells.FromKey("SubClassName").Value.ToString();
        }
      }
    }
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "export")
    {
      Utils.ExportToExcel(dg, "AllLinks" + item.Id, "AllLinks" + item.Id);
    }
  }
  #endregion

  #region Private methods
  private void updateCount(DataSet ds)
  {
    // init vars
    LCount = RLCount = 0;
    if (ds != null)
    {
      // loops on link list
      foreach (DataRow dr in ds.Tables[0].Rows)
      {
        LCount++;
        bool isRecommended = Convert.ToBoolean(dr["Recommended"]);
        if (isRecommended)
        {
          RLCount++;
        }
      }
    }
  }
  private void updateToolBarStat()
  {
    lbLCount.Text = "Link count: " + LCount.ToString();
    lbRCount.Text = "DS Recommended link count: " + RLCount.ToString();

    Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateToolbarStat", "<script>hideDiv('divToolbar', '" + (!SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT).Value).ToString() + "');</script>");
  }
  private void InitializeLinksGridGrouping()
  {
    int i = 0;
    int groupLinkTypeCount = 0;
    int groupLinkFromCount = 0;
    int groupFamilyCount = 0;
    bool currentLinkFrom = false;
    string currentFamily = string.Empty;
    LinkType currentLinkType = null;
    bool newLinkType = true;
    bool newLinkFrom = true;
    while (i < dg.Rows.Count)
    {
      #region Group by LinkType
      int linkTypeId = Convert.ToInt32(dg.Rows[i].Cells.FromKey("LinkTypeId").Value);
      if (i == 0 || (currentLinkType != null && currentLinkType.Id!= linkTypeId))
      {
        currentLinkType = LinkType.GetByKey(linkTypeId);
        newLinkType = true;
        dg.Rows.Insert(i, new UltraGridRow());
        UltraGridRow groupRow = dg.Rows[i];
        UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
        foreach (UltraGridCell cell in groupRow.Cells)
        {
          cell.Style.CssClass = string.Empty;
        }
        dg.Rows[i].Style.CssClass = "ptbgroup";
        UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
        groupCell.Text = HyperCatalog.Business.LinkType.GetByKey(linkTypeId).Name;
        groupCell.ColSpan = 4;
        i++;
      }
      #endregion

      if (currentLinkType != null && currentLinkType.IsBidirectional && dg.Rows[i].Cells.FromKey("LinkFrom") != null)
      {
        #region Group by LinkFrom
        bool linkFrom = Convert.ToBoolean(dg.Rows[i].Cells.FromKey("LinkFrom").Value);
        if (newLinkType || currentLinkFrom != linkFrom)
        {
          currentLinkFrom = linkFrom;
          newLinkFrom = true;
          newLinkType = false;
          dg.Rows.Insert(i, new UltraGridRow());
          UltraGridRow groupRow = dg.Rows[i];
          UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row          
          foreach (UltraGridCell cell in groupRow.Cells)
          {
            cell.Style.CssClass = string.Empty;
          }
          dg.Rows[i].Style.CssClass = "ptb4";
          UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
          if (linkFrom)
            groupCell.Text = "Companion list";
          else
              //Code modified for Links Requirement (PR668013) - to change 'Hardware' to 'Host' by Prachi on 10th Dec 2012
              //groupCell.Text = "Hardware list";
              groupCell.Text = "Host list";

          groupCell.ColSpan = 4;
          i++;
        }
        #endregion

        #region Group by Family
        if (dg.Rows[i].Cells.FromKey("Family") != null && dg.Rows[i].Cells.FromKey("Family").Value != null)
        {
          string family = string.Empty;
          if (linkFrom) // Companion list
            family = dg.Rows[i].Cells.FromKey("SubFamily").Value.ToString();
          else // Hardware list
            family = dg.Rows[i].Cells.FromKey("Family").Value.ToString();
          if (newLinkType || newLinkFrom || currentFamily != family)
          {
            currentFamily = family;
            newLinkType = false;
            newLinkFrom = false;
            dg.Rows.Insert(i, new UltraGridRow());
            UltraGridRow groupRow = dg.Rows[i];
            UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
            foreach (UltraGridCell cell in groupRow.Cells)
            {
              cell.Style.CssClass = string.Empty;
            }
            dg.Rows[i].Style.CssClass = "ptb5";
            UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
            groupCell.Text = family;
            groupCell.ColSpan = 4;
            i++;
          }
        }
        #endregion
      }

      #region bidirectional link type
      if (dg.Rows[i].Cells.FromKey("Bidirectional") != null && dg.Rows[i].Cells.FromKey("Bidirectional").Value != null)
      {
        bool isBidirectional = Convert.ToBoolean(dg.Rows[i].Cells.FromKey("Bidirectional").Value);
        if (!isBidirectional && dg.Rows[i].Cells.FromKey("Name") != null) // Cross sell, Bundle, ...
        {
          dg.Rows[i].Cells.FromKey("Name").ColSpan = 2;
        }
      }
      #endregion

      i++;
    }
  }
  #endregion
}
