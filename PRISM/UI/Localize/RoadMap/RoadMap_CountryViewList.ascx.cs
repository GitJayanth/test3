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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;

public partial class RoadMap_CountryViewList : UserControl
{
  #region Declarations
  private string _StatusList = string.Empty;
  private string _WorkflowStatus = string.Empty;
  private string _Countries = string.Empty;
  private string _Classes = string.Empty;
  private string _FilterLiveDate = string.Empty;
  private DateTime? _LiveDate1 = null;
  private DateTime? _LiveDate2 = null;
  private string _FilterObsoleteDate = string.Empty;
  private DateTime? _ObsoleteDate1 = null;
  private DateTime? _ObsoleteDate2 = null;
  private bool _Scope = true;
  #endregion

  #region Public accessors
  public string StatusList
  {
    get { return _StatusList; }
    set { _StatusList = value; }
  }
  public string WorkflowStatus
  {
    get { return _WorkflowStatus; }
    set { _WorkflowStatus = value; }
  }
  public string Countries
  {
    get { return _Countries; }
    set { _Countries = value; }
  }
  public string Classes
  {
    get { return _Classes; }
    set { _Classes = value; }
  }
  public string FilterLiveDate
  {
    get { return _FilterLiveDate; }
    set { _FilterLiveDate = value; }
  }
  public string FilterObsoleteDate
  {
    get { return _FilterObsoleteDate; }
    set { _FilterObsoleteDate = value; }
  }
  public DateTime? LiveDate1
  {
    get { return _LiveDate1; }
    set { _LiveDate1 = value; }
  }
  public DateTime? LiveDate2
  {
    get { return _LiveDate2; }
    set { _LiveDate2 = value; }
  }
  public DateTime? ObsoleteDate1
  {
    get { return _ObsoleteDate1; }
    set { _ObsoleteDate1 = value; }
  }
  public DateTime? ObsoleteDate2
  {
    get { return _ObsoleteDate2; }
    set { _ObsoleteDate2 = value; }
  }
  public bool Scope
  {
    get { return _Scope; }
    set { _Scope = value; }
  }
  #endregion

  #region Constantes
  private const int TIMEOUT = 300; // 5 min
  #endregion

  protected void Page_Load(object sender, EventArgs e) 
  {
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Promote").Enabled = false;
    }

    if (!_Scope)
    {
      UITools.HideToolBarSeparator(uwToolbar, "PromoteSep");
      UITools.HideToolBarButton(uwToolbar, "Promote");
    }
    else
    {
      UITools.ShowToolBarSeparator(uwToolbar, "PromoteSep");
      UITools.ShowToolBarButton(uwToolbar, "Promote");
    }

