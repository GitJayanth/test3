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
#endregion

namespace HyperCatalog.UI.Globalize
{
  /// <summary>
  /// Display list of TR Scope
  ///		--> Export in Excel
  ///		--> Filter on TR Scope name
  ///		--> Add a new TR Scope
  ///		--> Modify TR Scope (name or resource)
  ///	Add a new TR Scope or modify TR Scope
  ///		--> Save TR Scope
  ///		--> Delete TR Scope
  ///		--> Return to the list of TR Scopes
  /// </summary>
  public partial class TRScopes : HCPage
  {
    #region Declarations
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      InitializeComponent();
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
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
      this.uwToolBarEdit.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    { 
      if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
      {
        uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        uwToolBarEdit.Items.FromKeyButton("Save").Enabled = false;
        uwToolBarEdit.Items.FromKeyButton("Delete").Enabled = false;
      }

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    
    private void UpdateDataView()
    {
      #region Populate Languages Check box list
        //using (HyperCatalog.Business.LanguageList languageList = HyperCatalog.Business.Language.GetAll("LanguageCode NOT IN (Select LanguageCode FROM Cultures WHERE CultureTypeId = 0) AND LanguageCode NOT IN (Select LanguageCode FROM Cultures L, Countries C WHERE L.CountryCode = C.CountryCode AND L.CultureTypeId = 2 AND C.PLCDrivenTranslation=1)"))
        //{
        //    languageList.Sort("Name");
        //    cblLanguageScope.DataSource = languageList;
        //    cblLanguageScope.DataBind();
        //}
      #endregion

      string sSql = string.Empty;
      string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
        string cleanFilter = filter.Replace("'", "''").ToLower();
        sSql = " LOWER(ScopeName) like '%" + cleanFilter +"%' ";
        sSql += " OR LOWER(ScopeComment) like '%" + cleanFilter +"%' ";
      }
      using (TRScopeList trScopes = TRScope.GetAll(sSql))
      {
        trScopes.Sort("Name");

        dg.DataSource = trScopes;
        Utils.InitGridSort(ref dg);
        dg.DataBind();
        if (trScopes.Count == 0) lbTitle.Text = "No Language Scopes in the System";
        else lbTitle.Text = "Language Scopes list";
      }
      panelGrid.Visible= true;
    }

    private void UpdateDataEdit(string selTRScopeId)
    {
      cblLanguageScope.ClearSelection();
      TRScope trScope = null;
      if (selTRScopeId.Length>0)
        trScope = TRScope.GetByKey(Convert.ToInt32(selTRScopeId));

      if (trScope == null)
      { 
        lbTitle.Text = "TR Scope: New";	
        wneScopeId.Value = "-1";
        PanelRegionDDL.Visible = true;
        PanelRegionReadonly.Visible = false;
        PanelId.Visible = false;

        //RPM_Sateesh
        using (HyperCatalog.Business.CultureList culturesList = HyperCatalog.Business.Culture.GetAll("CultureCode IN (SELECT DISTINCT FallbackCode FROM Cultures(NOLOCK) WHERE CountryCode IN (SELECT CountryCode FROM Countries(NOLOCK) WHERE PLCDrivenTranslation = 0))"))
        {
            culturesList.Sort("Name");
            ddRegions.Items.Add("<-- Select a Region -->");
            foreach (HyperCatalog.Business.Culture c in culturesList)
            {
                ddRegions.Items.Add(c.Code);
            }
                      

        }
        //RPM_Sateesh

        UITools.HideToolBarButton(uwToolBarEdit, "Delete");
        UITools.HideToolBarSeparator(uwToolBarEdit, "DeleteSep");
      }
      else
      {
        lbTitle.Text = "TR Scope: " + trScope.Name;
        wneScopeId.Value = trScope.Id;
        PanelRegionDDL.Visible = false;
        PanelRegionReadonly.Visible = true;
        wteRegionCode.Value = trScope.RegionCode;
        txtName.Text = trScope.Name;
        txtComment.Text = trScope.Comment;
        PanelId.Visible = true;
        //RPM_Sateesh
        //ddRegions.Text = trScope.Name;
        //RPM_Sateesh
        lbTRScopeId.Visible = true;
        wneScopeId.Visible = true;
        wneScopeId.Enabled = false;

        using (HyperCatalog.Business.LanguageList languageList = HyperCatalog.Business.Language.GetAll("LanguageCode NOT IN (Select LanguageCode FROM Cultures WHERE CultureTypeId = 0) AND LanguageCode IN (SELECT LanguageCode FROM Cultures(NOLOCK) WHERE FallbackCode ='" + trScope.RegionCode + "')"))
        {
            languageList.Sort("Name");
            cblLanguageScope.DataSource = languageList;
            cblLanguageScope.DataBind();
        }

        foreach (HyperCatalog.Business.TRScopeLanguage cul in trScope.Languages)
        {
          foreach (ListItem item in cblLanguageScope.Items)
          {
            if (item.Value == cul.LanguageCode)
            {
              item.Selected = true;
            }
          }
        }
      }
      panelEdit.Visible = true;
      panelGrid.Visible = false;
      
    }

