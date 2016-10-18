#region History
//added for Merge of 8.5.01 with 9.0 - Mahesh - 10-July-2009.
#endregion History

#region Uses
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebTab;
#endregion

public partial class UI_Acquire_QDE_qde_countryviewain : System.Web.UI.Page
{
  #region declarations
  private Item item;
  private int levelId;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    using (item = QDEUtils.GetItemIdFromRequest())
    {
      levelId = item.LevelId;
      QDEUtils.UpdateCultureCodeFromRequest();
      #region retrieve translated product name
      string itemName = item.Name;
      if ((bool)SessionState.User.GetOptionById((int)OptionsEnum.OPT_SHOW_TRANSLATED_NAMES).Value)
      {
        using (HyperCatalog.Business.Chunk localizedItemNameChunk = HyperCatalog.Business.Chunk.GetByKey(item.Id, 1, SessionState.Culture.Code))
        {
          if (localizedItemNameChunk != null)
          {
            itemName = localizedItemNameChunk.Text;
          }
        }
      }
      #endregion
      if (item.Sku != string.Empty)
      {
        lTitle.Text = "[" + item.Sku + "] " + itemName;
      }
      else { lTitle.Text = itemName; }
      if (item.IsRoll)
      {
        lTitle.Text = "<img src='/hc_v4/img/ed_roll.gif' align='middle' title='Soft roll' border=0/> " + lTitle.Text;
      }
      lLevel.Text = item.Level.Name;
     
      //Modification for the Enhancement BUG# 70135


      for (int i = 0; i < webTab.Tabs.Count; i++)
      {
          string strTargetUrl = ((Tab)webTab.Tabs[i]).ContentPane.TargetUrl;
          
          if (strTargetUrl == "./qde_countryview.aspx?view=content")
          {

              ((Tab)webTab.Tabs[i]).ContentPane.TargetUrl += "&i=" + item.Id + "&f=IF_-1" + "&c=" + SessionState.Culture.Code;
          }//added for Merge of 8.5.01 with 9.0
          else if (strTargetUrl == "./qde_PDBView.aspx")
          {
              ((Tab)webTab.Tabs[i]).ContentPane.TargetUrl = ((Tab)webTab.Tabs[i]).ContentPane.TargetUrl;
          }//added for Merge of 8.5.01 with 9.0
          else
          {

              ((Tab)webTab.Tabs[i]).ContentPane.TargetUrl += "&i=" + item.Id + "&c=" + SessionState.Culture.Code;

          }

      }

      //Modification for the Enhancement 

    }
    string linksStartLevel = HyperCatalog.Business.ApplicationSettings.Parameters["LinksStartLevel"].Value.ToString();

      //Kalai code start here
    //if (item.Level.Id < Convert.ToInt32(linksStartLevel))
    //{
    //  webTab.Tabs.FromKey("links").Visible = webTab.Tabs.FromKey("cross").Visible = false;
    //}
    //else
    //{
    //  webTab.Tabs.FromKey("links").Visible = webTab.Tabs.FromKey("cross").Visible = true;
    //}
      //Kalai code ends here
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
