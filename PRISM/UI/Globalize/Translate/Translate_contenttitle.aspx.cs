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

public partial class Translate_contenttitle : HCPage
{
	protected void Page_Load(object sender, System.EventArgs e)
	{
    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_SETTING))
      UITools.DenyAccess(DenyMode.Frame); 
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

	}
	#endregion
}
