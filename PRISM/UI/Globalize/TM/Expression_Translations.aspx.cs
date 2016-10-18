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
using HyperCatalog.Shared;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using HyperComponents.Data.dbAccess;
#endregion

	/// <summary>
	/// Display the translations or Localizations of a master TM Expression
	/// --> Button "delete selected" to delete the translation selected in the grid
	/// --> Button "List" to return to the TM page
	/// </summary>
	public partial class Expression_Translation : HCPage
	{
    #region Declaration
    protected Infragistics.WebUI.WebDataInput.WebTextEdit WebTextEdit1;
    private System.Int64 expressionId;
    private TMExpression HCExpression;
    HyperCatalog.Business.LanguageList list = HyperCatalog.Business.Language.GetAll();
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
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

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
      lbMessage.Visible = false;
      if (Request["e"] != null)
      {
          expressionId = Convert.ToInt64(Request["e"]);
          if (!Page.IsPostBack)
          {
            ShowTMTranslations();
          }
      }
		}

    /// <summary>
    /// Show the list of translations for the selected TM Expression
    /// </summary>
    private void ShowTMTranslations()
    {
      using (HCExpression = TMExpression.GetByKey(Convert.ToInt32(expressionId)))
      {
        int TranslationsCount = HCExpression.Translations.Count - HCExpression.TranslationsMissingCount;
        UITools.RefreshTab(Page, "Translations", "'" + TranslationsCount.ToString() + "/" + HCExpression.Translations.Count.ToString() + "'");
        using (TMExpressionTranslationList list = TMExpressionTranslation.GetAll(expressionId, string.Empty))
        {
          dg.DataSource = list;
          Utils.InitGridSort(ref dg, false);
          dg.DisplayLayout.CellClickActionDefault = CellClickAction.NotSet;
          dg.DisplayLayout.ActivationObject.AllowActivation = false;
          dg.DataBind();
        }
      }
    }

    /// <summary>
    /// Display TM Expression translations/localizations datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      #region Display Edit Link in LanguageCode
      for (int i = 0; i < list.Count; i++)
      {
        if (list[i].Code == e.Row.Cells.FromKey("LanguageCode").Text)
        {
          //Display Edit Link in languageCode
          e.Row.Cells.FromKey("Language").Text = "[" + e.Row.Cells.FromKey("LanguageCode").Text + "] " + list[i].Name;
        }
      }
      #endregion
      #region Display Rtl style
      bool rtl = e.Row.Cells.FromKey("Rtl").Text.ToLower() == "true";
      if (rtl)
      {
        e.Row.Cells.FromKey("TMExpressionValue").Style.CssClass  = "rtl";
      }
      #endregion
      #region Display Value in Textbox
      TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Value").Column;
      TextBox tb = (TextBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("TXTChangedValue");
      tb.Text = e.Row.Cells.FromKey("TMExpressionValue").Text;
      tb.CssClass = e.Row.Cells.FromKey("TMExpressionValue").Style.CssClass;
      #endregion

    }

    /// <summary>
    ///  Action for toolbar buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Save
      if ( btn == "save")
      {
        Save();
      }
      #endregion
      #region Delete selected
      if ( btn == "delete")
      {
        Delete();
      }
      #endregion
    }

    /// <summary>
    /// Delete the selected translation of the TM Expression
    /// </summary>
    private void Delete()
    {
      for (int i=0; i<dg.Rows.Count; i++)
      {
        TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
        if (cb.Checked)
        {
          using (TMExpressionTranslation tt = TMExpressionTranslation.GetByKey(expressionId, dg.Rows[i].Cells.FromKey("LanguageCode").ToString()))
          {
            if (tt != null)
            {
              #region Delete result
              if (!tt.Delete(HyperCatalog.Shared.SessionState.User.Id))
              {
                lbMessage.Text = "Error: Translation [" + dg.Rows[i].Cells.FromKey("LanguageCode").ToString() + "] can't be deleted";
                lbMessage.CssClass = "hc_error";
                lbMessage.Visible = true;
                break;
              }
              #endregion
            }
            else
            {
              lbMessage.Text = "Error: Term translation [" + dg.Rows[i].Cells.FromKey("Language").ToString() + "] can't be deleted - translation not found";
              lbMessage.CssClass = "hc_error";
              lbMessage.Visible = true;
            }
          }
        }
      }
      ShowTMTranslations();
      lbMessage.Text = "Data deleted";
      lbMessage.CssClass = "hc_success";
      lbMessage.Visible = true;
    }

    /// <summary>
    /// Save translations
    /// </summary>
    private void Save()
    {
      bool canUpdate;
      for (int i=0; i<dg.Rows.Count; i++)
      {
        string languageCode = dg.Rows[i].Cells.FromKey("LanguageCode").Text;
        TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Value").Column;
        TextBox tb = (TextBox)((CellItem)col.CellItems[i]).FindControl("TXTChangedValue");
        string newTMExpressionValue = tb.Text;
        TMExpressionTranslation tt;
        using (tt = TMExpressionTranslation.GetByKey(expressionId, languageCode))
        {
          if (tt == null)
          {
            canUpdate = newTMExpressionValue != null;
            if (canUpdate) canUpdate = newTMExpressionValue.Trim() != string.Empty;
            #region New Translation
            if (canUpdate)
            {
              tt = new TMExpressionTranslation(expressionId, newTMExpressionValue, languageCode, dg.Rows[i].Cells.FromKey("Rtl").Text.ToLower() == "true",
                SessionState.User.Id, DateTime.UtcNow);
              int r = tt.Save();
              if (r < 0)
              {
                lbMessage.Text = "Error: Translation [" + languageCode + "] can't be created";
                lbMessage.CssClass = "hc_error";
                lbMessage.Visible = true;
                break;
              }
            }
            #endregion
          }
          #region Translation already exist
          else
          {
            if (tt.Value != newTMExpressionValue)
            {
              tt.LanguageCode = languageCode;
              tt.Value = newTMExpressionValue;
              tt.Rtl = dg.Rows[i].Cells.FromKey("Rtl").Text.ToLower() == "true";
              canUpdate = newTMExpressionValue != null;
              if (canUpdate) canUpdate = newTMExpressionValue.Trim() != string.Empty;
              #region Value modified
              if (canUpdate)
              {
                int r = tt.Save();
                if (r < 0)
                {
                  lbMessage.Text = "Error: Translation [" + languageCode + "] can't be updated";
                  lbMessage.CssClass = "hc_error";
                  lbMessage.Visible = true;
                  break;
                }
              }
              #endregion
              #region Value deleted
              else
              {
                if (!tt.Delete(HyperCatalog.Shared.SessionState.User.Id))
                {
                  lbMessage.Text = "Error: localization [" + languageCode + "] can't be deleted";
                  lbMessage.CssClass = "hc_error";
                  lbMessage.Visible = true;
                  break;
                }
              }
              #endregion
            }
          }
          #endregion
        }
      }
      ShowTMTranslations();
      lbMessage.Text = "Data saved";
      lbMessage.CssClass = "hc_success";
      lbMessage.Visible = true;
      using (Database dbObj = Utils.GetMainDB())
      {
        UITools.RefreshTab(Page, "Translations", Utils.GetCount(dbObj, String.Format("SELECT COUNT(*) FROM TMTranslations WHERE TMExpressionId = {0}",expressionId)));
      }
    }
	}
