#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using Reporting;


#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL 24/10/2005)
//  Changes incorporated for CR 5110 - Remediate the PRISM Query Tool ( Vivek Chandran Nair 12/01/2009)
// 1) Appending of the query string with nolock and row restriction commands
// 2) Exectuion of the query using Crystal_ReadOnly user
// 3) Setting the query to a property of the newly introduced class : ReportQuery
// 4) Restriction on the display rows increased to 4000
// 5) Modification of the messages displayed for Save/Syntax check and Run options of query tool
#endregion

/// <summary>
/// Display report properties
///		--> Return to the report list
///		--> Check the syntax
///		--> Run report
///		--> Save new or modified report
///		--> Delete selected report
/// </summary>
public partial class Report_Properties : HCPage
{
	#region Declarations

	private int reportId = -1;
	private bool showRoles = false;
	private Report rep = null;
	#endregion

	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		//
		// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
		//
		InitializeComponent();
		base.OnInit(e);
	}
		
	/// <summary>
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{    
		this.rolesDataList.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.roles_ItemDataBound);
		this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

	}
	#endregion
  
	protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
		{
			uwToolbar.Items.FromKeyButton("Save").Enabled = false;
			uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }
    #endregion

    try
		{
			if (Request["r"] != null)
				reportId = Convert.ToInt32(Request["r"]);

			if (Request["s"]!=null)
				showRoles = true;
			
			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}
      Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "InitMasterPage", "<script>txtCodeElement= '" + txtCode.ClientID + "'; txtReportId = '"+txtId.ClientID+"'</script>");
		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(Page.GetType(),"clientScript", "<script>back();</script>");    
		}
	}

	private void UpdateDataView()
	{
		panelDesc.Visible = false;
		panelRoles.Visible = false;
		panelSql.Visible = false;
		hlCreator.Visible = false;
    panelId.Visible = false;
    
		UITools.HideToolBarButton(uwToolbar, "Delete");
		UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");

		if (reportId>=0)
		{
			rep = Report.GetByKey(reportId);
			if (rep!=null)
			{
				UITools.ShowToolBarButton(uwToolbar, "Delete");
				UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");

				if (!showRoles)
				{
					UITools.ShowToolBarButton(uwToolbar, "Syntax");
					UITools.ShowToolBarButton(uwToolbar, "Run");
					UITools.ShowToolBarSeparator(uwToolbar, "SyntaxSep");

					panelDesc.Visible = true;
					panelSql.Visible = true;

          txtId.Text = rep.Id.ToString();
					txtCode.Text = rep.Code;
					txtDescription.Text = rep.Description;
					txtName.Text = rep.Name;
					hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(rep.Creator.Email);
					hlCreator.Text  = "Created by " + rep.Creator.FirstName + " " + rep.Creator.LastName  + " on " + rep.CreateDate.ToString() + "<br/><br/>";
					hlCreator.Visible = true;
          panelId.Visible = true;

					Page.DataBind();
				}
				else
				{
					UITools.HideToolBarButton(uwToolbar, "Syntax");
					UITools.HideToolBarButton(uwToolbar, "Run");
					UITools.HideToolBarSeparator(uwToolbar, "SyntaxSep");

					panelRoles.Visible = true;

					rolesDataList.DataSource = Role.GetAll();
					rolesDataList.DataBind();

					Page.DataBind();
				}
			}
			else
			{
				txtCode.Text = string.Empty;
				txtDescription.Text = string.Empty;
				txtName.Text = string.Empty;
			}
		}
		else
		{
			txtCode.Text = string.Empty;
			txtDescription.Text = string.Empty;
			txtName.Text = string.Empty;

			panelSql.Visible = true;
			Page.DataBind();
		}
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "syntax")
		{
			Database dbObj = new  Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString);
			string sql = "SET FMTONLY ON\n" + txtCode.Text + "\nSET FMTONLY OFF";
			dbObj.RunSQL(sql);
			dbObj.CloseConnection();
			if (dbObj.LastError.Length == 0)
			{
                lbError.Text = "Syntax check successful! Please note: Queries are only allowed to perform Select operations against the database (Read only). Queries containing Create, Insert ,Update or Delete statements will be blocked and will error out on 'Run'";
				lbError.CssClass = "hc_success";
				lbError.Visible = true;
			}
			else
			{
				lbError.Text = dbObj.LastError;
				lbError.CssClass = "hc_error";
				lbError.Visible = true;
			}
		}
        else if (btn == "run")
        {
            //  Changes incorporated for CR 5110 - Remediate the PRISM Query Tool ( Vivek Chandran Nair 12/01/2009)
            ReportQuery RQ = new ReportQuery();
            RQ.PSQLQuery = "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ \n SET ROWCOUNT " + ApplicationSettings.Parameters["QueryReportResultLimit"].Value.ToString() + "\n" + txtCode.Text + "\n set ROWCOUNT 0";//\n GO \n BEGIN TRANSACTION  +"\n COMMIT TRANSACTION";  
              Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SQLResultPage", "<script>open('Report_Result.aspx?q=sss','queryprocess','toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,width=800,height=600');</script>");
        }
        else if (btn == "delete")
        {
            if (Report.DeleteByKey(reportId, HyperCatalog.Shared.SessionState.User.Id))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
            }
            else
            {
                lbError.CssClass = "hc_success";
                lbError.Text = "Error: delete failed. The report can't be deleted";
                lbError.Visible = true;
            }
        }
        else if (btn == "save")
        {
            // Test if user is saving a new query          
            if (panelSql.Visible && !panelDesc.Visible)
            {
                panelSql.Visible = false;
                panelDesc.Visible = true;

                rolesDataList.DataSource = Role.GetAll();
                rolesDataList.DataBind();

                UITools.HideToolBarButton(uwToolbar, "Syntax");
                UITools.HideToolBarButton(uwToolbar, "Run");
                UITools.HideToolBarSeparator(uwToolbar, "SyntaxSep");

                Page.DataBind();
            }

            // We are updating an existing report.
            // Test if we are updating description or roles.
            bool newRep = false;
            Report rep = null;
            if (reportId >= 0)
            {
                rep = Report.GetByKey(reportId);
            }
            else
            {
                newRep = true;
                rep = new Report(reportId, string.Empty, string.Empty, HyperCatalog.Shared.SessionState.User.Id, string.Empty, null, null);
            }
            if (panelDesc.Visible)
            {
                rep.Name = txtName.Text.Trim();
                rep.Description = txtDescription.Text.Trim();
                rep.Code = txtCode.Text.Trim();
            }
            if (panelRoles.Visible)
            {
                rep.Roles.Clear();
                for (int i = 0; i < rolesDataList.Items.Count; i++)
                {
                    if (((CheckBox)rolesDataList.Items[i].FindControl("rolename")).Checked)
                    {
                        rep.Roles.Add(Role.GetByKey(Convert.ToInt32(((TextBox)rolesDataList.Items[i].FindControl("roleid")).Text)));
                    }
                }
            }
            if (rep.Code.Length > 4000)
            {
                lbError.CssClass = "hc_error";
                lbError.Text = "Error: Query size limit is restricted to 4000 characters. The query will not be saved. You can still execute the query provided the Business rules are followed.";
                lbError.Visible = true;
                UITools.HideToolBarButton(uwToolbar, "Save");
            }
            else
            {
                if (rep.Save())
                {
                    if (newRep)
                    {
                        if (panelRoles.Visible) 
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>alert('Data is saved! Please note: Queries are only allowed to perform Select operations against the database (Read only). Queries containing Create, Insert ,Update or Delete statements will be blocked and will error out on Run');back();</script>");
                            lbError.CssClass = "hc_success"; ;
                            lbError.Text = "";
                            lbError.Visible = true;
                        }
                        panelRoles.Visible = true;
                    }
                    else
                    {
                        lbError.CssClass = "hc_success"; ;
                        lbError.Text = "Data is saved! Please note: Queries are only allowed to perform Select operations against the database (Read only). Queries containing Create, Insert ,Update or Delete statements will be blocked and will error out on 'Run'";
                        lbError.Visible = true;
                    }
                }
                else
                {
                    lbError.CssClass = "hc_error";
                    lbError.Text = "Error: add/update failed. The report can't be saved";
                    lbError.Visible = true;
                }
            }
        }
	}

	private void roles_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
	{
		CheckBox c = (CheckBox)e.Item.FindControl("rolename");      
		TextBox cId = (TextBox)e.Item.FindControl("roleid");      
		if (rep!=null)
		{
			foreach (Role r in rep.Roles)
			{
				if ((r.Id.ToString() == cId.Text) || cId.Text == "0" || cId.Text == "17")
				{
					c.Checked = true;
					break;
				}
			}
		}
		else
		{
			c.Checked = (cId.Text == "0" || cId.Text == "17"); // Admin and SQL roles are checked
		}
	}
}
