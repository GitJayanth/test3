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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using HyperCatalog.Business;
#endregion

/// <summary>
/// Display user list having the selected role
///		--> Return to the role list
///		--> Export in Excel
///		--> Filter on all fields of the grid
/// </summary>
public partial class role_users : HCPage
{
	#region Declarations
	protected System.Web.UI.WebControls.Button BtnCancel;
	protected System.Web.UI.WebControls.DataList rights;
	
	private int roleId = -1;
	#endregion
    
	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		InitializeComponent();
		txtFilter.AutoPostBack = false;
		txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); 
    Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
		base.OnInit(e);
	}
		
	/// <summary>
	/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
	/// le contenu de cette méthode avec l'éditeur de code.
	/// </summary>
	private void InitializeComponent()
	{    
		this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
		this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

	}
	#endregion
    
	protected void Page_Load(object sender, System.EventArgs e)
	{
		try
		{
			      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
if (Request["r"] != null)
				roleId = Convert.ToInt32(Request["r"]);

			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}
		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");
		}
	}
    
	void UpdateDataView()
	{
		HyperCatalog.Business.Role role = HyperCatalog.Business.Role.GetByKey(roleId);
		if (role != null)
		{
			HyperCatalog.Business.UserList users = role.Users;

			if (users != null)
			{
				dg.DataSource = users;
				Utils.InitGridSort(ref dg, true);
				dg.DataBind();

				if (dg.Rows.Count > 0)
				{
					dg.Visible = true;
					lbNoresults.Visible = false;
				}
				else
				{
					if (txtFilter.Text.Length > 0)
						lbNoresults.Text = "No record match your search ("+txtFilter.Text+")";
					dg.Visible = false;
					lbNoresults.Visible = true;
				}
			}
		}
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "export")
		{
			Utils.ExportToExcel(dg, "Role_Users", "Role-" + Role.GetByKey(roleId).Name);
		}
	}

	private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		if (txtFilter.Text.Length > 0)
		{
			Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;

			string filter = txtFilter.Text.Trim().ToLower();
			string userName = r.Cells.FromKey("UserName").Value.ToString().ToLower();
			string orgName = r.Cells.FromKey("OrgName").Value.ToString().ToLower();

			if ((userName.Length == 0 || userName.IndexOf(filter) < 0) && (orgName.Length == 0 || orgName.IndexOf(filter) < 0))
				dg.Rows.Remove(r);
			else
				UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
		}
	}
}