    private void Save()
    {
      lbError.Text = string.Empty;
      TRScope trScope = null; 
      if (PanelId.Visible)
      {
        //Update
        trScope = TRScope.GetByKey(wneScopeId.ValueInt);
        trScope.Name = txtName.Text;
        trScope.Comment = txtComment.Text;
      }
      else
      {
        //Add
          if (ddRegions.SelectedValue.ToString() != "<-- Select a Region -->")
          {
              trScope = new TRScope(-1, ddRegions.SelectedValue.ToString(), txtName.Text, txtComment.Text, HyperCatalog.Shared.SessionState.User.Id, DateTime.UtcNow);
          }
          
        UITools.HideToolBarButton(uwToolBarEdit, "Delete");
        UITools.HideToolBarSeparator(uwToolBarEdit, "DeleteSep");
      }
      if (trScope != null)
      {
        trScope.Languages.Clear();
        foreach (ListItem obj in cblLanguageScope.Items)
        {
          if (obj.Selected)
          {
            trScope.Languages.Add(new TRScopeLanguage(trScope.Id, obj.Value));
          }
        }
        if (trScope.Languages.Count == 0)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Error: you must choose at least ONE language";
          lbError.Visible = true;
        }
        else
        {
          if (trScope.Save(HyperCatalog.Shared.SessionState.User.Id))
          {
            wneScopeId.Value = trScope.Id;
            wneScopeId.Text = trScope.Id.ToString();
            PanelId.Visible = true;
            PanelRegionDDL.Visible = false;
            PanelRegionReadonly.Visible = true;
            wteRegionCode.Value = trScope.RegionCode;
            wteRegionCode.Text = trScope.RegionCode.ToString();
            UITools.ShowToolBarButton(uwToolBarEdit, "Delete");
            UITools.ShowToolBarSeparator(uwToolBarEdit, "DeleteSep");
            lbError.Text = "Data saved!";
            lbError.CssClass = "hc_success";
            lbError.Visible = true;
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = "Error: " + TRScope.LastError;
            lbError.Visible = true;
          }
        }
      }
      else
      {
          if (ddRegions.SelectedValue.ToString() == "<-- Select a Region -->")
          {
              lbError.CssClass = "hc_error";
              lbError.Text = "Error: Scope could not be created, please make sure that the u Selected a Region.";
              lbError.Visible = true;
          }
          else
          {
              lbError.CssClass = "hc_error";
              lbError.Text = "Error: Scope could not be created, please make sure that the name is not already in use.";
              lbError.Visible = true;
          }
      }
    }

    private void Delete()
    {
      TRScope trScope = TRScope.GetByKey(wneScopeId.ValueInt);

      if (trScope != null)
      {
        if (trScope.Delete(HyperCatalog.Shared.SessionState.User.Id))
        {
          lbError.Visible = false;
          lbError.Text = string.Empty;
					
          panelEdit.Visible = false;
          panelGrid.Visible = true;
					
          UpdateDataView();
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Error: " + TRScope.LastError;
          lbError.Visible = true;
        }   
      }
      else
      { // Already deleted
        UpdateDataView();      
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }

    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
      string sId = cellItem.Cell.Row.Cells.FromKey("Id").Text;
			
      sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
      sId = Utils.CReplace(sId, "</b></font>", "", 1);
			
      UpdateDataEdit(sId);
    }
      //RPM_Sateesh
      protected void ddRegions_SelectedIndexChanged(object sender, System.EventArgs e)
      {
          #region Populate Languages Check box list
          using (HyperCatalog.Business.LanguageList languageList = HyperCatalog.Business.Language.GetAll("LanguageCode NOT IN (Select LanguageCode FROM Cultures WHERE CultureTypeId = 0) AND LanguageCode IN (SELECT LanguageCode FROM Cultures(NOLOCK) WHERE FallbackCode ='"+ddRegions.SelectedValue.ToString()+"')"))
          {
              languageList.Sort("Name");
              cblLanguageScope.DataSource = languageList;
              cblLanguageScope.DataBind();
          }
          #endregion
          
      }
      //RPM_Sateesh
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        UpdateDataView();
        try
        {
            Utils.ExportToExcel(dg, "TRScopes", "TRScopes");
        }
        catch (Exception ex)
        {
            lbError.Text = ex.Message + ";" + ex.InnerException;
        }
      }
      else if (btn == "add")
      {
        UpdateDataEdit(string.Empty);
      }
      else if (btn == "save")
      {
        Save();
      }
      else if (btn == "delete")
      {
        Delete();
      }
    }
  }
}