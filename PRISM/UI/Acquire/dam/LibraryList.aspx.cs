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

namespace HyperCatalog.UI.DAM.Pages
{
	/// <summary>
	/// Description r�sum�e de LibraryList.
	/// </summary>
    public partial class LibraryList : HCPage
	{
    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(this);

      base.OnLoad (e);
    }

    
    #region Code g�n�r� par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN�: Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// M�thode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette m�thode avec l'�diteur de code.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
