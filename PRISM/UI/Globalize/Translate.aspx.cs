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

using HyperCatalog.UI.Login;
using HyperCatalog.Business;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.Globilize
{
	public partial class Translate : HCPage
	{
		#region Protected vars
		protected string itemId="-1";
		#endregion
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
      if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_SETTING))
      {
        //UITools.ShowSearchBar(this);

				if (Request["i"]!=null)
					itemId=Request["i"].ToString();
				else
					itemId="-1";

				bool found = (SessionState.Culture.Type == CultureType.Locale);
				if (!found)
				{          
					CultureList cultureList = SessionState.User.Cultures;
					cultureList.Sort("Sort");
					foreach (HyperCatalog.Business.Culture culture in cultureList)
					{
						if (culture.Type != CultureType.Master)
						{
							found = true;
							SessionState.Culture = culture;
							break;
						}
					}          
				}
				if (!found)
				{
					// User has no primary cultures in its scope
					UITools.DenyAccess(DenyMode.Standard);
				}
				Page.DataBind();
			}
			else
				UITools.DenyAccess(DenyMode.Standard);
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
}
