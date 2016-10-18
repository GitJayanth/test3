namespace hc_homePageModules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
  using HyperCatalog.Shared;

	/// <summary>
	/// This class represents News control
	/// </summary>
	public partial class uc_news : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
    {

			// Obtain data
			if (!Page.IsPostBack)
				BindData();
		}
    private void BindData()
    {

      ModuleDB.NewsDB newsDB = new ModuleDB.NewsDB();
      using (DataSet ds = newsDB.GetNews())
      {
        if (ds != null)
        {
          uwgNews.DataSource = ds;
          uwgNews.DataBind();
          uwgNews.Columns.FromKey("CreateDate").Format = SessionState.User.FormatDate;

          ds.Dispose();
        }
      }
    }
    protected void uwgNews_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      e.Row.Cells.FromKey("ShortDescription").Text = "<a href='redirect.aspx?p=UI/Help.aspx?Page=../HyperHelp/News.aspx?n=" + e.Row.Cells.FromKey("NewsId").Text + "' target='_BLANK'>" + e.Row.Cells.FromKey("ShortDescription").Text + "</a>";
    }
}
}
