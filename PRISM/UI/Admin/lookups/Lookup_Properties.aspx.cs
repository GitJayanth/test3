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
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL Mickael 24/10/2005)
#endregion

/// <summary>
/// Display lookup properties
///		--> Save new or modified lookup
///		--> Delete lookup
///		--> Return to the list of lookup
/// </summary>
public partial class Lookup_Properties : HCPage
{
	#region Declarations

	private int lookupGroupId;
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

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }
    #endregion

    try
    {
      if (Request["lg"] != null)
        lookupGroupId = Convert.ToInt32(Request["lg"]);

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }

	// Display data
	protected void UpdateDataView()
	{
		if (lookupGroupId == -1)
		{
			// *************************************************************************
			// Provide an empty screen to create a new Input form
			// *************************************************************************
			panelId.Visible = false;
			Page.DataBind();

			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
		}
		else
		{	
			LookupGroup lookupObj = LookupGroup.GetByKey(lookupGroupId);

			txtGroupId.Text = lookupObj.Id.ToString();
			txtGroupName.Text = lookupObj.Name;
			txtComment.Text = lookupObj.Comment;
			cbMultiChoice.Checked = lookupObj.MultiChoice;

			panelId.Visible = true;

			Page.DataBind();
		}
	}

	// Save the new or updated Input form
	private void Save()
	{
		LookupGroup lookupObj = LookupGroup.GetByKey(lookupGroupId);
		if (lookupObj == null)
		{
			lookupObj = new LookupGroup(lookupGroupId, txtGroupName.Text.Trim(), txtComment.Text.Trim(), (bool)cbMultiChoice.Checked);
		}
		else
		{
			lookupObj.Name = txtGroupName.Text;
			lookupObj.Comment = txtComment.Text;
			lookupObj.MultiChoice = cbMultiChoice.Checked;
		}

		if (!lookupObj.Save(SessionState.User.Id))
		{
			lbError.CssClass = "hc_error";
			lbError.Text = LookupGroup.LastError;
			lbError.Visible = true;
		}
		else
		{
			lbError.Text = "Data saved!";
			lbError.CssClass = "hc_success";
			lbError.Visible = true;
      SessionState.ClearAppLookupGroups();
			if (lookupGroupId < 0)
			{
				lookupGroupId = lookupObj.Id;
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"reloadTab", "<script>DisplayTab("+lookupObj.Id+");</script>"); // display new tab
			}
		}      
	}

	// Delete current Input form
	private void Delete()
	{
		if (panelId.Visible)
		{
			LookupGroup lookupObj = LookupGroup.GetByKey(lookupGroupId);
			if (!lookupObj.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
				lbError.CssClass = "hc_error";
				lbError.Text = LookupGroup.LastError;
				lbError.Visible = true;
			} 
			else
			{
        SessionState.ClearAppLookupGroups();
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
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
		else if (btn == "delete")
		{
			Delete();
		}
	}

}

