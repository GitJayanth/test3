
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
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Collections.Specialized;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using HyperCatalog.UI.Tools;



public partial class Attribute : System.Web.UI.Page
{

    protected int code
    {
        get { return (int)ViewState["code"]; }
        set { ViewState["code"] = value; }
    }

    #region Controls
    #endregion

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }
    private void InitializeComponent()
    {

    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        if (!Page.IsPostBack)
        {
            DataSet ds1 = new DataSet();
            this.code = 0;
            OrgName.Checked = true;
            populateLstBoxBusinessUnitName(code);
            ds1 = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNamesByAccessories", new SqlParameter[] { new SqlParameter("@Code", code) });
            BUType.Text = "OrgName";
            BUType.Visible = true;
            BusinessList.DataSource = ds1;
            if (this.code == 3)
            {
                BusinessList.DataValueField = "Code";
                BusinessList.DataTextField = "Code";
            }
            else
            {
                BusinessList.DataValueField = "Code";
                BusinessList.DataTextField = "BizName";
            }
            BusinessList.DataBind();
            BusinessList.Items.Insert(0, "All");
            BusinessList.SelectedIndex = 0;
            BusinessList.Visible = true;
            DataSet ds2 = new DataSet();
            ds2 = dbObj.RunSPReturnDataSet("_GetRegion");
            if (dbObj.LastError == string.Empty || ds2.Tables[0].Rows.Count > 0)
            {
                ddlGeography.DataSource = ds2.Tables[0].DefaultView;
                ddlGeography.DataTextField = "RegionName";
                ddlGeography.DataValueField = "ShortCode";
                ddlGeography.DataBind();
                ddlGeography.Visible = true;

            }

            dbObj.CloseConnection();
            dbObj.Dispose();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString, 1000);
        ErrorMsg.Visible = false;
        try
        {
            ErrorMsg.Visible = false;
            QMReportsClass Obj_QMReportsClass = new QMReportsClass();
            String Biz = Obj_QMReportsClass.getSelectedValues(BusinessList);
            if (Biz == null)
            {
                ErrorMsg.Text = "Invalid selection";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ClientScript", "alert('" + ErrorMsg.Text + "');", true);
            }
            DataSet ds4 = new DataSet();
            if (this.code == 3)
            {
                ds4 = dbObj.RunSPReturnDataSet("Report_GetAccessAttributePl",
                        new SqlParameter("@PLCode", Biz),
                        new SqlParameter("@ShortCode", ddlGeography.SelectedValue));
            }
            else
            {
                ds4 = dbObj.RunSPReturnDataSet("Report_GetAccessAttributeOrg",
                         new SqlParameter("@OrgId", Biz),
                         new SqlParameter("@ShortCode", ddlGeography.SelectedValue),
                         new SqlParameter("@Code", this.code));

            }

            if (ds4.Tables[0].Rows.Count > 0)
            {
                DataTable objDt = new DataTable();
                objDt.Columns.Add(new DataColumn("ProductNumber"));
                objDt.Columns.Add(new DataColumn("ProductName"));
                objDt.Columns.Add(new DataColumn("ProductLineNo"));
                objDt.Columns.Add(new DataColumn("Status"));
                objDt.Columns.Add(new DataColumn("LiveDate"));
                objDt.Columns.Add(new DataColumn("ObsoleteDate"));

                objDt.Columns.Add(new DataColumn(Convert.ToString(ds4.Tables[0].Rows[0]["ContainerName"])));


                for (int rowIndex = 1; rowIndex < ds4.Tables[0].Rows.Count; rowIndex++)
                {
                    string curColumn = ds4.Tables[0].Rows[rowIndex]["ContainerName"].ToString();

                    if (!objDt.Columns.Contains(curColumn))
                    {
                        objDt.Columns.Add(new DataColumn(curColumn));
                    }

                }

                for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                {

                    DataRow[] drCol = objDt.Select("ProductNumber='" + ds4.Tables[0].Rows[i]["ProductNumber"].ToString() + "'");

                    if (drCol.Length > 0)
                    {
                        drCol[0]["ProductNumber"] = ds4.Tables[0].Rows[i]["ProductNumber"];
                        drCol[0]["ProductName"] = ds4.Tables[0].Rows[i]["ProductName"];
                        drCol[0]["ProductLineNo"] = ds4.Tables[0].Rows[i]["ProductLineNo"];
                        drCol[0]["Status"] = ds4.Tables[0].Rows[i]["Status"];
                        drCol[0]["LiveDate"] = ds4.Tables[0].Rows[i]["LiveDate"];
                        drCol[0]["ObsoleteDate"] = ds4.Tables[0].Rows[i]["ObsoleteDate"];
                        drCol[0][ds4.Tables[0].Rows[i]["ContainerName"].ToString()] = ds4.Tables[0].Rows[i]["ChunkValue"];
                    }
                    else
                    {
                        DataRow newRow = objDt.NewRow();
                        newRow[0] = ds4.Tables[0].Rows[i]["ProductNumber"].ToString();
                        newRow[ds4.Tables[0].Rows[i]["ContainerName"].ToString()] = ds4.Tables[0].Rows[i]["ChunkValue"];
                        objDt.Rows.Add(newRow);
                    }

                    objDt.AcceptChanges();

                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder(string.Empty);
                sb.Append("<html><head>");
                sb.Append("</head><body style='font-size: 14; font-family:Arial Unicode MS'>");
                sb.Append("<table style='font-size: 14; font-family:Arial Unicode MS' border = '1'>");
                sb.Append("Accessories with Attributes Report" + "<br>");
                string BizList = Obj_QMReportsClass.GetSelectedBiz_Reg(1, Biz, code);
                sb.Append("Selected " + BUType.Text + ": " + BizList + "<br>");

                sb.Append("Selected Region :" + ddlGeography.SelectedItem + "<br>");

                sb.Append("Exported On: " + SessionState.User.FormatUtcDate(DateTime.UtcNow, true, "MM/dd/yyyy") + "<br>");

                sb.Append("Exported By: " + SessionState.User.FullName + "<br>");
                
                sb.Append("</TR>");

                #region "Header"

                DataColumnCollection tableColumns = objDt.Columns;
                DataRowCollection tableRows = objDt.Rows;
                sb.Append("<tr>");
                foreach (DataColumn c in tableColumns)
                {
                    sb.Append("<th style='font-size: 14; font-family:Arial Unicode MS'>" + c.ColumnName.ToString() + "</th>");
                }

                #endregion

                foreach (DataRow dr in objDt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn dc in objDt.Columns)
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
                Response.AddHeader("content-disposition", "attachment;filename=" + "Accessories With Attributes" + ".xls");
                Response.ContentType = "application/vnd.ms-excel;";
                //Fix for CR 5109 - Prabhu R S
                Response.ContentEncoding = System.Text.Encoding.UTF8; 
                EnableViewState = false;
                Response.Write(sb.ToString());
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


    protected void populateLstBoxBusinessUnitName(int code)
    {
        Database dbObj = new Database(HyperCatalog.Business.ApplicationSettings.Components["Datawarehouse_DB"].ConnectionString);
        BusinessList.DataSource = dbObj.RunSPReturnDataSet("dbo._GetAllBusinessNamesByAccessories", new SqlParameter[] { new SqlParameter("@Code", code) });
        if (code == 3)
        {
            BusinessList.DataValueField = "Code";
            BusinessList.DataTextField = "Code";
        }
        else
        {
            BusinessList.DataValueField = "Code";
            BusinessList.DataTextField = "BizName";
        }
        BusinessList.DataBind();
        BusinessList.Items.Insert(0, "All");
        BusinessList.SelectedIndex = 0;
        BusinessList.Visible = true;
    }


    protected void OrgName_CheckedChanged(object sender, EventArgs e)
    {
        if (OrgName.Checked)
        {
            this.code = 0;
            BUType.Text = "OrgName";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(0);
        }

    }

    protected void GroupName_CheckedChanged(object sender, EventArgs e)
    {
        if (GroupName.Checked)
        {
            this.code = 1;
            BUType.Text = "GroupName";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(1);
        }

    }

    protected void GBUName_CheckedChanged(object sender, EventArgs e)
    {
        if (GBUName.Checked)
        {
            this.code = 2;
            BUType.Text = "GBU Name";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(2);
        }

    }

    protected void PLName_CheckedChanged(object sender, EventArgs e)
    {
        if (PLName.Checked)
        {
            this.code = 3;
            BUType.Text = "PLCode";
            BUType.Visible = true;
            populateLstBoxBusinessUnitName(3);
        }

    }

    public string getSelectedValues(ListBox lstBox)
    {
        string selValues = string.Empty;

        if (lstBox.SelectedValue == "All")
        {
            for (int i = 1; i <= lstBox.Items.Count - 1; i++)
            {
                selValues += lstBox.Items[i].Value.Trim() + ",";
            }
        }
        else
        {
            for (int i = 1; i <= lstBox.Items.Count - 1; i++)
            {
                if (lstBox.Items[i].Selected)
                    selValues += lstBox.Items[i].Value.Trim() + ",";
            }
        }

        if (selValues.Length == 0)
        {
            return null;
        }
        else
        {

            selValues = selValues.Remove(selValues.Length - 1, 1);

            return selValues.Trim();
        }


    }
}

