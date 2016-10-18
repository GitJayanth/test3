#region uses
using System;
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
using HyperCatalog.Business;
#endregion

#region Historic
// Update possible values with referent item (parent, grand parent, class and every where) (CHARENSOL Mickael 10/01/06)
#endregion

/// <summary>
/// Description résumée de Chunk_PossibleValues.
/// </summary>
public partial class Chunk_PossibleValues : HCPage
{
  #region Declarations
  private System.Int64 itemId;
  private int containerId;
  private int inputformId;
	private string cultureCode;
  #endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
		try
		{
      containerId = Convert.ToInt32(Request["d"]);
      itemId = QDEUtils.GetQueryItemIdFromRequest();
      inputformId = Convert.ToInt32(Request["f"]);
      cultureCode = QDEUtils.GetQueryCultureCodeFromRequest();

      if (!Page.IsPostBack)
      {
        UpdateFilter();
        UpdateDataView();
      }
    }
    catch (Exception ex)
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", "<script>alert('Error: " + UITools.CleanJSString(ex.ToString()) + "'");
    }
  }

	private void UpdateFilter()
	{
		DataSet dsFilters = new DataSet();
		DataTable dt = dsFilters.Tables.Add("Filters");
		dt.Columns.Add("Name", Type.GetType("System.String"));
		dt.Columns.Add("ItemName", Type.GetType("System.String"));
		dt.Columns.Add("LevelName", Type.GetType("System.String"));
		dt.Columns.Add("Id", Type.GetType("System.Int64"));

		// get parent, grand parent and class
		if (itemId >= 0)
		{
			// get parent
      Item item = QDEUtils.GetItemIdFromRequest();
			if (item != null && item.ParentId >= 0)
			{
        // Retrieve item parent
        using (Item parentItem = item.Parent)
        {
          if (parentItem.Level != null)
          {
            // add parent
            dt.Rows.Add(new object[] { "Parent", parentItem.Name, parentItem.Level.Name, parentItem.Id });

            // get grand parent
            if (parentItem != null && parentItem.ParentId >= 0)
            {
              // Retrievegrand parent
              using (Item grandParentItem = parentItem.Parent)
              {
                if (grandParentItem.Level != null)
                {
                  // add grand parent
                  dt.Rows.Add(new object[] { "Grand parent", grandParentItem.Name, grandParentItem.Level.Name, grandParentItem.Id });

                  // Retrieve class
                  using (ItemList classItems = Item.GetAll("ItemId = dbo.GetItemClassId(" + itemId + ")"))
                  {
                    if ((classItems != null) && (classItems.Count == 1))
                    {
                      Item classItem = (Item)classItems[0];
                      if (classItem.Id >= 0 && classItem.Level != null
                        && classItem.Id != item.ParentId && classItem.Id != parentItem.ParentId)
                      {
                        dt.Rows.Add(new object[] { "Class", classItem.Name, classItem.Level.Name, classItem.Id }); // add class
                      }
                    }
                  }
                }
              }
            }
          }
        }
			}
		}
		// Add every where
		dt.Rows.Add(new object[] {"Every where", string.Empty, string.Empty, -1});

		uwDDL_Filters.DataSource = dsFilters;
		uwDDL_Filters.DataTextField = "Name";
		uwDDL_Filters.DataValueField = "Id";
		uwDDL_Filters.DataBind();
		uwDDL_Filters.SelectedIndex = 0;

		dsFilters.Dispose();
	}

  private void UpdateDataView()
  {
    System.Int64 refItemId = -1;
    int possibleValuesCount = 0; // Possible value count
    ChunkList possibleValues = null; // Possible values list 
    int chunkPossibleValuesCount = Convert.ToInt32(SessionState.CacheParams["ChunkPossibleValuesCount"].Value); // Retrieve possible values count
    string cultureCode = SessionState.Culture.Code; // current culture code

    // Get ref item for search possible values
    if ((uwDDL_Filters != null) && (uwDDL_Filters.SelectedRow != null))
    {
      refItemId = Convert.ToInt64(uwDDL_Filters.SelectedRow.Cells.FromKey("Id").Value);
    }
    HyperCatalog.Business.Container container = SessionState.QDEContainer;

    if (container != null)
    {
      possibleValues = container.PossibleValues(cultureCode, chunkPossibleValuesCount, refItemId, inputformId);
      possibleValuesCount = container.PossibleValuesCount(cultureCode, refItemId, inputformId);

      // If too much values are available, we will only show a certain limit
      LbSubSet.Visible = possibleValuesCount > chunkPossibleValuesCount;
    }
    UITools.RefreshTab(Page, "PossibleValues", possibleValuesCount);

    if (possibleValues != null && possibleValues.Count > 0)
    {
      panelGrid.Visible = true;
      dgPossibleValues.DataSource = possibleValues;
      Utils.InitGridSort(ref dgPossibleValues);
      dgPossibleValues.DataBind();
      dgPossibleValues.DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.CellSelect;
    }
    else
    {
      panelGrid.Visible = false;
    }
  }

	protected void uwDDL_Filters_SelectedRowChanged(object sender, Infragistics.WebUI.WebCombo.SelectedRowChangedEventArgs e)
	{
		UpdateDataView();
	}
	protected void uwDDL_Filters_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		if (e.Row.Cells.FromKey("LevelName") != null && e.Row.Cells.FromKey("LevelName").Text.Length > 0
      && e.Row.Cells.FromKey("Name") != null && e.Row.Cells.FromKey("ItemName") != null)
    {
			e.Row.Cells.FromKey("Name").Text = e.Row.Cells.FromKey("Name").Text + ": "+e.Row.Cells.FromKey("ItemName").Text+" [" + e.Row.Cells.FromKey("LevelName").Text + "]";
    }
	}
  protected void dgPossibleValues_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    //TODO: Reintroduce the Right to Left if necessary
    /*    bool rtl = e.Row.Cells.FromKey("Rtl").Text.ToLower()=="true";
        if (rtl)
        {
          e.Row.Cells.FromKey("Value").Style.CssClass  = "rtl";
        }*/
    if (e.Row.Cells.FromKey("Value").Text == HyperCatalog.Business.Chunk.BlankValue)
    {
      e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
    }
    else
    {
      e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(e.Row.Cells.FromKey("Value").Text);
    }
    ChunkStatus cStatus = (ChunkStatus)Enum.Parse(typeof(ChunkStatus), e.Row.Cells.FromKey("Status").Value.ToString());
    string status = HyperCatalog.Business.Chunk.GetStatusFromEnum(cStatus);
    e.Row.Cells.FromKey("Status").Style.CssClass = "S" + status;
    e.Row.Cells.FromKey("Status").Value = string.Empty;
  }
}
