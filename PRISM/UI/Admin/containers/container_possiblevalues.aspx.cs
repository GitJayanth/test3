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
using Infragistics.WebUI.UltraWebGrid;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperComponents.Data.dbAccess;
using System.Data.SqlClient;
#endregion

/// <summary>
/// Display list of input forms containing the selected container
///		--> Return to the list of containers
///		--> Add container dependency
///		--> Delete several container dependencies
///		--> export in Excel
///		--> Filter on Name and Description fields
///     --> Bug 66022: added new functionlity of viewing the possible values based on cultures added new function loadCultureList.
/// </summary>
public partial class container_possiblevalues : HCPage
{
    #region Declarations

    private int containerId = -1;
    #endregion

    #region Code généré par le Concepteur Web Form
    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        txtFilter.AutoPostBack = false;
        txtFilter.Attributes.Add("onKeyDown", "KeyHandler(this);"); Page.ClientScript.RegisterStartupScript(Page.GetType(), "filtering", "<script defer=true>var culSelect ; culSelect = document.getElementById('" + Culturelist.ClientID + "') ; var txtFilterField;txtFilterField = document.getElementById('" + txtFilter.ClientID + "');</script>");
        base.OnInit(e);
    }

    /// <summary>
    /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
    /// le contenu de cette méthode avec l'éditeur de code.
    /// </summary>
    private void InitializeComponent()
    {
        Culturelist = (DropDownList)uwToolbar.Items.FromKeyCustom("CultureFilter").FindControl("Culturelist");
        if (Culturelist != null)
            Culturelist.SelectedIndexChanged += new EventHandler(CultureList_SelectedIndexChanged);
        this.uwToolbar.ButtonClicked += new Infragistics.WebUI.UltraWebToolbar.UltraWebToolbar.ButtonClickedEventHandler(this.uwToolbar_ButtonClicked);
        this.dg.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.dg_InitializeRow);

    }
    #endregion

    private void loadCulturelist(string selectedCulture)
    {
        Culturelist.Items.Clear();
        //add the "no-selection" culture at the top
        HyperCatalog.Business.CultureList CList = HyperCatalog.Shared.SessionState.User.Cultures;
        Culturelist.DataSource = CList;
        Culturelist.DataBind();
        if( selectedCulture != null)
            Culturelist.Items.FindByValue(selectedCulture).Selected = true;
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
        string selectedCulture = null;
        if (Request["filter"] != null)
        {
            txtFilter.Text = Request["filter"].ToString();
        }
        if (Request["c"] != null)
            containerId = Convert.ToInt32(Request["c"]);
        if (Request["CultureList"] != null)
        {
            selectedCulture = Request["CultureList"].Trim();
        }
        if (!Page.IsPostBack)
        {
            loadCulturelist(selectedCulture);
            UpdateDataView();
        }
    }


    void UpdateDataView()
    {
        // filter
        string cleanFilter = string.Empty;
        if (txtFilter.Text.Length > 0)
        {
            cleanFilter = txtFilter.Text.Replace("'", "''").ToLower();
            cleanFilter = cleanFilter.Replace("[", "[[]");
            cleanFilter = cleanFilter.Replace("_", "[_]");
            cleanFilter = cleanFilter.Replace("%", "[%]");
        }

        using (Database dbObj = Utils.GetMainDB())
        {
            using (DataSet ds = dbObj.RunSPReturnDataSet("_Container_GetPossibleValues", "PossibleValues",
               new SqlParameter("@ContainerId", containerId),
               new SqlParameter("@CultureCode", Culturelist.SelectedValue.ToString()),
               new SqlParameter("@ChunkValue", cleanFilter)))
            {
                dbObj.CloseConnection();

                if (dbObj.LastError.Length == 0)
                {
                    if (ds != null)
                    {
                        if (ds.Tables["PossibleValues"] != null && ds.Tables["PossibleValues"].Rows.Count > 0)
                        {
                            dg.DataSource = ds.Tables["PossibleValues"];
                            Utils.InitGridSort(ref dg);
                            dg.DataBind();

                            UITools.RefreshTab(this.Page, "PossibleValues", dg.Rows.Count);

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

                        ds.Dispose();
                    }
                }
                else
                {
                    throw new Exception("Error get possible values: " + dbObj.LastError);
                }
            }
        }
    }

    private void uwToolbar_ButtonClicked(object sender, Infragistics.WebUI.UltraWebToolbar.ButtonEvent be)
    {
        string btn = be.Button.Key.ToLower();
        if (btn == "export")
        {
            Utils.ExportToExcelFromGrid(dg, containerId + "-PossibleValues", containerId + "-PossibleValues", Page, null, containerId + "-Containers");

            //Utils.ExportToExcel(dg, containerId + "-PossibleValues", containerId + "-PossibleValues");
        }
    }

    private void dg_InitializeRow(object sender, Infragistics.WebUI.UltraWebGrid.RowEventArgs e)
    {
        if (e.Row.Cells.FromKey("Value").Text == HyperCatalog.Business.Chunk.BlankValue)
        {
            e.Row.Cells.FromKey("Value").Text = HyperCatalog.Business.Chunk.BlankText;
            e.Row.Cells.FromKey("Value").Style.CustomRules = string.Empty;
        }
        else
        {
            e.Row.Cells.FromKey("Value").Text = UITools.HtmlEncode(e.Row.Cells.FromKey("Value").Text).Replace(Environment.NewLine, "<br/>");
        }
        if (txtFilter.Text.Length > 0)
        {
            Infragistics.WebUI.UltraWebGrid.UltraGridRow r = e.Row;
            UITools.HiglightGridRowFilter(ref r, txtFilter.Text);
        }
    }
    private void CultureList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateDataView();
    }
}


