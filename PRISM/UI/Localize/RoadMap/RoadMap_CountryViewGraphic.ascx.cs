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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using HyperCatalog.Shared;

public partial class RoadMap_CountryViewGraphic : UserControl
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
  private Int64 _ItemId = -1;
  private DateTime _StartDate = DateTime.Now;
  private int _NbMonths = 6;
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
  public Int64 ItemId
  {
    get { return _ItemId; }
    set { _ItemId = value; }
  }
  public DateTime StartDate
  {
    get { return _StartDate; }
    set { _StartDate = value; }
  }
  public int NbMonths
  {
    get { return _NbMonths; }
    set { _NbMonths = value; }
  }
  #endregion

  #region Constantes
  private const int MONTH_WIDTH = 30;
  private const int TIMEOUT = 300; // 5 min
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.VIEW_ROADMAP))
    {
      try
      {
        // Display start date
        if (wdCalendar.Text.Length == 0)
        {
          wdCalendar.Value = DateTime.Now;
          _StartDate = DateTime.Now;
          _NbMonths = 6;
        }
        
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
        wdCalendar.CalendarLayout.Culture = ci;
      }
      catch (Exception ex)
      {
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "", "<script>alert('"+ex.Message+"');</script>");
      }
    }
  }
  public void Refresh()
  {
    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
    ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
    ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
    wdCalendar.CalendarLayout.Culture = ci;

    UpdateStyleCountry();
    UpdateDataView();
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
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "UpdateStyle", "<style>" + styles + "</style>");
      }
    }
  }
  public void UpdateDataView()
  {
    lbError.Visible = false;
    lbResult.Visible = false;

    if (_ItemId > -1)
    {
      UITools.HideToolBarButton(uwToolbar, "Back");
      UITools.HideToolBarSeparator(uwToolbar, "BackSep");
      UITools.HideToolBarButton(uwToolbar, "ListView");
      UITools.HideToolBarSeparator(uwToolbar, "ListViewSep");
    }
    else
    {
      UITools.HideToolBarButton(uwToolbar, "Close");
      UITools.HideToolBarSeparator(uwToolbar, "CloseSep");
    }

    // Retrieve the beginning of date
    if (wdCalendar.Value != null)
      _StartDate = (DateTime)wdCalendar.Value;
    // Retrieve the period
    _NbMonths = Convert.ToInt32(DDL_Period.SelectedValue);

    // Retrieve RoadMap with filter
    Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, TIMEOUT);
    if (dbObj != null)
    {
      DataSet ds = null;
      try
      {
        Trace.Warn("Before run _RoadMap_Countries");
        ds = dbObj.RunSPReturnDataSet("_RoadMap_Countries",
          new SqlParameter("@ItemId", _ItemId),
          new SqlParameter("@StatusList", _StatusList),
          new SqlParameter("@WorkflowStatusList", _WorkflowStatus),
          new SqlParameter("@CountryList", _Countries),
          new SqlParameter("@ClasseList", _Classes),
          new SqlParameter("@FilterLiveDate", _FilterLiveDate),
          new SqlParameter("@FilterObsolDate", _FilterObsoleteDate),
          new SqlParameter("@LiveDate1", (_LiveDate1.HasValue ? _LiveDate1.Value : DateTime.Now)),
          new SqlParameter("@LiveDate2", (_LiveDate2.HasValue ? _LiveDate2.Value : DateTime.Now)),
          new SqlParameter("@ObsoleteDate1", (_ObsoleteDate1.HasValue ? _ObsoleteDate1.Value : DateTime.Now)),
          new SqlParameter("@ObsoleteDate2", (_ObsoleteDate2.HasValue ? _ObsoleteDate2.Value : DateTime.Now)),
          new SqlParameter("@ProductScope", _Scope),
          new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id),
          new SqlParameter("@Filter", string.Empty));

        dbObj.CloseConnection();

        if (dbObj.LastError != null && dbObj.LastError.Length > 0)
        {
          lbError.Text = dbObj.LastError;
          lbError.Visible = true;
        }
        else if (ds != null)
        {
          if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
          {
            // create repeater
            rDates.DataSource = ds;
            Trace.Warn("Before run Databind");
            rDates.DataBind();
            Trace.Warn("After run Databind");

            rDates.Visible = true;

            lbResult.CssClass = "hc_success";
            lbResult.Text = "SKU count: " + ds.Tables[0].Rows.Count;
            lbResult.Visible = true;
          }
          else
          {
            lbResult.CssClass = "hc_success";
            lbResult.Text = "No SKU";
            lbResult.Visible = true;

            rDates.Visible = false;
          }

          ds.Dispose();
        }
      }
      catch (Exception e)
      {
        lbError.CssClass = "hc_error";
        lbError.Text = e.ToString();
        lbError.Visible = true;

        lbResult.Visible = false;
        rDates.Visible = false;
      }
      finally
      {
        if (ds != null)
          ds.Dispose();
      }
    }
  }

  #region Delegate
  public delegate void BackButtonClickHandler(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e);
  public event BackButtonClickHandler BackButtonClick;
  public delegate void ViewListButtonClickHandler(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e);
  public event ViewListButtonClickHandler ViewListButtonClick;
  public delegate void ViewGraphicButtonClickHandler(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent e);
  public event ViewGraphicButtonClickHandler ViewGraphicButtonClick;
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
      case "listview":
        if (ViewListButtonClick != null)
          ViewListButtonClick(this, be);
        break;
      case "show":
        ReIntializeVars();
        UpdateDataView();
        if (ViewGraphicButtonClick != null)
          ViewGraphicButtonClick(this, be);
        break;
    }
  }
  protected void rDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
  {
    if (e != null && e.Item != null)
    {
      if (e.Item.ItemType.Equals(ListItemType.Header))
      {
        #region "Header"
        Label lbHeader = (Label)e.Item.FindControl("lbHeader");
        if (lbHeader != null)
        {
          lbHeader.Text = "<tr valign='top'>";
          lbHeader.Text += "<td class='h'>Name</td>";
          DateTime curDateTitle = _StartDate;
          for (int i = 0; i <= _NbMonths; i++)
          {
            lbHeader.Text += "<td class='v' width='" + ComputeWidth().ToString() + "px'>" + curDateTitle.ToString(SessionState.User.FormatDate) + "&nbsp;</td>";
            curDateTitle = curDateTitle.AddMonths(1);
          }

          lbHeader.Text += "<td class='bs'>&nbsp;</td>";
          lbHeader.Text += "</tr>";
        }
        #endregion
      }
      else if (e.Item.ItemType.Equals(ListItemType.Item) || e.Item.ItemType.Equals(ListItemType.AlternatingItem))
      {
        #region Item
        int index = e.Item.ItemIndex;
        Label lbBar = (Label)e.Item.FindControl("lbBar");

        if (lbBar != null && rDates != null)
        {
          DataSet ds = (DataSet)rDates.DataSource;
          DataRow dr = null;
          string curItemSku = string.Empty;
          string curCountryCode = string.Empty;
          bool ADIsDifFD = false;
          bool RDIsDifOD = false;

          if (index >= 0 && ds.Tables != null && ds.Tables.Count > 0 && index < ds.Tables[0].Rows.Count)
          {
            dr = ds.Tables[0].Rows[index];
            if (dr != null)
            {
              lbBar.Text = "<tr>";

              #region "Display name"
              curItemSku = dr["ItemSku"].ToString();
              curCountryCode = dr["CountryCode"].ToString();
              ADIsDifFD = Convert.ToBoolean(dr["ADIsDifFD"]);
              RDIsDifOD = Convert.ToBoolean(dr["RDIsDifOD"]);

              lbBar.Text += "<td class='n ctry" + curCountryCode.ToLower() + "'>";
              if (curItemSku != null && curItemSku.Length > 0)
                lbBar.Text += "&nbsp;[" + curItemSku + "]";

              if (ADIsDifFD && RDIsDifOD)
                lbBar.Text += "&nbsp;<img src='/hc_v4/img/ARD.gif' height='16px' />";
              else if (ADIsDifFD)
                lbBar.Text += "&nbsp;<img src='/hc_v4/img/AD.gif' height='16px' />";
              else if (RDIsDifOD)
                lbBar.Text += "&nbsp;<img src='/hc_v4/img/RD.gif' height='16px' />";

              lbBar.Text += "</td>";
              #endregion

              #region "Draw row"
              lbBar.Text += "<td class='bb' colspan='" + (_NbMonths + 1).ToString() + "'>";
              lbBar.Text += DrawRow(dr);
              lbBar.Text += "</td>";
              #endregion

              lbBar.Text += "<td class='bs'>&nbsp;</td>";
              lbBar.Text += "</tr>";
            }
          }
        }
        #endregion
      }
    }
  }
  #endregion

  #region Private methods
  private string DrawRow(DataRow dr)
  {
    string drawing = string.Empty;
    if (dr != null)
    {
      drawing += "<table class='b'><tr>";
      drawing += "<td class='mr'></td>";
      drawing += "<td class='p' width='" + (ComputeWidth() * _NbMonths).ToString() + "px'>";
      drawing += "<A title=\"" + ShowHintInfo(dr) + "\" href='javascript://'>";
      drawing += "<table class='rc'><tr>";
      drawing += CreateBar(dr);
      drawing += "</tr>";
      drawing += "</table>";
      drawing += "</A>";
      drawing += "</td>";
      drawing += "</tr>";
      drawing += "</table>";
    }
    return drawing.Replace("\n", "");
  }
  private double ComputeWidth()
  {
    switch (_NbMonths)
    {
      case 6:
        return MONTH_WIDTH * 2.3;
      case 9:
        return MONTH_WIDTH * 2;
      case 12:
        return MONTH_WIDTH * 1.7;
      case 15:
        return MONTH_WIDTH * 1.5;
      case 18:
        return MONTH_WIDTH * 1.3;
      default:
        return MONTH_WIDTH;
    }
  }
  private string CreateBar(DataRow dr)
  {
    string bar = string.Empty;

    // Build date list
    ArrayList dateListSorted = BuildPLCDates(dr);

    // Create bar
    if (dateListSorted != null && dateListSorted.Count > 0)
    {
      int nbDays;
      double colWidth;
      TimeSpan difDate;

      DateTime endDate = _StartDate.AddMonths(_NbMonths);
      dateListSorted = RetrieveIncludePeriod(dateListSorted, _StartDate, endDate); // Contains (startDate, d1, d2, ..., endDate)

      for (int i=0; i<dateListSorted.Count; i++)
      {
        RoadMapDate currentDate = (RoadMapDate)dateListSorted[i];
        if (i < dateListSorted.Count - 1)
        {
          RoadMapDate nextDate = (RoadMapDate)dateListSorted[i + 1];
          nbDays = RetrieveNbDays(_StartDate, _NbMonths);

          difDate = nextDate.Date.Subtract(currentDate.Date);
          colWidth = Math.Round((difDate.TotalDays * 100) / nbDays, 0);

          if (colWidth == 0)
            colWidth = 1;

          if (nextDate.Date == endDate)
            bar += "<td class='" + currentDate.CssClass + "' width='*'>&nbsp;</td>";
          else
            bar += "<td class='" + currentDate.CssClass + "' width='" + colWidth.ToString() + "%'>&nbsp;</td>";
        }
      }
    }
    else
    {
      bar += "<td class='rg' width='100%'>&nbsp;</td>";
    }

    return bar;
  }
  private ArrayList BuildPLCDates(DataRow dr)
  {
    DateTime? blindDate = null;
    DateTime? fullDate = null;
    DateTime? obsoleteDate = null;
    ArrayList dateList = new ArrayList();
    string status = string.Empty;
    string workflowStatus = string.Empty;

    if (dr != null)
    {
      // Retrieve all dates
      if (dr.Table.Columns.IndexOf("BlindDate") > -1 && dr["BlindDate"] != null && dr["BlindDate"] != DBNull.Value)
        blindDate = Convert.ToDateTime(dr["BlindDate"]);
      if (dr.Table.Columns.IndexOf("FullDate") > -1 && dr["FullDate"] != null && dr["FullDate"] != DBNull.Value)
        fullDate = Convert.ToDateTime(dr["FullDate"]);
      if (dr.Table.Columns.IndexOf("ObsoleteDate") > -1 && dr["ObsoleteDate"] != null && dr["ObsoleteDate"] != DBNull.Value)
        obsoleteDate = Convert.ToDateTime(dr["ObsoleteDate"]);

      // Update status
      if (dr.Table.Columns.IndexOf("Status") > -1 && dr["Status"] != null && dr["Status"] != DBNull.Value)
        status = dr["Status"].ToString();

      string cssClass = string.Empty;
      if (status == "L") cssClass = "rl";
      else if (status == "O") cssClass = "ro";
      else if (status == "F")
      {
        if (workflowStatus == "R")
          cssClass = "rrv";
        else if (workflowStatus == "C")
          cssClass = "rcv";
      }
      else if (status == "E") cssClass = "re";

      if (blindDate.HasValue && fullDate.HasValue && blindDate.Value > fullDate.Value) cssClass = "ri";
      if (obsoleteDate.HasValue && fullDate.HasValue && fullDate.Value > obsoleteDate.Value) cssClass = "ri";
      if (blindDate.HasValue && obsoleteDate.HasValue && blindDate.Value > obsoleteDate.Value) cssClass = "ri";

      // Update workflow status
      if (dr.Table.Columns.IndexOf("WorkflowStatus") > -1 && dr["WorkflowStatus"] != null && dr["WorkflowStatus"] != DBNull.Value)
        workflowStatus = dr["WorkflowStatus"].ToString();

      // Build date list
      if (blindDate.HasValue)
        dateList.Add(new RoadMapDate(blindDate.Value, cssClass));
      if (fullDate.HasValue)
        dateList.Add(new RoadMapDate(fullDate.Value, cssClass));
      if (obsoleteDate.HasValue)
        dateList.Add(new RoadMapDate(obsoleteDate.Value, cssClass));

      // Sort list
      dateList = Sort(dateList);

      // Update css of the last date
      if (dateList != null)
        ((RoadMapDate)dateList[dateList.Count - 1]).CssClass = "rg";
    }

    return dateList;
  }
  private string ShowHintInfo(DataRow dr)
  {
    string info = string.Empty;
    string newLine = "&#013;";
    if (dr != null)
    {
      if (dr.Table.Columns.IndexOf("ItemSku") > -1 && dr["ItemSku"] != null && dr["ItemSku"] != DBNull.Value)
        info += "[" + dr["ItemSku"].ToString() + "]" + newLine;
      if (dr.Table.Columns.IndexOf("ItemName") > -1 && dr["ItemName"] != null && dr["ItemName"] != DBNull.Value)
        info += dr["ItemName"].ToString() + newLine;
      if (dr.Table.Columns.IndexOf("CountryName") > -1 && dr["CountryName"] != null && dr["CountryName"] != DBNull.Value)
        info += dr["CountryName"].ToString().ToUpper() + newLine;

      if (info.Length > 0)
        info += newLine;

      // Status and workflow status
      if (dr.Table.Columns.IndexOf("Status") > -1 && dr["Status"] != null && dr["Status"] != DBNull.Value)
        info += "Status: " + Item.GetStatusFromString(dr["Status"].ToString().ToUpper()).ToString() + newLine;
      if (dr.Table.Columns.IndexOf("WorkflowStatus") > -1 && dr["WorkflowStatus"] != null && dr["WorkflowStatus"] != DBNull.Value)
        info += "Workflow status: " + Item.GetWorkflowStatusFromString(dr["WorkflowStatus"].ToString().ToUpper()).ToString() + newLine;
      
      if (info.Length > 0)
        info += newLine;

      // Dates
      string datetype = string.Empty;
      if (dr.Table.Columns.IndexOf("BlindDate") > -1 && dr["BlindDate"] != null && dr["BlindDate"] != DBNull.Value)
      {
        if (dr.Table.Columns.IndexOf("BlindDateType") > -1 && dr["BlindDateType"] != null && dr["BlindDateType"] != DBNull.Value)
          datetype = " (" + dr["BlindDateType"].ToString() + ")";
        info += "Blind date: " + Convert.ToDateTime(dr["BlindDate"]).ToString(SessionState.User.FormatDate) + datetype + newLine;
      }
      if (dr.Table.Columns.IndexOf("FullDate") > -1 && dr["FullDate"] != null && dr["FullDate"] != DBNull.Value)
      {
        datetype = string.Empty;
        if (dr.Table.Columns.IndexOf("FullDateType") > -1 && dr["FullDateType"] != null && dr["FullDateType"] != DBNull.Value)
          datetype = " (" + dr["FullDateType"].ToString() + ")";
        info += "Full date: " + Convert.ToDateTime(dr["FullDate"]).ToString(SessionState.User.FormatDate) + datetype + newLine;
      }
      if (dr.Table.Columns.IndexOf("ObsoleteDate") > -1 && dr["ObsoleteDate"] != null && dr["ObsoleteDate"] != DBNull.Value)
      {
        datetype = string.Empty;
        if (dr.Table.Columns.IndexOf("ObsoleteDateType") > -1 && dr["ObsoleteDateType"] != null && dr["ObsoleteDateType"] != DBNull.Value)
          datetype = " (" + dr["ObsoleteDateType"].ToString() + ")";
        info += "Obsolete date: " + Convert.ToDateTime(dr["ObsoleteDate"]).ToString(SessionState.User.FormatDate) + datetype + newLine;
      }
      if (dr.Table.Columns.IndexOf("AnnouncementDate") > -1 && dr["AnnouncementDate"] != null && dr["AnnouncementDate"] != DBNull.Value)
      {
        datetype = string.Empty;
        if (dr.Table.Columns.IndexOf("AnnouncementDateType") > -1 && dr["AnnouncementDateType"] != null && dr["AnnouncementDateType"] != DBNull.Value)
          datetype = " (" + dr["AnnouncementDateType"].ToString() + ")";
        info += "Announcement date: " + Convert.ToDateTime(dr["AnnouncementDate"]).ToString(SessionState.User.FormatDate) + datetype + newLine;
      }
      if (dr.Table.Columns.IndexOf("RemovalDate") > -1 && dr["RemovalDate"] != null && dr["RemovalDate"] != DBNull.Value)
      {
        datetype = string.Empty;
        if (dr.Table.Columns.IndexOf("RemovalDateType") > -1 && dr["RemovalDateType"] != null && dr["RemovalDateType"] != DBNull.Value)
          datetype = " (" + dr["RemovalDateType"].ToString() + ")";
        info += "Removal date: " + Convert.ToDateTime(dr["RemovalDate"]).ToString(SessionState.User.FormatDate) + datetype + newLine;
      }
      info = info.Substring(0, (info.Length - newLine.Length));
    }

    return info;
  }
  private ArrayList Sort(ArrayList sourceList)
  {
    if (sourceList != null && sourceList.Count > 1)
    {
      ArrayList resultList = new ArrayList();
      foreach (RoadMapDate sd in sourceList)
      {
        bool isOk = false;
        int i = 0;
        while (i < resultList.Count && !isOk)
        {
          RoadMapDate rd = (RoadMapDate)resultList[i];
          if (sd.Date <= rd.Date)
          {
            resultList.Insert(i, sd);
            isOk = true;
          }
          i++;
        }
        if (!isOk)
          resultList.Add(sd);
      }
      return resultList;
    }
    else
      return sourceList;
  }
  private ArrayList RetrieveIncludePeriod(ArrayList sourceList, DateTime startD, DateTime endD)
  {
    // The source list is sorted by datetime
    ArrayList resultList = new ArrayList();
    resultList.Add(new RoadMapDate(startD, "rg")); // by default the bar is empty
    if (sourceList != null)
    {
      foreach (RoadMapDate d in sourceList)
      {
        if (d.Date <= _StartDate)
        {
          resultList.Clear();
          resultList.Add(new RoadMapDate(startD, d.CssClass));
        }
        else if (d.Date > startD && d.Date < endD)
        {
            resultList.Add(d);
        }
      }
    }
    resultList.Add(new RoadMapDate(endD, "rg")); 

    return resultList;
  }
  private int RetrieveNbDays(DateTime startD, int nbM)
  {
    int nbDays = 0;
    DateTime curDate = startD;
    for (int i = 0; i < nbM; i++)
    {
      nbDays += DateTime.DaysInMonth(curDate.Year, curDate.Month);
      curDate.AddMonths(1);
    }
    return nbDays;
  }
  private void ReIntializeVars()
  {
    // re-intialize variables
    if (_ItemId == -1 && Request["i"] != null)
    {
      if (Request["i"] != null)
        _ItemId = Convert.ToInt64(Request["i"]);
      if (Request["c"] != null)
        _Countries = Request["c"].ToString();
      _StatusList = "U,L,O,F,E";
      _WorkflowStatus = "U,R,C,E";
      _FilterLiveDate = "any";
      _FilterObsoleteDate = "any";
      _Scope = true;
    }
  }
  #endregion

  private class RoadMapDate
  {
    private DateTime _Date;
    private string _CssClass;

    public DateTime Date
    {
      get { return _Date; }
      set { _Date = value; }
    }

    public string CssClass
    {
      get { return _CssClass; }
      set { _CssClass = value; }
    }

    public RoadMapDate(DateTime date, string cssClass)
    {
      _Date = date;
      _CssClass = cssClass;
    }
  }
}
