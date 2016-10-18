using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Description résumée de QDE_Chunk.
  /// </summary>
  public partial class QDE_Chunk : HCPage
  {
    protected System.Web.UI.WebControls.Label lbJs;
    System.Int64 itemId;
    Culture cul;
    string cultureCode;
    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        if (Request["g"] != null && Request["r"] != null) // Grid name and Row index
        {
          string curPos = "";
          string moveNext = "false";
          itemId = QDEUtils.GetQueryItemIdFromRequest();
          cultureCode = QDEUtils.GetQueryCultureCodeFromRequest();          
          if (SessionState.Culture == null)
          {
            SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(cultureCode);
          }
          if (cultureCode != SessionState.Culture.Code)
          {
            SessionState.Culture = cul = HyperCatalog.Business.Culture.GetByKey(cultureCode);            
          }
          else
          {
            cul = SessionState.Culture;
          }

          if (Page.IsPostBack)
          {
            if (Request["curpos"] != null && Request["curpos"].ToString() != string.Empty)
            {
              curPos = Request["curpos"].ToString();
              moveNext = SessionState.User.GetOptionById((int)OptionsEnum.OPT_CHUNK_MOVENEXT).Value.ToString().ToLower();
            }
          }
          else
          {
            curPos = Request["r"].ToString();
          }
          string strScript = "<script>movenext=" + moveNext + ";gridName = '" + Request["g"].ToString() + "';rowIndex=" + curPos + ";countryCode='" + cul.CountryCode + "';cultureName='" + cul.Name + "';culCode='" + cul.Code + "';culId=" + HyperCatalog.Business.Culture.GetCultureTypeFromEnum(cul.Type).ToString() + ";iId=" + itemId.ToString() + ";</script>";
          Page.ClientScript.RegisterStartupScript(this.GetType(), "InitVars", strScript);
        }
        else
        {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Exit", "<script>window.close();</script>");
        }
      }
      catch (Exception ex)
      {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('" + ex.ToString().Replace(Environment.NewLine, "") + "');top.window.close();</script>");
      }
    }
  }
}
