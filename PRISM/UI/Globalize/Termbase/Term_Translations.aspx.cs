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
	/// Display the translations of a master term
	/// --> Button "delete selected" to delete the translation selected in the grid
	/// --> Button "List" to return to the Termbase page
	/// </summary>
	public partial class term_translations : HCPage
	{
    #region Declarations
    protected Infragistics.WebUI.WebDataInput.WebTextEdit WebTextEdit1;
    private System.Int64 termId;
    private Term HCTerm;
    LanguageList langList = HyperCatalog.Business.Language.GetAll();
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
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TERM_BASE)))
      {
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
      }
      #endregion
      if (Request["t"] != null)
      {        
        try
        {
          termId = Convert.ToInt64(Request["t"]);
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"ifid", "<script>var termid = '" + termId + "';</script>");
          if (!Page.IsPostBack)
          {
            ShowTermTranslations();
          }
          else
          {
            // action after changes in term translation edit window 
            if (Request["action"] != null && Request["action"].ToString().ToLower()=="reload")
            {
              ShowTermTranslations();
            }
          }
        }
        catch
        {
          UITools.DenyAccess(DenyMode.Standard);
        }
      }
		}

    /// <summary>
    /// Show the list of translations for the selected term
    /// </summary>
    private void ShowTermTranslations()
    {
      using (HCTerm = Term.GetByKey(Convert.ToInt32(termId)))
      {
        int TranslationsCount = HCTerm.Translations.Count - HCTerm.TranslationsMissingCount;
        UITools.RefreshTab(Page, "Translations", "'" + TranslationsCount.ToString() + "/" + HCTerm.Translations.Count.ToString() + "'");
        dg.DataSource = HCTerm.Translations;
        Utils.InitGridSort(ref dg);
        dg.DataBind();
      }
    }

    /// <summary>
    /// Display term translations datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      //Display Edit Link in LanguageCode
      for (int i = 0; i < langList.Count; i++)
      {
        if (langList[i].Code == e.Row.Cells.FromKey("LanguageCode").Text)
        {
          e.Row.Cells.FromKey("Language").Text = "<a href='javascript://' onclick=\"SC('" + e.Row.Cells.FromKey("LanguageCode").Text + "')\">[" + e.Row.Cells.FromKey("LanguageCode").Text + "] " + langList[i].Name + "</a>";
        }
      }
      #region Display Rtl style
      bool rtl = e.Row.Cells.FromKey("Rtl").Text.ToLower() == "true";
      if (rtl)
      {
        e.Row.Cells.FromKey("TermValue").Style.CssClass  = "rtl";
      }
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
      #region Delete selected
      if ( btn == "delete")
      {
        Delete();
      }
      #endregion
    }

    /// <summary>
    /// Delete the selected translation of the term
    /// </summary>
    private void Delete()
    {
      lbMessage.Text = string.Empty;
      for (int i=0; i<dg.Rows.Count; i++)
      {
        TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
        if (cb.Checked)
        {
          using (TermTranslation tt = TermTranslation.GetByKey(termId, dg.Rows[i].Cells.FromKey("LanguageCode").ToString()))
          {
            if (tt != null)
            {
              #region Delete result
              if (!tt.Delete(HyperCatalog.Shared.SessionState.User.Id))
              {
                lbMessage.Text = dg.Rows[i].Cells.FromKey("Language").ToString() + " - " + TermTranslation.LastError;
                lbMessage.CssClass = "hc_error";
                lbMessage.Visible = true;
                break;
              }
              #endregion
            }
          }
        }
      }
      ShowTermTranslations();
      lbMessage.Text = "Data deleted";
      lbMessage.CssClass = "hc_success";
      lbMessage.Visible = true;
    }
	}
