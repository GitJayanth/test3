#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.ItemManagement
{
  /// <summary>
  /// Description résumée de item_allChunks.
  /// </summary>
  public partial class item_allChunks : HCPage
  {
		#region Declarations

		private HyperCatalog.Business.Item item;
		private Culture culture;
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
        
      try
      {
        if (Request["filter"] != null)
        {
          txtFilter.Text = Request["filter"].ToString();
        }
        QDEUtils.GetItemIdFromRequest();
        item = SessionState.CurrentItem;
      }
      catch
      {
        UITools.JsCloseWin();
      }

      try
      {
        culture = QDEUtils.UpdateCultureCodeFromRequest();
        if (!Page.IsPostBack)
        {
          UpdateDataView();
        }
      }
      finally
      {
        culture.Dispose();
      }
    }

    private void UpdateDataView()
    {
      //***************************************
      // Retrieve Item info
      //***************************************
			dg.Visible = false;
			lbResult.Visible = false;
      if (item != null && culture != null)
      {
        // Update label for item name
        lbItemName.Text = item.Name + " (" + item.Level.Name + ")";
        if (item.Sku != string.Empty)
          lbItemName.Text = item.Sku + "-" + lbItemName.Text;

        //using (ChunkList itemChunks = item.Chunks(culture.Code))
        using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
        {
        string sSql = "EXECUTE dbo.QDE_GetItemCountryView " + item.Id + ", '" + culture.Code + "', 0 , 0";
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sSql, "chunks"))
        {
            if (ds != null)
            {
              dg.DataSource = ds.Tables[0];
              Utils.InitGridSort(ref dg, false);
              dg.DataBind();
              lbcultureName.Text = culture.Name;
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "cultureCode", "<script>l='" + culture.Name + "';</script>");
            }
          }
        }
      }

			if (dg != null && dg.Rows != null && dg.Rows.Count > 0)
			{
				dg.Visible = true;
			}
			else
			{
				lbResult.Visible = true;
			}
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      string filter = txtFilter.Text.Trim();
      bool keep = true;
      if (e.Row.Cells.FromKey("S") != null && e.Row.Cells.FromKey("S").Text == "M")
      {
        keep = false;
      }
      if (filter.Length > 0)
      {
        keep = false;
        Infragistics.WebUI.UltraWebGrid.CellsCollection cells = e.Row.Cells;
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in cells)
        {
          if (!c.Column.Hidden && c.Value != null && c.Text != HyperCatalog.Business.Chunk.BlankValue)
          {
            if (c.Text.ToLower().IndexOf(filter.ToLower()) >= 0)
            {
              c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
              keep = true;
            }
          }
        }
      }
      if (!keep)
      {
        e.Row.Delete();
      }
      else
      {
        string v = e.Row.Cells.FromKey("V").Text;
        bool rtl = (bool)e.Row.Cells.FromKey("rtl").Value;
        string LevelId = "[" + e.Row.Cells.FromKey("L").ToString() + "] ";
        // Update status
        string status = e.Row.Cells.FromKey("S").ToString();
        e.Row.Cells.FromKey("S").Value= "<img src='/hc_v4/img/S" + status + ".gif'>";
       
       
        // Update value if culture is in Rtl
        if (rtl && e.Row.Cells.FromKey("V").Text.Length > 0 && e.Row.Cells.FromKey("V").Text != Chunk.BlankValue)
          e.Row.Cells.FromKey("V").Text = "<span class='rtl'>" + UITools.HtmlEncode(v) + "</span>";
        else
        {
          e.Row.Cells.FromKey("V").Text = UITools.HtmlEncode(v);
        }
        // Update value if ILB
        if (e.Row.Cells.FromKey("V").Text == Chunk.BlankValue)
          e.Row.Cells.FromKey("V").Text = Chunk.BlankText;
        e.Row.Cells.FromKey("IN").Text = LevelId + e.Row.Cells.FromKey("IN").Text;

      }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        string f =  Utils.CleanFileName(lbItemName.Text);
        
        //Utils.ExportToExcel(dg, f, f);

        dg.Columns.FromKey("S").Hidden = true;
        dg.Columns.FromKey("ST").Hidden = false;
        Utils.ExportToExcelFromGrid(dg, f, f, Page, null, "All Chunks");
        dg.Columns.FromKey("S").Hidden = false;
        dg.Columns.FromKey("ST").Hidden = true;
      }
    }
  }
}