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
using System.Data.SqlClient;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
using System.Web.SessionState;
#endregion

public partial class UI_Acquire_BundlesInconsistencies : HCPage
{
  #region Declarations
  private string currentGroup = string.Empty;
  private string currentCountryGroup = string.Empty;
  private int bundleCount = 0;
  private int countryCount = 0;
  #endregion

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
        if ((SessionState.tPageIndexTerm != null) &&
            ((SessionState.tPageIndexTerm == "1") || (SessionState.tPageIndexTerm == "0")))
        {
          uwToolbar.Items.FromKeyButton("OnlyError").Selected = SessionState.tPageIndexTerm == "1";
        }
        else
        {
          uwToolbar.Items.FromKeyButton("OnlyError").Selected = true;
          SessionState.tPageIndexTerm = "1";
        }

        //UpdateDataView();
      }
    }
    else
    {
      lbNoresults.Text = "Sorry, you don't have the master culture assigned to your account allowing the display of this report";
      lbNoresults.CssClass = "hc_error";
      lbNoresults.Visible = true;
    }
    txtFilter.AutoPostBack = false;
    txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");
  }
    
  /**** For PL selection ****/
    protected void btGenerate_Click(object sender, EventArgs e)
   {
       if (GetSelPLs() == null)
       {
           RegisterStartupScript("hh", "<Script>alert('Please select the PLs')</script>");
           return;
       }
       UpdateDataView();
       wPanelPL.Expanded = false;
   }

  /// <summary>
  /// Display all bundles
  /// </summary>
  private void UpdateDataView()
  {
    //int retrieveOnlyError = 0;
    //if (uwToolbar.Items.FromKeyButton("OnlyError").Selected)
    //{
    //  retrieveOnlyError = 1;
    //}
      
    int retrieveOnlyError = Convert.ToInt32(SessionState.tPageIndexTerm);
    using (Database dbObj = Utils.GetMainDB())
    {
        /**** For PL selection ****/
        string selPls = GetSelPLs();
      DataSet ds = dbObj.RunSPReturnDataSet("_Item_BundlesCheckPLC",
        new SqlParameter("@UserId", SessionState.User.Id.ToString()),
        new SqlParameter("@RetrieveOnlyError", retrieveOnlyError),
        new SqlParameter("@FilterName", txtFilter.Text.Trim()),
          /**** For PL selection ****/
        new SqlParameter("@PLCode",selPls+""));
      if (dbObj.LastError == string.Empty)
      {
        if (ds.Tables[0].Rows.Count > 0)
        {
          dg.DataSource = ds.Tables[0];
          dg.DataBind();
          UITools.UpdatePLCGridHeader(dg);
          dg.Columns.FromKey("PID").Format = SessionState.User.FormatDate;
          dg.Columns.FromKey("POD").Format = SessionState.User.FormatDate;

          dg.Visible = true;
          lbNoresults.Visible = false;
        }
        else
        {
          dg.Visible = false;
          lbNoresults.Text = "No result found";
          lbNoresults.Visible = true;
          txtFilter.Text = "";
        }
      }
      else
      {
        lbNoresults.Text = "ERROR " + dbObj.LastError;
        lbNoresults.Visible = true;
        dg.Visible = false;
      }
    }
  }

  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    e.Row.Cells.FromKey("ItemName").Text = "<a href='../../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + e.Row.Cells.FromKey("ItemName").Text + "</a>";
    e.Row.Cells.FromKey("Status").Text = Utils.ColorStatus(e.Row.Cells.FromKey("Status").Text);

    string filter = txtFilter.Text.Trim();
    e.Row.Cells.FromKey("ItemName").Style.Wrap = true;
    e.Row.Cells.FromKey("ItemNumber").Style.Wrap = true;
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("ItemName");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell cSku = e.Row.Cells.FromKey("ItemNumber");
    if ((cName.Text.ToLower().IndexOf(filter.ToLower()) >= 0) || (cSku.Text.ToLower().IndexOf(filter.ToLower()) >= 0))
    {
      e.Row.Cells.FromKey("ItemName").Text = Utils.CReplace(e.Row.Cells.FromKey("ItemName").Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
      e.Row.Cells.FromKey("ItemNumber").Text = Utils.CReplace(e.Row.Cells.FromKey("ItemNumber").Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
    }

    // Bundle group
    if (e.Row.Cells.FromKey("Anomaly").Text == "")
    {
      e.Row.Cells.FromKey("CountryFlag").Text = "<img src='/hc_v4/img/flags/" + e.Row.Cells.FromKey("CountryCode").Text + ".gif'/>";
      e.Row.Cells.FromKey("CountryFlag").Title = "[" + e.Row.Cells.FromKey("CountryCode").Text + "] " + HyperCatalog.Business.Country.GetByKey(e.Row.Cells.FromKey("CountryCode").Text).Name;
      foreach (UltraGridCell cell in e.Row.Cells)
      {
        cell.Style.Font.Bold = true;
      }
    }

    if (e.Row.Cells.FromKey("Anomaly").Text == "None")
    {
      e.Row.Cells.FromKey("Anomaly").Text = string.Empty;
    }
  }

  protected void uwToolbar_ButtonClicked1(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
      
    string btn = be.Button.Key.ToLower();
    switch (btn)
    {
      case "onlyerror":
        {
          SessionState.tPageIndexTerm = (be.Button.Selected?"1":"0");

          if (GetSelPLs() == null && dg.Rows.Count > 0 && dg.Visible==true)
          {
              if (GetSelPLs() == null)
              {
                  RegisterStartupScript("hh", "<Script>alert('Please select the PLs')</script>");
                  dg.Visible = false;
                  return;
              }
          }
          if(GetSelPLs() != null && dg.Rows.Count>0)
          {
              UpdateDataView();
              dg.DisplayLayout.Pager.CurrentPageIndex = 1;
          }
          break;
        }
     case "filter":
        {
            if (GetSelPLs() == null && dg.Rows.Count > 0 && dg.Visible == true)
            {
                if (GetSelPLs() == null)
                {
                    RegisterStartupScript("hh", "<Script>alert('Please select the PLs')</script>");
                    dg.Visible = false;
                    return;
                }
            }
          UpdateDataView();
          break;
        }
      case "export":
        {
          dg.Columns.FromKey("CountryCode").Hidden = false;
          dg.Columns.FromKey("CountryFlag").Hidden = true;
          /********************** HO CODE **********************************/
            //Utils.ExportToExcel(dg, "BundlesInconsistencies", "BundlesInconsistencies");

          /************************ GDIC CODE for Export Functionality ********************************/

         
          //string stringToExport=Utils.ExportToExcelFromGrid(dg, "BundlesInconsistencies", "BundlesInconsistencies",Page);
          if (dg.Rows.Count > 0 && dg.Visible==true)
          {
              ListItemCollection lstItemCol = new ListItemCollection();
              lstItemCol.Add(new ListItem("Selected PLs:", GetSelPLs() + ""));

              Utils.ExportToExcelFromGrid(dg, "BundlesInconsistencies", "BundlesInconsistencies", Page, lstItemCol, "Bundles Report");
          }
         
          /******************* New End *******************************/

          dg.Columns.FromKey("CountryCode").Hidden = true;
          dg.Columns.FromKey("CountryFlag").Hidden = false;
          break;
        }
    }
     
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
