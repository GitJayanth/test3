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
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display cultures list
	/// </summary>
	public partial class Cultures : HCPage
	{
		#region Declarations
    protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator4;
    protected System.Web.UI.WebControls.TextBox txtExample;
    protected System.Web.UI.WebControls.Button BtnAdd;
		#endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
		
    /// <summary>
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {      
      #region Check Capabilities
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL)))
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
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
      webTab.Visible = false;
			string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " LOWER(CultureCode) like '%" + cleanFilter + "%'";
        sSql += " OR LOWER(CultureName) like '%" + cleanFilter + "%'";
      }

      panelGrid.Visible = dg.Visible = true;
      lbSpacer.Text = "";

      using (HyperCatalog.Business.CultureList cultures = HyperCatalog.Business.Culture.GetAll(sSql))
      {
        if (cultures != null)
        {
          if (cultures.Count > 0)
          {
            dg.DataSource = cultures;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            lbNoresults.Visible = false;
            dg.Visible = true;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

            lbNoresults.Visible = true;
            dg.Visible = false;
          }

          lbTitle.Text = UITools.GetTranslation("Cultures list");
        }
      }
    }

    void UpdateDataEdit(string selCultureCode)
    {
      panelGrid.Visible = false;
      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./localizations/localization_Properties.aspx?c=" + selCultureCode;
      if (selCultureCode == "-1")
      {
        lbTitle.Text = "Culture: New";
      }
      else
      {
        HyperCatalog.Business.Culture culture = HyperCatalog.Business.Culture.GetByKey(selCultureCode);
        lbTitle.Text = "Culture: " + culture.Name.ToString();  
      }
      webTab.Visible = true;
      webTab.SelectedTabIndex = 0;
    }
    
    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sId = cellItem.Cell.Row.Cells.FromKey("Code").Text;
			
				sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
				sId = Utils.CReplace(sId, "</b></font>", "", 1);

				UpdateDataEdit(sId);
			}
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "Cultures", "Cultures");
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }
  }
}
