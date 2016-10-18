#region uses
using System;
using System.Configuration;
using System.Data;
using System.Web.SessionState;
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;
using System.Web;
using System.Web.UI.WebControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperComponents.WebUI.CustomSpellChecker;
#endregion

public enum DenyMode
{
    /// <summary>
    /// Used for popup windows. Alert and close
    /// </summary>
    Popup,
    /// <summary>
    /// Used if page is supposed to be called inside a frame/iframe
    /// Alert and History.Back()
    /// </summary>
    Frame,
    /// <summary>
    /// Redirect to Forbidden.aspx
    /// </summary>
    Standard
}
/// <summary>
/// Description résumée de ProductLibrary.
/// </summary>
public class UITools
{

    #region PLC grid header or label
    public static void UpdatePLCGridHeader(Infragistics.WebUI.UltraWebGrid.UltraWebGrid dg)
    {
        #region PID
        if (dg.Columns.FromKey("PID") != null)
        {
            dg.Columns.FromKey("PID").Header.Caption = SessionState.CacheParams["HeaderShortNameLive"].Value.ToString();
            dg.Columns.FromKey("PID").Header.Title = SessionState.CacheParams["HeaderLongNameLive"].Value.ToString();
        }
        #endregion
        #region POD
        if (dg.Columns.FromKey("POD") != null)
        {
            dg.Columns.FromKey("POD").Header.Caption = SessionState.CacheParams["HeaderShortNameObsolete"].Value.ToString();
            dg.Columns.FromKey("POD").Header.Title = SessionState.CacheParams["HeaderLongNameObsolete"].Value.ToString();
        }
        #endregion
        #region Blind
        if (dg.Columns.FromKey("Blind") != null)
        {
            dg.Columns.FromKey("Blind").Header.Caption = SessionState.CacheParams["HeaderShortNameBlind"].Value.ToString();
            dg.Columns.FromKey("Blind").Header.Title = SessionState.CacheParams["HeaderLongNameBlind"].Value.ToString();
        }
        #endregion
        #region Announcement
        if (dg.Columns.FromKey("Announcement") != null)
        {
            dg.Columns.FromKey("Announcement").Header.Caption = SessionState.CacheParams["HeaderShortNameAnnouncement"].Value.ToString();
            dg.Columns.FromKey("Announcement").Header.Title = SessionState.CacheParams["HeaderLongNameAnnouncement"].Value.ToString();
        }
        #endregion
        #region Support
        if (dg.Columns.FromKey("Support") != null)
        {
            dg.Columns.FromKey("Support").Header.Caption = SessionState.CacheParams["HeaderShortNameSupport"].Value.ToString();
            dg.Columns.FromKey("Support").Header.Title = SessionState.CacheParams["HeaderLongNameSupport"].Value.ToString();
        }
        #endregion
        #region End of Life
        if (dg.Columns.FromKey("EndOfLife") != null)
        {
            dg.Columns.FromKey("EndOfLife").Header.Caption = SessionState.CacheParams["HeaderShortNameEndOfLife"].Value.ToString();
            dg.Columns.FromKey("EndOfLife").Header.Title = SessionState.CacheParams["HeaderLongNameEndOfLife"].Value.ToString();
        }
        #endregion
        #region Discontinue
        if (dg.Columns.FromKey("Discontinue") != null)
        {
            dg.Columns.FromKey("Discontinue").Header.Caption = SessionState.CacheParams["HeaderShortNameDiscontinue"].Value.ToString();
            dg.Columns.FromKey("Discontinue").Header.Title = SessionState.CacheParams["HeaderLongNameDiscontinue"].Value.ToString();
        }
        #endregion
        #region Removal
        if (dg.Columns.FromKey("Removal") != null)
        {
            dg.Columns.FromKey("Removal").Header.Caption = SessionState.CacheParams["HeaderShortNameRemoval"].Value.ToString();
            dg.Columns.FromKey("Removal").Header.Title = SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString();
        }
        #endregion
    }
    //Modified by Radha S for the QC - 6182 Label change to include the PMG acronyms --Start
    public static string PIDLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameLive"].Value.ToString();
            return SessionState.CacheParams["HeaderLongNameLive"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameLive"].Value.ToString() + ")";
        }
    }

    public static string PODLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameObsolete"].Value.ToString();
            return SessionState.CacheParams["HeaderLongNameObsolete"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameObsolete"].Value.ToString() + ")";
        }
    }

    public static string BlindLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameBlind"].Value.ToString();
            return SessionState.CacheParams["HeaderLongNameBlind"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameBlind"].Value.ToString() + ")";
        }
    }

    public static string AnnouncementLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameAnnouncement"].Value.ToString();
            //Code modified by Prachi on 22nd Sept 2012 for QC6182 (Prism 9.3)
            //return SessionState.CacheParams["HeaderLongNameAnnouncement"].Value.ToString() + "(" + SessionState.CacheParams["HeaderShortNameAnnouncement"].Value.ToString() + ")";
            return SessionState.CacheParams["HeaderLongNameAnnouncement"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameAnnouncement_WR"].Value.ToString() + ")";
        }
    }

    public static string DiscontinueLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameDiscontinue"].Value.ToString();
            return SessionState.CacheParams["HeaderLongNameDiscontinue"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameDiscontinue"].Value.ToString() + ")";
        }
    }

    public static string RemovalLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString();
            //Code modified by Prachi on 22nd Sept 2012 for QC6182 (Prism 9.3)
            //return SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString() + "(" + SessionState.CacheParams["HeaderShortNameRemoval"].Value.ToString() + ")";
            return SessionState.CacheParams["HeaderLongNameRemoval"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameRemoval_WR"].Value.ToString() + ")";
        }
    }

    public static string EOLLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameEndOfLife"].Value.ToString();
            return SessionState.CacheParams["HeaderLongNameEndOfLife"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameEndOfLife"].Value.ToString() + ")";
        }
    }

    public static string SupportLabel
    {
        get
        {
            //return SessionState.CacheParams["HeaderLongNameSupport"].Value.ToString();
            return SessionState.CacheParams["HeaderLongNameSupport"].Value.ToString() + " (" + SessionState.CacheParams["HeaderShortNameSupport"].Value.ToString() + ")";
        }
    }
    //Modified by Radha S for the QC - 6182 Label change to include the PMG acronyms --End
    #endregion

    public static string GetDisplayEmail(string realEmail)
    {
        if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.SEE_REAL_EMAIL))
        {
            return realEmail;
        }
        else
        {
            return SessionState.CacheParams["QualitySupportEmail"].Value.ToString();
        }
    }
    public static string GetDisplayName(string realName)
    {
        if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.SEE_REAL_EMAIL))
        {
            return realName;
        }
        else
        {
            return "contact Quality Team";
        }
    }

    public static bool CheckConnection(System.Web.UI.Page p)
    {
        //Commented on 01/18/2007 to enhance perf
        /*if (SessionState.User == null)
        {
          p.Response.Expires = 0;
          p.Response.Cache.SetNoStore();
          System.Web.HttpContext.Current.Response.Write("<script>var loginPage='" + p.ResolveUrl("~/login.aspx") + "';</script>\n");
          p.Response.Write("<SCRIPT src='/hc_v4/js/close.js?" + DateTime.Now.ToOADate().ToString() + "'></SCRIPT>");
          p.Response.End();
        }*/
        return true;
    }

    public static string GenerateHTMLSpacer(int nbSpaces)
    {
        string s = string.Empty;
        for (int i = 0; i < nbSpaces; i++)
        {
            s += "&nbsp;";
        }
        return s;
    }

    public static string GenerateSpacer(int nbSpaces)
    {
        return new string(' ', nbSpaces);
    }

    public static void ShowSearchBar(System.Web.UI.Page p)
    {
        p.ClientScript.RegisterStartupScript(p.GetType(), "ShowBar", "<script>document.getElementById(panelSearchId).style.display='';</script>");
    }
    public static void HideSearchBar(System.Web.UI.Page p)
    {
        p.ClientScript.RegisterStartupScript(p.GetType(), "HideBar", "<script>document.getElementById(panelSearchId).style.display='none';</script>");
    }
    public static void UpdateSearchBar(System.Web.UI.Page p)
    {
        p.ClientScript.RegisterStartupScript(p.GetType(), "SearchText", "<script>top.document.getElementById(top.txtSearchId).value='Search [" + SessionState.Culture.Code + "]';</script>");
    }
    public static void UpdateTitle(System.Web.UI.Page p)
    {
        string title = string.Empty;
        switch (SessionState.Culture.Type)
        {
            case HyperCatalog.Business.CultureType.Master:
                title = "Enter master content";
                break;
            case HyperCatalog.Business.CultureType.Regionale:
                title = "Regionalize - " + SessionState.Culture.Name;
                break;
            case HyperCatalog.Business.CultureType.Locale:
                title = "Country - " + SessionState.Culture.Name;
                break;
        }
        p.ClientScript.RegisterStartupScript(p.GetType(), "UpdateTitle", "<script>UpdateTitle('" + title + "');</script>");
    }

    public static void FindUserFirstCulture(bool localeIsOk)
    {

        bool found = true;
        if (SessionState.Culture == null || (!localeIsOk && SessionState.Culture.Type == CultureType.Locale))
        {
            found = false;
            LogNet.log.Debug("Default UITools: FindUserFirstCulture : Start Time=" + DateTime.Now);
            using (CultureList culs = SessionState.User.ItemCulturesRelevant)
            {
                LogNet.log.Debug("Default UITools: FindUserFirstCulture : End Time=" + DateTime.Now);
                culs.Sort("Sort");
                Int32 count = culs.Count;
                // Look for master culture first

                foreach (HyperCatalog.Business.Culture cul in culs)
                {
                    if (cul.Type == CultureType.Master)
                    {
                        found = true;
                        SessionState.Culture = cul;
                        break;
                    }
                }
                                if (!found)
                {
                                    // Look for regionale cultures
                    foreach (HyperCatalog.Business.Culture cul in culs)
                    {
                        if (cul.Type == HyperCatalog.Business.CultureType.Regionale)
                        {
                            found = true;
                            SessionState.Culture = cul;
                            break;
                        }
                    }
                  
                }
                if (!found && localeIsOk)
                {
                 
                    // Look for locale cultures
                    foreach (HyperCatalog.Business.Culture cul in culs)
                    {
                        if (cul.Type == HyperCatalog.Business.CultureType.Locale)
                        {
                            found = true;
                            SessionState.Culture = cul;
                            break;
                        }
                    }
                   
                }
            }
        }
        if (!found)
        {
            // User has no primary cultures in its scope
            //UITools.DenyAccess(DenyMode.Standard);
        }
    }
    public static void FindUserFirstCulture(CultureType type)
    {
        bool found = true;
        if (SessionState.Culture == null || SessionState.Culture.Type != type)
        {
            found = false;
            using (CultureList culs = SessionState.User.ItemCulturesRelevant)
            {
                culs.Sort("Name");
                // Look for master culture first
                foreach (HyperCatalog.Business.Culture cul in culs)
                {
                    if (cul.Type == type)
                    {
                        found = true;
                        SessionState.Culture = cul;
                        break;
                    }
                }
            }
            if (!found)
            {
                // User has no cultures of this type in its scope
                UITools.DenyAccess(DenyMode.Standard);
            }
        }
    }

    /*public static uilabels GetLabels()
    {    
     
      if (HttpContext.Current.Cache["labels"]==null)
      {
        uilabels labels = new uilabels();
        labels.ReadXml(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["UILabels"]),XmlReadMode.ReadSchema);
        labels.label.DefaultView.Sort= "master";         
        labels.culture.DefaultView.RowFilter = "code='" + ConfigurationManager.AppSettings["Language"] + "'";
        labels.culture.DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
        HttpContext.Current.Cache["labels"] = labels;
      }
      return (uilabels)HttpContext.Current.Cache["labels"];
    }
    */
    public static string GetTranslation(string s)
    {
        string t = s;
        /*try
        {
          uilabels labels = GetLabels();
          int c = labels.label.DefaultView.Find(s);
          if (c>=0)
          {
            if (labels.label[c].GetChildRows(labels.Relations[0]).Length>0)
            {
              DataRow lRow = labels.label[c].GetChildRows(labels.Relations[0])[0];  
              t = lRow["culture_text"].ToString();
            }
          }
          if (ConfigurationManager.AppSettings["DebugTranslation"].ToString()=="yes")
          {
            t = "[" + t +"]";
          }
      
        }
        catch (Exception ex)
        {
          t = t + "{" + ex +"}";
        }*/
        return t;
    }

    public static void TranslateProperty(string propertyName, System.Web.UI.Control c)
    {
        string controlValue;
        try
        {
            if (c.GetType().GetProperty(propertyName) != null)
            {
                controlValue = c.GetType().GetProperty(propertyName).GetValue(c, null).ToString();
                if (controlValue != string.Empty)
                {
                    if (ConfigurationManager.AppSettings["DebugTranslationShowType"].ToString() == "yes")
                    {
                        controlValue = controlValue + " - " + c.GetType().ToString();
                    }
                    c.GetType().GetProperty(propertyName).SetValue(c, GetTranslation(controlValue), null);
                }
            }
        }
        catch { }
    }

    public static void TranslatePage(System.Web.UI.ControlCollection Controls)
    {
        /*if (ConfigurationManager.AppSettings["Language"] != HyperCatalog.Shared.SessionState.MasterCulture.LanguageCode)
        {
          string translatableTypes = ConfigurationManager.AppSettings["NonTranslatable"].ToString();          
          string controlType;
          for (int i=0;i<Controls.Count;i++)
          {
            controlType = Controls[i].GetType().ToString().ToLower();
            if (translatableTypes.IndexOf(controlType)==-1)
            {            
              TranslateProperty("Text", Controls[i]);
              TranslateProperty("HeaderText", Controls[i]);
            }
            if (Controls[i].Controls.Count>0)
            {
              TranslatePage(Controls[i].Controls);
            }
          }
        }*/
    }
    public static void HideToolBarButton(Infragistics.WebUI.UltraWebToolbar.TBarButton btn)
    {
        //btn.DefaultStyle.CustomRules = "display:none";
        //btn.DefaultStyle.Width = Unit.Pixel(0);
        btn.Visible = false;
    }

    public static void HideToolBarButton(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string btnKey)
    {
        //uwToolbar.Items.FromKeyButton(btnKey).DefaultStyle.CustomRules = "display:none";
        //uwToolbar.Items.FromKeyButton(btnKey).DefaultStyle.Width = Unit.Pixel(0);
        HideToolBarButton(uwToolbar.Items.FromKeyButton(btnKey));
    }

    public static void HideToolBarLabel(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string lblKey)
    {
        uwToolbar.Items.FromKeyLabel(lblKey).DefaultStyle.Width = Unit.Pixel(0);
    }

    public static void HideToolBarCustom(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string lblKey)
    {
        uwToolbar.Items.FromKeyCustom(lblKey).Visible = false;
    }

    public static void HideToolBarSeparator(Infragistics.WebUI.UltraWebToolbar.TBSeparator sep)
    {
        sep.Width = Unit.Pixel(0);
    }
    public static void HideToolBarSeparator(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string sepKey)
    {
        HideToolBarSeparator(uwToolbar.Items.FromKeySeparator(sepKey));
    }

    public static void ShowToolBarButton(Infragistics.WebUI.UltraWebToolbar.TBarButton btn)
    {
        //btn.DefaultStyle.CustomRules = string.Empty;
        //btn.DefaultStyle.Width = btn.ToolBar.ItemWidthDefault;
        btn.Visible = true;
    }
    public static void ShowToolBarButton(Infragistics.WebUI.UltraWebToolbar.TBarButton btn, int width)
    {
        btn.DefaultStyle.Width = Unit.Pixel(width);
    }

    public static void ShowToolBarButton(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string btnKey)
    {
        ShowToolBarButton(uwToolbar.Items.FromKeyButton(btnKey));
    }

    public static void ShowToolBarButton(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string btnKey, int width)
    {
        ShowToolBarButton(uwToolbar.Items.FromKeyButton(btnKey), width);
    }

    public static void ShowToolBarLabel(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string lblKey)
    {
        uwToolbar.Items.FromKeyLabel(lblKey).DefaultStyle.Width = uwToolbar.ItemWidthDefault;
    }

    public static void ShowToolBarCustom(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string lblKey)
    {
        uwToolbar.Items.FromKeyCustom(lblKey).Visible = true;
    }

    public static void ShowToolBarSeparator(Infragistics.WebUI.UltraWebToolbar.TBSeparator sep)
    {
        sep.Width = Unit.Pixel(8);
    }
    public static void ShowToolBarSeparator(Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolbar, string sepKey)
    {
        uwToolbar.Items.FromKeySeparator(sepKey).Width = Unit.Pixel(8);
    }

    public static void HiglightGridRowFilter(ref Infragistics.WebUI.UltraWebGrid.UltraGridRow r, string txtFilter)
    {
        HiglightGridRowFilter(ref r, txtFilter, false);
    }

    public static void HiglightGridRowFilter(ref Infragistics.WebUI.UltraWebGrid.UltraGridRow r, string txtFilter, bool deleteNonMatchingRows)
    {
        string filter = txtFilter.Trim();
        if (filter != string.Empty)
        {
            // Hide sort rows if necessary
            if (r.Cells.FromKey("s_a") != null)
            {
                r.Band.Columns.Remove(r.Band.Columns.FromKey("s_a"));
            }
            bool keep = false;
            foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in r.Cells)
            {
                if ((c.Column.Type == Infragistics.WebUI.UltraWebGrid.ColumnType.NotSet) && (c.Text != null))
                {
                    c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
                    if (c.Text.IndexOf("<font color=red><b>" + filter + "</b></font>") > 0)
                    {
                        keep = true;
                    }
                }
            }
            if (!keep && deleteNonMatchingRows)
            {
                r.Delete();
            }
        }
    }
    public static void HiglightGridRowFilter2(ref Infragistics.WebUI.UltraWebGrid.UltraGridRow r, string txtFilter, bool enableIntelligentSort, int sortColIndex)
    {
        string filter = txtFilter.Trim();
        HiglightGridRowFilter(ref r, filter);
        if (filter == string.Empty && enableIntelligentSort)
        {
            Infragistics.WebUI.UltraWebGrid.UltraWebGrid g = r.Band.Grid;
            Utils.EnableIntelligentSort(ref g, sortColIndex);
        }
    }

    public static void HiglightGridRowFilter(ref Infragistics.WebUI.UltraWebGrid.UltraGridRow r, string txtFilter, params int[] columnIndex)
    {
        string filter = txtFilter.Trim();
        if (filter != string.Empty)
        {
            bool isOk = true;
            foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in r.Cells)
            {
                isOk = true;
                if (columnIndex != null)
                {
                    for (int i = 0; i < columnIndex.Length; i++)
                    {
                        isOk = isOk && (c.Column.Index != columnIndex[i]);
                    }
                }

                if ((c.Column.Type == Infragistics.WebUI.UltraWebGrid.ColumnType.NotSet)
                  && (c.Text != null) && isOk)
                {
                    c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
                }
            }
        }
    }

    public static void RefreshTab(System.Web.UI.Page p, string key, int v)
    {
        p.ClientScript.RegisterClientScriptBlock(p.GetType(), "RefreshTab", "<script>try{parent.ChangeTabText('" + key + "'," + v.ToString() + ");}catch(e){window.status = e.name + ' - ' + e.message;}</script>");
    }

    public static void RefreshTab(System.Web.UI.Page p, string key, string v)
    {
        p.ClientScript.RegisterClientScriptBlock(p.GetType(), "RefreshTab", "<script>try{parent.ChangeTabText('" + key + "'," + v.ToString() + ");}catch(e){}</script>");
    }

    public static void JsCloseWin()
    {
        UITools.JsCloseWin(string.Empty);
    }

    public static void JsCloseWin(string alertMsg)
    {
        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Write("<script language='javascript' src='/hc_v4/js/hypercatalog.js'></script>");
        System.Web.HttpContext.Current.Response.Write("<script>JSCloseWin('" + alertMsg.Replace("'", "\"") + "');</script>");
        //System.Web.HttpContext.Current.Response.End();
    }

    public static void FrameAlertAndBack()
    {
        FrameAlertAndBack(string.Empty);
    }
    public static void FrameAlertAndBack(string alertMsg)
    {
        if (alertMsg != string.Empty)
        {
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + alertMsg + "');</script>");
        }
        //System.Web.HttpContext.Current.Response.Write("<script>history.go(-1);</script>");
        System.Web.HttpContext.Current.Response.End();
    }

    public static void DenyAccess(DenyMode denyInfo)
    {
        switch (denyInfo)
        {
            case DenyMode.Popup:
                {
                    UITools.JsCloseWin("Access denied!");
                    break;
                }
            case DenyMode.Frame:
                {
                    UITools.FrameAlertAndBack("Access denied!");
                    break;
                }
            case DenyMode.Standard:
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/forbidden.aspx", true);
                    break;
                }
        }
    }
    public static string CleanJSString(string s)
    {
        return System.Web.HttpContext.Current.Server.HtmlEncode(s.Replace("\\", "\\\\").Replace(System.Environment.NewLine, "\\r\\n").Replace("'", "\\\'"));
    }
    public static string HtmlEncode(string s)
    {
        HttpServerUtility server = System.Web.HttpContext.Current.Server;
        if (server != null && s != null)
        {
            return server.HtmlEncode(s).Replace(server.HtmlEncode("<font color=red><b>"), "<font color=red><b>").Replace(server.HtmlEncode("</b></font>"), "</b></font>");
        }
        else
        {
            return string.Empty;
        }
    }

    public static int TextHasErrors(ref HyperComponents.WebUI.CustomSpellChecker.SpellChecker c, string v)
    {
        System.Collections.Specialized.StringCollection badWords = new System.Collections.Specialized.StringCollection();
        BadWord badWord = null;
        badWords.Clear();
        //check some text.   
        c.Check(v);
        int nbErrors = 0;
        //iterate through all bad words in the text.   
        while ((badWord = c.NextBadWord()) != null)
        {

            if (badWords.IndexOf(badWord.Word) < 0)
            {
                //Trace.Warn("          -> " + badWord.Word + " - " + badWord.Reason.ToString());
                nbErrors++;
                badWords.Add(badWord.Word);
            }

        }
        return nbErrors;
    }
    public static HyperComponents.WebUI.CustomSpellChecker.SpellChecker GetSpellChecker()
    {
        HyperComponents.WebUI.CustomSpellChecker.SpellChecker c = new HyperComponents.WebUI.CustomSpellChecker.SpellChecker();
        c.SetUserDictionary(SessionState.CacheParams["DictionarySpecificPath"].Value.ToString());
        c.AllowWordsWithDigits = true;
        c.SetIncludeUserDictionaryInSuggestions(true);
        c.AllowXML = true;
        c.SetAllowCapitalizedWords(true);
        c.SetConsiderationRange(80);
        return c;
    }

}
