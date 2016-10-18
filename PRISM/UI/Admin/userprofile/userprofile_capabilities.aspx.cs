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
using HyperCatalog.Shared;

	/// <summary>
  /// Description résumée de userprofile_capabilities.
	/// </summary>
  public partial class userprofile_capabilities : HCPage
	{
  
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
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
    }
    #endregion
    
    protected void Page_Load(object sender, System.EventArgs e)
		{
      if (!Page.IsPostBack)
        UpdateDataView(); 
		}

    private void UpdateDataView()
    {      
      if (SessionState.User != null)
      {
        dg.DataSource = SessionState.User.Role.Capabilities;
        dg.DataBind();
      }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      if (be.Button.Key.ToLower() == "export")
        Utils.ExportToExcel(dg, "CapabilitiesFor" + HyperCatalog.Shared.SessionState.User.FullName, "Capabilities" + HyperCatalog.Shared.SessionState.User.FullName);
    }
  }
