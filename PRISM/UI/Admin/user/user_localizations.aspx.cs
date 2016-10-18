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
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

#region History
	// Buttons "save" and "delete" read only (CHARENSOL 24/10/2005)
#endregion

/// <summary>
/// Display localization list of selected user
///		--> Return to the list of users
///		--> Select localizations
///		--> Save modification
/// </summary>
public partial class user_localizations : HCPage
{
	#region Declarations
	
	private int userId = -1;
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
		
	private void InitializeComponent()
	{
    this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
    this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

  }
	#endregion
    
	protected void Page_Load(object sender, System.EventArgs e)
  {
    #region Capabilities
    if (SessionState.User.IsReadOnly)
		{
			uwToolbar.Items.FromKeyButton("Save").Enabled = false;
		}
    if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS))
    {
      UITools.HideToolBarButton(uwToolbar, "Save");
      UITools.HideToolBarSeparator(uwToolbar, "SaveSep");
    }
    #endregion

    try
		{
			if (Request["u"] != null)
				userId = Convert.ToInt32(Request["u"]);
			ViewState["userId"] = userId.ToString();

			if (!Page.IsPostBack)
			{
				UpdateDataView();
			}
		}
		catch
		{
			Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"clientScript", "<script>back();</script>");  
		}
	}


	void UpdateDataView()
	{
		lbError.Visible = false;

    using (Database dbObj = Utils.GetMainDB())
    {
      using (DataSet ds = dbObj.RunSPReturnDataSet("_User_GetCulturesSorted", "cultures", new SqlParameter("@UserId", userId)))
      {
        dbObj.CloseConnection();
        if (dbObj.LastError.Length == 0)
        {
          if (ds != null)
          {
            dg.DataSource = ds.Tables[0];
            Utils.InitGridSort(ref dg, false);
            dg.DataBind();
            dg.DisplayLayout.AllowSortingDefault = AllowSorting.No;
            ds.Dispose();

            if (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_USERS) || SessionState.User.IsReadOnly)
            {
              TemplatedColumn colHeader = (TemplatedColumn)dg.Columns.FromKey("Select");
              HeaderItem cellItemHeader = colHeader.HeaderItem;
              CheckBox cbHeader = (CheckBox)cellItemHeader.FindControl("g_ca");
              cbHeader.Enabled = false;

              foreach (UltraGridRow r in dg.Rows)
              {
                TemplatedColumn col = (TemplatedColumn)r.Cells.FromKey("Select").Column;
                CellItem cellItem = (CellItem)col.CellItems[r.Index];
                CheckBox cb = (CheckBox)cellItem.FindControl("g_sd");
                cb.Enabled = false;
              }
            }
          }
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = dbObj.LastError;
          lbError.Visible = true;
        }
      }
    }
	}


  private void Save()
  {
    using (Database dbObj = Utils.GetMainDB())
    {

      // *************************************************************************
      // Update user localizations scope
      // *************************************************************************
      if (dg.Rows.Count > 0)
      {
        string updateSQL = string.Empty;
        string cultureCode;
        lbError.Visible = false;

        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow r in dg.Rows)
        {
          Infragistics.WebUI.UltraWebGrid.TemplatedColumn col = (Infragistics.WebUI.UltraWebGrid.TemplatedColumn)r.Cells.FromKey("Select").Column;
          CheckBox cb = (CheckBox)((Infragistics.WebUI.UltraWebGrid.CellItem)col.CellItems[r.Index]).FindControl("g_sd");

          cultureCode = r.Cells.FromKey("CultureCode").Text;
          updateSQL += "DELETE from UserCultures WHERE UserId = " + userId + " AND CultureCode='" + cultureCode + "'; ";

          if (cb.Checked)
          {
            updateSQL += "INSERT INTO UserCultures(UserId, CultureCode) VALUES(" + userId + ", '" + cultureCode + "'); ";
          }
        }

        dbObj.RunSQL(updateSQL);
        dbObj.CloseConnection();
        if (dbObj.LastError.Length > 0)
        {
          lbError.CssClass = "hc_error";
          lbError.Text = dbObj.LastError;
          lbError.Visible = true;
        }
        else
        {
          lbError.Text = "Data saved!";
          lbError.CssClass = "hc_success";
          lbError.Visible = true;
        }
      }
      else
      {
        lbError.CssClass = "hc_error";
        lbError.Text = "No Culture found";
        lbError.Visible = true;
      }
      UITools.RefreshTab(Page, "Cultures", Utils.GetCount(dbObj, string.Format("SELECT COUNT(*) FROM UserCultures Where UserId = {0}", userId)));
    }
  }

	private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
	{
		string btn = be.Button.Key.ToLower();
		if (btn == "save")
		{
			Save();
		}
	}

  private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
  {
    Infragistics.WebUI.UltraWebGrid.TemplatedColumn col = (Infragistics.WebUI.UltraWebGrid.TemplatedColumn)e.Row.Cells.FromKey("Select").Column;
    CheckBox cb = (CheckBox)((Infragistics.WebUI.UltraWebGrid.CellItem)col.CellItems[e.Row.Index]).FindControl("g_sd");
    cb.Checked = Convert.ToBoolean(e.Row.Cells.FromKey("InScope").Value);

    string sIndent = string.Empty;
    Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
    for (int i=0; i< Convert.ToInt32(r.Cells.FromKey("CultureTypeId").Value);i++)
    {
      sIndent += "&nbsp;&nbsp;&nbsp;";
    }
    r.Cells.FromKey("CultureName").Value = sIndent + "[" + r.Cells.FromKey("CultureCode").Value + "]&nbsp;" + r.Cells.FromKey("CultureName").Value;
    r.Cells.FromKey("CultureName").Style.CssClass = "culture_" + r.Cells.FromKey("CultureTypeId").Value;
    r.Cells.FromKey("CountryCode").Value = "<img src='/hc_v4/img/flags/" + r.Cells.FromKey("CountryCode").Value + ".gif'/>";
  }
}
