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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Globalize
{
  /// <summary>
  /// Display list of Classes and for each
  ///		--> Set the translation mode
  ///		--> The MTR days
  ///		--> The CTR Days
  ///		--> Modify CTR Language Scope
  ///		--> Each mode is "Product Line", propose for each language the list of PL that apply
  /// </summary>
  public partial class TRSettings : HCPage
	{
  
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
      this.uwClassList.ItemClicked += new Infragistics.WebUI.UltraWebListbar.ItemClickedEventHandler(this.uwClassList_ItemClicked);
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion

    #region published properties
    public enum CurrentItemTypeEnum
    {
      Class=0,
      Term=1
    }

    /// <summary>
    /// Currently browsed item
    /// </summary>
    public string ItemKey
    {
      set{ViewState["ItemKey"]=value;}
      get{return ViewState["ItemKey"]!=null?ViewState["ItemKey"].ToString():string.Empty;}
    }

    /// <summary>
    /// Currently browsed item
    /// </summary>
    public CurrentItemTypeEnum CurrentItemType
    {
      set{ViewState["ItemType"]=value.ToString();}
      get{return ViewState["ItemType"]!=null?(CurrentItemTypeEnum)Enum.Parse(typeof(CurrentItemTypeEnum), ViewState["ItemType"].ToString()):CurrentItemTypeEnum.Class;}
    }
    /// <summary>
    /// Currently browsed item
    /// </summary>
    public string CurrentPL
    {
      set{ViewState["CurrentPL"]=value.ToString();}
      get{return ViewState["CurrentPL"]!=null?ViewState["CurrentPL"].ToString():string.Empty;}
    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
	{
        uwToolbar.Items.FromKeyButton("save").Enabled = btnChangePL.Enabled = 
        rdTranslationMode.Enabled = cbPLLanguages.Enabled = cbMTRDays.Enabled = cbTermLanguages.Enabled =
        cbCTRDays.Enabled = cbCTRLanguages.Enabled = !SessionState.User.IsReadOnly;
        lbError.Text = "";
        if (!Page.IsPostBack)
        {
            UpdateDataView();
        }
        Utils.PreventMultiplePostback(btnChangePL, "onclick");
	}

    private void UpdateDataView()
    {
        #region RegionList population
        UpdateClassRegions();
        UpdateTermRegions();
        #endregion
        #region Init TranslationMode radio group
        rdTranslationMode.Items.Clear();
        rdTranslationMode.DataSource = Enum.GetValues(typeof(TRClassTranslationMode));
        rdTranslationMode.DataBind();
        #endregion
        #region Init ListBar
        //using (TRClassList iList = TRClass.GetAll(" RegionCode = '"+ ddClassRegions.SelectedValue.ToString()+"'"))
        //Added by Venkata 07-10-16
        using (TRClassList iList = GetAllTRClass(" RegionCode = '" + ddClassRegions.SelectedValue.ToString() + "'"))
        {
            foreach (TRClass tr_class in iList)
            {
              uwClassList.Groups.FromKey("classes").Items.Add(tr_class.Item.FullName, tr_class.Item.Id.ToString());
            }
        }
        using (TermTypeList iTypes = TermType.GetAll())
        {
            foreach (TermType termType in iTypes)
            {
              uwClassList.Groups.FromKey("others").Items.Add(termType.Name, termType.Code.ToString());
            }
        }

        uwClassList.SelectedGroup = 0;
        uwClassList.SelectedItem = uwClassList.Groups[0].Items[0];
        #endregion
        #region Update CheckBoxLists
        cbMTRDays.Items.Clear();
        for (int i = 1; i <= 31; i++)
        {
            cbMTRDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        cbCTRDays.Items.Clear();
        for (int i = 1; i <= 31; i++)
        {
            cbCTRDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        #endregion

        #region Update Product Line DropDown List
        using (HyperCatalog.Business.PLList plList = PL.GetAll())
        {
            plList.Sort("Code");
            ddPL.DataSource = plList;
            ddPL.DataBind();
            ddPL.Items.Insert(0, new ListItem("->PL<-", null));
            ddPL.SelectedIndex = 0;
            cbPLLanguages.Enabled = false;
        }
        #endregion

        UpdateOptions(uwClassList.Groups[0].Items[0].Key);
        
        pnl_ClassPanel.Visible = true;
        pnl_TermsPanel.Visible = false;
  }

    //Added by venkata 07-10-16
    public TRClassList GetAllTRClass(string filter)
    {
        using (Database dbObj = Utils.GetMainDB())
        {
            using (IDataReader rs = dbObj.RunSPReturnRS("_TRClass_GetAll",
              new SqlParameter("@Filter", filter),
              new SqlParameter("@Company", SessionState.CompanyName)))
            {
                if (dbObj.LastError != null && dbObj.LastError.Length == 0)
                {
                    HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GenerateCollectionFromReader col = new HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GenerateCollectionFromReader(GenerateTRClassCollectionFromReader);
                    TRClassList objCol = (TRClassList)col(rs);
                    rs.Close();
                    return objCol;
                }
                else
                {
                    throw new DataException("SQLDataAccessLayer: GetAllTRClass-> " + dbObj.LastError);
                }
            }
        }
    }

    private CollectionBase GenerateTRClassCollectionFromReader(IDataReader rs)
    {
        TRClassList col = new TRClassList();
        while (rs.Read())
        {
            //Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009
            TRClass obj = new TRClass(Convert.ToInt64(rs["ClassId"]), rs["RegionCode"].ToString().Trim(), TRClass.GetTranslationModeFromString(rs["TranslationMode"].ToString().Trim()), Convert.ToInt32(rs["ModifierId"]), GetProperDate(rs["ModifyDate"]));
            col.Add(obj);
        }
        return (col);
    }

    public static DateTime? GetProperDate(object d)
    {
        try
        {
            if (d == DBNull.Value)
            {
                return null;
            }
            else
            {
                return new DateTime?((DateTime)d);
            }
        }
        catch
        {
            return null;
        }
    }

    public TRClass GetTRClassByKey(System.Int64 classId, string regionCode)
    {
        TRClass.CleanError();
        using (Database dbObj = Utils.GetMainDB())
        {
            using (IDataReader rs = dbObj.RunSPReturnRS("dbo._TRClass_GetAll",
              new SqlParameter("@Filter", "ClassId = " + classId.ToString() + " AND RegionCode = '" + regionCode + "'"),
              new SqlParameter("@Company", SessionState.CompanyName)))
            {
                if (dbObj.LastError != null && dbObj.LastError.Length == 0)
                {
                    HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GenerateCollectionFromReader col = new HyperCatalog.DataAccessLayer.SqlDataAccessLayer.GenerateCollectionFromReader(GenerateTRClassCollectionFromReader);
                    TRClassList objCol = (TRClassList)col(rs);
                    rs.Close();
                    if (objCol.Count > 0)
                        return objCol[0];
                    else
                        return null;

                }
                else
                {
                    throw new DataException("SQLDataAccessLayer: GetTRClassByKey-> " + dbObj.LastError);
                }
            }
        }
    }
    //end 

    private void UpdateOptions(string itemKey)
    {
      ItemKey = itemKey;
      string regionCode = string.Empty;
      if (Utils.IsWholeNumber(ItemKey))
      {
          regionCode = ddClassRegions.SelectedValue.ToString();
          //QC 2746 -- Modified by Sateesh on 20 July 2009
          GetLanguagesByRegion(regionCode);
          //QC 2746 -- Modified by Sateesh on 20 July 2009
        CurrentItemType = CurrentItemTypeEnum.Class;
        try
        {
            //using (TRClass t = TRClass.GetByKey(Convert.ToInt64(ItemKey), regionCode))
            //Modified by Venkata 07 oct 16
            using (TRClass t = GetTRClassByKey(Convert.ToInt64(ItemKey), regionCode))
            {
                //ddClassRegions.Visible = true;
                //ddTermsRegions.Visible = false;
                pnl_ClassPanel.Visible = true;
                pnl_TermsPanel.Visible = false;
                UpdateClassOptions(t, regionCode);

            }
        }
        catch (Exception ex)
        {
            lbError.Visible = true;
            if (ex.Message.ToString() == "Object reference not set to an instance of an object.")
            {
                lbError.Text = "No TRClass exists for this Class,RegionCode combination.Contact Support.Please verify TRClassSettings table.";
            }
            lbError.Text += "ERROR:" + ex.Message;
            lbError.CssClass = "hc_error";
        }
      }
      else
      {
        CurrentItemType = CurrentItemTypeEnum.Term;
          
          //QC 2746 -- Modified by Sateesh on 20 July 2009
          regionCode = ddTermsRegions.SelectedValue.ToString();
          //QC 2746 -- Modified by Sateesh on 20 July 2009
          GetLanguagesByRegion(regionCode);
        using (TermType tt = TermType.GetByKey(Convert.ToChar(ItemKey)))
        {
            
            pnl_ClassPanel.Visible = false;
            pnl_TermsPanel.Visible = true;
            UpdateTermOptions(tt,regionCode);
        }
      }
    }

      private void UpdateClassRegions()
      {
          //Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009

          #region Region Dropdown initialization
          ddClassRegions.Items.Clear();
          using (HyperCatalog.Business.CultureList culturesList = HyperCatalog.Business.Culture.GetAll("CultureCode IN (SELECT DISTINCT FallbackCode FROM Cultures WHERE CultureTypeId= 2)"))
          {
              culturesList.Sort("Name");
              //ddClassRegions.Items.Add("<--Select a Region-->");
              foreach (HyperCatalog.Business.Culture c in culturesList)
              {
                  ddClassRegions.Items.Add(c.Code);
              }
          }
          ddClassRegions.SelectedIndex = 0;
          GetLanguagesByRegion(ddClassRegions.SelectedValue.ToString());
          #endregion
          //Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009
      }
    private void UpdateClassOptions(TRClass business, string regionCode)
    {
      lbItemName.Text = business.Name;
      rdTranslationMode.SelectedValue = business.TranslationMode.ToString();
      panelCTRSettings.Visible = panelTranslationMode.Visible =  true;
      panelPLSettings.Visible = business.TranslationMode == TRClassTranslationMode.PL;
      panelTermLanguages.Visible = false;
        
      // PL Settings
      if (panelPLSettings.Visible)
      {
        cbPLLanguages.ClearSelection();
        ddPL.SelectedIndex = 0;
        cbPLLanguages.Enabled = false;
      }
      // CTR/MTR Settings
      cbMTRDays.ClearSelection();
      cbCTRDays.ClearSelection();
      foreach (TRBatchDay d in business.BatchDays)
      {
        if (d.Type == TRBatchDayType.MTR)
        {
          cbMTRDays.Items[d.DayOfMonth -1].Selected = true;
        }
        else
        {
          cbCTRDays.Items[d.DayOfMonth -1].Selected = true;
        }
      }
      // CTR Languages
      cbCTRLanguages.ClearSelection();
      foreach (HyperCatalog.Business.Language cul in business.CTRLanguages)
      {
        ListItem li = cbCTRLanguages.Items.FindByValue(cul.Code);
        if (li != null) 
          li.Selected = true;
      }
      // PL Options
      DisplayPLOptions(business, ddPL.SelectedValue);
      //btnChangePL.Attributes.Add("onclick", "ConfirmPLChange('" + rdTranslationMode.SelectedValue + "')");
    }
    //Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009
      private void GetLanguagesByRegion(string regionCode)
      {
          using (LanguageList languagesList = HyperCatalog.Business.Language.GetAll("LanguageCode NOT IN (Select LanguageCode FROM Cultures WHERE CultureTypeId = 0) AND LanguageCode IN (SELECT LanguageCode FROM Cultures(NOLOCK) WHERE FallbackCode ='" + regionCode + "')"))
          {
              languagesList.Sort("Name");
              cbTermLanguages.Items.Clear();
              cbPLLanguages.Items.Clear();
              cbCTRLanguages.Items.Clear();
              cbCTRLanguages.DataSource = cbPLLanguages.DataSource = cbTermLanguages.DataSource = languagesList;
              cbTermLanguages.DataBind();
              cbPLLanguages.DataBind();
              cbCTRLanguages.DataBind();
              //cbTermLanguages.Enabled = cbCTRLanguages.Enabled = false;
          }

      }
    private void DisplayPLOptions(TRClass business,  string plCode)
    {
      CurrentPL = plCode;
      cbPLLanguages.ClearSelection();    
      foreach (TRProductLineLanguage t in business.PLLanguages)
      {
        if (t.ProductLineCode == plCode)
        {
          ListItem li = cbPLLanguages.Items.FindByValue(t.LanguageCode);
          if (li != null)
            li.Selected = true;
        }
      }
    }
      private void UpdateTermRegions()
      {
          //Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009
          #region Region Dropdown initialization
          ddTermsRegions.Items.Clear();
          //ddTermsRegions.Items.Add("<--Select a Region-->");          

          // Modified the cultureList filter as part of CR 5236
          //using (HyperCatalog.Business.CultureList culturesList = HyperCatalog.Business.Culture.GetAll("CultureCode IN (SELECT DISTINCT FallbackCode FROM Cultures(NOLOCK) WHERE CountryCode IN (SELECT CountryCode FROM Countries(NOLOCK) WHERE PLCDrivenTranslation = 0))"))
          using (HyperCatalog.Business.CultureList culturesList = HyperCatalog.Business.Culture.GetAll("CultureCode IN (SELECT DISTINCT FallbackCode FROM Cultures WHERE CultureTypeId= 2)"))
          {
              culturesList.Sort("Name");
              foreach (HyperCatalog.Business.Culture c in culturesList)
              {
                  ddTermsRegions.Items.Add(c.Code);
              }
          }
          ddTermsRegions.SelectedIndex = 0;
          GetLanguagesByRegion(ddTermsRegions.SelectedValue.ToString());
          #endregion
          //Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009

      }
    private void UpdateTermOptions(TermType termType, string regionCode)
    {
      lbItemName.Text = termType.Name;
      panelPLSettings.Visible = panelCTRSettings.Visible = panelTranslationMode.Visible =  false;
      panelTermLanguages.Visible = true;
      
      // MTR Settings
      cbMTRDays.ClearSelection();
      foreach (object d in termType.GetMTRDays(regionCode))
      {
        cbMTRDays.Items[Convert.ToInt32(d)-1].Selected = true;
      }
      // Term Languages
      cbTermLanguages.ClearSelection();
      foreach (HyperCatalog.Business.Language cul in termType.GetLanguages(regionCode))
      {
        ListItem li = cbTermLanguages.Items.FindByValue(cul.Code);
          if (li != null)
            li.Selected = true;        
      }
    }

    private void SaveCurrentItem()
    {
      if (CurrentItemType == CurrentItemTypeEnum.Class)
      {
        SaveCurrentBusiness();
      }
      else
      {
        SaveCurrentTerm();
      }
    }

    private void SaveCurrentBusiness()
    {
        try
        {
            //using (TRClass business = TRClass.GetByKey(Convert.ToInt64(ItemKey), ddClassRegions.SelectedValue.ToString()))
            //Modified by Venkata 07 oct 16
            using (TRClass business = GetTRClassByKey(Convert.ToInt64(ItemKey), ddClassRegions.SelectedValue.ToString()))
            {
                #region update Item if necessary
                if (rdTranslationMode.SelectedValue != business.TranslationMode.ToString())
                {
                    business.TranslationMode = (TRClassTranslationMode)Enum.Parse(typeof(TRClassTranslationMode), rdTranslationMode.SelectedValue);
                    business.Save(HyperCatalog.Shared.SessionState.User.Id, false, false, false);
                }
                #endregion
                #region save BatchDays
                business.BatchDays.Clear();
                foreach (ListItem item in cbCTRDays.Items)
                {
                    if (item.Selected)
                    {
                        business.BatchDays.Add(new TRBatchDay(business.Id, ddClassRegions.SelectedValue.ToString(), Convert.ToInt32(item.Value), TRBatchDayType.CTR));
                    }
                }
                foreach (ListItem item in cbMTRDays.Items)
                {
                    if (item.Selected)
                    {
                        business.BatchDays.Add(new TRBatchDay(business.Id, ddClassRegions.SelectedValue.ToString(), Convert.ToInt32(item.Value), TRBatchDayType.MTR));
                    }
                }
                business.SaveBatchDays();
                #endregion
                #region save CTR Languages
                business.CTRLanguages.Clear();
                foreach (ListItem item in cbCTRLanguages.Items)
                {
                    if (item.Selected)
                    {
                        using (HyperCatalog.Business.Language l = HyperCatalog.Business.Language.GetByKey(item.Value.ToString()))
                        {
                            business.CTRLanguages.Add(l);
                        }
                    }
                }
                business.SaveCTRLanguages();

                //Included for 4179 --Translation settings unique logging issue
                HyperCatalog.Business.ActivityLog.TRActivityLog(business, SessionState.User);


                #endregion
                #region save PL if necessary
                // clear selection for current language
                //for (int i = 0; i < business.PLLanguages.Count; i++)
                //{
                //    if (business.PLLanguages[i].LanguageCode == CurrentPL)
                 //   {
                  //      business.PLLanguages[i].Delete(SessionState.User.Id);
                   //     i--; // ensure couting is always correct
                 //   }
                //}
                if (rdTranslationMode.SelectedIndex == 1)
                {
                    // QC 2028 -> PL Languages in TRSettings not getting saved properly
                    business.PLLanguages.Clear();
                    foreach (ListItem item in cbPLLanguages.Items)
                    {
                        if (item.Selected)
                        {
                            Trace.Warn("Adding PL [" + CurrentPL + "] for id " + business.Id.ToString() + " and value = " + item.Value);
                            business.PLLanguages.Add(new TRProductLineLanguage(CurrentPL, business.Id, ddClassRegions.SelectedValue.ToString(), item.Value));
                        }
                    }
                    business.SaveProductLinesByPL(CurrentPL);

                    //Included for 4179 --Translation settings unique logging issue
                    //	  HyperCatalog.Business.ActivityLog.SaveProductLinesByPL(business, SessionState.User, CurrentPL);

                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            lbError.Visible = true;
            if (ex.Message.ToString() == "Object reference not set to an instance of an object.")
            {
                lbError.Text = "No TRClass exists for this Class,RegionCode combination.Contact Support.Please verify TRClassSettings table.";
            }
            lbError.Text += "ERROR:" + ex.Message;
            lbError.CssClass = "hc_error";
        }
    }

    private void SaveCurrentTerm()
    {
      using (TermType business = TermType.GetByKey(Convert.ToChar(ItemKey)))
      {
            #region save BatchDays
          business.RegionCode = ddTermsRegions.SelectedValue.ToString();
            business.MTRDays.Clear();
            foreach (ListItem item in cbMTRDays.Items)
            {
            if (item.Selected)
            {
              business.MTRDays.Add(Convert.ToInt32(item.Value));
            }
            }
            business.SaveMTRDays();
            #endregion
            #region save MTR Languages
            business.Languages.Clear();
            foreach (ListItem item in cbTermLanguages.Items)
            {
            if (item.Selected)
            {
              using (HyperCatalog.Business.Language l = HyperCatalog.Business.Language.GetByKey(item.Value.ToString()))
              {
                business.Languages.Add(l);
              }
            }
            }
            business.SaveLanguages();

            //Included for 4179 --Translation settings unique logging issue
            	HyperCatalog.Business.ActivityLog.TRActivityLog(business, SessionState.User);
      
            #endregion
           
      }
    }

    private void uwClassList_ItemClicked(object sender, Infragistics.WebUI.UltraWebListbar.WebListbarItemEvent e)
    {
      UpdateOptions(e.Item.Key);    
    }


    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      SaveCurrentItem();
    }

    protected void btnChangePL_Click(object sender, System.EventArgs e)
    {
        try
        {
            //using (TRClass business = TRClass.GetByKey(Convert.ToInt64(ItemKey), ddClassRegions.SelectedValue.ToString()))
            //Modified by Venkata 07 oct 16
            using (TRClass business = GetTRClassByKey(Convert.ToInt64(ItemKey), ddClassRegions.SelectedValue.ToString()))
            {
                business.TranslationMode = (TRClassTranslationMode)Enum.Parse(typeof(TRClassTranslationMode), rdTranslationMode.SelectedValue);
                business.Save(HyperCatalog.Shared.SessionState.User.Id);
                UpdateClassOptions(business, ddClassRegions.SelectedValue.ToString());
            }
        }
        catch (Exception ex)
        {
            lbError.Visible = true;
            if (ex.Message.ToString() == "Object reference not set to an instance of an object.")
            {
                lbError.Text = "No TRClass exists for this Class,RegionCode combination.Contact Support.Please verify TRClassSettings table.";
            }
            lbError.Text += "ERROR:" + ex.Message;
            lbError.CssClass = "hc_error";
        }
    }

    protected void ddPL_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      SaveCurrentItem();

      if (ddPL.SelectedIndex == 0)
      {
        cbPLLanguages.ClearSelection();
        cbPLLanguages.Enabled = false;
      }
      else
      {
        cbPLLanguages.Enabled = true;
        try
        {
            //using (TRClass business = TRClass.GetByKey(Convert.ToInt64(ItemKey), ddClassRegions.SelectedValue.ToString()))
            //Modified by Venkata 07 oct 16
            using (TRClass business = GetTRClassByKey(Convert.ToInt64(ItemKey), ddClassRegions.SelectedValue.ToString()))
            {
                DisplayPLOptions(business, ddPL.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            lbError.Visible = true;
            if (ex.Message.ToString() == "Object reference not set to an instance of an object.")
            {
                lbError.Text = "No TRClass exists for this Class,RegionCode combination.Contact Support.Please verify TRClassSettings table.";
            }
            lbError.Text += "ERROR:" + ex.Message;
            lbError.CssClass = "hc_error";
        }
      }
    }
//Modified by Sateesh for Setting Scope and MTR Dates (PCF: ACQ 8.10) - 28/05/2009
      
      protected void ddClassRegions_SelectedIndexChanged(object sender, System.EventArgs e)
      {
        
          #region Populate Languages Check box list
          
          GetLanguagesByRegion(ddClassRegions.SelectedValue.ToString());
          //cbPLLanguages.Enabled = ddPL.SelectedIndex != 0;
          UpdateOptions(uwClassList.SelectedItem.Key.ToString());
          
          #endregion
      }
      protected void ddTermsRegions_SelectedIndexChanged(object sender, System.EventArgs e) 
      {
          
          #region Populate Languages Check box list
          
          GetLanguagesByRegion(ddTermsRegions.SelectedValue.ToString());
          
          UpdateOptions(uwClassList.SelectedItem.Key.ToString());
          
          #endregion
      }
	}
}
