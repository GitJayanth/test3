#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
#endregion

public partial class language_properties : HCPage
{

  #region Declarations
  private HyperCatalog.Business.Language lang;
  private string languageCode = string.Empty;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    #region Check Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Save").Enabled = false;
      uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
    }

    if (!SessionState.User.HasCapability(CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
    }
    #endregion

    if (Request["l"] != null)
    {
      languageCode = Request["l"].ToString();
      lang = HyperCatalog.Business.Language.GetByKey(languageCode);
      if (!Page.IsPostBack)
      {
        UpdateDataEdit();
      }
    }

    if (!Page.IsPostBack)
    {
      #region Load dlTRIsoCode
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("_CultureIsoCode_GetAvailable", new System.Data.SqlClient.SqlParameter("@LanguageCode", languageCode)))
        {
          dbObj.CloseConnection();
          dlTRIsoCode.DataSource = ds;
          dlTRIsoCode.DataBind();
          if (dlTRIsoCode.Items.Count == 0)
          {
            dlTRIsoCode.Enabled = txtLanguageName.Enabled = txtLanguageCode.Enabled = txtEncoding.Enabled = false;
            uwToolbar.Items.FromKeyButton("Save").Enabled = false;
            lbError.Text = "All TR Iso code are already used. <br/>You cannot create a language without Translation ISO Code. <br/>Please contact support";
            lbError.CssClass = "hc_error";
            lbError.Visible = true;
          }
        }
      }
      #endregion
    }
  }

  private void UpdateDataEdit()
  {
    if (lang != null)
    {
      #region Retrieve information about the current language
      txtLanguageCode.Text = lang.Code;
      txtLanguageCode.Enabled = false;
      txtLanguageName.Text = lang.Name;
      txtDeliveryLanguageName.Text = lang.DeliveryName;
      txtEncoding.Text = lang.Encoding;
      if (lang.TRIsoCode.ToString() != string.Empty)
      {
        dlTRIsoCode.SelectedValue = lang.TRIsoCode;
      }
      cbRtl.Checked = lang.Rtl;
      #endregion
    }
    else 
    {
      #region New language
      UITools.HideToolBarButton(uwToolbar, "Delete");
      UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      #endregion
    }
  
  }

  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
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
  }

  private void Save()
  {
    if (languageCode == "-1")
    {
      HyperCatalog.Business.Language l = HyperCatalog.Business.Language.GetByKey(txtLanguageCode.Text.ToUpper().Trim());
        if (l == null)
        {
          // Create new language
          lang = new HyperCatalog.Business.Language(txtLanguageCode.Text.ToUpper().Trim(), txtLanguageName.Text.Trim(), txtDeliveryLanguageName.Text.Trim(),
            cbRtl.Checked, txtEncoding.Text.Trim(), dlTRIsoCode.SelectedValue);
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "This language code [" + txtLanguageCode.Text.Trim() + "] already exist !";
          lbError.Visible = true;
          return;
        }
    }
    else
    {
      // update language
      lang = HyperCatalog.Business.Language.GetByKey(languageCode);
      lang.Name = txtLanguageName.Text;
      lang.DeliveryName = txtDeliveryLanguageName.Text;
      lang.Encoding = txtEncoding.Text;
      lang.Rtl = cbRtl.Checked;
      lang.TRIsoCode = dlTRIsoCode.SelectedValue;
    }
    if (lang != null)
    {
      if (lang.Save())
      {
        // create/update
        lbError.Text = "Data saved!";
        lbError.CssClass = "hc_success";
        lbError.Visible = true;
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Language.LastError;
        lbError.Visible = true;
      }
      lang.Dispose();
    }
    else
    {
      lbError.CssClass = "hc_error";
      lbError.Text = "Error: Language not found";
      lbError.Visible = true;
    }
  }
  private void Delete()
  {
    using (HyperCatalog.Business.Language lang = HyperCatalog.Business.Language.GetByKey(languageCode))
    {
      if (lang != null)
      {
        if (lang.Delete(HyperCatalog.Shared.SessionState.User.Id))
        {
          Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", "<script>back();</script>");
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.Language.LastError;
          lbError.Visible = true;
        }
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "Error: Language not found";
        lbError.Visible = true;
      }
    }
  }
}
