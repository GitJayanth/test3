#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.ItemManagement
{
  /// <summary>
  /// Description résumée de item_CompareWith.
  /// </summary>
  public partial class item_CompareWith : HCPage
  {
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
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
      this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

    }
    #endregion

    private string itemId;
    private string cultureCode;

    protected void Page_Load(object sender, System.EventArgs e)
    {

      try
      {
        if (Request["i"] != null)
          itemId = Request["i"].ToString();
        cultureCode = QDEUtils.UpdateCultureCodeFromRequest().Code;
        //panelSku.Visible = HyperCatalog.Business.Item.GetByKey(Convert.ToInt64(itemId)).Level.SkuLevel;

      }
      catch
      {
        UITools.DenyAccess(DenyMode.Popup);
      }
      if (!Page.IsPostBack)
      {
          //Method to Populate ContainterType Values from ContainterType Table of Crystal DB to Containter Type Combo.
          Containerload();
          //Method to Compare Sku/Class
          Compare();
      }
    }

      private DataSet Containerload()
      {
          using (Database dbObj = Utils.GetMainDB())
          {
              using (DataSet ds1 = dbObj.RunSPReturnDataSet("_ContainerType_Load "))
              {
                  if (ds1.Tables[0].Rows.Count > 0)
                  {
                      ddlContainertype.DataSource = ds1.Tables[0].DefaultView;
                      ddlContainertype.DataTextField = "ContainerType";
                      ddlContainertype.DataValueField = "ContainerTypeCode";
                      ddlContainertype.DataBind();
                      ddlContainertype.Visible = true;
                  }
                  return ds1;
                  dbObj.CloseConnection();
                  dbObj.Dispose();
              }
          }
      }

    private DataSet Compare()
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        int maxCompareLevel = Convert.ToInt32(SessionState.CacheParams["Item_CompareMaxLevel"].Value);
        using (DataSet ds = dbObj.RunSPReturnDataSet("_Item_CompareGetComparableItems", "Items",
          new SqlParameter("@ItemId", itemId),
          new SqlParameter("@CultureCode", cultureCode),
          new SqlParameter("@RetrieveObsoletes", SessionState.User.ViewObsoletes),
          new SqlParameter("@MaxCompareLevel", maxCompareLevel)
          ))
        {
          dbObj.CloseConnection();
          if (ds != null)
          {
            dg.DataSource = ds.Tables[0];
            Utils.InitGridSort(ref dg);
            dg.DataBind();
            ds.Dispose();
          }
          return ds;
        }
      }
    }

    private void DoCompare()
    {
      string strItems = string.Empty;
      string preference = "c";
      string ContainerTypeCode = ddlContainertype.SelectedValue;
      int nbItems = 0;
      for (int i = 0; i < dg.Rows.Count; i++)
      {
        TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
        if (cb.Checked)
        {
          strItems += dg.Rows[i].Cells.FromKey("ItemId").Text + ",";
          nbItems++;
        }
      }
      if (txtCompareComponents.Text != string.Empty)
      {
        using (Database database = Utils.GetMainDB())
        {
          using (DataSet ds = database.RunSPReturnDataSet("_Item_CompareAnalyzeSkuList", "",
            new SqlParameter("@SkuList", txtCompareComponents.Text)
            ))
          {
            database.CloseConnection();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
              strItems += dr["ItemId"].ToString() + ",";
              nbItems++;
            }
          }
        }
      }
      if (nbItems > 0)
      {
          Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "DoCompare()", "<script>DoCompare('" + strItems + "', '" + cultureCode + "','" + preference + "','" + ContainerTypeCode + "');</script>");
      }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      DataRow dr = ((DataTable)dg.DataSource).Rows[e.Row.Index];
      if (dr["ItemNumber"].ToString() != string.Empty)
      {
        e.Row.Cells.FromKey("ItemName").Text = e.Row.Cells.FromKey("ItemName").Text + " [" + dr["ItemNumber"].ToString() + "]";
      }
      if (dr["ItemId"].ToString() == itemId)
      {
        TemplatedColumn col = (TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
        CheckBox cb = (CheckBox)((CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
        cb.Checked = true;
        cb.Enabled = false;
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in e.Row.Cells)
        {
          c.AllowEditing = Infragistics.WebUI.UltraWebGrid.AllowEditing.No;
          c.Style.ForeColor = Color.Gray;
        }
      }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      if (be.Button.Key.ToLower() == "compare")
      {
        DoCompare();
      }
    }
  }
}