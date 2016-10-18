#region uses
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
using System.Data.SqlClient;
using HyperComponents.Globalization.Win32;
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL 24/10/2005)
#endregion

/// <summary>
/// Display user's properties
///		--> Return to the user list
///		--> Save new or modified user
///		--> Delete user
///		--> Reactivate user (reintialize attempts left)
/// </summary>
public partial class user_properties : HCPage
{
	#region Declarations
  protected string emptyPassword = "____________";
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
    #region Capabilities
    if (SessionState.User.IsReadOnly)
		{
			uwToolbar.Items.FromKeyButton("Save").Enabled = false;
			uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
			uwToolbar.Items.FromKeyButton("Unlock").Enabled = false;
			uwToolbar.Items.FromKeyButton("SendInfo").Enabled = false;
		}
    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      UITools.HideToolBarButton(uwToolbar, "Unlock");
      UITools.HideToolBarSeparator(uwToolbar, "UnlockSep");
      UITools.HideToolBarButton(uwToolbar, "SendInfo");
      //UITools.HideToolBarSeparator(uwToolbar, "SendInfoSep");
    }
    #endregion

    lbError.CssClass = "hc_error";
		lbError.Visible = false;

		try
		{
			if (Request["u"] != null){
        int userId = Convert.ToInt32(Request["u"]);
        if (SessionState.EditedUser != null)
        {
          if (SessionState.EditedUser.Id != userId)
          {
            SessionState.EditedUser = HyperCatalog.Business.User.GetByKey(userId);
          }
        }
        else
        {
          SessionState.EditedUser = HyperCatalog.Business.User.GetByKey(userId);
        }
      }

			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}
		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"back", "<script>back();</script>");
		}
	}    
	private void UpdateDataView()
  {
    #region Get all roles
    using (CollectionView rolesList = new CollectionView(SessionState.AppRoles))
    {
      rolesList.Sort("Name");
      ddlRoles.DataSource = rolesList;
      ddlRoles.DataBind();
    }
    #endregion
    #region Get all organizations
    using (CollectionView organizationsList = new CollectionView(SessionState.AppOrganizations))
    {
      organizationsList.Sort("Name");
      cbOrgs.DataSource = organizationsList;
      cbOrgs.DataBind();
    }
    #endregion
    #region Get all time zones
    Win32TimeZone[] winTimeZones = TimeZones.GetTimeZones();
		Array.Sort(winTimeZones, new Win32TimeZoneComparer());
		DDL_TimeZone.DataSource = winTimeZones;
		DDL_TimeZone.DataTextField = "DisplayName";
		DDL_TimeZone.DataValueField= "Index";
		DDL_TimeZone.DataBind();
    #endregion

    //resetPwdBtn.Visible = SessionState.EditedUser != null;
    //useDefaultPwd.Visible = SessionState.EditedUser == null;
    if (SessionState.EditedUser != null)
		{
      // Update field
			txtFirstName.Text = SessionState.EditedUser.FirstName;
			txtLastName.Text = SessionState.EditedUser.LastName;
			txtEmail.Text = SessionState.EditedUser.Email;
			txtPseudo.Text = SessionState.EditedUser.Pseudo;
			//txtPassword.Text = SessionState.EditedUser.ClearPassword;
			cbIsActive.Checked = SessionState.EditedUser.IsActive;
      ddlFormatDate.SelectedValue = SessionState.EditedUser.FormatDate;
      ddlFormatTime.SelectedValue = SessionState.EditedUser.FormatTime;
			cbIsReadOnly.Checked = SessionState.EditedUser.IsReadOnly;
			if (DDL_TimeZone != null)
				DDL_TimeZone.SelectedValue = SessionState.EditedUser.GMTTimeZoneIndex.ToString();  
			if (ddlRoles != null)
				ddlRoles.SelectedValue = SessionState.EditedUser.Role.Id.ToString();
			if (cbOrgs != null) 
				cbOrgs.SelectedValue = SessionState.EditedUser.OrgId.ToString();
			// Hide 'Reactivate' button
			if (SessionState.EditedUser.AttemptsLeft > 0)
			{
				UITools.HideToolBarButton(uwToolbar, "Unlock");
				UITools.HideToolBarSeparator(uwToolbar, "UnlockSep");
			}

			// Hide 'Delete' button
			if (SessionState.EditedUser.Id == SessionState.User.Id)
			{
				UITools.HideToolBarButton(uwToolbar, "Delete");
				UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
			}
		}
		else
		{
      // Hide 'Delete' and 'Reactivate' buttons 
			UITools.HideToolBarButton(uwToolbar, "Delete");
			UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
			UITools.HideToolBarButton(uwToolbar, "Unlock");
			UITools.HideToolBarSeparator(uwToolbar, "UnlockSep");
			UITools.HideToolBarButton(uwToolbar, "SendInfo");
            //UITools.HideToolBarSeparator(uwToolbar, "SendInfoSep");
		}

    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS) || SessionState.User.IsReadOnly)
    {
        //txtFirstName.Enabled = txtLastName.Enabled = txtEmail.Enabled = cbIsActive.Enabled = cbIsReadOnly.Enabled =
        //  txtPseudo.Enabled = txtPassword.Enabled = resetPwdBtn.Enabled = useDefaultPwd.Enabled =
        //  cbOrgs.Enabled = DDL_TimeZone.Enabled = ddlRoles.Enabled = ddlFormatDate.Enabled = ddlFormatTime.Enabled = false;

        txtFirstName.Enabled = txtLastName.Enabled = txtEmail.Enabled = cbIsActive.Enabled = cbIsReadOnly.Enabled =
          txtPseudo.Enabled =
          cbOrgs.Enabled = DDL_TimeZone.Enabled = ddlRoles.Enabled = ddlFormatDate.Enabled = ddlFormatTime.Enabled = false;
    }
	}

	private void Save()
	{
    lbError.Visible = false;

    //if (checkPassword())
    //{
      bool newUser = false;
      if (SessionState.EditedUser == null)
      {
        SessionState.EditedUser = new HyperCatalog.Business.User();
        newUser = true;
      }

      if (SessionState.EditedUser != null)
      {
        SessionState.EditedUser.FirstName = txtFirstName.Text;
        SessionState.EditedUser.LastName = txtLastName.Text;
        SessionState.EditedUser.Email = txtEmail.Text;
        SessionState.EditedUser.Pseudo = txtPseudo.Text;
        //if (newUser || txtPassword.Text != emptyPassword)
         // SessionState.EditedUser.ClearPassword = newUser && useDefaultPwd.Checked ? HyperCatalog.Business.User.DefaultPassword : txtPassword.Text;
        SessionState.EditedUser.OrgId = Convert.ToInt32(cbOrgs.SelectedValue);
        SessionState.EditedUser.IsActive = cbIsActive.Checked;
        SessionState.EditedUser.IsReadOnly = cbIsReadOnly.Checked;
        SessionState.EditedUser.FormatDate = ddlFormatDate.SelectedValue;
        SessionState.EditedUser.FormatTime = ddlFormatTime.SelectedValue;
        SessionState.EditedUser.GMTTimeZoneIndex = DDL_TimeZone.SelectedValue;
        SessionState.EditedUser.RoleId = Convert.ToInt32(ddlRoles.SelectedValue);
        if (SessionState.EditedUser.Save())
        {
          SessionState.ClearAppUsers();
          if (SessionState.EditedUser.Id == SessionState.User.Id)
          {
            SessionState.User = SessionState.EditedUser;
          }
          if (newUser)
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>parent.window.location = '../Users.aspx?u="+SessionState.EditedUser.Id+"';</script>");
          else
          {
            //ClientScript.RegisterStartupScript(GetType(), "passText", "var passBox=document.getElementById('" + txtPassword.ClientID + "');passBox.value='" + emptyPassword + "';noSavePassword = 1;", true);
            
            lbError.Text = "Data saved!";
            lbError.CssClass = "hc_success";
            lbError.Visible = true;
          }
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Error while updating user: " + HyperCatalog.Business.User.LastError;
          lbError.Visible = true;
        }
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Error: User is null";
        lbError.Visible = true;
      }
    }
    //}
	private void Delete()
	{
		if (SessionState.EditedUser.Delete(HyperCatalog.Shared.SessionState.User.Id))
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = HyperCatalog.Business.User.LastError;
			lbError.Visible = true;
		}
	}
	private void Unlock()
	{
    if ((SessionState.EditedUser != null) && (!SessionState.EditedUser.Unlock()))
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Error: This user can't be unlocked";
			lbError.Visible = true;
		}
		else
		{
			lbError.Text = "User is unlocked!";
			lbError.CssClass = "hc_success";
			lbError.Visible = true;

			UITools.HideToolBarButton(uwToolbar, "Unlock");
			UITools.HideToolBarSeparator(uwToolbar, "UnlockSep");
		}
	}
	private void SendInfo()
	{
		lbError.Visible = false;

    if (SessionState.EditedUser != null)
		{
      bool isSent = SessionState.EditedUser.SendPassword();
      if (!isSent)
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Password is not sent";
        lbError.Visible = true;

        return;
      }
      bool isSentI = SessionState.EditedUser.SendInformation();
      if (!isSentI)
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Informtion are not sent";
        lbError.Visible = true;

        return;
      }

			lbError.CssClass = "hc_success";
			lbError.Text = "Information sent !";
			lbError.Visible = true;
		}
	}
  //private bool checkPassword()
  //{
  //  bool isOk = true;
  //  string password = (SessionState.EditedUser == null) && useDefaultPwd.Checked ? HyperCatalog.Business.User.DefaultPassword : txtPassword.Text;
  //  int maxPasswordLength = Convert.ToInt32(SessionState.CacheParams["Password_MaxLength"].Value);

  //  if (password.Length < 6 || password.Length > maxPasswordLength)
  //  {
  //    isOk = false;
  //    lbError.Visible = true;
  //    lbError.Text = "Your password must be at least six characters and no more than " + maxPasswordLength.ToString() + ".<br/>It can contain numbers, upper and lowercase letters, and some symbols.";
  //  }
  //  else
  //  {
  //    if (password.IndexOf(SessionState.CacheParams["AppName"].Value.ToString().ToLower()) >= 0 ||
  //        password.IndexOf(txtFirstName.Text.ToLower()) >= 0 ||
  //        password.IndexOf(txtLastName.Text.ToLower()) >= 0 ||
  //        password.IndexOf(txtPseudo.Text.ToLower()) >= 0)
  //    {
  //      isOk = false;
  //      lbError.Visible = true;
  //      lbError.Text = "Password cannot contain your first and last name, neither you pseudo nor the application name<br>" +
  //        "The password should contain a combination of upper- and lowercase letters, digits, and punctuation or other special characters";
  //    }
  //  }
  //  return isOk;
  //}

  protected void ResetPassword(object sender, EventArgs e)
  {
    if (SessionState.EditedUser != null)
    {
      SessionState.EditedUser.ClearPassword = HyperCatalog.Business.User.DefaultPassword;
      if (SessionState.EditedUser.Save(true))
      {
        if (SessionState.EditedUser.Id == SessionState.User.Id)
        {
          SessionState.User = SessionState.EditedUser;
        }
        lbError.Text = "Data saved!";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.User.LastError;
        lbError.Visible = true;
      }
    }
  }
	protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "save")
      {
        Save();
      }
      if (btn == "delete")
      {
        Delete();
      }
      if (btn == "unlock")
      {
        Unlock();
      }
      if (btn == "sendinfo")
      {
        SendInfo();
      }
    }
	}
}
