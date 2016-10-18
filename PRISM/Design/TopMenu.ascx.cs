namespace HyperCatalog.UI.Login.Design
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// Description résumée de CrystalMenu.
	/// </summary>
  public partial class TopMenu : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
      if (!Page.IsPostBack)
      {
        MenuBuilder.DrawMenu(ref wm);
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
		///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		///		le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{

    }
		#endregion
	}
}
