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
#endregion

/// <summary>
/// Display properties of input form type
///		- Save new or modified input form type
///		- Delete input form type (if it is not used)
///		- Return to the list of input form type
/// </summary>
public partial class inputformtype_properties : HCPage
{
	#region Declarations
	protected System.Web.UI.WebControls.TextBox txtDataTypeCode;
	protected System.Web.UI.WebControls.TextBox txtDataType;
	
	private char inputFormTypeCode = ' ';
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

    if (!SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }
    #endregion

    try
    {
      if (Request["d"] != null)
        inputFormTypeCode = Convert.ToChar(Request["d"]);

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

	private void UpdateDataView()
	{
		if (inputFormTypeCode == ' ')
		{
			// Create input form type
			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");

			txtInputFormTypeCode.Enabled = true;
		}
		else
		{
			// Modify input form type
			HyperCatalog.Business.InputFormType inputFormType = HyperCatalog.Business.InputFormType.GetByKey(inputFormTypeCode);
			if (inputFormType != null)
			{
				txtInputFormTypeCode.Text = inputFormType.Code.ToString();
				txtInputFormType.Text = inputFormType.Name;

				txtInputFormTypeCode.Enabled = false;

        if (SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL))
				{
					UITools.ShowToolBarButton(uwToolbar, "Delete");
					UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
				}
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: Input form type is null";
				lbError.Visible = true;
			}
		}
	}

	private void Save()
	{
		HyperCatalog.Business.InputFormType ifType = null;
		if (!txtInputFormTypeCode.Enabled)
		{
			// Update modified input form type
			ifType = HyperCatalog.Business.InputFormType.GetByKey(inputFormTypeCode);
			ifType.Name = txtInputFormType.Text;
		}
		else
		{
			// Save new input form type
			ifType = new HyperCatalog.Business.InputFormType(Convert.ToChar(txtInputFormTypeCode.Text), txtInputFormType.Text, 0);
		}
			
		if (ifType != null)
		{
			if (ifType.Save(txtInputFormTypeCode.Enabled))
			{
				if (txtInputFormTypeCode.Enabled)
				{
					// create
					Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
				}
				else
				{
					// update
					lbError.Text = "Data saved!";
					lbError.CssClass = "hc_success";
					lbError.Visible = true;
				}
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = HyperCatalog.Business.InputFormType.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: input form type is null";
			lbError.Visible = true;
		}
	}

	private void Delete()
	{
		HyperCatalog.Business.InputFormType inputFormType = HyperCatalog.Business.InputFormType.GetByKey(Convert.ToChar(inputFormTypeCode));
		if (inputFormType != null)
		{
			if (inputFormType.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
				lbError.Visible = false;
				lbError.Text = string.Empty;

				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = HyperCatalog.Business.InputFormType.LastError;
				lbError.Visible = true;
			} 
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: input form type is null";
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
		else if (btn == "delete")
		{
			Delete();
		}
	}
}
