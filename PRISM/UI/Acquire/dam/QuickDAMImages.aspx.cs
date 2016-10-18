using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebToolbar;
using Infragistics.WebUI.WebSchedule;

using HyperCatalog.Business.DAM;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.DAM
{
  /// <summary>
  /// Description résumée de QuickDAMImages.
  /// </summary>
    public partial class QuickDAMImages : HCPage
  {
    protected override void OnLoad(EventArgs e)
    {
      UITools.CheckConnection(this);

      if (!IsPostBack && Request["resource"]!=null)
      {
        string[] splitted = Request["resource"].Split('/');
        if (splitted.Length==2)
        {
          string libraryName = splitted[0];
          string resourceName = System.IO.Path.GetFileNameWithoutExtension(splitted[1]);

          resourceList.CurrentLibrary = Library.GetByKey(libraryName);
          resourceList.CurrentResource = HyperCatalog.Business.DAM.Resource.GetByKey(libraryName, resourceName);
        }
      }

      base.OnLoad (e);
    }


    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      ID = "ResourceList";
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
