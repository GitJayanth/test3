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
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Shared;
using System.Data.SqlClient;
#endregion

public partial class UI_Globalize_TRReports_Report : HCPage
{
  #region Declarations
  private string r;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    r = Request["r"].ToString();
    if (Request["filter"] != null)
    {
      txtFilter.Text = Request["filter"].ToString();
    }
    if (!Page.IsPostBack)
    {
      UpdateDataView();
    }
    else
    {
      // action after delete TR in TR properties window 
      if (Request["action"] != null && Request["action"].ToString().ToLower() == "reload")
      {
        UpdateDataView();
      }
    }
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
  }

  private void UpdateDataView()
  {
    string search = txtFilter.Text;
    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("_Project_TRReports",
        new SqlParameter("@UserId", SessionState.User.Id),
            new SqlParameter("@Report", Convert.ToInt32(r)),
        new SqlParameter("@Company", SessionState.CompanyName)))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Project_TRReports -> " + dbObj.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        else
        {
          #region Results
          if (ds.Tables[0].Rows.Count > 0)
          {
            dg.DataSource = ds;
            lbMessage.Visible = false;
            Utils.InitGridSort(ref dg);
            dg.DataBind();
            dg.Visible = true;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No TR to display";
            lbMessage.Visible = true;
            dg.Visible = false;
          }
          #endregion
        }
        dg.Columns.FromKey("BOT").Format = SessionState.User.FormatDate;
      dg.Columns.FromKey("TRId").Hidden = r != "2";
        dg.Columns.FromKey("InstantTR").Hidden = r != "2";
        dg.Columns.FromKey("Type").Hidden = r != "2";
      }
    }
  }
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    e.Row.Cells.FromKey("Level").Text = "[" + e.Row.Cells.FromKey("LevelId").Text + "] " + e.Row.Cells.FromKey("Level").Text;
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("Name");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cId = e.Row.Cells.FromKey("TRId");

    #region Search colorization
    string search = txtFilter.Text.Trim();
    string cIdText = cId.Text;
    if (search != string.Empty)
    {
        if (cId.Text != null)
        {
      if ((cId.Text.ToLower().IndexOf(search.ToLower())) >= 0 || (cName.Text.ToLower().IndexOf(search.ToLower())) >= 0)
      {
        cName.Text = Utils.CReplace(cName.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
        cId.Text = Utils.CReplace(cId.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
      }
      else
      {
        e.Row.Delete();
      }}
    }
    #endregion
    
    if (r == "2")
    {
      cId.Text = "<a href='javascript://' onclick=\"EditTR(" + cIdText + ")\">" + cId.Text + "</a>";
    }
    else
    {
      cName.Text = "<a href='../../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + cName.Text + "</a>";
    }
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Search
    if (btn == "search")
    {
      UpdateDataView();
    }
    #endregion
    #region Export
    if (btn == "export")
    {
      Utils.ExportToExcel(dg, "TRReports", "TRReports");
    }
    #endregion
  }
}
