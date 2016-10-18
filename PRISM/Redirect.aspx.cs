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
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

/// <summary>
/// Redirect to a page
/// </summary>
public partial class Redirect : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
    string p = string.Empty;
    if (Request["c"] != null)
    {
      if (SessionState.Culture.Code != Request["c"].ToString())
      {
        SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(Request["c"]);
      }
    }
    if (Request["i"] != null)
    {
      SessionState.User.LastVisitedItem = SessionState.User.LastVisitedItemReadOnly = Convert.ToInt64(Request["i"]);
      ItemStatus status = ItemStatus.Unknown;
      bool isEligible = Item.IsEligible(SessionState.User.LastVisitedItem, SessionState.Culture.CountryCode, ref status);
        //Nisha Verma commented the code for show obsolete to save properly
      //SessionState.User.ViewObsoletes = status == ItemStatus.Obsolete;
      SessionState.User.QuickSave();
    }
    if (Request["tr"] != null)
    {
      p = p + "tr=" + Request["tr"] + "&";
    }
    if (Request["p"] != null)
    {
      string redirectUrl = Server.UrlDecode(Request["p"]);
      if (p != string.Empty)
      {
        if (redirectUrl.IndexOf("?") > 0)
        {
          redirectUrl = redirectUrl + "&" + p;
        }
        else
        {
          redirectUrl = redirectUrl + "?" + p;
        }
      }
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", "<script>window.focus();window.location = '" + redirectUrl + "'; </script>");
    }
  }
}
