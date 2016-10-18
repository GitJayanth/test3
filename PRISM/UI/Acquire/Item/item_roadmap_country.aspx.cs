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

namespace HyperCatalog.UI.ItemManagement
{
  public partial class Item_Roadmap_Country : HCPage
  {
    #region Declarations
    private string countryCode = string.Empty;
    private Int64 itemId = -1;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      if (Request["i"] != null)
        itemId = Convert.ToInt64(Request["i"]);
      if (Request["c"] != null)
        countryCode = Request["c"].ToString();

      if (!IsPostBack)
      {
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      vGraphic.StatusList = "U,L,O,F,E";
      vGraphic.WorkflowStatus = "U,R,C,E";
      vGraphic.Countries = countryCode;
      vGraphic.ItemId = itemId;
      vGraphic.FilterLiveDate = "any";
      vGraphic.FilterObsoleteDate = "any";
      vGraphic.Scope = true;

      vGraphic.Refresh();
    }
  }
}
