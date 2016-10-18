#region Uses
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
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

namespace hc_termbase.UI.Globalize.Termbase
{
	/// <summary>
  /// Display the containers depending of the term 
	/// </summary>
	public partial class Term_Containers : HCPage
	{
    #region Declarations
    protected System.Int64 termId;
    #endregion

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
  
		protected void Page_Load(object sender, System.EventArgs e)
		{
      #region Check Capabilities
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TERM_BASE)))
      {
        uwToolbar.Items.FromKeyButton("Save").Enabled = false;
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
      }
      #endregion
      if (Request["t"] != null)
      {
        try
        {
          termId = Convert.ToInt64(Request["t"]);
          if (!Page.IsPostBack)
          {
            ShowTermContainers();
          }
        }
        catch
        {
          UITools.DenyAccess(DenyMode.Standard);
        }
      }
		}

    /// <summary>
    /// Show the list of containers depending of the selected term
    /// </summary>
    private void ShowTermContainers()
    {
      using (ContainerList list = HyperCatalog.Business.Container.GetAll("LabelId=" + termId))
      {
        dg.DataSource = list;
        Utils.InitGridSort(ref dg);
        dg.DataBind();
      }
    }

	}
}
