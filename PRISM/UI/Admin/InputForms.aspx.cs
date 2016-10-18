#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebTab;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display input form list
	/// </summary>
	public partial class InputForms : HCPage
	{
		#region Declarations


		public HyperCatalog.Business.InputFormList ifList;
    protected bool bHeritage;
    protected DataSet levelsDS, subLevelsDS;

		#endregion


    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY))
      {
        UITools.HideToolBarSeparator(uwToolbar, "AddSep");
        UITools.HideToolBarButton(uwToolbar, "Add");
      }

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
      else
      {
        if (Request["redirectId"] != null && Request["redirectId"].ToString() != string.Empty)
        {
          UpdateDataEdit(Request["redirectId"].ToString());
        }
      }

      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
    }
    
    // Display all the Input forms in the datagrid
    private void UpdateDataView()
    {
      Trace.Warn("filter :" + txtFilter.Text);
      panelTabs.Visible = false;
      panelGrid.Visible = dg.Visible = true;

			string filter = string.Empty;
			if (txtFilter.Text.Length > 0)
			{
        string cleanFilter = txtFilter.Text.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

				filter += " (LOWER(Name) LIKE '%" + cleanFilter+"%'";
				filter += " OR LOWER(ShortName) LIKE '%" + cleanFilter+"%'";
				filter += " OR LOWER(Description) LIKE '%" + cleanFilter+"%')";
			}
      using (ifList = HyperCatalog.Business.InputForm.GetAll(filter))
      {
        if (ifList != null)
        {
          if (ifList.Count > 0)
          {
            dg.DataSource = ifList;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your filter (" + txtFilter.Text + ")";

            dg.Visible = false;
            lbNoresults.Visible = true;
          }

          lbTitle.Text = "Input forms (" + ifList.Count + ")";
        }
      }
    }

    // Display an input form selected
    protected void UpdateDataEdit(string inputFormId)
    {
      Tab tbProperties = webTab.Tabs.GetTab(0);
      Tab tbContainers = webTab.Tabs.GetTab(1);
      Tab tbUsage = webTab.Tabs.GetTab(2);
      Tab tbPLs = webTab.Tabs.GetTab(3);
      Tab tbDV = webTab.Tabs.GetTab(4);
      panelGrid.Visible = false;
      InputForm inputForm = InputForm.GetByKey(Convert.ToInt32(inputFormId));
      tbProperties.ContentPane.TargetUrl = "./inputforms/inputforms_properties.aspx?i=" + inputFormId;
      if (inputForm == null)
      {
        tbContainers.Visible = tbUsage.Visible = tbPLs.Visible = tbDV.Visible = false;
        lbTitle.Text = "Input form: New";
      }
      else
      {        
        lbTitle.Text = "Input form: " + inputForm.Name;
        tbContainers.Visible = tbUsage.Visible = tbPLs.Visible = tbDV.Visible = true;
        if (tbContainers.Text.IndexOf(" (") > 0)
        {
          tbContainers.Text = tbContainers.Text.Substring(0, tbContainers.Text.IndexOf(" ("));
        }
        if (tbUsage.Text.IndexOf(" (") > 0)
        {
          tbUsage.Text = tbUsage.Text.Substring(0, tbUsage.Text.IndexOf(" ("));
        }
        if (tbPLs.Text.IndexOf(" (") > 0)
        {
          tbPLs.Text = tbPLs.Text.Substring(0, tbPLs.Text.IndexOf(" ("));
        }
        tbContainers.ContentPane.TargetUrl = "./inputforms/inputforms_containers.aspx?i=" + inputFormId;
        if (inputForm.Containers!=null)
        {
          tbContainers.Text = tbContainers.Text + " (" + inputForm.Containers.Count.ToString() + ")";
        }        
        tbUsage.ContentPane.TargetUrl = "./inputforms/inputforms_usage.aspx?i=" + inputFormId;
        if (inputForm.Items != null)
          tbUsage.Text = tbUsage.Text + " (" + inputForm.Items.Count.ToString() + ")";
        tbPLs.ContentPane.TargetUrl = "./inputforms/inputforms_productlines.aspx?i=" + inputFormId;
        if (inputForm.PLs != null)
        tbPLs.Text = tbPLs.Text + " (" + inputForm.PLs.Count.ToString() + ")";
        tbDV.ContentPane.TargetUrl = "./inputforms/inputforms_detailledview.aspx?i=" + inputFormId;
        
      }
      panelTabs.Visible = true;
      webTab.SelectedTabIndex = 0;
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      Trace.Warn(btn);
      if (btn == "filter")
      {
        UpdateDataView();
      }
      if (btn == "export")
      {
        UpdateDataView();
        Utils.ExportToExcelFromGrid(dg, "Input Forms", "Input Forms", Page,null, "Input Forms Report");
      }
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }

    }

    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      if (txtFilter.Text.Length > 0)
      {
        Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
        UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
      }
		}

    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sInputFormId = cellItem.Cell.Row.Cells.FromKey("Id").Text;
			
				sInputFormId = Utils.CReplace(sInputFormId, "<font color=red><b>", "", 1);
				sInputFormId = Utils.CReplace(sInputFormId, "</b></font>", "", 1);

				UpdateDataEdit(sInputFormId);
			}
    }
  }
}
