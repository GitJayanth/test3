#region Uses
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
using HyperComponents.Data.dbAccess;
using System.IO;
using System.Data.SqlClient;
using HyperCatalog.Shared;
#endregion

public partial class UI_Collaborate_SpecificReports : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {

  }

  public void ExtractFlatCatego(object sender, ImageClickEventArgs e)
  {
    char csvSeparator = (char)9;
    using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 240))
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("_Report_GetFlatCategorization", "Categorization",
        new SqlParameter("@ClassId", 0),
        new SqlParameter("@WithObso", Convert.ToInt32(cbWithObso.Checked)),
        new SqlParameter("@WithPL", Convert.ToInt32(cbWithPL.Checked)),
        new SqlParameter("@WithPLC", Convert.ToInt32(cbWithPLC.Checked))))
      {
        dbObj.CloseConnection();
        if ((ds.Tables[0].Rows.Count > 0) && (dbObj.LastError == string.Empty))
        {
            /****************** HO CODE **********************/
          //string s = Utils.ExportDataTableToCSV(ds.Tables[0], csvSeparator.ToString()).ToString();
            /****************** GDIC CODE **********************/
          string s = Utils.ExportDataTableToCSVForSpecificReport(ds.Tables[0], csvSeparator.ToString(), null,"Specific Reports").ToString();
            /****************************************************/
          //Fix for CR 5109 - Prabhu R S
          System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
          //System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(0);
          byte[] contentBytes = encoding.GetBytes(s);
          Response.ContentEncoding = System.Text.Encoding.UTF8;
          Response.Clear();
          Response.ClearContent();
          Response.ClearHeaders();
          Response.AddHeader("Accept-Header", contentBytes.Length.ToString());
          Response.ContentType = "application/text";
          Response.AppendHeader("Content-Disposition", "attachment;filename=\"FlatCategorization.txt\"; " +
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
        else
        {
          lbError.Text = (dbObj.LastError != string.Empty) ? "Error: " + dbObj.LastError : "No data retrieved";
          lbError.CssClass = "hc_error";
          lbError.Visible = true;
        }
      }
    }
  }

}
