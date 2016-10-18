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

public partial class UI_Collaborate_PLCReport : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
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
        using (DataSet ds = dbObj.RunSPReturnDataSet("_Item_GetAllProductsWithoutPLC", new SqlParameter("@UserId", SessionState.User.Id.ToString()),
                                                      new SqlParameter("@Company", SessionState.CompanyName)))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Item_GetAllProductsWithoutPLC -> " + dbObj.LastError;
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
            dg.Columns.FromKey("Class").Header.Caption = SessionState.ItemLevels[1].Name;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No product without plc found";
            lbMessage.Visible = true;
            dg.Visible = false;
          }
          #endregion
        }
      }
    }
  }
  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("Name");
    cName.Text = "<a href='../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + cName.Text + "</a>";
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
    #region Search
    if (btn == "search")
    {
      UpdateDataView();
    }
    #endregion
    #region Export
    if (btn == "export")
    {
        /************************ HO CODE **************************/
      //Utils.ExportToExcel(dg, "PLCMissingReport", "PLCMissingReport");
      /************************ GDIC CODE *************************/
        Utils.ExportToExcelFromGrid(dg, "PLCMissingReport", "PLCMissingReport", Page, null,"Missing PLC report");
    }
    #endregion
  }
}

