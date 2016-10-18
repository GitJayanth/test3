#region uses
using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

/// <summary>
/// Description résumée de QDE_Toolbar.
/// </summary>
public partial class QDE_Toolbar : HCPage
{
  #region Declarations
  private CultureType cultureType = CultureType.Master;
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

  }
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    // Retrieve the current culture type
    cultureType = SessionState.Culture.Type;
    if (Request["g"] != null)
      cultureType = (CultureType)Convert.ToInt32(Request["g"]);
    else
      cultureType = SessionState.Culture.Type;

    if (!Page.IsPostBack)
    {
      UpdateDataView();
      UITools.UpdateTitle(this.Page);
    }
  }

  protected void UpdateDataView()
  {
    DDL_Cultures.Visible = false;

    // retrieve all cultures for given user
    HyperCatalog.Business.CultureList cultureList = SessionState.User.ItemCulturesRelevant;

    // remove other culture type
    HyperCatalog.Business.CultureList cultures = new HyperCatalog.Business.CultureList();
    bool masterOrRegion = (cultureType == CultureType.Master || cultureType == CultureType.Regionale);

    foreach (HyperCatalog.Business.Culture c in cultureList)
    {
      if (masterOrRegion)
      {
        if (c.Type == CultureType.Master || c.Type == CultureType.Regionale)
          cultures.Add(c);
      }
      else
      {
        if (c.Type == CultureType.Locale)
          cultures.Add(c);
      }
    }

    if (cultures != null)
    {
      if (cultures.Count == 1)
      {
        // cultures contains only one culture
        DDL_Cultures.Visible = false;
      }
      else
      {
        if (SessionState.Culture.Type != cultureType)
          SessionState.Culture = cultures[0];

        CollectionView cv = new CollectionView(cultures);
        cv.Sort("Name");
        DDL_Cultures.DataSource = cv;
        DDL_Cultures.DataBind();

        DDL_Cultures.SelectedValue = SessionState.Culture.Code;
        DDL_Cultures.Visible = true;
        cv.Dispose();
      }
    }
  }

  protected void DDL_Cultures_SelectedIndexChanged(object sender, System.EventArgs e)
  {
    string cultureCode = DDL_Cultures.SelectedValue.ToString();
    SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(cultureCode);
    UITools.UpdateTitle(this.Page);
    UITools.UpdateSearchBar(this.Page);
    #region Eligibility
    // Check if current item is eligible
    // if not, select the first eligible item
    ItemStatus status = ItemStatus.Unknown;
    bool isEligible = false;
      
    //Fix for 2629 by Sateesh :11/08/2009 : Left pane - right pane inconsistency --PCF1: U,I,A statuses should not be visible in country catalog
    if (SessionState.Culture.Type == CultureType.Locale &&
        (Item.GetByKey(SessionState.User.LastVisitedItem).GetWorkflowStatus(SessionState.Culture.Code) == ItemWorkflowStatus.Initialized.ToString()
                 || Item.GetByKey(SessionState.User.LastVisitedItem).GetWorkflowStatus(SessionState.Culture.Code) == ItemWorkflowStatus.AcquisitionCompleted.ToString()
                 || Item.GetByKey(SessionState.User.LastVisitedItem).GetWorkflowStatus(SessionState.Culture.Code) == ItemWorkflowStatus.Unknown.ToString()) )
        isEligible = false;

    else isEligible = Item.IsEligible(SessionState.User.LastVisitedItem, SessionState.Culture.CountryCode, ref status);
    //Response.Write("Current IS " + SessionState.User.LastVisitedItem.ToString());
    //Response.Write(" - ELIGIBLE = " + isEligible.ToString() + "<br/>");
    if (isEligible)
    {
      if (status == ItemStatus.Obsolete && !SessionState.User.ViewObsoletes)
      {
        SessionState.User.ViewObsoletes = true;
        SessionState.User.QuickSave();
      }
    }
    else
    { // Find the first eligible Item for the user.
        //  08/12/2009 QC 2692 - Modified by Sateesh -- The workflow Status 'R'/'C' only should be  visible in locales
        SessionState.User.LastVisitedItem = SessionState.User.GetFirstCountryItem(SessionState.Culture.CountryCode, SessionState.Culture.Code);
      //Response.Write("CHANGED TO " + SessionState.User.LastVisitedItem.ToString());
      Item.IsEligible(SessionState.User.LastVisitedItem, SessionState.Culture.CountryCode, ref status);
      if (status == ItemStatus.Obsolete && !SessionState.User.ViewObsoletes)
      {
        SessionState.User.ViewObsoletes = true;
      }
      SessionState.User.QuickSave();
    }
    #endregion
    Page.ClientScript.RegisterStartupScript(this.GetType(), "reload", "<script>UpdateFrames('" + cultureCode + "');</script>");
  }

}



