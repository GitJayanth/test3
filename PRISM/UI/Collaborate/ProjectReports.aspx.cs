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

public partial class UI_Globalize_ProjectReports : HCPage
{
  protected void Page_Load(object sender, EventArgs e)
  {
      CultureList dsCultures = SessionState.User.Cultures;
      CollectionView Master = new CollectionView(dsCultures);
      Master.ApplyFilter("Type", CultureType.Master, CollectionView.FilterOperand.Equals);
      if (Master.Count > 0)
      {
        webTab.Tabs.FromKey("EOM").Visible = true;
      }
      else
      {
        webTab.Tabs.FromKey("EOM").Visible = false;
      }
      CollectionView Region = new CollectionView(dsCultures);
      Region.ApplyFilter("Type", CultureType.Regionale, CollectionView.FilterOperand.Equals);
      if (Region.Count > 0)
      {
        webTab.Tabs.FromKey("EOV").Visible = true;
      }
      else
      {
        webTab.Tabs.FromKey("EOV").Visible = false;
      }
      if ((Master.Count == 0) && (Region.Count == 0))
      {
        UITools.DenyAccess(DenyMode.Standard);
      }
  }
}
