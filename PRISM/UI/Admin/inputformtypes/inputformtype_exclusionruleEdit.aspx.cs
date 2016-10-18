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
using Infragistics.WebUI.UltraWebGrid;
#endregion

/// <summary>
/// This popup allows adding new exclusion rule
///		- Add exclusion rule
///		- close window
/// </summary>
public partial class inputformtype_exclusionruleEdit : HCPage
{
	#region Declarations

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
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
    }
    #endregion

    try
    {
      if (Request["i"] != null)
        inputFormTypeCode = Convert.ToChar(Request["i"]);

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
		lbError.Visible = false;
		InputFormType type = InputFormType.GetByKey(inputFormTypeCode);
		if (type != null)
		{
			// Get input form type
			lblTitle.Text = "New exclusion rule for input form type "+type.Name+" ["+type.Code+"]";

			// Build collection with all possible container types (not used in exclusion rule)
      ContainerTypeList containerTypes = GetPossibleContainerTypes();

      if (containerTypes.Count > 0)
      {
        dg.DataSource = containerTypes;
        Utils.InitGridSort(ref dg, false);
        dg.DataBind();
        dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
      }
      else
      {
        dg.Visible = false;
        lbError.Text = "No container types";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
        UITools.HideToolBarButton(uwToolbar, "Save");
        UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      }
		}
		else
		{
			// Wrong request data
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: exclusion rule of input form type is null";
			lbError.Visible = true;
		}
	}

	private ContainerTypeList GetPossibleContainerTypes()
	{
		// result list of container type
		ContainerTypeList resultList = new ContainerTypeList();

		// Get all container types
    using (ContainerTypeList containerTypes = ContainerType.GetAll())
    {

      // Get all exclusion rules for this input form type (input form type code is not empty)
      string sqlFilter = "InputFormTypeCode = '" + inputFormTypeCode + "'";
      using (InputFormTypeExclusionRuleList iftExclusionRules = InputFormTypeExclusionRule.GetAll(sqlFilter))
      {

        // Delete container type already used
        if (containerTypes != null)
        {
          foreach (ContainerType ct in containerTypes)
          {
            bool isFound = false;

            if (iftExclusionRules != null)
            {
              foreach (InputFormTypeExclusionRule er in iftExclusionRules)
                isFound = isFound || (er.ContainerTypeCode == ct.Code);
            }

            if (!isFound)
              resultList.Add(ct);
          }
        }
      }
    }
		return resultList;
	}

	/// <summary>
	/// Save exclusion rule
	/// </summary>
  private void Save()
  {
    //string containerTypeCode = ddlContainerType.SelectedValue;
    string msg = string.Empty;
    if (inputFormTypeCode != ' ')
    {
      for (int i = 0; i < dg.Rows.Count; i++)
      {
        TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
        string containerTypeCode = dg.Rows[i].Cells.FromKey("Code").ToString();

        if (containerTypeCode.Length == 1 && cb.Checked)
        {
          InputFormTypeExclusionRule rule = new InputFormTypeExclusionRule(inputFormTypeCode, Convert.ToChar(containerTypeCode));

          if (!rule.Save())
          {
            msg = InputFormTypeExclusionRule.LastError + "\\n";
          }
        }
      }
    }
    if (msg.Length == 0)
    {
      lbError.Text = "Data saved!";
      lbError.CssClass = "hc_success";
      lbError.Visible = true;

      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update", "<script>UpdateParentWithClose();</script>");
    }
    else
    {
      lbError.Text = msg;
      lbError.CssClass = "hc_error";
      lbError.Visible = true;
    }
  }

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if ( btn == "save")
		{
			Save();
		}    
	}
}
