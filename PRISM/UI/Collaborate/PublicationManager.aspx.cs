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

using HyperCatalog.UI.Login;
using HyperCatalog.Business;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.Collaborate
{
  public partial class PublicationManager : HCPage
  {
    protected override void OnLoad(EventArgs e)
    {
      Guid tokenId = Guid.Empty;
      try
      {
        tokenId = WSPubManager.UserLoginFromPRISM(SessionState.User.Id);
      }
      catch
      {
        lbError.Text = "The Publication Manager is not available.";
        lbError.CssClass = "hc_error";
        lbError.Visible = true;
      }
      if (tokenId != Guid.Empty)
        Response.Redirect(SessionState.CacheComponents["PublicationManager_UI"].URI + "?u=" + tokenId);
      else
      {
        lbError.Text = "You cannot have access to the Publication Manager. Your account may not have been affected to a group. Please contact an Publication support administrator.";
        lbError.CssClass = "hc_error";
        lbError.Visible = true;
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
  }
}
