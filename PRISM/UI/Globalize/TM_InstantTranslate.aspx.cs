#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperCatalog.Business;
using HyperCatalog.Globalization;
using HyperCatalog.UI.Acquire.QDE;
using HyperComponents.Data.dbAccess;
#endregion

namespace HyperCatalog.UI.Main.Globalize
{
	/// <summary>
	/// Description résumée de TM_InstantTranslate.
	/// </summary>
  public partial class TM_InstantTranslate : HCPage
  {
      #region "Declaration"
      protected System.Int64 itemId;
      private HyperCatalog.Business.Item item;
      private string cultureCode = string.Empty;
      private string containerList = string.Empty;
      private string warningMessage = string.Empty;
      private int threshholdLimit;
      #endregion
      

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      base.OnInit(e);
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.Ultrawebtoolbar1.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.Ultrawebtoolbar1_ButtonClicked);
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      //TODO: Bug sur la procedure Stockee
      #region check security
      if (!SessionState.User.HasCapability(CapabilitiesEnum.INSTANT_TRANSLATE))
      {
        UITools.DenyAccess(DenyMode.Popup);
      }
      #endregion
      else
      {
        using (item = QDEUtils.GetItemIdFromRequest())
        {
          itemId = item.Id;
          cultureCode = Request["c"].ToString();
          containerList = Request["con"].ToString();
          if (!Page.IsPostBack)
          {
            uwToolbar.Items.FromKeyLabel("ItemName").Text = item.FullName + " - TinyTM on demand";
            DoTranslation();
            Page.DataBind();
          }
        }
      }
    }

    private void DoTranslation()
    {
      lbError.Visible = dg.Visible = false;
      threshholdLimit = Convert.ToInt32(ApplicationSettings.Parameters["AutoTR_UIThresholdLimit"].Value.ToString());
      int TotalTRChunksCount;
      dgContainer.Visible = false;
        //QC 1646 : Cannot Auto-translate -> The time-out parameter is extended to 1 hr
      using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 3600)) 
      {
          //Modified by Prabhu for CR 5160 & ACQ 8.12 (PCF1: Auto TR Redesign)-- 21/May/09
          using (DataSet ds = dbObj.RunSPReturnDataSet("_TM_TranslateItem", new SqlParameter("@ItemId", itemId), new SqlParameter("@ContainerList", containerList),
                                            new SqlParameter("@RegionCode", cultureCode), new SqlParameter("@UserId", SessionState.User.Id)))
          {
              dbObj.CloseConnection();
              if (dbObj.LastError != string.Empty)
              {
                  lbError.Text = dbObj.LastError;
                  lbError.Visible = true;
                  dg.Visible = false;
                  dgContainer.Visible = false;
                  UITools.HideToolBarSeparator(Ultrawebtoolbar1, "CloseSep");
                  UITools.HideToolBarButton(Ultrawebtoolbar1, "Export");
              }
              else if (ds.Tables.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
              {
                  TotalTRChunksCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                  if (TotalTRChunksCount > threshholdLimit)
                  {
                      warningMessage = "<font color='red' size='2'><b>Error:</b> Selected containers cannot be auto translated.</font><br><br>" +
                      "Translatable Chunk Count: " + TotalTRChunksCount + "<br>" +
                      "IT Threshold Limit: " + threshholdLimit + "<br><br>" +
                      "Total number of chunks to be auto translated is more than IT recommended threshold limit. <br>" +
                      "Requesting user to uncheck few containers and try again.";

                      lblWarningMessage.Text = warningMessage;
                      lblWarningMessage.Visible = true;
                      dg.Visible = false;
                      dgContainer.Visible = true;
                      dgContainer.DataSource = ds.Tables[1];
                      Utils.InitGridSort(ref dgContainer);
                      dgContainer.DataBind();
                      UITools.HideToolBarSeparator(Ultrawebtoolbar1, "CloseSep");
                      UITools.HideToolBarButton(Ultrawebtoolbar1, "Export");
                  }
                  else
                  {
                      dg.Visible = true;
                      dgContainer.Visible = false;
                      dg.DataSource = ds.Tables[1];
                      ds.Tables.Remove(ds.Tables[0]);
                      ViewState["ds"] = ds;
                      Utils.InitGridSort(ref dg);
                      dg.DataBind();
                      //reset checkbox on success
                      Page.ClientScript.RegisterStartupScript(Page.GetType(), "ResetParentCheckBox", "ResetParentCheckBox();", true);
                  }
              }
              else
              {
                  lbError.Visible = true;
                  lbError.Text = "No content was eligible for Auto Translation.";
                  dg.Visible = false;
                  dgContainer.Visible = false;
                  UITools.HideToolBarSeparator(Ultrawebtoolbar1, "CloseSep");
                  UITools.HideToolBarButton(Ultrawebtoolbar1, "Export");
                  Page.ClientScript.RegisterStartupScript(Page.GetType(), "ResetParentCheckBox", "ResetParentCheckBox();", true);
              }
          }
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      if (Convert.ToInt32(e.Row.Cells.FromKey("Created").Value) > 0)
      {
        e.Row.Cells.FromKey("Created").Style.CssClass = "hc_success";
        e.Row.Cells.FromKey("Created").Style.Font.Bold = true;
      }
      if (Convert.ToInt32(e.Row.Cells.FromKey("Updated").Value) == 0)
      {
        e.Row.Cells.FromKey("Updated").Style.CssClass = "hc_success";
        e.Row.Cells.FromKey("Updated").Style.Font.Bold = true;
      }
    }

    private void Ultrawebtoolbar1_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        if (ViewState["ds"] != null)
        {
          Tools.Export.ExportInstantTinyTMReport((DataSet)ViewState["ds"], item.FullName.Replace("/", "-").Replace(@"\", "-"), this.Page);
        }
      }
    }
  }
}
