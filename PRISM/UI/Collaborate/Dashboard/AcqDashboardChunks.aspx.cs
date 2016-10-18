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
using Infragistics.WebUI.UltraWebGrid;
#endregion

#region History
/*=============Modification Details====================================
      Mod#1 change ExportToExcel to AcqExportToExcel
      Description: eZ# 70252 see Tools.cs for more details
      Modfication Date:06/13/2007
      Modified by: Ramachandran
      Mod#2 change Dynamic Band and Columns in UltraWebGird
      Description: eZ# 70977
      Modfication Date:06/25/2007
      Modified by: Ramachandran*/
#endregion

public partial class UI_Collaborate_Dashboard_AcqDashboardChunks : HCPage
{


  protected void Page_Load(object sender, EventArgs e)
  {
      if (!Page.IsPostBack)
      {
          lbTitle.Text = "Details per chunk";
          lbTitle.ForeColor = System.Drawing.Color.Orange;
          lbTitle.Font.Bold = true;
          #region Load DDL_Cultures
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
              lbMessage.Text = "Your profile is not correctly set. Contact an administrator to assign the correct culture to your profile.";
              lbMessage.CssClass = "hc_error";
              lbMessage.Visible = true;
              return;
          }
          #endregion
          DisplayData();
      }
      else
      {
          //ez# 70977 - Start
          // In this code Bands and Columns are dynamically added to dgResults. This is a kind of quasi-dynamic 
          // control which created ViewState Issues. To get around this problem ViewState for the Grid is disabled and
          // DisplayData is called both on PostBack and isNotPostBack.
          DisplayData();
          //ez# 70977 - End
      }
  }

  protected void DisplayData()
  {
    using (Database dbObj = new Database(SessionState.CacheComponents["Datawarehouse_DB"].ConnectionString))
    {
      // Check the Table existence
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
        sql = "EXEC Dash_Acquisition_Chunks '" + DDL_Cultures.SelectedValue.ToString() + "', 'OrgName'";
        sql += "EXEC Dash_Acquisition_Chunks '" + DDL_Cultures.SelectedValue.ToString() + "', 'GroupName'";
        sql += "EXEC Dash_Acquisition_Chunks '" + DDL_Cultures.SelectedValue.ToString() + "', 'GBUName'";
        sql += "EXEC Dash_Acquisition_Chunks '" + DDL_Cultures.SelectedValue.ToString() + "', 'PLName'";
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sql))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError != string.Empty)
          {
            lbMessage.Text = "[ERROR] Dash_Acquisition_Chunks -> " + dbObj.LastError;
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
              //ez# 70977 - Start
              //LinkTypes are different in Crystal and Gemstone
              //Hence dynamic columns are used here
              //The Band RowStyle Color will not be appearing in the Excel 
              // Band has to be named as Table, Table1 etc 
              UltraGridBand Table = new UltraGridBand();
              Table.Key = "Org";
              Table.Indentation = 15;
              LinkTypeList linkTypeList = LinkType.GetAll();
              // the first 6 columns are static after that the LinkTypes colunms are dynamic
              string[] Key = new string[6 + linkTypeList.Count];
              Key.SetValue("Code", 0);
              Key.SetValue("Name", 1);
              Key.SetValue("NbChunks", 2);
              Key.SetValue("NbWords", 3);
              Key.SetValue("NbCultures", 4);
              Key.SetValue("NbLinks", 5);
              int i = 6;
              foreach (LinkType lType in linkTypeList)
              {
                  Key.SetValue("NbLinks" + lType.Name, i);
                  i++;
              }
              // BaseColumnName is needed for Binding the correct data
              string[] BaseColumnName = (string[])Key.Clone();
              Key.CopyTo(BaseColumnName, 0);
              i = 6;
              foreach (LinkType lType in linkTypeList)
              {
                  BaseColumnName.SetValue("NbLinks_" + lType.Name, i);
                  i++;
              }
              string[] HeaderText = (string[])Key.Clone();
              HeaderText.SetValue("Code", 0);
              HeaderText.SetValue("Organization/Group/GBU/PL", 1);
              HeaderText.SetValue("Chunks count", 2);
              HeaderText.SetValue("Words count", 3);
              HeaderText.SetValue("Cultures count", 4);
              HeaderText.SetValue("Links count", 5);
              i = 6;
              foreach (LinkType lType in linkTypeList)
              {
                  HeaderText.SetValue("Links " + lType.Name + " count", i);
                  i++;
              }
              // UltraGridCoumns are created here
              for (i = 0; i < Key.Length; i++)
              {
                  UltraGridColumn uc = new UltraGridColumn();
                  uc.Key = Key[i];
                  uc.Header.Caption = HeaderText[i];
                  uc.BaseColumnName = BaseColumnName[i];
                  if (uc.Header.Caption.Equals("Organization/Group/GBU/PL"))
                  {
                      uc.Width = Unit.Pixel(315);
                  }
                  else
                  {
                      uc.Width = Unit.Pixel(80);
                  }
                  uc.Header.Style.Wrap = true;
                  uc.CellStyle.BorderStyle = BorderStyle.Solid;
                  uc.CellStyle.BorderWidth = Unit.Pixel(1);
                  uc.CellStyle.BorderColor = System.Drawing.Color.Silver;
                  Table.Columns.Add(uc);
              }
              Table.Columns.FromKey("Code").Hidden = true;
              UltraGridBand Table1 = new UltraGridBand();
              // Band and its Columns needs to be copied seperately
              Table1.CopyFrom(Table);
              Table1.Key = "Group";
              Table1.Columns.CopyFrom(Table.Columns);
              Table1.Columns.FromKey("Code").BaseColumnName = "GroupCode";
              //Tapering the indendation - 15 
              Table1.Columns.FromKey("Name").Width = Unit.Pixel(300);
              Table1.Columns.FromKey("Name").BaseColumnName = "GroupName";
              Table1.Columns.FromKey("Code").Key = "GroupCode";
              Table1.Columns.FromKey("Name").Key = "GroupName";
              UltraGridBand Table2 = new UltraGridBand();
              Table2.CopyFrom(Table);
              Table2.Key = "GBU";
              Table2.Columns.CopyFrom(Table.Columns);
              //Tapering the indendation - 15 
              Table2.Columns.FromKey("Name").Width = Unit.Pixel(285);
              Table2.Columns.FromKey("Code").BaseColumnName = "GBUCode";
              Table2.Columns.FromKey("Name").BaseColumnName = "GBUName";
              Table2.Columns.FromKey("Code").Key = "GBUCode";
              Table2.Columns.FromKey("Name").Key = "GBUName";
              UltraGridBand Table3 = new UltraGridBand();
              Table3.CopyFrom(Table);
              Table3.Key = "PL";
              Table3.Columns.CopyFrom(Table.Columns);
              //Tapering the indendation - 15 
              Table3.Columns.FromKey("Name").Width = Unit.Pixel(270);
              Table3.Columns.FromKey("Code").BaseColumnName = "PLCode";
              Table3.Columns.FromKey("Name").BaseColumnName = "PLName";
              Table3.Columns.FromKey("Code").Key = "PLCode";
              Table3.Columns.FromKey("Name").Key = "PLName";
              // Remove the heading for other Bands
              Table1.ColHeadersVisible = ShowMarginInfo.No;
              Table2.ColHeadersVisible = ShowMarginInfo.No;
              Table3.ColHeadersVisible = ShowMarginInfo.No;
              Table.Columns.FromKey("Code").BaseColumnName = "OrgCode";
              Table.Columns.FromKey("Name").BaseColumnName = "OrgName"; 
              Table.Columns.FromKey("Code").Key = "OrgCode";
              Table.Columns.FromKey("Name").Key = "OrgName";
              //Coloring of Bands
              Table.RowStyle.BackColor = System.Drawing.Color.WhiteSmoke;
              // I believe in using named colors but unfortunately
              // htmlcolors were used in this report so maintaining the same here 
              Table1.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1F9F9");
              Table2.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FAFCF1");
              Table3.RowStyle.BackColor = System.Drawing.Color.White;
              dgResults.Bands.Clear(); // Clear the default Band
              dgResults.Bands.Add(Table);
              dgResults.Bands.Add(Table1);
              dgResults.Bands.Add(Table2);
              dgResults.Bands.Add(Table3);
              //ez# 70977 - End
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
    // eZ# 70252 -- Start
        Utils.AcqExportToExcel(dgResults, "DashboardChunks", "DashboardChunks");
    // eZ# 70252 -- End
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