    if (ViewState["CountryStyle"] != null)
    {
      Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "UpdateStylePageLoad", "<style>" + ViewState["CountryStyle"].ToString() + "</style>");
    }
  }
  public void Refresh()
  {
    if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.VIEW_ROADMAP))
    {
      try
      {
        if (ViewState["Filter"] != null)
        {
          txtFilter.Text = ViewState["Filter"].ToString();
        }

        ViewState["CountryStyle"] = null;
        dg.DisplayLayout.Pager.Reset();
        UpdateStyleCountry();
        UpdateDataView();
      }
      catch (Exception ex)
      {
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "trace", "<script>alert('"+ex.Message+"');</script>");
      }
    }
  }
  private void UpdateStyleCountry()
  {    
    // Update style for countries
    if (_Countries.Length > 0)
    {
      string styles = string.Empty;
      string[] codes = _Countries.Split(new char[] { ',' });
      if (codes != null)
      {
        foreach (string c in codes)
        {
          styles += ".ctry" + c + "{ background-image: url(/hc_v4/img/flags/" + c.ToUpper() + ".gif); background-repeat: no-repeat; margin-left: 0px; background-position: top left; padding-left: 22px;}\n";
        }
        ViewState["CountryStyle"] = styles;
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "UpdateStyleRefresh", "<style>" + styles + "</style>");
      }
    }
  }
  public void UpdateDataView()
  {
    lbError.Visible = false;
    lbResult.Visible = false;

    // filter
    string filter = txtFilter.Text;
    if (filter.Length > 0)
    {
      string cleanFilter = filter.Replace("'", "''").ToLower();
      cleanFilter = cleanFilter.Replace("[", "[[]");
      cleanFilter = cleanFilter.Replace("_", "[_]");
      cleanFilter = cleanFilter.Replace("%", "[%]");

      filter = " LOWER(CountryName) LIKE '%" + cleanFilter + "%' ";
      filter += " OR LOWER(ItemName) LIKE '%" + cleanFilter + "%' ";
      filter += " OR LOWER(ItemSku) LIKE '%" + cleanFilter + "%' ";
    }

    // Retrieve RoadMap with filter
    using (Database dbObj = Utils.GetMainDB())
    {
      dbObj.TimeOut = TIMEOUT;
      if (dbObj != null)
      {
        try
        {
          Trace.Warn("Before run _RoadMap_Countries");
          using (DataSet ds = dbObj.RunSPReturnDataSet("_RoadMap_Countries",
            new SqlParameter("@ItemId", -1),
            new SqlParameter("@StatusList", _StatusList),
            new SqlParameter("@WorkflowStatusList", _WorkflowStatus),
            new SqlParameter("@CountryList", _Countries),
            new SqlParameter("@ClasseList", _Classes),
            new SqlParameter("@FilterLiveDate", _FilterLiveDate),
            new SqlParameter("@FilterObsolDate", _FilterLiveDate),
            new SqlParameter("@LiveDate1", (_LiveDate1.HasValue ? _LiveDate1.Value : DateTime.Now)),
            new SqlParameter("@LiveDate2", (_LiveDate2.HasValue ? _LiveDate2.Value : DateTime.Now)),
            new SqlParameter("@ObsoleteDate1", (_ObsoleteDate1.HasValue ? _ObsoleteDate1.Value : DateTime.Now)),
            new SqlParameter("@ObsoleteDate2", (_ObsoleteDate2.HasValue ? _ObsoleteDate2.Value : DateTime.Now)),
            new SqlParameter("@ProductScope", _Scope),
            new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id),
            new SqlParameter("@Filter", filter)))
          {
            dbObj.CloseConnection();

            if (dbObj.LastError != null && dbObj.LastError.Length > 0)
            {
              Response.Write((_LiveDate1.HasValue) ? _LiveDate1.Value.ToString(): string.Empty);
              Response.Write((_LiveDate2.HasValue) ? _LiveDate2.Value.ToString() : string.Empty);
              lbError.CssClass = "hc_error";
              lbError.Text = dbObj.LastError;
              lbError.Visible = true;
            }
            else if (ds != null)
            {
              if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
              {
                // create repeater
                dg.DataSource = ds;
                Utils.InitGridSort(ref dg, true);
                Trace.Warn("Before run Databind");
                dg.DataBind();
                Trace.Warn("After run Databind");

                UITools.UpdatePLCGridHeader(dg);
                dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
                dg.Visible = true;

                lbResult.CssClass = "hc_success";
                lbResult.Text = "SKU count: " + ds.Tables[0].Rows.Count;
                lbResult.Visible = true;
              }
              else
              {
                lbResult.CssClass = "hc_success";
                lbResult.Text = "No SKU";
                lbResult.Visible = true;

                dg.Visible = false;
              }
            }
          }
        }
        catch (Exception e)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = e.ToString();
          lbError.Visible = true;

          lbResult.Visible = false;
          dg.Visible = false;
        }
      }
    }
  }

  private void Promote()
  {
    if (dg != null && dg.Rows.Count > 0)
    {
      string itemId = string.Empty;
      string countryCode = string.Empty;
      string plcList = string.Empty;
      foreach (UltraGridRow r in dg.Rows)
      {
        itemId = string.Empty;
        countryCode = string.Empty;

        if (r.Cells.FromKey("ItemId") != null)
          itemId = r.Cells.FromKey("ItemId").ToString();
        if (r.Cells.FromKey("CountryCode") != null)
          countryCode = "'"+r.Cells.FromKey("CountryCode").ToString()+"'";

        if (r.Cells.FromKey("PromotePID") != null && r.Cells.FromKey("PromotePID").IsEditable() && Convert.ToBoolean(r.Cells.FromKey("PromotePID").Value))
        {
          if (plcList.Length > 0) 
            plcList += "|";

          plcList += itemId + "," + countryCode + ", 0";
        }
        if (r.Cells.FromKey("PromotePOD") != null && r.Cells.FromKey("PromotePOD").IsEditable() && Convert.ToBoolean(r.Cells.FromKey("PromotePOD").Value))
        {
          if (plcList.Length > 0) 
            plcList += "|";

          plcList += itemId + "," + countryCode + ", 1";
        }
      }
      Trace.Warn("plcList: "+plcList);

      if (plcList.Length > 0)
      {
        using (Database dbObj = Utils.GetMainDB())
        {
          if (dbObj != null)
          {
            try
            {
              int r = dbObj.RunSPReturnInteger("_RoadMap_PromoteToCountry",
                new SqlParameter("@PLCList", plcList),
                new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id));
              dbObj.CloseConnection();

              if (dbObj.LastError != null && dbObj.LastError.Length > 0)
              {
                lbError.CssClass = "hc_error";
                lbError.Text = plcList + "\n" + dbObj.LastError;
                lbError.Visible = true;
              }
              else
              {
                if (r > 0)
                {
                  lbError.CssClass = "hc_success";
                  lbError.Text = "Data seved!";
                  lbError.Visible = true;

                  //hiCacheRefresh.Value = DateTime.UtcNow.ToLongTimeString().ToString() + DateTime.UtcNow.Millisecond.ToString();
                  //Trace.Warn("hiCacheRefresh = " + DateTime.UtcNow.ToLongTimeString().ToString() + DateTime.UtcNow.Millisecond.ToString());
                  UpdateDataView();
                }
                else
                {
                  lbError.CssClass = "hc_error";
                  lbError.Text = "Error: date types are not updated";
                  lbError.Visible = true;
                }
              }
            }
            catch (Exception e)
            {
              lbError.CssClass = "hc_error";
              lbError.Text = e.ToString();
              lbError.Visible = true;
            }
          }
        }
      }
    }
  }
  private string GetStatusLinkColor(string className)
  {
    if (className.EndsWith("ro") || className.EndsWith("ro"))
    {
      return "white";
    }
    return "darkblue";
  }

  #region Delegate
  public delegate void BackButtonClickHandler(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e);
  public event BackButtonClickHandler BackButtonClick;
  public delegate void ViewGraphicButtonClickHandler(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e);
  public event ViewGraphicButtonClickHandler ViewGraphicButtonClick;
  public delegate void ViewListButtonClickHandler(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e);
  public event ViewListButtonClickHandler ViewListButtonClick;
  #endregion

  #region Event methods
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be) 
  {
    string btn = be.Button.Key.ToLower();
    switch (btn)
    {
      case "back":
        if (BackButtonClick != null)
          BackButtonClick(this, be);
        break;
      case "promote":
        Promote();
        if (ViewListButtonClick != null)
          ViewListButtonClick(this, be);
        break;
      case "graphicview":
        if (ViewGraphicButtonClick != null)
          ViewGraphicButtonClick(this, be);
        break;
      case "filter":
        ViewState["Filter"] = txtFilter.Text;
        if (ViewListButtonClick != null)
          ViewListButtonClick(this, be);
        break;
    }
  }
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    #region "Update country flag"
    if (e.Row.Cells.FromKey("CountryCode") != null
      && e.Row.Cells.FromKey("CountryName") != null
      && e.Row.Cells.FromKey("Country") != null
      && e.Row.Cells.FromKey("CountryCode").Value != null
      && e.Row.Cells.FromKey("CountryName").Value != null)
    {
      string countryCode = e.Row.Cells.FromKey("CountryCode").Value.ToString();
      string countryName = e.Row.Cells.FromKey("CountryName").Value.ToString();
      e.Row.Cells.FromKey("Country").Style.CssClass = "ctry" + countryCode;
      e.Row.Cells.FromKey("Country").Text = countryName;
    }
    #endregion
    #region "Update status"
    string cssClass = string.Empty;
    if (e.Row.Cells.FromKey("Status") != null && e.Row.Cells.FromKey("Status").Value != null
      && e.Row.Cells.FromKey("WorkflowStatus") != null && e.Row.Cells.FromKey("WorkflowStatus").Value != null)
    {
      string status = e.Row.Cells.FromKey("Status").Value.ToString();
      string worflowStatus = e.Row.Cells.FromKey("WorkflowStatus").Value.ToString();
      if (status.Equals('L') || status.Equals('O') || status.Equals('E'))
        e.Row.Cells.FromKey("Status").Text = Item.GetStatusFromString(status).ToString();
      else
        e.Row.Cells.FromKey("Status").Text = Item.GetWorkflowStatusFromString(worflowStatus).ToString();

      switch (status)
      {
        case "L": cssClass = "rl"; break;
        case "O": cssClass = "ro"; break;
        case "F":
          if (e.Row.Cells.FromKey("WorkflowStatus") != null && e.Row.Cells.FromKey("WorkflowStatus").Value != null)
          {
            string ws = e.Row.Cells.FromKey("WorkflowStatus").Value.ToString();
            if (ws == "R")
              cssClass = "rrv"; 
            else if (ws == "C")
              cssClass = "rcv"; 
          }
          break;
        case "E": cssClass = "re"; break;
        default: cssClass = string.Empty; break;
      }
      if (e.Row.Cells.FromKey("PID") != null
        && e.Row.Cells.FromKey("POD") != null)
      {
        e.Row.Cells.FromKey("PID").Style.CssClass = cssClass;
        e.Row.Cells.FromKey("POD").Style.CssClass = cssClass;

        DateTime? blindDate = null;
        DateTime? fullDate = null;
        DateTime? obsoleteDate = null;
        if (e.Row.Cells.FromKey("BlindDate").Value != null)
          blindDate = Convert.ToDateTime(e.Row.Cells.FromKey("BlindDate").Value);
        if (e.Row.Cells.FromKey("PID").Value != null)
          fullDate = Convert.ToDateTime(e.Row.Cells.FromKey("PID").Value);
        if (e.Row.Cells.FromKey("POD").Value != null)
          obsoleteDate = Convert.ToDateTime(e.Row.Cells.FromKey("POD").Value);

        if (blindDate.HasValue && fullDate.HasValue && blindDate.Value > fullDate.Value) cssClass = "ri";
        if (obsoleteDate.HasValue && fullDate.HasValue && fullDate.Value > obsoleteDate.Value) cssClass = "ri";
        if (blindDate.HasValue && obsoleteDate.HasValue && blindDate.Value > obsoleteDate.Value) cssClass = "ri";
      }
    }
    #endregion
    #region "Update link for date"
    if (e.Row.Cells.FromKey("PID") != null
      && e.Row.Cells.FromKey("POD") != null
      && e.Row.Cells.FromKey("CountryCode") != null
      && e.Row.Cells.FromKey("Status") != null
      && e.Row.Cells.FromKey("ItemId") != null)
    {
      if (e.Row.Cells.FromKey("PID").Value != null)
      {
        e.Row.Cells.FromKey("PID").Text = "<a href='javascript://' onclick='EditPLC(" + e.Row.Cells.FromKey("ItemId").Text +
                                               "," + e.Row.Cells.FromKey("PID").Column.Index.ToString() +
                                               "," + e.Row.Index.ToString() +
                                               ",&quot;" + dg.ClientID + "&quot;" +
                                               ",&quot;" + e.Row.Cells.FromKey("CountryCode").Text + "&quot;" +
                                               ");color=&quot;" + GetStatusLinkColor(cssClass) + "&quot;;'><u><font color=" + GetStatusLinkColor(cssClass) +
                                               ">" + ((DateTime)e.Row.Cells.FromKey("PID").Value).ToString(SessionState.User.FormatDate) + "</font></u></a>";
      }
      if (e.Row.Cells.FromKey("POD").Value != null)
      {
        e.Row.Cells.FromKey("POD").Text = "<a href='javascript://' onclick='EditPLC(" + e.Row.Cells.FromKey("ItemId").Text +
                                                   "," + e.Row.Cells.FromKey("PID").Column.Index.ToString() +
                                                   "," + e.Row.Index.ToString() +
                                                   ",&quot;" + dg.ClientID + "&quot;" +
                                                   ",&quot;" + e.Row.Cells.FromKey("CountryCode").Text + "&quot;" +
                                                   ");color=&quot;" + GetStatusLinkColor(cssClass) + "&quot;;'><u><font color=" + GetStatusLinkColor(cssClass) +
                                                   ">" + ((DateTime)e.Row.Cells.FromKey("POD").Value).ToString(SessionState.User.FormatDate) + "</font></u></a>";
      }
    }
    #endregion
    #region "Update outstanding (rdobadfd or rdod or adfd)"
    bool RDIsDifOB = false;
    bool ADIsDifFD = false;
    if (e.Row.Cells.FromKey("RDIsDifOB") != null && e.Row.Cells.FromKey("RDIsDifOB").Value != null)
      RDIsDifOB = Convert.ToBoolean(e.Row.Cells.FromKey("RDIsDifOB").Value);
    if (e.Row.Cells.FromKey("ADIsDifFD") != null && e.Row.Cells.FromKey("ADIsDifFD").Value != null)
      ADIsDifFD = Convert.ToBoolean(e.Row.Cells.FromKey("ADIsDifFD").Value);

    if (e.Row.Cells.FromKey("Outstanding") != null)
    {
      if (RDIsDifOB && ADIsDifFD)
        e.Row.Cells.FromKey("Outstanding").Style.CssClass = "rdobadfd";
      else if (RDIsDifOB)
        e.Row.Cells.FromKey("Outstanding").Style.CssClass = "rdod";
      else if (ADIsDifFD)
        e.Row.Cells.FromKey("Outstanding").Style.CssClass = "adfd";
    }
    #endregion
    #region "Update checkbox"
    string dateType = string.Empty;
    
    if (e.Row.Cells.FromKey("FullDateType") != null && e.Row.Cells.FromKey("FullDateType").Value != null
      && e.Row.Cells.FromKey("PromotePID") != null)
    {
      dateType = e.Row.Cells.FromKey("FullDateType").Value.ToString();
      if (dateType == "C")
      {
        e.Row.Cells.FromKey("PromotePID").Value = true;
        e.Row.Cells.FromKey("PromotePID").AllowEditing = AllowEditing.No;
      }
    } 
    if (e.Row.Cells.FromKey("ObsoleteDateType") != null && e.Row.Cells.FromKey("ObsoleteDateType").Value != null
      && e.Row.Cells.FromKey("PromotePOD") != null)
    {
      dateType = e.Row.Cells.FromKey("ObsoleteDateType").Value.ToString();
      if (dateType == "C")
      {
        e.Row.Cells.FromKey("PromotePOD").Value = true;
        e.Row.Cells.FromKey("PromotePOD").AllowEditing = AllowEditing.No;
      }
    }
    #endregion
    #region "Update filter"
    if (txtFilter.Text.Length > 0)
    {
      if (e.Row.Cells.FromKey("Country") != null && e.Row.Cells.FromKey("Name") != null && e.Row.Cells.FromKey("SKU") != null)
      {
        Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
        UITools.HiglightGridRowFilter(ref r, txtFilter.Text, new int[] { r.Cells.FromKey("Country").Column.Index }); 
        UITools.HiglightGridRowFilter(ref r, txtFilter.Text, new int[] { r.Cells.FromKey("Name").Column.Index });
        UITools.HiglightGridRowFilter(ref r, txtFilter.Text, new int[] { r.Cells.FromKey("SKU").Column.Index });
      }
    }
    #endregion
  }
  #endregion

}
