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

namespace HyperCatalog.UI.Collaborate
{
	public partial class Proofread : HCPage
	{
		#region Protected vars
		protected string itemId = "-1";
		#endregion
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
    if (!SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT))
				UITools.DenyAccess(DenyMode.Standard); 
			else
			{
				if (Request["i"] != null)
					itemId=Request["i"].ToString();
				else
					itemId="-1";
        if (Request["l"]!=null)
        {
          SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(Request["l"].ToString());
        }
        else
        {
          SessionState.Culture = HyperCatalog.Shared.SessionState.MasterCulture;
        }
				Page.DataBind();
			}
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
