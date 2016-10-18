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
using HyperCatalog.DataAccessLayer;
#endregion

	/// <summary>
	/// Description résumée de chunk_inheritance.
	/// </summary>
public partial class chunk_inheritance : HCPage
{
  private long itemId;
  private int containerId;
  private string cultureCode;
  
  protected void Page_Load(object sender, System.EventArgs e)
  {
    try
    {
      containerId = Convert.ToInt32(Request["d"]);
      itemId = QDEUtils.GetQueryItemIdFromRequest();
      cultureCode = QDEUtils.GetQueryCultureCodeFromRequest();

      if (!Page.IsPostBack)
      {
        ShowHeritage();
      }
    }
    catch (Exception ex)
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "alert('Error: " + UITools.CleanJSString(ex.ToString()) + "'", true);
    }
  }

  private void ShowHeritage()
  {
    SqlDataAccessLayer DBLayer = new SqlDataAccessLayer();
    using (DataSet ds = DBLayer.GetChunkHeritage(SessionState.User.Id, itemId, containerId, cultureCode))
    {
      dg.DataSource = ds;

      // Allow paging
      Utils.InitGridSort(ref dg, false);
      dg.DataBind();

      // Disable sorting
      dg.DisplayLayout.HeaderClickActionDefault = Infragistics.WebUI.UltraWebGrid.HeaderClickAction.NotSet;
      dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;

      // hide fallback column
      dg.Columns.FromKey("Culture").Hidden = (SessionState.Culture.Type == HyperCatalog.Business.CultureType.Master);
    }
  }

  protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    // Indent items
    int level = -1;
    if (e.Row.Cells.FromKey("Level") != null)
    {
      level = Convert.ToInt32(e.Row.Cells.FromKey("Level").Value);
    }
    if (e.Row.Cells.FromKey("Product") != null && level > -1)
    {
      e.Row.Cells.FromKey("Product").Style.Padding.Left = Unit.Pixel(level * 4);
    }

    // Display culture code
    if (e.Row.Cells.FromKey("CultureCode") != null && e.Row.Cells.FromKey("CountryCode") != null && e.Row.Cells.FromKey("CultureCode").Value.ToString() != string.Empty)
    {
      e.Row.Cells.FromKey("Culture").Value = "<img alt='" + e.Row.Cells.FromKey("CultureCode").Value.ToString() + "' src='/hc_v4/img/flags/" + e.Row.Cells.FromKey("CountryCode").Value.ToString() + ".gif'>";
    }

    // Colorize items if not in scope
    if (e.Row.Cells.FromKey("InScope") != null && !(bool)e.Row.Cells.FromKey("InScope").Value)
      e.Row.Style.ForeColor = Color.DarkGray;

    if (e.Row.Cells.FromKey("Value") != null)
    {
      if (e.Row.Cells.FromKey("Value").Text == HyperCatalog.Business.Chunk.BlankValue)
        e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
      else
        e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(e.Row.Cells.FromKey("Value").Text).Replace(Environment.NewLine, "<br/>");
    }
  }
}
