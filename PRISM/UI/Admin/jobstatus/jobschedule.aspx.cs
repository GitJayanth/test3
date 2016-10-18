using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;


public partial class UI_Admin_jobstatus_jobschedule : System.Web.UI.Page
{
    protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
    protected HyperCatalog.Business.CapabilitiesEnum updateCapability = HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL;
    private void InitializeComponent()
	{
		this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
	}

	protected void Page_Load(object sender, System.EventArgs e)
	{ 
	
        if (!HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
		{
			UITools.HideToolBarSeparator(uwToolbar, "AddSep");
			UITools.HideToolBarButton(uwToolbar, "Add");
		}
        if (!Page.IsPostBack)
		{
			UpdateDataView();
		}
    }
    protected override void OnPreRender(EventArgs e)
    { 
        uwToolbar.Items.FromKeyButton("Add").Enabled = !HyperCatalog.Shared.SessionState.User.IsReadOnly && HyperCatalog.Shared.SessionState.User.HasCapability(updateCapability); }

    private void UpdateDataView()
    {
        Database dbCrystal = Utils.GetMainDB();
        string strQuery = "SELECT AppComponentId,JobName,JobType,ScheduledDay,ScheduledTime,(EstimatedFinishTime+' mins') AS EstimatedFinishTime ,CutOffTime,IsActive FROM JobSchedule WITH (NOLOCK)";
        DataSet ds = dbCrystal.RunSQLReturnDataSet(strQuery, new SqlParameter[] { });
        if (ds != null)
        {
            Session["JobsSch"] = ds;
            DataTable dtJS = ds.Tables[0];
            if (dtJS != null)
            {
                if (dtJS.Rows.Count > 0)
                {
                    dg.DataSource = dtJS;
                    Utils.InitGridSort(ref dg);
                    dg.DataBind();
                    dg.Visible = true;
                    lbNoresults.Visible = false;
                }
            }
        }
    }
    //private void UpdateDataEdit(string JobName)
    //{
        
    //}
    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
		if (sender != null)
		{
			Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
			string sJobName = cellItem.Cell.Row.Cells.FromKey("JobName").Text;
		
			sJobName = Utils.CReplace(sJobName, "<font color=red><b>", "", 1);
			sJobName = Utils.CReplace(sJobName, "</b></font>", "", 1);
            
            panelGrid.Visible = false;
            panelTab.Visible = true;
            webTab.Tabs.GetTab(0).Visible = true;
            webTab.Tabs.GetTab(0).Text = "Properties";
            webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./jobscheduleedit.aspx?j=" + sJobName;
		}
    }

	protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "export")
		{
            ExportToExcel();
		}
		else if (btn == "add")
		{
            panelGrid.Visible = false;
            panelTab.Visible = true;
            webTab.Tabs.GetTab(0).Visible = true;
            webTab.Tabs.GetTab(0).Text = "New Job";
            webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./jobscheduleedit.aspx?";
		}
    }
    protected void ExportToExcel()
    {
        DataSet ds = (DataSet)Session["JobsSch"];
        if (ds != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<TABLE border=\"1\">");
            bool isHeader = true;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (isHeader)
                {
                    sb.Append("<TR>");
                    foreach (DataColumn col in ds.Tables[0].Columns)
                        if (col.ColumnMapping != MappingType.Hidden)
                            sb.Append("<TH style=\"font-weight:bold;\">" + col.Caption + "</TH>");
                    sb.Append("</TR>");
                    isHeader = false;
                }

                sb.Append("<TR>");
                foreach (DataColumn col in ds.Tables[0].Columns)
                    if (col.ColumnMapping != MappingType.Hidden)
                        sb.Append("<TD>" + UITools.HtmlEncode(row[col].ToString()) + "</TD>");
                sb.Append("</TR>");
            }
            sb.Append("</TABLE>");
            string s = sb.ToString();
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] contentBytes = encoding.GetBytes(s);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
            Response.ContentType = "application/vnd.ms-excel";
            //Fix for CR 5109 - Prabhu R S
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=\"JobSchedule.xls\"; " +
                              "size=" + s.Length.ToString() + "; " +
                              "creation-date=" + DateTime.Now.ToString("R") + "; " +
                              "modification-date=" + DateTime.Now.ToString("R") + "; " +
                             "read-date=" + DateTime.Now.ToString("R"));

            Response.OutputStream.Write(
            contentBytes, 0,
            Convert.ToInt32(contentBytes.Length));
            Response.Flush();
            try { Response.End(); }
            catch { }
        }
    }
  }

