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

namespace hc_termbase.UI.Globalize.TM
{
	/// <summary>
  /// Edit the translation selected of a TM Expression
  /// --> Button "Save" to update the TMExpression translation
  /// --> Button "Delete" to delete the TMExpression translation
	/// </summary>
	public partial class Expression_TranslationEdit : HCPage
	{
    #region Declarations
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.TextBox txtComment;

    protected System.Int64 expressionId;
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
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_MEMORY)))
      {
        uwToolbar.Items.FromKeyButton("Save").Enabled = false;
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
      }
      #endregion
      if (Request["e"] != null)
      {
        expressionId = Convert.ToInt64(Request["e"]);
        languageCode = Request["l"];
        if (!Page.IsPostBack)
        {
          try
          {
            using (HyperCatalog.Business.Language l = HyperCatalog.Business.Language.GetByKey(languageCode))
            {
              string languageName = l.Name;
              lbLanguage.Text = "[" + languageCode + "] " + languageName;
            } 
            using (TMExpression t = TMExpression.GetByKey(expressionId))
            {
              if (t != null)
              {
                txtTMExpressionValueMaster.Text = t.Value;
                using (TMExpressionTranslation tt = TMExpressionTranslation.GetByKey(expressionId, languageCode))
                {
                  if (tt == null)
                  {
                    #region New translation
                    uwToolbarTitle.Items.FromKeyLabel("Action").Text = "Creating";
                    hlCreator.Text = UITools.GetDisplayName(t.User.FullName);
                    hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(t.User.Email) + Server.HtmlEncode("?subject=TMTranslation[#" + t.Id.ToString() + " - " + t.LanguageCode + "]"); ;
                    lbOrganization.Text = t.User.OrgName;
                    lbOrganization.Visible = hlCreator.Text == t.User.FullName;
                    lbCreatedOn.Text = SessionState.User.FormatUtcDate(t.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
                    #endregion
                  }
                  else
                  {
                    #region Update translation
                    uwToolbarTitle.Items.FromKeyLabel("Action").Text = "Updating";
                    txtTMExpressionValue.Text = tt.Value;
                    hlCreator.Text = tt.User.FullName;
                    hlCreator.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(tt.User.Email) + Server.HtmlEncode("?subject=TMExpressionTranslation"); ;
                    lbOrganization.Text = tt.User.OrgName;
                    lbCreatedOn.Text = SessionState.User.FormatUtcDate(tt.ModifyDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
                    #endregion
                  }
                }
              }
              else
              {
                Response.Write("<script>alert('An error occurred when retrieving translation expression [" + Request["t"] + "]' );window.close();</script>");
                Response.End();
              }
            }
          }
          catch (Exception ex)
          {
            Response.Write("<script>alert('An error occurred when retrieving translation expression:" + ex.Message + " [" + Request["t"] + "]' );window.close();</script>");
            Response.End();
          }
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
      using (TMExpressionTranslation tt = TMExpressionTranslation.GetByKey(expressionId, languageCode))
      {
        if (tt != null)
        {
          #region Update translation
          tt.UserId = SessionState.User.Id;
          tt.Value = txtTMExpressionValue.Text.Trim();
          #endregion
          int r = tt.Save();
          #region Save result
          if (r > 0)
          {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "reloadParent", "<script>ReloadParent();top.close();</script>");
          }
          else
          {
            lbMessage.Text = TMExpressionTranslation.LastError;
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
          }
          #endregion
        }
        else
        {
          lbMessage.Text = TMExpressionTranslation.LastError;
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
      }
    }

    /// <summary>
    /// Delete the translation of the term
    /// </summary>
    private void BtnDelete()
    {
      using (TMExpressionTranslation tt = TMExpressionTranslation.GetByKey(expressionId, languageCode))
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
            lbMessage.Text = TMExpressionTranslation.LastError;
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
          }
          #endregion
        }
        else
        {
          lbMessage.Text = "Error: TM Expression translation can't be deleted - translation not found";
          lbMessage.CssClass = "hc_error";
          lbMessage.Visible = true;
        }
      }
    }


	}
}
