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

namespace HyperCatalog.UI
{
	/// <summary>
	/// Description résumée de Help.
	/// </summary>
	public partial class Help : System.Web.UI.Page
	{
  
		protected void Page_Load(object sender, System.EventArgs e)
		{
      try
      {
        string src = Server.UrlDecode(Request["Page"].ToString());
        src += (src.Contains("?")?"&":"?") + "t=" + Shared.SessionState.User.GetTempToken();
        frmHelp.Attributes.Add("src", src);
      }
      catch
      {
        Response.Redirect("~/");
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
