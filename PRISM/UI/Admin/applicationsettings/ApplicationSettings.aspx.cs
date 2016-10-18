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
using Infragistics.WebUI.UltraWebGrid;

namespace HyperCatalog.UI.Admin.Architecture
{
	/// <summary>
	/// Description résumée de ApplicationSettings.
	/// </summary>
	public partial class ApplicationSettings : HCPage
	{

    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(Page);

      if (Request["c"]!=null)
      {
        Infragistics.WebUI.UltraWebTab.Tab componentTab = webTab.Tabs.FromKeyTab("Components");
        componentTab.ContentPane.TargetUrl = "./Components.aspx?c=" + Request["c"];
        //        if (Request["ReturnUrl"]!=null)
        //          componentTab.ContentPane.TargetUrl += "&ReturnUrl=" + Server.UrlEncode(Request["ReturnUrl"]);
        webTab.SelectedTabObject = componentTab;
      }

      base.OnLoad (e);
    }
    protected override void OnPreRender(EventArgs e)
    {
      backButton.Visible = Request["ReturnUrl"]!=null;
      if (Request["ReturnTitle"]!=null)
        backButton.ToolTip = "Back to " + Request["ReturnTitle"];
      
      base.OnPreRender (e);
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
      backButton.Click += new ImageClickEventHandler(backButton_Click);
    }
    #endregion

    private void backButton_Click(object sender, ImageClickEventArgs e)
    {
      Response.Redirect(Request["ReturnUrl"]);
    }
  }
}
