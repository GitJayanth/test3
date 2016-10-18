#region uses
using System;
using System.Drawing;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

/// <summary>
/// Add container dependency
///		--> save container dependency
///		--> close window
/// </summary>
public partial class container_dependencyEdit : HCPage
{
	#region Declarations

	private int featureContainerId = -1;
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
      uwToolbar.Items.FromKeyButton("save").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_DICTIONARY))
    {
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Save");
    }

    try
    {
      if (Request["c"] != null)
        featureContainerId = Convert.ToInt32(Request["c"]);

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>top.window.close();</script>");
    }
  }

	private void UpdateDataView()
	{
		lbError.Visible = false;

		string filter = string.Empty;
		filter += " ContainerTypeCode='T'";
		filter += " AND Id NOT IN (SELECT TechspecContainerId FROM ContainerDependencies WHERE FeatureContainerId="+featureContainerId+")";
		filter += " ORDER BY Tag";

    using (ContainerList containers = HyperCatalog.Business.Container.GetAll(filter))
    {
      ddlTechspecContainers.DataSource = containers;
      ddlTechspecContainers.DataTextField = "Tag";
      ddlTechspecContainers.DataValueField = "Id";
      ddlTechspecContainers.DataBind();
    }
	}

	private void Save()
	{
		int techspecContainerId = -1;
		if (ddlTechspecContainers != null)
		{
			string sTechspecCont = ddlTechspecContainers.SelectedValue;
			if (sTechspecCont.Length > 0)
				techspecContainerId = Convert.ToInt32(sTechspecCont);
		}
		if (techspecContainerId > 0)
		{
			Container fContainer = Container.GetByKey(featureContainerId);
			Container tContainer = Container.GetByKey(techspecContainerId);
			ContainerDependency dependency = new ContainerDependency(fContainer, tContainer);
			if (dependency.Save())
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"close", "<script>UpdateParent("+featureContainerId+");</script>");
			}
			else
			{
				lbError.CssClass = "hc_error";
				lbError.Text = ContainerDependency.LastError;
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
	}
}
