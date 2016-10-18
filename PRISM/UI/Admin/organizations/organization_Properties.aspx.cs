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
using System.Data.SqlClient;
using HyperCatalog.Shared;
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL 24/10/2005)
#endregion

/// <summary>
/// Display organization properties
///		--> Save new or modified organization
///		--> Delete organization
///		--> Return to the organization list
/// </summary>
public partial class organization_Properties : HCPage
{
	#region Declarations

	private int orgId = -1;
	#endregion
    
	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
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
			uwToolbar.Items.FromKeyButton("Delte").Enabled = false;
    }
    #endregion

    try
		{
			if (Request["o"] != null)
				orgId = Convert.ToInt32(Request["o"]);

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

    
	protected void UpdateDataView()
	{
		if (orgId >= 0)
		{
			Organization organization = Organization.GetByKey(orgId);
			if (organization != null)
			{
				// *************************************************************************
				// Display the organization
				// *************************************************************************
				txtOrgId.Text = organization.Id.ToString();
				txtOrgCode.Text = organization.Code;
				txtOrgName.Text = organization.Name;
				txtOrgType.Text = organization.Type;
				txtDescription.Text = organization.Description;

				panelId.Visible = true;
        txtOrgCode.Enabled = false;
			}
		}
		else
		{
      // Create new organization
      txtOrgCode.Enabled = true;
			panelId.Visible = false;

			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
		}
	}

	private void Save()
	{
		// Retrieve the new text in the templated columns

		// Prepare the command text
		bool newItem = false;

		Organization organization = null;
		if (orgId == -1)
		{
			organization = new Organization(orgId, txtOrgCode.Text, txtOrgName.Text, txtDescription.Text, txtOrgType.Text);
			newItem = true;
		}
		else
		{
			organization = Organization.GetByKey(orgId);
			if (organization != null)
			{
				organization.Code = txtOrgCode.Text;
				organization.Name = txtOrgName.Text;
				organization.Type = txtOrgType.Text;
				organization.Description = txtDescription.Text;
			}
		}

		if (organization.Save())
		{
      SessionState.ClearAppOrganizations();
      if (newItem)
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
			}
			else
			{
				lbError.CssClass = "hc_success";
				lbError.Text = "Data saved!";
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = Organization.LastError;
			lbError.Visible = true;
		}
	}

	private void Delete()
	{
		Organization organization = Organization.GetByKey(orgId);
		if (organization != null)
		{
			if (organization.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
        SessionState.ClearAppOrganizations();
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>"); 
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = Organization.LastError;
				lbError.Visible = true;
			}
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
