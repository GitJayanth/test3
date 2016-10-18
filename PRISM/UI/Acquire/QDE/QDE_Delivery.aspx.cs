#region Uses
using System;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using HyperCatalog.Business;
using Infragistics.WebUI.UltraWebGrid;
using System.Data.SqlClient;
#endregion

namespace HyperCatalog.UI.Acquire.QDE
{
  /// <summary>
  /// Description résumée de QDE_Delivery.
  /// </summary>
  public partial class QDE_Delivery : HCPage
  {
    #region Declarations
    /* Alternate for CR 5096(Removal of rejection functionality)--start
    protected int stat_total, stat_totalMandatory, stat_nbFinal, stat_nbDraft, stat_nbMissing, stat_nbRejected;
    protected int stat_nbFinal_inh, stat_nbDraft_inh, stat_nbRejected_inh; */
    protected int stat_total, stat_totalMandatory, stat_nbFinal, stat_nbDraft, stat_nbMissing;
    protected int stat_nbFinal_inh, stat_nbDraft_inh;
    // Alternate for CR 5096(Removal of rejection functionality)--end
    private System.Int64 itemId;
    private string currentGroup = string.Empty;
    private string currentLinkGroup = string.Empty;
    private int groupCount = 0;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      // *************************************************************************
      // Retrieve Product information
      // *************************************************************************
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      /* Alternate for CR 5096(Removal of rejection functionality)--start
      stat_total = stat_totalMandatory = stat_nbFinal = stat_nbDraft = stat_nbMissing = stat_nbRejected = 0;
      stat_nbDraft_inh = stat_nbFinal_inh = stat_nbRejected_inh = 0; */
      stat_total = stat_totalMandatory = stat_nbFinal = stat_nbDraft = stat_nbMissing = 0;
      stat_nbDraft_inh = stat_nbFinal_inh = 0;
      //Alternate for CR 5096(Removal of rejection functionality)--end
      QDEUtils.GetItemIdFromRequest();
      itemId = SessionState.CurrentItem.Id;
      if (SessionState.CurrentItem.MasterPublishingDate.HasValue)
      {
        uwToolbar.Items.FromKeyLabel("mpd").Text = "Published on : " + SessionState.CurrentItem.MasterPublishingDate.Value.ToString(SessionState.User.FormatDate);
      }
      else
      {
        uwToolbar.Items.FromKeyLabel("mpd").Text = "Published on: N/A";
      }
      QDEUtils.UpdateCultureCodeFromRequest();

