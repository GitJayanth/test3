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

public partial class UI_Globalize_ProjectReports_ProjectReport : HCPage
{
  #region Declarations
  private string r;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    r = Request["r"].ToString();
    lbTitle.ForeColor = System.Drawing.Color.Orange;
    lbTitle.Font.Bold = true;
    lbTitle.Visible = true;
    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 21/May/09
    switch (r)
    {
      case "1": lbTitle.Text = "Projects with future Beginning of Regionalization (BOR) Date";
        break;
    case "2": lbTitle.Text = "Projects with future End Of Regionalization (EOR) Date";
        break;
    }
    if (SessionState.User.HasCultureInScope(HyperCatalog.Business.Culture.GetMasterCulture().Code))
    {
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    else
    {
      lbMessage.Text = "Sorry, you don't have the master culture assigned to your account allowing the display of this report";
      lbMessage.CssClass = "hc_error";
      lbMessage.Visible = true;
    }
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");
  }

  private void UpdateDataView()
  {
    string search = txtFilter.Text;
    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("_Project_Reports",
        new SqlParameter("@UserId", SessionState.User.Id),
        new SqlParameter("@Report", Convert.ToInt32(r)),
        new SqlParameter("@Company", SessionState.CompanyName)))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Project_Reports -> " + dbObj.LastError;
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
            if (r == "1")
            {
              dg.Columns.FromKey("ProjectDate").Header.Caption = "BOR";
              dg.Columns.FromKey("IsInitialized").Hidden = false;
            }
            if (r == "2")
            {
              dg.Columns.FromKey("ProjectDate").Header.Caption = "EOR";
              dg.Columns.FromKey("IsInitialized").Hidden = true;
            }
            dg.Columns.FromKey("ProjectDate").Format = SessionState.User.FormatDate;
            dg.Visible = true;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No Project to display";
            lbMessage.Visible = true;
            dg.Visible = lbTitle.Visible = false;

          }
          #endregion
        }
        dg.Columns.FromKey("ProjectDate").Format = SessionState.User.FormatDate;
        dg.Columns.FromKey("Class").Header.Caption = SessionState.ItemLevels[1].Name;
      }
    }
  }
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    e.Row.Cells.FromKey("Level").Text = "[" + e.Row.Cells.FromKey("LevelId").Text + "] " + e.Row.Cells.FromKey("Level").Text;
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("Name");
    //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 21/May/09
    cName.Text = "<a href='../../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "&c=" + e.Row.Cells.FromKey("CultureCode").Text + "' target='_BLANK'\">" + cName.Text + "</a>";
    #region Search colorization
    string search = txtFilter.Text.Trim();
    if (search != string.Empty)
    {
      if ((cName.Text.ToLower().IndexOf(search.ToLower())) >= 0)
      {
        cName.Text = Utils.CReplace(cName.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
      }
      else
      {
        e.Row.Delete();
      }
    }
    #endregion
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
        /***************** HO CODE ****************/
      //Utils.ExportToExcel(dg, "ProjectReports", "ProjectReports");
        
      /******************* GDIC CODE ********************/
        Utils.ExportToExcelFromGrid(dg, "ProjectReports", "ProjectReports", Page, null,"Project reports");
    }
    #endregion
  }
}
