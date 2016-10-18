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
  public partial class uc_todaysactivity : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Obtain data
			if (!Page.IsPostBack)
				BindData();
		}
    private void BindData()
    {
      ModuleDB.TodaysActivityDB TodaysActivityDB = new ModuleDB.TodaysActivityDB();
      using (DataTable dt = TodaysActivityDB.GetTodaysActivity())
      {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (dt.Rows.Count > 0)
        {
          sb.Append("<TABLE BORDER='0' WIDTH='100%' CELLSPACING='0' CELLPADDING='2' class='datatable'>");
          sb.Append("<TR><TH class='gh2'width='100'>Language</TH>");

          for (int i = 0; i < dt.Rows.Count; i++)
          {
            sb.Append("<TH class='gh2'>" + dt.Rows[i][0].ToString() + "</TH>");
          }
          sb.Append("</TR><TR align='center'><TH class='gh2'>#Chunks</TH>");
          for (int i = 0; i < dt.Rows.Count; i++)
          {
            string fb =  dt.Rows[i]["IsMaster"].ToString()=="1"?"<font color='red'><b>":dt.Rows[i]["IsRegion"].ToString()=="1"?"<font color='darkblue'>":"";
            string fe =  dt.Rows[i]["IsMaster"].ToString()=="1"?"</b></font>":dt.Rows[i]["IsRegion"].ToString()=="1"?"</font>":"";
            sb.Append("<TD>" + fb + dt.Rows[i][1].ToString() + fe + "</TD>");
          }
          sb.Append("</TR></TABLE>");
          lbTodayActivity.Text = sb.ToString();
        }
        else
        {
          lbTodayActivity.Text = "No content was updated today";
        }
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
