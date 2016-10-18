using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;

public partial class UI_Admin_pls_PL_Add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadGBUs();
            loadBusiness();
        }
        lbMsg.Visible = false;
    }
    
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "save")
        {
            Save();
            if(lbMsg.Text == "Data saved!")
                be.Button.Enabled = false;
        }
    }
    private void Save()
    {
        if (ddlGBUName.SelectedIndex == 0)
        {
            logError("Please select valid GBU Name");
            return;
        }
        if (txtPLCode.Text.Trim() == "")
        {
            logError("Please enter PLCode");
            return;
        }
        if (txtPLName.Text.Trim() == "")
        {
            logError("Please enter PL Name");
            return;
        }
        if (chkPMActive.Checked)
        {
            if (!chkIsActive.Checked)
            {
                logError("Please select valid GBU Name");
                return;
            }
            if(ddlBusName.SelectedIndex==0)
            {
                logError("Please select valid Business Name");
                return;
            }
        }
        if (PL.GetByKey(txtPLCode.Text.Trim()) != null)
        {
            logError("PL Code already exists");
            return;
        }
        if (txtPLCode.Text.Trim().Length > 2)
        {
            logError("Length of the PL Code should not be more than two.");
            return;
        }

        String insertQry = "INSERT INTO BPL SELECT DISTINCT OrgCode,GroupCode,GBUCode,'"+txtPLCode.Text.Trim()+"' AS 'PLCode',OrgName,GroupName,GBUName,'"+txtPLName.Text.Trim() +"' AS 'PLName','"+(chkIsActive.Checked?1:0).ToString()+"' AS 'IsActive',GETUTCDATE() AS 'ModifyDate' FROM BPL WHERE GBUCode = '"+ddlGBUName.SelectedValue.Trim()+"'";
        if(chkPMActive.Checked)
            insertQry = insertQry + ";" + "INSERT INTO BusinessProductLines (BusinessId,PLCode,ModifyDate) VALUES ('" + ddlBusName.SelectedValue.Trim() + "','" + txtPLCode.Text.Trim() + "',GETUTCDATE())";

        using (Database dbObj = Utils.GetMainDB())
        {
            dbObj.RunSQL(insertQry);
            dbObj.CloseConnection();
            if (dbObj.LastError.Length > 0)
            {
                logError(dbObj.LastError);
            }
            else
            {
                lbMsg.Text = "Data saved!";
                lbMsg.CssClass = "hc_success";
                lbMsg.Visible = true;
            }
        }
    }
    private void loadGBUs()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT * FROM View_PLGBUs ORDER BY GBUName", new SqlParameter[] { }))
            {
                dbObj.CloseConnection();
                if (dbObj.LastError == string.Empty)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlGBUName.DataSource = ds.Tables[0];
                        ddlGBUName.DataTextField = "GBUName";
                        ddlGBUName.DataValueField = "GBUCode";
                        ddlGBUName.DataBind();
                        ddlGBUName.Items.Insert(0, new ListItem("-- Select GBU Name --","0"));
                    }
                }
                else
                {
                    logError(dbObj.LastError);
                    
                }

            }
        }
        
    }

    private void loadBusiness()
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSQLReturnDataSet("SELECT BusinessId,BusinessName FROM Businesses", new SqlParameter[] { }))
            {
                dbObj.CloseConnection();
                if (dbObj.LastError == string.Empty)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlBusName.DataSource = ds.Tables[0];
                        ddlBusName.DataTextField = "BusinessName";
                        ddlBusName.DataValueField = "BusinessId";
                        ddlBusName.DataBind();
                        ddlBusName.Items.Insert(0, new ListItem("-- Select Business Name --", "0"));
                    }
                }
                else
                {
                    logError(dbObj.LastError);

                }

            }
        }

    }
    private void logError(string errMsg)
    {
        lbMsg.CssClass = "hc_error";
        lbMsg.Text = errMsg;
        lbMsg.Visible = true;
    }
}
