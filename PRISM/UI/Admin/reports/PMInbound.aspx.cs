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
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;


public partial class UI_Admin_PMInbound : System.Web.UI.Page
 {
     String FileD = "";
     protected void Page_Load(object sender, EventArgs e)
     {
         //String Filepath = "";
         Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);

         //SqlDataReader rr = dbObj.RunSQLReturnRS("SELECT [Value] FROM SB_appParameters WHERE [Name] LIKE 'Report_FilePath'");
         //while (rr.Read())
         //{
         //    Filepath = rr.GetString(0);
         //}
         // FileD = Filepath + "Delivery.xls";
         //ErrorMsg.Visible = false;
         if (!Page.IsPostBack)
         {

             DCDate.Value = DateTime.UtcNow.ToString("MM/dd/yyyy");
             DCDate.MaxDate = DateTime.Today;
             DCDate.MinDate = DCDate.MaxDate.Subtract(new TimeSpan(30, 0, 0, 0));


             DataSet ds1 = new DataSet();
             ds1 = dbObj.RunSPReturnDataSet("_GetAllBusinesses");
             if (dbObj.LastError == string.Empty || ds1.Tables[0].Rows.Count > 0)
             {
                 ddlBusinessUnit.DataSource = ds1.Tables[0].DefaultView;
                 ddlBusinessUnit.DataTextField = "BusinessName";
                 ddlBusinessUnit.DataValueField = "BusinessId";
                 ddlBusinessUnit.DataBind();
                 ddlBusinessUnit.Visible = true;
             }
         }
     }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        try
        {
            DataSet ds3 = new DataSet();
            ds3 = dbObj.RunSPReturnDataSet("Report_GetPM_Inbound",
                      new SqlParameter("@BusinessId", ddlBusinessUnit.SelectedValue),
                      new SqlParameter("@Date", Convert.ToDateTime(DCDate.Text).ToString("MM/dd/yyyy")));

            if (ds3.Tables[0].Rows.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
                sb.Append("<html><head>");
                sb.Append("</head><body>");
                sb.Append("<table border = '1'>");
                sb.Append("PMInboundReport <br>");
                sb.Append("Selected Business Unit" + " : " + ddlBusinessUnit.SelectedItem + "<br>");
                sb.Append("Date" + " : " + Convert.ToDateTime(DCDate.Text).ToString("MM/dd/yyyy") + "<br>");
                sb.Append("Exported By" + " : " + HyperCatalog.Shared.SessionState.User.FullName + "<br>");
                DataTable dt = ds3.Tables[0];
                sb.Append("</TR>");

                DataColumnCollection tableColumns = ds3.Tables[0].Columns;
                //DataRowCollection tableRows = ds3.Tables[0].Rows;
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
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = string.Empty;
                Response.AddHeader("content-disposition", "attachment;filename=PMInboundReport.xls");
                Response.ContentType = "application/vnd.ms-excel;";
                //page.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
                //Fix for CR 5109 - Prabhu R S
                Response.ContentEncoding = System.Text.Encoding.UTF8;      //page.Response.AppendHeader("Content-Length", System.Text.Encoding.UTF8.GetByteCount(exportContent).ToString());
                EnableViewState = false;
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                ErrorMsg.Text = "No Records to be exported";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMsg.Text + "');", true);

            }
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }

    }

  }
