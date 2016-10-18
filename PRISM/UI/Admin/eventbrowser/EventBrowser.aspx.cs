#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
#endregion

#region History
/*=============Modification Details====================================
      Mod#1 Time format changed to GMT
      Description: QC791
      Modfication Date:12/10/2007
      Modified by: Jothi*/
#endregion
public partial class EventBrowser : HCZipPage
{
  private const int MAX_EXCEL_SIZE = 65536;

  protected int filterComponentId
  {
    get
    {
      try
      {
        return Convert.ToInt32(componentList.SelectedValue);
      }
      catch
      {
        return -1;
      }
    }
  }

  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      if (!eventsGrid.IsXmlHttpRequest)
      {
        setRange("Today");
        using (HyperCatalog.Business.ApplicationComponentCollection batchComponents = HyperCatalog.Business.ApplicationComponent.GetAll(HyperCatalog.Business.ApplicationComponentTypes.Batch))
        {
          componentList.DataSource = batchComponents;
          componentList.DataTextField = "Name";
          componentList.DataValueField = "Id";
          componentList.DataBind();
          componentList.Items.Insert(0, new ListItem("-- All batches", "-1"));
        }

        #region url parameters

        if (Request["AppComponentId"] != null)
        {
          try
          {
            componentList.SelectedValue = Request["AppComponentId"];
            mainToolBar.Items.Remove(mainToolBar.Items.FromKeyCustom("componentList"));
            mainToolBar.Items.Remove(mainToolBar.Items.FromKeyLabel("filter"));
            mainToolBar.Items.Remove(mainToolBar.Items.FromKeySeparator("filterSep"));
          }
          catch { }

        }
        if (Request["before"] != null)
        {
          try
          {
            //Response.Write("Init Date Before<br/>"); 
            endDate.Value = DateTime.Parse(Request["before"]);
            startDate.Value = DateTime.MinValue;
          }
          catch { }
        }
        if (Request["after"] != null)
        {
          try
          {
            //Response.Write("Init Date after<br/>");
            startDate.Value = DateTime.Parse(Request["after"]);
          }
          catch { }
        }
        #endregion
      }
      mainToolBar.Items.FromKeyButton("Today").Selected = true;
    }
    eventsGrid.DisplayLayout.EnableInternalRowsManagement = false;
    eventsGrid.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
    eventsGrid.DisplayLayout.LoadOnDemand = Infragistics.WebUI.UltraWebGrid.LoadOnDemand.Xml;
    Utils.InitGridSort(ref eventsGrid, false);
    System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
    ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
    ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
    startDate.CalendarLayout.Culture = ci;
    endDate.CalendarLayout.Culture = ci;
    startDate.MinDate = DateTime.Today.AddMonths(-1);
    startDate.MaxDate = DateTime.Today;
    endDate.MinDate = DateTime.Today.AddMonths(-1);
    endDate.MaxDate = DateTime.Today;
  }
  private DataSet GetDataSet()
  {
    using (Database dbObj = Utils.GetMainDB())
    {
      if (Session["JobsDS"] == null)
      {
        Session["JobsDS"] = dbObj.RunSPReturnDataSet("_EventBrowser_GetAllJobsReturnDS",
          new SqlParameter("@StartDate", startDate.Value),
          new SqlParameter("@EndDate", ((DateTime)endDate.Value).AddDays(1)),
          new SqlParameter("@ComponentId", filterComponentId));
        if (dbObj.LastError != string.Empty)
        {
          //Response.Write("Init Dataset from DB and ERROR = " + dbObj.LastError + "<br/>");
        }
        else
        {
          //Response.Write("Init Dataset from DB and row count = " + ((DataSet)Session["JobsDS"]).Tables[0].Rows.Count.ToString() + "<br/>");
        }
      }
      dbObj.CloseConnection();
    }
    using (DataSet ds = ((DataSet)Session["JobsDS"]))
    {
      if (ds != null && ds.Tables.Count > 0)
      {
        //Response.Write("Return Dataset from Session and row count = " + ((DataSet)Session["JobsDS"]).Tables[0].Rows.Count.ToString() + "<br/>");
        //Response.Write("BindGrid<br/>");
        lError.Visible = false;
        eventsGrid.Visible = true;
        if (ds.Tables.Count > 0)
        {
          #region No result
          if (ds.Tables[0].Rows.Count == 0)
          {
            eventsGrid.Visible = false;
            lError.Text = "Sorry, your query is returning no result";
            lError.Visible = true;
            lError.CssClass = "hc_error";
          }
          Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "totalrows", "var totalRows = " + ds.Tables[0].Rows.Count.ToString() + ";", true);
          #endregion
        }

        return ds;
      }
    }
    eventsGrid.Visible = false;
    lError.Text = "Sorry, an expected error occurred";
    lError.Visible = true;
    lError.CssClass = "hc_error";
    return null;
  }

  #region Web Form Designer generated code
  override protected void OnInit(EventArgs e)
  {
    //
    // CODEGEN: This call is required by the ASP.NET Web Form Designer.
    //
    InitializeComponent();
    base.OnInit(e);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.eventsGrid.InitializeDataSource += new Infragistics.WebUI.UltraWebGrid.InitializeDataSourceEventHandler(this.eventsGrid_InitializeDataSource);
  }
  #endregion
  protected override void OnPreRender(EventArgs e)
  {
    eventsGrid.Columns.FromKey("Component").ServerOnly = filterComponentId >= 0;
  }
  private void eventsGrid_InitializeDataSource(object sender, Infragistics.WebUI.UltraWebGrid.UltraGridEventArgs e)
  {
    //Response.Write("eventsGrid_InitializeDataSource<br/>");
    if (mainToolBar.Items.FromKeyButton("Today").Selected)
    {
      setRange("Today");
    }
    if (mainToolBar.Items.FromKeyButton("Yesterday").Selected)
    {
      setRange("Yesterday");
    }
    if (mainToolBar.Items.FromKeyButton("LastWeek").Selected)
    {
      setRange("LastWeek");
    }

    if (((DateTime)startDate.Value) == DateTime.Today.Date && ((DateTime)endDate.Value) == DateTime.Today.Date)
      setRange("Today");
    else if (((DateTime)startDate.Value) == DateTime.Today.AddDays(-1).Date && ((DateTime)endDate.Value) == DateTime.Today.AddDays(-1).Date)
      setRange("Yesterday");
    else if (((DateTime)startDate.Value) == DateTime.Today.AddDays(-7).Date && ((DateTime)endDate.Value) == DateTime.Today.Date)
      setRange("LastWeek");
    else
    {
      advancedToolBar.ClientSideEvents.InitializeToolbar = "";
      //Response.Write("advancedToolBar_ButtonClicked -> Resets Dataset <br/>");
      if (Session["JobsDS"] != null) { ((DataSet)Session["JobsDS"]).Dispose(); Session["JobsDS"] = null; }
      mainToolBar.Items.FromKeyButton("Today").Selected = false;
      mainToolBar.Items.FromKeyButton("Yesterday").Selected = false;
      mainToolBar.Items.FromKeyButton("LastWeek").Selected = false;
    }

    eventsGrid.DataSource = GetDataSet();
  }

  public static string GetStatus(int status)
  {
    switch (status)
    {
      case 0: //STILLRUNNING
        return "<font style=\"color:orange;\">Running</font>";
      case 1: //SUCCESS
        return "<font style=\"color:green;\">Success</font>";
      case 2: // FAILURE
        return "<font style=\"color:red;\">Failure</font>";
      default:
        return String.Empty;

    }
  }

  public static string severityImgSrc(HyperCatalog.Business.EventBrowser.Severities severity)
  {
    string commonImgPath = "/hc_v4/img/";
    switch (severity)
    {
      case HyperCatalog.Business.EventBrowser.Severities.WARNING:
        return commonImgPath + "ed_warning.gif";
      case HyperCatalog.Business.EventBrowser.Severities.ERROR:
        return commonImgPath + "ed_error2.gif";
      default:
        return commonImgPath + "ed_information.gif";
    }
  }

  public static string GetEvents(bool hasInfo, bool hasWarnings, bool hasErrors)
  {
    string result = string.Empty;
    if (hasInfo)
      result += " <img src=\"" + severityImgSrc(HyperCatalog.Business.EventBrowser.Severities.INFORMATION) + "\" alt=\"Informations\">";
    if (hasWarnings)
      result += " <img src=\"" + severityImgSrc(HyperCatalog.Business.EventBrowser.Severities.WARNING) + "\" alt=\"Warnings\">";
    if (hasErrors)
      result += " <img src=\"" + severityImgSrc(HyperCatalog.Business.EventBrowser.Severities.ERROR) + "\" alt=\"Errors\">";
    return result;
  }

  public static string displayDuration(TimeSpan duration)
  {
    if (duration.Ticks == 0)
      return "-";

    string result = string.Empty;
    if (duration.Days > 0)
      result += duration.Days + " days";
    else
    {
      if (duration.Hours > 0)
        result += " " + duration.Hours + "h";
      if (duration.Minutes > 0)
        result += " " + duration.Minutes + "m";
      if (duration.Seconds > 0)
        result += " " + duration.Seconds + "s";
    }

    if (result == string.Empty)
      result += " 0." + duration.Milliseconds + "s";

    return result;
  }

  protected void eventsGrid_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.UltraGridCell s = e.Row.Cells.FromKey("StartDate");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell f = e.Row.Cells.FromKey("EndDate");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell d = e.Row.Cells.FromKey("Duration");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell ev = e.Row.Cells.FromKey("Events");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell r = e.Row.Cells.FromKey("Result");

    TimeSpan duration = f.Value != null ? ((DateTime)f.Value).Subtract((DateTime)s.Value).Duration() : DateTime.UtcNow.Subtract((DateTime)s.Value).Duration();
    d.Text = displayDuration(duration);
    /****** Below line is commented for QC# 791 ****/
    //s.Value = HyperCatalog.Shared.SessionState.User.GMTTimeZone.ToLocalTime((DateTime)s.Value).ToString();
    s.Value = "<a href= 'eventbrowserdetail.aspx?e=" + e.Row.Cells.FromKey("JobId").Value.ToString() + "' target='jobdetail'>" + s.Text + "</a>";
    r = e.Row.Cells.FromKey("Result");
    r.Text = GetStatus(Convert.ToInt32(r.Value));

    ev = e.Row.Cells.FromKey("Events");
    ev.Text = GetEvents((bool)e.Row.Cells.FromKey("HasInfos").Value, (bool)e.Row.Cells.FromKey("HasWarnings").Value, (bool)e.Row.Cells.FromKey("HasErrors").Value);
    ev.Value = ev.Text;
  }

  private void setRange(string key)
  {
    //Response.Write("setRange(" + key +") -> Resets Dataset <br/>");

    if (Session["JobsDS"] != null) { ((DataSet)Session["JobsDS"]).Dispose(); Session["JobsDS"] = null; }
    advancedToolBar.ClientSideEvents.InitializeToolbar = "showHideAdvancedToolBar();";
    switch (key)
    {
      case "Today":
        mainToolBar.Items.FromKeyButton("Today").Selected = true;
        mainToolBar.Items.FromKeyButton("Yesterday").Selected = false;
        mainToolBar.Items.FromKeyButton("LastWeek").Selected = false;
        startDate.Value = DateTime.Today.Date;
        endDate.Value = DateTime.Today.Date;
        break;
      case "Yesterday":
        mainToolBar.Items.FromKeyButton("Today").Selected = false;
        mainToolBar.Items.FromKeyButton("Yesterday").Selected = true;
        mainToolBar.Items.FromKeyButton("LastWeek").Selected = false;
        startDate.Value = DateTime.Today.AddDays(-1).Date;
        endDate.Value = DateTime.Today.AddDays(-1).Date;
        break;
      case "LastWeek":
        mainToolBar.Items.FromKeyButton("Today").Selected = false;
        mainToolBar.Items.FromKeyButton("Yesterday").Selected = false;
        mainToolBar.Items.FromKeyButton("LastWeek").Selected = true;
        startDate.Value = DateTime.Today.AddDays(-7).Date;
        endDate.Value = DateTime.Today.Date;
        break;
    }
  }

  protected void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    switch (be.Button.Key)
    {
      case "Export":
        ExportToExcel();
        break;
    }
  }
  protected void advancedToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
  }

  protected void csvLink_Click(object sender, EventArgs e)
  {
    DataSet ds = (DataSet)Session["JobsDS"];
    if (ds != null)
    {
      string s = Utils.ExportDataTableToCSV(ds.Tables[0], ";").ToString();
      System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
      byte[] contentBytes = encoding.GetBytes(s);

      Response.Clear();
      Response.ClearContent();
      Response.ClearHeaders();
      Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
      Response.ContentType = "application/text";
      //Fix for CR 5109 - Prabhu R S
      Response.ContentEncoding = System.Text.Encoding.UTF8;
      Response.AppendHeader("Content-Disposition", "attachment;filename=\"EventLogs.txt\"; " +
                        "size=" + s.Length.ToString() + "; " +
                        "creation-date=" + DateTime.Now.ToString("R") + "; " +
                        "modification-date=" + DateTime.Now.ToString("R") + "; " +
                       "read-date=" + DateTime.Now.ToString("R"));

      Response.OutputStream.Write(
      contentBytes, 0,
      Convert.ToInt32(contentBytes.Length));
      Response.Flush();
      try { Response.End(); }
      catch { }
    }
  }
  protected void ExportToExcel()
  {
    DataSet ds = (DataSet)Session["JobsDS"];
    if (ds != null)
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      sb.Append("<TABLE border=\"1\">");
      bool isHeader = true;
      foreach (DataRow row in ds.Tables[0].Rows)
      {
        if (isHeader)
        {
          sb.Append("<TR>");
          foreach (DataColumn col in ds.Tables[0].Columns)
            if (col.ColumnMapping != MappingType.Hidden)
              sb.Append("<TH style=\"font-weight:bold;\">" + col.Caption + "</TH>");
          sb.Append("</TR>");
          isHeader = false;
        }

        sb.Append("<TR>");
        foreach (DataColumn col in ds.Tables[0].Columns)
          if (col.ColumnMapping != MappingType.Hidden)
            sb.Append("<TD>" + UITools.HtmlEncode(row[col].ToString()) + "</TD>");
        sb.Append("</TR>");
      }
      sb.Append("</TABLE>");
      string s =sb.ToString();
      System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
      byte[] contentBytes = encoding.GetBytes(s);

      Response.Clear();
      Response.ClearContent();
      Response.ClearHeaders();
      Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
      Response.ContentType = "application/vnd.ms-excel";
      //Fix for CR 5109 - Prabhu R S
      Response.ContentEncoding = System.Text.Encoding.UTF8;
      Response.AppendHeader("Content-Disposition", "attachment;filename=\"EventLogs.xls\"; " +
                        "size=" + s.Length.ToString() + "; " +
                        "creation-date=" + DateTime.Now.ToString("R") + "; " +
                        "modification-date=" + DateTime.Now.ToString("R") + "; " +
                       "read-date=" + DateTime.Now.ToString("R"));

      Response.OutputStream.Write(
      contentBytes, 0,
      Convert.ToInt32(contentBytes.Length));
      Response.Flush();
      try { Response.End(); }
      catch { }
    }
  }
}
