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
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI.Admin
{
	/// <summary>
	/// Description résumée de Reports.
	/// </summary>
	public partial class Analysis : HCPage
	{
    protected int AnalyzeId = -1;

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
      InitializeComponent();
      base.OnInit(e);
      this.txtFilter.AutoPostBack = false;
    }
		
    /// <summary>
    ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    ///		le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
      this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);

    }
    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
      if (!Page.IsPostBack) {
       txtFilter.Text ="Test";
      }

      #region Check Capabilities
      if ((HyperCatalog.Shared.SessionState.User.IsReadOnly))// || (!!HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.DESIGN_ANALYSES)))
      {
        //uwToolbar.Items.FromKeyButton("Add").Enabled = false;
        //webTab.Tabs.FromKeyTab("Properties").Enabled = false;
        //webTab.Tabs.FromKeyTab("Roles").Enabled = false;
      }
      #endregion


      // User must not see links in the grid
      if (!Page.IsPostBack)
      {        
        //dg.Columns.FromKey("NameLink").ServerOnly = !HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.DESIGN_ANALYSES);
        dg.Columns.FromKey("Name").ServerOnly = !dg.Columns.FromKey("NameLink").ServerOnly;
        UpdateDataView(string.Empty);
      }
        txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");       
    }

    private void UpdateDataView(string filter)
    {
      panelGrid.Visible = true;
      webTab.Visible = false;
      if (filter==string.Empty)
      {
        dg.DataSource = HyperCatalog.Business.Analysis.GetByUserId(HyperCatalog.Shared.SessionState.User.Id);
        Utils.InitGridSort(ref dg);
        dg.DataBind();
      }
      else  
      {
        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow r in dg.Rows)
        {
          bool keep = false;
          foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell cell in r.Cells)
          {
            if (cell.Text!=null)
            {
              if (!cell.Column.Hidden && cell.Text.ToLower().IndexOf(filter.ToLower())!=-1)
              {
                cell.Text = Utils.CReplace(cell.Text, "<font color=red><b>", "", 1);
                cell.Text = Utils.CReplace(cell.Text, "</b></font>", "", 1);
                cell.Text = Utils.CReplace(cell.Text, filter, "<font color=red><b>" + filter +"</b></font>", 1);
                keep = true;
              }
            }            
          }
          r.Hidden = !keep;
        }
      }
    }

    protected void UpdateDataEdit(string AnalyzeId)
    {
      panelGrid.Visible = false;
      if (Convert.ToInt32(AnalyzeId)<0)
      { 
        lbTitle.Text = "Analysis: New";
        webTab.Tabs.GetTab(1).Visible = false;
        webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./Analysis/Analysis_Properties.aspx";
      }
      else
      {
        webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./Analysis/Analysis_properties.aspx?d=" + AnalyzeId;
        webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./Analysis/Analysis_Roles.aspx?d=" + AnalyzeId;      
        webTab.Tabs.GetTab(1).Visible = true;      
        HyperCatalog.Business.Analysis a = HyperCatalog.Business.Analysis.GetByKey(Convert.ToInt32(AnalyzeId));
        lbTitle.Text = "Analysis: " ;//+ a.Name;
      }
      webTab.Visible = true;
    }

    /*
    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
      //User creator = ((AnalyzeList)dg.DataSource).Item(e.Row.Index).Creator;
      int creatorId =Convert.ToInt32(dg.Rows[e.Row.Index].Cells.FromKey("CreatorId").Value);
      HyperCatalog.Business.User  creator =  HyperCatalog.Business.User.GetByKey(creatorId);
      //User creator  =  dg.Rows[e.Row.Index].Cells["Creator"].Value;

      e.Row.Cells.FromKey("Owner").Text = creator.FirstName + " " + creator.LastName; 
      Infragistics.WebUI.UltraWebGrid.UltraGridCell c;
      c = e.Row.Cells.FromKey("CreateDate");
      if (c.Text != null)
      {
        c.Text = ((DateTime?)c.Value).ToString(SessionState.User.FormatDate);
      }
      c = e.Row.Cells.FromKey("LastRun");
      if (((DateTime?)c.Value).HasValue)  
      {
        c.Text = ((DateTime)c.Value).ToString(SessionState.User.FormatDate);
      }
    }

    */
    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
      if (be.Button.Key =="Add")
      {
        UpdateDataEdit("-999");
      }
      else
      {
        UpdateDataView(txtFilter.Text);
      }
    }

    // "Name" Link Button event handler
    protected void UpdateGridItem(object sender, System.EventArgs e)
    {
      //if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.DESIGN_ANALYSES))
      //{ 
        Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent); 
        UpdateDataEdit(cellItem.Cell.Row.Cells.FromKey("Id").Text);
      //}
      //else
      //{
      //  UITools.DenyAccess(DenyMode.Frame);
      //}
    }
  }
}
