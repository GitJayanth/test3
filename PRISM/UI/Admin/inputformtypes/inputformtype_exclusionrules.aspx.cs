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
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
#endregion

/// <summary>
/// Display exclusion rules for this input form
///		- Add exclusion rule
///		- Delete selected exclusion rules
///		- Export in Excel
///		- Filter on all fields of the grid
///		- Return to the list of input form types
/// </summary>
public partial class inputformtype_exclusionrules : HCPage
{
	#region Declarations

	private char inputFormTypeCode = ' ';
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
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Add").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarSeparator(uwToolbar, "AddSep");
      UITools.HideToolBarButton(uwToolbar, "Add");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }
    #endregion

    Page.ClientScript.RegisterStartupScript(Page.GetType(), "initialize", "<script>action='" + hAction.ClientID + "';</script>");

    try
    {
      // Get parameter (input form type code selected)
      if (Request["d"] != null)
      {
        inputFormTypeCode = Convert.ToChar(Request["d"]);
        hInputFormTypeCode.Value = inputFormTypeCode.ToString();
      }

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if ((Page.IsPostBack) && (hAction.Value.Equals("update")))
      {
        // Update if it is necessary
        UpdateDataView();

        // Re-initialize
        hAction.Value = string.Empty;
      }
      else if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "initIds", "<script>hInputFormTypeCodeId = '" + hInputFormTypeCode.ClientID + "';</script>");
  }

	private void UpdateDataView()
	{
		if (inputFormTypeCode != ' ')
		{
			string sqlFilter = string.Empty;
			sqlFilter += " InputFormTypeCode = '"+inputFormTypeCode.ToString()+"'";
			if (txtFilter.Text.Length>0)
			{
				string cleanFilter = txtFilter.Text.Replace("'", "''").ToLower();
				sqlFilter += " AND LOWER(ContainerType) LIKE '%"+cleanFilter+"%'";
			}
			HyperCatalog.Business.InputFormTypeExclusionRuleList iftExclusionRules = HyperCatalog.Business.InputFormTypeExclusionRule.GetAll(sqlFilter);
			dg.DataSource = iftExclusionRules;
			Utils.InitGridSort(ref dg);
			dg.DataBind();
			dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;

			// Refresh tab title 
			UITools.RefreshTab(this.Page, "ExclusionRules", iftExclusionRules.Count);

			if (dg.Rows.Count == 0)
			{
				UITools.HideToolBarButton(uwToolbar, "Delete");
				UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");          
			}
			else
			{
        if (SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL))
				{
					UITools.ShowToolBarButton(uwToolbar, "Delete");
					UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");          
					uwToolbar.Items.FromKeyButton("Delete").DefaultStyle.Width = Unit.Pixel(120);
				}
			}
		}
	}

	/// <summary>
	/// Delete multiple values on the grid
	/// </summary>
	private void Delete()
	{
		lbError.Visible=false;
		for (int i=0; i<dg.Rows.Count; i++)
		{
			TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
			CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
			if (cb.Checked)
			{
				string sCTCode = dg.Rows[i].Cells.FromKey("ContainerTypeCode").Text;
				string sIFTCode = dg.Rows[i].Cells.FromKey("InputFormTypeCode").Text;

				sCTCode = Utils.CReplace(sCTCode, "<font color=red><b>", "", 1);
				sCTCode = Utils.CReplace(sCTCode, "</b></font>", "", 1);
				sIFTCode = Utils.CReplace(sIFTCode, "<font color=red><b>", "", 1);
				sIFTCode = Utils.CReplace(sIFTCode, "</b></font>", "", 1);

				char iftCode = Convert.ToChar(sIFTCode);
				char ctCode = Convert.ToChar(sCTCode);

				HyperCatalog.Business.InputFormTypeExclusionRule iftExclusionRule = HyperCatalog.Business.InputFormTypeExclusionRule.GetByKey(iftCode, ctCode);
				if (!iftExclusionRule.Delete(HyperCatalog.Shared.SessionState.User.Id))
				{
					lbError.CssClass = "hc_error";
					lbError.Text = HyperCatalog.Business.InputFormTypeExclusionRule.LastError;
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
		if (btn == "delete")
		{
			Delete();
		}
		else if (btn == "export")
		{
			UpdateDataView();
			Utils.ExportToExcel(dg, "InputFormTypeExclusionRules", "InputFormTypeExclusionRules");
		}
	}

	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
	}
}
