#region Uses
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
using HyperCatalog.Business;
using HyperCatalog.Shared;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
  /// Display capabilities list
  ///		--> Add new capability
  ///		--> Modify capability
  ///		--> Export in Excel
  ///		--> Filter on all fields of the grid
	/// </summary>
	public partial class Capabilities : HCPage
	{
    #region Declarations
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
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {    
			this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
			this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

		}
    #endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
      if (Request["filter"] != null)
      {
        txtFilter.Text = Request["filter"].ToString();
      }
      if (!Page.IsPostBack)
      {
        UpdateDataView();
      }
		}

    /// <summary>
    /// Display all capabilities
    /// </summary>
    private void UpdateDataView()
    {
      string sSql = string.Empty;
      webTab.Visible = false;
      string filter = txtFilter.Text;
      if (filter!=string.Empty)
      {
				string cleanFilter = filter.Replace("'", "''").ToLower();
				cleanFilter = cleanFilter.Replace("[", "[[]");
				cleanFilter = cleanFilter.Replace("_", "[_]");
				cleanFilter = cleanFilter.Replace("%", "[%]");

        sSql += " LOWER(Name) like '%" + cleanFilter +"%' ";
        sSql += " OR LOWER(Description) like '%" + cleanFilter +"%' ";
      }

      using (CapabilityList capabilities = Capability.GetAll(sSql))
      {
        if (capabilities != null)
        {
          if (capabilities.Count > 0)
          {
            dg.DataSource = capabilities;
            Utils.InitGridSort(ref dg);
            dg.DataBind();

            dg.Visible = true;
            lbNoresults.Visible = false;
          }
          else
          {
            if (txtFilter.Text.Length > 0)
              lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";

            dg.Visible = false;
            lbNoresults.Visible = true;
          }
        }
        else
        {
          lbError.CssClass = "hc_error";
          lbError.Text = "Error: a system error occurred";
          lbError.Visible = true;
        }
      }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      string btn = be.Button.Key.ToLower();
      #region Export
      if (btn == "export")
      {
        Utils.ExportToExcel(dg, "Capibilities", "Capibilities");
      }
      #endregion
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
      UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
    }
    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      if (sender != null)
      {
        Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);      
        lbTitle.Text = "Editing " + cellItem.Cell.Row.Cells.FromKey("Name").Text;
        string sId = cellItem.Cell.Row.Cells.FromKey("Id").Text;
			
        sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
        sId = Utils.CReplace(sId, "</b></font>", "", 1);
			
        UpdateDataEdit(sId);
      }
    }
    protected void UpdateDataEdit(string capabilityId)
    {
      panelGrid.Visible = false;
      webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./capabilities/capability_properties.aspx?r=" + capabilityId;
      webTab.Visible = true;
    }


	}
}
