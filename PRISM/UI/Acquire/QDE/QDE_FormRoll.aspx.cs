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

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// Form contains two tabs if item has roll
	/// </summary>
  public partial class QDE_FormRoll : HCPage
  {
    #region Declarations
    protected string itemLevelName = string.Empty;

    private Item itemRoll = null;
    private string action = string.Empty;
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
      this.webTab.TabClick += new Infragistics.WebUI.UltraWebTab.TabClickEventHandler(this.webTab_TabClick);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        lbError.Visible = false;
        QDEUtils.GetItemIdFromRequest(); // get item
        QDEUtils.UpdateCultureCodeFromRequest();
        if (SessionState.CurrentItem == null)
        {
          webTab.Visible = false;
          Response.End();
        }
        else // item is not null
        {
          if (SessionState.CurrentItem.GetRoll() == null)
          {
            if (SessionState.CurrentItem.IsRoll && SessionState.CurrentItem.RefItem != null)
              SessionState.CurrentItem = SessionState.CurrentItem.RefItem;
            else
            {
              if (SessionState.Culture.Type == CultureType.Locale && !SessionState.CurrentItem.IsCountrySpecific)
              {
                Response.Redirect("QDE_CountryViewMain.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code, true);
              }
              else
              {
                Response.Redirect("QDE_Forms.aspx?i=" + SessionState.CurrentItem.Id, true);
              }
            }
          }

          using (itemRoll = SessionState.CurrentItem.GetRoll())
          {
            // Initialize vars
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Initialize", "<script>itemNameId = '" + lbItemName.ClientID + "';</script>");

            // Redirect on country view
            if (SessionState.Culture.Type == CultureType.Locale && !SessionState.CurrentItem.IsCountrySpecific)
            {
              if ((itemRoll == null) || (itemRoll != null && !itemRoll.IsInitialized)) // item has not roll or the roll is not initialized
              {
                Response.Redirect("QDE_CountryViewMain.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code, true);
                Response.End();
              }
              else // item has roll or item is roll
              {
                SessionState.CurrentItem = itemRoll;

                if (!Page.IsPostBack)
                {
                  pInfo.Visible = false;
                  ShowTabs();
                  Page.DataBind();
                }
              }
            }
            else
            {
              action = string.Empty;
              if (Request["a"] != null) // get action
                action = Request["a"].ToString();

              if (!Page.IsPostBack)
              {
                if (itemRoll == null
                  || (itemRoll != null && !itemRoll.IsInitialized && SessionState.Culture.Type != CultureType.Master)) // item has not roll or item is not roll or roll item is not initialized for regional culture
                {
                  Response.Redirect("QDE_Forms.aspx?i=" + SessionState.CurrentItem.Id, true);
                }
                else // item has roll or item is roll
                {
                    if (itemRoll != null && action.Equals("delete"))
                    {
                        if (HyperCatalog.Business.ApplicationParameter.IOHeirachyStatus() == 0)
                        {
                            DeleteItemRoll(itemRoll); // delete roll
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Softrole Error", "<script>alert(\"The Product Hierarchy Refresh job is in progress.  User cannot perform this action.  Please try once the job completes!\");</script>");
                            SessionState.CurrentItem = itemRoll;

                            ShowTabs();
                            Page.DataBind();
                        }
                    }
                    else
                    {
                        SessionState.CurrentItem = itemRoll;

                        ShowTabs();
                        Page.DataBind();
                    }
                }
              }
            }
          }
        }
      }
      finally
      {
        if (SessionState.CurrentItem.RefItem != null) SessionState.CurrentItem.RefItem.Dispose();
      }
    }

    private void ShowTabs()
    {
      webTab.Tabs.Clear();
      HyperCatalog.Business.Chunk localizedProductNameChunk = null;
      try
      {
        if (SessionState.Culture.Type != CultureType.Master && ((bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value))
          localizedProductNameChunk = HyperCatalog.Business.Chunk.GetByKey(SessionState.CurrentItem.Id, 1, SessionState.Culture.Code);

        // Update item name and level name
        if (localizedProductNameChunk == null)
          lbItemName.Text = SessionState.CurrentItem.FullName;
        else
        {
          if (SessionState.CurrentItem.Sku != null && SessionState.CurrentItem.Sku.Length > 0)
            lbItemName.Text = "[" + SessionState.CurrentItem.Sku + "] - " + localizedProductNameChunk.Text;
          else
            lbItemName.Text = localizedProductNameChunk.Text;
        }
        if (lbItemName.Text.Length > 50)
          lbItemName.Text = lbItemName.Text.Substring(0, 49) + "...";
        if (SessionState.CurrentItem.IsRoll)
          lbItemName.Text = "<img src='/hc_v4/img/ed_roll.gif'> " + lbItemName.Text;

        itemLevelName = SessionState.CurrentItem.Level.Name;
        string url1, url2;
        if (SessionState.CurrentItem.IsRoll) // item is roll
        {
          // Add tabs (original and roll)
          url1 = "QDE_Forms.aspx?i=" + SessionState.CurrentItem.RefItemId;
          url2 = "QDE_Forms.aspx?i=" + SessionState.CurrentItem.Id;
          if (SessionState.Culture.Type == CultureType.Locale && !SessionState.CurrentItem.IsCountrySpecific)
          {
            url1 = "QDE_CountryViewMain.aspx?i=" + SessionState.CurrentItem.RefItemId + "&c=" + SessionState.Culture.Code;
            url2 = "QDE_CountryViewMain.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code;
          }
          AddTab("Original", "Original item", "tb_ori", "/hc_v4/img/ed_content.gif", url1);
          AddTab("Roll", "Roll item", "tb_roll", "/hc_v4/img/ed_roll.gif", url2);

          webTab.SelectedTabIndex = 1; // roll is selected
        }
        else // item is original
        {
          // Add tabs (original and roll)
          url1 = "QDE_Forms.aspx?i=" + SessionState.CurrentItem.Id;
          url2 = "QDE_Forms.aspx?i=" + itemRoll.Id;
          if (SessionState.Culture.Type == CultureType.Locale && !SessionState.CurrentItem.IsCountrySpecific)
          {
            url1 = "QDE_CountryViewMain.aspx?i=" + SessionState.CurrentItem.Id + "&c=" + SessionState.Culture.Code;
            url2 = "QDE_CountryViewMain.aspx?i=" + itemRoll.Id + "&c=" + SessionState.Culture.Code;
          }
          AddTab("Original", "Original item", "tb_ori", "/hc_v4/img/ed_content.gif", url1);
          AddTab("Roll", "Roll item", "tb_roll", "/hc_v4/img/ed_roll.gif", url2);
          webTab.SelectedTabIndex = 0; // original is selected
        }
      }
      finally
      {
        if (localizedProductNameChunk!=null) localizedProductNameChunk.Dispose();
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

    private void DeleteItemRoll(Item itemRoll)
    {
      if (itemRoll != null)
      {
        using (Roll roll = itemRoll.ItemRoll)
        {
          if (roll.Delete(SessionState.User.Id))
          {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "UpdatePage", "<script>UpdatePage(" + SessionState.CurrentItem.Id + ");</script>");
          }
          else
          {
            lbError.CssClass = "hc_error";
            lbError.Text = Roll.LastError;
            lbError.Visible = true;
            ShowTabs();
            Page.DataBind();
          }
        }
      }
    }

    private void webTab_TabClick(object sender, Infragistics.WebUI.UltraWebTab.WebTabEvent e)
    {
      HyperCatalog.Business.Chunk localizedProductNameChunk = null;
      try
      {        
        bool isRoll = false;
        if (SessionState.CurrentItem != null)
        {
          if (e.Tab.Key == "tb_ori")
          {
            if (SessionState.Culture.Type != CultureType.Master)
              localizedProductNameChunk = HyperCatalog.Business.Chunk.GetByKey(SessionState.CurrentItem.Id, 1, SessionState.Culture.Code);

            // Update item name and level name
            if (localizedProductNameChunk == null)
              lbItemName.Text = SessionState.CurrentItem.FullName;
            else
            {
              if (SessionState.CurrentItem.Sku != null && SessionState.CurrentItem.Sku.Length > 0)
                lbItemName.Text = "[" + SessionState.CurrentItem.Sku + "] " + localizedProductNameChunk.Text;
              else
                lbItemName.Text = localizedProductNameChunk.Text;
            }

            itemLevelName = SessionState.CurrentItem.Level.Name;
          }
          else // tb_roll
          {
            using (Item rollItem = SessionState.CurrentItem.GetRoll())
            {
              if (rollItem != null)
              {
                isRoll = true;
                lbItemName.Text = rollItem.FullName;
                if (SessionState.Culture.Type != CultureType.Master)
                  localizedProductNameChunk = HyperCatalog.Business.Chunk.GetByKey(rollItem.Id, 1, SessionState.Culture.Code);

                // Update item name and level name
                if (localizedProductNameChunk == null)
                  lbItemName.Text = rollItem.FullName;
                else
                {
                  if (rollItem.Sku != null && rollItem.Sku.Length > 0)
                    lbItemName.Text = "[" + rollItem.Sku + "] - " + localizedProductNameChunk.Text;
                  else
                    lbItemName.Text = localizedProductNameChunk.Text;
                }

                itemLevelName = rollItem.Level.Name;
              }
            }
          }

          if (lbItemName.Text.Length > 50)
            lbItemName.Text = lbItemName.Text.Substring(0, 49) + "...";

          if (isRoll)
            lbItemName.Text = "<img src='/hc_v4/img/ed_roll.gif'> " + lbItemName.Text;
        }
      }
      finally
      {
        if (localizedProductNameChunk != null) localizedProductNameChunk.Dispose();
      }
    }
  }
}
