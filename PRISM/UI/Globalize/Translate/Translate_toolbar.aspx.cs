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

public partial class Translate_toolbar : HCPage
{
	#region Protected vars
	#endregion
	
	protected void Page_Load(object sender, System.EventArgs e)
	{
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_SETTING))
    {
		  if (!Page.IsPostBack)
				BindCombo();
		}		
		else
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

	private void BindCombo()
	{
		CultureList cultureList = new CultureList();
		if (SessionState.Culture.Type != CultureType.Master)
		{ 
			DDL_Cultures.Visible = true;
			foreach(HyperCatalog.Business.Culture culture in SessionState.User.Cultures)
			{
				if (culture.Type != CultureType.Master)
				{
					cultureList.Add(culture);
				}
			}
			cultureList.Sort("Sort");
			DDL_Cultures.DataSource = cultureList;
			DDL_Cultures.DataBind();
			DDL_Cultures.SelectedValue = SessionState.Culture.Code;
		}
		else
		{
			DDL_Cultures.Visible = false;
		}
	}

	protected void DDL_Cultures_SelectedIndexChanged(object sender, System.EventArgs e)
	{
		SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(DDL_Cultures.SelectedValue.ToString());
		Page.ClientScript.RegisterStartupScript(this.GetType(),"reload", "<script>UpdateFrames();</script>");
	}
}
