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
using HyperCatalog.Shared;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebTab;
#endregion

#region Historic
// Update count of possible values from parent item (Mickael CHARENSOL 10/01/06)
#endregion

// <summary>
// Description résumée de Item.
// </summary>
public partial class ChunkWindow : HCPage
{
  protected HyperCatalog.Business.Container container = null;
  protected void Page_Load(object sender, System.EventArgs e)
  {
    string cultureCode = string.Empty;
    int containerId = -1;
    long itemId = -1;
    bool isMandatory = false;
    int inputformId = -1;
    string extraParam = string.Empty;
    bool isRegion = false;
    try
    {
      #region try
      itemId = QDEUtils.GetQueryItemIdFromRequest();
      containerId = Convert.ToInt32(Request["d"]);
      if (Request["m"] != null)
        isMandatory = Convert.ToBoolean(Request["m"]);
      using (Culture curCulture = QDEUtils.UpdateCultureCodeFromRequest())
      {
        cultureCode = curCulture.Code;
        if (Request["ifcid"] != null && Request["ifcid"].ToString()!="-1")
        {
          using (InputFormContainer ifc = InputFormContainer.GetByKey(Convert.ToInt32(Request["ifcid"])))
          {
            if (ifc != null)
            {
              inputformId = ifc.InputFormId;
            }
          }
        }
        bool simpleView = SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_SIMPLIFIED_CHUNK_WINDOW).Value;
        Tab
          tabChunk = webTab.Tabs.FromKeyTab("Chunk"),
          tabMaster = webTab.Tabs.FromKeyTab("Master"),
          tabContainer = webTab.Tabs.FromKeyTab("Container"),
          tabHistory = webTab.Tabs.FromKeyTab("History"),
          tabRegionalization = webTab.Tabs.FromKeyTab("Regionalization"),
          tabTranslation = webTab.Tabs.FromKeyTab("Translation"),
          tabPossibleValues = webTab.Tabs.FromKeyTab("PossibleValues"),
          tabInheritance = webTab.Tabs.FromKeyTab("Inheritance"),
          tabImage = webTab.Tabs.FromKeyTab("Image");
        if (curCulture.CountryCode != string.Empty)
        {
          uwToolbar.Items.FromKeyLabel("Culture").Image = "/hc_v4/img/flags/" + curCulture.CountryCode + ".gif";
        }
        GetChunk(itemId, containerId, cultureCode);
        uwToolbar.Items.FromKeyLabel("Culture").Text = curCulture.Name;
        container = SessionState.QDEContainer;
        if (container == null)
        {
          throw new Exception("An error occurred when retrieving data. Container is null[id = " + containerId.ToString() + "]");
        }
        // *************************************************************************
        // Only show master tab if culture code is not master
        // *************************************************************************        
        if ((curCulture.Type != CultureType.Master))
        {
          tabMaster.ContentPane.TargetUrl = tabMaster.ContentPane.TargetUrl + "?d=" + containerId.ToString() + "&i=" + itemId.ToString() + "&c=" + curCulture.Code;
          using (Chunk masterChunk = Chunk.GetByKey(itemId, containerId, curCulture.FallbackCode))
          {
            tabMaster.Visible = masterChunk != null;
            if (masterChunk != null && !masterChunk.Text.Equals(HyperCatalog.Business.Chunk.BlankValue))
            {
              extraParam = "&hf=1"; // Has Fallback = 1. This will be used in the Chunk button bar to accelerate the code.
            }
          }
        }

        // Change Tab height according to hidden field (to avoid recompiling)
        webTab.Height = Unit.Pixel(Convert.ToInt16(chunkHeight.Value));
        // *************************************************************************
        // Determine what we are currently doing: New or Update
        // *************************************************************************
        // SHOW/HIdE Possible values tab
        using (Item item = QDEUtils.GetItemIdFromRequest())
        {
          if (!simpleView && container != null && curCulture.Type == CultureType.Master)
          {
            // Get count of possible values from parent
            //int possibleValueCount = container.PossibleValuesCount(cultureCode, item.ParentId, inputformId);
            tabPossibleValues.Visible = true;
            //(phil - performances)tabPossibleValues.Text = tabPossibleValues.Text + " (" + possibleValueCount + ")";
            tabPossibleValues.ContentPane.TargetUrl = tabPossibleValues.ContentPane.TargetUrl + "?d=" + container.Id.ToString() + "&i=" + itemId.ToString() + "&c=" + cultureCode + "&f=" + inputformId.ToString();
          }

          // SHOW Inheritance tab
          if (item != null && item.Level.Id <= ItemLevel.GetSkuLevel().Id)
          {
            tabInheritance.Visible = (!simpleView && (container.InheritanceMethodId > 0));
            tabInheritance.ContentPane.TargetUrl = tabInheritance.ContentPane.TargetUrl + "?i=" + itemId.ToString() + "&d=" + container.Id.ToString() + "&c=" + cultureCode;
          }
        }
        if (SessionState.QDEChunk != null)
        {
          uwToolbar.Items.FromKeyLabel("Action").Text = container.Group.Path + container.Group.Name + "/" + container.Tag;
          tabHistory.Visible = SessionState.QDEChunk.HistoryCount > 0 && !simpleView;
          tabHistory.Text = tabHistory.Text + " (" + SessionState.QDEChunk.HistoryCount.ToString() + ")";
          tabHistory.ContentPane.TargetUrl = tabHistory.ContentPane.TargetUrl + "?d=" + container.Id.ToString() + "&i=" + SessionState.QDEChunk.ItemId.ToString() + "&c=" + cultureCode;
          uwToolbar.Items.FromKeyLabel("Action").ToolTip = container.Group.Path + container.Group.Name + "/" + container.Tag + " [" + container.Name + "]";
        }
        else // Create
        {
          uwToolbar.Items.FromKeyLabel("Action").Text = container.Group.Path + container.Group.Name + "/" + container.Tag;
          tabHistory.Visible = false;
        }

        if (!simpleView)
        {
          // SHOW/HIDE Regionalization Tab            
          int nbRegionalization = HyperCatalog.Business.Chunk.GetRegionalizationsCount(itemId, container.Id, cultureCode);
          tabRegionalization.Visible = nbRegionalization > 0;
          tabRegionalization.ContentPane.TargetUrl = tabRegionalization.ContentPane.TargetUrl + "?d=" + containerId.ToString() + "&i=" + itemId.ToString() + "&c=" + cultureCode;
          tabRegionalization.Text = tabRegionalization.Text + " (" + nbRegionalization + ")";

          // SHOW/HIDE Translation Tab            
          int nbTranslation = HyperCatalog.Business.Chunk.GetTranslationsCount(itemId, container.Id, cultureCode);
          tabTranslation.Visible = nbTranslation > 0;
          tabTranslation.ContentPane.TargetUrl = tabTranslation.ContentPane.TargetUrl + "?d=" + containerId.ToString() + "&i=" + itemId.ToString() + "&c=" + cultureCode;
          tabTranslation.Text = tabTranslation.Text + " (" + nbTranslation + ")";
        }
        /////////////////////////////////////////////////////////////
        //Determine which kind of interface we should show
        /////////////////////////////////////////////////////////////
        string masterCultureCode = HyperCatalog.Shared.SessionState.MasterCulture.Code;
          //get code for regions and master culture
        HyperCatalog.Business.CultureList allCultures = HyperCatalog.Business.Culture.GetAll("CultureTypeId IN (0,1)");
        foreach (HyperCatalog.Business.Culture culcode in allCultures)
        {
            if (curCulture.Code == culcode.Code)
                isRegion = true; 
        }

        switch (Convert.ToChar(container.DataType.Code))
        {
          case 'D': tabChunk.ContentPane.TargetUrl = "Chunk_Date.aspx";
            break;
          case 'L':// List is not supported
            tabChunk.ContentPane.TargetUrl = "Chunk_Text.aspx";
            break;
          case 'N': tabChunk.ContentPane.TargetUrl = "Chunk_Number.aspx";
            break;
          case 'B': tabChunk.ContentPane.TargetUrl = "Chunk_Boolean.aspx";
            break;
          // A MODIFIER ICI POUR PHOTOS
          case 'U': goto case 'S';
          case 'P': goto case 'S';
          case 'R':
          case 'S': goto case 'T';
          case 'T': tabChunk.ContentPane.TargetUrl = "Chunk_Text.aspx";
            break;
        }
        // IF containe type is Photo, override dataTyping
        //2007/01/19 - modif greg - old : if (container.ContainerTypeCode == 'P' || container.ContainerTypeCode == 'L')
        if (container.ContainerTypeCode == 'P' || container.DataTypeCode == 'P')
        {
          tabChunk.ContentPane.TargetUrl = "Chunk_Resource.aspx";
          if (SessionState.QDEChunk != null && SessionState.QDEChunk.Text != HyperCatalog.Business.Chunk.BlankValue)
          {
            // SHOW Image preview tab
           /************ Ref QC#871 (Making Image preview tab visible) ********************/
            tabImage.Visible = true;
            tabImage.ContentPane.TargetUrl = tabImage.ContentPane.TargetUrl + "?d=" + container.Id.ToString() + "&i=" + SessionState.QDEChunk.ItemId.ToString() + "&c=" + cultureCode;
          }
        }
        if (container.InputMask != string.Empty && masterCultureCode == curCulture.Code)
        {
          tabChunk.ContentPane.TargetUrl = "Chunk_InputMask.aspx";
        }

          //if (container.LookupId >= 0 && masterCultureCode == curCulture.Code)
        if (container.LookupId >= 0 && isRegion ) // Prabhu Fix for HPeZilla Bug 69251 - LUT not showing up in regions for editing
        {
          tabChunk.ContentPane.TargetUrl = "Chunk_Lookup.aspx";
          if (Request["ifcid"] != null && Request["ifcid"].ToString() != "-1")
          {
            using (InputFormContainer inputFormContainer = InputFormContainer.GetByKey(Convert.ToInt32(Request["ifcid"])))
            {
              if (inputFormContainer != null)
              {
                extraParam += "&t=" + InputFormContainer.GetTypeFromEnum(inputFormContainer.Type);
              }
            }
          }
        }

        /* Alternate for CR 5096(Removal of rejections)
        if (SessionState.QDEChunk != null && SessionState.QDEChunk.Status == ChunkStatus.Rejected)
        {
          tabChunk.ContentPane.TargetUrl = "Chunk_Rejected.aspx";
          tabMaster.Visible = tabHistory.Visible = tabRegionalization.Visible = tabTranslation.Visible =
          tabPossibleValues.Visible = tabInheritance.Visible = tabImage.Visible = false;
        }
         */
        tabChunk.ContentPane.TargetUrl = tabChunk.ContentPane.TargetUrl + "?d=" + container.Id.ToString() + "&c=" + cultureCode + "&i=" + itemId.ToString() + "&m=" + isMandatory.ToString() + extraParam;

        // If chunk is coming from List in Input Form, force type to list
        if (Request["ifcid"] != null && Request["ifcid"].ToString() !="-1")
        {
          InputFormContainer inputFormContainer = InputFormContainer.GetByKey(Convert.ToInt32(Request["ifcid"]));
          if (inputFormContainer != null)
          {
            //if (inputFormContainer.Type != InputFormContainerType.Normal && masterCultureCode == curCulture.Code)
              if (inputFormContainer.Type != InputFormContainerType.Normal && isRegion)// Prabhu Fix for HPeZilla Bug 69251 - LUT not showing up in regions for editing
            {
              tabChunk.ContentPane.TargetUrl = "Chunk_Lookup.aspx?d=" + container.Id.ToString() + "&c=" + cultureCode + "&i=" + itemId.ToString() + "&m=" + isMandatory.ToString() + "&t=" + InputFormContainer.GetTypeFromEnum(inputFormContainer.Type) + "&ifid=" + Request["ifcid"].ToString() + extraParam;
            }
          }
        }

        if (tabTranslation.Visible)
        {
          tabChunk.ContentPane.TargetUrl = tabChunk.ContentPane.TargetUrl + "&ht=1"; // HasTranslations
        }
        if (SessionState.CurrentItemIsUserItem)
        {
          tabChunk.ContentPane.TargetUrl = tabChunk.ContentPane.TargetUrl + "&ui=1"; // HasItemInScope
        }

        tabContainer.ContentPane.TargetUrl = tabContainer.ContentPane.TargetUrl + "?d=" + container.Id.ToString();
        tabContainer.Visible = !simpleView;
        Page.DataBind();
      }
      #endregion
    }
    catch (Exception ex)
    {
      Response.Write(ex.ToString());
      Response.End();
    }
  }
  

  #region "GetChunk"
  public static HyperCatalog.Business.Chunk GetChunk(System.Int64 itemId, int containerId, string cultureCode)
  {
    if (SessionState.QDEChunk == null || SessionState.QDEChunk.ItemId != itemId ||
        SessionState.QDEChunk.ContainerId != containerId || SessionState.QDEChunk.CultureCode != cultureCode)
    {
      if (SessionState.QDEChunk != null) SessionState.QDEChunk.Dispose();
      //SessionState.QDEChunk = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, cultureCode);
      if (SessionState.QDEContainer != null) SessionState.QDEContainer.Dispose();
      //SessionState.QDEContainer = HyperCatalog.Business.Container.GetByKey(containerId);
    }
	SessionState.QDEChunk = HyperCatalog.Business.Chunk.GetByKey(itemId, containerId, cultureCode);
    SessionState.QDEContainer = HyperCatalog.Business.Container.GetByKey(containerId);
    return SessionState.QDEChunk;
  }
  public static void ForceTranslationsTo(HyperCatalog.Business.Chunk chunk, int userId, ChunkStatus status)
  {
    chunk.ForceTranslationsTo(userId, status);
    //SessionState.QDEChunk = null;
  }
  #endregion
}
