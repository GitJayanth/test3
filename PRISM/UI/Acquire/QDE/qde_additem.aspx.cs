//===========================================================================
// This file was modified as part of an ASP.NET 2.0 Web project conversion.
// The class name was changed and the class modified to inherit from the abstract base class 
// in file 'App_Code\Migrated\ui\acquire\qde\Stub_qde_additem_aspx_cs.cs'.
// During runtime, this allows other classes in your web application to bind and access 
// the code-behind page using the abstract base class.
// The associated content page 'ui\acquire\qde\qde_additem.aspx' was also modified to refer to the new class name.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================
#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
#endregion
  
#region "History"
// 30/06/2006 All comment is used to check PLC for main component and component list of bundle, but Gemstone cannot integrated this check during the import of product master (Mickael CHARENSOL)
// Aug/06/2099 Balakume - QC 2828  - TrackCount changes Java Script issue 
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Description résumée de QDE_AddProduct.
  /// </summary>
  public partial class Migrated_QDE_AddItem : QDE_AddItem
  {
    #region Declarations
    private System.Int64 parentItemId;
    private HyperCatalog.Business.Item item;
    private HyperCatalog.Business.Item parentItem;
    private int _NbBundleComponents = 0;
    private DataSet dsBundleCheck;
    protected System.Web.UI.WebControls.Repeater rpMarketing;
    #endregion

    #region Constantes
    // Declare hardcoded Link Type for bundles
    private const int BUNDLE_LINKTYPE = 3;
    private const int ROLE_ID_COUNTRY_MANAGER = 6;
    #endregion

    #region Properties
    override public StepEnum Step
    {
      get { return (StepEnum)ViewState["Step"]; }
      set { ViewState["Step"] = value; }
    }
    override public int StepNumber
    {
      get { return (int)ViewState["StepNumber"]; }
      set { ViewState["StepNumber"] = value; }
    }
    override public int NbSteps
    {
      get { return (int)ViewState["NbSteps"]; }
      set { ViewState["NbSteps"] = value; }
    }
    override public bool IsNewSku
    {
      get { return (bool)ViewState["IsNewSku"]; }
      set { ViewState["IsNewSku"] = value; }
    }
    override public int SkuLevel
    {
      get { return (int)ViewState["SkuLevel"]; }
      set { ViewState["SkuLevel"] = value; }
    }
    override public int MainComponentSort
    {
      get { return (int)ViewState["MainComponentSort"]; }
      set { ViewState["MainComponentSort"] = value; }
    }
    override public int NbErrorsInMainComponentPLC
    {
      get { return (int)ViewState["NbErrorsInMainComponentPLC"]; }
      set { ViewState["NbErrorsInMainComponentPLC"] = value; }
    }
    override public int NbFatalErrorsInMainComponentPLC
    {
      get { return (int)ViewState["NbFatalErrorsInMainComponentPLC"]; }
      set { ViewState["NbFatalErrorsInMainComponentPLC"] = value; }
    }
    override public int NbErrorsInComponentsPLC
    {
      get { return (int)ViewState["NbErrorsInComponentsPLC"]; }
      set { ViewState["NbErrorsInComponentsPLC"] = value; }
    }
    override public int NbFatalErrorsInComponentsPLC
    {
      get { return (int)ViewState["NbFatalErrorsInComponentsPLC"]; }
      set { ViewState["NbFatalErrorsInComponentsPLC"] = value; }
    }
    #endregion

    protected override void OnPreRender(EventArgs e)
    {
	    // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
        // Since project will be maintained from Regions, it cannot be added while item creation -->
        /*
          if (!Page.IsPostBack)
          { // Init language scope with languages coming from higher levels
            ProjectInformationControl.InitScopeFromParent(parentItem);
          }
        */
      base.OnPreRender(e);
    }
    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        parentItemId = Convert.ToInt64(Request["i"]);
        if (SessionState.User.HasItemInScope(parentItemId) 
	        && ((SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS) && SessionState.Culture.Type == CultureType.Master)
                || (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanCreateProductInLocalLanguage 
                   && (SessionState.User.RoleId == ROLE_ID_COUNTRY_MANAGER || SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS)))))
        {

         if (HyperCatalog.Business.ApplicationParameter.IOHeirachyStatus() == 0)

        {
          lbError.Visible = false;
          parentItem = HyperCatalog.Business.Item.GetByKey(parentItemId);
          #region Retrieve Item Name and Sku containers
          HyperCatalog.Business.Container containerName = HyperCatalog.Business.Container.GetByKey(1); // ItemName
          HyperCatalog.Business.Container containerSku = HyperCatalog.Business.Container.GetByKey(2); // ItemSku
          if (!IsPostBack)
            uwToolbar.Items.FromKeyLabel("ItemName").Text = buildTitle(null);
          string maxLenItemName = containerName.MaxLength.ToString();
          string maxLenSku = containerSku.MaxLength.ToString();
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "rulename", "<script>strRuleName = '" + containerName.EntryRule.ToString() + "';</script>");
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "rulesku", "<script>strRuleSku = '" + containerSku.EntryRule.ToString() + "';</script>");
          #endregion
          #region Init Text fields
          txtProductName.TextMode = TextBoxMode.SingleLine;
          if (Convert.ToInt32(maxLenItemName) > 0)
          {
            txtProductName.MaxLength = Convert.ToInt32(maxLenItemName);
            //QC 2828 Starts
            //txtProductName.Attributes.Add("onkeyup", "TrackCount(this,'textcount'," + maxLenItemName + ")");
            //txtProductName.Attributes.Add("onfocus", "TrackCount(this,'textcount'," + maxLenItemName + ")");
            txtProductName.Attributes.Add("onkeyup", "TrackCountQDEPage(this,'textcount'," + maxLenItemName + ")");
            txtProductName.Attributes.Add("onfocus", "TrackCountQDEPage(this,'textcount'," + maxLenItemName + ")");
            //QC 2828 Ends
            txtProductName.Attributes.Add("onkeypress", "LimitText(this," + maxLenItemName + ")");
          }
          txtSku.TextMode = TextBoxMode.SingleLine;
          string cleanSKUJs = @"this.value=CleanSku(this.value, normalSku);"; // normalSku is defined in the HTML page
          txtSku.Attributes.Add("onKeyPress", cleanSKUJs);
          txtSku.Attributes.Add("onBlur", cleanSKUJs);
          if (Convert.ToInt32(maxLenSku) > 0)
          {
            txtSku.MaxLength = Convert.ToInt32(maxLenSku);
            //QC 2828 Starts
            //txtSku.Attributes.Add("onKeyUp", cleanSKUJs + "TrackCount(this,'textcountSku'," + maxLenSku + ")");
            //txtSku.Attributes.Add("onFocus", "TrackCount(this,'textcountSku'," + maxLenSku + ")");
            txtSku.Attributes.Add("onKeyUp", cleanSKUJs + "TrackCountQDEPage(this,'textcountSku'," + maxLenSku + ")");
            txtSku.Attributes.Add("onFocus", "TrackCountQDEPage(this,'textcountSku'," + maxLenSku + ")");
            //QC 2828 Ends
            txtSku.Attributes.Add("onKeyPress", cleanSKUJs + "LimitText(this," + maxLenSku + ");");
          }
          #endregion

          if (!Page.IsPostBack)
          {
            Step = StepEnum.MainInfo;
            NbSteps = 3;
            StepNumber = 1;
            SkuLevel = HyperCatalog.Shared.SessionState.SkuLevel.Id;
            MainComponentSort = 0;
            IsNewSku = false;

            // Init Beginning of project with current date, just for information
            ProcessStep();
            #region Populate higher possible levels
            ItemLevelList levels = null;
            if (SessionState.Culture.Type == CultureType.Locale)
            {
              levels = new ItemLevelList();
              using (ItemLevelList allLevels = ItemLevel.GetAll())
              {
                foreach (ItemLevel l in allLevels)
                {
                  if (l.SkuLevel)
                  {
                    levels.Add(l);
                    break;
                  }
                }
              }
            }
            else
              levels = parentItem.Level.HigherPossibleLevels();
            if (levels != null)
            {
              if (levels.Count > 0)
                levels.Sort("Id");
              cbLevels.DataSource = levels;
              cbLevels.DataBind();
              if (cbLevels.Items.Count == 1)
              {
                cbLevels.Enabled = false;
              }
            #endregion
              #region Init project
              // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
              // Since project will be maintained from Regions, it cannot be added while item creation -->
              //ProjectInformationControl.Item = null;
              #endregion
              using (ItemTypeList types = ItemType.GetAll())
              {
                #region Populate ItemTypes combo
                cbItemType.DataSource = types;
                cbItemType.DataBind();
                #endregion
                #region Populate MarketSegments Check Box list
                using (MarketSegmentList ms = HyperCatalog.Business.MarketSegment.GetAll())
                {
                  cblMarketSegments.DataSource = ms;
                  cblMarketSegments.DataBind();
                }

                // Visible or not inherited market segments
                if (SessionState.CurrentItem.LevelId > 1)
                {
                  cbNotInheritedMS.Visible = true;
                  ItemMarketSegmentList imsList = ItemMarketSegment.GetDefinedByParents(parentItem.Id, SessionState.Culture.CountryCode);

                  foreach (ListItem cb in cblMarketSegments.Items)
                  {
                    cb.Attributes["onclick"] = "UpdateMS(\""+cbNotInheritedMS.ClientID+"\")";

                    if (imsList != null)
                    {
                      foreach (ItemMarketSegment ims in imsList)
                      {
                        if (Convert.ToInt32(cb.Value) == ims.MarketSegmentId)
                        {
                          cb.Selected = true;
                          cbNotInheritedMS.Checked = true;
                          break;
                        }
                      }
                    }
                  }
                }
                else
                {
                  cbNotInheritedMS.Visible = false;
                }
                #endregion
                #region Populate Publishers Check Box list
                string filterPublisher = string.Empty;
                if (SessionState.Culture.Type == CultureType.Locale)
                {
                  // it is Country Specific
                  filterPublisher = "ExcludeFromCountries = 0";
                }
                using (PublisherList p = HyperCatalog.Business.Publisher.GetAll(filterPublisher))
                {
                  cblPublishers.DataSource = p;
                  cblPublishers.DataBind();
                }
                #endregion
                #region Popular Product Line Drop Down List
                PLList PLs = SessionState.User.PLs;
                using (CollectionView activePLs = new CollectionView(PLs))
                {
                  activePLs.ApplyFilter("IsActive", true, CollectionView.FilterOperand.Equals);
                  ddlPL.DataSource = activePLs;
                  ddlPL.DataTextField = "Code";
                  ddlPL.DataValueField = "Code";
                  ddlPL.DataBind();
                }
                if (parentItem.ChildProductLines.Count > 0)
                {
                  ddlPL.SelectedValue = parentItem.ChildProductLines[0].Code;
                }
                #endregion

                foreach (ItemLevel l in levels)
                {
                  if (l.SkuLevel)
                  {
                    panelSku.Visible = panelRetailOnly.Visible = cbLevels.SelectedValue == l.Id.ToString();
                    IsNewSku = true;
                    break;
                  }
                }
                panelSku.Visible = panelRetailOnly.Visible = cbLevels.SelectedIndex == cbLevels.Items.Count - 1;
                // For options, display the option box
                panelOption.Visible = !cbLevels.Enabled && cbLevels.Items.Count > 0 && Convert.ToInt32(cbLevels.Items[0].Value) > ItemLevel.GetSkuLevel().Id;
                if (panelOption.Visible)
                {
                  lbSku.Text = parentItem.Sku + " ";
                }
                cbLevels_SelectedIndexChanged(sender, e);

                if (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanCreateProductInLocalLanguage)
                {
                    // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
                    // Since project will be maintained from Regions, it cannot be added while item creation -->
                  //panelProjectYesOrNo.Visible = false;
                  panelMarketSegments.Visible = SessionState.Culture.Country.CanLocalizeMarketSegments;
                  panelPublishers.Visible = SessionState.Culture.Country.CanLocalizePublishers && IsNewSku;
                }
              }
            }
            else
            {
              UITools.JsCloseWin("Error: It is not possible to add a child under level [" + parentItem.Level.Id.ToString() + "]");
            }
          }
        }
         else
         {
          UITools.JsCloseWin("The Product Hierarchy Refresh job is in progress.  User cannot perform this action.  Please try once the job completes!");
        }
        }
        else
        {
          UITools.JsCloseWin("Access Denied!");
        }
      }      
      
      catch (Exception ex)
      {
        UITools.JsCloseWin("Error: " + UITools.CleanJSString(ex.ToString() + " [LevelId =" + parentItem.Level.Id.ToString() + "]"));
      }
    }
    
    protected void Ultrawebtoolbar1_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      switch (be.Button.Key)
      {
        case "Apply":
          {
            //TODO: Mettre le sort correctement en fonction du tri dans la listbox
            CreateItem();
            break;
          }
        #region case "Prev":
        case "Prev":
          {
            if (StepNumber > 1)
              StepNumber--;
            switch (Step)
            {
              case StepEnum.SortInfo:
                if (IsNewSku && Convert.ToInt32(cbItemType.SelectedValue) == (int)ItemTypesEnum.BUNDLE)
                  Step = StepEnum.BundleInfo;
                else
                  Step = StepEnum.MainInfo;
                break;
              case StepEnum.BundleInfo:
                {
                  Step = StepEnum.MainInfo;
                  break;
                }
              case StepEnum.PLCInfo:
                {
                  Step = StepEnum.SortInfo;
                  break;
                }
              case StepEnum.CheckBundlePLC:
                {
                  Step = StepEnum.PLCInfo;
                  break;
                }
              //case StepEnum.Project:
              //  {
              //    if (IsNewSku)
              //    {
              //      //                if  (Convert.ToInt32(cbItemType.SelectedValue) == (int)ItemTypesEnum.BUNDLE)
              //      //                {
              //      //                  Step=StepEnum.CheckBundlePLC;
              //      //                }
              //      //                else
              //      //                {
              //      Step = StepEnum.PLCInfo;
              //      //                }
              //    }
              //    else
              //    {
              //      Step = StepEnum.SortInfo;
              //    }
              //    break;
              //  }
              case StepEnum.Summarize:
                {
                  //if (rdProject.SelectedValue == "Yes")
                  //{
                  //  Step = StepEnum.Project;
                  //}
                  //else
                  //{
                    if (IsNewSku)
                    {
                      //                  if  (Convert.ToInt32(cbItemType.SelectedValue) == (int)ItemTypesEnum.BUNDLE)
                      //                  {
                      //                    Step=StepEnum.CheckBundlePLC;
                      //                  }
                      //                  else
                      //                  {
                      Step = StepEnum.PLCInfo;
                      //                  }
                    }
                    else
                    {
                      Step = StepEnum.SortInfo;
                    }
                  //}
                  break;
                }
            }
            ProcessStep();
            break;
          }
        #endregion
        #region case "Next":
        case "Next":
          {
            if (StepNumber < NbSteps) StepNumber++;
            switch (Step)
            {
              case StepEnum.MainInfo:
                if (IsNewSku && Convert.ToInt32(cbItemType.SelectedValue) == (int)ItemTypesEnum.BUNDLE)
                {
                  Step = StepEnum.BundleInfo;
                  //               NbSteps = 6;
                  NbSteps = 5;
                }
                else
                {
                  Step = StepEnum.SortInfo;
                  if (IsNewSku)
                  {
                    NbSteps = 4;
                  }
                  else
                  {
                    NbSteps = 3;
                  }
                }
                //if (rdProject.SelectedValue == "Yes")
                //  NbSteps++;
              /***** Trimming the ProductName (QC #857) ****************/
                uwToolbar.Items.FromKeyLabel("ItemName").Text = buildTitle((txtProductName.Text+"").Trim());
                break;
              case StepEnum.BundleInfo:
                {
                  Step = StepEnum.SortInfo;
                  break;
                }
              case StepEnum.SortInfo:
                {
                  if (IsNewSku)
                  {
                    Step = StepEnum.PLCInfo;
                  }
                  else
                  {
                    //if (rdProject.SelectedValue == "Yes")
                    //{
                    //  Step = StepEnum.Project;
                    //}
                    //else
                    //{
                      Step = StepEnum.Summarize;
                    //}
                  }
                  break;
                }
              case StepEnum.PLCInfo:
                {
                  switch (PLCInformationControl.ValidatePLC())
                  {
                    case PLCErrorEnum.CorruptedData:
                      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PlcError", "<script>alert(\"The values you've provided can't be saved\\nSee tooltips for more info.\");</script>");
                      StepNumber--;
                      break;
                    case PLCErrorEnum.NoDates:
                    //Decision was taken during business review to allow NO plc during creation
                    //StepNumber--;
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PlcError", "<script>alert('You Must provide at least one valid date');</script>");
                    //break;
                    default:
                      // No Error
                      //                  if (IsNewSku && Convert.ToInt32(cbItemType.SelectedValue) == (int)ItemTypesEnum.BUNDLE)
                      //                  {
                      //                    Step = StepEnum.CheckBundlePLC;
                      //                  }
                      //                  else
                      //                  {
                      //if (rdProject.SelectedValue == "Yes")
                      //{
                      //  Step = StepEnum.Project;
                      //}
                      //else
                      //{
                        Step = StepEnum.Summarize;
                      //}
                      //                  }
                      break;
                  }
                  break;
                }
              case StepEnum.CheckBundlePLC:
                {
                  //if (rdProject.SelectedValue == "Yes")
                  //{
                  //  Step = StepEnum.Project;
                  //}
                  //else
                  //{
                    Step = StepEnum.Summarize;
                  //}
                  break;
                }
              //case StepEnum.Project:
              //  {
              //    //if (ProjectInformationControl.ValidateProjectDates())
              //    //{
              //      Step = StepEnum.Summarize;
              //    //}
              //    //else
              //    //{
              //    //  StepNumber--;
              //    //  Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ProjectError", "<script>alert(\"" + ProjectInformationControl.LastError + "\");</script>");
              //    //  Step = StepEnum.Project;
              //    //}
              //    break;
              //  }
            }
            ProcessStep();
            break;
        #endregion
          }
      }
    }
    protected void cbLevels_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      IsNewSku = panelSku.Visible = panelRetailOnly.Visible = cbLevels.SelectedValue == SkuLevel.ToString();
      UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
      // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
      // Since project will be maintained from Regions, it cannot be added while item creation -->
      // rdProject.Enabled = true;
      if (!panelSku.Visible)
      {
        txtSku.Text = string.Empty;
        cbRetailOnly.Checked = false;
        // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
        // Since project will be maintained from Regions, it cannot be added while item creation -->
        //if (Convert.ToInt32(cbLevels.SelectedValue) > SkuLevel)
        //{
        //  rdProject.Enabled = false;
        //  rdProject.SelectedIndex = 0;
        //}
      }
      else
      {
        PLCInformationControl.LoadPLCGrid();
      }
    }
    private string buildTitle(string newProdName)
    {
      return parentItem.FullName + " - New Child" + (newProdName == null ? "" : ": " + newProdName);
    }
    private void HideControls()
    {
      PanelMainInfo.Visible = PanelSortInfo.Visible = PanelBundle.Visible = PanelPLC.Visible = PanelBundlePLC.Visible = PanelSummary.Visible = false;
      UITools.ShowToolBarButton(Ultrawebtoolbar1, "Apply");
      UITools.ShowToolBarButton(Ultrawebtoolbar1, "Prev");
      UITools.ShowToolBarButton(Ultrawebtoolbar1, "Next");
    }
    private void ProcessStep()
    {
      HideControls();
      switch (Step)
      {
        case StepEnum.MainInfo:
          lbWizardTitle.Text = "Item description";
          PanelMainInfo.Visible = true;
          StepNumber = 1;
          UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
          Ultrawebtoolbar1.Items.FromKeyButton("Prev").DefaultStyle.CustomRules += "visibility:hidden;";
          //UITools.HideToolBarButton(Ultrawebtoolbar1, "Prev");
          break;
        case StepEnum.SortInfo:
          lbWizardTitle.Text = "Sort information";
          PanelSortInfo.Visible = true;
          UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
          SortInfo();
          break;
        case StepEnum.BundleInfo:
          lbWizardTitle.Text = "Bundle components";
          PanelBundle.Visible = true;
          UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
          BundleInfo();
          break;
        case StepEnum.PLCInfo:
          lbWizardTitle.Text = "PLC information";
          PanelPLC.Visible = true;
          PLCInformationControl.Enabled = false;
          UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
          break;
        //        case StepEnum.CheckBundlePLC: 
        //          lbWizardTitle.Text = "PLC Bundle control";
        //          PanelBundlePLC.Visible = true;
        //          UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
        //          CheckBundlePLC();
        //          break;
      // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
      // Since project will be maintained from Regions, it cannot be added while item creation -->
        //case StepEnum.Project:
        //    lbWizardTitle.Text = "Project milestones";
        //    PanelProject.Visible = true;
        //    UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
        //  break;
        case StepEnum.Summarize:
          lbWizardTitle.Text = "Summary";
          PanelSummary.Visible = true;
          UITools.HideToolBarButton(Ultrawebtoolbar1, "Next");
          CreateSummarizeLabel();
          break;
      }
      if (Step != StepEnum.MainInfo)
      {
        lbWizardTitle.Text = lbWizardTitle.Text + " [step " + StepNumber.ToString() + "/" + NbSteps.ToString() + "]";
      }
    }

    #region Sort Item
    private void SortInfo()
    {
      SortInfoControl.Item = parentItem;
      #region Add the Item at the correct save position
      /***** Trimming the ProductName (QC #857) ****************/
      string fullName = (txtProductName.Text+"").Trim();
      if (txtSku.Text != string.Empty)
      {
        fullName = "[" + txtSku.Text + "] " + fullName;
      }
      if (txtOption.Text != string.Empty)
      {
        fullName = "[" + lbSku.Text.Trim() + " " + txtOption.Text  + "] " + fullName;
      }
      SortInfoControl.AddItem(-1, "[" + cbLevels.SelectedValue + "] - " + fullName);
      #endregion
    }

    #endregion
    #region Bundle
    private void BundleInfo()
    {
      #region Fill cbMainComponent
      // Retrieve Sku Level siblinh parentItems which are not bundles
      ItemList siblingItems = parentItem.GetPossibleMainComponents(SessionState.Culture.Code);
      if (siblingItems != null && siblingItems.Count > 0)
      {
        siblingItems.Sort("Sort");
      }
      cbMainComponent.Items.Clear();
      cbMainComponent.Items.Add(new ListItem("<---- Please Select ------>", ""));

      if (siblingItems != null)
      {
        foreach (Item lstItem in siblingItems)
        {
          ListItem newItem = new ListItem("[" + lstItem.LevelId + "] - " + lstItem.FullName, lstItem.Id.ToString());
          if (newItem.Text.Length > 80) newItem.Text = newItem.Text.Substring(0, 49) + "...";
          cbMainComponent.Items.Add(newItem);
        }
      }
      cbMainComponent.SelectedIndex = MainComponentSort;
      if (cbMainComponent.SelectedIndex == 0)
      {
        UITools.HideToolBarButton(Ultrawebtoolbar1, "Next");
      }
      #endregion
    }
    protected void cbMainComponent_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      MainComponentSort = cbMainComponent.SelectedIndex;
      panelBundleComponents.Visible = cbMainComponent.SelectedIndex != 0;
      LbNbComponents.Visible = false;
      dgComponents.Rows.Clear();
      dgComponents.Visible = false;
      txtBundleComponents.Text = string.Empty;
      using (Item mainComponent = Item.GetByKey(Convert.ToInt64(cbMainComponent.SelectedValue))){
        ddlPL.SelectedValue = mainComponent.ProductLineCode;
      }
      UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
    }
    
    protected void BtnAnalyzeComponents_Click(object sender, System.EventArgs e)
    {
      PanelBundle.Visible = true;
      PanelMainInfo.Visible = PanelSortInfo.Visible = PanelPLC.Visible = false;
      UITools.HideToolBarButton(Ultrawebtoolbar1, "Apply");
      using (HyperComponents.Data.dbAccess.Database database = Utils.GetMainDB())
      {
        using (DataSet ds = database.RunSPReturnDataSet("_Item_WizardBundleAnalyze", "",
          new SqlParameter("@BaseSku", txtSku.Text),
          new SqlParameter("@MainComponentSkuId", cbMainComponent.SelectedValue),
          new SqlParameter("@ComponentsSku", txtBundleComponents.Text)
          ))
        {
          database.CloseConnection();
          dgComponents.DataSource = ds.Tables[0];
          _NbBundleComponents = 0;
          txtBundleComponents.Text = string.Empty;
          dgComponents.DataBind();
          dgComponents.Visible = true;
          ds.Dispose();
          bool canProceed = false;
          if (txtBundleComponents.Text != string.Empty)
          {
            txtBundleComponents.Text = txtBundleComponents.Text.Substring(0, txtBundleComponents.Text.Length - 1);
          }
          foreach (UltraGridRow r in dgComponents.Rows)
          {
            if (Convert.ToInt32(r.Cells.FromKey("Error").Value) == 0)
            {
              canProceed = true;
              break;
            }
          }
          panelBundleComponents.Visible = true;
          UITools.HideToolBarButton(Ultrawebtoolbar1, "Next");
          if (canProceed)
          {
            UITools.ShowToolBarButton(Ultrawebtoolbar1, "Next");
          }
          if (_NbBundleComponents > 0)
          {
            LbNbComponents.Text = _NbBundleComponents.ToString() + "/" + dgComponents.Rows.Count.ToString() + " component(s) found. You can proceed";
          }
          else
          {
            LbNbComponents.Text = "The analyze provided 0 possible components. One component at least is required. Please retry or cancel";
          }
          LbNbComponents.Text = "<br>" + LbNbComponents.Text;
          LbNbComponents.Visible = true;
        }
      }
    }
    protected void dgComponents_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      // If the proposed Sku is incorrect, change the cell style
      if (Convert.ToInt32(e.Row.Cells.FromKey("Error").Value) < 0)
      {
        e.Row.Cells.FromKey("ItemName").Style.CssClass = "hc_error";
      }
      switch (Convert.ToInt32(e.Row.Cells.FromKey("Error").Value))
      {
        case ((int)BundleComponentsErrorEnum.BundleItem):
          e.Row.Cells.FromKey("ItemName").Text = "<b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - This Item is already a bundle";
          break;
        case ((int)BundleComponentsErrorEnum.MainComponent):
          e.Row.Cells.FromKey("ItemName").Text = "<b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - This parentItem is the main component";
          break;
        case ((int)BundleComponentsErrorEnum.MissingSku):
          e.Row.Cells.FromKey("ItemName").Text = "<b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - This sku does not exists";
          break;
        default:
          e.Row.Cells.FromKey("ItemName").Text = @"<span class='hc_status" + e.Row.Cells.FromKey("Status").Text + "'>" + e.Row.Cells.FromKey("Status").Text + @"</span>" +
            "<b>[" + e.Row.Cells.FromKey("ItemNumber").Text + "]</b> - " + e.Row.Cells.FromKey("ItemName").Text;
          txtBundleComponents.Text = txtBundleComponents.Text + e.Row.Cells.FromKey("ItemNumber").Text + ",";
          _NbBundleComponents++;
          break;
      }
    }
    #endregion
    #region CheckBundlePLC
    private string GetSqlDateString(DateTime? d)
    {
      if (d.HasValue) return "'" + d.Value.Day + "/" + d.Value.Month + "/" + d.Value.Year + "'";
      return "NULL";
    }
    private void CheckBundlePLC()
    {
      
      System.Text.StringBuilder str_SQL_PLC_Statement = new System.Text.StringBuilder();
      PLCInformationControl.ValidatePLC();
      NbErrorsInMainComponentPLC = NbFatalErrorsInMainComponentPLC = NbFatalErrorsInComponentsPLC = NbErrorsInComponentsPLC = 0;
      foreach (PLC plc in PLCInformationControl.AddUpdatePLCList)
      {
        int excluded = 0;
        if (plc.Excluded) excluded = 1;
        str_SQL_PLC_Statement.Append("INSERT INTO [TEMPTABLE] VALUES(" +
          "'" + plc.CountryCode + "'" +
          ", " + GetSqlDateString(plc.BlindDate) +
          ", " + GetSqlDateString(plc.FullDate) +
          ", " + GetSqlDateString(plc.ObsoleteDate) +
          ", " + GetSqlDateString(plc.AnnouncementDate) +
          ", " + GetSqlDateString(plc.RemovalDate) +
          ", " + excluded.ToString() + ");");
      }
      using (HyperComponents.Data.dbAccess.Database database = Utils.GetMainDB())
      {
        using (dsBundleCheck = database.RunSPReturnDataSet("_Item_WizardBundleCheckPLC", "",
          new SqlParameter("@BaseSku", txtSku.Text),
          new SqlParameter("@MainComponentSkuId", cbMainComponent.SelectedValue),
          new SqlParameter("@BundlePLCStatement", str_SQL_PLC_Statement.ToString()),
          new SqlParameter("@ComponentsSku", txtBundleComponents.Text)
          ))
        {
          database.CloseConnection();
          dgPLCMainReport.DataSource = dsBundleCheck.Tables[0];
          dgPLCMainReport.DataBind();
          dgPLCComponentsReport.DataSource = dsBundleCheck.Tables[1];
          dgPLCComponentsReport.DataBind();
          Utils.InitGridSort(ref dgPLCComponentsReport, false);
          Utils.InitGridSort(ref dgPLCMainReport, false);
          dgPLCMainReport.DisplayLayout.AllowSortingDefault = AllowSorting.No;
          dgPLCComponentsReport.DisplayLayout.AllowSortingDefault = AllowSorting.No;
          lbBundleMainComponent.Text = "Main component (" + dsBundleCheck.Tables[0].Rows[0]["ItemNumber"].ToString() + ") PLC report";
          if (NbErrorsInMainComponentPLC > 0)
          {
            lbBundleMainComponent.Text = lbBundleMainComponent.Text + " [" + NbErrorsInMainComponentPLC.ToString() + " error(s)]";
          }
          lbBundleComponents.Text = "Components report";
          if (NbErrorsInComponentsPLC > 0)
          {
            lbBundleComponents.Text = lbBundleComponents.Text + " [" + NbErrorsInComponentsPLC.ToString() + " error(s)]";
          }
          if (NbErrorsInComponentsPLC > 0 || NbErrorsInMainComponentPLC > 0)
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "WarningPLCCheck", "<script>bundlePLCWarning = true;</script>");
          }
          if (NbFatalErrorsInComponentsPLC > 0 || NbFatalErrorsInMainComponentPLC > 0)
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPLCCheck", "<script>bundlePLCError = true;</script>");
          }
        }
      }
    }
    protected void dgPLCMainReport_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      UltraGridCell countryCell = e.Row.Cells.FromKey("Country");
      UltraGridCell errorCell = e.Row.Cells.FromKey("Error");
      UltraGridCell statusCell = e.Row.Cells.FromKey("Status");
      UltraGridCell descriptionCell = e.Row.Cells.FromKey("Description");
      int errorCode = Convert.ToInt32(errorCell.Value);
      if (errorCode < 0)
      {
        DataRow dr = dsBundleCheck.Tables[0].Rows[e.Row.Index];
        statusCell.Text = "<img src='/hc_v4/img/ed_warning.gif' border=0 valign='middle'/>";
        switch ((PLCBundleErrorEnum)errorCode)
        {
          case PLCBundleErrorEnum.ExcludedInCountry:
            {
              descriptionCell.Text = "This product is excluded in [" + countryCell.Text + "]";
              break;
            }
          case PLCBundleErrorEnum.FullDateAfterBundlePID:
            {
              descriptionCell.Text = "PID date [" + Convert.ToDateTime(dr["MainPID"]).ToString(SessionState.User.FormatDate) + "] > Bundle PID [" + Convert.ToDateTime(dr["PID"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
          case PLCBundleErrorEnum.FullDateAfterBundlePOD:
            {
              descriptionCell.Text = "PID date [" + Convert.ToDateTime(dr["MainPID"]).ToString(SessionState.User.FormatDate) + "] > Bundle POD [" + Convert.ToDateTime(dr["POD"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
          case PLCBundleErrorEnum.NotRelevantInCountry:
            {
              descriptionCell.Text = "This product has no PLC in country [" + countryCell.Text + "]";
              statusCell.Text = "<img src='/hc_v4/img/ed_error.gif' border=0 valign='middle'/>";
              NbFatalErrorsInMainComponentPLC++;
              break;
            }
          case PLCBundleErrorEnum.ObsoleteDateBeforeBundlePID:
            {
              descriptionCell.Text = "POD date [" + Convert.ToDateTime(dr["MainPOD"]).ToString(SessionState.User.FormatDate) + "] < Bundle PID [" + Convert.ToDateTime(dr["PID"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
          case PLCBundleErrorEnum.ObsoleteDateBeforeBundlePOD:
            {
              descriptionCell.Text = "POD date [" + Convert.ToDateTime(dr["MainPOD"]).ToString(SessionState.User.FormatDate) + "] < Bundle POD [" + Convert.ToDateTime(dr["PID"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
        }
        NbErrorsInMainComponentPLC++;
      }
      else
      {
        statusCell.Text = "<img src='/hc_v4/img/ed_ok.gif' border=0 valign='middle'/>";
      }
      countryCell.Text = "<img src='/hc_v4/img/flags/" + e.Row.Cells.FromKey("Country").Text + ".gif' border=0 valign='middle'/>&nbsp;" + countryCell.Text + " - " + e.Row.Cells.FromKey("CountryName").Text;
      countryCell.Style.CustomRules = "class='hc_country'";

    }
    protected void dgPLCComponentsReport_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      UltraGridCell countryCell = e.Row.Cells.FromKey("Country");
      UltraGridCell errorCell = e.Row.Cells.FromKey("Error");
      UltraGridCell statusCell = e.Row.Cells.FromKey("Status");
      UltraGridCell descriptionCell = e.Row.Cells.FromKey("Description");
      int errorCode = Convert.ToInt32(errorCell.Value);
      if (errorCode < 0)
      {
        DataRow dr = dsBundleCheck.Tables[1].Rows[e.Row.Index];
        statusCell.Text = "<img src='/hc_v4/img/ed_warning.gif' border=0 valign='middle'/>";
        switch ((PLCBundleErrorEnum)errorCode)
        {
          case PLCBundleErrorEnum.ExcludedInCountry:
            {
              descriptionCell.Text = "This product is excluded in [" + countryCell.Text + "]";
              break;
            }
          case PLCBundleErrorEnum.FullDateAfterBundlePID:
            {
              descriptionCell.Text = "PID date [" + Convert.ToDateTime(dr["ComponentPID"]).ToString(SessionState.User.FormatDate) + "] > Bundle PID [" + Convert.ToDateTime(dr["PID"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
          case PLCBundleErrorEnum.FullDateAfterBundlePOD:
            {
              descriptionCell.Text = "PID date [" + Convert.ToDateTime(dr["ComponentPID"]).ToString(SessionState.User.FormatDate) + "] > Bundle POD [" + Convert.ToDateTime(dr["POD"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
          case PLCBundleErrorEnum.NotRelevantInCountry:
            {
              descriptionCell.Text = "This product has no PLC in country [" + countryCell.Text + "]";
              statusCell.Text = "<img src='/hc_v4/img/ed_error.gif' border=0 valign='middle'/>";
              NbFatalErrorsInComponentsPLC++;
              break;
            }
          case PLCBundleErrorEnum.ObsoleteDateBeforeBundlePID:
            {
              descriptionCell.Text = "POD date [" + Convert.ToDateTime(dr["ComponentPOD"]).ToString(SessionState.User.FormatDate) + "] < Bundle PID [" + Convert.ToDateTime(dr["PID"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
          case PLCBundleErrorEnum.ObsoleteDateBeforeBundlePOD:
            {
              descriptionCell.Text = "POD date [" + Convert.ToDateTime(dr["ComponentPOD"]).ToString(SessionState.User.FormatDate) + "] < Bundle POD [" + Convert.ToDateTime(dr["POD"]).ToString(SessionState.User.FormatDate) + "]";
              break;
            }
        }
        NbErrorsInComponentsPLC++;
      }
      else
      {
        statusCell.Text = "<img src='/hc_v4/img/ed_ok.gif' border=0 valign='middle'/>";
      }
      countryCell.Text = "<img src='/hc_v4/img/flags/" + e.Row.Cells.FromKey("Country").Text + ".gif' border=0 valign='middle'/>&nbsp;" + countryCell.Text + " - " + e.Row.Cells.FromKey("CountryName").Text;
      countryCell.Style.CustomRules = "class='hc_country'";
    }
    #endregion
    #region Summarize
    void CreateSummarizeLabel()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      if (txtSku.Text != String.Empty)
      {
        sb.Append("[<b>" + txtSku.Text + "</b>] ");
        /***** Trimming the ProductName (QC #857) ****************/
        sb.Append((txtProductName.Text+"").Trim() + " " + cbItemType.SelectedItem.Text + " product");
      }
      else
      {
          /***** Trimming the ProductName (QC #857) ****************/
        sb.Append((txtProductName.Text+"").Trim() + " " + cbLevels.SelectedItem.Text);
      }
      sb.Append(" is now ready to be created");

      // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
      // Since project will be maintained from Regions, it cannot be added while item creation -->
      //if (rdProject.SelectedItem.Value == "Yes")
      //{
      //  sb.Append(" as a project");
      //  if (ProjectInformationControl.Languages.Count > 0)
      //  {
      //    sb.Append(" (" + ProjectInformationControl.Languages.Count.ToString() + " targeted languages)");
      //  }
      //}

      sb.Append(".");
      lbSummary.Text = sb.ToString();
    }
    #endregion
    #region CreateItem
    private void CreateItem()
    {
      string masterCulture = HyperCatalog.Shared.SessionState.MasterCulture.Code;
      //bool isProject = rdProject.SelectedValue == "Yes";
      bool isProject = false;
      string plCode = string.Empty;
      Trace.Warn("Creating new Item - Starts ");
      
      #region Compute Sort
      // The whole computation is make at the very end if no errors have occurred
      SortInfoControl.SaveSort();
      Trace.Warn(" -> Sort = " + SortInfoControl.SelectedIndex.ToString());
      int newItemSort = SortInfoControl.SelectedIndex;
      #endregion
      if (IsNewSku)
      {
        #region Compute PL
        plCode = ddlPL.SelectedValue;
        Trace.Warn(" -> PLCode = " + plCode);
        #endregion
      }

      /***** Trimming the ProductName (QC #857) ****************/
      //Modified by Sateesh for PCF1: Regional Project Management--Moving IsProject,TranslationMode properties to region level-- 18/June/09
      using (Item newItem = new Item(parentItemId, Convert.ToInt32(cbLevels.SelectedValue), cbRetailOnly.Checked, (txtProductName.Text+"").Trim(), txtOption.Text.Trim() != string.Empty ? lbSku.Text.Trim() + " " + txtOption.Text.Trim() : txtSku.Text, newItemSort, ItemStatus.Live, string.Empty, Convert.ToInt32(cbItemType.SelectedValue), HyperCatalog.Shared.SessionState.User.Id, null, false, false, -1, plCode))
      {
        bool savePLC = false;
        bool saveDates = false;
        bool saveLinks = false;

        lbError.Text = string.Empty;
        lbCreatedItem.Visible = true;

        #region Specific country
        if (SessionState.Culture.Type == CultureType.Locale && SessionState.Culture.Country.CanCreateProductInLocalLanguage)
        {
          newItem.CountrySpecificCode = SessionState.Culture.CountryCode;
        }
        #endregion
        #region Attach PLC
        if (IsNewSku)
        {
          PLCInformationControl.ValidatePLC();
          newItem.PLCDates.Clear();
          foreach (PLC plc in PLCInformationControl.AddUpdatePLCList)
          {
            newItem.PLCDates.Add(plc);
          }
          savePLC = true;
        }
        #endregion

        #region Project
        // Modified by Prabhu for ACQ 3.0 (PCF1: Regional Project Management)-- 18/May/09 -->
        // Since project will be maintained from Regions, it cannot be added while item creation -->
        //if (isProject)
        //{
        //  newItem.Milestones.EndOfMasterAcquisition = (DateTime?)ProjectInformationControl.Eoa;
        //  newItem.Milestones.EndOfRegionalValidation = (DateTime?)ProjectInformationControl.Eov;
        //  newItem.Milestones.BeginningOfTranslation = (DateTime?)ProjectInformationControl.Bot;
        //  newItem.Milestones.EndOfTranslation = (DateTime?)ProjectInformationControl.Eot;
        //  newItem.Translations.Clear();
        //  ListItemCollection lg = ProjectInformationControl.Languages;
        //  foreach (ListItem obj in lg)
        //  {
        //    newItem.Translations.Add(HyperCatalog.Business.Language.GetByKey(obj.Value));
        //  }
        //  saveDates = true;
        //}
        #endregion

        #region bundle
        if (IsNewSku && Convert.ToInt32(cbItemType.SelectedValue) == (int)ItemTypesEnum.BUNDLE)
        {
          newItem.RefItemId = Convert.ToInt64(cbMainComponent.SelectedValue);
          double linkSort = 0;
          foreach (UltraGridRow r in dgComponents.Rows)
          {
            if (Convert.ToInt32(r.Cells.FromKey("Error").Value) == 0)
            {
              linkSort += 10;
                //01/31/2012 Links Management: Sorting and Recommending - Removed ItemSort parameter from constructor.
              Link lnk = new Link(-1, Convert.ToInt64(r.Cells.FromKey("ItemId").Value), BUNDLE_LINKTYPE, SessionState.Culture.CountryCode,
                                  linkSort, HyperCatalog.Shared.SessionState.User.Id, DateTime.UtcNow, DateTime.UtcNow);
              newItem.LinksFrom.Add(lnk);
            }
          }
          saveLinks = true;
        }
        #endregion

        Trace.Warn("Creating new Item - Save");
        if (newItem.Save(HyperCatalog.Shared.SessionState.User.Id, true, savePLC, saveLinks, saveDates))
        {
          Trace.Warn("Id of new item: " + newItem.Id);

          #region "Extra JS Code added because of Empty Master Template Added on 02/22/2007"
          string script = string.Empty;
          script += "<input type='hidden' id='aj_id' name='aj_id'/>";
          script += "<script>";
          script += "count = 0;";
          if (IsNewSku)
            script += "maxCount = 3;";
          else
            script += "maxCount = 2;";
          script += "lbMessage = 'tdCreatedItem';";
          script += "message = 'Created item - DONE';";
          script += "document.getElementById(\"aj_id\").value = " + newItem.Id.ToString() + ";";
          script += "</script>";
          Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "initvars", script);
          #endregion

          bool isSavedMS = true;
          bool isSaveP = true;

          #region "Save market segments"
          Trace.Warn("Creating new Item - Save market segments - Enter");
          ItemMarketSegment ims = null;
          int msCount = 0;
          foreach (ListItem cb in cblMarketSegments.Items)
          {
            if (cb.Selected)
            {
              using (ims = new ItemMarketSegment(newItem.Id, SessionState.Culture.CountryCode, Convert.ToInt32(cb.Value), true, SessionState.User.Id))
              {
                if (!ims.Save(SessionState.User.Id))
                {
                  lbError.Text = lbError.Text + " ->" + ItemMarketSegment.LastError + "<br/>";
                  isSavedMS = false;
                  break;
                }
                else
                {
                  msCount++;
                  isSavedMS = true;
                }
              }
            }
          }
          // save -1 if not inherited market segments and none
          if (msCount == 0 && cbNotInheritedMS.Checked && isSavedMS)
          {
            using (ims = new ItemMarketSegment(newItem.Id, SessionState.Culture.CountryCode, -1, true, SessionState.User.Id))
            {
              if (!ims.Save(SessionState.User.Id))
              {
                lbError.Text = lbError.Text + " ->" + ItemMarketSegment.LastError + "<br/>";
                isSavedMS = false;
              }
              else
                isSavedMS = true;
            }
          }
          Trace.Warn("Creating new Item - Save market segments - Exit");
          #endregion
          if (IsNewSku)
          {
            #region "Save publishers"
            Trace.Warn("Creating new Item - Save publishers - Enter");
            foreach (ListItem cb in cblPublishers.Items)
            {
              if (cb.Selected)
              {
                if (!newItem.SavePublisher(Convert.ToInt32(cb.Value), SessionState.Culture.CountryCode, SessionState.User.Id))
                {
                  lbError.Text = lbError.Text + " ->" + Item.LastError + "<br/>";
                  isSaveP = false;
                  break;
                }
                else
                  isSaveP = true;
              }
            }
            Trace.Warn("Creating new Item - Save publishers - Exit");
            #endregion

            Trace.Warn("Creating new Item - Compute status - Enter");
            Item.UpdateAllItemStatuses(newItem.Id);
            if (Item.LastError != null && Item.LastError.Length > 0)
              lbError.Text = lbError.Text + " ->" + Item.LastError + "<br/>";
            Trace.Warn("Creating new Item - Compute status - Exit");

            if (lbError.Text.Length > 0)
            {
              lbCreatedItem.Visible = false;
              lbError.Visible = true;
              return;
            }

            Trace.Warn("Creating new Item - Update publishers - Enter");
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdatePublisherWorkingTable", "RefreshPublishers();", true);
            Trace.Warn("Creating new Item - Update publishers - Exit");
          }

          if (lbError.Text.Length > 0)
          {
            lbCreatedItem.Visible = false;
            lbError.Visible = true;
            return;
          }
          // Modification by ammu for collaterals
          Trace.Warn("Creating new Item - Under Collaterals-Enter");
          HyperComponents.Data.dbAccess.Database database = Utils.GetMainDB();
          SqlParameter[] sqlParam = new SqlParameter[1];
          sqlParam[0] = new SqlParameter("@ItemId", newItem.Id);
          bool success = database.RunSP("ItemSetCompanyFromType", sqlParam);
          if (success == true)
          {
              Trace.Warn("Created new Item - Under Collaterals-Exit");
          }
          Trace.Warn("Creating new Item - Compute item languages - Enter");
          Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateLanguageWorkingTable", "RefreshItemLanguage();", true);
          Trace.Warn("Creating new Item - Compute item languages - Exit");

          Trace.Warn("Creating new Item - Update market segments - Enter");
          Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateMarketWorkingTable", "RefreshMarketSegments();", true);
          Trace.Warn("Creating new Item - Update market segments - Exit");

          Trace.Warn("Creating new Item - Updating User Session");
          SessionState.User.LastVisitedItem = newItem.Id;
          Trace.Warn("Creating new Item - Updating Sort");
          SortInfoControl.Items[SortInfoControl.SelectedIndex].Value = newItem.Id.ToString();
         
        }
        else
        {
          #region "Error"
          if (Item.LastError != string.Empty && lbError.Text == string.Empty)
          {
            lbError.Text = "<center>Item cannot be created<br/> ->" + Item.LastError + "<br/>";
          }
          if (ItemDates.LastError != string.Empty && lbError.Text == string.Empty)
          {
            lbError.Text = "<center>Item cannot be created<br/> ->" + ItemDates.LastError + "<br/>";
          }
          if (Link.LastError != string.Empty && lbError.Text == string.Empty)
          {
            lbError.Text = "<center>Item cannot be created<br/> ->" + Link.LastError + "<br/>";
          }
          if (HyperCatalog.Business.Chunk.LastError != string.Empty && lbError.Text == string.Empty)
          {
            lbError.Text = "<center>Item cannot be created<br/> ->" + HyperCatalog.Business.Chunk.LastError + "<br/>";
          }
          if (PLC.LastError != string.Empty && lbError.Text == string.Empty)
          {
            lbError.Text = "<center>Item cannot be created<br/> ->" + PLC.LastError + "<br/>";
          }
          if (lbError.Text != string.Empty)
            lbError.Text = lbError.Text + "</center>";
          if (panelSku.Visible)
          {
            lbError.Text = lbError.Text + "<br><center>Item cannot be created<br/> -> Sku already exists" + "<br/>";
          }
          lbError.Visible = true;
          lbCreatedItem.Visible = false;
          #endregion
        }
       
      }
   

       }
    #endregion

    protected void cbItemType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cbItemType.SelectedValue == BUNDLE_LINKTYPE.ToString())
      {
        ddlPL.Visible = false;
        rvPL.Enabled = false;
        rvPL.Display = ValidatorDisplay.None;
        rvPL.EnableClientScript = false;
        txtPLInfo.Visible = true;
      }
      else {
        ddlPL.Visible = true;
        rvPL.Enabled = true;
        rvPL.Display = ValidatorDisplay.Dynamic;
        rvPL.EnableClientScript = true;
        txtPLInfo.Visible = false;
      }
    }
}
}