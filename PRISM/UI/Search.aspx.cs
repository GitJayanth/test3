#region uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Shared;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.UI
{
    /// <summary>
    /// Description résumée de Search.
    /// </summary>
    public partial class Search : HCPage
    {
        private string searchString = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["q"] != null)
            {
                searchString = Request["q"].ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InitText", "<script>document.getElementById(txtSearchId).value='" + searchString.Replace("'", "''") + "';</script>");
                DisplaySearchResults();
            }
            if (!Page.IsPostBack)
            {
                QDEUtils.UpdateCultureCodeFromRequest();
                if (Session["SearchResult"] != null)
                {
                    ((DataSet)Session["SearchResult"]).Dispose();
                    Session["SearchResult"] = null;
                }
                UpdateDataView();
                UITools.ShowSearchBar(Page);
            }
        }
        protected void UpdateDataView()
        {
            DDL_Cultures.Visible = false;

            // retrieve all cultures for given user
            HyperCatalog.Business.CultureList cultures = SessionState.User.Cultures;

            if (cultures != null)
            {
                if (cultures.Count == 1)
                {
                    // cultures contains only one culture
                    DDL_Cultures.Visible = false;
                }
                else
                {
                    CollectionView cv = new CollectionView(cultures);
                    cv.Sort("Name");
                    DDL_Cultures.DataSource = cv;
                    DDL_Cultures.DataBind();

                    DDL_Cultures.SelectedValue = SessionState.Culture.Code;
                    DDL_Cultures.Visible = true;
                    cv.Dispose();
                }
            }
        }

        private void DisplaySearchResults()
        {
            uwToolbar.Items.FromKeyButton("Export").Enabled = lbError.Visible = lbRecordcount.Visible = dg.Visible = false;
            searchString = searchString.Trim().ToLower();
            if (searchString.Length > 3)
            {
                //ItemList searchItems = HyperCatalog.Business.Item.Search(searchString, SessionState.User.Id, SessionState.Culture.Code);
                //if (searchItems != null && searchItems.Count > 0)
                //{
                //  uwToolbar.Items.FromKeyButton("Export").Enabled = lbRecordcount.Visible = dg.Visible = true;
                //  Utils.InitGridSort(ref dg);
                //  dg.DataSource = searchItems;
                //  dg.DataBind();
                //  lbRecordcount.Text = searchItems.Count.ToString() + " record";
                //  if (searchItems.Count > 1)
                //  {
                //    lbRecordcount.Text = lbRecordcount.Text + "s";
                //  }
                //  lbRecordcount.Text = lbRecordcount.Text + " found in ";
                //}
                //else
                //{
                //  lbError.Text = "No record match your search (<b>" + searchString + "</b>) in ";
                //  lbError.Visible = true;
                //}
                using (DataSet ds = GetSearchResults())
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        uwToolbar.Items.FromKeyButton("Export").Enabled = lbRecordcount.Visible = dg.Visible = true;
                        Utils.InitGridSort(ref dg);
                        dg.DataSource = ds.Tables[0].DefaultView;
                        dg.DataBind();
                        lbRecordcount.Text = ds.Tables[0].Rows.Count.ToString() + " record";
                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            lbRecordcount.Text = lbRecordcount.Text + "s";
                        }
                        lbRecordcount.Text = lbRecordcount.Text + " found in ";
                    }
                    else
                    {
                        lbError.Text = "No record match your search (<b>" + searchString + "</b>) in ";
                        lbError.Visible = true;
                    }
                }

            }
            else
            {
                lbError.Text = "please provide a longer expression (<b>" + searchString + "</b>)";
                lbError.Visible = true;
                DDL_Cultures.Visible = false;
            }

        }

        protected DataSet GetSearchResults()
        {
            if (Session["SearchResult"] == null)
            {
                using (HyperComponents.Data.dbAccess.Database dbObj = Utils.GetMainDB())
                {
                    Session["SearchResult"] = dbObj.RunSPReturnDataSet("_Item_Search", new SqlParameter("@SearchString", searchString),
                         new SqlParameter("@UserId", SessionState.User.Id), new SqlParameter("@CultureCode", SessionState.Culture.Code),
                         new SqlParameter("@CompanyName", SessionState.CompanyName));
                }
            }
            return ((DataSet)Session["SearchResult"]);
        }
        protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            if (btn == "export")
            {
                Utils.ExportToExcel(dg, "SearchResult", "SearchResult");
            }
        }

        protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            string status = e.Row.Cells.FromKey("Status").Text.ToLower().Substring(0, 1);
            bool isRoll = (bool)e.Row.Cells.FromKey("IsRoll").Value;
            string company = e.Row.Cells.FromKey("COMPANY").Text;
            Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
            UITools.HiglightGridRowFilter(ref r, searchString);
            if (e.Row.Cells.FromKey("Sku").Value != null)
            {
                e.Row.Cells.FromKey("FullName").Text = "[" + e.Row.Cells.FromKey("Sku").Text + "] - " + e.Row.Cells.FromKey("FullName").Text;
            }
            switch (status)
            {
                case "o": e.Row.Cells.FromKey("FullName").Text = "<font color=gray>[O] </font>" + e.Row.Cells.FromKey("FullName").Text;
                    break;
                case "f": e.Row.Cells.FromKey("FullName").Text = "<font color=green>[F] </font>" + e.Row.Cells.FromKey("FullName").Text;
                    break;
                case "e": e.Row.Cells.FromKey("FullName").Text = "<font color=darkblue>[E] </font>" + e.Row.Cells.FromKey("FullName").Text;
                    break;
            }
            if (isRoll)
            {
                e.Row.Cells.FromKey("FullName").Text = e.Row.Cells.FromKey("FullName").Text + " <img border='0' title='Roll Item' valign='top' src='/hc_v4/img/ed_roll.gif'/>";
                e.Row.Style.VerticalAlign = VerticalAlign.Bottom;
            }
            Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("FullName");
            if (SessionState.Culture.Type == HyperCatalog.Business.CultureType.Locale)
                cName.Text = "<a href='../redirect.aspx?i=" + e.Row.Cells.FromKey("Id").Text + "&c=" + SessionState.Culture.Code + "&p=UI/Globalize/qdetranslate.aspx'\">" + cName.Text + "</a>";
            else
                cName.Text = "<a href='../redirect.aspx?p=UI/Acquire/qde.aspx&i=" + e.Row.Cells.FromKey("Id").Text + "'\">" + cName.Text + "</a>";

        }
        protected void DDL_Cultures_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string cultureCode = DDL_Cultures.SelectedValue.ToString();
            SessionState.Culture = HyperCatalog.Business.Culture.GetByKey(cultureCode);
            if (Session["SearchResult"] != null)
            {
                ((DataSet)Session["SearchResult"]).Dispose();
                Session["SearchResult"] = null;
            }
            UITools.ShowSearchBar(Page);
            DisplaySearchResults();
        }

    }
}
