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
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

/// <summary>
/// Display list of input forms containing the selected container
///		--> Return to the list of containers
///		--> Add container dependency
///		--> Delete several container dependencies
///		--> export in Excel
///		--> Filter on Name and Description fields
/// </summary>
public partial class container_dependencies : HCPage
{
	#region Declarations

	private int featureContainerId = -1;
	#endregion

	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		InitializeComponent();
		txtFilter.AutoPostBack = false;
		txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
		base.OnInit(e);
	}
		
	/// <summary>
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{    
		this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
		this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

	}
	#endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
    {
      UITools.HideToolBarSeparator(uwToolbar, "AddSep");
      UITools.HideToolBarButton(uwToolbar, "Add");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }

    try
    {
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");

      if (Request["c"] != null)
      {
        featureContainerId = Convert.ToInt32(Request["c"]);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "init", "<script>document.getElementById('hFeatureContainerId').Value=" + featureContainerId.ToString() + "</script>");
      }

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }
  
    
	void UpdateDataView()
	{
		string filter = string.Empty;
		filter += " FeatureContainerId="+featureContainerId;

		// filter
		if (txtFilter.Text.Length > 0)
		{
			string cleanFilter = txtFilter.Text.Replace("'", "''").ToLower();
			cleanFilter = cleanFilter.Replace("[", "[[]");
			cleanFilter = cleanFilter.Replace("_", "[_]");
			cleanFilter = cleanFilter.Replace("%", "[%]");

			filter += " AND (LOWER(TC.Tag) like '%" +  cleanFilter + "%'";
			filter += " OR LOWER(TC.ContainerName) like '%" +  cleanFilter + "%'";
			filter += " OR LOWER(TC.Definition) like '%" +  cleanFilter + "%')";
		}

		// Get all container dependencies
    using (ContainerDependencyList dependencies = ContainerDependency.GetAll(filter))
    {

      if (dependencies != null)
      {
        if (dependencies.Count > 0)
        {
          dg.DataSource = dependencies;
          Utils.InitGridSort(ref dg);
          dg.DataBind();

          UITools.RefreshTab(this.Page, "ContainerDependencies", dg.Rows.Count);

          if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
          {
            UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
            UITools.ShowToolBarButton(uwToolbar, "Delete");
            uwToolbar.Items.FromKeyButton("Delete").DefaultStyle.Width = Unit.Pixel(120);
          }

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

        // refresh tab count
        UITools.RefreshTab(Page, "ContainerDependencies", dependencies.Count);
      }
    }
	}

	private void Delete()
	{
		for (int i=0; i<dg.Rows.Count; i++)
		{
			TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
			CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
			if (cb.Checked)
			{
				int techspecContainerId = -1;
				string sTechspecContainerId = dg.Rows[i].Cells.FromKey("Id").Value.ToString();
				if (sTechspecContainerId.Length > 0)
					techspecContainerId = Convert.ToInt32(sTechspecContainerId);

				ContainerDependency dependency = ContainerDependency.GetByKey(featureContainerId, techspecContainerId);
				if (!dependency.Delete(HyperCatalog.Shared.SessionState.User.Id))
				{
					lbError.CssClass = "hc_error";
					lbError.Text = ContainerDependency.LastError;
					lbError.Visible = true;
					break;
				}
			}
		}
		UpdateDataView();
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
if (btn == "export")
		{
			HyperCatalog.Business.Container container = HyperCatalog.Business.Container.GetByKey(featureContainerId);
			if (container != null)
				Utils.ExportToExcel(dg, "["+container.Tag+"] "+container.Name+"- Dependencies", "["+container.Tag+"] "+container.Name+"- Dependencies");
		}
		else if (btn == "delete")
		{
			Delete();
		}
	}

	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		// Update link
		e.Row.Cells.FromKey("Tag").Text = "<a href='javascript://' onclick=\"OpenPopupInputForms("+e.Row.Cells.FromKey("Id").ToString()+")\">"+e.Row.Cells.FromKey("Tag").Text+"</a>";
        
		// Update filter
		if (txtFilter.Text.Length > 0)
		{
			Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
			UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
		}
	}
}

