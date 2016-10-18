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
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
	/// <summary>
	/// Description résumée de QDE_ItemScope.
	/// </summary>
	public partial class QDE_ItemScope : HCPage
  {
    #region Declarations
    private string _Script = string.Empty;
    private System.Int64 itemId;
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
		{
      if ((!SessionState.User.HasCapability(CapabilitiesEnum.LOCALIZE_PRODUCTS)
          && !SessionState.User.HasCapability(CapabilitiesEnum.LOCALIZE_CHUNKS)
          && !SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_FINAL_CHUNKS_MARKET_SEGMENT)
          && !SessionState.User.HasCapability(CapabilitiesEnum.EDIT_DELETE_DRAFT_CHUNKS))
        || (!SessionState.User.HasCultureInScope(SessionState.Culture.Code))
        || (SessionState.User.Items.Count == 0)
        || (SessionState.User.IsReadOnly))
      {
         webTab.Tabs.FromKey("My").Visible = false;
      }

      if (Request["i"] != null && !Page.IsPostBack)
      {
        QDEUtils.GetItemIdFromRequest();
        if (!SessionState.TVAllItems)
        {
          GoTab("my");
        }
        else
        {
          GoTab("all");
        }
      }
      else
      {
        if (Request["all"] != null && !Page.IsPostBack)
        {
          webTab.SelectedTabIndex = 1;
        }
        else
        {
          if (!webTab.Tabs.FromKey("My").Visible)
          {
            GoTab("all");
          }
        }
      }
		}

    protected void webTab_TabClick(object sender, Infragistics.WebUI.UltraWebTab.WebTabEvent e)
    {
      string tab = e.Tab.Key.ToLower();
      GoTab(tab);
    }
    private void GoTab(string tab)
    {
      if (!webTab.Tabs.FromKey("My").Visible && tab == "my")
      {
        SessionState.TVAllItems = true;
        GoTab("all");
        return;
      }
      else
      {
        SessionState.TVAllItems = false;
      }
     
      _Script = "<script>GO('" + webTab.ClientID +"'"; // Options
      if (tab != "options")
      {
        SessionState.TVAllItems = tab == "all";
        _Script = "<script>GI('" + webTab.ClientID +"', ";
        if (SessionState.TVAllItems)
        {
          _Script += "1, " + SessionState.User.LastVisitedItemReadOnly.ToString();
        }
        else
        {
          _Script += "0, " + SessionState.User.LastVisitedItem.ToString();
        }
      }
      _Script += ");</script>";
      lbScript.Text = _Script;      
    }
	}
}
