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
  /// Display list of TM expressions in master language
  /// --> search text
  /// --> export in excel
  /// --> Add a new TM Expression
  /// </summary>
  public partial class TM : HCPage
  {
    #region Declarations
    protected System.Web.UI.WebControls.DropDownList DDL_TermTypeList;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      this.txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");        Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "txtFilterField = document.getElementById('" + txtFilter.ClientID + "');", true);       
      cbMatch.Attributes.Add("onClick", "WholeWord(this);");
      base.OnInit(e);
    }
		
    /// <summary>
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion
    
    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Check Capabilities
      if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_MEMORY)))
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
      }
      #endregion
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (Request["ww"] != null)
      {
        cbMatch.Checked = Request["ww"].ToString()=="1";
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }

    /// <summary>
    /// Display TM expressions
    /// </summary>
    private void UpdateDataView()
    {
      string search = txtFilter.Text;
      TMExpressionsList _tm;
      string sSql = String.Empty;
      #region Definition Search
      if (search != string.Empty) // Search on all expressions
      {
        string cleanSearch = search.Replace("'", "''").ToLower();
        cleanSearch = search.Replace("[", "[[]");
        cleanSearch = search.Replace("_", "[_]");
        cleanSearch = search.Replace("%", "[%]");
        if (cbMatch.Checked)
        {
          sSql += " Lower(TMExpressionValue) = '" + cleanSearch.ToLower() + "' ";
        }
        else
        {
          sSql += " Lower(TMExpressionValue) like '%" + cleanSearch.ToLower() + "%' ";
        }
      }
      #endregion
      using (HyperComponents.Data.dbAccess.Database dbObj = new HyperComponents.Data.dbAccess.Database(SessionState.CacheComponents["CRYSTAL_DB"].ConnectionString))
      {
        using (System.Data.SqlClient.SqlDataReader rs = dbObj.RunSPReturnRS("_TM_GetAll", 
          new System.Data.SqlClient.SqlParameter("@Filter", sSql),
            new System.Data.SqlClient.SqlParameter("@Mode", 1),
          new System.Data.SqlClient.SqlParameter("@Company", SessionState.CompanyName)))
        {
          #region No result
          rs.Read();
          int c = Convert.ToInt32(rs["ResultCount"]);
          if (c == 0)
          {
            lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";
            lbNoresults.Visible = lbMessage.Visible = true;
            dg.Visible = false;
          }
          #endregion
          #region Results
          else
          {
            #region Too much results
            if (c > Convert.ToInt32(SessionState.CacheParams["TMMaxRows"].Value))
            {
              lbNoresults.Text = "There are " + c.ToString() + " expressions found over " + SessionState.CacheParams["TMMaxRows"].Value + ", please refine your search.";
              lbNoresults.Visible = lbMessage.Visible = true;
              dg.Visible = false;
            }
            #endregion
            #region Display results
            else
            {
              using (_tm = TMExpression.GetAll(sSql))
              {
                dg.DataSource = _tm;
                dg.Columns[1].HeaderText = "TM [" + _tm.Count.ToString() + " item(s) found]";
                dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.Yes;
                lbNoresults.Visible = lbMessage.Visible = false;
                Utils.InitGridSort(ref dg);
                dg.DataBind();
                dg.Visible = true;
                #region PageIndex session
                if (SessionState.tmPageIndexExpression != string.Empty)
                {
                  dg.DisplayLayout.Pager.CurrentPageIndex = Convert.ToInt32(SessionState.tmPageIndexExpression);
                }
                else
                {
                  dg.DisplayLayout.Pager.CurrentPageIndex = 1;
                  SessionState.tmPageIndexExpression = string.Empty;
                }
                #endregion
              }
            }
            #endregion
          }
          dg.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate + ' ' + SessionState.User.FormatTime;
          rs.Close();
          #endregion
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
      #region Add new TM Expression
      if (btn == "add")
      {
        UpdateDataEdit("-1");
      }
      #endregion
      #region Export
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "TMExpressions", "TMExpressions");
      }
      #endregion
    }

    /// <summary>
    /// Display TM datagrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      /// display format for ModifyDate
      Infragistics.WebUI.UltraWebGrid.UltraGridCell c = e.Row.Cells.FromKey("ModifyDate");
      if (c.Text != null)
      {
        c.Text = ((DateTime)c.Value).ToShortDateString();
      }
      #region Search colorization
      string search = txtFilter.Text.Trim();
      if (search != string.Empty)
      {
        c = e.Row.Cells.FromKey("TMExpressionValue");
        c.Text = Utils.CReplace(c.Text, search, "<font color=red><b>" + search +"</b></font>", 1);
      }
      #endregion
    }

    /// <summary>
    /// Link to the ItemId selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
      UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("TMExpressionId").Text);
    }

    /// <summary>
    /// Display the selected expression properties
    /// </summary>
    /// <param name="selTermId">ExpressionId</param>
    void UpdateDataEdit(string selExpressionId)
    {
      SessionState.tmPageIndexExpression = dg.DisplayLayout.Pager.CurrentPageIndex.ToString();
      panelgrid.Visible = false;
      webTab.EnableViewState = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./TM/Expression_Properties.aspx?e=" + selExpressionId;
      if (selExpressionId == "-1")
      {
        #region New TM Expression
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "";
        webTab.Tabs[1].Visible = false;
        lbTitle.Text = "Expression: New";
        #endregion
      }
      else
      {
        #region TM Expression selected
        using (TMExpression exp = TMExpression.GetByKey(Convert.ToInt32(selExpressionId)))
        {
          string expValue = exp.Value.ToString();
          if (expValue.Length > 50) { expValue = expValue.Substring(0, 50) + "..."; }
          lbTitle.Text = "Expression: " + expValue;
          webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./TM/Expression_Translations.aspx?e=" + selExpressionId;
          #region Translation Count
          using (HyperCatalog.Business.LanguageList c = HyperCatalog.Business.Language.GetAll())
          {
            int LanguagesCount = c.Count;
            int TranslationsCount = LanguagesCount - exp.TranslationsMissingCount;
            webTab.Tabs.GetTab(1).Text = "Translations (" + TranslationsCount.ToString() + "/" + LanguagesCount.ToString() + ")";
          }
          #endregion
        #endregion
        }
      }
      webTab.Visible = true;      
    }

  
  }
}
