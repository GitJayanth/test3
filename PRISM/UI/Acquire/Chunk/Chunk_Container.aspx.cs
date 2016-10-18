#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Acquire.Chunk
{
  /// <summary>
  /// Description résumée de Chunk_Container.
  /// </summary>
  public partial class Chunk_Container : HCPage
  {

    protected HyperCatalog.Business.Container container;
		
    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        ContainerInfoPanel.ContainerId = Convert.ToInt32(Request["d"]);
      }
      catch (Exception ex)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('Error: " + UITools.CleanJSString(ex.ToString()) + "'");
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