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
using HyperCatalog.Business;
#endregion
	/// <summary>
/// Description résumée de Chunk_Regionalizations.
	/// </summary>
public partial class Chunk_Regionalizations : HCPage
{
  private System.Int64 itemId;
  private int containerId;
  private string cultureCode;
  protected HyperCatalog.Business.Chunk chunk;
  protected System.Web.UI.WebControls.Button BtnClose;
  
  #region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    //
    // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
    //
    InitializeComponent();
    base.OnInit(e);
  }
		
  /// <summary>
  /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
  /// le contenu de cette méthode avec l'éditeur de code.
  /// </summary>
  private void InitializeComponent()
  {    
    this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

  }
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    
    try
    {
      containerId = Convert.ToInt32(Request["d"]);
      cultureCode = QDEUtils.UpdateCultureCodeFromRequest().Code;
      itemId = QDEUtils.GetQueryItemIdFromRequest();
      UpdateDataView();
    }
    catch (Exception ex)
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('Error: " + UITools.CleanJSString(ex.ToString()) + "'");
    }
  }

  private void UpdateDataView()
  {
    dg.DataSource = HyperCatalog.Business.Chunk.GetRegionalizations(itemId, containerId, cultureCode);
    Utils.InitGridSort(ref dg, false);
    dg.DataBind();
  }

  private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    bool rtl = e.Row.Cells.FromKey("rtl").Text.ToLower()=="true";
    if (rtl)
    {
      e.Row.Cells.FromKey("Value").Style.CssClass  = "rtl";
    }
    if (e.Row.Cells.FromKey("Value").Text == HyperCatalog.Business.Chunk.BlankValue)
    {
      e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
    }
    else
    {
      e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(e.Row.Cells.FromKey("Value").Text);
    }
    ChunkStatus cStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus),e.Row.Cells.FromKey("Status").Value.ToString());
    string status = HyperCatalog.Business.Chunk.GetStatusFromEnum(cStatus);
    e.Row.Cells.FromKey("Status").Style.CssClass = "S" + status;
    e.Row.Cells.FromKey("Status").Value = string.Empty;
  }
}
