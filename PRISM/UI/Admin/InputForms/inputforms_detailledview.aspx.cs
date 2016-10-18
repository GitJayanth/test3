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
using HyperCatalog.Business;
using System.Data.SqlClient;

namespace HyperCatalog.UI.Inputforms
{	/// <summary>
  /// Description résumée de userprofile_products.
  /// </summary>
  public partial class inputform_detailledview : HCPage
  {
		#region Declarations
    
    private string InputFormId = string.Empty;
		#endregion

    #region Constantes
    private const string SP_INPUTFORM_DETAILLEDVIEW = "dbo._InputForm_GetDetailedView";
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
      if (Request["i"] != null)
      {
        // Retrieve Id of input form
        InputFormId = Request["i"].ToString();

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
        if (!Page.IsPostBack)
        {
          UpdateDataView();
        }
      }
    }

    void UpdateDataView()
    {
      // Open connection
      using (Database dbObj = Utils.GetMainDB())
      {

        // SQL query
        using (DataSet ds = dbObj.RunSPReturnDataSet(SP_INPUTFORM_DETAILLEDVIEW,
          new SqlParameter("@InputFormId", InputFormId)))
        {
          dbObj.CloseConnection();

          if (ds != null)
          {
            if (dbObj.LastError.Length == 0)
            {
              dg.DataSource = ds.Tables[0];
              Utils.InitGridSort(ref dg, false);
              dg.DataBind();
              dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;

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
            else
            {
              lbError.CssClass = "hc_error";
              lbError.Text = dbObj.LastError;
              lbError.Visible = true;
            }
          }
        }
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;

      if (txtFilter.Text.Length > 0)
      {
        string filter = txtFilter.Text.Trim().ToLower();
        string c;
        bool keep = false;
        for (int i = 1; i < 6; i++)
        {
          c = r.Cells[i].Text;
          if (c!= null && c.ToLower().IndexOf(filter) >= 0)
          {
            keep = true;
            break;
          }
        }
        if (keep)
          UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
        else 
          r.Delete();
      }

    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, InputFormId.ToString() + "-Detailled_View", InputFormId.ToString() + "-Detailled_View");
      }
    }
    protected void dg_PreRender(object sender, EventArgs e)
    {
      string curTag = string.Empty;
      int rowspan = 0;
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r;
      string curStyle = "ugd";
      for (int i = 0; i < dg.Rows.Count;i++ )
      {
        string tag = dg.Rows[i].Cells.FromKey("Tag").Text;
        dg.Rows[i].Cells.FromKey("PV").Style.CssClass = "ugd";
        dg.Rows[i].Cells.FromKey("PV").Style.CssClass = "ugd";
        dg.Rows[i].Cells.FromKey("VC").Style.CssClass = "ugd";
        dg.Rows[i].Cells.FromKey("PV").Style.Wrap = true;
        dg.Rows[i].Cells.FromKey("VC").Style.Wrap = true;
        dg.Rows[i].Cells.FromKey("CN").Style.Wrap = true;
        if (tag != curTag && i > 0)
        {
          dg.Rows[i - rowspan].Cells.FromKey("Comment").Style.Wrap = true;
          for (int j = 0; j < 5; j++)
          {
            dg.Rows[i - rowspan].Cells[j].Style.CssClass = curStyle;
            dg.Rows[i - rowspan].Cells[j].RowSpan = rowspan;
          }
          rowspan = 1;
          curTag = tag;

          curStyle = curStyle == "ugd"?"uga":"ugd";
        }
        else
        {
          rowspan++;
          for (int j = 0; j < 5; j++)
          {
            dg.Rows[i].Cells[j].Style.CssClass = curStyle;
          }
        }
      }
      if (dg.Rows.Count > 0)
      {
        dg.Rows[dg.Rows.Count - rowspan].Cells.FromKey("Comment").RowSpan = rowspan;
        dg.Rows[dg.Rows.Count - rowspan].Cells.FromKey("M").RowSpan = rowspan;
        dg.Rows[dg.Rows.Count - rowspan].Cells.FromKey("CN").RowSpan = rowspan;
        for (int j = 0; j < 5; j++)
        {
          dg.Rows[dg.Rows.Count - rowspan].Cells[j].Style.CssClass = curStyle;
          dg.Rows[dg.Rows.Count - rowspan].Cells[j].RowSpan = rowspan;
        }
      }
    }
}
}