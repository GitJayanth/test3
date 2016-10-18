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
using HyperCatalog.UI.Tools;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of lookup
	/// </summary>
	public partial class Lookups : HCPage
	{
		#region Declarations
		public HyperCatalog.Business.LookupGroupList lookupsList;
		#endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Capabilities
      if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY) || SessionState.User.IsReadOnly)
      {
        UITools.HideToolBarSeparator(uwToolbar, "AddSep");
        UITools.HideToolBarButton(uwToolbar, "Add");
      }
      #endregion
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitVar", "<script>var webtab='" + webTab.ClientID + "'</script>");
    }

    // Display all the Input forms in the datagrid
    private void UpdateDataView()
    {
      panelTabs.Visible = false;
      panelGrid.Visible = dg.Visible = true;
			string filter = txtFilter.Text;
      if (filter.Length > 0) // filter on all groups
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        filter = " LOWER(GroupName) like '%" + cleanFilter + "%' ";
      }
      using (lookupsList = HyperCatalog.Business.LookupGroup.GetAll(filter))
      {
        if (lookupsList != null)
        {
          if (lookupsList.Count > 0)
          {
            dg.DataSource = lookupsList;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";
            dg.Visible = false;
            lbNoresults.Visible = true;
          }
        }
      }
    }
    // Display an input form selected
    private void UpdateDataEdit(string lookupGroupId)
    {
      panelGrid.Visible = false;
      LookupGroup lookupGroup = LookupGroup.GetByKey(Convert.ToInt32(lookupGroupId));
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./lookups/Lookup_Properties.aspx?lg=" + lookupGroupId;
      if (lookupGroup == null)
      { 
        webTab.Tabs[1].Visible = false;
        lbTitle.Text = "Lookup group: New";
      }
      else
      {        
        lbTitle.Text = "Lookup group: " + lookupGroup.Name;
        webTab.Tabs[1].Visible = true;
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./lookups/Lookup_Values.aspx?lg=" + lookupGroupId;
        if (lookupGroup.Values != null)
        {
          webTab.Tabs.GetTab(1).Text = " Values(" + lookupGroup.Values.Count.ToString() + ")";
        }        
      }
      panelTabs.Visible = true;
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "LookupTable", "LookupTable");
      }
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }

    }
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }
    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sGroupId = cellItem.Cell.Row.Cells.FromKey("GroupId").Text;
			
				sGroupId = Utils.CReplace(sGroupId, "<font color=red><b>", "", 1);
				sGroupId = Utils.CReplace(sGroupId, "</b></font>", "", 1);

				UpdateDataEdit(sGroupId);
			}
    }
  }
}
