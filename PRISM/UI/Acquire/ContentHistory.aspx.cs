#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
#endregion

public partial class UI_Acquire_ContentHistory : HCPage
{
    private const int MAX_EXCEL_SIZE = 65536;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {

           /* if (!eventsGrid.IsXmlHttpRequest)
            {
                setRange("Today");
            }
            mainToolBar.Items.FromKeyButton("Today").Selected = true;*/
            eventsGrid.Visible = false;
        }
        eventsGrid.Columns.FromKey("Date").Format = HyperCatalog.Shared.SessionState.User.FormatDate + " " + HyperCatalog.Shared.SessionState.User.FormatTime;
        Utils.InitGridSort(ref eventsGrid, true);
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
        startDate.CalendarLayout.Culture = ci;
        endDate.CalendarLayout.Culture = ci;
        startDate.MinDate = DateTime.Today.AddMonths(-1);
        startDate.MaxDate = DateTime.Today;
        endDate.MinDate = DateTime.Today.AddMonths(-1);
        endDate.MaxDate = DateTime.Today;

        

    }

    private DataSet GetDataSet()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            DataSet ds;
            /**** For PL selection ****/
            string selPls = GetSelPLs();
            ds = dbObj.RunSPReturnDataSet("_Report_ContentHistory",
                new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id),
                new SqlParameter("@StartDate", startDate.Value),
                new SqlParameter("@EndDate", ((DateTime)endDate.Value).AddDays(1)),
                new SqlParameter("@MineOnly", mainToolBar.Items.FromKeyButton("MineOnly").Selected),
                /**** For PL selection ****/
                new SqlParameter("@PLCode", selPls + "")
                );
            dbObj.CloseConnection();

            lError.Visible = false;
            eventsGrid.Visible = true;
            
            lRecordcount.Text = "<b>Recordcount : </b>" + ds.Tables[0].Rows.Count.ToString();
            //lRecordcount.Visible = true;
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables[0].Rows.Count >= MAX_EXCEL_SIZE)
                {
                    tooManyRowsPanel.Visible = true;
                    UITools.HideToolBarButton(mainToolBar, "Export");
                    XLSPanel.Visible = (ds.Tables[0].Rows.Count <= MAX_EXCEL_SIZE);
                }
            }
            if (ds.Tables.Count > 0)
            {
                #region No result
                if (ds.Tables[0].Rows.Count == 0)
                {
                    eventsGrid.Visible = false;
                    lError.Text = "Sorry, your query is returning no result";
                    lError.Visible = true;
                    lError.CssClass = "hc_error";
                }
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "totalrows", "var totalRows = " + ds.Tables[0].Rows.Count.ToString() + ";", true);
                //eventsGrid.DisplayLayout.RowsRange = 50;
                #endregion
            }
            return ds;
        }
        eventsGrid.Visible = false;
        lError.Text = "Sorry, an expected error occurred";
        lError.Visible = true;
        lError.CssClass = "hc_error";
        return null;
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.eventsGrid.InitializeDataSource += new Infragistics.WebUI.UltraWebGrid.InitializeDataSourceEventHandler(this.eventsGrid_InitializeDataSource);
    }
    #endregion

    
    private void eventsGrid_InitializeDataSource(object sender, Infragistics.WebUI.UltraWebGrid.UltraGridEventArgs e)
    {
        
        if (mainToolBar.Items.FromKeyButton("Today").Selected)
        {
            setRange("Today");
        }
        if (mainToolBar.Items.FromKeyButton("Yesterday").Selected)
        {
            setRange("Yesterday");
        }
        if (mainToolBar.Items.FromKeyButton("LastWeek").Selected)
        {
            setRange("LastWeek");
        }
        /**** For PL selection 3.5 release****/
        if ((startDate.Value == null || endDate.Value == null) && mainToolBar.Items.FromKeyButton("MineOnly").Selected)
        {
            setRange("Today");
        }
        if (startDate.Value != null && endDate.Value != null)
        {
            if (((DateTime)startDate.Value) == DateTime.Today.Date && ((DateTime)endDate.Value) == DateTime.Today.Date)
                setRange("Today");
            else if (((DateTime)startDate.Value) == DateTime.Today.AddDays(-1).Date && ((DateTime)endDate.Value) == DateTime.Today.AddDays(-1).Date)
                setRange("Yesterday");
            else if (((DateTime)startDate.Value) == DateTime.Today.AddDays(-7).Date && ((DateTime)endDate.Value) == DateTime.Today.Date)
                setRange("LastWeek");
            else
            {
                advancedToolBar.ClientSideEvents.InitializeToolbar = "";
                mainToolBar.Items.FromKeyButton("Today").Selected = false;
                mainToolBar.Items.FromKeyButton("Yesterday").Selected = false;
                mainToolBar.Items.FromKeyButton("LastWeek").Selected = false;
            }
            /**** For PL selection 3.5 release****/
            if (GetSelPLs() == null)
            {
                RegisterStartupScript("hh", "<Script>alert('Please select the PLs')</script>");
                return;
            }
            
            
            eventsGrid.DataSource = GetDataSet();
            eventsGrid.DataBind();
            wPanelPL.Expanded = false;
        }
    }

    protected void eventsGrid_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        if (e.Row.Cells.FromKey("Culture").Value != null)
            e.Row.Cells.FromKey("Geography").Value = e.Row.Cells.FromKey("Culture").Value;
        else if (e.Row.Cells.FromKey("Country").Value != null)
            e.Row.Cells.FromKey("Geography").Value = e.Row.Cells.FromKey("Country").Value;
        else if (e.Row.Cells.FromKey("Language").Value != null)
            e.Row.Cells.FromKey("Geography").Value = e.Row.Cells.FromKey("Language").Value;
        else
            e.Row.Cells.FromKey("Geography").Value = HyperCatalog.Shared.SessionState.MasterCulture.Name;
        Infragistics.WebUI.UltraWebGrid.UltraGridCell dateCell = e.Row.Cells.FromKey("Date");
        dateCell.Value = HyperCatalog.Shared.SessionState.User.GMTTimeZone.ToLocalTime((DateTime)dateCell.Value).ToString();
        if (e.Row.Cells.FromKey("ItemDeleted").Text != "1")
        {
            Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("ItemName");
            cName.Text = "<a href='../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("ItemId").Text + "'\">" + cName.Text + "</a>";
        }
    }
    private void setRange(string key)
    {
        advancedToolBar.ClientSideEvents.InitializeToolbar = "showHideAdvancedToolBar();";
        switch (key)
        {
            case "Today":
                mainToolBar.Items.FromKeyButton("Today").Selected = true;
                mainToolBar.Items.FromKeyButton("Yesterday").Selected = false;
                mainToolBar.Items.FromKeyButton("LastWeek").Selected = false;
                startDate.Value = DateTime.Today.Date;
                endDate.Value = DateTime.Today.Date;
                break;
            case "Yesterday":
                mainToolBar.Items.FromKeyButton("Today").Selected = false;
                mainToolBar.Items.FromKeyButton("Yesterday").Selected = true;
                mainToolBar.Items.FromKeyButton("LastWeek").Selected = false;
                startDate.Value = DateTime.Today.AddDays(-1).Date;
                endDate.Value = DateTime.Today.AddDays(-1).Date;
                break;
            case "LastWeek":
                mainToolBar.Items.FromKeyButton("Today").Selected = false;
                mainToolBar.Items.FromKeyButton("Yesterday").Selected = false;
                mainToolBar.Items.FromKeyButton("LastWeek").Selected = true;
                startDate.Value = DateTime.Today.AddDays(-7).Date;
                endDate.Value = DateTime.Today.Date;
                break;
        }
    }

    protected void mainToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {   
        switch (be.Button.Key)
        {
            case "Export":
                /********************** HO CODE **********************************/
                //Utils.ExportToExcel(eventsGrid, "Content history", "Content history");
                /************************ GDIC CODE for Export Functionality ********************************/


                //string stringToExport=Utils.ExportToExcelFromGrid(dg, "BundlesInconsistencies", "BundlesInconsistencies",Page);
                ListItemCollection lstItemCol = new ListItemCollection();
                lstItemCol.Add(new ListItem("Selected PLs:", GetSelPLs() + ""));

                Utils.ExportToExcelFromGrid(eventsGrid, "Content History", "Content History", Page , lstItemCol, "Content History");

                /******************* New End *******************************/
                break;
        }
    }
    protected void advancedToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {

    }
    protected void csvLink_Click(object sender, EventArgs e)
    {
        DataSet ds;
        using (Database dbObj = Utils.GetMainDB())
        {
            ds = dbObj.RunSPReturnDataSet("_Report_ContentHistory",
                new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id),
                new SqlParameter("@StartDate", startDate.Value),
                new SqlParameter("@EndDate", ((DateTime)endDate.Value).AddDays(1)),
                new SqlParameter("@MineOnly", mainToolBar.Items.FromKeyButton("MineOnly").Selected));
            dbObj.CloseConnection();
        }
        if (ds != null)
        {
            hideColumns(ds);
            System.Text.StringBuilder sb = Utils.ExportDataTableToCSV(ds.Tables[0]);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] contentBytes = encoding.GetBytes(sb.ToString());

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
            Response.ContentType = "application/text";
            Response.AppendHeader("content-disposition", "attachment;filename=\"HistoryResults.csv\"; " +
                                  "size=" + sb.Length.ToString() + "; " +
                                  "creation-date=" + DateTime.Now.ToString("R") + "; " +
                                  "modification-date=" + DateTime.Now.ToString("R") + "; " +
                                  "read-date=" + DateTime.Now.ToString("R"));
            //Fix for CR 5109 - Prabhu R S
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            EnableViewState = false;
            Response.OutputStream.Write(contentBytes, 0, Convert.ToInt32(contentBytes.Length));
            Response.Flush();
            Response.End();
        }
        if (Session["HistoryDataSet"] != null) { ((DataSet)Session["HistoryDataSet"]).Dispose(); Session["HistoryDataSet"] = null; }
    }

    protected void excelLink_Click(object sender, EventArgs e)
    {
        DataSet ds;
        using (Database dbObj = Utils.GetMainDB())
        {
            ds = dbObj.RunSPReturnDataSet("_Report_ContentHistory",
                new SqlParameter("@UserId", HyperCatalog.Shared.SessionState.User.Id),
                new SqlParameter("@StartDate", startDate.Value),
                new SqlParameter("@EndDate", ((DateTime)endDate.Value).AddDays(1)),
                new SqlParameter("@MineOnly", mainToolBar.Items.FromKeyButton("MineOnly").Selected));
            dbObj.CloseConnection();
        }
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
                        sb.Append("<TH style=\"font-weight:bold;\">" + col.Caption + "</TH>");
                    sb.Append("</TR>");
                    isHeader = false;
                }

                sb.Append("<TR>");
                foreach (DataColumn col in ds.Tables[0].Columns)
                    sb.Append("<TD>" + UITools.HtmlEncode(row[col].ToString()) + "</TD>");
                sb.Append("</TR>");
            }
            sb.Append("</TABLE>");
            string stream = sb.ToString();

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment; filename=HistoryResults.xls");
            Response.ContentType = "application/vnd.ms-excel";
            //Fix for CR 5109 - Prabhu R S
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(stream).ToString());

            EnableViewState = false;
            Response.Write(stream);
            Response.End();
        }
        if (Session["HistoryDataSet"] != null) { ((DataSet)Session["HistoryDataSet"]).Dispose(); Session["HistoryDataSet"] = null; }
    }

    private void hideColumns(DataSet ds)
    {
        ds.Tables[0].Columns["Deleted"].ColumnMapping = MappingType.Hidden;
        ds.Tables[0].Columns["ItemId"].ColumnMapping = MappingType.Hidden;
        ds.Tables[0].Columns["UserId"].ColumnMapping = MappingType.Hidden;
        ds.Tables[0].Columns["ElementId"].ColumnMapping = MappingType.Hidden;
        ds.Tables[0].Columns["Description"].ColumnMapping = MappingType.Hidden;
        ds.Tables[0].Columns["CultureCode"].ColumnMapping = MappingType.Hidden;
        ds.Tables[0].Columns["CultureName"].ColumnMapping = MappingType.Hidden;
    }
    /**** For PL selection 3.5 release****/
    private string GetSelPLs()
    {
        string selPls = string.Empty;
        PLList checkedPLs = PLTree.GetCheckedPLs();
        foreach (PL pl in checkedPLs)
        {

            selPls = selPls + pl.Code + ",";

        }
        if (selPls != string.Empty)
        {
            selPls = selPls.Remove(selPls.Length - 1, 1);
            return selPls;
        }
        else
            return null;
    }
}
