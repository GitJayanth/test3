using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;


public partial class Report_Compat : System.Web.UI.Page
{
    protected int code
    {
        get { return (int)ViewState["code"]; }
        set { ViewState["code"] = value; }
    }

    protected int code1
    {
        get { return (int)ViewState["code1"]; }
        set { ViewState["code1"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        //DataSet ds = new DataSet();

        if (!Page.IsPostBack)
        {
            DataSet ds = new DataSet();
            ds = dbObj.RunSPReturnDataSet("_getLinks");
            if (dbObj.LastError == string.Empty || ds.Tables[0].Rows.Count > 0)
            {
                Linksdropdown.DataSource = ds.Tables[0].DefaultView;
                Linksdropdown.DataTextField = "LinkTypeName";
                Linksdropdown.DataValueField = "LinkTypeId";
                Linksdropdown.DataBind();
                Linksdropdown.Visible = true;
            }
            DataSet ds1 = new DataSet();
            this.code = 0;
            this.code1 = 0;
            ds1 = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNames", new SqlParameter[] { new SqlParameter("@Code", code) });
            BUType.Text = "OrgName";
            BUType.Visible = true;
            BusinessList.DataSource = ds1;
            BusinessList.DataValueField = "Code";
            BusinessList.DataTextField = "BizName";
            BusinessList.DataBind();
            BusinessList.Items.Insert(0, "All");
            BusinessList.SelectedIndex = 0;
            BusinessList.Visible = true;
            BUType1.Text = "OrgName";
            BUType1.Visible = true;
            BusinessList1.DataSource = ds1;
            BusinessList1.DataValueField = "Code";
            BusinessList1.DataTextField = "BizName";
            BusinessList1.DataBind();
            BusinessList1.Items.Insert(0, "All");
            BusinessList1.SelectedIndex = 0;
            BusinessList1.Visible = true;

            DataSet ds2 = new DataSet();
            ds2 = dbObj.RunSPReturnDataSet("_GetRegion");


            if (dbObj.LastError == string.Empty || ds2.Tables[0].Rows.Count > 0)
            {
                CulturesList.DataSource = ds2.Tables[0].DefaultView;
                CulturesList.DataTextField = "RegionName";
                CulturesList.DataValueField = "ShortCode";
                CulturesList.DataBind();
                CulturesList.Items.Insert(0, "All");
                CulturesList.SelectedIndex = 0;
                CulturesList.Visible = true;


                CulturesList1.DataSource = ds2.Tables[0].DefaultView;
                CulturesList1.DataTextField = "RegionName";
                CulturesList1.DataValueField = "ShortCode";
                CulturesList1.DataBind();
                CulturesList1.Items.Insert(0, "All");
                CulturesList1.SelectedIndex = 0;
                CulturesList1.Visible = true;
            }

        }


    }
    protected void submit1_Click(object sender, EventArgs e)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);

        ErrorMsg.Visible = false;
        try
        {
            ErrorMsg.Visible = false;
            DataSet ds3 = new DataSet();
            QMReportsClass QS = new QMReportsClass();
            String Biz = QS.getSelectedValues(BusinessList);
            String Cultures = QS.getSelectedValues(CulturesList);
            if (Biz == null || Cultures == null)
            {
                ErrorMsg.Text = "Invalid selection";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMsg.Text + "');", true);
                //ErrorMsg.Visible = true;

            }
            else
            {
		//QC 777- start- Declaring CSV separator as comma
                char csvSeparator = ',';
		//QC 777 end
                ds3 = dbObj.RunSPReturnDataSet("Compatibilities_Generate",
                      new SqlParameter("@LinkType", Linksdropdown.SelectedValue),
                      new SqlParameter("@Business", Biz),
                      new SqlParameter("@RegionName", Cultures),
                      new SqlParameter("@Code", code));
		
		// QC777 If the number of rows returned by the dataset is less than 65536, the report will be generated in excel format


                if (ds3.Tables[0].Rows.Count > 0 && ds3.Tables[0].Rows.Count<65536)
                {

                    string BizList = "";
                    string Regions = "";

                    QMReportsClass objQMReports = new QMReportsClass();
                    BizList = objQMReports.GetSelectedBiz_Reg(1, Biz, code);
                    Regions = objQMReports.GetSelectedBiz_Reg(0, Cultures, 0);
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
                    sb.Append("<html><head>");
                    //   sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=" + page.Request.ContentEncoding.WebName+ "'>");
                    sb.Append("</head><body style='font-size: 14; font-family:Arial Unicode MS'>");
                    // item name
                    sb.Append("<table style='font-size: 14; font-family:Arial Unicode MS' border = '1'>");
                    // sb.Append("<tr>");
                    sb.Append("PRODUCTS WITH COMPATIBILITIES REPORT" + "<br>");

                    sb.Append("Selected " + BUType.Text + ": " + BizList + "<br>");

                    sb.Append("Selected " + LinkType.Text + ": " + Linksdropdown.SelectedItem + "<br>");
                    sb.Append("Selected " + Label1.Text + ": " + Regions + "<br>");

                    sb.Append("Exported On: " + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy") + "<br>");
                    sb.Append("Generated By:" + SessionState.User.FullName + "<br>");
                    DataTable dt = ds3.Tables[0];
                    sb.Append("</TR>");

                    #region "Header"

                    DataColumnCollection tableColumns = ds3.Tables[0].Columns;
                    DataRowCollection tableRows = ds3.Tables[0].Rows;
                    sb.Append("<tr>");
                    foreach (DataColumn c in tableColumns)
                    {
                        sb.Append("<th>" + c.ColumnName.ToString() + "</th>");
                    }

                    #endregion

                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn dc in dt.Columns)
                        {

                            sb.Append("<td>" + dr[dc].ToString() + "</td>");

                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</body></html>");
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = string.Empty;
                    Response.AddHeader("content-disposition", "attachment;filename=" + "Products_With_Compatibilities" + ".xls");
                    Response.ContentType = "application/vnd.ms-excel;";
                    //page.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
                    //Fix for CR 5109 - Prabhu R S
                    Response.ContentEncoding = System.Text.Encoding.UTF8; //page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
                    EnableViewState = false;
                    Response.Write(sb.ToString());
                }
		// QC777 start If the number of rows returned by the dataset is greated than 65536, the report will be generated in CSV format
                else if (ds3.Tables[0].Rows.Count > 0 && ds3.Tables[0].Rows.Count > 65536)
                {
                    string BizList = "";
                    string Regions = "";

                    QMReportsClass objQMReports = new QMReportsClass();
                    BizList = objQMReports.GetSelectedBiz_Reg(1, Biz, code);
                    Regions = objQMReports.GetSelectedBiz_Reg(0, Cultures, 0);
                    ListItemCollection lstItemCol = new ListItemCollection();
                    lstItemCol.Add(new ListItem("Selected " + BUType.Text + ": " + BizList + ""));
                    lstItemCol.Add(new ListItem("Selected " + LinkType.Text + ": " + Linksdropdown.SelectedItem + ""));
                    lstItemCol.Add(new ListItem("Selected " + Label1.Text + ": " + Regions + ""));

                    string s = Utils.ExportDataTableToCSVForSpecificReport(ds3.Tables[0], csvSeparator.ToString(), lstItemCol, "PRODUCTS WITH COMPATIBILITIES REPORT").ToString();

                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    byte[] contentBytes = encoding.GetBytes(s);

                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
                    Response.ContentType = "application/txt";
                    //Fix for CR 5109 - Prabhu R S
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.AppendHeader("Content-Disposition", "attachment;filename=\"Products_With_Compatibilities.csv\"; " +
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
		//QC 777 end

                else
                {
                    ErrorMsg.Text = "No Records to be exported";
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMsg.Text + "');", true);
                    //ErrorMsg.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }

    }
    protected void submit2_Click(object sender, EventArgs e)
    {
        try
        {
            ErrorMsg1.Visible = false;
            Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
            DataSet ds = new DataSet();
            QMReportsClass QS = new QMReportsClass();
            String Biz1 = QS.getSelectedValues(BusinessList1);
            String Cultures1 = QS.getSelectedValues(CulturesList1);
            if (Biz1.Length == 0 || Cultures1.Length == 0)
            {
                ErrorMsg1.Text = "Invalid selection";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMsg1.Text + "');", true);
                //ErrorMsg.Visible = true;

            }
            else
            {
                ds = dbObj.RunSPReturnDataSet("Non_Compatibilities_Generate",
                    new SqlParameter("@Business", Biz1),
                    new SqlParameter("@RegionName", Cultures1),
                    new SqlParameter("@Code", this.code1));

// QC777 If the number of rows returned by the dataset is less than 65536, the report will be generated in excel format


                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count < 65536)
                {

                    string BizList1 = "";
                    string Regions1 = "";

                    QMReportsClass objQMReports = new QMReportsClass();
                    BizList1 = objQMReports.GetSelectedBiz_Reg(1, Biz1, this.code1);
                    Regions1 = objQMReports.GetSelectedBiz_Reg(0, Cultures1, 0);

                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
                    sb.Append("<html><head>");
                    //   sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=" + page.Request.ContentEncoding.WebName+ "'>");
                    sb.Append("</head><body style='font-size: 14; font-family:Arial Unicode MS'>");
                    // item name
                    sb.Append("<table style='font-size: 14; font-family:Arial Unicode MS' border = '1'>");
                    // sb.Append("<tr>");
                    sb.Append("PRODUCTS WITHOUT COMPATIBILITIES REPORT" + "<br>");

                    sb.Append("Selected " + BUType1.Text + ": " + BizList1 + "<br>");

                    sb.Append("Selected " + Label2.Text + ": " + Regions1 + "<br>");

                    sb.Append("Exported On: " + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy") + "<br>");
                    sb.Append("Generated By:" + SessionState.User.FullName + "<br>");

                    DataTable dt = ds.Tables[0];
                    sb.Append("</TR>");

                    #region "Header"

                    DataColumnCollection tableColumns = ds.Tables[0].Columns;
                    DataRowCollection tableRows = ds.Tables[0].Rows;
                    sb.Append("<tr>");
                    foreach (DataColumn c in tableColumns)
                    {
                        sb.Append("<th>" + c.ColumnName.ToString() + "</th>");
                    }

                    #endregion

                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn dc in dt.Columns)
                        {

                            sb.Append("<td>" + dr[dc].ToString() + "</td>");

                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("</body></html>");
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = string.Empty;
                    Response.AddHeader("content-disposition", "attachment;filename=" + "Products_Without_Compatibilities" + ".xls");
                    Response.ContentType = "application/vnd.ms-excel;";
                    //page.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
                    //Fix for CR 5109 - Prabhu R S
                    Response.ContentEncoding = System.Text.Encoding.UTF8;  //page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
                    EnableViewState = false;
                    Response.Write(sb.ToString());
                }

		// QC777 If the number of rows returned by the dataset is greater than 65536, the report will be generated in CSV format
                else if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count > 65536)
                {
                    char csvSeparator = ',';
                    string BizList1 = "";
                    string Regions1 = "";

                    QMReportsClass objQMReports = new QMReportsClass();
                    BizList1 = objQMReports.GetSelectedBiz_Reg(1, Biz1, code1);
                    Regions1 = objQMReports.GetSelectedBiz_Reg(0, Cultures1, 0);
                    ListItemCollection lstItemCol = new ListItemCollection();
                    lstItemCol.Add(new ListItem("Selected " + BUType1.Text + ": " + BizList1 + ""));
                    lstItemCol.Add(new ListItem("Selected " + Label2.Text + ": " + Regions1 + ""));

                    string s = Utils.ExportDataTableToCSVForSpecificReport(ds.Tables[0], csvSeparator.ToString(), lstItemCol, "PRODUCTS WITHOUT COMPATIBILITIES REPORT").ToString();

                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    byte[] contentBytes = encoding.GetBytes(s);

                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
                    Response.ContentType = "application/txt";
                    //Fix for CR 5109 - Prabhu R S
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.AppendHeader("Content-Disposition", "attachment;filename=\"Products_Without_Compatibilities.csv\"; " +
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
		//QC 777 end

                else
                {
                    ErrorMsg1.Text = "No Records to be exported";
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMsg1.Text + "');", true);
                    //ErrorMsg1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }



    protected void populateLstBoxBusinessUnitName(int code, string str)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        if (str.Equals("First"))
        {
            BusinessList.DataSource = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNames", new SqlParameter[] { new SqlParameter("@Code", code) });
            BusinessList.DataValueField = "Code";
            BusinessList.DataTextField = "BizName";
            BusinessList.DataBind();
            BusinessList.Items.Insert(0, "All");
            BusinessList.SelectedIndex = 0;
            BusinessList.Visible = true;
        }
        if (str.Equals("Second"))
        {
            BusinessList1.DataSource = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNames", new SqlParameter[] { new SqlParameter("@Code", code) });
            BusinessList1.DataValueField = "Code";
            BusinessList1.DataTextField = "BizName";
            BusinessList1.DataBind();
            BusinessList1.Items.Insert(0, "All");
            BusinessList1.SelectedIndex = 0;
            BusinessList1.Visible = true;
        }

    }





    protected void OrgName_CheckedChanged(object sender, EventArgs e)
    {
        if (OrgName.Checked)
        {
            this.code = 0;
            BUType.Text = "OrgName";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(0, "First");

        }
        if (OrgName1.Checked)
        {
            this.code1 = 0;
            BUType1.Text = "OrgName";
            BUType1.Visible = true;
            populateLstBoxBusinessUnitName(0, "Second");
        }
    }
    protected void GroupName_CheckedChanged(object sender, EventArgs e)
    {
        if (GroupName.Checked)
        {
            this.code = 1;
            BUType.Text = "GroupName";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(1, "First");
        }
        if (GroupName1.Checked)
        {
            this.code1 = 1;
            BUType1.Text = "GroupName";
            BUType1.Visible = true;
            populateLstBoxBusinessUnitName(1, "Second");
        }
    }
    protected void GBUName_CheckedChanged(object sender, EventArgs e)
    {
        if (GBUName.Checked)
        {
            this.code = 2;
            BUType.Text = "GBU Name";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(2, "First");
        }
        if (GBUName1.Checked)
        {
            this.code1 = 2;
            BUType1.Text = "GBU Name";
            BUType1.Visible = true;
            populateLstBoxBusinessUnitName(2, "Second");
        }
    }
}
