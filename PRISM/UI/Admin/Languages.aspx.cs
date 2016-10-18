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
#endregion

namespace HyperCatalog.UI.Admin
{
  /// <summary>
  /// Display languages list
  /// </summary>
  public partial class Languages : HCPage
  {
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
      if (filter != string.Empty)
      {
        string cleanFilter = filter.Replace("'", "''").ToLower();
        cleanFilter = cleanFilter.Replace("[", "[[]");
        cleanFilter = cleanFilter.Replace("_", "[_]");
        cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " LOWER(LanguageCode) like '%" + cleanFilter + "%'";
        sSql += " OR LOWER(LanguageName) like '%" + cleanFilter + "%'";
      }

      panelGrid.Visible = dg.Visible = true;
      lbSpacer.Text = "";

      using (HyperCatalog.Business.LanguageList languages = HyperCatalog.Business.Language.GetAll(sSql))
      {
        if (languages != null)
        {
          if (languages.Count > 0)
          {
            dg.DataSource = languages;
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

          lbTitle.Text = UITools.GetTranslation("languages list");
        }
      }
    }

    void UpdateDataEdit(string selLanguageCode)
    {
      panelGrid.Visible = false;
      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./languages/language_Properties.aspx?l=" + selLanguageCode;
      if (selLanguageCode == "-1")
      {
        lbTitle.Text = "Language: New";
      }
      else
      {
        HyperCatalog.Business.Language language = HyperCatalog.Business.Language.GetByKey(selLanguageCode);
        lbTitle.Text = "Language: " + language.Name.ToString();
      }
      webTab.Visible = true;
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
        Utils.ExportToExcel(dg, "Languages", "Languages");
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }
  }
}