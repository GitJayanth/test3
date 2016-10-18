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
using HyperComponents.Globalization.Win32;
using HyperCatalog.Shared;

  /// <summary>
  /// Description résumée de userprofile_identification.
  /// </summary>
public partial class userprofile_identification : HCPage
{
  
  // This variable is required to bind datat with the aspx page
  protected HyperCatalog.Business.User currentUser;

  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (!Page.IsPostBack)
    {
      BindUserData();
      lbError.Visible = false;
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
    this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

  }
  #endregion

  private void BindUserData()
  {
    SessionState.User = HyperCatalog.Business.User.GetByKey(SessionState.User.Id);
    currentUser = SessionState.User;
    if (currentUser != null)
    {
      Win32TimeZone[] winTimeZones;
      winTimeZones = TimeZones.GetTimeZones();
      Array.Sort(winTimeZones, new Win32TimeZoneComparer());
      DDL_TimeZone.DataSource = winTimeZones;
      DDL_TimeZone.DataTextField = "DisplayName";
      DDL_TimeZone.DataValueField= "Index";
      Page.DataBind();
      DDL_TimeZone.SelectedValue = currentUser.GMTTimeZone.Index.ToString();
      DDLFormatDate.SelectedValue = currentUser.FormatDate;
      DDLFormatTime.SelectedValue = currentUser.FormatTime;
    }
    else
    {
      Response.Write("An error occurred while retrieving User");
    }
  }

  private void Save()
  {
    lbError.Visible = false;

    SessionState.User.FirstName = txtFirstName.Text;
    SessionState.User.LastName = txtLastName.Text;
    SessionState.User.Email = txtEmail.Text;
    SessionState.User.Email2 = txtEmail2.Text;
    SessionState.User.Email3 = txtEmail3.Text;
    SessionState.User.Pseudo = txtPseudo.Text;
    SessionState.User.GMTTimeZone = HyperComponents.Globalization.Win32.TimeZones.GetTimeZone(DDL_TimeZone.SelectedValue);
    SessionState.User.FormatDate = DDLFormatDate.SelectedValue;
    SessionState.User.FormatTime = DDLFormatTime.SelectedValue;
    if (!SessionState.User.Save())
    {
      BindUserData();

      lbError.CssClass = "hc_error";
      lbError.Text = "Error while updating your profile: " + HyperCatalog.Business.User.LastError;
      lbError.Visible = true;
    }
    else
    {
      BindUserData();
      lbError.CssClass = "hc_success";
      lbError.Text = "Profile updated";
      lbError.Visible = true;
    }
  }

  private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    if (btn == "save")
    {
      Save();
    }
  }
}
