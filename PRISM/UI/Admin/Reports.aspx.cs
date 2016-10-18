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
using HyperCatalog.Shared;
#endregion

#region History
// Add capabilities (CHARENSOL Mickael 24/10/2005)
#endregion

namespace HyperCatalog.UI.Admin
{
    /// <summary>
    /// Display list of report
    /// </summary>
    public partial class Reports : HCPage
    {
        #region Declarations
        #endregion

        #region Code généré par le Concepteur Web Form
        override protected void OnInit(EventArgs e)
        {
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
	    //QC-724. Filter option for query report.
	    //Fix start- Assiging the value to filter.text

            if (Request["filter"] != null)
            {
                txtFilter.Text = Request["filter"].ToString();
            }
            //724 end
            #region Capabilities
            if (SessionState.User.IsReadOnly)
            {
                uwToolbar.Items.FromKeyButton("Add").Enabled = false;
            }
            #endregion
            if (!Page.IsPostBack)
            {
                UpdateDataView();
            }
        }

        private void UpdateDataView()
        {
            panelGrid.Visible = true;
            webTab.Visible = false;
                    //QC724 Start -Removed the condition to bind the grid even if the filter string is not empty
		    // if(filter!=string.Empty)
            string filter = txtFilter.Text;
            
                ReportList list = Report.GetByUserId(HyperCatalog.Shared.SessionState.User.Id);
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        dg.DataSource = list;
                        Utils.InitGridSort(ref dg);
                        dg.DataBind();
                        dg.Rows[0].Selected = true;
                        dg.Visible = true;
                        lbNoresults.Visible = false;
                    }
                    else
                    {
                        lbNoresults.Visible = true;
                        dg.Visible = false;
                    }
                }
            
            if(filter!=string.Empty)
            {
                if (dg != null)
                {
                    foreach (Infragistics.WebUI.UltraWebGrid.UltraGridRow r in dg.Rows)
                    {
                        bool keep = false;
                        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell cell in r.Cells)
                        {
                            if (cell.Text != null)
                            {
                                if (!cell.Column.Hidden && cell.Text.ToLower().IndexOf(filter.ToLower()) != -1)
                                {
                                    cell.Text = Utils.CReplace(cell.Text, "<font color=red><b>", "", 1);
                                    cell.Text = Utils.CReplace(cell.Text, "</b></font>", "", 1);
                                    cell.Text = Utils.CReplace(cell.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);
                                    keep = true;
                                }
                            }
                        }
                        r.Hidden = !keep;
                    }
//Commented the below code since the logic is repeated .

                    //if (dg.Rows.Count == 0)
                    //{
                    //    lbNoresults.Text = "No record match your search (" + txtFilter.Text + ")";
                    //    dg.Visible = false;
                    //    lbNoresults.Visible = true;
                    //}
                    //else
                    //{
                    //    lbNoresults.Visible = false;
                    //    dg.Visible = true;
                    //}
                }
            }
        }

        protected void UpdateDataEdit(string reportId)
        {
            panelGrid.Visible = false;
            webTab.Tabs.GetTab(0).ContentPane.TargetUrl = "./reports/report_properties.aspx?r=" + reportId;
            if (Convert.ToInt32(reportId) < 0)
            {
                lbTitle.Text = "Report: New";
                webTab.Tabs.GetTab(1).Visible = false;
            }
            else
            {
                webTab.Tabs.GetTab(1).ContentPane.TargetUrl = "./reports/report_properties.aspx?s=1&r=" + reportId;
                webTab.Tabs.GetTab(1).Visible = true;
                Report r = Report.GetByKey(Convert.ToInt32(reportId));
                lbTitle.Text = "Role: " + r.Name;
            }
            webTab.Visible = true;
        }

        private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            User creator = ((ReportList)dg.DataSource)[e.Row.Index].Creator;
            e.Row.Cells.FromKey("Owner").Text = creator.FirstName + " " + creator.LastName;
            Infragistics.WebUI.UltraWebGrid.UltraGridCell c;
            c = e.Row.Cells.FromKey("CreateDate");
            if (c.Text != null)
            {
                c.Text = ((DateTime?)c.Value).Value.ToString(SessionState.User.FormatDate);
            }
            c = e.Row.Cells.FromKey("LastRun");
            if (((DateTime?)c.Value).HasValue)
            {
                c.Text = ((DateTime?)c.Value).Value.ToString(SessionState.User.FormatDate);
            }
        }

        private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            if (be.Button.Key == "Add")
            {
                UpdateDataEdit("-999");
            }
            else
            {
                UpdateDataView();
            }
        }

        // "Name" Link Button event handler
        protected void UpdateGridItem(object sender, System.EventArgs e)
        {
            if (sender != null)
            {
                if (HyperCatalog.Shared.SessionState.User.HasCapability(CapabilitiesEnum.SQL_BUILDER))
                {
                    Infragistics.WebUI.UltraWebGrid.CellItem cellItem = ((Infragistics.WebUI.UltraWebGrid.CellItem)((System.Web.UI.WebControls.LinkButton)sender).Parent);
                    string sId = cellItem.Cell.Row.Cells.FromKey("Id").Text;

                    sId = Utils.CReplace(sId, "<font color=red><b>", "", 1);
                    sId = Utils.CReplace(sId, "</b></font>", "", 1);

                    UpdateDataEdit(sId);
                }
                else
                {
                    UITools.DenyAccess(DenyMode.Standard);
                }
            }
        }
    }
}
