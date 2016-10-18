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
#endregion uses

/// <summary>
/// Display all containers attaching to input form
///		- Add containers to the input form
///		- Modify sort in input form
///		- Modify comment, mandatory, sort in container
///		- Delete container 
///		- Return to the list of input form
/// </summary>
public partial class InputForms_ContainersEditProperties : HCPage
{
	#region Declaration
	protected System.Web.UI.WebControls.Label Label11;

	private System.Int64 InputFormContainerId;
	private int InputFormId;
  private HyperCatalog.Business.Container refContainer;
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
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
    }

    // get parameters
    try
    {
      if (Request["c"] != null)
        InputFormContainerId = Convert.ToInt64(Request["c"]);
      if (Request["f"] != null)
        InputFormId = Convert.ToInt32(Request["f"]);

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "<script>parent.close();</script>");
    }
  }


	/// <summary>
	/// Display data
	/// </summary>
	private void UpdateDataView()
	{
		lbError.Visible = false;
		InputFormContainer ifc = InputFormContainer.GetByKey(InputFormContainerId);

		// list of input form container type
    DDL_InputType.Items.Clear();
		foreach (string t in Enum.GetNames(typeof(InputFormContainerType)))
		{
			DDL_InputType.Items.Add(t);
		}

		if (ifc != null) // modify input form container
		{
			hlCreator.Visible = true;
			lbOrganization.Visible = true;
			lbCreatedOn.Visible = true;
			lbOpening.Visible = true;
			lbClosing.Visible = true;

			DDL_InputType.SelectedValue = ifc.Type.ToString();
      DDL_InputType.Enabled = (ifc.ParentInputForm!=null?ifc.ParentInputForm.Items.Count==0:false);

			cbContainers.Value = ifc.Container.Tag;
			txtContainer.Text = ifc.Container.Tag;

			cbContainers.Visible = false;
			txtContainer.Visible = true;

			txtComment.Text = ifc.Comment;
			chkMandatory.Checked = ifc.Mandatory;
      cbRegionalizable.Checked = ifc.Regionalizable;

			if (ifc.Modifier != null) // IFContainer already modified
			{
				hlCreator.Text =  ifc.Modifier.FullName;
				hlCreator.NavigateUrl = "mailto:" +  UITools.GetDisplayEmail(ifc.Modifier.Email) + Server.HtmlEncode("?subject=InputFormContainers[#" + ifc.InputFormId + " - " + ifc.ContainerTag + "]");
				lbOrganization.Text = ifc.Modifier.OrgName;
        lbCreatedOn.Text = "Updated on " + SessionState.User.FormatUtcDate(ifc.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
			}
			else // IFContainer only created
			{
				hlCreator.Text =  ifc.Creator.FullName;
				hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(ifc.Creator.Email) + Server.HtmlEncode("?subject=InputFormContainers[#" + ifc.InputFormId + " - " + ifc.ContainerTag + "]");
				lbOrganization.Text = ifc.Creator.OrgName;
        lbCreatedOn.Text = "Created on " + SessionState.User.FormatUtcDate(ifc.CreateDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
			}

      if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY))
			{
				UITools.ShowToolBarButton(uwToolbar, "Delete");
				UITools.ShowToolBarSeparator(uwToolbar, "DeleteSep");  
			}
		}
		else // attach a container
		{
			hlCreator.Visible = false;
			lbOrganization.Visible = false;
			lbCreatedOn.Visible = false;
			lbOpening.Visible = false;
			lbClosing.Visible = false;

			DDL_InputType.Enabled = true;

			cbContainers.Visible = true;
			txtContainer.Visible = false;
			txtContainer.Text = string.Empty;

			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");  
		}
	}

	/// <summary>
	/// Toolbar actions
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
		if ( btn == "delete")
		{
			Delete();
		}
	}


	/// <summary>
	/// Save IFContainer data
	/// </summary>
	private void Save()
	{
		if (cbContainers.Value == null)
		{
			cbContainers.Value = txtContainer.Text;
		}

		refContainer = HyperCatalog.Business.Container.GetByTag(cbContainers.Value);
    if (refContainer != null)
		{
			bool canSave = false;

			InputFormContainer ifc = InputFormContainer.GetByKey(InputFormContainerId);
			if (ifc == null) // attach new container
			{
				CheckDependency(); // check container dependencies

				ifc = new InputFormContainer(InputFormContainerId, refContainer.Id,
					InputFormId, string.Empty,
					0, 
          refContainer.Tag,
          refContainer.Name,
          refContainer.Regionalizable,
          txtComment.Text, 
					chkMandatory.Checked, 
					(InputFormContainerType)Enum.Parse(typeof(InputFormContainerType), 
					DDL_InputType.SelectedValue, false),
					SessionState.User.Id, 
					DateTime.UtcNow, -1, null);
				canSave = true;
			}
			else // modify comment, mandatory or sort
			{
				ifc.ModifierId = SessionState.User.Id;
				ifc.ModifyDate = DateTime.UtcNow;
				ifc.Mandatory = chkMandatory.Checked; 
				ifc.Comment = txtComment.Text.Trim();
        ifc.Type = (InputFormContainerType)Enum.Parse(typeof(InputFormContainerType), DDL_InputType.SelectedValue, false);
				canSave = true;
			}

			// save in database
			if (canSave)
			{ 
				if (!ifc.Save())
				{
					// Error
					lbError.CssClass = "hc_error";
					lbError.Text = InputFormContainer.LastError;
					lbError.Visible = true;
				}
				else
				{
          lbError.CssClass = "hc_success";
          lbError.Text = "Data saved!";
          lbError.Visible = true;
          InputFormContainerId = ifc.Id;
					
					if (((InputFormContainerType)Enum.Parse(typeof(InputFormContainerType), DDL_InputType.SelectedValue, false) == InputFormContainerType.SingleChoiceList) ||
              ((InputFormContainerType)Enum.Parse(typeof(InputFormContainerType), DDL_InputType.SelectedValue, false) == InputFormContainerType.MultiChoiceList))
						Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"reloadWindow", "<script>ReloadParent();DisplayTab("+ifc.Id+", "+ifc.InputFormId+", 1);</script>"); // display new tab
  				else
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reloadWindow", "<script>ReloadParent();DisplayTab(" + ifc.Id + ", " + ifc.InputFormId + ", 0);</script>"); // reload tab
				}
			}
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: the container [" + cbContainers.Value + "] doesn't exist.";
			lbError.Visible = true;
		}
	}


	/// <summary>
	/// delete IFContainer
	/// </summary>
	private void Delete()
	{
		InputFormContainer ifc = InputFormContainer.GetByKey(InputFormContainerId);
		if (!ifc.Delete(HyperCatalog.Shared.SessionState.User.Id))
		{
			lbError.CssClass = "hc_error";
			lbError.Text = InputFormContainer.LastError;
			lbError.Visible = true;
		}
		else
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"reloadParent", "<script>ReloadParent('');top.close();</script>");
		}
	}

  private void CheckDependency()
  {
		InputForm inputForm = InputForm.GetByKey(InputFormId);
		if (inputForm != null)
		{
      if (refContainer.ContainerTypeCode == 'F') // feature container
      {
        // Retrieve all container dependencies for this feature container
        string filter = " FeatureContainerId=" + refContainer.Id;
        using (ContainerDependencyList dependencies = ContainerDependency.GetAll(filter))
        {
          if ((dependencies != null) && (dependencies.Count > 0))
          {
            // Create list of tech spec containers (in container dependencies)
            string techSpecsList = string.Empty;
            foreach (HyperCatalog.Business.ContainerDependency d in dependencies)
            {
              // Current tech spec container
              HyperCatalog.Business.Container techSpecContainer = d.TechspecContainer;

              // tech spec container is or not attached to this input form
              int i = 0;
              using (InputFormContainerList ifcList = InputFormContainer.GetAll("InputFormId=" + InputFormId + " AND ContainerId=" + techSpecContainer.Id))
              {
                if ((ifcList == null) || (ifcList.Count == 0))
                {
                  i++;
                  if (techSpecsList.Length > 0)
                    techSpecsList += "\\n";
                  techSpecsList += " " + i.ToString() + " - " + techSpecContainer.Name + " [" + techSpecContainer.Tag + "]";
                }
              }
            }

            // Tech specs (in container dependencies) are not attached to this input form
            if (techSpecsList.Length > 0)
            {
              string msg = "REMINDER - are the following tech spec containers in your tech specs input forms?\\n";
              Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "warning", "<script>alert('" + msg + techSpecsList + "');</script>");
            }
          }
        }
      }
		}
  }
}
