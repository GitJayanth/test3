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
using System.Web.SessionState;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

public partial class UI_Admin_InputForms_Inputforms_productlines : HCPage
{

  #region Declarations
  private HyperCatalog.Business.InputForm ifObj = null;
  private System.Int32 ifId;
  #endregion

  protected void Page_Load(object sender, EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("ApplyChanges").Enabled = false;
    }

    if (!SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.MANAGE_CARTOGRAPHY))
    {
      UITools.HideToolBarButton(uwToolbar, "ApplyChanges");
    }
    #endregion

    ifId = Convert.ToInt32(Request["i"]);

    if (!Page.IsPostBack)
    {
      ifObj = HyperCatalog.Business.InputForm.GetByKey(ifId);
    }
  }

  protected override void OnLoadComplete(EventArgs e)
  {
    base.OnLoadComplete(e);
    if (ifObj != null)
    {
      Trace.Warn("Input Forms -> Check PLs (" + ifObj.PLs.Count.ToString() + " items)");
      PLTree.CheckPLs(ifObj.PLs);
    }
  }
  protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
  {
    string btn = be.Button.Key.ToLower();
    Trace.Warn("uwToolbar_ButtonClicked with btn = " + btn);
    if (btn == "applychanges")
    {
      ApplyChanges();
    }

  }

  // Save the new or updated Input form
  private void ApplyChanges()
  {
    ifObj = HyperCatalog.Business.InputForm.GetByKey(ifId);
    ifObj.PLs.Clear();
    Trace.Warn("Calling PLTree.GetCheckedPLs()");
    PLList checkedPLs = PLTree.GetCheckedPLs();
    foreach (PL pl in checkedPLs)
    {
      ifObj.PLs.Add(pl);
    }
    if (!ifObj.Save(true))
    {
      lbError.CssClass = "hc_error";
      lbError.Text = HyperCatalog.Business.InputForm.LastError;
      lbError.Visible = true;
    }
    else
    {
      Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "RefreshTabs", "<script>RefreshTabs('PLs', " + ifObj.PLs.Count.ToString() + ", 'Usage', " + ifObj.Items.Count.ToString() + ");</script>");
      lbError.Text = "Data saved!";
      lbError.CssClass = "hc_success";
      lbError.Visible = true;
    }
  }
}
