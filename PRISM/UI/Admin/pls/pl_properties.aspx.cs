#region Uses
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
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;
#endregion

#region History
// create by Pervenche REY 19/09/2006
#endregion

/// <summary>
/// Display pl properties (only readonly)
///		--> Return to the list
/// </summary>
public partial class UI_Admin_pls_pl_properties : HCPage
{
  #region Declarations
  private string plCode = null;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    try
    {
      if (Request["p"] != null)
        plCode = Request["p"];

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
      lbMsg.Visible = false;
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }

  protected void UpdateDataView()
  {
    if (plCode != null)
    {
      PL p = PL.GetByKey(plCode);
      txtCode.Text = p.Code;
      txtPath.Text = p.Organization.Name + "/" + p.Group.Name + "/" + p.GBU.Name;
      txtName.Text = p.Name;
      cbActive.Checked = p.IsActive;
      cbActive.Enabled = !cbActive.Checked;
      ddlBusName.DataSource= getBusinesses().Tables[0];
      ddlBusName.DataTextField = "BusinessName";
      ddlBusName.DataValueField = "BusinessId";
      ddlBusName.DataBind();
      ddlBusName.Items.Insert(0, new ListItem("--- Select ---", "0"));
      if (p.BusinessId.Trim() == "")
      {
          ddlBusName.SelectedIndex = 0;
          cbPMActive.Checked = false;
      }
      else
      {
          ddlBusName.SelectedIndex = ddlBusName.Items.IndexOf(new ListItem(p.BusinessName.Trim(), p.BusinessId.Trim()));
          ddlBusName.Enabled = false;
          cbPMActive.Checked = true;
      }
      UITools.ShowToolBarButton(uwToolbar, "Save");
      if (!cbActive.Enabled)
      {
        //UITools.HideToolBarButton(uwToolbar, "Save");
        lbUsage.Text = "This PL is currently attached to item or input forms";
      }
      
    }
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "save")
    {
      Save();
    }
  }

    private void Save()
    {
        if (cbPMActive.Checked && !cbActive.Checked)
        {
            lbMsg.CssClass = "hc_error";
            lbMsg.Text = "Please check IsActive option";
            lbMsg.Visible = true;
            return;
        }
        if (ddlBusName.SelectedIndex == 0 && cbPMActive.Checked)
        {
            lbMsg.CssClass = "hc_error";
            lbMsg.Text = "Please select valid business name";
            lbMsg.Visible = true;
            return;
        }
        string updateSQL = "UPDATE BPL SET PLName = '"+txtName.Text.Trim()+"' , IsActive =  "+ (cbActive.Checked?1:0).ToString() +", ModifyDate = GETUTCDATE() WHERE PLCode = '"+txtCode.Text.Trim()+"' AND ( IsActive <> "+(cbActive.Checked?1:0).ToString()+" OR PLName <> '"+txtName.Text.Trim()+"' )";
        if (!cbPMActive.Checked)
            updateSQL = updateSQL + ";" + "DELETE FROM BusinessProductLines WHERE PLCode = '" + txtCode.Text.Trim() + "'";
        else if(ddlBusName.Enabled)
        {
            updateSQL = updateSQL + ";" + "INSERT INTO BusinessProductLines (BusinessId,PLCode,ModifyDate) VALUES ('" + ddlBusName.SelectedValue.Trim() + "','" + txtCode.Text.Trim() + "',GETUTCDATE())";
        }
        using (Database dbObj = Utils.GetMainDB())
        {
            dbObj.RunSQL(updateSQL);
            dbObj.CloseConnection();
            if (dbObj.LastError.Length > 0)
            {
                lbMsg.CssClass = "hc_error";
                lbMsg.Text = dbObj.LastError;
                lbMsg.Visible = true;
            }
            else
            {
                lbMsg.Text = "Data saved!";
                lbMsg.CssClass = "hc_success";
                lbMsg.Visible = true;
            }
        }
    }

    private DataSet getBusinesses()
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
                        return ds;
                    }
                }
                else
                {
                    lbMsg.CssClass = "hc_error";
                    lbMsg.Text = dbObj.LastError;
                    lbMsg.Visible = true;
                    return null;
                }

            }
        }
        return null;
        
    }

}
