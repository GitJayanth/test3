//using AJAXControls;
using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using HyperCatalog.Business;
using HyperComponents.Data.dbAccess;

namespace HyperCatalog.UI.Main.AjaxControls
{
  /// <summary>
  /// Description résumée de LookupContainer.
  /// </summary>
  [WebService(Namespace = "http://www.hypercatalog.fr/webservices/",
     Description = "")]

  public class AdvancedSearch : WebService//, IPopupService
  {
    public AdvancedSearch()
    {
      //CODEGEN : Cet appel est requis par le Concepteur des services Web ASP.NET
      InitializeComponent();
    }

    #region Code généré par le Concepteur de composants
		
    //Requis par le Concepteur des services Web 
    private IContainer components = null;
				
    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
    }

    /// <summary>
    /// Nettoyage des ressources utilisées.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if(disposing && components != null)
      {
        components.Dispose();
      }
      base.Dispose(disposing);		
    }
		
    #endregion

    /// <summary>
    /// </summary>
    [WebMethod(Description = "")]
    public string GetContainerGroups(int parentContainerGroupId)
    {
      string result = "";
      foreach (Business.ContainerGroup group in Business.ContainerGroup.GetAll("ContainerGroupParentId=" + parentContainerGroupId.ToString()))
        result += "<OPTION value=" + group.Id + ">" + group.Name + "</OPTION>";
      return result;
    }
  }
}
