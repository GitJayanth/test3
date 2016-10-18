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
	public partial class uc_counters : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Obtain data
			if (!Page.IsPostBack)
				BindData();
		}
		private void BindData()
		{
			ModuleDB.CountersDB countersDB=new ModuleDB.CountersDB();
      using (DataSet ds = countersDB.GetCounters())
      {        
        if (ds != null)
        {
          uwgCounters.DataSource = ds;
          uwgCounters.DataBind();

          ds.Dispose();
        }
      }
		}
	}
}
