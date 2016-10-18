#region Uses
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using Infragistics.WebUI.UltraWebGrid;
using System.Data.SqlClient;
using HyperComponents.Data.dbAccess;
#endregion

namespace HyperCatalog.UI.Admin.Architecture
{
    /// <summary>
    /// Handles reinstate request to restore Obsolete Products to Live state.
    /// </summary>
    public partial class ReinstateSKU : HCPage
    {
        #region Declarations
        protected Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar uwToolBarTitle;
        private int NbSKUs = 0;
        string strSKUList = string.Empty;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["filter"] != null)
            {
                txtFilter.Text = Request["filter"].ToString();
            }

            UITools.CheckConnection(Page);
            if (!Page.IsPostBack)
            {
                if (SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_ITEMS))
                {
                    // Disable button
                    uwToolbar.Items.FromKeyButton("Export").Enabled = false;
                    uwToolbar.Items.FromKeyButton("Reinstate").Enabled = true;
                }
                else
                {
                    uwToolbar.Items.FromKeyButton("Export").Enabled = false;
                    uwToolbar.Items.FromKeyButton("Reinstate").Enabled = false;
                }
                DisplayData();
            }
            txtFilter.AutoPostBack = false;
            txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "'); </script>");
        }
        #endregion

        #region Reinstate SKU
        private void Reinstate()
        {
            strSKUList = string.Empty;
            int cntChkBx = 0; int retVal = 0;
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                TemplatedColumn col = (TemplatedColumn)dg.Rows[i].Cells.FromKey("Select").Column;
                CheckBox cb = (CheckBox)((CellItem)col.CellItems[i]).FindControl("g_sd");
                if (cb.Checked)
                {
                    cntChkBx += 1;
                    strSKUList += StripTags(dg.Rows[i].Cells.FromKey("sku").Value.ToString()) + "|";
                    using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 600))
                    {
                        retVal = dbObj.RunSPReturnInteger("dbo._ReinstateSKU_Del", new SqlParameter("@ItemId", Convert.ToInt64(dg.Rows[i].Cells.FromKey("ItemId").Value)), new SqlParameter("@UserId", SessionState.User.Id));
                        dbObj.CloseConnection();
                    }
                    if (retVal < 0)
                    {
                        lbMessage.Text = "Error: Not able to reinstate selected SKU" + dg.Rows[i].Cells.FromKey("sku").Value;
                        lbMessage.CssClass = "hc_error";
                        lbMessage.Visible = true;
                        break;
                    }
                }
            }
            if (cntChkBx > 0)
            {
                hfSKUList.Value = strSKUList;
                DisplayData();
                LbNbSKUs.ForeColor = Color.Green;
                LbNbSKUs.Font.Bold = false;
                LbNbSKUs.Text = "Selected SKU(s) has been successfully reinstated.";
                LbNbSKUs.Visible = true;
                lblDwlReport.Text = "To download the report on reinstated countries for the selected SKU's, please ";
                lblDwlReport.Visible = true;
                lbDwlReport.Visible = true;
            }
            else
            {
                LbNbSKUs.Text = "Please select at least one product to reinstate.";
                LbNbSKUs.Visible = true;
                LbNbSKUs.ForeColor = Color.Red;
                LbNbSKUs.Font.Bold = true;
                lblDwlReport.Visible = false;
                lbDwlReport.Visible = false;
            }
        }
        #endregion

        public string StripTags(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        #region Display Data
        /// <summary>
        /// Display data into the grid
        /// </summary>
        private void DisplayData()
        {
            using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 600))
            {
                DataSet ds = null;
                try
                {
                    ds = dbObj.RunSPReturnDataSet("_ReinstateSKU_GetAll", new SqlParameter("@Company", SessionState.CompanyName));
                    dbObj.CloseConnection();
                    if (dbObj.LastError != string.Empty)
                    {
                        lbMessage.Text = "[ERROR] " + dbObj.LastError;
                        lbMessage.CssClass = "hc_error";
                        lbMessage.Visible = true;
                    }
                    else
                    {
                        #region Results
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dg.DataSource = ds;
                            lbNoresults.Visible = false;
                            Utils.InitGridSort(ref dg);
                            dg.DataBind();

                            dg.Visible = true;
                            uwToolbar.Items.FromKeyButton("Export").Enabled = true;
                            uwToolbar.Items.FromKeyButton("Reinstate").Enabled = true;
                        }
                        #endregion

                        #region No result
                        else
                        {
                            lbNoresults.Text = "No product found";
                            lbNoresults.Visible = true;
                            uwToolbar.Items.FromKeyButton("Export").Enabled = false;
                            uwToolbar.Items.FromKeyButton("Reinstate").Enabled = false;
                            dg.Visible = false;
                        }
                        #endregion
                    }
                }
                finally
                {
                    if (ds != null) ds.Dispose();
                }
            }
        }
        #endregion

        #region "Event Handler Methods"
        protected void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            if (btn.Equals("reinstate"))
            {
                Reinstate();
            }
            #region Search
            if (btn.Equals("search"))
            {
                DisplayData();
            }
            #endregion
            if (btn.Equals("export"))
            {
                using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 600))
                {
                    DataSet dsExport = null;
                    try
                    {
                        dsExport = dbObj.RunSPReturnDataSet("_ReinstateSKU_Export");
                        dbObj.CloseConnection();
                        Utils.ExportDataSetToExcel(this, dsExport, "SKUsToReinstate.xls");
                    }
                    finally
                    {
                        if (dsExport != null) dsExport.Dispose();
                    }
                }
                //Utils.ExportToExcel(dg, "SKUsToReinstate", "SKUsToReinstate");
            }
        }

        protected void lbDwlReport_Click(object sender, EventArgs e)
        {
            using (Database dbObj = new Database(SessionState.CacheComponents["Crystal_DB"].ConnectionString, 600))
            {
                DataSet ds = null;
                try
                {
                    ds = dbObj.RunSPReturnDataSet("_ReinstateSKU_GetReport", new SqlParameter("@UserId", SessionState.User.Id), new SqlParameter("@SKUList", hfSKUList.Value.ToString()));
                    dbObj.CloseConnection();
                    Utils.ExportDataSetToExcel(this, ds, "SKUReinstatedCountries.xls");
                }
                finally
                {
                    if (ds != null) ds.Dispose();
                }
            }
        }

        protected void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            Infragistics.WebUI.UltraWebGrid.UltraGridCell cName = e.Row.Cells.FromKey("ItemName");
            Infragistics.WebUI.UltraWebGrid.UltraGridCell cNum = e.Row.Cells.FromKey("sku");

            #region Search colorization
            string search = txtFilter.Text.Trim();
            if (search != string.Empty)
            {
                if ((cName.Text.ToLower().IndexOf(search.ToLower())) >= 0 || (cNum.Text.ToLower().IndexOf(search.ToLower()) >= 0))
                {
                    cName.Text = Utils.CReplace(cName.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
                    cNum.Text = Utils.CReplace(cNum.Text, search, "<font color=red><b>" + search + "</b></font>", 1);
                }
                else
                {
                    e.Row.Delete();
                    return;
                }
            }
            #endregion
        }

        #endregion

    }
}
