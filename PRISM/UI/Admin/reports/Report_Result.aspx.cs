#region uses
using System;
using System.Collections;
using System.Configuration;
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
using HyperComponents.Data.dbAccess;
using Reporting;
#endregion
#region History
//  Changes incorporated for CR 5110 - Remediate the PRISM Query Tool ( Vivek Chandran Nair 12/01/2009)
// 1) Appending of the query string with nolock and row restriction commands
// 2) Exectuion of the query using Crystal_ReadOnly user
// 3) Setting the query to a property of the newly introduced class : ReportQuery
// 4) Restriction on the display rows increased to 4000
// 5) Modification of the messages displayed for Save/Syntax check and Run options of query tool
// 6) SQL query retrieved from the property of the newly introduced class : ReportQuery 
#endregion
/// <summary>
/// Display reult
///		--> Export in excel
///		--> Filter on all fields
/// </summary>
public partial class Report_Result : HCPage
{
    #region Declarations

    private int reportId;
    #endregion

    #region Properties
    private string sSql
    {
        get
        {
            if (ViewState["sql"] != null)
            {
                return ViewState["sql"].ToString();
            }
            return string.Empty;
        }
        set { ViewState["sql"] = value; }
    }


    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN : Cet appel est requis par le Concepteur Web Form ASP.NET.
        //
        InitializeComponent();
        txtFilter.AutoPostBack = false;
        txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);");
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

        try
        {
            if (Request["filter"] != null)
            {
                txtFilter.Text = Request["filter"].ToString();
            }

            if (Request["r"] != null)
                reportId = Convert.ToInt32(Request["r"]);
            if (Request["q"] != null)               
                sSql = Request["q"];
            ReportQuery RQ = new ReportQuery();
            sSql = RQ.PSQLQuery.ToString();
            if (Session["test"] != null)
            {
                sSql = Session["SQLCode"].ToString();
            }
          
            if (Request["r"] == null && Request["q"] == null)
            {
                Response.Write("<script>window.close();</script>");
                Response.End();
            }
        }
        catch
        {
            if (Request["q"] == null)
            {
                Response.Write("<script>window.close();</script>");
                Response.End();
            }
            else
            {
                ReportQuery RQ = new ReportQuery();
                sSql = RQ.PSQLQuery.ToString();
            }
        }
        
             UpdateDataView();
        
    }

    private void UpdateDataView()
    {
        lbError.Visible = false;
        Report r = Report.GetByKey(reportId);
        if (r != null)
        {
            r.LastRunDate = DateTime.Now;
            if (!r.Save())
            {
                lbError.CssClass = "hc_error";
                lbError.Text = Report.LastError;
                lbError.Visible = true;
                return;
            }
            sSql = r.Code;
        }
        Database dbObj;
        if (SessionState.CacheParams["AppName"].Value.ToString().Contains("Crystal"))
        {
            dbObj = new Database(ConfigurationManager.AppSettings["Crystal_DBReadonly"].ToString());
        }
        else
        {
            dbObj = new Database(ConfigurationManager.AppSettings["Gemstone_DBReadonly"].ToString());
        }
        DataSet ds = dbObj.RunSQLReturnDataSet(sSql);
        dbObj.CloseConnection();
        if (dbObj.LastError == string.Empty && ds.Tables.Count > 0)
        {
            lbResume.Text = "<br/><b> Table[1]</b> - " + ds.Tables[0].Rows.Count.ToString() + " rows(s). <b><i>(The query tool will return a max of upto " + SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value.ToString() + " records only for both Gridview and Excel Export.)</i></b>";
            if (ds.Tables[0].Rows.Count <= Convert.ToInt32(SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value))
            {
                dg.DataSource = ds.Tables[0];
                Utils.InitGridSort(ref dg, true);
                dg.DataBind();
                foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn col in dg.Columns)
                {
                    col.CellMultiline = Infragistics.WebUI.UltraWebGrid.CellMultiline.Yes;
                    col.CellStyle.Wrap = true;
                }
                if (ds.Tables.Count > 1)
                {
                    string layout = dg.DisplayLayout.SaveLayout();
                    for (int i = 1; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].Rows.Count <= Convert.ToInt32(SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value))
                        {
                            Infragistics.WebUI.UltraWebGrid.UltraWebGrid newDg = new Infragistics.WebUI.UltraWebGrid.UltraWebGrid("dg" + i);
                            newDg.DataSource = ds.Tables[i];
                            newDg.DataBind();
                            newDg.DisplayLayout.LoadLayout(layout, true, true, false, false);
                            foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn col in newDg.Columns)
                            {
                                col.CellMultiline = Infragistics.WebUI.UltraWebGrid.CellMultiline.Yes;
                                col.CellStyle.Wrap = true;
                            }
                            int nb = i + 1;
                            phGrids.Controls.Add(new LiteralControl("<br><b> Table[" + nb.ToString() + "]</b> - " + ds.Tables[i].Rows.Count.ToString() + " rows(s) returned.<br><br>"));
                            phGrids.Controls.Add(newDg);
                        }
                        else
                        {
                            phGrids.Controls.Add(new LiteralControl("<br><hr> Sorry, your query is returning too many rows (<b> Table[" + i.ToString() + "]</b>, max=" + SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value.ToString() + ")</font><br><br>"));
                            break;
                        }
                    }
                }
            }
            else
            {
                dg.Visible = lbResume.Visible = false;
                phGrids.Controls.Add(new LiteralControl("<br><hr> Sorry, your query is returning too many rows (max=" + SessionState.CacheParams["MaxSearchQueryDisplayedRows"].Value.ToString() + ")</font><br><br>"));
            }
            ds.Dispose();
        }
        else
        {
            if (dbObj.LastError != string.Empty)
            {
                dg.Visible = lbResume.Visible = false;
                phGrids.Controls.Add(new LiteralControl("<br><hr> Sorry, your query is not correct or you are trying to execute a query containing Create, Insert ,Update or Delete statements: <font color=red>" + dbObj.LastError + "</font><br><br>"));
            }
            else
                if (ds.Tables.Count == 0)
                {
                    dg.Visible = false;
                    lbResume.Visible = false;
                    phGrids.Controls.Add(new LiteralControl("<br><hr> Your query is completed <br><br>"));
                }

        }
    }


    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "export")
        {
            if (sSql != string.Empty)
            {
                ///************************ HO CODE **************************/
                //Utils.ExportToExcel(dg, "QueryReport", "QueryReport");

                ///************************ GDIC CODE *************************/
                Utils.ExportToExcelFromGrid(dg, "QueryReport", "QueryReport", Page.Page, null, "Query Report");
            }
        }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        //If row is visible, test if filter is not empty
        string filter = txtFilter.Text.Trim();
        if (filter != string.Empty)
        {
            bool keep = false;
            foreach (Infragistics.WebUI.UltraWebGrid.UltraGridCell c in e.Row.Cells)
            {
                if (c.Text != null)
                {
                    if (!c.Column.Hidden && c.Text.ToLower().IndexOf(filter.ToLower()) != -1)
                    {
                        //c.Text = Utils.CReplace(c.Text, "<font color=red><b>", "", 1);
                        //c.Text = Utils.CReplace(c.Text, "</b></font>", "", 1);
                        //Fix for QC-944(P4) Handled special cases for check boxes in the grid
                        if (!("true".Equals(c.Text.ToLower()) || "false".Equals(c.Text.ToLower())))
                        {
                            c.Text = Utils.CReplace(c.Text, filter, "<font color=red><b>" + filter + "</b></font>", 1);

                            //Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
                            //UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
                            keep = true;
                        }
                      
                        
                    }
                }
            }
            if (!keep)
            {
                e.Row.Delete();
            }
        }
    }
}
