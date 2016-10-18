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

namespace HyperCatalog.UI.Help
{
	/// <summary>
  /// Description résumée de About.
	/// </summary>
	public partial class About : HCPage
	{
    protected override void OnLoad(EventArgs e)
    {
      if (!IsPostBack)
      {
        Bind();
        unvisible.DataBind();
      }

      base.OnLoad(e);
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

    private void Bind()
    {
      DBRep.DataSource = Business.ApplicationComponent.GetAll(Business.ApplicationComponentTypes.Database);
      DBRep.DataBind();

      WSRep.DataSource = Business.ApplicationComponent.GetAll(Business.ApplicationComponentTypes.WebService);
      WSRep.DataBind();

      string[] dlls = System.IO.Directory.GetFiles(System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "bin"), "*.dll");
      ArrayList externalDlls = new ArrayList();
      ArrayList apiDlls = new ArrayList();
      ArrayList componentDlls = new ArrayList();
      ArrayList internalDlls = new ArrayList();      
      foreach (string dll in dlls)
      {
        System.IO.FileInfo dllInfo = new System.IO.FileInfo(dll);
        if (dllInfo.Name.ToUpper().StartsWith("PRISM") || dllInfo.Name.ToUpper().StartsWith("HYPERCATALOG") || dllInfo.Name.ToUpper().StartsWith("HYPERCOMPONENT"))
        {
          if (dllInfo.Name.ToUpper().StartsWith("PRISM.API") || dllInfo.Name.ToUpper().StartsWith("HYPERCATALOG.BUSINESS"))
            apiDlls.Add(dll);
          else if (dllInfo.Name.ToUpper().StartsWith("PRISM.COMPONENTS") || dllInfo.Name.ToUpper().StartsWith("HYPERCOMPONENT"))
            componentDlls.Add(dll);
          else
            internalDlls.Add(dll);
        }
        else
          externalDlls.Add(dll);
      }
      DLLRep.DataSource = new object[][] { 
          new object[] { "API", apiDlls} , 
          new object[] { "Components", componentDlls} ,
          new object[] { "Others", internalDlls} ,
          new object[] { "Externals", externalDlls} 
        };
      DLLRep.DataBind();
    }
    
	}
}
