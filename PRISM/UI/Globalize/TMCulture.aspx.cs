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
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using Infragistics.WebUI.UltraWebGrid;
#endregion

namespace HyperCatalog.UI.Globalize
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TMCulture : HCPage
    {
        #region declarations
        protected System.Web.UI.WebControls.DropDownList DDL_TermTypeList;
        protected System.Web.UI.WebControls.DropDownList DropDownList1;
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

        /// <summary>
        ///		Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        ///		le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
            this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);
            this.txtFilter.AutoPostBack = false;
            txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "txtFilterField = document.getElementById('" + txtFilter.ClientID + "');", true);

        }
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region Check Capabilities
            if ((SessionState.User.IsReadOnly) || (!SessionState.User.HasCapability(CapabilitiesEnum.MANAGE_TRANSLATION_MEMORY)))
            {
                uwToolbar.Items.FromKeyButton("Save").Enabled = false;
            }
            #endregion
            if (Request["filter"] != null)
            {
                txtFilter.Text = Request["filter"].ToString();
            }
            if (!Page.IsPostBack)
            {
                #region Load Languages list
                /// Retrieve all Languages
                using (LanguageList dsLanguages = Language.GetAll())
                {
                    DDL_Languages.DataSource = dsLanguages;
                    DDL_Languages.DataBind();
                }
                #endregion
                lbInfo.Visible = true;
                dg.Visible = false;
                if (Request["filter"] != null)
                {
                    UpdateDataView();
                }
                else
                {
                    SessionState.tmFilterExpression = null;
                }
            }
            else
            {
                // action after changes in term translation edit window 
                if (Request["action"] != null && Request["action"].ToString().ToLower() == "reload")
                {
                    UpdateDataView();
                }

            }
            txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");
        }


        /// <summary>
        /// Display TM
        /// </summary>
        private void UpdateDataView()
        {
            if (SessionState.tmFilterExpression != null)
            {
                DDL_Languages.SelectedValue = SessionState.tmFilterExpression;
            }
            if (txtFilter.Text != string.Empty)
            {
                DataSet dsTM;
                using (Database dbObj = Utils.GetMainDB())
                {
                    SqlParameter sqlSearch, languageCode, company;
                    languageCode = new SqlParameter("@LanguageCode", DDL_Languages.SelectedValue.ToString());
                    string search = txtFilter.Text;
                    #region Definition Search
                    sqlSearch = new SqlParameter("@Filter", string.Empty);
                    if (search != string.Empty) // filter on all terms
                    {
                        string cleanFilter = search.Replace("'", "''").ToLower();
                        cleanFilter = cleanFilter.Replace("[", "[[]");
                        cleanFilter = cleanFilter.Replace("_", "[_]");
                        cleanFilter = cleanFilter.Replace("%", "[%]");
                        sqlSearch.Value = " ((Lower(REPLACE(Value, CHAR(160), ' ')) like '%" + cleanFilter.ToLower() + "%') OR (Lower(REPLACE(TranslatedValue, CHAR(160), ' ')) like '%" + cleanFilter.ToLower() + "%'))";
                    }
                    company = new SqlParameter("@Company", SessionState.CompanyName);
                    #endregion
                    using (dsTM = dbObj.RunSPReturnDataSet("_TM_GetExpressionsToTranslate", "TM", languageCode, sqlSearch, company))
                    {
                        if (dbObj.LastError == string.Empty)
                        {
                            int c = dsTM.Tables[0].Rows.Count;
                            if (c == 0)
                            {
                                lbNoResults.Text = "No record match your search (" + txtFilter.Text + ") in " + DDL_Languages.SelectedItem;
                                lbNoResults.Visible = lbInfo.Visible = true;
                                dg.Visible = false;
                            }
                            #region Results
                            else
                            {
                                #region Too much results
                                if (c > Convert.ToInt32(SessionState.CacheParams["TMMaxRows"].Value))
                                {
                                    lbNoResults.Text = "There are " + c.ToString() + " expressions found over " + SessionState.CacheParams["TMMaxRows"].Value + ", please refine your search.";
                                    lbNoResults.Visible = lbInfo.Visible = true;
                                    dg.Visible = false;
                                }
                                #endregion
                                #region Display results
                                else
                                {
                                    dg.Columns.FromKey("TMExpressionValue").HeaderText = "TM [" + dsTM.Tables[0].Rows.Count.ToString() + " item(s)] found in Master";
                                    dg.Bands[0].ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.Yes;
                                    dg.DataSource = dsTM.Tables[0].DefaultView;
                                    Utils.InitGridSort(ref dg);
                                    dg.DataBind();
                                    lbNoResults.Visible = lbInfo.Visible = false;
                                    dg.Visible = true;
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            lbNoResults.Text = "An error occured: " + dbObj.LastError;
                            lbNoResults.Visible = lbInfo.Visible = true;
                            dg.Visible = false;
                        }
                    }
                }
            }
            else
            {
                lbNoResults.Text = "Please enter a search value";
                lbNoResults.Visible = lbInfo.Visible = true;
                dg.Visible = false;
            }
        }

        /// <summary>
        /// Action for toolbar buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="be"></param>
        private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
        {
            string btn = be.Button.Key.ToLower();
            #region Export
            if (btn == "export")
            {
                Utils.ExportToExcel(dg, "TMExpressions", "TMExpressions");
            }
            #endregion
        }


        /// <summary>
        /// Refresh TM if filter by culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DDL_Languages_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SessionState.tmFilterExpression = DDL_Languages.SelectedValue;
            UpdateDataView();
        }


        /// <summary>
        /// Display TM datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
        {
            #region Search colorization
            UltraGridRow r = e.Row;
            UITools.HiglightGridRowFilter(ref r, txtFilter.Text.Trim());
            #endregion
            #region Display Edit Link in TMExpressionValue
            //Display Edit Link in CultureCode
            r.Cells.FromKey("TMExpressionValue").Text = "<a href='javascript://' onclick=\"SC('" + r.Cells.FromKey("TMExpressionId").Text + "', '" + DDL_Languages.SelectedValue.ToString() + "')\">" + r.Cells.FromKey("TMExpressionValue").Text + "</a>";
            #endregion
        }
    }
}


