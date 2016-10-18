/*
 History :
 * Modified the Existing code and added a new functionality for CAS Link Export.
 * Author : Radha S
 * Date : 21/12/2012
 */
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
using Infragistics.Documents.Excel;
using System.IO;

public partial class Report_NodeLevelCompat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PR659863 - Modified the existing code to differentiate between the Link Export and Cas Link Export by Radha S
            string strLink = Request["export"].ToString();
            Int32 linkItemId = Convert.ToInt32(Request["i"].ToString());
            string strCulture = Request["c"].ToString();
            if (strLink == "linkExport")
            {
                LinkExport(linkItemId, strCulture);
            }
            else
            {
                CasforLinkExport(linkItemId, strCulture);
            }
            if (hfcon.Value == "yes")
            {
                emptyExcel(linkItemId, strCulture);
            }
    }
    public void LinkExport(int linkItemId, string strCulture)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
        DataSet ds = new DataSet();
        int lnkItem = linkItemId;
        string strCultureCode = strCulture;
        DataSet d = new DataSet();
        d = dbObj.RunSQLReturnDataSet("Select dbo.GetItemName(" + lnkItem.ToString() + ")");
        ds = dbObj.RunSPReturnDataSet("Node_Level_Compatibilities",
            new SqlParameter("@ItemId", Convert.ToInt32(lnkItem.ToString())),
            new SqlParameter("@CultureId", strCultureCode));

        if (ds.Tables[0].Rows.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
            sb.Append("<html><head>");
            sb.Append("</head><body>");
            sb.Append("<table border = '1'>");
            sb.Append("Link Export <br>");
            DataRowCollection tableRows = d.Tables[0].Rows;
            foreach (DataRow row in tableRows)
            {
                object[] rowItems = row.ItemArray;
                sb.Append("Host Name :" + rowItems[0].ToString() + "<br>");
                sb.Append("Culture: " + Request["c"].ToString() + "<br>");
            }
            sb.Append("Exported on: " + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy:HH:mm:ss") + "<br>");
            DataTable dt = ds.Tables[0];
            sb.Append("</TR>");

            DataColumnCollection tableColumns = ds.Tables[0].Columns;
            sb.Append("<tr>");
            foreach (DataColumn c in tableColumns)
            {
                sb.Append("<th>" + c.ColumnName.ToString() + "</th>");
            }
            sb.Append("</tr>");
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
            string fileName = string.Empty;
            fileName += "LinkExport.xls";
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = string.Empty;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/vnd.ms-excel;";
            //Fix for CR 5109 - Prabhu R S
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            EnableViewState = false;
            Response.Write(sb.ToString());
            Response.End();
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('There are no links associated with this node');window.close();", true);
        }
    }
    #region CAS Link Export
    // - CAS Link Export - PR659863
    public void CasforLinkExport(int linkItemId, string strCulture)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
        DataSet ds = new DataSet();
        string strCultureCode = strCulture;
        Int32 lnkItemId = linkItemId;
        DataSet d = new DataSet();
        //Retrieving the Item name and all Created links
        d = dbObj.RunSQLReturnDataSet("select dbo.GetItemName(" + lnkItemId + "),LevelId from Items where ItemId = '" + lnkItemId + "'");
        //Retrieve the links created at that host level
        ds = dbObj.RunSPReturnDataSet("_CASLink_GetContent",
            new SqlParameter("@ItemId", lnkItemId),
            new SqlParameter("@CultureCode", strCultureCode));
        if (ds.Tables[0].Rows.Count > 0)
        {
            formatExcel(ds, d, strCultureCode);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "ClientScript", "excelDownload()", true);
        }
    }
    #endregion
    #region - Creating an empty excel if there are no links at host items
    //Creating Empty Excel if there are no links at Host item
    public void emptyExcel(int linkItemId, string strCulture)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Crystal_DB"].ConnectionString);
        DataSet ds = new DataSet(); ;
        string strCultureCode = strCulture;
        Int32 lnkItemId = linkItemId;
        DataSet d = new DataSet();
        //Retrieving the Item name and level id
        d = dbObj.RunSQLReturnDataSet("select dbo.GetItemName(" + lnkItemId + "),LevelId from Items where ItemId = '" + lnkItemId + "'");
        ds = dbObj.RunSPReturnDataSet("_CASLink_GetContent",
            new SqlParameter("@ItemId", lnkItemId),
            new SqlParameter("@CultureCode", strCultureCode));
        formatExcel(ds, d, strCultureCode);
    }
    #endregion
    #region Formatting the excel and append the value to the excel
    public void formatExcel(DataSet ds, DataSet d, string strCultureCode)
    {
        Infragistics.Documents.Excel.Workbook workbook = new Infragistics.Documents.Excel.Workbook();
        int rowIndex;
        foreach (DataTable dt in ds.Tables)
        {
            Infragistics.Documents.Excel.Worksheet worksheet = workbook.Worksheets.Add(dt.TableName);
            DataRowCollection tableRows = d.Tables[0].Rows;
            for (int colindex = 0; colindex < dt.Columns.Count; colindex++)
            {
                worksheet.Columns[colindex].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                #region Column Section
                worksheet.Rows[0].Cells[colindex].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                worksheet.Rows[0].Cells[colindex].CellFormat.Alignment = HorizontalCellAlignment.Center;
                worksheet.Rows[0].Cells[0].Value = dt.Columns[0].ColumnName;
                worksheet.Rows[0].Cells[1].Value = dt.Columns[1].ColumnName;
                worksheet.Rows[0].Cells[2].Value = dt.Columns[2].ColumnName;
                worksheet.Rows[0].Cells[3].Value = dt.Columns[3].ColumnName;
                worksheet.Rows[0].Cells[4].Value = dt.Columns[4].ColumnName;
                worksheet.Rows[0].Cells[5].Value = dt.Columns[5].ColumnName;
                worksheet.Rows[0].Cells[6].Value = dt.Columns[6].ColumnName;
                worksheet.Rows[0].Cells[7].Value = dt.Columns[7].ColumnName;
                worksheet.Rows[0].Cells[8].Value = dt.Columns[8].ColumnName;
                worksheet.Rows[0].Cells[9].Value = dt.Columns[9].ColumnName;
                #endregion
            }
            #region Row Section
            rowIndex = 1;
            foreach (DataRow dr in dt.Rows)
            {
                worksheet.Rows[rowIndex].CellFormat.Font.Bold = ExcelDefaultableBoolean.False;
                Infragistics.Documents.Excel.WorksheetRow row = worksheet.Rows[rowIndex++];

                for (int colIndex = 0; colIndex < dr.ItemArray.Length; colIndex++)
                {
                    row.Cells[colIndex].Value = dr.ItemArray[colIndex];
                }
            }
            #endregion
        }
        string fileName = string.Empty;
        string formatFileName = DateTime.Now.ToString("yyyyMMdd") + "-" + DateTime.Now.ToString("HHmmss");
        DataRowCollection tableRow = d.Tables[0].Rows;
        foreach (DataRow row in tableRow)
        {
            string hostName = row[0].ToString();
            fileName += "CASLinkExport-" + hostName + "-" + strCultureCode + "-" + SessionState.User.Id.ToString() + "-" + formatFileName + ".xls";
        }
        workbook.ActiveWorksheet = workbook.Worksheets[0];
        MemoryStream stream = new MemoryStream();
        BIFF8Writer.WriteWorkbookToStream(workbook, stream);
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        Response.ContentType = "application/ms-excel";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("Content-Length", stream.Length.ToString());
        EnableViewState = false;
        Response.OutputStream.Write(stream.ToArray(), 0, Convert.ToInt32(stream.Length));
        Response.End();
    }
    #endregion
}