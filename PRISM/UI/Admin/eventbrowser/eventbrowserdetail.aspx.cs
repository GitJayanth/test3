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
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperCatalog.Business.EventBrowser;
#endregion

#region History
/*=============Modification Details====================================
      
      Mod#1 Converting the Time to GMT
      Description: QC791 
      Modfication Date:12/10/2007
      Modified by: Jothi */


#endregion
public partial class eventbrowserdetail : HCPage
{
  private System.Guid jobId;
  
  #region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    //
    // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
    //
    InitializeComponent();
    
    base.OnInit(e);
  }
		
  /// <summary>
  /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
  /// le contenu de cette méthode avec l'éditeur de code.
  /// </summary>
  private void InitializeComponent()
  {
    this.eventsGrid.InitializeDataSource += new Infragistics.WebUI.UltraWebGrid.InitializeDataSourceEventHandler(this.eventsGrid_InitializeDataSource);
  }
  #endregion

  protected HyperCatalog.Business.ApplicationComponentCollection appComponents = HyperCatalog.Business.ApplicationSettings.Components;
  protected void Page_Load(object sender, System.EventArgs e)
  {

    try
    {
      jobId = new Guid(Request["e"]);
      eventsGrid.DisplayLayout.EnableInternalRowsManagement = false;
      eventsGrid.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
      eventsGrid.DisplayLayout.LoadOnDemand = Infragistics.WebUI.UltraWebGrid.LoadOnDemand.Xml;
      Utils.InitGridSort(ref eventsGrid, false);
    }
    catch (Exception ex)
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('Error: " + UITools.CleanJSString(ex.ToString()) + "'");
    }
  }


  private void eventsGrid_InitializeDataSource(object sender, Infragistics.WebUI.UltraWebGrid.UltraGridEventArgs e)
  {
    eventsGrid.DataSource = GetEvent();
  }
  private HyperCatalog.Business.EventBrowser.EventCollection GetEvent()
  {
    jobId = new Guid(Request["e"]);
    if (Cache[jobId.ToString()] == null)
    {
      using (HyperCatalog.Business.EventBrowser.Job j = HyperCatalog.Business.EventBrowser.Job.GetByKey(jobId))
      {
        if (j != null)
        {
          propertiesTitleLbl.Text = appComponents[j.AppComponentId].Name + " [" + displayStartDate(j) + "]";
          j.Events.ResetFilter();
          Utils.InitGridSort(ref eventsGrid, false);
          eventsGrid.Columns.FromKey("Date").Format = HyperCatalog.Shared.SessionState.User.FormatDate + ' ' + HyperCatalog.Shared.SessionState.User.FormatTime;
          eventsGrid.DisplayLayout.RowsRange = j.Events.Count;
          txtStartValue.Text = displayStartDate(j);
          txtUserValue.Text = j.UserFullName;
          txtDurationValue.Text = displayDuration(j.Duration);
          txtResultValue.Text = displaySuccess(j.Status);
          txtComponentValue.Text = j.AppComponentName;
          HttpContext.Current.Cache.Add(jobId.ToString(), j.Events, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration,
            System.Web.Caching.CacheItemPriority.Low, null);
          Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "totalrows", "var totalRows = " + j.Events.Count.ToString() + ";", true);
        }
      }      
    }
    if (Cache[jobId.ToString()] == null)
    {
      UITools.JsCloseWin();
      return null;
    }
    else
    {
      return (HyperCatalog.Business.EventBrowser.EventCollection)Cache[jobId.ToString()];
    }
  }

  public static string displayStartDate(Job job)
  {
    if (job != null)
    {
      Event startDate = job.StartEvent;
      // Fix for QC791 - Jothi.
      if (startDate != null)
          return startDate.DateTime.ToString();
        //return HyperCatalog.Shared.SessionState.User.GMTTimeZone.ToLocalTime(startDate.DateTime).ToString();
    }
    return "-";
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
      result += "0." + duration.Milliseconds + "s";

    return result;
  }

  public static string displaySuccess(JobStatus status)
  {
    switch (status)
    {
      case JobStatus.STILLRUNNING:
        return "<font style=\"color:orange;\">Running</font>";
      case JobStatus.SUCCESS:
        return "<font style=\"color:green;\">Success</font>";
      case JobStatus.UNIQUE:
        return String.Empty;
      default:
        return "<font style=\"color:red;\">Failure</font>";
    }
  }
  
  public static string severityImgSrc(Severities severity)
  {
    string commonImgPath = "/hc_v4/img/";
    switch (severity)
    {
      case Severities.BEGIN:
        return commonImgPath + "ed_begin.gif";
      case Severities.SUCCESS:
        return commonImgPath + "ed_end.gif";
      case Severities.FAILURE:
        return commonImgPath + "ed_error.gif";
      case Severities.WARNING:
        return commonImgPath + "ed_warning.gif";
      case Severities.ERROR:
        return commonImgPath + "ed_error2.gif";
      default:
        return commonImgPath + "ed_information.gif";
    }
  }

  protected void eventsGrid_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
  }
  protected void eventsToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    switch (be.Button.Key)
    {
      case "Refresh":
        if (Cache[jobId.ToString()] != null) { ((HyperCatalog.Business.EventBrowser.EventCollection)Cache[jobId.ToString()]).Dispose(); Cache[jobId.ToString()] = null; }
        break;
      case "Export":
        Utils.ExportToExcel(eventsGrid, "Events", "Events");
        break;
    }

  }
}
