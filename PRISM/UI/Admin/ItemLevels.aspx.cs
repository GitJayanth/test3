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
using HyperComponents.Data.dbAccess;    
using System.Data.SqlClient;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Display list of item level
	/// </summary>
	public partial class ItemLevels : HCPage
	{
		#region Declarations
    protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
    protected System.Web.UI.WebControls.Label Label3;
    protected System.Web.UI.WebControls.Label Label4;
    protected System.Web.UI.WebControls.Label Label5;
    protected System.Web.UI.WebControls.Label Label6;

    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      //
      // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
      //
      InitializeComponent();
      txtFilter.AutoPostBack = false;
      txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
      base.OnInit(e);
    }
		
    /// <summary>
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      #region Capabilities
      if (HyperCatalog.Shared.SessionState.User.IsReadOnly)
			{
				//uwToolbar.Items.FromKeyButton("Add").Enabled = false;
			}
      #endregion

      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
    }

    private void UpdateDataView()
    {
      string sSql = "SELECT LevelId, LevelName, Optional, SkuLevel FROM ItemLevels";

			string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " WHERE LOWER(LevelName) LIKE '%" + cleanFilter +"%' ";
      }

      using (Database dbObj = Utils.GetMainDB())
      {
        using (DataSet ds = dbObj.RunSQLReturnDataSet(sSql, "ItemLevels"))
        {
          dbObj.CloseConnection();
          if (dbObj.LastError.Length > 0)
          {
            lbError.CssClass = "hc_error";
            lbError.Text = dbObj.LastError;
            lbError.Visible = true;
          }
          else
          {
            if (ds != null)
            {
              if (ds.Tables["ItemLevels"] != null && ds.Tables["ItemLevels"].Rows.Count > 0)
              {
                dg.DataSource = ds.Tables["ItemLevels"];
                Utils.InitGridSort(ref dg);
                dg.DataBind();

                lbNoresults.Visible = false;
                dg.Visible = true;
              }
              else
              {
                if (txtFilter.Text.Length > 0)
                  lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";
                dg.Visible = false;
                lbNoresults.Visible = true;
              }
              ds.Dispose();

              panelGrid.Visible = true;
              lbTitle.Text = UITools.GetTranslation("Item levels list");
            }
          }
        }
      }
    }

    void UpdateDataEdit(string selLevelId)
    {
      panelGrid.Visible = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./itemlevels/itemlevels_properties.aspx?l=" + selLevelId;
			if (selLevelId == "-1")
			{ 
				lbTitle.Text = "Item level: New";

				webTab.Visible = true;
			}
			else
			{
				ItemLevel level = ItemLevel.GetByKey(Convert.ToInt32(selLevelId));
				if (level != null)
				{
					lbTitle.Text = "Item level: " + level.Name;
					webTab.Tabs[0].Visible = true;
					webTab.Visible = true;
				}
			}
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      if ( btn == "add")
      {
        UpdateDataEdit("-1");
      }
      if (btn == "export")
      {
				Utils.ExportToExcel(dg, "ItemLevels", "ItemLevels");
      }
    }
    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
			if (sender != null)
			{
				Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
				string sId = cellItem.Cell.Row.Cells.FromKey("Id").Text;
			
				sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
				sId = Utils.CReplace(sId, "</b></font>", "", 1);
			
				UpdateDataEdit(sId);
			}
    }
  }
}
