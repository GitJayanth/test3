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
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
  public partial class qde_itemwhoswhodetail : HCPage
  {
    #region Declarations
    private string itemId;
    private string cultureCode;
    private string userId;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      if (Request["u"] != null)
      {
        userId = Request["u"].ToString();
      }
      else { userId = SessionState.User.Id.ToString(); }
      itemId = QDEUtils.GetQueryItemIdFromRequest().ToString();
      cultureCode = QDEUtils.UpdateCultureCodeFromRequest().Code;
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      bool canSeeRealEmails = SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.SEE_REAL_EMAIL);
      using (Item _item = QDEUtils.GetItemIdFromRequest())
      {
        lItemName.Text = _item.Name;
        lItemLevel.Text = _item.Level.Name;

        using (HyperCatalog.Business.User _user = HyperCatalog.Business.User.GetByKey(Convert.ToInt32(userId)))
        {

          using (Database dbObj = Utils.GetMainDB())
          {
            using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_WhosWhoDetail", "Chunks",
              new SqlParameter("@ItemId", itemId),
              new SqlParameter("@CultureCode", cultureCode),
              new SqlParameter("@UserId", userId)))
            {
              dbObj.CloseConnection();

              if (dbObj.LastError == string.Empty)
              {
                string count = ds.Tables[0].Rows.Count.ToString();
                dg.DataSource = ds.Tables[0];
                dg.DataBind();
                lUserName.Text = "<a href='mailto:" + UITools.GetDisplayEmail(_user.Email.ToString()) + "'>" + _user.FullName +
                  "</a> (" + count + (Convert.ToInt32(count) > 1 ? " chunks)" : " chunk)");
              }
              else
              {
                dg.Visible = false;
                lbError.Text = "Error: " + dbObj.LastError.ToString();
                lbError.CssClass = "hc_error";
                lbError.Visible = true;
              }
            }
          }
        }
      }
    }
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      //Display Status logo
      if (e.Row.Cells.FromKey("Status").Value != null)
      {
        e.Row.Cells.FromKey("Status").Style.CssClass = "S" + e.Row.Cells.FromKey("Status");
        e.Row.Cells.FromKey("Status").Value = string.Empty;
      }
    }
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      switch (btn)
      {
        case "export":
          {
            Utils.ExportToExcel(dg, "ChunksList", "ChunksList");
            break;
          }
      }

    }
}
}
