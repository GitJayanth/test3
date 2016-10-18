#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperCatalog.SpellChecker;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Acquire
{
	/// <summary>
	/// Description résumée de SpecificWords.
	/// </summary>
	public partial class SpecificWords : HCPage
	{

    HyperCatalog.WebServices.TranslationWS.WSTranslation ws;		
    #region Code généré par le Concepteur Web Form
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
			//
			InitializeComponent();
      this.txtFilter.AutoPostBack = false;
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
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      #region Check Capabilities
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_SPELL_CHECKER)))
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        uwToolbar.Items.FromKeyButton("Delete").Enabled = false;
        uwToolbar.Items.FromKeyButton("Approve").Enabled = false;
        uwToolbar.Items.FromKeyButton("Build").Enabled = false;
      }
      #endregion      
      lbMessage.Visible = false;
      if (!Page.IsPostBack)
      {
        #region Load languages list 
//        DDL_Cultures.DataSource = HyperCatalog.Business.Culture.GetAll("CultureTypeId<>"+HyperCatalog.Business.Languages.GetAll();
//        DDL_Cultures.DataBind();
        #endregion
        #region FilterWord session
        uwToolbar.Items.FromKeyButton("NotApproved").Selected = SessionState.swNotApproved;
        #endregion
        ViewState["LanguageCode"] = HyperCatalog.Shared.SessionState.MasterCulture.LanguageCode;
        UpdateDataView();
      }
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
    }


    /// <summary>
    /// Display all words by culture
    /// </summary>
    private void UpdateDataView()
    {
      string filter = txtFilter.Text;
      string sSql = " LanguageCode = '" + ViewState["LanguageCode"].ToString() + "'";
      SessionState.swNotApproved = false;
      if (uwToolbar.Items.FromKeyButton("NotApproved").Selected)
      {
        SessionState.swNotApproved = true;
        sSql += " AND Approved = 0";
      }
      #region Definition Search
      if (filter != string.Empty)
      {
        if (sSql != string.Empty) { sSql += " AND "; }
        string cleanFilter = filter.Replace("'", "''").ToLower();
        cleanFilter = cleanFilter.Replace("[", "[[]");
        cleanFilter = cleanFilter.Replace("_", "[_]");
        cleanFilter = cleanFilter.Replace("%", "[%]");
        sSql += " LOWER(Text) like '%" + cleanFilter.ToLower() + "%' ";
      }
      #endregion
      using (SpecificDictionaryWordList wordlist = SpecificDictionaryWord.GetAll(sSql))
      {
        #region No result
        if (wordlist == null)
        {
          lbNoresults.Text = "No record match in " + HyperCatalog.Business.Language.GetByKey(ViewState["LanguageCode"].ToString()).Name;
          lbNoresults.Visible = true;
          UITools.HideToolBarButton(uwToolbar, "Delete");
          UITools.HideToolBarSeparator(uwToolbar, "SepDelete");
          dg.Visible = false;
        }
        else
        {
          if (wordlist.Count == 0)
          {
            lbNoresults.Text = "No record match in " + HyperCatalog.Business.Language.GetByKey(ViewState["LanguageCode"].ToString()).Name;
            lbNoresults.Visible = true;
            UITools.HideToolBarButton(uwToolbar, "Delete");
            UITools.HideToolBarSeparator(uwToolbar, "SepDelete");
            dg.Visible = false;

          }
        #endregion
          #region Results
          else
          {
            dg.DataSource = wordlist;
            lbNoresults.Visible = false;
            dg.Columns[1].HeaderText = "Words [" + wordlist.Count.ToString() + " item(s)]";
            dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.Yes;
            Utils.InitGridSort(ref dg);
            dg.DataBind();
            dg.Columns.FromKey("Submitted").Format = SessionState.User.FormatDate;
            dg.Columns.FromKey("Approved").Format = SessionState.User.FormatDate;
            dg.Visible = true;
            UITools.ShowToolBarButton(uwToolbar, "Delete");
            UITools.ShowToolBarSeparator(uwToolbar, "SepDelete");
            #region PageIndex session
            if (SessionState.swPageIndexWord != string.Empty)
            {
              dg.DisplayLayout.Pager.CurrentPageIndex = Convert.ToInt32(SessionState.swPageIndexWord);
            }
            else
            {
              dg.DisplayLayout.Pager.CurrentPageIndex = 1;
              SessionState.swPageIndexWord = string.Empty;
            }
            #endregion
          }
        }
          #endregion
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
      #region Add new word
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }
      #endregion
      #region Export
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "SpecificWords", "SpecificWords");
      }
      #endregion
      #region Build
      if (btn == "build")
      {
        ws = HyperCatalog.WebServices.WSInterface.Translation;
        //ws.UserSpecificWordsGlossaryCompleted += new HyperCatalog.WebServices.TranslationWS.UserSpecificWordsGlossaryCompletedEventHandler(ws_UserSpecificWordsGlossaryCompleted); 
        string credential = ws.SignOn(SessionState.User.Pseudo, SessionState.User.ClearPassword);
        bool r = ws.UserSpecificWordsGlossary(credential, ViewState["LanguageCode"].ToString());
        if (r)
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Dictionary rebuilt successfully!\\nThe update will be effective in a few minutes\\nas soon as the repliweb job is completed');</script>");
        }
        else
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Dictionary rebuilt failed! [" + UITools.CleanJSString(ws.GetLastError()) + "]');</script>");
        }
        ws.SignOff(credential);
      }
      #endregion
      #region Delete
      if ( btn == "delete")
      {
        DeleteSelectedItems();
      }
      #endregion
      #region Approve
      if ( btn == "approve")
      {
        ApproveSelectedItems();
      }
      #endregion
      #region NotApproved
      if ( btn == "notapproved")
      {
        UpdateDataView();
        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
      }
      #endregion
    }

    void ws_UserSpecificWordsGlossaryCompleted(object sender, HyperCatalog.WebServices.TranslationWS.UserSpecificWordsGlossaryCompletedEventArgs e)
    {
      if (e.Result){
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Dictionary rebuilt successfully!');</script>");
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('Dictionary rebuilt failed! ["+UITools.CleanJSString(ws.GetLastError())+"]');</script>");
      }
    }


    /// <summary>
    /// Format words datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      #region Display format for SubmitDate
      Infragistics.WebUI.UltraWebGrid.UltraGridCell c;
      c = e.Row.Cells.FromKey("Submitted");
      if (c.Value != null && c.Value != DBNull.Value)
      {
        c.Value = HyperCatalog.Shared.SessionState.User.FormatUtcDate(Convert.ToDateTime(c.Value));
      }
      #endregion
      #region Display format for ApproveDate
      c = e.Row.Cells.FromKey("Approved");
      if (c.Value != null && c.Value != DBNull.Value)
      {
        c.Value = HyperCatalog.Shared.SessionState.User.FormatUtcDate(Convert.ToDateTime(c.Value));
      }
      #endregion
      #region Filter colorization
      string search = txtFilter.Text.Trim();
      if (search != string.Empty)
      {
        c = e.Row.Cells.FromKey("Text");
        c.Text = Utils.CReplace(c.Text, search, "<font color=red><b>" + search +"</b></font>", 1);
      }
      #endregion
    }


    /// <summary>
    /// Select the Language
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DDL_Cultures_SelectedIndexChanged(object sender, System.EventArgs e)
    {
//      ViewState["CultureCode"] = DDL_Cultures.SelectedValue;
//      UpdateDataView();
//      dg.DisplayLayout.Pager.CurrentPageIndex = 1;
    }


    /// <summary>
    /// Display the selected word properties
    /// </summary>
    /// <param name="selWord">word</param>
    void UpdateDataEdit(string selWord)
    {
      SessionState.swFilterWord = txtFilter.Text;
      SessionState.swPageIndexWord = dg.DisplayLayout.Pager.CurrentPageIndex.ToString();
      //DDL_Cultures.Enabled = false;
      panelGrid.Visible = false;
      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./SpellChecker/Word_properties.aspx?w=" + selWord + "&l=" + ViewState["LanguageCode"];
      webTab.SelectedTabIndex = 0;
      if (selWord == "-1")
      {
        #region New Word
        lbTitle.Text = "New word";
        #endregion
      }
      else
      {
        #region Word selected
        SpecificDictionaryWord w = SpecificDictionaryWord.GetByKey(selWord, ViewState["LanguageCode"].ToString());
        if (selWord.Length > 50) { selWord = selWord.Substring(0, 50) + "...";}
        lbTitle.Text = "Word: " + selWord;
        #endregion
      }
      panelTabWord.Visible = true;      
    }


    /// <summary>
    /// Delete words selected
    /// </summary>
    private void DeleteSelectedItems()
    {
      lbMessage.Text = string.Empty;
      int nbDeletedRows = 0;
      int nbSelectedRows = 0;
      foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow dr in dg.Rows)
      {
        TemplatedColumn col = (TemplatedColumn)dr.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[dr.Index]).FindControl("g_sd");
        if (cb.Checked)
        {
          nbSelectedRows++;
          if (!SpecificDictionaryWord.DeleteByKey(dr.Cells.FromKey("word").ToString(), ViewState["LanguageCode"].ToString(),SessionState.User.Id))
          {
            lbMessage.Text = "Error: word [" + dr.Cells.FromKey("word").ToString() + "] can't be deleted<br>";
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
            break;
          } 
          else
          {
            nbDeletedRows ++;
          }
        }
      }
      if (nbSelectedRows == 0)
      {
        lbMessage.Text = "No words selected";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
      else
      {
        lbMessage.Text = nbDeletedRows.ToString() + " words deleted";
        lbMessage.CssClass = "hc_success";
        lbMessage.Visible = true;
        UpdateDataView();
        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
      }
    }


    /// <summary>
    /// Approve words selected
    /// </summary>
    private void ApproveSelectedItems()
    {
      lbMessage.Text = string.Empty;
      int nbApprovedRows = 0;
      int nbSelectedRows = 0;
      foreach (UltraGridRow dr in dg.Rows)
      {
        TemplatedColumn col = (TemplatedColumn)dr.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[dr.Index]).FindControl("g_sd");
        if (cb.Checked)
        {
          nbSelectedRows++;
          SpecificDictionaryWord w = SpecificDictionaryWord.GetByKey(dr.Cells.FromKey("word").ToString(), ViewState["LanguageCode"].ToString());
          if (w != null)
          {
            if (!w.Approved)
            {
              w.Approved = true;
              w.ApproveDate = DateTime.UtcNow;
              w.ApproverId = SessionState.User.Id;
              if (!w.Save())
              {
                lbMessage.Text = "Error: word [" + dr.Cells.FromKey("word").ToString() + "] can't be approved<br>";
                lbMessage.CssClass = "hc_error";
                lbMessage.Visible = true;
                break;
              }
              else
              {
                nbApprovedRows ++;
              }
            }
          }
          else
          {
            lbMessage.Text = "Error: word [" + dr.Cells.FromKey("word").ToString() + "] not found<br>";
            lbMessage.CssClass = "hc_error";
            lbMessage.Visible = true;
            break;
          }
        }
      }
      if (nbSelectedRows == 0)
      {
        lbMessage.Text = "No words selected";
        lbMessage.CssClass = "hc_error";
        lbMessage.Visible = true;
      }
      else
      {
        lbMessage.Text = nbApprovedRows.ToString() + " words approved";
        lbMessage.CssClass = "hc_success";
        lbMessage.Visible = true;
        UpdateDataView();
        dg.DisplayLayout.Pager.CurrentPageIndex = 1;
      }
    }


    /// <summary>
    /// Link to the word selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
      UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("word").Text);
    }



 	}
}
