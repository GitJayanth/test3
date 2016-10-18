#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using System.Web.SessionState;
#endregion

	/// <summary>
	/// Edit the translation selected of a term
	/// --> Button "Save" to create or update the term translation
	/// --> Button "Delete" to delete the term translation
	/// </summary>
	public partial class term : HCPage
	{
    #region Declarations
    protected Infragistics.WebUI.UltraWebTab.UltraWebTab webTab;
    protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.DropDownList DDL_TermTypeList;
    protected System.Web.UI.WebControls.Label lbCulture;
    protected System.Int64 termId;
    protected string languageCode;
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
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

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
      termId = Convert.ToInt64(Request["t"]);
      languageCode = Request["c"];
      if (!Page.IsPostBack)
      {
        try
        {
          using (HyperCatalog.Business.Language currentLanguage = HyperCatalog.Business.Language.GetByKey(languageCode))
          {
            string languageName = currentLanguage.Name;
            if (currentLanguage.Rtl)
            {
              txtTermValue.CssClass = "hc_rtledit";
            }
            lbLanguage.Text = "[" + languageCode + "] " + languageName;
            using (Term t = Term.GetByKey(termId))
            {
              if (t != null)
              {
                txtTermValueMaster.Text = t.Value;
                using (TermTranslation tt = TermTranslation.GetByKey(termId, languageCode))
                {
                  if (tt == null)
                  {
                    #region New translation
                    uwToolbarTitle.Items.FromKeyLabel("Action").Text = "Creating";
                    hlCreator.Text = UITools.GetDisplayName(t.User.FullName);
                    hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(t.User.Email) + Server.HtmlEncode("?subject=TermTranslation[#" + t.Id.ToString() + " - " + t.LanguageCode + "]"); ;
                    lbOrganization.Text = t.User.OrgName;
                    lbOrganization.Visible = hlCreator.Text == t.User.FullName;
                    lbCreatedOn.Text = SessionState.User.FormatUtcDate(t.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
                    UITools.HideToolBarButton(uwToolbar, "Delete");
                    UITools.HideToolBarSeparator(uwToolbar, "SepDelete");
                    #endregion
                  }
                  else
                  {
                    #region Update translation
                    uwToolbarTitle.Items.FromKeyLabel("Action").Text = "Updating";
                    txtTermValue.Text = tt.Value;
                    hlCreator.Text = tt.User.FullName;
                    hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(tt.User.Email) + Server.HtmlEncode("?subject=TermTranslation"); ;
                    lbOrganization.Text = tt.User.OrgName;
                    lbCreatedOn.Text = SessionState.User.FormatUtcDate(tt.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
                    UITools.ShowToolBarButton(uwToolbar, "Delete");
                    UITools.ShowToolBarSeparator(uwToolbar, "SepDelete");
                    #endregion
                  }
                }
              }
              else
              {
                Response.Write("<script>alert('An error occurred when retrieving translation term [" + Request["t"] + "]' );window.close();</script>");
                Response.End();
              }
            }
          }
        }
        catch (Exception ex)
        {
          Response.Write("<script>alert('An error occurred when retrieving translation term:" + ex.Message + " [" + Request["t"] + "]' );window.close();</script>");
          Response.End();
        }
      }
    }

    /// <summary>
    /// Action for toolbar buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Save
      if ( btn == "save")
      {
        BtnSave();
      }
      #endregion
      #region Delete
      if ( btn == "delete")
      {
        BtnDelete();
      }
      #endregion
    }

    /// <summary>
    /// Save the translation of the term
    /// </summary>
    private void BtnSave()
    {
      TermTranslation tt;
      using (tt = TermTranslation.GetByKey(termId, languageCode))
      {
        if (tt == null)
        {
          #region new translation
          tt = new TermTranslation(termId, txtTermValue.Text.Trim(), languageCode, false, SessionState.User.Id, DateTime.UtcNow);
          #endregion
        }
        else
        {
          #region Update translation
          tt.UserId = SessionState.User.Id;
          tt.Value = txtTermValue.Text.Trim();
          #endregion
        }
        int r = tt.Save();
        #region Save result
        if (r > 0)
        {
          Page.ClientScript.RegisterStartupScript(this.GetType(), "reloadParent", "<script>ReloadParent();top.close();</script>");
        }
        else
        {
          lbMessage.Text = TermTranslation.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
        #endregion
      }
    }

    /// <summary>
    /// Delete the translation of the term
    /// </summary>
    private void BtnDelete()
    {
      using (TermTranslation tt = TermTranslation.GetByKey(termId, languageCode))
      {
        if (tt != null)
        {
          #region Delete result
          if (tt.Delete(HyperCatalog.Shared.SessionState.User.Id))
          {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "reloadParent", "<script>ReloadParent();top.close();</script>");
          }
          else
          {
            lbMessage.Text = TermTranslation.LastError;
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
          }
          #endregion
        }
        else
        {
          lbMessage.Text = "Error: Term translation can't be deleted - translation not found";
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
      }
    }
}
