#region Uses
using System;
using System.Collections;
using System.Configuration;
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
using HyperCatalog.Shared;
#endregion

	/// <summary>
	/// Description résumée de userprofile_password.
	/// </summary>
  ///*********************************************************************
  /// History: 
  ///   * 2006-04-07 : Updated The function Save(). (Gemini CRYS-240)
  ///*********************************************************************

public partial class userprofile_password : HCPage
{
  
  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (HyperCatalog.Shared.SessionState.User.IsDefaultPassword)
    {
      lbError.Visible = true;
      lbError.CssClass = "hc_error";
      lbError.Text = "You still use the default password, please choose one of your own.";
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

  private void Save()
  {
    if (txtNewPassword.Text == txtNewPasswordConfirm.Text)
    {
      lbError.ForeColor =Color.Red;
      int maxPasswordLength = Convert.ToInt32(SessionState.CacheParams["Password_MaxLength"].Value);
      if (txtNewPassword.Text.Length < 6 || txtNewPassword.Text.Length > maxPasswordLength)
      {
        lbError.Visible = true;
        lbError.Text = "Your password must be at least six characters and no more than " + maxPasswordLength.ToString() + ".<br/>It can contain numbers, upper and lowercase letters, and some symbols.";
      }          
      else
      {        
        if (SessionState.User.ClearPassword == txtOldPassword.Text)
        {
          // Test if password is not containing some forbidden info
          bool safePassword = true;
          string password = txtNewPassword.Text.ToLower();
          if (password.IndexOf(SessionState.CacheParams["AppName"].Value.ToString().ToLower())>=0 ||
              password.IndexOf(SessionState.User.FirstName.ToLower())>=0 ||
              password.IndexOf(SessionState.User.LastName.ToLower())>=0 ||
              password.IndexOf(SessionState.User.Pseudo.ToLower())>=0
            )
          {
            safePassword = false;
          }
          if (safePassword)
          {
            /*int bitSize = PasswordAnalyzer.GetBitSize(txtNewPassword.Text);
            if (bitSize > Convert.ToInt32(SessionState.CacheParams["Password_MinBitKeySize"].Value))
            {*/
            SessionState.User.ClearPassword = txtNewPassword.Text;
            if (SessionState.User.Save())
            {
              lbError.ForeColor = Color.Green;
              lbError.Visible = true;
              lbError.Text = "Password changed <br>";//(It is equivalent to a " + bitSize.ToString() + " bit key).<br>";
            }
            else
            {
              SessionState.User.ClearPassword = txtOldPassword.Text;
              lbError.Visible = true;
              lbError.CssClass = "hc_error";
              lbError.Text = "Error while updating your profile: " + HyperCatalog.Business.User.LastError;
            }
            /*}
            else
            {
              lbError.Visible = true;
              lbError.Text = "Password is not safe Enough (It is equivalent to a " + bitSize.ToString() +" bit key).<br>" +
                "The password should contain a combination of upper- and lowercase letters, digits, and punctuation or other special characters";
            }*/
          }
          else
          {
            lbError.Visible = true;
            lbError.Text = "Password cannot contain your first and last name, neither you pseudo nor the application name<br>" +
              "The password should contain a combination of upper- and lowercase letters, digits, and punctuation or other special characters";
          }
        }
        else
        {
          lbError.Visible = true;
          lbError.Text = "The password you typed does not match the one for this account.<br/>Please type your correct password.";
        }
      }
    }
    else
    {
      lbError.Visible = true;
      lbError.Text = "Your new password entries did not match.<br/>Please retype them, and type your old password again.";
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
