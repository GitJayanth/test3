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

using System.IO;
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using Infragistics.WebUI.UltraWebToolbar;
using Infragistics.WebUI.WebSchedule;

using HyperCatalog.Business.DAM;
using HyperCatalog.Shared;

namespace HyperCatalog.UI.DAM
{
  /// <summary>
  /// Description résumée de DAMImages.
  /// </summary>
  public partial class DAMImages : HCPage
  {

    protected void Page_Load(object sender, EventArgs e)
    {
      webTab.Visible = SessionState.User.HasCapability(Business.CapabilitiesEnum.MANAGE_RESOURCES);
      // if user doesn't have the capability to manage the resources
      // just display the resource list for download
      resourceList.Visible = !webTab.Visible;
    }
}

}
