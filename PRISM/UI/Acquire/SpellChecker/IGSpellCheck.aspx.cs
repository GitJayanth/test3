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

public partial class UI_Acquire_SpellChecker_IGSpellCheck : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    aj_id.Value = Request["i"].ToString();
    aj_cid.Value = Request["c"].ToString();
    aj_uid.Value = HyperCatalog.Shared.SessionState.User.Id.ToString();
    aj_un.Value = HyperCatalog.Shared.SessionState.User.FullName.ToString();

    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "InitVarName",
          "<script>var aj_id = '" + aj_id.ClientID + "';" + Environment.NewLine +
          "var aj_cid = '" + aj_cid.ClientID + "';" + Environment.NewLine +
          "var aj_uid = '" + aj_uid.ClientID + "';" + Environment.NewLine +
          "var aj_un = '" + aj_un.ClientID + "';" + Environment.NewLine +
            "</script>");
  }
}
