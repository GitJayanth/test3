#region uses
using System;
using System.Xml;

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
using HyperCatalog.Shared.Defs;
using HyperCatalog.Business;
using HyperCatalog.UI.Main;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Description résumée de qde_formcontent.
  /// </summary>
  public partial class qde_formcontent : HCPage
  {
    #region Declaration
    protected System.Web.UI.WebControls.Label lbTitle;
    protected System.Web.UI.WebControls.CheckBox CheckBox1;
    bool isUserItem = false;
    //Added by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
    bool isAutoTRButton = false;

/*  Alternate for CR 5096(Removal of rejection functionality)--start
    protected int stat_total, stat_nbFinal, stat_nbDraft, stat_nbMissing, stat_nbRejected;
    protected int stat_totalMandatory, stat_Mandatory_F, stat_Mandatory_D, stat_Mandatory_R, stat_Mandatory_M;
    protected int stat_nbFinal_inh, stat_nbDraft_inh, stat_nbRejected_inh;
*/
    protected int stat_total, stat_nbFinal, stat_nbDraft, stat_nbMissing;
    protected int stat_totalMandatory, stat_Mandatory_F, stat_Mandatory_D, stat_Mandatory_M;
    protected int stat_nbFinal_inh, stat_nbDraft_inh;
//  Alternate for CR 5096(Removal of rejection functionality)--end
    private Culture fallBackCulture, fallBackCulture2, fallBackCulture3;
    private string currentGroup = string.Empty;
    private int groupCount = 0;
    private string startTab = string.Empty;
    private string inputFormId, masterCultureCode;
    private System.Int64 itemId;
    private bool viewOnlyMandatory = false;
    private bool viewOnlyRegionalizable = false;
    TemplatedColumn col;

    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
    #endregion

    /// <summary>
    /// currentGroup will store the path for the current container
    /// Since we are expecting to receive from the SP a list ordered by
    /// path, each time the path changes, a new title row will be added in the 
    /// grid
    /// </summary>
    protected void Page_Load(object sender, System.EventArgs e)
    {
      // *************************************************************************
      // Retrieve Product information
      // *************************************************************************
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      col = (TemplatedColumn)dg.Columns.FromKey("Select");

      /* Alternate for CR 5096(Removal of rejection functionality)--start
      stat_totalMandatory = stat_Mandatory_D = stat_Mandatory_F = stat_Mandatory_M = stat_Mandatory_R = 0;
      stat_total = stat_nbFinal = stat_nbDraft = stat_nbMissing = stat_nbRejected = 0;
      stat_nbDraft_inh = stat_nbFinal_inh = stat_nbRejected_inh = 0; 
      */
      stat_totalMandatory = stat_Mandatory_D = stat_Mandatory_F = stat_Mandatory_M = 0;
      stat_total = stat_nbFinal = stat_nbDraft = stat_nbMissing = 0;
      stat_nbDraft_inh = stat_nbFinal_inh = 0;
      //Alternate for CR 5096(Removal of rejection functionality)--end

      itemId = QDEUtils.GetItemIdFromRequest().Id;
      QDEUtils.UpdateCultureCodeFromRequest();
      viewOnlyMandatory = uwToolbar.Items.FromKeyButton("MandatoryOnly").Selected;
      viewOnlyRegionalizable = uwToolbar.Items.FromKeyButton("RegionalizableOnly").Selected;
      // Retrieve culture and master culture code
      if (SessionState.Culture == null)
      {
        UITools.FindUserFirstCulture(false);
      }
      masterCultureCode = HyperCatalog.Shared.SessionState.MasterCulture.Code;

      // Retrieve input form id
      if (Request["f"] != null)
      {
        inputFormId = Request["f"].ToString(); // for example: IF_177
        inputFormId = inputFormId.Substring(3, inputFormId.Length - 3);
      }

      // Update fallbacks
      if (SessionState.Culture.Type == CultureType.Regionale)
      {
        fallBackCulture = SessionState.Culture.Fallback;
      }
      if (SessionState.Culture.Type == CultureType.Locale)
      {
        fallBackCulture = SessionState.Culture.Fallback;
        fallBackCulture2 = fallBackCulture.Fallback;
      }

      dg.Columns.FromKey("Select").ServerOnly = !SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_DRAFT_CHUNKS) &&
        !SessionState.User.HasCapability(CapabilitiesEnum.DELETE_ITEMS);

      if (!Page.IsPostBack)
      {
        ViewState["nbCellsWithIndex"] = 0; 
        UpdateDataView();
      }
      else
      {
        if (Request["action"] != null)
        {
          if (Request["action"].ToString() == "reload")
          {
            UpdateDataView();
          }
        }

      }
      isUserItem = SessionState.CurrentItemIsUserItem;
      CheckIfSpellCheckOrMoveStatus();
      CheckIfContentCanBePasted();
      CheckIfContentCanBeDeleted();
      CheckIfAutoTranslate();
      CheckIfRegionalizable();

      //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 01/Jul/09
      string strScript = "<script>inputFormName='" + ViewState["InputFormName"].ToString() + "'; inputFormId=" + inputFormId + "; isAutoTRButton = " + isAutoTRButton.ToString().ToLower() + "; containerLimit=" + Convert.ToString(ApplicationSettings.Parameters["AutoTR_UIContainerLimit"].Value) + "; cultureCode='" + SessionState.Culture.Code + "';iId=" + itemId.ToString() + ";nbCellsWithIndex=" + ViewState["nbCellsWithIndex"].ToString() + ";</script>";
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitFormName", strScript);
    }


    private void CheckIfSpellCheckOrMoveStatus()
    {
      if (!isUserItem || inputFormId.Equals("-1"))
      {
        UITools.HideToolBarButton(uwToolbar, "SpellCheck");
        UITools.HideToolBarSeparator(uwToolbar, "SpellCheckSep");
        UITools.HideToolBarButton(uwToolbar, "MoveStatusTo");
        UITools.HideToolBarSeparator(uwToolbar, "MoveStatusToSep");
      }
      else
      {
        UITools.ShowToolBarButton(uwToolbar, "SpellCheck");
        UITools.ShowToolBarSeparator(uwToolbar, "SpellCheckSep");
        UITools.ShowToolBarButton(uwToolbar, "MoveStatusTo");
        UITools.ShowToolBarSeparator(uwToolbar, "MoveStatusToSep");
      }
    }
    private void CheckIfAutoTranslate()
    {
        //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
        if (SessionState.Culture.Type == CultureType.Regionale && isUserItem && SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT))
        {
            UITools.ShowToolBarButton(uwToolbar, "TinyTM");
            UITools.ShowToolBarSeparator(uwToolbar, "TinyTMSep");
            isAutoTRButton = true;
        }
        else
        {
            UITools.HideToolBarButton(uwToolbar, "TinyTM");
            UITools.HideToolBarSeparator(uwToolbar, "TinyTMSep");
        }
    }
    private void CheckIfContentCanBeDeleted()
    {
      if (!isUserItem || !SessionState.User.HasCapability(CapabilitiesEnum.DELETE_ITEMS))
      {
        UITools.HideToolBarButton(uwToolbar, "Delete");
        UITools.HideToolBarSeparator(uwToolbar, "DeleteSep");
      }
    }
    private void CheckIfContentCanBePasted()
    {
      UITools.HideToolBarButton(uwToolbar, "Paste");
      UITools.HideToolBarSeparator(uwToolbar, "PasteSep");

      if (!isUserItem || !SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT))
      {
        UITools.HideToolBarButton(uwToolbar, "Copy");
        UITools.HideToolBarSeparator(uwToolbar, "CopySep");
      }
      else
      {
        if (SessionState.Clipboard.Items.Count > 0 &&
          SessionState.User.LastVisitedItem != SessionState.Clipboard.ItemId &&
          Convert.ToInt32(inputFormId) == SessionState.Clipboard.InputFormId &&
          SessionState.Culture.Code == SessionState.Clipboard.CultureCode)
        {
          bool allFound, isFound;
          allFound = true;
          foreach (ClipboardItem i in SessionState.Clipboard.Items)
          {
            isFound = false;
            foreach (UltraGridRow dr in dg.Rows)
            {
              isFound = dr.Cells.FromKey("ContainerId").Text == i.ContainerId.ToString();
              if (isFound) break;
            }
            allFound = isFound;
            if (!allFound) break;
          }
          if (allFound)
          {
            UITools.ShowToolBarButton(uwToolbar, "Paste");
            UITools.ShowToolBarSeparator(uwToolbar, "PasteSep");
          }
          else
          {
            UITools.HideToolBarButton(uwToolbar, "Paste");
            UITools.HideToolBarSeparator(uwToolbar, "PasteSep");
          }
        }
      }
    }
    private void CheckIfRegionalizable()
    {
      if (SessionState.Culture.Type == CultureType.Regionale && isUserItem && !inputFormId.Equals("-1"))
      {
        UITools.ShowToolBarSeparator(uwToolbar, "RegionSep");
        UITools.ShowToolBarButton(uwToolbar, "Regionalizable");
        UITools.ShowToolBarSeparator(uwToolbar, "RegionOnlySep");
        UITools.ShowToolBarButton(uwToolbar, "RegionalizableOnly");
      }
      else
      {
        UITools.HideToolBarSeparator(uwToolbar, "RegionSep");
        UITools.HideToolBarButton(uwToolbar, "Regionalizable");
        UITools.HideToolBarSeparator(uwToolbar, "RegionOnlySep");
        UITools.HideToolBarButton(uwToolbar, "RegionalizableOnly");
      }
    }

    private void UpdateDataView()
    {
      string sTab;
      string filter = txtFilter.Text;
      dg.Rows.Clear();
      // *************************************************************************
      // Analyse which tab has been choosen
      // *************************************************************************
      sTab = "IF_-1"; // All content by default
      if (Request.QueryString["f"] != null)
        sTab = Request.QueryString["f"].ToString().ToUpper();

      if (sTab.StartsWith("IF"))
      {
        inputFormId = sTab.Substring(3, sTab.Length - 3);
        BuildInputFormTab(filter);
        InitializeGridGrouping();
      }
      else
      {
        UITools.DenyAccess(DenyMode.Frame);
        Response.End();
      }

      // Update label "lbListType" in tool bar
      uwToolbar.Items.FromKeyLabel("lbListType").DefaultStyle.Width = Unit.Pixel(120);
      if (filter.Length > 0)
      {
        uwToolbar.Items.FromKeyLabel("lbListType").Text = "Partial list";
      }
      else
      {
        if (viewOnlyMandatory || viewOnlyRegionalizable)
        {
          uwToolbar.Items.FromKeyLabel("lbListType").Text = string.Empty;
          if (viewOnlyMandatory)
          {
            uwToolbar.Items.FromKeyLabel("lbListType").Text = "Mandatory/";
          }
          if (viewOnlyRegionalizable)
          {
            if (uwToolbar.Items.FromKeyLabel("lbListType").Text.Length > 0)
            {
              uwToolbar.Items.FromKeyLabel("lbListType").DefaultStyle.Width = Unit.Pixel(180);
            }
            uwToolbar.Items.FromKeyLabel("lbListType").Text = uwToolbar.Items.FromKeyLabel("lbListType").Text + "Regionalizable/";
          }
          uwToolbar.Items.FromKeyLabel("lbListType").Text = uwToolbar.Items.FromKeyLabel("lbListType").Text + "only";
        }
        else
        {
          uwToolbar.Items.FromKeyLabel("lbListType").Text = "Complete list";
        }
      }

      // update stat
      txtTotal.Text = stat_total.ToString();
      txtTotalMandatory.Text = "(Total: " + stat_totalMandatory.ToString();
      if (stat_Mandatory_F > 0)
        txtTotalMandatory.Text += ", Final: " + stat_Mandatory_F.ToString();
      if (stat_Mandatory_D > 0)
        txtTotalMandatory.Text += ", Draft: " + stat_Mandatory_D.ToString();
      /* Alternate for CR 5096(Removal of rejection functionality)
      if (stat_Mandatory_R > 0)
        txtTotalMandatory.Text += ", Rejected: " + stat_Mandatory_R.ToString();
       */
      if (stat_Mandatory_M > 0)
        txtTotalMandatory.Text += ", Missing: " + stat_Mandatory_M.ToString();
      txtTotalMandatory.Text += ")";

      int notInheritedDraft = stat_nbDraft - stat_nbDraft_inh;
      txtNbDraft.Text = stat_nbDraft.ToString() + " (" + stat_nbDraft_inh.ToString() + "+" + notInheritedDraft.ToString() + ")";
      int notInheritedFinal = stat_nbFinal - stat_nbFinal_inh;
      txtnbFinal.Text = stat_nbFinal.ToString() + " (" + stat_nbFinal_inh.ToString() + "+" + notInheritedFinal.ToString() + ")";
      txtNbMissing.Text = stat_nbMissing.ToString();

      /* Alternate for CR 5096(Removal of rejection functionality)
      if (SessionState.Culture.Type == CultureType.Regionale)
      {
        int notInheritedRejected = stat_nbRejected - stat_nbRejected_inh;
        txtNbRejected.Text = "&nbsp;&nbsp;<img height='11' src='/hc_v4/img/SR.gif' width='11' style='vertical-align:top' alt=''/>&nbsp;Rejected: " + stat_nbRejected.ToString() + " (" + stat_nbRejected_inh.ToString() + "+" + notInheritedRejected.ToString() + ")";
      }
      else
      {
        txtNbRejected.Visible = false;
      }
       */

      // Update visibility
      if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT) != null)
      {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "UpdateVisibility", "<script>hideDiv('divToolbar', '" + (!SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TOOLBAR_STAT).Value).ToString() + "')</script>");
      }
    }
    private void BuildInputFormTab(string filter)
    {
      // Show/Hide Culture column
      if (Convert.ToInt32(inputFormId) >= 0 && SessionState.Culture.Type != CultureType.Master && SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_CULTURE) != null)
      {
        dg.Columns.FromKey("Country").ServerOnly = (!SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_CULTURE).Value);
      }
      else
      {
        dg.Columns.FromKey("Country").ServerOnly = true;
      }
      if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_INHERITANCEMODE) != null)
      {
        dg.Columns.FromKey("InheritanceMethodId").ServerOnly = (!SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_INHERITANCEMODE).Value);
      }
      if (Convert.ToInt32(inputFormId) >= 0)
      {
        // Show/Hide Comment column
        if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_COMMENT) != null)
        {
          dg.Columns.FromKey("Comment").ServerOnly = (!SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_COMMENT).Value);
        }
        InputForm inputForm = InputForm.GetByKey(Convert.ToInt32(inputFormId));
        ViewState["InputFormName"] = inputForm.Name;
        SessionState.QDETab = "tb_" + inputFormId;

        // Show / Hide Paste button in the toolbar
        if (SessionState.Clipboard.Items.Count > 0)
        {
          if (SessionState.Clipboard.Items.Item(0).ItemId == itemId)
          {
            UITools.HideToolBarButton(uwToolbar, "Paste");
            UITools.HideToolBarSeparator(uwToolbar, "PasteSep");
          }
        }
        else
        {
          UITools.HideToolBarButton(uwToolbar, "Paste");
          UITools.HideToolBarSeparator(uwToolbar, "PasteSep");
        }
      }
      else
      {
        SessionState.QDETab = "tb_all";
        ViewState["InputFormName"] = "All attached content";
      }

      using (InputFormChunkList chunkList = InputFormChunk.GetByInputForm(itemId, Convert.ToInt32(inputFormId), SessionState.Culture.Code))
      {
        dg.DataSource = chunkList;
        Utils.InitGridSort(ref dg, false);
        dg.DataBind();
        dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
        dg.DisplayLayout.Pager.AllowPaging = false;
      }
    }

    /// <summary>
    /// This function allow user to Copy/Cut selected chunks to the internal clipboard
    /// </summary>
    /// <param name="action"></param>
    private void ClipboardItems(ClipboardAction action)
    {
      SessionState.Clipboard.Action = action;
      SessionState.Clipboard.Items.Clear();
      SessionState.Clipboard.ItemId = itemId;
      SessionState.Clipboard.InputFormId = Convert.ToInt32(inputFormId);
      SessionState.Clipboard.CultureCode = SessionState.Culture.Code;
      int nbCopyItems = 0;

      // Uncheck Header cell if necessary
      ((CheckBox)(col.HeaderItem.FindControl("g_ca"))).Checked = false;

      // Loop items and copy to clipboard
      int currentIndex = -1;
      bool isInherited, hasFallback = false;

      for (int i = 0; i < col.CellItems.Count; i++)
      {
        currentIndex = ((CellItem)col.CellItems[i]).Cell.Row.Index;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
        if (cb != null && cb.Checked)
        {
            //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/Jun/09
            isInherited = Convert.ToBoolean(dg.Rows[currentIndex].Cells.FromKey("Inherited").Value);
            hasFallback = Convert.ToBoolean(dg.Rows[currentIndex].Cells.FromKey("hasFallback").Value);
            if (!isInherited && !hasFallback)
            {
                SessionState.Clipboard.Items.Add(new ClipboardItem(Convert.ToInt64(itemId),
                  Convert.ToInt32(dg.Rows[currentIndex].Cells.FromKey("ContainerId").Value),
                  dg.Rows[currentIndex].Cells.FromKey("ContainerName").Value.ToString(), SessionState.Culture.Code));
                nbCopyItems++;
                cb.Checked = false;
            }
        }
      }
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clipboard", "<script>alert('" + nbCopyItems.ToString() + " item(s) copied to clipboard');</script>");
    }
    /// <summary>
    /// This function allow user to delete content
    /// </summary>
    /// <param name="action"></param>
    private void DeleteSelectedItems()
    {
      int nbDeletedItems = 0;
      string errorMessage = string.Empty;

      // Uncheck Header cell if necessary
      ((CheckBox)(col.HeaderItem.FindControl("g_ca"))).Checked = false;

      // Loop items and delete chunk value
      int currentIndex = -1;
      bool isInherited, hasFallback = false;

      for (int i = 0; i < col.CellItems.Count; i++)
      {
        currentIndex = ((CellItem)col.CellItems[i]).Cell.Row.Index;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
        if (cb != null && cb.Checked)
        {
            //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/Jun/09
            isInherited = Convert.ToBoolean(dg.Rows[currentIndex].Cells.FromKey("Inherited").Value);
            hasFallback = Convert.ToBoolean(dg.Rows[currentIndex].Cells.FromKey("hasFallback").Value);

          if (HyperCatalog.Business.Chunk.DeleteByKey(Convert.ToInt64(itemId),
            Convert.ToInt32(dg.Rows[currentIndex].Cells.FromKey("ContainerId").Value),
            SessionState.Culture.Code, SessionState.User.Id))
          {
              //Modified by Prabhu for QC 2728 - Java Error is coming while deleting a inherited and fallback content at region
              if (!isInherited && !hasFallback)
                  nbDeletedItems++;
          }
          else
          {
            errorMessage += "Cannot delete " + dg.Rows[currentIndex].Cells.FromKey("ContainerName").Text + " -> " + HyperCatalog.Business.Chunk.LastError + Environment.NewLine;
          }
        }
      }

      UpdateDataView();
      if (errorMessage == string.Empty)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowAlert", "<script>alert('" + nbDeletedItems.ToString() + " chunk(s) deleted');</script>");
      }
      else
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowAlert", "<script>alert('Some chunks raised errors: " + Environment.NewLine + errorMessage + ");</script>");
      }
    }

    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      switch (btn)
      {
        case "export":
          {
            int ifId = Convert.ToInt32(inputFormId);
            ExportUtils.ExportGridOfFrameContent(itemId, ifId, uwToolbar.Items.FromKeyButton("MandatoryOnly").Selected, uwToolbar.Items.FromKeyButton("RegionalizableOnly").Selected, txtFilter.Text, this);
            break;
          }
        case "delete":
          {
            DeleteSelectedItems();
            UpdateDataView();
            break;
          }
        case "copy":
          {
            ClipboardItems(ClipboardAction.Copy);
            UpdateDataView();
            break;
          }
        case "cut":
          {
            ClipboardItems(ClipboardAction.Cut);
            UpdateDataView();
            break;
          }
        case "mandatoryonly":
          {
            viewOnlyMandatory = be.Button.Selected;
            UpdateDataView();
            break;
          }
        case "regionalizableonly":
          {
            viewOnlyRegionalizable = be.Button.Selected;
            UpdateDataView();
            break;
          }
      }
    }
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      string filter = txtFilter.Text.Trim();
      bool keep = true;
      bool isMandatory = Convert.ToBoolean(e.Row.Cells.FromKey("IsMandatory").Value);
      bool isInherited = Convert.ToBoolean(e.Row.Cells.FromKey("Inherited").Value);
      bool isResource = Convert.ToBoolean(e.Row.Cells.FromKey("IsResource").Value);
      bool isBoolean = Convert.ToBoolean(e.Row.Cells.FromKey("IsBoolean").Value);
      bool readOnly = Convert.ToBoolean(e.Row.Cells.FromKey("ReadOnly").Value);
      bool isRegionalizable = Convert.ToBoolean(e.Row.Cells.FromKey("IsRegionalizable").Value);
      bool hasFallback = Convert.ToBoolean(e.Row.Cells.FromKey("hasFallback").Value);
      if (viewOnlyMandatory && !isMandatory)
      {
        keep = false;
        e.Row.Delete();
      }
      else
      {
        if (viewOnlyRegionalizable && !isRegionalizable)
        {
          keep = false;
          e.Row.Delete();
        }
        else
        {
          // Display Regionalizable Flag for regianalizable containers
          if (filter != string.Empty && keep)
          {
            keep = false;
            foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in e.Row.Cells)
            {
              if (!c.Column.Hidden && c.Value != null && c.Text != HyperCatalog.Business.Chunk.BlankValue
                   && (c.Column.Key == "ContainerName" || c.Column.Key == "Value" || c.Column.Key == "Comment"))
              {
                if (c.Text.ToLower().IndexOf(filter.ToLower()) >= 0)
                {
                  c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
                  keep = true;
                }
              }
            }
          }
          if (!keep)
          {
            e.Row.Delete();
          }
          if (keep)
          {
            UltraGridCell aCell = e.Row.Cells.FromKey("Status"), bCell, vCell;

            keep = aCell.Text != string.Empty;

            stat_total++;

            // Update image for language/country
            if (aCell.Value != null)
            {
              ChunkStatus cStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus), aCell.Value.ToString());
              if (cStatus != ChunkStatus.Missing)
              { //"sc" = sourceCode field
                e.Row.Cells.FromKey("Country").Value = "<img alt='" + e.Row.Cells.FromKey("sc").Value.ToString() + "' src='/hc_v4/img/flags/" + e.Row.Cells.FromKey("cc").Value.ToString() + ".gif'>";
              }
            }
            // Update Index column (to navigate in chunk list)
            e.Row.Cells.FromKey("Index").Value = e.Row.Index;
            ViewState["nbCellsWithIndex"] = Convert.ToInt32(ViewState["nbCellsWithIndex"]) + 1;
            //If RTL languages, ensure correct display        
            vCell = e.Row.Cells.FromKey("Value");
            if ((bool)e.Row.Cells.FromKey("Rtl").Value)
              vCell.Style.CustomRules = "direction: rtl;";//unicode-bidi:bidi-override;";

            // Update CultureCode column
            //e.Row.Cells.FromKey("CultureCode").Text = SessionState.Culture.Code;

            // Container group
            string containerGroup = e.Row.Cells.FromKey("Path").Text;
            if (currentGroup != containerGroup)
            {
              currentGroup = containerGroup;
              groupCount++;
            }

            //Display Edit Link in Container Name
            int rowIndex = e.Row.Index + groupCount;
            aCell = e.Row.Cells.FromKey("ContainerName");
            aCell.Text = "<a href='javascript://' onclick=\"ed(" + rowIndex.ToString() + ")\">" + aCell.Text + "</a>";

            // Check if ReadOnly container
            if (readOnly)
              aCell.Text = aCell.Text + " <img src='/hc_v4/img/ed_glasses.gif'/>";
            string sCultureCode = e.Row.Cells.FromKey("sc").Text;
            if (isRegionalizable && !viewOnlyRegionalizable && SessionState.Culture.Type == CultureType.Regionale)
            {
              aCell.Text = aCell.Text + " <img src='/hc_v4/img/cr.gif'/>";
            }


            //Display Mandatory logo
            aCell = e.Row.Cells.FromKey("Mandatory");
            aCell.Style.CssClass = "ptb1";
            aCell.Text = string.Empty; // by default
            if (isMandatory)
            {
              aCell.Style.CssClass = "SCM"; // Status Chunk Mandatory
              stat_totalMandatory++;
            }

            //Display Status logo
            aCell = e.Row.Cells.FromKey("Status");
            if (aCell.Value != null)
            {
              ChunkStatus cStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus), aCell.Value.ToString());
              string status = HyperCatalog.Business.Chunk.GetStatusFromEnum(cStatus);
              aCell.Style.CssClass = "S" + status;
              aCell.Value = string.Empty;

              // Update counts of status
              switch (cStatus)
              {
                case ChunkStatus.Draft: stat_nbDraft++;
                  if (isInherited) stat_nbDraft_inh++;
                  if (isMandatory) stat_Mandatory_D++;
                  break;
                case ChunkStatus.Final: stat_nbFinal++;
                  if (isInherited) stat_nbFinal_inh++;
                  if (isMandatory) stat_Mandatory_F++;
                  break;
                /* Alternate for CR 5096(Removal of rejection functionality)
                case ChunkStatus.Rejected: stat_nbRejected++;
                  if (isInherited) stat_nbRejected_inh++;
                  if (isMandatory) stat_Mandatory_R++;
                  break; */
                case ChunkStatus.Missing: stat_nbMissing++;
                  if (isMandatory) stat_Mandatory_M++;
                  break;
              }
            }

            // Check if value is inherited          
            vCell.Style.CssClass = "ptb3"; // by default
            vCell.Style.Wrap = true; // by default
            if (isInherited)
            {
              vCell.Style.CssClass = "overw";
              vCell.Style.Wrap = true;
            }

            // Ensure multiline is kept
            if (vCell.Value != null)
            {
              // if chunk is resource, try do display it
              if (isResource && vCell.Text != string.Empty)
              {
                string sUrl = vCell.Text;
                if (sUrl == HyperCatalog.Business.Chunk.BlankValue)
                {
                  vCell.Text = HyperCatalog.Business.Chunk.BlankText;
                  vCell.Style.CustomRules = string.Empty;
                }
                else
                {
                  // Call HyperPublisher WebMethod to convert URL to absolute URL
                  XmlDocument xmlInfo = new XmlDocument();
                  try
                  {
                    string fullPath = Business.ApplicationSettings.Components["DAM_Provider"].URI + "/" + sUrl + "?DAM_avoid404=1&DAM_culture=" + SessionState.Culture.Code;
                    vCell.Text = "<a href='" + fullPath + "' target='_blank'><img src='" + fullPath + "&thumbnail=1&size=40' title='" + vCell.Text + "' border=0/></a>";
                  }
                  catch (Exception ex)
                  {
                    vCell.Text = "<img src='/hc_v4/img/ed_notfound.gif' title='An exception occurred: " + UITools.CleanJSString(ex.ToString()) + "' border=0/>";
                    Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
                  }
                }
              }
              else
              {
                // BLANK Value is replace by Readable sentence
                if (vCell.Text == HyperCatalog.Business.Chunk.BlankValue)
                {
                  vCell.Text = HyperCatalog.Business.Chunk.BlankText;
                  vCell.Style.CustomRules = string.Empty;
                }
                else
                {
                  if (isBoolean && vCell.Text != string.Empty)
                  {
                    try
                    {
                      vCell.Text = Convert.ToBoolean(vCell.Value) ? "Yes" : "No";
                    }
                    catch { } // Value is not boolean!
                  }
                  else
                  {
                    vCell.Text = UITools.HtmlEncode(vCell.Text);
                  }
                }

                // Display Fallback on cultures if current Culture <> "master"
                string sourceCultureCode = e.Row.Cells.FromKey("sc").Text;
                if (sourceCultureCode != SessionState.Culture.Code)
                {
                  if (fallBackCulture != null && fallBackCulture.Code == sourceCultureCode)// fallback 1
                    vCell.Style.CustomRules = vCell.Style.CustomRules + "color:blue;font-style:italic;";
                  else if (fallBackCulture2 != null && fallBackCulture2.Code == sourceCultureCode) // fallback 2
                    vCell.Style.CustomRules = vCell.Style.CustomRules + "color:red;font-style:italic;";
                  else if (fallBackCulture3 != null && fallBackCulture3.Code == sourceCultureCode) // fallback 3
                    vCell.Style.CustomRules = vCell.Style.CustomRules + "color:green;font-style:italic;";

                }
              }
              // Inheritance Method Id
              bCell = e.Row.Cells.FromKey("InheritanceMethodId");
              string methodId = bCell.Text;
              string strMethod = methodId == "0" ? "Fixed" : methodId == "1" ? "All Levels" : "Push Down";
              bCell.Text = "<img alt='" + strMethod + "' src='/hc_v4/img/inh_" + methodId + ".gif'/>";
              bCell.Style.CssClass = vCell.Style.CssClass;

              // Do not display checkbox for missing chunks and read only attributes
              if (SessionState.Culture.Type == CultureType.Master && (e.Row.Cells.FromKey("Status").Style.CssClass == "SM" || readOnly || e.Row.Cells.FromKey("sc").Text != SessionState.Culture.Code || isInherited))
              {
                CheckBox c = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
                c.Enabled = false;
              }
              //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/Jun/09
              else if (SessionState.Culture.Type == CultureType.Regionale && (e.Row.Cells.FromKey("Status").Style.CssClass == "SM" || readOnly))// && !isAutoTRButton)
              {
                  CheckBox c = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
                  c.Enabled = false;
              }
              else
              {
                  CheckBox c = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
                  c.Enabled = true;
              }
            }
          }
        }
      }
    }

    private void InitializeGridGrouping()
    {
      int begCol = dg.Columns.FromKey("Select").Index;
      if (dg.Columns.FromKey("Select").ServerOnly)
      {
          begCol = dg.Columns.FromKey("Mandatory").Index;
      }

      int i = 0;
      int x = dg.Rows.Count;
      TemplatedColumn col = (TemplatedColumn)dg.Columns.FromKey("Select");
      if (dg.Rows.Count > 0)
      {
        groupCount = 0;
        int colIndex = dg.Rows[i].Cells.FromKey("Path").Column.Index;
        while (i < dg.Rows.Count)
        {
          string containerGroup = dg.Rows[i].Cells[colIndex].Value.ToString();
          if (i == 0 || currentGroup != containerGroup)
          {
            currentGroup = containerGroup;
            dg.Rows.Insert(i, new UltraGridRow());
            CheckBox c = (CheckBox)((CellItem)col.CellItems[x]).FindControl("g_sd");
            c.Visible = false;
            Label l = (Label)((CellItem)col.CellItems[x]).FindControl("grp_lbl");
            l.Visible = true;
            l.Text = currentGroup;
            l.CssClass = "ptbgroup";
            l.BorderStyle = BorderStyle.None;
            UltraGridRow groupRow = dg.Rows[i];
            UltraGridCell groupCellMax = groupRow.Cells[dg.Columns.Count - 1]; // initialize all cells for this row
            foreach (UltraGridCell cell in groupRow.Cells)
            {
              cell.Style.CssClass = string.Empty;
            }
            dg.Rows[i].Style.CssClass = "ptbgroup";
            UltraGridCell groupCell = groupRow.Cells[begCol];
            groupCell.ColSpan = dg.Columns.Count - 1 - begCol;
            groupCell.Text = containerGroup;
            i++;
            x++;
          }
          i++;
        }
      }
    }
  }
}
