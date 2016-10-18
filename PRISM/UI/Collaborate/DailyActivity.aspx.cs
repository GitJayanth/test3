#region Uses
using System;
using System.IO;
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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources;

using Infragistics.UltraChart.Core.Layers;
using Infragistics.UltraChart.Core;
using Infragistics.UltraChart.Core.ColorModel;
using Infragistics.UltraChart.Data;
using Infragistics.UltraChart.Core.Primitives;
#endregion

public partial class UI_Collaborate_DailyActivities : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      #region Load Cultures list
      /// Retrieve all Users Cultures
      using (CultureList dsCultures = HyperCatalog.Business.Culture.GetAll())
      {
          //Fix start for bug:70400 
          //dsCultures.Sort("Type");
          dsCultures.Sort("Name");
          //Fix end 

        if (dsCultures.Count > 0)
        {
          DDL_Cultures.DataSource = dsCultures;
          DDL_Cultures.DataBind();
          DDL_Cultures.Items.FindByValue(SessionState.Culture.Code).Selected = true;
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
        Display30Days();
      }
    }
  }

  /// <summary>
  /// Display grid for 30 days
  /// </summary>
  protected void Display30Days()
  {
    dgByDay.Visible = dgByUser.Visible = false;
    lbByDay.Visible = lbByUser.Visible = false;
    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Report_DailyActivity_1_30Days", new SqlParameter("@CultureCode", DDL_Cultures.SelectedValue.ToString())))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Report_DailyActivity_1_30Days -> " + dbObj.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        else
        {
          #region Results
          if (ds.Tables[0].Rows.Count > 0)
          {
            chart30Days.DataSource = ds.Tables[0];
            chart30Days.DataBind();
            lbMessage.Visible = false;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No result found in this culture " + DDL_Cultures.SelectedItem.ToString();
            lbMessage.Visible = true;
          }
          #endregion
        }
      }
    }
  }

  /// <summary>
  /// Display grid by user
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void DisplayByUser(object sender, EventArgs e)
  {
    Display30Days();
    string u = ((CellItem)((LinkButton)sender).NamingContainer).Cell.Row.Cells.FromKey("UserId").Text;
    string d = ((CellItem)((LinkButton)sender).NamingContainer).Cell.Row.Cells.FromKey("Date").Text;
    using (HyperCatalog.Business.User user = HyperCatalog.Business.User.GetByKey(Convert.ToInt32(u)))
    {
      lbByUser.Text = "<b>Activity for the user " + user.FullName + " for " + DateTime.Parse(d).ToString(SessionState.User.FormatDate) + "</b>";
    }
    lbByDay.Visible = lbByUser.Visible = true;
    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Report_DailyActivity_3_ByDayByUser",
        new SqlParameter("@Date", DateTime.Parse(d)),
        new SqlParameter("@UserId", u),
        new SqlParameter("@CultureCode", DDL_Cultures.SelectedValue.ToString())))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Report_DailyActivity_3_ByDayByUser -> " + dbObj.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        else
        {
          #region Results
          if (ds.Tables[0].Rows.Count > 0)
          {
            dgByUser.DataSource = ds;
            dgByUser.DataBind();
            Utils.InitGridSort(ref dgByUser, false);
            dgByUser.Visible = true;
            dgByDay.Visible = true;
            lbMessage.Visible = false;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No result found in this culture " + DDL_Cultures.SelectedItem.ToString() + " and at this date ";
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
            dgByUser.Visible = false;
          }
          #endregion
        }
      }
    }
  }

  protected void DDL_Cultures_SelectedIndexChanged(object sender, EventArgs e)
  {
    Display30Days();
  }

  protected void dgByUser_InitializeRow(object sender, RowEventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("ItemName");
    cName.Text = "<a href='../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + cName.Text + "</a>";
    string altStatus = string.Empty;
    string status = e.Row.Cells.FromKey("Status").Text;
    switch (status)
    {
      case "M": altStatus = "Missing"; break;
      //case "R": altStatus = "Rejected"; break; --Commented for alternate for CR 5096
      case "D": altStatus = "Draft"; break;
      case "F": altStatus = "Final"; break;
      default: altStatus = string.Empty; break;
    }
    e.Row.Cells.FromKey("Status").Text = "<img src='/hc_v4/img/S" + status + ".gif' align='center' valign='middle' alt='" + altStatus + "'/>";
    e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(e.Row.Cells.FromKey("Value").Text);
  }
  protected void ShowByDay(DateTime d)
  {
    Display30Days();
    dgByUser.Visible = lbByUser.Visible = false;
    lbByDay.Text = "<b>Activity by user for " + d.ToString(SessionState.User.FormatDate) + "</b>";
    lbByDay.Visible = true;
    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Report_DailyActivity_2_ByDay",
        new SqlParameter("@Date", d),
        new SqlParameter("@CultureCode", DDL_Cultures.SelectedValue.ToString())))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Report_DailyActivity_2_ByDay -> " + dbObj.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        else
        {
          #region Results
          if (ds.Tables[0].Rows.Count > 0)
          {
            dgByDay.DataSource = ds;
            dgByDay.DataBind();
            dgByDay.Visible = true;
            lbMessage.Visible = false;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No result found in this culture " + DDL_Cultures.SelectedItem.ToString() + " and at this date " + d.ToShortDateString();
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
            dgByDay.Visible = false;
          }
          #endregion
        }
      }
    }
  }
  protected void chart30Days_ChartDataClicked(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
  {
    ShowByDay(DateTime.Parse(e.RowLabel));
  }
  protected void dgByUser_SortColumn(object sender, SortColumnEventArgs e)
  {
    Display30Days();
    dgByDay.Visible = dgByUser.Visible = true;
    lbByDay.Visible = lbByUser.Visible = true;
  }
  protected void dgByUser_GroupColumn(object sender, ColumnEventArgs e)
  {
    Display30Days();
    dgByDay.Visible = dgByUser.Visible = true;
    lbByDay.Visible = lbByUser.Visible = true;
  }
  protected void dgByUser_UnGroupColumn(object sender, ColumnEventArgs e)
  {
    Display30Days();
    dgByDay.Visible = dgByUser.Visible = true;
    lbByDay.Visible = lbByUser.Visible = true;
  }
}
