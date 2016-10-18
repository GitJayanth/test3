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
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Globalize
{
  /// <summary>
  /// Display list of terms in master language
  /// --> filter by TermType or by alphabet
  /// --> search text
  /// --> export in excel
  /// --> Access Settings page
  /// --> button "Add" to create new term (master)
  /// </summary>
  public partial class Termbase : HCPage
  {
    #region declarations

    private CollectionView termTypeView= null;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      this.txtFilter.AutoPostBack = false;
      this.DDL_TermTypeList.SelectedIndexChanged += new System.EventHandler(this.DDL_TermTypeList_SelectedIndexChanged);
      base.OnInit(e);
    }
		
    /// <summary>
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
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
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        uwToolbar.Items.FromKeyButton("Settings").Enabled = false;
      }
      #endregion

      errorMsg.Visible = false;

      if (Request["filter"] != null)
      {
        SessionState.tFilterTerm = txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        #region Load TermType list
        using (TermTypeList termTypes = TermType.GetAll())
        {
          DDL_TermTypeList.Items.Clear();
          DDL_TermTypeList.Items.Add("All");
          foreach (TermType t in termTypes)
          {
            DDL_TermTypeList.Items.Add(new ListItem(t.Name, t.Code.ToString()));
          }
        }
        #endregion

        #region TermType session
        if (SessionState.tTermType != string.Empty)
          DDL_TermTypeList.SelectedValue = SessionState.tTermType;
        #endregion
        #region FilterTerm session
        if (SessionState.tFilterTerm != string.Empty)
          txtFilter.Text = SessionState.tFilterTerm;
        #endregion
        #region Letter session
        if (SessionState.tLetter == string.Empty)
          SessionState.tLetter = "A";
        uwToolbar.Items.FromKeyButton(SessionState.tLetter).Pressed(true);
        #endregion
        #region Validation report date chooser
        Infragistics.WebUI.WebSchedule.WebDateChooser startDate = (Infragistics.WebUI.WebSchedule.WebDateChooser)advancedToolBar.Items.FromKeyCustom("startDate").FindControl("startDate");
        if (startDate != null)
        {
          System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
          ci.DateTimeFormat.ShortDatePattern = SessionState.User.FormatDate;
          ci.DateTimeFormat.LongDatePattern = SessionState.User.FormatDate;
          startDate.CalendarLayout.Culture = ci;
          startDate.Value = DateTime.Today.AddDays(-5);
        }
        #endregion

        ShowTermBase();
      }
      else
      {
        if (Request["redirectId"] != null && Request["redirectId"].ToString() != string.Empty)
        {
          UpdateDataEdit(Request["redirectId"].ToString());
        }
      }
    }

    /// <summary>
    /// Display the list of terms
    /// </summary>
    /// <param name="letter"></param>
    /// <param name="filter"></param>
    private void ShowTermBase()
    {
      string letter = SessionState.tLetter;
      string type = SessionState.tTermType = DDL_TermTypeList.SelectedValue;
      string filter = SessionState.tFilterTerm;
      TermList termbase;
      string sSql = String.Empty;
      #region Definition Filter
      if (filter != string.Empty) // filter on all terms
      {
        string cleanFilter = filter.Replace("'", "''").ToLower();
        cleanFilter = cleanFilter.Replace("[", "[[]");
        cleanFilter = cleanFilter.Replace("_", "[_]");
        cleanFilter = cleanFilter.Replace("%", "[%]");
        sSql += " Lower(TermValue) like '%" + cleanFilter.ToLower() + "%' ";
      }
      if (letter != "All")  // only filter on alphabet's letter selected
      {
        if (sSql != string.Empty) {sSql += " AND ";}
        sSql += " (TermValue like '" + letter.ToUpper() +"%' OR  TermValue like '" + letter.ToLower() + "%') ";
      }
      if (type.ToLower() != "all") // filter on the term's type
      {
        if (sSql != string.Empty) {sSql += " AND ";}
        sSql += " TermTypeCode = '" + DDL_TermTypeList.SelectedValue.Substring(0,1).ToString() + "' ";
      }
      #endregion
      using (termbase = Term.GetAll(sSql))
      {
        lbNoresults.Visible = false;
        if (termbase.Count == 0)
        {
          lbNoresults.Text = ShowNoDataResult(type, letter);
          lbNoresults.Visible = true;
          dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.No;
          dg.Visible = false;
        }
        else
        {
          termTypeView = new CollectionView(TermType.GetAll());
          dg.DataSource = termbase;
          dg.DataBind();
          dg.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime;
          dg.Columns[1].HeaderText = "Terms [" + termbase.Count.ToString() + " item(s)] found";
          dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.Yes;
          Utils.InitGridSort(ref dg);
          dg.Visible = true;
          #region PageIndex session
          if (SessionState.tPageIndexTerm != string.Empty)
          {
            dg.DisplayLayout.Pager.CurrentPageIndex = Convert.ToInt32(SessionState.tPageIndexTerm);
          }
          else
          {
            dg.DisplayLayout.Pager.CurrentPageIndex = 1;
            SessionState.tPageIndexTerm = string.Empty;
          }
          #endregion
        }
      }
    }

    /// <summary>
    /// Display the correct message if there is no record.
    /// </summary>
    /// <param name="_type">term's type selected</param>
    /// <param name="_letter">letter selected</param>
    /// <returns></returns>
    private string ShowNoDataResult(string _type, string _letter)
    {
      string r = string.Empty;
      // all types
      if (_type.ToLower() == "all")
      {
        // all terms
        if (_letter == "All")
        {
          // with filter
          if (txtFilter.Text != string.Empty)
          {
            r = "No record match your search (" + txtFilter.Text + ")";
          }
            // without filter
          else
          {
            r = "No term in database";
          }
        }
          // filter by letter
        else
        {
          r = "No term starting with \""  + _letter + "\"";
        }
      }
      // filter by type
      else
      {
        // all terms
        if (_letter == "All")
        {
          // with filter
          if (txtFilter.Text != string.Empty)
          {
            r = "No record match your search (" + txtFilter.Text + ") for " + _type;
          }
            // without filter
          else
          {
            r = "No " + _type + " terms in database";
          }
        }
          // filter by letter
        else
        {
          r = "No " + _type + " terms starting with \""  + _letter + "\"";
        }
      }
      return r;
    }


    /// <summary>
    /// Action for toolbar buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="be"></param>
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Add new term
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }
      #endregion
      #region Settings
      if (btn == "settings")
      {
        panelGrid.Visible = false;
        webTabSettings.EnableViewState = false;
        webTabSettings.Tabs.GetTab(0).ContentPane.TargetUrl = "./Termbase/Setting_TermTypeCultures.aspx";        
        lbTitle.Text = "Settings";
        panelTabSettings.Visible = true;
      }
      #endregion
      #region Show All
      if (btn == "all")
      {
        if (SessionState.tLetter != string.Empty)
        {
          uwToolbar.Items.FromKeyButton(SessionState.tLetter).Pressed(false);
        }
        be.Button.Pressed(true);
        SessionState.tLetter = "All";
        ShowTermBase();
      }
      #endregion
      #region Select letter in alphabet
      if ((btn == "[0-9]")||(btn == "a")||(btn == "b")||(btn == "c")||(btn == "d")||(btn == "d")||(btn == "e")||(btn == "f")||
        (btn == "g")||(btn == "h")||(btn == "i")||(btn == "j")||(btn == "k")||(btn == "l")||(btn == "m")||(btn == "n")||
        (btn == "o")||(btn == "p")||(btn == "q")||(btn == "r")||(btn == "s")||(btn == "t")||(btn == "u")||(btn == "v")||
        (btn == "w")||(btn == "x")||(btn == "y")||(btn == "z"))
      {
        if (SessionState.tLetter != string.Empty)
        {
          uwToolbar.Items.FromKeyButton(SessionState.tLetter).Pressed(false);
        }
        be.Button.Pressed(true);
        txtFilter.Text = string.Empty;
        SessionState.tFilterTerm = string.Empty;
        SessionState.tLetter = be.Button.Key;
        ShowTermBase();
      }
      #endregion
    }
    protected void advancedToolBar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      advancedToolBar.ClientSideEvents.InitializeToolbar = "";
      uwToolbar.Items.FromKeyButton("Export").Pressed(true);
      switch (be.Button.Key)
      {
        case "TotalExport":
          using (HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["CRYSTAL_DB"].ConnectionString))
          {
            string code = DDL_TermTypeList.SelectedIndex > 0 ? DDL_TermTypeList.SelectedValue.ToString() : string.Empty;
            using (DataSet ds = dbObj.RunSPReturnDataSet("_Term_Export", new System.Data.SqlClient.SqlParameter("@TermTypeCode", code)))
            {
              code = code == string.Empty ? " " : code; // assigning the code to have empty space if it is empty
              using (TermType tt = TermType.GetByKey(Convert.ToChar(code)))  // If the code value is " ", then tt would be null
              {
                code = code != " " ? tt.Name : "All terms";
              }
              HyperCatalog.UI.Tools.Export.ExportTermReport(ds, code, this.Page);
            }
          }
          break;
        case "RunValidationReport":
          using (HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["CRYSTAL_DB"].ConnectionString))
          {
            Infragistics.WebUI.WebSchedule.WebDateChooser startDate = (Infragistics.WebUI.WebSchedule.WebDateChooser)advancedToolBar.Items.FromKeyCustom("startDate").FindControl("startDate");
            if (startDate != null)
              using (DataSet ds = dbObj.RunSPReturnDataSet("_Term_GetChoicesValidationReport", new System.Data.SqlClient.SqlParameter("@DayNew", startDate.Value)))
              {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                  HyperCatalog.UI.Tools.Export.ExportTermValidationReport(ds, (DateTime)startDate.Value, Page);
                else
                {
                  errorMsg.Text = "No activity since this date.";
                  errorMsg.Visible = true;
                }
              }
          }
          break;
        default:
          Response.Write("coucou");
          break;
      }
    }

    /// <summary>
    /// Display termbase datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridCell c;
      // display format for ModifyDate
      c = e.Row.Cells.FromKey("ModifyDate");
      if (c.Text != null)
      {
        c.Text = ((DateTime?)c.Value).Value.ToShortDateString();
      }
      // 

      #region Filter colorization
      string filter = txtFilter.Text.Trim();
      if (filter != string.Empty)
      {
         c = e.Row.Cells.FromKey("Name");
         c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter +"</b></font>", 1);
      }
      #endregion
    }

 
    /// <summary>
    /// Refresh termbase if filter by type
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DDL_TermTypeList_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      ShowTermBase();
    }

    /// <summary>
    /// Link to the ItemId selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
      UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("TermId").Text);
    }

    /// <summary>
    /// Display the selected term properties
    /// </summary>
    /// <param name="selTermId">TermId</param>
    void UpdateDataEdit(string selTermId)
    {
      Session["PageIndexTerm"] = dg.DisplayLayout.Pager.CurrentPageIndex;
      Session["TermType"] = DDL_TermTypeList.SelectedValue;
      Session["FilterTerm"] = txtFilter.Text;
      panelGrid.Visible = false;
      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./Termbase/Term_Properties.aspx?t=" + selTermId;
      
      if (selTermId== "-1")
      {
        #region New Term
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "";
        webTab.Tabs[1].Visible = false;
        webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "";
        webTab.Tabs[2].Visible = false;
        lbTitle.Text = "Term: New";
        #endregion
      }
      else
      {
        #region Term selected
        using (Term HCTerm = Term.GetByKey(Convert.ToInt32(selTermId)))
        {
          string termValue = HCTerm.Value.ToString();
          if (termValue.Length > 50) { termValue = termValue.Substring(0, 50) + "..."; }
          lbTitle.Text = "Term: " + termValue;
          webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./Termbase/Term_Translations.aspx?t=" + selTermId;
          webTab.Tabs.GetTab(2).ContentPane.TargetUrl = "./Termbase/Term_Containers.aspx?t=" + selTermId;
          HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString);
          #region Translations Count
          TermTranslationList tlist;
          int TranslationsCount;
          using (tlist = TermTranslation.GetAll(Convert.ToInt32(selTermId), "TermTypeCode='" + HCTerm.TermTypeCode + "'"))
          {
            TranslationsCount = tlist.Count - HCTerm.TranslationsMissingCount;
          }
          #endregion
          webTab.Tabs.GetTab(1).Text = "Translations (" + TranslationsCount.ToString() + "/" + tlist.Count.ToString() + ")";
          #region Containers Count
          webTab.Tabs.GetTab(2).Visible = false;
          using (ContainerList clist = HyperCatalog.Business.Container.GetAll("LabelId=" + selTermId))
          {
            if (clist.Count > 0)
            {
              webTab.Tabs.GetTab(2).Text = "Containers (" + clist.Count + ")";
              webTab.Tabs.GetTab(2).Visible = true;
            }
          }
          #endregion
        }
        #endregion
      }
      panelTabTerm.Visible = true;
      webTab.SelectedTabIndex = 0;
    }

  }
}
