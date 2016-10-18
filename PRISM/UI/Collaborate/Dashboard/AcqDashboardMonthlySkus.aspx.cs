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
public partial class UI_Collaborate_Dashboard_AcqDashboardMonthlySkus : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      lbTitle.Text = "Product activity";
      lbTitle.ForeColor = System.Drawing.Color.Orange;
      lbTitle.Font.Bold = true;

      //----------------------Fix for Bug:70817 start-----------------
      //#region load Month
      //string mm = DateTime.Today.AddMonths(1).Month.ToString();
      //string yy = DateTime.Today.AddMonths(1).Year.ToString();
      //string d = yy + "-" + ((mm.Length==2)?mm:"0" + mm) + "-01";
      //DDL_Month.Items.Add(new ListItem(d, d));
      //for (int i=0; i<11; i++)
      //{
      //  mm = DateTime.Today.AddMonths(-i).Month.ToString();
      //  yy = DateTime.Today.AddMonths(-i).Year.ToString();
      //  d = yy + "-" + ((mm.Length == 2) ? mm : "0" + mm) + "-01";
      //  DDL_Month.Items.Add(new ListItem(d, d));
      //}
      //#endregion
      #region load Month
      DateTime mindate;
      DateTime maxdate;
      DateTime date;
      using (Database dbObj = new Database(SessionState.CacheComponents["Datawarehouse_DB"].ConnectionString))
      {
          using (System.Data.SqlClient.SqlDataReader rs = dbObj.RunSQLReturnRS("SELECT MIN(dateRef1) AS mindate, MAX(dateRef1) AS maxdate FROM Dash_AcquisitionMonthly"))
          {
              if (rs.Read())
              {
                  mindate = System.Convert.ToDateTime(rs["mindate"].ToString());
                  maxdate = System.Convert.ToDateTime(rs["maxdate"].ToString());
                  date = mindate;
                  for (int i = mindate.Year; i <= maxdate.Year; i++)
                  {
                      for (int j = ((i == mindate.Year) ? mindate.Month : 1); j <= ((i == maxdate.Year) ? maxdate.Month : 12); j++)
                      {
                          DDL_Month.Items.Add(new ListItem(date.ToShortDateString().ToString(), date.ToString()));
                          date = date.AddMonths(1);
                      }
                  }
              }
              dbObj.CloseConnection();
          }
      }
      #endregion
      //----------------------Fix for Bug:70817 end-------------------

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
        sql = "EXEC Dash_AcquisitionMonthlySKUs 'OrgName', '" + DDL_Month.SelectedValue.ToString() + "'";
        sql += "EXEC Dash_AcquisitionMonthlySKUs 'GroupName', '" + DDL_Month.SelectedValue.ToString() + "'";
        sql += "EXEC Dash_AcquisitionMonthlySKUs 'GBUName', '" + DDL_Month.SelectedValue.ToString() + "'";
        sql += "EXEC Dash_AcquisitionMonthlySKUs 'PLName', '" + DDL_Month.SelectedValue.ToString() + "'";
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sql))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError != string.Empty)
          {
            lbMessage.Text = "[ERROR] Dash_AcquisitionMonthlySKUs -> " + dbObj.LastError;
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
              Utils.InitGridSort(ref dgResults, false);
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

  protected void DDL_Month_SelectedIndexChanged(object sender, EventArgs e)
  {
    DisplayData();
  }

  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
        //eZ# 70252 -- Start
        Utils.AcqExportToExcel(dgResults, "DashboardProductActivity", "DashboardProductActivity");
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
