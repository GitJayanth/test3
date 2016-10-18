namespace hc_homePageModules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// This class represents Counter control
	/// </summary>
  public partial class uc_comingtranslations : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Obtain data
			if (!Page.IsPostBack)
				BindData();
		}
		private void BindData()
		{
      ModuleDB.ComingTranslationsDB comingTranslationsDB = new ModuleDB.ComingTranslationsDB();
      using (DataSet ds = comingTranslationsDB.GetComingTranslations())
      {
        uwgTranslations.DataSource = ds;
        uwgTranslations.DataBind();
        Utils.InitGridSort(ref uwgTranslations, false);
        uwgTranslations.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
        uwgTranslations.Columns.FromKey("BOT").Format = HyperCatalog.Shared.SessionState.User.FormatDate;

      }
		}
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      if (e.Row.Cells.FromKey("ItemNumber").Value != null)
      {
        e.Row.Cells.FromKey("ItemName").Text = "[" + e.Row.Cells.FromKey("ItemNumber").Text + "] " + e.Row.Cells.FromKey("ItemName").Text;
      }
      e.Row.Cells.FromKey("ItemName").Text = "<a href='redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "' target='_BLANK'\">" + e.Row.Cells.FromKey("ItemName").Text + "</a>";

      //e.Row.Cells.FromKey("NbDays").Text = "[" + e.Row.Cells.FromKey("NbDays").Text + "]";
      e.Row.Cells.FromKey("LevelId").Text = "[" + e.Row.Cells.FromKey("LevelId").Text + "]";
    }

	}
}
