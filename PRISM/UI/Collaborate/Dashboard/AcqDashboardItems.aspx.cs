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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

#region History
/*=============Modification Details====================================
      Mod#1 change ExportToExcel to AcqExportToExcel
      Description: eZ# 70252 see Tools.cs for more details
      Modfication Date:06/13/2007
      Modified by: Ramachandran*/
#endregion
public partial class UI_Collaborate_Dashboard_AcqDashboardItems : HCPage
{

  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      lbTitle.Text = "Details per product";
      lbTitle.ForeColor = System.Drawing.Color.Orange;
      lbTitle.Font.Bold = true;
      #region Load DDL_Cultures
      /// Retrieve all User Cultures, and keep only primary
      CultureList dsCultures = SessionState.User.Cultures;
      dsCultures.Sort("Type");
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
        return;
      }
      #endregion
      DisplayData();
    }
  }

  protected void DisplayData()
  {
    using (Database dbObj = new Database(SessionState.CacheComponents["Datawarehouse_DB"].ConnectionString))
    {
      // Check the table existence
      string sql = "SELECT TOP 1 * FROM Dash_Acquisition WITH (NOLOCK)";
      dbObj.RunSQL(sql);
      if (dbObj.LastError != string.Empty)
      {
        lbMessage.Text = "[ERROR] Datawarehouse database is not accessible, please contact support - " + dbObj.LastError;
        lbMessage.Visible = true;
        dgResults.Visible = false;
        dbObj.CloseConnection();
      }
      else
      {
        sql = "EXEC Dash_Acquisition_Items '" + DDL_Cultures.SelectedValue.ToString() + "', 'OrgName'";
        sql += "EXEC Dash_Acquisition_Items '" + DDL_Cultures.SelectedValue.ToString() + "', 'GroupName'";
        sql += "EXEC Dash_Acquisition_Items '" + DDL_Cultures.SelectedValue.ToString() + "', 'GBUName'";
        sql += "EXEC Dash_Acquisition_Items '" + DDL_Cultures.SelectedValue.ToString() + "', 'PLName'";
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sql))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError != string.Empty)
          {
            lbMessage.Text = "[ERROR] Dash_Acquisition_Items -> " + dbObj.LastError;
            lbMessage.Visible = true;
          }
          else
          {
            #region Results
            if (ds.Tables[0].Rows.Count > 0)
            {
              DataColumn parentColumn;
              DataColumn childColumn;
              DataRelation relation;
              try
              {
                parentColumn = ds.Tables[0].Columns["OrgCode"];
                childColumn = ds.Tables[1].Columns["OrgCode"];
                relation = new System.Data.DataRelation("OrgCode", parentColumn, childColumn);
                ds.Relations.Add(relation);
                parentColumn = ds.Tables[1].Columns["GroupCode"];
                childColumn = ds.Tables[2].Columns["GroupCode"];
                relation = new System.Data.DataRelation("GroupCode", parentColumn, childColumn);
                ds.Relations.Add(relation);
                parentColumn = ds.Tables[2].Columns["GBUCode"];
                childColumn = ds.Tables[3].Columns["GBUCode"];
                relation = new System.Data.DataRelation("GBUCode", parentColumn, childColumn);
                ds.Relations.Add(relation);
               
              }
              catch (Exception ex)
              {
                lbMessage.Text = ex.ToString();
                lbMessage.Visible = true;
              }
              dgResults.DataSource = ds.Tables[0].DefaultView;
              dgResults.DataBind();
              Utils.InitGridSort(ref dgResults, true);
              lbMessage.Visible = false;
              dgResults.Visible = true;
            }
            #endregion
            #region No result
            else
            {
              lbMessage.Text = "No result found";
              lbMessage.Visible = true;
              dgResults.Visible = false;
            }
            #endregion
          }
        }
      }
    }
  }
  protected void DDL_Cultures_SelectedIndexChanged(object sender, EventArgs e)
  {
    SessionState.Culture.Code = DDL_Cultures.SelectedValue;
    DisplayData();
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
        //eZ# 70252 -- Start
        Utils.AcqExportToExcel(dgResults, "DashboardItems", "DashboardItems");
        //eZ# 70252 -- End
    }
    if (btn == "expand")
    {
      if (be.Button.Text == "Expand all")
      {
        dgResults.ExpandAll(true);
        be.Button.Text = "Collapse all";
      }
      else
      {
        dgResults.ExpandAll(false);
        be.Button.Text = "Expand all";
      }
    }
    #endregion

  }
  protected void dgResults_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    if (e.Row.Band.Key == "Table")
    {
      e.Row.Cells.FromKey("OrgName").Text = "[" + e.Row.Cells.FromKey("OrgCode").Text + "] " + e.Row.Cells.FromKey("OrgName").Text;
    }
    if (e.Row.Band.Key == "Table1")
    {
      e.Row.Cells.FromKey("GroupName").Text = "[" + e.Row.Cells.FromKey("GroupCode").Text + "] " + e.Row.Cells.FromKey("GroupName").Text;
    }
    if (e.Row.Band.Key == "Table2")
    {
      e.Row.Cells.FromKey("GBUName").Text = "[" + e.Row.Cells.FromKey("GBUCode").Text + "] " + e.Row.Cells.FromKey("GBUName").Text;
    }
    if (e.Row.Band.Key == "Table3")
    {
      e.Row.Cells.FromKey("PLName").Text = "[" + e.Row.Cells.FromKey("PLCode").Text + "] " + e.Row.Cells.FromKey("PLName").Text;
    }
  }
}
