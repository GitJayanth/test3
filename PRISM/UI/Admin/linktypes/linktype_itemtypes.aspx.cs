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
using System.Data.SqlClient;

/// <summary>
/// Item types can have this link type
/// </summary>
public partial class linktype_itemtypes : HCPage
{
	#region Declarations

	private int linkTypeId = -1;
	#endregion

  protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
    {
      uwToolbar.Items.FromKeyButton("Apply").Enabled = false;
    }

    if (!HyperCatalog.Shared.SessionState.User.HasCapability(HyperCatalog.Business.CapabilitiesEnum.EXTEND_CONTENT_MODEL))
    {
      UITools.HideToolBarSeparator(uwToolbar, "ApplySep");
      UITools.HideToolBarButton(uwToolbar, "Apply");
    }
    #endregion

    try
    {
      if (Request["t"] != null)
        linkTypeId = Convert.ToInt32(Request["t"]);

      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }
    catch
    {
      Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", "<script>back();</script>");
    }
  }

	private void UpdateDataView()
	{
		lbError.Visible = false;
		DataSet ds = null;

		HyperCatalog.Business.LinkType linkType = HyperCatalog.Business.LinkType.GetByKey(linkTypeId);
		try
		{
			if (linkType != null)
			{
				ds = linkType.GetItemTypes();

				if (ds != null)
				{
					dg.DataSource = ds;
					Utils.InitGridSort(ref dg, false);
					dg.DataBind();

					dg.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;

					lbNoresults.Visible = false;
					dg.Visible = true;
				}
				else
				{
					dg.Visible = false;
					lbNoresults.Visible = true;
				}
			}
		}
		finally
		{
			if (ds != null)
				ds.Dispose();
		}
	}

	private void Save()
	{
		lbError.Visible = false;

		HyperCatalog.Business.LinkType linkType = HyperCatalog.Business.LinkType.GetByKey(linkTypeId);
		if (linkType != null)
		{
      if (dg != null)
      {
        System.Text.StringBuilder sItemTypes = new System.Text.StringBuilder(String.Empty);
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow r in dg.Rows)
        {
          Infragistics.WebUI.UltraWebGrid.TemplatedColumn col = (Infragistics.WebUI.UltraWebGrid.TemplatedColumn)r.Cells.FromKey("Select").Column;
          CheckBox cb = (CheckBox)((Infragistics.WebUI.UltraWebGrid.CellItem)col.CellItems[r.Index]).FindControl("g_sd");
          if (cb.Checked)
          {
            string itemTypeId = r.Cells.FromKey("ItemTypeId").Value.ToString();
            if (sItemTypes.Length > 0)
              sItemTypes.Append("|");

            sItemTypes.Append(itemTypeId);
          }
        }

        if (!linkType.SaveItemTypes(sItemTypes.ToString()))
        {
          lbError.CssClass = "hc_error";
          lbError.Text = HyperCatalog.Business.LinkType.LastError;
          lbError.Visible = true;
        }
        else
        {
          // Refresh title tab
          UITools.RefreshTab(Page, "ItemTypes", linkType.GetItemTypeCount());

          lbError.CssClass = "hc_success";
          lbError.Text = "Data saved!";
          lbError.Visible = true;
        }
      }
		}
		else
		{
			lbError.CssClass = "hc_error";
			lbError.Text = "Link type ("+linkTypeId+") doesn't exist";
			lbError.Visible = true;
		}
	}

	protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if ( btn == "apply")
		{
			Save();
		}
	}

	protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
	{
		Infragistics.WebUI.UltraWebGrid.TemplatedColumn col = (Infragistics.WebUI.UltraWebGrid.TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
		CheckBox cb = (CheckBox)((Infragistics.WebUI.UltraWebGrid.CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");

		cb.Checked = Convert.ToBoolean(e.Row.Cells.FromKey("IsLinked").Value);
	}
}
