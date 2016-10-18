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
  public partial class qde_itemwhoswho : HCPage
  {
    #region Declarations
    private string itemId;
    private string cultureCode;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      itemId = QDEUtils.GetQueryItemIdFromRequest().ToString();
      cultureCode = QDEUtils.UpdateCultureCodeFromRequest().Code;
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      Item _item = Item.GetByKey(Convert.ToInt32(itemId));
      lItemName.Text = _item.Name;
      lItemLevel.Text = _item.Level.Name;

      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("dbo._Item_WhosWho", "Items",
          new SqlParameter("@ItemId", itemId),
          new SqlParameter("@CultureCode", cultureCode)))
        {
          dbObj.CloseConnection();

          if (dbObj.LastError == string.Empty)
          {
            Label title;
            DataTable dt;
            DataRow dr;
            int i;
            string curRole;
            bool canSeeRealEmails = SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.SEE_REAL_EMAIL);

            Infragistics.WebUI.Misc.WebPanel wp;
            string userEmail;
            int nbUsers;

            #region Creator
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
              #region title
              title = new Label();
              title.Text = "Creator";
              title.CssClass = "ptbgroup";
              title.Width = Unit.Percentage(100);
              panelUsers.Controls.Add(title);
              #endregion
              panelUsers.Controls.Add(new LiteralControl("<table width='100%' cellspacing='0' cellpadding='0' border='0'>"));
              for (i = 0; i < dt.Rows.Count; i++)
              {
                dr = dt.Rows[i];
                Label creator = new Label();
                creator.Controls.Add(new LiteralControl(Environment.NewLine + "<tr><td width='200'><a href='mailto:" + UITools.GetDisplayEmail(dr["eMail"].ToString()) + "'>" + dr["UserName"].ToString() + "</a></td><td align='right'>" + dr["Organization"].ToString() + "</td>"));
                panelUsers.Controls.Add(creator);
              }
              panelUsers.Controls.Add(new LiteralControl("</table>"));
              panelUsers.Controls.Add(new LiteralControl("<br/>"));
            }
            #endregion

            #region Modifier
            dt = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
              #region title
              title = new Label();
              title.Text = "Modifier";
              title.CssClass = "ptbgroup";
              title.Width = Unit.Percentage(100);
              panelUsers.Controls.Add(title);
              #endregion
              panelUsers.Controls.Add(new LiteralControl("<table width='100%' cellspacing='0' cellpadding='0' border='0'>"));
              for (i = 0; i < dt.Rows.Count; i++)
              {
                dr = dt.Rows[i];
                Label creator = new Label();
                creator.Controls.Add(new LiteralControl(Environment.NewLine + "<tr><td width='200'><a href='mailto:" + UITools.GetDisplayEmail(dr["eMail"].ToString()) + "'>" + dr["UserName"].ToString() + "</a></td><td align='right'>" + dr["Organization"].ToString() + "</td>"));
                panelUsers.Controls.Add(creator);
              }
              panelUsers.Controls.Add(new LiteralControl("</table>"));
              panelUsers.Controls.Add(new LiteralControl("<br/>"));
            }
            #endregion

            #region Item users list
            dt = ds.Tables[2];
            if (dt.Rows.Count > 0)
            {
              #region title
              title = new Label();
              title.Text = "Item Users list";
              title.CssClass = "ptbgroup";
              title.Width = Unit.Percentage(100);
              panelUsers.Controls.Add(title);
              #endregion
              curRole = string.Empty;
              nbUsers = 0;
              wp = null;
              for (i = 0; i < dt.Rows.Count; i++)
              {
                dr = dt.Rows[i];
                if (dr["RoleName"].ToString() != curRole)
                {
                  if (wp != null)
                  {
                    wp.Header.Text = curRole + " (" + nbUsers + ")";
                    wp.Controls.Add(new LiteralControl("</table>"));
                  }
                  wp = new Infragistics.WebUI.Misc.WebPanel();
                  curRole = dr["RoleName"].ToString();
                  wp.ID = "role" + i.ToString();
                  wp.Width = Unit.Percentage(100);
                  wp.ImageDirectory = "/hc_v4/img";
                  wp.Header.ExpandedAppearance.Style.CssClass = "ptb5"; // "hc_webpanelexp";
                  wp.Header.ExpansionIndicator.AlternateText = "Expand/Collapse";
                  wp.Header.ExpansionIndicator.CollapsedImageUrl = "ed_dt.gif";
                  wp.Header.ExpansionIndicator.ExpandedImageUrl = "ed_upt.gif";
                  wp.Header.CollapsedAppearance.Style.CssClass = "ptb5"; // "hc_webpanelcol";
                  wp.Header.TextAlignment = Infragistics.WebUI.Misc.TextAlignment.Left;
                  wp.Expanded = false;
                  panelUsers.Controls.Add(wp);
                  panelUsers.Controls.Add(new LiteralControl("<br/>"));
                  wp.Controls.Add(new LiteralControl("<table width='100%' cellspacing='0' cellpadding='0' border='0'>"));
                }
                nbUsers++;
                wp.Controls.Add(new LiteralControl(Environment.NewLine + "<tr><td width='200'><a href='mailto:" + UITools.GetDisplayEmail(dr["eMail"].ToString()) + "'>" + dr["UserName"].ToString() + "</a></td><td align='right'>" + dr["Organization"].ToString() + "</td>"));
              }
              wp.Controls.Add(new LiteralControl("</table>"));
              wp.Header.Text = curRole + " (" + nbUsers + ")";
            }
            #endregion

            #region Content contributors
            dt = ds.Tables[3];
            if (dt.Rows.Count > 0)
            {
              #region title
              title = new Label();
              title.Text = "Content contributors";
              title.CssClass = "ptbgroup";
              title.Width = Unit.Percentage(100);
              panelUsers.Controls.Add(title);
              #endregion
              curRole = string.Empty;
              nbUsers = 0;
              wp = null;
              for (i = 0; i < dt.Rows.Count; i++)
              {
                dr = dt.Rows[i];
                if (dr["RoleName"].ToString() != curRole)
                {
                  if (wp != null)
                  {
                    wp.Header.Text = curRole + " (" + nbUsers + ")";
                    wp.Controls.Add(new LiteralControl("</table>"));
                  }
                  wp = new Infragistics.WebUI.Misc.WebPanel();
                  curRole = dr["RoleName"].ToString();
                  wp.ID = "contributorsrole" + i.ToString();
                  wp.Width = Unit.Percentage(100);
                  wp.ImageDirectory = "/hc_v4/img";
                  wp.Header.ExpandedAppearance.Style.CssClass = "ptb5"; // "hc_webpanelexp";
                  wp.Header.ExpansionIndicator.AlternateText = "Expand/Collapse";
                  wp.Header.ExpansionIndicator.CollapsedImageUrl = "ed_dt.gif";
                  wp.Header.ExpansionIndicator.ExpandedImageUrl = "ed_upt.gif";
                  wp.Header.CollapsedAppearance.Style.CssClass = "ptb5"; // "hc_webpanelcol";
                  wp.Header.TextAlignment = Infragistics.WebUI.Misc.TextAlignment.Left;
                  wp.Expanded = true;
                  panelUsers.Controls.Add(wp);
                  panelUsers.Controls.Add(new LiteralControl("<br/>"));
                  wp.Controls.Add(new LiteralControl("<table width='100%' cellspacing='0' cellpadding='0' border='0'>"));
                }
                nbUsers++;
                wp.Controls.Add(new LiteralControl(Environment.NewLine + "<tr><td width='200'><a href='mailto:" + UITools.GetDisplayEmail(dr["eMail"].ToString()) + "'>" +
                  dr["UserName"].ToString() + "</a></td><td><a href='javascript://' title='Click here to show the full list' onclick=\"OpenDetail(" + itemId + ",'" + cultureCode + "'," + dr["UserId"].ToString() + ");return false;\">" +
                  dr["ChunkCount"].ToString() + (Convert.ToInt32(dr["ChunkCount"]) > 1 ? " chunks" : " chunk") +
                  "</a></td><td align='right'>" + dr["Organization"].ToString() + "</td>"));
              }
              wp.Controls.Add(new LiteralControl("</table>"));
              wp.Header.Text = curRole + " (" + nbUsers + ")";
            }
            #endregion
          }
          else
          {
            lbError.Text = "Error: " + dbObj.LastError.ToString();
            lbError.CssClass = "hc_error";
            lbError.Visible = true;
          }
        }
      }
    }
  }
}
