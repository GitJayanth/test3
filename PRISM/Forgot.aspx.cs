#region uses
using System;
using System.Net.Mail;
using System.Web.Security;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Configuration;
#endregion

namespace HyperCatalog.UI
{
  /// <summary>
  /// Description résumée de Forgot.
  /// </summary>

  public partial class Forgot : HCPage
  {

    private string _loginCookieName
    {
      get
      {
        return FormsAuthentication.FormsCookieName + "login";
      }
    }
    
    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        lbError.Text = string.Empty;
        HttpCookie loginCookie = Context.Request.Cookies[_loginCookieName];
        if (null != loginCookie)
        {
          txtLogin.Text = loginCookie.Value;
        }

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
      this.validateBtn.Click += new ImageClickEventHandler(validateBtn_Click);
      linkLogin.Click += new EventHandler(linkLogin_Click);
    }
    #endregion

    void validateBtn_Click(object sender, ImageClickEventArgs e)
    {
      if (txtLogin.Text.Length > 0)
      {
        HyperCatalog.Business.User user = HyperCatalog.Business.User.GetByKey(txtLogin.Text);
        if (user != null)
        {
          if (user.SendPassword())
          {
//            lbError.CssClass = "hc_success";
            lbError.Visible = true;
            lbError.Text = "Check your mailbox, you should soon receive your credentials.";
          }
          else
          {
//            lbError.CssClass = "hc_error";
            lbError.Visible = true;
            lbError.Text = "The email failed to be sent. Sorry for the inconveniance.<BR/>Please try again later.";
          }
        }
        else
        {
//          lbError.CssClass = "hc_error";
          lbError.Visible = true;
          lbError.Text = "User not found in the system.";
        }
      }
    }

    void linkLogin_Click(object sender, EventArgs e)
    {
      Response.Redirect("./Login.aspx");
    }

  }
}