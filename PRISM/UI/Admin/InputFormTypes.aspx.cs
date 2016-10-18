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
using Infragistics.WebUI.WebDataInput;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display the list of input form type
	///		- Add input form type
	///		- Modify input form type (with exclusions)
	///		- Export in Excel
	///		- Filter on all fields of the grid
	/// </summary>
	public partial class InputFormTypes : HCPage
	{
		#region Declarations
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
      if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      }

      if (!HyperCatalog.Shared.SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL))
      {
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
        UITools.HideToolBarButton(uwToolbar, "Save");
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
    }

		private void UpdateDataView()
		{
			string sSql = string.Empty;

			// filter
			if (txtFilter.Text != string.Empty)
			{
				string cleanFilter = txtFilter.Text.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

				sSql += " LOWER(InputFormType) LIKE '%"+cleanFilter+"%'";
			}

      using (HyperCatalog.Business.InputFormTypeList inputFormTypeList = HyperCatalog.Business.InputFormType.GetAll(sSql, "Sort"))
      {
        if (inputFormTypeList != null)
        {
          if (inputFormTypeList.Count > 0)
          {
            dg.DataSource = inputFormTypeList;
            Utils.InitGridSort(ref dg, false);
            dg.DataBind();

            if (txtFilter.Text == string.Empty)
              Utils.EnableIntelligentSort(ref dg, Convert.ToInt32(txtSortColPos.Value)); // sort column

            dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
            dg.Columns.Remove(dg.Columns.FromKey("__Spacer"));

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

		private void UpdateDataEdit(string selInputFormType)
		{
			if (selInputFormType==string.Empty)
			{ 
				// Create input form type
				webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./inputformtypes/inputformtype_properties.aspx";
				lbTitle.Text = "Input form type: New";
				webTab.Tabs.GetTab(1).Visible = false;

				panelGrid.Visible = false;
				webTab.Visible = true;
			}
			else
			{
				// Modify input form types
				char code = Convert.ToChar(selInputFormType);
				HyperCatalog.Business.InputFormType inputFormType = HyperCatalog.Business.InputFormType.GetByKey(code);

				if (inputFormType != null)
				{
					webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./inputformtypes/inputformtype_properties.aspx?d=" + inputFormType.Code;
					webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./inputformtypes/inputformtype_exclusionrules.aspx?d=" + inputFormType.Code;
					string sqlFilter = " InputFormTypeCode = '"+inputFormType.Code.ToString()+"'";
					webTab.Tabs.GetTab(1).Text = webTab.Tabs.GetTab(1).Text+" ("+HyperCatalog.Business.InputFormTypeExclusionRule.GetAll(sqlFilter).Count.ToString()+")";
					webTab.Tabs.GetTab(1).Visible = true;      

					lbTitle.Text = "Input form type: " + inputFormType.Name;

					panelGrid.Visible = false;
					webTab.Visible = true;
				}
			}
		}

		/// <summary>
		/// Apply new sort
		/// </summary>
		private void Save()
		{
			lbError.Visible = false;

			if (dg != null)
			{
				bool isSaved = false;
				foreach (UltraGridRow r in dg.Rows)
				{
					char code = Convert.ToChar(r.Cells.FromKey("Code").Value);
					HyperCatalog.Business.InputFormType ift = HyperCatalog.Business.InputFormType.GetByKey(code);
					ift.Sort = r.Index;

					if (!ift.Save(false))
					{
						lbError.CssClass = "hc_error";
						lbError.Text = HyperCatalog.Business.InputFormType.LastError;
						lbError.Visible=true;

						isSaved = false;
						break;
					}
					else
						isSaved = true;
				}

				if (isSaved)
				{
					lbError.CssClass = "hc_success";
					lbError.Text = "New sort saved!";
					lbError.Visible=true;
				}
			}
		}

		private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
		{
			if (txtFilter.Text.Length > 0)
			{
				Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
				UITools.HiglightGridRowFilter(ref r, txtFilter.Text);   
			}
		}

		private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
		{
			string btn = be.Button.Key.ToLower();
			if (btn == "add")
			{
				UpdateDataEdit(string.Empty);
			}
			else if (btn == "export")
			{
				UpdateDataView();
				Utils.ExportToExcel(dg, "InputFormTypes", "InputFormTypes");
			}
			else if (btn == "save")
			{
				Save();
			}
		}

		// "Name" Link Button event handler
		protected void UpdateGridItem(object sender, System.EventArgs e)
		{
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sInputFormTypeId = cellItem.Cell.Row.Cells.FromKey("Id").Text;

				sInputFormTypeId = Utils.CReplace(sInputFormTypeId, "<font color=red><b>", "", 1);
				sInputFormTypeId = Utils.CReplace(sInputFormTypeId, "</b></font>", "", 1);

				UpdateDataEdit(sInputFormTypeId);
			}
		}
	}
}
