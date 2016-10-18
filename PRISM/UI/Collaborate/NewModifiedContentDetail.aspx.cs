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
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using System.Data.SqlClient;
using HyperCatalog.Shared;
#endregion

public partial class UI_Collaborate_NewModifiedContentDetail : HCPage
{
  #region Declarations
  private Int64 itemId;
  private Culture cul;
  private string frequency;
  private string filter;
  #endregion

  private int TIME_OUT = 600;

  protected void Page_Load(object sender, EventArgs e)
  {
    if ((Request["i"] != null) && (Request["c"] != null) && (Request["f"] != null) && (Request["p"] != null))
    {
      itemId = Convert.ToInt64(Request["i"]);
      using (cul = HyperCatalog.Business.Culture.GetByKey(Request["c"].ToString()))
      {
        frequency = Request["p"].ToString();
        filter = Request["f"].ToString();

        if (cul != null)
        {
          uwToolbarTitle.Items.FromKeyLabel("Culture").Image = "/hc_v4/img/flags/" + cul.CountryCode + ".gif";
          uwToolbarTitle.Items.FromKeyLabel("Culture").Text = cul.Name + "&nbsp;";
        }
        using (HyperCatalog.Business.Item item = HyperCatalog.Business.Item.GetByKey(itemId))
        {
          uwToolbarTitle.Items.FromKeyLabel("ItemName").Text = item.Name;
        }

        if (!Page.IsPostBack)
        {
          DisplayData();
        }
      }
    }
    else
    {
      UITools.DenyAccess(DenyMode.Popup);
      return;
    }
    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "InitVars", "<script>itemId='" + itemId + "';cultureCode='" + cul.Code + "';</script>");
  }

  protected void DisplayData()
  {
    using (Database dbObj = Utils.GetMainDB())
    {
        string selPLs = Session["SelPls"] + "";
      dbObj.TimeOut = TIME_OUT;
      using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Report_ContentAddUpd",
        new SqlParameter("@ItemId", itemId.ToString()),
        new SqlParameter("@Frequency", frequency),
        new SqlParameter("@CultureCode", cul.Code),
        new SqlParameter("@Filter", filter),
       new SqlParameter("@UserId", SessionState.User.Id.ToString()),
          /**** For PL selection ****/
          new SqlParameter("@PLCODE",selPLs+"")
        ))

      {
        dbObj.CloseConnection();
        if (dbObj.LastError != string.Empty)
        {
          lError.Text = "[ERROR] _Report_ContentAddUpd -> " + dbObj.LastError;
          lError.Visible = true;
        }
        else
        {
          lContent.ForeColor = lLinks.ForeColor = lMarketSegments.ForeColor = lPublishers.ForeColor = System.Drawing.Color.Orange;
          lContent.Font.Bold = lLinks.Font.Bold = lMarketSegments.Font.Bold = lPublishers.Font.Bold = true;
          #region Results Content
          if (ds.Tables[0].Rows.Count > 0)
          {
            dgContent.DataSource = ds.Tables[0].DefaultView;
            dgContent.DataBind();
            Utils.InitGridSort(ref dgContent, false);
            lError.Visible = false;
            dgContent.Visible = true;
            dgContent.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate;
          }
          #endregion
          #region No result
          else
          {
            dgContent.Visible = lContent.Visible = false;
          }
          #endregion
          #region Results Links
          if (ds.Tables[1].Rows.Count > 0)
          {
            dgLinks.DataSource = ds.Tables[1].DefaultView;
            dgLinks.DataBind();
            Utils.InitGridSort(ref dgLinks, false);
            lError.Visible = false;
            dgLinks.Visible = true;
            dgLinks.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate;
          }
          #endregion
          #region No result
          else
          {
            dgLinks.Visible = lLinks.Visible = false;
          }
          #endregion
          #region Results Market segments
          if (ds.Tables[2].Rows.Count > 0)
          {
            dgMarketSegments.DataSource = ds.Tables[2].DefaultView;
            dgMarketSegments.DataBind();
            Utils.InitGridSort(ref dgMarketSegments, false);
            lError.Visible = false;
            dgMarketSegments.Visible = lMarketSegments.Visible = true;
            dgMarketSegments.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate;
          }
          #endregion
          #region No result
          else
          {
            dgMarketSegments.Visible = lMarketSegments.Visible = false;
          }
          #endregion
          #region Results Publishers
          if (ds.Tables[3].Rows.Count > 0)
          {
            dgPublishers.DataSource = ds.Tables[3].DefaultView;
            dgPublishers.DataBind();
            Utils.InitGridSort(ref dgPublishers, false);
            lError.Visible = false;
            dgPublishers.Visible = lPublishers.Visible = true;
            dgPublishers.Columns.FromKey("ModifyDate").Format = SessionState.User.FormatDate;
          }
          #endregion
          #region No result
          else
          {
            dgPublishers.Visible = lPublishers.Visible = false;
          }
          #endregion
          if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0
           && ds.Tables[2].Rows.Count == 0 && ds.Tables[3].Rows.Count == 0)
          {
            lError.Text = "No result found";
            lError.Visible = true;
          }
        }
      }
    }
  }


  protected void dgResults_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    e.Row.Cells.FromKey("ContainerName").Text = "<a href='javascript://' onclick=\"return OpenChunkWindow('" + e.Row.Cells.FromKey("ContainerId").Text + "')\">" + e.Row.Cells.FromKey("ContainerName").Text + "</a>";
    Infragistics.WebUI.UltraWebGrid.UltraGridCell newValue = e.Row.Cells.FromKey("NewValue");
    Infragistics.WebUI.UltraWebGrid.UltraGridCell oldValue = e.Row.Cells.FromKey("OldValue");
    if (newValue.Text == HyperCatalog.Business.Chunk.BlankValue)
    {
      newValue.Text = HyperCatalog.Business.Chunk.BlankText;
      newValue.Style.ForeColor = System.Drawing.Color.Gray;
    }
    if (oldValue.Text == HyperCatalog.Business.Chunk.BlankValue)
    {
      oldValue.Text = HyperCatalog.Business.Chunk.BlankText;
      oldValue.Style.ForeColor = System.Drawing.Color.Gray;
    }
  }

}
