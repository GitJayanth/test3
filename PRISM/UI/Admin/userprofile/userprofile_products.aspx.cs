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
using HyperComponents.Data.dbAccess;

/// <summary>
/// Description résumée de userprofile_products.
/// </summary>
public partial class userprofile_products : HCPage
{
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
        HyperCatalog.Business.ItemList items = HyperCatalog.Shared.SessionState.User.Items;
        if (items != null)
        {
            dg.DataSource = items;
            Utils.InitGridSort(ref dg, true);
            dg.DataBind();

            if (dg.Rows.Count > 0)
            {
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

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        // Update level name
        HyperCatalog.Business.ItemLevel level = HyperCatalog.Business.ItemLevel.GetByKey(Convert.ToInt32(e.Row.Cells.FromKey("LevelId").Value));
        if (level != null)
            e.Row.Cells.FromKey("LevelName").Text = level.Name;

        // Update filter
        if (txtFilter.Text.Length > 0)
        {
            Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;

            string filter = txtFilter.Text.Trim().ToLower();
            string itemName = r.Cells.FromKey("ItemName").Value.ToString().ToLower();
            string levelName = r.Cells.FromKey("LevelName").Value.ToString().ToLower();

            if ((itemName.Length == 0 || itemName.IndexOf(filter) < 0) && (levelName.Length == 0 || levelName.IndexOf(filter) < 0))
                dg.Rows.Remove(r);
            else
                UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
        }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "export")
        {
          Utils.ExportToExcel(dg, "ItemsFor" + HyperCatalog.Shared.SessionState.User.FullName, "ItemsFor" + HyperCatalog.Shared.SessionState.User.FullName);
        }
    }
}
