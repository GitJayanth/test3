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
using System.Data.SqlClient;
using HyperCatalog.Business;
#endregion

#region history
	// Buttons "save" and "delete" read only (CHARENSOL Mickael 24/10/2005)
#endregion


/// <summary>
/// Display item level properties
///		--> Save new or modified item level
///		--> Delete item level
///		--> Return to the list of item level
/// </summary>
public partial class itemlevels_Properties : HCPage
{
	#region Declarations
	protected int levelId = -1;
	#endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    UITools.HideToolBarButton(uwToolbar, "Delete");

    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }
    #endregion

    try
    {
      if (Request["l"] != null)
        levelId = Convert.ToInt32(Request["l"]);

      if (!Page.IsPostBack)
      {
        UpdateDataEdit();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }

	private void UpdateDataEdit()
	{
    // Retrieve sku level if it exists$
    ItemLevelList list = ItemLevel.GetAll();
    ItemLevel skuLevel = null;
    foreach (ItemLevel l in list)
    {
      if (l.SkuLevel)
      {
        skuLevel = l;
        break;
      }
    }

		ItemLevel level = ItemLevel.GetByKey(levelId);
		if (level != null)
		{
			// *************************************************************************
			// Display the itemLevel
			// *************************************************************************
			txtLevelId.Text = level.Id.ToString();
			txtLevelName.Text = level.Name;
			cbOptional.Checked = level.Optional;
			cbSkuLevel.Checked = level.SkuLevel;
      txtExportName.Text = level.LevelExportName;

      cbSkuLevel.Enabled = (skuLevel == null || skuLevel.Id == level.Id);
			txtLevelId.Enabled = false;
			panelId.Visible = true;
		}
		else
		{
			// *************************************************************************
			// Provide an empty screen to create a new Role
			// *************************************************************************
			panelId.Visible = false;
      cbSkuLevel.Enabled = (skuLevel == null);

			UITools.HideToolBarButton(uwToolbar, "Delete");   
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }
    Page.DataBind();
  }

  #region Persistence methods
  private void Save()
	{
		ItemLevel level = null;
		if (!panelId.Visible)
		{
			// add new item level  -- not used
			//level = new ItemLevel(-1, txtLevelName.Text, cbOptional.Checked, cbSkuLevel.Checked);
      lbError.Text = "Impossible to create new level";
      lbError.CssClass = "hc_error";
      lbError.Visible = true;
      return;
		}
		else
		{
			// update item level
			level = ItemLevel.GetByKey(Convert.ToInt32(txtLevelId.Text));
			if (level != null)
			{
				level.Name = txtLevelName.Text;
				level.Optional = cbOptional.Checked;
				level.SkuLevel = cbSkuLevel.Checked;
        level.LevelExportName = txtExportName.Text;
			}
		}

		if (level != null)
		{
			if (level.Save())
			{
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
				lbError.Text = ItemLevel.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Item level is null";
			lbError.Visible = true;
		}
	}
	private void Delete()
	{
		ItemLevel level = ItemLevel.GetByKey(Convert.ToInt32(txtLevelId.Text));
		if (level != null)
		{
			if (level.Delete(HyperCatalog.Shared.SessionState.User.Id))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = ItemLevel.LastError;
				lbError.Visible = true;
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: Item level is null";
			lbError.Visible = true;
		}
  }
  #endregion

  #region Event methods
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
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
  #endregion
}