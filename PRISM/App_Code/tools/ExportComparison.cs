#region uses
using System;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.Shared;
using System.Web.UI;
using System.Data;
#endregion

namespace HyperCatalog.UI
{
  /// <summary>
  /// this class allows exporting the grid of comparison process
  /// </summary>
    /* 
    * Fix for QC 2299: DCC: The exported file from compare with option was not getting exported correctly
    * Fixed by Prabhu
    * Date: 29 Jan 09
    * Release: PRISM 7.0.01
    * Code Change History:
    * ExportGrid Method re-written
    */

    public class ExportComparison
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    public ExportComparison(){}

    /// <summary>
    /// Export the data in Excel file
    /// </summary>
    /// <param name="itemId">Id of the item</param>
    /// <param name="inputFormId">Id of the input form</param>
    /// <param name="viewMandatory">Display mandatory or not</param>
    /// <param name="filter">Filter in the grid</param>
    /// <param name="page">Current page</param>
    public static void ExportGrid(DataSet ds, Page page)
    {
        // string contains html code to 
        System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);

        sb.Append("<html><head>");
        sb.Append("</head><body>");

        #region "Header"
        // item name
        sb.Append("<table border='1'>");
        //-- Item columns
        DataTable dtItems = ds.Tables[0];
        int nbCols = dtItems.Rows.Count;

        #region Export by
        sb.Append("<tr><td style='font-size: 14; font-weight: bold; background-color: lightgrey'>Exported by</td><td colspan='" + nbCols + "'>");
        sb.Append(SessionState.User.Email);
        sb.Append(" - <i>");
        sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatDate));
        sb.Append(" at ");
        sb.Append(SessionState.User.FormatUtcDate(DateTime.UtcNow, true, SessionState.User.FormatTime));
        sb.Append(" (");
        sb.Append(SessionState.CacheParams["AppName"].Value.ToString());
        sb.Append(")");
        sb.Append("</i></td></tr><tr><td></td></tr>");
        #endregion

        //-- First xmlname column
        int firstColSize = 180;
        int defaultColSize = 250;
        sb.Append("<tr><td style='font-size: 14; font-weight: bold; background-color: lightgrey'; colspan = 0; width='" + firstColSize + "'>&nbsp;</td>");
        foreach (DataRow dr in dtItems.Rows)
        {
            string colHeader = dr["ItemName"].ToString();
            if (dr["ItemNumber"].ToString() != string.Empty)
            {
                colHeader = dr["ItemNumber"].ToString();
            }
            if (nbCols > 4)
            {
                defaultColSize = 250;
            }
            sb.Append("<td align='center' style='font-size: 14; font-weight: bold; background-color: lightgrey'; colspan = 0; width='" + defaultColSize + "'>" + colHeader + "</td>");
        }
        sb.Append("</tr>");
        #endregion

        #region Add content
        string cell = string.Empty;
        DataTable dt = ds.Tables[1];
        string prevGroup = string.Empty, curGroup = string.Empty;
        string chunkValue = string.Empty;
        int colspan = 0, cellcolspan = 0;
        string cellColor = "#D0D0D0", defaultCellColor = "white";
        int i = 0;
        while (i < dt.Rows.Count)
        {
            curGroup = dt.Rows[i]["ContainerGroup"].ToString();
            if (prevGroup != curGroup)
            {
                // Add Group name
                sb.Append("<tr valign='top'><td width='" + (firstColSize + (defaultColSize * nbCols)) + "' wordwrap='true' style='font-weight:bold; color:white; background-color: #003366' colspan='" + nbCols + 1 + "'>" + curGroup + "</td></tr>");
                prevGroup = curGroup;
            }
            // Add Tag name
            cell = "<td style='font-weight:bold;background-color: " + cellColor + "; text-color: black;' wordwrap='true'>" + "[" + dt.Rows[i]["LevelId"].ToString() + "] " + dt.Rows[i]["tag"].ToString() + "</td>";
            // Add values
            chunkValue = dt.Rows[i]["ChunkValue"].ToString();
            if (chunkValue == Chunk.BlankValue)
            {
                chunkValue = Chunk.BlankText;
            }
            else
            {
                chunkValue = UITools.HtmlEncode(chunkValue);
            }
            cell += "<td style='background-color: " + defaultCellColor + "; text-color: black;' align='center' colspan='" + cellcolspan + "'>" + chunkValue + "</td>";
            sb.Append("<tr valign='top'>" + cell + "</tr>");
            i++;
        }
        ds.Dispose();
        #endregion

        sb.Append("</table>");
        sb.Append("</body></html>");

        string fileName = string.Empty;
        fileName += "Comparison.xls";
        string exportContent = sb.ToString();
        page.Response.Clear();
        page.Response.ClearContent();
        page.Response.ClearHeaders();
        page.Response.Charset = string.Empty;
        page.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        page.Response.ContentType = "application/vnd.ms-excel;";
        //Fix for CR 5109 - Prabhu R S
        page.Response.ContentEncoding = System.Text.Encoding.UTF8;
        page.EnableViewState = false;
        page.Response.Write(exportContent);
        page.Response.End();
    }
  }
}
