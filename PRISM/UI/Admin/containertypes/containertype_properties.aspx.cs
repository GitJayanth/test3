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
using System.Data.SqlClient;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

#region History
	// Button "save" and "delete" read only (CHARENSOL Mickael 24/10/2005)
#endregion

/// <summary>
/// Display container properties
///		--> Return to the list of container typer
///		--> Save new or modified container type
///		--> Delete container type (if it is not used)
/// </summary>
public partial class containertype_properties : HCPage
{
	#region Declarations
	private string typeCode = string.Empty;
	#endregion

	#region Code généré par le Concepteur Web Form
	override protected void OnInit(EventArgs e)
	{
		//
		// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
		//
		InitializeComponent();
		txtTypeCode.Attributes.Add("OnKeyUp","this.value = this.value.toUpperCase();");
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
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }

    if (Request["d"] != null)
      typeCode = Request["d"].ToString();

    if (!Page.IsPostBack)
    {
      UpdateDataView();
    }
  }

	private void UpdateDataView()
	{
    txtTypeCodeDisable.Visible = false;
    txtTypeCode.Visible = false;
		if (typeCode.Length == 0)
		{
			// Create new container type
      txtTypeCode.Visible = true;

			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
		}
		else
		{
			// Update container type
			ContainerType containerType = ContainerType.GetByKey(Convert.ToChar(typeCode));
			if (containerType != null)
			{
        txtTypeCodeDisable.Text = containerType.Code.ToString();
				txtTypeName.Text = containerType.Name;
				cbIsResource.Checked = containerType.IsResource;

        txtTypeCodeDisable.Visible = true;
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: Container type is null";
				lbError.Visible = true;
			}
		}
	}
	private void Save()
	{
		ContainerType type = null;
		if (txtTypeCode.Visible)
		{
			type = new ContainerType(Convert.ToChar(txtTypeCode.Text), txtTypeName.Text, cbIsResource.Checked);
		}
		else
		{
			type = ContainerType.GetByKey(Convert.ToChar(txtTypeCodeDisable.Text));
			if (type != null)
			{
				type.Name = txtTypeName.Text;
				type.IsResource = cbIsResource.Checked;
			}
		}
		if (type != null)
		{
      if (type.Save(txtTypeCodeDisable.Text == string.Empty))
			{
        SessionState.ClearAppContainerTypes();
        if (txtTypeCode.Visible)
				{
					// Create new container type
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
				}
				else
				{
					// Update container type
					lbError.Text = "Data saved!";
					lbError.CssClass = "hc_success";
					lbError.Visible = true;
				}
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = ContainerType.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Container type is null";
			lbError.Visible = true;
		}
	} 
	private void Delete()
	{
    ContainerType type = ContainerType.GetByKey(Convert.ToChar(txtTypeCodeDisable.Text));
		if (type != null)
		{
			if (type.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = ContainerType.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Container type is null";
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
