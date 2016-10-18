#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
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
using HyperCatalog.Business;
using HyperCatalog.UI.Acquire.QDE;
#endregion

namespace HyperCatalog.UI.ItemManagement
{
  /// <summary>
  /// Description résumée de WebForm1.
  /// </summary>
  public partial class item_Comparison : HCPage
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

    }
    #endregion

    private string itemId;
    private string cultureCode;
    private bool staticMode = true;
    private string preference;
    private string ContainerTypeCode;



    protected void Page_Load(object sender, System.EventArgs e)
    {
      try
      {
        if (Request["i"] != null)
          itemId = Request["i"].ToString();
        cultureCode = QDEUtils.UpdateCultureCodeFromRequest().Code;
        if (Request["m"] != null)
          staticMode = Request["m"] == "s"; // Test comparison mode, static or dynamic  
        if (Request["preference"] != null)
          preference = Request["preference"].ToString();  // Preference Value for Compare or Compare With               
        if (Request["ContainerTypeCode"] != null)           // ContainerTypeCode Value for Compare or Compare With
          ContainerTypeCode = Request["ContainerTypeCode"].ToString();        
      }
      catch (Exception ex)
      {
        String err = ex.Message;
        UITools.JsCloseWin(err);
      }
      if (!Page.IsPostBack)
      {
          if (preference != "c")
          {
              //UpdateDataView(ds);
              //Method to Retain the Value of ItemId and CultureCode while Navigating to Item_Compare.aspx
              CompareWrtContainerType();
          }
          else
          {
              //Method to Compare Sku/Class
              Compare();
          }
      }
    }

      private DataSet Compare()
      {
          using (Database dbObj = Utils.GetMainDB())
          {
              DataSet ds = null;

              if (!staticMode)
              {
                  ds = dbObj.RunSPReturnDataSet("_Item_Compare", "",
                  new SqlParameter("@ItemList", itemId),
                  new SqlParameter("@CultureCode", cultureCode),
                  new SqlParameter("@ContainerTypeCode", ddlContainertype.SelectedValue));
              }
              else
              {
                  ds = dbObj.RunSPReturnDataSet("_Item_CompareWithSibbling", "",
                  new SqlParameter("@ItemId", itemId),
                  new SqlParameter("@CultureCode", cultureCode),
                  new SqlParameter("@RetrieveObsoletes", SessionState.User.ViewObsoletes),
                  new SqlParameter("@UserId", SessionState.User.Id),
                  new SqlParameter("@ContainerTypeCode", ddlContainertype.SelectedValue));
              }

              dbObj.CloseConnection();
              UpdateDataView(ds);
              return ds;
          }
      }

      private DataSet CompareWrtContainerType()
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
          //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CompareWrtContainerType()", "<script>CompareWrtContainerType('" + itemId + "', '" + cultureCode + "');</script>");
      }

    private void UpdateDataView(DataSet ds)
    {
      if (ds != null)
      {
        dg.Columns.Clear();
        dg.Rows.Clear();
        ////////////////////////////
        /// Create Grid
        ///////////////////////////

        //-- Create Columns
        //-- First xmlname column
        UltraGridColumn col = new UltraGridColumn();
        col.Key = "col_tag";
        col.Width = Unit.Pixel(180);
        dg.Columns.Add(col);

        //-- Item columns
        DataTable dtItems = ds.Tables[0];
        int nbCols = dtItems.Rows.Count;
        foreach (DataRow dr in dtItems.Rows)
        {
          string colHeader = dr["ItemName"].ToString();
          if (dr["ItemNumber"].ToString() != string.Empty)
          {
            colHeader = dr["ItemNumber"].ToString();
          }
          col = new UltraGridColumn();
          col.Key = "col_" + dr["ItemId"].ToString();
          col.Header.Caption = colHeader;
          col.Header.Style.Wrap = true;
          if (nbCols > 4)
          {
            col.Width = Unit.Pixel(150);
          }
          else
          {
            col.Width = Unit.Pixel(210);
          }
          dg.Columns.Add(col);
        }
        //-- Add content
        DataTable dt = ds.Tables[1];
        string prevGroup = string.Empty, curGroup = string.Empty;
        UltraGridRow ur = null;
        UltraGridCell cell = null;
        int i = 0;
        while (i < dt.Rows.Count)
        {
          curGroup = dt.Rows[i]["ContainerGroup"].ToString();
          if (prevGroup != curGroup)
          {
            ur = new UltraGridRow();
            ur.Style.Wrap = true;
            cell = new UltraGridCell();
            cell.Text = prevGroup = curGroup;
            ur.Style.BackColor = Color.DarkBlue;
            ur.Style.ForeColor = Color.White;
            cell.Style.Font.Bold = true;
            cell.Style.Wrap = true;
            cell.ColSpan = nbCols + 1;
            ur.Cells.Add(cell);
            dg.Rows.Add(ur);
          }
          // Add Tag name
          ur = new UltraGridRow();
          ur.Style.Wrap = true;
          cell = new UltraGridCell();
          cell.Text = "[" + dt.Rows[i]["LevelId"].ToString() + "] " + dt.Rows[i]["tag"].ToString();
          cell.Style.Font.Bold = true;
          cell.Style.Wrap = true;
          ur.Cells.Add(cell);

          // Add values
          string chunkValue, prevChunkValue = string.Empty;
          int colspan = 0;
          for (int j = 0; j < nbCols; j++)
          {
            chunkValue = dt.Rows[i]["ChunkValue"].ToString().Trim();
            if (j != 0) colspan++;
            if ((prevChunkValue != chunkValue && j > 0) || j == nbCols - 1)
            {
              if (prevChunkValue == chunkValue) colspan++;
              if (colspan > 0)
              {
                AddChunkCell(ur, dt.Rows[i]["ContainerTypeCode"].ToString(), prevChunkValue, colspan);
              }
              for (int k = 0; k < colspan - 1; k++)
              {
                AddChunkCell(ur, dt.Rows[i]["ContainerTypeCode"].ToString(), prevChunkValue, 0);
              }
              if (j == nbCols - 1 && prevChunkValue != chunkValue)
              {
                AddChunkCell(ur, dt.Rows[i]["ContainerTypeCode"].ToString(), chunkValue, colspan);
              }
              colspan = 0;
            }
            prevChunkValue = chunkValue;
            i++;
            //if (i < dt.Rows.Count){i++;}
          }
          dg.Rows.Add(ur);
        }
        ds.Dispose();
      }
    }

    private void AddChunkCell(UltraGridRow ur, string containerTypeCode, string chunkValue, int colspan)
    {
      UltraGridCell cellChunk = new UltraGridCell();
      cellChunk.Text = chunkValue;
      cellChunk.Style.Wrap = true;
      if (containerTypeCode == "P" && cellChunk.Text != string.Empty)
      { // Photo
        XmlDocument xmlInfo = new XmlDocument();
        string imgPath = HCPage.WSDam.ResourceGetByPath(cellChunk.Text);
        if (imgPath != string.Empty)
        {
          try
          {
            xmlInfo.LoadXml(imgPath);
            System.Xml.XmlNode node = xmlInfo.DocumentElement;
            System.Xml.XmlNode fileNode = node.FirstChild;
            string fullPath = node.Attributes["uri"].InnerText;
            cellChunk.Text = "<img src='" + fullPath + "?thumbnail=1&size=40' title='" + cellChunk.Text + "' border=0/>";
          }
          catch (Exception ex)
          {
            cellChunk.Text = "<img src='/hc_v4/img/ed_notfound.gif' title='An exception occurred: " + ex.Message + "' border=0/>";
            Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
          }
        }
        else
          cellChunk.Text = "<img src='/hc_v4/img/ed_notfound.gif' title='not found' border=0/>";
      }
      else
      {
        if (cellChunk.Text == Chunk.BlankValue)
        {
          cellChunk.Text = Chunk.BlankText;
        }
        else
        {
          cellChunk.Text = UITools.HtmlEncode(cellChunk.Text);
        }
      }
      cellChunk.Style.HorizontalAlign = HorizontalAlign.Center;
      cellChunk.Style.BorderWidth = Unit.Pixel(1);
      cellChunk.Style.BorderStyle = BorderStyle.Inset;
      cellChunk.Style.BorderDetails.WidthTop = Unit.Pixel(0);
      cellChunk.Style.BorderDetails.WidthBottom = Unit.Pixel(0);

      if (colspan > 1)
      {
        cellChunk.ColSpan = colspan;
      }
      ur.Cells.Add(cellChunk);

    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "compare")
        {
            Compare();
            //Method to Retain the Value of ItemId,CultureCode,preference and ContainerTypeCode while Navigating to Item_Comparison.aspx
            //CompareWrtContainerTypes();
        }

        if (btn == "export")
        {
            using (DataSet ds = Compare())
            {
                ExportComparison.ExportGrid(ds, this);
            }
        }
    }
  }
}