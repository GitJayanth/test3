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

public partial class UI_Collaborate_NPIReport : HCPage
{

  #region Declarations
  string curCulture = string.Empty;
  CultureType curCultureType = CultureType.Master;
  int MaxRows = 0;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request["filter"] != null)
    {
      txtFilter.Text = Request["filter"].ToString();
    }
    if (Request["c"] != null && Request["filter"] == null)
    {
      SessionState.tmFilterExpression = Request["c"].ToString();
    }
    if (!Page.IsPostBack)
    {
      /// Retrieve all Users Cultures
      using (CultureList dsCultures = HyperCatalog.Business.User.GetByKey(SessionState.User.Id).Cultures)
      {
        dsCultures.Sort("Type");
        foreach (HyperCatalog.Business.Culture c in dsCultures)
        {
          if (c.Country == null)
          {
            DDL_Countries.Items.Add(new ListItem(c.Name, c.Code));
          }
          else
          {
            if (DDL_Countries.Items.FindByText(c.Country.Name) == null)
            {
              DDL_Countries.Items.Add(new ListItem(c.Country.Name, c.Code));
            }
            c.Dispose();
          }
        }
        UpdateDataView();
      }

      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
    }
  }

  private void UpdateDataView()
  {
    if (SessionState.tmFilterExpression != null)
    {
      DDL_Countries.SelectedValue = SessionState.tmFilterExpression;
    }
    MaxRows = Convert.ToInt32(SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value);
    string search = txtFilter.Text;
    using (Database dbObj = Utils.GetMainDB())
    {
      using (HyperCatalog.Business.Culture c = HyperCatalog.Business.Culture.GetByKey(DDL_Countries.SelectedValue))
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("_Item_GetNPI",
          new SqlParameter("@UserId", SessionState.User.Id.ToString()),
          new SqlParameter("@CountryCode", c.CountryCode),
          new SqlParameter("@DayCountNPI", SessionState.CacheParams["DayCountNPI"].Value),
          new SqlParameter("@MaxRows", MaxRows),
          new SqlParameter("@Filter", txtFilter.Text),
          new SqlParameter("@Company", SessionState.CompanyName)))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError != string.Empty)
          {
            lbMessage.Text = "[ERROR] _Item_GetNPI -> " + dbObj.LastError;
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
          }
          else
          {
            using (HyperCatalog.Business.Culture selCul = HyperCatalog.Business.Culture.GetByKey(DDL_Countries.SelectedValue))
            {
              curCultureType = selCul.Type;
            }
            #region Results
            if (ds.Tables[0].Rows.Count > 0)
            {
              dg.DataSource = ds;
              lbMessage.Visible = false;
              Utils.InitGridSort(ref dg);
              dg.DataBind();
              dg.Visible = true;
              dg.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate;
              dg.Columns.FromKey("Class").Header.Caption = SessionState.ItemLevels[1].Name;

              if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 1)
              {
                int count = Convert.ToInt32(ds.Tables[1].Rows[0]["ProductCount"]);
                lbMessage.CssClass = "hc_success";
                if (count <= MaxRows)
                  lbMessage.Text = "Product count: " + count.ToString() + "<br />";
                else
                  lbMessage.Text = "Product count: " + count.ToString() + " (" + MaxRows.ToString() + " products are displayed)<br />Your report is returning too many rows (max = " + MaxRows.ToString() + ")<br />";
                lbMessage.Visible = true;
              }
            }
            #endregion
            #region No result
            else
            {
              lbMessage.CssClass = "hc_success";
              lbMessage.Text = "No new product found";
              lbMessage.Visible = true;
              dg.Visible = false;
            }
            #endregion
          }
        }
      }
    }
  }

  protected void DDL_Countries_SelectedIndexChanged(object sender, EventArgs e)
  {
    SessionState.tmFilterExpression = DDL_Countries.SelectedValue; 
    UpdateDataView();
  }

  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = null;
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cNumber = null;
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cClass = null;
    if (e.Row.Cells.FromKey("Name") != null)
      cName = e.Row.Cells.FromKey("Name");
    if (e.Row.Cells.FromKey("PN") != null)
      cNumber = e.Row.Cells.FromKey("PN");
    if (e.Row.Cells.FromKey("Class") != null)
      cClass = e.Row.Cells.FromKey("Class");

    #region Search colorization
    if (cName != null && cNumber != null && cClass != null)
    {
      string search = txtFilter.Text.Trim();
      string itemName = cName.Text.ToLower();
      string itemNumber = cNumber.Text.ToLower();
      string className = cClass.Text.ToLower();

      if (search.Length > 0)
      {
        if ((itemName.IndexOf(search.ToLower())) >= 0)
        {
          cName.Text = Utils.CReplace(cName.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
        }
        if ((itemNumber.IndexOf(search.ToLower())) >= 0)
        {
          cNumber.Text = Utils.CReplace(cNumber.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
        }
        if ((className.IndexOf(search.ToLower())) >= 0)
        {
          cClass.Text = Utils.CReplace(cClass.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
        }
      }
    }
    #endregion

    #region Level name
    if (e.Row.Cells.FromKey("Level") != null && e.Row.Cells.FromKey("LevelId") != null)
      e.Row.Cells.FromKey("Level").Text = "[" + e.Row.Cells.FromKey("LevelId") + "] " + e.Row.Cells.FromKey("Level");
    #endregion

    #region Product name
    if (cName != null)
    {
      if (curCultureType == HyperCatalog.Business.CultureType.Locale)
        cName.Text = "<a href='../../redirect.aspx?i=" + e.Row.Cells.FromKey("ItemId").Text + "&c=" + DDL_Countries.SelectedValue + "&p=UI/Globalize/qdetranslate.aspx' target='_BLANK'\">" + cName.Text + "</a>";
      else
        cName.Text = "<a href='../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + cName.Text + "</a>";
    }
    #endregion
  }

  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
        /************************ HO CODE **************************/
      //Utils.ExportToExcel(dg, "NPIReport", "NPIReport");
        
        /************************ GDIC CODE *************************/
        ListItemCollection lstItemCol = new ListItemCollection();
        lstItemCol.Add(new ListItem("Selected Country :", DDL_Countries.SelectedItem + ""));
        Utils.ExportToExcelFromGrid(dg, "NPIReport", "NPIReport", Page, lstItemCol, "NPI Report");
    }
    #endregion
  }
}
