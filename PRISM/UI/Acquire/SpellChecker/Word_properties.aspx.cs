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
using HyperCatalog.Shared;
using HyperCatalog.SpellChecker;
using HyperCatalog.Business;
#endregion

	/// <summary>
	/// Display the word properties in language selected
	/// --> Button "Save" to create or update the word
	/// --> Button "Delete" to delete the word
	/// --> Button "List" to return to the Specific words page 
	/// </summary>
  public partial class SpecificWord_Properties : HCPage
  {
    #region Declarations
    private string word, languageCode = string.Empty;
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
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_SPELL_CHECKER)))
      {
        uwToolbar.Items.FromKeyButton("Save").Enabled = false;
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
      }
      #endregion            
      try
      {
        word = Request["w"].ToString();
        if (word == "-1") {word = string.Empty;}
        languageCode = Request["l"].ToString();
        if (!Page.IsPostBack)
        {
          ShowWord();
        }
      }
      catch
      {
        UITools.DenyAccess(DenyMode.Standard);      
      }
    }

    /// <summary>
    /// Display word properties
    /// </summary>
    private void ShowWord()
    {
      #region Retrieve information about the current container
      SpecificDictionaryWord  specificWord = SpecificDictionaryWord.GetByKey(word, languageCode);
      if (specificWord != null)
      {
        txtValue.Text = specificWord.Text;
        txtComment.Text = specificWord.Comment;

        hlSubmitter.Text =  specificWord.SubmitterName;
        hlSubmitter.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(specificWord.Submitter.Email) + Server.HtmlEncode("?subject=The word you've submitted [" + word +"]");
        lbOrganizationSubmitter.Text = specificWord.Submitter.OrgName;
        lbSubmittedOn.Text = SessionState.User.FormatUtcDate(specificWord.SubmitDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
        panelSubmitter.Visible = true;
        if (specificWord.Approver != null)
        {
          hlApprover.Text =  specificWord.ApproverName;
          hlApprover.NavigateUrl = "mailto:" + UITools.GetDisplayEmail(specificWord.Approver.Email) + Server.HtmlEncode("?subject=The word you've apporved [" + word +"]");
          lbOrganizationApprover.Text = specificWord.Approver.OrgName;
          lbApprovedOn.Text = SessionState.User.FormatUtcDate(specificWord.ApproveDate.Value, true, SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime);
          panelApprover.Visible = true;
        }

        UITools.ShowToolBarButton(uwToolbar, "Delete");
        UITools.ShowToolBarSeparator(uwToolbar, "SepDelete");  
      }
      else
      {
        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "SepDelete");  
      }
      #endregion
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
    /// Save the word properties 
    /// </summary>
    private void BtnSave()
    {
      SpecificDictionaryWord w = SpecificDictionaryWord.GetByKey(txtValue.Text, languageCode);
      string m = string.Empty;
      if (w == null)
      {
        w = new HyperCatalog.SpellChecker.SpecificDictionaryWord(txtValue.Text, languageCode, txtComment.Text,
             SessionState.User.Id, SessionState.User.FullName, SessionState.User.Id, SessionState.User.FullName,
             null, null, DateTime.UtcNow, DateTime.UtcNow, false);
        m = "Word created";
      }
      else
      {
        w.ApproverId = SessionState.User.Id;
        w.ApproveDate = DateTime.UtcNow;
        w.Comment = txtComment.Text.Trim();
        w.Approved = false;
        m = "Word updated"; 
      }
      if (w.Save())
      {
        lbMessage.Text = m;
        lbMessage.CssClass = "hc_success";
        lbMessage.Visible = true;
      }
      else
      {
        lbMessage.Text = "Error: Word can't be created/updated";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
    }


    /// <summary>
    /// Delete the word
    /// </summary>
    private void BtnDelete()
    {
      if (!SpecificDictionaryWord.DeleteByKey(word, languageCode,SessionState.User.Id))
      {
        lbMessage.Text = "Error: Word can't be deleted";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>"); 
      }
    }
      
	}
