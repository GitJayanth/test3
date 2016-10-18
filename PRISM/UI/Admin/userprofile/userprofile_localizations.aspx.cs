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
using System.Data.SqlClient;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;

/// <summary>
/// Description résumée de properties.
/// </summary>
public partial class userprofile_localizations : HCPage
{
  #region Declarations
  protected string userId;
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
  }
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
    if (!Page.IsPostBack)
    {
      UpdateDataView();
    }
}
  void UpdateDataView()
  {
    string filter = txtFilter.Text;
    CultureList culs;
    if (filter != string.Empty)
    {
      culs = new CultureList();
      foreach (HyperCatalog.Business.Culture cul in SessionState.User.Cultures)
      {
        if (cul.Code.ToLower().IndexOf(filter) >= 0 ||
          cul.Name.ToLower().IndexOf(filter) >= 0)
        {
          culs.Add(cul);
        }
      }
    }
    else
    {
      culs = SessionState.User.Cultures;
    }

    if (culs != null)
    {
      if (culs.Count > 0)
      {
        dg.DataSource = culs;
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
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "export")
    {
      UpdateDataView();
      Utils.ExportToExcel(dg, "CatalogsFor" + SessionState.User.FullName, "CatalogsFor" + SessionState.User.FullName); 
    }
  }
}
