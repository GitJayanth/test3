#region Copyright (c)  Hewlett-Packard. All Rights Reserved
/* ---------------------------------------------------------------------*
*        THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.       *
* --------------------------------------------------------------------- *
 * History Section
 * Date             Name            Description                                         Ref
 * June 02 2009     S.Balakumar     ACQ - 11 (ACQ - 8.20) - Translating Choice Lists    #ACQ8.20
 * June 29 2009     S.Balakumar     QC 2691               - Term check if it alreayd exists has been added 
 * July 07 2009     s.Balakumar     QC 2712               - Not able to add new value in Value tab of choice list in inputform containers #2712
 * --------------------------------------------------------------------- *
*/
#endregion
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
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.WebDataInput;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL Mickael 24/10/2005)
#endregion

/// <summary>
/// Display values for selected lookup
///		--> Return to the list of lookup
///		--> Add a new value
///		--> Delete selected values
///		--> Apply modifications (save)
/// </summary>
public partial class Lookup_Values : HCPage
{
	#region Declarations

	private int lookupGroupId;
    private bool IsTranslateDefaultOption = false;//ACQ8.20
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
		this.btAddRow.Click += new System.Web.UI.ImageClickEventHandler(this.btAddRow_Click);

	}
	#endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities

      //#ACQ8.20 Starts
      try
      {
          if ("1".Equals(Convert.ToString(ApplicationSettings.Parameters["TermsIsTranslatableDefault"].Value)))
              IsTranslateDefaultOption = true;
          else
              IsTranslateDefaultOption = false;
      }
      catch (Exception ex) { }
      //#ACQ8.20 Ends

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
      // get parameter
      if (Request["lg"] != null)
      {
        lookupGroupId = Convert.ToInt32(Request["lg"]);
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "initVars", "var gId=" + lookupGroupId .ToString(), true);
      }

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

	/// <summary>
	/// Display the list of values attached to the current lookup group
	/// </summary>
	private void UpdateDataView()
	{
		if (lookupGroupId != -1)
		{
			LookupValueList lookupValuesList = LookupGroup.GetByKey(lookupGroupId).Values;
			dg.DataSource = lookupValuesList;
			Utils.InitGridSort(ref dg, false);
			dg.DataBind();

			Utils.EnableIntelligentSort(ref dg, Convert.ToInt32(txtSortColPos.Value));
			dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
			dg.DisplayLayout.CellClickActionDefault = CellClickAction.Edit;

			// Refresh tab title
			UITools.RefreshTab(Page, "Values", lookupValuesList.Count);

			if (dg.Rows.Count == 0)
			{
				UITools.HideToolBarButton(uwToolbar, "Delete");
				UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");          
			}
			else
			{
        if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
				{
					UITools.ShowToolBarButton(uwToolbar, "Delete");
					UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");
				}
			}
		}
	} 

	/// <summary>
	/// Delete multiple values on the grid
	/// </summary>
	private void Delete()
	{
		lbError.Visible=false;
		for (int i=0; i<dg.Rows.Count; i++)
		{
			TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
			CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
			if (cb.Checked)
			{
				LookupValue lgValuesObj = LookupValue.GetByKey(Convert.ToInt32(dg.Rows[i].Cells.FromKey("LookupValueId").Text));
				if (!lgValuesObj.Delete())
				{
					lbError.CssClass = "hc_error";
					lbError.Text = LookupValue.LastError;
					lbError.Visible = true;

					break;
				}
			}
		}
		UpdateDataView();
	}
  
	/// <summary>
	/// update multiple lines on the grid
	/// </summary>
	private void Save()
	{
		bool isOk = false;
		for (int i=0; i<dg.Rows.Count; i++)
		{
      if (dg.Rows[i].Cells.FromKey("TermValue").Value == null)
      {
        isOk = false;
      }
      else
      {
        LookupValue lv = LookupValue.GetByKey(Convert.ToInt32(dg.Rows[i].Cells.FromKey("LookupValueId").Value));
        if (lv != null)
        {
          lv.Sort = i;
          lv.Text = dg.Rows[i].Cells.FromKey("TermValue").Value.ToString();
          lv.Comment = (dg.Rows[i].Cells.FromKey("Comment").Value == null ? string.Empty : dg.Rows[i].Cells.FromKey("Comment").Value.ToString());
          //#ACQ8.20 Starts
          TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("IsTranslateRow").Column;
          CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("IsTranslateRows");
          bool isTranslatable = cb.Checked;
          lv.IsTranslatable = isTranslatable;
          //#ACQ8.20 Ends

          if (!lv.Save(SessionState.User.Id))
          {
            lbError.CssClass = "hc_error";
            lbError.Text = LookupValue.LastError;
            lbError.Visible = true;
            return;
          }
          else
            isOk = true;
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Lookup value doesn't exists";
          lbError.Visible = true;
          return;
        }
      }
		}
		
		if (isOk) // Success
		{
			// Display message of success
			lbError.CssClass = "hc_success";
			lbError.Text = "Data saved!";
			lbError.Visible = true;
		}

		UpdateDataView();
	}

	/// <summary>
	/// Toolbar action
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="be"></param>
	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if ( btn == "save")
		{
			Save();
		}
		if (btn == "delete")
		{
			Delete();
		}
    if (btn == "export")
    {
      string lookupGroupName = LookupGroup.GetByKey(lookupGroupId).Name;
      Utils.ExportToExcel(dg, "LookupGroup" + lookupGroupName,  lookupGroupName);
    }
	}

	private void btAddRow_Click(object sender, System.Web.UI.ImageClickEventArgs e)
	{
		lbError.Visible = false;
		if (txtValue.Text.Length > 0)
        {
            //QC2691 Check for validation of Terms if it already exists was not there so added 
            TermList Termdetails = Term.GetAll("TermValue = '" + txtValue.Text + "' AND TermTypeCode='C'");
            LookupValue lgValuesObj;
            if (Termdetails == null || Termdetails.Count == 0) //#2712 
            //if (Termdetails == null) //#2712 Commented        
            lgValuesObj = new LookupValue(-1, lookupGroupId, 0, txtValue.Text, txtComment.Text, IsTranslateDefaultOption);
            else
            lgValuesObj = new LookupValue(-1, lookupGroupId, 0, txtValue.Text, txtComment.Text, Termdetails[0].IsTranslatable);

			if (!lgValuesObj.Save(SessionState.User.Id))
			{
				lbError.CssClass = "hc_error";
				lbError.Text = LookupValue.LastError;
				lbError.Visible = true;
			}
			else
			{
				txtComment.Text = string.Empty;
				txtValue.Text = string.Empty;
				UpdateDataView();      
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Value cannot be empty";
			lbError.Visible = true;
		}
	}
}