      if (!Page.IsPostBack)
      {
        UpdateDataView(false);
      }
      SessionState.QDETab = "tb_delivery";
    }
    private void UpdateDataView(bool export)
    {
      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSPReturnDataSet("dbo.QDE_GetItemChunksToDeliver",
          new SqlParameter("@ItemId", itemId),
          new SqlParameter("@CultureCode", SessionState.Culture.Code)))
        {
          ds.Tables[0].TableName = "Content";
          ds.Tables[1].TableName = "PLC";
          dbObj.CloseConnection();

          if (dbObj.LastError == string.Empty)
          {
            #region Display chunks
            dgChunks.DataSource = ds.Tables[0];
            Utils.InitGridSort(ref dgChunks, false);
            dgChunks.DataBind();
            InitializeChunksGridGrouping();
            dgChunks.DisplayLayout.AllowSortingDefault = AllowSorting.No;
            dgChunks.DisplayLayout.ReadOnly = ReadOnly.PrintingFriendly;
            #endregion

            #region Display PLC
            if (ds.Tables[1].Rows.Count > 0)
            {
              dgPLC.DataSource = ds.Tables[1];
              Utils.InitGridSort(ref dgPLC, false);
              dgPLC.DataBind();
              UITools.UpdatePLCGridHeader(dgPLC);
              dgPLC.DisplayLayout.AllowSortingDefault = AllowSorting.No;
              dgPLC.DisplayLayout.ReadOnly = ReadOnly.PrintingFriendly;
              if (dgPLC.Columns.FromKey("PID") != null)
                dgPLC.Columns.FromKey("PID").Format = SessionState.User.FormatDate;
              if (dgPLC.Columns.FromKey("POD") != null)
                dgPLC.Columns.FromKey("POD").Format = SessionState.User.FormatDate;
              labelPLC.Visible = true;
              dgPLC.Visible = true;
            }
            else
            {
              labelPLC.Visible = false;
              dgPLC.Visible = false;
            }
            #endregion

            #region Display links
            ds.Tables.Add(dbObj.RunSPReturnDataSet("dbo._Link_GetContent", "Links",
              new SqlParameter("@ItemId", itemId),
              new SqlParameter("@CultureCode", SessionState.Culture.Code),
              new SqlParameter("@LinkTypeId", -1),
              new SqlParameter("@EligibleOnly", true)).Tables[0].Copy());
              dbObj.CloseConnection();

              if (dbObj.LastError != null && dbObj.LastError.Length == 0)
              {
                if (ds.Tables["Links"] != null)
                {
                  dgLinks.DataSource = ds.Tables["Links"];
                  Utils.InitGridSort(ref dgLinks, false);
                  dgLinks.DataBind();
                  dgLinks.DisplayLayout.AllowSortingDefault = AllowSorting.No;
                  dgLinks.DisplayLayout.ReadOnly = ReadOnly.PrintingFriendly;
                  labelLinks.Visible = true;
                  dgLinks.Visible = true;
                  InitializeLinksGridGrouping();
                }
                else
                {
                  labelLinks.Visible = false;
                  dgLinks.Visible = false;
                }
              }
            
            #endregion
          }
          if (export)
          {
            Utils.ExportDataSetToExcel(this, ds, "Delivery.xls");
          }
        }
      }
    }

    private void InitializeChunksGridGrouping()
    {
      int i = 0;
      groupCount = 0;
      while (i < dgChunks.Rows.Count)
      {
        string containerGroup = dgChunks.Rows[i].Cells.FromKey("Path").Value.ToString();
        if (i == 0 || currentGroup != containerGroup)
        {
          currentGroup = containerGroup;
          dgChunks.Rows.Insert(i, new UltraGridRow());
          UltraGridRow groupRow = dgChunks.Rows[i];
          UltraGridCell groupCellMax = groupRow.Cells[dgChunks.Columns.Count - 1]; // initialize all cells for this row
          foreach (UltraGridCell cell in groupRow.Cells)
          {
            cell.Style.CssClass = string.Empty;
          }
          dgChunks.Rows[i].Style.CssClass = "ptbgroup";
          UltraGridCell groupCell = groupRow.Cells.FromKey("Mandatory");
          groupCell.Text = containerGroup;
          groupCell.ColSpan = 5;
          i++;
        }
        i++;
      }
    }
    private void InitializeLinksGridGrouping()
    {
      int i = 0;
      int groupLinkTypeCount = 0;
      int groupLinkFromCount = 0;
      int groupFamilyCount = 0;
      bool currentLinkFrom = false;
      string currentFamily = string.Empty;
      LinkType currentLinkType = null;
      bool newLinkType = true;
      bool newLinkFrom = true;
      while (i < dgLinks.Rows.Count)
      {
        #region Group by LinkType
        int linkTypeId = Convert.ToInt32(dgLinks.Rows[i].Cells.FromKey("LinkTypeId").Value);
        if (i == 0 || (currentLinkType != null && currentLinkType.Id != linkTypeId))
        {
          currentLinkType = LinkType.GetByKey(linkTypeId);
          newLinkType = true;
          dgLinks.Rows.Insert(i, new UltraGridRow());
          UltraGridRow groupRow = dgLinks.Rows[i];
          UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row
          foreach (UltraGridCell cell in groupRow.Cells)
          {
            cell.Style.CssClass = string.Empty;
          }
          dgLinks.Rows[i].Style.CssClass = "ptbgroup";
          UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
          groupCell.Text = HyperCatalog.Business.LinkType.GetByKey(linkTypeId).Name;
          groupCell.ColSpan = 4;
          i++;
        }
        #endregion

        if (currentLinkType != null && currentLinkType.IsBidirectional && dgLinks.Rows[i].Cells.FromKey("LinkFrom") != null)
        {
          #region Group by LinkFrom
          bool linkFrom = Convert.ToBoolean(dgLinks.Rows[i].Cells.FromKey("LinkFrom").Value);
          if (newLinkType || currentLinkFrom != linkFrom)
          {
            currentLinkFrom = linkFrom;
            newLinkFrom = true;
            newLinkType = false;
            dgLinks.Rows.Insert(i, new UltraGridRow());
            UltraGridRow groupRow = dgLinks.Rows[i];
            UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row          
            foreach (UltraGridCell cell in groupRow.Cells)
            {
              cell.Style.CssClass = string.Empty;
            }
            dgLinks.Rows[i].Style.CssClass = "ptb4";
            UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
            if (linkFrom)
              groupCell.Text = "Companion list";
            else
                //Code modified to change 'Hardware' to 'Host'
                //groupCell.Text = "Hardware list";
                groupCell.Text = "Host list";

            groupCell.ColSpan = 4;
            i++;
          }
          #endregion

          #region Group by Family
          if (dgLinks.Rows[i].Cells.FromKey("Family") != null && dgLinks.Rows[i].Cells.FromKey("Family").Value != null)
          {
            string family = string.Empty;
            if (linkFrom) // Companion list
              family = dgLinks.Rows[i].Cells.FromKey("SubFamily").Value.ToString();
            else // Hardware list
              family = dgLinks.Rows[i].Cells.FromKey("Family").Value.ToString();
            if (newLinkType || newLinkFrom || currentFamily != family)
            {
              currentFamily = family;
              newLinkType = false;
              newLinkFrom = false;
              dgLinks.Rows.Insert(i, new UltraGridRow());
              UltraGridRow groupRow = dgLinks.Rows[i];
              UltraGridCell groupCellMax = groupRow.Cells[dgLinks.Columns.Count - 1]; // initialize all cells for this row
              foreach (UltraGridCell cell in groupRow.Cells)
              {
                cell.Style.CssClass = string.Empty;
              }
              dgLinks.Rows[i].Style.CssClass = "ptb5";
              UltraGridCell groupCell = groupRow.Cells.FromKey("Class");
              groupCell.Text = family;
              groupCell.ColSpan = 4;
              i++;
            }
          }
          #endregion
        }

        #region bidirectional link type
        if (dgLinks.Rows[i].Cells.FromKey("Bidirectional") != null && dgLinks.Rows[i].Cells.FromKey("Bidirectional").Value != null)
        {
          bool isBidirectional = Convert.ToBoolean(dgLinks.Rows[i].Cells.FromKey("Bidirectional").Value);
          if (!isBidirectional && dgLinks.Rows[i].Cells.FromKey("Name") != null) // Cross sell, Bundle, ...
          {
            dgLinks.Rows[i].Cells.FromKey("Name").ColSpan = 2;
          }
        }
        #endregion

        i++;
      }
    }

    #region Event methods
    protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      CellsCollection cells = e.Row.Cells;
      string filter = txtFilter.Text.Trim();
      bool keep = true;
      bool isMandatory = Convert.ToBoolean(cells.FromKey("IsMandatory").Value);
      bool isInherited = Convert.ToBoolean(cells.FromKey("Inherited").Value);
      bool isResource = Convert.ToBoolean(cells.FromKey("IsResource").Value);
      bool isBoolean = Convert.ToBoolean(cells.FromKey("IsBoolean").Value);
      bool readOnly = Convert.ToBoolean(cells.FromKey("ReadOnly").Value);

      if (filter.Length > 0)
      {
        keep = false;
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in cells)
        {
          if (!c.Column.Hidden && c.Value != null && c.Text != HyperCatalog.Business.Chunk.BlankValue)
          {
            if (c.Text.ToLower().IndexOf(filter.ToLower()) >= 0)
            {
              c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
              keep = true;
            }
          }
        }
      }
      if (!keep)
      {
        e.Row.Delete();
      }
      else
      {
        //If RTL languages, ensure correct display        
        if ((bool)cells.FromKey("Rtl").Value)
          cells.FromKey("Value").Style.CustomRules = "direction: rtl;";//unicode-bidi:bidi-override;";

        // Update CultureCode column
        cells.FromKey("CultureCode").Text = SessionState.Culture.Code;

        // Container group
        string containerGroup = cells.FromKey("Path").Text;
        if (currentGroup != containerGroup)
        {
          currentGroup = containerGroup;
          groupCount++;
        }

        // Check if ReadOnly container
        if (readOnly)
          cells.FromKey("ContainerName").Text = cells.FromKey("ContainerName").Text + " <img src='/hc_v4/img/ed_glasses.gif'/>";

        //Display Mandatory logo
        UltraGridCell aCell = cells.FromKey("Mandatory");
        aCell.Style.CssClass = "ptb1";
        aCell.Text = string.Empty; // by default
        if (isMandatory)
        {
          aCell.Style.CssClass = "SCM"; // Status Chunk Mandatory
        }

        // Update Item column
        aCell = cells.FromKey("ItemId");
        if (aCell.Text != itemId.ToString())
          aCell.Value = itemId.ToString();

        //Display Status logo
        aCell = cells.FromKey("Status");
        if (aCell.Value != null)
        {
          ChunkStatus cStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus), HyperCatalog.Business.Chunk.GetStatusFromString(cells.FromKey("Status").Value.ToString()).ToString());
          string status = HyperCatalog.Business.Chunk.GetStatusFromEnum(cStatus);
          aCell.Style.CssClass = "S" + status;
          aCell.Value = string.Empty;
        }

        // Check if value is inherited
        aCell = cells.FromKey("Value");
        aCell.Style.CssClass = "ptb3"; // by default
        aCell.Style.Wrap = true; // by default
        if (isInherited)
        {
          aCell.Style.CssClass = "overw";
          aCell.Style.Wrap = true;
        }

        // Ensure multiline is kept
        if (aCell.Value != null)
        {
          // if chunk is resource, try do display it
          if (isResource && aCell.Text != string.Empty)
          {
            try
            {
              string sUrl = aCell.Text;
              // Call HyperPublisher WebMethod to convert URL to absolute URL
              XmlDocument xmlInfo = new XmlDocument();
              xmlInfo.LoadXml(HCPage.WSDam.ResourceGetByPath(sUrl));
              System.Xml.XmlNode node = xmlInfo.DocumentElement;
              string fullPath = node.Attributes["uri"].InnerText;
              if (fullPath.ToLower().IndexOf("notfound") > 0 || fullPath == string.Empty)
              {
                fullPath = "/hc_v4/img/ed_notfound.gif";
              }
              aCell.Text = "<img src='" + fullPath + "?thumbnail=1&size=40' title='" + aCell.Text + "' border=0/>";
            }
            catch (Exception ex)
            {
              Trace.Warn("DAM", "Exception processing DAM: " + ex.Message);
            }
          }
          else
          {
            // BLANK Value is replace by Readable sentence
            if (aCell.Text == HyperCatalog.Business.Chunk.BlankValue)
            {
              aCell.Text = HyperCatalog.Business.Chunk.BlankText;
              aCell.Style.CustomRules = string.Empty;
            }
            else
            {
              if (isBoolean && aCell.Text != string.Empty)
              {
                try
                {
                  if (Convert.ToBoolean(aCell.Value))
                  {
                    aCell.Text = "Yes";
                  }
                  else
                  {
                    aCell.Text = "No";
                  }
                }
                catch { } // Value is not boolean!
              }
              else
              {
                aCell.Text = UITools.HtmlEncode(aCell.Text);
              }
            }

            // Display Fallback on cultures if current Culture <> "master"
            string sourceCultureCode = cells.FromKey("SourceCode").Text;
            if (sourceCultureCode != SessionState.Culture.Code)
            {
              aCell.Style.CustomRules = aCell.Style.CustomRules + "color:blue;font:italic;";
            }
          }
        }
      }
    }
    protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if (btn == "export")
      {
        UpdateDataView(true);
      }
    }
    protected void dgLinks_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      CellsCollection cells = e.Row.Cells;
      if (cells.FromKey("IsExcluded") != null && Convert.ToBoolean(cells.FromKey("IsExcluded").Value))
      {
        e.Row.Delete();
      }
      else
      {
        // Retrieve country code
        if (cells.FromKey("CountryCode") != null && cells.FromKey("ImageCountry") != null)
        {
          string countryCode = string.Empty;
          if (cells.FromKey("CountryCode") != null && cells.FromKey("CountryCode").Value != null)
            countryCode = cells.FromKey("CountryCode").ToString();

          // Update image for current country
          if (countryCode.Length > 0 && cells.FromKey("ImageCountry") != null)
            cells.FromKey("ImageCountry").Text = "<img title=\"" + countryCode + "\" src=\"/hc_v4/img/flags/" + countryCode.ToLower() + ".gif\">";
        }

        if (cells.FromKey("LinkFrom") != null)
        {
          bool linkFrom = Convert.ToBoolean(cells.FromKey("LinkFrom").Value);
          if (!linkFrom) // Hardware list
          {
            if (cells.FromKey("Name") != null && cells.FromKey("ItemName") != null && cells.FromKey("ItemName").Value != null)
              cells.FromKey("Name").Text = cells.FromKey("ItemName").Value.ToString();
            if (cells.FromKey("SKU") != null && cells.FromKey("ItemSKU") != null && cells.FromKey("ItemSKU").Value != null)
              cells.FromKey("SKU").Text = cells.FromKey("ItemSKU").Value.ToString();
            if (cells.FromKey("Class") != null && cells.FromKey("ClassName") != null && cells.FromKey("ClassName").Value != null)
              cells.FromKey("Class").Text = cells.FromKey("ClassName").Value.ToString();
          }
          else // Companion list
          {
            if (cells.FromKey("Name") != null && cells.FromKey("SubItemName") != null && cells.FromKey("SubItemName").Value != null)
              cells.FromKey("Name").Text = cells.FromKey("SubItemName").Value.ToString();
            if (cells.FromKey("SKU") != null && cells.FromKey("SubItemSKU") != null && cells.FromKey("SubItemSKU").Value != null)
              cells.FromKey("SKU").Text = cells.FromKey("SubItemSKU").Value.ToString();
            if (cells.FromKey("Class") != null && cells.FromKey("SubClassName") != null && cells.FromKey("SubClassName").Value != null)
              cells.FromKey("Class").Text = cells.FromKey("SubClassName").Value.ToString();
          }

          if (cells.FromKey("Name") != null)
            cells.FromKey("Name").Style.Wrap = true;
          if (cells.FromKey("SKU") != null)
            cells.FromKey("SKU").Style.Wrap = true;
          if (cells.FromKey("Class") != null)
            cells.FromKey("Class").Style.Wrap = true;
        }
      }
    }
    #endregion
  }
}
