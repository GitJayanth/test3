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
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
#endregion

public partial class UI_Collaborate_NewModifiedContentReport : HCPage
{
  private int TIME_OUT = 600;

  protected void Page_Load(object sender, EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      #region Load DDL_Cultures
      /// Retrieve all User Cultures, and keep only primary
      CultureList dsCultures = SessionState.User.Cultures;
      dsCultures.Sort("Type,Name");
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
      // in case of notification link, retrieve the report requested
      if ((Request["c"] != null) && (Request["f"] != null) && (Request["p"] != null))
      {
        if (Request["c"] != null) { DDL_Cultures.ClearSelection(); DDL_Cultures.Items.FindByValue(Request["c"].ToString()).Selected = true; }
        if (Request["f"] != null) { DDL_Filter.ClearSelection(); DDL_Filter.Items.FindByValue(Request["f"].ToString()).Selected = true; }
        if (Request["p"] != null) { DDL_Days.ClearSelection(); DDL_Days.Items.FindByValue(Request["p"].ToString()).Selected = true; }

        UpdateResult();
      }
      else
        dgResults.Visible = lbMessage.Visible = false;
    }
  }
  protected void btSearch_Click(object sender, EventArgs e)
  {
      if (GetSelPLs() == null)
      {
          RegisterStartupScript("hh", "<Script>alert('Please select PLs')</script>");
          return;
      }
    UpdateResult();
  }

  private void UpdateResult()
  {
    lbMessage.Visible = false;
    using (Database dbObj = Utils.GetMainDB())
    {
        /**** For PL selection ****/
        
        string selPls = GetSelPLs();
      dbObj.TimeOut = TIME_OUT;
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Report_ContentAddUpd",
        new SqlParameter("@Frequency", DDL_Days.SelectedValue.ToString()),
        new SqlParameter("@CultureCode", DDL_Cultures.SelectedValue.ToString()),
        new SqlParameter("@ItemId", -1),
        new SqlParameter("@Filter", DDL_Filter.SelectedValue.ToString()),
        new SqlParameter("@UserId", SessionState.User.Id.ToString()),
         /**** For PL selection ****/
        new SqlParameter("@PLCODE", selPls + "")))
        
      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lbMessage.Text = "[ERROR] _Report_ContentAddUpd -> " + dbObj.LastError;
          lbMessage.Visible = true;
        }
        else
        {
          #region Results
          int nbCount = ds.Tables[0].Rows.Count;
          if (nbCount > 0)
          {
            int max = Convert.ToInt32(SessionState.CacheParams["MaxContentModifiedNewDisplayedRows"].Value);

            if (nbCount > max)
            {
              Label1.Text = "Count: " + nbCount + " (maximum displayed: " + max + ")";
              for (int i = nbCount - 1; i > max; i--)
              {
                ds.Tables[0].Rows.RemoveAt(i);
              }
            }
            else
            {
              Label1.Text = "Count: " + nbCount;
            }
            dgResults.DataSource = ds.Tables[0];
            Utils.InitGridSort(ref dgResults);
            dgResults.DataBind();
            wPanelPL.Expanded = false;
            Session["SelPls"] = GetSelPLs();
            dgResults.Columns.FromKey("ClassName").Header.Caption = SessionState.ItemLevels[1].Name;
            dgResults.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate;
            using (HyperCatalog.Business.Culture c = HyperCatalog.Business.Culture.GetByKey(DDL_Cultures.SelectedValue))
            {
              if (c.Type != CultureType.Locale)
                dgResults.Columns.FromKey("WorkflowStatus").ServerOnly = true;
              else
                dgResults.Columns.FromKey("WorkflowStatus").ServerOnly = false;
            }
            lbMessage.Visible = false;
            dgResults.Visible = true;
            uwToolbar.Visible = true;
            Label1.Visible = true;
          }
          #endregion
          #region No result
          else
          {
            lbMessage.Text = "No result found";
            lbMessage.Visible = true;
            dgResults.Visible = false;
            uwToolbar.Visible = false;
            Label1.Visible = false;
          }
          #endregion
        }
      }
    }
  }

  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    #region Export
    if (btn == "export")
    {
        /************************ HO CODE **************************/
      //Utils.ExportToExcel(dgResults, "ContentReport", "ContentReport"

        /************************ GDIC CODE *************************/
        DateTime D = DateTime.UtcNow.Date;
        int Diff  = (int.Parse(DDL_Days.SelectedValue)-1);
        
        ListItemCollection lstItemCol = new ListItemCollection();
          lstItemCol.Add(new ListItem("Selected PLs:",GetSelPLs()+ ""));
          lstItemCol.Add(new ListItem("Start Date :", D.AddDays(-Diff).ToShortDateString()));
          lstItemCol.Add(new ListItem("End Date :", D.ToShortDateString()));
              //D.AddDays(-Diff)));*/
        Utils.ExportToExcelFromGrid(dgResults, "NewModifiedContentReport", "NewModifiedContentReport", Page, lstItemCol, "New/Modified Content Report");
    }
    #endregion
  }
  protected void dgResults_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    e.Row.Cells.FromKey("ClassName").Text = "[" + e.Row.Cells.FromKey("ClassId").Text + "] " + e.Row.Cells.FromKey("ClassName").Text;
    e.Row.Cells.FromKey("ItemName").Text = "<a href='javascript://' onclick=\"return OpenDetail('" + e.Row.Cells.FromKey("ItemId").Text + "', '" + DDL_Cultures.SelectedValue.ToString() + "', '" + DDL_Filter.SelectedValue.ToString() + "', '" + DDL_Days.SelectedValue.ToString() + "')\">" + e.Row.Cells.FromKey("ItemName").Text + "</a>";
  }
  /********* For PL Selection **************/
  private string GetSelPLs()
  {
      string selPls = string.Empty;
      PLList checkedPLs = PLTree.GetCheckedPLs();
      foreach (PL pl in checkedPLs)
      {

          selPls = selPls + pl.Code + ",";

      }
      if (selPls != string.Empty)
      {
          selPls = selPls.Remove(selPls.Length - 1, 1);
          return selPls;
      }
      else
          return null;

  }
  private string GetSelPLNames()
  {
      string selPlNames = string.Empty;
      PLList checkedPLs = PLTree.GetCheckedPLs();
      foreach (PL pl in checkedPLs)
      {

          selPlNames = selPlNames + pl.Name + ",";

      }
      if (selPlNames != string.Empty)
      {
          selPlNames = selPlNames.Remove(selPlNames.Length - 1, 1);
          return selPlNames;
      }
      else
          return null;
  }
}
