#region History
//Added for Merging PDB view tab with 9.0 - Mahesh - 10-July-2009.
#endregion History

#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Description résumée de qde_forms.
  /// </summary>
  public partial class qde_forms : HCPage
  {
    #region Declarations
    //protected string itemName;
    protected string itemLevelName;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      QDEUtils.GetItemIdFromRequest();
      QDEUtils.UpdateCultureCodeFromRequest();

      // Initialize vars
      Page.ClientScript.RegisterStartupScript(Page.GetType(), "Initialize", "<script>itemNameId = '" + lbItemName.ClientID + "';</script>");

      if (SessionState.CurrentItem != null)
      {
        pnlTitle.Visible = true;
        if (!Page.IsPostBack)
        {
          using (Item itemRoll = SessionState.CurrentItem.GetRoll())
          {
            if (itemRoll != null || SessionState.CurrentItem.IsRoll)
              pnlTitle.Visible = false;
          }
          ShowTabs();
          Page.DataBind();
        }
      }
      else
      {
        UITools.DenyAccess(DenyMode.Frame);
      }
    }
    private void AddTab(string sText, string sToolTip, string sKey, string sDefaultImage, string sTargetUrl)
    {
      Infragistics.WebUI.UltraWebTab.Tab newTab = new Infragistics.WebUI.UltraWebTab.Tab(sText);
      newTab.Tooltip = sToolTip;
      newTab.Key = sKey;
      newTab.DefaultImage = sDefaultImage;
      newTab.ContentPane.TargetUrl = sTargetUrl;
      webTab.Tabs.Add(newTab);
    }
    private void ShowTabs()
		{
			webTab.Tabs.Clear();
      if (SessionState.CurrentItem != null)
			{
        HyperCatalog.Business.Chunk localizedProductNameChunk = null;
        try
        {
          if (SessionState.Culture.Type != CultureType.Master && ((bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value))
            localizedProductNameChunk = HyperCatalog.Business.Chunk.GetByKey(SessionState.CurrentItem.Id, 1, SessionState.Culture.Code);
          if (localizedProductNameChunk == null)
            lbItemName.Text = SessionState.CurrentItem.FullName;
          else
          {
            if (SessionState.CurrentItem.Sku != null && SessionState.CurrentItem.Sku.Length > 0)
              lbItemName.Text = "[" + SessionState.CurrentItem.Sku + "] - " + localizedProductNameChunk.Text;
            else
              lbItemName.Text = localizedProductNameChunk.Text;
          }
        }
        finally
        {
          if (localizedProductNameChunk != null) localizedProductNameChunk.Dispose();
        }
        if (lbItemName.Text.Length > 50)
				{
          lbItemName.Text = lbItemName.Text.Substring(0, 49) + "...";
				}
        itemLevelName = SessionState.CurrentItem.Level.Name;

				InputForm defaultInputForm = InputForm.GetByKey(0);

				// Add input form tabs
				string tabTitle = string.Empty;
				string tabDesc= string.Empty;
				int nbMandatoryChunks = 0;

        #region "Master or Region or Locale to create country specific"
        if (SessionState.Culture.Type == CultureType.Master 
            || SessionState.Culture.Type == CultureType.Regionale
            || (SessionState.Culture.Type == CultureType.Locale && SessionState.CurrentItem.IsCountrySpecific))
        {
          // Add information tab
          AddTab("Information", "Information for item #" + SessionState.CurrentItem.Id.ToString(), "tb_info", "/hc_v4/img/ed_about.gif", "QDE_ItemAbout.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code);

          // Not other tab if item is for country specific (in master culture)
          if (!(SessionState.CurrentItem.IsCountrySpecific && SessionState.Culture.Type == CultureType.Master))
          {
            InputFormList itemInputForms = SessionState.CurrentItem.GetInputForms(SessionState.Culture.Code);
            if (itemInputForms != null)
            {
              if (itemInputForms.Count > 0)
              {
                foreach (InputForm inputForm in itemInputForms)
                {
                  if (inputForm.ShortName != string.Empty && inputForm.Id != 0)
                  {
                    nbMandatoryChunks = 0;
                    foreach (InputFormContainer ifc in inputForm.Containers)
                    {
                      if (ifc.Mandatory)
                        nbMandatoryChunks++;
                    }
                    tabTitle = inputForm.ShortName + " (" + nbMandatoryChunks.ToString() + "/" + inputForm.Containers.Count.ToString() + ")";
                    tabDesc = inputForm.Description + " (Mandatory/Total)";
                    AddTab(tabTitle, tabDesc, "tb_" + inputForm.Id.ToString(), "/hc_v4/img/ed_inputforms.gif", "QDE_FormContent.aspx?i=" + SessionState.CurrentItem.Id + "&f=IF_" + inputForm.Id.ToString() + "&c=" + SessionState.Culture.Code);
                  }
                }
              }
              tabTitle = "Content";
              tabDesc = "Content attached to this product (Mandatory/Total)";
              AddTab(tabTitle, tabDesc, "tb_all", "/hc_v4/img/ed_content.gif", "QDE_FormContent.aspx?i=" + SessionState.CurrentItem.Id + "&f=IF_-1" + "&c=" + SessionState.Culture.Code);

              // Add link tab
              if (Convert.ToInt32(SessionState.CacheParams["LinksStartLevel"].Value) <= SessionState.CurrentItem.LevelId
                && SessionState.CurrentItem.LevelId <= HyperCatalog.Shared.SessionState.SkuLevel.Id
    && !SessionState.CurrentItem.IsRoll)
              {
                AddTab("Links", "Product compatibilities and links", "tb_lnk", "/hc_v4/img/ed_links.gif", "../links/Links_main.aspx?i=" + SessionState.CurrentItem.Id.ToString() + "&c=" + SessionState.Culture.Code);
              }
            }

            if (SessionState.CurrentItem.LevelId <= HyperCatalog.Shared.SessionState.SkuLevel.Id)
            {
              // Add Delivery tab
              AddTab("Delivery", "Chunks to deliver", "tb_delivery", "/hc_v4/img/ed_delivery.gif", "QDE_Delivery.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code);
              //Added for Merging PDB view tab with 9.0
              AddTab("PDBView", "Data Delivered to PDB", "tb_PDBVIew", "/hc_v4/img/ed_delivery.gif", "QDE_PDBView.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code);
              //Added for Merging PDB view tab with 9.0
            }
            if (SessionState.QDETab != null)
            {
              if (webTab.Tabs.FromKeyTab(SessionState.QDETab) != null)
              {
                webTab.SelectedTabObject = webTab.Tabs.FromKeyTab(SessionState.QDETab);
              }
              else
              {
                webTab.SelectedTabIndex = 0;
                SessionState.QDETab = webTab.Tabs[0].Key;
              }
            }
            else
            {
              webTab.SelectedTabIndex = 0;
              SessionState.QDETab = webTab.Tabs[0].Key;
            }
          }
        }
      }		
      #endregion
    }
  }
}