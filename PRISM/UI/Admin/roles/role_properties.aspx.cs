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
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Data.SqlClient;
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL 24/10/2005)
#endregion

/// <summary>
/// Display role properties
///		--> Return to the list
///		--> Save new or modified role
///		--> Delete role (if it is not used)
/// </summary>
public partial class role_Properties : HCPage
{
	#region Declarations
	protected System.Web.UI.WebControls.HyperLink hlCreator;
	protected System.Web.UI.WebControls.Label Label6;
	protected System.Web.UI.WebControls.TextBox txtTemplateId;
	protected System.Web.UI.WebControls.TextBox txtTemplateName;
	protected System.Web.UI.WebControls.DropDownList dlTemplateType;
	protected System.Web.UI.WebControls.TextBox txtFileName;
	protected System.Web.UI.WebControls.RequiredFieldValidator rv1;
	
	private int roleId = -1;
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
				roleId = Convert.ToInt32(Request["r"]);

			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}

			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");  
		}
	}
	    
	protected void UpdateDataView()
	{
		if (roleId >= 0)
		{
			Role role = Role.GetByKey(roleId);
			if (role != null)
			{
        txtRoleId.Text = roleId.ToString();
				txtRoleName.Text = role.Name;
				txtDescription.Text = role.Description;

				panelId.Visible = true;
				Page.DataBind();
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: Role is null";
				lbError.Visible = true;
			}
		}
		else
		{
			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");

			panelId.Visible = false;
			Page.DataBind();
		}
	}

	private void Save()
	{
		Role role = null;
		if (!panelId.Visible)
		{
			// Create role
			role = new Role(-1, txtRoleName.Text, txtDescription.Text);
		}
		else
		{
			// Update role
			role = Role.GetByKey(Convert.ToInt32(txtRoleId.Text));
			if (role != null)
			{
				role.Name = txtRoleName.Text;
				role.Description = txtDescription.Text;
			}
		}

		if (role != null)
		{
			if (role.Save())
			{
        SessionState.ClearAppRoles();
        if (!panelId.Visible)
				{
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
				}
				else
				{
					lbError.Text = "Data saved!";
					lbError.CssClass = "hc_success";
					lbError.Visible = true;
				}
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = Role.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Role is null";
			lbError.Visible = true;
		}
	}

	private void Delete()
	{
		Role role = Role.GetByKey(Convert.ToInt32(txtRoleId.Text));
		if (role != null)
		{
			if (role.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
        SessionState.ClearAppRoles();
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>"); 
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = Role.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Role is null";
			lbError.Visible = true;
		}
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "save")
		{
			Save();
		}
		if (btn == "delete")
		{
			Delete();
		}    
	}
}
