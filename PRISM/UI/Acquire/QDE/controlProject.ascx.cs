namespace HyperCatalog.UI.Acquire.QDE
{
  #region uses
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
  using HyperCatalog.Business;
  using HyperCatalog.Shared;
  #endregion

	/// <summary>
	/// Description résumée de controlProject.
	/// </summary>
	public partial class controlProject : System.Web.UI.UserControl
	{
    string _LastError = string.Empty;

    #region published properties of the control
    public object Bop
    {
      get { return wdBop.Value; }
    }
    public object Eoa
    {
      get { return wdEoa.Value; }
      //set { wdEoa.Value = value; }
    }
    public object Eov
    {
      get { return wdEov.Value; }
      //set { wdEov.Value = value; }
    }
    public object Bot
    {
      get { return wdBot.Value; }
      //set { wdBot.Value = value; }
    }
    public object Eot
    {
      get { return wdEot.Value; }
      //set { wdBot.Value = value; }
    }
    public HyperCatalog.Business.Item Item
    {
      set { ViewState["Item"] = value; InitItem(); }
      get {return ViewState["Item"]==null?null:(Item)ViewState["Item"];}
    }
    public bool IsNewProject
    {
      get {return ViewState["IsNewProject"]==null?false:(bool)ViewState["IsNewProject"];}
    }
    public ListItemCollection Languages
    {
      get 
      {
        ListItemCollection items = new ListItemCollection();
        foreach (ListItem obj in cblLanguageScope.Items)
        {
          if (obj.Selected)
          {
            items.Add(new ListItem(obj.Text, obj.Value));
          }        
        }  
        return items;
      }
    }    
    public bool Enabled
    {
      get
      {
        return ViewState["PCE"] != null ? (bool)ViewState["PCE"] : false;
        
    }
      set
      {
        ViewState["PCE"] = wdBop.Enabled = wdEoa.Enabled =
             wdEov.Enabled = wdBot.Enabled = ddlScopes.Enabled = cblLanguageScope.Enabled = value;
			}
    }
    public string LastError
    {
      get {return _LastError;}
    }
    public bool CanOverrideProject
    {
      get { return ViewState["CanOverrideProject"] != null ? (bool)ViewState["CanOverrideProject"] : false; }
      set { ViewState["CanOverrideProject"] = lnkOverride.Visible = this.Enabled = value; }
    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
        _LastError = string.Empty;
        cvEoa.ValidationGroup = cvBot.ValidationGroup = cvEov.ValidationGroup = valEov.ValidationGroup = valBot.ValidationGroup = pVal.ClientID;
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
        ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
        wdBop.CalendarLayout.Culture = ci;
        wdEoa.CalendarLayout.Culture = ci;
        wdEov.CalendarLayout.Culture = ci;
        wdBot.CalendarLayout.Culture = ci;
        wdEot.CalendarLayout.Culture = ci;
    }

 

    private void InitItem()
    {
      #region Populate Languages Check box list
      string filter = "LanguageCode <> dbo.GetLanguageCode('" + HyperCatalog.Shared.SessionState.Culture.Code + "') AND LanguageCode NOT IN (SELECT LanguageCode FROM Cultures Cu, Countries C WHERE Cu.CountryCode = C.CountryCode AND C.PLCDrivenTranslation = 1)";
      cblLanguageScope.DataSource = HyperCatalog.Business.Language.GetByCulture(HyperCatalog.Shared.SessionState.Culture.Code, filter);
      cblLanguageScope.DataBind();
      #endregion
      
			#region Populate Scopes combo box
			if (cblLanguageScope.Items.Count > 0)
			{
                       //Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009
               using (TRScopeList trScopeAll = TRScope.GetAll("RegionCode = '"+ SessionState.Culture.Code+"'" ))
                {
                  ddlScopes.DataSource = trScopeAll;
                  ddlScopes.DataBind();
                  ddlScopes.Items.Insert(0, new ListItem("---> Please make your choice <---", string.Empty));
                  panelScopeTitle.Visible = true;
                  panelScopeLanguages.Visible = true;
                }
                //Modified by Sateesh for Language Scope Management (PCF: ACQ 3.6) - 27/05/2009
			}
			else
			{
				panelScopeTitle.Visible = false;
				panelScopeLanguages.Visible = false;
			}
      #endregion

      //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09
      SessionState.CurrentItem.RegionCode = SessionState.Culture.Code;
        if(Item.Milestones.InheritedItem != null)
            SessionState.CurrentItem.Milestones.InheritedItem.RegionCode = SessionState.Culture.Code;
      Enabled = true;
      //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 22/Jul/09
      //Fix for QC 2765 - UAT: Beginning of Regionalization Date when setting a new project does not default to today's date
      // SET BOR date same as BOP for new Projects
      wdBop.Value = wdEoa.Value = DateTime.Now;
      wdEov.Value = wdBot.Value = wdEot.Value = null;
      if (Item != null && Item.Milestones != null && Item.Milestones.BeginningOfRegionalization.HasValue)
      {
        panelScopeLanguages.Visible = panelScopeTitle.Visible = Item.TranslationMode == TRClassTranslationMode.Standard;
        wdBop.Value = Item.Milestones.CreationDate;
        //wdBop.Value = Item.CreateDate.Value;
        if (Item.Milestones.InheritedItem != null && Item.Milestones.InheritedItem.Milestones.CreationDate.HasValue)
        {
          wdBop.Value = Item.Milestones.InheritedItem.Milestones.CreationDate.Value;
        }
        if (Item.Milestones.BeginningOfRegionalization != null)
            wdEoa.Value = Item.Milestones.BeginningOfRegionalization.Value;
        if (Item.Milestones.EndOfRegionalization != null)
            wdEov.Value = Item.Milestones.EndOfRegionalization.Value;
        if (Item.Milestones.BeginningOfTranslation != null)
          wdBot.Value = Item.Milestones.BeginningOfTranslation.Value;
        if (Item.Milestones.EndOfTranslation != null)
          wdEot.Value = Item.Milestones.EndOfTranslation.Value;

        cblLanguageScope.ClearSelection();
        foreach (HyperCatalog.Business.Language cul in Item.Translations)
        {
          foreach (ListItem item in cblLanguageScope.Items)
          {
            if (item.Value == cul.Code)
            {
              item.Selected = true;
            }
          }
        }
        if (Item.Milestones.Inherited)
        {
          // Propose to the user to create its own project.
          // In the meantime, disable everything
          Enabled = false;
          lnkOverride.Visible = true;
        }
      }
      else
      {
        wdBop.Value = DateTime.UtcNow;
      }

      // if it is not region, then user cannot modify master publishing date, beginning of project, 
      // end of translation, beginning of localization and end of localization
      //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 26/May/09 -- check
      //if (HyperCatalog.Shared.SessionState.Culture.Type != HyperCatalog.Business.CultureType.Master) 
      if (HyperCatalog.Shared.SessionState.Culture.Type != HyperCatalog.Business.CultureType.Regionale)
      {
          panelBOP.Visible = panelEOT.Visible = false;
          wdEoa.Enabled = ddlScopes.Enabled = lnkOverride.Visible = wBtnAdd.Enabled = wBtnReplace.Enabled = false;

          if (HyperCatalog.Shared.SessionState.Culture.Type == HyperCatalog.Business.CultureType.Locale)
        {
            cblLanguageScope.Enabled = false;
        }
      }
      if (cblLanguageScope.Items.Count == 0)
      {
          //Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 26/May/09 -- check
          panelScopeLanguages.Visible = panelScopeTitle.Visible = false;
      }
      if (Item != null && Item.IsMinimizedByCulture(SessionState.Culture.Code))
      {
        wdEoa.Enabled = wdBop.Enabled = wdEot.Enabled = wdEov.Enabled = wdBot.Enabled =
          ddlScopes.Enabled = lnkOverride.Visible = wBtnAdd.Enabled = wBtnReplace.Enabled = cblLanguageScope.Enabled = false;
      }
      // Item is a roll, then scope of language is disable
      //if (Item != null && Item.IsRoll)
      //{
      //  ddlScopes.Enabled = cblLanguageScope.Enabled = panelScopeLanguages.Enabled = false;
      //  wBtnAdd.Enabled = wBtnReplace.Enabled = false;
      //}
    }
    public void InitScopeFromParent(HyperCatalog.Business.Item parentItem)
    {
      if (parentItem!=null)
      {
        cblLanguageScope.ClearSelection();
        foreach (HyperCatalog.Business.Language cul in parentItem.Translations)
        {
          foreach (ListItem item in cblLanguageScope.Items)
          {
            if (item.Value == cul.Code)
            {
              item.Selected = true;
            }
          }
        }
      }
    }
    public bool ValidateProjectDates()
    {
      _LastError = string.Empty;
      // If some languages have been selected, we must ensure that some dates are provided
      if (Languages.Count > 0)
      {
        if (wdBot.Value == System.DBNull.Value)
        {
          _LastError = "If no scope is defined, you cannot specify translation dates";
        }
      }
      return _LastError == string.Empty;
    }

    protected void lnkOverride_Click(object sender, System.EventArgs e)
    {
      InitItem();
      ViewState["IsNewProject"] = true;
      wdBop.Value = wdEoa.Value = DateTime.Now;        
      wdEov.Value = wdBot.Value = wdEot.Value = null;
      lnkOverride.Visible = false;this.Enabled = true;
    }

    protected void wBtnReplace_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (ddlScopes.SelectedIndex > 0)
      {
        for (int i=0; i< cblLanguageScope.Items.Count; i++)
        {
          cblLanguageScope.Items[i].Selected = false;
        }
        TRScope scope = TRScope.GetByKey(Convert.ToInt32(ddlScopes.SelectedValue));
        if (scope!=null)
        {
          foreach (TRScopeLanguage l in scope.Languages)
          {
            for (int i=0; i< cblLanguageScope.Items.Count; i++)
            {
              if (cblLanguageScope.Items[i].Value == l.LanguageCode)
              {
                cblLanguageScope.Items[i].Selected = true;
              }
            }
          }
        }
        ddlScopes.SelectedIndex = 0;
      }
    }
    protected void wBtnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
      if (ddlScopes.SelectedIndex > 0)
      {
        TRScope scope = TRScope.GetByKey(Convert.ToInt32(ddlScopes.SelectedValue));
        if (scope!=null)
        {
          foreach (TRScopeLanguage l in scope.Languages)
          {
            for (int i=0; i< cblLanguageScope.Items.Count; i++)
            {
              if (cblLanguageScope.Items[i].Value == l.LanguageCode)
              {
                cblLanguageScope.Items[i].Selected = true;
              }
            }
          }
        }
        ddlScopes.SelectedIndex = 0;
      }
    }
 }
}
