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
	// Button "save" read only (CHARENSOL Mickael 24/10/2005)
#endregion

/// <summary>
/// Display data type properties
///		--> Save new or modified data type
///		--> Return to the list of data type
/// </summary>
public partial class datatype_properties : HCPage
{
	#region Declarations
 // textarea, boolean, date, line, resource

	
	private string dataTypeCode = string.Empty;
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
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }

    if (Request["d"] != null)
      dataTypeCode = Request["d"].ToString();

    if (!Page.IsPostBack)
    {
      UpdateDataView();
    }
  }

  private void UpdateDataView()
  {
		// Hide buttons "save" and "delete"
    UITools.HideToolBarButton(uwToolbar, "Save");
    UITools.HideToolBarButton(uwToolbar, "Delete");
		UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
		UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");

		if (dataTypeCode.Length > 0)
		{
			// Update data type
			DataType dataType = DataType.GetByKey(Convert.ToChar(dataTypeCode));
			if (dataType != null)
			{
				txtDataTypeCode.Text = dataType.Code.ToString();
				txtDataType.Text  = dataType.Name;
				txtDescription.Text = dataType.Description;
				txtExample.Text = dataType.Sample;
				txtComment.Text = dataType.Comment;
				txtRegularExpression.Text = dataType.RegularExpression;
				txtInputType.Text = dataType.InputType;
				cbIsActive.Checked = dataType.IsActive;

				txtDataTypeCode.Enabled = false;

				// temporary
				txtDataType.Enabled = false;
				txtDescription.Enabled = false;
				txtExample.Enabled = false;
				txtComment.Enabled = false;
				txtRegularExpression.Enabled = false;
				txtInputType.Enabled = false;
				cbIsActive.Enabled = false;
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = "Error: Data type is null";
				lbError.Visible = true;
			}
		}
		else
		{
			// Create new data type
			txtDataTypeCode.Enabled = true;
		}
  }
  private void Save()
  {
		DataType type = null;
		if (txtDataTypeCode.Enabled)
		{
			type = new DataType(Convert.ToChar(txtDataTypeCode.Text), txtDataType.Text, 
				Server.HtmlEncode(txtDescription.Text), Server.HtmlEncode(txtExample.Text), 
				Server.HtmlEncode(txtComment.Text), txtRegularExpression.Text, 
				txtInputType.Text, cbIsActive.Checked);
		}
		else
		{
			type = DataType.GetByKey(Convert.ToChar(txtDataTypeCode.Text));
			type.Name = txtDataType.Text;
			type.Description = Server.HtmlEncode(txtDescription.Text);
			type.Comment = Server.HtmlEncode(txtComment.Text);
			type.RegularExpression = txtRegularExpression.Text;
			type.Sample = Server.HtmlEncode(txtExample.Text);
			type.InputType = txtInputType.Text;
			type.IsActive = cbIsActive.Checked;
		}

		if (type != null)
		{
			if (type.Save(txtDataTypeCode.Enabled))
			{
        SessionState.ClearAppDataTypes();
        if (txtDataTypeCode.Enabled)
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
				lbError.Text = DataType.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Data type is null";
			lbError.Visible = true;
		}
  }

	private void Delete()
	{
		DataType type = DataType.GetByKey(Convert.ToChar(txtDataType.Text));
		if (type != null)
		{
			if (type.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = DataType.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Data type is null";
			lbError.Visible = true;
		}
	}

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "Save")
		{
		}
		else if (btn == "Delete")
		{
			Delete();
		}
	}
}

