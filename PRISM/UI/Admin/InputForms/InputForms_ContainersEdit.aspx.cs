#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebTab;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion uses

  /// <summary>
  /// Description résumée de InputForms_ContainersEdit.
  /// </summary>
public partial class InputForms_ContainersEdit : HCPage
{
	#region Declarations

	private System.Int64 InputFormContainerId;
	private int InputFormId;
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

	}
	#endregion

	protected void Page_Load(object sender, System.EventArgs e)
	{
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_CARTOGRAPHY))
		{
			try
			{
				if (Request["c"] != null)
					InputFormContainerId = Convert.ToInt64(Request["c"]);
				if (Request["f"] != null)
					InputFormId = Convert.ToInt32(Request["f"]);
			
				InputFormContainer ifc = InputFormContainer.GetByKey(InputFormContainerId);
			
				Infragistics.WebUI.UltraWebTab.Tab 
					tabProperties = webTab.Tabs.FromKeyTab("Properties"),
					tabPossibleValues = webTab.Tabs.FromKeyTab("PossibleValues");
				if (InputFormContainerId == -1)
				{         
					uwToolbar.Items.FromKeyLabel("Action").Text = "Adding container";
					webTab.Tabs[1].Visible = false;
				}
				else
				{
					uwToolbar.Items.FromKeyLabel("Action").Text = "Updating [" + ifc.ContainerName + "]";
						webTab.Tabs[1].Visible = ifc.Type != InputFormContainerType.Normal;
				}
				tabProperties.ContentPane.TargetUrl = tabProperties.TargetUrl + "?c=" + InputFormContainerId.ToString() + "&f=" + InputFormId;
				tabPossibleValues.ContentPane.TargetUrl = tabPossibleValues.TargetUrl + "?c=" + InputFormContainerId.ToString() + "&f=" + InputFormId;
			
				Page.DataBind();
				webTab.Visible = true;
			}
			catch
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"close", "<script>window.close();</script>");
			}
		}
		else
		{
			UITools.DenyAccess(DenyMode.Popup);
		}
	}
}