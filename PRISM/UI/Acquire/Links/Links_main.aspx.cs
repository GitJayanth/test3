#region uses
using System;
using System.Configuration;
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
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
#endregion

/// <summary>
/// Main page for links
/// </summary>
public partial class links_main : HCPage
{
  protected void Page_Load(object sender, System.EventArgs e)
  {
    if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_LINKS))
    {
      QDEUtils.GetItemIdFromRequest();
      QDEUtils.UpdateCultureCodeFromRequest();
      if (!Page.IsPostBack)
      {
        InitTabs();
        SessionState.QDETab = "tb_lnk";
      }
    }
    else
    {
      UITools.DenyAccess(DenyMode.Frame);
    }
  }

  /// <summary>
  /// Retrieve all available links types in the database to build the form
  /// </summary>
  private void InitTabs()
  {
    lbError.Visible = false;

    Infragistics.WebUI.UltraWebTab.Tab newTab;
    webTab.Tabs.Clear();

    bool withLinkCount = false;
    if (SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT) != null
      && SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_LINK_COUNT).Value)
    {
      withLinkCount = true;
    }

    using (DataSet ds = SessionState.CurrentItem.GetLinkTypesCount(SessionState.Culture.Code, -1, withLinkCount))
    {
      if (Item.LastError.Length == 0)
      {
        if (ds != null && ds.Tables.Count > 0)
        {
          foreach (DataRow dr in ds.Tables[0].Rows)
          {
            string linkTypeId = dr["LinkTypeId"].ToString();
            string linkTypeName = dr["LinkTypeName"].ToString();
            string nbLinks = dr["NbLinks"].ToString();
            string linkFrom = dr["LinkFrom"].ToString();
            if (linkFrom.Equals("True"))
              linkFrom = "1";
            else
              linkFrom = "0";

            string icon = dr["Icon"].ToString();
            // Update title of the new tab
            string tabName = linkTypeName;
            if (withLinkCount)
            {
              tabName = tabName + " (" + nbLinks + ")";
            }
            if (SessionState.Culture.Type == CultureType.Master && linkTypeName == "CrossSell") //Added if else loop to disable CrossSell in Master - Prabhu - 8 Oct 07
            {
            }
            else
            {
                // Create new tab for this links
                newTab = new Infragistics.WebUI.UltraWebTab.Tab(tabName);
                // Update key of the new tab
                newTab.Key = "tb_" + linkTypeId;
                // Update image of the new tab
                if (icon.Length > 0)
                    newTab.DefaultImage = "/hc_v4/img/" + icon;
                // Update URL of the new tab
                newTab.ContentPane.TargetUrl = "Links_list.aspx?i=" + SessionState.CurrentItem.Id.ToString() + "&t=" + linkTypeId + "&f=" + linkFrom + "&c=" + SessionState.Culture.Code;
                // Add the new tab
                webTab.Tabs.Add(newTab);
            }
          }
        }

        // Tab name
        string tabContentName = "All links";
        if (withLinkCount)
        {
          tabContentName = tabContentName + " (" + Link.GetLinksCount(SessionState.CurrentItem.Id, SessionState.Culture.Code) + ")";
        }
        // Add Content tab (contains all links applicable at this node)
        newTab = new Infragistics.WebUI.UltraWebTab.Tab(tabContentName);
        // Update image of the new tab
        newTab.DefaultImage = "/hc_v4/img/ed_links.gif";
        // Update key of the new tab
        newTab.Key = "tb_Content";
        // Update URL of the new tab
        newTab.ContentPane.TargetUrl = "Links_content.aspx?i=" + SessionState.CurrentItem.Id.ToString();
        // Add the new tab
        webTab.Tabs.Add(newTab);

        webTab.Visible = true;
      }
    }
  }
}
