namespace hc_homePageModules
{
  using System;
  using System.Data;
  using System.Drawing;
  using System.Web;
  using System.Web.UI.WebControls;
  using System.Web.UI.HtmlControls;

  /// <summary>
  /// This class represents Welcome control
  /// </summary>
  public partial class uc_welcome : System.Web.UI.UserControl
  {
    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack)
        BindData();
    }

    private void BindData()
    {
      Module.ModuleTitle= "Welcome to " + HyperCatalog.Shared.SessionState.CacheParams["AppName"].Value.ToString();
      ModuleDB.MenusDB menusDB = new ModuleDB.MenusDB();
      using (DataSet ds = menusDB.GetMenus())
      {
        if (ds != null)
        {
          uwgWelcome.DataSource = ds;
          uwgWelcome.DataBind();
        }
      }
    }
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      if (e.Row.Cells.FromKey("URL").Value != null)
      {
        e.Row.Cells.FromKey("Text").Text = "<li><a href='" + e.Row.Cells.FromKey("URL").Text + "'> " + e.Row.Cells.FromKey("Text").Text +"</a></li>";
      }
    }

  }
}
