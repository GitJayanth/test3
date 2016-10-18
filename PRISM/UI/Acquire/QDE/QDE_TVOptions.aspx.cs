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

	/// <summary>
	/// Description résumée de QDE_TVOptions.
	/// </summary>
public partial class QDE_TVOptions : HCPage
{
	#region Declarations
	protected System.Web.UI.WebControls.Repeater rOptions;

	private HyperCatalog.Business.User user;
	#endregion

  #region Code généré par le Concepteur Web Form
  override protected void OnInit(EventArgs e)
  {
    //
    // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
    //
		InitializeComponent();
		//this.cbComment.CheckedChanged += new System.EventHandler(this.cbComment_CheckedChanged);
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
		lbError.Visible = false;
		lbError.Text = string.Empty;

		user = SessionState.User;
		if (!Page.IsPostBack)
		{
			UpdateDataView();
		}
	}

	private void UpdateDataView()
	{
    // Hide all component
    cbComment.Visible = false;
    cbCulture.Visible = false;
    cbInheritanceMode.Visible = false;
    cbToolbarStat.Visible = false;
    cbShowLinkCount.Visible = false;
    lbComment.Visible = false;
    lbCulture.Visible = false;
    lbInheritanceMode.Visible = false;
    lbToolbarStat.Visible = false;
    lbShowLinkCount.Visible = false;

		// Update view obsolete
		cbViewObsoletes.Checked = user.ViewObsoletes;

		// Update comment
    Option optComment = Option.GetByKey((int)OptionsEnum.OPT_SHOW_COMMENT);
    if (optComment != null)
    {
      lbComment.Text = optComment.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_COMMENT) != null)
        cbComment.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_COMMENT).Value;
      lbComment.Visible = true;
      cbComment.Visible = true;
    }
    // Update culture
    Option optCulture = Option.GetByKey((int)OptionsEnum.OPT_SHOW_CULTURE);
    if (optCulture != null)
    {
      lbCulture.Text = optCulture.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_CULTURE) != null)
        cbCulture.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_CULTURE).Value;
      lbCulture.Visible = true;
      cbCulture.Visible = true;
    }
    // Update toolbar containing the statistics
    Option optToolbar = Option.GetByKey((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT);
    if (optToolbar != null)
    {
      lbToolbarStat.Text = optToolbar.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT) != null)
        cbToolbarStat.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT).Value;
      lbToolbarStat.Visible = true;
      cbToolbarStat.Visible = true;
    }
    // Update Inheritance Mode viewability
    Option optInheritance = Option.GetByKey((int)OptionsEnum.OPT_SHOW_INHERITANCEMODE);
    if (optInheritance != null)
    {
      lbInheritanceMode.Text = optInheritance.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_INHERITANCEMODE) != null)
        cbInheritanceMode.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_INHERITANCEMODE).Value;
      lbInheritanceMode.Visible = true;
      cbInheritanceMode.Visible = true;
    }
    // Update Chunk Move viewability
    Option optChunkMoveNext = Option.GetByKey((int)OptionsEnum.OPT_CHUNK_MOVENEXT);
    if (optChunkMoveNext != null)
    {
      lbChunkMoveNext.Text = optChunkMoveNext.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_CHUNK_MOVENEXT) != null)
        cbChunkMoveNext.Checked = user.GetOptionById((int)OptionsEnum.OPT_CHUNK_MOVENEXT).Value;
      lbChunkMoveNext.Visible = true;
      cbChunkMoveNext.Visible = true;
    }
    // Update Chunk simplified window
    Option optChunkShowSimplified = Option.GetByKey((int)OptionsEnum.OPT_SHOW_SIMPLIFIED_CHUNK_WINDOW);
    if (optChunkShowSimplified != null)
    {
      lbChunkShowSimplified.Text = optChunkShowSimplified.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_SIMPLIFIED_CHUNK_WINDOW) != null)
        cbChunkShowSimplified.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_SIMPLIFIED_CHUNK_WINDOW).Value;
      lbChunkShowSimplified.Visible = true;
      cbChunkShowSimplified.Visible = true;
    }

    // Update Translation viewability
    Option optShowTranslation = Option.GetByKey((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES);
    if (optShowTranslation != null)
    {
      lbShowTranslatableName.Text = optShowTranslation.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES) != null)
        cbShowTranslatableName.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value;
      lbShowTranslatableName.Visible = true;
      cbShowTranslatableName.Visible = true;
    }

    // Update Link count visibility
    Option optShowLinkCount = Option.GetByKey((int)OptionsEnum.OPT_SHOW_LINK_COUNT);
    if (optShowLinkCount != null)
    {
      lbShowLinkCount.Text = optShowLinkCount.Name;
      if (user.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT) != null)
        cbShowLinkCount.Checked = user.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT).Value;
      lbShowLinkCount.Visible = true;
      cbShowLinkCount.Visible = true;
    }
    // Update shrink  viewability
    /*Option optShowShrinkName = user.GetOptionById((int)OptionsEnum.OPT_SHOW_SHRINKED_NAMES);
    if (optShowShrinkName != null)
    {
      lbShowShrinkedName.Text = optShowShrinkName.Name;
      cbShowShrinkedName.Checked = optShowShrinkName.Value;
      lbShowShrinkedName.Visible = true;
      cbShowShrinkedName.Visible = true;
    }*/
  }

	#region "Event methods"
	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "save")
		{
			Save();
		}
	}
	#endregion

	#region "Persistence"
	private void Save()
	{
		// Update 'View obsolete' option
		SessionState.User.ViewObsoletes = cbViewObsoletes.Checked;
    if (!SessionState.User.QuickSave())
		{
			lbError.CssClass = "hc_error";
			lbError.Text = HyperCatalog.Business.User.LastError;
			lbError.Visible = true;
			return;
		}

		// Update 'comment' option
		Option optComment = user.GetOptionById((int)OptionsEnum.OPT_SHOW_COMMENT);
    if (optComment != null)
    {
      optComment.Value = cbComment.Checked;
      if (!optComment.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

		// Update 'culture' option
		Option optCulture = user.GetOptionById((int)OptionsEnum.OPT_SHOW_CULTURE);
    if (optCulture != null)
    {
      optCulture.Value = cbCulture.Checked;
      if (!optCulture.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

		// Update 'toolbar stat' option
		Option optToolbar = user.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT);
    if (optToolbar != null)
    {
      optToolbar.Value = cbToolbarStat.Checked;
      if (!optToolbar.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

    // Update 'toolbar stat' option
    Option optInheritanceMethod = user.GetOptionById((int)OptionsEnum.OPT_SHOW_INHERITANCEMODE);
    if (optInheritanceMethod != null)
    {
      optInheritanceMethod.Value = cbInheritanceMode.Checked;
      if (!optInheritanceMethod.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

    // Update 'chunk move next' option
    Option optChunkMoveNext = user.GetOptionById((int)OptionsEnum. OPT_CHUNK_MOVENEXT);
    if (optChunkMoveNext != null)
    {
      optChunkMoveNext.Value = cbChunkMoveNext.Checked;
      if (!optChunkMoveNext.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

    // Update 'Simplified chunk window' option
    Option optChunkSimplifiedWindow = user.GetOptionById((int)OptionsEnum.OPT_SHOW_SIMPLIFIED_CHUNK_WINDOW);
    if (optChunkSimplifiedWindow != null)
    {
      optChunkSimplifiedWindow.Value = cbChunkShowSimplified.Checked;
      if (!optChunkSimplifiedWindow.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

    // Update 'Show Translation in Tree view' option
    Option optShowTranslation = user.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES);
    if (optShowTranslation != null)
    {
      optShowTranslation.Value = cbShowTranslatableName.Checked;
      if (!optShowTranslation.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }    
    
    // Update 'Show Translation in Tree view' option
    Option optShowLinkCount = user.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT);
    if (optShowLinkCount != null)
    {
      optShowLinkCount.Value = cbShowLinkCount.Checked;
      if (!optShowLinkCount.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }

    // Update 'Show shrinked names in tree view' option
    /*Option optShowShrinkName = user.GetOptionById((int)OptionsEnum.OPT_SHOW_SHRINKED_NAMES);
    if (optShowShrinkName != null)
    {
      optShowShrinkName.Value = cbShowShrinkedName.Checked;
      if (!optShowShrinkName.UpdateOptionByUserId(user.Id))
      {
        lbError.CssClass = "hc_error";
        lbError.Text = HyperCatalog.Business.Option.LastError;
        lbError.Visible = true;
        return;
      }
    }*/

    lbError.CssClass = "hc_success";
		lbError.Text = "Data saved!";
		lbError.Visible = true;
    SessionState.User.Options.Clear();
    SessionState.User.Options = null;

		// Update frame content (grid)
		UpdateFrameContent();
	}
	#endregion

	private void UpdateFrameContent()
	{
		string sTab = SessionState.QDETab;
		if (sTab != null && sTab.Length > 0)
		{
			if (!sTab.Equals("tb_info") && !sTab.Equals("tb_delivery"))
			{
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"UpdateFrameContent", "<script>UpdateGrid();</script>");
			}
		}
	}
}
