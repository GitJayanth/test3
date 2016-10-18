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

/// <summary>
/// LinkType tab contains properties
/// </summary>
public partial class linktype_properties : HCPage
{
	#region Declarations
	
	private int linkTypeId = -1;
	#endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }
    
    if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }
    #endregion

    try
    {
      if (Request["t"] != null)
        linkTypeId = Convert.ToInt32(Request["t"]);

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "back", "<script>back();</script>");
    }
    txtDescription.Attributes.Add("onKeyUp", "maxLengthException(this);");
    txtDescription.Attributes.Add("onKeyPress", "maxLengthException(this);");
    txtDescription.Attributes.Add("onKeyDown", "maxLengthException(this);");
    txtDescription.Attributes.Add("onChange", "maxLengthException(this);");
    txtDescription.Attributes.Add("onBlur", "maxLengthException(this);");
  }

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


	private void UpdateDataView()
	{
		lbError.Visible = false;

		if (linkTypeId == -1) // new link type
		{ 
			// Default value
			cbBidirectional.Checked = true;
			wneTypeId.Value = -1;

			// hide Id of the link type
			pnlId.Visible = false;

			// hide Delete button
			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
		}
		else // update link type
		{
			LinkType linkType = LinkType.GetByKey(linkTypeId);
			if (linkType != null)
			{
				// update field
				wneTypeId.Value = linkType.Id;
				txtTypeName.Text = linkType.Name;
				cbBidirectional.Checked = linkType.IsBidirectional;
				txtIcon.Text = linkType.Icon;
				txtDescription.Text = linkType.Description;

				// show Id of the link type but it is disable
				lbTypeId.Visible = true;
				wneTypeId.Visible = true;
				wneTypeId.Enabled = false;

				int linkCount = linkType.GetLinkCount();
				cbBidirectional.Enabled = !(linkCount > 0);
			}
		}
	}

	private void Save()
	{
		lbError.Visible = false;

		LinkType linkType = null;
		if (linkTypeId == -1) // new link type
		{
			linkType = new LinkType(linkTypeId, txtTypeName.Text, txtIcon.Text, txtDescription.Text, 0, cbBidirectional.Checked);
		}
		else
		{
			linkType = LinkType.GetByKey(linkTypeId);

			// Update field
			if (linkType != null)
			{
				linkType.Name = txtTypeName.Text;
				linkType.Icon = txtIcon.Text;
				linkType.Description = txtDescription.Text;
				linkType.IsBidirectional = cbBidirectional.Checked;
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Link type doesn't exist";
				lbError.Visible = true;

				return;
			}
		}

		// Save link type
		if (linkType.Save())
		{
			if (linkTypeId == -1) // created link type
			{
				linkTypeId = linkType.Id;

				// Show Delete button
        if (SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
				{
					UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
					UITools.ShowToolBarButton(uwToolbar, "Delete");
				}

				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>DisplayTab("+linkType.Id+");</script>"); 
			}
			else // updated link type
			{
				lbError.Text = "Data saved!";
				lbError.CssClass = "hc_success";
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = LinkType.LastError;
			lbError.Visible = true;
		}
	}

	private void Delete()
	{
		lbError.Visible = false;

		// Retrieve link type selected
		LinkType linkType = LinkType.GetByKey(linkTypeId);
		if (linkType != null)
		{
			if (linkType.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"back", "<script>back();</script>");
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = LinkType.LastError;
				lbError.Visible = true;
			}   
		}
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn.Equals("save"))
		{
			Save();
		}
		else if (btn.Equals("delete"))
		{
			Delete();
		}
	}
}

